using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_PO_Transaction")]
    public partial class InspPoTransaction
    {
        public InspPoTransaction()
        {
            QuQuotationPos = new HashSet<QuQuotationPo>();
        }

        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        [Column("AQL")]
        public int? Aql { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int Unit { get; set; }
        public int? UnitCount { get; set; }
        public int BookingQuantity { get; set; }
        public int? PickingQuantity { get; set; }
        [StringLength(1500)]
        public string Remarks { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column("Fb_Report_Id")]
        public int? FbReportId { get; set; }
        [Column("Combine_Product_Id")]
        public int? CombineProductId { get; set; }
        [Column("AQL_Quantity")]
        public int? AqlQuantity { get; set; }
        [Column("Combine_AQL_Quantity")]
        public int? CombineAqlQuantity { get; set; }
        [Column("PO_DetailId")]
        public int PoDetailId { get; set; }
        [Column("Fb_Mission_Product_Id")]
        public int? FbMissionProductId { get; set; }

        [ForeignKey("Aql")]
        [InverseProperty("InspPoTransactions")]
        public virtual RefLevelPick1 AqlNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspPoTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("Critical")]
        [InverseProperty("InspPoTransactionCriticalNavigations")]
        public virtual RefPick1 CriticalNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspPoTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FbReportId")]
        [InverseProperty("InspPoTransactions")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspPoTransactions")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("Major")]
        [InverseProperty("InspPoTransactionMajorNavigations")]
        public virtual RefPick1 MajorNavigation { get; set; }
        [ForeignKey("Minor")]
        [InverseProperty("InspPoTransactionMinorNavigations")]
        public virtual RefPick1 MinorNavigation { get; set; }
        [ForeignKey("PoDetailId")]
        [InverseProperty("InspPoTransactions")]
        public virtual CuPurchaseOrderDetail PoDetail { get; set; }
        [ForeignKey("Unit")]
        [InverseProperty("InspPoTransactions")]
        public virtual RefUnit UnitNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspPoTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Po")]
        public virtual ICollection<QuQuotationPo> QuQuotationPos { get; set; }
    }
}