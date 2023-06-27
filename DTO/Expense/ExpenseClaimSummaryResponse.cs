using DTO.CommonClass;
using DTO.HumanResource;
using DTO.Location;
using DTO.OfficeLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Expense
{
    public class ExpenseClaimSummaryResponse
    {
        public IEnumerable<Office> LocationList { get; set;  }

        public IEnumerable<StaffInfo> EmployeeList { get; set;  }

        public IEnumerable<ExpenseStatus> StatusList { get; set;  }

        public List<CommonDataSource> HrOutSourceCompanyList { get; set; }

        public ExpenseClaimSummaryResult Result { get; set;  }
    }

    public enum ExpenseClaimSummaryResult
    {
        Success = 1,
        CannotFindLocationList = 2, 
        CannotFindEmployeeList = 3, 
        CannotFindStatsList = 4
    }
}
