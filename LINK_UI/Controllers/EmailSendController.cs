using Components.Core.contracts;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.Common;
using DTO.CommonClass;
using DTO.EmailLog;
using DTO.EmailSend;
using DTO.File;
using DTO.MasterConfig;
using Entities.Enums;
using LINK_UI.App_start;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class EmailSendController : ControllerBase
    {
        private readonly IEmailSendManager _manager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private static IConfiguration _configuration = null;
        private readonly IEmailManager _emailManager;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        public EmailSendController(IEmailSendManager manager, IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IConfiguration configuration,
            IEmailManager emailManager, IInspectionBookingManager inspManager, ITenantProvider filterService, IAPIUserContext ApplicationContext)
        {
            _manager = manager;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _configuration = configuration;
            _emailManager = emailManager;
            _inspManager = inspManager;
            _filterService = filterService;
            _ApplicationContext = ApplicationContext;
        }

        [HttpGet("booking-status-data")]
        public async Task<DataSourceResponse> GetBookingStatusList()
        {
            return await _manager.GetBookingStatusList();
        }

        [HttpGet("ae-list")]
        public async Task<AeListResponse> GetAEList()
        {
            return await _manager.GetAeList();
        }

        [HttpPost("email-send-summary-search")]
        [Right("EmailSendSummary")]
        public async Task<EmailSendSummaryResponse> GetEmailSendSummary([FromBody] EmailSendSummaryRequest request)
        {
            return await _manager.GetEmailSendSummary(request);
        }

        [HttpPost("booking-report-details")]
        [Right("EmailSendSummary")]
        public async Task<EmailSendBookingReportResponse> GetBookingReportDetails(BookingReportRequest request)
        {
            return await _manager.GetBookingReportDetails(request);
        }


        [HttpPost("invoice-details")]
        [Right("EmailSendSummary")]
        public async Task<EmailSendInvoiceResponse> GetInvoiceDetails(EmailRuleRequestByInvoiceNumbers request)
        {
            return await _manager.GetBookingInvoiceDetails(request);
        }

        [HttpDelete("{Id}")]
        public async Task<DeleteResponse> Delete(int Id)
        {
            return await _manager.Delete(Id);
        }

        [HttpDelete("delete-invoice-file/{Id}")]
        public async Task<DeleteResponse> DeleteInvoiceFile(int Id)
        {
            return await _manager.DeleteInvoiceFile(Id);
        }

        [HttpPost("email-send-file-details")]
        [Right("EmailSendSummary")]
        public async Task<EmailSendFileListResponse> GetEmailSendFileDetails(BookingReportRequest request)
        {
            return await _manager.GetEmailSendFileDetails(request);
        }

        [HttpPost("invoice-send-file-details")]
        [Right("EmailSendSummary")]
        public async Task<EmailSendFileListResponse> GetInvoiceSendFileDetails(InvoiceSendFilesRequest request)
        {
            return await _manager.GetInvoiceSendFileDetails(request);
        }

        [HttpGet("get-file-type-data")]
        public async Task<DataSourceResponse> GetFileTypeList()
        {
            return await _manager.GetFileTypeList();
        }

        [HttpGet("get-invoice-file-types")]
        public async Task<DataSourceResponse> GetInvoiceFileTypeList()
        {
            return await _manager.GetInvoiceFileTypeList();
        }

        [HttpPost("saveInvoiceAttachments")]
        public async Task<EmailSendFileUploadResponse> Save(InvoiceSendFileUpload request)
        {
            return await _manager.SaveInvoiceAttachments(request);
        }

        [HttpPost("save")]
        public async Task<EmailSendFileUploadResponse> Save(EmailSendFileUpload request)
        {
            return await _manager.Save(request);
        }

        [HttpGet("email-multiple-send-validation/{customerId}/{serviceId}")]
        public async Task<ReportSendTypeResponse> ValidateMutipleEmailSend(int customerId, int serviceId)
        {
            return await _manager.ValidateMultipleEmailSendByCustomer(customerId, 1);
        }

        [HttpPost("ExportEmailSendSummary")]
        public async Task<IActionResult> ExportEmailSendSummary(EmailSendSummaryRequest request)
        {
            var response = await _manager.GetEmailSendSummary(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("EmailSendSummary", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("get-email-rule-data")]
        public async Task<EmailRuleResponse> GetEmailRuleData(BookingReportRequest request)
        {
            return await _manager.GetEmailRuleData(request);
        }

        [HttpPost("get-email-rule-data-by-invoicelist")]
        public async Task<EmailRuleResponse> GetEmailRuleDataByInvoiceList(EmailRuleRequestByInvoiceNumbers request)
        {

            var bookingNumbers = await _manager.GetBookingNumbersByInvoice(request.InvoiceList);

            BookingReportRequest newEmailRuleRequest = new BookingReportRequest();
            newEmailRuleRequest.BookingIdList = bookingNumbers;
            newEmailRuleRequest.EmailSendingtype = request.EmailSendingtype;
            newEmailRuleRequest.ServiceId = request.ServiceId;
            newEmailRuleRequest.InvoiceType = request.InvoiceType;

            return await _manager.GetEmailRuleData(newEmailRuleRequest);
        }

        [HttpPost("email-info-by-email-rule")]
        public async Task<EmailDataResponse> FetchEmaildetailsbyEmailRule(EmailPreviewRequest request)
        {

            // set invoice send request format.
            if (request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
            {
                var invoiceNoList = request.EmailReportPreviewData.Select(x => x.InvoiceNo).Distinct().ToList();
                var bookingAndInvoiceMapping = await _manager.GetBookingDataByInvoiceNoList(invoiceNoList);
                if (bookingAndInvoiceMapping.Any())
                {
                    request.EmailReportPreviewData = new List<EmailReportPreviewDetail>();
                    foreach (var item in bookingAndInvoiceMapping)
                    {
                        request.EmailReportPreviewData.Add(new EmailReportPreviewDetail()
                        {
                            InvoiceNo = item.InvoiceNo,
                            BookingId = item.bookingId.GetValueOrDefault()
                        });
                    }
                }
            }

            var response = await _manager.FetchEmaildetailsbyEmailRule(request);
            var emailDataResponse = new EmailDataResponse();
            var dataListResponse = new List<EmailPreviewDataResponse>();
            int index = 1;

            var entityId = _filterService.GetCompanyId();
            var _settings = _emailManager.GetMailSettingConfiguration(entityId);

            foreach (var item in response.EmailBodyTempList)
            {
                var emailPreviewBody = new EmailPreviewDataResponse();
                emailPreviewBody.EmailCCList = response.EmailCCList;
                emailPreviewBody.EmailToList = response.EmailToList;
                emailPreviewBody.EmailBCCList = response.EmailBCCList;
                emailPreviewBody.EmailValidOption = item.EmailValidOption;
                emailPreviewBody.EmailSubject = item.EmailSubject;
                emailPreviewBody.CustomerId = item.CustomerId;
                item.SenderEmail = _settings.SenderEmail;

                item.RecipientName = (!string.IsNullOrEmpty(item.RecipientName)) ? item.RecipientName : "All";
                item.UserUploadFileList = new List<EmailAttachments>();
                if (request.EmailReportAttachment.Any())
                {
                    var reportIds = item.ReportList.Select(x => x.ReportId).ToList();
                    var bookingIds = item.ReportList.Select(x => x.BookingId).ToList();

                    var allLevelAttachments = request.EmailReportAttachment.Where(x => x.BookingId == null && x.ReportId == null).ToList();
                    var reportLevelAttachments = request.EmailReportAttachment.
                        Where(x => x.BookingId == null && x.ReportId != null && reportIds.Contains(x.ReportId.GetValueOrDefault())).ToList();
                    var bookingLevelAttachments = request.EmailReportAttachment.Where(x => x.BookingId != null
                    && x.ReportId == null && bookingIds.Contains(x.BookingId.GetValueOrDefault())).ToList();

                    var bothLevelAttachments = request.EmailReportAttachment.Where(x => x.BookingId != null && x.ReportId != null &&
                    bookingIds.Contains(x.BookingId.GetValueOrDefault()) && reportIds.Contains(x.ReportId.GetValueOrDefault())).ToList();

                    if (allLevelAttachments.Any())
                    {
                        foreach (var reportAttachment in allLevelAttachments)
                        {
                            item.UserUploadFileList.Add(new EmailAttachments()
                            {
                                FileLink = reportAttachment.FileLink,
                                FileName = reportAttachment.FileName,
                                FileType = reportAttachment.FileType
                            });
                        }
                    }

                    if (reportLevelAttachments.Any())
                    {
                        foreach (var reportAttachment in reportLevelAttachments)
                        {
                            item.UserUploadFileList.Add(new EmailAttachments()
                            {
                                FileLink = reportAttachment.FileLink,
                                FileName = reportAttachment.FileName,
                                FileType = reportAttachment.FileType
                            });
                        }
                    }

                    if (bookingLevelAttachments.Any())
                    {
                        foreach (var reportAttachment in bookingLevelAttachments)
                        {
                            item.UserUploadFileList.Add(new EmailAttachments()
                            {
                                FileLink = reportAttachment.FileLink,
                                FileName = reportAttachment.FileName,
                                FileType = reportAttachment.FileType
                            });
                        }
                    }

                    if (bothLevelAttachments.Any())
                    {
                        foreach (var reportAttachment in bothLevelAttachments)
                        {
                            item.UserUploadFileList.Add(new EmailAttachments()
                            {
                                FileLink = reportAttachment.FileLink,
                                FileName = reportAttachment.FileName,
                                FileType = reportAttachment.FileType
                            });
                        }
                    }
                }

                var reportNames = string.Join(", ", item.ReportList.Select(x => x.ReportName).Distinct().ToArray());
                emailPreviewBody.RuleId = response.RuleId;

                if (item.EmailValidOption == (int)EmailValidOption.EmailSizeExceed)
                {
                    emailPreviewBody.EmailBody = "Report size exceeded for the " + reportNames + " reports.";
                    emailPreviewBody.ReportBookingList = item.ReportBookingList;
                }
                else if (request.EsTypeId == (int)EmailSendingType.ReportSend)
                {
                    emailPreviewBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                    emailPreviewBody.EmailBody = this.GetEmailBody("Emails/ReportSend/ReportSendEmail", item);
                    emailPreviewBody.ReportBookingList = item.ReportBookingList;
                }
                else if (request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                {

                    // Add Invoice level attachments data while preview email
                    item.UserUploadFileList = new List<EmailAttachments>();
                    var invoiceAttachments = request.EmailReportAttachment.Where(x => x.InvoiceNo != null).ToList();
                    if (invoiceAttachments.Any())
                    {
                        foreach (var invoiceAttachment in invoiceAttachments)
                        {
                            if (invoiceAttachment != null)
                            {
                                item.UserUploadFileList.Add(new EmailAttachments()
                                {
                                    FileLink = invoiceAttachment.FileLink,
                                    FileName = invoiceAttachment.FileName,
                                    FileType = invoiceAttachment.FileType
                                });
                            }
                        }
                    }

                    emailPreviewBody.RuleId = request.EmailRuleId;
                    emailPreviewBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                    var masterConfigs = await _inspManager.GetMasterConfiguration();
                    var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                    emailPreviewBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                    item.EntityName = entityName;
                    item.PreInvoiceEmailContent1 = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.PreInvoiceEmailContent1).Select(x => x.Value).FirstOrDefault();
                    item.PreInvoiceEmailContent2 = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.PreInvoiceEmailContent2).Select(x => x.Value).FirstOrDefault();
                    emailPreviewBody.InvoiceNo = item.InvoiceNumber;
                    emailPreviewBody.EmailBody = this.GetEmailBody("Emails/InvoiceSend/InvoiceSend", item);

                    // set booking response for email send
                    foreach (var bookingData in request.EmailReportPreviewData)
                    {
                        emailPreviewBody.ReportBookingList = new List<ReportBooking>();
                        emailPreviewBody.ReportBookingList.Add(new ReportBooking()
                        { InspectionId = bookingData.BookingId, EsTypeId = request.EsTypeId, ReportRevision = bookingData.ReportRevision, ReportVersion = bookingData.ReportVersion });
                    }
                }

                emailPreviewBody.AttachmentList = item.AttachmentList;
                emailPreviewBody.EmailId = index;
                dataListResponse.Add(emailPreviewBody);
                index++;
            }

            emailDataResponse.Result = response.Result;
            emailDataResponse.Data = dataListResponse;

            return emailDataResponse;
        }

        [HttpPost("send-email")]
        public async Task<bool> SendEmail([FromBody] List<EmailPreviewDataResponse> request)
        {
            bool isSuccess = false;

            if (request.Any())
            {

                foreach (var emailTemplate in request)
                {
                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = String.Join(";", emailTemplate.EmailToList),
                        Cclist = String.Join(";", emailTemplate.EmailCCList),
                        Bcclist = String.Join(";", emailTemplate.EmailBCCList),
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Email Send Page",
                        Subject = emailTemplate.EmailSubject,
                        Body = emailTemplate.EmailBody
                    };

                    if (emailTemplate.AttachmentList.Any())
                    {
                        var fileList = new List<FileResponse>();

                        foreach (var item in emailTemplate.AttachmentList)
                        {
                            fileList.Add(new FileResponse() { FileLink = item.FileLink, Name = item.FileName, FileStorageType = (int)FileStorageType.Link, MimeType = item.FileType });
                        }

                        emailLogRequest.FileList = fileList;
                    }

                    // Add email booking report list
                    if (emailTemplate.ReportBookingList.Any())
                    {
                        emailLogRequest.BookingReportList = emailTemplate.ReportBookingList;
                    }

                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    await PublishQueueMessage(emailQueueRequest, emailLogRequest);
                }
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
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

        [HttpGet("getemailsendhistory/{inspectionId}/{reportId}/{EmailTypeId}")]
        public async Task<EmailSendHistoryResponse> GetEmailSendHistory(int inspectionId, int reportId, int EmailTypeId)
        {
            return await _manager.GetEmailSendHistory(inspectionId, reportId, EmailTypeId);
        }


        [HttpGet("getreportVersion/{apiReportId}/{fbReportId}/{requestVersion}")]
        public async Task<FbReportRevisionNoResponse> GetFBReportVersion(int apiReportId, int fbReportId, int requestVersion)
        {
            var fbToken = getFbToken();
            return await _manager.SetFbReportVersion(apiReportId, fbReportId, requestVersion, fbToken);
        }

        [HttpGet("checkFbReportIsInvalidated/{fbReportId}")]
        public async Task<bool> getFbReportStatus(int fbReportId)
        {
            var fbToken = getFbToken();
            return await _manager.CheckFbReportIsInvalidated(fbReportId, fbToken);
        }

        private string getFbToken()
        {
            var Fbclaims = new List<Claim>
            {
                new Claim("email",_configuration["FbAdminEmail"]),
                new Claim("firstname", _configuration["FbAdminUserName"]),
                new Claim("lastname", ""),
                new Claim("role", "admin"),
                new Claim("redirect", "")
            };
            return AuthentificationService.CreateFBToken(Fbclaims, _configuration["FBKey"]);

        }

        [HttpPost("auto-customer-decision")]
        public async Task<AutoCustomerDecisionResponse> AutoCustomerDecisionList(AutoCustomerDecisionRequest request)
        {
            return await _manager.AutoCustomerDecisionList(request);
        }
    }
}