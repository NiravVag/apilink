using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("KPI_TemplateSubModule")]
    public partial class KpiTemplateSubModule
    {
        public int IdTemplate { get; set; }
        public int IdSubModule { get; set; }

        [ForeignKey("IdSubModule")]
        [InverseProperty("KpiTemplateSubModules")]
        public virtual ApSubModule IdSubModuleNavigation { get; set; }
        [ForeignKey("IdTemplate")]
        [InverseProperty("KpiTemplateSubModules")]
        public virtual KpiTemplate IdTemplateNavigation { get; set; }
    }
}