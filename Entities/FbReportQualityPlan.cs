using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_QualityPlan")]
    public partial class FbReportQualityPlan
    {
        public FbReportQualityPlan()
        {
            FbReportQualityPlanMeasurementDefectsPoms = new HashSet<FbReportQualityPlanMeasurementDefectsPom>();
            FbReportQualityPlanMeasurementDefectsSizes = new HashSet<FbReportQualityPlanMeasurementDefectsSize>();
        }

        public int Id { get; set; }
        public int? FbReportDetailsId { get; set; }
        [StringLength(500)]
        public string Title { get; set; }
        public int? TotalDefectiveUnits { get; set; }
        [StringLength(500)]
        public string Result { get; set; }
        public int? TotalQtyMeasurmentDefects { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [StringLength(100)]
        public string TotalPiecesMeasurmentDefects { get; set; }
        [StringLength(100)]
        public string SampleInspected { get; set; }
        [StringLength(100)]
        public string ActualMeasuredSampleSize { get; set; }

        [ForeignKey("FbReportDetailsId")]
        [InverseProperty("FbReportQualityPlans")]
        public virtual FbReportDetail FbReportDetails { get; set; }
        [InverseProperty("QualityPlan")]
        public virtual ICollection<FbReportQualityPlanMeasurementDefectsPom> FbReportQualityPlanMeasurementDefectsPoms { get; set; }
        [InverseProperty("QualityPlan")]
        public virtual ICollection<FbReportQualityPlanMeasurementDefectsSize> FbReportQualityPlanMeasurementDefectsSizes { get; set; }
    }
}