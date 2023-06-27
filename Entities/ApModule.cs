using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AP_Module")]
    public partial class ApModule
    {
        public ApModule()
        {
            ApModuleRoles = new HashSet<ApModuleRole>();
            ApSubModules = new HashSet<ApSubModule>();
            KpiColumns = new HashSet<KpiColumn>();
            KpiTemplates = new HashSet<KpiTemplate>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string DataSourceName { get; set; }
        public int DataSourceTypeId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("DataSourceTypeId")]
        [InverseProperty("ApModules")]
        public virtual RefDataSourceType DataSourceType { get; set; }
        [InverseProperty("IdModuleNavigation")]
        public virtual ICollection<ApModuleRole> ApModuleRoles { get; set; }
        [InverseProperty("IdModuleNavigation")]
        public virtual ICollection<ApSubModule> ApSubModules { get; set; }
        [InverseProperty("IdModuleNavigation")]
        public virtual ICollection<KpiColumn> KpiColumns { get; set; }
        [InverseProperty("IdModuleNavigation")]
        public virtual ICollection<KpiTemplate> KpiTemplates { get; set; }
    }
}