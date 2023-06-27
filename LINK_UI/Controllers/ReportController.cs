using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using BI.Maps;
using Components.Web;
using Contracts.Managers;
using DTO.Common;
using DTO.Inspection;
using DTO.Report;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static DTO.Common.Static_Data_Common;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger = null;
        private readonly IReportManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;

        public ReportController(IReportManager manager, ILogger<ReportController> logger, ISharedInspectionManager helper)
        {
            _manager = manager;
            _logger = logger;
            _helper = helper;
        }

        [HttpPost("Search")]
        [Right("customer-report")]
        [Authorize(Policy = "ApiUserPolicy")]
        public async Task<CustomerReportSummaryResponse> Search([FromBody] InspectionSummarySearchRequest request)
        {
            _logger.LogInformation("Search_CustomerReportSummary");
            var res = await _manager.GetAllInspectionReports(request);
            return res;

        }

        [HttpGet("GetProducts/{bookingId}")]
        [Authorize(Policy = "ApiUserPolicy")]
        public async Task<IEnumerable<ReportProductItem>> GetProducts(int bookingId)
        {
            _logger.LogInformation("GetProducts_CustomerReportSummary");
            return await _manager.GetProductsByBooking(bookingId);

        }

        [HttpGet("GetContainers/{bookingId}")]
        [Authorize(Policy = "ApiUserPolicy")]
        public async Task<IEnumerable<ReportProductItem>> GetContainers(int bookingId)
        {
            _logger.LogInformation("GetProducts_CustomerReportSummary");
            return await _manager.GetContainersByBooking(bookingId);

        }


        [Right("customer-report")]
        [HttpPost("ExportReportSummary")]
        [Authorize(Policy = "ApiUserPolicy")]
        public async Task<IActionResult> ExportReportSummary([FromBody] InspectionSummarySearchRequest request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.pageSize = PageSize;

            var response = await _manager.ExportReportDataSummary(request);

            if (response == null)
                return NotFound();

            //if call is from customer report then call customer report template else fillingreview template
            if (request.CallingFrom == (int)PageTypeSummary.CustomerReport)
            {
                Stream stream = _helper.GetAsStreamObject(response.customerReportData);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BookingSummaryTemplate.xlsx");
            }
            if (request.CallingFrom == (int)PageTypeSummary.FillingReview)
            {
                Stream stream = _helper.GetAsStreamObject(response.fillingReportData);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FillingReportTemplate.xlsx");
            }


            return NotFound();
        }

        [HttpPost("StatusUpdate")]
        [Right("customer-report")]
        [Authorize(Policy = "ApiUserPolicy")]
        public async Task<CustomerReportSummaryResponseResult> StatusUpdate([FromBody] BookingStatusRequest request)
        {
            return await _manager.BookingStatusUpdate(request);
        }

        [HttpPost("UpdateCustomReport")]
        [Right("customer-report")]
        [Authorize(Policy = "ApiUserPolicy")]
        public async Task<UploadCustomReportResponse> UpdateCustomReport([FromBody] UploadCustomReportRequest request)
        {
            return await _manager.UpdateCustomReport(request);
        }

        [HttpPost("getInspectionOccupancy")]
        [Authorize(Policy = "ApiUserPolicy")]
        public async Task<InspectionOccupancySummaryResponse> GetInsepctionOccupancy([FromBody] InspectionOccupancySearchRequest request)
        {
            return await _manager.GetInspectionOccupancySummary(request);
        }


        [HttpPost("exportInspectionOccupancy")]
        [Authorize(Policy = "ApiUserPolicy")]
        public async Task<IActionResult> ExportInsepctionOccupancy([FromBody] InspectionOccupancySearchRequest request)
        {
            var response = await _manager.ExportInspectionOccupanySummary(request);
            if (response == null || response.Result == InspectionOccupancyResult.NotFound)
                return NotFound();
            var stream = _helper.GetAsStreamObject(response.InspectionOccupancies);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InspectionOccupancy.xlsx");
        }


    }
}