using DTO.Expense;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.FileModels
{
    public class ExpenseSummaryModel
    {
        public IEnumerable<ExpenseClaim> Items { get; set; }

        public string Employes { get; set;  }


        public IEnumerable<ExpenseStatus> StatusList { get; set; }

        public string LocationName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string DateType { get; set;  }
    }
}
