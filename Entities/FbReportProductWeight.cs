using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Product_Weight")]
    public partial class FbReportProductWeight
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
        public string DiscrepancyToPack { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int FbReportId { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(100)]
        public string Unit { get; set; }

        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportProductWeights")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportProductWeights")]
        public virtual CuProduct Product { get; set; }
    }
}