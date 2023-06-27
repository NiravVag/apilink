using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Modules")]
    public partial class RefModule
    {
        public RefModule()
        {
            DfCuConfigurations = new HashSet<DfCuConfiguration>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool Active { get; set; }

        [InverseProperty("Module")]
        public virtual ICollection<DfCuConfiguration> DfCuConfigurations { get; set; }
    }
}