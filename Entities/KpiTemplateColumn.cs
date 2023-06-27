using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("KPI_TemplateColumn")]
    public partial class KpiTemplateColumn
    {
        public int Id { get; set; }
        public int IdTemplate { get; set; }
        public int? IdColumn { get; set; }
        [StringLength(200)]
        public string ColumnName { get; set; }
        public bool? SumFooter { get; set; }
        public bool? Group { get; set; }
        public int? OrderColumn { get; set; }
        public int? OrderFilter { get; set; }
        public bool? SelectMultiple { get; set; }
        public bool Required { get; set; }
        public bool? FilterLazy { get; set; }
        [StringLength(300)]
        public string Valuecolumn { get; set; }

        [ForeignKey("IdColumn")]
        [InverseProperty("KpiTemplateColumns")]
        public virtual KpiColumn IdColumnNavigation { get; set; }
        [ForeignKey("IdTemplate")]
        [InverseProperty("KpiTemplateColumns")]
        public virtual KpiTemplate IdTemplateNavigation { get; set; }
    }
}