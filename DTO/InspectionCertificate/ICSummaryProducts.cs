using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{
    public class ICSummaryProducts
    {
        public int BookingNumber { get; set; }
        public string PoNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductDesc { get; set; }
        public int BookingQuantity { get; set; }
        public double InspectedQuantity { get; set; }
        public int ICShipmentQuantity { get; set; }
        public string ReportTitle { get; set; }
        public string ReportStatus { get; set; }
    }
}
