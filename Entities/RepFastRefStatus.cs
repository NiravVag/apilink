using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REP_FAST_REF_Status")]
    public partial class RepFastRefStatus
    {
        public RepFastRefStatus()
        {
            RepFastTranLogs = new HashSet<RepFastTranLog>();
            RepFastTransactions = new HashSet<RepFastTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("StatusNavigation")]
        public virtual ICollection<RepFastTranLog> RepFastTranLogs { get; set; }
        [InverseProperty("StatusNavigation")]
        public virtual ICollection<RepFastTransaction> RepFastTransactions { get; set; }
    }
}