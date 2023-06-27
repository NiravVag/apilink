using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{
    public class ICBookingSearchRequest
    {
        public int? BookingId { get; set; }
        public DateObject ServiceFromDate { get; set; }
        public DateObject ServiceToDate { get; set; }
        public int? CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
    }
}
