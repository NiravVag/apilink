using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DM_REF_MODULE")]
    public partial class DmRefModule
    {
        public DmRefModule()
        {
            DmDetails = new HashSet<DmDetail>();
            DmRights = new HashSet<DmRight>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        [StringLength(500)]
        public string ModuleName { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int? Ranking { get; set; }
        public bool NeedCustomer { get; set; }
        [Column("DM_Level")]
        public int? DmLevel { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("DmRefModules")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Module")]
        public virtual ICollection<DmDetail> DmDetails { get; set; }
        [InverseProperty("IdModuleNavigation")]
        public virtual ICollection<DmRight> DmRights { get; set; }
    }
}