using Components.Web;
using Contracts.Managers;
using DTO.RejectionDashboard;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class RejectionDashboardController : ControllerBase
    {
        private readonly IRejectionDashboardManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;

        public RejectionDashboardController(IRejectionDashboardManager manager, ISharedInspectionManager helper)
        {
            _manager = manager;
            _helper = helper;
        }

        [HttpPost("getAPIResultDashboard")]
        [Right("rejectionDashboard")]
        public async Task<RejectResultDashboardResponse> GetAPIResultDashboard(RejectionDashboardSearchRequest request)//change request
        {
            return await _manager.GetAllAPIResultDashboard(request);
        }

        [HttpPost("getCustomerResultDashboard")]
        [Right("rejectionDashboard")]
        public async Task<RejectResultDashboardResponse> GetCustomerResultDashboard(RejectionDashboardSearchRequest request)//change request
        {
            return await _manager.GetCustomerResultDashboard(request);
        }

        [HttpPost("apiResultChartExport")]
        [Right("rejectionDashboard")]
        public async Task<IActionResult> ExportApiResultChart(RejectionDashboardSearchRequest request)//change request
        {
            var response = await _manager.ExportAPIResultDashboard(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/RejectionDashboard/ApiResultChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("customerResultChartExport")]
        [Right("rejectionDashboard")]
        public async Task<IActionResult> ExportCustomerResultChart(RejectionDashboardSearchRequest request)//change request
        {
            var response = await _manager.ExportCustomerResultDashboard(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/RejectionDashboard/CustomerResultChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("getProductCategoryDashboard")]
        [Right("rejectionDashboard")]
        public async Task<ProductCategoryChartResponse> GetProductCategoryDashboard(RejectionDashboardSearchRequest request)//change request
        {
            return await _manager.GetProductCategoryResultDashboard(request);
        }

        [HttpPost("productCategoryChartExport")]
        [Right("rejectionDashboard")]
        public async Task<IActionResult> ExportProductCategoryChart(RejectionDashboardSearchRequest request)//change request
        {
            var response = await _manager.ExportProductCategoryDashboard(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/RejectionDashboard/ProductCategoryChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("getVendorDashboard")]
        [Right("rejectionDashboard")]
        public async Task<VendorChartResponse> GetVendorDashboard(RejectionDashboardSearchRequest request)//change request
        {
            return await _manager.GetVendorResultDashboard(request);
        }

        [HttpPost("vendorChartExport")]
        [Right("rejectionDashboard")]
        public async Task<IActionResult> ExportVendorChart(RejectionDashboardSearchRequest request)//change request
        {
            var response = await _manager.ExportVendorDashboard(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/RejectionDashboard/VendorChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("getCountryDashboard")]
        [Right("rejectionDashboard")]
        public async Task<CountryChartResponse> GetCountryDashboard(CountryChartRequest request)//change request
        {
            return await _manager.GetCountryResultDashboard(request);
        }

        [HttpPost("countryChartExport")]
        [Right("rejectionDashboard")]
        public async Task<IActionResult> ExportCountryChart(RejectionDashboardSearchRequest request)//change request
        {
            var response = await _manager.ExportCountryDashboard(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/RejectionDashboard/CountryChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("getRejectDashboard")]
        [Right("rejectionDashboard")]
        public async Task<RejectChartResponse> GetRejectDashboard(RejectChartRequest request)//change request
        {
            return await _manager.GetCustomerReportReject(request);
        }
        [HttpPost("getRejectSubCatogory")]
        [Right("rejectionDashboard")]
        public async Task<RejectChartResponse> GetRejectSubCatogory(RejectChartSubcatogoryRequest request)//change request
        {
            return await _manager.GetCustomerReportRejectSubcatogory(request);
        }
        [HttpPost("getRejectSubCatogory2")]
        [Right("rejectionDashboard")]
        public async Task<RejectChartResponse> GetRejectSubCatogory2(RejectChartSubcatogory2Request request)//change request
        {
            return await _manager.GetCustomerReportRejectSubcatogory2(request);
        }

        [HttpPost("getRejectDashboardPopUpData")]
        [Right("rejectionDashboard")]
        public async Task<RejectionPopUpResponse> GetRejectDashboardPopUpData(RejectionDashboardSearchRequest request)//change request
        {
            return await _manager.GetRejectPopUpData(request);
        }
        
        [HttpPost("getRejectImages")]
        [Right("rejectionDashboard")]
        public async Task<RejectionImageResponse> GetRejectImageData(RejectionDashboardSearchRequest request)//change request
        {
            return await _manager.GetRejectionImages(request);
        }

        [HttpPost("exportRejectDashboard")]
        [Right("rejectionDashboard")]
        public async Task<IActionResult> ExportRejectDashboard(RejectChartSubcatogory2Request request)
        {
            Stream stream = null;
            var res = await _manager.ExportRejectionDashboardData(request);
            stream = _helper.GetAsStreamObject(res);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RejectionDashboardAnalysis.xlsx");
        }

        [HttpPost("getReportRejectionRate")]
        public async Task<RejectionRateResponse> GetReportRejectionRate(RejectionDashboardSearchRequest request)
        {
            return await _manager.GetReportRejectionRate(request);
        }

        [HttpPost("exportReportRejectionRate")]
        public async Task<IActionResult> ExportReportRejectionRate(RejectionDashboardSearchRequest request)
        {
            var res = await _manager.ExportReportRejectionRate(request);
            Stream stream = _helper.GetAsStreamObjectAndLoadDataTable(res);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RejectionRate.xlsx");
        }
    }
}