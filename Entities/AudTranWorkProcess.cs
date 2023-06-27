using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_WorkProcess")]
    public partial class AudTranWorkProcess
    {
        public int Id { get; set; }
        [Column("Audit_Id")]
        public int AuditId { get; set; }
        [Column("WorkProcess_Id")]
        public int WorkProcessId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public bool Active { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranWorkProcesses")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTranWorkProcessCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudTranWorkProcessDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("WorkProcessId")]
        [InverseProperty("AudTranWorkProcesses")]
        public virtual AudWorkProcess WorkProcess { get; set; }
    }
}