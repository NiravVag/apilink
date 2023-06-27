using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_FA_Contact")]
    public partial class AudTranFaContact
    {
        public int Id { get; set; }
        [Column("Contact_Id")]
        public int ContactId { get; set; }
        [Column("Audit_Id")]
        public int AuditId { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranFaContacts")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("ContactId")]
        [InverseProperty("AudTranFaContacts")]
        public virtual SuContact Contact { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTranFaContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudTranFaContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}