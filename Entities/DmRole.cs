using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DM_Role")]
    public partial class DmRole
    {
        public DmRole()
        {
            DmRights = new HashSet<DmRight>();
        }

        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? StaffId { get; set; }
        public bool EditRight { get; set; }
        public bool DownloadRight { get; set; }
        public bool DeleteRight { get; set; }
        public bool UploadRight { get; set; }
        public int? EntityId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public bool? Active { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("DmRoleCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("DmRoleDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DmRoles")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("DmRoles")]
        public virtual ItRole Role { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("DmRoles")]
        public virtual HrStaff Staff { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("DmRoleUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("DmRole")]
        public virtual ICollection<DmRight> DmRights { get; set; }
    }
}