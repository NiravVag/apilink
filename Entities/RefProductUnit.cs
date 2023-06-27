using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Product_Units")]
    public partial class RefProductUnit
    {
        public RefProductUnit()
        {
            CuPurchaseOrderDetails = new HashSet<CuPurchaseOrderDetail>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefProductUnits")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Unit")]
        public virtual ICollection<CuPurchaseOrderDetail> CuPurchaseOrderDetails { get; set; }
    }
}