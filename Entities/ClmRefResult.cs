using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_REF_Result")]
    public partial class ClmRefResult
    {
        public ClmRefResult()
        {
            ClmTranFinalDecisions = new HashSet<ClmTranFinalDecision>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }
        public bool? IsValidate { get; set; }
        public bool? IsFinal { get; set; }

        [InverseProperty("FinalDecisionNavigation")]
        public virtual ICollection<ClmTranFinalDecision> ClmTranFinalDecisions { get; set; }
    }
}