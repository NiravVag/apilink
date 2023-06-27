using Contracts.Repositories;
using DTO.Expense;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class FoodAllowanceRepository : Repository, IFoodAllowanceRepository
    {
        public FoodAllowanceRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<bool> CheckFoodAllowanceExist(int id, int countryId, DateTime startDate, DateTime endDate)
        {
            return await _context.EcFoodAllowances.
                Where(x => x.Id != id && x.Active.Value && x.CountryId == countryId &&
                !((x.StartDate > endDate) || (x.EndDate < startDate))).AnyAsync();
        }

        public async Task<EcFoodAllowance> GetFoodAllowance(int id)
        {
            return await _context.EcFoodAllowances.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<EcAutQcFoodExpense>> GetAutoQcFoodExpenseList(int countryId, DateTime startDate, DateTime endDate)
        {
            return await _context.EcAutQcFoodExpenses.
                   Where(x => x.Active.Value
                   && !x.IsFoodAllowanceConfigured.Value &&
                   !((x.ServiceDate > endDate) || (x.ServiceDate < startDate))
                   && x.FactoryCountry == countryId).ToListAsync();
        }

        public IQueryable<FoodAllowanceSummaryRepoItem> GetFoodAllowanceSummary()
        {
            return _context.EcFoodAllowances.Where(x => x.Active.Value)
                .Select(x => new FoodAllowanceSummaryRepoItem
                {
                    Id = x.Id,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CurrencyId = x.CurrencyId,
                    Currency = x.Currency.CurrencyName,
                    FoodAllowance = x.FoodAllowance
                });
        }
    }
}
