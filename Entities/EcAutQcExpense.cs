using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_AUT_QC_Expense")]
    public partial class EcAutQcExpense
    {
        public int Id { get; set; }
        public int? InspectionId { get; set; }
        public int? QcId { get; set; }
        public int? StartPort { get; set; }
        public int? FactoryTown { get; set; }
        public int? TripType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceDate { get; set; }
        public double? TravelTariff { get; set; }
        public int? TravelTariffCurrency { get; set; }
        public double? FoodAllowance { get; set; }
        public int? FoodAllowanceCurrency { get; set; }
        public int? EntityId { get; set; }
        public bool? Active { get; set; }
        public bool? IsExpenseCreated { get; set; }
        public bool? IsTravelAllowanceConfigured { get; set; }
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
        [InverseProperty("EcAutQcExpenseCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EcAutQcExpenseDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FactoryTown")]
        [InverseProperty("EcAutQcExpenses")]
        public virtual RefTown FactoryTownNavigation { get; set; }
        [ForeignKey("FoodAllowanceCurrency")]
        [InverseProperty("EcAutQcExpenseFoodAllowanceCurrencyNavigations")]
        public virtual RefCurrency FoodAllowanceCurrencyNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("EcAutQcExpenses")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("QcId")]
        [InverseProperty("EcAutQcExpenses")]
        public virtual HrStaff Qc { get; set; }
        [ForeignKey("StartPort")]
        [InverseProperty("EcAutQcExpenses")]
        public virtual EcAutRefStartPort StartPortNavigation { get; set; }
        [ForeignKey("TravelTariffCurrency")]
        [InverseProperty("EcAutQcExpenseTravelTariffCurrencyNavigations")]
        public virtual RefCurrency TravelTariffCurrencyNavigation { get; set; }
        [ForeignKey("TripType")]
        [InverseProperty("EcAutQcExpenses")]
        public virtual EcAutRefTripType TripTypeNavigation { get; set; }
    }
}