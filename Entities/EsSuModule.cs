using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_SU_Module")]
    public partial class EsSuModule
    {
        public EsSuModule()
        {
            EsSuTemplateMasters = new HashSet<EsSuTemplateMaster>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsSuModules")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [InverseProperty("Module")]
        public virtual ICollection<EsSuTemplateMaster> EsSuTemplateMasters { get; set; }
    }
}