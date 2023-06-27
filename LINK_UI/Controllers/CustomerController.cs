using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Customer;
using DTO.FullBridge;
using DTO.HumanResource;
using DTO.Master;
using DTO.TCF;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQUtility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager _manager = null;
        private readonly IHumanResourceManager _hrManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private static IConfiguration _configuration = null;
        private readonly IEventBookingLogManager _eventLog = null;
        private readonly ITenantProvider _tenantProvider;

        public CustomerController(ICustomerManager manager, IHumanResourceManager hrManager, IRabbitMQGenericClient rabbitMQClient,
            IConfiguration configuration, IEventBookingLogManager eventLog, ITenantProvider tenantProvider)
        {
            _manager = manager;
            _hrManager = hrManager;
            _rabbitMQClient = rabbitMQClient;
            _configuration = configuration;
            _eventLog = eventLog;
            _tenantProvider = tenantProvider;
        }

        [HttpGet()]
        [Right("customer-summary")]
        public async Task<CustomerSummaryResponse> GetCustomerSummary()
        {
            var response = await _manager.GetCustomerSummary();
            return response;
        }
        [HttpGet("customergroup")]
        [Right("customer-summary")]
        public async Task<CustomerGroupResponse> GetCustomerGroup()
        {
            var response = await _manager.GetCustomerGroup();
            return response;
        }
        [HttpGet("language")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetLanguage()
        {
            var response = await _manager.GetLanguage();
            return response;
        }
        [HttpGet("prospectstatus")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetProspectStatus()
        {
            var response = await _manager.GetProspectStatus();
            return response;
        }
        [HttpGet("marketsegment")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetMarketSegment()
        {
            var response = await _manager.GetMarketSegment();
            return response;
        }
        [HttpGet("businesstype")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetBusinessType()
        {
            var response = await _manager.GetBusinessType();
            return response;
        }
        [HttpGet("addresstype")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetAddressType()
        {
            var response = await _manager.GetAddressType();
            return response;
        }
        [HttpGet("invoicetype")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetInvoiceType()
        {
            var response = await _manager.GetInvoiceType();
            return response;
        }

        [HttpGet("accountingleader")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetAccountingLeader()
        {
            var response = await _manager.GetAccountingLeader();
            return response;
        }

        [HttpGet("getKAMStaffDetails")]
        [Right("customer-summary")]
        ////public StaffKAMProfileResponse GetHRStaffWithKAMProfiles()
        ////{
        ////    return _hrManager.GetHRStaffWithKAMProfiles();
        ////}
        public async Task<DataSourceResponse> GetKAMStaff()
        {
            var response = await _hrManager.GetKAMStaff();
            return response;
        }

        [HttpGet("salesincharge/{departname}")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetSalesIncharge(string departname)
        {
            List<int?> departmentIds;
            ////departmentIds = new List<int?>() { 5,14 };
            List<int> departIds = await _hrManager.GetDepartmentIdsByName(departname);
            departmentIds = departIds.ConvertAll<int?>(x => (int?)x);
            var response = await _hrManager.GetSalesIncharge(departmentIds);
            return response;
        }

        [HttpGet("activitieslevel")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetActivitiesLevel()
        {
            var response = await _manager.GetActivitiesLevel();
            return response;
        }

        [HttpGet("relationshipstatus")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetRelationshipStatus()
        {
            var response = await _manager.GetRelationshipStatus();
            return response;
        }

        [HttpGet("brandpriority")]
        [Right("customer-summary")]
        public async Task<CustomerSourceResponse> GetBrandPriority()
        {
            var response = await _manager.GetBrandPriority();
            return response;
        }


        [HttpGet("getcustomerbyid/{id}")]
        [Right("customer-summary")]
        public async Task<CustomerSummaryResponse> GetCustomerbyId(int? id)
        {
            var response = await _manager.GetCustomerbyId(id);
            return response;
        }
        [HttpGet("getcustomercontactsummary/id")]
        [Right("customer-summary")]
        public async Task<CustomerSummaryResponse> GetCustomerContactSummary(int id)
        {
            var response = await _manager.GetCustomerSummary();
            return response;
        }

        [HttpPost("search")]
        [Right("customer-summary")]
        public CustomerSearchResponse CustomerSearch([FromBody] CustomerSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null || request.pageSize == 0)
                request.pageSize = 10;
            return _manager.GetCustomerData(request);
        }


        [HttpPost("save")]
        [Right("customer-summary")]
        public async Task<SaveCustomerResponse> Save([FromBody] CustomerDetails request)
        {
            request.isCallFrom = (int)CustomerModuleEnum.API;
            var response = await _manager.Save(request);

            if (response.Id > 0)
            {
                //add the customer details
                UpdateCustomerToFB(request.ApiServiceIds, response.Id, MasterDataType.CustomerCreation);

                await UpdateCustomerDetailsToTCF(request.ApiServiceIds, response.Id, MasterDataType.CustomerCreation);
            }
            return response;
        }

        private async Task UpdateCustomerDetailsToTCF(IEnumerable<int?> apiServiceIds, int customerId, MasterDataType masterDataMap)
        {
            bool isTCFCustomer = false;
            if (apiServiceIds != null && apiServiceIds.Contains((int)Service.Tcf))
                isTCFCustomer = true;

            //push the customer account to FB if selected api service is not TCF
            if (isTCFCustomer)
            {
                var tcfCustomerRequest = new MasterDataRequest()
                {
                    Id = Guid.NewGuid(),
                    SearchId = customerId,
                    ExternalClient = ExternalClient.TCF,
                    MasterDataType = masterDataMap,
                    EntityId = _tenantProvider.GetCompanyId()
                };
                var logResponse = await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                {
                    AccountId = customerId,
                    DataType = (int)TCFDataType.Customer,
                    //LogInformation= JsonConvert.SerializeObject(tcfCustomerRequest),
                    ResponseMessage = "Before Calling the queue"
                });
                await _rabbitMQClient.Publish<MasterDataRequest>(_configuration["AccountQueue"], tcfCustomerRequest);
            }
            else
            {
                await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                {
                    AccountId = customerId,
                    DataType = (int)TCFDataType.Customer,
                    ResponseMessage = "TCF Service Not Enabled"
                });
            }
        }

        /// <summary>
        /// Add or Update the customer details to fullbridge
        /// </summary>
        /// <param name="apiServiceIds"></param>
        /// <param name="customerId"></param>
        /// <param name="masterDataMap"></param>
        private async void UpdateCustomerToFB(IEnumerable<int?> apiServiceIds, int customerId, MasterDataType masterDataMap)
        {
            bool isTCFCustomer = false;
            if (apiServiceIds.Count() == 1 && apiServiceIds.Contains((int)Service.Tcf))
                isTCFCustomer = true;



            //push the customer account to FB if selected api service is TCF
            if (apiServiceIds != null && !isTCFCustomer)
            {
                var fbCustomerRequest = new MasterDataRequest()
                {
                    Id = Guid.NewGuid(),
                    SearchId = customerId,
                    ExternalClient = ExternalClient.FullBridge,
                    MasterDataType = masterDataMap,
                    EntityId = _tenantProvider.GetCompanyId()
                };
                await _rabbitMQClient.Publish<MasterDataRequest>(_configuration["AccountQueue"], fbCustomerRequest);
            }
        }

        [HttpGet("edit/{id}")]
        [Right("customer-summary")]
        public async Task<EditCustomerResponse> GetCustomerDetails(int id)
        {
            return await _manager.GetEditCustomer(id);
        }

        [HttpGet("delete/{id}")]
        [Right("customer-summary")]
        public async Task<CustomerDeleteResponse> DeleteCustomer(int id)
        {
            return await _manager.DeleteCustomer(id);
        }

        [HttpGet("GetCustomerByUsertType")]
        [Right("customer-summary")]
        public async Task<CustomerSummaryResponse> GetCustomerByUsertType()
        {
            return await _manager.GetCustomersByUserType();
        }


        [HttpGet("getCustomerByCheckPointUsertType")]
        [Right("customer-summary")]
        public async Task<CustomerSummaryResponse> GetCustomerByCheckPointUsertType()
        {
            return await _manager.GetCustomerByCheckPointUsertType();
        }
        [HttpGet("getcustomerbrands/{id}")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerBrands(int id)
        {
            var response = await _manager.GetCustomerBrands(id);
            return response;
        }

        [HttpGet("getcustomerdepartments/{id}")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerDepartments(int id)
        {
            var response = await _manager.GetCustomerDepartments(id);
            return response;
        }

        [HttpGet("getcustomercontactList/{id}")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerContacts(int id)
        {
            var response = await _manager.GetCustomerContactsbyCustomer(id);
            return response;
        }


        [HttpGet("getcustomerAddress/{id}")]
        [Right("customer-summary")]
        public async Task<AddressDataSourceResponse> GetCustomerContactList(int id)
        {
            var response = await _manager.GetCustomerAddressbyCustomer(id);
            return response;
        }

        [HttpGet("getcustomerbuyers/{id}")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerBuyers(int id)
        {
            var response = await _manager.GetCustomerBuyers(id);
            return response;
        }

        [HttpGet("getcustomerproductcategory/{id}")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerProductCategoryList(int id)
        {
            var response = await _manager.GetCustomerProductCategoryList(id);
            return response;
        }

        [HttpPost("getcustomerproductsubcategory")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerProductSubCategoryList(CustomerSubCategory customerSubCategory)
        {
            var response = await _manager.GetCustomerProductSubCategoryList(customerSubCategory.CustomerId, customerSubCategory.ProductCategory);
            return response;
        }


        [HttpPost("getcustomerproductsubcategoryList")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerProductSub2CategoryList(CustomerSubCategory2Request customerSubCategory)
        {
            var response = await _manager.GetCustomerProductSub2CategoryList(customerSubCategory);
            return response;
        }

        [HttpGet("getcustomerpricecategory/{id}")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerPriceCategory(int id)
        {
            var response = await _manager.GetCustomerPriceCategory(id);
            return response;
        }


        [HttpGet("getcustomerpricedata/{id}")]
        [Right("customer-summary")]
        public async Task<CustomerPriceData> GetCustomerPriceData(int id)
        {
            var response = await _manager.GetCustomerPriceData(id);
            return response;
        }

        [HttpGet("GetCustomerByName/{name}")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerByName(string name)
        {
            return await _manager.GetCustomerByName(name);
        }

        [HttpGet("GetCustomerByCustomerId/{customerId}")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCustomerById(int customerId)
        {
            return await _manager.GetCustomerByCustomerId(customerId);
        }

        [HttpPost("GetCustomerDatasource")]
        public async Task<DataSourceResponse> GetCustomerDataSource(CommonDataSourceRequest request)
        {
            return await _manager.GetCustomerDataSource(request);
        }

        #region Price Category

        [HttpPost("price-category-by-customer")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetPriceCategoryDataSource(CommonCustomerSourceRequest request)
        {
            return await _manager.GetPriceCategoryDataSource(request);
        }

        #endregion

        [HttpPost("GetCustomerDataSourceList")]
        public async Task<CustomerGLCodeResponse> GetCustomerGLCodeSourceList(CustomerDataSourceRequest request)
        {
            return await _manager.GetCustomerGLCodeSourceList(request);
        }

        [HttpPost("get-customer-by-supplier")]
        public async Task<CustomerDataSourceResponse> GetCustomerDataSourceBySupplier(CommonDataSourceRequest request)
        {
            return await _manager.GetCustomerDataSourceBySupplier(request);
        }

        [HttpGet("GetCustomerKAMDetails/{customerId}")]
        public async Task<CustomerKamDetail> GetCustomerKAMDetails(int customerId)
        {
            return await _manager.GetCustomerKAMDetails(customerId);
        }

        [HttpGet("get-customer-product-category/{customerId}")]
        public async Task<DataSourceResponse> GetCustomerProductCategories(int customerId)
        {
            return await _manager.GetCustomerProductCategories(customerId);
        }

        [HttpGet("get-season-config/{customerId}")]
        public async Task<DataSourceResponse> GetCustomerSeasonConfiguration(int customerId)
        {
            return await _manager.GetCustomerSeasonConfiguration(customerId);
        }

        [HttpGet("get-customer-entitylist/{customerId}")]
        public async Task<DataSourceResponse> GetCustomerEntityList(int customerId)
        {
            return await _manager.GetCustomerEntityList(customerId);
        }

        [HttpGet("get-customer-sisterCompany/{customerId}")]
        public async Task<DataSourceResponse> GetCustomerSisterCompany(int customerId)
        {
            return await _manager.GetCustomerSisterCompany(customerId);
        }

        [HttpPost("get-customer-by-userType")]
        public async Task<DataSourceResponse> GetCustomerByUserType(CommonDataSourceRequest request)
        {
            return await _manager.GetCustomerByUserType(request);
        }

        [HttpGet("get-customer-contacts-address-list/{customerId}")]
        [Right("edit-booking")]
        public Task<CustomerContactAddressDetails> GetCustomerContactsAndAddressList(int customerId)
        {
            return _manager.GetCustomerContactAddressDetails(customerId);
        }
    }
}
