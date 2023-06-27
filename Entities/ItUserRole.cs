using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_UserRole")]
    public partial class ItUserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("ItUserRoles")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("ItUserRoles")]
        public virtual ItRole Role { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ItUserRoles")]
        public virtual ItUserMaster User { get; set; }
    }
}