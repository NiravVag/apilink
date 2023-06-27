using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_Hold_Reasons")]
    public partial class InspRefHoldReason
    {
        public InspRefHoldReason()
        {
            InspTranHoldReasons = new HashSet<InspTranHoldReason>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string Reason { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }
        public int? EntityId { get; set; }

        [InverseProperty("ReasonTypeNavigation")]
        public virtual ICollection<InspTranHoldReason> InspTranHoldReasons { get; set; }
    }
}