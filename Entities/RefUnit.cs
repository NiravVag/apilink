using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Unit")]
    public partial class RefUnit
    {
        public RefUnit()
        {
            InspProductTransactions = new HashSet<InspProductTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefUnits")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("UnitNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactions { get; set; }
    }
}