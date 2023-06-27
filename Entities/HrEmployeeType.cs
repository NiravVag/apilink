using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_EMployeeType")]
    public partial class HrEmployeeType
    {
        public HrEmployeeType()
        {
            HrStaffs = new HashSet<HrStaff>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string EmployeeTypeName { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrEmployeeTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("EmployeeType")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
    }
}