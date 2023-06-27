using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Customer;
using DTO.References;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class CSConfigManager : ApiCommonData, ICSConfigManager
    {
        private readonly ICSConfigRepository _repo = null;
        private readonly IReferenceRepository _referenceRepo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly HRMap _hrmap = null;
        private readonly CSConfigMap _csconfigmap = null;
        private readonly ReferenceMap _refmap = null;
        private readonly IUserRepository _userRepository;
        private readonly IHumanResourceRepository _humanResourceRepository = null;
        private readonly ITenantProvider _filterService = null;
        public CSConfigManager(
            ICSConfigRepository repo,
            ITenantProvider filterService,
            IHumanResourceRepository humanResourceRepository,
            IReferenceRepository referenceRepo, IAPIUserContext applicationContext, IUserRepository userRepository)
        {
            _repo = repo;
            _referenceRepo = referenceRepo;
            _filterService = filterService;
            _ApplicationContext = applicationContext;
            _hrmap = new HRMap();
            _csconfigmap = new CSConfigMap();
            _humanResourceRepository = humanResourceRepository;
            _refmap = new ReferenceMap();
            _userRepository = userRepository;
        }
        public async Task<CSResponse> GetCustomerService()
        {
            var response = new CSResponse();
            var data = await _repo.GetAllCusService();
            if (data == null || data.Count == 0)
                response.CSList = null;
            else
            {
                response.CSList = data.Select(x => _hrmap.GetCustomerCS(x));
                response.Result = CSResult.Success;
            }
            return response;
        }
        //this delete from Cs configuration page
        public async Task<CSConfigDeleteResponse> DeleteCSConfig(CSConfigDelete id)
        {
            foreach (var _id in id.Id)
            {
                var CSConfig = await _repo.GetCSConfigDetails(_id);
                if (CSConfig == null)
                    return new CSConfigDeleteResponse { Result = CSConfigDeleteResult.NotFound };
                CSConfig.Active = false;
                _repo.Save(CSConfig);
            }
            return new CSConfigDeleteResponse { Result = CSConfigDeleteResult.Success };
        }

        // this delete operation happen when remove the office access from office configuration page
        public async Task<CSConfigDeleteResponse> DeleteCSConfigSummaryByLocation(int staffid, List<int> lstloc)
        {
            var CSConfig = await _repo.GetCSConfigDetailsByLocationId(staffid, lstloc);
            if (CSConfig != null && CSConfig.Any())
            {
                foreach (var cs in CSConfig)
                {
                    if (cs != null)
                    {
                        cs.Active = false;
                        _repo.Save(cs);
                    }
                }
            }

            return new CSConfigDeleteResponse { Result = CSConfigDeleteResult.Success };
        }
        public async Task<EditCSConfigResponse> GetEditCSConfig(int? id)
        {
            var response = new EditCSConfigResponse();
            if (id != null)
            {
                var customer = await _repo.GetCSConfigDetails(id);
                if (customer == null)
                    return null;
                response.CSConfigDetails = _csconfigmap.GetCSConfigSummaryDetails(customer);
                if (response.CSConfigDetails == null)
                    return new EditCSConfigResponse { Result = EditCSConfigResult.CannotGetCustomer };
            }
            response.Result = EditCSConfigResult.Success;
            return response;
        }

        public async Task<ServiceResponse> GetService()
        {
            var response = new ServiceResponse();
            var data = await _referenceRepo.GetServices();
            if (data == null || data.Count == 0)
                response.ServiceList = null;
            else
            {
                response.ServiceList = data.Select(x => _refmap.GetService(x));
                response.Result = ServiceResult.Success;
            }
            return response;
        }

        private List<SaveOneCSConfig> CSListInnerLoop(SaveCSConfig request, int? _customerId, int _officeId, bool _updateBool)
        {
            var CSList = new List<SaveOneCSConfig>();
            if (request.ServiceId != null && request.ServiceId.Count() > 0)
            {
                foreach (var _serviceId in request.ServiceId)
                {
                    //prouduct category only apply to inspection service
                    if (_serviceId == (int)Entities.Enums.Service.InspectionId && request.ProductCategoryId != null && request.ProductCategoryId.Count() > 0)
                    {
                        foreach (var _productCategoryId in request.ProductCategoryId)
                        {
                            if (_updateBool)
                                CSList.Add(_csconfigmap.SaveMapCSConfigEntity(_customerId,
                                    request.UserId, _officeId, _serviceId, _productCategoryId));
                            else
                                _updateBool = true;
                        }
                    }
                    else
                    {
                        if (_updateBool)
                            CSList.Add(_csconfigmap.SaveMapCSConfigEntity(_customerId,
                                 request.UserId, _officeId, _serviceId, null));
                        else
                            _updateBool = true;
                    }
                }
            }
            else
            {
                if (_updateBool)
                    CSList.Add(_csconfigmap.SaveMapCSConfigEntity(_customerId,
                                request.UserId, _officeId, null, null));
                else
                    _updateBool = true;
            }
            return CSList;
        }
        private List<SaveOneCSConfig> CSListLoop(SaveCSConfig request, bool _updateBool)
        {
            var CSList = new List<SaveOneCSConfig>();
            if (request.OfficeLocationId != null && request.OfficeLocationId.Count() > 0)
            {
                foreach (var _officeId in request.OfficeLocationId)
                {
                    if (request.CustomerId != null && request.CustomerId.Count() > 0)
                    {
                        foreach (var _customerId in request.CustomerId)
                        {
                            CSList.AddRange(CSListInnerLoop(request, _customerId, _officeId, _updateBool));
                        }
                    }
                    else
                        CSList.AddRange(CSListInnerLoop(request, null, _officeId, _updateBool));
                }
            }
            return CSList;
        }

        public async Task<SaveCSConfResponse> CSConfigSave(SaveCSConfig request)
        {
            var response = new SaveCSConfResponse();
            var CSList = new List<SaveOneCSConfig>();
            bool _updateBool = false;
            if (request.Id == 0)
            {
                _updateBool = true;
                //loop the fields to insert
                CSList.AddRange(CSListLoop(request, _updateBool));
                foreach (var _CS in CSList)
                {
                    CuCsConfiguration entity = _csconfigmap.MapCSConfigEntity(_CS);
                    response.CSConfigId = _repo.SaveNewCSConfig(entity);
                }
                response.Result = CSResult.Success;
            }
            else
            {
                var entity = await _repo.GetCSConfigDetails(request.Id);
                if (entity == null)
                    return new SaveCSConfResponse { Result = CSResult.CSIsNotFound };
                _csconfigmap.UpdateCSConfigEnity(entity, request);
                await _repo.EditCSConfig(entity);
                response.CSConfigId = entity.Id;
                _updateBool = false;
                //loop the fields to update
                CSList.AddRange(CSListLoop(request, _updateBool));
                foreach (var _CS in CSList)
                {
                    CuCsConfiguration entit = _csconfigmap.MapCSConfigEntity(_CS);
                    response.CSConfigId = _repo.SaveNewCSConfig(entit);
                }
                response.Result = CSResult.Success;
            }
            return response;
        }

        public CSConfigSearchResponse GetAllCSConfig(CSConfigSearchRequest request)
        {
            if (request == null)
                return new CSConfigSearchResponse() { Result = CSConfigSearchResResult.NotFound };
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;
            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 20;
            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            int take = request.pageSize.Value;
            var response = new CSConfigSearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };
            var data = _repo.GetAllCSConfig();
            if (request != null)
            {
                if (request.CustomerId > 0)
                    data = data.Where(x => x.CustomerId == request.CustomerId);
                if (request.UserId > 0)
                    data = data.Where(x => x.UserId == request.UserId);
                if (request.ServiceId != null && request.ServiceId.Count() > 0)
                    data = data.Where(x => request.ServiceId.ToList().Contains(x.ServiceId));
                if (request.OfficeLocationId != null && request.OfficeLocationId.Count() > 0)
                    data = data.Where(x => request.OfficeLocationId.ToList().Contains(x.OfficeLocationId));
                else
                {
                    var locIds = new List<int>();
                    if (_ApplicationContext.LocationList != null)
                        locIds = _ApplicationContext.LocationList.ToList();
                    if (locIds != null && locIds.Count() > 0)
                        data = data.Where(x => locIds.ToList().Contains(x.OfficeLocationId));
                }
                if (request.ProductCategoryId != null && request.ProductCategoryId.Count() > 0)
                    data = data.Where(x => request.ProductCategoryId.ToList().Contains(x.ProductCategoryId));
            }
            response.TotalCount = data.Count();
            try
            {
                if (response.TotalCount == 0)
                {
                    response.Result = CSConfigSearchResResult.NotFound;
                    return response;
                }
                var result = data.Skip(skip).Take(take).ToList();
                if (result == null || !result.Any())
                    return new CSConfigSearchResponse() { Result = CSConfigSearchResResult.NotFound };
                var _resultdata = result.Select(x => _csconfigmap.GetCustomerServiceConfigSearchResult(x));
                return new CSConfigSearchResponse()
                {
                    Result = CSConfigSearchResResult.Success,
                    TotalCount = response.TotalCount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                    Data = _resultdata
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CSAllocationResponse> GetCSAllocations(CSAllocationSearchRequest request)
        {
            if (request == null)
                return new CSAllocationResponse() { Result = CSAllocationResult.NotFound };
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;
            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;
            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            int take = request.pageSize.Value;
            var response = new CSAllocationResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };
            var data = _repo.GetAllCSAllocations();

            if (request != null)
            {
                if (request.CustomerId.HasValue && request.CustomerId > 0)
                    data = data.Where(x => x.CustomerId == request.CustomerId);

                if (request.UserType.HasValue && request.UserType > 0)
                    data = data.Where(x => x.UserType == request.UserType);                
                if (request.ServiceId > 0)
                    data = data.Where(x => x.DaUserByServices.Any(y => y.ServiceId == request.ServiceId));
                if (request.ProductCategoryIds != null && request.ProductCategoryIds.Any())
                    data = data.Where(x => x.DaUserByProductCategories.Any(y => y.ProductCategoryId.HasValue && request.ProductCategoryIds.Contains(y.ProductCategoryId.Value)));
                if (request.UserIds != null && request.UserIds.Any())
                    data = data.Where(x => request.UserIds.Contains(x.UserId));
                if (request.CustomerId > 0 && request.DepartmentIds != null && request.DepartmentIds.Any())
                    data = data.Where(x => x.DaUserByDepartments.Any(y => y.DepartmentId.HasValue && request.DepartmentIds.Contains(y.DepartmentId.Value)));
                if (request.CustomerId > 0 && request.BrandIds != null && request.BrandIds.Any())
                    data = data.Where(x => x.DaUserByBrands.Any(y => y.BrandId.HasValue && request.BrandIds.Contains(y.BrandId.Value)));
                if (request.OfficeId > 0)
                    data = data.Where(x => x.DaUserRoleNotificationByOffices.Any(y => y.OfficeId == request.OfficeId));
                if (request.FactoryCountryIds != null && request.FactoryCountryIds.Any())
                    data = data.Where(x => x.DaUserByFactoryCountries.Any(y => request.FactoryCountryIds.Contains(y.FactoryCountryId)));
            }

            response.TotalCount = await data.AsNoTracking().CountAsync();
            try
            {
                if (response.TotalCount == 0)
                {
                    response.Result = CSAllocationResult.NotFound;
                    return response;
                }

                var resultData = await data.Skip(skip).Take(take).Select(x => new CSAllocationData()
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerId.HasValue ? x.Customer.CustomerName : "",
                    BackupCS = x.PrimaryCs,
                    BackupReportChecker = x.PrimaryReportChecker,
                    UserType = x.UserType,
                    UserId = x.UserId,
                    StaffName = x.User.Staff.PersonName
                }).AsNoTracking().ToListAsync();

                var brands = await _repo.GetDaUserByBrands(resultData.Select(x => x.Id));
                var departments = await _repo.GetDaUserByDepartments(resultData.Select(x => x.Id));
                var productCategories = await _repo.GetDaUserByProductCategories(resultData.Select(x => x.Id));
                var services = await _repo.GetDaUserByServices(resultData.Select(x => x.Id));
                var offices = await _repo.GetDaUserRoleNotificationByOffices(resultData.Select(x => x.Id));
                var factoryCountries = await _repo.GetDaUserByFactoryCountry(data.Select(x => x.Id));
                var _resultdata = resultData.Select(x => _csconfigmap.GetCSAllocationSearchResult(x, brands, services, departments, offices, productCategories, factoryCountries)).ToList();
                return new CSAllocationResponse()
                {
                    Result = CSAllocationResult.Success,
                    TotalCount = response.TotalCount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                    CSList = _resultdata
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<SaveCSAllocationResponse> SaveCSAllocationAsync(SaveCSAllocation request)
        {
            var userIds = request.Staffs.Select(x => x.Id);
            //get all da user customer by customer id and HRProfile cs and report checker
            var savedDaUserCustomers = _repo.GetQueryable<DaUserCustomer>(x => x.Active == true && x.CustomerId.HasValue && userIds.Contains(x.UserId) && x.CustomerId == request.CustomerId && (x.UserType == (int)HRProfile.CS || x.UserType == (int)HRProfile.ReportChecker)).ToList();
            List<DaUserByBrand> brands = null;
            List<DaUserByDepartment> departments = null;
            List<DaUserByProductCategory> productCategories = null;
            List<DaUserByRole> roles = null;
            List<DaUserByService> services = null;
            List<DaUserRoleNotificationByOffice> offices = null;
            List<DaUserCustomer> daUserCustomers = null;
            List<DaUserByFactoryCountry> factoryCountries = null;
            //get user roles by userids
            var savedUserRoles = await _userRepository.GetUserRolesByUserIds(userIds);
            //if any saved da user customer available then fetch the da user brands, da user dept, da user product categories, da user services, da user offices, etc
            if (savedDaUserCustomers.Any())
            {
                var daUserCustomerIds = savedDaUserCustomers.Select(x => x.Id);
                brands = await _repo.GetDaUserByBrandByIds(daUserCustomerIds);
                departments = await _repo.GetDaUserByDepartmentByIds(daUserCustomerIds);
                productCategories = await _repo.GetDaUserByProductCategoryByIds(daUserCustomerIds);
                services = await _repo.GetDaUserByServiceByIds(daUserCustomerIds);
                offices = await _repo.GetDaUserRoleNotificationByOfficeByIds(daUserCustomerIds);
                factoryCountries = await _repo.GetDaUserByFactoryCountries(daUserCustomerIds);
                // based on userids fectch the da user customer 
                daUserCustomers = await _repo.GetDaUserCustomerByUserIds(userIds);
                // based on da user customers to select the da user roles
                roles = await _repo.GetDaUserByRoleByIds(daUserCustomers.Select(x => x.Id));
            }

            var entityId = _filterService.GetCompanyId();
            //staff loop
            foreach (var staff in request.Staffs)
            {
                //fetch the existing da user customer 
                var daUserCustomer = savedDaUserCustomers.FirstOrDefault(x => x.UserId == staff.Id);
                // fetch the da user roles;
                var userRoles = savedUserRoles.Where(x => x.UserId == staff.Id);
                // if da user customer available
                if (daUserCustomer != null)
                {
                    //saved da user customers any
                    if (savedDaUserCustomers.Any())
                    {
                        if (request.IsEdit)
                        {
                            // delete da user brands
                            var deleteBrands = brands.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.BrandId.HasValue && !request.BrandIds.Contains(x.BrandId.Value));
                            if (deleteBrands.Any())
                            {
                                _repo.RemoveEntities<DaUserByBrand>(deleteBrands);
                            }

                            // delete da user departments
                            var deleteDepartments = departments.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.DepartmentId.HasValue && !request.DepartmentIds.Contains(x.DepartmentId.Value));
                            if (deleteDepartments.Any())
                            {
                                _repo.RemoveEntities<DaUserByDepartment>(deleteDepartments);
                            }

                            // delete da user offices
                            var deleteOffices = offices.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.OfficeId.HasValue && !request.OfficeIds.Contains(x.OfficeId.Value));
                            if (deleteOffices.Any())
                            {
                                _repo.RemoveEntities<DaUserRoleNotificationByOffice>(deleteOffices);
                            }

                            // delete da user product categories
                            var deleteProductCategories = productCategories.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.ProductCategoryId.HasValue && !request.ProductCategoryIds.Contains(x.ProductCategoryId.Value));
                            if (deleteProductCategories.Any())
                            {
                                _repo.RemoveEntities<DaUserByProductCategory>(deleteProductCategories);
                            }

                            // delete da user offices
                            var deleteFactoryCountries = factoryCountries.Where(x => x.DaUserCustomerId == daUserCustomer.Id && !request.FactoryCountryIds.Contains(x.FactoryCountryId));
                            if (deleteFactoryCountries.Any())
                            {
                                _repo.RemoveEntities<DaUserByFactoryCountry>(deleteFactoryCountries);
                            }


                            // delete da user roles and user roles 
                            var deleteRoles = roles.Where(x => x.DauserCustomerId == daUserCustomer.Id && !staff.Notification.Contains(x.RoleId));
                            if (deleteRoles.Any())
                            {
                                _repo.RemoveEntities<DaUserByRole>(deleteRoles);
                                //roles.RemoveAll(x => deleteRoles.Select(y => y.Id).Contains(x.Id));
                                //get all of da user customers by userId
                                var currentDaUserCustomerOfUser = daUserCustomers.Where(x => x.UserId == daUserCustomer.UserId);
                                var daUserCustomerIDs = currentDaUserCustomerOfUser.Select(x => x.Id);
                                //get da user customer roles by da user customer Ids and deleted roles not include the deleted da user roles
                                var currenctUserDaUserRoles = roles.Where(x => daUserCustomerIDs.Contains(x.DauserCustomerId) && !deleteRoles.Select(y => y.Id).Contains(x.Id));
                                //get roles from the based on user id
                                var currenctUserRoles = userRoles.Where(x => x.UserId == daUserCustomer.UserId);
                                var deleteUserRoles = new List<ItUserRole>();

                                // loop of deleted user role
                                foreach (var daUserByRole in deleteRoles)
                                {
                                    //any da user by role is available for another da user customer 
                                    if (!currenctUserDaUserRoles.Any(x => x.RoleId == daUserByRole.RoleId))
                                    {
                                        var userRole = currenctUserRoles.FirstOrDefault(x => x.RoleId == daUserByRole.RoleId);
                                        if (userRole != null)
                                            deleteUserRoles.Add(userRole);
                                    }
                                }
                                if (deleteUserRoles.Any())
                                    _repo.RemoveEntities(deleteUserRoles);

                            }

                            var deleteServices = services.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.ServiceId.HasValue && !request.ServiceIds.Contains(x.ServiceId.Value));
                            if (deleteServices.Any())
                            {
                                _repo.RemoveEntities<DaUserByService>(deleteServices);
                            }

                        }
                        else
                        {
                            if (
                           Enumerable.SequenceEqual(request.BrandIds.OrderBy(x => x), brands.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.BrandId.HasValue).Select(y => y.BrandId.Value).OrderBy(x => x)) &&
                           Enumerable.SequenceEqual(request.DepartmentIds.OrderBy(x => x), departments.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.DepartmentId.HasValue).Select(y => y.DepartmentId.Value).OrderBy(x => x)) &&
                           Enumerable.SequenceEqual(request.OfficeIds.OrderBy(x => x), offices.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.OfficeId.HasValue).Select(y => y.OfficeId.Value).OrderBy(x => x)) &&
                           Enumerable.SequenceEqual(request.ProductCategoryIds.OrderBy(x => x), productCategories.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.ProductCategoryId.HasValue).Select(y => y.ProductCategoryId.Value).OrderBy(x => x)) &&
                           Enumerable.SequenceEqual(request.ServiceIds.OrderBy(x => x), services.Where(x => x.DauserCustomerId == daUserCustomer.Id && x.ServiceId.HasValue).Select(y => y.ServiceId.Value).OrderBy(x => x)) &&
                           Enumerable.SequenceEqual(staff.Notification.OrderBy(x => x), roles.Where(x => x.DauserCustomerId == daUserCustomer.Id).Select(y => y.RoleId).OrderBy(x => x)) &&
                           Enumerable.SequenceEqual(request.FactoryCountryIds.OrderBy(x => x), factoryCountries.Where(x => x.DaUserCustomerId == daUserCustomer.Id).Select(y => y.FactoryCountryId).OrderBy(x => x)) &&
                          staff.PrimaryCS == daUserCustomer.PrimaryCs && staff.PrimaryReportChecker == daUserCustomer.PrimaryReportChecker && staff.Profile == daUserCustomer.UserType)
                            {
                                return new SaveCSAllocationResponse() { Result = CSAllocationResult.StaffAlreadyConfigured };
                            }
                        }
                    }



                    var dbBrandIds = brands.Where(x => x.BrandId.HasValue && x.DauserCustomerId == daUserCustomer.Id).Select(x => x.BrandId.Value);
                    var dbDepartmentIds = departments.Where(x => x.DepartmentId.HasValue && x.DauserCustomerId == daUserCustomer.Id).Select(x => x.DepartmentId.Value);
                    var dbOfficeIds = offices.Where(x => x.OfficeId.HasValue && x.DauserCustomerId == daUserCustomer.Id).Select(x => x.OfficeId.Value);
                    var dbFactoryCountryIds = factoryCountries.Where(x => x.DaUserCustomerId == daUserCustomer.Id).Select(x => x.FactoryCountryId).ToList();
                    var dbProductCategoryIds = productCategories.Where(x => x.ProductCategoryId.HasValue && x.DauserCustomerId == daUserCustomer.Id).Select(x => x.ProductCategoryId.Value);
                    var dbServiceIds = services.Where(x => x.ServiceId.HasValue && x.DauserCustomerId == daUserCustomer.Id).Select(x => x.ServiceId.Value);
                    var dbRoleIds = roles.Where(x => x.DauserCustomerId == daUserCustomer.Id).Select(x => x.RoleId);


                    var input = new SaveDaUserCustomerItem()
                    {
                        BrandIds = request.BrandIds.Where(x => !dbBrandIds.Contains(x)),
                        CreatedBy = _ApplicationContext.UserId,
                        CustomerId = request.CustomerId,
                        DepartmentIds = request.DepartmentIds.Where(x => !dbDepartmentIds.Contains(x)),
                        OfficeIds = request.OfficeIds.Where(x => !dbOfficeIds.Contains(x)),
                        EntityId = entityId,
                        ProductCategoryIds = request.ProductCategoryIds.Where(x => !dbProductCategoryIds.Contains(x)),
                        ServiceIds = request.ServiceIds.Where(x => !dbServiceIds.Contains(x)),
                        NotificationIds = staff.Notification.Where(x => !dbRoleIds.Contains(x)),
                        PrimaryCS = staff.PrimaryCS,
                        PrimaryReportChecker = staff.PrimaryReportChecker,
                        Profile = staff.Profile,
                        FactoryCountryIds = request.FactoryCountryIds.Where(x => !dbFactoryCountryIds.Contains(x))
                    };
                    daUserCustomer = _csconfigmap.MapDaUserCustomerEntity(daUserCustomer, input);
                    _repo.EditEntity(daUserCustomer);
                }
                else
                {
                    daUserCustomer = _csconfigmap.MapDaUserCustomerEntity(staff.Id, entityId, staff, request, _ApplicationContext.UserId);
                    await _repo.SaveCSAllocation(daUserCustomer);
                }

                var dbUserRoleIds = userRoles.Select(y => y.RoleId);
                var newNotificationRoles = staff.Notification.Where(x => !dbUserRoleIds.Contains(x));
                foreach (var role in newNotificationRoles)
                {
                    var userRole = new ItUserRole() { UserId = staff.Id, RoleId = role, EntityId = entityId };
                    _repo.AddEntity(userRole);
                }

            }

            await _repo.Save();


            return new SaveCSAllocationResponse() { Result = CSAllocationResult.Success };
        }

        public async Task<GetCSAllocationResponse> GetCSAllocation(int id)
        {
            var daUserCustomer = _repo.GetSingle<DaUserCustomer>(x => x.Id == id && x.Active.HasValue && x.Active.Value);
            if (daUserCustomer == null)
                return new GetCSAllocationResponse() { Result = CSAllocationResult.NotFound };
            var daUserCustomerIds = new List<int> { daUserCustomer.Id };
            var brands = await _repo.GetDaUserByBrands(daUserCustomerIds);
            var departments = await _repo.GetDaUserByDepartments(daUserCustomerIds);
            var productCategories = await _repo.GetDaUserByProductCategories(daUserCustomerIds);
            var roles = await _repo.GetDaUserByRoles(daUserCustomerIds);
            var services = await _repo.GetDaUserByServices(daUserCustomerIds);
            var offices = await _repo.GetDaUserRoleNotificationByOffices(daUserCustomerIds);

            var factoryCountries = await _repo.GetDaUserByFactoryCountry(daUserCustomerIds);

            var staff = await _humanResourceRepository.GetStaffByUserId(daUserCustomer.UserId);
            var csAllocation = _csconfigmap.GetCSAllocationMap(daUserCustomer, staff, brands.Select(x => x.Id), services.Select(x => x.Id), departments.Select(x => x.Id), offices.Select(x => x.Id), productCategories.Select(x => x.Id), roles.Select(x => x.Id), factoryCountries.Select(x => x.Id).ToList());
            return new GetCSAllocationResponse() { Result = CSAllocationResult.Success, CSAllocation = csAllocation };
        }
        public async Task<DeleteCSAllocationResponse> DeleteAllocationsAsync(CSAllocationDeleteItem request)
        {
            var daUserCustomers = await _repo.GetDaUserCustomerByIds(request.DaUserCustomerIds);
            var ids = daUserCustomers.Select(x => x.Id);
            var userIds = daUserCustomers.Select(x => x.UserId);
            //new List<IEnumerable<int>>() { ids };

            //get the da user brands, departments, product categories, offices
            var brands = await _repo.GetDaUserByBrandByIds(ids);
            var departments = await _repo.GetDaUserByDepartmentByIds(ids);
            var productCategories = await _repo.GetDaUserByProductCategoryByIds(ids);
            var services = await _repo.GetDaUserByServiceByIds(ids);
            var offices = await _repo.GetDaUserRoleNotificationByOfficeByIds(ids);
            var factoryCountries = await _repo.GetDaUserByFactoryCountries(ids);
            //get user roles based on user ids
            var userRoles = await _userRepository.GetUserRolesByUserIds(userIds);
            //get da user roles based on userids
            var daUserCustomerOfUsers = await _repo.GetDaUserCustomerByUserIds(userIds);
            var daUserByRoles = await _repo.GetDaUserByRoleByIds(daUserCustomerOfUsers.Select(x => x.Id));
            //loop of ids 
            foreach (var daUsercustomerId in request.DaUserCustomerIds)
            {
                //get da user customer 
                var daUserCustomer = daUserCustomers.FirstOrDefault(x => x.Id == daUsercustomerId);
                if (daUserCustomer == null)
                    return new DeleteCSAllocationResponse() { Result = CSAllocationResult.NotFound };

                //get da user by roles based on the da user customer id
                var daUserCustomerRoles = daUserByRoles.Where(x => x.DauserCustomerId == daUserCustomer.Id);

                _repo.RemoveEntities<DaUserByBrand>(brands.Where(x => x.DauserCustomerId == daUserCustomer.Id));
                _repo.RemoveEntities<DaUserByDepartment>(departments.Where(x => x.DauserCustomerId == daUserCustomer.Id));
                _repo.RemoveEntities<DaUserByProductCategory>(productCategories.Where(x => x.DauserCustomerId == daUserCustomer.Id));
                _repo.RemoveEntities<DaUserByRole>(daUserCustomerRoles);
                _repo.RemoveEntities<DaUserByService>(services.Where(x => x.DauserCustomerId == daUserCustomer.Id));
                _repo.RemoveEntities<DaUserRoleNotificationByOffice>(offices.Where(x => x.DauserCustomerId == daUserCustomer.Id));
                _repo.RemoveEntities<DaUserByFactoryCountry>(factoryCountries.Where(x => x.DaUserCustomerId == daUserCustomer.Id));

                //filter the roles
                daUserByRoles = daUserByRoles.Where(x => x.DauserCustomerId != daUserCustomer.Id).ToList();

                //get the da user customer by user ids
                var currentDaUserCustomerOfUser = daUserCustomerOfUsers.Where(x => x.UserId == daUserCustomer.UserId);
                var daUserCustomerIDs = currentDaUserCustomerOfUser.Select(x => x.Id);

                //get the current da user roles
                var currentUserDaUserRoles = daUserByRoles.Where(x => daUserCustomerIDs.Contains(x.DauserCustomerId));
                // get the current user roles
                var currentUserRoles = userRoles.Where(x => x.UserId == daUserCustomer.UserId);
                var deleteUserRoles = new List<ItUserRole>();

                //loop delete da user roles
                foreach (var daUserByRole in daUserCustomerRoles)
                {
                    //check any current da user role  available
                    if (!currentUserDaUserRoles.Any(x => x.RoleId == daUserByRole.RoleId))
                    {
                        var userRole = currentUserRoles.FirstOrDefault(x => x.RoleId == daUserByRole.RoleId);
                        if (userRole != null)
                            deleteUserRoles.Add(userRole);
                    }
                }
                if (deleteUserRoles.Any())
                    _repo.RemoveEntities(deleteUserRoles);


                _repo.RemoveEntities<DaUserCustomer>(new List<DaUserCustomer>() { daUserCustomer });
            }
            await _repo.Save();
            return new DeleteCSAllocationResponse() { Result = CSAllocationResult.Success };

        }

        public async Task<List<ExportCSAllocationData>> ExportCSAllocationSummary(CSAllocationSearchRequest request)
        {
            if (request == null)
            {
                return null;
            }
            var data = _repo.GetAllCSAllocations();

            if (request != null)
            {
                if (request.CustomerId > 0)
                    data = data.Where(x => x.CustomerId == request.CustomerId);
                if (request.UserType > 0)
                    data = data.Where(x => x.UserType == request.UserType);
                if (request.ServiceId > 0)
                    data = data.Where(x => x.DaUserByServices.Any(y => y.ServiceId == request.ServiceId));
                if (request.ProductCategoryIds != null && request.ProductCategoryIds.Any())
                    data = data.Where(x => x.DaUserByProductCategories.Any(y => y.ProductCategoryId.HasValue && request.ProductCategoryIds.Contains(y.ProductCategoryId.Value)));
                if (request.CustomerId > 0 && request.DepartmentIds != null && request.DepartmentIds.Any())
                    data = data.Where(x => x.DaUserByDepartments.Any(y => y.DepartmentId.HasValue && request.DepartmentIds.Contains(y.DepartmentId.Value)));
                if (request.CustomerId > 0 && request.BrandIds != null && request.BrandIds.Any())
                    data = data.Where(x => x.DaUserByBrands.Any(y => y.BrandId.HasValue && request.BrandIds.Contains(y.BrandId.Value)));
                if (request.OfficeId > 0)
                    data = data.Where(x => x.DaUserRoleNotificationByOffices.Any(y => y.OfficeId == request.OfficeId));
                if (request.FactoryCountryIds != null && request.FactoryCountryIds.Any())
                    data = data.Where(x => x.DaUserByFactoryCountries.Any(y => request.FactoryCountryIds.Contains(y.FactoryCountryId)));
            }

            var resultData = await data.Select(x => new CSAllocationData()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                CustomerName = x.CustomerId.HasValue ? x.Customer.CustomerName : "",
                BackupCS = x.PrimaryCs,
                BackupReportChecker = x.PrimaryReportChecker,
                UserType = x.UserType,
                UserId = x.UserId,
                StaffName = x.User.Staff.PersonName
            }).AsNoTracking().ToListAsync();

            var brands = await _repo.GetDaUserByBrands(data.Select(x => x.Id));
            var departments = await _repo.GetDaUserByDepartments(data.Select(x => x.Id));
            var productCategories = await _repo.GetDaUserByProductCategories(data.Select(x => x.Id));
            var services = await _repo.GetDaUserByServices(data.Select(x => x.Id));
            var factoryCountries = await _repo.GetDaUserByFactoryCountry(data.Select(x => x.Id));
            var offices = await _repo.GetDaUserRoleNotificationByOffices(data.Select(x => x.Id));
            return resultData.Select(x => _csconfigmap.ExportCSAllocationSearchResult(x, brands, departments, productCategories, services, offices, factoryCountries)).ToList();

        }
    }
}
