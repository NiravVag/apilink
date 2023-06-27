using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_LevelPick2")]
    public partial class RefLevelPick2
    {
        public RefLevelPick2()
        {
            CuServiceTypes = new HashSet<CuServiceType>();
        }

        public int Id { get; set; }
        [StringLength(20)]
        public string Value { get; set; }
        public bool Active { get; set; }

        [InverseProperty("LevelPick2Navigation")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
    }
}