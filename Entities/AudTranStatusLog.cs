using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_Status_Log")]
    public partial class AudTranStatusLog
    {
        public int Id { get; set; }
        [Column("Status_Id")]
        public int StatusId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        [Column("Audit_Id")]
        public int AuditId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranStatusLogs")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTranStatusLogs")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("AudTranStatusLogs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("AudTranStatusLogs")]
        public virtual AudStatus Status { get; set; }
    }
}