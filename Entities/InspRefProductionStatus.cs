using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_ProductionStatus")]
    public partial class InspRefProductionStatus
    {
        public InspRefProductionStatus()
        {
            InspProductTransactions = new HashSet<InspProductTransaction>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("ProductionStatusNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactions { get; set; }
    }
}