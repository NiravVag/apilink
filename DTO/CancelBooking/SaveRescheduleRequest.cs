using DTO.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.CancelBooking
{
 public   class SaveRescheduleRequest
    {
        public int BookingId { get; set; }
        public int ReasonTypeId { get; set; }
        public int? TimeTypeId { get; set; }
        public double? TravelExpense { get; set; }
        public int? CurrencyId { get; set; }
        [StringLength(500)]
        public string Comment { get; set; }
        [StringLength(500)]
        public string InternalComment { get; set; }
        public Boolean IsEmailToAccounting { get; set; }
        public DateObject ServiceFromDate { get; set; }
        public DateObject ServiceToDate { get; set; }
        public bool IsKeepAllocatedQC { get; set; }
        public DateObject FirstServiceDateFrom { get; set; }
        public DateObject FirstServiceDateTo { get; set; }
        public int UserId { get; set; }
    }
}
