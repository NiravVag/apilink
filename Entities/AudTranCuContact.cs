using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_CU_Contact")]
    public partial class AudTranCuContact
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
        [InverseProperty("AudTranCuContacts")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("ContactId")]
        [InverseProperty("AudTranCuContacts")]
        public virtual CuContact Contact { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTranCuContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudTranCuContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}