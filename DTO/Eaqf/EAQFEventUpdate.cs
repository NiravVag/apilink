using DTO.Common;

namespace DTO.Eaqf
{
    public class EAQFRescheduleEventUpdate
    {
        public string Classification { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string ReasonChange { get; set; }
    }
    public class EAQFBookingEventUpdate
    {
        public string Classification { get; set; }
        public int StatusId { get; set; }
        public string StatusDate { get; set; }
    }

    public class EAQFBookingEventCancelUpdate
    {
        public string Classification { get; set; }
        public string Reason { get; set; }
    }

    public class EAQFEventUpdate
    {
        public int BookingId { get; set; }
        public int StatusId { get; set; }
        public DateObject ServiceFromDate { get; set; }
        public DateObject ServiceToDate { get; set; }
        public int ReasonTypeId { get; set; }
    }



    public enum EAQFBookingEventRequestType
    {
        AddStatus = 1,
        DateChange = 2,
        CancelStatus = 3,
    }
}
