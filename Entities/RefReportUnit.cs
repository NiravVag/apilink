using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_ReportUnit")]
    public partial class RefReportUnit
    {
        public RefReportUnit()
        {
            CuServiceTypes = new HashSet<CuServiceType>();
        }

        public int Id { get; set; }
        [StringLength(20)]
        public string Value { get; set; }
        [Column("Active ")]
        public bool Active { get; set; }

        [InverseProperty("ReportUnitNavigation")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
    }
}