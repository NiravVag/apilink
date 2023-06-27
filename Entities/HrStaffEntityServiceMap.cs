using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Staff_Entity_Service_Map")]
    public partial class HrStaffEntityServiceMap
    {
        public int Id { get; set; }
        public int? StaffId { get; set; }
        public int? ServiceId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrStaffEntityServiceMaps")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("HrStaffEntityServiceMaps")]
        public virtual RefService Service { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffEntityServiceMaps")]
        public virtual HrStaff Staff { get; set; }
    }
}