using DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.DefectDashboard
{
    public class DefectDashboard
    {

    }

    public class DefectDashboardRequest 
    {
        [RequiredGreaterThanZero(ErrorMessage = "EDIT_BOOKING.MSG_CUS_REQ")]
        public int CustomerId { get; set; }

        public int? SupplierId { get; set; }
        public IEnumerable<int?> FactoryIds { get; set; }
        public IEnumerable<int?> FactoryCountryIds { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        public DateObject FromDate { get; set; }

        [DateGreaterThanAttribute(otherPropertyName = "FromDate", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        public DateObject ToDate { get; set; }

        public int? InnerDefectYearId { get; set; }
        public IEnumerable<int?> SelectedDeptIdList { get; set; }
        public IEnumerable<int?> SelectedBrandIdList { get; set; }
        public IEnumerable<int?> SelectedBuyerIdList { get; set; }
        public IEnumerable<int?> SelectedCollectionIdList { get; set; }
    }

    public class DefectDashboardFilterRequest
    {
        public int? CustomerId { get; set; }

        public int? SupplierId { get; set; }
        public IEnumerable<int> FactoryIds { get; set; }
        public IEnumerable<int> FactoryCountryIds { get; set; }

        public DateObject FromDate { get; set; }

        public DateObject ToDate { get; set; }

        public int? InnerDefectYearId { get; set; }
        public IEnumerable<int> SelectedDeptIdList { get; set; }
        public IEnumerable<int> SelectedBrandIdList { get; set; }
        public IEnumerable<int> SelectedBuyerIdList { get; set; }
        public IEnumerable<int?> SelectedCollectionIdList { get; set; }
        public IEnumerable<int> SelectedProdCategoryIdList { get; set; }
        public IEnumerable<int> SelectedProductIdList { get; set; }
        public IEnumerable<int> ServiceTypelst { get; set; }
        public IEnumerable<GroupByFilter> GroupByFilter { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public int SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
    }

    public class BookingReportModel
    {
        public int BookingId { get; set; }
        public int? ReportId { get; set; }
        public int? FactoryId
        {
            get; set;
        }      
    }
    public class BookingReportResponse
    {
        public List<BookingReportModel> BookingReportModel { get; set; }
        public List<DefectYear> MonthXAxis { get; set; }
        public DefectDashboardResult Result { get; set; }
    }

    public class DefectCategoryResponse
    {
        public List<DefectCategoryModel> DefectCategoryList { get; set; }
        public DefectDashboardResult Result { get; set; }
    }

    public class DefectCategoryModel
    {
        public string CategoryName { get; set; }
        public string Color { get; set; }
        public int DefectCountByCategory { get; set; }
    }

    public class DefectCategoryExport
    {
        public string CategoryName { get; set; }
        public int DefectCountByCategory { get; set; }
    }
    public class DefectCategoryExportResponse
    {
        public List<DefectCategoryExport> DefectCategoryList { get; set; }
        public DefectDashboardFilterExport RequestFilters { get; set; }
        public DefectDashboardResult Result { get; set; }
    }

    public enum DefectDashboardResult
    {
        Success = 1,
        NotFound = 2,
        Error = 3,
        RequestNotCorrectFormat = 4
    }

    public class DefectDashboardFilterExport
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string FactoryCountryName { get; set; }        
        public int DefectCountYear { get; set; }
        public string Product { get; set; }
        public string ProductCategory { get; set; }
    }

    public class DefectSubSupOrFactFilterExport
    {
        public string SubDefectName { get; set; }
        public string SubSupOrFact { get; set; }
        public string DefectSelectName { get; set; }
    }

    public class GroupByRequestFilter
    {
        public bool FactoryCountry { get; set; }
        public bool Supplier { get; set; }
        public bool Factory { get; set; }
        public bool Brand { get; set; }
    }

    public enum GroupByFilter
    {
        FactoryCountry = 1,
        Supplier = 2,
        Factory = 3,
        Brand = 4
    }
}