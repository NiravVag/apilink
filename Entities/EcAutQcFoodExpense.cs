using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_AUT_QC_FoodExpense")]
    public partial class EcAutQcFoodExpense
    {
        public EcAutQcFoodExpense()
        {
            EcExpensesClaimDetais = new HashSet<EcExpensesClaimDetai>();
        }

        public int Id { get; set; }
        public int? QcId { get; set; }
        public int? InspectionId { get; set; }
        public int? FactoryCountry { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceDate { get; set; }
        public double? FoodAllowance { get; set; }
        public int? FoodAllowanceCurrency { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }
        public bool? IsExpenseCreated { get; set; }
        public bool? IsFoodAllowanceConfigured { get; set; }
        [StringLength(2500)]
        public string Comments { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EcAutQcFoodExpenseCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EcAutQcFoodExpenseDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FactoryCountry")]
        [InverseProperty("EcAutQcFoodExpenses")]
        public virtual RefCountry FactoryCountryNavigation { get; set; }
        [ForeignKey("FoodAllowanceCurrency")]
        [InverseProperty("EcAutQcFoodExpenses")]
        public virtual RefCurrency FoodAllowanceCurrencyNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("EcAutQcFoodExpenses")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("QcId")]
        [InverseProperty("EcAutQcFoodExpenses")]
        public virtual HrStaff Qc { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("EcAutQcFoodExpenseUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("QcFoodExpense")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetais { get; set; }
    }
}