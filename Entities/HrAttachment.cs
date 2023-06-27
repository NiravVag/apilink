using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Attachment")]
    public partial class HrAttachment
    {
        public int Id { get; set; }
        public Guid GuidId { get; set; }
        [StringLength(1000)]
        public string UniqueId { get; set; }
        [Column("Staff_Id")]
        public int StaffId { get; set; }
        public int FileTypeId { get; set; }
        [Required]
        [StringLength(500)]
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadDate { get; set; }
        public bool Active { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("DeletedBy")]
        [InverseProperty("HrAttachmentDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FileTypeId")]
        [InverseProperty("HrAttachments")]
        public virtual HrFileType FileType { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrAttachments")]
        public virtual HrStaff Staff { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("HrAttachmentUsers")]
        public virtual ItUserMaster User { get; set; }
    }
}