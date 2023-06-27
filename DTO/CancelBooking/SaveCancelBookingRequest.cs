using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.CancelBooking
{
    public class SaveCancelBookingRequest
    {
        public int BookingId { get; set; }
        public int ReasonTypeId { get; set; }
        public int? TimeTypeId { get; set; }
        public double? TravelExpense { get; set; }
        public int? CurrencyId { get; set; }
        public bool IsCancelKeepAllocatedQC { get; set; }
        [StringLength(500)]
        public string Comment { get; set; }
        [StringLength(500)]
        public string InternalComment { get; set; }
        public Boolean IsEmailToAccounting { get; set; }
    }
}
