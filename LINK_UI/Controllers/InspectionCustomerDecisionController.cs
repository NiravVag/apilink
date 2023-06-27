using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Components.Core.contracts;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.EmailLog;
using DTO.EmailSend;
using DTO.EmailSendingDetails;
using DTO.File;
using DTO.Inspection;
using DTO.InspectionCustomerDecision;
using DTO.MasterConfig;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InspectionCustomerDecisionController : ControllerBase
    {
        private readonly IInspectionCustomerDecisionManager _manager = null;
        private readonly IEmailSendingDetailsManager _esManager = null;

        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private static IConfiguration _configuration = null;
        private readonly ISharedInspectionManager _helper = null;
        private readonly IEmailManager _emailManager;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly ITenantProvider _filterService = null;
        public InspectionCustomerDecisionController(IInspectionCustomerDecisionManager manager,
            IEmailLogQueueManager emailLogQueueManager, IEmailSendingDetailsManager esManager, IRabbitMQGenericClient rabbitMQClient, IConfiguration configuration,
            ISharedInspectionManager helper, IEmailManager emailManager, IInspectionBookingManager inspManager, ITenantProvider filterService)
        {
            _manager = manager;
            _emailLogQueueManager = emailLogQueueManager;
            _rabbitMQClient = rabbitMQClient;
            _configuration = configuration;
            _esManager = esManager;
            _helper = helper;
            _emailManager = emailManager;
            _inspManager = inspManager;
            _filterService = filterService;
        }

        [Right("edit-booking")]
        [HttpGet("GetCustomerDecisionList/{customerId}")]
        public async Task<CustomerDecisionListResponse> GetCustomerDecisionList(int customerId)
        {
            return await _manager.GetCustomerDecisionList(customerId);
        }

        [Right("edit-booking")]
        [HttpGet("GetCustomerDecision/{reportId}")]
        public async Task<CustomerDecisionResponse> GetCustomerDecision(int reportId)
        {
            return await _manager.GetCustomerDecisionData(reportId);
        }

        [Right("edit-booking")]
        [HttpPost("SaveCustomerDecision")]
        public async Task<CustomerDecisionSaveResponse> SaveCustomerDecision(CustomerDecisionSaveRequest request)
        {
            var result = await _manager.AddCustomerDecision(request);
            var dataListResponse = new List<EmailPreviewDataResponse>();
            if (result.Result == CustomerDecisionSaveResponseResult.success)
            {
                // get email info like booking and product and report details based on report id.
                var emailData = await _esManager.GetCustomerDecisionEmailConfigurationContacts(new[] { request.ReportId }.ToList(), request.BookingId, request.sendEmailToFactoryContacts, request.CustomerResultId);
                if (emailData != null && emailData.Result == EmailPreviewResponseResult.success)
                {
                    int index = 1;

                    if (emailData.EmailBodyTempList == null || !emailData.EmailBodyTempList.Any())
                    {
                        result.Result = CustomerDecisionSaveResponseResult.noEmailBodyConfiguration;
                        return result;
                    }

                    var entityId = _filterService.GetCompanyId();
                    var _settings = _emailManager.GetMailSettingConfiguration(entityId);

                    foreach (var item in emailData.EmailBodyTempList)
                    {
                        var emailPreviewBody = new EmailPreviewDataResponse();

                        if (emailData.EmailToList == null || !emailData.EmailToList.Any())
                        {
                            result.Result = CustomerDecisionSaveResponseResult.noEmailRecipientsConfiguration;
                            return result;
                        }

                        if (string.IsNullOrEmpty(item.EmailSubject))
                        {
                            result.Result = CustomerDecisionSaveResponseResult.noEmailSubjectConfiguration;
                            return result;
                        }

                        emailPreviewBody.EmailCCList = emailData.EmailCCList;
                        emailPreviewBody.EmailToList = emailData.EmailToList;
                        emailPreviewBody.EmailValidOption = item.EmailValidOption;
                        emailPreviewBody.EmailSubject = item.EmailSubject;
                        item.RecipientName = (!string.IsNullOrEmpty(item.RecipientName)) ? item.RecipientName : "All";
                        item.UserUploadFileList = new List<EmailAttachments>();

                        item.SenderEmail = _settings.SenderEmail;
                        var reportNames = string.Join(", ", item.ReportList.Select(x => x.ReportName).Distinct().ToArray());
                        emailPreviewBody.RuleId = emailData.RuleId;
                        if (item.EmailValidOption == (int)EmailValidOption.EmailSizeExceed)
                        {
                            emailPreviewBody.EmailBody = "Report size exceeded for the " + reportNames + " reports.";
                        }
                        else
                        {
                            emailPreviewBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                            emailPreviewBody.EmailBody = this.GetEmailBody("Emails/CustomerDecision/CustomerDecision", item);
                        }

                        if (string.IsNullOrEmpty(emailPreviewBody.EmailBody))
                        {
                            result.Result = CustomerDecisionSaveResponseResult.noEmailBodyConfiguration;
                            return result;
                        }

                        emailPreviewBody.ReportBookingList = item.ReportBookingList;
                        emailPreviewBody.EmailId = index;
                        dataListResponse.Add(emailPreviewBody);
                        index++;
                    }

                    var res = await SendCustomerDecisionEmail(dataListResponse);

                    result.Result = res ? CustomerDecisionSaveResponseResult.success : CustomerDecisionSaveResponseResult.fail;
                }
                else
                {
                    result.Result = emailData?.Result == EmailPreviewResponseResult.multipleRuleFound ? CustomerDecisionSaveResponseResult.multipleRuleFound :
                    CustomerDecisionSaveResponseResult.noemailconfiguration;
                }
            }
            else if (result.Result == CustomerDecisionSaveResponseResult.noroleconfiguration)
            {
                result.Result = CustomerDecisionSaveResponseResult.noroleconfiguration;
                return result;
            }

            return result;
        }

        [Right("edit-booking")]
        [HttpGet("GetCustomerDecisionReportsData/{reportId}")]
        public async Task<ReportCustomerDecisionResponse> GetCustomerDecisionReportsData(int reportId)
        {
            return await _manager.GetCustomerDecisionReportsData(reportId);

        }

        /// <summary>
        /// Send Customer Decision email
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private async Task<bool> SendCustomerDecisionEmail(List<EmailPreviewDataResponse> request)
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
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Customer Decision Page",
                        Subject = emailTemplate.EmailSubject,
                        Body = emailTemplate.EmailBody
                    };

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
        /// publish to queue
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

        [HttpPost("CustomerDecisionSummary")]
        public async Task<CustomerDecisionReponse> CustomerDecisionSummary(CustomerDecisionSummaryRequest request)
        {
            return await _manager.CustomerDecisionSummary(request);
        }

        [HttpGet("Customer-Decision-Booking-Products/{bookingId}")]
        public async Task<EditCustomerDecisionResponse> GetCustomerDecisionBookingAndProducts(int bookingId)
        {
            return await _manager.GetCustomerDecisionBookingAndProducts(bookingId);
        }

        [Right("edit-booking")]
        [HttpPost("SaveCustomerDecisionList")]
        public async Task<CustomerDecisionSaveResponse> SaveCustomerDecisionList(CustomerDecisionListSaveRequest request)
        {
            var result = await _manager.SaveCustomerDecisionList(request);
            var dataListResponse = new List<EmailPreviewDataResponse>();

            if (result.Result == CustomerDecisionSaveResponseResult.success)
            {
                // check any email configuration
                var emailData = await _esManager.GetCustomerDecisionEmailConfigurationContacts(request.ReportIdList, request.BookingId, request.sendEmailToFactoryContacts, request.CustomerResultId);
                if (emailData != null && emailData.Result == EmailPreviewResponseResult.success)
                {
                    int index = 1;

                    if (emailData.EmailBodyTempList == null || !emailData.EmailBodyTempList.Any())
                    {
                        result.Result = CustomerDecisionSaveResponseResult.noEmailBodyConfiguration;
                        return result;
                    }

                    var bookingIds = new[] { request.BookingId }.ToList();
                    var serviceTypeList = await _inspManager.GetServiceType(bookingIds);
                    var serviceTypeIds = serviceTypeList.Select(x => x.serviceTypeId).ToList();

                    var customerDecisionTemplates = await _manager.GetCusDecisionTemplate();
                    var customerTemplate = customerDecisionTemplates.Where(x => x.IsDefault.Value);
                    if (customerDecisionTemplates.Any(x => x.CustomerId != null))
                    {
                        customerTemplate = customerDecisionTemplates.Where(x => x.CustomerId == request.CustomerResultId);
                    }

                    if (customerDecisionTemplates.Any(x => x.ServiceTypeId != null))
                    {
                        customerTemplate = customerDecisionTemplates.Where(x => serviceTypeIds.Contains(x.ServiceTypeId.GetValueOrDefault()));
                    }

                    var entityId = _filterService.GetCompanyId();
                    var _settings = _emailManager.GetMailSettingConfiguration(entityId);

                    foreach (var item in emailData.EmailBodyTempList)
                    {
                        var emailPreviewBody = new EmailPreviewDataResponse();

                        if (emailData.EmailToList == null || !emailData.EmailToList.Any())
                        {
                            result.Result = CustomerDecisionSaveResponseResult.noEmailRecipientsConfiguration;
                            return result;
                        }

                        if (string.IsNullOrEmpty(item.EmailSubject))
                        {
                            result.Result = CustomerDecisionSaveResponseResult.noEmailSubjectConfiguration;
                            return result;
                        }

                        emailPreviewBody.EmailCCList = emailData.EmailCCList;
                        emailPreviewBody.EmailToList = emailData.EmailToList;
                        emailPreviewBody.EmailValidOption = item.EmailValidOption;
                        emailPreviewBody.EmailSubject = item.EmailSubject;
                        item.RecipientName = (!string.IsNullOrEmpty(item.RecipientName)) ? item.RecipientName : "All";
                        item.UserUploadFileList = new List<EmailAttachments>();
                        item.SenderEmail = _settings.SenderEmail;
                        var reportNames = string.Join(", ", item.ReportList.Select(x => x.ReportName).Distinct().ToArray());
                        emailPreviewBody.RuleId = emailData.RuleId;
                        if (item.EmailValidOption == (int)EmailValidOption.EmailSizeExceed)
                        {
                            emailPreviewBody.EmailBody = "Report size exceeded for the " + reportNames + " reports.";
                        }
                        else
                        {
                            emailPreviewBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                            if (customerTemplate.FirstOrDefault() != null)
                            {
                                var templatePath = customerTemplate.FirstOrDefault().TemplatePath;
                                emailPreviewBody.EmailBody = this.GetEmailBody(templatePath, item);
                            }
                            else
                            {
                                emailPreviewBody.EmailBody = this.GetEmailBody("Emails/CustomerDecision/CustomerDecision", item);
                            }
                        }

                        if (string.IsNullOrEmpty(emailPreviewBody.EmailBody))
                        {
                            result.Result = CustomerDecisionSaveResponseResult.noEmailBodyConfiguration;
                            return result;
                        }

                        emailPreviewBody.ReportBookingList = item.ReportBookingList;
                        emailPreviewBody.AttachmentList = item.AttachmentList;
                        emailPreviewBody.EmailId = index;
                        dataListResponse.Add(emailPreviewBody);
                        index++;
                    }

                    var res = await SendCustomerDecisionEmail(dataListResponse);

                    result.Result = res ? CustomerDecisionSaveResponseResult.success : CustomerDecisionSaveResponseResult.fail;
                }
                else
                {
                    result.Result = emailData?.Result == EmailPreviewResponseResult.multipleRuleFound ? CustomerDecisionSaveResponseResult.multipleRuleFound :
                    CustomerDecisionSaveResponseResult.noemailconfiguration;
                }
            }
            return result;
        }

        [HttpGet("Customer-Decision-Problematic-Remarks/{id}/{reportId}")]
        public async Task<CusDecisionProblematicRemarksResponse> GetProblematicRemarksByReport(int id, int reportId)
        {
            return await _manager.GetProblematicRemarksByReport(id, reportId);
        }

        [HttpPost("export-customer-decision")]
        public async Task<IActionResult> ExportCustomerDecisionSummary(CustomerDecisionSummaryRequest request)
        {
            Stream stream = null;
            var res = await _manager.ExportCustomerDecisionSummary(request);
            stream = _helper.GetAsStreamObjectAndLoadDataTable(res);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer_Decision_Export.xlsx");
        }
    }
}
