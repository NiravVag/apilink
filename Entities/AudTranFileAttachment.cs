using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_File_Attachment")]
    public partial class AudTranFileAttachment
    {
        public int Id { get; set; }
        [Column("Audit_Id")]
        public int AuditId { get; set; }
        [Required]
        [StringLength(500)]
        public string FileName { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadDate { get; set; }
        public bool Active { get; set; }
        public string FileUrl { get; set; }
        [StringLength(1000)]
        public string UniqueId { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? FbMissionUrlId { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranFileAttachments")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudTranFileAttachmentDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("AudTranFileAttachmentUsers")]
        public virtual ItUserMaster User { get; set; }
    }
}