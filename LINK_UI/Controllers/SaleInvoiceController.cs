using Contracts.Managers;
using DTO.InvoicePreview;
using DTO.SaleInvoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class SaleInvoiceController : ControllerBase
    {
        private readonly ISaleInvoiceManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;
        public SaleInvoiceController(ISaleInvoiceManager manager, ISharedInspectionManager helper)
        {
            _manager = manager;
            _helper = helper;
        }

        [HttpPost("getSaleInvoiceSummary")]
        public async Task<SaleInvoiceSummaryResponse> GetSaleInvoiceSummary(SaleInvoiceSummaryRequest request)
        {
            return await _manager.GetSaleInvoiceSummary(request);
        }

        [HttpPost("ExportSaleInvoiceSearchSummary")]
        public async Task<IActionResult> ExportSaleInvoiceSearchSummary(SaleInvoiceSummaryRequest request)
        {
            if (request == null)
                return BadRequest();
            var response = await _manager.ExportSaleInvoiceSearchSummary(request);
            if (response == null)
                return NotFound();
            var stream = _helper.GetAsStreamObject(response);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sale_invoice_summary.xlsx");
        }
    }
}
