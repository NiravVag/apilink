using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Zone")]
    public partial class RefZone
    {
        public RefZone()
        {
            RefCityDetails = new HashSet<RefCityDetail>();
            RefCounties = new HashSet<RefCounty>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? LocationId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefZones")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("RefZones")]
        public virtual RefLocation Location { get; set; }
        [InverseProperty("Zone")]
        public virtual ICollection<RefCityDetail> RefCityDetails { get; set; }
        [InverseProperty("Zone")]
        public virtual ICollection<RefCounty> RefCounties { get; set; }
    }
}