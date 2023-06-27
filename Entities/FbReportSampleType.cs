using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_SampleTypes")]
    public partial class FbReportSampleType
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        [StringLength(1000)]
        public string SampleType { get; set; }
        public string Description { get; set; }
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
        [InverseProperty("FbReportSampleTypes")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportSampleTypes")]
        public virtual CuProduct Product { get; set; }
    }
}