using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_TRAN_ClaimRefund")]
    public partial class ClmTranClaimRefund
    {
        public int Id { get; set; }
        public int? Claimid { get; set; }
        public int? RefundTypeId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("Claimid")]
        [InverseProperty("ClmTranClaimRefunds")]
        public virtual ClmTransaction Claim { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ClmTranClaimRefundCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("ClmTranClaimRefundDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("RefundTypeId")]
        [InverseProperty("ClmTranClaimRefunds")]
        public virtual ClmRefRefundType RefundType { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("ClmTranClaimRefundUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}