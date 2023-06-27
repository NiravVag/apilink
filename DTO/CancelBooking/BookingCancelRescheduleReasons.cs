
using System.Collections.Generic;

namespace DTO.CancelBooking
{
    public class BookingCancelRescheduleReasonItem
    {
        public int Id { get; set; }
        public string Reason { get; set; }
    }
    public class BookingCancelRescheduleResponse
    {
        public IEnumerable<BookingCancelRescheduleReasonItem> ResponseList { get; set; }
        public BookingCancelRescheduleReasonsResult Result { get; set; }
    }
    public enum BookingCancelRescheduleReasonsResult
    {
        Success = 1,
        NotFound = 2,
        CannotGetBookingDetail =3
    }
}
