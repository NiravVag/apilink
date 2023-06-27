using Contracts.Managers;
using DTO.Eaqf;
using LINK_UI.Controllers.EXTERNAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LINK_UI.Controllers.EAQF
{
    [Route("api/EAQF/[controller]")]
    [Authorize(Policy = "ApiUserPolicy")]
    [ApiController]
    public class EaqfDashboardController : ExternalBaseController
    {
        private readonly IRejectionDashboardManager _manager = null;
        public EaqfDashboardController(IRejectionDashboardManager manager)
        {
            _manager = manager;
        }

        [HttpGet("ResultAnalytics")]
        public async Task<IActionResult> GetReportResultAnalytics([FromQuery] EaqfDashboardRequest request)
        {
            var response = await _manager.GetReportResultAnalytics(request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpGet("RejectionAnalytics")]
        public async Task<IActionResult> GetReportRejectionAnalytics([FromQuery] EaqfDashboardRequest request)
        {
            var response = await _manager.GetReportRejectionAnalytics(request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpGet("RejectionResultByCategory")]
        public async Task<IActionResult> GetReportResultByCategoryAnalytics([FromQuery] EaqfDashboardRequest request)
        {
            var response = await _manager.GetReportRejectionResultByProductCategory(request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpGet("RejectionResultByFactory")]
        public async Task<IActionResult> GetReportResultByFactoryAnalytics([FromQuery] EaqfDashboardRequest request)
        {
            var response = await _manager.GetReportRejectionResultByFactory(request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpGet("DefectDetails")]
        public async Task<IActionResult> GetReportDefectDetails([FromQuery] EaqfDashboardRequest request)
        {
            var response = await _manager.GetReportDefectAnalytics(request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpGet("DefectTypeDetails")]
        public async Task<IActionResult> GetReportDefectTypeDetails([FromQuery] EaqfDashboardRequest request)
        {
            var response = await _manager.GetDefectTypeDetails(request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpGet("DefectResultByProductCategory")]
        public async Task<IActionResult> GetReportDefectResultByCategoryAnalytics([FromQuery] EaqfDashboardRequest request)
        {
            var response = await _manager.GetReportDefectAnalyticsByProductCatgory(request);
            return BuildCommonEaqfResponse(response);
        }
    }
}
