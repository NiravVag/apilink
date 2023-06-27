using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.Inspection;
using DTO.MasterConfig;
using DTO.Report;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static DTO.Common.Static_Data_Common;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InternalFBReportsController : Controller
    {
        private readonly ILogger<ReportController> _logger = null;
        private readonly IFBInternalReportManager _manager = null;
        public InternalFBReportsController(IFBInternalReportManager manager, ILogger<ReportController> logger, ITenantProvider filterService)
        {
            _manager = manager;
            _logger = logger;
        }

        [HttpPost("Search")]
        public async Task<InternalFBReportSummaryResponse> Search([FromBody] InspectionSummarySearchRequest request)
        {
            _logger.LogInformation("Search_Internal_ReportSummary");
            var res = await _manager.GetAllInspectionReportProducts(request);
            return res;

        }

        [HttpGet("GetProducts/{bookingId}")]
        public async Task<IQueryable<InternalReportProductItem>> GetProducts(int bookingId)
        {
            _logger.LogInformation("GetProducts_Internal_ReportSummary");
            return await _manager.GetProductsByBooking(bookingId);

        }

        [HttpPost("ExportInternalFBReports")]
        public async Task<IActionResult> ExportReportSummary([FromBody] InspectionSummarySearchRequest request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.pageSize = PageSize;

            var response = await _manager.ExportInternalFBReports(request);

            if (response == null)
                return NotFound();
            return await this.FileAsync("InternalFBReportSummary", response, Components.Core.entities.FileType.Excel);
        }

        [HttpGet("qcinspectiondetaildownload/{id}")]
        [Right("inspection-certificate")]
        public async Task<IActionResult> qcinspectiondetailpreview(int id, [FromServices] IQCInspectionDetailPDF previewService)
        {
            if (id > 0)
            {
                var qcInspectionDetails = await _manager.GetQCInspectionDetails(id);
                if (qcInspectionDetails != null)
                {
                    var document = previewService.CreateQCInspectionDetailDocument(qcInspectionDetails);
                    var fileContentResult= File(document.Content, document.MimeType);
                    return File(document.Content, document.MimeType);
                }
            }
            return NotFound();
        }


        [HttpGet("GetQcPicking/{bookingId}")]
        public async Task<IActionResult> GetQcPicking(int bookingId, [FromServices] IPickingPDF previewService)
        {
            var data = await _manager.GetQcPickingDetails(bookingId);
            var entMasterConfigItem = new EntMasterConfigItem();
            var materConfigs = await _manager.GetMasterConfiguration();
            entMasterConfigItem.Entity = materConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            entMasterConfigItem.ImageLogo = materConfigs.Where(x => x.Type == (int)EntityConfigMaster.ImageLogo).Select(x => x.Value).FirstOrDefault();
            if (data.Count() > 0)
            {
                var document = previewService.CreatePickingDocument(data, entMasterConfigItem);
                return File(document.Content, document.MimeType);
            }

            return NotFound();
        }


    }

}