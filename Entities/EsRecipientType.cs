using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_Recipient_Type")]
    public partial class EsRecipientType
    {
        public int Id { get; set; }
        [Column("Es_Details_Id")]
        public int? EsDetailsId { get; set; }
        [Column("Recipient_Type_Id")]
        public int? RecipientTypeId { get; set; }
        public bool? IsTo { get; set; }
        [Column("IsCC")]
        public bool? IsCc { get; set; }
        public bool? Active { get; set; }
        [Column("Created_By")]
        public int? CreatedBy { get; set; }
        [Column("Created_On", TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsRecipientTypes")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsRecipientTypes")]
        public virtual EsDetail EsDetails { get; set; }
        [ForeignKey("RecipientTypeId")]
        [InverseProperty("EsRecipientTypes")]
        public virtual EsRefRecipientType RecipientType { get; set; }
    }
}