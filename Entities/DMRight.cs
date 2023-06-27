using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DM_RIGHT")]
    public partial class DmRight
    {
        public int Id { get; set; }
        public int IdModule { get; set; }
        public int? IdRole { get; set; }
        public int? IdStaff { get; set; }
        public bool EditRight { get; set; }
        public bool DeleteRight { get; set; }
        public bool DownloadRight { get; set; }
        public int? EntityId { get; set; }       
        [Column("DM_RoleId")]
        public int? DmRoleId { get; set; }

        [ForeignKey("DmRoleId")]
        [InverseProperty("DmRights")]
        public virtual DmRole DmRole { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DmRights")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("IdModule")]
        [InverseProperty("DmRights")]
        public virtual DmRefModule IdModuleNavigation { get; set; }
        [ForeignKey("IdRole")]
        [InverseProperty("DmRights")]
        public virtual ItRole IdRoleNavigation { get; set; }
        [ForeignKey("IdStaff")]
        [InverseProperty("DmRights")]
        public virtual HrStaff IdStaffNavigation { get; set; }
    }
}