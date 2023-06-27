using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REP_CUS_Decision_Template")]
    public partial class InspRepCusDecisionTemplate
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? EntityId { get; set; }
        [StringLength(500)]
        public string TemplatePath { get; set; }
        public bool? IsDefault { get; set; }
        public int? ServiceTypeId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("InspRepCusDecisionTemplates")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspRepCusDecisionTemplates")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ServiceTypeId")]
        [InverseProperty("InspRepCusDecisionTemplates")]
        public virtual RefServiceType ServiceType { get; set; }
    }
}