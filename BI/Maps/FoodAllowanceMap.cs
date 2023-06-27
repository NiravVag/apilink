using DTO.Common;
using DTO.Expense;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class FoodAllowanceMap: ApiCommonData
    {
        public FoodAllowanceSummaryItem MapFoodAllowanceSummary(FoodAllowanceSummaryRepoItem entity, bool hasItRole)
        {
            return new FoodAllowanceSummaryItem
            {
                Id = entity.Id,
                CountryName = entity.CountryName,
                Currency = entity.Currency,
                StartDate = entity.StartDate.ToString(StandardDateFormat),
                EndDate = entity.EndDate.ToString(StandardDateFormat),
                FoodAllowance = entity.FoodAllowance,
                ShowDeleteButton = hasItRole
            };
        }

        public FoodAllowanceSummaryEditItem MapFoodAllowanceEdit(FoodAllowanceSummaryRepoItem entity)
        {
            return new FoodAllowanceSummaryEditItem
            {
                Id = entity.Id,
                CountryId = entity.CountryId,
                CurrencyId = entity.CurrencyId,
                StartDate = Static_Data_Common.GetCustomDate(entity.StartDate),
                EndDate = Static_Data_Common.GetCustomDate(entity.EndDate),
                FoodAllowance = entity.FoodAllowance
            };
        }
    }
}
