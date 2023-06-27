using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_Role_Right")]
    public partial class ItRoleRight
    {
        public int RoleId { get; set; }
        public int RightId { get; set; }

        [ForeignKey("RightId")]
        [InverseProperty("ItRoleRights")]
        public virtual ItRight Right { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("ItRoleRights")]
        public virtual ItRole Role { get; set; }
    }
}