using DTO.Common;
using DTO.Inspection;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Invoice
{
    public class SaveManualInvoice
    {
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int InvoiceTo { get; set; }
        public int? SupplierId { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public DateObject InvoiceDate { get; set; }
        public string Attn { get; set; }
        [Required]
        public string BilledName { get; set; }
        [Required]
        public string BilledAddress { get; set; }
        public int BillingEntity { get; set; }
        public string Email { get; set; }
        public int? ServiceId { get; set; }
        public int? BookingNo { get; set; }
        public int? ServiceType { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        public string PaymentTerms { get; set; }
        [Required]
        public int BankId { get; set; }
        [Required]
        public int OfficeId { get; set; }
        public int? PaymentDuration { get; set; }
        public int? CountryId { get; set; }
        [Required]
        public DateObject FromDate { get; set; }
        [Required]
        public DateObject ToDate { get; set; }
        [Required]
        public IEnumerable<SaveManualInvoiceItem> InvoiceItems { get; set; }
        public int? PaymentMode { get; set; }
        public string PaymentRef { get; set; }
        public int UserId { get; set; }

    }

    public class SaveManualInvoiceItem
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double? ServiceFee { get; set; }
        public double? Manday { get; set; }
        public double? UnitPrice { get; set; }
        public double? EXPChargeBack { get; set; }
        public double? OtherCost { get; set; }
        public double? Discount { get; set; }
        public string Remarks { get; set; }
    }

    public enum ManualInvoiceResult
    {
        Success = 1,
        NotFound = 2,
        InvoiceNoAlreadyExist = 3,
        InvoiceItemNotFound = 4
    }

    public class SaveManualInvoiceResponse
    {
        public int ManaualInvoiceId { get; set; }
        public ManualInvoiceResult Result { get; set; }
    }

    public class DeleteManualInvoiceResponse
    {
        public ManualInvoiceResult Result { get; set; }
    }

    public class ManualInvoiceSummaryRequest
    {
        public int CustomerId { get; set; }
        public int InvoiceTo { get; set; }
        public string InvoiceNo { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public bool? IsEAQF { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
        public IEnumerable<int> InvoiceStatusId { get; set; }
    }

    public class ManualInvoiceSummaryItem
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string BilledName { get; set; }
        public string Attn { get; set; }
        public string Service { get; set; }
        public string ServiceType { get; set; }
        public double? ServiceFee { get; set; }
        public double? EXPChargeBack { get; set; }
        public double? OtherCost { get; set; }
        public double? Tax { get; set; }
        public double? TaxAmount { get; set; }
        public double? TotalFee { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }

        public string InvoicePdfUrl { get; set; }
    }

    public class ManualInvoiceSummaryResponse
    {
        public List<ManualInvoiceSummaryItem> Data { get; set; }
        public List<InspectionStatus> StatusCountList { get; set; }
        public ManualInvoiceResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
    public class ManualInvoiceItemRepo
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int InvoiceTo { get; set; }
        public string InvoiceToName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string BilledName { get; set; }
        public string Attn { get; set; }
        public string Service { get; set; }
        public string ServiceType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public double? Tax { get; set; }
        public double? TaxAmount { get; set; }
        public double? TotalFee { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool? IsEAQF { get; set; }
    }
    public class ManualInvoiceDetailItemRepo
    {
        public int InvManualId { get; set; }
        public double? ServiceFee { get; set; }
        public double? EXPChargeBack { get; set; }
        public double? OtherCost { get; set; }

    }
    public class GetManualInvoiceResponse
    {
        public SaveManualInvoice ManualInvoice { get; set; }
        public ManualInvoiceResult Result { get; set; }
    }

    public class ManualInvoiceSummaryExportResponse
    {
        public List<ManualInvoiceExportSummary> ManualInvoices { get; set; }
        public ManualInvoiceExportSummaryRequestFilter RequestFilters { get; set; }
        public ManualInvoiceResult Result { get; set; }
    }

    public class ManualInvoiceExportSummaryRequestFilter
    {
        public string CustomerName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string InvoiceTo { get; set; }
        public string InvoiceNo { get; set; }
    }
    public class ManualInvoiceExportSummary
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceTo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BilledName { get; set; }
        public string Attn { get; set; }
        public string Service { get; set; }
        public string ServiceType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public double? Tax { get; set; }
        public double? TaxAmount { get; set; }
        public double? ServiceFee { get; set; }
        public double? ExpChargeBack { get; set; }
        public double? SubTotal { get; set; }
        public double? OtherCost { get; set; }
        public double? TotalAmount { get; set; }
    }
    public class ManualInvoiceExportRepoItem
    {
        public int CustomerId { get; set; }
        public int InvoiceTo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceToName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string BilledName { get; set; }
        public double? TotalAmount { get; set; }
        public double? SubTotal { get; set; }
        public string Attn { get; set; }
        public string Service { get; set; }
        public string ServiceType { get; set; }
        public double? Tax { get; set; }
        public double? TaxAmount { get; set; }
        public double? ServiceFee { get; set; }
        public double? ExpChargeBack { get; set; }
        public double? OtherCost { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }

    public class EAQFManualInvoiceData
    {
        public string BilledName { get; set; }
        public string BilledAddress { get; set; }
        public string Attn { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDateTime { get; set; }
        public string InvoiceDate { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentRef { get; set; }
    }
    public class EAQFManualInvoiceFastReport
    {
        public IEnumerable<EAQFManualInvoiceData> Invoice { get; set; }
        public List<InvManTranDetail> InvoiceItems { get; set; }
    }
    
}