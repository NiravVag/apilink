using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_InspSummary")]
    public partial class FbReportInspSummary
    {
        public FbReportInspSummary()
        {
            FbReportInspSubSummaries = new HashSet<FbReportInspSubSummary>();
            FbReportInspSummaryPhotos = new HashSet<FbReportInspSummaryPhoto>();
            FbReportProblematicRemarks = new HashSet<FbReportProblematicRemark>();
        }

        public int Id { get; set; }
        public int FbReportDetailId { get; set; }
        public int FbReportInspsumTypeId { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Result { get; set; }
        public string Remarks { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int? ResultId { get; set; }
        public int? Sort { get; set; }
        [StringLength(100)]
        public string ScoreValue { get; set; }
        [StringLength(100)]
        public string ScorePercentage { get; set; }

        [ForeignKey("FbReportDetailId")]
        [InverseProperty("FbReportInspSummaries")]
        public virtual FbReportDetail FbReportDetail { get; set; }
        [ForeignKey("FbReportInspsumTypeId")]
        [InverseProperty("FbReportInspSummaries")]
        public virtual FbReportInspSummaryType FbReportInspsumType { get; set; }
        [ForeignKey("ResultId")]
        [InverseProperty("FbReportInspSummaries")]
        public virtual FbReportResult ResultNavigation { get; set; }
        [InverseProperty("FbReportSummary")]
        public virtual ICollection<FbReportInspSubSummary> FbReportInspSubSummaries { get; set; }
        [InverseProperty("FbReportSummary")]
        public virtual ICollection<FbReportInspSummaryPhoto> FbReportInspSummaryPhotos { get; set; }
        [InverseProperty("FbReportSummary")]
        public virtual ICollection<FbReportProblematicRemark> FbReportProblematicRemarks { get; set; }
    }
}