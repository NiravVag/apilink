using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("LOG_Email_Queue")]
    public partial class LogEmailQueue
    {
        public LogEmailQueue()
        {
            LogBookingReportEmailQueues = new HashSet<LogBookingReportEmailQueue>();
            LogEmailQueueAttachments = new HashSet<LogEmailQueueAttachment>();
        }

        public int Id { get; set; }
        [StringLength(2000)]
        public string Subject { get; set; }
        public string Body { get; set; }
        public int? SourceId { get; set; }
        [StringLength(200)]
        public string SourceName { get; set; }
        public string ToList { get; set; }
        [Column("CCList")]
        [StringLength(2000)]
        public string Cclist { get; set; }
        [Column("BCCList")]
        [StringLength(2000)]
        public string Bcclist { get; set; }
        public int? Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SendOn { get; set; }
        public int? TryCount { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("LogEmailQueues")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("LogEmailQueues")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("EmailLog")]
        public virtual ICollection<LogBookingReportEmailQueue> LogBookingReportEmailQueues { get; set; }
        [InverseProperty("EmailQueue")]
        public virtual ICollection<LogEmailQueueAttachment> LogEmailQueueAttachments { get; set; }
    }
}