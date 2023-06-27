using Contracts.Managers;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DTO.AuditDashboard;
using Components.Web;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class AuditDashboardController : ControllerBase
    {
        private readonly IAuditDashboardManager _manager = null;
        public AuditDashboardController(IAuditDashboardManager manager)
        {
            _manager = manager;
        }

        [HttpPost("getAuditCountryGeocode")]
        public async Task<AuditMapGeoLocation> GetAuditCountryGeoCode(AuditDashboardMapFilterRequest request)
        {
            return await _manager.GetAuditCountryGeoCode(request);
        }

        [HttpPost("getAuditDashboardSummary")]
        public async Task<AuditDashboardResponse> GetAuditDashboardSummary(AuditDashboardMapFilterRequest request)
        {
            return await _manager.GetAuditDashboardSummary(request);
        }

        [HttpPost("getServiceTypeAuditDashboard")]
        public async Task<ResultAnalyticsAuditDashboardResponse> GetServiceTypeAuditDashboard(AuditDashboardMapFilterRequest request)
        {
            return await _manager.GetServiceTypeAuditDashboard(request);
        }

        [HttpPost("getAuditTypeAuditdashboard")]
        public async Task<ResultAnalyticsAuditDashboardResponse> GetAuditTypeAuditDashboard(AuditDashboardMapFilterRequest request)
        {
            return await _manager.GetAuditTypeAuditDashboard(request);
        }

        [HttpPost("getOverViewDashboard")]
        public async Task<OverviewAuditDashboardResponse> GetOverviewAuditDashboard(AuditDashboardMapFilterRequest request) 
        {
            return await _manager.OverviewDashboardSearch(request);
        } 
        [HttpPost("ServiceTypeExport")]
        public async Task<IActionResult> ExportServiceTypeChart(AuditDashboardMapFilterRequest request)
        {
            var response = await _manager.ExportServiceTypeChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/AuditDashboard/ServiceTypeChartExport", response, Components.Core.entities.FileType.Excel);
        }
        [HttpPost("AuditTypeExport")]
        public async Task<IActionResult> ExportAuditTypeChart(AuditDashboardMapFilterRequest request)
        {
            var response = await _manager.ExportAuditTypeChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/AuditDashboard/AuditTypeChartExport", response, Components.Core.entities.FileType.Excel);
        }

    }
}
