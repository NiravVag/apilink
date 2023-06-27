using DTO.Common;
using DTO.Supplier;
using System.ComponentModel.DataAnnotations;

namespace DTO.Eaqf
{
    public class SaveEaqfAuditRequest
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "EDIT_COMPLAINT.MSG_USER_REQ")]
        public int UserId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EDIT_BOOKING.MSG_CUS_REQ")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_CUS_SERVICETYPE_REQ")]
        public int ServiceTypeId { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        public DateObject ServiceDateFrom { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        [DateGreaterThanAttribute(otherPropertyName = "ServiceDateFrom", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        public DateObject ServiceDateTo { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SUP_REQ")]
        public EaqfSupplierDetails Vendor { get; set; }
        public EaqfSupplierDetails Factory { get; set; }
        public int TotalStaff { get; set; }
        public string SurfaceArea { get; set; }
        public int FactoryType { get; set; }
        public int AuditType { get; set; }
        public string EaqfRef { get; set; }
    }
}
