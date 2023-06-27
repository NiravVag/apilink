using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Packing_BatteryInfo")]
    public partial class FbReportPackingBatteryInfo
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        [StringLength(2000)]
        public string BatteryType { get; set; }
        [StringLength(2000)]
        public string BatteryModel { get; set; }
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
        [InverseProperty("FbReportPackingBatteryInfos")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportPackingBatteryInfos")]
        public virtual CuProduct Product { get; set; }
    }
}