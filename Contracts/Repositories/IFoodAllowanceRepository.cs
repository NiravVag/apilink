using DTO.Expense;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IFoodAllowanceRepository: IRepository
    {
        Task<bool> CheckFoodAllowanceExist(int id, int countryId, DateTime startDate, DateTime endDate);
        Task<EcFoodAllowance> GetFoodAllowance(int id);
        IQueryable<FoodAllowanceSummaryRepoItem> GetFoodAllowanceSummary();
        Task<List<EcAutQcFoodExpense>> GetAutoQcFoodExpenseList(int countryId, DateTime startDate, DateTime endDate);
    }
}
