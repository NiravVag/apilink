using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EventBookingLog")]
    public partial class EventBookingLog
    {
        public int Id { get; set; }
        [Column("Booking_Id")]
        public int? BookingId { get; set; }
        [Column("Audit_Id")]
        public int? AuditId { get; set; }
        [Column("Status_Id")]
        public int? StatusId { get; set; }
        public string LogInformation { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column("Quotation_Id")]
        public int? QuotationId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EventBookingLogs")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EventBookingLogs")]
        public virtual ApEntity Entity { get; set; }
    }
}