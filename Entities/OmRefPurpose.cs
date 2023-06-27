using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("OM_REF_Purpose")]
    public partial class OmRefPurpose
    {
        public OmRefPurpose()
        {
            OmDetails = new HashSet<OmDetail>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("Purpose")]
        public virtual ICollection<OmDetail> OmDetails { get; set; }
    }
}