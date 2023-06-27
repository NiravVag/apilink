using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_REF_Recipient")]
    public partial class EsRefRecipient
    {
        public EsRefRecipient()
        {
            EsAdditionalRecipients = new HashSet<EsAdditionalRecipient>();
        }

        public int Id { get; set; }
        public int? Sort { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("RecipientNavigation")]
        public virtual ICollection<EsAdditionalRecipient> EsAdditionalRecipients { get; set; }
    }
}