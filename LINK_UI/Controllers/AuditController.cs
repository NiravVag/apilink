using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO.Audit;
using DTO.Supplier;
using Contracts.Managers;
using Microsoft.AspNetCore.Authorization;
using LINK_UI.Filters;
using System.IO;
using static DTO.Common.Static_Data_Common;
using Components.Web;
using DTO.Location;
using DTO.OfficeLocation;
using DTO.EmailLog;
using DTO.FullBridge;
using RabbitMQUtility;
using Microsoft.Extensions.Configuration;
using Entities.Enums;
using DTO.Inspection;
using DTO.MasterConfig;
using DTO.Common;
using DTO.CommonClass;
using Components.Core.entities.Emails;
using Components.Core.contracts;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class AuditController : ControllerBase
    {
        private IAuditManager _manager = null;
        private ILocationManager _location = null;
        private ISupplierManager _supplier = null;
        private IBookingEmailLogQueueManager _bookingEmailLogQueueManager;
        private IRabbitMQGenericClient _rabbitMQClient;
        private IConfiguration _configuration;
        private readonly IEmailManager _emailManager;
        private readonly ITenantProvider _filterService = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;

        public AuditController(IAuditManager manager, ILocationManager location, ISupplierManager supplier,
            IBookingEmailLogQueueManager bookingEmailLogQueueManager,
            IRabbitMQGenericClient rabbitMQClient, IEmailManager emailManager, IEmailLogQueueManager emailLogQueueManager,
            ITenantProvider filterService, IConfiguration configuration)
        {
            _manager = manager;
            _location = location;
            _supplier = supplier;
            _bookingEmailLogQueueManager = bookingEmailLogQueueManager;
            _rabbitMQClient = rabbitMQClient;
            _configuration = configuration;
            _emailManager = emailManager;
            _filterService = filterService;
            _emailLogQueueManager = emailLogQueueManager;
        }

        [HttpGet("add")]
        [Right("edit-audit")]
        public async Task<EditAuditResponse> Add()
        {
            return await _manager.EditAudit(null);
        }

        [HttpGet("EditAudit/{id}")]
        [Right("edit-audit")]
        public async Task<EditAuditResponse> EditAudit(int id)
        {
            return await _manager.EditAudit(id);
        }

        [Right("edit-audit")]
        [HttpGet("GetAuditDetailsByCustomerId/{id}")]
        public async Task<EditAuidtCusDetails> GetAuditDetailsByCustomerId(int id)
        {
            return await _manager.GetAuditDetailsByCustomerId(id);
        }
        [Right("edit-audit")]
        [HttpGet("GetAuditEvaluationRound")]
        public async Task<AuditEvaluationRoundResponse> GetAuditEvaluationRound()
        {
            return await _manager.GetEvaluationRound();
        }
        [Right("edit-audit")]
        [HttpGet("GetAuditOffice")]
        public OfficeSummaryResponse GetAuditOffice()
        {
            return _manager.GetAuditOffice();
        }
        [Right("edit-audit")]
        [HttpGet("GetAuditType")]
        public async Task<AuditTypeResponse> GetAuditType()
        {
            return await _manager.GetAuditType();
        }
        [Right("edit-audit")]
        [HttpGet("GetAuditWorkprocess")]
        public async Task<AuditWorkprocessResponse> GetAuditWorkProcess()
        {
            return await _manager.GetAuditWorkProcess();
        }
        [Right("edit-audit")]
        [HttpGet("GetSupplierDetailsById/{cusid},{supid}")]
        public async Task<EditAuditSupDetails> GetSupplierDetailsById(int cusid, int supid)
        {
            return await _manager.GetSupplierDetailsByCustomerIdSupplierId(cusid, supid);
        }

        [Right("edit-audit")]
        [HttpGet("GetSupplierDetailsById/{supid}")]
        public async Task<EditAuditSupDetails> GetSupplierDetailsById(int supid)
        {
            return await _manager.GetSupplierDetailsByCustomerIdSupplierId(null, supid);
        }
        [Right("edit-audit")]
        [HttpGet("GetAuditBookingContactDetails/{factid}")]
        public async Task<AuditBookingContactResponse> GetAuditBookingContactDetails(int factid)
        {
            return await _manager.GetAuditBookingContactInformation(factid);
        }
        [Right("edit-audit")]
        [HttpGet("GetAuditBookingRuleDetails/{cusid},{factid}")]
        public async Task<AuditBookingRuleResponse> GetAuditBookingRuleDetails(int cusid, int factid)
        {
            return await _manager.GetAuditBookingRules(cusid, factid);
        }
        [Right("edit-audit")]
        [HttpGet("GetAuditCS/{factid},{cusid}")]
        public async Task<AuditCSResponse> GetAuditCS(int factid, int? cusid)
        {
            return await _manager.GetAuditCS(factid, cusid);
        }
        [Right("edit-audit")]
        [HttpGet("GetFactoryDetailsById/{cusid},{factid}")]
        public async Task<EditAuditFactDetails> GetFactoryDetailsById(int cusid, int factid)
        {
            return await _manager.GetFactoryDetailsByCustomerIdFactoryId(cusid, factid);
        }

        [Right("edit-audit")]
        [HttpGet("GetFactoryDetailsById/{factid}")]
        public async Task<EditAuditFactDetails> GetFactoryDetailsById(int factid)
        {
            return await _manager.GetFactoryDetailsByCustomerIdFactoryId(null, factid);
        }

        [Right("edit-audit")]
        [HttpPost("save")]
        public async Task<SaveAuditResponse> SaveAudit([FromBody] AuditDetails request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            if (request == null)
                return new SaveAuditResponse { Result = SaveAuditResult.RequestNotCorrectFormat };

            var response = await _manager.SaveAudit(request);
            if (response.Result == SaveAuditResult.Success && response.Id > 0)
            {
                if (request.AuditId == 0)
                {
                    var bookingFbLog = new BookingFbLogData()
                    {
                        BookingId = response.Id,
                        FbBookingSyncType = FbBookingSyncType.AuditCreation,
                        TryCount = 1
                    };
                    await PublishFbAuditBookingQueue(bookingFbLog);

                }
                else
                {
                    var bookingFbLog = new BookingFbLogData()
                    {
                        BookingId = response.Id,
                        FbBookingSyncType = FbBookingSyncType.AuditUpdation,
                        TryCount = 1,
                        IsMissionUpdated = response.IsMissionUpdated
                    };
                    await PublishFbAuditBookingQueue(bookingFbLog);
                }

                await SendEmailAndNotifications(request, response, broadCastService, configuration);
            }
            return response;
        }

        private async Task SendEmailAndNotifications(AuditDetails request, SaveAuditResponse response, IBroadCastService broadCastService, IConfiguration configuration)
        {
            SetInspNotifyResponse notifRres = null;

            notifRres = await _manager.BookingTaskNotification(response.Id, request.StatusId, request);

            BookingMailRequest mailData = await _manager.GetBookingMailDetail(response.Id, request, true);

            var masterConfigs = await _manager.GetMasterConfiguration();

            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            string baseUrl = _configuration["BaseUrl"];

            var CCUserList = new List<string>();
            var ToUserList = new List<string>();

            if (notifRres != null)
            {
                mailData.quotationExists = notifRres.quotationExists;
                mailData.StatusName = notifRres.StatusName;

                if (notifRres.UserList != null)
                {
                    CCUserList.AddRange(notifRres.UserList.Select(x => x.EmailAddress));
                }

                if (notifRres.ToRecipients != null)
                {
                    ToUserList.AddRange(notifRres.ToRecipients.Select(x => x.EmailAddress));
                }

                if (notifRres.CustomerEmail != null && !request.IsEaqf)
                {
                    ToUserList.Add(notifRres.CustomerEmail);
                }
            }

            if (!request.IsEaqf && request.IsSupplierOrFactoryEmailSend
               && request.AuditTypeid != (int)AuditTypeEnum.UnAnnounced)
            {
                ToUserList.Add(mailData.SupplierMail);
                ToUserList.Add(mailData.FactoryMail);
            }

            //To user list .has to be mandatory if no data , send to Booking team.
            if (CCUserList.Count == 0 || ToUserList.Count == 0)
            {
                var bookingTeamGroupEmail = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.BookingTeamGroupEmail).Select(x => x.Value).FirstOrDefault();
                CCUserList.Add(bookingTeamGroupEmail);
            }
            var entityId = _filterService.GetCompanyId();
            var _settings = _emailManager.GetMailSettingConfiguration(entityId);

            mailData.SenderEmail = _settings.SenderEmail;
            // check cc is exist in to addrees - if available remove from cc list
            CCUserList.RemoveAll(ccemail => ToUserList.Contains(ccemail));
            ToUserList.RemoveAll(temail => temail == null);
            CCUserList.RemoveAll(ccemail => ccemail == null);

            if (!request.IsEaqf && request.FactoryId > 0)
            {
                var ccEmails = await _manager.GetCCEmailConfigurationEmailsByCustomer(request.CustomerId, request.FactoryId, request.StatusId);
                if (ccEmails != null && ccEmails.Any())
                {
                    CCUserList.AddRange(ccEmails);
                }
            }

            var emailQueueRequest = new EmailDataRequest
            {
                TryCount = 1,
                Id = Guid.NewGuid()
            };

            var subjectStatusName = notifRres.StatusName;

            var emailLogRequest = new EmailLogData()
            {
                ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                TryCount = 1,
                Status = (int)EmailStatus.NotStarted,
                SourceName = "Save Booking",
                SourceId = response.Id,
                Subject = $"{entityName} Audit Booking - {subjectStatusName} (AUD - {response.Id}, Customer: {mailData.CustomerName}, Supplier: {mailData.SupplierName},Service: {mailData.ServiceType}"
            };

            emailLogRequest.Subject = mailData.BookingType == (int)AuditTypeEnum.UnAnnounced ? emailLogRequest.Subject + ", BookingType: " + mailData.BookingTypeValue + ")" : emailLogRequest.Subject + ")";


            var urlBookingRequest = baseUrl + string.Format(configuration["UrlAuditRequest"], response.Id, entityName);

            mailData.EntityName = entityName;
            if (request.StatusId == (int)AuditBookingStatus.Received)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Audit/AuditBookingRequest", (mailData, urlBookingRequest));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);

            }
            else if (request.StatusId == (int)AuditBookingStatus.Confirmed)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Audit/AuditBookingConfirm", (mailData, urlBookingRequest));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            else if (request.StatusId == (int)AuditBookingStatus.Rescheduled)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Audit/AuditBookingReschedule", (mailData, urlBookingRequest));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            else if (request.StatusId == (int)AuditBookingStatus.Cancel)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Audit/AuditBookingCancel", (mailData, urlBookingRequest));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
        }

        private async Task PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest)
        {
            if (!string.IsNullOrWhiteSpace(emailLogRequest.ToList) || !string.IsNullOrEmpty(emailLogRequest.Cclist))
            {
                var resultId = await _emailLogQueueManager.AddEmailLog(emailLogRequest);
                emailQueueRequest.EmailQueueId = resultId;
                await _rabbitMQClient.Publish<EmailDataRequest>(_configuration["EmailQueue"], emailQueueRequest);
            }

        }

        [Right("edit-audit")]
        [HttpPost("attached/{auditid}")]
        [DisableRequestSizeLimit]
        public bool UploadAttachedFiles(int auditid)
        {
            if (Request.Form.Files != null && Request.Form.Files.Any())
            {
                var dict = new Dictionary<Guid, byte[]>();

                foreach (var file in Request.Form.Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        Guid.TryParse(file.Name, out Guid guid);
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            dict.Add(guid, fileBytes);
                        }
                    }
                }

                //_manager.UploadFiles(auditid, dict).Wait();
                return true;
            }
            return false;
        }

        [Right("audit-summary")]
        [HttpGet("Getauditsummary")]
        public async Task<AuditSummaryResponse> Getauditsummary()
        {
            return await _manager.GetAuditSummary();
        }

        [Right("audit-summary")]
        [HttpPost("SearchAuditSummary")]
        public async Task<AuditSummarySearchResponse> SearchAuditSummary([FromBody] AuditSummarySearchRequest request)
        {
            if (request == null)
                return new AuditSummarySearchResponse() { Result = AuditSummarySearchResponseResult.NotFound };
            return await _manager.SearchAuditSummary(request);
        }

        [Right("audit-summary")]
        [HttpPost("ExportAuditSearchSummary")]
        public async Task<IActionResult> ExportAuditSearchSummary([FromBody] AuditSummarySearchRequest request)
        {
            if (request == null)
                return NotFound();
            request.Index = 0;// Convert.ToInt32(ExportPagination.PageIndex);
            request.pageSize = 100000;// Convert.ToInt32(ExportPagination.PageSize);
            var response = await _manager.SearchAuditSummary(request);
            if (response == null || response.Result != AuditSummarySearchResponseResult.Success)
                return NotFound();
            return await this.FileAsync("AuditSearchSummary", response.Data, Components.Core.entities.FileType.Excel);
        }

        [HttpGet("GetCancelEditAuditDetails/{id},{optypeid}")]
        [Right("audit-cancel")]
        public async Task<EditCancelRescheduleAuditResponse> GetCancelEditAuditDetails(int id, int optypeid)
        {
            return await _manager.GetAuditCancelDetails(id, optypeid);
        }

        [HttpGet("GetAuditBasicDetails/{id}")]
        [Right("audit-report")]
        public async Task<AuditBasicDetailsResponse> GetAuditBasicDetails(int id)
        {
            return await _manager.GetAuditBasicDetails(id);
        }

        [Right("cancel-audit")]
        [HttpPost("SaveCancelrescheduleAudit")]
        public async Task<SaveCancelRescheduleAuditResponse> SaveCancelrescheduleAudit([FromBody] AuditSaveCancelRescheduleItem request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            if (request == null)
                return new SaveCancelRescheduleAuditResponse { Result = SaveCancelAuditResult.RequestNotCorrectFormat };

            var response = await _manager.SaveAuditCancelDetails(request);

            AuditDetails emailRequest = new AuditDetails();
            emailRequest.AuditId = request.AuditId;
            emailRequest.FactoryId = response.FactoryId;
            emailRequest.CustomerId = response.CustomerId;

            SaveAuditResponse emailResponse = new SaveAuditResponse();
            emailResponse.Id = request.AuditId;

            if (request.Optypeid == (int)AuditBookingOperationType.Cancel)
            {
                emailRequest.StatusId = (int)AuditBookingStatus.Cancel;
                var bookingFbLog = new BookingFbLogData()
                {
                    BookingId = request.AuditId,
                    FbBookingSyncType = FbBookingSyncType.AuditBookingCancellation,
                    TryCount = 1
                };
                await PublishFbAuditBookingQueue(bookingFbLog);
                await SendEmailAndNotifications(emailRequest, emailResponse, broadCastService, configuration);
            }
            else if (request.Optypeid == (int)AuditBookingOperationType.Reschedule)
            {
                emailRequest.StatusId = (int)AuditBookingStatus.Rescheduled;
                emailRequest.ServiceDateFrom = response.PrevServiceDateFrom.GetCustomDate();
                emailRequest.ServiceDateTo = response.PrevServiceDateTo.GetCustomDate();
                var bookingFbLog = new BookingFbLogData()
                {
                    BookingId = request.AuditId,
                    FbBookingSyncType = FbBookingSyncType.AuditUpdation,
                    TryCount = 1,
                    IsMissionUpdated = true
                };
                await PublishFbAuditBookingQueue(bookingFbLog);

                await SendEmailAndNotifications(emailRequest, emailResponse, broadCastService, configuration);
            }
            return response;
        }

        [Right("audit-report")]
        [HttpGet("reportfiles/{id}")]
        public async Task<IActionResult> GetAuditReportFiles(int id)
        {
            var file = await _manager.GetAuditReport(id);

            if (file.Result == DTO.File.FileResult.NotFound)
                return NotFound();

            return File(file.Content, file.MimeType); // returns a FileStreamResult
        }
        [Right("audit-report")]
        [HttpPost("saveauditreport")]
        public async Task<SaveAuditReportResponse> SaveAuditReport([FromBody] AuditReportDetails request)
        {
            if (request == null)
                return new SaveAuditReportResponse { Result = SaveAuditReportResponseResult.RequestNotCorrectFormat };

            return await _manager.SaveAuditReportDetails(request);

        }
        [HttpGet("GetScheduledAuditor/{id}")]
        public async Task<AuditorResponse> GetScheduledAuditor(int id)
        {
            return await _manager.GetScheduledAuditors(id);
        }
        [HttpGet("GetAuditReportDetails/{id}")]
        [Right("audit-report")]
        public async Task<AuditReportSummary> GetAuditReportDetails(int id)
        {
            return await _manager.GetAuditReportSummary(id);
        }

        [HttpGet("GetAuditStatus")]
        [Right("audit-report")]
        public async Task<AuditStatusResponse> GetAuditStatus()
        {
            return await _manager.GetAuditStatuses();
        }

        [HttpGet("GetAuditServiceType/{customerid}")]
        [Right("audit-report")]
        public async Task<AuditServiceTypeResponse> GetAuditServiceType(int customerid)
        {
            return await _manager.GetAuditServiceType(customerid);
        }

        [HttpGet("GetProductCategory/{customerId},{serviceTypeId}")]
        public async Task<AuditProductCategoryResponse> GetProductCategory(int customerId, int serviceTypeId)
        {
            return await _manager.GetProductCategory(customerId, serviceTypeId);
        }

        private async Task PublishFbAuditBookingQueue(BookingFbLogData bookingFbLog)
        {
            var resultId = await _bookingEmailLogQueueManager.AddBookingFbQueueLog(bookingFbLog);
            var fbBookingRequest = new FbBookingRequest()
            {
                Id = Guid.NewGuid(),
                ResultId = resultId,
                TryCount = 1
            };
            await _rabbitMQClient.Publish<FbBookingRequest>(_configuration["FBBookingSyncQueue"], fbBookingRequest);
        }
    }
}
