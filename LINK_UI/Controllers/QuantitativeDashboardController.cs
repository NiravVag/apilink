using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.ManagementDashboard;
using DTO.ProductManagement;
using DTO.QuantitativeDashboard;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class QuantitativeDashboardController : ControllerBase
    {
        private readonly IQuantitativeDashboardManager _manager = null;

        public QuantitativeDashboardController(IQuantitativeDashboardManager manager)
        {
            _manager = manager;
        }

        [HttpPost("getQuantitativeDashboardSummary")]
        [Right("quantitativeDashboard")]
        public async Task<QuantitativeDashboardResponse> GetDashboardSummary(QuantitativeDashboardFilterRequest request)//change request
        {
            return await _manager.GetAllQuantitativeDashboardSummary(request);
        }


        [HttpPost("get-manday-count-by-year")]
        [Right("quantitativeDashboard")]
        public async Task<QuantitativeMandayYearChartResponse> GetMandayYearChart(QuantitativeDashboardFilterRequest request)
        {
            return await _manager.GetMandayYearChart(request);
        }

        [HttpPost("MandaychartExport")]
        [Right("managementDashboard")]
        public async Task<IActionResult> ExportMandayChart(QuantitativeDashboardFilterRequest request)
        {
            var response = await _manager.ExportMandayChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/QuantitativeDashboard/MandayYearChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("MandayByCountry")]
        [Right("quantitativeDashboard")]
        public async Task<QuantitativeDashboardCommonResponse> GetMandayByCountry(QuantitativeDashboardFilterRequest request)
        {
            return await _manager.GetMandayByCountry(request);
        }

        [HttpPost("MandayCountrychartExport")]
        [Right("managementDashboard")]
        public async Task<IActionResult> ExportMandayByCountryChart(QuantitativeDashboardFilterRequest request)
        {
            var response = await _manager.ExportMandayCountryChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/QuantitativeDashboard/MandayByCountryChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("TurnOverSummary")]
        [Right("quantitativeDashboard")]
        public async Task<TurnOverDataResponse> GetTurnOverSummary(QuantitativeDashboardFilterRequest request)
        {
            return await _manager.GetTurnOverSummary(request);
        }

        [HttpPost("TurnOverServiceTypechartExport")]
        [Right("quantitativeDashboard")]
        public async Task<IActionResult> ExportTurnOverByServiceTypeChart(QuantitativeDashboardFilterRequest request)
        {
            var response = await _manager.ExportTurnOverByServiceTypeChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/QuantitativeDashboard/TurnOverByServiceTypeExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("getInspectionServiceTypeDashboard")]
        [Right("quantitativeDashboard")]
        public async Task<QuantitativeDashboardCommonResponse> GetServiceTypeDashboard(QuantitativeDashboardFilterRequest request)//change request
        {
            return await _manager.GetServiceTypeChart(request);
        }

        [HttpPost("InspectionServiceTypechartExport")]
        [Right("quantitativeDashboard")]
        public async Task<IActionResult> ExportInspectionByServiceTypeChart(QuantitativeDashboardFilterRequest request)
        {
            var response = await _manager.ExportInspectionByServiceTypeChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/QuantitativeDashboard/InspectionByServiceTypeExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("get-order-quantity-list")]
        [Right("quantitativeDashboard")]
        public async Task<PiecesInspectedChartResponse> GetBookingQuantityDashboard(QuantitativeDashboardFilterRequest request)
        {
            return await _manager.GetBookingQuantityData(request);
        }

        [HttpPost("get-order-quantity-list-export")]
        [Right("quantitativeDashboard")]
        public async Task<IActionResult> GetBookingQuantityDashboardExport(QuantitativeDashboardFilterRequest request)
        {
            var response = await _manager.GetBookingQuantityDashboardExport(request);
            if (response == null || response.Result != QuantitativeDashboardResult.Success)
                return NotFound();
            return await this.FileAsync("Excel/QuantitativeDashboard/QuantityOrderDetailsChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("get-product-category-list")] 
        [Right("quantitativeDashboard")]
        public async Task<ProductCategoryDashboardResponse> GetProductCategoryList(ProductCategoryChartRequest request)//change request
        {
            return await _manager.GetProductCategoryList(request);
        }

        [HttpPost("get-product-category-list-export")]
        [Right("quantitativeDashboard")]
        public async Task<IActionResult> ProductCategoryListExport(QuantitativeDashboardFilterRequest request)
        {
            var response = await _manager.ProductCategoryListExport(request);
            if (response == null || response.Result != QuantitativeDashboardResult.Success)
                return NotFound();
            return await this.FileAsync("Excel/QuantitativeDashboard/ProductCategoryCountChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [Right("product-category")]
        [HttpGet("productcategory")]
        public async Task<ProductCategoryResponse> GetProductCategorySummary()
        {
            return await _manager.GetProductCategoryList();
        }
    }
}