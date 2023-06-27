using DTO.Invoice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInvoiceBankManager
    {
        Task<InvoiceBankSaveResponse> SaveInvoiceBankDetails(InvoiceBankSaveRequest request);
        Task<InvoiceBankGetResponse> GetInvoiceBankDetails(int bankId);
        Task<InvoiceBankGetAllResponse> GetAllInvoiceBankDetails(InvoiceBankSummary request);
        Task<InvoiceBankSaveResponse> UpdateInvoiceBankDetails(InvoiceBankSaveRequest request);
        Task<InvoiceBankDeleteResponse> RemoveInvoiceBankDetails(int bankId);
        Task<InvoiceBankGetResponse> GetTaxDetails(int bankId, DateTime toDate);
    }
}
