using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_Product_Category_Config")]
    public partial class EsProductCategoryConfig
    {
        public int Id { get; set; }
        public int ProductCategoryId { get; set; }
        public int EsDetailsId { get; set; }

        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsProductCategoryConfigs")]
        public virtual EsDetail EsDetails { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("EsProductCategoryConfigs")]
        public virtual RefProductCategory ProductCategory { get; set; }
    }
}