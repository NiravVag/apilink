using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_FabricDefects")]
    public partial class FbReportFabricDefect
    {
        public int Id { get; set; }
        public int? FbreportdetailsId { get; set; }
        public int? InspPoTransactionId { get; set; }
        public int? InspColorTransactionId { get; set; }
        [StringLength(100)]
        public string LengthUnit { get; set; }
        [StringLength(200)]
        public string DyeLot { get; set; }
        [StringLength(100)]
        public string RollNumber { get; set; }
        [StringLength(100)]
        public string Result { get; set; }
        [StringLength(200)]
        public string AcceptanceCriteria { get; set; }
        [StringLength(200)]
        public string Points100Sqy { get; set; }
        [StringLength(200)]
        public string LengthOriginal { get; set; }
        [StringLength(200)]
        public string LengthActual { get; set; }
        [StringLength(200)]
        public string WeightOriginal { get; set; }
        [StringLength(200)]
        public string WeightActual { get; set; }
        [StringLength(200)]
        public string WidthOriginal { get; set; }
        [StringLength(200)]
        public string WidthActual { get; set; }
        [StringLength(100)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Location { get; set; }
        [StringLength(200)]
        public string Point { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("FbreportdetailsId")]
        [InverseProperty("FbReportFabricDefects")]
        public virtual FbReportDetail Fbreportdetails { get; set; }
        [ForeignKey("InspColorTransactionId")]
        [InverseProperty("FbReportFabricDefects")]
        public virtual InspPurchaseOrderColorTransaction InspColorTransaction { get; set; }
        [ForeignKey("InspPoTransactionId")]
        [InverseProperty("FbReportFabricDefects")]
        public virtual InspPurchaseOrderTransaction InspPoTransaction { get; set; }
    }
}