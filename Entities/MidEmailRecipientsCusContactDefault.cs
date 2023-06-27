using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Email_Recipients_CusContactDefault")]
    public partial class MidEmailRecipientsCusContactDefault
    {
        public int Id { get; set; }
        public int? EmailTypeId { get; set; }
        [Column("Cus_ContactId")]
        public int CusContactId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("MidEmailRecipientsCusContactDefaultCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CusContactId")]
        [InverseProperty("MidEmailRecipientsCusContactDefaults")]
        public virtual CuContact CusContact { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailRecipientsCusContactDefaultDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EmailTypeId")]
        [InverseProperty("MidEmailRecipientsCusContactDefaults")]
        public virtual MidEmailType EmailType { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailRecipientsCusContactDefaultModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
    }
}