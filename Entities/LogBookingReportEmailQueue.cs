using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("LOG_Booking_Report_Email_Queue")]
    public partial class LogBookingReportEmailQueue
    {
        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int? InspectionId { get; set; }
        [Column("Report_Id")]
        public int? ReportId { get; set; }
        [Column("Audit_Id")]
        public int? AuditId { get; set; }
        [Column("Es_Type_Id")]
        public int? EsTypeId { get; set; }
        [Column("Email_Log_Id")]
        public int? EmailLogId { get; set; }
        public int? EntityId { get; set; }
        public int? ReportRevision { get; set; }
        public int? ReportVersion { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("LogBookingReportEmailQueues")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("EmailLogId")]
        [InverseProperty("LogBookingReportEmailQueues")]
        public virtual LogEmailQueue EmailLog { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("LogBookingReportEmailQueues")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("EsTypeId")]
        [InverseProperty("LogBookingReportEmailQueues")]
        public virtual EsType EsType { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("LogBookingReportEmailQueues")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("ReportId")]
        [InverseProperty("LogBookingReportEmailQueues")]
        public virtual FbReportDetail Report { get; set; }
    }
}