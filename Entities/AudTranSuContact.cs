using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_SU_Contact")]
    public partial class AudTranSuContact
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
        [InverseProperty("AudTranSuContacts")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("ContactId")]
        [InverseProperty("AudTranSuContacts")]
        public virtual SuContact Contact { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTranSuContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudTranSuContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}