using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_DIS_TRAN_PeriodInfo")]
    public partial class InvDisTranPeriodInfo
    {
        public int Id { get; set; }
        public int DiscountId { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal LimitFrom { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal LimitTo { get; set; }
        public bool? NotificationSent { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public bool? Active { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvDisTranPeriodInfoCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvDisTranPeriodInfoDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DiscountId")]
        [InverseProperty("InvDisTranPeriodInfos")]
        public virtual InvDisTranDetail Discount { get; set; }
    }
}