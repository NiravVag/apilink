using Components.Core.contracts;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Customer;
using DTO.EmailLog;
using DTO.File;
using DTO.Invoice;
using DTO.MasterConfig;
using DTO.Quotation;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class QuotationController : ControllerBase
    {
        private readonly IQuotationManager _quotationManager = null;
        private readonly ISharedInspectionManager _helper = null;
        private readonly IConfiguration _configuration = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private readonly IEmailManager _emailManager;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly ITenantProvider _filterService = null;
        public QuotationController(IQuotationManager quotationManager, ISharedInspectionManager helper, IConfiguration configuration,
            IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IEmailManager emailManager, IInspectionBookingManager inspManager, ITenantProvider filterService)
        {
            _quotationManager = quotationManager;
            _helper = helper;
            _configuration = configuration;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _emailManager = emailManager;
            _inspManager = inspManager;
            _filterService = filterService;
        }

        [HttpGet("quotation-summary")]
        [Right("supplier-summary")]
        public async Task<QuotationSummaryResponse> GetQuotationSummary()
        {
            return await _quotationManager.GetQuotationSummary();
        }

        [HttpPost("quotation-list")]
        [Right("supplier-summary")]
        public async Task<QuotationDataSummaryResponse> GetQuotationList([FromBody] QuotationSummaryGenRequest request)
        {
            // Get Quotation List
            return await _quotationManager.GetQuotationList(request);
        }



        [HttpGet()]
        [Right("supplier-summary")]
        public async Task<QuotationResponse> GetQuotation()
        {
            return await _quotationManager.GetQuotation(null);
        }

        [HttpGet("{id}")]
        [Right("supplier-summary")]
        public async Task<QuotationResponse> GetQuotation(int id)
        {
            return await _quotationManager.GetQuotation(id);
        }

        [HttpGet("customer-list/{countryId}/{serviceId}")]
        [Right("supplier-summary")]
        public async Task<QuotationDataSourceResponse> GetCustomerList(int countryId, int serviceId)
        {
            if (countryId <= 0)
                return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.CountryEmpty };
            if (serviceId <= 0)
                return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.ServiceEmpty };

            return await _quotationManager.GetCustomerList(countryId, serviceId);
        }

        [HttpGet("supplier-list/{customerId}")]
        [Right("supplier-summary")]
        public async Task<QuotationDataSourceResponse> GetSupplierList(int customerId)
        {
            if (customerId <= 0)
                return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.CustomerEmpty };

            return await _quotationManager.GetSupplierList(customerId);
        }

        [HttpGet("cust-contact-list/{customerId}")]
        [Right("supplier-summary")]
        public async Task<QuotationContactListResponse> GetCustomerContactList(int customerId)
        {
            if (customerId <= 0)
                return new QuotationContactListResponse { Result = QuotationContactListResult.CustomerEmpty };

            return await _quotationManager.GetCustomerContactList(customerId);
        }

        [HttpGet("factory-list/{supplierId}")]
        [Right("supplier-summary")]
        public async Task<QuotationDataSourceResponse> GetFactoryList(int supplierId)
        {
            if (supplierId <= 0)
                return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.CustomerEmpty };

            return await _quotationManager.GetFactoryList(supplierId);
        }

        [HttpGet("supp-contact-list/{supplierId}/{customerId}")]
        [Right("supplier-summary")]
        public async Task<QuotationContactListResponse> GetSupplierContactList(int supplierId, int customerId)
        {
            if (supplierId <= 0)
                return new QuotationContactListResponse { Result = QuotationContactListResult.SupplierEmpty };

            if (customerId <= 0)
                return new QuotationContactListResponse { Result = QuotationContactListResult.CustomerEmpty };

            return await _quotationManager.GetSupplierContactList(supplierId, customerId);
        }

        [HttpGet("fact-contact-list/{factoryId}/{customerId}")]
        [Right("supplier-summary")]
        public async Task<QuotationContactListResponse> GetFactoryContactList(int factoryId, int customerId)
        {
            if (factoryId <= 0)
                return new QuotationContactListResponse { Result = QuotationContactListResult.FactoryEmpty };

            if (customerId <= 0)
                return new QuotationContactListResponse { Result = QuotationContactListResult.CustomerEmpty };

            return await _quotationManager.GetFactoryContactList(factoryId, customerId);
        }


        [HttpGet("intern-contact-list/{locationId}/{customerId}")]
        [Right("supplier-summary")]
        public async Task<QuotationContactListResponse> GetInternalContactList(int locationId, int customerId)
        {
            if (customerId <= 0)
                return new QuotationContactListResponse { Result = QuotationContactListResult.CustomerEmpty };
            if (locationId <= 0)
                return new QuotationContactListResponse { Result = QuotationContactListResult.officeIsEmpty };

            return await _quotationManager.GetInternalContactList(locationId, customerId);
        }


        [HttpPost("order-list")]
        [Right("supplier-summary")]
        public async Task<QuotationOrderListResponse> FilterOrderList([FromBody] FilterOrderRequest request)
        {
            return await _quotationManager.GetOrders(request);
        }

        [HttpPut()]
        [Right("supplier-summary")]
        public async Task<SaveQuotationResponse> Save([FromBody] QuotationDetails request)
        {

            // check booking status is hold or cancelled
            var masterConfigs = await _inspManager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            string baseUrl = _configuration["BaseUrl"];

            if (request.Id > 0)
            {
                var statusList = await _quotationManager.GetBookingStatusList(request.Id);

                if (statusList.Contains((int)BookingStatus.Cancel))
                {
                    return new SaveQuotationResponse() { Result = SaveQuotationResult.BookingIsCancelled };
                }

                if (statusList.Contains((int)BookingStatus.Hold))
                {
                    return new SaveQuotationResponse() { Result = SaveQuotationResult.BookingIsHold };
                }
            }

            var saveQuotatioNRequest = new SaveQuotationRequest
            {
                Model = request,
                Url = request.Id <= 0 ? baseUrl + _configuration["UrlQuotation"] : baseUrl + string.Format(_configuration["UrlQuotation"], request.Id, entityName),
                OnSendEmail = (emailRequest) =>
                {
                    if (request.Id <= 0)
                    {
                        if (request.IsToForward)
                            this.SendEmail("Emails/Quotation/NewQuotationVerified", emailRequest, baseUrl, entityName);
                        else
                            this.SendEmail("Emails/Quotation/NewQuotationConfirmed", emailRequest, baseUrl, entityName);
                    }
                    else
                    {

                        if (request.StatusId == QuotationStatus.QuotationVerified && request.IsToForward)
                            this.SendEmail("Emails/Quotation/NewQuotationVerified", emailRequest, baseUrl, entityName);
                        else if (request.StatusId == QuotationStatus.AERejected || request.StatusId == QuotationStatus.ManagerRejected
                        //|| request.StatusId == QuotationStatus.CSRejectedAfterCustomerRejected
                        )
                        {
                            if (request.IsToForward)
                                this.SendEmail("Emails/Quotation/NewQuotationVerified", emailRequest, baseUrl, entityName);
                            else
                                this.SendEmail("Emails/Quotation/NewQuotationConfirmed", emailRequest, baseUrl, entityName);
                        }
                    }
                },
                FactoryBookingInfoList = request.FactoryBookingInfoList
            };

            if (request.IsToForward && request.SkipQuotationSentToClient)
            {
                return new SaveQuotationResponse() { Result = SaveQuotationResult.SkipSentToClientAndIsForwardToSelected };
            }

            return await _quotationManager.SaveQuotation(saveQuotatioNRequest);
        }


        [HttpGet("factory-address/{idFactory}")]
        [Right("supplier-summary")]
        public async Task<AddressFactoryResponse> GetFactoryAddress(int idFactory)
        {

            string address = await _quotationManager.GetFactoryAddress(idFactory);

            if (!string.IsNullOrEmpty(address))
                return new AddressFactoryResponse { Address = address, Result = AddressFactoryResult.Success };

            return new AddressFactoryResponse { Result = AddressFactoryResult.NotFound };
        }


        private void SendEmail(string ViewPath, SendEmailRequest request, string baseUrl, string entityName)
        {
            string url = baseUrl + string.Format(_configuration["UrlQuotationSummary"], request.Model.Id, entityName);

            var entityId = _filterService.GetCompanyId();
            var _settings = _emailManager.GetMailSettingConfiguration(entityId);
            request.SenderEmail = _settings.SenderEmail;

            var emailQueueRequest = new EmailDataRequest
            {
                TryCount = 1,
                Id = Guid.NewGuid()
            };

            var emailLogRequest = new EmailLogData()
            {
                ToList = (request.RecepientList != null && request.RecepientList.Count() > 0) ? request.RecepientList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                Cclist = (request.CcList != null && request.CcList.Count() > 0) ? request.CcList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                TryCount = 1,
                Status = (int)EmailStatus.NotStarted,
                SourceName = "Quotation Page",
                FileList = request.FileList,
                SourceId = request.Model.Id,
                Subject = request.Subject
            };

            emailLogRequest.Body = this.GetEmailBody(ViewPath, (request.Model, request.RecepitName, url));
            PublishQueueMessage(emailQueueRequest, emailLogRequest);
        }


        [HttpPost("status")]
        [Right("supplier-summary")]
        public async Task<SetStatusQuotationResponse> SetStatus([FromBody] SetStatusRequest request)
        {
            var masterConfigs = await _inspManager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            string baseUrl = _configuration["BaseUrl"];

            // check booking status is hold or cancelled
            if (request.Id > 0 && request.StatusId != (int)QuotationStatus.Canceled)
            {
                var statusList = await _quotationManager.GetBookingStatusList(request.Id);

                if (statusList.Contains((int)BookingStatus.Cancel))
                {
                    return new SetStatusQuotationResponse() { Result = SetStatusQuotationResult.BookingIsCancelled };
                }

                if (statusList.Contains((int)BookingStatus.Hold))
                {
                    return new SetStatusQuotationResponse() { Result = SetStatusQuotationResult.BookingIsHold };
                }
            }


            if (request.Id <= 0)
                return new SetStatusQuotationResponse { Result = SetStatusQuotationResult.QuotationNotFound };

            var actionSendList = new Dictionary<QuotationStatus, Action<SendEmailRequest>>()
            {
                { QuotationStatus.ManagerApproved, (emailRequest) =>  this.SendEmail("Emails/Quotation/QuotationApproved", emailRequest, baseUrl, entityName) },
                { QuotationStatus.AERejected,  (emailRequest) =>  this.SendEmail("Emails/Quotation/QuotationRejected", emailRequest, baseUrl, entityName) },
                { QuotationStatus.ManagerRejected,  (emailRequest) =>  this.SendEmail("Emails/Quotation/QuotationRejected", emailRequest, baseUrl, entityName) },
                { QuotationStatus.Canceled,  (emailRequest) =>  this.SendEmail("Emails/Quotation/QuotationCancelled", emailRequest, baseUrl, entityName) },
                { QuotationStatus.SentToClient,  (emailRequest) =>  this.SendEmail("Emails/Quotation/QuotationSent", emailRequest, baseUrl, entityName) },
                { QuotationStatus.CustomerValidated,  (emailRequest) =>  this.SendEmail("Emails/Quotation/QuotationCustConfirmed", emailRequest, baseUrl, entityName) },
                { QuotationStatus.CustomerRejected,  (emailRequest) =>  this.SendEmail("Emails/Quotation/QuotationCustRejected", emailRequest, baseUrl, entityName) }
            };

            var requestManager = new SetStatusBusinessRequest
            {
                Id = request.Id,
                CusComment = request.CusComment,
                IdStatus = (QuotationStatus)request.StatusId,
                OnSendEmail = actionSendList.TryGetValue((QuotationStatus)request.StatusId, out Action<SendEmailRequest> post) ? post : null,
                ApiRemark = request.ApiRemark,
                Url = baseUrl + string.Format(_configuration["UrlQuotation"], request.Id, entityName),
                ApiInternalRemark = request.ApiInternalRemark,
                ConfirmDate = request.ConfirmDate ?? DateTime.Now.GetCustomDate()
            };

            return await _quotationManager.SetStatus(requestManager);
        }

        //[Right("supplier-summary")]
        [HttpGet("preview/{id}")]
        [Right("supplier-summary")]
        public async Task<QuotationFileResponse> Preview(int id, [FromServices] IQuotationPDF previewService)
        {
            var quotation = await _quotationManager.GetQuotationDetails(id);
            string strPdfFilePath = string.Empty;

            if (quotation != null)
            {
                strPdfFilePath = await _quotationManager.GetquotationPdfPath(id);

                if (string.IsNullOrEmpty(strPdfFilePath))
                {
                    var document = previewService.CreateDocument(quotation);
                    strPdfFilePath = _quotationManager.SavePdfReferenceToCloudAndUpdatewithQuotation(document, id);
                }
            }

            return new QuotationFileResponse() { FilePath = strPdfFilePath };
        }

        [HttpGet("version/{id}")]
        [Right("supplier-summary")]
        public async Task<IActionResult> GetQutoationVersion(Guid id)
        {
            FileResponse response = await _quotationManager.GetQuotationVersion(id);

            if (response.Result != DTO.File.FileResult.Success)
                return NotFound();

            return File(response.Content, response.MimeType);
        }
        [Right("quotation-summary")]
        [HttpPost("export-quotation")]
        public async Task<IActionResult> ExportQuotationSummary([FromBody] QuotationSummaryGenRequest request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            if (request.ServiceId == Service.InspectionId)
            {
                var response = await _quotationManager.GetQuotationSummaryInspExportDetails(request);
                if (response == null || response.Result == QuotationExportResult.NotFound)
                    return NotFound();
                var stream = _helper.GetAsStreamObject(response.QuotationInspProdExportList);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "quotation.xlsx");
            }
            if (request.ServiceId == Service.AuditId)
            {
                request.Index = pageindex;
                request.PageSize = PageSize;
                var response = await _quotationManager.GetQuotationSummaryAuditExportDetails(request);
                if (response == null || response.Result == QuotationExportResult.NotFound)
                    return NotFound();

                var stream = _helper.GetAsStreamObject(response.QuotationAuditExportList);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "quotation.xlsx");
            }
            return NotFound();

        }

        [HttpPost("quotation-manday")]
        [Right("edit-quotation")]
        public async Task<QuotationMandayResponse> GetQuotationManday([FromBody] QuotationMandayRequest request)
        {
            return await _quotationManager.QuotationManday(request);
        }
        [HttpPost("quotation-sampleqty")]
        [Right("edit-quotation")]
        public async Task<bool> CheckQuotationSampleQtyAndBookingSampleQtyAreEqual([FromBody] IEnumerable<QuotProduct> quotProducts)
        {
            return await _quotationManager.CheckQuotationSampleQtyAndBookingSampleQtyAreEqual(quotProducts);
        }

        /// <summary>
        /// Save email data into log table and publish to queue
        /// </summary>
        /// <param name="emailQueueRequest"></param>
        /// <param name="emailLogRequest"></param>
        /// <returns></returns>
        private void PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest)
        {
            var resultId = _emailLogQueueManager.AddEmailLog(emailLogRequest).Result;
            emailQueueRequest.EmailQueueId = resultId;
            _rabbitMQClient.Publish<EmailDataRequest>(_configuration["EmailQueue"], emailQueueRequest);
        }

        [HttpPost("clientQuotation")]
        [Right("quotation-summary")]
        public async Task<IActionResult> GetClientQuotation([FromBody] int quotationId)
        {
            var response = await _quotationManager.GetClientQuotation(quotationId);
            if (response == null || response.Result != ClinetQuotationResult.Success)
                return NotFound();
            return await this.FileAsync("AdeoQuotationExport", response.QuotationDetails, Components.Core.entities.FileType.Excel);
        }


        [HttpPost("invoiceSave")]
        [Right("quotation-summary")]
        public async Task<SaveQuotationResponse> SaveInvoice([FromBody] InvoiceRequest request)
        {
            return await _quotationManager.SaveInvoice(request);
        }

        [HttpGet("getInvoice/{quotationId}/{serviceId}")]
        [Right("quotation-summary")]
        public async Task<QuotationEditInvoiceItem> GetInvoice(int quotationId, int serviceId)
        {
            return await _quotationManager.GetInvoice(quotationId, serviceId);
        }

        [HttpPost("getUnitPrice")]
        public async Task<CustomerPriceCardUnitPriceResponse> GetUnitPriceByCustomerPriceCardRule(UnitPriceCardRequest request)
        {
            return await _quotationManager.GetCustomerPriceCardUnitPriceData(request);
        }

        [HttpPost("getPriceCardData")]
        public async Task<QuotationPriceCard> GetPriceCard(CustomerPriceCardRequest request)
        {
            return await _quotationManager.GetCustomerPriceCardData(request);
        }

        [HttpPost("getSamplingUnitPrice")]
        [Right("quotation-summary")]
        public async Task<SamplingUnitPriceResponse> GetSamplingUnitPrice(List<SamplingUnitPriceRequest> samplingUnitPriceRequest)
        {
            return await _quotationManager.GetSamplingUnitPriceByBooking(samplingUnitPriceRequest);
        }

        [HttpGet("getBillPaidByList")]
        [Right("quotation-summary")]
        public async Task<DataSourceResponse> GetBillPaidByList()
        {
            return await _quotationManager.GetBillPaidByList();
        }

        [HttpGet("getQuotationStatusColor")]
        [Right("quotation-summary")]
        public async Task<QuotationSummaryStatusResponse> GetQuotationStatusColor()
        {
            return await _quotationManager.GetQuotationStatusColor();
        }

        [HttpPost("getSkipQuotationSentToClientCheckpoint")]
        [Right("quotation-summary")]
        public async Task<bool> GetSkipQuotationSentToClientCheckpoint(QuotCheckpointRequest request)
        {
            return await _quotationManager.GetSkipQuotationSentToClientCheckpoint(request);
        }

        [HttpGet("get-Calculated_working-manday/{bookingId}")]
        [Right("quotation-summary")]
        public async Task<CalculatedWorkingHoursResponse> GetCalculatedWorkingManday(int bookingId)
        {
            return await _quotationManager.GetCalculatedWorkingManday(bookingId);
        }


        [HttpPost("getTravelMatrixData")]
        public async Task<QuotationTravelMatrixResponse> GetTravelMatrixData(TravelMatrixRequest request)
        {
            return await _quotationManager.GetTravelMatrixData(request);
        }

        [HttpGet("get-price-card-travel/{ruleId}")]
        [Right("edit-quotation")]
        public async Task<PriceCardTravelResponse> GetPriceCardTravel(int ruleId)
        {
            return await _quotationManager.GetPriceCardTravel(ruleId);
        }

        [HttpGet("save-working-manday/{bookingId}")]
        [Right("quotation-summary")]
        public async Task<CalculatedWorkingHoursResponse> SaveWorkingManday(int bookingId)
        {
            return await _quotationManager.SaveWorkingManday(bookingId);
        }

        [HttpPost("factory-booking-info")]
        public async Task<FactoryBookingInfoResponse> FactoryBookingInfo(FactoryBookingInfoRequest request)
        {
            return await _quotationManager.FactoryBookingInfo(request);
        }
    }
}