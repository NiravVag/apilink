using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Contracts.Managers;
using DTO.InvoicePreview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "InvoicePdfUserPolicy")]
    public class InvoicePreviewController : ControllerBase
    {
        private readonly IInvoicePreviewManager _manager = null;
        private readonly ICustomerManager _customerManager = null;

        public InvoicePreviewController(IInvoicePreviewManager manager, ICustomerManager customerManager)
        {
            _manager = manager;
            _customerManager = customerManager;
        }

        [HttpGet("GetCustomers")]
        public async Task<IEnumerable<DataCommon>> GetCustomers()
        {
            return await _customerManager.GetCustomerItemList();
        }

        [HttpGet("GetInvoiceType")]
        public IEnumerable<DataCommon> GetInvoiceType()
        {
            return _manager.GetInvoiceType();
        }

        [HttpGet("GetInvoicePreview")]
        public IEnumerable<DataCommon> GetInvoicePreview()
        {
            return _manager.GetInvoicePreview();
        }

        [HttpGet("GetBankInfo")]
        public async Task<IEnumerable<InvoiceBankPreview>> GetBankInfo()
        {
            return await _manager.GetBankInfo();
        }

        [HttpGet("GetInvoice/{invoiceNo}/{invoicePreview}")]
        public async Task<IEnumerable<List<DataCommon>>> GetInvoice(string invoiceNo, int invoicePreview)
        {
            return await _manager.GetInvoiceDetails(invoiceNo, invoicePreview);
        }

        [HttpPost("SaveInvoicePdfUrl")]
        public async Task<SaveInvoicePdfResponse> SaveInvoicePdfUrl(SaveInvoicePdfUrl invoicePdfUrl)
        {
            return await _manager.SaveInvoicePdfUrl(invoicePdfUrl);
        }
        
        [HttpGet("downloadInvoicePdf")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadInvoicePdf([FromQuery] string invoiceNo,[FromQuery] string fileName)
        {            
            var response = await _manager.GetInvoicePreviewFile(invoiceNo, !string.IsNullOrWhiteSpace(fileName) ? fileName : null);
            if (response == null)
                return NotFound();

            if (response.Result != InvoiceDownloadResult.Success)
                return BadRequest(response);

            return File(response.Invoice, "application/pdf", response.FileName);
        }
    }
}