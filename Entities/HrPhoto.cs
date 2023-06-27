using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Photo")]
    public partial class HrPhoto
    {
        public Guid GuidId { get; set; }
        [StringLength(1000)]
        public string UniqueId { get; set; }
        public int StaffId { get; set; }
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
        [InverseProperty("HrPhotoDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrPhotos")]
        public virtual HrStaff Staff { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("HrPhotoUsers")]
        public virtual ItUserMaster User { get; set; }
    }
}