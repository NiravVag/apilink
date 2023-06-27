using Contracts.Repositories;
using DTO.SaleInvoice;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class SaleInvoiceRepository : Repository, ISaleInvoiceRepository
    {
        public SaleInvoiceRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<List<SaleInvoiceFile>> GetInvoiceTransactionFiles(List<int> invoiceIdList)
        {
            return await _context.InvTranFiles.Where(x => invoiceIdList.Contains(x.InvoiceId.GetValueOrDefault()) && x.Active.Value).Select(x => new SaleInvoiceFile()
            {
                InvoiceNo = x.InvoiceNo,
                UniqueId = x.UniqueId
            }).AsNoTracking().ToListAsync();
        }
    }
}
