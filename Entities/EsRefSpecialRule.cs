using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_REF_Special_Rule")]
    public partial class EsRefSpecialRule
    {
        public EsRefSpecialRule()
        {
            EsSpecialRules = new HashSet<EsSpecialRule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsRefSpecialRules")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EsRefSpecialRules")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("SpecialRule")]
        public virtual ICollection<EsSpecialRule> EsSpecialRules { get; set; }
    }
}