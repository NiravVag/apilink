using DTO.Common;
using DTO.CommonClass;
using DTO.Dashboard;
using DTO.Manday;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.RejectionDashboard
{
    public class RejectionDashboardFilterRequest
    {
        [RequiredGreaterThanZero(ErrorMessage = "EDIT_BOOKING.MSG_CUS_REQ")]
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public IEnumerable<int?> SelectedCountryIdList { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        public DateObject ServiceDateFrom { get; set; }

        [DateGreaterThanAttribute(otherPropertyName = "ServiceDateFrom", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        public DateObject ServiceDateTo { get; set; }
        public IEnumerable<int?> SelectedDeptIdList { get; set; }
        public IEnumerable<int?> SelectedBrandIdList { get; set; }
        public IEnumerable<int?> SelectedBuyerIdList { get; set; }
        public IEnumerable<int?> SelectedCollectionIdList { get; set; }
        public IEnumerable<int> StatusIdList { get; set; }
        public int? CountryId { get; set; }
        public int? popUpSelectedPhotoSupplierId { get; set; }
        public int? popUpSelectedPhotoFactoryId { get; set; }
        //to fetch the reject dashboard pop up data
        public int Month { get; set; }
        public int Year { get; set; }
        public string RejectReason { get; set; }
        public int FbResultId { get; set; }
        public string SearchBy { get; set; }
        public List<string> SummaryNames { get; set; }
        public List<string> SubcatogoryList { get; set; }
    }
    public class RejectionDashboardSearchRequest
    {
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public IEnumerable<int> SelectedCountryIdList { get; set; }
        public DateObject ServiceDateFrom { get; set; }
        public DateObject ServiceDateTo { get; set; }
        public IEnumerable<int> SelectedDeptIdList { get; set; }
        public IEnumerable<int> SelectedBrandIdList { get; set; }
        public IEnumerable<int> SelectedBuyerIdList { get; set; }
        public IEnumerable<int> SelectedProdCategoryIdList { get; set; }
        public IEnumerable<int> SelectedProductIdList { get; set; }
        public IEnumerable<int?> SelectedCollectionIdList { get; set; }
        public IEnumerable<int> StatusIdList { get; set; }
        public int? CountryId { get; set; }
        public int? popUpSelectedPhotoSupplierId { get; set; }
        public int? popUpSelectedPhotoFactoryId { get; set; }
        //to fetch the reject dashboard pop up data
        public int Month { get; set; }
        public int Year { get; set; }
        public string RejectReason { get; set; }
        public int FbResultId { get; set; }
        public string SearchBy { get; set; }
        public List<string> SummaryNames { get; set; }
        public List<string> SubcatogoryList { get; set; }
        public IEnumerable<int> SelectedFactoryIdList { get; set; }
        public IEnumerable<int> SelectedSupplierIdList { get;set; }
        public IEnumerable<int> SelectedServiceTypeIdList { get; set; }
        public IEnumerable<GroupByFilter> GroupByFilter { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public bool IsExport { get; set; }
        public int SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
    }
    public enum GroupByFilter
    {
        FactoryCountry = 1,
        Supplier = 2,
        Factory = 3,
        Brand = 4
    }

    public enum RejectionDashboardResult
    {
        Success = 1,
        Fail = 2,
        NotFound = 3
    }

    public class ProductCategoryChartResponse
    {
        public IEnumerable<ResultData> Data { get; set; }
        public IEnumerable<CommonDataSource> ProductCategoryList { get; set; }
        public List<CommonDataSource> LegendList { get; set; }
        public double TotalReports { get; set; }
        public RejectionDashboardResult Result { get; set; }
    }



    public class ResultData
    {
        public string ResultName { get; set; }
        public double Count { get; set; }
        public string Color { get; set; }
        public IEnumerable<ChartItem> Data { get; set; }
    }

    public class ChartItem
    {
        public int? ResultId { get; set; }
        public string ResultName { get; set; }
        public int TotalCount { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public int BookingCount { get; set; }
        public int NewId { get; set; }
        public double Percentage { get; set; }
        public int GrandTotal { get; set; }
    }

    public class RejectResultDashboardResponse
    {
        public List<CustomerAPIRADashboard> Data { get; set; }
        public List<int> BookingIdList { get; set; }
        public int TotalReports { get; set; }
        public RejectionDashboardResult Result { get; set; }
    }

    public class RejectionCommonExport
    {
        public IEnumerable<RejectionDashboardCommonItem> Data { get; set; }
        public int Total { get; set; }
        public RejectionDashboardRequestExport RequestFilters { get; set; }
    }

    public class RejectionDashboardCommonItem
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class RejectionDashboardRequestExport
    {
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }

        public string Brand { get; set; }
        public string Department { get; set; }
        public string Buyer { get; set; }
        public string Collection { get; set; }
        public string FactoryCountry { get; set; }
        public string Product { get; set; }
        public string ProductCategory { get; set; }
    }

    public class ChartExport
    {
        public IEnumerable<ChartExportItem> Data { get; set; }
        public double Total { get; set; }
        public List<string> ResultNames { get; set; }
        public RejectionDashboardRequestExport RequestFilters { get; set; }
    }

    public class ChartExportItem
    {
        public string Name { get; set; }
        public double Count { get; set; }
        public List<ChartItem> Data { get; set; }
    }

    public class VendorChartResponse
    {
        public IEnumerable<ResultData> Data { get; set; }
        public IEnumerable<CommonDataSource> SupplierList { get; set; }
        public double TotalReports { get; set; }
        public RejectionDashboardResult Result { get; set; }
    }

    public class CountryChartItem
    {
        public int? ResultId { get; set; }
        public string ResultName { get; set; }
        public int TotalCount { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public int? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
    }

    public class CountryChartRequest
    {
        public RejectionDashboardSearchRequest SearchRequest { get; set; }
        public int? CountryId { get; set; }
        public bool ClearSelection { get; set; }
    }
    public class RejectChartRequest
    {
        public RejectionDashboardSearchRequest SearchRequest { get; set; }
        public int FbResultId { get; set; }
    }
    public class RejectChartSubcatogoryRequest
    {
        public RejectionDashboardSearchRequest SearchRequest { get; set; }
        public int FbResultId { get; set; }
        public List<string> ResultNames { get; set; }
        public List<int> ResultId { get; set; }
    }
    public class RejectChartSubcatogory2Request
    {
        public RejectionDashboardSearchRequest SearchRequest { get; set; }
        public int FbResultId { get; set; }
        public List<string> ResultNames { get; set; }
        public List<string> SubCatogory { get; set; }
    }

    public class CountryChartResponse
    {
        public IEnumerable<ResultData> Data { get; set; }
        public IEnumerable<CommonDataSource> YAxisData { get; set; }
        public int? SelectedCountryId { get; set; }
        public List<CommonDataSource> CountryList { get; set; }
        public double TotalReports { get; set; }
        public RejectionDashboardResult Result { get; set; }
    }

    public class CountryChartExportItem
    {
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public double Count { get; set; }
        public List<CountryChartItem> Data { get; set; }
    }

    public class CountryChartExport
    {
        public IEnumerable<CountryChartExportItem> Data { get; set; }
        public double Total { get; set; }
        public List<string> ResultNames { get; set; }
        public RejectionDashboardRequestExport RequestFilters { get; set; }
    }

    public class RejectChartMonthItem
    {
        public string Name { get; set; }
        public string ReasonName { get; set; }
        public string Subcatogory { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
        public int MonthCount { get; set; }
        public int TotalCount { get; set; }
        public int FbReportDetailId { get; set; }
        public int Month { get; set; }
    }

    public class RejectChartYearData
    {
        public string Name { get; set; }
        public string ReasonName { get; set; }
        public string Subcatogory { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
        public List<RejectChartMonthItem> MonthlyData { get; set; }
    }

    public class RejectChartResponse
    {
        public List<RejectChartYearData> Data { get; set; }
        public List<string> RejectReasonList { get; set; }
        public List<MandayYear> MonthNameList { get; set; }
        public RejectionDashboardResult Result { get; set; }
    }

    public class RejectionPopUpResponse
    {
        public RejectionPopUpData Data { get; set; }
        public List<CommonDataSource> SupplierList { get; set; }
        public List<CommonDataSource> FactoryList { get; set; }
        public RejectionDashboardResult Result { get; set; }
    }

    public class RejectionPopUpData
    {
        public int ReportCount { get; set; }
        public int InspectionCount { get; set; }
        public int FactoryCount { get; set; }
        public List<RejectionFactoryData> SupplierData { get; set; }
    }

    public class RejectionSupplierData
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<RejectionFactoryData> FactoryData { get; set; }
    }

    public class RejectionFactoryData
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? FactoryId { get; set; }
        public string FactoryName { get; set; }
        public int RejectionCount { get; set; }
        public int ReportCount { get; set; }
        public int BookingCount { get; set; }
        public int PhotoCount { get; set; }
        public string ReasonName { get; set; }
        public int Month { get; set; }
        public List<RejectionReportData> ReportInfo { get; set; }
    }

    public class RejectionReportData
    {
        public string ReportNo { get; set; }
        public string ReportLink { get; set; }
        public string FinalManualReportLink { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
    }

    public class RejectionImageResponse
    {
        public List<RejectionImageResult> Data { get; set; }
        public RejectionDashboardResult Result { get; set; }
    }

    public class RejectionImageResult
    {
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }

    public class ExportRejectionTableData
    {
        [Description("Name")]
        public string ReasonName { get; set; }
        [Description("Sub Category")]
        public string SubCategory { get; set; }
        [Description("Product Name")]
        public string SubCategory2 { get; set; }
        [Description("Year")]
        public int Year { get; set; }
        [Description("Month")]
        public string Month { get; set; }
        [Description("Count")]
        public int RejectionCount { get; set; }
    }

    public class ExportRejectionTableRepoData
    {
        public string ReasonName { get; set; }
        public string SubCategory { get; set; }
        public string SubCategory2 { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int RejectionCount { get; set; }
        public int ReportId { get; set; }
    }
}
