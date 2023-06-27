using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("COMP_REF_Category")]
    public partial class CompRefCategory
    {
        public CompRefCategory()
        {
            CompTranComplaintsDetails = new HashSet<CompTranComplaintsDetail>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Sort { get; set; }

        [InverseProperty("ComplaintCategoryNavigation")]
        public virtual ICollection<CompTranComplaintsDetail> CompTranComplaintsDetails { get; set; }
    }
}