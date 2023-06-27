using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_ProductType")]
    public partial class CuProductType
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        public int? CustomerId { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }
        public int? EntityId { get; set; }
        public int? LinkProductType { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuProductTypes")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuProductTypes")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("LinkProductType")]
        [InverseProperty("CuProductTypes")]
        public virtual RefProductCategorySub2 LinkProductTypeNavigation { get; set; }
    }
}