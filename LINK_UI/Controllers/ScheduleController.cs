using System.Threading.Tasks;
using DTO.Common;
using DTO.Schedule;
using LINK_UI.Filters;
using Contracts.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using LINK_UI.App_start;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System;
using static DTO.Common.Static_Data_Common;
using Components.Web;
using DTO.CommonClass;
using DTO.HumanResource;
using System.IO;
using OfficeOpenXml;
using BI.Utilities;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class ScheduleController : ControllerBase
    {
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IScheduleManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;
        private readonly IOfficeLocationManager _officeManager = null;
        private static IConfiguration _configuration = null;

        public ScheduleController(IScheduleManager manager, IAPIUserContext applicationContext,
            IConfiguration configuration, IOfficeLocationManager officeManager, ISharedInspectionManager helper)
        {
            _manager = manager;
            _ApplicationContext = applicationContext;
            _configuration = configuration;
            _officeManager = officeManager;
            _helper = helper;
        }

        [HttpPost("ScheduleSummary")]
        [Right("schedule-summary")]
        public async Task<ScheduleSearchResponse> GetScheduleDetails([FromBody] ScheduleSearchRequest request)
        {
            var res = await _manager.GetScheduleDetails(request);
            return res;
        }
        [HttpPost("SaveSchedule")]
        [Right("schedule-allocation")]
        public async Task<SaveScheduleResponse> SaveSchedule([FromBody] SaveScheduleRequest request)
        {
            string strFbToken = getFbToken();
            return await _manager.SaveSchedule(request, strFbToken);
        }
        [HttpGet("GetBookingAllocation/{bookingId}")]
        [Right("schedule-allocation")]
        public async Task<ScheduleAllocation> GetBookingAllocation(int bookingId)
        {
            return await _manager.GetBookingAllocation(bookingId);
        }
        [Right("schedule-summary")]
        [HttpPost("ExportScheduleSearchSummary")]
        public async Task<IActionResult> ExportScheduleSearchSummary([FromBody] ScheduleSearchRequest request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.pageSize = PageSize;
            var response = await _manager.GetScheduleDetails(request);
            Stream stream = null;
            if (request.ExportType == (int)ExportScheduleType.QCLevel)
            {
                var data = await _manager.ExportSummary(response.Data);
                if (response.Result != ScheduleSearchResponseResult.Success)
                    return NotFound();

                stream = _helper.GetAsStreamObject(data);
            }
            else if (request.ExportType == (int)ExportScheduleType.ProductLevel)
            {
                var data = await _manager.ExportSummaryProductLevel(response.Data);
                if (response.Result != ScheduleSearchResponseResult.Success)
                    return NotFound();
                stream = _helper.GetAsStreamObject(data);
            }

            if (stream == null)
                return NotFound();

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ScheduleSearchSummary.xls");
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

        [HttpPost("GetQCDetails")]
        [Right("schedule-allocation")]
        public async Task<AllocationStaffList> GetQCDetails(AllocationStaffSearchRequest allocationStaffSearchRequest)
        {
            return await _manager.GetQCList(allocationStaffSearchRequest);
        }

        [HttpPost("GetMandayForecast")]
        [Right("schedule-allocation")]
        public async Task<MandayForecastResponse> GetMandayForecast([FromBody] MandayForecastRequest request)
        {
            var response = await _manager.GetManDayForecast(request);
            return response;
        }

        [HttpGet("GetStaffwithLeave/{date}/{locationId}/{zoneid}")]
        [Right("schedule-allocation")]
        public async Task<StaffLeaveInfoResponse> GetStaffLeaveDetails(string date, int locationId, int zoneid)
        {
            return await _manager.GetStaffDetailsWithLeave(date, locationId, zoneid);
        }

        [HttpGet("GetManday/{bookingId}")]
        [Right("schedule-allocation")]
        public async Task<QuotScheduleMandayResponse> GetMandayDetails(int bookingId)
        {
            return await _manager.GetMandayDetails(bookingId);
        }

        [HttpPost("SaveManday")]
        [Right("schedule-allocation")]
        public async Task<int> SaveManday([FromBody] QuotScheduleManday request)
        {
            return await _manager.SaveManday(request);
        }

        [HttpPost("getqcdatasource")]
        public async Task<DataSourceResponse> GetQcDataSource(CommonQcSourceRequest request)
        {
            return await _manager.GetQcDataSource(request);
        }

        [Right("schedule-summary")]
        [HttpPost("ExportMandayForecast")]
        public async Task<IActionResult> ExportMandayForecast([FromBody] MandayForecastRequest request)
        {
            if (request == null)
                return NotFound();
            var response = await _manager.GetManDayForecast(request);
            if (response == null || response.Result != ScheduleSearchResponseResult.Success)
                return NotFound();
            return await this.FileAsync("MandayForecast", response, Components.Core.entities.FileType.Excel);
        }
        [HttpPost("GetQcVisibilityByBooking")]
        [Right("schedule-summary")]
        public async Task<BookingDataQcVisibleResponse> GetQcVisibility([FromBody] QcVisibilityBookingRequest request)
        {
            var response = await _manager.GetQcVisibilityByBooking(request);
            return response;
        }
        [HttpPost("UpdateQcVisibility")]
        [Right("schedule-summary")]
        public async Task<int> UpdateQcVisibility([FromBody] BookingDataQcVisibleRequest request)
        {
            return await _manager.UpdateQcVisibileData(request);
        }

        [HttpPost("GetDuplicateTravelExpenseData")]
        [Right("schedule-allocation")]
        public async Task<List<DuplicateTravelAllowance>> GetDuplicateTravelExpenseData([FromBody] SaveScheduleRequest request)
        {
            return await _manager.GetDuplicateTravelExpenseData(request);
        }

        [HttpGet("getproductdetails/{bookingId}")]
        [Right("schedule-summary")]
        public async Task<ScheduleProductModelResponse> GetProductPODetails(int bookingId)
        {
            return await _manager.GetProductPODetails(bookingId);
        }
    }
}