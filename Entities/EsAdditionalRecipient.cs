using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_AdditionalRecipients")]
    public partial class EsAdditionalRecipient
    {
        public int Id { get; set; }
        public int? EmailDetailId { get; set; }
        [StringLength(255)]
        public string AdditionalEmail { get; set; }
        public int? Recipient { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsAdditionalRecipients")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EmailDetailId")]
        [InverseProperty("EsAdditionalRecipients")]
        public virtual EsDetail EmailDetail { get; set; }
        [ForeignKey("Recipient")]
        [InverseProperty("EsAdditionalRecipients")]
        public virtual EsRefRecipient RecipientNavigation { get; set; }
    }
}