using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_Status_Role")]
    public partial class EcStatusRole
    {
        public int IdRole { get; set; }
        public int IdStatus { get; set; }

        [ForeignKey("IdRole")]
        [InverseProperty("EcStatusRoles")]
        public virtual ItRole IdRoleNavigation { get; set; }
        [ForeignKey("IdStatus")]
        [InverseProperty("EcStatusRoles")]
        public virtual EcExpClaimStatus IdStatusNavigation { get; set; }
    }
}