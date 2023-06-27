using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("LOG_Email_Queue_Attachments")]
    public partial class LogEmailQueueAttachment
    {
        public int Id { get; set; }
        public Guid GuidId { get; set; }
        [Column("Email_Queue_Id")]
        public int EmailQueueId { get; set; }
        [Required]
        [StringLength(500)]
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? FileStorageType { get; set; }
        [StringLength(2000)]
        public string FileUniqueId { get; set; }
        [StringLength(2000)]
        public string FileLink { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("LogEmailQueueAttachments")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EmailQueueId")]
        [InverseProperty("LogEmailQueueAttachments")]
        public virtual LogEmailQueue EmailQueue { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("LogEmailQueueAttachments")]
        public virtual ApEntity Entity { get; set; }
    }
}