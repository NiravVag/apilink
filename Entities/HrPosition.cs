using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Position")]
    public partial class HrPosition
    {
        public HrPosition()
        {
            HrStaffs = new HashSet<HrStaff>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Position_Name")]
        [StringLength(50)]
        public string PositionName { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrPositions")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Position")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
    }
}