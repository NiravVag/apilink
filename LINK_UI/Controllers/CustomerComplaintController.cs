using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Core.entities.Emails;
using Contracts.Managers;
using DTO.Customer;
using DTO.References;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO.EmailLog;
using RabbitMQUtility;
using Microsoft.Extensions.Configuration;
using Components.Web;
using DTO.Inspection;
using Components.Core.contracts;
using DTO.MasterConfig;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CustomerComplaintController : ControllerBase
    {
        private readonly ICustomerComplaintManager _manager = null;
        private static IConfiguration _configuration = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private readonly ISharedInspectionManager _helper = null;
        private readonly IEmailManager _emailManager;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly ITenantProvider _filterService = null;
        public CustomerComplaintController(ICustomerComplaintManager manager, IConfiguration configuration, IRabbitMQGenericClient rabbitMQClient,
            IEmailLogQueueManager emailLogQueueManager, ISharedInspectionManager helper, IEmailManager emailManager, IInspectionBookingManager inspManager, ITenantProvider filterService)
        {
            _manager = manager;
            _configuration = configuration;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _helper = helper;
            _emailManager = emailManager;
            _inspManager = inspManager;
            _filterService = filterService;
        }
        [HttpGet("ComplaintDetailsById/{id}")]
        public async Task<ComplaintDetailedInfoResponse> GetComplaintDetailsById(int id)
        {
            return await _manager.GetComplaintDetailsById(id);
        }
        [HttpGet("getComplaintType")]
        [Right("customer-summary")]
        public async Task<ComplaintDataResponse> getComplaintType()
        {
            return await _manager.GetComplaintType();
        }
        [HttpGet("getComplaintCategory")]
        [Right("customer-summary")]
        public async Task<ComplaintDataResponse> getComplaintCategory()
        {
            return await _manager.GetComplaintCategory();
        }
        [HttpGet("getComplaintRecipientType")]
        [Right("customer-summary")]
        public async Task<ComplaintDataResponse> getComplaintRecipientType()
        {
            return await _manager.GetComplaintRecipientType();
        }
        [HttpGet("getComplaintDepartment")]
        [Right("customer-summary")]
        public async Task<ComplaintDataResponse> GetComplaintDepartment()
        {
            return await _manager.GetComplaintDepartment();
        }
        [HttpPost("GetBookingNoDatasource")]
        public async Task<BookingNoDataSourceResponse> GetBookingNoDatasource(BookingNoDataSourceRequest request)
        {
            return await _manager.GetBookingNoDataSource(request);
        }

        [HttpGet("GetBookingProductDetails/{bookingId}")]
        [Right("customer-summary")]
        public async Task<ComplaintBookingProductDataResponse> GetBookingProductDetails(int bookingId)
        {
            return await _manager.GetProductItemByBooking(bookingId);
        }

        [HttpGet("GetAuditDetails/{auditId}")]
        [Right("customer-summary")]
        public async Task<ComplaintBookingDataResponse> GetAuditDetails(int auditId)
        {
            return await _manager.GetAuditDetails(auditId);
        }
        [HttpPost]
        [Right("customer-summary")]
        public async Task<SaveComplaintResponse> SaveComplaints([FromBody] ComplaintDetailedInfo request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            SaveComplaintResponse response = await _manager.SaveComplaints(request);
            try
            {
                if (response.Result == ComplaintResult.Success && request.Id == 0 && response.Id > 0)// Send mail for New Complaint
                {
                    await SendEmailAndNotifications(request, response, broadCastService, configuration);
                }
            }
            catch (Exception)//errr on email send
            {
                response.Result = ComplaintResult.ComplaintSavedNotificationError;
            }
            return response;

        }
        /// <summary>
        /// Send email and notifications
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="broadCastService"></param>
        /// <returns></returns>
        private async Task SendEmailAndNotifications(ComplaintDetailedInfo request, SaveComplaintResponse response, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            var mailData = await _manager.GetComplaintEmailData(request, response.Id);

            var entityId = _filterService.GetCompanyId();
            var _settings = _emailManager.GetMailSettingConfiguration(entityId);
            mailData.SenderEmail = _settings.SenderEmail;

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
                ToList = mailData.StaffEmailID,
                Cclist = mailData.CurrentUserEmailID,
                TryCount = 1,
                Status = (int)EmailStatus.NotStarted,
                SourceName = "Complaint Register",
                SourceId = response.Id,
                Subject = "New Complaint Registered"
            };
            emailLogRequest.Body = this.GetEmailBody("Emails/Complaint/ComplaintRegister", (mailData, baseUrl + string.Format(configuration["UrlComplaintRegister"], response.Id, entityName)));
            await PublishQueueMessage(emailQueueRequest, emailLogRequest);
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
        [HttpGet("RemoveComplaintDetail/{id}")]
        public async Task<RemoveComplaintDetailResponse> RemoveComplaintDetail(int id)
        {
            return await _manager.RemoveComplaintDetail(id);
        }

        [HttpPost("GetComplaintSummary")]
        public async Task<ComplaintSummaryResponse> GetComplaintSummary(ComplaintSummaryRequest request)
        {
            return await _manager.GetComplaintSummary(request);
        }

        [HttpDelete("{Id}")]
        public async Task<DeleteComplaintResponse> Delete(int id)
        {
            return await _manager.DeleteComplaint(id);
        }

        [HttpPost("ExportComplaintSummary")]
        public async Task<IActionResult> ExportComplaintSummary(ComplaintSummaryRequest request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.pageSize = PageSize;
            var response = await _manager.GetComplaintDetails(request);
            var data = await _manager.ExportCompalintSummary(response.Data);
            if (response.Result != ComplaintResult.Success)
                return NotFound();

            var stream = _helper.GetAsStreamObject(data);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ComplaintSearchSummary.xls");
        }
    }
}