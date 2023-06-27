using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.ManagementDashboard;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class ManagementDashboardController : ControllerBase
    {
        private readonly IManagementDashboardManager _manager = null;
        public ManagementDashboardController(IManagementDashboardManager manager)
        {
            _manager = manager;
        }

        [HttpPost("getMandayDashboardSummary")]
        [Right("managementDashboard")]
        public async Task<ManagementDashboardResponse> GetDashboardSummary([FromBody] ManagementDashboardRequest request)
        {
            return await _manager.GetManagementDashboardSearch(request);
        }

        [HttpPost("get-manday-count-by-year")]
        [Right("managementDashboard")]
        public async Task<MandayYearChartManagementDashboardResponse> GetMandayYearChart([FromBody] ManagementDashboardRequest request)
        {
            return await _manager.GetMandayYearChart(request);
        }

        [HttpPost("getOverviewDashboard")]
        [Right("managementDashboard")]
        public async Task<OverviewDashboardResponse> GetOverviewDashboard([FromBody] ManagementDashboardRequest request)
        {
            return await _manager.OverviewDashboardSearch(request);
        }

        [HttpPost("getRejectDashboard")]
        [Right("managementDashboard")]
        public async Task<InspectionRejectDashboardResponse> GetRejectDashboard(ManagementDashboardRequest request)
        {
            return await _manager.GetInspectionRejectChartByQuery(request);
        }

        [HttpPost("getProductCategoryDashboard")]
        [Right("managementDashboard")]
        public async Task<ProductCategoryDashboardResponse> GetProductCategoryDashboard(ManagementDashboardRequest request)
        {
            return await _manager.ProductCategoryDashboardSearchByBookingRequest(request);
        }

        [HttpPost("getResultDashboard")]
        [Right("managementDashboard")]
        public async Task<ResultDashboardResponse> GetResultDashboard(ManagementDashboardRequest request)
        {
            return await _manager.GetResultDashboard(request);
        }

        [HttpPost("getServiceTypeDashboard")]
        [Right("managementDashboard")]
        public async Task<ResultAnalyticsDashboardResponse> GetServiceTypeDashboard(ManagementDashboardRequest request)
        {
            return await _manager.GetServiceTypeChart(request);
        }

        [HttpPost("getAverageBookingTimeDashboard")]
        [Right("managementDashboard")]
        public async Task<AverageBookingTimeResponse> GetAverageBookingStatusChangeTimeDashboard(ManagementDashboardRequest request)
        {
            return await _manager.GetAverageBookingStatusChangeTime(request);
        }

        [HttpPost("getAverageQuotationTimeDashboard")]
        [Right("managementDashboard")]
        public async Task<AverageQuotationTimeResponse> GetAverageQuotationStatusChangeTimeDashboard(ManagementDashboardRequest request)
        {
            return await _manager.GetAverageQuotationStatusChangeTime(request);
        }

        [HttpPost("ProductCategoryExport")]
        [Right("managementDashboard")]
        public async Task<IActionResult> ExportProductCategoryChart(ManagementDashboardRequest request)
        {
            var response = await _manager.ExportProductCategoryChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/ManagementDashboard/ProductCategoryChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("ServiceTypeExport")]
        [Right("managementDashboard")]
        public async Task<IActionResult> ExportServiceTypeChart(ManagementDashboardRequest request)
        {
            var response = await _manager.ExportServiceTypeChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/ManagementDashboard/ServiceTypeChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("ResultExport")]
        [Right("managementDashboard")]
        public async Task<IActionResult> ExportResultChart(ManagementDashboardRequest request)
        {
            var response = await _manager.ExportResultChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/ManagementDashboard/ResultChartExport", response, Components.Core.entities.FileType.Excel);
        }

    }
}