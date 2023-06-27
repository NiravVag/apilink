using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.References;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class CustomerServiceConfigManager : ICustomerServiceConfigManager
    {

        private readonly ICustomerRepository _customerRepo = null;
        private readonly ICustomerServiceConfigRepository _repo = null;
        private readonly IReferenceRepository _referenceRepo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly CustomerMap _customermap = null;
        private readonly ReferenceMap _refmap = null;
        private ITenantProvider _filterService = null;

        public CustomerServiceConfigManager(
            ICustomerRepository customerRepo,
            ICustomerServiceConfigRepository repo,
            IReferenceRepository referenceRepo, IAPIUserContext applicationContext,
            ITenantProvider filterService)
        {
            _repo = repo;
            _customerRepo = customerRepo;
            _referenceRepo = referenceRepo;
            _ApplicationContext = applicationContext;
            _customermap = new CustomerMap();
            _refmap = new ReferenceMap();
            _filterService = filterService;
        }

        public async Task<CustomerServiceConfigSearchResponse> GetCustomerServiceConfigData(CustomerServiceConfigSearchRequest request)
        {
            var response = new CustomerServiceConfigSearchResponse { Index = request.index.Value, PageSize = request.pageSize.Value };
            if (request.customerValue != null)
            {


                var serviceData = await GetServiceconfig(request.customerValue, request.serviceValue);
                //var serviceDataList = await _repo.GetServiceTypeByCustomerID(request.customerValue,request.serviceValue);

                //var services = await _referenceRepo.GetServices();

                //var serviceTypes = await _referenceRepo.GetServiceTypes();

                //var productCategory = await _referenceRepo.GetProductCategories();

                //var customerList = await _customerRepo.GetCustomersItems();

                //var serviceData = serviceDataList
                //            .Join(services, sdl => sdl.ServiceId, s => s.Id, (sdl, s)
                //                   => new { sdl, s })
                //             .Join(serviceTypes, sdls => sdls.sdl.ServiceTypeId, st => st.Id, (sdls, st)
                //                    => new { sdls, st })
                //             .Join(customerList, sdlsc => sdlsc.sdls.sdl.CustomerId, c => c.Id, (sdlsc, c)
                //                    => new { sdlsc, c })
                //             .GroupJoin(productCategory, sdlscp => sdlscp.sdlsc.sdls.sdl.ProductCategoryId, pc => pc.Id, (FResult, pc)
                //                       => new { FResult, ProductCategory=pc.SingleOrDefault() })
                //             .Select(sel => new CustomerServiceConfig { Id=sel.FResult.sdlsc.sdls.sdl.Id,
                //                    Service = sel.FResult.sdlsc.sdls.s.Name,
                //                 ServiceType = sel.FResult.sdlsc.st.Name,
                //                 CustomerName = sel.FResult.c.CustomerName, SamplingMethod = "AQL",
                //                 ProductCategory =sel.ProductCategory!=null?sel.ProductCategory.Name:""
                //             });


                //var customerContacts = data.CuContacts.Where(x => x.Active == true);

                //if (request.contactName != null && request.contactName != string.Empty)
                //{
                //    customerContacts = customerContacts.Where(x => x.ContactName == request.contactName || x.Email == request.contactName)
                //                        .ToList();
                //}

                //customerContacts = customerContacts.OrderBy(contact => contact.ContactName);

                response.TotalCount = serviceData.Count();

                if (response.TotalCount == 0)
                {
                    response.Result = CustomerServiceConfigSearchResult.NotFound;
                    return response;
                }
                int skip = (request.index.Value - 1) * request.pageSize.Value;
                response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                response.Data = serviceData;
                //response.Data = customerContacts.Skip(skip).Take(request.pageSize.Value).Select(x => _customermap.GetCustomerContactItem(x)).ToArray();
                //response.Result =

                response.Result = CustomerServiceConfigSearchResult.Success;

            }
            return response;
        }


        public async Task<CustomerServConfigSummaryResponse> GetCustomerServiceConfigSummary()
        {
            var response = new CustomerServConfigSummaryResponse();

            var data = await _customerRepo.GetCustomersItems();
            var serviceData = await _referenceRepo.GetServices();
            if ((data == null || data.Count == 0) && (serviceData == null || serviceData.Count == 0))
            {
                response.CustomerList = null;
                response.ServiceList = null;
            }
            else
            {
                response.CustomerList = data.Select(x => _customermap.GetCustomerItem(x, ""));

                response.ServiceList = serviceData.Select(x => _refmap.GetService(x));
                response.IsEdit = (_ApplicationContext.CustomerId == 0);

                response.Result = CustomerServConfigSummaryResult.Success;
            }

            return response;
        }

        public async Task<CustomerServiceConfigResponse> GetEditCustomerServiceConfigSummary(CustomerServiceConfigRequest request)
        {

            var response = new CustomerServiceConfigResponse { Index = request.index.Value, PageSize = request.pageSize.Value };

            var data = await _customerRepo.GetCustomersItems();
            var serviceData = await _referenceRepo.GetServices();
            if ((data == null || data.Count == 0) && (serviceData == null || serviceData.Count == 0))
            {
                response.CustomerList = null;
                response.ServiceList = null;
            }
            else
            {
                response.CustomerList = data.Select(x => _customermap.GetCustomerItem(x, ""));

                response.ServiceList = serviceData.Select(x => _refmap.GetService(x));

                var serviceDataList = await GetServiceconfig(request.customerID, null);

                if (serviceDataList != null)
                {
                    response.TotalCount = serviceDataList.Count();
                    int skip = (request.index.Value - 1) * request.pageSize.Value;
                    response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                    response.Data = serviceDataList.Skip(skip).Take(request.pageSize.Value).ToArray();
                }


                //response.IsEdit = (_ApplicationContext.CustomerId == 0);

                response.Result = CustomerServiceConfigResult.Success;
            }

            return response;
        }

        private async Task<IEnumerable<CustomerServiceConfig>> GetServiceconfig(int? customerID, int? serviceID)
        {
            if (customerID != null)
            {


                var serviceDataList = await _repo.GetServiceTypeByCustomerID(customerID, serviceID);

                var services = await _referenceRepo.GetServices();

                var serviceTypes = await _referenceRepo.GetServiceTypes();

                var productCategory = await _referenceRepo.GetProductCategories();

                var customerList = await _customerRepo.GetCustomersItems();

                var serviceData = serviceDataList
                            .Join(services, sdl => sdl.ServiceId, s => s.Id, (sdl, s)
                                   => new { sdl, s })
                             .Join(serviceTypes, sdls => sdls.sdl.ServiceTypeId, st => st.Id, (sdls, st)
                                    => new { sdls, st })
                             .Join(customerList, sdlsc => sdlsc.sdls.sdl.CustomerId, c => c.Id, (sdlsc, c)
                                    => new { sdlsc, c })
                             .GroupJoin(productCategory, sdlscp => sdlscp.sdlsc.sdls.sdl.ProductCategoryId, pc => pc.Id, (FResult, pc)
                                       => new { FResult, ProductCategory = pc.SingleOrDefault() })
                             .Select(sel => new CustomerServiceConfig
                             {
                                 Id = sel.FResult.sdlsc.sdls.sdl.Id,
                                 Service = sel.FResult.sdlsc.sdls.s.Name,
                                 ServiceType = sel.FResult.sdlsc.st.Name,
                                 CustomerName = sel.FResult.c.CustomerName,
                                 SamplingMethod = "AQL",
                                 ProductCategory = sel.ProductCategory != null ? sel.ProductCategory.Name : ""
                             });
                return serviceData;
            }

            return null;
        }

        public EditCustomerServiceConfigResponse GetEditCustomerServiceConfig(int? id)
        {
            EditCustomerServiceConfigResponse response = new EditCustomerServiceConfigResponse();
            var serviceConfigData = _repo.GetServiceTypeByServiceID(id);
            if (serviceConfigData != null)
            {
                var customer = _customerRepo.GetCustomerByID(serviceConfigData.CustomerId);
                var customerServiceConfig = _customermap.MapCustomerServiceConfigData(serviceConfigData, customer.CustomerName);
                response.CustomerServiceConfigData = customerServiceConfig;
            }
            else
            {
                response.Result = EditCustomerServiceConfigResult.CannotGetServiceType;
            }

            response.Result = EditCustomerServiceConfigResult.Success;

            return response;


        }

        public async Task<CustomerServiceConfigMasterResponse> GetCustomerServiceConfigMaster()
        {
            CustomerServiceConfigMasterResponse response = new CustomerServiceConfigMasterResponse();

            var serviceList = await _referenceRepo.GetServices();

            if (serviceList != null && serviceList.Count > 0)
            {
                response.ServiceList = serviceList.Select(x => _refmap.GetService(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetService;
            }

            var productCategoryList = await _referenceRepo.GetProductCategories();

            if (productCategoryList != null && productCategoryList.Count > 0)
            {
                response.ProductCategoryList = productCategoryList.Select(x => _refmap.GetProductCategory(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetProductCategory;
            }

            var pickTypeList = await _referenceRepo.GetServicePickType();

            if (pickTypeList != null && pickTypeList.Count > 0)
            {
                response.PickTypeList = pickTypeList.Select(x => _refmap.GetPickType(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetPickType;
            }

            var levelPick1List = await _referenceRepo.GetServiceLevelPickFirst();

            if (levelPick1List != null && levelPick1List.Count > 0)
            {
                response.LevelPick1List = levelPick1List.Select(x => _refmap.GetLevelPick1(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetLevelPick1;
            }

            var levelPick2List = await _referenceRepo.GetServiceLevelPickSecond();

            if (levelPick2List != null && levelPick2List.Count > 0)
            {
                response.LevelPick2List = levelPick2List.Select(x => _refmap.GetLevelPick2(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetLevelPick2;
            }

            var pick1List = await _referenceRepo.GetServicePickFirst();

            if (pick1List != null && pick1List.Count > 0)
            {
                response.Pick1List = pick1List.Select(x => _refmap.GetPick1(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetPick1;
            }

            var pick2List = await _referenceRepo.GetServicePickSecond();

            if (pick2List != null && pick2List.Count > 0)
            {
                response.Pick2List = pick2List.Select(x => _refmap.GetPick2(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetPick2;
            }

            var defectClassificationList = await _referenceRepo.GetServiceDefectClassification();

            if (defectClassificationList != null && defectClassificationList.Count > 0)
            {
                response.DefectClassificationList = defectClassificationList.Select(x => _refmap.GetDefectClassification(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetDefectClassification;
            }

            var reportUnitList = await _referenceRepo.GetServiceReportUnit();

            if (reportUnitList != null && reportUnitList.Count > 0)
            {
                response.ReportUnitList = reportUnitList.Select(x => _refmap.GetReportUnit(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetReportUnit;
            }

            var dpPointList = await _referenceRepo.GetDpPointList();

            if (dpPointList != null && dpPointList.Count > 0)
            {
                response.DpPointList = dpPointList.Select(x => _refmap.GetDpPoint(x));
            }
            else
            {
                response.Result = CustomerServiceConfigMasterResult.CannotGetPick1;
            }

            response.Result = CustomerServiceConfigMasterResult.Success;

            return response;


        }

        public async Task<SaveCustomerServiceConfigResponse> Save(EditCustomerServiceConfigData request)
        {
            var response = new SaveCustomerServiceConfigResponse();

            if (request.Id == 0)
            {
                var CustomerService = _repo.GetServiceTypeBySPST(request.Service, request.ServiceType, request.CustomerID);
                if (CustomerService != null)
                {
                    return new SaveCustomerServiceConfigResponse { Result = SaveCustomerServiceConfigResult.CustomerServiceConfigExists };
                }
                else
                {
                    CuServiceType entity = CustomerMap.MapCustomerServiceConfigEntity(request,_ApplicationContext.UserId, _filterService.GetCompanyId());

                    if (entity == null)
                        return new SaveCustomerServiceConfigResponse { Result = SaveCustomerServiceConfigResult.CustomerServiceConfigIsNotFound };

                    response.Id = await _repo.AddCustomerServiceConfig(entity);

                    if (response.Id == 0)
                        return new SaveCustomerServiceConfigResponse { Result = SaveCustomerServiceConfigResult.CustomerServiceConfigIsNotSaved };

                    response.Result = SaveCustomerServiceConfigResult.Success;
                }

                return response;
            }
            else
            {
                var entity = _repo.GetServiceTypeByServiceID(request.Id);

                if (entity == null)
                    return new SaveCustomerServiceConfigResponse { Result = SaveCustomerServiceConfigResult.CustomerServiceConfigIsNotFound };



                _customermap.UpdateCustomerServiceConfigEntity(entity, request, _ApplicationContext.UserId);

                await _repo.EditCustomerServiceConfig(entity);
                response.Id = entity.Id;

                response.Result = SaveCustomerServiceConfigResult.Success;
            }

            return response;
        }

        public async Task<CustomerServiceConfigDeleteResponse> DeleteCustomerService(int id)
        {
            var customerService = _repo.GetServiceTypeByServiceID(id);
            customerService.DeletedBy = _ApplicationContext.UserId;
            customerService.DeletedOn = DateTime.Now;

            if (customerService == null)
                return new CustomerServiceConfigDeleteResponse { Id = id, Result = CustomerServiceConfigDeleteResult.NotFound };

            await _repo.RemoveCustomerServiceConfig(id);

            return new CustomerServiceConfigDeleteResponse { Id = id, Result = CustomerServiceConfigDeleteResult.Success };

        }

        public async Task<CustomerServicePickResponse> GetLevelPickFirst()
        {
            CustomerServicePickResponse response = new CustomerServicePickResponse();


            var levelPick1List = await _referenceRepo.GetServiceLevelPickFirst();

            if (levelPick1List != null && levelPick1List.Count > 0)
            {
                response.LevelPickList = levelPick1List.Select(x => _refmap.GetLevelPick1(x));
            }
            else
            {
                response.Result = CustomerServicePickResult.CannotGetLevelPick;
            }

            var pickList = await _referenceRepo.GetServicePickFirst();

            if (pickList != null && pickList.Count > 0)
            {
                response.PickList = pickList.Select(x => _refmap.GetPick1(x)).OrderByDescending(x=>x.Value);
            }
            else
            {
                response.Result = CustomerServicePickResult.CannotGetPick;
            }

            response.Result = CustomerServicePickResult.Success;

            return response;
        }
        public EditCustomerServiceConfigResponse ServiceByCustomerAndServiceTypeID(int customerId, int serviceTypeId)
        {
            EditCustomerServiceConfigResponse response = new EditCustomerServiceConfigResponse();
            var serviceConfigData = _repo.ServiceByCustomerAndServiceTypeID(customerId,serviceTypeId);
            if (serviceConfigData != null)
            {
                var customerServiceConfig = _customermap.MapCustomerServiceConfigData(serviceConfigData, "");
                response.CustomerServiceConfigData = customerServiceConfig;
            }
            else
            {
                response.Result = EditCustomerServiceConfigResult.CannotGetServiceType;
            }

            response.Result = EditCustomerServiceConfigResult.Success;

            return response;


        }

        //fetch the serrvice type by customer
        public async Task<DataSourceResponse> GetServiceconfig(int customerId, int serviceId)
        {
            var data = await _repo.GetCustomerServiceType(customerId, serviceId);

            if(data == null || !data.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }

        /// <summary>
        /// Check service type mapped with the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        public async Task<bool> CheckServiceTypeMappedWithCustomer(int customerId, int serviceTypeId)
        {
            return await _repo.CheckServiceTypeMappedWithCustomer(customerId, serviceTypeId);
        }
    }
}
