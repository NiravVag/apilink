using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Cu_REF_BrandPriority")]
    public partial class CuRefBrandPriority
    {
        public CuRefBrandPriority()
        {
            CuBrandpriorities = new HashSet<CuBrandpriority>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("Brandpriority")]
        public virtual ICollection<CuBrandpriority> CuBrandpriorities { get; set; }
    }
}