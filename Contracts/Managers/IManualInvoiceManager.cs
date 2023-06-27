using DTO.Eaqf;
using DTO.Invoice;
using DTO.InvoicePreview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IManualInvoiceManager
    {
        Task<SaveManualInvoiceResponse> SaveManualInvoice(SaveManualInvoice request);
        Task<ManualInvoiceSummaryResponse> GetManualInvoiceSummary(ManualInvoiceSummaryRequest request);
        Task<SaveManualInvoiceResponse> UpdateManualInvoice(SaveManualInvoice request);
        Task<DeleteManualInvoiceResponse> DeleteManualInvoice(int id);
        Task<GetManualInvoiceResponse> GetManualInvoice(int id);
        Task<bool> CheckInvoiceNumberExist(string invoiceNumber);

        Task<ManualInvoiceSummaryExportResponse> ExportManualInvoiceSummary(ManualInvoiceSummaryRequest request);

        Task<object> SaveEAQFManualInvoice(SaveQuotationEaqfRequest request, bool isNewCreate = false);

        Task<EAQFManualInvoiceFastReport> GetEaqfManualInvoice(int invoiceId);

        Task<SaveInvoicePdfResponse> SaveInvoicePdfUrl(int manualInvoiceId, string filePath, string uniqueId, int createdBy);
        Task<int> GetManualInvoiceIdbyInvoiceNumber(string invoiceNo);
    }
}
