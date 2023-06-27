using AutoMapper;
using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.FullBridge;
using DTO.InvoicePreview;
using DTO.References;
using DTO.UserAccount;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static BI.TenantProvider;

namespace BI
{
    public class CustomerManager : ApiCommonData, ICustomerManager
    {
        private readonly ICustomerRepository _repo = null;
        private readonly ICustomerContactRepository _customerContactRepo = null;
        private readonly IUserAccountRepository _userAccountRepo = null;
        private readonly IMapper _mapper = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private static IConfiguration _Configuration = null;
        private readonly IUserRepository _userRepository = null;
        private readonly ICustomerCheckPointManager _ccpManager = null;
        private readonly ILocationRepository _locationRepository = null;
        private readonly CustomerMap _customermap = null;
        private readonly ITenantProvider _filterService = null;
        private readonly ICustomerContactManager _customercontactmanager = null;
        public CustomerManager(ICustomerRepository repo, IMapper mapper, IAPIUserContext applicationContextService, IConfiguration configuration,
            IUserRepository userRepository,
                                                                        ICustomerContactRepository customerContactRepo, IUserAccountRepository userAccountRepo,
            ICustomerCheckPointManager ccpManager, ILocationRepository locationRepository, ITenantProvider filterService, ICustomerContactManager customerContactManager
            )
        {
            _repo = repo;
            _mapper = mapper;
            _ApplicationContext = applicationContextService;
            _customerContactRepo = customerContactRepo;
            _userAccountRepo = userAccountRepo;
            _Configuration = configuration;
            _userRepository = userRepository;
            _ccpManager = ccpManager;
            _locationRepository = locationRepository;
            _customermap = new CustomerMap();
            _filterService = filterService;
            _customercontactmanager = customerContactManager;
        }

        public async Task<IEnumerable<ServiceType>> GetCustomerAuditServiceType(int CustomerId)
        {
            var data = await _repo.GetCustomerAuditServiceType(CustomerId);
            if (data == null || data.Count == 0)
                return null;
            return data.Select(x => _customermap.GetCustomerServiceType(x));
        }

        public async Task<CustomerServiceTypeResponse> GetCustomerInspectionServiceType(int CustomerId)
        {
            CustomerServiceTypeResponse response = new CustomerServiceTypeResponse();
            var data = await _repo.GetCustomerInspectionServiceType(CustomerId);
            var serviceTypeList = data.Select(x => _customermap.GetCustomerServiceType(x));
            if (serviceTypeList != null)
            {
                response.CustomerServiceList = serviceTypeList;
                response.Result = CustomerServiceTypeResult.Success;
            }
            else
            {
                response.Result = CustomerServiceTypeResult.CannotGetServiceTypeList;
            }
            return response;
        }

        public async Task<IEnumerable<CustomerBrand>> GetCustomerBrandsByUserId(int CustomerId, int UserId)
        {
            var data = await _repo.GetCustomerBrandsByUserId(CustomerId);
            if (data == null || data.Count == 0)
                return null;
            return data.Select(x => _customermap.GetCustomerBrand(x));
        }



        public async Task<IEnumerable<CustomerDepartment>> GetCustomerDepartmentByUserId(int CustomerId, int UserId)
        {
            var data = await _repo.GetCustomerDepartmentByUserId(CustomerId);
            if (data == null || data.Count == 0)
                return null;
            return data.Select(x => _customermap.GetCustomerDepartment(x));
        }

        public async Task<IEnumerable<CustomerBuyers>> GetCustomerBuyerByUserId(int CustomerId)
        {
            var data = await _repo.GetCustomerBuyerByUserId(CustomerId);
            if (data == null || data.Count == 0)
                return null;
            return data.Select(x => _customermap.GetCustomerBuyer(x));
        }

        public async Task<IEnumerable<CustomerItem>> GetCustomerItems()
        {
            var data = await _repo.GetCustomersItems();
            if (data == null || data.Count == 0)
                return null;

            return data.Select(x => _customermap.GetCustomerItem(x, ""));
        }
        //fetch customer data based on id
        public async Task<CustomerItem> GetCustomerItemById(int customerId)
        {
            var data = await _repo.GetCustomerItemById(customerId);
            if (data == null)
                return null;

            return _customermap.GetCustomerItem(data, "");
        }

        //GetCustomerSummary

        public async Task<CustomerSummaryResponse> GetCustomerSummary()
        {
            var response = new CustomerSummaryResponse();

            var data = await _repo.GetCustomersItems();
            if (data == null || data.Count == 0)
            {
                response.CustomerList = null;
            }
            else
            {
                response.CustomerList = data.Select(x => _customermap.GetCustomerItem(x, ""));
                response.IsEdit = (_ApplicationContext.CustomerId == 0);

                response.Result = CustomerSummaryResult.Success;
            }
            return response;
        }
        public async Task<CustomerSummaryResponse> GetCustomerbyId(int? id)
        {
            var response = new CustomerSummaryResponse();

            var data = (id != null && id != 0) ? await _repo.GetCustomerbyId(id) : await _repo.GetCustomersItems();
            if (data == null || data.Count == 0)
            {
                response.CustomerList = null;
            }
            else
            {
                response.CustomerList = data.Select(x => _customermap.GetCustomerItem(x, ""));
                response.IsEdit = (_ApplicationContext.CustomerId == 0);

                response.Result = CustomerSummaryResult.Success;
            }
            return response;
        }


        public async Task<CustomerGroupResponse> GetCustomerGroup()
        {
            var response = new CustomerGroupResponse();

            var data = await _repo.GetCustomerGroup();
            if (data == null || data.Count == 0)
            {
                response.CustomerGroup = null;
            }
            else
            {
                response.CustomerGroup = data.Select(x => new CustomerGroup
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }
        public async Task<CustomerSourceResponse> GetLanguage()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetLanguage();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }
        public async Task<CustomerSourceResponse> GetProspectStatus()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetProspectStatus();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }
        public async Task<CustomerSourceResponse> GetMarketSegment()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetMarketSegment();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }
        public async Task<CustomerSourceResponse> GetBusinessType()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetBusinessType();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }
        public async Task<CustomerSourceResponse> GetAddressType()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetAddressType();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }
        public async Task<CustomerSourceResponse> GetInvoiceType()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetInvoiceType();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }

        public async Task<CustomerSourceResponse> GetAccountingLeader()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetAccountingLeader();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }


        public async Task<CustomerSourceResponse> GetActivitiesLevel()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetActivitiesLevel();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }

        public async Task<CustomerSourceResponse> GetRelationshipStatus()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetRelationshipStatus();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }


        public async Task<CustomerSourceResponse> GetBrandPriority()
        {
            var response = new CustomerSourceResponse();

            var data = await _repo.GetBrandPriority();
            if (data == null || data.Count == 0)
            {
                response.CustomerSource = null;
            }
            else
            {
                response.CustomerSource = data.Select(x => _mapper.Map<CustomerSource>(x)).ToArray();

                response.Result = CustomerGroupResult.Success;
            }
            return response;
        }

        public async Task<IEnumerable<Season>> GetCustomerSeason(int CustomerId)
        {
            var data = await _repo.GetCustomerSeason(CustomerId);
            if (data == null || data.Count == 0)
                return null;

            return data.Select(x => _customermap.GetCustomerSeason(x));
        }

        public CustomerSearchResponse GetCustomerData(CustomerSearchRequest request)
        {
            var response = new CustomerSearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            
            var resultData = _repo.GetAllCustomersItems();
            if (request.customerData != null)
            {
                if (request.customerData.GroupId > 0)
                {
                    resultData = resultData.Where(x => x.Group == request.customerData.GroupId);
                }
                if (request.customerData.CustomerId > 0)
                {
                    resultData = resultData.Where(x => x.Id == request.customerData.CustomerId);
                }
                if (request.customerData.IsEAQF.GetValueOrDefault())
                {
                    resultData = resultData.Where(x => x.IsEaqf == request.customerData.IsEAQF.GetValueOrDefault());
                }
                response.TotalCount = resultData.Count();

                if (response.TotalCount == 0)
                {
                    response.Result = CustomerSearchResult.NotFound;
                    return response;
                }
                int skip = (request.Index.Value - 1) * request.pageSize.Value;
                response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                response.Data = resultData.Skip(skip).Take(request.pageSize.Value).Select(x => _customermap.GetCustomerItems(x, GetCustomerGroupName(x.Group).Result)).ToArray();

                response.Result = CustomerSearchResult.Success;

            }

            return response;
        }

        public async Task<EditCustomerResponse> GetEditCustomer(int? id)
        {
            var response = new EditCustomerResponse();

            if (id != null)
            {
                response.CustomerDetails = await GetCustomerDetailsByID(id);
                if (response.CustomerDetails == null)
                    return new EditCustomerResponse { Result = EditCustomerResult.CannotGetCustomer };
            }
            response.Result = EditCustomerResult.Success;
            return response;
        }

        public async Task<IEnumerable<CustomerItem>> GetCustomersByUserId(int UserId)
        {
            var data = await _repo.GetCustomersByUserId(UserId);

            if (data == null || data.Count == 0)
                return null;

            return data.Select(x => _customermap.GetCustomerItem(x, ""));
        }
        private async Task<CustomerDetails> GetCustomerDetailsByID(int? id)
        {
            var customer = await _repo.GetCustomerDetails(id);

            if (customer == null)
                return null;

            return _customermap.GetCustomerDetails(customer);
        }

        public async Task<SaveCustomerResponse> ValidateCustomerData(CustomerDetails request)
        {
            var response = new SaveCustomerResponse();
            if (request.Id == 0 && !string.IsNullOrEmpty(request.Name))
            {
                var customer = await _repo.GetCustomerByName(request.Name);
                if (customer != null && customer.Any())
                {
                    response.Result = SaveCustomerResult.DuplicateCustomerNameFound;
                }
            }
            else if (request.Id != 0 && !string.IsNullOrEmpty(request.Name))
            {
                var customer = await _repo.GetOtherAcountCustomerName(request.Name, request.Id);
                if (customer != null && customer.Any())
                {
                    response.Result = SaveCustomerResult.DuplicateCustomerNameFound;
                }
            }


            if (request.Id == 0 && !string.IsNullOrEmpty(request.GlCode))
            {
                var customerGlCode = await _repo.GetCustomerByGLCode(request.GlCode);
                if (customerGlCode != null && customerGlCode.Any())
                {
                    response.Result = SaveCustomerResult.DuplicateGLCodeFound;
                }
            }
            else if (request.Id != 0 && !string.IsNullOrEmpty(request.GlCode))
            {
                var customerGlCode = await _repo.GetOtherAcountGLCode(request.GlCode, request.Id);
                if (customerGlCode != null && customerGlCode.Any())
                {
                    response.Result = SaveCustomerResult.DuplicateGLCodeFound;
                }
            }

            if (request.Id == 0 && !string.IsNullOrEmpty(request.Email))
            {
                var customerEmail = await _repo.GetCustomerByEmail(request.Email);
                if (customerEmail != null && customerEmail.Any())
                {
                    response.Result = SaveCustomerResult.DuplicateEmailFound;
                }
            }
            else if (request.Id != 0 && !string.IsNullOrEmpty(request.Email))
            {
                var customerEmail = await _repo.GetOtherAccountEmail(request.Email, request.Id);

                if (customerEmail != null && customerEmail.Any())
                {
                    response.Result = SaveCustomerResult.DuplicateEmailFound;
                }
            }

            return response;
        }

        public async Task<SaveCustomerResponse> Save(CustomerDetails request)
        {
            //added custom validations if validation fails return response
            var response = await ValidateCustomerData(request);
            if (response.Result != 0)
                return response;

            if (request.Id > 0 && request.SisterCompanyIds != null && request.SisterCompanyIds.Any() && request.SisterCompanyIds.Any(x => x == request.Id))
            {
                return new SaveCustomerResponse()
                {
                    Result = SaveCustomerResult.SameCompanyAsSisterComapny
                };
            }

            var entityID = _filterService.GetCompanyId();
            var userID = _ApplicationContext.UserId;
            if (_filterService.GetCompanyId() == 0)
                entityID = Convert.ToInt16(_Configuration["ExternalAccessorEntityID"]);
            if (_ApplicationContext.UserId == 0)
                userID = Convert.ToInt16(_Configuration["ExternalAccessorUserId"]);

            var isItTeamRole = _ApplicationContext.RoleList?.Any(x => x == (int)RoleEnum.IT_Team);
            if (request.Id == 0)
            {
                CuCustomer entity = _customermap.MapCustomerEntity(request, entityID, userID, isItTeamRole.GetValueOrDefault());

                if (entity == null)
                    return new SaveCustomerResponse { Result = SaveCustomerResult.CustomerIsNotFound };

                // Add customer address list
                if (request.CustomerAddresses != null)
                {
                    foreach (var item in request.CustomerAddresses)
                    {
                        var address = _customermap.MapCustomerAddressEntity(item, userID);

                        entity.CuAddresses.Add(address);

                        _repo.AddEntity(address);
                    }
                }

                response.Id = await _repo.AddCustomer(entity);

                if (response.Id == 0)
                    return new SaveCustomerResponse { Result = SaveCustomerResult.CustomerIsNotSaved };

                if (response.Id > 0)
                {
                    response.Result = SaveCustomerResult.Success;
                }

                return response;
            }
            else
            {

                var entity = await _repo.GetCustomerDetails(request.Id);

                if (entity == null)
                    return new SaveCustomerResponse { Result = SaveCustomerResult.CustomerIsNotFound };

                _customermap.UpdateCustomerEnity(entity, request, userID);

                //deleted cu entity 
                var deleteCustomerEntityList = entity.CuEntities.Where(x => x.Active == true && !request.CustomerEntityIds.Contains(x.EntityId)).ToList();

                //get the db customer entity id
                var dbCustomerEntityIdList = entity.CuEntities.Where(x => x.Active == true).Select(y => y.EntityId).ToList();
                //fetch new entityid
                var newCustomerEntityList = request.CustomerEntityIds.Where(x => !dbCustomerEntityIdList.Contains(x)).ToList();

                //get active customer contacts
                var customerContacts = entity.CuContacts.Where(x => x.Active == true);
                CustomerContactEntityData model = null;
                //based on deleted customer entity , new customer entity and customer contacts fetch the customer contact entity data
                if ((deleteCustomerEntityList.Any() || newCustomerEntityList.Any()) && customerContacts.Any())
                {
                    model = await GetCustomerContactEntityData(customerContacts, deleteCustomerEntityList.Any());
                }

                //if user remove any entity 
                if (deleteCustomerEntityList.Any())
                {
                    var deleteCustomerEntityResult = DeleteCustomerEntity(deleteCustomerEntityList, model);
                    if (deleteCustomerEntityResult != null)
                        return deleteCustomerEntityResult;
                }

                if (newCustomerEntityList.Any())
                {
                    AddCustomerEntityList(newCustomerEntityList, request, model);
                }

                if (isItTeamRole.GetValueOrDefault())
                {
                    await UpdateCustomerSisterEntity(entity, request, userID, entityID);
                }



                // Update customer address only if it is calling from the API.
                if (request.isCallFrom != null && request.isCallFrom == (int)CustomerModuleEnum.API)
                {
                    // Customer AddressList
                    var addressIds = request.CustomerAddresses.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
                    var lstAddressToremove = new List<CuAddress>();
                    var CuAddresses = entity.CuAddresses.Where(x => !addressIds.Contains(x.Id));
                    foreach (var item in CuAddresses)
                    {
                        item.Active = false;
                        lstAddressToremove.Add(item);
                    }

                    _repo.EditEntities(lstAddressToremove);

                    if (request.CustomerAddresses != null)
                    {
                        foreach (var item in request.CustomerAddresses.Where(x => x.Id <= 0))
                            entity.CuAddresses.Add(new CuAddress
                            {
                                Id = item.Id,
                                Address = item.Address?.Trim(),
                                BoxPost = item.BoxPost?.Trim(),
                                ZipCode = item.ZipCode?.Trim(),
                                AddressType = item.AddressType,
                                CountryId = item.CountryId,
                                CityId = item.CityId,
                                CreatedBy = userID,
                                CreatedOn = DateTime.Now,
                                Active = true
                            });

                        var lstAddressToEdit = new List<CuAddress>();
                        foreach (var item in request.CustomerAddresses.Where(x => x.Id > 0))
                        {
                            var address = entity.CuAddresses.FirstOrDefault(x => x.Id == item.Id);

                            if (address != null)
                            {

                                address.Id = item.Id;
                                address.Address = item.Address?.Trim();
                                address.BoxPost = item.BoxPost?.Trim();
                                address.ZipCode = item.ZipCode?.Trim();
                                address.AddressType = item.AddressType;
                                address.CountryId = item.CountryId;
                                address.CityId = item.CityId;
                                address.UpdatedBy = userID;
                                address.UpdatedOn = DateTime.Now;
                                address.Active = true;
                                lstAddressToEdit.Add(address);

                            }
                            else
                                return new SaveCustomerResponse { Result = SaveCustomerResult.CustomerAddressNotFound };

                        }

                        if (lstAddressToEdit.Count > 0)
                            _repo.EditEntities(lstAddressToEdit);
                    }
                }

                UpdateCustomerServices(request, entity);
                UpdateKam(request, entity);
                UpdateSalesIncharge(request, entity);
                UpdateBrandPriority(request, entity);

                await _repo.EditCustomer(entity);
                response.Id = entity.Id;

                if (response.Id > 0)
                {
                    response.Result = SaveCustomerResult.Success;
                }

            }

            return response;
        }

        /// <summary>
        /// update customer sister entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="entityId"></param>
        private async Task UpdateCustomerSisterEntity(CuCustomer entity, CustomerDetails request, int userId, int entityId)
        {
            if (request.SisterCompanyIds == null)
                request.SisterCompanyIds = new List<int>();

            //deleted cu sister company
            var deleteSisterCompanyList = entity.CuSisterCompanyCustomers.Where(x => x.Active == true && !request.SisterCompanyIds.Contains(x.SisterCompanyId)).ToList();

            //get the db customer entity id
            var dbSisterCompanyIdList = entity.CuSisterCompanyCustomers.Where(x => x.Active == true).Select(y => y.SisterCompanyId);
            //fetch new entityid
            var newSisterCompanyList = request.SisterCompanyIds.Where(x => !dbSisterCompanyIdList.Contains(x)).ToList();

            if (deleteSisterCompanyList.Any())
            {
                var contactIds = entity.CuContacts.Select(y => y.Id).ToList();
                var customerContactSisterCompaines = await _repo.GetSisterCompaniesContactByCustomerContactIds(contactIds);
                var deleteSisterCompanyIds = deleteSisterCompanyList.Select(y => y.SisterCompanyId).ToList();
                var deleteCustomerContactSisterCompanies = customerContactSisterCompaines.Where(x => deleteSisterCompanyIds.Contains(x.SisterCompanyId)).ToList();

                if (deleteCustomerContactSisterCompanies.Any())
                {
                    //delete the customer contact sister company
                    deleteCustomerContactSisterCompanies.ForEach(x =>
                    {
                        x.Active = false;
                        x.DeletedOn = DateTime.Now;
                        x.DeletedBy = userId;
                    });

                    _repo.EditEntities(deleteCustomerContactSisterCompanies);
                }

                //delete the customer sister company
                deleteSisterCompanyList.ForEach(x =>
                {
                    x.Active = false;
                    x.DeletedOn = DateTime.Now;
                    x.DeletedBy = userId;
                });

                _repo.EditEntities(deleteSisterCompanyList);
            }

            //add new sister company
            if (newSisterCompanyList.Any())
            {
                foreach (var customerId in newSisterCompanyList)
                {
                    var cuSisterCompany = new CuSisterCompany()
                    {
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        SisterCompanyId = customerId,
                        EntityId = entityId
                    };

                    entity.CuSisterCompanyCustomers.Add(cuSisterCompany);
                    _repo.AddEntity(cuSisterCompany);
                }
            }
        }

        private void UpdateCustomerServices(CustomerDetails request, CuCustomer entity)
        {
            var serviceIds = request.ApiServiceIds.Select(x => x).ToArray();
            var lstServiceToremove = new List<CuApiService>();
            var services = entity.CuApiServices.Where(x => !serviceIds.Contains(x.ServiceId) && x.Active.HasValue && x.Active.Value);
            var existingServices = entity.CuApiServices.Where(x => serviceIds.Contains(x.ServiceId) && x.Active.HasValue && x.Active.Value);

            // Remove if data does not exist in the db.

            foreach (var item in services)
            {
                lstServiceToremove.Add(item);
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _ApplicationContext.UserId;
                item.Active = false;
            }

            _repo.EditEntities(lstServiceToremove);

            if (request.ApiServiceIds != null)
            {
                // Add if data is new it means id = 0;
                foreach (var id in serviceIds)
                {
                    if (!existingServices.Any() || !existingServices.Any(x => x.ServiceId == id))
                    {
                        entity.CuApiServices.Add(new CuApiService()
                        {
                            ServiceId = id,
                            Active = true,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }


        private void UpdateKam(CustomerDetails request, CuCustomer entity)
        {
            var KamIds = request?.Kam?.Select(x => x).ToArray();
            var lstKamToremove = new List<CuKam>();
            var Kams = entity.CuKams.Where(x => !KamIds.Contains(x.KamId) && x.Active.HasValue && x.Active.Value == 1);
            var existingKams = entity.CuKams.Where(x => KamIds.Contains(x.KamId) && x.Active.HasValue && x.Active.Value == 1);
            // Remove if data does not exist in the db.
            foreach (var item in Kams)
            {
                lstKamToremove.Add(item);
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _ApplicationContext.UserId;
                item.Active = 0;
            }

            _repo.EditEntities(lstKamToremove);

            if (request.Kam != null)
            {
                // Add if data is new it means id = 0;
                foreach (var id in KamIds)
                {
                    if (!existingKams.Any() || !existingKams.Any(x => x.KamId == id))
                    {
                        entity.CuKams.Add(new CuKam()
                        {
                            KamId = id,
                            Active = 1,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }

        private void UpdateSalesIncharge(CustomerDetails request, CuCustomer entity)
        {
            var SalesInchargeIds = request?.SalesIncharge?.Select(x => x).ToArray();
            var lstSalesInchargeToremove = new List<CuSalesIncharge>();
            var SalesIncharges = entity.CuSalesIncharges.Where(x => !SalesInchargeIds.Contains(x.StaffId) && x.Active.HasValue && x.Active.Value == 1);
            var existingSalesIncharges = entity.CuSalesIncharges.Where(x => SalesInchargeIds.Contains(x.StaffId) && x.Active.HasValue && x.Active.Value == 1);
            // Remove if data does not exist in the db.
            foreach (var item in SalesIncharges)
            {
                lstSalesInchargeToremove.Add(item);
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _ApplicationContext.UserId;
                item.Active = 0;
            }

            _repo.EditEntities(lstSalesInchargeToremove);

            if (request?.SalesIncharge != null)
            {
                // Add if data is new it means id = 0;
                foreach (var id in SalesInchargeIds)
                {
                    if (!existingSalesIncharges.Any() || !existingSalesIncharges.Any(x => x.StaffId == id))
                    {
                        entity.CuSalesIncharges.Add(new CuSalesIncharge()
                        {
                            StaffId = id,
                            Active = 1,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }

        private void UpdateBrandPriority(CustomerDetails request, CuCustomer entity)
        {
            var brandPriorityIds = request?.BrandPriority?.Select(x => x).ToArray();
            var lstBrandPriorityToremove = new List<CuBrandpriority>();
            var brandPrioritys = entity.CuBrandpriorities.Where(x => !brandPriorityIds.Contains(x.BrandpriorityId) && x.Active.HasValue && x.Active.Value == 1);
            var existingbrandPrioritys = entity.CuBrandpriorities.Where(x => brandPriorityIds.Contains(x.BrandpriorityId) && x.Active.HasValue && x.Active.Value == 1);
            // Remove if data does not exist in the db.
            foreach (var item in brandPrioritys)
            {
                lstBrandPriorityToremove.Add(item);
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _ApplicationContext.UserId;
                item.Active = 0;
            }

            _repo.EditEntities(lstBrandPriorityToremove);

            if (request?.BrandPriority != null)
            {
                // Add if data is new it means id = 0;
                foreach (var id in brandPriorityIds)
                {
                    if (!existingbrandPrioritys.Any() || !existingbrandPrioritys.Any(x => x.BrandpriorityId == id))
                    {
                        entity.CuBrandpriorities.Add(new CuBrandpriority()
                        {
                            BrandpriorityId = id,
                            Active = 1,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }

        /// <summary>
        /// remove customer entity from db
        /// </summary>
        /// <param name="deleteCustomerEntityList"></param>
        /// <param name="customerContactEntityData"></param>
        /// <returns></returns>
        private SaveCustomerResponse DeleteCustomerEntity(List<CuEntity> deleteCustomerEntityList, CustomerContactEntityData customerContactEntityData)
        {
            //select delete customer entity id
            var deleteCustomerEntityIdList = deleteCustomerEntityList.Select(x => x.EntityId);
            //if customer have contacts
            if (customerContactEntityData != null)
            {
                //check the deleted customer entity is not selected as primary entity
                if (customerContactEntityData.CustomerContacts != null && customerContactEntityData.CustomerContacts.Any(x => x.PrimaryEntity.HasValue && deleteCustomerEntityIdList.Contains(x.PrimaryEntity.Value)))
                    return new SaveCustomerResponse() { Result = SaveCustomerResult.CustomerEntiyNotRemoved };

                if (customerContactEntityData.CustomerContactEntityMapList != null && customerContactEntityData.CustomerContactEntityMapList.Any())
                {
                    //remove from the CU_Contact_Entity_Map table
                    var deleteCustomerContactEntityMapList = customerContactEntityData.CustomerContactEntityMapList.Where(x => deleteCustomerEntityIdList.Contains(x.EntityId));
                    if (deleteCustomerContactEntityMapList.Any())
                        _repo.RemoveEntities(deleteCustomerContactEntityMapList);
                }


                if (customerContactEntityData.CustomerContactUserRoleList != null && customerContactEntityData.CustomerContactUserRoleList.Any())
                {
                    //remove from the IT_UserRole table based on customer contact and entity id
                    var deleteCustomerContactUserRoleList = customerContactEntityData.CustomerContactUserRoleList.Where(x => deleteCustomerEntityIdList.Contains(x.EntityId));
                    if (deleteCustomerContactUserRoleList.Any())
                        _repo.RemoveEntities(deleteCustomerContactUserRoleList);
                }

                if (customerContactEntityData.CustomerContactEntityServiceMapList != null && customerContactEntityData.CustomerContactEntityServiceMapList.Any())
                {
                    //remove from the CU_Contact_Entity_Service_Map table based on the entity id
                    var deleteCustomerContactEntityServiceMapList = customerContactEntityData.CustomerContactEntityServiceMapList.Where(x => deleteCustomerEntityIdList.Contains(x.EntityId.GetValueOrDefault()));
                    if (deleteCustomerContactEntityServiceMapList.Any())
                        _repo.RemoveEntities(deleteCustomerContactEntityServiceMapList);
                }
            }

            //remove from the CU_Entity table

            deleteCustomerEntityList.ForEach(x =>
            {
                x.Active = false;
                x.DeletedBy = _ApplicationContext.UserId;
                x.DeletedOn = DateTime.Now;
            });
            _repo.EditEntities(deleteCustomerEntityList);
            return null;
        }

        //add new customer entities
        private void AddCustomerEntityList(List<int> newCustomerEntityList, CustomerDetails request, CustomerContactEntityData customerContactEntityData)
        {
            List<CuContact> mapEntityCustomerContactList = null;
            if (newCustomerEntityList.Any())
            {
                // if new entity to map with existing customer contact
                if (customerContactEntityData != null && customerContactEntityData.CustomerContacts != null && customerContactEntityData.CustomerContacts.Any() && request.MapCustomerContactEntityIds != null && request.MapCustomerContactEntityIds.Any())
                {
                    // fetch the customer contact
                    mapEntityCustomerContactList = customerContactEntityData.CustomerContacts.Where(x => request.MapCustomerContactEntityIds.Contains(x.Id)).ToList();
                }

                //loop for new entity
                newCustomerEntityList.ForEach(entityId =>
                {
                    //add new CuEntity table
                    var cuEntity = new CuEntity()
                    {
                        CustomerId = request.Id,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = entityId
                    };

                    _repo.AddEntity(cuEntity);


                    if (customerContactEntityData != null && mapEntityCustomerContactList != null && mapEntityCustomerContactList.Any())
                    {
                        //selected customer contact loop
                        mapEntityCustomerContactList.ForEach(customerContact =>
                        {
                            if (!customerContactEntityData.CustomerContactEntityMapList.Any(a => a.ContactId == customerContact.Id && a.EntityId == entityId))
                            {

                                //add the customer contact entity map
                                var customerContactEntityMap = new CuContactEntityMap()
                                {
                                    EntityId = entityId,
                                    ContactId = customerContact.Id
                                };
                                _repo.AddEntity(customerContactEntityMap);

                            }


                            if (customerContactEntityData.CustomerContactEntityServiceMapList != null && customerContactEntityData.CustomerContactEntityServiceMapList.Any())
                            {
                                //get the cutomer contact primary service
                                var customerContactPrimaryEntityServiceMaps = customerContactEntityData.CustomerContactEntityServiceMapList.Where(z => z.ContactId == customerContact.Id && z.EntityId == customerContact.PrimaryEntity).ToList();

                                //primarty service loop
                                customerContactPrimaryEntityServiceMaps.ForEach(z =>
                                {
                                    //add new cu contact entity service map
                                    var cuContactEntityServiceMap = new CuContactEntityServiceMap()
                                    {
                                        EntityId = entityId,
                                        ServiceId = z.ServiceId,
                                    };
                                    _repo.AddEntity(cuContactEntityServiceMap);
                                    customerContact.CuContactEntityServiceMaps.Add(cuContactEntityServiceMap);
                                });
                            }

                            if (customerContactEntityData.CustomerContactUserList != null && customerContactEntityData.CustomerContactUserList.Any())
                            {
                                //get the customer contact user based on customer contact id
                                var customerContactUserIdList = customerContactEntityData.CustomerContactUserList.Where(x => x.CustomerContactId == customerContact.Id).Select(z => z.Id);
                                if (customerContactUserIdList != null && customerContactUserIdList.Any())
                                {
                                    var customerContactUserRoles = customerContactEntityData.CustomerContactUserRoleList.Where(z => customerContactUserIdList.Contains(z.UserId));
                                    var newlyEntityIdUserRoles = customerContactUserRoles.Where(x => x.EntityId == entityId);
                                    if (newlyEntityIdUserRoles.Any())
                                    {
                                        var newlyEntityIdRoles = newlyEntityIdUserRoles.Select(z => z.RoleId);
                                        customerContactUserRoles = customerContactUserRoles.Where(y => !newlyEntityIdRoles.Contains(y.RoleId));
                                    }
                                    //get the customer contact user role 
                                    var customerContactUserRoleList = customerContactUserRoles.Where(y => y.EntityId == customerContact.PrimaryEntity).Select(z => new { z.UserId, z.RoleId }).Distinct();
                                    //map user role with new entity
                                    //Table:- IT_UserRole
                                    customerContactUserRoleList.ToList().ForEach(a =>
                                    {
                                        var userRole = new ItUserRole()
                                        {
                                            EntityId = entityId,
                                            RoleId = a.RoleId,
                                            UserId = a.UserId
                                        };
                                        _repo.AddEntity(userRole);
                                    });

                                }
                            }


                        });
                    }

                });
            }
        }

        /// <summary>
        /// get the customer contact entity data
        /// </summary>
        /// <param name="cuContacts"></param>
        /// <param name="isForDeleteCustomerEntity"></param>
        /// <returns></returns>
        private async Task<CustomerContactEntityData> GetCustomerContactEntityData(IEnumerable<CuContact> cuContacts, bool isForDeleteCustomerEntity)
        {
            if (!cuContacts.Any())
                return null;

            var model = new CustomerContactEntityData();
            var customerContactIds = cuContacts.Select(x => x.Id);
            model.CustomerContactEntityServiceMapList = await _repo.GetCustomerContactEntityServiceMapByCustomerId(customerContactIds);
            var users = await _userAccountRepo.GetUserByCustomerContactIds(customerContactIds);
            model.CustomerContactUserList = users;
            if (users.Any())
                model.CustomerContactUserRoleList = await _userRepository.GetUserRolesByUserIdsIgnoreQueryFilter(users.Select(y => y.Id));
            model.CustomerContacts = cuContacts;

            model.CustomerContactEntityMapList = await _repo.GetCustomerContactEntityMapByCustomerId(customerContactIds);
            return model;
        }
        public async Task<CustomerDeleteResponse> DeleteCustomer(int id)
        {
            var customer = await _repo.GetCustomerDetails(id);

            if (customer == null)
                return new CustomerDeleteResponse { Id = id, Result = CustomerDeleteResult.NotFound };

            await _repo.RemoveCustomer(id);

            return new CustomerDeleteResponse { Id = id, Result = CustomerDeleteResult.Success };

        }

        public async Task<CustomerSummaryResponse> GetCustomersByUserType()
        {
            var response = new CustomerSummaryResponse();
            switch (_ApplicationContext.UserType)
            {
                case Entities.Enums.UserTypeEnum.InternalUser:
                    {
                        response.CustomerList = await GetCustomersByUserId(_ApplicationContext.StaffId);
                        if (response.CustomerList == null || response.CustomerList.Count() == 0)
                            response.CustomerList = await GetCustomerItems();
                        response.Result = CustomerSummaryResult.Success;
                        break;
                    }
                case Entities.Enums.UserTypeEnum.Customer:
                    {
                        var cuslist = await GetCustomerItems();
                        if (cuslist == null || cuslist.Count() == 0)
                            return new CustomerSummaryResponse() { Result = CustomerSummaryResult.CannotGetCustomerList };
                        response.CustomerList = cuslist.Where(x => x.Id == _ApplicationContext.CustomerId).ToList();
                        response.Result = CustomerSummaryResult.Success;
                        break;
                    }
                case Entities.Enums.UserTypeEnum.Supplier:
                    {
                        response.CustomerList = await GetCustomersBySupplierId(_ApplicationContext.SupplierId);
                        if (response.CustomerList == null || response.CustomerList.Count() == 0)
                            return new CustomerSummaryResponse() { Result = CustomerSummaryResult.CannotGetCustomerList };
                        response.Result = CustomerSummaryResult.Success;
                        break;
                    }
                case Entities.Enums.UserTypeEnum.Factory:
                    {
                        response.CustomerList = await GetCustomersBySupplierId(_ApplicationContext.FactoryId);
                        if (response.CustomerList == null || response.CustomerList.Count() == 0)
                            return new CustomerSummaryResponse() { Result = CustomerSummaryResult.CannotGetCustomerList };
                        response.Result = CustomerSummaryResult.Success;
                        break;
                    }

            }
            return response;
        }

        public async Task<IEnumerable<CustomerItem>> GetCustomersBySupplierId(int SupplierId)
        {
            var data = await _repo.GetCustomersBySupplierId(SupplierId);
            if (data == null || data.Count() == 0)
                return null;
            return data.Select(x => _customermap.GetCustomerItem(x, ""));
        }

        private async Task<string> GetCustomerGroupName(int? GroupID)
        {
            var customerGroup = await _repo.GetCustomerGroup();
            var GroupName = customerGroup?.Where(x => x.Id == GroupID).Select(x => x.Name).SingleOrDefault();
            return GroupName;
        }


        public List<CustomerAddressData> GetCustomerAddress(int CustomerId)
        {
            var CustomerAddressList = _customerContactRepo.GetCustomerAddressByCustomerID(CustomerId);
            var CuAddressList = CustomerAddressList.Select(x => new CustomerAddressData { Address = x.Address, Id = x.Id }).ToList();
            return CuAddressList;
        }

        public async Task<CustomerServiceTypeResponse> GetReInspectionServiceType(int CustomerId)
        {
            CustomerServiceTypeResponse response = new CustomerServiceTypeResponse();
            var data = await _repo.GetReInspectionServiceType(CustomerId);
            var serviceTypeList = data.Select(x => _customermap.GetCustomerServiceType(x));
            if (serviceTypeList != null)
            {
                response.CustomerServiceList = serviceTypeList;
                response.Result = CustomerServiceTypeResult.Success;
            }
            else
            {
                response.Result = CustomerServiceTypeResult.CannotGetServiceTypeList;
            }
            return response;
        }
        // Get Customer By User Type and check the checkpoint too 
        public async Task<CustomerSummaryResponse> GetCustomerByCheckPointUsertType()
        {
            var response = new CustomerSummaryResponse();
            int[] checkpointIds = new[] { (int)CheckPointTypeEnum.ICRequired };
            int serviceId = (int)Entities.Enums.Service.InspectionId;

            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.InternalUser:
                    {
                        var customerList = await GetCustomersByUserId(_ApplicationContext.StaffId);

                        if (customerList == null || customerList.Count() == 0)
                            customerList = await GetCustomerItems();

                        //checkpoint filter for customer
                        response.CustomerList = await GetCustomerByCheckpoint(customerList, serviceId, checkpointIds);

                        response.Result = CustomerSummaryResult.Success;
                        break;
                    }
                case UserTypeEnum.Customer:
                    {
                        var customerData = await GetCustomerItemById(_ApplicationContext.CustomerId);

                        if (customerData == null)
                            return new CustomerSummaryResponse() { Result = CustomerSummaryResult.CannotGetCustomerList };

                        var customerList = new List<CustomerItem>();
                        customerList.Add(customerData);

                        //checkpoint filter for customer
                        response.CustomerList = await GetCustomerByCheckpoint(customerList, serviceId, checkpointIds);

                        response.Result = CustomerSummaryResult.Success;
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        var customerList = await GetCustomersBySupplierId(_ApplicationContext.SupplierId);

                        //checkpoint filter for customer
                        response.CustomerList = await GetCustomerByCheckpoint(customerList, serviceId, checkpointIds);

                        if (response.CustomerList == null || response.CustomerList.Count() == 0)
                            return new CustomerSummaryResponse() { Result = CustomerSummaryResult.CannotGetCustomerList };
                        response.Result = CustomerSummaryResult.Success;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        var customerList = await GetCustomersBySupplierId(_ApplicationContext.FactoryId);

                        //checkpoint filter for customer
                        response.CustomerList = await GetCustomerByCheckpoint(customerList, serviceId, checkpointIds);

                        if (response.CustomerList == null || response.CustomerList.Count() == 0)
                            return new CustomerSummaryResponse() { Result = CustomerSummaryResult.CannotGetCustomerList };
                        response.Result = CustomerSummaryResult.Success;
                        break;
                    }
            }
            return response;
        }
        /// <summary>
        /// check checkpoint list with customer id list who has IC required check point 
        /// </summary>
        /// <param name="customerList"></param>
        /// <returns>return customer list who has ic required check point</returns>
        private async Task<IEnumerable<CustomerItem>> GetCustomerByCheckpoint(IEnumerable<CustomerItem> customerList, int serviceId, int[] checkpointIds)
        {
            var customerIdList = customerList.Select(x => x.Id).ToList();

            //get check point list based on customerid list, serviceid, checkpoint id list
            var checkpointList = await _ccpManager.GetCheckPointList(customerIdList, serviceId, checkpointIds);

            //return customer list who has the checkpoints
            return customerList.Where(x => checkpointList.Select(y => y.CustomerId).Contains(x.Id));
        }

        public async Task<LinkErrorResponse> ValidateSaveCustomerRequest(SaveCustomerCrmRequest request, int requestType)
        {
            DateTime startDate;

            if (request.ZohoCustomerId <= 0)
            {
                return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest,
                    BadRequest, new List<string>() { "ZohoCustomerId cannot be zero or empty" });
            }
            if (requestType == (int)ZohoCustomerRequestEnum.Save && !DateTime.TryParseExact(request.StartDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                        DateTimeStyles.None, out startDate))
            {

                return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest, BadRequest,
                    new List<string>() { "StartDate is not a valid datetime format.Please give the mentioned date format(dd/MM/yyyy)" });
            }

            if (request.CustomerAddresses == null || !request.CustomerAddresses.Any())
            {

                return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest, BadRequest,
                    new List<string>() { "Customer Address length cannot be zero" });
            }

            if (request.ZohoCustomerId > 0)
            {
                var zohoCustomerId = request.ZohoCustomerId;
                var customer = await _repo.GetCustomerByZohoId(zohoCustomerId);
                if (requestType == (int)ZohoCustomerRequestEnum.Save)
                {
                    if (customer != null)
                    {

                        return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest,
                            BadRequest, new List<string>() { "Zoho CustomerId Already Exists" });
                    }
                }
                else if (requestType == (int)ZohoCustomerRequestEnum.Update)
                {
                    if (customer == null)
                    {
                        return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest,
                            BadRequest, new List<string>() { "Zoho CustomerId Not Exists" });
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Save Zoho Customer Data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<object> SaveZohoCRMCustomer(SaveCustomerCrmRequest request)
        {
            //validate the save customer request
            var validationErrorResponse = await ValidateSaveCustomerRequest(request, (int)ZohoCustomerRequestEnum.Save);
            if (validationErrorResponse != null)
                return validationErrorResponse;

            // Map Customer Entity
            var entityID = CompanyList.FirstOrDefault(x => x.Value == request.Company).Key;

            //Map data and build customer entity
            var customerEntity = _customermap.MapZohoCustomerEntity(request, null, entityID);

            //Map customer Address with customer entity
            if (request.CustomerAddresses != null && request.CustomerAddresses.Any())
            {
                customerEntity.CustomerAddresses = await MapZohoCustomerAddress(request.CustomerAddresses);
            }

            //Map sales Country with customer entity
            if (request.SalesCountry != null && request.SalesCountry.Any())
            {
                customerEntity.SalesCountry = new List<int>();
                foreach (var countryName in request.SalesCountry)
                {
                    var country = await _locationRepository.GetCountries(countryName);
                    if (country != null)
                        customerEntity.SalesCountry.Add(country.Id);
                }
            }

            // Map Customer Entity

            if (string.IsNullOrEmpty(request.Company))
            {
                return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                     BadRequest, new List<string>() { "Company is not Valid" });
            }



            if (entityID > 0)
                customerEntity.CustomerEntityIds = new List<int>() { entityID };
            else
                return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                       BadRequest, new List<string>() { "Company is not Valid" });

            // save customer
            var response = await Save(customerEntity);

            if (response != null)
            {
                if (response.Result == SaveCustomerResult.DuplicateCustomerNameFound)
                {

                    return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                             BadRequest, new List<string>() { "Duplicate Customer Name Found" });
                }
                if (response.Result == SaveCustomerResult.DuplicateGLCodeFound)
                {
                    return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                     BadRequest, new List<string>() { "Duplicate GLCode Found" });
                }
                if (response.Result == SaveCustomerResult.DuplicateEmailFound)
                {
                    return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                    BadRequest, new List<string>() { "Duplicate Email Id Found" });
                }
            }
            return BuildCommonLinkSuccessResponse(HttpStatusCode.Created,
                    Success, response.Id);
        }

        /// <summary>
        /// Map customer address as per zoho data
        /// </summary>
        /// <param name="customerAddresses"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerAddress>> MapZohoCustomerAddress(IEnumerable<CrmCustomerAddress> customerAddresses)
        {
            var customerAddressList = new List<CustomerAddress>();

            int rowCount = 1;
            foreach (var addressInput in customerAddresses)
            {
                var address = new CustomerAddress();

                //Map address type as headoffice if it is first address
                if (rowCount == 1)
                    address.AddressType = (int)ZohoCustomerAddressTypeEnum.HeadOffice;
                else
                    address.AddressType = (int)ZohoCustomerAddressTypeEnum.Accounting;

                address.CountryId = null;
                //Map CountryId from zoho country name
                if (!string.IsNullOrEmpty(addressInput.Country))
                {
                    var country = await _locationRepository.GetCountries(addressInput.Country);
                    if (country != null)
                        address.CountryId = country.Id;
                }


                address.CityId = null;
                //Map CityId from zoho city name
                if (!string.IsNullOrEmpty(addressInput.City))
                {
                    var city = await _locationRepository.GetCities(addressInput.City);
                    if (city != null)
                        address.CityId = city.Id;
                }

                address.Address = addressInput.Address;
                address.BoxPost = addressInput.BoxPost;
                address.ZipCode = addressInput.ZipCode;

                customerAddressList.Add(address);
                rowCount++;
            }
            return customerAddressList;
        }

        /// <summary>
        /// Map Zoho CRM Data to customerdetails
        /// </summary>
        /// <param name="request"></param>
        /// <param name="zohoCustomerId"></param>
        /// <returns></returns>
        public async Task<object> UpdateZohoCustomer(SaveCustomerCrmRequest request, long zohoCustomerId)
        {
            request.ZohoCustomerId = zohoCustomerId;
            //validate the save customer request
            var errorResponse = await ValidateSaveCustomerRequest(request, (int)ZohoCustomerRequestEnum.Update);
            if (errorResponse != null)
                return errorResponse;

            var customerDetails = new CustomerDetails();
            var customer = await _repo.GetCustomerByZohoId(request.ZohoCustomerId);
            // Map Customer Entity
            var entityID = CompanyList.FirstOrDefault(x => x.Value == request.Company).Key;

            if (customer != null)
            {
                customerDetails.ZohoCustomerId = zohoCustomerId;
                customerDetails = _customermap.MapZohoCustomerEntity(request, customer.Id, entityID, customer.GLCode, customer.StartDate);
                customerDetails.isCallFrom = (int)CustomerModuleEnum.Zoho;
            }

            DateTime startDate;
            if (DateTime.TryParseExact(request.StartDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                        DateTimeStyles.None, out startDate))
            {
                customerDetails.StartDate = new DateObject() { Year = startDate.Year, Month = startDate.Month, Day = startDate.Day };
            }

            if (request.SalesCountry != null && request.SalesCountry.Any())
            {
                customerDetails.SalesCountry = new List<int>();
                foreach (var countryName in request.SalesCountry)
                {
                    var country = await _locationRepository.GetCountries(countryName);
                    if (country != null)
                        customerDetails.SalesCountry.Add(country.Id);
                }
            }

            if (string.IsNullOrEmpty(request.Company))
            {
                return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                     BadRequest, new List<string>() { "Company is not Valid" });
            }

            if (entityID > 0)
                customerDetails.CustomerEntityIds = new List<int>() { entityID };
            else
                return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                       BadRequest, new List<string>() { "Company is not Valid" });

            var response = await Save(customerDetails);

            if (response != null)
            {
                if (response.Result == SaveCustomerResult.DuplicateCustomerNameFound)
                {

                    return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                             BadRequest, new List<string>() { "Duplicate Customer Name Found" });
                }
                if (response.Result == SaveCustomerResult.DuplicateGLCodeFound)
                {
                    return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                     BadRequest, new List<string>() { "Duplicate GLCode Found" });
                }
                if (response.Result == SaveCustomerResult.DuplicateEmailFound)
                {
                    return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                    BadRequest, new List<string>() { "Duplicate Email Id Found" });
                }
            }
            return BuildCommonLinkSuccessResponse(HttpStatusCode.OK,
                    Success, response.Id);
        }
        /// <summary>
        /// Get the customer details by zoho id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> GetCustomerByZohoId(long id)
        {
            //get the customer by zoho id
            var customer = await _repo.GetCustomerDetailsByZohoID(id);
            if (customer != null)
            {
                var CustomerDetails = _customermap.GetCustomerDetails(customer);
                if (CustomerDetails == null)
                {
                    return new LinkGetSuccessResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Not Found" };
                }
                else
                {
                    return new LinkGetSuccessResponse() { StatusCode = HttpStatusCode.OK, Data = CustomerDetails, Message = "Success" };
                }
            }
            return new LinkGetSuccessResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Not Found" };
        }
        /// <summary>
        /// Get the customer details by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ZohoCustomerDetailResponse> GetCustomerDetailsByName(string name)
        {
            ZohoCustomerDetailResponse response = new ZohoCustomerDetailResponse();
            //get the customer by name
            var customerDetails = await _repo.GetCustomerDetailsByName(name);

            if (customerDetails == null)
                return new ZohoCustomerDetailResponse { Result = CustomerDetailResult.CannotGetCustomer };
            else
            {
                response.CustomerDetails = customerDetails;
                response.Result = CustomerDetailResult.Success;
            }

            return response;
        }
        public async Task<object> GetCustomerByGLCode(string glCode)
        {
            //get the customer by zoho id
            var customer = await _repo.GetCustomerDetailsByGLCode(glCode);
            if (customer != null)
            {
                var CustomerDetails = _customermap.GetCustomerDetails(customer);

                if (CustomerDetails == null)
                {
                    return new LinkGetSuccessResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Not Found" };
                }
                else
                {
                    return new LinkGetSuccessResponse() { StatusCode = HttpStatusCode.OK, Message = "Success", Data = CustomerDetails };
                }
            }
            return new LinkGetSuccessResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Not Found" };
        }

        public async Task<IEnumerable<CommonDataSource>> GetCustomerMerchandiserById(int CustomerId)
        {
            var data = await _repo.GetCustomerMerchandiserById(CustomerId);
            if (data == null || data.Count == 0)
                return null;
            return data;
        }

        public async Task<List<CustomerAccountingAddress>> GetCustomerAddressByListCusId(List<int> lstcustomerId)
        {
            return await _repo.GetCustomerAddressByListCusId(lstcustomerId);

        }

        //get the collection based on customer Id
        public async Task<IEnumerable<CommonDataSource>> GetCustomerCollection(int CustomerId)
        {
            var data = await _repo.GetCustomerCollection(CustomerId);
            if (data == null || data.Count == 0)
                return null;
            return data;
        }

        //get the price category based on customer Id
        public async Task<DataSourceResponse> GetCustomerPriceCategory(int CustomerId)
        {
            var data = await _repo.GetCustomerPriceCategory(CustomerId);
            if (data == null || data.Count == 0)
                return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };

        }

        public async Task<CustomerPriceData> GetCustomerPriceData(int CustomerId)
        {
            return await _repo.GetCustomerPriceData(CustomerId);
        }

        /// <summary>
        /// Get the customer brands by customerid
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerBrands(int CustomerId)
        {
            var data = await _repo.GetCustomerBrands(CustomerId);
            if (data == null || data.Count == 0)
                return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };

        }

        /// <summary>
        /// Get the customer departments by customerid
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerDepartments(int CustomerId)
        {
            var data = await _repo.GetCustomerDepartments(CustomerId);
            if (data == null || data.Count == 0)
                return new DataSourceResponse { DataSourceList = null, Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };

        }

        /// <summary>
        /// Get the customer buyers by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerBuyers(int CustomerId)
        {
            var data = await _repo.GetCustomerBuyers(CustomerId);
            if (data == null || data.Count == 0)
                return new DataSourceResponse { DataSourceList = null, Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };

        }

        /// <summary>
        /// Get customer product category list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerProductCategoryList(int CustomerId)
        {
            var data = await _repo.GetCustomerProductCategoryList(CustomerId);
            if (data == null || data.Count == 0)
                return new DataSourceResponse { DataSourceList = null, Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };

        }

        public async Task<DataSourceResponse> GetCustomerProductSubCategoryList(int CustomerId, List<int?> productCategory)
        {
            var data = await _repo.GetCustomerProductSubCategoryList(CustomerId, productCategory);
            if (data == null || data.Count == 0)
                return new DataSourceResponse { DataSourceList = null, Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };

        }

        public async Task<DataSourceResponse> GetCustomerProductSub2CategoryList(CustomerSubCategory2Request customerSubCategory)
        {
            var data = await _repo.GetCustomerProductSub2CategoryList(customerSubCategory.ProductCategory, customerSubCategory.ProductSubCategory);
            if (data == null || data.Count == 0)
                return new DataSourceResponse { DataSourceList = null, Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };
        }

        //get Customer by customer name substring
        public async Task<DataSourceResponse> GetCustomerByName(string name)
        {
            var response = new DataSourceResponse();
            var data = await _repo.GetCustomerByNameAutocomplete(name);
            if (data == null || data.Count == 0)
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = data;
                response.Result = DataSourceResult.Success;
            }

            return response;
        }

        //get Customer by customer name substring
        public async Task<AddressDataSourceResponse> GetCustomerAddressbyCustomer(int CustomerId)
        {
            var response = new AddressDataSourceResponse();
            var data = await _repo.GetCustomerAddress(CustomerId);
            if (data == null || data.Count == 0)
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = data;
                response.Result = DataSourceResult.Success;
            }

            return response;
        }

        /// <summary>
        /// get customer contacts by customer 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerContactsbyCustomer(int CustomerId)
        {
            var response = new DataSourceResponse();
            var data = await _repo.GetCustomerContactListbyCustomer(CustomerId);
            if (data == null || data.Count == 0)
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = data;
                response.Result = DataSourceResult.Success;
            }

            return response;
        }

        /// Get the customer departments by customerids
        public async Task<ParentDataSourceResponse> GetCustomerDepartments(IEnumerable<int> customerIds)
        {
            var data = await _repo.GetCustomerDepartments(customerIds);
            if (data == null || !data.Any())
                return new ParentDataSourceResponse { DataSourceList = null, Result = DataSourceResult.CannotGetList };
            return new ParentDataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };

        }

        /// Get the customer brands by customerids
        public async Task<ParentDataSourceResponse> GetCustomerBrands(IEnumerable<int> customerIds)
        {
            var data = await _repo.GetCustomerBrands(customerIds);
            if (data == null || !data.Any())
                return new ParentDataSourceResponse { DataSourceList = data, Result = DataSourceResult.CannotGetList };
            return new ParentDataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };

        }

        //get Customer by customer name substring
        public async Task<DataSourceResponse> GetCustomerByCustomerId(int customerId)
        {
            var response = new DataSourceResponse();

            var list = new[] { customerId };
            var data = await _repo.GetCustomerById(list.ToList());
            if (data == null || data.Count == 0)
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = data;
                response.Result = DataSourceResult.Success;
            }

            return response;
        }

        //get customer list and map as key value for invoice pdf
        public async Task<IEnumerable<DataCommon>> GetCustomerItemList()
        {
            var data = await _repo.GetCustomersItems();
            if (data == null || data.Count == 0)
                return null;

            return data.Select(x => _customermap.GetCustomerData(x));
        }

        public async Task<DataSourceResponse> GetCustomerDataSource(CommonDataSourceRequest request)
        {
            var response = new DataSourceResponse();

            var data = _repo.GetCustomerDataSource();

            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        data = data.Where(x => x.Id == _ApplicationContext.CustomerId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        if (request.IsStatisticsVisible)
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.SupplierId && y.IsStatisticsVisibility == request.IsStatisticsVisible));
                        else
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.SupplierId));

                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        if (request.IsStatisticsVisible)
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.FactoryId && y.IsStatisticsVisibility == request.IsStatisticsVisible));
                        else
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.FactoryId));
                        break;
                    }

            }

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.CustomerName != null && EF.Functions.Like(x.CustomerName, $"%{request.SearchText.Trim()}%"));
            }

            if (request.Id > 0)
            {
                data = data.Where(x => x.Id == request.Id);
            }

            IEnumerable<CommonDataSource> filteredCustomers = null;
            if (request.IdList != null && request.IdList.Any())
            {
                filteredCustomers = await data.Where(x => request.IdList.Contains(x.Id)).Select(x => new CommonDataSource()
                {
                    Id = x.Id,
                    Name = x.CustomerName
                }).AsNoTracking().ToListAsync();
                data = data.Where(x => !request.IdList.Contains(x.Id));
            }

            var cuslist = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

            if (cuslist == null || !cuslist.Any())
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                var result = cuslist.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.CustomerName
                }).OrderBy(x => x.Name).ToList();
                if (filteredCustomers != null && filteredCustomers.Any())
                {
                    result.InsertRange(0, filteredCustomers);
                }
                response.DataSourceList = result;
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        #region Price Category

        /// <summary>
        /// get price Category by customer id and filter apply by price Category name
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetPriceCategoryDataSource(CommonCustomerSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _repo.GetPriceCategoryDataSource(request.CustomerId);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                var priceCategoryList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

                if (priceCategoryList == null || !priceCategoryList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = priceCategoryList;
                    response.Result = DataSourceResult.Success;
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion      

        /// <summary>
        /// Get the customer datasource list with GL Code
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CustomerGLCodeResponse> GetCustomerGLCodeSourceList(CustomerDataSourceRequest request)
        {
            var response = new CustomerGLCodeResponse() { Result = CustomerGLCodeSourceResult.CannotGetList };
            //get the Iqueryable customer datasource
            var data = _repo.GetCustomerDataSource();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.CustomerName != null && EF.Functions.Like(x.CustomerName, $"%{request.SearchText.Trim()}%"));
            }

            if (request.ServiceId > 0)
            {
                data = data.Where(x => x.CuApiServices.Any(y => y.ServiceId == request.ServiceId));
            }
            //Customer GLCode filter
            if (request.CustomerGLCodes != null && request.CustomerGLCodes.Any())
            {
                data = data.Where(x => request.CustomerGLCodes.Contains(x.GlCode));
            }
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        data = data.Where(x => x.Id == _ApplicationContext.CustomerId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.SupplierId));
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.FactoryId));
                        break;
                    }

            }
            //execute the customer list
            var customerList = await data.Skip(request.Skip).Take(request.Take).Select(x => new CustomerGLCodeSource
            {
                Id = x.Id,
                GLCode = x.GlCode,
                Name = x.CustomerName
            }).ToListAsync();

            //assign it to customer list
            if (customerList != null && customerList.Any())
            {
                response.DataSourceList = customerList.OrderBy(x => x.Name);
                response.Result = CustomerGLCodeSourceResult.Success;
            }
            return response;
        }

        /// <summary>
        /// get customer list by supplier
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CustomerDataSourceResponse> GetCustomerDataSourceBySupplier(CommonDataSourceRequest request)
        {
            var response = new CustomerDataSourceResponse();

            var data = _repo.GetCustomerDataSourceFromSupplier();

            //search by customer name
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.CustomerName != null && EF.Functions.Like(x.CustomerName, $"%{request.SearchText.Trim()}%"));
            }

            if (request.Id > 0)
            {
                data = data.Where(x => x.Id == request.Id);
            }

            //customer data filter by supplier id
            if (request.SupplierType == (int)Supplier_Type.Supplier_Agent && request.SupplierId > 0)
            {
                data = data.Where(x => x.SupplierId == request.SupplierId);
            }
            else if (request.SupplierType == (int)Supplier_Type.Factory && request.FactoryId > 0)
            {
                data = data.Where(x => x.SupplierId == request.FactoryId);
            }

            var supList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

            if (supList == null || !supList.Any())
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = supList.Select(x => new CommonDataSource()
                {
                    Name = x.CustomerName,
                    Id = x.Id
                }).OrderBy(x => x.Name);
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        /// <summary>
        /// Get the KAM(Key account manager) Details mapped to customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerKamDetail> GetCustomerKAMDetails(int customerId)
        {
            return await _repo.GetCustomerKAMDetails(customerId);
        }

        /// <summary>
        /// Get the customer product categories 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerProductCategories(int customerId)
        {
            var data = await _repo.GetCustomerProductCategories(customerId);
            if (data != null && data.Any())
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get the customer season configuration details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerSeasonConfiguration(int? customerId)
        {
            var seasonConfig = _repo.GetCustomerSeasonConfigQuery();

            //apply active
            seasonConfig = seasonConfig.Where(x => x.Active.Value);

            if (customerId > 0)
            {
                seasonConfig = seasonConfig.Where(x => x.CustomerId == customerId || (x.IsDefault.HasValue && x.IsDefault.Value));
            }

            var data = await seasonConfig.Select(x => new CommonDataSource() { Id = x.Id, Name = x.Season.Name }).ToListAsync();
            if (data != null && data.Any())
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        public async Task<DataSourceResponse> GetCustomerEntityList(int customerId)
        {
            var customerEntities = await _repo.GetCustomerEntityByCustomerId(customerId);
            if (customerEntities != null && customerEntities.Any())
            {
                return new DataSourceResponse() { DataSourceList = customerEntities, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { DataSourceList = customerEntities, Result = DataSourceResult.CannotGetList };
        }

        public async Task<object> SaveEAQFCustomer(SaveEaqfCustomerRequest request)
        {
            var response = new EaqfSaveSuccessResponse();
            try
            {
                //added custom validations if validation fails return response
                int customerId = 0;
                var errorResponse = await ValidateEaqfCustomer(request, customerId);
                if (errorResponse != null)
                    return errorResponse;

                // default is aqf
                int entityId = (int)Company.aqf;
                int userId = _ApplicationContext.UserId;
                if (_ApplicationContext.UserId == 0)
                    userId = Convert.ToInt16(_Configuration["ExternalAccessorUserId"]);

                // map customer request 
                CuCustomer customerEntity = _customermap.MapCustomersEntity(request, entityId, userId);
                response.Id = await _repo.AddCustomer(customerEntity);

                response.statusCode = HttpStatusCode.OK;
                response.message = Success;
                return response;
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { "InternalServerError" });
            }
        }

        public async Task<object> SaveEAQFCustomerContact(int customerId, SaveEaqfCustomerContactRequest request)
        {
            var response = new EaqfCustomerContactSaveSuccessResponse();
            try
            {
                // Added custom validations if validation fails return response
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var IsEmailExists = await _repo.IsCustomerContactExists(x => x.Active == true && x.Email.ToLower().Trim() == request.Email.ToLower().Trim());
                    if (IsEmailExists)
                    {
                        return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { EmailExists });
                    }
                }

                var customer = await _repo.GetEaqfCustomerDetailsByCustomerId(customerId);

                if (customer == null)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Customer Id" });
                }

                // default is aqf
                int entityID = (int)Company.aqf;
                var userID = _ApplicationContext.UserId;
                if (_ApplicationContext.UserId == 0)
                    userID = Convert.ToInt16(_Configuration["ExternalAccessorUserId"]);

                var customerContact = new CuContact()
                {
                    CustomerId = customerId,
                    ContactName = request.FirstName,
                    Phone = string.IsNullOrEmpty(request.Phone) ? "0" : request.Phone,
                    LastName = request.LastName,
                    Email = request.Email?.Trim(),
                    Active = true,
                    CreatedBy = userID,
                    CreatedOn = DateTime.Now,
                    CuCustomerContactTypes = new List<CuCustomerContactType>(){new CuCustomerContactType()
                    {
                        ContactTypeId = (int)CustomercontactType.Operations,
                    } },
                    CuContactServices = new List<CuContactService>() { new CuContactService()
                        {
                            ServiceId = (int)APIServiceEnum.Inspection,
                            Active=true
                        }
                    },
                    CuContactEntityMaps = new List<CuContactEntityMap>() { new CuContactEntityMap()
                        {
                            EntityId = entityID
                        }
                    },
                    CuContactEntityServiceMaps = new List<CuContactEntityServiceMap>() { new CuContactEntityServiceMap()
                        {
                             ServiceId =(int)APIServiceEnum.Inspection,
                             EntityId = entityID
                        }
                    },
                    PrimaryEntity = entityID
                };

                response.Id = await _repo.AddCustomerContact(customerContact);

                //create the credentials 
                var contactdetails = await _customerContactRepo.GetCustomerContactByCustomerId(customerId);
                if (contactdetails.Any())
                {
                    var contact = contactdetails.FirstOrDefault(x => x.Id == response.Id);
                    if (contact != null)
                    {
                        var userrequest = new CustomerContactUserRequest()
                        {
                            ContactId = contact.Id,
                            CustomerId = customerId,
                            Fullname = string.Format(request.FirstName, " ", request.LastName),
                            UserName = request.Email,
                            UserTypeId = (int)UserTypeEnum.Customer,
                            CreatedBy = userID
                        };

                        var credentials = await _customercontactmanager.CreateCustomerContactUserCredential(userrequest, CustomerContactCredentialsFrom.EAQF);

                        if (credentials != null && credentials.Result == SaveResult.Success)
                        {
                            response.UserId = credentials.Id;
                        }
                    }
                }

                if (response.Id == 0)
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { BadRequest });

                response.statusCode = HttpStatusCode.OK;
                response.message = Success;
                return response;
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { "InternalServerError" });
            }
        }

        /// <summary>
        /// update eaqf customer contacts
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="contactId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> UpdateEAQFCustomerContact(int customerId, int contactId, SaveEaqfCustomerContactRequest request)
        {
            var response = new EaqfSaveSuccessResponse();
            try
            {
                //added custom validations if validation fails return response
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var IsEmailExists = await _repo.IsCustomerContactExists(x => x.Active == true && x.Id != contactId && x.Email.ToLower().Trim() == request.Email.ToLower().Trim());
                    if (IsEmailExists)
                    {
                        return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { EmailExists });
                    }
                }

                var userID = _ApplicationContext.UserId;
                if (_ApplicationContext.UserId == 0)
                    userID = Convert.ToInt16(_Configuration["ExternalAccessorUserId"]);

                var entity = await _customerContactRepo.GetCustomerContactByCustomerIdAndContactId(customerId, contactId);

                if (entity == null)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer contact not found" });
                }

                entity.ContactName = request.FirstName?.Trim();
                entity.LastName = request.LastName?.Trim();
                entity.Phone = string.IsNullOrEmpty(request.Phone) ? "0" : request.Phone?.Trim();
                entity.Email = request.Email?.Trim();
                entity.UpdatedBy = userID;
                entity.UpdatedOn = DateTime.Now;
                await _repo.EditCustomerContact(entity);

                response.Id = contactId;
                response.statusCode = HttpStatusCode.OK;
                response.message = Success;
                return response;
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { "InternalServerError" });
            }
        }

        public async Task<object> SaveEAQFCustomerAddress(int customerId, SaveEaqfCustomerAddressRequest request)
        {
            var response = new EaqfSaveSuccessResponse();
            try
            {
                // default is aqf
                int entityID = (int)Company.aqf;
                var userID = _ApplicationContext.UserId;
                if (_ApplicationContext.UserId == 0)
                    userID = Convert.ToInt16(_Configuration["ExternalAccessorUserId"]);

                var customer = await _repo.GetEaqfCustomerDetailsByCustomerId(customerId);

                if (customer == null)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Customer Id" });
                }

                var country = await _locationRepository.GetCountriesByAlpha2Code(request.Country);

                if (country == null)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidCountry });
                }

                var cityList = _locationRepository.GetCityQueryableByCountryId(country.Id);

                // map default first city
                var firstCity = await cityList.FirstOrDefaultAsync();

                if (!string.IsNullOrWhiteSpace(request.City.ToLower()))
                {
                    cityList = cityList.Where(x => x.CityName.ToLower() == request.City.ToLower());
                }

                var city = await cityList.FirstOrDefaultAsync();

                var officeAddress = new CuAddress()
                {
                    CustomerId = customerId,
                    CountryId = country.Id,
                    CityId = city != null ? city.Id : firstCity.Id,
                    AddressType = request.AddressTypeId,
                    Address = request.Address,
                    BoxPost = request.BoxPost,
                    ZipCode = request.ZipCode,
                    Active = true,
                    EntityId = entityID,
                    CreatedBy = userID,
                    CreatedOn = DateTime.Now
                };

                response.Id = await _repo.AddCustomerAddress(officeAddress);
                response.statusCode = HttpStatusCode.OK;
                response.message = Success;
                return response;
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { "InternalServerError" });
            }
        }

        /// <summary>
        /// Update Customer Address
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> UpdateEAQFCustomer(int customerId, SaveEaqfCustomerRequest request)
        {
            var response = new EaqfSaveSuccessResponse();
            try
            {
                if (request == null)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Request" });
                }

                if (customerId <= 0)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Customer Id" });
                }

                var errorResponse = await ValidateEaqfCustomer(request, customerId);
                if (errorResponse != null)
                    return errorResponse;

                int userId = _ApplicationContext.UserId;
                if (_ApplicationContext.UserId == 0)
                    userId = Convert.ToInt16(_Configuration["ExternalAccessorUserId"]);

                var entity = await _repo.GetEaqfCustomerDetailsByCustomerId(customerId);

                if (entity == null)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Customer Id" });
                }

                entity.CustomerName = request.CompanyName?.Trim().ToUpper();
                entity.Phone = request.Phone?.Trim();
                entity.Email = request.Email?.Trim();
                entity.Website = request.Website?.Trim();
                entity.UpdatedBy = userId;
                entity.UpdatedOn = DateTime.Now;

                await _repo.EditCustomer(entity);
                response.Id = customerId;
                response.statusCode = HttpStatusCode.OK;
                response.message = Success;
                return response;
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { "InternalServerError" });
            }
        }

        public async Task<object> UpdateEAQFCustomerAddress(int customerId, int addressId, UpdateEaqfCustomerAddressRequest request)
        {
            var response = new EaqfSaveSuccessResponse();
            try
            {
                if (request == null)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Request" });
                }

                if (customerId <= 0)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid customer id" });
                }

                if (addressId <= 0)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid address id" });
                }

                var userID = _ApplicationContext.UserId;
                if (_ApplicationContext.UserId == 0)
                    userID = Convert.ToInt16(_Configuration["ExternalAccessorUserId"]);

                var entity = await _repo.GetEaqfCustomerAddress(customerId, addressId);

                if (entity == null)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Address not found" });
                }

                int? cityId = null;
                int? countryId = null;

                var addressCountry = await _locationRepository.GetCountriesByAlpha2Code(request.Country);

                if (addressCountry != null)
                {
                    countryId = addressCountry.Id;
                }

                var addresscity = await _locationRepository.GetCityByCityName(request.City);

                if (addresscity != null)
                {
                    cityId = addresscity.Id;
                }

                entity.Address = request.Address?.Trim();
                entity.ZipCode = request.ZipCode?.Trim();
                entity.BoxPost = request.BoxPost?.Trim();
                entity.AddressType = request.AddressTypeId;
                entity.CountryId = countryId > 0 ? countryId : null;
                entity.CityId = cityId > 0 ? cityId : null;
                entity.UpdatedBy = userID;
                entity.UpdatedOn = DateTime.Now;

                await _repo.EditCustomerAddress(entity);
                response.Id = addressId;

                response.statusCode = HttpStatusCode.OK;
                response.message = Success;
                return response;
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { "InternalServerError" });
            }
        }

        public async Task<EaqfErrorResponse> ValidateEaqfCustomer(SaveEaqfCustomerRequest request, int customerId)
        {
            if (!string.IsNullOrEmpty(request.CompanyName))
            {
                var IsCustomerExists = await _repo.IsCustomerExists(x => x.Active == true && x.Id != customerId
                && x.CustomerName.ToLower().Trim() == request.CompanyName.ToLower().Trim());
                if (IsCustomerExists)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { CompanyNameExists });
                }
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                var IsEmailExists = await _repo.IsCustomerExists(x => x.Active == true && x.Id != customerId
                && x.Email.ToLower().Trim() == request.Email.ToLower().Trim());
                if (IsEmailExists)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { EmailExists });
                }
            }

            return null;
        }

        public EaqfErrorResponse BuildCommonEaqfResponse(HttpStatusCode statusCode, string message, List<string> errors)
        {
            return new EaqfErrorResponse()
            {
                errors = errors,
                statusCode = statusCode,
                message = message
            };
        }

        public EaqfSaveSuccessResponse BuildCommonEaqSuccessResponse(HttpStatusCode statusCode, string message)
        {
            return new EaqfSaveSuccessResponse()
            {
                statusCode = statusCode,
                message = message
            };
        }

        public async Task<object> GetEAQFCustomer(int Index, int PageSize, string CompanyName, string Email)
        {
            var response = new EaqfGetSuccessResponse() { };
            var eaqf = new GetEaqfCustomer() { Index = Index, PageSize = PageSize };
            try
            {
                var data = _repo.GetCustomerDataSource();
                if (!string.IsNullOrEmpty(CompanyName) && !string.IsNullOrWhiteSpace(CompanyName))
                    data = data.Where(x => x.CustomerName.ToLower() == CompanyName.ToLower());
                if (!string.IsNullOrEmpty(Email) && !string.IsNullOrWhiteSpace(Email))
                    data = data.Where(x => x.Email.ToLower() == Email.ToLower());


                eaqf.TotalCount = await data.AsNoTracking().CountAsync();
                if (eaqf.TotalCount == 0)
                {
                    return new EaqfGetSuccessResponse() { statusCode = HttpStatusCode.BadRequest, message = BadRequest, data = eaqf };
                }
                int skip = (Index - 1) * PageSize;
                var customers = await data.Skip(skip).Take(PageSize).AsNoTracking().ToListAsync();
                var customerIds = customers.Select(x => x.Id).ToList();
                var customerContact = await _customerContactRepo.GetCustomerContactByCustomerIds(customerIds);
                var customerAddressList = await _customerContactRepo.GetCustomerAddressListByCustomerId(customerIds);

                response.statusCode = HttpStatusCode.OK;
                response.message = Success;
                eaqf.customerList = customers.Select(x => _customermap.GetEAQFCustomerMap(x, customerContact, customerAddressList)).ToList();
                response.data = eaqf;
                return response;
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { InternalServerError });
            }

        }


        /// <summary>
        /// get customer sister company by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerSisterCompany(int customerId)
        {
            var customerSisterCompanies = await _repo.GetCustomerSisterCompanyByCustomerId(customerId);
            if (customerSisterCompanies != null && customerSisterCompanies.Any())
            {
                return new DataSourceResponse() { DataSourceList = customerSisterCompanies, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { DataSourceList = customerSisterCompanies, Result = DataSourceResult.CannotGetList };
        }

        public async Task<DataSourceResponse> GetCustomerByUserType(CommonDataSourceRequest request)
        {
            var response = new DataSourceResponse();

            var data = _repo.GetCustomerDataSource();

            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        var contactId = await _userRepository.GetCustomerContactIdByUserId(_ApplicationContext.UserId);
                        var sisterCompanyIds = await _repo.GetSisterCompanieIdsByCustomerContactId(contactId);
                        data = data.Where(x => x.Id == _ApplicationContext.CustomerId || sisterCompanyIds.Contains(x.Id));
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        if (request.IsStatisticsVisible)
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.SupplierId && y.IsStatisticsVisibility == request.IsStatisticsVisible));
                        else
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.SupplierId));
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        if (request.IsStatisticsVisible)
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.FactoryId && y.IsStatisticsVisibility == request.IsStatisticsVisible));
                        else
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.FactoryId));
                        break;
                    }

            }

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.CustomerName != null && EF.Functions.Like(x.CustomerName, $"%{request.SearchText.Trim()}%"));
            }

            if (request.Id > 0)
            {
                data = data.Where(x => x.Id == request.Id);
            }

            if (request.IdList != null && request.IdList.Any())
            {
                data = data.Where(x => request.IdList.Contains(x.Id));
            }

            var cuslist = request.IsVirtualScroll ? await data.Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync()
                : await data.AsNoTracking().ToListAsync();

            if (cuslist == null || !cuslist.Any())
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = cuslist.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.CustomerName
                }).OrderBy(x => x.Name).ToList();
                response.Result = DataSourceResult.Success;
            }
            return response;

        }

        /// <summary>
        /// Get the customer contact and address details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerContactAddressDetails> GetCustomerContactAddressDetails(int customerId)
        {
            var response = new CustomerContactAddressDetails();

            //get the customer address list
            var customerAddressList = await _repo.GetCustomerAddress(customerId);

            //get the customer contact list
            var customerContacts = await _repo.GetCustomerContact(customerId);

            //assign the customer address list
            if (customerAddressList.Any())
            {
                response.AddressList = customerAddressList;
                response.Result = CustomerAddressContactResult.Success;
            }
            else
                response.Result = CustomerAddressContactResult.AddressNotFound;

            //assign the customer contact list
            if (customerContacts.Any())
            {
                response.ContactList = customerContacts;
                response.Result = CustomerAddressContactResult.Success;
            }
            else
                response.Result = CustomerAddressContactResult.ContactNotFound;

            return response;
        }
    }
}
