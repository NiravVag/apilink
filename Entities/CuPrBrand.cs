using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_Brand")]
    public partial class CuPrBrand
    {
        public int Id { get; set; }
        [Column("Cu_Price_Id")]
        public int CuPriceId { get; set; }
        [Column("Brand_Id")]
        public int? BrandId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("CuPrBrands")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrBrandCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPriceId")]
        [InverseProperty("CuPrBrands")]
        public virtual CuPrDetail CuPrice { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrBrandDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrBrandUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}