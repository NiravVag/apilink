using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_REF_RefundType")]
    public partial class ClmRefRefundType
    {
        public ClmRefRefundType()
        {
            ClmTranClaimRefunds = new HashSet<ClmTranClaimRefund>();
            ClmTranCustomerRequestRefunds = new HashSet<ClmTranCustomerRequestRefund>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("RefundType")]
        public virtual ICollection<ClmTranClaimRefund> ClmTranClaimRefunds { get; set; }
        [InverseProperty("RefundType")]
        public virtual ICollection<ClmTranCustomerRequestRefund> ClmTranCustomerRequestRefunds { get; set; }
    }
}