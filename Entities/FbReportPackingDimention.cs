using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Packing_Dimention")]
    public partial class FbReportPackingDimention
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        [StringLength(2000)]
        public string PackingType { get; set; }
        [StringLength(2000)]
        public string SpecClientValuesL { get; set; }
        [StringLength(2000)]
        public string SpecClientValuesW { get; set; }
        [StringLength(2000)]
        public string SpecClientValuesH { get; set; }
        [StringLength(2000)]
        public string PrintedPackValuesL { get; set; }
        [StringLength(2000)]
        public string PrintedPackValuesW { get; set; }
        [StringLength(2000)]
        public string PrintedPackValuesH { get; set; }
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

        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportPackingDimentions")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportPackingDimentions")]
        public virtual CuProduct Product { get; set; }
    }
}