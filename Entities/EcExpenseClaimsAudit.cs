using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_ExpenseClaimsAudit")]
    public partial class EcExpenseClaimsAudit
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ExpenseClaimDetailId { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("EcExpenseClaimsAudits")]
        public virtual AudTransaction Booking { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("EcExpenseClaimsAuditCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EcExpenseClaimsAuditDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ExpenseClaimDetailId")]
        [InverseProperty("EcExpenseClaimsAudits")]
        public virtual EcExpensesClaimDetai ExpenseClaimDetail { get; set; }
    }
}