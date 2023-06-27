using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DF_DDL_SourceType")]
    public partial class DfDdlSourceType
    {
        public DfDdlSourceType()
        {
            DfCuConfigurations = new HashSet<DfCuConfiguration>();
            DfCuDdlSourceTypes = new HashSet<DfCuDdlSourceType>();
            DfDdlSources = new HashSet<DfDdlSource>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool Active { get; set; }

        [InverseProperty("DataSourceTypeNavigation")]
        public virtual ICollection<DfCuConfiguration> DfCuConfigurations { get; set; }
        [InverseProperty("Type")]
        public virtual ICollection<DfCuDdlSourceType> DfCuDdlSourceTypes { get; set; }
        [InverseProperty("TypeNavigation")]
        public virtual ICollection<DfDdlSource> DfDdlSources { get; set; }
    }
}