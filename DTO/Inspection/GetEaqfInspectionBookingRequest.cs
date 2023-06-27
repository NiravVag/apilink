using DTO.Common;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inspection
{
    public class GetEaqfInspectionBookingRequest
    {
        // [Required]
        public int? Index { get; set; }

        // [Required]
        public int? pageSize { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EDIT_BOOKING.MSG_CUS_REQ")]
        public int CustomerId { get; set; }
        public string ServiceType { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        public string ServiceFromDate { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]        
        public string ServiceToDate { get; set; }


    }
}
