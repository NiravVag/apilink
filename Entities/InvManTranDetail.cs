using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_MAN_TRAN_Details")]
    public partial class InvManTranDetail
    {
        public int Id { get; set; }
        [Column("Inv_ManualId")]
        public int InvManualId { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public double? ServiceFee { get; set; }
        public double? ExpChargeBack { get; set; }
        public double? OtherCost { get; set; }
        public double? Subtotal { get; set; }
        [StringLength(2000)]
        public string Remarks { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public double? Discount { get; set; }
        public double? UnitPrice { get; set; }
        public double? Manday { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvManTranDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvManTranDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InvManualId")]
        [InverseProperty("InvManTranDetails")]
        public virtual InvManTransaction InvManual { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvManTranDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}