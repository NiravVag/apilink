using DTO.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IFoodAllowanceManager
    {
        Task<SaveResponse> Save(FoodAllowance request);
        Task<SaveResponse> Update(FoodAllowance request);
        Task<SaveResponse> Delete(int id);
        Task<FoodAllowanceSummaryResponse> GetFoodAllowanceSummary(FoodAllowanceSummaryRequest request);
        Task<FoodAllowanceEditResponse> EditFoodAllowance(int id);
    }
}
