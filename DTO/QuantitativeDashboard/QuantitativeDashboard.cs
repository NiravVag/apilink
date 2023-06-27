using DTO.Common;
using DTO.Manday;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.QuantitativeDashboard
{
    public class QuantitativeDashboardRequest
    {
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public DateObject ServiceDateFrom { get; set; }
        public DateObject ServiceDateTo { get; set; }
        public IEnumerable<int?> SelectedCountryIdList { get; set; }
        public IEnumerable<int?> SelectedDeptIdList { get; set; }
        public IEnumerable<int?> SelectedBrandIdList { get; set; }
        public IEnumerable<int?> SelectedBuyerIdList { get; set; }
        public IEnumerable<int?> SelectedCollectionIdList { get; set; }
        public int? ProductCategoryId { get; set; }
    }

    public class QuantitativeDashboardFilterRequest
    {
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public DateObject ServiceDateFrom { get; set; }
        public DateObject ServiceDateTo { get; set; }
        public IEnumerable<int> SelectedCountryIdList { get; set; }
        public IEnumerable<int> SelectedDeptIdList { get; set; }
        public IEnumerable<int> SelectedBrandIdList { get; set; }
        public IEnumerable<int> SelectedBuyerIdList { get; set; }
        public IEnumerable<int?> SelectedCollectionIdList { get; set; }
        public IEnumerable<int> SelectedProdCategoryIdList { get; set; }
        public IEnumerable<int> SelectedProductIdList { get; set; }
        public int? ProductCategoryId { get; set; }
    }

    public class QuantitativeDashboardResponse
    {
        public QuantitativeDashboardItem Data { get; set; }
        public List<int> InspectionIdList { get; set; }
        public QuantitativeDashboardResult Result { get; set; }
    }

    public class QuantitativeDashboardItem
    {
        public double TotalManday { get; set; }
        public int TotalInspCount { get; set; }
        public int TotalReportCount { get; set; }
        public int TotalFactoryCount { get; set; }
        public int TotalQcCount { get; set; }
        public int TotalVendorCount { get; set; }
    }

    public enum QuantitativeDashboardResult
    {
        Success = 1,
        NotFound = 2,
        Fail = 3,
        RequestNotCorrectFormat = 4
    }

    public class QuantitativeMandayYearChart
    {
        public int Year { get; set; }
        public double MandayCount { get; set; }
        public string Color { get; set; }
        public double Percentage { get; set; }
        public IEnumerable<MandayYearChartItem> MonthlyData { get; set; }
    }

    public class QuantitativeMandayYearChartResponse
    {
        public IEnumerable<QuantitativeMandayYearChart> Data { get; set; }
        public IEnumerable<MandayYear> MonthYearXAxis { get; set; }
        public QuantitativeDashboardResult Result { get; set; }
    }

    public class QuantitativeDashboardRequestExport
    {
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string Brand { get; set; }
        public string Department { get; set; }
        public string Buyer { get; set; }
        public string Collection { get; set; }
        public string Product { get; set; }
        public string ProductCategory { get; set; }
        public string FactoryCountry { get; set; }
    }

    public class QuantitativeMandayYearExport
    {
        public IEnumerable<MandayYearChartItem> Data { get; set; }
        public double Total { get; set; }
        public QuantitativeDashboardRequestExport RequestFilters { get; set; }
    }

    public class QuantitativeDashboardCommonItem
    {
        public string Name { get; set; }
        public double Count { get; set; }
        public string Color { get; set; }
        public double Percentage { get; set; }
        public double LastYearCount { get; set; }
    }

    public class QuantitativeDashboardCommonResponse
    {
        public List<QuantitativeDashboardCommonItem> Data { get; set; }
        public QuantitativeDashboardResult Result { get; set; }
    }

    public class MandayCountry
    {
        public double? Manday { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int Year { get; set; }
    }

    public class QuantitativeCommonExport
    {
        public IEnumerable<QuantitativeDashboardCommonItem> Data { get; set; }
        public double Total { get; set; }
        public QuantitativeDashboardRequestExport RequestFilters { get; set; }
    }

    public class TurnOverDataItem
    {
        public double TotalTurnOver { get; set; }
        public double CustomerTurnOver { get; set; }
        public double SupplierTurnOver { get; set; }
        public double TotalTurnOverPercentage { get; set; }
        public double CustomerTurnOverPercentage { get; set; }
        public double SupplierTurnOverPercentage { get; set; }
    }

    public class TurnOverData
    {
        public TurnOverDataItem TurnOverDataItem { get; set; }
        public List<QuantitativeDashboardCommonItem> ServiceTypeChartData { get; set; }
    }

    public class TurnOverDataResponse
    {
        public TurnOverData Data { get; set; }
        public QuantitativeDashboardResult Result { get; set; }
    }

    public class TurnOverItem
    {
        public int? InvoiceTo { get; set; }
        public double? TotalInvoiceFee { get; set; }
        public double? ExtraFee { get; set; }
        public int ServicetypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public int? CurrencyId { get; set; }
    }

    
    public class MandayCountryYearData
    {
        public List<MandayCountry> CurrentYearList { get; set; }
        public List<MandayCountry> LastYearList { get; set; }
    }

    public class DateList
    {
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
    }

    public class ProductCategoryChartRequest
    {
        public QuantitativeDashboardFilterRequest SearchRequest { get; set; }
        public int? ProductCategoryId { get; set; }
    }   
}
