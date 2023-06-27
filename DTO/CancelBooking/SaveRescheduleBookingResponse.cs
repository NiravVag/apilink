
namespace DTO.CancelBooking
{
    public class SaveRescheduleResponse
    {
        public int? BookingType { get; set; }
        public bool IsEaqf { get; set; }
        public SaveRescheduleResult Result { get; set; }
    }
    public enum SaveRescheduleResult
    {
        Success = 1,
        CancelBookingNotAdded = 2,
        BookingNotFound = 3,
        RequestNotCorrectFormat = 4,
        BookingStatusNotUpdated = 5,
        BookingSavedNotificationError = 6,
        BookingQuotationExists = 7,
        ServiceFromDateFormatWasWrong = 8,
        ServiceToDateFormatWasWrong = 9
    }
}
