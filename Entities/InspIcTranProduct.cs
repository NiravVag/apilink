using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_IC_TRAN_Products")]
    public partial class InspIcTranProduct
    {
        public int Id { get; set; }
        [Column("ICId")]
        public int? Icid { get; set; }
        public int? ShipmentQty { get; set; }
        public int? BookingProductId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public int? PoColorId { get; set; }

        [ForeignKey("BookingProductId")]
        [InverseProperty("InspIcTranProducts")]
        public virtual InspPurchaseOrderTransaction BookingProduct { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspIcTranProductCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspIcTranProductDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("Icid")]
        [InverseProperty("InspIcTranProducts")]
        public virtual InspIcTransaction Ic { get; set; }
        [ForeignKey("PoColorId")]
        [InverseProperty("InspIcTranProducts")]
        public virtual InspPurchaseOrderColorTransaction PoColor { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspIcTranProductUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}