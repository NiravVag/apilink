using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_TRAN_CustomerRequestRefund")]
    public partial class ClmTranCustomerRequestRefund
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
        [InverseProperty("ClmTranCustomerRequestRefunds")]
        public virtual ClmTransaction Claim { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ClmTranCustomerRequestRefundCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("ClmTranCustomerRequestRefundDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("RefundTypeId")]
        [InverseProperty("ClmTranCustomerRequestRefunds")]
        public virtual ClmRefRefundType RefundType { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("ClmTranCustomerRequestRefundUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}