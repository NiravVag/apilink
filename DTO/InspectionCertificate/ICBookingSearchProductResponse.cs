using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{
    public class ICBookingSearchProductResponse
    {
        public string PONo { get; set; }
        public int POId { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int? ShipmentQty { get; set; }
        public double RemainingQty { get; set; }
        public string DestinationCountry { get; set; }
        public string Unit { get; set; }
        public int? FBReportStatus { get; set; }
        public int? FBReportId { get; set; }
        public int BookingId { get; set; }
        public int? InspPOTransactionId { get; set; }
        public bool EnableCheckbox { get; set; }
        public bool Checked { get; set; }
        public int TotalICQty { get; set; }
        public double PresentedQty { get; set; }
        public int? PoColorId { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public int? BusinessLine { get; set; }
    }
}
