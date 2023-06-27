using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DF_DDL_Source")]
    public partial class DfDdlSource
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int Type { get; set; }
        public bool Active { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey("Type")]
        [InverseProperty("DfDdlSources")]
        public virtual DfDdlSourceType TypeNavigation { get; set; }
    }
}