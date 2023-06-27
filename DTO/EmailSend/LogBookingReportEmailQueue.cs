using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.EmailSend
{

    //public class LogBookingReportEmailQueueData
    //{
    //    public int? InspectionId { set; get; }
    //    public int? ReportCount { set; get; }
    //}

    public class LogBookingReportEmailQueueData
    {
        public int? InspectionId { set; get; }
        public int? ReportId { set; get; }
        public int? EmailLogId { get; set; }
        public int? StatusId { get; set; }
    }

    public class LogEmailQueues
    {
        public int? EmailLogId { get; set; }
        public int? StatusId { get; set; }
    }

    public class LogEmailSuccessReportCount
    {
        public int? InspectionId { get; set; }
        public int ReportCount { get; set; }
    }

    public class LogEmailSuccessData
    {
        public int? InspectionId { get; set; }
        public int? ReportId { get; set; }
        public int ReportCount { get; set; }
    }

    public class LogEmailReportCount
    {
        public int? InspectionId { get; set; }
        public int? ReportId { get; set; }
        public int ReportCount { get; set; }
    }
}

