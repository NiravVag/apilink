using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_REF_Email_Size")]
    public partial class EsRefEmailSize
    {
        public EsRefEmailSize()
        {
            EsDetails = new HashSet<EsDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public double? Value { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsRefEmailSizes")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [InverseProperty("EmailSizeNavigation")]
        public virtual ICollection<EsDetail> EsDetails { get; set; }
    }
}