using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Leave_Type")]
    public partial class HrLeaveType
    {
        public HrLeaveType()
        {
            HrLeaves = new HashSet<HrLeave>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        public bool Active { get; set; }
        [Column("Total_Days")]
        public int? TotalDays { get; set; }
        public int? IdTran { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrLeaveTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("LeaveType")]
        public virtual ICollection<HrLeave> HrLeaves { get; set; }
    }
}