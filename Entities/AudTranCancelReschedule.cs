using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_Cancel_Reschedule")]
    public partial class AudTranCancelReschedule
    {
        public int Id { get; set; }
        public int ReasonTypeId { get; set; }
        public int? TimeTypeId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TravellingExpense { get; set; }
        public int? CurrencyId { get; set; }
        [StringLength(500)]
        public string Comments { get; set; }
        [StringLength(500)]
        public string InternalComments { get; set; }
        [Column("Audit_Id")]
        public int AuditId { get; set; }
        public int OperationTypeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranCancelReschedules")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTranCancelReschedules")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CurrencyId")]
        [InverseProperty("AudTranCancelReschedules")]
        public virtual RefCurrency Currency { get; set; }
        [ForeignKey("ReasonTypeId")]
        [InverseProperty("AudTranCancelReschedules")]
        public virtual AudCancelRescheduleReason ReasonType { get; set; }
    }
}