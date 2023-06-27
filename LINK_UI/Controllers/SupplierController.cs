using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.EmailLog;
using DTO.Location;
using DTO.Master;
using DTO.MasterConfig;
using DTO.Supplier;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierManager _manager = null;
        private readonly ILocationManager _locationManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private static IConfiguration _configuration = null;
        private readonly ISharedInspectionManager _helper = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly ITenantProvider _tenantProvider;

        public SupplierController(ISupplierManager manager, ILocationManager locManager, IRabbitMQGenericClient rabbitMQClient, IConfiguration configuration,
            ISharedInspectionManager helper, IEmailLogQueueManager emailLogQueueManager, IInspectionBookingManager inspManager,
            ITenantProvider tenantProvider)
        {
            _manager = manager;
            _locationManager = locManager;
            _rabbitMQClient = rabbitMQClient;
            _configuration = configuration;
            _helper = helper;
            _emailLogQueueManager = emailLogQueueManager;
            _inspManager = inspManager;
            _tenantProvider = tenantProvider;
        }


        [HttpGet()]
        [Right("supplier-summary")]
        public async Task<SupplierSummaryResponse> GetSupplierSummary()
        {
            var response = await _manager.GetSupplierSummary();

            return response;
        }

        [Right("supplier-summary")]
        [HttpPost("export")]
        public async Task<IActionResult> ExportSummary([FromBody] SupplierSearchRequestNew request)
        {
            var response = await _manager.GetSupplierSummaryExportDetails(request);
            if (response == null || response.Result == SupplierSearchResult.NotFound)
                return NotFound();
            var stream = _helper.GetAsStreamObject(response.SupplierExportList);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "supplier.xlsx");
        }

        [HttpPost("[action]")]
        [Right("supplier-summary")]
        public async Task<SupplierSearchItemResponse> Search([FromBody] SupplierSearchRequestNew request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null || request.pageSize == 0)
                request.pageSize = 10;

            return await _manager.GetSupplierSearchData(request);
        }


        [Right("supplier-summary")]
        [HttpGet("GetSupplierChild/{id}/{type}")]
        public async Task<SupplierSearchItemResponse> SupplierChild(int id, int type)
        {
            return await _manager.GetSupplierSearchChildData(id, type);
        }

        [HttpGet("delete/{id}")]
        [Right("edit-supplier")]
        public async Task<SupplierDeleteResponse> DeleteSupplier(int id)
        {
            return await _manager.DeleteSupplier(id);
        }


        [Right("supplier-summary")]
        [HttpGet("edit/{id}")]
        public async Task<EditSupplierResponse> GetSupplierDetails(int id)
        {
            return await _manager.GetEditSupplier(id);
        }


        [Right("new-supplier")]
        [HttpGet("add")]
        public async Task<EditSupplierResponse> Add()
        {
            return await _manager.GetEditSupplier(null);
        }

        [Right("edit-supplier")]
        [HttpPost("save")]
        public async Task<SaveSupplierResponse> Save([FromBody] SupplierDetails request, [FromServices] IConfiguration configuration)
        {
            var response = await _manager.Save(request);
            if (response.Id > 0)
            {

                if (request.TypeId == (int)Supplier_Type.Supplier_Agent)
                {
                    //default set as create supplier
                    MasterDataType masterDataType = MasterDataType.SupplierCreation;
                    UpdateSupplierDetailsToFB(request.ApiServiceIds, response.Id, masterDataType);

                    UpdateSupplierDetailsToTCF(request.ApiServiceIds, response.Id, masterDataType);

                    if (request.IsNewSupplier && request.Id == 0 && response.ParentId.HasValue)
                    {
                        masterDataType = MasterDataType.FactoryCreation;
                        UpdateSupplierDetailsToFB(request.ApiServiceIds, response.ParentId.Value, masterDataType);

                        //UpdateSupplierDetailsToTCF(request.ApiServiceIds, response.ParentId.Value, masterDataType);
                    }
                }
                else if (request.TypeId == (int)Supplier_Type.Factory)
                {
                    if (request.IsNewSupplier && request.Id == 0 && response.ParentId.HasValue)
                    {
                        UpdateSupplierDetailsToFB(request.ApiServiceIds, response.ParentId.Value, MasterDataType.SupplierCreation);

                        UpdateSupplierDetailsToTCF(request.ApiServiceIds, response.ParentId.Value, MasterDataType.SupplierCreation);
                    }

                    var masterDataType = (request.Id > 0) ? MasterDataType.FactoryUpdation : MasterDataType.FactoryCreation;
                    UpdateSupplierDetailsToFB(request.ApiServiceIds, response.Id, masterDataType);

                    //UpdateSupplierDetailsToTCF(request.ApiServiceIds, response.Id, masterDataType);
                }
            }

            if (response.FactoryResult == SaveSupplierResult.FactoryCountyTownNotFound && response.Id > 0)
            {
                await FactorySendMail(response, configuration);
            }

            return response;
        }

        /// <summary>
        /// factory address county or town data missing if china has a country, will send mail
        /// </summary>
        /// <param name="response"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private async Task FactorySendMail(SaveSupplierResponse response, IConfiguration configuration)
        {
            try
            {
                var masterConfigs = await _inspManager.GetMasterConfiguration();
                var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                string baseUrl = _configuration["BaseUrl"];

                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };

                var emailLogRequest = new EmailLogData()
                {
                    ToList = (response.ToEmailList != null && response.ToEmailList.Any()) ? response.ToEmailList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "Save Booking",
                    SourceId = response.Id,
                    Subject = $"Factory {response.FactoryName} Info missing"
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingFactory", (response, baseUrl + string.Format(configuration["UrlBookingRequest"], response.Id, entityName)));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save email data into log table and publish to queue
        /// </summary>
        /// <param name="emailQueueRequest"></param>
        /// <param name="emailLogRequest"></param>
        /// <returns></returns>
        private async Task PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest)
        {
            var resultId = await _emailLogQueueManager.AddEmailLog(emailLogRequest);
            emailQueueRequest.EmailQueueId = resultId;
            await _rabbitMQClient.Publish<EmailDataRequest>(_configuration["EmailQueue"], emailQueueRequest);
        }

        /// <summary>
        /// Add or Update the supplier details
        /// </summary>
        /// <param name="apiServiceIds"></param>
        /// <param name="supplierId"></param>
        /// <param name="masterDataMap"></param>
        private async void UpdateSupplierDetailsToFB(IEnumerable<int?> apiServiceIds, int supplierId, MasterDataType masterDataMap)
        {
            bool isTCFSupplier = false;
            if (apiServiceIds != null && apiServiceIds.Count() == 1 && apiServiceIds.Contains((int)Service.Tcf))
                isTCFSupplier = true;

            //push the supplier account to FB if selected api service is not TCF
            if (!isTCFSupplier)
            {
                var fbSupplierRequest = new MasterDataRequest()
                {
                    Id = Guid.NewGuid(),
                    SearchId = supplierId,
                    ExternalClient = ExternalClient.FullBridge,
                    MasterDataType = masterDataMap,
                    EntityId = _tenantProvider.GetCompanyId()
                };
                await _rabbitMQClient.Publish<MasterDataRequest>(_configuration["AccountQueue"], fbSupplierRequest);
            }
        }

        private async void UpdateSupplierDetailsToTCF(IEnumerable<int?> apiServiceIds, int supplierId, MasterDataType masterDataMap)
        {
            bool isTCFSupplier = false;
            if (apiServiceIds != null && apiServiceIds.Contains((int)Service.Tcf))
                isTCFSupplier = true;

            //push the supplier account to FB if selected api service is not TCF
            if (isTCFSupplier)
            {
                var tcfSupplierRequest = new MasterDataRequest()
                {
                    Id = Guid.NewGuid(),
                    SearchId = supplierId,
                    ExternalClient = ExternalClient.TCF,
                    MasterDataType = masterDataMap
                };
                await _rabbitMQClient.Publish<MasterDataRequest>(_configuration["AccountQueue"], tcfSupplierRequest);
            }
        }

        [Right("supplier-summary")]
        [HttpGet("states/{id}")]
        public StatesResponse GetStates(int id)
        {
            return _locationManager.GetStates(id);
        }

        [Right("supplier-summary")]
        [HttpGet("cities/{id}")]
        public CitiesResponse GetCities(int id)
        {
            return _locationManager.GetCities(id);
        }

        [HttpGet("GetsupplierBycustomerid/{cusid}")]
        [Right("supplier-summary")]
        public async Task<SupplierListResponse> GetsupplierBycustomerid(int cusid, bool isBookingRequest = false)
        {
            return await _manager.GetSuppliersByUserType(cusid, isBookingRequest);
        }

        [HttpGet("GetfactoryBysupid/{supid}")]
        [Right("supplier-summary")]
        public async Task<SupplierListResponse> GetfactoryBysupid(int supid)
        {
            return await _manager.GetFactorysByUserType(null, supid);
        }

        [HttpGet("GetSupplierByName/{supName}/{type}")]
        [Right("supplier-summary")]
        public async Task<SupplierListResponse> GetSupplierByName(string supName, int type)
        {
            return await _manager.GetSupplierByName(supName, type);
        }
        [HttpGet("GetSupplierHeadOfficeAddress/{supId}")]
        [Right("supplier-summary")]
        public async Task<SupplierAddress> GetSupplierHeadOfficeAddress(int supId)
        {
            return await _manager.GetSupplierHeadOfficeAddress(supId);
        }

        [HttpGet("GetfactoryBycustomeridsupId/{cusid}/{supid}")]
        [Right("supplier-summary")]
        public async Task<SupplierListResponse> GetfactoryBycustomeridsupid(int? cusid, int supid)
        {
            return await _manager.GetFactorysByUserType(cusid, supid);
        }

        [HttpPost("getfactorydatasource")]
        [Right("supplier")]
        public async Task<DataSourceResponse> GetSupplierDataSource(CommonDataSourceRequest request)
        {
            return await _manager.GetSupplierDataSource(request);
        }

        [HttpPost("getSupplierorFactoryList")]
        [Right("supplier")]
        public async Task<DataSourceResponse> GetSupplierorFactoryList(CommonSupplierSourceRequest request)
        {
            return await _manager.GetFactoryOrSupplierList(request);
        }

        [HttpPost("GetSupplierDataSourceList")]
        [Right("supplier")]
        public async Task<DataSourceResponse> GetSupplierDataSourceList(SupplierDataSourceRequest request)
        {
            return await _manager.GetSupplierDataSourceList(request);
        }

        [HttpGet("GetSupplierContactByBooking/{bookingId}/{supType}/{serviceType}")]
        public async Task<DataSourceResponse> GetSupplierContactByBooking(int bookingId, int supType, int serviceType)
        {
            return await _manager.GetSupplierContactByBooking(bookingId, supType, serviceType);
        }

        [HttpPost("supplier-factory-data")]
        [Right("supplier")]
        public async Task<DataSourceResponse> GetFactoryDataSourceBySupplier(CommonDataSourceRequest request)
        {
            return await _manager.GetFactoryDataSourceBySupplier(request);
        }


        [HttpPost("GetSupplierByCountryDatasource")]
        public async Task<List<CommonDataSource>> GetSupplierByCountryDatasource(CommonSupplierSourceRequest request)
        {
            return await _manager.GetSupplierByCountryDataSourceNew(request);
        }

        [HttpPost("GetSupplierList")]
        public async Task<DataSourceResponse> GetSupplierDatasourceWithoutDependency(CommonDataSourceRequest request)
        {
            return await _manager.GetSupplierList(request);
        }

        [HttpPost("GetFactoryList")]
        public async Task<DataSourceResponse> GetFactoryrList(CommonDataSourceRequest request)
        {
            return await _manager.GetFactoryrList(request);
        }

        [HttpGet("getsupplierFactorAddress/{id}")]
        public async Task<AddressDataSourceResponse> GetSupplierFactoryAddress(int id)
        {
            return await _manager.GetSupplierFactorAddressById(id);
        }

        [HttpGet("get-base-supplier-contact-data/{id}")]
        public async Task<SupplierContactDataResponse> GetBaseSupplierContactData(int id)
        {
            return await _manager.GetBaseSupplierContactData(id);
        }

        [HttpPost("getExistSupplierDetails")]
        [Right("supplier")]
        public async Task<SupplierDataResponse> GetExistSupplierDetails(SupplierDetails request)
        {
            return await _manager.GetExistSupplierDetails(request);
        }

        [HttpPost("addEntityIntoSupplier")]
        public async Task<SaveSupplierResponse> UpdateEntityIntoSupplier([FromBody] SupplierData request)
        {
            var response = await _manager.UpdateSupplierEntity(request);
            return response;
        }

        /// <summary>
        /// get the supplier level by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("getSupplierLevelByCustomerId/{customerId}")]
        public async Task<DataSourceResponse> GetSupplierLevelByCustomerId(int customerId)
        {
            return await _manager.GetSupplierLevelByCustomerId(customerId);
        }

        [HttpPost("gradeBySupplierIdAndCustomerIdAndBookingIds")]
        public async Task<SupplierGradeResponse> GetGradeBySupplierIdAndCustomerIdAndBookingIds(SupplierGradeRequest request)
        {
            return await _manager.GetGradeBySupplierIdAndCustomerIdAndBookingIds(request);
        }
    }
}