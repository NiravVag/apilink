using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_REF_FileType")]
    public partial class InvRefFileType
    {
        public InvRefFileType()
        {
            InvTranFiles = new HashSet<InvTranFile>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }
        public bool? IsUpload { get; set; }

        [InverseProperty("FileTypeNavigation")]
        public virtual ICollection<InvTranFile> InvTranFiles { get; set; }
    }
}