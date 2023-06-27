using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_PackingPackagingLabelling_Product_Defect")]
    public partial class FbReportPackingPackagingLabellingProductDefect
    {
        public int Id { get; set; }
        [Column("PackingPackagingLabelling_Id")]
        public int? PackingPackagingLabellingId { get; set; }
        [StringLength(500)]
        public string Code { get; set; }
        [Column("RDNumber")]
        [StringLength(500)]
        public string Rdnumber { get; set; }
        public int? PackingType { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(500)]
        public string Severity { get; set; }
        public int? Quantity { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("PackingPackagingLabellingId")]
        [InverseProperty("FbReportPackingPackagingLabellingProductDefects")]
        public virtual FbReportPackingPackagingLabellingProduct PackingPackagingLabelling { get; set; }
    }
}