using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PurchaseOrder_Details")]
    public partial class CuPurchaseOrderDetail
    {
        public int Id { get; set; }
        [Column("PO_Id")]
        public int PoId { get; set; }
        [Column("Product_Id")]
        public int ProductId { get; set; }
        [Column("Unit_Id")]
        public int? UnitId { get; set; }
        [Column("Destination_Country_Id")]
        public int? DestinationCountryId { get; set; }
        [Column("ETD", TypeName = "datetime")]
        public DateTime? Etd { get; set; }
        [StringLength(1000)]
        public string FactoryReference { get; set; }
        public int Quantity { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedTime { get; set; }
        [Column("Booking_Status")]
        public int? BookingStatus { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPurchaseOrderDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPurchaseOrderDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DestinationCountryId")]
        [InverseProperty("CuPurchaseOrderDetails")]
        public virtual RefCountry DestinationCountry { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuPurchaseOrderDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("PoId")]
        [InverseProperty("CuPurchaseOrderDetails")]
        public virtual CuPurchaseOrder Po { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("CuPurchaseOrderDetails")]
        public virtual CuProduct Product { get; set; }
        [ForeignKey("UnitId")]
        [InverseProperty("CuPurchaseOrderDetails")]
        public virtual RefProductUnit Unit { get; set; }
    }
}