using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Packing_Weight")]
    public partial class FbReportPackingWeight
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        [StringLength(2000)]
        public string SpecClientValues { get; set; }
        [StringLength(2000)]
        public string WeightPackValues { get; set; }
        [StringLength(2000)]
        public string Tolerance { get; set; }
        public double? NoPcs { get; set; }
        [StringLength(2000)]
        public string MeasuredValues { get; set; }
        [StringLength(1000)]
        public string DiscrepancyToSpec { get; set; }
        [StringLength(1000)]
        public string DiscrepancyToPacking { get; set; }
        [StringLength(2000)]
        public string Result { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int FbReportId { get; set; }
        [StringLength(100)]
        public string Unit { get; set; }
        [StringLength(500)]
        public string PackingType { get; set; }

        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportPackingWeights")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportPackingWeights")]
        public virtual CuProduct Product { get; set; }
    }
}