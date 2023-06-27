using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.ExtraFees;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using static DTO.Common.Static_Data_Common;
using Components.Web;
using System;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class ExtraFeesController : ControllerBase
    {
        private readonly IExtraFeesManager _manager = null;

        public ExtraFeesController(IExtraFeesManager manager)
        {
            _manager = manager;
        }

        [HttpPost("booking-no-details")]
        [Right("extra-fees")]
        public async Task<BookingDataResponse> GetBookingNoList([FromBody] BookingDataSourceRequest request)
        {
            return await _manager.GetBookingNoList(request);
        }

        [HttpPost()]
        [Right("extra-fees")]
        public async Task<SaveResponse> Save([FromBody] ExtraFees request)
        {
            return await _manager.Save(request);
        }

        [HttpGet("tax/{bankId}/{bookingId}")]
        [Right("extra-fees")]
        public async Task<TaxResponse> GetTaxDetail(int bankId, int bookingId)
        {
            return await _manager.GetTaxValue(bankId, bookingId);
        }

        [HttpGet("{extraFeeId}")]
        [Right("extra-fees")]
        public async Task<EditResponse> Edit(int extraFeeId)
        {
            return await _manager.Edit(extraFeeId);
        }

        [HttpGet("invoice/{bookingId}/{billedToId}/{serviceId}")]
        [Right("extra-fees")]
        public async Task<DataSourceResponse> GetInvoiceNoList(int bookingId, int billedToId, int serviceId)
        {
            return await _manager.GetInvoiceNoList(bookingId, billedToId, serviceId);
        }

        [HttpGet("cancel/{extraFeeId}")]
        [Right("extra-fees")]
        public async Task<CancelResponse> Cancel(int extraFeeId)
        {
            return await _manager.Cancel(extraFeeId);
        }

        [HttpGet("generate-manual-invoice/{extraFeeId}")]
        public async Task<ManualInvoiceResponse> GenerateManualInvoice(int extraFeeId)
        {
            return await _manager.GenerateManualInvoice(extraFeeId);
        }

        [HttpPost("getExtrafeesSummary")]
        public async Task<ExtraFeeResponse> GetInvoiceSummary(ExtraFeeRequest requestDto)
        {
            return await _manager.GetExFeeSummary(requestDto);
        }

        [HttpPost("ExportExtrafeesSearchSummary")]
        public async Task<IActionResult> ExportInvoiceSearchSummary(ExtraFeeRequest request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.pageSize = PageSize;
            var response = await _manager.ExportExtrafeeSearchSummary(request);
            if (response == null)
                return NotFound();
            return await this.FileAsync("ExtraFeeSearchSummary", response, Components.Core.entities.FileType.Excel);
        }

        [HttpGet("getextrafeestatus")]
        public async Task<DataSourceResponse> GetExtrafeeStatus()
        {
            return await _manager.GetExtraFeeStatusList();
        }

        [HttpPut("cancelExtraFeeInvoice/{extraFeeId}")]
        public async Task<SaveResponse> CancelExtraFeeInvoice(int extraFeeId)
        {
            return await _manager.CancelExtraFeeInvoice(extraFeeId);
        }
    }
}