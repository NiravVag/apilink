using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_AUT_TRAN_Status_Log")]
    public partial class InvAutTranStatusLog
    {
        public int Id { get; set; }
        [Column("Invoice_Id")]
        public int? InvoiceId { get; set; }
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
        [InverseProperty("InvAutTranStatusLogs")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvAutTranStatusLogs")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvAutTranStatusLogs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InvAutTranStatusLogs")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("InvoiceId")]
        [InverseProperty("InvAutTranStatusLogs")]
        public virtual InvAutTranDetail Invoice { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("InvAutTranStatusLogs")]
        public virtual InvStatus Status { get; set; }
    }
}