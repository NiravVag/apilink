using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Level_Custom")]
    public partial class SuLevelCustom
    {
        public SuLevelCustom()
        {
            SuGrades = new HashSet<SuGrade>();
        }

        public int Id { get; set; }
        public int? LevelId { get; set; }
        public int? CustomerId { get; set; }
        public bool IsDefault { get; set; }
        [StringLength(500)]
        public string CustomName { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("SuLevelCustoms")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("LevelId")]
        [InverseProperty("SuLevelCustoms")]
        public virtual SuLevel Level { get; set; }
        [InverseProperty("Level")]
        public virtual ICollection<SuGrade> SuGrades { get; set; }
    }
}