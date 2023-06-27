using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_Transaction")]
    public partial class ClmTransaction
    {
        public ClmTransaction()
        {
            ClmTranAttachments = new HashSet<ClmTranAttachment>();
            ClmTranClaimRefunds = new HashSet<ClmTranClaimRefund>();
            ClmTranCustomerRequestRefunds = new HashSet<ClmTranCustomerRequestRefund>();
            ClmTranCustomerRequests = new HashSet<ClmTranCustomerRequest>();
            ClmTranDefectFamilies = new HashSet<ClmTranDefectFamily>();
            ClmTranDepartments = new HashSet<ClmTranDepartment>();
            ClmTranFinalDecisions = new HashSet<ClmTranFinalDecision>();
            ClmTranReports = new HashSet<ClmTranReport>();
            InvCreTranClaimDetails = new HashSet<InvCreTranClaimDetail>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string ClaimNo { get; set; }
        public int? InspectionNo { get; set; }
        public int? StatusId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ClaimDate { get; set; }
        [StringLength(100)]
        public string RequestedContactName { get; set; }
        public int? ClaimForm { get; set; }
        public int? ReceivedFrom { get; set; }
        public int? ClaimSource { get; set; }
        public string ClaimDescription { get; set; }
        public int? CustomerPriority { get; set; }
        public double? CustomerReqRefundAmount { get; set; }
        public int? CustomerReqRefundCurrency { get; set; }
        public string CustomerComments { get; set; }
        [Column("QCControl_100Goods")]
        public bool? Qccontrol100goods { get; set; }
        public double? DefectPercentage { get; set; }
        public int? NoOfPieces { get; set; }
        [Column("CompareToAQL")]
        public double? CompareToAql { get; set; }
        public int? DefectDistribution { get; set; }
        [StringLength(1000)]
        public string Color { get; set; }
        [StringLength(500)]
        public string DefectCartonInspected { get; set; }
        [Column("FOB_Price")]
        public double? FobPrice { get; set; }
        [Column("FOB_Currency")]
        public int? FobCurrency { get; set; }
        [Column("Retail_Price")]
        public double? RetailPrice { get; set; }
        [Column("Retail_Currency")]
        public int? RetailCurrency { get; set; }
        public int? ClaimValidateResult { get; set; }
        [StringLength(2000)]
        public string ClaimRemarks { get; set; }
        [StringLength(2000)]
        public string ClaimRecommendation { get; set; }
        public double? ClaimRefundAmount { get; set; }
        public int? ClaimRefundCurrency { get; set; }
        [StringLength(2000)]
        public string ClaimRefundRemarks { get; set; }
        public double? RealInspectionFees { get; set; }
        public int? RealInspectionFeesCurrency { get; set; }
        public string AnalyzerFeedback { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AnalyzedOn { get; set; }
        public int? AnalyzedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ValidatedOn { get; set; }
        public int? ValidatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ClosedOn { get; set; }
        public int? ClosedBy { get; set; }

        [ForeignKey("AnalyzedBy")]
        [InverseProperty("ClmTransactionAnalyzedByNavigations")]
        public virtual ItUserMaster AnalyzedByNavigation { get; set; }
        [ForeignKey("ClaimRefundCurrency")]
        [InverseProperty("ClmTransactionClaimRefundCurrencyNavigations")]
        public virtual RefCurrency ClaimRefundCurrencyNavigation { get; set; }
        [ForeignKey("ClaimSource")]
        [InverseProperty("ClmTransactions")]
        public virtual ClmRefSource ClaimSourceNavigation { get; set; }
        [ForeignKey("ClosedBy")]
        [InverseProperty("ClmTransactionClosedByNavigations")]
        public virtual ItUserMaster ClosedByNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ClmTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerPriority")]
        [InverseProperty("ClmTransactions")]
        public virtual ClmRefPriority CustomerPriorityNavigation { get; set; }
        [ForeignKey("CustomerReqRefundCurrency")]
        [InverseProperty("ClmTransactionCustomerReqRefundCurrencyNavigations")]
        public virtual RefCurrency CustomerReqRefundCurrencyNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("ClmTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("ClmTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FobCurrency")]
        [InverseProperty("ClmTransactionFobCurrencyNavigations")]
        public virtual RefCurrency FobCurrencyNavigation { get; set; }
        [ForeignKey("InspectionNo")]
        [InverseProperty("ClmTransactions")]
        public virtual InspTransaction InspectionNoNavigation { get; set; }
        [ForeignKey("RealInspectionFeesCurrency")]
        [InverseProperty("ClmTransactionRealInspectionFeesCurrencyNavigations")]
        public virtual RefCurrency RealInspectionFeesCurrencyNavigation { get; set; }
        [ForeignKey("ReceivedFrom")]
        [InverseProperty("ClmTransactions")]
        public virtual ClmRefReceivedFrom ReceivedFromNavigation { get; set; }
        [ForeignKey("RetailCurrency")]
        [InverseProperty("ClmTransactionRetailCurrencyNavigations")]
        public virtual RefCurrency RetailCurrencyNavigation { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("ClmTransactions")]
        public virtual ClmRefStatus Status { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("ClmTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [ForeignKey("ValidatedBy")]
        [InverseProperty("ClmTransactionValidatedByNavigations")]
        public virtual ItUserMaster ValidatedByNavigation { get; set; }
        [InverseProperty("Claim")]
        public virtual ICollection<ClmTranAttachment> ClmTranAttachments { get; set; }
        [InverseProperty("Claim")]
        public virtual ICollection<ClmTranClaimRefund> ClmTranClaimRefunds { get; set; }
        [InverseProperty("Claim")]
        public virtual ICollection<ClmTranCustomerRequestRefund> ClmTranCustomerRequestRefunds { get; set; }
        [InverseProperty("Claim")]
        public virtual ICollection<ClmTranCustomerRequest> ClmTranCustomerRequests { get; set; }
        [InverseProperty("Claim")]
        public virtual ICollection<ClmTranDefectFamily> ClmTranDefectFamilies { get; set; }
        [InverseProperty("Claim")]
        public virtual ICollection<ClmTranDepartment> ClmTranDepartments { get; set; }
        [InverseProperty("Claim")]
        public virtual ICollection<ClmTranFinalDecision> ClmTranFinalDecisions { get; set; }
        [InverseProperty("Claim")]
        public virtual ICollection<ClmTranReport> ClmTranReports { get; set; }
        [InverseProperty("Claim")]
        public virtual ICollection<InvCreTranClaimDetail> InvCreTranClaimDetails { get; set; }
    }
}