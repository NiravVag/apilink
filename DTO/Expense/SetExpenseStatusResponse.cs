using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Expense
{
    public class SetExpenseStatusResponse
    {
        public ExpenseClaim  Data { get; set;  }

        public IEnumerable<int> UserIds { get; set; }

        public string ManagerEmail { get; set;  }

        public string ManagerName { get; set;  }

        public int ManagerUserId { get; set; }

        public IEnumerable<User.User> ClaimUserList { get; set;  } 

        public SetExpenseStatusResult Result { get; set;  }
    }

    public enum SetExpenseStatusResult
    {
        Success = 1,
        CannotUpdateStatus = 2, 
        NoAccess = 3, 
        ExpenseNotFound = 4
    }
}
