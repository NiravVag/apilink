using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_IC_Title")]
    public partial class InspIcTitle
    {
        public InspIcTitle()
        {
            InspIcTransactions = new HashSet<InspIcTransaction>();
        }

        public int Id { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("Ictitle")]
        public virtual ICollection<InspIcTransaction> InspIcTransactions { get; set; }
    }
}