using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_Right_Entity")]
    public partial class ItRightEntity
    {
        public int Id { get; set; }
        public int? RightId { get; set; }
        public int? EntityId { get; set; }
        public int? CustomerId { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("ItRightEntities")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("ItRightEntities")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("RightId")]
        [InverseProperty("ItRightEntities")]
        public virtual ItRight Right { get; set; }
    }
}