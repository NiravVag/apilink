using DTO.HumanResource;
using DTO.OfficeLocation;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.Expense
{
    public class ExpenseClaimListRequest
    {
        public int EmployeeTypeId { get; set; }
        public IEnumerable<StaffInfo> EmployeeValues { get; set; }

        public IEnumerable<ExpenseStatus> StatusValues { get; set; }

        public IEnumerable<Office> LocationValues { get; set; }

        public DateObject StartDate { get; set; }

        public DateObject EndDate { get; set; }

        public IEnumerable<int> ClaimTypeIds { get; set; }

        public bool IsClaimDate { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }

        public List<int> ClaimIdList { get; set; }

        public List<int> CompanyIds { get; set; }
        public IEnumerable<int> PayrollCompanyIds { get; set; }
        public int ExportType { get; set; }
        public bool IsAutoExpense { get; set; }

    }

    public class ExpenseClaimDataRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IEnumerable<int> StatusIds { get; set; }

        public IEnumerable<int> StaffIds { get; set; }

        public IEnumerable<int> LocIds { get; set; }

        public bool IsClaimDate { get; set; }

        public int Take { get; set; }

        public int Skip { get; set; }

        public IEnumerable<int> ClaimTypeIds { get; set; }
        public IEnumerable<int> PayrollCompanyIds { get; set; }
        public bool IsAutoExpense { get; set; }

    }

    public class PendingExpenseRequest
    {
        public int QcId { get; set; }
        public List<int> BookingList { get; set; }
    }

    public class ExpenseClaimUpdateStatus
    {
        public int Id { get; set; }
        public bool ExpenseType { get; set; }
        public int CurrentStatusId { get; set; }
        public int NextStatusId { get; set; }
    }

    public class PendingBookingExpenseRequest
    {
        public int QcId { get; set; }
        public List<int?> BookingIdList { get; set; }
    }

    public class PendingBookingExpenseResponse
    {
        public List<int> BookingIdList { get; set; }
        public PendingBookingExpenseResponseResult Result { get; set; }

    }

    public enum PendingBookingExpenseResponseResult
    {
        notConfigure = 1,
        configure = 2
    }
    public enum ExportExpenseType
    {
        ExpenseSummaryKPI = 1,
        ExpenseDetailKPI = 2
    }

}
