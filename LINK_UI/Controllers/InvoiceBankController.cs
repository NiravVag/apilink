using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InvoiceBankController : ControllerBase
    {

        private readonly IInvoiceBankManager _invoiceBankManager = null;

        public InvoiceBankController(IInvoiceBankManager invoiceBankManager)
        {
            _invoiceBankManager = invoiceBankManager;
        }

        [HttpPost]
        public async Task<InvoiceBankSaveResponse> Save(InvoiceBankSaveRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new InvoiceBankSaveResponse() { Result = InvoiceBankSaveResult.InvoiceBankRequestIsNotValid };
            }
            return await _invoiceBankManager.SaveInvoiceBankDetails(request);
        }

        [HttpGet("{id}")]
        public async Task<InvoiceBankGetResponse> Get(int id)
        {
            return await _invoiceBankManager.GetInvoiceBankDetails(id);
        }

        [HttpGet("{index}/{pageSize}")]
        public async Task<InvoiceBankGetAllResponse> Get(int index, int pageSize)
        {
            InvoiceBankSummary objRequest = new InvoiceBankSummary() { Index = index, PageSize = pageSize };
            return await _invoiceBankManager.GetAllInvoiceBankDetails(objRequest);
        }

        [HttpPut("{id}")]
        public async Task<InvoiceBankSaveResponse> Update(InvoiceBankSaveRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new InvoiceBankSaveResponse() { Result = InvoiceBankSaveResult.InvoiceBankRequestIsNotValid };
            }
            return await _invoiceBankManager.UpdateInvoiceBankDetails(request);
        }

        [HttpDelete("{id}")]
        public async Task<InvoiceBankDeleteResponse> Delete(int id)
        {
            return await _invoiceBankManager.RemoveInvoiceBankDetails(id);
        }

        [HttpPost("getTaxDetails/{bankId}")]
        public async Task<InvoiceBankGetResponse> GetTaxDetails(int bankId, InvoiceBankTaxRequest request)
        {
            return await _invoiceBankManager.GetTaxDetails(bankId, request.ToDate.ToDateTime());
        }

    }
}
