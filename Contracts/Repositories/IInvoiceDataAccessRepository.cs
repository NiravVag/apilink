using DTO.InvoiceDataAccess;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IInvoiceDataAccessRepository : IRepository
    {
        IQueryable<InvDaTransaction> GetInvoiceDataAccessQuery();
        Task<List<InvoiceCustomerDataAccess>> GetInvoiceCustomerDataAccess(List<int> invoiceDataAccess);
        Task<List<InvoiceTypeDataAccess>> GetInvoiceTypeDataAccess(List<int> invoiceDataAccess);
        Task<List<InvoiceOfficeDataAccess>> GetInvoiceOfficeDataAccess(List<int> invoiceDataAccess);
        Task<InvDaTransaction> GetInvoiceDataAccess(int id);
        Task<InvDaTransaction> GetStaffInvoiceDataAccess(int staffId);
        Task<List<InvDaTransaction>> GetInvoiceDataAccessListExist(int id);
        Task<bool> IsStaffHasInvoiceDataAccess(int staffId);
    }
}
