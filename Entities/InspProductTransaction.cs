using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_Product_Transaction")]
    public partial class InspProductTransaction
    {
        public InspProductTransaction()
        {
            FbReportRdnumbers = new HashSet<FbReportRdnumber>();
            InspPurchaseOrderColorTransactions = new HashSet<InspPurchaseOrderColorTransaction>();
            InspPurchaseOrderTransactions = new HashSet<InspPurchaseOrderTransaction>();
            QuInspProducts = new HashSet<QuInspProduct>();
        }

        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        [Column("Product_Id")]
        public int ProductId { get; set; }
        [Column("AQL")]
        public int? Aql { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int Unit { get; set; }
        public int? UnitCount { get; set; }
        public int TotalBookingQuantity { get; set; }
        [Column("Combine_Product_Id")]
        public int? CombineProductId { get; set; }
        [Column("AQL_Quantity")]
        public int? AqlQuantity { get; set; }
        [Column("Combine_AQL_Quantity")]
        public int? CombineAqlQuantity { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public string Remarks { get; set; }
        public bool? Active { get; set; }
        [Column("Fb_Report_Id")]
        public int? FbReportId { get; set; }
        public bool? IsEcopack { get; set; }
        [Column("FBTemplateId")]
        public int? FbtemplateId { get; set; }
        public int? SampleType { get; set; }
        public bool? IsDisplayMaster { get; set; }
        [Column("Parent_Product_Id")]
        public int? ParentProductId { get; set; }
        public int? BookingFormSerial { get; set; }
        public int? EntityId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AsReceivedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TfReceivedDate { get; set; }
        public bool? IsGoldenSampleAvailable { get; set; }
        [StringLength(500)]
        public string GoldenSampleComments { get; set; }
        public bool? IsSampleCollection { get; set; }
        [StringLength(500)]
        public string SampleCollectionComments { get; set; }
        public int? ProductionStatus { get; set; }
        public int? PackingStatus { get; set; }

        [ForeignKey("Aql")]
        [InverseProperty("InspProductTransactions")]
        public virtual RefLevelPick1 AqlNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspProductTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("Critical")]
        [InverseProperty("InspProductTransactionCriticalNavigations")]
        public virtual RefPick1 CriticalNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspProductTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspProductTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FbReportId")]
        [InverseProperty("InspProductTransactions")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("FbtemplateId")]
        [InverseProperty("InspProductTransactions")]
        public virtual FbReportTemplate Fbtemplate { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspProductTransactions")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("Major")]
        [InverseProperty("InspProductTransactionMajorNavigations")]
        public virtual RefPick1 MajorNavigation { get; set; }
        [ForeignKey("Minor")]
        [InverseProperty("InspProductTransactionMinorNavigations")]
        public virtual RefPick1 MinorNavigation { get; set; }
        [ForeignKey("PackingStatus")]
        [InverseProperty("InspProductTransactions")]
        public virtual InspRefPackingStatus PackingStatusNavigation { get; set; }
        [ForeignKey("ParentProductId")]
        [InverseProperty("InspProductTransactionParentProducts")]
        public virtual CuProduct ParentProduct { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("InspProductTransactionProducts")]
        public virtual CuProduct Product { get; set; }
        [ForeignKey("ProductionStatus")]
        [InverseProperty("InspProductTransactions")]
        public virtual InspRefProductionStatus ProductionStatusNavigation { get; set; }
        [ForeignKey("SampleType")]
        [InverseProperty("InspProductTransactions")]
        public virtual RefSampleType SampleTypeNavigation { get; set; }
        [ForeignKey("Unit")]
        [InverseProperty("InspProductTransactions")]
        public virtual RefUnit UnitNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspProductTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportRdnumber> FbReportRdnumbers { get; set; }
        [InverseProperty("ProductRef")]
        public virtual ICollection<InspPurchaseOrderColorTransaction> InspPurchaseOrderColorTransactions { get; set; }
        [InverseProperty("ProductRef")]
        public virtual ICollection<InspPurchaseOrderTransaction> InspPurchaseOrderTransactions { get; set; }
        [InverseProperty("ProductTran")]
        public virtual ICollection<QuInspProduct> QuInspProducts { get; set; }
    }
}