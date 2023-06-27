using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Quantity_Details")]
    public partial class FbReportQuantityDetail
    {
        public int Id { get; set; }
        public int FbReportDetailId { get; set; }
        public int InspPoTransactionId { get; set; }
        public double? OrderQuantity { get; set; }
        public double? PresentedQuantity { get; set; }
        public double? InspectedQuantity { get; set; }
        public double? ShipmentQuantity { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public double? ProductionStatus { get; set; }
        public double? PackingStatus { get; set; }
        public double? TotalUnits { get; set; }
        public double? TotalCartons { get; set; }
        public double? FinishedPackedUnits { get; set; }
        public double? FinishedUnpackedUnits { get; set; }
        public double? NotFinishedUnits { get; set; }
        public double? SelectCtnQty { get; set; }
        [Column("SelectCtnNO")]
        public string SelectCtnNo { get; set; }
        public int? InspColorTransactionId { get; set; }
        [Column("Fabric_Points100Sqy")]
        public double? FabricPoints100Sqy { get; set; }
        [Column("Fabric_AcceptanceCriteria")]
        public double? FabricAcceptanceCriteria { get; set; }
        public double? ProducedQuantity { get; set; }
        [Column("Fabric_OverLessProducedQty")]
        [StringLength(100)]
        public string FabricOverLessProducedQty { get; set; }
        [Column("Fabric_RejectedQuantity")]
        public double? FabricRejectedQuantity { get; set; }
        [Column("Fabric_RejectedRolls")]
        public double? FabricRejectedRolls { get; set; }
        [Column("Fabric_DemeritPts")]
        public double? FabricDemeritPts { get; set; }
        [Column("Fabric_Tolerance")]
        public double? FabricTolerance { get; set; }
        [Column("Fabric_Rating")]
        [StringLength(100)]
        public string FabricRating { get; set; }

        [ForeignKey("FbReportDetailId")]
        [InverseProperty("FbReportQuantityDetails")]
        public virtual FbReportDetail FbReportDetail { get; set; }
        [ForeignKey("InspColorTransactionId")]
        [InverseProperty("FbReportQuantityDetails")]
        public virtual InspPurchaseOrderColorTransaction InspColorTransaction { get; set; }
        [ForeignKey("InspPoTransactionId")]
        [InverseProperty("FbReportQuantityDetails")]
        public virtual InspPurchaseOrderTransaction InspPoTransaction { get; set; }
    }
}