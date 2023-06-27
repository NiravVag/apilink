using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_DataSourceType")]
    public partial class RefDataSourceType
    {
        public RefDataSourceType()
        {
            ApModules = new HashSet<ApModule>();
            ApSubModules = new HashSet<ApSubModule>();
            KpiColumns = new HashSet<KpiColumn>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [InverseProperty("DataSourceType")]
        public virtual ICollection<ApModule> ApModules { get; set; }
        [InverseProperty("DataSourceType")]
        public virtual ICollection<ApSubModule> ApSubModules { get; set; }
        [InverseProperty("FilterDataSourceType")]
        public virtual ICollection<KpiColumn> KpiColumns { get; set; }
    }
}