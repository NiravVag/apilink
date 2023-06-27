using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Expense
{

    public class ExpenseClaimTypeResponse
    {
        public IEnumerable<ExpenseClaimType> expenseClaimTypeList { get; set; }
        public ExpenseClaimTypeResult expenseClaimTypeResult { get; set; }
    }

    public class ExpenseClaimType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
    
    public enum ExpenseClaimTypeResult
    {
       Success=1,NotFound=2
    }
}
