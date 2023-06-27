using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_ProductBarcodesInfo")]
    public partial class FbReportProductBarcodesInfo
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int FbReportId { get; set; }
        [StringLength(500)]
        public string BarCode { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }

        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportProductBarcodesInfos")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportProductBarcodesInfos")]
        public virtual CuProduct Product { get; set; }
    }
}