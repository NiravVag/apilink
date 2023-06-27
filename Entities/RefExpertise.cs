using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Expertise")]
    public partial class RefExpertise
    {
        public RefExpertise()
        {
            HrStaffExpertises = new HashSet<HrStaffExpertise>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefExpertises")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Expertise")]
        public virtual ICollection<HrStaffExpertise> HrStaffExpertises { get; set; }
    }
}