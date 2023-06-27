using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_PriceCategory")]
    public partial class CuPrPriceCategory
    {
        public int Id { get; set; }
        [Column("Cu_Price_Id")]
        public int CuPriceId { get; set; }
        [Column("PriceCategory_Id")]
        public int? PriceCategoryId { get; set; }
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

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrPriceCategoryCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPriceId")]
        [InverseProperty("CuPrPriceCategories")]
        public virtual CuPrDetail CuPrice { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrPriceCategoryDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("PriceCategoryId")]
        [InverseProperty("CuPrPriceCategories")]
        public virtual CuPriceCategory PriceCategory { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrPriceCategoryUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}