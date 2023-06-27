using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Entity_Map")]
    public partial class HrEntityMap
    {
        public int StaffId { get; set; }
        public int EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrEntityMaps")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrEntityMaps")]
        public virtual HrStaff Staff { get; set; }
    }
}