using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_FileAttachment")]
    public partial class HrFileAttachment
    {
        public int Id { get; set; }
        public Guid GuidId { get; set; }
        [Required]
        [StringLength(200)]
        public string FullFileName { get; set; }
        public byte[] File { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadDate { get; set; }
        [Column("staff_id")]
        public int StaffId { get; set; }
        public int FileTypeId { get; set; }

        [ForeignKey("FileTypeId")]
        [InverseProperty("HrFileAttachments")]
        public virtual HrFileType FileType { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrFileAttachments")]
        public virtual HrStaff Staff { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("HrFileAttachments")]
        public virtual ItUserMaster User { get; set; }
    }
}