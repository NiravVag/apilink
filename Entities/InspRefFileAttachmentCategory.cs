using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_FileAttachment_Category")]
    public partial class InspRefFileAttachmentCategory
    {
        public InspRefFileAttachmentCategory()
        {
            InspTranFileAttachmentZips = new HashSet<InspTranFileAttachmentZip>();
            InspTranFileAttachments = new HashSet<InspTranFileAttachment>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int? Sort { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("FileAttachmentCategory")]
        public virtual ICollection<InspTranFileAttachmentZip> InspTranFileAttachmentZips { get; set; }
        [InverseProperty("FileAttachmentCategory")]
        public virtual ICollection<InspTranFileAttachment> InspTranFileAttachments { get; set; }
    }
}