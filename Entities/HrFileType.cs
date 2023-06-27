using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_FileType")]
    public partial class HrFileType
    {
        public HrFileType()
        {
            HrAttachments = new HashSet<HrAttachment>();
            HrFileAttachments = new HashSet<HrFileAttachment>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string FileTypeName { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrFileTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("FileType")]
        public virtual ICollection<HrAttachment> HrAttachments { get; set; }
        [InverseProperty("FileType")]
        public virtual ICollection<HrFileAttachment> HrFileAttachments { get; set; }
    }
}