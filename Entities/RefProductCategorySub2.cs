using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_ProductCategory_Sub2")]
    public partial class RefProductCategorySub2
    {
        public RefProductCategorySub2()
        {
            CuPrTranSubcategories = new HashSet<CuPrTranSubcategory>();
            CuPriceCategoryPcsub2S = new HashSet<CuPriceCategoryPcsub2>();
            CuProductTypes = new HashSet<CuProductType>();
            CuProducts = new HashSet<CuProduct>();
            InspTransactions = new HashSet<InspTransaction>();
            QcBlProductSubCategory2S = new HashSet<QcBlProductSubCategory2>();
            RefProductCategorySub3S = new HashSet<RefProductCategorySub3>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        [Column("ProductSubCategoryID")]
        public int ProductSubCategoryId { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }
        [Column("Fb_Product_SubCategory2_Id")]
        public int? FbProductSubCategory2Id { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefProductCategorySub2S")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ProductSubCategoryId")]
        [InverseProperty("RefProductCategorySub2S")]
        public virtual RefProductCategorySub ProductSubCategory { get; set; }
        [InverseProperty("SubCategory2")]
        public virtual ICollection<CuPrTranSubcategory> CuPrTranSubcategories { get; set; }
        [InverseProperty("ProductSubCategoryId2Navigation")]
        public virtual ICollection<CuPriceCategoryPcsub2> CuPriceCategoryPcsub2S { get; set; }
        [InverseProperty("LinkProductTypeNavigation")]
        public virtual ICollection<CuProductType> CuProductTypes { get; set; }
        [InverseProperty("ProductCategorySub2Navigation")]
        public virtual ICollection<CuProduct> CuProducts { get; set; }
        [InverseProperty("ProductSubCategory2")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
        [InverseProperty("ProductSubCategory2")]
        public virtual ICollection<QcBlProductSubCategory2> QcBlProductSubCategory2S { get; set; }
        [InverseProperty("ProductSubCategory2")]
        public virtual ICollection<RefProductCategorySub3> RefProductCategorySub3S { get; set; }
    }
}