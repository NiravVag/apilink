using DTO.Common;
using DTO.Dashboard;
using DTO.Manday;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ManagementDashboard
{
    public class ManagementDashboardRequest
    {
        public DateObject ServiceDateFrom { get; set; }
        public DateObject ServiceDateTo { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> FactoryIdList { get; set; }
        public List<int> StatusIdList { get; set; }
        public List<int> CountryIdList { get; set; }
        public List<int?> OfficeIdList { get; set; }
        public int MandayChartType { get; set; }
    }

    public class ManagementDashboardResponse
    {
        public ManagementDashboardItem Data { get; set; }
        public List<int> InspectionIdList { get; set; }
        public List<int> OfficeIds { get; set; }
        public ManagementDashboardResult Result { get; set; }
    }

    public class ManagementDashboardItem
    {
        public double TotalManday { get; set; }
        public int TotalInspCount { get; set; }
        public int TotalProductCount { get; set; }
        public int TotalFactoryCount { get; set; }
        public double TotalMandayLastYear { get; set; }
        public int TotalInspCountLastYear { get; set; }
        public int TotalProductCountLastYear { get; set; }
        public int TotalFactoryCountLastYear { get; set; }
        public double FactoryDifferencePercentage { get; set; }
        public double InspectionDifferencePercentage { get; set; }
        public double ProductDifferencePercentage { get; set; }
        public double MandayDifferencePercentage { get; set; }
        public int TotalCustomerCount { get; set; }
        public int TotalCustomerCountLastYear { get; set; }
        public double CustomerDifferencePercentage { get; set; }
        public int TotalReportCount { get; set; }
        public int TotalReportCountLastYear { get; set; }
        public double ReportDifferencePercentage { get; set; }
    }

    public enum ManagementDashboardResult
    {
        Success = 1,
        Fail = 2,
        NotFound = 3
    }

    public class InspectionRejectDashboardResponse
    {
        public List<InspectionRejectDashboard> Data { get; set; }
        public ManagementDashboardResult Result { get; set; }
    }

    public class ResultAnalyticsDashboardResponse
    {
        public List<CustomerAPIRADashboard> Data { get; set; }
        public ManagementDashboardResult Result { get; set; }
    }

    public class OverviewChart
    {
        public int TotalReports { get; set; }
        public int TotalBookingCount { get; set; }
        public int TotalCustomerCount { get; set; }
        public int ClaimRate { get; set; }
        public int QuotationRejectedByCustomerCount { get; set; }
    }

    public class OverviewDashboardResponse
    {
        public OverviewChart Data { get; set; }
        public ManagementDashboardResult Result { get; set; }
    }

    public class ProductCategoryDashboardItem
    {
        public int? Id { get; set; }
        public string StatusName { get; set; }
        public string Name { get; set; }
        public int TotalCount { get; set; }
        public string StatusColor { get; set; }
    }

    public class ProductCategoryDashboardResponse
    {
        public List<ProductCategoryDashboardItem> Data { get; set; }
        public ManagementDashboardResult Result { get; set; }
    }

    public class MandayYearChartManagementDashboardResponse
    {
        public IEnumerable<MandayYearChart> Data { get; set; }
        public MandayYearChart Budget { get; set; }
        public IEnumerable<MandayYear> MonthYearXAxis { get; set; }
        public MandayDashboardResult Result { get; set; }
    }

    public class BudgetManday
    {
        public double MandayCount { get; set; }
        public string Color { get; set; }
    }


    public class ResultDashboardResponse
    {
        public List<CustomerAPIRADashboard> Data { get; set; }
        public ManagementDashboardResult Result { get; set; }
    }

    public class AverageBookingTimeResponse
    {
        public AverageBookingTimeItem Data { get; set; }
        public ManagementDashboardResult Result { get; set; }
    }

    public class AverageBookingTimeItem
    {
        public double RequestToVerified { get; set; }
        public double VerifiedToConfirmed { get; set; }
        public double ConfirmedToScheduled { get; set; }
        public double DateRevisions { get; set; }
    }

    public class QuotationStatusLogData
    {
        public int BookingId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime BookingCreatedDate { get; set; }
        public int StatusId { get; set; }
    }

    public class BookingCreatedData
    {
        public int BookingId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class AverageQuotationTimeResponse
    {
        public AverageQuotationTimeItem Data { get; set; }
        public ManagementDashboardResult Result { get; set; }
    }

    public class AverageQuotationTimeItem
    {
        public double RequestToVerified { get; set; }
        public double VerifiedToSentToClient { get; set; }
        public double SentToClientToValidated { get; set; }
        public int TotalDays { get; set; }
    }
    public class BookingByStatusChangeDate
    {
        public int BookingId { get; set; }
        public DateTime? StatusChangeDate { get; set; }
    }

    public class ManagementDashboardRequestExport
    {
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string CountryList { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string FactoryList { get; set; }
    }

    public class ManagementDashboardChartExport
    {
        public List<ManagementDashboardExportItem> Data { get; set; }
        public double Total { get; set; }
        public ManagementDashboardRequestExport RequestFilters { get; set; }
    }

    public class ManagementDashboardExportItem
    {
        public int Count { get; set; }
        public string Name { get; set; }
    }

    public class ServiceTypeChartData
    {
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public int Count { get; set; }
    }
}
