using DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.Expense
{
    public class ExpenseClaimListResponse
    {
        public ExpenseClaimListResult Result { get; set;  }

        public IEnumerable<ExpenseClaimGroup> ExpenseClaimGroupList { get; set;  }

        public bool CanCheck { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<ExpenseStatus> ExpenseStatusList { get; set; }
        public List<int> ExpenseIdList { get; set; }
    }
    

    public class ExpenseClaimGroup
    {
        public IEnumerable<ExpenseClaim> Items { get; set; }

        public double ExpenseAmout { get; set; }

        public double FoodAllowance { get; set; }

        public double TotalAmount { get; set; }
    }

    public enum ExpenseClaimListResult
    {
        Success = 1, 
        StartDateRequired = 2, 
        EndDateRequired = 3, 
        StatusRequired = 5,
        EmployeeRequired = 6,
        NotFound = 7,
        NoAffectedLocations = 8
    }
    public class ExpenseFoodClaimRequest
    {
        [RequiredGreaterThanZero(ErrorMessage = "EXPENSE.MSG_COUNTRY_REQ")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "EXPENSE.MSG_EXDATE_REQ")]
        public DateObject ExpenseDate { get; set; }
        [RequiredGreaterThanZero(ErrorMessage = "EXPENSE.MSG_CURRENCY_REQ")]
        public int CurrencyId { get; set; }
    }
    public class ExpenseFoodClaimResponse
    {
        public ExpenseResponseResult Result { get; set; }
        public decimal ActualAmount { get; set; }
    }
    public enum ExpenseResponseResult
    {
        Success = 1,
        NoDataFound = 2,
        RequestNotCorrectFormat = 3,
        DateFormatIsNotValid = 4
    }


}
