using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_REF_File_Type")]
    public partial class EsRefFileType
    {
        public EsRefFileType()
        {
            EsTranFiles = new HashSet<EsTranFile>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsRefFileTypes")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [InverseProperty("FileType")]
        public virtual ICollection<EsTranFile> EsTranFiles { get; set; }
    }
}