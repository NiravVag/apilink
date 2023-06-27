using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_PurchaseOrder_Transaction")]
    public partial class InspPurchaseOrderTransaction
    {
        public InspPurchaseOrderTransaction()
        {
            FbReportFabricDefects = new HashSet<FbReportFabricDefect>();
            FbReportInspDefects = new HashSet<FbReportInspDefect>();
            FbReportQuantityDetails = new HashSet<FbReportQuantityDetail>();
            FbReportRdnumbers = new HashSet<FbReportRdnumber>();
            InspIcTranProducts = new HashSet<InspIcTranProduct>();
            InspPurchaseOrderColorTransactions = new HashSet<InspPurchaseOrderColorTransaction>();
            InspTranPickings = new HashSet<InspTranPicking>();
        }

        public int Id { get; set; }
        [Column("PO_Id")]
        public int PoId { get; set; }
        [Column("Container_Ref_Id")]
        public int? ContainerRefId { get; set; }
        [Column("Product_Ref_Id")]
        public int ProductRefId { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        public int BookingQuantity { get; set; }
        public int? PickingQuantity { get; set; }
        public string Remarks { get; set; }
        [Column("Destination_Country_Id")]
        public int? DestinationCountryId { get; set; }
        [Column("ETD", TypeName = "datetime")]
        public DateTime? Etd { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        [StringLength(1000)]
        public string CustomerReferencePo { get; set; }
        [Column("Fb_Mission_Product_Id")]
        public int? FbMissionProductId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("ContainerRefId")]
        [InverseProperty("InspPurchaseOrderTransactions")]
        public virtual InspContainerTransaction ContainerRef { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspPurchaseOrderTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspPurchaseOrderTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DestinationCountryId")]
        [InverseProperty("InspPurchaseOrderTransactions")]
        public virtual RefCountry DestinationCountry { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspPurchaseOrderTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspPurchaseOrderTransactions")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("PoId")]
        [InverseProperty("InspPurchaseOrderTransactions")]
        public virtual CuPurchaseOrder Po { get; set; }
        [ForeignKey("ProductRefId")]
        [InverseProperty("InspPurchaseOrderTransactions")]
        public virtual InspProductTransaction ProductRef { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspPurchaseOrderTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("InspPoTransaction")]
        public virtual ICollection<FbReportFabricDefect> FbReportFabricDefects { get; set; }
        [InverseProperty("InspPoTransaction")]
        public virtual ICollection<FbReportInspDefect> FbReportInspDefects { get; set; }
        [InverseProperty("InspPoTransaction")]
        public virtual ICollection<FbReportQuantityDetail> FbReportQuantityDetails { get; set; }
        [InverseProperty("Po")]
        public virtual ICollection<FbReportRdnumber> FbReportRdnumbers { get; set; }
        [InverseProperty("BookingProduct")]
        public virtual ICollection<InspIcTranProduct> InspIcTranProducts { get; set; }
        [InverseProperty("PoTrans")]
        public virtual ICollection<InspPurchaseOrderColorTransaction> InspPurchaseOrderColorTransactions { get; set; }
        [InverseProperty("PoTran")]
        public virtual ICollection<InspTranPicking> InspTranPickings { get; set; }
    }
}