using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_File_Attachment")]
    public partial class InspTranFileAttachment
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string UniqueId { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
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
        [Column("Fb_Id")]
        public int? FbId { get; set; }
        public string FileDescription { get; set; }
        public int? ZipStatus { get; set; }
        public int? ZipTryCount { get; set; }
        [Column("FileAttachment_CategoryId")]
        public int? FileAttachmentCategoryId { get; set; }
        public bool? IsbookingEmailNotification { get; set; }
        [Column("IsReportSendToFB")]
        public bool? IsReportSendToFb { get; set; }

        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranFileAttachmentDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FileAttachmentCategoryId")]
        [InverseProperty("InspTranFileAttachments")]
        public virtual InspRefFileAttachmentCategory FileAttachmentCategory { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranFileAttachments")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("InspTranFileAttachmentUsers")]
        public virtual ItUserMaster User { get; set; }
    }
}