using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.User;
using DTO.UserAccount;
using DTO.UserProfile;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BI
{
    public class UserAccountManager : ApiCommonData, IUserAccountManager
    {
        #region Declaration 
        private readonly IUserAccountRepository _repository = null;
        private readonly ICacheManager _cache = null;
        private readonly ILogger _logger = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ILocationRepository _locationRepository = null;
        private readonly IHumanResourceRepository _hrRepository = null;
        private readonly ICustomerRepository _cusRepository = null;
        private readonly ISupplierRepository _supRepository = null;
        private readonly IUserRepository _userRepository = null;
        private readonly LocationMap LocationMap = null;
        private readonly UserAccountMap UserAccountMap = null;
        private ITenantProvider _filterService = null;
        private readonly ITenantProvider _tenantProvider = null;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        #endregion Declaration

        #region Constructor
        public UserAccountManager(IUserAccountRepository repository, ILocationRepository locationRepository, IHumanResourceRepository hrRepository, ICacheManager cache, ILogger<UserAccountManager> logger,
            IAPIUserContext applicationContext, ICustomerRepository cusRepository, ISupplierRepository supRepository, IUserRepository userRepository, ITenantProvider filterService,
            ITenantProvider tenantProvider, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
            _ApplicationContext = applicationContext;
            _locationRepository = locationRepository;
            _hrRepository = hrRepository;
            _cusRepository = cusRepository;
            _supRepository = supRepository;
            _userRepository = userRepository;
            _filterService = filterService;
            LocationMap = new LocationMap();
            UserAccountMap = new UserAccountMap();
            _tenantProvider = tenantProvider;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion Constructor

        public UserAccountResponse GetUserAccountSummary()
        {
            var response = new UserAccountResponse();
            try
            {
                response.CountryList = _locationRepository.GetCountryList().Select(LocationMap.GetCountry).ToArray();

                if (response.CountryList == null)
                    return new UserAccountResponse { Result = UserAccountResult.CannotGetListCountry };
                response.Result = UserAccountResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get user account summary");
            }
            return response;
        }

        public async Task<UserAccountSearchResponse> GetUserAccountSearchSummary(UserAccountSearchRequest request)
        {
            var response = new UserAccountSearchResponse() { Index = request.Index.Value, PageSize = request.pageSize.Value };
            try
            {
                int skip = (request.Index.Value - 1) * request.pageSize.Value;
                switch (request.UserTypeId)
                {
                    case (int)UserTypeEnum.InternalUser:

                        response = await GetUserAccountListByType(request, response, UserTypeEnum.InternalUser);
                        break;
                    case (int)UserTypeEnum.Customer:
                        var customer = _cusRepository.GetAllCustomersItems();
                        if (!string.IsNullOrEmpty(request.Name) && !string.IsNullOrWhiteSpace(request.Name))
                        {
                            customer = customer.Where(x => x.CustomerName.ToLower().Contains(request.Name.ToLower()));
                        }
                        if (request.CountryValues != null && request.CountryValues.Any())
                        {
                            customer = customer.Where(x => request.CountryValues.Any(y => x.CuCustomerBusinessCountries.Any(z => z.BusinessCountryId == y.Id)));
                        }
                        response.TotalCount = customer.Count();
                        response.Data = customer.Skip(skip).Take(request.pageSize.Value).Select(UserAccountMap.GetCustomer).ToArray();
                        break;
                    case (int)UserTypeEnum.Supplier:
                        var supplier = _supRepository.GetSuppliersSearchData().Where(x => x.Type.Id == (int)Supplier_Type.Supplier_Agent);
                        if (!string.IsNullOrEmpty(request.Name) && !string.IsNullOrWhiteSpace(request.Name))
                        {
                            supplier = supplier.Where(x => x.SupplierName.ToLower().Contains(request.Name.ToLower()));
                        }
                        if (request.CountryValues != null && request.CountryValues.Any())
                        {
                            supplier = supplier.Where(x => x.SuAddresses.Any(z => request.CountryValues.Select(x => x.Id).Contains(z.CountryId)));
                        }
                        response.TotalCount = await supplier.Select(x => x.Id).CountAsync();
                        supplier = supplier.Skip(skip).Take(request.pageSize.Value);
                        response.Data = await supplier
                            .Select(x => new UserAccount()
                            {
                                Id = x.Id,
                                Name = x.SupplierName,
                                HasAccount = x.ItUserMasterSuppliers.Any(),
                                UserTypeId = (int)UserTypeEnum.Supplier
                            }).AsNoTracking().ToListAsync();

                        var supplieraadress = await _supRepository.GetAddressCountry(supplier.Select(x => x.Id));
                        response.Data = response.Data.Select(x => UserAccountMap.MapAccountCountry(x, supplieraadress));

                        break;
                    case (int)UserTypeEnum.Factory:
                        var factory = _supRepository.GetSuppliersSearchData().Where(x => x.Type.Id == (int)Supplier_Type.Factory);
                        if (!string.IsNullOrEmpty(request.Name) && !string.IsNullOrWhiteSpace(request.Name))
                        {
                            factory = factory.Where(x => x.SupplierName.ToLower().Contains(request.Name.ToLower()));
                        }
                        if (request.CountryValues != null && request.CountryValues.Any())
                        {
                            factory = factory.Where(x => x.SuAddresses.Any(z => request.CountryValues.Select(x => x.Id).Contains(z.CountryId)));
                        }
                        response.TotalCount = await factory.Select(x => x.Id).CountAsync();

                        response.Data = await factory.Skip(skip).Take(request.pageSize.Value)
                                        .Select(x => new UserAccount()
                                        {
                                            Id = x.Id,
                                            Name = x.SupplierName,
                                            HasAccount = x.ItUserMasterSuppliers.Any(),
                                            UserTypeId = (int)UserTypeEnum.Factory
                                        }).AsNoTracking().ToListAsync();
                        var factoryraadress = await _supRepository.GetAddressCountry(factory.Select(x => x.Id));
                        response.Data = response.Data.Select(x => UserAccountMap.MapAccountCountry(x, factoryraadress));
                        break;
                    case (int)UserTypeEnum.OutSource:
                        response = await GetUserAccountListByType(request, response, UserTypeEnum.OutSource);
                        break;
                    default:
                        break;
                }
                response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                if (response.TotalCount == 0)
                {
                    response.Result = UserAccountSearchResult.NotFound;
                    return response;
                }
                response.Result = UserAccountSearchResult.Success;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get user account search summary");
            }
            return response;
        }

        /// <summary>
        /// Get the user account list by user type
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="userTypeEnum"></param>
        /// <returns></returns>
        public async Task<UserAccountSearchResponse> GetUserAccountListByType(UserAccountSearchRequest request, UserAccountSearchResponse response, UserTypeEnum userTypeEnum)
        {
            var hrStaffQuery = _hrRepository.GetStaffData();
            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            //if user type is internal user take the employees which is not available in outsource type
            if (userTypeEnum == UserTypeEnum.InternalUser)
                hrStaffQuery = hrStaffQuery.Where(x => x.EmployeeTypeId == (int)EmployeeTypeEnum.Permanent);
            //if user type is internal user take the employees which is available in outsource type
            else if (userTypeEnum == UserTypeEnum.OutSource)
                hrStaffQuery = hrStaffQuery.Where(x => x.EmployeeTypeId != (int)EmployeeTypeEnum.Permanent);

            //apply the name filter
            if (!string.IsNullOrEmpty(request.Name) && !string.IsNullOrWhiteSpace(request.Name))
            {
                hrStaffQuery = hrStaffQuery.Where(x => x.PersonName.ToLower().Contains(request.Name.ToLower()));
            }
            //apply the country filter
            if (request.CountryValues != null && request.CountryValues.Any())
            {
                hrStaffQuery = hrStaffQuery.Where(x => request.CountryValues.Any(y => y.Id == x.NationalityCountryId));
            }
            //execute the total count
            response.TotalCount = await hrStaffQuery.CountAsync();
            //execute the result
            //map the user account data
            var staffData = await hrStaffQuery.OrderBy(x => x.PersonName).Skip(skip).Take(request.pageSize.Value).ToListAsync();

            response.Data = staffData.Select(x => UserAccountMap.MapUserAccountList(x, (int)userTypeEnum));

            return response;
        }

        public async Task<EditUserAccountResponse> GetUserAccountDetail(UserAccountSearchRequest request)
        {
            var response = new EditUserAccountResponse() { Index = request.Index.Value, PageSize = request.pageSize.Value };
            try
            {
                var rolelst = _userRepository.GetRoleList();
                response.RoleList = rolelst.Select(UserAccountMap.GetRole).ToArray();
                if (response.RoleList == null)
                    return new EditUserAccountResponse { Result = EditUserAccountResult.CanNotGetRoleList };

                var users = _repository.GetUserByType(request.UserTypeId);
                var types = _repository.GetUserTypes();

                switch (request.UserTypeId)
                {
                    case (int)UserTypeEnum.InternalUser:
                        response.Name = _hrRepository.GetStaffList().FirstOrDefault(x => x.Id == request.Id).PersonName;
                        users = users.Where(x => x.StaffId == request.Id);
                        break;
                    case (int)UserTypeEnum.Customer:
                        var customer = _cusRepository.GetAllCustomersItems().FirstOrDefault(x => x.Id == request.Id);
                        response.Name = customer.CustomerName;
                        response.ContactList = await _cusRepository.GetCustomerContacts(customer.Id);
                        users = users.Where(x => x.CustomerId == request.Id);
                        break;
                    case (int)UserTypeEnum.Supplier:
                        var supplier = _supRepository.GetSuppliersSearchData().Where(x => x.Type.Id == (int)Supplier_Type.Supplier_Agent && x.Id == request.Id).FirstOrDefault();
                        response.Name = supplier.SupplierName;
                        response.ContactList = await _supRepository.GetSupplierContactById(supplier.Id);
                        users = users.Where(x => x.SupplierId == request.Id);
                        break;
                    case (int)UserTypeEnum.Factory:
                        var factory = _supRepository.GetSuppliersSearchData().Where(x => x.Type.Id == (int)Supplier_Type.Factory && x.Id == request.Id).FirstOrDefault();
                        response.Name = factory.SupplierName;
                        response.ContactList = await _supRepository.GetSupplierContactById(factory.Id);
                        users = users.Where(x => x.FactoryId == request.Id);
                        break;
                    case (int)UserTypeEnum.OutSource:
                        var hrStaff = _hrRepository.GetStaffList().FirstOrDefault(x => x.Id == request.Id);
                        response.Name = hrStaff?.PersonName;
                        users = users.Where(x => x.StaffId == request.Id);
                        break;
                    default:
                        break;
                }
                response.RoleName = types.FirstOrDefault(x => x.Id == request.UserTypeId).Label;
                response.TotalCount = await users.CountAsync();
                if (response.TotalCount == 0)
                {
                    response.Result = EditUserAccountResult.CanNotGetUserAccountList;
                    return response;
                }
                int skip = (request.Index.Value - 1) * request.pageSize.Value;
                response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                response.UserAccountList =await users.Skip(skip).Take(request.pageSize.Value).Select(x => UserAccountMap.GetUserAccountItem(x, response.ContactList)).IgnoreQueryFilters().ToArrayAsync();
                if (response.UserAccountList == null)
                    return new EditUserAccountResponse { Result = EditUserAccountResult.CanNotGetUserAccountList };

                response.Result = EditUserAccountResult.Success;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get user account detail");
            }

            return response;
        }

        public async Task<SaveUserAccountResponse> SaveUserAccount(UserAccountItem request)
        {
            var response = new SaveUserAccountResponse();
            try
            {
                if (request.Id == 0)
                {
                    response = await AddUser(request);
                }
                else
                {
                    response = await EditUser(request);
                }
                var entityId = _filterService.GetCompanyId();
                //Get the service configured for the user
                response.ServiceIds = await GetServiceAccess(request, entityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save user account");
            }
            return response;
        }

        /// <summary>
        /// Get the service configured for the user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<List<int>> GetServiceAccess(UserAccountItem request, int entityId)
        {
            var serviceIds = new List<int>();
            switch (request.UserTypeId.GetValueOrDefault(0))
            {
                case (int)UserTypeEnum.Supplier:
                    {
                        serviceIds = await _supRepository.GetSupplierContactServiceIds(request.Contact, entityId);
                        break;
                    }
                case (int)UserTypeEnum.Customer:
                    {
                        serviceIds = await _cusRepository.GetCustomerContactServiceIds(request.Contact, entityId);
                        break;
                    }
            }

            return serviceIds;
        }

        private async Task<SaveUserAccountResponse> AddUser(UserAccountItem request)
        {
            try
            {
                if (_repository.GetUserByName(request.UserName, null))
                    return new SaveUserAccountResponse() { Result = SaveResult.DuplicateName };

                ItUserMaster entity = UserAccountMap.MapUserAccountEntity(request, _ApplicationContext.UserId);

                if (entity == null)
                    return new SaveUserAccountResponse() { Result = SaveResult.CannotMapRequestToEntites };

                int id = await _repository.SaveNewUserAccount(entity);

                if (id != 0)
                    return new SaveUserAccountResponse() { Id = entity.Id, Result = SaveResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save new user account");
            }
            return new SaveUserAccountResponse() { Result = SaveResult.CannotSaveUserAccount };
        }

        private async Task<SaveUserAccountResponse> EditUser(UserAccountItem request)
        {
            var entity = await _userRepository.GetUserDetails(request.Id);
            try
            {
                if (_repository.GetUserByName(request.UserName, request.Id))
                    return new SaveUserAccountResponse() { Result = SaveResult.DuplicateName };

                if (entity == null)
                    return new SaveUserAccountResponse() { Result = SaveResult.CurrentUserAccountNotFound };

                #region mapping
                var Roles = entity.ItUserRoles.ToList();

                if (Roles.Count > 0)
                    _userRepository.RemoveEntities(Roles);

                var updatedby = _ApplicationContext.UserId;
                entity = UserAccountMap.MapEditUserAccountEntity(entity, request, updatedby);

                foreach (var element in request.Roles)
                {
                    var roleEntityList = request.UserRoleEntityList.Where(x => x.RoleId == element).SelectMany(x => x.RoleEntity).ToArray();

                    foreach (var entityId in roleEntityList)
                    {
                        ItUserRole role = new ItUserRole();
                        role.UserId = request.Id;
                        role.RoleId = element;
                        role.EntityId = entityId;
                        entity.ItUserRoles.Add(role);
                        _repository.AddEntity(role);
                    }

                }

                #endregion
                int re = await _repository.SaveEditUserAccount(entity);
                if (re > 0)
                    return new SaveUserAccountResponse { Id = entity.Id, Result = SaveResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save edit user account");
            }
            return new SaveUserAccountResponse { Id = entity.Id, Result = SaveResult.CannotSaveUserAccount };

        }

        public async Task<DeleteUserAccountResponse> RemoveUserAccount(int id)
        {
            var item = await _userRepository.GetUserDetails(id);
            try
            {
                if (item == null)
                    return new DeleteUserAccountResponse { Id = id, Result = DeleteResult.NotFound };
                var updatedby = _ApplicationContext.UserId;
                await _repository.RemoveUserAccount(item, updatedby);

                return new DeleteUserAccountResponse() { Id = id, Result = DeleteResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "remove user account");
            }
            return new DeleteUserAccountResponse() { Id = id, Result = DeleteResult.CannotDelete };
        }
        public bool LoggedUserRoleExists(int roleId)
        {
            var isRole = _ApplicationContext.RoleList.Contains(roleId) ?
                            true : false;
            return isRole;
        }

        //get user name by id
        public async Task<UserNameResponse> GetUserName(int id)
        {
            try
            {
                //get user name from it_usermaster table
                string UserName = await _repository.GetUserName(id);

                if (!string.IsNullOrEmpty(UserName))
                {
                    return new UserNameResponse()
                    {
                        Result = ResponseResult.Success,
                        Name = UserName
                    };
                }
                else
                {
                    return new UserNameResponse()
                    { Result = ResponseResult.NotFound };
                }
            }
            catch (Exception ex)
            {
                return new UserNameResponse()
                { Result = ResponseResult.Failure };
            }

        }
        /// <summary>
        /// login details get for supplier or factory contact
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveUserResponse> GetLoginUserDetail(int contactId, int usertypeId)
        {
            try
            {
                if (!(contactId > 0 || usertypeId > 0))
                    return new SaveUserResponse() { Result = SaveResult.RequestNotCorrectFormat };

                //contact id check
                if (contactId > 0)
                {
                    var credentialData = new ItUserMaster();

                    //CREDENTIALS exists check
                    var credentialsList = _repository.GetUserDetails();

                    //factory - 1
                    if (usertypeId == (int)UserTypeEnum.Factory)
                    {
                        credentialData = await credentialsList.Where(x => x.FactoryContactId == contactId).FirstOrDefaultAsync();

                    }
                    else if (usertypeId == (int)UserTypeEnum.Supplier)
                    {
                        credentialData = await credentialsList.Where(x => x.SupplierContactId == contactId).FirstOrDefaultAsync();
                    }
                    else if (usertypeId == (int)UserTypeEnum.Customer)
                    {
                        credentialData = await credentialsList.Where(x => x.CustomerContactId == contactId).FirstOrDefaultAsync();
                    }

                    if (credentialData?.Id > 0)
                    {
                        return new SaveUserResponse()
                        {
                            Id = credentialData.Id,
                            Password = UserAccountMap.DecryptPassword(credentialData.Password),
                            UserName = credentialData.LoginName,
                            ChangePassword = credentialData.ChangePassword,
                            Result = SaveResult.Success
                        };
                    }
                    else
                    {
                        return new SaveUserResponse()
                        {
                            Result = SaveResult.CurrentUserAccountNotFound
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get user account");
            }
            return new SaveUserResponse()
            {
                Result = SaveResult.Failure
            };
        }
        /// <summary>
        /// add the user to it_usermaster
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveUserResponse> AddUserDetails(UserAccountItem request)
        {
            try
            {             
                if (request == null)
                    return new SaveUserResponse() { Result = SaveResult.RequestNotCorrectFormat };

                var UserId = request.CreatedBy > 0 ? request.CreatedBy.Value : _ApplicationContext.UserId;

                request = await MapUserData(request);

                ItUserMaster entity = UserAccountMap.MapUserAccountEntity(request, UserId);

                if (entity == null)
                    return new SaveUserResponse() { Result = SaveResult.CannotMapRequestToEntites };

                int id = await _repository.SaveNewUserAccount(entity);

                if (id > 0)
                    return new SaveUserResponse()
                    {
                        Id = entity.Id,
                        Password = UserAccountMap.DecryptPassword(entity.Password),
                        UserName = entity.LoginName,
                        Result = SaveResult.Success
                    };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save new user account");
            }
            return new SaveUserResponse() { Result = SaveResult.CannotSaveUserAccount };
        }

        /// <summary>
        /// map user data - user type, roles, username, fullname, password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<UserAccountItem> MapUserData(UserAccountItem request)
        {
            if (!string.IsNullOrWhiteSpace(request.UserName))
            {
                var isUserExists = await _repository.CheckLoginNameExist(request.UserName);
                if (isUserExists)
                    request.UserName = request.UserName.Replace("@", DateTime.Now.ToString("ddMMyyyyHHmmss") + "@");
            }

            request.Password = GetRandomPassword(10);

            //eg: supplier name => wuxi thermal Ltd and contact name => nixon antony
            //user name => sup_wtl_nixon

            return request;
        }


        public async Task<RolesResponse> GetRoles()
        {
            var data = await _repository.GetListAsync<ItRole>(x => x.Active != null && x.Active.Value);

            if (data == null || !data.Any())
                return new RolesResponse
                {
                    Result = RolesResult.NotFound
                };

            return new RolesResponse
            {
                Result = RolesResult.Success,
                List = data.Select(x => new DTO.UserProfile.Role
                {
                    Id = x.Id,
                    Name = x.RoleName
                })
            };
        }

        /// <summary>
        /// get unique user name
        /// </summary>
        /// <param name="requestUserName"></param>
        /// <returns></returns>
        private async Task<string> GetUniqueUserName(string requestUserName)
        {
            string userNameFormat = string.Empty;
            var userName = "";

            if (!string.IsNullOrWhiteSpace(requestUserName))
            {
                bool isNumeric = false;
                int number = 0;

                userNameFormat = requestUserName.Replace(" ", "").Trim();
                userName = userNameFormat;

                //user name exists
                string sameFormatExistsUserName = await _repository.GetLoginName(userNameFormat);

                var userNameLength = userNameFormat.Length;

                if (!string.IsNullOrWhiteSpace(sameFormatExistsUserName))
                {
                    var maxDigit = sameFormatExistsUserName.Substring(userNameLength, sameFormatExistsUserName.Length - userNameLength);
                    isNumeric = int.TryParse(maxDigit, out number);
                    int i = 0;
                    do
                    {
                        if (isNumeric)
                        {
                            number = number + 1;
                        }
                        else if (i >= 1)
                        {
                            number = number + 1;
                        }
                        else
                        {
                            number = 1;
                        }

                        userName = userNameFormat + number;
                        i++;
                    } while (await _repository.CheckLoginNameExist(userName));
                }
            }
            return userName;
        }
        //get Customer by customer name substring
        public async Task<DataSourceResponse> GetUserDataSource(UserDataSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _hrRepository.GetUserDataSource();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }
                if (request.Id != null && request.Id > 0)
                {
                    data = data.Where(x => x.Id == request.Id);
                }

                // filter by user ids
                if (request.IdList != null && request.IdList.Any())
                {
                    data = data.Where(x => request.IdList.Contains(x.Id));
                }

                var userList = await data.Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync();

                if (userList == null || !userList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = userList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get the user application details(for customer,supplier,factory)
        /// </summary>
        /// <returns></returns>
        public async Task<UserApplicantDetails> GetUserApplicantDetails()
        {
            var userApplicantDetails = new UserApplicantDetails();
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        var customerContactId = await _repository.GetCustomerContactIdByUser(_ApplicationContext.UserId);
                        userApplicantDetails = await _repository.GetCustomerContactUserApplicationDetails(customerContactId);
                        userApplicantDetails.ContactId = customerContactId;
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        var supplierContactId = await _repository.GetSupplierContactIdByUser(_ApplicationContext.UserId);
                        userApplicantDetails = await _repository.GetSupplierContactUserApplicationDetails(supplierContactId);
                        userApplicantDetails.ContactId = supplierContactId;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        var factoryContactId = await _repository.GetFactoryContactIdByUser(_ApplicationContext.UserId);
                        userApplicantDetails = await _repository.GetSupplierContactUserApplicationDetails(factoryContactId);
                        userApplicantDetails.ContactId = factoryContactId;
                        break;
                    }
            }

            return userApplicantDetails;
        }

        public async Task<UserCredentialsMailTemplateResponse> GetUserCredentialsMailDetail(int userId)
        {
            var response = new UserCredentialsMailTemplateResponse();
            var data = _repository.GetQueryable<ItUserMaster>(x => x.Id == userId);
            if (data == null || !data.Any())
                response.Result = UserCredentialsMailTemplateResult.DataNotFound;

            response.UserCredentialsMailTemplate = await data.Select(x => new UserCredentialsMailTemplate
            {
                UserType = x.Factory != null ? "factory" : "supplier",
                Name = x.Factory.SupplierName ?? x.Supplier.SupplierName,
                ContactName = x.SupplierContact.ContactName ?? x.FactoryContact.ContactName,
            }).FirstOrDefaultAsync();
            response.Result = UserCredentialsMailTemplateResult.Success;

            return response;
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>

        public async Task<ForgotPasswordResponse> ForgotPassword(string username)
        {
            var userId = (await _repository.GetSingleAsync<ItUserMaster>(x => x.Active && x.LoginName == username))?.Id;
            if(userId != null)
            {
                var userInfo = await _userRepository.GetUserInfo(userId.Value);
                
                //set external user email id
                switch (userInfo.UserType)
                {
                    case UserTypeEnum.Customer:
                        {
                            var res = await _cusRepository.GetCustomerContactEmailEntityByUserId(userInfo.Id);
                            userInfo.EmailAddress = res.EmailId ?? "";
                            userInfo.EntityId = res.EntityId.ToString();
                            break;
                        }
                    case UserTypeEnum.Supplier:
                    case UserTypeEnum.Factory:
                        {
                            var res = await _supRepository.GetSupplierContactEmailEntityByUserId(userInfo.Id);
                            userInfo.EmailAddress = res.EmailId ?? "";
                            userInfo.EntityId = res.EntityId.ToString();
                            break;
                        }
                    case UserTypeEnum.InternalUser:
                    case UserTypeEnum.OutSource:
                        {
                            var companyId = await _hrRepository.GetHrStaffEntityId(userInfo.StaffId);
                            userInfo.EntityId = companyId.ToString();
                            break;
                        }
                }

                var headers = _httpContextAccessor?.HttpContext?.Request?.Headers;
                headers["entityId"] = EncryptionDecryption.EncryptStringAES(userInfo.EntityId); 
                return new ForgotPasswordResponse()
                {
                    Id = userInfo.Id,
                    UserName = userInfo.LoginName,
                    FullName = userInfo.FullName,
                    EmailId = userInfo.EmailAddress,
                    Result = ForgotPasswordResult.Success
                };
            }
            else
            {
                return new ForgotPasswordResponse()
                {
                    Result = ForgotPasswordResult.CurrentUserAccountNotFound
                };

            }
        }


        private static string GetRandomPassword(int length)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            var chars = Enumerable.Range(0, length)
                .Select(x => Pool[random.Next(0, Pool.Length)]);
            return new string(chars.ToArray());
        }
    }
}
