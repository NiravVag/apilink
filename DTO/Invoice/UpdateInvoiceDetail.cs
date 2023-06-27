using DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.Invoice
{

    public class UpdateInvoiceDetailRequest
    {
        public UpdateInvoiceBaseDetail invoiceBaseDetail { get; set; }

        public List<UpdateInvoiceDetail> invoiceDetails { get; set; }
    }
    public class UpdateInvoiceBaseDetail
    {
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public DateObject InvoiceDate { get; set; }
        [Required]
        public DateObject PostDate { get; set; }
        public string Subject { get; set; }
        [Required]
        public int? BillTo { get; set; }
        public string BilledName { get; set; }
        [Required]
        public string BilledAddress { get; set; }
        [Required]
        public IEnumerable<int?> ContactIds { get; set; }
        [Required]
        public string PaymentTerms { get; set; }
        [Required]
        public string PaymentDuration { get; set; }
        [Required]
        public int? Office { get; set; }
        [Required]
        public int? BillMethod { get; set; }
        public string Currency { get; set; }
        [Required]
        public int? InvoicePaymentStatus { get; set; }
        public DateObject InvoicePaymentDate { get; set; }
        public double TotalInvoiceFees { get; set; }
        public double TotalTaxAmount { get; set; }
        public double TotalTravelFees { get; set; }
        [Required]
        public int ServiceId { get; set; }
    }


    public class UpdateInvoiceDetail
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public int BookingNo { get; set; }
        public double ManDays { get; set; }
        public double UnitPrice { get; set; }
        public double InspectionFees { get; set; }
        public double TravelAirFees { get; set; }
        public double TravelLandFees { get; set; }
        public double TravelOtherFees { get; set; }
        public double HotelFees { get; set; }
        public double OtherFees { get; set; }
        public double Discount { get; set; }
        public string Remarks { get; set; }
        public double TotalTravelFees { get; set; }
        public double TotalInvoiceFees { get; set; }
        public double TotalTaxAmount { get; set; }
        public double TotalInspectionFees { get; set; }
    }

    public class UpdateInvoiceDetailsResponse
    {
        public UpdateInvoiceDetailResult Result { get; set; }
    }

    public enum UpdateInvoiceDetailResult
    {
        Success=1,
        Failure=2
    }
}
