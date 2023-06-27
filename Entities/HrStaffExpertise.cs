using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Staff_Expertise")]
    public partial class HrStaffExpertise
    {
        [Column("staff_id")]
        public int StaffId { get; set; }
        public int ExpertiseId { get; set; }

        [ForeignKey("ExpertiseId")]
        [InverseProperty("HrStaffExpertises")]
        public virtual RefExpertise Expertise { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffExpertises")]
        public virtual HrStaff Staff { get; set; }
    }
}