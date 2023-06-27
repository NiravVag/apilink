using DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO.QcDashboard
{
    public class QcDashboardSearchRequest
    {
       
        public int? CustomerId { get; set; }
        public DateObject serviceDateFrom { get; set; }
        public DateObject serviceDateTo{ get; set; }
    }
    public class QcDashboardCalendarResponse
    {
        public int TodayCount { get; set; }
        public int TomorrowCount { get; set; }
        public int UpcomingAllocatedCount { get; set; }
        public IEnumerable<QcDashboardCalendar> QcCalendar { get; set; }
        public QcDashboardResponseResult Result { get; set; }
    }

    public class QcDashboardCalendar
    {
        public DateTime? ServiceDateFrom { get; set; }
        public int calendarDay { get; set; }
        public int calendarDate { get; set; }
        public int calendarMonth { get; set; }
        public int DayType { get; set; }
        public IEnumerable<QcDashboardCalendarScheduleItem> QcCalendarSchedule { get; set; }
    }
    public class QcDashboardCalendarScheduleItem
    {
        public string BookingIds { get; set; }
        public int? FactoryId { get; set; }
        public string FactoryName { get; set; }
        public string FactoryAddress { get; set; }
        public DateTime? ServiceDateFrom { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        public DateTime? ScheduleServiceDate { get; set; }

    }
    public class QcDashboardReportsResponse
    {
        public IEnumerable<QcReportscount> QcReportscount { get; set; }
        public QcDashboardResponseResult Result { get; set; }
    }
    public class QcReportscount
    {
        public int ReportCount { get; set; }
        public DateTime? ServiceDate { get; set; }
    }
    

    public class QcRejectionReportsResponse
    {
        public QcDashboardResponseResult Result { get; set; }
        public int InspectedBooking { get; set; }
        public int RejectionBooking { get; set; }
        public int QcRejectionBooking { get; set; }
        public double RejectionPercentage { get; set; }
        public double QcRejectionPercentage { get; set; }
    }
    public class QcDashboardCountResponse
    {
        public QcDashboardResponseResult Result { get; set; }
        public int customerCount { get; set; }
        public int factoryCount { get; set; }
        public int inspectionCount { get; set; }
        public int reportCount { get; set; }
    }

    public class QcReports
    {
        public int bookingId { get; set; }
        public int? ReportId {get; set; }
        public DateTime? ServiceDateFrom { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        
    }
    public class QcDashboardCountData
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int? FactoryId { get; set; }
        

    }




    public enum QcDashboardResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
}
