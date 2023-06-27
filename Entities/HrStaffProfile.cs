using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Staff_Profile")]
    public partial class HrStaffProfile
    {
        [Column("staff_id")]
        public int StaffId { get; set; }
        [Column("profile_id")]
        public int ProfileId { get; set; }

        [ForeignKey("ProfileId")]
        [InverseProperty("HrStaffProfiles")]
        public virtual HrProfile Profile { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffProfiles")]
        public virtual HrStaff Staff { get; set; }
    }
}