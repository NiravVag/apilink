using DTO.Audit;
using DTO.Common;
using DTO.Inspection;
using DTO.Kpi;
using DTO.Report;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DTO.Invoice
{
    public class InvoiceStatusSummaryResult
    {
        public List<InvoiceStatusSummaryItem> invoiceStatusDataList { get; set; }
        public IEnumerable<ServiceTypeList> serviceTypeList { get; set; }
        public List<InvoiceBookingQuotation> quotationList { get; set; }
        public List<FactoryContact> FactoryContactIdList { get; set; }
        public List<SupplierContact> SupplierContactIdList { get; set; }
        public List<SupplierAddressDetails> SupplierAddressIdList { get; set; }
        public List<InvoiceItem> InvoiceList { get; set; }
        public List<InvoiceItem> ExtraFeesInvoiceList { get; set; }
        public List<InspectionStatus> InvoiceInspectionStatusList { get; set; }
        public List<AuditStatus> InvoiceAuditStatusList { get; set; }
        public List<InspectionHoldReasons> InspectionHoldReasons { get; set; }
        public List<BookingBrandAccess> BookingBrandList { get; set; }
        public int TotalCount { get; set; }

        public int ServiceId { get; set; }
    }

    public class InvoiceInspSummary
    {
        public List<InvoiceStatusSummaryItem> BookingDataList { get; set; }
        public IQueryable<InspTransaction> InspTransaction { get; set; }

        public int TotalCount { get; set; }
    }

    public class InvoiceAudSummary
    {
        public List<InvoiceStatusSummaryItem> BookingDataList { get; set; }
        public IQueryable<AudTransaction> AudTransaction { get; set; }

        public int TotalCount { get; set; }
    }

    public class InvoiceStatusSummaryResponse
    {
        public List<InvoiceStatusSummary> Data { get; set; }
        public string Bangladesh_BankId { get; set; }
        public IEnumerable<InspectionStatus> InvoiceStatuslst { get; set; }
        public IEnumerable<AuditStatus> InvoiceAuditStatusList { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public InvoiceStatusResult Result { get; set; }
    }

    public class InvoiceStatusSummaryItem
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public int? BookingId { get; set; }
        public int? AuditId { get; set; }
        public DateTime BookingServiceDateFrom { get; set; }
        public DateTime BookingServiceDateTo { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int CustomerId { get; set; }
        public int OfficeId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string InvoiceTo { get; set; }
        public int? InvoiceTypeId { get; set; }
        public string InvoiceTypeName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool? IsInspection { get; set; }
        public int? PaymentStatusId { get; set; }
        public string PaymentStatusName { get; set; }
        public DateTime? PaymentDate { get; set; }

        public int BookingStatusId { get; set; }
        public string FactoryCountry { get; set; }
    }
    public class InvoiceStatusSummary
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }

        public int? BookingId { get; set; }
        public int? AuditId { get; set; }
        public int? QuotationId { get; set; }
        public string ServiceDate { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public DateObject ServiceStartDate { get; set; }
        public DateObject ServiceEndDate { get; set; }
        public int QuotationStatusId { get; set; }
        public int BillTo { get; set; }
        public int ExtraFeeBillTo { get; set; }
        public int PaymentTerms { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string InvoiceTo { get; set; }
        public string ExtraFeesInvoiceTo { get; set; }
        public int? InvoiceTypeId { get; set; }
        public string InvoiceTypeName { get; set; }
        public int? BookingStatusId { get; set; }
        public string BookingStatusName { get; set; }
        public string InvoiceStatusColor { get; set; }
        public string InvoiceStatusName { get; set; }
        public string InvoiceDate { get; set; }

        public bool? IsInspection { get; set; }
        public int? PaymentStatusId { get; set; }
        public string PaymentStatusName { get; set; }
        public string PaymentDate { get; set; }

        public string Service { get; set; }
        public string HoldType { get; set; }
        public string HoldReason { get; set; }
        public string QuotationStatus { get; set; }
        public int QuotationSupplierId { get; set; }
        public string QuotationSupplierName { get; set; }
        public string QuotationSupplierAddress { get; set; }
        public int QuotationFactoryId { get; set; }
        public string QuotationFactoryName { get; set; }
        public string QuotationFactoryAddress { get; set; }
        public List<int> QuotationSupplierContacts { get; set; }
        public List<int> QuotationFactoryContacts { get; set; }
        public string QuotationBilledName { get; set; }
        public double QuotationTotalFees { get; set; }
        public string QuotationCurrencyName { get; set; }
        public string QuotationCurrencyCode { get; set; }
        public int QuotationCurrencyId { get; set; }
        public string BillingEntityName { get; set; }
        public int BillingEntityId { get; set; }
        public string BankName { get; set; }
        public int BankId { get; set; }
        public int BankcurrencyId { get; set; }
        public string BankcurrencyName { get; set; }
        public int ExchangeRate { get; set; }
        public List<BankTaxData> TaxList { get; set; }
        public string CustomerLegalName { get; set; }
        public string SupplierLegalName { get; set; }
        public string FactoryLegalName { get; set; }
        public string FactoryCountry { get; set; }
        public string BrandNames { get; set; }
        public double? InvoiceAmount { get; set; }
        public string CurrencyCode { get; set; }

        public string ExtraFeeInvoiceNo { get; set; }
        public double? ExtraFeesAmount { get; set; }
        public string ExtraFeesStatusName { get; set; }
        public string ExtraFeesInvoiceDate { get; set; }
        public string ExtraFeesCurrencyCode { get; set; }
        public int? ExtraFeesPaymentStatusId { get; set; }
        public string ExtraFeesPaymentStatusName { get; set; }
        public string ExtraFeesPaymentDate { get; set; }
    }
    public class ExportInvoiceStatus
    {
        [Description("Booking No")]
        public int? BookingId { get; set; }
        [Description("Quotation No")]
        public int? QuotationId { get; set; }
        [Description("Service Date")]
        public string ServiceDate { get; set; }
        [Description("Invoice No")]
        public string InvoiceNo { get; set; }
        [Description("Invoice Amount")]
        public double? InvoiceAmount { get; set; }
        [Description("Currency")]
        public string CurrencyCode { get; set; }
        [Description("Invoice Date")]
        public string InvoiceDate { get; set; }
        [Description("Service Name")]
        public string ServiceName { get; set; }
        [Description("Brand")]
        public string BrandNames { get; set; }

        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Supplier Name")]
        public string SupplierName { get; set; }
        [Description("Factory Name")]
        public string FactoryName { get; set; }
        [Description("Billed To")]
        public string InvoiceTo { get; set; }
        [Description("Invoice Type")]
        public string InvoiceTypeName { get; set; }
        [Description("Status Name")]
        public string StatusName { get; set; }
        [Description("Payment Status")]
        public string PaymentStatusName { get; set; }
        [Description("Payment Date")]
        public DateTime? PaymentDate { get; set; }

        [Description("Quotation Status")]
        public string QuotationStatus { get; set; }

        [Description("Hold Type")]
        public string HoldType { get; set; }

        [Description("Hold Reason")]
        public string HoldReason { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
        [Description("Communication")]
        public string Communication { get; set; }

        [Description("Extra/penalty InvoiceNo")]
        public string ExtraFeeInvoiceNo { get; set; }
        [Description("Extra/Penalty Status")]
        public string ExtraFeesStatusName { get; set; }
        [Description("Extra/Penalty Total")]
        public double? ExtraFeesAmount { get; set; }
        [Description("Extra/Penalty Invoice date")]
        public string ExtraFeesInvoiceDate { get; set; }
        [Description("Extra/Penalty BilledTo")]
        public string ExtraFeesInvoiceTo { get; set; }
        [Description("Extra/Penalty Payment Status")]
        public string ExtraFeesPaymentStatusName { get; set; }
        [Description("Extra/Penalty Payment Date")]
        public string ExtraFeesPaymentDate { get; set; }

        [Description("Extra/Penalty Currency")]
        public string ExtraFeesCurrency { get; set; }

    }



    public enum InvoiceStatusResult
    {
        Success = 1,
        NotFound = 2,
        Failure = 3
    }



    public class InvoiceStatusSummaryRequest
    {
        public DateObject InvoiceFromDate { get; set; }
        public DateObject InvoiceToDate { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        [Required]
        public int ServiceId { get; set; }
        public int? InvoiceTo { get; set; }
        public int? InvoiceType { get; set; }
        public int? SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        public List<int> InvoiceStatusId { get; set; }
        public List<int> ActualInvoiceStatusId { get; set; }
        public int? DateTypeId { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public IEnumerable<int> StatusIdlst { get; set; }

        public IEnumerable<int?> OfficeIdList { get; set; }
        public IEnumerable<int> FactoryCountryIds { get; set; }
        public IEnumerable<int?> PaymentStatusIdList { get; set; }
        public IEnumerable<int?> BrandIdList { get; set; }
    }

    public enum InvoiceStatusSummaryStatusList
    {
        Pending = 1,
        Invoiced = 2
    }

    public class InvoiceItem
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public int? BookingId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string InvoiceTo { get; set; }
        public int? InvoiceTypeId { get; set; }
        public string InvoiceTypeName { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool? IsInspection { get; set; }
        public int? PaymentStatusId { get; set; }
        public string PaymentStatusName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double? InvoiceAmount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class InvoiceCommunicationSaveRequest
    {
        [Required]
        [MaxLength(2000)]
        public string Comment { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
    }

    public class InvoiceCommunicationSaveResponse
    {
        public InvoiceCommunicationSaveResultResponse Result { get; set; }
    }
    public enum InvoiceCommunicationSaveResultResponse
    {
        Success = 1,
        RequestNotCorrectFormat = 2,
        Failed = 3
    }
    public enum InvoiceCommunicationTableResultResponse
    {
        Success = 1,
        NotFound = 2,
        Failed = 3,
        RequestNotCorrectFormat = 4
    }
    public class InvoiceCommunicationTableResponse
    {
        public InvoiceCommunicationTableResultResponse Result { get; set; }
        public List<InvoiceCommunicationTable> InvoiceCommunicationTableList { get; set; }
    }
    public class InvoiceCommunicationTable
    {
        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
    }
    public class InvoiceCommunicationTableRepo
    {
        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
