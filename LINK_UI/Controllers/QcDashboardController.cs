using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.Dashboard;
using DTO.Manday;
using DTO.QcDashboard;
using DTO.Schedule;
using DTO.User;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class QcDashboardController : ControllerBase
    {
        private readonly IQcDashboardManager _manager = null;
        public QcDashboardController(IQcDashboardManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Get the schedule for qc
        /// </summary>
        /// <returns></returns>
        [HttpGet("QcScheduleDetails")]
        [Right("get-dashboard")]
        public async Task<QcDashboardCalendarResponse> GetQcScheduleDetails()
        {
            var res = await _manager.GetQcScheduleDetails();
            return res;
        }
        /// <summary>
        /// Get the qc Productivity Details
        /// </summary>
        /// <returns></returns>
        [HttpPost("QcProductivityDetails")]
        [Right("get-dashboard")]
        public async Task<QcDashboardReportsResponse> GetQcProductivityDetails(QcDashboardSearchRequest request)
        {
            var res = await _manager.GetQcProductivityDetails(request);
            return res;
        }
        /// <summary>
        /// Get the Qc Rejection Details
        /// </summary>
        /// <returns></returns>
        [HttpPost("QcRejectionDetails")]
        [Right("get-dashboard")]
        public async Task<QcRejectionReportsResponse> GetQcRejectionDetails(QcDashboardSearchRequest request)
        {
            var res = await _manager.GetQcRejectionDetails(request);
            return res;
        }
        /// <summary>
        /// Get the Qc Dashboard Count Details
        /// </summary>
        /// <returns></returns>
        [HttpPost("QcDashboardCountDetails")]
        [Right("get-dashboard")]
        public async Task<QcDashboardCountResponse> GetQcDashboardCountDetails(QcDashboardSearchRequest request)
        {
            var res = await _manager.GetQcDashboardCountDetails(request);
            return res;
        }
    }
}