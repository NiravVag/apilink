using DTO.Common;
using DTO.Inspection;
using DTO.Kpi;
using DTO.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.Invoice
{
    public class InvoiceSummaryResponse
    {
        public List<InvoiceSummary> Data { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<InspectionStatus> InvoiceStatuslst { get; set; }
        public InvoiceSummaryResult Result { get; set; }
    }

    public class InvoiceSummary
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceTo { get; set; }
        public string InvoiceToName { get; set; }
        public string InvoiceTypeName { get; set; }
        public int InvoiceTypeId { get; set; }
        public string Service { get; set; }
        public int ServiceId { get; set; }
        public string ServiceType { get; set; }
        public string InvoiceCurrency { get; set; }
        public double TravelFee { get; set; }
        public double OtherExpense { get; set; }
        public double? HotelFee { get; set; }
        public double? Discount { get; set; }
        public double? ExtraFees { get; set; }
        public double InspFees { get; set; }
        public double TotalFee { get; set; }
        public bool IsInspection { get; set; }
        public bool IsTravelExpense { get; set; }
        public int InvoiceStatusId { get; set; }
        public string InvoiceStatusName { get; set; }
        public List<int> CustomerIdList { get; set; }
        public int BillTo { get; set; }
        public int BankId { get; set; }
        public string CustomerName { get; set; }
        public string FactoryCountry { get; set; }
        public string PaymentStatusName { get; set; }
        public string BillingMethodName { get; set; }
        public string InvoiceOfficeName { get; set; }
    }

    public class InvoiceBookingData
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int AuditId { get; set; }
        public double InspFees { get; set; }
        public double TravelFee { get; set; }
        public double OtherExpense { get; set; }
        public string CreatedBy { get; set; }
        public double? HotelFee { get; set; }
        public double? Discount { get; set; }
        public double? InvoiceTotal { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceTo { get; set; }
        public string InvoiceToName { get; set; }
        public string InvoiceTypeName { get; set; }
        public string Service { get; set; }
        public string ServiceType { get; set; }
        public string InvoiceCurrency { get; set; }
        public bool IsInspection { get; set; }
        public bool IsTravelExpense { get; set; }
        public double? TravelAirFee { get; set; }
        public double? TravelLandFee { get; set; }
        public double? TravelOtherFee { get; set; }
        public string ProrateBookingNo { get; set; }
        public string InvoieRemarks { get; set; }
        public string CustomerName { get; set; }
        public double? ExtraFees { get; set; }
        public int? BilledTo { get; set; }
        public string BilledToName { get; set; }
        public string InvoiceOffice { get; set; }
        public string BookingServiceDateFrom { get; set; }
        public string BookingServiceDateTo { get; set; }
        public double? UnitPrice { get; set; }
        public double? BillingManDays { get; set; }
        public string InvoiceMethod { get; set; }
        public int? InvoicePaymentStatus { get; set; }
        public string FactoryCountry { get; set; }
        public string BilledName { get; set; }
    }
    public class ExportInvoiceBookingData
    {
        [Description("Invoice No")]
        public string InvoiceNo { get; set; }
        [Description("Invoice Date")]
        public DateTime? InvoiceDate { get; set; }
        [Description("Invoice Type")]
        public string InvoiceTypeName { get; set; }
        [Description("Billed To")]
        public string InvoiceTo { get; set; }
        [Description("Billed To Name")]
        public string InvoiceToName { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Service Line")]
        public string Service { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Booking No")]
        public int BookingId { get; set; }
        [Description("Service Date From")]
        public DateTime BookingServiceDateFrom { get; set; }
        [Description("Service Date To")]
        public DateTime BookingServiceDateTo { get; set; }
        [Description("Invoice Currency")]
        public string InvoiceCurrency { get; set; }
        [Description("Invoice Office")]
        public string InvoiceOfficeName { get; set; }
        [Description("Billing Method")]
        public string BillingMethodName { get; set; }
        [Description("Payment Status")]
        public string PaymentStatusName { get; set; }
        [Description("Inspection Fee")]
        public double InspFees { get; set; }
        [Description("Travel Land Expense")]
        public double? TravelLandFee { get; set; }
        [Description("Travel Air Expense")]
        public double? TravelAirFee { get; set; }
        [Description("Travel Other Expense")]
        public double? TravelOtherFee { get; set; }
        [Description("Travel Total Expense")]
        public double TravelFee { get; set; }
        [Description("Hotel Fee")]
        public double? HotelFee { get; set; }
        [Description("Other Expense")]
        public double OtherExpense { get; set; }
        [Description("Discount")]
        public double? Discount { get; set; }
        [Description("Extra Fee")]
        public double? ExtraFees { get; set; }
        [Description("Total Fee")]
        public double? InvoiceTotal { get; set; }
        [Description("Is Inspection")]
        public bool IsInspection { get; set; }
        [Description("Is Travel Expense")]
        public bool IsTravelExpense { get; set; }
        [Description("Created By")]
        public string CreatedBy { get; set; }
        [Description("Quotation Man Day")]
        public int? QuotationManDay { get; set; }
        [Description("Actual Man Day")]
        public double? ActualManday { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }


    }

    public class InvoiceBookingSummaryResponse
    {
        public List<InvoiceBookingData> Data { get; set; }
        public InvoiceSummaryResult Result { get; set; }
    }

    public enum InvoiceSummaryResult
    {
        Success = 1,
        NotFound = 2,
        Failure = 3
    }

    public class InvoiceSummaryItem
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceTo { get; set; }
        public int? InvoiceToId { get; set; }

        public int OfficeId { get; set; }
        public int? InvoiceTypeId { get; set; }
        public string InvoiceTypeName { get; set; }
        public int? BookingId { get; set; }
        public int? AuditId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string InvoiceCurrency { get; set; }
        public double? InspFees { get; set; }
        public double? TravelFee { get; set; }
        public double? TravelAirFee { get; set; }
        public double? TravelLandFee { get; set; }
        public double? TravelOtherFee { get; set; }
        public double? OtherExpense { get; set; }
        public double? TotalFee { get; set; }
        public bool? IsInspection { get; set; }
        public bool? IsTravelExpense { get; set; }
        public string CreatedBy { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public double? TaxValue { get; set; }
        public double? HotelFee { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? ExtraFees { get; set; }
        public double? Discount { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime BookingServiceDateFrom { get; set; }
        public DateTime BookingServiceDateTo { get; set; }
        public string ProrateBookingNo { get; set; }
        public string InvoiceRemarks { get; set; }
        public string Invoiceoffice { get; set; }
        public int? QuManday { get; set; }
        public double ActualManday { get; set; }
        public double? UnitPrice { get; set; }
        public double? BillingManDays { get; set; }
        public string FactoryCountry { get; set; }
        public string PaymentStatusName { get; set; }
        public string BillingMethodName { get; set; }
        public string InvoiceOfficeName { get; set; }
        public int BankId { get; set; }
        public int PaymentStatusId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string InvoicedName { get; set; }
    }

    public class InvoiceExtraFeeItem
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public int? InvoiceId { get; set; }
        public int? BookingId { get; set; }
        public int? AuditId { get; set; }
        public int? InvoiceTo { get; set; }
        public double? TotalExtraFees { get; set; }
        public double? TotalExtraSubFees { get; set; }
        public double? TotalExtrFeeTax { get; set; }
        public int? BilledTo { get; set; }
    }

    public class InvoiceSummaryRequest
    {
        public DateObject InvoiceFromDate { get; set; }
        public DateObject InvoiceToDate { get; set; }
        public int? InvoiceTo { get; set; }
        public int? InvoiceType { get; set; }
        public int? SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        [Required]
        public int ServiceId { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public int? DateTypeId { get; set; }
        public List<int> InvoiceStatusId { get; set; }
        public List<int> FactoryCountryIds { get; set; }
        public List<int> OfficeIdList { get; set; }
        public List<int> BillingMethodIdList { get; set; }
        public List<int> PaymentStatusIdList { get; set; }
    }

    public class InvoicePdfCreatedRequest
    {
        public List<string> InvoiceNumbers { get; set; }
    }

    public class InvoicePdfCreatedResponse
    {
        public List<string> InvoiceNumbers { get; set; }
        public InvoicePdfCreatedResponseResult Result { get; set; }
    }

    public enum InvoicePdfCreatedResponseResult
    {
        PdfCreatedToAllInvoice = 1,
        PdfCreatedToFewInvoice = 2,
        PdfNotCreatedToAnyInvoice = 3,
        RequestIsNotValid = 4
    }

    public class InvoiceViewData
    {
        public string CustomerId { get; set; }
        public string InvoiceType { get; set; }
        public string TemplateName { get; set; }
        public string EntityId { get; set; }
        public string InvoicePreview { get; set; }
        public bool IsServerRequest { get; set; }
    }

    public class InvoiceReportTemplate
    {
        public List<InvoiceReportTemplateResult> ResultList { get; set; }
        public int Total { get; set; }
    }

    public class InvoiceReportTemplateResult
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string CustomerId { get; set; }
        public object Customer { get; set; }
        public DateTime CreationDate { get; set; }
        public object CreatorUser { get; set; }
        public DateTime ModificationDate { get; set; }
        public object ModifierUser { get; set; }
        public bool Active { get; set; }
        public int InvoiceType { get; set; }
    }

    public class InvoiceReportTemplateItem
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string CustomerId { get; set; }
        public int InvoiceType { get; set; }
    }

    public class InvoiceReportTemplateResponse
    {
        public List<InvoiceReportTemplateItem> ResultList { get; set; }
        public InvoiceSummaryResult Result { get; set; }
    }

    public class InvoiceReportUrlResponse
    {
        public string Url { get; set; }
        public string EntityId { get; set; }
        public InvoiceSummaryResult Result { get; set; }
    }

    public class InvoiceCancelResponse
    {
        public string Data { get; set; }
        public InvoiceSummaryResult Result { get; set; }
    }

    public class InvoiceSummarySearchResult
    {
        public List<InvoiceSummaryItem> invoiceDataList { get; set; }
        public List<InvoiceExtraFeeItem> invoiceExtraFeeList { get; set; }
        public IEnumerable<ServiceTypeList> serviceTypeList { get; set; }
        public int TotalCount { get; set; }
        public List<InspectionStatus> StatusCountList { get; set; }
    }

    public class InvoiceKpiTemplateResponse
    {
        public List<KPITemplate> TemplateList { get; set; }
        public InvoiceSummaryResult Result { get; set; }
    }

    public class InvoiceReportTemplateRequest
    {
        public List<string> InvoicePreviewTypes { get; set; }
        public string CustomerId { get; set; }
    }

    public class InvoiceKpiTemplateRequest
    {
        public List<int?> CustomerIds { get; set; }
    }
}
