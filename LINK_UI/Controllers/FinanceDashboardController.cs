using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.FinanceDashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class FinanceDashboardController : ControllerBase
    {
        private readonly IFinanceDashboardManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;

        public FinanceDashboardController(IFinanceDashboardManager manager, ISharedInspectionManager helper)
        {
            _manager = manager;
            _helper = helper;
        }

        [HttpPost("getBilledManday")]
        public async Task<MandayYearChartFinanceDashboardResponse> GetBilledMandayChartData(FinanceDashboardRequest request)
        {
            return await _manager.GetBilledMandayData(request);
        }

        [HttpPost("getMandayRate")]
        public async Task<MandayYearChartFinanceDashboardResponse> GetMandayRateChartData(FinanceDashboardRequest request)
        {
            return await _manager.GetMandayRateData(request);
        }

        [HttpPost("get-Booking-Details")]
        public async Task<FinanceDashboardBookingDataResponse> GetBookingData(FinanceDashboardRequest request)
        {
            return await _manager.GetBookingIdList(request);
        }

        [HttpPost("get-Turnover-Chart-Details")]
        public async Task<FinanceDashboardTurnOverResponse> GetFinanceDashboardTurnOverData(List<int> bookingIdList)
        {
            return await _manager.GetFinanceDashboardTurnOverData(bookingIdList);
        }

        [HttpPost("get-ChargeBack-Chart-Details")]
        public async Task<ChargeBackChartResponse> GetFinanceDashboardChargeBackData(List<int> bookingIdList)
        {
            return await _manager.GetChargeBackChartData(bookingIdList);
        }

        [HttpPost("get-Quotation-Chart-Details")]
        public async Task<QuotationChartResponse> GetQuotationData(List<int> bookingIdList)
        {
            return await _manager.GetQuotationChartData(bookingIdList);
        }

        [HttpPost("BilledMandaychartExport")]
        public async Task<IActionResult> ExportBilledMandayChart(FinanceDashboardRequest request)
        {
            var response = await _manager.ExportBilledMandayChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/FinanceDashboard/BilledMandayExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("CountrychartExport")]
        public async Task<IActionResult> ExportCountryTurnoverChart(FinanceDashboardRequest request)
        {
            var response = await _manager.CountryTurnOverExport(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/FinanceDashboard/CountryTurnoverExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("ProdCategorychartExport")]
        public async Task<IActionResult> ExportProdCategoryTurnoverChart(FinanceDashboardRequest request)
        {
            var response = await _manager.ProductCategoryTurnOverExport(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/FinanceDashboard/ProdCategoryTurnoverExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("ServiceTypechartExport")]
        public async Task<IActionResult> ExportServiceTypeTurnoverChart(FinanceDashboardRequest request)
        {
            var response = await _manager.ServiceTypeTurnOverExport(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/FinanceDashboard/ServiceTypeTurnoverExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("get-RationAnalysis-Details")]
        public async Task<FinanceDashboardRatioAnalysisResponse> GetRatioAnalysisList(FinanceDashboardSearchRequest request)
        {
            return await _manager.GetRatioAnalysisList(request);
        }
        [HttpPost("RatioAnalysisExport")]
        public async Task<IActionResult> ExportRatioAnalysis(FinanceDashboardSearchRequest request)
        {
            if (request == null)
                return NotFound();

            var response = await _manager.ExportRatioAnalysisList(request);
            if (response == null || !response.Any())
                return NotFound();
            var stream = _helper.GetAsStreamObject(response);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ratio_Analysis.xlsx");
        }

        [HttpGet("get-employee-types")]
        public async Task<DataSourceResponse> GetEmployeeTypes()
        {
            return await _manager.GetEmployeeTypes();
        }
    }
}