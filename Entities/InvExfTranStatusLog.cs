using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_EXF_TRAN_Status_Log")]
    public partial class InvExfTranStatusLog
    {
        public int Id { get; set; }
        [Column("ExtraFee_Id")]
        public int? ExtraFeeId { get; set; }
        [Column("Inspection_Id")]
        public int? InspectionId { get; set; }
        [Column("Audit_Id")]
        public int? AuditId { get; set; }
        [Column("Status_Id")]
        public int? StatusId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("InvExfTranStatusLogs")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvExfTranStatusLogs")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvExfTranStatusLogs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ExtraFeeId")]
        [InverseProperty("InvExfTranStatusLogs")]
        public virtual InvExfTransaction ExtraFee { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InvExfTranStatusLogs")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("InvExfTranStatusLogs")]
        public virtual InvExfStatus Status { get; set; }
    }
}