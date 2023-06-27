using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_INSP_CUS_decision")]
    public partial class RefInspCusDecision
    {
        public RefInspCusDecision()
        {
            RefInspCusDecisionConfigs = new HashSet<RefInspCusDecisionConfig>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("CusDec")]
        public virtual ICollection<RefInspCusDecisionConfig> RefInspCusDecisionConfigs { get; set; }
    }
}