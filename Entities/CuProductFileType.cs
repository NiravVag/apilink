using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Product_FileType")]
    public partial class CuProductFileType
    {
        public CuProductFileType()
        {
            CuProductFileAttachments = new HashSet<CuProductFileAttachment>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public int? Sort { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("FileType")]
        public virtual ICollection<CuProductFileAttachment> CuProductFileAttachments { get; set; }
    }
}