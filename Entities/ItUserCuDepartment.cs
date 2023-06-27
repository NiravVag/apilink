using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_User_CU_Department")]
    public partial class ItUserCuDepartment
    {
        [Column("User_Id")]
        public int UserId { get; set; }
        [Column("Department_Id")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        [InverseProperty("ItUserCuDepartments")]
        public virtual CuDepartment Department { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ItUserCuDepartments")]
        public virtual ItUserMaster User { get; set; }
    }
}