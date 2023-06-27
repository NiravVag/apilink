using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CheckPointType")]
    public partial class CuCheckPointType
    {
        public CuCheckPointType()
        {
            CuCheckPoints = new HashSet<CuCheckPoint>();
        }

        public int Id { get; set; }
        [StringLength(1500)]
        public string Name { get; set; }
        public bool Active { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("CuCheckPointTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("CheckpointType")]
        public virtual ICollection<CuCheckPoint> CuCheckPoints { get; set; }
    }
}