using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CancelBooking
{
    public class EditCancelBookingResponse
    {
        public EditCancelBookingItem ItemDetails { get; set; }
        public SaveCancelBookingRequest CancelItem { get; set; }
        public SaveRescheduleRequest RescheduleItem { get; set; }
        public CancelBookingResponseResult Result { get; set; }
    }
    public enum CancelBookingResponseResult
    {
        success = 1,
        CannotGetBookingDetails = 2,
        CannotGetCancelDetails = 3,
        CannotGetRescheduleDetails =4
    }
}
