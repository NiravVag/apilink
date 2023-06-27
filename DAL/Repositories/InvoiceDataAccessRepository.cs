using Contracts.Repositories;
using DTO.InvoiceDataAccess;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class InvoiceDataAccessRepository : Repository, IInvoiceDataAccessRepository
    {
        public InvoiceDataAccessRepository(API_DBContext context) : base(context)
        {
        }
        public IQueryable<InvDaTransaction> GetInvoiceDataAccessQuery()
        {
            return _context.InvDaTransactions.
                Include(x => x.Staff)
                .Where(x => x.Active);
        }

        public async Task<InvDaTransaction> GetInvoiceDataAccess(int id)
        {
            return await _context.InvDaTransactions.Where(x => x.Id == id && x.Active)
                          .Include(x => x.InvDaCustomers)
                          .Include(x => x.InvDaInvoiceTypes)
                          .Include(x => x.InvDaOffices)
                          .FirstOrDefaultAsync();
        }

        public async Task<List<InvDaTransaction>> GetInvoiceDataAccessListExist(int id)
        {
            return await _context.InvDaTransactions.Where(x => x.Id != id && x.Active)
                          .Include(x => x.InvDaCustomers)
                          .Include(x => x.InvDaInvoiceTypes)
                          .Include(x => x.InvDaOffices)
                          .ToListAsync();
        }

        public async Task<InvDaTransaction> GetStaffInvoiceDataAccess(int staffId)
        {
            return await _context.InvDaTransactions.Where(x => x.StaffId == staffId && x.Active)
                          .Include(x => x.InvDaCustomers)
                          .Include(x => x.InvDaInvoiceTypes)
                          .Include(x => x.InvDaOffices)
                          .FirstOrDefaultAsync();
        }

        public async Task<List<InvoiceCustomerDataAccess>> GetInvoiceCustomerDataAccess(List<int> invoiceDataAccess)
        {
            return await _context.InvDaCustomers.Where(x => invoiceDataAccess.Contains(x.InvDaId) && x.Active)
                  .Select(x => new InvoiceCustomerDataAccess
                  {
                      DataAccessId = x.InvDaId,
                      CustomerName = x.Customer.CustomerName,
                      CustomerId = x.CustomerId
                  }).AsNoTracking().ToListAsync();
        }

        public async Task<List<InvoiceTypeDataAccess>> GetInvoiceTypeDataAccess(List<int> invoiceDataAccess)
        {
            return await _context.InvDaInvoiceTypes.Where(x => invoiceDataAccess.Contains(x.InvDaId) && x.Active)
                .Select(x => new InvoiceTypeDataAccess
                {
                    DataAccessId = x.InvDaId,
                    InvoiceTypeName = x.InvoiceType.Name,
                    InvoiceTypeId = x.InvoiceTypeId
                })
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<InvoiceOfficeDataAccess>> GetInvoiceOfficeDataAccess(List<int> invoiceDataAccess)
        {
            return await _context.InvDaOffices.Where(x => invoiceDataAccess.Contains(x.InvDaId) && x.Active)
                  .Select(x => new InvoiceOfficeDataAccess
                  {
                      DataAccessId = x.InvDaId,
                      InvoiceOfficeName = x.Office.LocationName,
                      InvoiceOfficeId = x.OfficeId
                  })
               .AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsStaffHasInvoiceDataAccess(int staffId)
        {
            return await _context.InvDaTransactions.AnyAsync(x => x.StaffId == staffId && x.Active == true);
        }
    }
}
