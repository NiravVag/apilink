using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Product_File_Attachment")]
    public partial class CuProductFileAttachment
    {
        public CuProductFileAttachment()
        {
            CuProductMscharts = new HashSet<CuProductMschart>();
        }

        public int Id { get; set; }
        [StringLength(1000)]
        public string UniqueId { get; set; }
        [Column("Product_Id")]
        public int ProductId { get; set; }
        [Required]
        [StringLength(500)]
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadDate { get; set; }
        public bool Active { get; set; }
        [Column("Booking_Id")]
        public int? BookingId { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [Column("FileType_Id")]
        public int? FileTypeId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("CuProductFileAttachments")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuProductFileAttachmentDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuProductFileAttachments")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FileTypeId")]
        [InverseProperty("CuProductFileAttachments")]
        public virtual CuProductFileType FileType { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("CuProductFileAttachments")]
        public virtual CuProduct Product { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("CuProductFileAttachmentUsers")]
        public virtual ItUserMaster User { get; set; }
        [InverseProperty("ProductFile")]
        public virtual ICollection<CuProductMschart> CuProductMscharts { get; set; }
    }
}