using DTO.Common;
using DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class InspDashboardMobileResponse
    {
        public MobileResult meta { get; set; }
        public DashBoardGraph data { get; set; }
    }

    public class MobileResult
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class DashBoardGraph
    {
        public List<MobileResultAnalyticsDashboard> resultAnalytics { get; set; }
        public List<CommonMobileDataSource> failReasons { get; set; }
        public MobileManDayOverviewDashboard manDayOverview { get; set; }
        public List<DailyInspection> dailyInspection { get; set; }
        public int pendingQuotationCount { get; set; }
        public int taskCount { get; set; }
    }

    public class MobileResultAnalyticsDashboard 
    {
        public int? key { get; set; }
        public string title { get; set; }
        public SvgColor svg { get; set; }
        public int count { get; set; }
    }

    public class SvgColor
    {
        public string fill { get; set; }
    }

    public class CommonMobileDataSource
    {
        public int key { get; set; }
        public int percent { get; set; }
        public string title { get; set; }
        public SvgColor svg { get; set; }
    }

    public class MobileManDayOverviewDashboard
    {
        public int totalInspectionCount { get; set; }
        public double inspectionAverageCount { get; set; }
        public double totalManDayCount { get; set; }
        public double manDayAverageCount { get; set; }
    }

    public class DailyInspection
    {
        public int key { get; set; }
        public string day { get; set; }
        public int count { get; set; }
        public DateObject date { get; set; }
    }

    public class MobileDashboardResponse
    {
        public InspectionManDayOverview manDayData { get; set; }
        public List<CustomerAPIRADashboard> apiData { get; set; }
        public List<InspectionRejectDashboard> rejectData { get; set; }
        public List<BookingDetail> bookingData { get; set; }
        public int pendingQuotationCount { get; set; }
        public List<BookingDetail> currentBookingData { get; set; }
    }

    public class MobileTaskResponse
    {
        public List<MobileTaskModel> data { get; set; }
        public MobileResult meta { get; set; }
    }

    public class MobileTaskModel
    {
        public int key { get; set; }

        public int type { get; set; }

        public int linkId { get; set; }

        public string taskText { get; set; }
    }
}

