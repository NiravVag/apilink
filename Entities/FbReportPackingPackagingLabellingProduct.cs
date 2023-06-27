using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_PackingPackagingLabelling_Product")]
    public partial class FbReportPackingPackagingLabellingProduct
    {
        public FbReportPackingPackagingLabellingProduct()
        {
            FbReportPackingPackagingLabellingProductDefects = new HashSet<FbReportPackingPackagingLabellingProductDefect>();
        }

        public int Id { get; set; }
        public int? FbReportdetailsId { get; set; }
        public int? SampleSizeCtns { get; set; }
        public int? PackingType { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int? TotalDefectiveUnits { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CartonQty { get; set; }

        [ForeignKey("FbReportdetailsId")]
        [InverseProperty("FbReportPackingPackagingLabellingProducts")]
        public virtual FbReportDetail FbReportdetails { get; set; }
        [InverseProperty("PackingPackagingLabelling")]
        public virtual ICollection<FbReportPackingPackagingLabellingProductDefect> FbReportPackingPackagingLabellingProductDefects { get; set; }
    }
}