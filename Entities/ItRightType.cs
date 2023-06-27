using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_Right_Type")]
    public partial class ItRightType
    {
        public ItRightType()
        {
            ItRightMaps = new HashSet<ItRightMap>();
            ItRights = new HashSet<ItRight>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }
        public int? Service { get; set; }

        [ForeignKey("Service")]
        [InverseProperty("ItRightTypes")]
        public virtual RefService ServiceNavigation { get; set; }
        [InverseProperty("RightType")]
        public virtual ICollection<ItRightMap> ItRightMaps { get; set; }
        [InverseProperty("RightTypeNavigation")]
        public virtual ICollection<ItRight> ItRights { get; set; }
    }
}