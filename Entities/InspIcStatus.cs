using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_IC_Status")]
    public partial class InspIcStatus
    {
        public InspIcStatus()
        {
            InspIcTransactions = new HashSet<InspIcTransaction>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string StatusName { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("IcstatusNavigation")]
        public virtual ICollection<InspIcTransaction> InspIcTransactions { get; set; }
    }
}