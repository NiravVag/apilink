using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Booking_RequestLog")]
    public partial class FbBookingRequestLog
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? MissionId { get; set; }
        public int? ReportId { get; set; }
        [StringLength(1000)]
        public string RequestUrl { get; set; }
        public int? MissionProductId { get; set; }
        public string LogInformation { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? AccountId { get; set; }
        public int? EntityId { get; set; }
        public int? ServiceId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("FbBookingRequestLogs")]
        public virtual ApEntity Entity { get; set; }
    }
}