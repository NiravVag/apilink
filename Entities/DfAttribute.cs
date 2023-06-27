using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DF_Attributes")]
    public partial class DfAttribute
    {
        public DfAttribute()
        {
            DfControlTypeAttributes = new HashSet<DfControlTypeAttribute>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        public string DataType { get; set; }
        public bool Active { get; set; }

        [InverseProperty("Attribute")]
        public virtual ICollection<DfControlTypeAttribute> DfControlTypeAttributes { get; set; }
    }
}