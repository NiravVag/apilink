using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_QualityPlan_MeasurementDefectsSize")]
    public partial class FbReportQualityPlanMeasurementDefectsSize
    {
        public int Id { get; set; }
        public int? QualityPlanId { get; set; }
        [StringLength(500)]
        public string Size { get; set; }
        public int? Quantity { get; set; }

        [ForeignKey("QualityPlanId")]
        [InverseProperty("FbReportQualityPlanMeasurementDefectsSizes")]
        public virtual FbReportQualityPlan QualityPlan { get; set; }
    }
}