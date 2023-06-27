using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_REF_Status")]
    public partial class ClmRefStatus
    {
        public ClmRefStatus()
        {
            ClmTransactions = new HashSet<ClmTransaction>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("Status")]
        public virtual ICollection<ClmTransaction> ClmTransactions { get; set; }
    }
}