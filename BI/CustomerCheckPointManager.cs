using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Customer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class CustomerCheckPointManager : ApiCommonData, ICustomerCheckPointManager
    {
        private readonly IReferenceRepository _referenceRepo = null;
        private readonly ICustomerCheckPointRepository _repo = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly CheckPointMap checkmap = null;
        private ITenantProvider _filterService = null;

        public CustomerCheckPointManager(
           ICustomerCheckPointRepository repo,
           IReferenceRepository referenceRepo, IAPIUserContext applicationContext, 
           ITenantProvider filterService)
        {
            _repo = repo;
            _referenceRepo = referenceRepo;
            _applicationContext = applicationContext;
            checkmap = new CheckPointMap();
            _filterService = filterService;
        }
        public async Task<CheckPointResponse> GetCheckPointType()
        {
            try
            {
                var response = new CheckPointResponse();
                var data = await _repo.GetCheckPointType();
                if (data == null || data.Count == 0)
                {
                    response.Result = CheckPointResult.CannotGet;
                    response.CheckPointList = null;
                }
                else
                {
                    response.CheckPointList = data.Select(x => checkmap.GetCheckPoint(x));
                    response.Result = CheckPointResult.Success;
                }
                return response;
            }
            catch (Exception ex)
            {
                return new CheckPointResponse() { Result = CheckPointResult.CannotGet };
            }

        }
        public async Task<CustomerCheckPointGetResponse> GetCustomerCheckPointSummary(int? cusId, int? serviceId)
        {
            var response = new CustomerCheckPointGetResponse();
            try
            {
                //response.CustomerCheckPointList = cusId > 0 && serviceId > 0 ?
                //    _repo.GetCusCPByCusServiceId(cusId, serviceId).
                //    Select(CheckPointMap.GetCustomerCheckPoint).ToArray() :
                //    cusId > 0 && serviceId == 0 ?
                //    _repo.GetCusCPByCustomerId(cusId).Select(CheckPointMap.GetCustomerCheckPoint).ToArray() :
                // _repo.GetCustomerCheckPoint().Select(CheckPointMap.GetCustomerCheckPoint).ToArray();

                if(cusId > 0 && serviceId > 0)
                {
                    response.CustomerCheckPointList = await _repo.GetCusCPByCusServiceId(cusId, serviceId);
                    
                }

                else if (cusId > 0 && serviceId == 0)
                {
                    response.CustomerCheckPointList = await _repo.GetCusCPByCustomerId(cusId);
                }

                else
                {
                    response.CustomerCheckPointList = await _repo.GetCustomerCheckPoint();
                }

                var checkpointIds = response.CustomerCheckPointList.Select(x => x.Id).Distinct().ToList();

                var brandList = await _repo.GetCustomerCheckPointBrand(checkpointIds);

                var deptList = await _repo.GetCustomerCheckPointDept(checkpointIds);

                var serviceTypeList = await _repo.GetCustomerCheckPointServiceType(checkpointIds);

                var countryList = await _repo.GetCustomerCheckPointCountry(checkpointIds);
                var res = response.CustomerCheckPointList.Select(x => checkmap.GetCustomerCheckPoint(x, brandList.Where(y => y.CheckPointId == x.Id).ToList(),
                    deptList.Where(y => y.CheckPointId == x.Id).ToList(), serviceTypeList.Where(y => y.CheckPointId == x.Id).ToList(), countryList.Where(y => y.CheckPointId == x.Id).ToList()));

                response.CustomerCheckPointList = res;

                if (response.CustomerCheckPointList == null)
                    return new CustomerCheckPointGetResponse { Result = CheckPointGetResult.NotFound };
                else if (response.CustomerCheckPointList != null && response.CustomerCheckPointList.Count() <= 0)
                    return new CustomerCheckPointGetResponse { Result = CheckPointGetResult.CheckPointListNoData };
                response.Result = CheckPointGetResult.Success;
            }
            catch (Exception ex)
            {
                return new CustomerCheckPointGetResponse { Result = CheckPointGetResult.CannotGetCheckPointList };
            }
            return response;
        }
        public async Task<CustomerCheckPointSaveResponse> Save(CustomerCheckPointSaveRequest request)
        {
            try
            {
                if (request.Id == 0)
                    return await AddCustomerCP(request);
                else
                    return await EditCustomerCP(request);
            }
            catch (Exception ex)
            {
                return new CustomerCheckPointSaveResponse { Result = CPSaveResult.IsNotSaved };
            }
        }
        private async Task<CustomerCheckPointSaveResponse> AddCustomerCP(CustomerCheckPointSaveRequest request)
        {
            try
            {
                var _entityId = _filterService.GetCompanyId();
                CuCheckPoint entity = CheckPointMap.MapCheckPointSaveEntity(request, _applicationContext.UserId, _entityId);

                if (request.BrandId != null && request.BrandId.Any())
                {
                    List<CuCheckPointsBrand> brandEntity = new List<CuCheckPointsBrand>();
                    foreach (var brandId in request.BrandId)
                    {
                        brandEntity.Add(new CuCheckPointsBrand
                        {
                            CheckpointId = entity.Id,
                            BrandId = brandId,
                            Active = true,
                            CreatedBy = _applicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            EntityId=_entityId
                        });
                    }

                    entity.CuCheckPointsBrands = brandEntity;
                }

                if (request.DeptId != null && request.DeptId.Any())
                {
                    List<CuCheckPointsDepartment> deptEntity = new List<CuCheckPointsDepartment>();
                    foreach (var deptId in request.DeptId)
                    {
                        deptEntity.Add(new CuCheckPointsDepartment
                        {
                            CheckpointId = entity.Id,
                            DeptId = deptId,
                            Active = true,
                            CreatedBy = _applicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            EntityId = _entityId
                        });
                    }

                    entity.CuCheckPointsDepartments = deptEntity;
                }

                if (request.ServiceTypeId != null && request.ServiceTypeId.Any())
                {
                    List<CuCheckPointsServiceType> serviceTypeEntity = new List<CuCheckPointsServiceType>();
                    foreach (var serviceTypeId in request.ServiceTypeId)
                    {
                        serviceTypeEntity.Add(new CuCheckPointsServiceType
                        {
                            CheckpointId = entity.Id,
                            ServiceTypeId = serviceTypeId,
                            Active = true,
                            CreatedBy = _applicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            EntityId = _entityId
                        });
                    }

                    entity.CuCheckPointsServiceTypes = serviceTypeEntity;
                }

                AddCpCountryList(request, entity);
                
                if (entity == null)
                    return new CustomerCheckPointSaveResponse() { Result = CPSaveResult.CannotMapRequestToEntites };
                bool isExists = await _repo.IsRecordExists(entity);
                if (isExists)
                    return new CustomerCheckPointSaveResponse() { Result = CPSaveResult.RecordExists };
                int id = await _repo.SaveCustomerCP(entity);
                if (id != 0)
                {                 
                    return new CustomerCheckPointSaveResponse() { Result = CPSaveResult.Success };
                }
                else
                    return new CustomerCheckPointSaveResponse() { Result = CPSaveResult.IsNotSaved };
            }
            catch (Exception ex)
            {
                return new CustomerCheckPointSaveResponse() { Result = CPSaveResult.IsNotSaved };
            }
        }
        private async Task<CustomerCheckPointSaveResponse> EditCustomerCP(CustomerCheckPointSaveRequest request)
        {
            var entity = await _repo.GetCustomerCPbyId(request.Id);
            try
            {
                if (entity == null)
                    return new CustomerCheckPointSaveResponse() { Result = CPSaveResult.CurrentCustomerCPNotFound };
                var _entityId = _filterService.GetCompanyId();
                entity = checkmap.UpdateCustomerCPEntity(request, entity, _applicationContext.UserId);
                bool isExists = await _repo.IsRecordExists(entity);

                request.EntityId = _entityId;

                UpdateCpBrandList(request, entity);
                UpdateCpServiceTypeList(request, entity);
                UpdateCpDepartmentList(request, entity);
                UpdateCpCountryList(request, entity);

                if (isExists)
                    return new CustomerCheckPointSaveResponse() { Result = CPSaveResult.RecordExists };
                int id = await _repo.UpdateCustomerCP(entity);
                if (id > 0)
                    return new CustomerCheckPointSaveResponse { Result = CPSaveResult.Success };
                else
                    return new CustomerCheckPointSaveResponse() { Result = CPSaveResult.IsNotSaved };
            }
            catch (Exception ex)
            {
                return new CustomerCheckPointSaveResponse { Result = CPSaveResult.IsNotSaved };
            }
        }

        private void AddCpCountryList(CustomerCheckPointSaveRequest request, CuCheckPoint entity)
        {
            var _entityId = _filterService.GetCompanyId();
            if (request.CountryIdList != null && request.CountryIdList.Any())
            {
                List<CuCheckPointsCountry> countryEntityList = new List<CuCheckPointsCountry>();
                foreach (var countryId in request.CountryIdList)
                {
                    countryEntityList.Add(new CuCheckPointsCountry
                    {
                        CheckpointId = entity.Id,
                        CountryId = countryId,
                        Active = true,
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = _entityId
                    });
                }

                entity.CuCheckPointsCountries = countryEntityList;
            }
        }

        private void UpdateCpCountryList(CustomerCheckPointSaveRequest request, CuCheckPoint entity)
        {
            var countryIdList = request.CountryIdList.Select(x => x).ToArray();

            var lstCountrysToremove = new List<CuCheckPointsCountry>();

            var cpCountryList = entity.CuCheckPointsCountries.Where(x => !countryIdList.Contains(x.CountryId) && x.Active);

            var existCountryIdList = entity.CuCheckPointsCountries.Where(x => countryIdList.Contains(x.CountryId) && x.Active);

            // Remove if data does not exist in the db.

            foreach (var item in cpCountryList)
            {
                item.Active = false;
                item.DeletedBy = _applicationContext.UserId;
                item.DeletedOn = DateTime.Now;
                lstCountrysToremove.Add(item);
            }

            _repo.EditEntities(lstCountrysToremove);

            // Update if data already exist in the db

            if (request.CountryIdList != null)
            {
                // Add if data is new it means id = 0
                foreach (var id in countryIdList)
                {
                    if (!existCountryIdList.Any() || !existCountryIdList.Any(x => x.CountryId == id))
                    {
                        entity.CuCheckPointsCountries.Add(new CuCheckPointsCountry()
                        {
                            CheckpointId = entity.Id,
                            CountryId = id,
                            Active = true,
                            CreatedBy = _applicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            EntityId = request.EntityId
                        });
                    }
                }
            }
        }

        public async Task<CustomerCheckPointDeleteResponse> Delete(int deleteRequest)
        {
            try
            {
                var deleteEntity = await _repo.GetCustomerCPbyId(deleteRequest);

                if (deleteEntity == null)
                    return new CustomerCheckPointDeleteResponse { Result = CustomerCPDeleteResult.NotFound };
                deleteEntity = checkmap.DeleteEntity(deleteEntity, _applicationContext.UserId);

                if(deleteEntity.CuCheckPointsBrands.Any())
                {
                    deleteEntity.CuCheckPointsBrands.ToList().ForEach(x => { x.Active = false; x.UpdatedBy = _applicationContext.UserId; x.UpdatedOn = DateTime.Now; });
                }

                if (deleteEntity.CuCheckPointsDepartments.Any())
                {
                    deleteEntity.CuCheckPointsDepartments.ToList().ForEach(x => { x.Active = false; x.UpdatedBy = _applicationContext.UserId; x.UpdatedOn = DateTime.Now; });
                }

                if (deleteEntity.CuCheckPointsServiceTypes.Any())
                {
                    deleteEntity.CuCheckPointsServiceTypes.ToList().ForEach(x => { x.Active = false; x.UpdatedBy = _applicationContext.UserId; x.UpdatedOn = DateTime.Now; });
                }

                _repo.Save(deleteEntity);
                return new CustomerCheckPointDeleteResponse { Result = CustomerCPDeleteResult.Success };
            }
            catch (Exception ex)
            {
                return new CustomerCheckPointDeleteResponse { Result = CustomerCPDeleteResult.NotFound };
            }
        }
        // get checkpoint list based on customer list , service id, checkpoint list
        public async Task<List<CuCheckPoint>> GetCheckPointList(IEnumerable<int> customerIdList, int serviceId, IEnumerable<int> checkPointIdList)
        {
            return await _repo.GetCheckPointList(customerIdList, serviceId, checkPointIdList);
        }


        // get customer checkpoint list by customer Id
        public async Task<List<CuCheckPoint>> GetCheckPointListByCustomer(List<int> customerIdList, int serviceId)
        {
            return await _repo.GetCustomerCheckPointByCustomer(customerIdList, serviceId);
        }

        //update the checkpoint brands
        private void UpdateCpBrandList(CustomerCheckPointSaveRequest request, CuCheckPoint entity )
        {
            var brandIdList = request.BrandId.Select(x => x).ToArray();

            var lstBrandsToremove = new List<CuCheckPointsBrand>();

            var cpBrandList = entity.CuCheckPointsBrands.Where(x => !brandIdList.Contains(x.BrandId) && x.Active);

            var existbrandIdList = entity.CuCheckPointsBrands.Where(x => brandIdList.Contains(x.BrandId) && x.Active);

            // Remove if data does not exist in the db.

            foreach (var item in cpBrandList)
            {
                item.Active = false;
                item.UpdatedBy = _applicationContext.UserId;
                item.UpdatedOn = DateTime.Now;
                lstBrandsToremove.Add(item);
            }

            _repo.EditEntities(lstBrandsToremove);

            // Update if data already exist in the db

            if (request.BrandId != null)
            {
                // Add if data is new it means id = 0
                foreach (var id in brandIdList)
                {
                    if (!existbrandIdList.Any() || !existbrandIdList.Any(x => x.BrandId == id))
                    {
                        entity.CuCheckPointsBrands.Add(new CuCheckPointsBrand()
                        {
                            CheckpointId = entity.Id,
                            BrandId = id,
                            Active = true,
                            CreatedBy = _applicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            EntityId = request.EntityId
                        });
                    }
                }
            }
        }

        //update the checkpoint service types
        public void UpdateCpServiceTypeList(CustomerCheckPointSaveRequest request, CuCheckPoint entity)
        {
            var serviceTypeIdList = request.ServiceTypeId.Select(x => x).ToArray();

            var lstServiceTypeToremove = new List<CuCheckPointsServiceType>();

            var cpServiceTypeList = entity.CuCheckPointsServiceTypes.Where(x => !serviceTypeIdList.Contains(x.ServiceTypeId) && x.Active);

            var existServiceTypeIdList = entity.CuCheckPointsServiceTypes.Where(x => serviceTypeIdList.Contains(x.ServiceTypeId) && x.Active);

            // Remove if data does not exist in the db.

            foreach (var item in cpServiceTypeList)
            {
                item.Active = false;
                item.UpdatedBy = _applicationContext.UserId;
                item.UpdatedOn = DateTime.Now;
                lstServiceTypeToremove.Add(item);
            }

            _repo.EditEntities(lstServiceTypeToremove);

            // Update if data already exist in the db

            if (request.ServiceTypeId != null)
            {
                // Add if data is new it means id = 0
                foreach (var id in serviceTypeIdList)
                {
                    if (!existServiceTypeIdList.Any() || !existServiceTypeIdList.Any(x => x.ServiceTypeId == id))
                    {
                        entity.CuCheckPointsServiceTypes.Add(new CuCheckPointsServiceType()
                        {
                            CheckpointId = entity.Id,
                            ServiceTypeId = id,
                            Active = true,
                            CreatedBy = _applicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            EntityId = request.EntityId
                        });
                    }
                }
            }
        }

        //update the checkpoint departments
        public void UpdateCpDepartmentList(CustomerCheckPointSaveRequest request, CuCheckPoint entity)
        {
            var deptIdList = request.DeptId.Select(x => x).ToArray();

            var lstDeptToremove = new List<CuCheckPointsDepartment>();

            var cpDeptList = entity.CuCheckPointsDepartments.Where(x => !deptIdList.Contains(x.DeptId) && x.Active);

            var existDeptIdList = entity.CuCheckPointsDepartments.Where(x => deptIdList.Contains(x.DeptId) && x.Active);

            // Remove if data does not exist in the db.

            foreach (var item in cpDeptList)
            {
                item.Active = false;
                item.UpdatedBy = _applicationContext.UserId;
                item.UpdatedOn = DateTime.Now;
                lstDeptToremove.Add(item);
            }

            _repo.EditEntities(lstDeptToremove);

            // Update if data already exist in the db

            if (request.DeptId != null)
            {
                // Add if data is new it means id = 0
                foreach (var id in deptIdList)
                {
                    if (!existDeptIdList.Any() || !existDeptIdList.Any(x => x.DeptId == id))
                    {
                        entity.CuCheckPointsDepartments.Add(new CuCheckPointsDepartment()
                        {
                            CheckpointId = entity.Id,
                            DeptId = id,
                            Active = true,
                            CreatedBy = _applicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            EntityId = request.EntityId
                        });
                    }
                }
            }
        }

        // get checkpoint brand list based on checkpoint list
        public async Task<List<CommonCheckPointDataSource>> GetCheckPointBrandList(List<int> checkPointIdList)
        {
            return await _repo.GetCustomerCheckPointBrand(checkPointIdList);
        }

        // get checkpoint dept list based on checkpoint list
        public async Task<List<CommonCheckPointDataSource>> GetCheckPointDeptList(List<int> checkPointIdList)
        {
            return await _repo.GetCustomerCheckPointDept(checkPointIdList);
        }

        // get checkpoint service type list based on checkpoint list
        public async Task<List<CommonCheckPointServiceTypeDataSource>> GetCheckPointServiceTypeList(List<int> checkPointIdList)
        {
            return await _repo.GetCustomerCheckPointServiceType(checkPointIdList);
        }

        public async Task<CustomerCheckPoint> GetCustomerCheckpoint(int customerId, int serviceId, int checkpointTypeId)
        {
            var checkpoint = await _repo.GetCustomerCheckpoint(customerId, serviceId, checkpointTypeId);

            if (checkpoint == null)
                return null;

            var checkpointIds = new[] { checkpoint.Id }.ToList();

            var brandList = await _repo.GetCustomerCheckPointBrand(checkpointIds);

            var deptList = await _repo.GetCustomerCheckPointDept(checkpointIds);

            var serviceTypeList = await _repo.GetCustomerCheckPointServiceType(checkpointIds);

            var countryList = await _repo.GetCustomerCheckPointCountry(checkpointIds);

            return checkmap.GetCustomerCheckPoint(checkpoint, brandList, deptList, serviceTypeList, countryList);
        }

        /// <summary>
        /// Get the checkpoint list by customerid,serviceid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<CommonCheckPointDataSourceResponse> GetCustomerCheckPointList(int customerId, int serviceId)
        {
            var response = new CommonCheckPointDataSourceResponse() { Result = CustomerCheckPointResult.NotFound };

            var checkPointList = await _repo.GetCustomerCheckPointList(customerId, serviceId);

            if (checkPointList.Any())
            {
                response.CheckPointList= checkPointList;
                response.Result = CustomerCheckPointResult.Success;
            }

            return response;
        }
    }
}
