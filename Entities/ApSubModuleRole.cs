using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AP_SubModuleRole")]
    public partial class ApSubModuleRole
    {
        public int IdSubModule { get; set; }
        public int IdRole { get; set; }

        [ForeignKey("IdRole")]
        [InverseProperty("ApSubModuleRoles")]
        public virtual ItRole IdRoleNavigation { get; set; }
        [ForeignKey("IdSubModule")]
        [InverseProperty("ApSubModuleRoles")]
        public virtual ApSubModule IdSubModuleNavigation { get; set; }
    }
}