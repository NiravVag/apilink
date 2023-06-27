using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AP_ModuleRole")]
    public partial class ApModuleRole
    {
        public int IdModule { get; set; }
        public int IdRole { get; set; }

        [ForeignKey("IdModule")]
        [InverseProperty("ApModuleRoles")]
        public virtual ApModule IdModuleNavigation { get; set; }
        [ForeignKey("IdRole")]
        [InverseProperty("ApModuleRoles")]
        public virtual ItRole IdRoleNavigation { get; set; }
    }
}