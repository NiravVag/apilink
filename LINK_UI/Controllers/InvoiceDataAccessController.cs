using Contracts.Managers;
using DTO.InvoiceDataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InvoiceDataAccessController : ControllerBase
    {
        private readonly IInvoiceDataAccessManager _invoiceDataAccessManager = null;
        public InvoiceDataAccessController(IInvoiceDataAccessManager invoiceDataAccessManager)
        {
            _invoiceDataAccessManager = invoiceDataAccessManager;
        }

        [HttpPost("save")]
        public async Task<SaveInvoiceDataAccessResponse> Save(SaveInvoiceDataAccessRequest request)
        {
            return await _invoiceDataAccessManager.Save(request);
        }

        [HttpPut("update")]
        public async Task<SaveInvoiceDataAccessResponse> Update(SaveInvoiceDataAccessRequest request)
        {
            return await _invoiceDataAccessManager.Save(request);
        }

        [HttpGet("edit/{id}")]
        public async Task<EditInvoiceDataAccessResponse> Edit(int id)
        {
            return await _invoiceDataAccessManager.Edit(id);
        }

        [HttpPost("getInvoiceDataAccessSummaryData")]
        public async Task<InvoiceDataAccessSummaryResponse> getInvoiceDataAccessSummaryData(InvoiceDataAccessSummaryRequest request)
        {
            return await _invoiceDataAccessManager.GetSummaryData(request);
        }

        [HttpDelete("delete/{id}")]
        public async Task<DeleteInvoiceDataAccessResponseResult> Delete(int id)
        {
            return await _invoiceDataAccessManager.Delete(id);
        }
    }
}
