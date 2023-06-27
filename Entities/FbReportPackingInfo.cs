using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_PackingInfo")]
    public partial class FbReportPackingInfo
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        [StringLength(2000)]
        public string MaterialType { get; set; }
        public string PackagingDesc { get; set; }
        public double? PieceNo { get; set; }
        [StringLength(1000)]
        public string Quantity { get; set; }
        [StringLength(2000)]
        public string Location { get; set; }
        [StringLength(1000)]
        public string NetWeightPerQty { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int FbReportId { get; set; }

        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportPackingInfos")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportPackingInfos")]
        public virtual CuProduct Product { get; set; }
    }
}