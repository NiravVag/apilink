using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Invoice
{
    public class QuotationInspectionTravelCost
    {
        public int QuotationId { get; set; }
        public int BookingId { get; set; }       
        public double TravelLandCost { get; set; }
        public double TravelAirCost { get; set; }
        public double TravelHotelCost { get; set; }
        public double OtherCost { get; set; }
        public double Discount { get; set; }
        public double TotalCost { get; set; }
        public double InspectionFees { get; set; }
        public double UnitPrice { get; set; }
        public double Mandays { get; set; }
        public int BillingTo { get; set; }
        public int InvoiceType { get; set; }
        public int? RuleId { get; set; }
        public double? SuggestedManday { get; set; }
        public string PaymentTermsValue { get; set; }
        public int? PaymentTermsCount { get; set; }
    }
    public class QuotationInspectionBilledTo
    {
        public int BookingId { get; set; }
        public int BillingTo { get; set; }
    }
    public class QuotationAuditBilledTo
    {
        public int AuditId { get; set; }
        public int BillingTo { get; set; }
    }
}
