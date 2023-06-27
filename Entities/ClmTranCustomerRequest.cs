using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_TRAN_CustomerRequest")]
    public partial class ClmTranCustomerRequest
    {
        public int Id { get; set; }
        public int? Claimid { get; set; }
        public int? CustomerRequestId { get; set; }
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
        [InverseProperty("ClmTranCustomerRequests")]
        public virtual ClmTransaction Claim { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ClmTranCustomerRequestCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerRequestId")]
        [InverseProperty("ClmTranCustomerRequests")]
        public virtual ClmRefCustomerRequest CustomerRequest { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("ClmTranCustomerRequestDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("ClmTranCustomerRequestUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}