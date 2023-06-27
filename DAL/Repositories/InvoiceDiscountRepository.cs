using Contracts.Repositories;
using DTO.CommonClass;
using DTO.Invoice;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{

    /// <summary>
    /// invoice discount repo
    /// </summary>
    public class InvoiceDiscountRepository : Repository, IInvoiceDiscountRepository
    {
        public InvoiceDiscountRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// get invoice discount types
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInvoicDiscountTypes()
        {
            return await _context.InvDisRefTypes.Where(x => x.Active.HasValue && x.Active.Value).Select(x => new CommonDataSource()
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get invoice discount summary
        /// </summary>
        /// <returns></returns>
        public IQueryable<InvDisTranDetail> GetInvoiceDiscountSummary()
        {
            return _context.InvDisTranDetails.Where(x => x.Active.HasValue && x.Active.Value);
        }

        /// <summary>
        /// get invoice discount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InvDisTranDetail> GetInvoiceDiscount(int id)
        {
            return await _context.InvDisTranDetails.FindAsync(id);
        }

        /// <summary>
        /// get countries by discount ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<InvoiceDiscountCommonDataSource>> GetCountryByInvDisIds(IEnumerable<int> ids)
        {
            return await _context.InvDisTranCountries.Where(x => x.Active.HasValue && x.Active.Value && ids.Contains(x.DiscountId)).Select(x => new InvoiceDiscountCommonDataSource()
            {
                Id = x.CountryId,
                Name = x.Country.CountryName,
                DiscountId = x.DiscountId
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get invoice discount periods
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvDisTranPeriodInfo>> GetInvoiceDiscountPeriods(IEnumerable<int> ids)
        {
            return await _context.InvDisTranPeriodInfos.Where(x => x.Active.HasValue && x.Active.Value && ids.Contains(x.DiscountId)).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get invoice discount transaction country by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvDisTranCountry>> GetInvoiceDiscountCountry(IEnumerable<int> ids)
        {
            return await _context.InvDisTranCountries.Where(x => ids.Contains(x.DiscountId)).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// check invoice discount by discount id, discount type, period from and period to 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dicountType"></param>
        /// <param name="periodFrom"></param>
        /// <param name="periodTo"></param>
        /// <returns></returns>
        public async Task<bool> CheckInvoiceDiscountPeriod(int id, int customerId,int dicountType, DateTime periodFrom, DateTime periodTo)
        {
            return await _context.InvDisTranDetails.AnyAsync(x => x.Id != id && x.CustomerId==customerId && x.DiscountType == dicountType && ((x.PeriodFrom <= periodFrom && periodFrom <= x.PeriodTo) || (x.PeriodFrom <= periodTo && periodTo <= x.PeriodTo)));
        }        

        public async Task<List<CommonDataSource>> GetCustomerBussinessCountriesByCustomerId(int customerId)
        {
            return await _context.CuCustomerBusinessCountries.Where(x => x.CustomerId == customerId).Select(x => new CommonDataSource() { Id = x.BusinessCountryId, Name = x.BusinessCountry.CountryName }).AsNoTracking().ToListAsync();
        }
    }
}
