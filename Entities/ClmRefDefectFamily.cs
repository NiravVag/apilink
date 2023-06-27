using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_REF_DefectFamily")]
    public partial class ClmRefDefectFamily
    {
        public ClmRefDefectFamily()
        {
            ClmTranDefectFamilies = new HashSet<ClmTranDefectFamily>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("DefectFamily")]
        public virtual ICollection<ClmTranDefectFamily> ClmTranDefectFamilies { get; set; }
    }
}