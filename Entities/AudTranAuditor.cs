using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_Auditors")]
    public partial class AudTranAuditor
    {
        public int Id { get; set; }
        [Column("Audit_Id")]
        public int AuditId { get; set; }
        [Column("Staff_Id")]
        public int StaffId { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedTime { get; set; }
        public int? DeletedBy { get; set; }
        public bool? IsAudited { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranAuditors")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTranAuditorCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudTranAuditorDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("AudTranAuditors")]
        public virtual HrStaff Staff { get; set; }
    }
}