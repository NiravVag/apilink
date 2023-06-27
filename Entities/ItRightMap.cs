using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_Right_Map")]
    public partial class ItRightMap
    {
        public int Id { get; set; }
        public int RightId { get; set; }
        public int RightTypeId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("RightId")]
        [InverseProperty("ItRightMaps")]
        public virtual ItRight Right { get; set; }
        [ForeignKey("RightTypeId")]
        [InverseProperty("ItRightMaps")]
        public virtual ItRightType RightType { get; set; }
    }
}