using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BI.Maps.APP;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.EmailLog;
using DTO.EmailSend;
using DTO.EmailSendingDetails;
using DTO.File;
using DTO.Inspection;
using DTO.InspectionCustomerDecision;
using DTO.MobileApp;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;
using MobileResult = DTO.MobileApp.Result;

namespace LINK_UI.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InspReportMobileController : ControllerBase
    {
        private readonly IFBInternalReportManager _manager = null;
        private readonly IInspectionCustomerDecisionManager _cusDecisionManager = null;
        private readonly IEmailSendingDetailsManager _esManager = null;
        private static IConfiguration _configuration = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;

        public InspReportMobileController(IFBInternalReportManager manager,
            IInspectionCustomerDecisionManager cusDecisionManager,
            IEmailSendingDetailsManager esManager, IConfiguration configuration,
            IEmailLogQueueManager emailLogQueueManager, IRabbitMQGenericClient rabbitMQClient)
        {
            _manager = manager;
            _cusDecisionManager = cusDecisionManager;
            _esManager = esManager;
            _configuration = configuration;
            _emailLogQueueManager = emailLogQueueManager;
            _rabbitMQClient = rabbitMQClient;
        }

        [HttpPost("getInspReportMobileSummary")]
        [Right("report-summary")]
        public async Task<InspReportMobileResponse> GetInspectionReportSummary(InspSummaryMobileRequest request)
        {
            return await _manager.GetMobileInspectionReportSummary(request);
        }

        [Right("report-summary")]
        [HttpPost("GetBookingReportDetails")]
        public async Task<InspReportProductsMobileResponse> GetBookingProductsAndStatus(InspSummaryMobileRequest request)
        {
            return await _manager.GetMobileBookingProductsAndStatus(request);
        }

        [Right("report-summary")]
        [HttpPost("SaveCustomerDecision")]
        public async Task<CustomerDecisionSaveMobileResponse> SaveCustomerDecision(CustomerDecisionMobileSaveRequest request)
        {
            var result = await _cusDecisionManager.SaveMobileCustomerDecision(request);

            var dataListResponse = new List<EmailPreviewDataResponse>();
            if (result.meta.success)
            {
                var bookingId = await _cusDecisionManager.GetBookingIdByReportId(request.reportId);
                // get email info like booking and product and report details based on report id.
                var emailData = await _esManager.GetCustomerDecisionEmailConfigurationContacts(new[] { request.reportId }.ToList(), bookingId, request.emailFlag,request.resultId);
                if (emailData != null && emailData.Result == EmailPreviewResponseResult.success)
                {
                    int index = 1;

                    if (emailData.EmailBodyTempList == null || !emailData.EmailBodyTempList.Any())
                    {
                        result.data = MobileResult.noemailconfiguration;
                        return result;
                    }

                    foreach (var item in emailData.EmailBodyTempList)
                    {
                        var emailPreviewBody = new EmailPreviewDataResponse();

                        if (emailData.EmailToList == null || !emailData.EmailToList.Any() || string.IsNullOrEmpty(item.EmailSubject))
                        {
                            result.data = MobileResult.noemailconfiguration;
                            return result;
                        }

                        emailPreviewBody.EmailCCList = emailData.EmailCCList;
                        emailPreviewBody.EmailToList = emailData.EmailToList;
                        emailPreviewBody.EmailValidOption = item.EmailValidOption;
                        emailPreviewBody.EmailSubject = item.EmailSubject;
                        item.RecipientName = (!string.IsNullOrEmpty(item.RecipientName)) ? item.RecipientName : "All";
                        item.UserUploadFileList = new List<EmailAttachments>();

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
                            result.data = MobileResult.noemailconfiguration;
                            return result;
                        }

                        emailPreviewBody.ReportBookingList = item.ReportBookingList;
                        emailPreviewBody.EmailId = index;
                        dataListResponse.Add(emailPreviewBody);
                        index++;
                    }

                    var res = await SendCustomerDecisionEmail(dataListResponse);

                    result.data = res ? MobileResult.success : MobileResult.fail;
                }
                else
                {
                    result.data = MobileResult.noemailconfiguration;
                }
            }

            return result;
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
    }
}