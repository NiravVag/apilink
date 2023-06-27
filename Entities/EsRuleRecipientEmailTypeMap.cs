using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_RULE_Recipient_EmailType_Map")]
    public partial class EsRuleRecipientEmailTypeMap
    {
        public int Id { get; set; }
        public int? EmailTypeId { get; set; }
        public int? RecipientTypeId { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("EmailTypeId")]
        [InverseProperty("EsRuleRecipientEmailTypeMaps")]
        public virtual EsType EmailType { get; set; }
        [ForeignKey("RecipientTypeId")]
        [InverseProperty("EsRuleRecipientEmailTypeMaps")]
        public virtual EsRefRecipientType RecipientType { get; set; }
    }
}