using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_REF_FileType")]
    public partial class ClmRefFileType
    {
        public ClmRefFileType()
        {
            ClmTranAttachments = new HashSet<ClmTranAttachment>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("FileTypeNavigation")]
        public virtual ICollection<ClmTranAttachment> ClmTranAttachments { get; set; }
    }
}