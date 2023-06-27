using Components.Web;
using Contracts.Managers;
using DTO.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ApiUserPolicy")]
    [ApiController]
    public class ManualInvoiceController : Controller
    {
        private readonly IManualInvoiceManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;
        public ManualInvoiceController(IManualInvoiceManager manager, ISharedInspectionManager helper)
        {
            _manager = manager;
            _helper = helper;
        }
        [HttpPost("save-manual-invoice")]
        public async Task<SaveManualInvoiceResponse> SaveManualInvoice(SaveManualInvoice request)
        {
            return await _manager.SaveManualInvoice(request);
        }


        [HttpPost("manual-invoice-summary")]
        public async Task<ManualInvoiceSummaryResponse> GetManualInvoiceSummary(ManualInvoiceSummaryRequest request)
        {
            return await _manager.GetManualInvoiceSummary(request);
        }


        [HttpPost("update-manual-invoice")]
        public async Task<SaveManualInvoiceResponse> UpdateManualInvoice(SaveManualInvoice request)
        {
            return await _manager.UpdateManualInvoice(request);
        }

        [HttpPost("delete-manual-invoice/{id}")]
        public async Task<DeleteManualInvoiceResponse> DeleteManualInvoice(int id)
        {
            return await _manager.DeleteManualInvoice(id);
        }

        [HttpGet("get-manual-invoice/{id}")]
        public async Task<GetManualInvoiceResponse> GetManualInvoice(int id)
        {
            return await _manager.GetManualInvoice(id);
        }

        [HttpGet("checkinvoicenumberexist/{invoiceNo}")]
        public async Task<bool> CheckInvoiceNumberExist(string invoiceNo)
        {
            return await _manager.CheckInvoiceNumberExist(invoiceNo);
        }

        [HttpPost("export-manual-invoice")]
        public async Task<IActionResult> GetExportManualInvoiceSummary(ManualInvoiceSummaryRequest request)
        {
            var response = await _manager.ExportManualInvoiceSummary(request);
            if (response == null || !response.ManualInvoices.Any())
                return NotFound();
            return await this.FileAsync("Excel/Invoice/ManualInvoiceSummary", response, Components.Core.entities.FileType.Excel);
        }
    }
}
