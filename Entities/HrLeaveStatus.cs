using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Leave_Status")]
    public partial class HrLeaveStatus
    {
        public HrLeaveStatus()
        {
            HrLeaves = new HashSet<HrLeave>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Label { get; set; }
        public int? IdTran { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrLeaveStatuses")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("StatusNavigation")]
        public virtual ICollection<HrLeave> HrLeaves { get; set; }
    }
}