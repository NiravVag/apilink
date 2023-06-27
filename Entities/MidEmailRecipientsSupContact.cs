using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Email_Recipients_SupContacts")]
    public partial class MidEmailRecipientsSupContact
    {
        public int Id { get; set; }
        public int EmailConfigId { get; set; }
        [Column("Sup_ContactId")]
        public int SupContactId { get; set; }
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
        [InverseProperty("MidEmailRecipientsSupContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailRecipientsSupContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EmailConfigId")]
        [InverseProperty("MidEmailRecipientsSupContacts")]
        public virtual MidEmailRecipientsConfiguration EmailConfig { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailRecipientsSupContactModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("SupContactId")]
        [InverseProperty("MidEmailRecipientsSupContacts")]
        public virtual SuContact SupContact { get; set; }
    }
}