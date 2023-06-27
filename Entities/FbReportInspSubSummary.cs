using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_InspSub_Summary")]
    public partial class FbReportInspSubSummary
    {
        public int Id { get; set; }
        public int FbReportSummaryId { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Result { get; set; }
        public int? ResultId { get; set; }
        public int? Sort { get; set; }
        public string Remarks { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("FbReportSummaryId")]
        [InverseProperty("FbReportInspSubSummaries")]
        public virtual FbReportInspSummary FbReportSummary { get; set; }
        [ForeignKey("ResultId")]
        [InverseProperty("FbReportInspSubSummaries")]
        public virtual FbReportResult ResultNavigation { get; set; }
    }
}