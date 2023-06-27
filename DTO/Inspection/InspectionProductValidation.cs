using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class ProductValidateData
    {
        public int BookingId { get; set; }
        public int PoTransactionId { get; set; }
        public bool QuotationExists { get; set; }
        public bool PickingExists { get; set; }
        public bool ReportExists { get; set; }
        public bool ValidationSuccess { get; set; }
    }
}
