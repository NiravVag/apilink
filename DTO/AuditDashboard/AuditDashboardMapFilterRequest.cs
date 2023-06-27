using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Common;

namespace DTO.AuditDashboard
{
    public class AuditDashboardMapFilterRequest
    {
        public int SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public IEnumerable<int> FactoryIdlst { get; set; }
        public IEnumerable<int> StatusIdlst { get; set; }
        public int DateTypeid { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public IEnumerable<int?> Officeidlst { get; set; }
        public IEnumerable<int> FactoryCountryIdList { get; set; }
        public IEnumerable<int> AuditorIdList { get; set; }
        public IEnumerable<int> ServiceTypeIdList { get; set; }
    }

    public class AuditMapGeoLocation
    {
        public List<AuditCountryGeoCode> CountryGeoCode { get; set; }
        public MapGeoLocationResult CountryGeoCodeResult { get; set; }
        public List<AuditProvinceGeoCode> ProvinceGeoCode { get; set; }
        public MapGeoLocationResult ProvinceGeoCodeResult { get; set; }
        public List<AuditFactoryGeoCode> FactoryGeoCode { get; set; }
    }

    public class AuditCountryGeoCode
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string FactoryCountryName { get; set; }
        public string FactoryCountryCode { get; set; }
        public int FactoryCountryId { get; set; }
        public int TotalCount { get; set; }
        public string FactoryProvinceName { get; set; }
        public int FactoryProvinceId { get; set; }
        public decimal? ProvinceLongitude { get; set; }
        public decimal? ProvinceLatitude { get; set; }
        public string FactoryName { get; set; }
        public int FactoryId { get; set; }
        public decimal? FactoryLongitude { get; set; }
        public decimal? FactoryLatitude { get; set; }
    }

    public class AuditProvinceGeoCode
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string FactoryProvinceName { get; set; }
        public int FactoryProvinceId { get; set; }
        public int TotalCount { get; set; }
    }

    public class AuditFactoryGeoCode
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string FactoryName { get; set; }
        public int FactoryId { get; set; }
        public int TotalCount { get; set; }
    }

    public class AuditDashboardResponse
    {
        public AuditDashboardItem Data { get; set; }
        public AuditDashboardResult Result { get; set; }
    }

    public class AuditDashboardItem
    {
        public int TotalAuditInProgressCount { get; set; }
        public int TotalAuditPlanningCount { get; set; }
        public int TotalAuditedCount { get; set; }
        public int TotalFactoryCount { get; set; }
    }

    public class ResultAnalyticsAuditDashboardResponse
    {
        public List<CustomerAPIRAAuditDashboard> Data { get; set; }
        public AuditDashboardResult Result { get; set; }
    }

    public class CustomerAPIRAAuditDashboard
    {
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int TotalCount { get; set; }
    }

    public class AuditChartData 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class OverviewAuditDashboardResponse
    {
        public OverviewAuditChart Data { get; set; }
        public AuditDashboardResult Result { get; set; }
    }

    public class AuditDashboardChartExport
    {
        public List<AuditDashboardExportItem> Data { get; set; }
        public double Total { get; set; }
        public AuditDashboardRequestExport RequestFilters { get; set; }
        public AuditDashboardResult Result { get; set; }
    }

    public class AuditDashboardExportItem
    {
        public int Count { get; set; }
        public string Name { get; set; }
    }

    public class AuditDashboardRequestExport
    {
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string CountryList { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string FactoryList { get; set; }
    }

    public class OverviewAuditChart
    {
        public int TotalReports { get; set; }
        public int TotalBookingCount { get; set; }
        public int TotalCustomerCount { get; set; }
    }
    public enum MapGeoLocationResult
    {
        Success = 1,
        Failure = 2
    }

    public enum AuditDashboardResult
    {
        Success = 1,
        Fail = 2,
        NotFound = 3,
        RequestError = 4
    }
}
