using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_REF_RecipientType")]
    public partial class EsRefRecipientType
    {
        public EsRefRecipientType()
        {
            EsRecipientTypes = new HashSet<EsRecipientType>();
            EsRuleRecipientEmailTypeMaps = new HashSet<EsRuleRecipientEmailTypeMap>();
        }

        public int Id { get; set; }
        [StringLength(300)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column("Created_By")]
        public int? CreatedBy { get; set; }
        [Column("Created_On", TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsRefRecipientTypes")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [InverseProperty("RecipientType")]
        public virtual ICollection<EsRecipientType> EsRecipientTypes { get; set; }
        [InverseProperty("RecipientType")]
        public virtual ICollection<EsRuleRecipientEmailTypeMap> EsRuleRecipientEmailTypeMaps { get; set; }
    }
}