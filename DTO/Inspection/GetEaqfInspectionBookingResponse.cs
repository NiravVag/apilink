using DTO.Audit;
using DTO.Quotation;
using DTO.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class GetEaqfInspectionBookingResponse
    {
        public IEnumerable<EaqfBookingItem> EaqfBookingData { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

    }

    public class GetEaqfAuditBookingResponse
    {
        public IEnumerable<EaqfAuditBookingItem> EaqfBookingData { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

    }

    public class EaqfBookingItem
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductType { get; set; }
        public int? ReportRequest { get; set; }
        public bool? IsSameDayReport { get; set; }
        public string VendorName { get; set; }
        public string FactoryName { get; set; }
        public string FactoryState { get; set; }
        public string FactoryCity { get; set; }
        public string FactoryCountry { get; set; }
        public string Instructions { get; set; }
        public string EaqfRef { get; set; }
        public IEnumerable<BookingRepoStatus> InspectionStatus { get; set; }

    }

    public class EaqfAuditBookingItem
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }       
        public string VendorName { get; set; }
        public string FactoryName { get; set; }
        public string FactoryState { get; set; }
        public string FactoryCity { get; set; }
        public string FactoryCountry { get; set; }     
        public string EaqfRef { get; set; }
        public IEnumerable<BookingRepoStatus> AuditStatus { get; set; }

    }

    public class EaqfProductDetails
    {
        public string PoNo { get; set; }
        public string ProductReference { get; set; }
        public string Description { get; set; }
        public int Unit { get; set; }
        public int? Quantity { get; set; }
        public int? AqlLevel { get; set; }
        public int? AQLCritical { get; set; }
        public int? AQLMajor { get; set; }
        public int? AQLMinor { get; set; }
        public string DestinationCountry { get; set; }
    }
    public class GetEaqfInspectionBookingReportResponse
    {
        public IEnumerable<EaqfReportDetails> EaqfBookingReportData { get; set; }
        public int TotalCount { get; set; }
    }

    public class GetEaqfAuditBookingReportResponse
    {
        public IEnumerable<EaqfAuditReportDetails> EaqfBookingReportData { get; set; }
        public int TotalCount { get; set; }
    }
    public class EaqfReportDetails
    {
        public int BookingId { get; set; }
        public string ProductReference { get; set; }
        public string ProductReferenceDescription { get; set; }
        public string PoNumber { get; set; }
        public string ReportTitle { get; set; }
        public string ReportLink { get; set; }
        public string ReportResult { get; set; }
        public string ReportStatus { get; set; }
    }

    public class EaqfAuditReportDetails
    {
        public int BookingId { get; set; }
        public string ReportTitle { get; set; }
        public string ReportLink { get; set; }
        public string Score { get; set; }
        public string ReportStatus { get; set; }
    }

    public class GetEaqfInspectionBookingInvoiceResponse
    {
        public IEnumerable<EaqfInvoiceDetails> EaqfBookingInvoiceData { get; set; }
        public int TotalCount { get; set; }
    }

    public class EaqfInvoiceDetails
    {
        public int BookingId { get; set; }
        public string InvoicePdfUrl { get; set; }
        public string InvoiceNo { get; set; }
        public double InvoiceTotal { get; set; }
    }
}
