using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_DefectClassification")]
    public partial class RefDefectClassification
    {
        public RefDefectClassification()
        {
            CuServiceTypes = new HashSet<CuServiceType>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Value { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefDefectClassifications")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("DefectClassificationNavigation")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
    }
}