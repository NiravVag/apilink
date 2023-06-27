using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_Buyer")]
    public partial class CuPrBuyer
    {
        public int Id { get; set; }
        [Column("Cu_Price_Id")]
        public int CuPriceId { get; set; }
        [Column("Buyer_Id")]
        public int? BuyerId { get; set; }
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

        [ForeignKey("BuyerId")]
        [InverseProperty("CuPrBuyers")]
        public virtual CuBuyer Buyer { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrBuyerCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPriceId")]
        [InverseProperty("CuPrBuyers")]
        public virtual CuPrDetail CuPrice { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrBuyerDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrBuyerUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}