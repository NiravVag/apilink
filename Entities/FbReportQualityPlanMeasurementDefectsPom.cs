using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_QualityPlan_MeasurementDefectsPOM")]
    public partial class FbReportQualityPlanMeasurementDefectsPom
    {
        public int Id { get; set; }
        public int? QualityPlanId { get; set; }
        [Column("CodePOM")]
        [StringLength(500)]
        public string CodePom { get; set; }
        [Column("POM")]
        [StringLength(500)]
        public string Pom { get; set; }
        [Column("CriticalPOM")]
        [StringLength(500)]
        public string CriticalPom { get; set; }
        public int? Quantity { get; set; }
        [StringLength(500)]
        public string SpecZone { get; set; }

        [ForeignKey("QualityPlanId")]
        [InverseProperty("FbReportQualityPlanMeasurementDefectsPoms")]
        public virtual FbReportQualityPlan QualityPlan { get; set; }
    }
}