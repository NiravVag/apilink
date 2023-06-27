using DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Claim
{
    #region Pending Claim
    public class PendingClaimSearchRequest
    {
        public int? SearchTypeId { get; set; }
        public int? CustomerId { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public string SearchTypeText { get; set; }
        public int? OfficeId { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
    }
    public class PendingClaimSummaryResponse
    {
        public IEnumerable<PendingClaimResponse> Data { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public bool IsAccountingCreditNoteRole { get; set; }
        public CreditNoteResult Result { get; set; }
    }
    public class GetPendingClaimResponse
    {
        public IEnumerable<GetPendingClaimData> Data { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public CreditNoteResult Result { get; set; }
        public bool IsAccountingCreditNoteRole { get; set; }
    }
    public class GetPendingClaimData : SaveCreditNoteItem
    {
        public int? BookingNo { get; set; }
        public string InvoiceNo { get; set; }
        public string ClaimNo { get; set; }
        public string InspectionDate { get; set; }
        public IEnumerable<string> Product { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
        public double? InspectionFee { get; set; }
        public string Office { get; set; }
        public string Currency { get; set; }
    }
    public class PendingClaimRepoItem
    {
        public int ClaimId { get; set; }
        public string ClaimNo { get; set; }
        public int? InspectionId { get; set; }
        public int? InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public string InvoiceNo { get; set; }
        public string Customer { get; set; }
        public DateTime? ClaimDate { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string ServiceType { get; set; }
        public string Product { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string Office { get; set; }
        public int? OfficeId { get; set; }
        public double? InspectionFee { get; set; }
    }
    public class PendingClaimResponse
    {
        public int CustomerId { get; set; }
        public int? ClaimId { get; set; }
        public int? BookingNo { get; set; }
        public string InvoiceNo { get; set; }
        public string Customer { get; set; }
        public string ClaimDate { get; set; }
        public string ServiceDate { get; set; }
        public string ServiceType { get; set; }
        public string Product { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
        public string Office { get; set; }
    }
    #endregion

    #region Credit Note
    public class CreditNoteSearchRequest
    {
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public string CreditNo { get; set; }
        public int? CreditType { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }
    public class SaveCreditNoteResponse
    {
        public CreditNoteResult Result { get; set; }
    }
    public class SaveCreditNote
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        [Required]
        public int? CreditTypeId { get; set; }
        [Required]
        public string CreditNo { get; set; }
        [Required]
        public DateObject CreditDate { get; set; }
        [Required]
        public DateObject PostDate { get; set; }
        [Required]
        public string BilledTo { get; set; }
        [Required]
        public string BilledAddress { get; set; }
        public IEnumerable<int?> ContactPersons { get; set; }
        [Required]
        public int? CurrencyId { get; set; }
        [Required]
        public string PaymentTerms { get; set; }
        [Required]
        public int? PaymentDuration { get; set; }
        public int? OfficeId { get; set; }
        [Required]
        public int BankId { get; set; }
        public string Subject { get; set; }
        [Required]
        public IEnumerable<SaveCreditNoteItem> SaveCreditNotes { get; set; }
    }
    public class SaveCreditNoteItem
    {
        public int Id { get; set; }
        [Required]
        public int ClaimId { get; set; }
        [Required]
        public int InspectionId { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        [Required]
        public decimal? RefundAmount { get; set; }
        public decimal? SortAmount { get; set; }
        public string Remarks { get; set; }
    }

    public class EditCreditNoteItem : SaveCreditNoteItem
    {
        public string BookingNo { get; set; }
        public string InvoiceNo { get; set; }
        public string ClaimNo { get; set; }
        public string InspectionDate { get; set; }
        public IEnumerable<string> Product { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
        public double? InspectionFee { get; set; }
        public string Office { get; set; }
        public string Currency { get; set; }
    }
    public class CreditNoteSummaryResponse
    {
        public bool IsAccountingCreditNoteRole { get; set; }
        public IEnumerable<CreditNoteSummaryItem> Data { get; set; }
        public CreditNoteResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }

    public class CreditNoteSummaryItem
    {
        public int Id { get; set; }
        public int? CreditId { get; set; }
        public int? CustomerId { get; set; }
        public string CreditNo { get; set; }
        public string CreditType { get; set; }
        public string BillTo { get; set; }
        public string CreditDate { get; set; }
        public string PostDate { get; set; }
        public string InspectionNo { get; set; }
        public decimal? RefundTotal { get; set; }
        public decimal? SortTotal { get; set; }
        public string Currency { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
    public class CreditNoteDetailsRepoItem
    {
        public int? InspectionId { get; set; }
        public int? CreditId { get; set; }
        public int? CustomerId { get; set; }
        public decimal? RefundAmount { get; set; }
        public decimal? SortAmount { get; set; }
    }
    public class CreditNoteSummaryRepoItem
    {
        public int Id { get; set; }
        public string CreditNo { get; set; }
        public int? CreditTypeId { get; set; }
        public DateTime? CreditDate { get; set; }
        public string CreditType { get; set; }
        public string BillTo { get; set; }
        public DateTime? PostDate { get; set; }
        public string Currency { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    public class ExportCreditNoteSummaryRepoItem
    {
        public int Id { get; set; }
        public string CreditNo { get; set; }
        public string ClaimNo { get; set; }
        public string InspectionNo { get; set; }
        public string InvoiceNo { get; set; }
        public int? CreditTypeId { get; set; }
        public DateTime? ServiceToDate { get; set; }
        public DateTime? ServiceFromDate { get; set; }
        public DateTime? CreditDate { get; set; }
        public string CreditType { get; set; }
        public string BillTo { get; set; }
        public DateTime? PostDate { get; set; }
        public double? InspectionFee { get; set; }
        public string InspectionFeeCurrency { get; set; }
        public decimal? RefundAmount { get; set; }
        public decimal? SortAmount { get; set; }
        public string Currency { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string PaymentTerms { get; set; }
        public int? PaymentDuration { get; set; }
        public string Office { get; set; }
        public string Bank { get; set; }
        public string Remark { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
    public class ExportCreditNoteSummary
    {

        [Description("Credit No")]
        public string CreditNo { get; set; }
        [Description("Claim No")]
        public string ClaimNo { get; set; }
        [Description("Booking No")]
        public string InspectionNo { get; set; }
        [Description("Invoice No")]
        public string InvoiceNo { get; set; }
        [Description("Service To Date")]
        public DateTime? ServiceToDate { get; set; }
        [Description("Service From Date")]
        public DateTime? ServiceFromDate { get; set; }
        [Description("Cerdit Date")]
        public DateTime? CreditDate { get; set; }
        [Description("Credit Type")]
        public string CreditType { get; set; }
        [Description("Bill To")]
        public string BillTo { get; set; }
        [Description("Post Date")]
        public DateTime? PostDate { get; set; }
        [Description("Inspection Fee")]
        public double? InspectionFee { get; set; }
        [Description("Inspection Fee Currency")]
        public string InspectionFeeCurrency { get; set; }
        [Description("Refund Amount")]
        public decimal? RefundAmount { get; set; }
        [Description("Sort Amount")]
        public decimal? SortAmount { get; set; }
        [Description("Currency")]
        public string Currency { get; set; }
        [Description("Product Category")]
        public string Category { get; set; }
        [Description("Product Sub Category")]
        public string SubCategory { get; set; }
        [Description("Payment Terms")]
        public string PaymentTerms { get; set; }
        [Description("Payment Duration")]
        public int? PaymentDuration { get; set; }
        public string Office { get; set; }
        public string Bank { get; set; }
        public string Remark { get; set; }
        [Description("Created On")]
        public DateTime? CreatedOn { get; set; }
        [Description("Created By")]
        public string CreatedBy { get; set; }
    }
    public class CreditNoteClaimRepoItem : SaveCreditNoteItem
    {
        public string InvoiceNo { get; set; }
        public string ClaimNo { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string Product { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public double? InspectionFee { get; set; }
        public string Office { get; set; }
        public string Currency { get; set; }
    }
    public class EditCreditNoteResponse
    {
        public SaveCreditNote CreditNote { get; set; }
        public CreditNoteResult Result { get; set; }
        public bool IsAccountingCreditNoteRole { get; set; }
    }

    public class DeleteCreditNoteResponse
    {
        public CreditNoteResult Result { get; set; }
    }

    public class CreditNoteInvoiceDetails
    {
        public int InvoiceId { get; set; }
        public int? InspectionId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoicePostDate { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public double? InspectionFees { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankSwiftCode { get; set; }
        public string BankAddress { get; set; }
    }
    #endregion

    public enum CreditNoteResult
    {
        Success = 1,
        NotFound = 2,
        CreditNoAlreadyExist = 3,
        CreditClaimNotFound = 4,
        SameCustomer = 5
    }
}
