using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_LocationType")]
    public partial class RefLocationType
    {
        public RefLocationType()
        {
            RefLocations = new HashSet<RefLocation>();
        }

        public int Id { get; set; }
        [Required]
        [Column("SGT_Location_Type")]
        [StringLength(50)]
        public string SgtLocationType { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefLocationTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("LocationType")]
        public virtual ICollection<RefLocation> RefLocations { get; set; }
    }
}