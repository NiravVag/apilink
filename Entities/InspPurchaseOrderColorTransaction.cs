using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_PurchaseOrder_Color_Transaction")]
    public partial class InspPurchaseOrderColorTransaction
    {
        public InspPurchaseOrderColorTransaction()
        {
            FbReportFabricDefects = new HashSet<FbReportFabricDefect>();
            FbReportInspDefects = new HashSet<FbReportInspDefect>();
            FbReportQuantityDetails = new HashSet<FbReportQuantityDetail>();
            FbReportRdnumbers = new HashSet<FbReportRdnumber>();
            InspIcTranProducts = new HashSet<InspIcTranProduct>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string ColorCode { get; set; }
        [StringLength(50)]
        public string ColorName { get; set; }
        public int? PoTransId { get; set; }
        public int? ProductRefId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }
        public int? BookingQuantity { get; set; }
        public int? PickingQuantity { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspPurchaseOrderColorTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspPurchaseOrderColorTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspPurchaseOrderColorTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("PoTransId")]
        [InverseProperty("InspPurchaseOrderColorTransactions")]
        public virtual InspPurchaseOrderTransaction PoTrans { get; set; }
        [ForeignKey("ProductRefId")]
        [InverseProperty("InspPurchaseOrderColorTransactions")]
        public virtual InspProductTransaction ProductRef { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspPurchaseOrderColorTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("InspColorTransaction")]
        public virtual ICollection<FbReportFabricDefect> FbReportFabricDefects { get; set; }
        [InverseProperty("InspColorTransaction")]
        public virtual ICollection<FbReportInspDefect> FbReportInspDefects { get; set; }
        [InverseProperty("InspColorTransaction")]
        public virtual ICollection<FbReportQuantityDetail> FbReportQuantityDetails { get; set; }
        [InverseProperty("PoColor")]
        public virtual ICollection<FbReportRdnumber> FbReportRdnumbers { get; set; }
        [InverseProperty("PoColor")]
        public virtual ICollection<InspIcTranProduct> InspIcTranProducts { get; set; }
    }
}