using DTO.CommonClass;
using DTO.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInvoiceDiscountManager
    {
        Task<DataSourceResponse> GetInvoiceDiscountTypes();
        Task<InvoiceDiscountSearchResponse> GetInvoiceDiscountSummary(InvoiceDiscountSearchRequest request);
        Task<DeleteInvoiceDiscountResponse> DeleteInvoiceDiscount(int id);
        Task<SaveInvoiceDiscountResponse> SaveInvoiceDiscount(SaveInvoiceDiscount request);
        Task<SaveInvoiceDiscountResponse> UpdateInvoiceDiscount(SaveInvoiceDiscount request);
        Task<EditInvoiceDiscountResponse> EditInvoiceDiscount(int id);
        Task<DataSourceResponse> GetCustomerBusinessCountries(CommonDataSourceRequest request);
    }
}
