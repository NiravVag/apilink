using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_AUT_QC_TravelExpense")]
    public partial class EcAutQcTravelExpense
    {
        public EcAutQcTravelExpense()
        {
            EcExpensesClaimDetais = new HashSet<EcExpensesClaimDetai>();
        }

        public int Id { get; set; }
        public int? QcId { get; set; }
        public int? InspectionId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceDate { get; set; }
        public int? StartPort { get; set; }
        public int? FactoryTown { get; set; }
        public int? TripType { get; set; }
        public double? TravelTariff { get; set; }
        public int? TravelTariffCurrency { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }
        public bool? IsExpenseCreated { get; set; }
        public bool? IsTravelAllowanceConfigured { get; set; }
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
        [InverseProperty("EcAutQcTravelExpenseCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EcAutQcTravelExpenseDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FactoryTown")]
        [InverseProperty("EcAutQcTravelExpenses")]
        public virtual RefTown FactoryTownNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("EcAutQcTravelExpenses")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("QcId")]
        [InverseProperty("EcAutQcTravelExpenses")]
        public virtual HrStaff Qc { get; set; }
        [ForeignKey("StartPort")]
        [InverseProperty("EcAutQcTravelExpenses")]
        public virtual EcAutRefStartPort StartPortNavigation { get; set; }
        [ForeignKey("TravelTariffCurrency")]
        [InverseProperty("EcAutQcTravelExpenses")]
        public virtual RefCurrency TravelTariffCurrencyNavigation { get; set; }
        [ForeignKey("TripType")]
        [InverseProperty("EcAutQcTravelExpenses")]
        public virtual EcAutRefTripType TripTypeNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("EcAutQcTravelExpenseUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("QcTravelExpense")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetais { get; set; }
    }
}