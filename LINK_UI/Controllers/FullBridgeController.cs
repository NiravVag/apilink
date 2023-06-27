using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Core.contracts;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.EmailLog;
using DTO.EmailSend;
using DTO.FullBridge;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "FbUserPolicy")]
    public class FullBridgeController : ControllerBase
    {
        private readonly IFullBridgeManager _fbManager = null;
        private readonly ITenantProvider _filterService;
        private readonly IEmailLogQueueManager _emailLogQueueManager;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IConfiguration _configuration;
        private readonly IEmailSendingDetailsManager _esManager = null;
        private readonly IEmailManager _emailManager;

        public FullBridgeController(IFullBridgeManager fbManager, ITenantProvider filterService, IEmailLogQueueManager emailLogQueueManager,
            IRabbitMQGenericClient rabbitMQClient, IConfiguration configuration, IEmailSendingDetailsManager esManager, IEmailManager emailManager)
        {
            _fbManager = fbManager;
            _filterService = filterService;
            _emailLogQueueManager = emailLogQueueManager;
            _rabbitMQClient = rabbitMQClient;
            _configuration = configuration;
            _esManager = esManager;
            _emailManager = emailManager;
        }

        // POST: api/FullBridge/Status
        [HttpPost("Status/{reportId}")]
        public async Task<FbStatusResponse> Post(int reportId, [FromBody] FbStatusRequest request)
        {
            return await _fbManager.UpdateFBFillingAndReviewStatus(reportId, request, false);
        }

        // POST: api/FullBridge/ReportInfo
        [HttpPost("ReportInfo/{reportId}")]
        public async Task<FbStatusResponse> SaveReportInformation(int reportId, [FromBody] FbReportDataRequest request)
        {
            var result = await _fbManager.SaveFBReportDetails(reportId, request);
            if (result.IsNewReportFormatCheckPoint)
            {
                await PublishFastReportQueueMessage(new List<int>() { result.ReportId }, result.InspectionId);
            }
            return result;
        }

        /// <summary>
        /// push the fast repot queue
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        private async Task PublishFastReportQueueMessage(List<int> fbReportIds, int? bookingId)
        {
            FastReportRequest fastReportRequest = new FastReportRequest();
            fastReportRequest.ReportIds = fbReportIds;
            fastReportRequest.BookingId = bookingId;
            fastReportRequest.EntityId = _filterService.GetCompanyId();

            await _rabbitMQClient.Publish<FastReportRequest>(_configuration["FastReportQueue"], fastReportRequest);
        }
    }
}
