using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Expense
{
    public class ExpenseStatus
    {
        public int Id { get; set;  }

        public string Label { get; set;  }

        public int TotalCount { get; set; }

        public string LabelColor { get; set; }
    }
}
