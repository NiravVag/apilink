using DTO.SaleInvoice;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ISaleInvoiceManager
    {
        Task<SaleInvoiceSummaryResponse> GetSaleInvoiceSummary(SaleInvoiceSummaryRequest request);
        Task<dynamic> ExportSaleInvoiceSearchSummary(SaleInvoiceSummaryRequest request);
    }
}
