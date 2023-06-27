using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_InspSummary_Photo")]
    public partial class FbReportInspSummaryPhoto
    {
        public int Id { get; set; }
        public int FbReportSummaryId { get; set; }
        [StringLength(1000)]
        public string Photo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        [StringLength(1500)]
        public string Description { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey("FbReportSummaryId")]
        [InverseProperty("FbReportInspSummaryPhotos")]
        public virtual FbReportInspSummary FbReportSummary { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportInspSummaryPhotos")]
        public virtual CuProduct Product { get; set; }
    }
}