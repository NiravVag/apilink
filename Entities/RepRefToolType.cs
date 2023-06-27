using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REP_REF_ToolType")]
    public partial class RepRefToolType
    {
        public RepRefToolType()
        {
            RepFastTemplateConfigs = new HashSet<RepFastTemplateConfig>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string ToolName { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("ReportToolType")]
        public virtual ICollection<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
    }
}