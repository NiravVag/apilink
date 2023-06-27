using DTO.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Eaqf
{
    public class EaqfDashboardRequest
    {
        [RequiredGreaterThanZero(ErrorMessage = "EDIT_BOOKING.MSG_CUS_REQ")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        public string ToDate { get; set; }
        public string Country { get; set; }
        public string ProductCategory { get; set; }
        public string ProductName { get; set; }
        public string Factory { get; set; }
        public string Vendor { get; set; }
        public string ServiceType { get; set; }
    }

    public class EaqfDashboardResponse
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class EaqfDefectTypeReport
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Critical { get; set; }
        public int TotalCount { get; set; }

    }
}
