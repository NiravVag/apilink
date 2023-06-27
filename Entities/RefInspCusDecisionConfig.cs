using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_INSP_CUS_Decision_Config")]
    public partial class RefInspCusDecisionConfig
    {
        public RefInspCusDecisionConfig()
        {
            EsResultConfigs = new HashSet<EsResultConfig>();
            InspRepCusDecisions = new HashSet<InspRepCusDecision>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string CustomDecisionName { get; set; }
        public int? CustomerId { get; set; }
        public bool? Active { get; set; }
        public bool? Default { get; set; }
        public int CusDecId { get; set; }

        [ForeignKey("CusDecId")]
        [InverseProperty("RefInspCusDecisionConfigs")]
        public virtual RefInspCusDecision CusDec { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("RefInspCusDecisionConfigs")]
        public virtual CuCustomer Customer { get; set; }
        [InverseProperty("CustomerResult")]
        public virtual ICollection<EsResultConfig> EsResultConfigs { get; set; }
        [InverseProperty("CustomerResult")]
        public virtual ICollection<InspRepCusDecision> InspRepCusDecisions { get; set; }
    }
}