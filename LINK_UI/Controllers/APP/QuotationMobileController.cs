using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BI.Maps.APP;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.Common;
using DTO.EmailLog;
using DTO.Inspection;
using DTO.MasterConfig;
using DTO.MobileApp;
using DTO.Quotation;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;

namespace LINK_UI.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class QuotationMobileController : ControllerBase
    {
        private readonly IQuotationManager _manager = null;
        private readonly IConfiguration _configuration = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IInspectionBookingManager _inspManager = null;
        public QuotationMobileController(IQuotationManager manager, IConfiguration configuration, IEmailLogQueueManager emailLogQueueManager,
            IRabbitMQGenericClient rabbitMQClient, IInspectionBookingManager inspManager)
        {
            _manager = manager;
            _configuration = configuration;
            _emailLogQueueManager = emailLogQueueManager;
            _rabbitMQClient = rabbitMQClient;
            _inspManager = inspManager;
        }

        [Right("quotation-pending")]
        [HttpPost("GetPendingQuotation")]
        public async Task<InspQuotationMobileResponse> GetPendingQuotation(InspSummaryMobileRequest request)
        {
            return await _manager.GetMobilePendingQuotation(request);
        }
        [Right("quotation-pending")]
        [HttpGet("GetQuotationStatus")]
        public FilterDataSourceResponse GetMobileQuotationStatus()
        {
            return _manager.GetMobileQuotationStatus();
        }
        [HttpGet("preview/{id}")]
        [Right("quotation-pending")]
        public async Task<IActionResult> Preview(int id, [FromServices] IQuotationPDF previewService)
        {
            var quotation = await _manager.GetQuotationDetails(id);

            if (quotation != null)
            {
                var document = previewService.CreateDocument(quotation);
                return File(document.Content, document.MimeType);
            }

            return NotFound();
        }
        [Right("quotation-save")]
        [HttpPost("SaveQuotation")]
        public async Task<QuotationValidateResponse> SaveQuotation(QuotationValidateRequest request)
        {
            var response = new QuotationValidateResponse();
            try
            {
                var masterConfigs = await _inspManager.GetMasterConfiguration();
                var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                string baseUrl = _configuration["BaseUrl"];

                var actionSendList = new Dictionary<QuotationStatus, Action<SendEmailRequest>>()
            {
                { QuotationStatus.CustomerValidated,(emailRequest) => this.SendEmail("Emails/Quotation/QuotationCustConfirmed", emailRequest, baseUrl, entityName) }
            };

                var requestManager = new SetStatusBusinessRequest
                {
                    Id = request.quotationId,
                    CusComment = request.comments,
                    IdStatus = (QuotationStatus)request.statusId,
                    OnSendEmail = actionSendList.TryGetValue((QuotationStatus)request.statusId, out Action<SendEmailRequest> post) ? post : null,
                    ApiRemark = "",
                    Url = baseUrl + string.Format(_configuration["UrlQuotation"], request.quotationId, entityName),
                    ApiInternalRemark = "",
                    ConfirmDate = Static_Data_Common.GetCustomDate(DateTime.Now)
                };

                var data = await _manager.SetStatus(requestManager);

                response.data = data.Result == SetStatusQuotationResult.Success ? Result.success : Result.fail;
                response.meta = new MobileResult { success = true, message = "Quotation Save successful" };
            }

            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Quotation Save failed." };
            }

            return response;
        }

        private void SendEmail(string ViewPath, SendEmailRequest request, string baseUrl, string entityName)
        {
            string url = baseUrl + string.Format(_configuration["UrlQuotation"], request.Model.Id, entityName);

            // Send Email 
            var emailRequest = new EmailRequest
            {
                Id = Guid.NewGuid(),
                Recepients = request.RecepientList,
                CCList = request.CcList,
                Subject = request.Subject,
                FileList = request.FileList,
                ReturnToUpdate = (id, state, error) => _manager.UpdateEmailState(id, (int)state, true)
            };

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
    }
}