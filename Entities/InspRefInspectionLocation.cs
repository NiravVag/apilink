using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_InspectionLocation")]
    public partial class InspRefInspectionLocation
    {
        public InspRefInspectionLocation()
        {
            CuPrInspectionLocations = new HashSet<CuPrInspectionLocation>();
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("InspectionLocation")]
        public virtual ICollection<CuPrInspectionLocation> CuPrInspectionLocations { get; set; }
        [InverseProperty("InspectionLocationNavigation")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}