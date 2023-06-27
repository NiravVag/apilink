using DTO.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.FileModels
{
    public class ExpenseClaimModel
    {
        public ExpenseClaim Item { get; set;  }

        public double? Total { get; set;  }

        public IDictionary<byte[], string> ImageList { get; set;  }
    }
}
