using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Buyer_API_Services")]
    public partial class CuBuyerApiService
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int ServiceId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("CuBuyerApiServices")]
        public virtual CuBuyer Buyer { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuBuyerApiServiceCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuBuyerApiServiceDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuBuyerApiServices")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("CuBuyerApiServices")]
        public virtual RefService Service { get; set; }
    }
}