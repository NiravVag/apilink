using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Qualification")]
    public partial class HrQualification
    {
        public HrQualification()
        {
            HrStaffs = new HashSet<HrStaff>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Qualification_Name")]
        [StringLength(50)]
        public string QualificationName { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrQualifications")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Qualification")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
    }
}