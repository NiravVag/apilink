using Contracts.Managers;
using DTO.CommonClass;
using DTO.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InvoiceStatusController : ControllerBase
    {
        private readonly IInvoiceStatusManager _manager = null;
        private static IConfiguration _configuration = null;
        private readonly ISharedInspectionManager _helper = null;

        public InvoiceStatusController(IInvoiceStatusManager manager, IConfiguration configuration, ISharedInspectionManager helper)
        {
            _manager = manager;
            _configuration = configuration;
            _helper = helper;
        }

        //get Invoice Status Summary
        [HttpPost("getInvoiceStatusSummary")]
        public async Task<InvoiceStatusSummaryResponse> GetInvoiceStatusSummary(InvoiceStatusSummaryRequest requestDto)
        {
            return await _manager.GetInvoiceStatusSummary(requestDto);
        }


        //export Invoice Status Summary
        [HttpPost("ExportInvoiceSearchSummary")]
        public async Task<IActionResult> ExportInvoiceStatusSummary(InvoiceStatusSummaryRequest request)
        {
            int pageIndex = 0;
            int pagesize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageIndex;
            request.pageSize = pagesize;
            var response = await _manager.ExportInvoiceStatusSummary(request);
            if (response == null)
                return NotFound();
            var stream = _helper.GetAsStreamObject(response);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "invoice_Status_Summary.xlsx");
        }

        [HttpGet("getStatusByService/{serviceId}")]
        public async Task<DataSourceResponse> GetStatusByService(int serviceId)
        {
            return await _manager.GetStatusListByService(serviceId);
        }
        [HttpPost("invoice-communication-save")]
        public async Task<InvoiceCommunicationSaveResponse> SaveInvoiceCommunication(InvoiceCommunicationSaveRequest request)
        {
            return await _manager.SaveInvoiceCommunication(request);
        }
        [HttpGet("get-invoice-communication-summary/{invoiceNo}")]
        public async Task<InvoiceCommunicationTableResponse> GetInvoiceCommunicationData(string invoiceNo)
        {
            return await _manager.GetInvoiceCommunicationData(invoiceNo);
        }
    }
}
