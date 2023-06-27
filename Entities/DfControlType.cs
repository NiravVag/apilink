using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DF_ControlTypes")]
    public partial class DfControlType
    {
        public DfControlType()
        {
            DfControlTypeAttributes = new HashSet<DfControlTypeAttribute>();
            DfCuConfigurations = new HashSet<DfCuConfiguration>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool Active { get; set; }

        [InverseProperty("ControlType")]
        public virtual ICollection<DfControlTypeAttribute> DfControlTypeAttributes { get; set; }
        [InverseProperty("ControlType")]
        public virtual ICollection<DfCuConfiguration> DfCuConfigurations { get; set; }
    }
}