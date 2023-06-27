using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Sample_Pickings")]
    public partial class FbReportSamplePicking
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        [StringLength(2000)]
        public string SampleType { get; set; }
        [StringLength(2000)]
        public string Destination { get; set; }
        [StringLength(1000)]
        public string Quantity { get; set; }
        public string Comments { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int FbReportId { get; set; }

        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportSamplePickings")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportSamplePickings")]
        public virtual CuProduct Product { get; set; }
    }
}