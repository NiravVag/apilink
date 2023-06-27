using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_PackingStatus")]
    public partial class InspRefPackingStatus
    {
        public InspRefPackingStatus()
        {
            InspProductTransactions = new HashSet<InspProductTransaction>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("PackingStatusNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactions { get; set; }
    }
}