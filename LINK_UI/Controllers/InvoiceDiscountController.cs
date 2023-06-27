using Contracts.Managers;
using DTO.CommonClass;
using DTO.Invoice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceDiscountController : ControllerBase
    {
        private readonly IInvoiceDiscountManager _invoiceDiscountManager = null;

        public InvoiceDiscountController(IInvoiceDiscountManager invoiceDiscountManager)
        {
            _invoiceDiscountManager = invoiceDiscountManager;
        }

        [HttpGet]
        [Route("get-invoice-discount-type")]
        public async Task<DataSourceResponse> GetInvoiceDiscountTypes()
        {
            return await _invoiceDiscountManager.GetInvoiceDiscountTypes();
        }

        [HttpPost]
        [Route("invoice-discount-summary")]
        public async Task<InvoiceDiscountSearchResponse> GetInvoiceDiscountSummary(InvoiceDiscountSearchRequest request)
        {
            return await _invoiceDiscountManager.GetInvoiceDiscountSummary(request);
        }

        [HttpDelete]
        [Route("delete-invoice-discount/{id}")]
        public async Task<DeleteInvoiceDiscountResponse> DeleteInvoiceDiscount(int id)
        {
            return await _invoiceDiscountManager.DeleteInvoiceDiscount(id);
        }

        [HttpPost]
        [Route("save-invoice-discount")]
        public async Task<SaveInvoiceDiscountResponse> SaveInvoiceDiscount([FromBody] SaveInvoiceDiscount request)
        {
            return await _invoiceDiscountManager.SaveInvoiceDiscount(request);
        }

        [HttpPost]
        [Route("update-invoice-discount")]
        public async Task<SaveInvoiceDiscountResponse> UpdateInvoiceDiscount([FromBody] SaveInvoiceDiscount request)
        {
            return await _invoiceDiscountManager.UpdateInvoiceDiscount(request);
        }

        [HttpGet]
        [Route("edit-invoice-discount/{id}")]
        public async Task<EditInvoiceDiscountResponse> EditInvoiceDiscount(int id)
        {
            return await _invoiceDiscountManager.EditInvoiceDiscount(id);
        }

        [HttpPost]
        [Route("get-customer-bussiness-countries")]
        public async Task<DataSourceResponse> GetCustomerBussinessCountries(CommonDataSourceRequest request)
        {
            return await _invoiceDiscountManager.GetCustomerBusinessCountries(request);
        }

    }
}
