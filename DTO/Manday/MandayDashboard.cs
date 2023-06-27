using DTO.Common;
using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Manday
{
    public class MandayDashboardRequest
    {
        public DateObject ServiceDateFrom { get; set; }
        public DateObject ServiceDateTo { get; set; }
        public DateObject ComparedServiceDateFrom { get; set; }
        public DateObject ComparedServiceDateTo { get; set; }
        public List<int> CountryIdList { get; set; }
        public List<int> CustomerIdList { get; set; }
        public int ServiceId { get; set; }
        public List<int?> OfficeIdList { get; set; }
        public int MandayEmployeeTypeSubYear { get; set; }
        public int? MandayYearSubCustomerId { get; set; }
        public int? MandayYearSubCountryId { get; set; }
        public int? MandayCustomerSubCountryId { get; set; }
        public int? MandayCountrySubCountryId { get; set; }
        public int? MandayCountrySubProvinceId { get; set; }
        public int? MandayEmployeeTypeSubCustomerId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> StatusIdList { get; set; }
        public MandayDashboardChartType ChartType { get; set; }
        public int MandayType { get; set; }
        public bool IsCompareData { get; set; }
    }

    public enum MandayType
    {
        EstimatedManday = 1,
        ActualManday
    }

    public enum MandayDashboardChartType
    {
        MandayByYear = 1,
        MandayByEmployeeType = 2,
        Other = 3
    }
    public class MandayDashboardResponse
    {
        public MandayDashboardItem Data { get; set; }
        public MandayDashboardResult Result { get; set; }
    }

    public class MandayDashboardItem
    {
        public double TotalManday { get; set; }
        public int TotalCount { get; set; }
        public int TotalReportCount { get; set; }

        public double ComparedTotalManday { get; set; }
        public int ComparedTotalCount { get; set; }
        public int ComparedTotalReportCount { get; set; }
    }

    public enum MandayDashboardResult
    {
        Success = 1,
        Fail = 2,
        NotFound = 3
    }

    public class MandayYearChart
    {
        public int Year { get; set; }
        public double MandayCount { get; set; }
        public string Color { get; set; }
        public IEnumerable<MandayYearChartItem> MonthlyData { get; set; }
    }

    public class MandayYearChartItem
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public double MonthManDay { get; set; }
        public double MonthActualManDay { get; set; }
        public string MonthName { get; set; }
        public double? BudgetManday { get; set; }
    }

    public class MandayYearChartResponse
    {
        public IEnumerable<MandayYearChart> Data { get; set; }
        public IEnumerable<MandayYear> MonthYearXAxis { get; set; }
        public MandayDashboardResult Result { get; set; }
    }

    public class MandayYear
    {
        public int year { get; set; }
        public int month { get; set; }
        public string MonthName { get; set; }
        public string Month_Year { get; set; }
    }

    public class MandayCountryChart
    {
        public string Name { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        public double MandayCount { get; set; }
        public string Color { get; set; }
        public double? ComparedMandayCount { get; set; }
        public double? ComparedPercentage { get; set; }
    }

    public class MandayCountryChartResponse
    {
        public IEnumerable<MandayCountryChart> Data { get; set; }
        public double TotalCount { get; set; }
        public MandayDashboardResult Result { get; set; }
    }

    public class MandayCustomerChart
    {
        public int InspectionId { get; set; }
        public string CustomerName { get; set; }
        public double MandayCount { get; set; }
        public double Percentage { get; set; }
        public string Color { get; set; }
        public int? InspectionCount { get; set; }
        public int? ReportCount { get; set; }
        public double? InspectedQty { get; set; }
        public double? OrderQty { get; set; }
        public double? PresentedQty { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        public double? ComparedPercentage { get; set; }
        public double? ComparedMandayCount { get; set; }
        public int? ComparedInspectionCount { get; set; }
        public int? ComparedReportCount { get; set; }
        public double? ComparedInspectedQty { get; set; }
        public double? ComparedOrderQty { get; set; }
        public double? ComparedPresentedQty { get; set; }
    }

    public class MandayCustomerChartData
    {
        public string CustomerName { get; set; }
        public int InspectionCount { get; set; }
        public int ReportCount { get; set; }
        public double? InspectedQty { get; set; }
        public double? OrderQty { get; set; }
        public double? PresentedQty { get; set; }
        public DateTime? ServiceDateTo { get; set; }
    }

    public class MandayCustomerChartResponse
    {
        public IEnumerable<MandayCustomerChart> Data { get; set; }
        public double Total { get; set; }
        public MandayDashboardResult Result { get; set; }
    }

    public class EmployeeTypes
    {
        public string EmployeeType { get; set; }
        public double MandayCount { get; set; }
        public double QuotationManday { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public double MandayPercentage { get; set; }
        public DateTime? ServiceDateTo { get; set; }
    }

    public class EmployeeTypeYear
    {
        public string EmployeeType { get; set; }
        public double MandayCount { get; set; }
        public string Color { get; set; }
        public IEnumerable<EmployeeTypes> MonthlyData { get; set; }
        public double? ComparedMandayCount { get; set; }
    }

    public class MandayEmployeeTypeChartResponse
    {
        public IEnumerable<EmployeeTypeYear> Data { get; set; }
        public IEnumerable<MandayYear> MonthYearXAxis { get; set; }
        public MandayDashboardResult Result { get; set; }
    }

    public class AuditResponseManday
    {
        public int AuditId { get; set; }
        public int? OfficeId { get; set; }
        public int StatusId { get; set; }
        public int CustomerId { get; set; }
        public int FactoryId { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
    }

    public class MandayCountryChartExport
    {
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public double MandayCount { get; set; }
        public double? ComparedMandayCount { get; set; }
        public DateTime? ServiceDateTo { get; set; }
    }

    public class MandayEmployeeTypeChartExportItem
    {
        public string MonthName { get; set; }
        public double MonthCount { get; set; }
        public List<EmployeeTypes> Data { get; set; }
    }

    public class MandayEmployeeTypeChartExport
    {
        public IEnumerable<MandayEmployeeTypeChartExportItem> Data { get; set; }
        public double Total { get; set; }
        public List<string> EmployeeTypeNames { get; set; }
        public MandayDashboardRequestExport RequestFilters { get; set; }
    }

    public class MandayYearExport
    {
        public IEnumerable<MandayYearChartItem> Data { get; set; }
        public double Total { get; set; }
        public MandayDashboardRequestExport RequestFilters { get; set; }
    }

    public class MandayCountryChartExportResponse
    {
        public IEnumerable<MandayCountryChartExport> Data { get; set; }
        public MandayDashboardRequestExport RequestFilters { get; set; }
        public double Total { get; set; }
        public bool IsCompareData { get; set; }
    }

    public class MandayDashboardRequestExport
    {
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string ComparedServiceDateFrom { get; set; }
        public string ComparedServiceDateTo { get; set; }
        public string CountryIdList { get; set; }
        public string Service { get; set; }
        public string OfficeIdList { get; set; }
        public string CustomerList { get; set; }
        public string MandayYearSubCustomer { get; set; }
        public string MandayYearSubCountry { get; set; }
        public string MandayCustomerSubCountry { get; set; }
        public string MandayCountrySubCountry { get; set; }
        public string MandayCountrySubProvince { get; set; }
        public string MandayEmployeeTypeSubCustomer { get; set; }
        public int MandayEmployeeTypeSubYear { get; set; }
        public int MandayType { get; set; }
    }

    public class MandayCustomerChartExportResponse
    {
        public IEnumerable<MandayCustomerChart> Data { get; set; }
        public int ServiceId { get; set; }
        public bool IsCompareData { get; set; }
        public double Total { get; set; }
        public MandayDashboardRequestExport RequestFilters { get; set; }
        public MandayDashboardResult Result { get; set; }
    }

    public class MandayListCurrenctPrevious
    {
        public List<MandayYearChartItem> CurrentMandayList { get; set; }
        public List<MandayYearChartItem> PreviousMandayList { get; set; }
    }
}
