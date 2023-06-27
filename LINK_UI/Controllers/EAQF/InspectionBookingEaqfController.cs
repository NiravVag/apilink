//using Components.Core.contracts;
//using Components.Core.entities.Emails;
//using Contracts.Managers;
//using DTO.CancelBooking;
//using DTO.Customer;
//using DTO.Eaqf;
//using DTO.EmailLog;
//using DTO.Inspection;
//using DTO.Invoice;
//using DTO.InvoicePreview;
//using DTO.MasterConfig;
//using Entities.Enums;
//using FastReport.Export.Pdf;
//using FastReport.Web;
//using LINK_UI.App_start;
//using LINK_UI.Controllers.EXTERNAL;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using RabbitMQUtility;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using static DTO.Common.ApiCommonData;
//using Components.Web;

//namespace LINK_UI.Controllers.EAQF
//{
//    [Route("api/EAQF/[controller]")]
//    [Authorize(Policy = "EAQFUserPolicy")]
//    [ApiController]
//    public class InspectionBookingEaqfController : ExternalBaseController
//    {
//        private readonly IInspectionBookingManager _manager = null;
//        private readonly ITenantProvider _filterService = null;
//        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
//        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
//        private static IConfiguration _configuration = null;
//        private readonly ICancelBookingManager _cbManager = null;
//        private readonly IManualInvoiceManager _manualInvoiceManager = null;
//        private readonly IInspectionCustomReportManager _inspectionCustomReportManager;
//        private readonly IEmailManager _emailManager;
//        public InspectionBookingEaqfController(IInspectionBookingManager manager, ITenantProvider filterService,
//            IEmailLogQueueManager emailLogQueueManager,
//            IRabbitMQGenericClient rabbitMQClient,
//            ICancelBookingManager cbManager,
//            IManualInvoiceManager manualInvoiceManager,
//            IInspectionCustomReportManager inspectionCustomReportManager,
//            IEmailManager emailManager,
//            IConfiguration configuration)
//        {
//            _manager = manager;
//            _filterService = filterService;
//            _emailLogQueueManager = emailLogQueueManager;
//            _rabbitMQClient = rabbitMQClient;
//            _configuration = configuration;
//            _cbManager = cbManager;
//            _manualInvoiceManager = manualInvoiceManager;
//            _inspectionCustomReportManager = inspectionCustomReportManager;
//            _emailManager = emailManager;
//        }
//        [HttpPost]
//        public async Task<IActionResult> SaveEaqfInspectionBooking(SaveEaqfInsepectionRequest request)
//        {
//            var response = await _manager.SaveEaqfInspectionBooking(request);
//            //status code
//            System.Reflection.PropertyInfo pi = response.GetType().GetProperty("statusCode");
//            if (pi != null)
//            {
//                System.Reflection.PropertyInfo piObj = response.GetType().GetProperty("data");
//                if (piObj != null)
//                {
//                    var bookingEaqfResponse = (BookingEaqfResponse)(piObj.GetValue(response, null));

//                    if (bookingEaqfResponse.IsTechincalDocumentsAddedOrRemoved)
//                    {
//                        await PublishFileAttachmentQueueMessage(bookingEaqfResponse.MissionId);
//                    }

//                    await SendEmailAndNotifications(request, bookingEaqfResponse, _configuration);
//                }
//            }
            
//            return BuildCommonEaqfResponse(response);
//        }

//        /// <summary>
//        /// Send Email Notification for EAQF Booking
//        /// </summary>
//        /// <param name="request"></param>
//        /// <param name="response"></param>
//        /// <param name="configuration"></param>
//        /// <returns></returns>
//        private async Task SendEmailAndNotifications(SaveEaqfInsepectionRequest request, BookingEaqfResponse response, IConfiguration configuration)
//        {
//            //get the booking mail data
//            BookingMailRequest mailData = await _manager.GetBookingMailDetail(response.MissionId, true, false,request.UserId);

//            //get the notification list
//            SetInspNotifyResponse notifRres = await _manager.BookingTaskNotification(response.MissionId, false, response.StatusId, response.SaveInsepectionRequest);

//            var masterConfigs = await _manager.GetMasterConfiguration();
//            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
//            string baseUrl = _configuration["BaseUrl"];

//            var CCUserList = new List<string>();
//            var ToUserList = new List<string>();

//            //To user list .has to be mandatory if no data , send to Booking team.
//            if (CCUserList.Count == 0 || ToUserList.Count == 0)
//            {
//                var bookingTeamGroupEmail = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.BookingTeamGroupEmail).Select(x => x.Value).FirstOrDefault();
//                CCUserList.Add(bookingTeamGroupEmail);
//            }

//            //add notification user list 
//            if (notifRres != null)
//            {
//                if (notifRres.UserList != null)
//                {
//                    CCUserList.AddRange(notifRres.UserList.Select(x => x.EmailAddress));
//                }

//                if (notifRres.ToRecipients != null)
//                {
//                    ToUserList.AddRange(notifRres.ToRecipients.Select(x => x.EmailAddress));
//                }
//            }

//            var entityId = _filterService.GetCompanyId();
//            var _settings = _emailManager.GetMailSettingConfiguration(entityId);


//            mailData.SenderEmail = _settings.SenderEmail;
//            // check cc is exist in to addrees - if available remove from cc list
//            CCUserList.RemoveAll(ccemail => ToUserList.Contains(ccemail));
//            ToUserList.RemoveAll(temail => temail == null);
//            CCUserList.RemoveAll(ccemail => ccemail == null);

//            var emailQueueRequest = new EmailDataRequest
//            {
//                TryCount = 1,
//                Id = Guid.NewGuid()
//            };

//            var subjectStatusName = "Requested";

//            var emailLogRequest = new EmailLogData()
//            {
//                ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
//                Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
//                TryCount = 1,
//                Status = (int)EmailStatus.NotStarted,
//                SourceName = "Save Booking",
//                SourceId = response.MissionId,
//                Subject = $"{entityName} Inspection Booking - {subjectStatusName} (INS - {response.MissionId}, Customer: {mailData.CustomerName}, Supplier: {mailData.SupplierName}"
//            };

//            var urlBookingRequest = baseUrl + string.Format(configuration["UrlBookingRequest"], response.MissionId, entityName);
//            mailData.EntityName = entityName;

//            if (response.StatusId == (int)BookingStatus.Received)
//            {
//                emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingRequest", (mailData, urlBookingRequest));
//                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
//            }
//        }

//        /// <summary>
//        /// Publish the message queue
//        /// </summary>
//        /// <param name="emailQueueRequest"></param>
//        /// <param name="emailLogRequest"></param>
//        /// <returns></returns>
//        private async Task PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest)
//        {
//            if (!string.IsNullOrWhiteSpace(emailLogRequest.ToList) || !string.IsNullOrEmpty(emailLogRequest.Cclist))
//            {
//                var resultId = await _emailLogQueueManager.AddEmailLog(emailLogRequest);
//                emailQueueRequest.EmailQueueId = resultId;
//                await _rabbitMQClient.Publish<EmailDataRequest>(_configuration["EmailQueue"], emailQueueRequest);
//            }

//        }

//        [HttpPost("Event/{BookingId}")]
//        public async Task<IActionResult> BookingEventUpdate(int BookingId, [FromBody] EaqfEvent request)
//        {
//            var response = await _manager.EaqfBookingEventUpdate(BookingId, request);

//            System.Reflection.PropertyInfo pi = response.GetType().GetProperty("statusCode");
//            if (pi != null)
//            {
//                var statusCode = (HttpStatusCode)(pi.GetValue(response, null));
//                if (statusCode != HttpStatusCode.OK)
//                {
//                    return BuildCommonEaqfResponse(response);
//                }

//                var invoiceId = await _manualInvoiceManager.GetManualInvoiceIdbyInvoiceNumber(request.InvoiceNo);

//                if (invoiceId > 0)
//                {
//                    var result = await GenerateInovicePdf(invoiceId, request.UserId);
//                    return BuildCommonEaqfResponse(result);
//                }
//            }

//            return BuildCommonEaqfResponse(response);
//        }

//        [HttpPost("RescheduleEvent/{BookingId}")]
//        public async Task<IActionResult> BookingRescheduleEventUpdate(int BookingId, [FromBody] EaqfRescheduleEvent request)
//        {
//            if (request == null)
//            {
//                return BuildCommonEaqfResponse(EaqfErrorResponse(HttpStatusCode.BadRequest, "BadRequest", new List<string>() { "Request object is not valid" }));
//            }

//            if (BookingId <= 0)
//            {
//                return BuildCommonEaqfResponse(EaqfErrorResponse(HttpStatusCode.BadRequest, "BadRequest", new List<string>() { "Booking id is not valid" }));
//            }
//            SaveRescheduleResponse response = null;
//            SaveRescheduleRequest rescheduleRequest = new SaveRescheduleRequest();
//            rescheduleRequest.BookingId = BookingId;
//            rescheduleRequest.CurrencyId = 156; // USD
//            rescheduleRequest.IsKeepAllocatedQC = true;
//            rescheduleRequest.ServiceFromDate = request.ServiceDateFrom;
//            rescheduleRequest.ServiceToDate = request.ServiceDateTo;
//            rescheduleRequest.FirstServiceDateFrom = request.ServiceDateFrom;
//            rescheduleRequest.FirstServiceDateTo = request.ServiceDateTo;
//            rescheduleRequest.ReasonTypeId = (int)RescheduleReasonType.RequestByEaqf;
//            rescheduleRequest.UserId = request.UserId;
//            // Save request
//            var fbToken = getFbToken();

//            // check the booking status is valid to cancel it

//            if (request.Classification.Trim().ToLower() == DateChange.ToLower())
//            {
//                await _cbManager.SaveRescheduleDetails(rescheduleRequest, fbToken);
//            }
//            else if (request.Classification.Trim().ToLower() == DateChangePenalty.ToLower())
//            {
//                response = await _cbManager.SaveRescheduleDetails(rescheduleRequest, fbToken);
//                SaveQuotationEaqfRequest invoiceEAQFRequest = new SaveQuotationEaqfRequest();
//                List<EAQFOrderDetails> orderDetails = new List<EAQFOrderDetails>();
//                if (response.Result == SaveRescheduleResult.Success)
//                {
//                    invoiceEAQFRequest.CurrencyCode = "USD";
//                    invoiceEAQFRequest.BookingId = BookingId;
//                    invoiceEAQFRequest.Service = (int)(Service.InspectionId);
//                    invoiceEAQFRequest.UserId = request.UserId;
//                    invoiceEAQFRequest.PaymentMode = request.PaymentMode;
//                    invoiceEAQFRequest.PaymentRef = request.PaymentRef;
//                    orderDetails.Add(
//                        new EAQFOrderDetails()
//                        {
//                            Description = "Less than 2 days before service from EAQF",
//                            Amount = request.Amount,
//                            OrderType = "otherfee"
//                        }
//                    );

//                    invoiceEAQFRequest.OrderDetails = orderDetails;
//                    var invoice = await _manualInvoiceManager.SaveEAQFManualInvoice(invoiceEAQFRequest, true);

//                    if (invoice != null)
//                    {
//                        var invoiceStatusCodePropertry = invoice.GetType().GetProperty("statusCode");
//                        if (invoiceStatusCodePropertry != null)
//                        {
//                            var statusCode = (HttpStatusCode)(invoiceStatusCodePropertry.GetValue(invoice, null));
//                            if (statusCode != HttpStatusCode.OK)
//                            {
//                                return BuildCommonEaqfResponse(invoice);
//                            }
//                            else
//                            {
//                                System.Reflection.PropertyInfo piObj = invoice.GetType().GetProperty("data");
//                                if (piObj != null)
//                                {
//                                    var invoiceResponse = (EAQFInvoiceResponse)(piObj.GetValue(invoice, null));
//                                    var result = await GenerateInovicePdf(invoiceResponse.InvoiceId, request.UserId);
//                                    return BuildCommonEaqfResponse(result);
//                                }
//                            }
//                        }
//                    }
//                    else
//                    {
//                        return BuildCommonEaqfResponse(EaqfErrorResponse(HttpStatusCode.InternalServerError, "Internal server error", new List<string>() { "Internal server error" }));
//                    }
//                }
//            }
//            else
//            {
//                return BuildCommonEaqfResponse(EaqfErrorResponse(HttpStatusCode.BadRequest, "Bad Request", new List<string>() { "Classification is not Valid" }));
//            }

//            var successResponse = new EaqfGetSuccessResponse()
//            {
//                message = "Success",
//                statusCode = HttpStatusCode.OK
//            };

//            return BuildCommonEaqfResponse(successResponse);
//        }

//        private async Task<object> GenerateInovicePdf(int invoiceId, int userId)
//        {
//            WebReport InspReport = new WebReport();
//            SaveInvoicePdfResponse saveInvoicePdf = new SaveInvoicePdfResponse();
//            string reportPath = "";
//            string invoiceNo = "";
//            try
//            {
//                if (invoiceId > 0)
//                {
//                    var invoice = await _manualInvoiceManager.GetEaqfManualInvoice(invoiceId);

//                    if (invoice == null)
//                        return new SaveInvoicePdfResponse() { Result = SaveInvoicePdfResult.InvoiceNotFound };

//                    invoiceNo = invoice.Invoice.FirstOrDefault()?.InvoiceNo;

//                    var invoicePdfPath = Path.Combine("Views", "Report_Templates", "EAQF", "E-AQF-Invoice.frx");

//                    InspReport.Report.Load(invoicePdfPath);

//                    if (invoice.Invoice != null)
//                    {
//                        InspReport.Report.RegisterData(invoice.Invoice, "Invoice");
//                    }


//                    if (invoice.InvoiceItems != null)
//                    {
//                        InspReport.Report.RegisterData(invoice.InvoiceItems, "InvoiceItems");
//                    }

//                    InspReport.Report.Prepare();
//                    string fileExtension = "pdf";
//                    string fileName = invoice.Invoice.FirstOrDefault().InvoiceNo + "." + fileExtension;

//                    PDFExport reportPDF = new PDFExport();
//                    reportPDF.PrintOptimized = true;
//                    reportPDF.JpegCompression = true;
//                    reportPDF.JpegQuality = 60;
//                    using (var memory = new MemoryStream())
//                    {
//                        InspReport.Report.Export(reportPDF, memory);
//                        var result = _inspectionCustomReportManager.FetchCloudReportUrl(memory, fileName, fileExtension, Entities.Enums.FileContainerList.InvoiceSend);
//                        reportPath = result.filePath;
//                        if (!string.IsNullOrWhiteSpace(result.filePath))
//                        {
//                            saveInvoicePdf = await _manualInvoiceManager.SaveInvoicePdfUrl(invoiceId, result.filePath, result.uniqueId, userId);
//                            if (saveInvoicePdf.Result != SaveInvoicePdfResult.Success)
//                                return EaqfErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error", new List<string>() { "Invoice pdf url not saved" });
//                        }
//                        else
//                        {
//                            return EaqfErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error", new List<string>() { "Invoice pdf is not saved in the database" });
//                        }
//                    }
//                }
//                else
//                {
//                    return EaqfErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error", new List<string>() { "Something went wrong, Invoice is not found" });
//                }

//                InspReport.Report.Dispose();
//                return new EaqfGetSuccessResponse()
//                {
//                    data = new SaveEAQFInvoicePDFResponse()
//                    {
//                        invoicePdfUrl = reportPath,
//                        invoiceNo = invoiceNo
//                    },
//                    statusCode = HttpStatusCode.OK,
//                    message = "Success"
//                };
//            }
//            catch (Exception ex)
//            {
//                saveInvoicePdf.Result = SaveInvoicePdfResult.Error;
//                InspReport.Report.Dispose();
//            }
//            return saveInvoicePdf;
//        }

//        private EaqfErrorResponse EaqfErrorResponse(HttpStatusCode statusCode, string message, List<string> errors)
//        {
//            return new EaqfErrorResponse()
//            {
//                errors = errors,
//                statusCode = statusCode,
//                message = message
//            };
//        }
//        private async Task PublishFileAttachmentQueueMessage(int bookingId)
//        {
//            FileAttachmentToZipRequest fileAttachmentToZipRequest = new FileAttachmentToZipRequest();
//            fileAttachmentToZipRequest.InspectionId = bookingId;
//            fileAttachmentToZipRequest.EntityId = _filterService.GetCompanyId();

//            var emailLogRequest = new EmailLogData()
//            {
//                TryCount = 1,
//                Status = (int)EmailStatus.NotStarted,
//                SourceName = "Save Booking Files as Zip",
//                SourceId = bookingId,

//            };

//            await _emailLogQueueManager.AddEmailLog(emailLogRequest);

//            await _rabbitMQClient.Publish<FileAttachmentToZipRequest>(_configuration["ProcessZipFileQueue"], fileAttachmentToZipRequest);
//        }

//        [HttpGet("bookinginfo")]
//        public async Task<IActionResult> GetEaqfInspectionBooking([FromQuery] GetEaqfInspectionBookingRequest request)
//        {
//            var response = await _manager.GetEaqfInspectionBooking(request);
//            return BuildCommonEaqfResponse(response);
//        }

//        [HttpGet("reportinfo")]
//        public async Task<IActionResult> GetEaqfInspectionBookingReportInformation(string bookingIds)
//        {
//            var response = await _manager.GetEaqfInspectionReportBooking(bookingIds);
//            return BuildCommonEaqfResponse(response);
//        }

//        /// <summary>
//        /// Get FB token based on the needs
//        /// </summary>
//        /// <returns></returns>
//        private string getFbToken()
//        {
//            var Fbclaims = new List<Claim>
//            {
//                new Claim("email",_configuration["FbAdminEmail"]),
//                new Claim("firstname", _configuration["FbAdminUserName"]),
//                new Claim("lastname", ""),
//                new Claim("role", "admin"),
//                new Claim("redirect", "")
//            };
//            return AuthentificationService.CreateFBToken(Fbclaims, _configuration["FBKey"]);
//        }
//    }
//}
