using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Problematic_Remarks")]
    public partial class FbReportProblematicRemark
    {
        public int Id { get; set; }
        public int FbReportSummaryId { get; set; }
        public int? ProductId { get; set; }
        public string Remarks { get; set; }
        [StringLength(2000)]
        public string Result { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        [Column("Sub_Category")]
        [StringLength(1000)]
        public string SubCategory { get; set; }
        [Column("Sub_Category2")]
        [StringLength(1000)]
        public string SubCategory2 { get; set; }
        [StringLength(1000)]
        public string CustomerRemarkCode { get; set; }

        [ForeignKey("FbReportSummaryId")]
        [InverseProperty("FbReportProblematicRemarks")]
        public virtual FbReportInspSummary FbReportSummary { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportProblematicRemarks")]
        public virtual CuProduct Product { get; set; }
    }
}