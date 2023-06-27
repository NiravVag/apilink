using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PriceCategory_PCSub2")]
    public partial class CuPriceCategoryPcsub2
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductSubCategoryId2 { get; set; }
        public int PriceCategoryId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuPriceCategoryPcsub2S")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("PriceCategoryId")]
        [InverseProperty("CuPriceCategoryPcsub2S")]
        public virtual CuPriceCategory PriceCategory { get; set; }
        [ForeignKey("ProductSubCategoryId2")]
        [InverseProperty("CuPriceCategoryPcsub2S")]
        public virtual RefProductCategorySub2 ProductSubCategoryId2Navigation { get; set; }
    }
}