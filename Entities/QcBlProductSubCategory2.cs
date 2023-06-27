using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QC_BL_ProductSubCategory2")]
    public partial class QcBlProductSubCategory2
    {
        public int Id { get; set; }
        [Column("QCBLId")]
        public int Qcblid { get; set; }
        public int? ProductSubCategory2Id { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("QcBlProductSubCategory2S")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("ProductSubCategory2Id")]
        [InverseProperty("QcBlProductSubCategory2S")]
        public virtual RefProductCategorySub2 ProductSubCategory2 { get; set; }
        [ForeignKey("Qcblid")]
        [InverseProperty("QcBlProductSubCategory2S")]
        public virtual QcBlockList Qcbl { get; set; }
    }
}