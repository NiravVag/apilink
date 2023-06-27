using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AP_SubModule")]
    public partial class ApSubModule
    {
        public ApSubModule()
        {
            ApSubModuleRoles = new HashSet<ApSubModuleRole>();
            KpiColumns = new HashSet<KpiColumn>();
            KpiTemplateSubModules = new HashSet<KpiTemplateSubModule>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        public int IdModule { get; set; }
        [Required]
        [StringLength(200)]
        public string DataSourceName { get; set; }
        public int DataSourceTypeId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("DataSourceTypeId")]
        [InverseProperty("ApSubModules")]
        public virtual RefDataSourceType DataSourceType { get; set; }
        [ForeignKey("IdModule")]
        [InverseProperty("ApSubModules")]
        public virtual ApModule IdModuleNavigation { get; set; }
        [InverseProperty("IdSubModuleNavigation")]
        public virtual ICollection<ApSubModuleRole> ApSubModuleRoles { get; set; }
        [InverseProperty("IdSubModuleNavigation")]
        public virtual ICollection<KpiColumn> KpiColumns { get; set; }
        [InverseProperty("IdSubModuleNavigation")]
        public virtual ICollection<KpiTemplateSubModule> KpiTemplateSubModules { get; set; }
    }
}