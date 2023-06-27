using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("KPI_Template")]
    public partial class KpiTemplate
    {
        public KpiTemplate()
        {
            KpiTemplateColumns = new HashSet<KpiTemplateColumn>();
            KpiTemplateSubModules = new HashSet<KpiTemplateSubModule>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public bool IsShared { get; set; }
        public bool UseXlsFormulas { get; set; }
        public int IdModule { get; set; }

        [ForeignKey("IdModule")]
        [InverseProperty("KpiTemplates")]
        public virtual ApModule IdModuleNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("KpiTemplates")]
        public virtual ItUserMaster User { get; set; }
        [InverseProperty("IdTemplateNavigation")]
        public virtual ICollection<KpiTemplateColumn> KpiTemplateColumns { get; set; }
        [InverseProperty("IdTemplateNavigation")]
        public virtual ICollection<KpiTemplateSubModule> KpiTemplateSubModules { get; set; }
    }
}