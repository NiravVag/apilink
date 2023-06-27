using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_ExpenseClaimsInspection")]
    public partial class EcExpenseClaimsInspection
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
        [InverseProperty("EcExpenseClaimsInspections")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("EcExpenseClaimsInspectionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EcExpenseClaimsInspectionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ExpenseClaimDetailId")]
        [InverseProperty("EcExpenseClaimsInspections")]
        public virtual EcExpensesClaimDetai ExpenseClaimDetail { get; set; }
    }
}