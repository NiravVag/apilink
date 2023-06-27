using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QC_BL_ProductSubCategory")]
    public partial class QcBlProductSubCategory
    {
        public int Id { get; set; }
        [Column("QCBLId")]
        public int Qcblid { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("QcBlProductSubCategories")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("ProductSubCategoryId")]
        [InverseProperty("QcBlProductSubCategories")]
        public virtual RefProductCategorySub ProductSubCategory { get; set; }
        [ForeignKey("Qcblid")]
        [InverseProperty("QcBlProductSubCategories")]
        public virtual QcBlockList Qcbl { get; set; }
    }
}