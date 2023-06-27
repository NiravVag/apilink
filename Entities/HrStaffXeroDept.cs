using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Hr_Staff_XeroDept")]
    public partial class HrStaffXeroDept
    {
        public HrStaffXeroDept()
        {
            HrStaffs = new HashSet<HrStaff>();
        }

        public int Id { get; set; }
        [StringLength(250)]
        public string DeptName { get; set; }
        public int? Active { get; set; }

        [InverseProperty("XeroDept")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
    }
}