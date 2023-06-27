using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Ref_Delimiter")]
    public partial class RefDelimiter
    {
        public RefDelimiter()
        {
            EsSuTemplateMasters = new HashSet<EsSuTemplateMaster>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column("Is_File")]
        public bool? IsFile { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("RefDelimiters")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [InverseProperty("Delimiter")]
        public virtual ICollection<EsSuTemplateMaster> EsSuTemplateMasters { get; set; }
    }
}