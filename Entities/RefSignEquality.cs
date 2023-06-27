using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_SignEquality")]
    public partial class RefSignEquality
    {
        public RefSignEquality()
        {
            KpiColumns = new HashSet<KpiColumn>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Label { get; set; }

        [InverseProperty("FilterSignEquality")]
        public virtual ICollection<KpiColumn> KpiColumns { get; set; }
    }
}