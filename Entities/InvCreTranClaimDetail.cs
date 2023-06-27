using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_CRE_TRAN_ClaimDetails")]
    public partial class InvCreTranClaimDetail
    {
        public int Id { get; set; }
        public int? CreditId { get; set; }
        public int? ClaimId { get; set; }
        public int? InvoiceId { get; set; }
        public int? InspectionId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RefundAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SortAmount { get; set; }
        [StringLength(2000)]
        public string Remarks { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("ClaimId")]
        [InverseProperty("InvCreTranClaimDetails")]
        public virtual ClmTransaction Claim { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvCreTranClaimDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CreditId")]
        [InverseProperty("InvCreTranClaimDetails")]
        public virtual InvCreTransaction Credit { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvCreTranClaimDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InvCreTranClaimDetails")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("InvoiceId")]
        [InverseProperty("InvCreTranClaimDetails")]
        public virtual InvAutTranDetail Invoice { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvCreTranClaimDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}