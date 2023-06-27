using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Result")]
    public partial class FbReportResult
    {
        public FbReportResult()
        {
            EsResultConfigs = new HashSet<EsResultConfig>();
            FbReportDetails = new HashSet<FbReportDetail>();
            FbReportInspSubSummaries = new HashSet<FbReportInspSubSummary>();
            FbReportInspSummaries = new HashSet<FbReportInspSummary>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string ResultName { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("ApiResult")]
        public virtual ICollection<EsResultConfig> EsResultConfigs { get; set; }
        [InverseProperty("Result")]
        public virtual ICollection<FbReportDetail> FbReportDetails { get; set; }
        [InverseProperty("ResultNavigation")]
        public virtual ICollection<FbReportInspSubSummary> FbReportInspSubSummaries { get; set; }
        [InverseProperty("ResultNavigation")]
        public virtual ICollection<FbReportInspSummary> FbReportInspSummaries { get; set; }
    }
}