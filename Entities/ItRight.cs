using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_Right")]
    public partial class ItRight
    {
        public ItRight()
        {
            InverseParent = new HashSet<ItRight>();
            ItRightEntities = new HashSet<ItRightEntity>();
            ItRightMaps = new HashSet<ItRightMap>();
            ItRoleRights = new HashSet<ItRoleRight>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        [StringLength(500)]
        public string TitleName { get; set; }
        [StringLength(500)]
        public string MenuName { get; set; }
        [StringLength(500)]
        public string Path { get; set; }
        public bool? IsHeading { get; set; }
        public bool? Active { get; set; }
        [StringLength(500)]
        public string Glyphicons { get; set; }
        public int? Ranking { get; set; }
        [Column("MenuName_IdTran")]
        public int? MenuNameIdTran { get; set; }
        [Column("TitleName_IdTran")]
        public int? TitleNameIdTran { get; set; }
        public int? EntityId { get; set; }
        [Required]
        public bool? ShowMenu { get; set; }
        public int? RightType { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("ItRights")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("InverseParent")]
        public virtual ItRight Parent { get; set; }
        [ForeignKey("RightType")]
        [InverseProperty("ItRights")]
        public virtual ItRightType RightTypeNavigation { get; set; }
        [InverseProperty("Parent")]
        public virtual ICollection<ItRight> InverseParent { get; set; }
        [InverseProperty("Right")]
        public virtual ICollection<ItRightEntity> ItRightEntities { get; set; }
        [InverseProperty("Right")]
        public virtual ICollection<ItRightMap> ItRightMaps { get; set; }
        [InverseProperty("Right")]
        public virtual ICollection<ItRoleRight> ItRoleRights { get; set; }
    }
}