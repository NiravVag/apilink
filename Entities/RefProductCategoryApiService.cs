using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Product_Category_API_Services")]
    public partial class RefProductCategoryApiService
    {
        public int Id { get; set; }
        [Column("Product_Category_Id")]
        public int ProductCategoryId { get; set; }
        public int ServiceId { get; set; }
        [StringLength(50)]
        public string CustomName { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("RefProductCategoryApiServiceCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("RefProductCategoryApiServiceDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("RefProductCategoryApiServices")]
        public virtual RefProductCategory ProductCategory { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("RefProductCategoryApiServices")]
        public virtual RefService Service { get; set; }
    }
}