using DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Expense
{
    public class FoodAllowance
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public DateObject StartDate { get; set; }
        public DateObject EndDate { get; set; }
        public double FoodAllowanceValue { get; set; }
        public int CurrencyId { get; set; }
    }

    public enum FoodAllowanceResult
    {
        Success = 1,
        Fail = 2,
        NotFound = 3,
        AlreadyExists = 4
    }
    public class SaveResponse
    {
        public FoodAllowanceResult Result { get; set; }
    }
    public class FoodAllowanceSummaryRequest
    {
        public int? CountryId { get; set; }
        public DateObject StartDate { get; set; }
        public DateObject EndDate { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }

    public class FoodAllowanceSummaryRepoItem
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal FoodAllowance { get; set; }
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
    }

    public class FoodAllowanceSummaryItem
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal FoodAllowance { get; set; }
        public string Currency { get; set; }
        public bool ShowDeleteButton { get; set; }
    }
    public class FoodAllowanceSummaryResponse
    {
        public List<FoodAllowanceSummaryItem> Data { get; set; }
        public FoodAllowanceResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
    public class FoodAllowanceSummaryEditItem
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public DateObject StartDate { get; set; }
        public DateObject EndDate { get; set; }
        public decimal FoodAllowance { get; set; }
        public int CurrencyId { get; set; }
    }
    public class FoodAllowanceEditResponse
    {
        public List<FoodAllowanceSummaryEditItem> Data { get; set; }
        public FoodAllowanceResult Result { get; set; }
    }
}
