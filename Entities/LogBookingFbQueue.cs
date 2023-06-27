using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("LOG_Booking_FB_Queue")]
    public partial class LogBookingFbQueue
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int FbBookingSyncType { get; set; }
        public int TryCount { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public bool? IsMissionUpdated { get; set; }
        public int Status { get; set; }
        public int EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("LogBookingFbQueues")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("LogBookingFbQueues")]
        public virtual ApEntity Entity { get; set; }
    }
}