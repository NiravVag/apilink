using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Product_Dimention")]
    public partial class FbReportProductDimention
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        [StringLength(2000)]
        public string SpecClientValuesL { get; set; }
        [StringLength(2000)]
        public string SpecClientValuesW { get; set; }
        [StringLength(2000)]
        public string SpecClientValuesH { get; set; }
        [StringLength(2000)]
        public string DimensionPackValuesL { get; set; }
        [StringLength(2000)]
        public string DimensionPackValuesW { get; set; }
        [StringLength(2000)]
        public string DimensionPackValuesH { get; set; }
        [StringLength(2000)]
        public string Tolerance { get; set; }
        public double? NoPcs { get; set; }
        [StringLength(2000)]
        public string MeasuredValuesL { get; set; }
        [StringLength(2000)]
        public string MeasuredValuesW { get; set; }
        [StringLength(2000)]
        public string MeasuredValuesH { get; set; }
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
        [InverseProperty("FbReportProductDimentions")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportProductDimentions")]
        public virtual CuProduct Product { get; set; }
    }
}