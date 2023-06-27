using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DA_UserByRole")]
    public partial class DaUserByRole
    {
        public int Id { get; set; }
        [Column("DAUserCustomerId")]
        public int DauserCustomerId { get; set; }
        public int RoleId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("DaUserByRoles")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DauserCustomerId")]
        [InverseProperty("DaUserByRoles")]
        public virtual DaUserCustomer DauserCustomer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DaUserByRoles")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("DaUserByRoles")]
        public virtual ItRole Role { get; set; }
    }
}