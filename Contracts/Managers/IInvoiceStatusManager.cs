using DTO.CommonClass;
using DTO.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInvoiceStatusManager
    {
        Task<InvoiceStatusSummaryResponse> GetInvoiceStatusSummary(InvoiceStatusSummaryRequest requestDto);
        Task<List<ExportInvoiceStatus>> ExportInvoiceStatusSummary(InvoiceStatusSummaryRequest requestDto);

        Task<DataSourceResponse> GetStatusListByService(int serviceId);
        Task<InvoiceCommunicationSaveResponse> SaveInvoiceCommunication(InvoiceCommunicationSaveRequest request);
        Task<InvoiceCommunicationTableResponse> GetInvoiceCommunicationData(string invoiceNo);
    }
}
