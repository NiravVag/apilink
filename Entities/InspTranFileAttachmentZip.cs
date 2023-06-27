using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_File_Attachment_zip")]
    public partial class InspTranFileAttachmentZip
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int? InspectionId { get; set; }
        public int? FileAttachmentCategoryId { get; set; }
        [StringLength(500)]
        public string FileName { get; set; }
        [StringLength(500)]
        public string FileUrl { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranFileAttachmentZipCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranFileAttachmentZipDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FileAttachmentCategoryId")]
        [InverseProperty("InspTranFileAttachmentZips")]
        public virtual InspRefFileAttachmentCategory FileAttachmentCategory { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranFileAttachmentZips")]
        public virtual InspTransaction Inspection { get; set; }
    }
}