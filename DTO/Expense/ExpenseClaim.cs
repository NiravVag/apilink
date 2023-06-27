using DTO.HumanResource;
using DTO.Location;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.Expense
{
    public class ExpenseClaim
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LocationName { get; set; }

        public string StaffEmail { get; set; }

        public DateObject ClaimDate { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int LocationId { get; set; }

        public int StaffId { get; set; }

        public string ClaimNo { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public string ExpensePuropose { get; set; }

        public int? CurrencyId { get; set; }

        public double? ExpenseAmout { get; set; }

        public double? FoodAllowance { get; set; }

        public string CurrencyName { get; set; }

        public double? TotalAmount { get; set; }

        public double? Tax { get; set; }

        public double? TaxTotalAmount { get; set; }

        public int ManDay { get; set; }

        public string StatusUserName { get; set; }

        public string StatusDate { get; set; }

        public IEnumerable<ExpenseClaimDetails> ExpenseList { get; set; }

        public bool CanApprove { get; set; }

        public string Comment { get; set; }

        public int? ClaimTypeId { get; set; }

        public string ClaimType { get; set; }

        public bool isChecked { get; set; }

        public bool ManagerApproveRequired { get; set; }
        public bool? IsAutoExpense { get; set; }
        public bool? IsTraveAllowanceExist { get; set; }
        public bool? IsFoodAllowanceExist { get; set; }

        public int UserTypeId { get; set; }

        public int? EmployeeTypeId { get; set; }

        public string EmployeeType { get; set; }

        public string OutSourceCompanyName { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public bool IsTravelExpense { get; set; }
        public string OfficeName { get; set; }
        public string PayrollCompanyName { get; set; }
        public string AutoExpense { get; set; }
        public List<int?> BookingIdList { get; set; }

    }

    public class ExpenseClaimDetails
    {
        public int Id { get; set; }

        public int? BookingNo { get; set; }

        public DateObject ExpenseDate { get; set; }

        public int ExpenseTypeId { get; set; }

        public string ExpenseTypeLabel { get; set; }

        public bool Receipt { get; set; }

        public City StartCity { get; set; }

        public City DestCity { get; set; }

        public int CurrencyId { get; set; }

        public int? TripMode { get; set; }

        public string CurrencyName { get; set; }

        public double ActualAmount { get; set; }

        public double? Tax { get; set; }

        public double? TaxAmount { get; set; }

        public int? ManDay { get; set; }

        public double? ExchangeRate { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }

        public bool? IsAutoExpense { get; set; }

        public IEnumerable<ExpenseClaimReceipt> Files { get; set; }

        public int? QcId { get; set; }

        public string QcName { get; set; }
        public string StartPortName { get; set; }
        public string EndPortName { get; set; }
    }

    public class ExpenseClaimReceipt
    {
        public int Id { get; set; }

        public Guid GuidId { get; set; }

        public string Uniqueld { get; set; }

        public string FileName { get; set; }

        public bool IsNew { get; set; }

        public string MimeType { get; set; }

        public string FileUrl { get; set; }

        public int ExpenseId { get; set; }

    }

    public class HROutSourceCompany
    {
        public int? QcId { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }
    }
    public class ExpenseQCPort
    {
        public string StartPort { get; set; }
        public string EndPort { get; set; }
        public int TravelQCExpenseId { get; set; }
    }

    public class ExpenseDataRepo
    {
        public DateTime ClaimDate { get; set; }
        public string ClaimNo { get; set; }
        public int? ClaimTypeId { get; set; }
        public int CountryId { get; set; }
        public string ExpensePurpose { get; set; }
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int StaffId { get; set; }
        public int StatusId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CheckedDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime? RejectDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public string ApproveByFullName { get; set; }
        public string CheckedByFullName { get; set; }
        public string PaidByFullName { get; set; }
        public string RejectByFullName { get; set; }
        public string CancelByFullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ClaimTypeName { get; set; }
        public string PersonName { get; set; }
        public int? PayrollCurrencyId { get; set; }
        public string PayrollCurrencyName { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }
        public string HrOutSourceCompanyName { get; set; }
        public string PayrollCompanyName { get; set; }
        public string LocationName { get; set; }
        public string StatusName { get; set; }
        public string Comment { get; set; }
        public bool? IsAutoExpense { get; set; }
    }
    public class ExpenseDetailsRepo
    {
        public bool? IsAutoExpense { get; set; }
        public int? BookingId { get; set; }
        public double Amount { get; set; }
        public int ExpenseId { get; set; }
        public int ExpenseTypeId { get; set; }
    }
}
