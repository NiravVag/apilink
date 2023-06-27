using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_TRAN_SpecialRule")]
    public partial class CuPrTranSpecialRule
    {
        public int Id { get; set; }
        [Column("Cu_Price_Id")]
        public int? CuPriceId { get; set; }
        public int? MandayProductivity { get; set; }
        public int? MandayReports { get; set; }
        public double? UnitPrice { get; set; }
        [Column("PieceRate_Billing_Q_Start")]
        public int? PieceRateBillingQStart { get; set; }
        [Column("Piecerate_Billing_Q_End")]
        public int? PiecerateBillingQEnd { get; set; }
        public double? AdditionalFee { get; set; }
        [Column("Piecerate_MinBilling")]
        public double? PiecerateMinBilling { get; set; }
        public int? PerInterventionRange1 { get; set; }
        public int? PerInterventionRange2 { get; set; }
        [Column("Max_Style_Per_Day")]
        public double? MaxStylePerDay { get; set; }
        [Column("Max_Style_Per_Week")]
        public double? MaxStylePerWeek { get; set; }
        [Column("Max_Style_Per_Month")]
        public double? MaxStylePerMonth { get; set; }
        public double? InterventionFee { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrTranSpecialRuleCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPriceId")]
        [InverseProperty("CuPrTranSpecialRules")]
        public virtual CuPrDetail CuPrice { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrTranSpecialRuleDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}