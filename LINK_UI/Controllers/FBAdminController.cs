using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Components.Core.entities.Emails;
using Contracts.Managers;
using DTO.Common;
using DTO.EmailLog;
using DTO.FullBridge;
using LINK_UI.App_start;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQUtility;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class FBReportController : ControllerBase
    {

        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IFBReportManager _fbManager = null;
        private static IConfiguration _configuration = null;
        private readonly ILogger<FBReportController> _logger;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly ITenantProvider _filterService;

        public FBReportController(IFBReportManager manager, IAPIUserContext applicationContext, ITenantProvider filterService,
            IConfiguration configuration, ILogger<FBReportController> logger, IRabbitMQGenericClient rabbitMQClient)
        {
            _fbManager = manager;
            _ApplicationContext = applicationContext;
            _configuration = configuration;
            _logger = logger;
            _rabbitMQClient = rabbitMQClient;
            _filterService = filterService;
        }

        [HttpGet("UpdateFBBookingDetails/{bookingId}")]
        [Right("schedule-allocation")]
        public async Task<bool> UpdateFBBookingDetails(int bookingId)
        {
            string strFbToken = getFbToken();
            return await _fbManager.UpdateFBBookingDetails(bookingId, strFbToken);
        }

        [HttpGet("UpdateFBProductDetails/{bookingId}")]
        [Right("schedule-allocation")]
        public async Task<bool> UpdateFBProductDetails(int bookingId)
        {
            string strFbToken = getFbToken();
            return await _fbManager.UpdateFBProductDetails(bookingId, strFbToken);
        }

        [HttpGet("CreateFBMission/{bookingId}")]
        [Right("schedule-allocation")]
        public async Task<bool> CreateFullBridgeMission(int bookingId)
        {
            string strFbToken = getFbToken();
            return await _fbManager.CreateFBMission(bookingId, strFbToken);
        }

        [HttpGet("DeleteFBMission/{bookingId}")]
        [Right("schedule-allocation")]
        public async Task<SaveMissionResponse> DeleteFullBridgeMission(int bookingId)
        {
            string strFbToken = getFbToken();
            return await _fbManager.DeleteFBMission(bookingId, 0, strFbToken);
        }

        [HttpGet("CreateFBReport/{bookingId}/{productId}")]
        [Right("schedule-allocation")]
        public async Task<bool> CreateFullBridgeReport(int bookingId, int productId)
        {
            string strFbToken = getFbToken();
            return await _fbManager.CreateFBReport(bookingId, strFbToken, productId);
        }

        [HttpGet("DeleteFBReport/{bookingId}/{fbReportId}/{apiReportId}")]
        [Right("schedule-allocation")]
        public async Task<DeleteReportResponse> DeleteFullBridgeReport(int bookingId, int fbReportId, int apiReportId)
        {
            string strFbToken = getFbToken();
            return await _fbManager.DeleteFBReport(bookingId, fbReportId, strFbToken, apiReportId);
        }

        [HttpGet("FetchFBReport/{fbReportId}/{apiReportId}")]
        [Right("schedule-allocation")]
        public async Task<UpdateReportResponse> FetchFBReport(int fbReportId, int apiReportId)
        {
            string strFbToken = getFbToken();
            var result = await _fbManager.FetchFBReport(fbReportId, strFbToken, apiReportId);
            if (result.IsNewReportFormatCheckPoint)
            {
                await PublishFastReportQueueMessage(new List<int>() { apiReportId }, result.InspectionId);
            }
            return result;
        }

        [HttpGet("fetch-fb-report-by-booking/{bookingId}/{option}")]
        [Right("schedule-allocation")]
        public async Task<UpdateReportResponse> FetchFBReportByBooking(int bookingId, int option)
        {
            string strFbToken = getFbToken();

            var response = await _fbManager.FetchFBReportByBooking(bookingId, option, strFbToken);

            if (response != null && response.FastReportIds != null && response.FastReportIds.Any())
            {
                await PublishFastReportQueueMessage(response.FastReportIds.Select(y => y.FbReportId).ToList(), bookingId);
            }

            if (response.Result == UpdateReportResponseResult.ReportFetchMax)
            {
                foreach (var reports in response.ReportIds)
                {
                    if (reports.ApiReportId > 0)
                    {
                        var fbReportFetchRequest = new FbReportFetchRequest()
                        {
                            Id = Guid.NewGuid(),
                            ReportId = reports.ApiReportId,
                            FbReportId = reports.FbReportId
                        };
                        await _rabbitMQClient.Publish<FbReportFetchRequest>(_configuration["FbReportQueue"], fbReportFetchRequest);
                    }
                }

                return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportSyncShortly };
            }

            return response;
        }

        [HttpPost("FetchFBReportBulk")]
        [Right("schedule-allocation")]
        public async Task<FBBulkReportResponse> FetchFBReportBulk(List<ReportIdData> reportList, string password)
        {

            if (password == "api!@#$%^&*()_+")
            {
                if (reportList.Any())
                {
                    foreach (var report in reportList)
                    {

                        if (report.ApiReportId > 0)
                        {
                            var fbReportFetchRequest = new FbReportFetchRequest()
                            {
                                Id = Guid.NewGuid(),
                                ReportId = report.ApiReportId,
                                FbReportId = report.FbReportId
                            };
                            await _rabbitMQClient.Publish<FbReportFetchRequest>(_configuration["FbReportQueue"], fbReportFetchRequest);
                        }
                    }
                }
                else
                {
                    return new FBBulkReportResponse() { Result = "Report List is empty, Please try again." };
                }
            }
            else
            {
                return new FBBulkReportResponse() { Result = "Password is not Valid Please try again." };
            }

            return new FBBulkReportResponse() { Result = "All the reports updated properly." };
        }

        [HttpGet("schedule-fetch-fb-report")]
        [Right("schedule-allocation")]
        public async Task<FBBulkReportResponse> ScheduleFetchFBReportBulk()
        {
            // from yeterday
            var startDate = DateTime.Now.AddDays(-1);

            var scheduleDaysCount = _configuration["ScheduleReportFetchDays"];

            // upto configure date 
            var endDate = startDate.AddDays(-Int32.Parse(scheduleDaysCount));

            var reportList = await _fbManager.getReportIdsbyBookingServiceDates(startDate, endDate);

            foreach (var report in reportList)
            {
                if (report.ApiReportId > 0)
                {
                    var fbReportFetchRequest = new FbReportFetchRequest()
                    {
                        Id = Guid.NewGuid(),
                        ReportId = report.ApiReportId,
                        FbReportId = report.FbReportId
                    };
                    await _rabbitMQClient.Publish<FbReportFetchRequest>(_configuration["FbReportQueue"], fbReportFetchRequest);
                }
            }

            return new FBBulkReportResponse() { Result = "All the reports updated properly." };
        }

        /// <summary>
        /// Get FB token based on the needs
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
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
