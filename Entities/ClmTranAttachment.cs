using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_TRAN_Attachments")]
    public partial class ClmTranAttachment
    {
        public int Id { get; set; }
        [StringLength(2000)]
        public string UniqueId { get; set; }
        public int? ClaimId { get; set; }
        public int? FileType { get; set; }
        [StringLength(1000)]
        public string FileName { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public string FileUrl { get; set; }
        public int? EntityId { get; set; }
        public bool? Active { get; set; }
        public string FileDesc { get; set; }

        [ForeignKey("ClaimId")]
        [InverseProperty("ClmTranAttachments")]
        public virtual ClmTransaction Claim { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ClmTranAttachmentCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("ClmTranAttachmentDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("ClmTranAttachments")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FileType")]
        [InverseProperty("ClmTranAttachments")]
        public virtual ClmRefFileType FileTypeNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("ClmTranAttachmentUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}