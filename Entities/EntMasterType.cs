using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ENT_Master_Type")]
    public partial class EntMasterType
    {
        public EntMasterType()
        {
            EntMasterConfigs = new HashSet<EntMasterConfig>();
        }

        public int Id { get; set; }
        [StringLength(300)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("TypeNavigation")]
        public virtual ICollection<EntMasterConfig> EntMasterConfigs { get; set; }
    }
}