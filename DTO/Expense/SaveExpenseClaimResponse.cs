using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Expense
{
    public class SaveExpenseClaimResponse
    {
        public SaveExpenseClaimResult Result { get; set; }

        public IEnumerable<User.User> UserList { get; set; }

        public ExpenseClaim ExpenseClaim { get; set; }

        public List<ExpenseClaim> ExpenseClaimList { get; set; }

        public List<int?> CreatedExpenseBookingIds { get; set; }
    }

    public enum SaveExpenseClaimResult
    {
        Success = 1,
        RequestIncorrect = 2,
        CurrentExpenseClaimNotFound = 3,
        UnAuthorized = 4,
        StaffEntityNotMatched = 5,
        ClaimNumberAlreadyExist = 6,
        ClaimAlreadyDoneForInspection = 7
    }


    public class SaveOutSourceExpenseClaimResponse
    {
        public SaveExpenseClaimResult Result { get; set; }

        public IEnumerable<User.User> UserList { get; set; }

        public List<ExpenseClaim> ExpenseClaimList { get; set; }
    }
}
