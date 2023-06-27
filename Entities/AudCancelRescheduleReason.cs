using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_Cancel_Reschedule_Reasons")]
    public partial class AudCancelRescheduleReason
    {
        public AudCancelRescheduleReason()
        {
            AudTranCancelReschedules = new HashSet<AudTranCancelReschedule>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
        public bool IsCancel { get; set; }
        public bool IsReschedule { get; set; }
        public bool IsDefault { get; set; }
        public bool IsSgT { get; set; }
        [Column("Customer_Id")]
        public int? CustomerId { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("AudCancelRescheduleReasons")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("AudCancelRescheduleReasons")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("ReasonType")]
        public virtual ICollection<AudTranCancelReschedule> AudTranCancelReschedules { get; set; }
    }
}