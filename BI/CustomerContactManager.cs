using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Inspection;
using DTO.UserAccount;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BI
{
    public class CustomerContactManager : ApiCommonData, ICustomerContactManager
    {
        private readonly ICustomerRepository _customerRepo = null;
        private readonly ICustomerContactRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IReferenceManager _referenceManager = null;
        private static IConfiguration _Configuration = null;
        private readonly IUserAccountRepository _userAccountRepo = null;
        private readonly IUserRepository _userRepository = null;
        private readonly CustomerMap _customermap = null;
        private readonly IUserAccountManager _userAccountManager;
        private readonly ITenantProvider _filterService = null;
        public CustomerContactManager(
            ICustomerRepository customerRepo,
            ICustomerContactRepository repo,
            ILocationRepository locationRepository,
            IAPIUserContext applicationContextService,
            IReferenceManager referenceManager,
            IConfiguration configuration,
            IUserAccountRepository userAccountRepo,
            IUserRepository userRepository,
            IUserAccountManager userAccountManager,
            ITenantProvider filterService)
        {
            _repo = repo;
            _customerRepo = customerRepo;
            _ApplicationContext = applicationContextService;
            _referenceManager = referenceManager;
            _Configuration = configuration;
            _userAccountRepo = userAccountRepo;
            _userRepository = userRepository;
            _filterService = filterService;
            _customermap = new CustomerMap();
            _userAccountManager = userAccountManager;
        }

        public async Task<IEnumerable<CustomerContact>> GetCustomerContacts(int CustomerId)
        {
            var data = await _repo.GetCustomerContacts(CustomerId);
            if (data == null && data.Count == 0)
                return null;
            return data.Select(x => _customermap.GetCustomerContact(x));
        }

        public CustomerContactSearchResponse GetCustomerContactData(CustomerContactSearchRequest request)
        {
            var response = new CustomerContactSearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };
            if (request.customerValue != null)
            {

                var data = _repo.GetCustomerContactsByID(request.customerValue);

                var customerContacts = data.CuContacts.Where(x => x.Active == true);

                if (request.contactName != null && request.contactName != string.Empty)
                {
                    customerContacts = customerContacts.Where(x => x.ContactName == request.contactName || x.Email == request.contactName)
                                        .ToList();
                }

                if (request.CuBrandList.Count() > 0)
                {
                    customerContacts = customerContacts.Where(x => x.CuContactBrands.Where(y => y.Active).Any(y => request.CuBrandList.Contains(y.BrandId)));
                }

                if (request.CudepartmentList.Count() > 0)
                {
                    customerContacts = customerContacts.Where(x => x.CuContactDepartments.Where(y => y.Active).Any(y => request.CudepartmentList.Contains(y.DepartmentId)));
                }

                if (request.CuServiceList.Count() > 0)
                {
                    customerContacts = customerContacts.Where(x => x.CuContactServices.Where(y => y.Active).Any(y => request.CuServiceList.Contains(y.ServiceId)));
                }

                customerContacts = customerContacts.OrderBy(contact => contact.ContactName);

                response.TotalCount = customerContacts.Count();

                if (response.TotalCount == 0)
                {
                    response.Result = CustomerContactSearchResult.NotFound;
                    return response;
                }
                int skip = (request.Index.Value - 1) * request.pageSize.Value;
                response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                response.Data = customerContacts.Skip(skip).Take(request.pageSize.Value).Select(x => _customermap.GetCustomerContactItem(x)).ToArray();
                response.ContactBrandList = request.CuBrandList.ToList();
                response.ContactDepartmentList = request.CudepartmentList.ToList();
                response.ContactServiceList = request.CuServiceList.ToList();

                response.Result = CustomerContactSearchResult.Success;

            }
            return response;
        }

        public async Task<CustomerContactSummaryResponse> GetCustomerContactSummary(CustomerContactSummaryRequest request)
        {
            var response = new CustomerContactSummaryResponse { Index = request.index.Value, PageSize = request.pageSize.Value };
            if (request.customerID != null)
            {

                var customerContactList = await _repo.GetCustomerContacts(request.customerID);

                var customerContacts = customerContactList.Where(x => x.Active == true).OrderBy(contact => contact.ContactName);
                response.TotalCount = customerContacts.Count();

                if (response.TotalCount == 0)
                {
                    response.Result = CustomerContactSummaryResult.NotFound;
                    return response;
                }
                int skip = (request.index.Value - 1) * request.pageSize.Value;
                response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                response.CustomerContacts = customerContacts.Skip(skip).Take(request.pageSize.Value).Select(x => _customermap.GetCustomerContactItem(x)).ToArray();


                response.Result = CustomerContactSummaryResult.Success;

            }
            return response;
        }

        public async Task<CustomerContactDeleteResponse> DeleteCustomerContact(int id)
        {
            var customerContact = _repo.GetCustomerContactByContactID(id);
            customerContact.DeletedBy = _ApplicationContext.UserId;
            customerContact.DeletedOn = DateTime.Now;
            customerContact = _customermap.UpdateCuContactBrandEntity(customerContact, _ApplicationContext.UserId);

            if (customerContact == null)
                return new CustomerContactDeleteResponse { Id = id, Result = CustomerContactDeleteResult.NotFound };

            await _repo.RemoveCustomerContact(id);

            var userId = await _userAccountRepo.GetUserByContactId(id);
            var updatedby = _ApplicationContext.UserId;
            await _userAccountRepo.RemoveUserAccount(userId, updatedby);

            return new CustomerContactDeleteResponse { Id = id, Result = CustomerContactDeleteResult.Success };

        }

        public async Task<EditCustomerContactResponse> GetEditCustomerContact(int? id)
        {
            var response = new EditCustomerContactResponse();

            if (id != null)
            {
                var CustomerContacts = _repo.GetCustomerContactByContactID(id);
                if (CustomerContacts != null)
                {
                    var CustomerID = CustomerContacts.CustomerId;
                    var CustomerAddressList = _repo.GetCustomerAddressByCustomerID(CustomerID);
                    var CuAddressList = CustomerAddressList.Select(x => new CustomerAddressData { Address = x.Address, Id = x.Id }).ToList();

                    var ContactTypeList = _repo.GetContactTypes();

                    //get the contact sister company ids
                    var contactSisterCompanies = await _customerRepo.GetSisterCompanieIdsByCustomerContactId(id.Value);
                    var CustomerContactDetails = _customermap.GetCustomerContactDetails(CustomerContacts, CuAddressList, ContactTypeList, contactSisterCompanies);
                    response.CustomerContactDetails = CustomerContactDetails;
                    if (response.CustomerContactDetails == null)
                        return new EditCustomerContactResponse { Result = EditCustomerContactResult.CannotGetCustomer };

                    // contact brand list
                    response.ContactBrandList = await GetCustomerBrandsByUserId(CustomerContactDetails.CustomerID, _ApplicationContext.UserId);

                    // contact department list
                    response.ContactDepartmentList = await GetCustomerDepartmentByUserId(CustomerContactDetails.CustomerID, _ApplicationContext.UserId);

                    // contact department list
                    response.ContactServiceList = await _referenceManager.GetServices();

                    //contact report to list
                    response.ReportToList = await _repo.GetCustomerContactList(CustomerID, id.GetValueOrDefault());

                }
            }
            response.Result = EditCustomerContactResult.Success;
            return response;
        }


        public async Task<IEnumerable<CustomerBrand>> GetCustomerBrandsByUserId(int CustomerId, int UserId)
        {
            var data = await _customerRepo.GetCustomerBrandsByUserId(CustomerId);
            if (data == null || data.Count == 0)
                return null;
            return data.Select(x => _customermap.GetCustomerBrand(x));
        }

        public async Task<IEnumerable<CustomerDepartment>> GetCustomerDepartmentByUserId(int CustomerId, int UserId)
        {
            var data = await _customerRepo.GetCustomerDepartmentByUserId(CustomerId);
            if (data == null || data.Count == 0)
                return null;
            return data.Select(x => _customermap.GetCustomerDepartment(x));
        }

        public async Task<CustomerContactResponse> GetCustomerContact(int? customerID)
        {
            var response = new CustomerContactResponse();

            response.CustomerContactDetails = new CustomerContactDetails();
            response.CustomerContactDetails.Name = "";
            response.CustomerContactDetails.JobTitle = "";
            response.CustomerContactDetails.Email = "";
            response.CustomerContactDetails.Mobile = "";
            response.CustomerContactDetails.Phone = "";
            response.CustomerContactDetails.Fax = "";
            response.CustomerContactDetails.Others = "";
            response.CustomerContactDetails.Office = 0;
            response.CustomerContactDetails.Comments = "";
            response.CustomerContactDetails.PromotionalEmail = true;


            var CustomerAddressList = _repo.GetCustomerAddressByCustomerID(customerID);
            var CuAddressList = CustomerAddressList.Select(x => new CustomerAddressData { Address = x.Address, Id = x.Id }).ToList();
            var ContactTypes = _repo.GetContactTypes();
            var ContactTypeList = _customermap.GetContactTypes(ContactTypes);

            if (CustomerAddressList == null)
            {
                return new CustomerContactResponse { Result = CustomerContactResult.CannotGetAddressList };
            }


            if (ContactTypeList == null)
            {
                return new CustomerContactResponse { Result = CustomerContactResult.CannotGetContactTypes };
            }
            response.CustomerAddressList = CuAddressList;
            response.ContactTypeList = ContactTypeList;
            response.CustomerContactDetails.CustomerAddressList = CuAddressList;
            response.CustomerContactDetails.ContactTypeList = ContactTypeList;

            // contact brand list
            response.ContactBrandList = await GetCustomerBrandsByUserId(customerID.Value, _ApplicationContext.UserId);

            // contact department list
            response.ContactDepartmentList = await GetCustomerDepartmentByUserId(customerID.Value, _ApplicationContext.UserId);

            // contact department list
            response.ContactServiceList = await _referenceManager.GetServices();

            response.ReportToList = await _repo.GetCustomerContactList(customerID.Value, 0);

            response.Result = CustomerContactResult.Success;
            return response;
        }

        public async Task<SaveCustomerContactResponse> ValidateCustomerContactData(CustomerContactDetails request)
        {
            var response = new SaveCustomerContactResponse();
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var customercontact = await _repo.GetCustomerContactByEmail(request.Email, request.CustomerID);
                if (customercontact != null && customercontact.Id != request.Id)
                    response.Result = SaveCustomerContactResult.DuplicateEmailIDExists;
            }
            return response;
        }

        public async Task<SaveCustomerContactResponse> Save(CustomerContactDetails request)
        {
            var response = new SaveCustomerContactResponse();
            var userID = _ApplicationContext.UserId;
            if (_ApplicationContext.UserId == 0)
                userID = Convert.ToInt16(_Configuration["ExternalAccessorUserId"]);

            var entityId = _filterService.GetCompanyId();
            if (_filterService.GetCompanyId() == 0)
                entityId = Convert.ToInt16(_Configuration["ExternalAccessorEntityID"]);
            response = await ValidateCustomerContactData(request);
            if (response.Result != 0)
                return response;

            if (request.Id == 0)
            {
                CuContact entity = _customermap.MapCustomerContactEntity(request, userID, entityId);

                if (entity == null)
                    return new SaveCustomerContactResponse { Result = SaveCustomerContactResult.CustomerContactIsNotFound };

                response.Id = _repo.AddCustomerContact(entity);

                if (response.Id == 0)
                    return new SaveCustomerContactResponse { Result = SaveCustomerContactResult.CustomerContactIsNotSaved };

                response.Result = SaveCustomerContactResult.Success;

                return response;
            }
            else
            {
                var entity = _repo.GetCustomerContactByContactID(request.Id);

                if (entity == null)
                    return new SaveCustomerContactResponse { Result = SaveCustomerContactResult.CustomerContactIsNotFound };

                // remove customer contact entity and service mapping
                _repo.RemoveEntities(entity.CuContactEntityServiceMaps);

                if (request.ApiEntityIds != null && request.ApiEntityIds.Any())
                    await UpdateCustomerContactMapEntities(entity, request);

                _customermap.UpdateCustomerContactEnity(entity, request, userID);

                UpdateCustomerContactSisterCompanies(entity, request, userID, entityId);

                response.Id = _repo.EditCustomerContact(entity);

                response.Result = SaveCustomerContactResult.Success;
            }

            return response;
        }

        //add or delete the customer contact entities and user roles
        private async Task UpdateCustomerContactMapEntities(CuContact entity, CustomerContactDetails request)
        {
            //deleteCuCustomerEntityMaps
            var deleteCuCustomerEntityMaps = entity.CuContactEntityMaps.Where(x => !request.ApiEntityIds.Contains(x.EntityId)).ToList();

            //get the db cu customer entity id
            var dbCuCustomerEntityIdList = entity.CuContactEntityMaps.Select(x => x.EntityId);

            //fetch new apientityid
            var newCuContactEntityIds = request.ApiEntityIds.Where(x => !dbCuCustomerEntityIdList.Contains(x)).ToList();


            IEnumerable<ItUserRole> customerContactUserRoles = null;
            if (deleteCuCustomerEntityMaps.Any() || newCuContactEntityIds.Any())
            {
                var customerContactUsers = await _userAccountRepo.GetUserByCustomerContactIds(new List<int>() { entity.Id });
                customerContactUserRoles = await _userRepository.GetUserRolesByUserIdsIgnoreQueryFilter(customerContactUsers.Select(x => x.Id));
            }

            if (deleteCuCustomerEntityMaps.Any())
            {
                if (customerContactUserRoles != null && customerContactUserRoles.Any())
                {
                    var deleteCustomerContactEntityIds = deleteCuCustomerEntityMaps.Select(x => x.EntityId);
                    var deleteCustomerContactUserRoles = customerContactUserRoles.Where(x => deleteCustomerContactEntityIds.Contains(x.EntityId));
                    if (deleteCustomerContactUserRoles.Any())
                        _repo.RemoveEntities(deleteCustomerContactUserRoles);
                }
                _repo.RemoveEntities(deleteCuCustomerEntityMaps);
            }

            //new customer contact entity
            if (newCuContactEntityIds.Any())
            {
                //new customer contact entity list
                newCuContactEntityIds.ForEach(entityId =>
                {
                    //add CU_Contact_Entity_Map
                    var cuContactEntityMap = new CuContactEntityMap()
                    {
                        EntityId = entityId
                    };

                    entity.CuContactEntityMaps.Add(cuContactEntityMap);
                    _repo.AddEntity(cuContactEntityMap);


                    if (customerContactUserRoles != null && customerContactUserRoles.Any())
                    {
                        //get the customer contact primray user roles
                        var customerContactUserPrimaryRoles = customerContactUserRoles.Where(x => x.EntityId == entity.PrimaryEntity).ToList();
                        if (customerContactUserPrimaryRoles.Any())
                        {
                            //customer contact primray user roles loop
                            customerContactUserPrimaryRoles.ForEach(primaryUserRole =>
                            {
                                //add the IT_UserRole table
                                var itUserRole = new ItUserRole()
                                {
                                    EntityId = entityId,
                                    RoleId = primaryUserRole.RoleId,
                                    UserId = primaryUserRole.UserId
                                };
                                _repo.AddEntity(itUserRole);
                            });
                        }

                    }
                });
            }
        }


        private void UpdateCustomerContactSisterCompanies(CuContact entity, CustomerContactDetails request, int userId, int entityId)
        {
            if (request.ContactSisterCompanyIds == null)
                request.ContactSisterCompanyIds = new List<int>();

            var deleteCustomerContactSisterCompanies = entity.CuContactSisterCompanies.Where(x => x.Active == true && !request.ContactSisterCompanyIds.Contains(x.SisterCompanyId)).ToList();

            //get the db contact sister company id
            var dbContactSisterCompanyIdList = entity.CuContactSisterCompanies.Where(x => x.Active == true).Select(x => x.SisterCompanyId).ToList();

            //fetch new sister company id
            var newContactSisterCompanyIds = request.ContactSisterCompanyIds.Where(x => !dbContactSisterCompanyIdList.Contains(x)).ToList();


            //delete customer contact sister companie
            if (deleteCustomerContactSisterCompanies.Any())
            {
                deleteCustomerContactSisterCompanies.ForEach(x =>
                {
                    x.Active = false;
                    x.DeletedOn = DateTime.Now;
                    x.DeletedBy = userId;
                });
                _repo.EditEntities(deleteCustomerContactSisterCompanies);
            }

            //new customer contact sister company
            if (newContactSisterCompanyIds.Any())
            {
                //new customer contact sister company list
                newContactSisterCompanyIds.ForEach(sisterCompanyId =>
                {
                    //add customer contact sister company
                    var cuContactSisterCompany = new CuContactSisterCompany()
                    {
                        EntityId = entityId,
                        Active = true,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now,
                        SisterCompanyId = sisterCompanyId
                    };

                    entity.CuContactSisterCompanies.Add(cuContactSisterCompany);
                    _repo.AddEntity(cuContactSisterCompany);

                });
            }
        }
        public async Task<EditCustomerContactResponse> GetContactBrandByCusId(int customerId)
        {
            return new EditCustomerContactResponse
            {
                CustomerContactDetails = null,

                // contact brand list
                ContactBrandList = await GetCustomerBrandsByUserId(customerId, _ApplicationContext.UserId),

                // contact department list
                ContactDepartmentList = await GetCustomerDepartmentByUserId(customerId, _ApplicationContext.UserId),

                // contact department list
                ContactServiceList = await _referenceManager.GetServices(),

                Result = EditCustomerContactResult.Success
            };
        }

        public async Task<CustomerContactDataResponse> GetCustomerContact(int customerId, int contactId)
        {
            var response = new CustomerContactDataResponse();

            var CustomerContact = await _repo.GetCustomerContacts(customerId, contactId);
            if (CustomerContact == null)
                response.Result = CustomerContactDataResult.CannotGetContactData;
            if (CustomerContact != null)
            {
                var CustomerContactDetails = _customermap.MapCustomerContactData(CustomerContact);
                response.CustomerContactDetails = CustomerContactDetails;
            }
            response.Result = CustomerContactDataResult.Success;
            return response;
        }
        /// <summary>
        /// Map zoho customer contact data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="zohoCustomerId"></param>
        /// <param name="zohoContactId"></param>
        /// <returns></returns>
        public async Task<object> SaveZohoCRMContact(SaveZohoCrmCustomerContactDetails request, string zohoCustomerId, string zohoContactId)
        {

            var validationErrorResponse = await ValidateSaveZohoRequest(zohoCustomerId, zohoContactId, request, (int)ZohoCustomerRequestEnum.Save);

            if (validationErrorResponse != null)
                return validationErrorResponse;


            //Generate Customer Details Data
            var customerContactDetails = new CustomerContactDetails();

            //get the customer by zoho customerid
            var zohoCustomerIdValue = long.Parse(zohoCustomerId);
            var zohoContactIdValue = long.Parse(zohoContactId);

            var customer = await _customerRepo.GetCustomerByZohoId(zohoCustomerIdValue);


            if (customer != null)
            {
                //get the customer address by customerid
                var office = await _customerRepo.GetZohoCustomerAddressById(customer.Id);
                //map the customer contact as per the zoho input
                customerContactDetails = _customermap.MapSaveZohoCustomerContact(request, customer.Id, office);
                customerContactDetails.ZohoContactId = zohoContactIdValue;
                customerContactDetails.ZohoCustomerId = zohoCustomerIdValue;
            }

            if (string.IsNullOrEmpty(request.Company))
            {
                return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                     BadRequest, new List<string>() { "Company is not Valid" });
            }

            var entityID = CompanyList.FirstOrDefault(x => x.Value == request.Company).Key;

            if (entityID > 0)
            {
                customerContactDetails.ApiEntityIds = new List<int>() { entityID };
                customerContactDetails.PrimaryEntity = entityID;
            }
            else
                return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                       BadRequest, new List<string>() { "Company is not Valid" });

            //save the customer contact details
            var response = await Save(customerContactDetails);
            if (response != null && response.Result == SaveCustomerContactResult.DuplicateEmailIDExists)
            {


                return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                BadRequest, new List<string>() { "Duplicate Email Id Exists" });

            }

            if (response != null)

                return BuildCommonLinkSuccessResponse(HttpStatusCode.Created,
                 Success, response.Id);

            return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                      BadRequest, new List<string>() { "Unknown error - Please contact IT-Team." });
        }

        /// <summary>
        /// Update the customer contact details by zohocustomerid and zohocontactid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="zohoCustomerId"></param>
        /// <param name="zohoContactId"></param>
        /// <returns></returns>
        public async Task<object> UpdateZohoCRMContact(SaveZohoCrmCustomerContactDetails request, string zohoCustomerId, string zohoContactId)
        {
            var validationErrorResponse = await ValidateSaveZohoRequest(zohoCustomerId, zohoContactId, request, (int)ZohoCustomerRequestEnum.Update);

            if (validationErrorResponse != null)
                return validationErrorResponse;

            var customerContactDetails = new CustomerContactDetails();
            //get customer by zoho id
            var zohoCustomerIdValue = long.Parse(zohoCustomerId);
            var zohoContactIdValue = long.Parse(zohoContactId);
            var customer = await _customerRepo.GetCustomerByZohoId(zohoCustomerIdValue);
            if (customer != null)
            {
                //get customer address by customerid
                var office = await _customerRepo.GetZohoCustomerAddressById(customer.Id);
                customerContactDetails = _customermap.MapSaveZohoCustomerContact(request, customer.Id, office);
                customerContactDetails.ZohoContactId = zohoContactIdValue;
                customerContactDetails.ZohoCustomerId = zohoCustomerIdValue;
                //get the customer contact by zoho customer id and contact id
                var customerContact = await _repo.GetCustomerContactByZohoData(zohoCustomerIdValue, zohoContactIdValue);
                if (customerContact != null)
                    customerContactDetails.Id = customerContact.Id;
            }

            if (string.IsNullOrEmpty(request.Company))
            {
                return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                     BadRequest, new List<string>() { "Company is not Valid" });
            }

            var entityID = CompanyList.FirstOrDefault(x => x.Value == request.Company).Key;

            if (entityID > 0)
            {
                customerContactDetails.PrimaryEntity = entityID;
                customerContactDetails.ApiEntityIds = new List<int>() { entityID };
            }
            else
                return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                       BadRequest, new List<string>() { "Company is not Valid" });

            //save the customer contact details
            var response = await Save(customerContactDetails);
            if (response != null && response.Result == SaveCustomerContactResult.DuplicateEmailIDExists)
            {
                return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest,
                BadRequest, new List<string>() { "Duplicate Email Id Exists" });
            }

            if (response != null)
                return BuildCommonLinkSuccessResponse(HttpStatusCode.OK,
                 Success, response.Id);

            return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest,
             BadRequest, new List<string>() { "Unknown Error - Please contact IT-Team" });
        }

        /// <summary>
        /// Get the customer contacts by zoho customer id and contactid
        /// </summary>
        /// <param name="zohoCustomerId"></param>
        /// <param name="zohoContactId"></param>
        /// <returns></returns>
        public async Task<object> GetCustomerContactByZohoData(long zohoCustomerId, long zohoContactId)
        {
            var response = new CustomerContactDataResponse();
            //get customer contact by zoho customerid and contactid
            var zohoCustomerContact = await _repo.GetCustomerContactByZohoData(zohoCustomerId, zohoContactId);
            if (zohoCustomerContact != null)
            {

                var CustomerContactDetails = _customermap.MapCustomerContactData(zohoCustomerContact);
                return new LinkGetSuccessResponse() { Data = CustomerContactDetails, StatusCode = HttpStatusCode.OK, Message = "Success" };
            }


            return new LinkGetSuccessResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Not Found" };
        }

        /// <summary>
        /// Get the customer contacts by zoho customerid and contact emailid
        /// </summary>
        /// <param name="zohoContactId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<object> GetCustomerContactByEmailAndID(long zohoCustomerId, string email)
        {
            //get customer contact by zoho customerid and emailid
            var zohoCustomerContact = await _repo.GetCustomerContactByEmailAndZohoID(zohoCustomerId, email);
            if (zohoCustomerContact != null)
            {

                var CustomerContactDetails = _customermap.MapCustomerContactData(zohoCustomerContact);

                return new LinkGetSuccessResponse() { Data = CustomerContactDetails, StatusCode = HttpStatusCode.OK, Message = "Success" };

            }
            return new LinkGetSuccessResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Not Found" };
        }

        /// <summary>
        /// Get the customer contacts by zoho customerid
        /// </summary>
        /// <param name="zohoCustomerId"></param>
        /// <returns></returns>
        public async Task<object> GetCustomerContactByZohoCustomerId(long zohoCustomerId)
        {
            var customerContactList = await _repo.GetCustomerContactByZohoId(zohoCustomerId);

            if (customerContactList != null && customerContactList.Any())
            {
                var CustomerContactDetails = customerContactList.Select(x => _customermap.MapCustomerContactData(x)).ToList();

                return new LinkGetSuccessResponse() { Data = CustomerContactDetails, StatusCode = HttpStatusCode.OK, Message = "Success" };
            }

            return new LinkGetSuccessResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Not Found" };

        }

        public async Task<LinkErrorResponse> ValidateSaveZohoRequest(string customerId,
            string customerContactId, SaveZohoCrmCustomerContactDetails request, int requestType)
        {

            if (!string.IsNullOrEmpty(customerId))
            {
                var zohoCustomerId = long.Parse(customerId);
                var customer = await _customerRepo.GetCustomerByZohoId(zohoCustomerId);
                if (customer == null)
                {
                    return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest,
                     BadRequest, new List<string>() { "ZohoCustomerId Not Exists" });
                }
            }

            if (!string.IsNullOrEmpty(customerContactId))
            {
                var zohoCustomerContactId = long.Parse(customerContactId);
                var customerContact = await _repo.GetCustomerContactByZohoContactId(zohoCustomerContactId);

                if (requestType == (int)ZohoCustomerRequestEnum.Save)
                {
                    if (customerContact != null)
                    {

                        return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                            BadRequest, new List<string>() { "ZohoCustomerContact Already Exists" });
                    }
                }
                else if (requestType == (int)ZohoCustomerRequestEnum.Update && customerContact == null)
                {

                    return BuildCommonLinkErrorResponse(HttpStatusCode.BadRequest,
                        BadRequest, new List<string>() { "ZohoCustomerContact Already Exists" });

                }
            }
            return null;
        }

        //public async Task<LinkErrorResponse> ValidateUpdateZohoRequest(string customerId, string customerContactId, SaveZohoCrmCustomerContactDetails request)
        //{
        //    if (!string.IsNullOrEmpty(customerId))
        //    {
        //        var zohoCustomerIdValue = long.Parse(customerId);
        //        var customer = await _customerRepo.GetCustomerByZohoId(zohoCustomerIdValue);
        //        if (customer == null)
        //        {


        //            return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest,
        //           BadRequest, new List<string>() { "ZohoCustomerId Not Exists" });
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(customerContactId))
        //    {
        //        var zohoContactIdValue = long.Parse(customerContactId);
        //        var customerContact = await _repo.GetCustomerContactByZohoContactId(zohoContactIdValue);
        //        if (customerContact == null)
        //        {
        //            return BuildCommonLinkErrorResponse(System.Net.HttpStatusCode.BadRequest,
        //            BadRequest, new List<string>() { "ZohoCustomerContactId Not Found" });
        //        }
        //    }

        //    return null;
        //}

        /// <summary>
        /// Get the customer contact datasource list(it is generic function but currently using only for tcf system)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerContactDataSourceList(CustomerContactDataSourceRequest request)
        {
            var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            var data = _repo.GetCustomerContactDataSourceList();

            //filter the data
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.ContactName != null && EF.Functions.Like(x.ContactName, $"%{request.SearchText.Trim()}%"));
            }

            if (request.ContactIds != null && request.ContactIds.Any())
            {
                data = data.Where(x => request.ContactIds.Contains(x.Id));
            }

            if (request.ServiceId > 0)
            {
                data = data.Where(x => x.CuContactServices.Any(y => y.ServiceId == request.ServiceId));
            }

            if (request.CustomerIds != null && request.CustomerIds.Any())
            {
                data = data.Where(x => request.CustomerIds.Contains(x.CustomerId));
            }

            if (request.CustomerIds != null && request.CustomerIds.Any())
            {
                data = data.Where(x => request.CustomerIds.Contains(x.CustomerId));
            }

            if (request.CustomerGLCodes != null && request.CustomerGLCodes.Any())
            {
                data = data.Where(x => request.CustomerGLCodes.Contains(x.Customer.GlCode));
            }

            if (request.ContactTypeIds != null && request.ContactTypeIds.Any())
            {
                data = data.Where(x => x.CuCustomerContactTypes.Any(y => request.ContactTypeIds.Contains(y.ContactTypeId)));
            }

            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        data = data.Where(x => x.CustomerId == _ApplicationContext.CustomerId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        data = data.Where(x => x.Customer.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.SupplierId));
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        data = data.Where(x => x.Customer.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.FactoryId));
                        break;
                    }
            }

            //execute the data
            var contactList = await data.Skip(request.Skip).Take(request.Take).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.ContactName
            }).ToListAsync();

            //assign it it to datasource list
            if (contactList != null && contactList.Any())
            {
                response.DataSourceList = contactList;
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        //get the customer contact by booking Id
        public async Task<DataSourceResponse> GetCustomerContactByBooking(int bookingId)
        {
            var data = await _repo.GetCustomerContactByBooking(bookingId);

            if (data == null && !data.Any())
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
        /// Get Audit Customer Contacts by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerContactByBookingAndService(int bookingId, int serviceId)
        {
            List<CommonDataSource> data = null;

            if (serviceId == (int)Entities.Enums.Service.AuditId)
            {
                data = await _repo.GetAuditCustomerContactByBooking(bookingId);
            }
            else if (serviceId == (int)Entities.Enums.Service.InspectionId)
            {
                data = await _repo.GetCustomerContactByBooking(bookingId);
            }

            if (data == null && !data.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }

        public async Task<CustomerContactSummaryResponse> GetCustomerContactsByCustomerId(int customerId)
        {
            var customerContacts = await _repo.GetCustomerContactByCustomerId(customerId);
            if (customerContacts == null || !customerContacts.Any())
                return new CustomerContactSummaryResponse() { Result = CustomerContactSummaryResult.NotFound };

            var customerContactTypes = await _repo.GetCustomerContactTypesByContactIds(customerContacts.Select(x => x.Id));

            if (customerContactTypes.Any())
            {
                customerContacts.ForEach(x =>
                {
                    x.ContactTypeName = string.Join(", ", customerContactTypes.Where(y => y.ParentId == x.Id).Select(z => z.Name));
                });
            }

            return new CustomerContactSummaryResponse() { CustomerContacts = customerContacts, Result = CustomerContactSummaryResult.Success };
        }

        /// <summary>
        /// get customer contact name list
        /// </summary>
        /// <returns></returns>
        //public async Task<List<CommonDataSource>> GetCustomerContactNameList()
        //{
        //    return await _repo.GetCustomerContactNameList();
        //}

        public async Task<SaveUserResponse> CreateCustomerContactUserCredential(CustomerContactUserRequest request, CustomerContactCredentialsFrom contactrequestfrom)
        {
            if (request.ContactId > 0)
            {
                var customerContact = _repo.GetCustomerContactByContactID(request.ContactId);
                var entityService = customerContact.CuContactEntityServiceMaps?.ToList();
                if (entityService == null || !entityService.Any())
                    return new SaveUserResponse
                    {
                        errors = new List<string>() { "Select one service for this contact." },
                        Result = SaveResult.Failure
                    };
                var userAccountItem = new UserAccountItem()
                {
                    Fullname = request.Fullname,
                    PrimaryEntity = customerContact.PrimaryEntity,
                    UserTypeId = request.UserTypeId,
                    Contact = request.ContactId,
                    UserId = request.CustomerId,
                    UserName = request.UserName,
                    CreatedBy= request.CreatedBy
                };

                if (entityService != null && entityService.Any())
                {
                    var entityIds = entityService.Select(x => x.EntityId.Value).Distinct().ToList();
                    if (contactrequestfrom == CustomerContactCredentialsFrom.ContactPage)
                    {
                        userAccountItem.UserRoleEntityList = new List<UserRoleEntity>()
                            {
                                new UserRoleEntity { RoleId = (int)RoleEnum.Customer, RoleEntity = entityIds },
                                new UserRoleEntity { RoleId = (int)RoleEnum.QuotationConfirmation, RoleEntity = entityIds },
                                new UserRoleEntity { RoleId = (int)RoleEnum.EditInspectionCustomerDecision, RoleEntity = entityIds }

                            };
                        var tcfEntityIds = entityService.Where(x => x.ServiceId == (int)Service.Tcf)?.Select(x => x.EntityId.Value).Distinct().ToList();
                        if (tcfEntityIds != null && tcfEntityIds.Any())
                        {
                            var tcfCustomerRole = new UserRoleEntity { RoleId = (int)RoleEnum.TCFCustomer, RoleEntity = tcfEntityIds };
                            userAccountItem.UserRoleEntityList.Add(tcfCustomerRole);
                        }
                    }
                    else if(contactrequestfrom == CustomerContactCredentialsFrom.EAQF)
                    {
                        userAccountItem.UserRoleEntityList = new List<UserRoleEntity>()
                            {
                                new UserRoleEntity { RoleId = (int)RoleEnum.Customer, RoleEntity = entityIds },

                            };
                    }


                }

                var response = await _userAccountManager.AddUserDetails(userAccountItem);
                return response;
            }
            else
            {
                return new SaveUserResponse() { Result = SaveResult.CurrentUserAccountNotFound };
            }
        }
    }
}
