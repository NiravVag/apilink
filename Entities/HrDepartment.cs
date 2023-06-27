using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Department")]
    public partial class HrDepartment
    {
        public HrDepartment()
        {
            HrStaffs = new HashSet<HrStaff>();
            InverseDeptParent = new HashSet<HrDepartment>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Department_Name")]
        [StringLength(50)]
        public string DepartmentName { get; set; }
        public bool Active { get; set; }
        [Column("Department_Code")]
        [StringLength(50)]
        public string DepartmentCode { get; set; }
        public int? DeptParentId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("DeptParentId")]
        [InverseProperty("InverseDeptParent")]
        public virtual HrDepartment DeptParent { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("HrDepartments")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
        [InverseProperty("DeptParent")]
        public virtual ICollection<HrDepartment> InverseDeptParent { get; set; }
    }
}