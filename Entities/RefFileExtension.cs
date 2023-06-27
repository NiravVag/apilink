using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_File_Extension")]
    public partial class RefFileExtension
    {
        public RefFileExtension()
        {
            RepFastTemplateConfigs = new HashSet<RepFastTemplateConfig>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string ExtensionName { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("FileExtension")]
        public virtual ICollection<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
    }
}