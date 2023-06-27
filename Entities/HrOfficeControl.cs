using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_OfficeControl")]
    public partial class HrOfficeControl
    {
        public int StaffId { get; set; }
        public int LocationId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrOfficeControls")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("HrOfficeControls")]
        public virtual RefLocation Location { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrOfficeControls")]
        public virtual HrStaff Staff { get; set; }
    }
}