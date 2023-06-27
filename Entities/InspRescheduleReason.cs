using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_Reschedule_Reasons")]
    public partial class InspRescheduleReason
    {
        public InspRescheduleReason()
        {
            InspTranReschedules = new HashSet<InspTranReschedule>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
        public bool IsDefault { get; set; }
        [Column("IsAPI")]
        public bool IsApi { get; set; }
        [Column("Customer_Id")]
        public int? CustomerId { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("InspRescheduleReasons")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspRescheduleReasons")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("ReasonType")]
        public virtual ICollection<InspTranReschedule> InspTranReschedules { get; set; }
    }
}