using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Email_Modules")]
    public partial class MidEmailModule
    {
        public MidEmailModule()
        {
            MidEmailTypes = new HashSet<MidEmailType>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("MidEmailModuleCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailModuleDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("MidEmailModules")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailModuleModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [InverseProperty("Module")]
        public virtual ICollection<MidEmailType> MidEmailTypes { get; set; }
    }
}