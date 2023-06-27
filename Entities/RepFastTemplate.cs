using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REP_FAST_Template")]
    public partial class RepFastTemplate
    {
        public RepFastTemplate()
        {
            RepFastTemplateConfigs = new HashSet<RepFastTemplateConfig>();
        }

        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }
        public int? EntityId { get; set; }

        [InverseProperty("Template")]
        public virtual ICollection<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
    }
}