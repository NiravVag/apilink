using DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Eaqf
{
    public class EaqfEvent
    {
        public string Classification { get; set; }
        public string InvoiceNo { get; set; }
        public int UserId { get; set; }
    }

    public class EaqfRescheduleEvent
    {
        public string Classification { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        public DateObject ServiceDateFrom { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        [DateGreaterThanAttribute(otherPropertyName = "ServiceDateFrom", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        public DateObject ServiceDateTo { get; set; }
        public double Amount { get; set; }
        public int UserId { get; set; }
        public int PaymentMode { get; set; }
        public string PaymentRef { get; set; }
    }
}
