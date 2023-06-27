using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QC_BL_ProductCatgeory")]
    public partial class QcBlProductCatgeory
    {
        public int Id { get; set; }
        [Column("QCBLId")]
        public int Qcblid { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("QcBlProductCatgeories")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("QcBlProductCatgeories")]
        public virtual RefProductCategory ProductCategory { get; set; }
        [ForeignKey("Qcblid")]
        [InverseProperty("QcBlProductCatgeories")]
        public virtual QcBlockList Qcbl { get; set; }
    }
}