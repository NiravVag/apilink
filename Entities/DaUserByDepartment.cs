using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DA_UserByDepartment")]
    public partial class DaUserByDepartment
    {
        public int Id { get; set; }
        [Column("DAUserCustomerId")]
        public int DauserCustomerId { get; set; }
        public int? DepartmentId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("DaUserByDepartments")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DauserCustomerId")]
        [InverseProperty("DaUserByDepartments")]
        public virtual DaUserCustomer DauserCustomer { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("DaUserByDepartments")]
        public virtual CuDepartment Department { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DaUserByDepartments")]
        public virtual ApEntity Entity { get; set; }
    }
}