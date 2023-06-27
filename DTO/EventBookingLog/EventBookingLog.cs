using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.EventBookingLog
{
    public class EventBookingLogInfo
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? AuditId { get; set; }
        public int? QuotationId { get; set; }
        public int? StatusId { get; set; }
        public int? UserId { get; set; }
        public string LogInformation { get; set; }

    }

    public class EventLogRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? EventId { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? EntityId { get; set; }
        public DateTime? ResponseTime { get; set; }
    }

    public class FbLogInfo
    {
        public string LogInformation { get; set; }
    }


    public class FBBookingLogInfo
    {
        public int? ServiceId { get; set; }
        public int? BookingId { get; set; }
        public int? AccountId { get; set; }
        public int? ReportId { get; set; }
        public int? MissionId { get; set; }
        public int? MissionProductId { get; set; }
        public string RequestUrl { get; set; }
        public int? CreatedBy { get; set; }
        public string LogInformation { get; set; }
    }

    public class ZohoRequestLogInfo
    {
        public int Id { get; set; }
        public long? CustomerId { get; set; }
        public string RequestUrl { get; set; }
        public string LogInformation { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
