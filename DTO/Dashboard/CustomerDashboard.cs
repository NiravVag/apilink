using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dashboard
{

    public class CustomerBusinessOVDashboard
    {
        public int BookingCount { get; set; }
        public int ProductsCount { get; set; }
        public int FactoryCount { get; set; }
        public double ManDays { get; set; }
        public int ProductPercentage { get; set; }
        public int FactoryPercentage { get; set; }
        public double ManDayPercentage { get; set; }
    }

    public class CustomerAPIRADashboard
    {
        public int? Id { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int TotalCount { get; set; }
    }

    public class ProductCategoryDashboard
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int TotalCount { get; set; }
        public string ImagePath { get; set; }
    }

    public class CustomerResultDashboard
    {
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int TotalCount { get; set; }
    }

    public class InspectionRejectDashboard
    {
        public int Rank { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int TotalCount { get; set; }
    }

    public class SupplierPerformanceDashboard
    {
        public int BookingLeadTime { get; set; }
        public int ETDLeadTime { get; set; }
        public int BookingRevisons { get; set; }
    }

    public class ManDayData
    {
        public string Name { get; set; }
        public double TotalManDays { get; set; }
    }

    public class ManDayDashboard
    {
        public List<ManDayData> CurrentYearData { get; set; }
        public List<ManDayData> LastYearData { get; set; }
    }

    public class InspectionManDayOverview
    {
        public int TotalInspections { get; set; }
        public double TotalManDays { get; set; }
        public int TotalReports { get; set; }
        public int LastDaysPercentage { get; set; }
        public int AveragePercentage { get; set; }
        public int TotalManDaysPercentage { get; set; }
        public bool AverageExceeds { get; set; }
        public bool ManDayExceeds { get; set; }
    }

    public class InspectionManDayData
    {
        public int TotalInspections { get; set; }
        public double TotalManDays { get; set; }
    }

    public class BusinessOVBookingDetail
    {
        public int BookingCount { get; set; }
        public int ProductsCount { get; set; }
        public int FactoryCount { get; set; }
        public double ManDays { get; set; }
    }

    public class QuotationTaskData
    {
        public int PendingQuotations { get; set; }
        public int CompletedQuotations { get; set; }
    }

    public class CustomerFactoryDashboard
    {
        public string FactoryName { get; set; }
        public string FactoryRegionalName { get; set; }
        public string FactoryAddress { get; set; }
        public string FactoryRegionalAddress { get; set; }
        public int BookingCount { get; set; }
        public decimal? Latitude { get; set; }
        public int TotalReportCount { get; set; }
        public decimal? Longitude { get; set; }
        public List<DefectData> DefectList { get; set; }
        public int TotalReportStatusCount { get; set; }
        public List<ReportStatusCount> ResportStatusCount { get; set; }

    }

    public class ReportStatusCount
    {
        public string StatusName { get; set; }
        public int ReportCount { get; set; }
        public string StatusColor { get; set; }
    }

    public class DefectData
    {
        public string DefectName { get; set; }
        public double DefectPercentage { get; set; }
    }

    public class FbReportCustomerDashboard
    {
        public int? FbReportId { get; set; }
        public int? ResultId { get; set; }
    }

    public class InspectionMandayDashboard
    {
        public int InspectionId { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        public double? MandayCount { get; set; }
        public double ActualMandayCount { get; set; }
    }

    public class CustomerDecisionCount
    {
        public int PendingDecisionCount { get; set; }
    }
}
