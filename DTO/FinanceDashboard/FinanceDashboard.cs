using DTO.Common;
using DTO.CommonClass;
using DTO.Kpi;
using DTO.Manday;
using DTO.Schedule;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.FinanceDashboard
{
    public class FinanceDashboardRequest
    {
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> FactoryIdList { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        public DateObject ServiceDateFrom { get; set; }

        [DateGreaterThanAttribute(otherPropertyName = "ServiceDateFrom", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        public DateObject ServiceDateTo { get; set; }
        public IEnumerable<int?> DeptIdList { get; set; }
        public IEnumerable<int?> BrandIdList { get; set; }
        public IEnumerable<int?> BuyerIdList { get; set; }
        //public IEnumerable<int?> SelectedCollectionIdList { get; set; }
        public IEnumerable<int> StatusIdList { get; set; }
        public List<int> CountryIdList { get; set; }
        public List<int> OfficeIdList { get; set; }
        public List<int?> ServiceTypeList { get; set; }
        public List<int?> RatioCustomerIdList { get; set; }
        public bool IsBilledMandayExport { get; set; }
    }
    public class FinanceDashboardSearchRequest
    {
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> FactoryIdList { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        public DateObject ServiceDateFrom { get; set; }

        [DateGreaterThanAttribute(otherPropertyName = "ServiceDateFrom", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        public DateObject ServiceDateTo { get; set; }
        public IEnumerable<int> DeptIdList { get; set; }
        public IEnumerable<int> BrandIdList { get; set; }
        public IEnumerable<int> BuyerIdList { get; set; }
        //public IEnumerable<int?> SelectedCollectionIdList { get; set; }
        public IEnumerable<int> StatusIdList { get; set; }
        public List<int> CountryIdList { get; set; }
        public List<int> OfficeIdList { get; set; }
        public List<int?> ServiceTypeList { get; set; }
        public List<int> RatioCustomerIdList { get; set; }
        public bool IsBilledMandayExport { get; set; }
        public List<int> RatioEmployeeTypeIdList { get; set; }
    }

    public enum FinanceDashboardResult
    {
        Success = 1,
        Fail = 2,
        NotFound = 3,
        RequestNotCorrectFormat = 4
    }

    public class MandayRateItem
    {
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
        public double MonthManDay { get; set; }
        public double NotRoundedMonthManDay { get; set; }
        public double InspFees { get; set; }
        public int CurrencyId { get; set; }
        public double Rate { get; set; }
        public int Id { get; set; }
        public double BudgetManday { get; set; }
    }
    
    public class FinanceDashboardMandayData
    {
        public List<MandayYearChartItem> BilledManday { get; set; }
        public List<MandayYearChartItem> BilledMandayBudget { get; set; }
        public List<MandayRateItem> MandayRateBudget { get; set; }
        public List<MandayRateItem> MandayRate { get; set; }
    }

    public class FinanceDashboardMandayResponse
    {
        public FinanceDashboardMandayData Data { get; set; }
        public FinanceDashboardResult Result { get; set; }
    }

    public class MandaySPRequest
    {
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> @FactoryIdList { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> @FactoryCountryIdList { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> OfficeIdList { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> ServiceTypeIdList { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> BrandIdList { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> DepartmentIdList { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> @BuyerIdList { get; set; }
        public int? EntityId { get; set; }
    }

    public class MandayYearChartFinanceDashboardResponse
    {
        public IEnumerable<MandayYearChart> BilledMandayData { get; set; }
        public MandayYearChart BilledMandayBudget { get; set; }
        public IEnumerable<MandayYearRateChart> MandayRateData { get; set; }
        public MandayYearRateChart MandayRateBudget { get; set; }
        public IEnumerable<MandayYear> MonthYearXAxis { get; set; }
        public FinanceDashboardResult Result { get; set; }
    }

    public class MandayYearRateChart
    {
        public int Year { get; set; }
        public double MandayCount { get; set; }
        public string Color { get; set; }
        public IEnumerable<MandayRateItem> MonthlyData { get; set; }
    }

    public class FinanceDashboardBookingDataResponse
    {
        public List<int> Data { get; set; }
        public FinanceDashboardResult Result { get; set; }
    }

    public class FinanceTurnOverDbItem
    {
        public int BookingId { get; set; }
        public double TotalInvoiceFee { get; set; }
        public int InvoiceCurrencyId { get; set; }
        public double TotalExtraFee { get; set; }
        public int ExtraFeeCurrencyId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public int ProdCategoryId { get; set; }
        public string ProdCategoryName { get; set; }
        public double TravelTotalFees { get; set; }
        public bool PriceCardTravelIncluded { get; set; }
    }

    public class ExchangeCurrencyItem
    {
        public int Id { get; set; }
        public double Fee { get; set; }
        public int CurrencyId { get; set; }

        public int ExtraFeeId { get; set; }
        public double ExtraFee { get; set; }
        public int ExtraFeeCurrencyId { get; set; }

        public int Year { get; set; }
        public bool PriceCardTravelIncluded { get; set; }
        public int CustomerId { get; set; }
    }

    public class TurnOverSpRequest
    {
        [MapData(Type = "Udt_Int")]
        public List<CommonId> BookingIdList { get; set; }
        public int? EntityId { get; set; }
    }

    public class FinanceDashboardCommonItem
    {
        public string Name { get; set; }
        public double Count { get; set; }
        public string Color { get; set; }
    }

    public class FinanceDashboardTurnOverResponse
    {
        public List<FinanceDashboardCommonItem> CountryData { get; set; }
        public List<FinanceDashboardCommonItem> ProductCategoryData { get; set; }
        public List<FinanceDashboardCommonItem> ServieTypeData { get; set; }
        public FinanceDashboardResult Result { get; set; }
    }

    public class FinanceDashboardRequestExport
    {
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string FactoryList { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string DeptList { get; set; }
        public string BrandList { get; set; }
        public string BuyerList { get; set; }
        public string StatusList { get; set; }
        public string CountryList { get; set; }
        public string OfficeList { get; set; }
        public string ServiceTypeList { get; set; }
    }

    public class ChargeBackSpData
    {
        public List<ExchangeCurrencyItem> TotalExpenseAmount { get; set; }
        public List<FinanceTurnOverDbItem> InvoiceData { get; set; }
    }

    public class ChargeBackChartData
    {
        public double TotalExpense { get; set; }
        public double ChargeBack { get; set; }
        public double ChargeBackRatio { get; set; }
        public double TotalRevenue { get; set; }
        public double RevenueChargeBackRatio { get; set; }
    }

    public class ChargeBackChartResponse
    {
        public ChargeBackChartData Data { get; set; }
        public FinanceDashboardResult Result { get; set; }
    }

    public class QuotationCountData
    {
        public int QuotationCount { get; set; }
        public int RejectedQuotationCount { get; set; }
        public double RejectionPercentage { get; set; }
    }

    public class QuotationChartResponse
    {
        public QuotationCountData Data { get; set; }
        public FinanceDashboardResult Result { get; set; }
    }

    public class FinanceDashboardMandayExportItem
    {
        public List<MandayYearChartItem> Data { get; set; }
        public FinanceDashboardRequestExport RequestFilters { get; set; }
        public double Total { get; set; }
        public double BudgetMandayTotal { get; set; }
        public bool IsBilledManday { get; set; }
    }

    public class FinanceDashboardCommonExportItem
    {
        public List<FinanceDashboardCommonItem> Data { get; set; }
        public FinanceDashboardRequestExport RequestFilters { get; set; }
        public int Total { get; set; }
    }

    public class ExchangeCurrency
    {
        public int TargetCurrency { get; set; }
        public int Currency { get; set; }
    }
    public class FinanceDashboardRatioAnalysisResponse
    {
        public List<FinanceDashboardRatioAnalysis> Data { get; set; }
        public FinanceDashboardResult Result { get; set; }
    }
    public class FinanceDashboardRatioAnalysis
    {
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public double? BilledManday { get; set; }
        public double ProductionManday { get; set; }
        public double? Ratio { get; set; }
        public double Revenue { get; set; }
        public double? BilledAvgManday { get; set; }
        public double? ProductionAvgManday { get; set; }
        public double IsTotal { get; set; }
        public double ChargeBack { get; set; }
        public double TotalExpense { get; set; }
        public double NetIncome { get; set; }
        public double NetIncomeAvg { get; set; }
        public double ActualManday { get; set; }
    }
    public class FinanceDashboardExportRatioAnalysis
    {
        [Description("Customer")]
        public string Customer { get; set; }
        [Description("Bill MD")]
        public double? BilledManday { get; set; }
        [Description("Actual MD")]
        public double ActualManday { get; set; }
        [Description("Produciton MD")]
        public double ProductionManday { get; set; }
        [Description("Ratio")]
        public double? Ratio { get; set; }
        [Description("Revenue")]
        public double Revenue { get; set; }
        [Description("ChargeBack")]
        public double Chargeback { get; set; }
        [Description("Expenses")]
        public double Expense { get; set; }
        [Description("Net Income")]
        public double Income { get; set; }
        [Description("Bill A MD rate")]
        public double? BilledAvgManday { get; set; }
        [Description("NET MD Rate USD")]
        public double? NetIncomeMDRate { get; set; }
        [Description("Production A MD")]
        public double? ProductionAvgManday { get; set; }

    }

    public class FinanceDashboardBilledMandayRepo
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public double? BilledManday { get; set; }
       

    }
    public class FinanceDashboardInspectionFeesRepo
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public double? inspectionFees { get; set; }
        public double? discount { get; set; }
        public int? CurrencyId { get; set; }
        public double? ExtraFees { get; set; }
        public int? ExtraFeeCurrencyId { get; set; }
        public double? TravelAir { get; set; }
        public double? TravleLand { get; set; }
        public double? HotelFee { get; set; }
        public double? OtherFee { get; set; }
        public double TotalChargeBack { get; set; }
    }
    public class FinanceDashboardScheduleMandayRepo
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public double ActualManDay { get; set; }
        public int EmployeeTypeId { get; set; }
        public int QcId { get; set; }
        public DateTime ServiceDate { get; set; }
    }

    public class RatioAnalysisTableData
    {
        public List<FinanceDashboardBilledMandayRepo> BilledManday { get; set; }
        public List<FinanceDashboardInspectionFeesRepo> InspectionFeesList { get; set; }
        public List<FinanceDashboardScheduleMandayRepo> ScheduleManday { get; set; }
        public List<ExchangeCurrencyItem> ExpenseData { get; set; }
        public List<ProductionManDay> ProductionManDayList { get; set; }
    }

    public class ProductionManDay
    {
        public int QcId { get; set; }
        public DateTime ServiceDate { get; set; }
        public int CustomerId { get; set; }
        public int BookingId { get; set; }
        public double ProductionManday { get; set; }
    }
}
