using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_REF_Priority")]
    public partial class ClmRefPriority
    {
        public ClmRefPriority()
        {
            ClmTransactions = new HashSet<ClmTransaction>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("CustomerPriorityNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactions { get; set; }
    }
}