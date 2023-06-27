using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_ProductCategory_Sub3")]
    public partial class RefProductCategorySub3
    {
        public RefProductCategorySub3()
        {
            CuProducts = new HashSet<CuProduct>();
            QuWorkLoadMatrices = new HashSet<QuWorkLoadMatrix>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(1000)]
        public string Name { get; set; }
        public int ProductSubCategory2Id { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("RefProductCategorySub3CreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("RefProductCategorySub3DeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("RefProductCategorySub3S")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ProductSubCategory2Id")]
        [InverseProperty("RefProductCategorySub3S")]
        public virtual RefProductCategorySub2 ProductSubCategory2 { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("RefProductCategorySub3UpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("ProductCategorySub3Navigation")]
        public virtual ICollection<CuProduct> CuProducts { get; set; }
        [InverseProperty("ProductSubCategory3")]
        public virtual ICollection<QuWorkLoadMatrix> QuWorkLoadMatrices { get; set; }
    }
}