using DTO.SaleInvoice;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ISaleInvoiceRepository : IRepository
    {
        Task<List<SaleInvoiceFile>> GetInvoiceTransactionFiles(List<int> invoiceIdList);
    }
}
