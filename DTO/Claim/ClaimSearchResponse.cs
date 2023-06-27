using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DTO.Claim
{
    public class ClaimSearchResponse
    {
        public IEnumerable<ClaimItem> Data { get; set; }

        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<ClaimsStatus> ClaimStatuslst { get; set; }

        public ClaimSearchResponseResult Result { get; set; }
    }

    public class ClaimItem
    {
        public InspTransaction InspectionTransaction { get; set; }
        public ClmTranFinalDecision ClmTranFinalDecisions { get; set; }
        public int ClaimId { get; set; }
        public string ClaimNo { get; set; }
        public string ClaimDate { get; set; }
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string InspectionDate { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string ServiceDate { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int? StatusId { get; set; }
        public string Office { get; set; }
        public int OfficeId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string FinalDecision { get; set; }
    }
    public enum ClaimSearchResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }

    public class ClaimCancelResponse
    {
        public int Id { get; set; }

        public ClaimCancelResult Result { get; set; }
    }
    public enum ClaimCancelResult
    {
        Success = 1,
        NotFound = 2
    }

    public class ClaimSearchExportResponse
    {
        public List<ClaimExportItem> Data { get; set; }

        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public ClaimSearchResponseResult Result { get; set; }
    }
    public class ClaimExportItem
    {
        public int ClaimId { get; set; }
        public string ClaimNo { get; set; }
        public int? Inspection { get; set; }
        public IEnumerable<string> Report { get; set; }
        public DateTime? ClaimDate { get; set; }
        public string CustomerName { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string ClaimReceivedFrom { get; set; }
        public string SupplierName { get; set; }
        public string InspectionDate { get; set; }
        public string StatusName { get; set; }
        public string Office { get; set; }
        public string OfficeCountry { get; set; }
        public string InspectionCountry { get; set; }
        public string Factory { get; set; }
        public IEnumerable<string> FinalDecision { get; set; }
        public string ClaimSource { get; set; }
        public IEnumerable<string> DefectFamily { get; set; }
        public IEnumerable<string> Department { get; set; }
        public IEnumerable<string> CustomerRequestRefund { get; set; }
        public string ClaimDescription { get; set; }
        public IEnumerable<string> CustomerRequestType { get; set; }
        public string CustomerRequestPriority { get; set; }
        public double? Amount { get; set; }
        public string CustomerReqRefundCurrency { get; set; }
        public string Remarks { get; set; }
        public string InspectionDetails { get; set; }
        public IEnumerable<string> InspectorName { get; set; }
        public IEnumerable<string> ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string Color { get; set; }
        public double? OrderQty { get; set; }
        public double? InspectedQty { get; set; }
        public double? FobPrice { get; set; }
        public string FobCurrency { get; set; }
        public double? RetailPrice { get; set; }
        public string RetailCurrency { get; set; }
        public string AnalyzerFeedback { get; set; }
        public int? ClaimValidateResult { get; set; }
        public string ValidatorClaimResult { get; set; }
        public string ClaimRecommendation { get; set; }
        public string ValidatorReviewComment { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double? InvoiceAmount { get; set; }
        public string InvoiceCurrency { get; set; }
        public double? RealInspectionFees { get; set; }
        public string RealInspectionFeesCurrency { get; set; }
        public IEnumerable<string> FinalRefundType { get; set; }
        public double? FinalAmount { get; set; }
        public string FinalCurrency { get; set; }
        public string FinalDecisionRemarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string AnalyzedBy { get; set; }
        public DateTime? AnalyzedDate { get; set; }
        public string ValidatedBy { get; set; }
        public DateTime? ValidatedDate { get; set; }
        public string ClosedBy { get; set; }
        public DateTime? ClosedDate { get; set; }
        public InspTransaction InspectionTransaction { get; set; }
        public ICollection<ClmTranCustomerRequestRefund> ClmTranCustomerRequestRefunds { get; set; }
        public ICollection<ClmTranCustomerRequest> ClmTranCustomerRequests { get; set; }
        public ICollection<ClmTranDefectFamily> ClmTranDefectFamilies { get; set; }
        public ICollection<ClmTranDepartment> ClmTranDepartments { get; set; }
        public ICollection<ClmTranFinalDecision> ClmTranFinalDecisions { get; set; }
        public ICollection<ClmTranReport> ClmTranReports { get; set; }
        public ICollection<ClmRefResult> ClmTranFinalDecisionName { get; set; }
        public ICollection<ClmTranAttachment> ClmTranAttachments { get; set; }
        public ICollection<ClmTranClaimRefund> ClmTranClaimRefunds { get; set; }
    }
    public class ClaimItemExportSummary
    {
        [Description("Claim#")]
        public string ClaimNo { get; set; }
        [Description("Report#")]
        public string Report { get; set; }
        [Description("Inspection#")]
        public int Inspection { get; set; }
        [Description("Customer")]
        public string CustomerName { get; set; }
        [Description("Claim Date")]
        public string ClaimDate { get; set; }
        [Description("Claim Status")]
        public string ClaimStatus { get; set; }
        [Description("Claim Received From")]
        public string ClaimReceivedFrom { get; set; }
        [Description("Claim Source")]
        public string ClaimSource { get; set; }
        [Description("Defect Family")]
        public string DefectFamily { get; set; }

        [Description("Department")]
        public string Department { get; set; }
        [Description("Claim Description")]
        public string ClaimDescription { get; set; }
        [Description("Customer Request Type")]
        public string CustomerRequestType { get; set; }
        [Description("Customer Request Priority")]
        public string CustomerRequestPriority { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Inspection Date")]
        public string InspectionDate { get; set; }
        [Description("Office")]
        public string Office { get; set; }
        [Description("Office Country")]
        public string OfficeCountry { get; set; }
        [Description("Inspection Country")]
        public string InspectionCountry { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }
        [Description("Customer Request Refund Type")]
        public string CustomerRequestRefundType { get; set; }
        [Description("Customer Request Refund Amount")]
        public double? Amount { get; set; }
        [Description("Customer Request Refund Currency")]
        public string CustomerReqRefundCurrency { get; set; }
        public string Remarks { get; set; }
        [Description("Inspector Name")]
        public string InspectorName { get; set; }
        [Description("Product Name")]
        public string ProductName { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Sub Category")]
        public string ProductSubCategory { get; set; }
        [Description("Color")]
        public string Color { get; set; }
        [Description("Order Qty")]
        public double? OrderQty { get; set; }
        [Description("Inspected Qty")]
        public double? InspectedQty { get; set; }
        [Description("Fob Value")]
        public double? FobValue { get; set; }
        [Description("Fob Currency")]
        public string FobCurrency { get; set; }
        [Description("Retail Value")]
        public double? RetailValue { get; set; }
        [Description("Retail Currency")]
        public string RetailCurrency { get; set; }
        [Description("Analyzer Feedback")]
        public string AnalyzerFeedback { get; set; }
        [Description("Invoice Number")]
        public string InvoiceNumber { get; set; }
        [Description("Invoice Date")]
        public string InvoiceDate { get; set; }
        [Description("Invoice Amount")]
        public double? InvoiceAmount { get; set; }
        [Description("Invoice Currency")]
        public string InvoiceCurrency { get; set; }
        [Description("Real Inspection Fees")]
        public double? RealInspectionFees { get; set; }
        [Description("Real Inspection Fees Currency")]
        public string RealInspectionFeesCurrency { get; set; }
        [Description("Validator Claim Result")]
        public string ValidatorClaimResult { get; set; }
        [Description("Validator Recommendation")]
        public string ValidatorRecommendation { get; set; }
        [Description("Validator Review Comment")]
        public string ValidatorReviewComment { get; set; }
        [Description("Final Decision")]
        public string FinalDecision { get; set; }
        [Description("Final Refund Type")]
        public string FinalRefundType { get; set; }
        [Description("Refund Amount")]
        public double? RefundAmount { get; set; }
        [Description("Refund Currency")]
        public string RefundCurrency { get; set; }
        [Description("Final Decision Remarks")]
        public string FinalDecisionRemarks { get; set; }
        [Description("Created By")]
        public string CreatedBy { get; set; }
        [Description("Created Date")]
        public string CreatedDate { get; set; }
        [Description("Analyzed By")]
        public string AnalyzedBy { get; set; }
        [Description("Analyzed Date")]
        public string AnalyzedDate { get; set; }
        [Description("Validated By")]
        public string ValidatedBy { get; set; }
        [Description("Validated Date")]
        public string ValidatedDate { get; set; }
        [Description("Closed By")]
        public string ClosedBy { get; set; }
        [Description("Closed Date")]
        public string ClosedDate { get; set; }
    }

    public class InspectionQcDetail
    {
        public int ClaimId { get; set; }

        public int BookingId { get; set; }
        public int QcId { get; set; }
        public string Name { get; set; }
    }

    public class InspectionInvoiceDetail
    {
        public int ClaimId { get; set; }

        public int BookingId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public DateTime? BilledDate { get; set; }
        public double? TotalInvoiceFees { get; set; }
        public int? InvoiceCurrency { get; set; }
        public string InvoiceCurrencyName { get; set; }
        public RefCurrency InvoiceCurrencyNavigation { get; set; }
    }
}
