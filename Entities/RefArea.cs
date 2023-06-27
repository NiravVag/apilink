using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Area")]
    public partial class RefArea
    {
        public RefArea()
        {
            RefCountries = new HashSet<RefCountry>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Area_Name")]
        [StringLength(50)]
        public string AreaName { get; set; }
        public bool Active { get; set; }

        [InverseProperty("Area")]
        public virtual ICollection<RefCountry> RefCountries { get; set; }
    }
}