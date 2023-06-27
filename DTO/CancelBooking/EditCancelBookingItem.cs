using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CancelBooking
{
    public class EditCancelBookingItem
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int StatusId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string ProductCategory { get; set; }
        public DateObject FirstServiceDateFrom { get; set; }
        public DateObject FirstServiceDateTo { get; set; }
        public bool IsMultiBookingQuotation { get; set; }
        public bool IsRescheduleBooking { get; set; }
        //public string Office { get; set; }

        //public int StatusId { get; set; }

        //public int LeadTime { get; set; }

        //public IEnumerable<DateObject> HolidayDates { get; set; }
    }
}
