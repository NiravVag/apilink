using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PurchaseOrder_Attachment")]
    public partial class CuPurchaseOrderAttachment
    {
        public int Id { get; set; }
        public Guid GuidId { get; set; }
        [Column("Po_Id")]
        public int PoId { get; set; }
        [Required]
        [StringLength(500)]
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadDate { get; set; }
        public bool Active { get; set; }

        [ForeignKey("PoId")]
        [InverseProperty("CuPurchaseOrderAttachments")]
        public virtual CuPurchaseOrder Po { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("CuPurchaseOrderAttachments")]
        public virtual ItUserMaster User { get; set; }
    }
}