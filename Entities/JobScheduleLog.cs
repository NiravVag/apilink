using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("JOB_Schedule_Log")]
    public partial class JobScheduleLog
    {
        public int Id { get; set; }
        [Column("Booking_Id")]
        public int? BookingId { get; set; }
        [Column("Report_Id")]
        public int? ReportId { get; set; }
        [Column("Schedule_Type")]
        public int? ScheduleType { get; set; }
        [StringLength(500)]
        public string FileName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("JobScheduleLogs")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("JobScheduleLogs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ReportId")]
        [InverseProperty("JobScheduleLogs")]
        public virtual FbReportDetail Report { get; set; }
    }
}