using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.Dashboard;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CSDashboardController : ControllerBase
    {
        private readonly ICSDashboardManager _csDashboardManager = null;

        public CSDashboardController(ICSDashboardManager csDashboardManager)
        {
            _csDashboardManager = csDashboardManager;
        }

        [HttpPost("get-new-data-count")]
        [Right("cs-dashboard")]
        public async Task<GetNewBookingRelatedCountResponse> GetCountNewBookingRelatedDetails(CSDashboardModelRequest request)
        {
            return await _csDashboardManager.GetCountNewBookingRelatedDetails(request);
        }

        [HttpPost("get-service-type-details")]
        [Right("cs-dashboard")]
        public async Task<CSDashboardserviceTypeResponse> GetServiceTypeList(CSDashboardModelRequest request)
        {
            return await _csDashboardManager.GetServiceTypeList(request);
        }

        [HttpPost("get-manday-by-office-details")]
        [Right("cs-dashboard")]
        public async Task<CSDashboardMandayByOfficeResponse> GetMandayByOfficeList(CSDashboardModelRequest request)
        {
            return await _csDashboardManager.GetMandayByOfficeList(request);
        }

        [HttpPost("get-report-count-by-day-details")]
        [Right("cs-dashboard")]
        public async Task<DayFBReportCountResponse> GetReportCountByDayList(CSDashboardModelRequest request)
        {
            return await _csDashboardManager.GetReportCountByDayList(request);
        }


        [HttpPost("get-status-count-by-logged-user-details")]
        [Right("cs-dashboard")]
        public async Task<StatusListCountResponse> GetStatusByLoggedUserList(CSDashboardModelRequest request)
        {
            return await _csDashboardManager.GetStatusByLoggedUserList(request);
        }
    }
}