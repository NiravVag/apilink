using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_Status")]
    public partial class InvStatus
    {
        public InvStatus()
        {
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvAutTranStatusLogs = new HashSet<InvAutTranStatusLog>();
            InvManTransactions = new HashSet<InvManTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }

        [InverseProperty("InvoiceStatusNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<InvAutTranStatusLog> InvAutTranStatusLogs { get; set; }
        [InverseProperty("StatusNavigation")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
    }
}