using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_Role")]
    public partial class ItRole
    {
        public ItRole()
        {
            ApModuleRoles = new HashSet<ApModuleRole>();
            ApSubModuleRoles = new HashSet<ApSubModuleRole>();
            DaUserByRoles = new HashSet<DaUserByRole>();
            DmRights = new HashSet<DmRight>();
            DmRoles = new HashSet<DmRole>();
            EcStatusRoles = new HashSet<EcStatusRole>();
            ItRoleRights = new HashSet<ItRoleRight>();
            ItUserRoles = new HashSet<ItUserRole>();
            UgRoles = new HashSet<UgRole>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string RoleName { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }
        [Required]
        public bool? PrimaryRole { get; set; }
        public bool SecondaryRole { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("ItRoles")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("IdRoleNavigation")]
        public virtual ICollection<ApModuleRole> ApModuleRoles { get; set; }
        [InverseProperty("IdRoleNavigation")]
        public virtual ICollection<ApSubModuleRole> ApSubModuleRoles { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<DaUserByRole> DaUserByRoles { get; set; }
        [InverseProperty("IdRoleNavigation")]
        public virtual ICollection<DmRight> DmRights { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<DmRole> DmRoles { get; set; }
        [InverseProperty("IdRoleNavigation")]
        public virtual ICollection<EcStatusRole> EcStatusRoles { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<ItRoleRight> ItRoleRights { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<ItUserRole> ItUserRoles { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<UgRole> UgRoles { get; set; }
    }
}