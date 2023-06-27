using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_Status")]
    public partial class QuStatus
    {
        public QuStatus()
        {
            QuQuotations = new HashSet<QuQuotation>();
            QuTranStatusLogs = new HashSet<QuTranStatusLog>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Label { get; set; }
        public int? TranId { get; set; }
        public int EntityId { get; set; }
        [Required]
        public bool? Active { get; set; }

        [InverseProperty("IdStatusNavigation")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<QuTranStatusLog> QuTranStatusLogs { get; set; }
    }
}