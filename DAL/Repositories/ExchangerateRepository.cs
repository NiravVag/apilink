using Contracts.Repositories;
using DTO.FinanceDashboard;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ExchangeRateRepository : Repository, IExchangeRateRepository
    {
        public ExchangeRateRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EmExchangeRateType>> GetRateTypeList()
        {
            return await _context.EmExchangeRateTypes.Where(x => x.Active != null && x.Active.Value).ToListAsync();
        }

        public async Task<IEnumerable<EmExchangeRate>> GetExchangeRateList(int currencyId, DateTime fromDate, DateTime toDate, int exchangeTypeId)
        {
            return await _context.EmExchangeRates
                                .Include(x => x.Currencyid2Navigation)
                                .Include(x => x.CurrencyId1Navigation)
                                .Where(x => x.Active != null && x.Active.Value
                                        && x.CurrencyId1 == currencyId
                                        && x.ExRateTypeId == exchangeTypeId
                                        && ((x.BeginDate >= fromDate && x.BeginDate <= toDate)
                                        || (x.EndDate >= fromDate && x.EndDate <= toDate)))
                                .OrderBy(x => x.BeginDate).ToListAsync();
        }

        public async Task<IEnumerable<EmExchangeRate>> GetExchangeRateList(int currencyId, DateTime date, int exchangeTypeId)
        {
            return await _context.EmExchangeRates
                    .Include(x => x.Currencyid2Navigation)
                    .Include(x => x.CurrencyId1Navigation)
                    .Where(x => x.Active != null && x.Active.Value
                            && x.CurrencyId1 == currencyId
                            && x.ExRateTypeId == exchangeTypeId
                            && x.BeginDate <= date
                            && x.EndDate >= date)
                    .OrderBy(x => x.BeginDate).ToListAsync();
        }

        public async Task<IEnumerable<EmExchangeRate>> GetConversionList(IEnumerable<int> idList)
        {
            return await _context.EmExchangeRates.Where(x => idList.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<EmExchangeRate>> GetExchanges(int targetId, int currencyId, DateTime date, int type)
        {
            return await _context.EmExchangeRates.Where(x => date >= x.BeginDate
                                && date <= x.EndDate
                                && x.ExRateTypeId == type
                                && x.Active.HasValue && x.Active.Value
                                && (x.CurrencyId1 == targetId || x.Currencyid2 == targetId || x.CurrencyId1 == currencyId || x.Currencyid2 == currencyId))
                                .ToListAsync();
        }

        public async Task<decimal> GetFoodAllowance(DateTime date, int countryId)
        {
            var item = await _context.EcFoodAllowances.FirstOrDefaultAsync(x => date >= x.StartDate && date <= x.EndDate && x.CountryId == countryId);

            if (item != null)
                return item.FoodAllowance;

            return 0;
        }

        public async Task<IEnumerable<EmExchangeRate>> GetExchangesList(List<ExchangeCurrency> currencyList, DateTime date, int type)
        {
            var targetCurrencyList = currencyList.Select(x => x.TargetCurrency).ToList();
            var currencyToConvertList = currencyList.Select(x => x.Currency).ToList();

            return await _context.EmExchangeRates.Where(x => x.Active == true && date >= x.BeginDate
                                && date <= x.EndDate
                                && x.ExRateTypeId == type
                                && (targetCurrencyList.Contains(x.CurrencyId1) || targetCurrencyList.Contains(x.Currencyid2) ||
                                currencyToConvertList.Contains(x.CurrencyId1) || currencyToConvertList.Contains(x.Currencyid2)))
                                .AsNoTracking().ToListAsync();
        }
    }
}
