using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_CU_Buyer")]
    public partial class InspTranCuBuyer
    {
        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        [Column("Buyer_Id")]
        public int BuyerId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("InspTranCuBuyers")]
        public virtual CuBuyer Buyer { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranCuBuyerCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranCuBuyerDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranCuBuyers")]
        public virtual InspTransaction Inspection { get; set; }
    }
}