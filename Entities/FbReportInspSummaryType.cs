using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_InspSummary_Type")]
    public partial class FbReportInspSummaryType
    {
        public FbReportInspSummaryType()
        {
            FbReportInspSummaries = new HashSet<FbReportInspSummary>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Type { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("FbReportInspsumType")]
        public virtual ICollection<FbReportInspSummary> FbReportInspSummaries { get; set; }
    }
}