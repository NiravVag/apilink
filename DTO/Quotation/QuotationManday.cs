using System;
using System.Collections.Generic;

namespace DTO.Quotation
{
    public class QuotationManday
    {
        public int BookingId { get; set; }
        public int QuotationId { get; set; }
        public string ServiceDate { get; set; }
        public double? ManDay { get; set; }
        public string Remarks { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }

    public class ActualManDayDateCount
    {
        public DateTime ServiceDate { get; set; }
        public double ActualManDayCount { get; set; }
    }

    public class QcActualManDayRepo
    {
        public DateTime ServiceDate { get; set; }
        public int QcId { get; set; }
        public double ActualManDay { get; set; }
    }

    public enum QuotationMandayResult
    {
        Success = 1,
        NotFound = 2
    }
    public class QuotationMandayResponse
    {
        public IEnumerable<QuotationManday> QuotationMandaysList { get; set; }
        public QuotationMandayResult MandayResult { get; set; }
    }
    public class BookingDate
    {
        public int BookingId { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
    }
    public class BookingDateChangeInfo
    {
        public int BookingId { get; set; }
        public string PreviousServiceDateFrom { get; set; }
        public string PreviousServiceDateTo { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public BookingDateChangeInfoResult Result { get; set; }
    }
    public enum BookingDateChangeInfoResult
    {
        Verified = 1,
        DateChanged = 2,
        ServiceNotFound = 3,
        NodateFound = 4,
        Error = 5
    }

    public class QuotManday
    {
        public int BookingId { get; set; }
        public double? ManDay { get; set; }
        public double? TravelManDay { get; set; }        
        public double? SuggestedManday { get; set; }
    }

    public class QuotationBooking
    {
        public int BookingId { get; set; }
        public int? QuotationId { get; set; }
    }

    public class QuotationData
    {
        public int BookingId { get; set; }
        public int QuotationId { get; set; }
        public double? OtherCost { get; set; }
    }
}
