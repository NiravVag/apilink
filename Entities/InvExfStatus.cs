using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_EXF_Status")]
    public partial class InvExfStatus
    {
        public InvExfStatus()
        {
            InvExfTranStatusLogs = new HashSet<InvExfTranStatusLog>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public bool? Sort { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [InverseProperty("Status")]
        public virtual ICollection<InvExfTranStatusLog> InvExfTranStatusLogs { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
    }
}