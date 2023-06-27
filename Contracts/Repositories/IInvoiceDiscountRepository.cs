using DTO.CommonClass;
using DTO.Invoice;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    /// <summary>
    /// interface invoice discount repository
    /// </summary>
    public interface IInvoiceDiscountRepository : IRepository
    {
        Task<List<CommonDataSource>> GetInvoicDiscountTypes();
        IQueryable<InvDisTranDetail> GetInvoiceDiscountSummary();
        Task<bool> CheckInvoiceDiscountPeriod(int id, int cutomerId,int dicountType, DateTime periodFrom, DateTime periodTo);
        Task<InvDisTranDetail> GetInvoiceDiscount(int id);
        Task<List<InvoiceDiscountCommonDataSource>> GetCountryByInvDisIds(IEnumerable<int> ids);

        Task<IEnumerable<InvDisTranPeriodInfo>> GetInvoiceDiscountPeriods(IEnumerable<int> ids);
        Task<IEnumerable<InvDisTranCountry>> GetInvoiceDiscountCountry(IEnumerable<int> ids);

        Task<List<CommonDataSource>> GetCustomerBussinessCountriesByCustomerId(int customerId);
    }
}
