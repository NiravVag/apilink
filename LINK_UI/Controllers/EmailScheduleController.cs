using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.EmailLog;
using DTO.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailScheduleController : ControllerBase
    {
        private readonly IEmailScheduleManager _emailScheduleManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private static IConfiguration _configuration = null;

        public EmailScheduleController(IEmailScheduleManager emailScheduleManager, IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IConfiguration configuration)
        {
            _emailScheduleManager = emailScheduleManager;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _configuration = configuration;
        }

        [HttpGet("getScheduleQCEmail")]

        public async Task<bool> getScheduleQCEmail(string offices = "")
        {
            bool isSuccess = false;
            bool isFromScheduler = true;
            var scheduleQCEmailTemplateList = await _emailScheduleManager.scheduleQCEmail(isFromScheduler, null, offices);

            foreach (var scheduleQCEmailTemplate in scheduleQCEmailTemplateList)
            {
                var emailLogRequest = new EmailLogData()
                {
                    ToList = scheduleQCEmailTemplate.QCEmailID,
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "ScheduleEmail From Scheduler",
                    //SourceId = response.Id,
                    Subject = $"Inspection Daily Schedule(" + scheduleQCEmailTemplate.ServiceFromDate.ToString("dd/MM/yyyy") + " to " + scheduleQCEmailTemplate.ServiceToDate.ToString("dd/MM/yyyy") + ")",
                };

                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/Scheduling/ScheduleEmailToQC", scheduleQCEmailTemplate);
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            isSuccess = true;
            return isSuccess;
        }

        /// <summary>
        /// Send Schedule email from the schedule summary page
        /// </summary>
        /// <param name="emailQueueRequest"></param>
        /// <param name="emailLogRequest"></param>
        /// <returns></returns>
        [HttpPost("getScheduleQCEmail")]
        public async Task<bool> SendEmailFromUI([FromBody] List<int> bookingIds)
        {
            bool isSuccess = false;
            bool isFromScheduler = false;
            var scheduleQCEmailTemplateList = await _emailScheduleManager.scheduleQCEmail(isFromScheduler, bookingIds, null);

            if (scheduleQCEmailTemplateList.Count > 0)
            {

                foreach (var scheduleQCEmailTemplate in scheduleQCEmailTemplateList)
                {
                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = scheduleQCEmailTemplate.QCEmailID,
                        Cclist = scheduleQCEmailTemplate.CurrentUserEmailID,
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "ScheduleEmail From UI",
                        //SourceId = response.Id,
                        Subject = $"Inspection Schedule Modified (" + scheduleQCEmailTemplate.ServiceFromDate.ToString("dd/MM/yyyy") + " - " + scheduleQCEmailTemplate.ServiceToDate.ToString("dd/MM/yyyy") + ")",
                    };

                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    emailLogRequest.Body = this.GetEmailBody("Emails/Scheduling/ScheduleEmailToQC", scheduleQCEmailTemplate);
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

    }
}