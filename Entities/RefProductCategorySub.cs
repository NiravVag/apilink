using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_ProductCategory_Sub")]
    public partial class RefProductCategorySub
    {
        public RefProductCategorySub()
        {
            CuPrProductSubCategories = new HashSet<CuPrProductSubCategory>();
            CuPrTranSubcategories = new HashSet<CuPrTranSubcategory>();
            CuProductCategories = new HashSet<CuProductCategory>();
            CuProducts = new HashSet<CuProduct>();
            InspTransactions = new HashSet<InspTransaction>();
            QcBlProductSubCategories = new HashSet<QcBlProductSubCategory>();
            RefProductCategorySub2S = new HashSet<RefProductCategorySub2>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("ProductCategoryID")]
        public int ProductCategoryId { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }
        [Column("Fb_Product_SubCategory_Id")]
        public int? FbProductSubCategoryId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefProductCategorySubs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("RefProductCategorySubs")]
        public virtual RefProductCategory ProductCategory { get; set; }
        [InverseProperty("ProductSubCategory")]
        public virtual ICollection<CuPrProductSubCategory> CuPrProductSubCategories { get; set; }
        [InverseProperty("SubCategory")]
        public virtual ICollection<CuPrTranSubcategory> CuPrTranSubcategories { get; set; }
        [InverseProperty("LinkProductSubCategoryNavigation")]
        public virtual ICollection<CuProductCategory> CuProductCategories { get; set; }
        [InverseProperty("ProductSubCategoryNavigation")]
        public virtual ICollection<CuProduct> CuProducts { get; set; }
        [InverseProperty("ProductSubCategory")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
        [InverseProperty("ProductSubCategory")]
        public virtual ICollection<QcBlProductSubCategory> QcBlProductSubCategories { get; set; }
        [InverseProperty("ProductSubCategory")]
        public virtual ICollection<RefProductCategorySub2> RefProductCategorySub2S { get; set; }
    }
}