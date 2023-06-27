using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.DefectDashboard;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class DefectDashboardController : ControllerBase
    {
        private readonly IDefectDashboardManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;

        public DefectDashboardController(IDefectDashboardManager manager, ISharedInspectionManager helper)
        {
            _manager = manager;
            _helper = helper;
        }

        [HttpPost("get-booking-details")]
        [Right("defect-dashboard")]
        public async Task<BookingReportResponse> GetBookingReportDetails(DefectDashboardFilterRequest request) //changerequest
        {
            // return await _manager.GetBookingReportDetails(request);

            return await _manager.GetAllBookingReportDetails(request);
            
        }

        [HttpPost("get-defect-category-details")]
        [Right("defect-dashboard")]
        public async Task<DefectCategoryResponse> GetDefectCategoryList(DefectDashboardFilterRequest request)//changerequest
        {
            return await _manager.GetAllDefectCategoryList(request);
        }

        [HttpPost("get-defect-category-export")]
        [Right("defect-dashboard")]
        public async Task<IActionResult> ExportDefectCategoryList(DefectDashboardFilterRequest request)//changerequest
        {
           // var response = await _manager.GetDefectCategoryExportList(request);
            var response = await _manager.GetDefectCategoryExportList(request);
            if (response.Result != DefectDashboardResult.Success)
                return NotFound();
            return await this.FileAsync("Excel/Defect/DefectCategoryChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("get-defect-year-count-export")]
        [Right("defect-dashboard")]
        public async Task<IActionResult> ExportDefectCountYearList(DefectDashboardFilterRequest request) //changerequest
        {
            var response = await _manager.ExportDefectCountYearList(request);
            if (response.Result != DefectDashboardResult.Success)
                return NotFound();
            return await this.FileAsync("Excel/Defect/DefectYearCountChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("get-defect-year-count-details")]
        [Right("defect-dashboard")]
        public async Task<DefectYearInnerCountResponse> GetDefectYearListByInnerFilter(DefectDashboardFilterRequest request)//Change request
        {
            return await _manager.GetDefectYearListByInnerFilter(request);
        }

        [HttpPost("get-pareto-defect-list")]
        [Right("defect-dashboard")]
        public async Task<ParetoDefectResponse> GetParetoDefectList(DefectDashboardFilterRequest request)//Change request
        {
            return await _manager.GetAllDefectCount(request); 
        }

        [HttpPost("get-pareto-defect-list-export")]
        [Right("defect-dashboard")]
        public async Task<IActionResult> GetParetoDefectListExport(DefectDashboardFilterRequest request)//Change request
        {
            var response = await _manager.GetParetoDefectListExport(request);
            if (response.Result != DefectDashboardResult.Success)
                return NotFound();
            return await this.FileAsync("Excel/Defect/DefectParetoChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("get-low-performance-defect-list")]
        [Right("defect-dashboard")]
        public async Task<DefectPerformanceResponse> GetLowPerformanceDefectList(DefectPerformanceAnalysis request)
        {
            return await _manager.GetLowPerformanceDefectList(request);
        }

        [HttpPost("get-defect-list")]
        [Right("defect-dashboard")]
        public async Task<DataSourceResponse> GetDefectDataSource(CommonDataSourceRequest request)  
        {
            return await _manager.GetDefectDataSource(request);
        }

        [HttpPost("get-defect-low-performance-export")]
        [Right("defect-dashboard")]
        public async Task<IActionResult> GetLowPerformanceExport(DefectPerformanceAnalysis request)
        {
            var response = await _manager.GetLowPerformanceExport(request);
            if (response.Result != DefectDashboardResult.Success)
                return NotFound();
            return await this.FileAsync("Excel/Defect/DefectLowPerformanceChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("get-defect-count-list")]
        [Right("defect-dashboard")]
        public async Task<DefectCountResponse> GetDefectCountByFilters(DefectPerformanceAnalysis request)
        {
            return await _manager.GetDefectCountByFilters(request);
        }

        [HttpPost("get-defect-photo-list")]
        [Right("defect-dashboard")]
        public async Task<DefectPhotoResponse> GetDefectPhotoListByFilters(DefectPerformanceAnalysis request)
        {
            return await _manager.GetDefectPhotoListByFilters(request);
        }

        [HttpPost("get-country-defect-export")]
        [Right("defect-dashboard")]
        public async Task<IActionResult> GetCountryDefectExport(DefectDashboardFilterRequest request)//Change request
        {
            var response = await _manager.GetCountryDefectListExport(request);
            if (response.Result != DefectDashboardResult.Success)
                return NotFound();
            return await this.FileAsync("Excel/Defect/DefectCountryChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("get-country-defect-list")]
        [Right("defect-dashboard")]
        public async Task<CountryDefectChartResponse> GetCountryDefectList(DefectDashboardFilterRequest request)//Change request
        {
            return await _manager.GetCountryDefectList(request);
        }

        [HttpPost("getReportDefectPareto")]
        public async Task<ReportDefectResponse> GetReportDefectPareto(DefectDashboardFilterRequest request)
        {
            return await _manager.GetReportDefectPareto(request);
        }

        [HttpPost("export-report-defect-pareto")]
        public async Task<IActionResult> ExportReportDefectPareto(DefectDashboardFilterRequest request)
        {
            var res = await _manager.ExportReportDefectPareto(request);
            Stream stream = _helper.GetAsStreamObjectAndLoadDataTable(res);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DefectPareto.xlsx");
        }
    }
}