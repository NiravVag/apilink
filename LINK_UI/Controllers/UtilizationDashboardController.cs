using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.Manday;
using DTO.UtilizationDashboard;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class UtilizationDashboardController : ControllerBase
    {
        private readonly IUtilizationDashboardManager _manager = null;

        public UtilizationDashboardController(IUtilizationDashboardManager manager)
        {
            _manager = manager;
        }

        [HttpPost("Search")]
        [Right("utilizationDashboard")]
        public async Task<UtilizationResponse> GetUtilizationSummary([FromBody] UtilizationDashboardRequest request)
        {
            return await _manager.GetCapacityUtilizationReport(request);
        }

        [Right("utilizationDashboard")]
        [HttpPost("ExportUtilizationDashboard")]
        public async Task<IActionResult> ExportInspectionSearchSummary([FromBody] UtilizationDashboardRequest request)
        {
            if (request == null)
                return NotFound();
            var response = await _manager.GetCapacityUtilizationReportExport(request);

            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/UtilizationDashboard/UtilizationDashboardExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("ManDayYearChart")]
        [Right("mandayDashboard")]
        public async Task<MandayYearChartResponse> GetMandayYearChart([FromBody] UtilizationDashboardRequest request)
        {
            return await _manager.GetMandayYearChart(request);
        }

        [HttpPost("ManDayYearChartExport")]
        [Right("mandayDashboard")]
        public async Task<IActionResult> ExportMandayYearChart([FromBody] UtilizationDashboardRequest request)
        {
            var response = await _manager.ExportMandayYearChart(request);
            if (response == null && !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/ManDay/MandayYearChartExport", response, Components.Core.entities.FileType.Excel);
        }
    }
}