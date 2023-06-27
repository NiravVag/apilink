using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DM_Department")]
    public partial class DmDepartment
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        [Column("DMFileId")]
        public int? DmfileId { get; set; }

        [ForeignKey("DepartmentId")]
        [InverseProperty("DmDepartments")]
        public virtual CuDepartment Department { get; set; }
        [ForeignKey("DmfileId")]
        [InverseProperty("DmDepartments")]
        public virtual DmFile Dmfile { get; set; }
    }
}