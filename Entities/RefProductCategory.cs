using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_ProductCategory")]
    public partial class RefProductCategory
    {
        public RefProductCategory()
        {
            CuCsConfigurations = new HashSet<CuCsConfiguration>();
            CuPrProductCategories = new HashSet<CuPrProductCategory>();
            CuProducts = new HashSet<CuProduct>();
            CuServiceTypes = new HashSet<CuServiceType>();
            DaUserByProductCategories = new HashSet<DaUserByProductCategory>();
            EsProductCategoryConfigs = new HashSet<EsProductCategoryConfig>();
            HrStaffProductCategories = new HashSet<HrStaffProductCategory>();
            InspTransactions = new HashSet<InspTransaction>();
            QcBlProductCatgeories = new HashSet<QcBlProductCatgeory>();
            RefProductCategoryApiServices = new HashSet<RefProductCategoryApiService>();
            RefProductCategorySubs = new HashSet<RefProductCategorySub>();
            RepFastTemplateConfigs = new HashSet<RepFastTemplateConfig>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }
        [Column("Fb_ProductCategory_Id")]
        public int? FbProductCategoryId { get; set; }
        public int? BusinessLineId { get; set; }

        [ForeignKey("BusinessLineId")]
        [InverseProperty("RefProductCategories")]
        public virtual RefBusinessLine BusinessLine { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("RefProductCategories")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<CuCsConfiguration> CuCsConfigurations { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<CuPrProductCategory> CuPrProductCategories { get; set; }
        [InverseProperty("ProductCategoryNavigation")]
        public virtual ICollection<CuProduct> CuProducts { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<DaUserByProductCategory> DaUserByProductCategories { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<EsProductCategoryConfig> EsProductCategoryConfigs { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<HrStaffProductCategory> HrStaffProductCategories { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<QcBlProductCatgeory> QcBlProductCatgeories { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<RefProductCategoryApiService> RefProductCategoryApiServices { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<RefProductCategorySub> RefProductCategorySubs { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
    }
}