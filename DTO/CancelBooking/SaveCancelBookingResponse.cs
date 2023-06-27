
using DTO.Quotation;

namespace DTO.CancelBooking
{
    public class SaveCancelBookingResponse
    {
        public int? BookingType { get; set; }
        public SetStatusBusinessRequest QuotationData { get; set; }
        public CancelInvoiceData CancelInvoiceData { get; set; }
        public SaveCancelBookingResponseResult Result { get; set; }
        public bool IsEaqf { get; set; }

    }
    public enum SaveCancelBookingResponseResult
    {
        Success = 1,
        BookingNotFound = 3,
        CancelBookingNotAdded = 2,
        RequestNotCorrectFormat =4,
        BookingStatusNotUpdated =5,
        BookingSavedNotificationError = 6,
        BookingQuotationExists = 7,
        QuotationNotCancelledError = 8,
        RemoveMultiBookingQuotation=9
    }
}
