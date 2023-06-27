using DTO.CommonClass;
using DTO.Inspection;
using DTO.Location;
using DTO.References;
using DTO.RepoRequest.Audit;
using DTO.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DTO.Expense
{
    public class ExpenseClaimResponse
    {
        public IEnumerable<Country> CountryList { get; set; }

        public IEnumerable<Currency> CurrencyList { get; set; }

        public IEnumerable<ExpenseTypeSource> ExpenseTypeList { get; set; }

        public ExpenseClaim ExpenseClaim { get; set; }

        public IEnumerable<ExpenseStatus> StatusList { get; set; }

        public bool CanEdit { get; set; }

        public bool CanApprove { get; set; }

        public bool CanCheck { get; set; }

        public ExpenseClaimResult Result { get; set; }

        public ExpenseBookingDetailAccess expenseBookingDetailAccess { get; set; }
    }

    public class ExpenseBookingDetailAccess
    {
        public bool BookingNoVisible { get; set; }

        public bool BookingNoEnabled { get; set; }

        public bool BookingDetailVisible { get; set; }

        public bool ClaimTypeEnabled { get; set; }
    }

    public enum ExpenseClaimResult
    {
        Success = 1,
        CannotFindCountries = 2,
        CannotFindCurrentStaff = 3,
        CannotFindLocation = 4,
        CannotFindCurrencies = 5,
        CannotFindExpenseTypes = 6,
        CannotFindCurrentExpenseClaim = 7,
        CannotShowThisExpense = 8,
        CannotFindStatsList = 9
    }

    public class ExpenseClaimVoucherItem
    {
        public int Id { get; set; }
        public int ClaimDetailId { get; set; }
        public DateTime Date { get; set; }
        public int ExpenseTypeId { get; set; }
        public int? ClaimTypeId { get; set; }
        public double? Amount { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public string RegionalName { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string ClaimNo { get; set; }
        public string BankAccountNo { get; set; }
        public string PayrollCurrency { get; set; }
        public string Status { get; set; }
        public string ClaimType { get; set; }
        public int? PaidId { get; set; }
        public int LocationId { get; set; }
        public int? DeptId { get; set; }
        public string PayrollCompanyName { get; set; }
        public string OfficeName { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int? InspectionId { get; set; }
        public int? AuditId { get; set; }
        public string CountryName { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string StartPortName { get; set; }
        public int EndPortName { get; set; }
        public string CurrencyName { get; set; }
        public string ExpenseStatus { get; set; }
        public string TripTypeName { get; set; }
        public bool? IsAutoExpense { get; set; }
    }

    public class ExpenseClaimVoucherData
    {
        public List<ExpenseVoucherExportItem> ClaimData { get; set; }
        public string Entity { get; set; }
        public string Office { get; set; }
        public string ClaimType { get; set; }
        public string EnglishName { get; set; }
        public string RegionalName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }

    public class ExpenseBookingData
    {
        public int ClaimId { get; set; }
        public int ClaimDetailId { get; set; }
        public int? BookingId { get; set; }
    }

    public class ExpenseAuditData
    {
        public int ClaimId { get; set; }
        public int ClaimDetailId { get; set; }
        public int? AuditId { get; set; }
    }

    public class ExpenseVoucherExportItem
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string InspNo { get; set; }
        public string AuditNo { get; set; }
        public double TravelAllowance { get; set; }
        public double ReportAllowance { get; set; }
        public double AddOrDeductAllowance { get; set; }
        public double AllowanceTotal { get; set; }
        public double PlaneTravel { get; set; }
        public double FerryTravel { get; set; }
        public double TaxiTravel { get; set; }
        public double DdTravel { get; set; }
        public double TrainTravel { get; set; }
        public double BusTravel { get; set; }
        public double OtherTravel { get; set; }
        public double HotelExpense { get; set; }
        public double AirportTax { get; set; }
        public double Visa { get; set; }
        public double OtherExpense { get; set; }
        public double MiscellaneousExpense { get; set; }
        public double EntertainmentExpense { get; set; }
        public double TravelFeeTotal { get; set; }
        public double Total { get; set; }
        public string ClaimNo { get; set; }
        public string ExpenseStatus { get; set; }
        public string OfficeName { get; set; }
        public string PayrollCompanyName { get; set; }
        public string AutoExpense { get; set; }
    }

    public class ExportExpenseClaimSummaryKpi
    {
        public string Location { get; set; }
        public string DeptName { get; set; }
        public string ClaimDate { get; set; }
        public string ToDate { get; set; }
        public string ClaimNo { get; set; }
        public string EmpName { get; set; }
        public string RegionalEmpName { get; set; }
        public string BankAccNo { get; set; }
        public double TotalTravelExpense { get; set; }
        public double TravelAllowance { get; set; }
        public double ReportAllowance { get; set; }
        public double FoodAllowance { get; set; }
        public double OtherExpense { get; set; }
        public double TotalAmt { get; set; }
        public string PayrollCurrency { get; set; }
        public string ExpenseStatus { get; set; }
        public string ClaimType { get; set; }
        public string PaymentStatus { get; set; }
        public string PayrollCompanyName { get; set; }
        public string CustomerName { get; set; }
        public string ExpenseDate { get; set; }
        public string Country { get; set; }
        public double MandayCost { get; set; }
        public double HotelExpense { get; set; }
        public double Phone { get; set; }
        public double Visa { get; set; }
        public string AutoExpense { get; set; }
    }

    public class ExportExpenseClaimSummaryKpiResponse
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<ExportExpenseClaimSummaryKpi> Data { get; set; }
    }
    public class ExpenseExportKPIMap
    {
        public List<ExpenseClaimVoucherItem> expenseClaimList { get; set; }
        public IEnumerable<CommonDataSource> deptData { get; set; }
        public ExpenseClaimListRequest request { get; set; }
        public IEnumerable<BookingDetail> InspectionBookingList { get; set; }
        public IEnumerable<AuditCusFactDetails> AuditBookingList { get; set; }
        public List<FactoryCountry> AuditFactoryAddressList { get; set; }
        public List<FactoryCountry> InspFactoryAddressList { get; set; }
        public List<AuditServiceTypeData> AuditServiceTypeList { get; set; }
        public IEnumerable<ServiceTypeList> InspServiceTypeList { get; set; }
    }

    public class ExportExpenseSummaryKpiResponse
    {
        public List<ExportExpenseSummaryDetailKpi> ExpenseSummaryDetailKpiList { get; set; }
        public ExpenseSummaryDetailKpiListResult Result { get; set; }
    }

    public enum ExpenseSummaryDetailKpiListResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
    public class ExportExpenseSummaryDetailKpi
    {
        [Description("UserName")]
        public string EmployeeName { get; set; }
        [Description("Claim#")]
        public string ClaimNumber { get; set; }
        [Description("Inspection#")]
        public int? ClaimInspectionNumber { get; set; }
        [Description("Audit#")]
        public int? ClaimAuditNumber { get; set; }
        [Description("Country")]
        public string Country { get; set; }
        [Description("Province")]
        public string Province { get; set; }
        [Description("City")]
        public string City { get; set; }
        [Description("County")]
        public string County { get; set; }
        [Description("Customer")]
        public string Customer { get; set; }
        [Description("Inspection Date")]
        public string InspDate { get; set; }
        [Description("Expense Date")]
        public string ExpenseDate { get; set; }
        [Description("From City")]
        public string FromCity { get; set; }
        [Description("To City")]
        public string ToCity { get; set; }
        [Description("Start Port")]
        public string StartPort { get; set; }
        [Description("End Port")]
        public string EndPort { get; set; }
        [Description("Bus")]
        public double? Bus { get; set; }
        [Description("Taxi")]
        public double? Taxi { get; set; }
        [Description("Train")]
        public double? Train { get; set; }
        [Description("Ferry")]
        public double? Ferry { get; set; }
        [Description("Air")]
        public double? Air { get; set; }
        [Description("Travelling Other Modes")]
        public double? TravelOtherModes { get; set; }
        [Description("Hotel Expense")]
        public double? HotelExpense { get; set; }
        [Description("Food Allowance")]
        public double? FoodAllowance { get; set; }
        [Description("Visa")]
        public double? Visa { get; set; }
        [Description("Phone")]
        public double? Phone { get; set; }
        [Description("Manday Cost")]
        public double? MandayCost { get; set; }
        [Description("Others")]
        public double? Others { get; set; }
        [Description("Total")]
        public double? Total { get; set; }
        [Description("Currency")]
        public string Currency { get; set; }
        [Description("Expense Status")]
        public string ExpenseStatus { get; set; }
        [Description("Booking Status")]
        public string BookingStatus { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Trip Type")]
        public string TripType { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }
    }

    public class ExpenseBookingDataMap
    {
        public string CustomerName { get; set; }
        public string FactoryName { get; set; }
        public string StatusName { get; set; }
        public DateTime? InspDate { get; set; }
        public string CountryName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Town { get; set; }
        public string ServiceTypeName { get; set; }
    }
}