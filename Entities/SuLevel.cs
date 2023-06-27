using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Level")]
    public partial class SuLevel
    {
        public SuLevel()
        {
            SuLevelCustoms = new HashSet<SuLevelCustom>();
            SuSuppliers = new HashSet<SuSupplier>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Level { get; set; }

        [InverseProperty("Level")]
        public virtual ICollection<SuLevelCustom> SuLevelCustoms { get; set; }
        [InverseProperty("Level")]
        public virtual ICollection<SuSupplier> SuSuppliers { get; set; }
    }
}