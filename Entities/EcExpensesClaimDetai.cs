using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_ExpensesClaimDetais")]
    public partial class EcExpensesClaimDetai
    {
        public EcExpensesClaimDetai()
        {
            EcExpenseClaimsAudits = new HashSet<EcExpenseClaimsAudit>();
            EcExpenseClaimsInspections = new HashSet<EcExpenseClaimsInspection>();
            EcReceiptFileAttachments = new HashSet<EcReceiptFileAttachment>();
            EcReceiptFiles = new HashSet<EcReceiptFile>();
        }

        public int Id { get; set; }
        public int ExpenseId { get; set; }
        public bool Receipt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExpenseDate { get; set; }
        public int ExpenseTypeId { get; set; }
        public int? StartCityId { get; set; }
        public int? ArrivalCityId { get; set; }
        public int CurrencyId { get; set; }
        public double Amount { get; set; }
        [Column("Ammount_HK")]
        public double? AmmountHk { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [Column("Exchange_Rate")]
        public double? ExchangeRate { get; set; }
        public double? TransitTime { get; set; }
        [Required]
        [Column("active")]
        public bool? Active { get; set; }
        public int? PayrollCurrencyId { get; set; }
        public double? AmountPayroll { get; set; }
        public double? PayrollExchangeRate { get; set; }
        public int? EntityId { get; set; }
        public int? TripType { get; set; }
        [Column("Qc_Auto_ExpenseId")]
        public int? QcAutoExpenseId { get; set; }
        public bool? IsAutoExpense { get; set; }
        [Column("IsManagerApproved ")]
        public bool? IsManagerApproved { get; set; }
        [Column("Qc_Travel_ExpenseId")]
        public int? QcTravelExpenseId { get; set; }
        [Column("Qc_Food_ExpenseId")]
        public int? QcFoodExpenseId { get; set; }
        public int? InspectionId { get; set; }
        public int? AuditId { get; set; }
        public int? ManDay { get; set; }
        public double? Tax { get; set; }
        public double? TaxAmount { get; set; }

        [ForeignKey("ArrivalCityId")]
        [InverseProperty("EcExpensesClaimDetaiArrivalCities")]
        public virtual RefCity ArrivalCity { get; set; }
        [ForeignKey("AuditId")]
        [InverseProperty("EcExpensesClaimDetais")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CurrencyId")]
        [InverseProperty("EcExpensesClaimDetaiCurrencies")]
        public virtual RefCurrency Currency { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EcExpensesClaimDetais")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ExpenseId")]
        [InverseProperty("EcExpensesClaimDetais")]
        public virtual EcExpencesClaim Expense { get; set; }
        [ForeignKey("ExpenseTypeId")]
        [InverseProperty("EcExpensesClaimDetais")]
        public virtual EcExpensesType ExpenseType { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("EcExpensesClaimDetais")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("PayrollCurrencyId")]
        [InverseProperty("EcExpensesClaimDetaiPayrollCurrencies")]
        public virtual RefCurrency PayrollCurrency { get; set; }
        [ForeignKey("QcFoodExpenseId")]
        [InverseProperty("EcExpensesClaimDetais")]
        public virtual EcAutQcFoodExpense QcFoodExpense { get; set; }
        [ForeignKey("QcTravelExpenseId")]
        [InverseProperty("EcExpensesClaimDetais")]
        public virtual EcAutQcTravelExpense QcTravelExpense { get; set; }
        [ForeignKey("StartCityId")]
        [InverseProperty("EcExpensesClaimDetaiStartCities")]
        public virtual RefCity StartCity { get; set; }
        [ForeignKey("TripType")]
        [InverseProperty("EcExpensesClaimDetais")]
        public virtual EcAutRefTripType TripTypeNavigation { get; set; }
        [InverseProperty("ExpenseClaimDetail")]
        public virtual ICollection<EcExpenseClaimsAudit> EcExpenseClaimsAudits { get; set; }
        [InverseProperty("ExpenseClaimDetail")]
        public virtual ICollection<EcExpenseClaimsInspection> EcExpenseClaimsInspections { get; set; }
        [InverseProperty("Expense")]
        public virtual ICollection<EcReceiptFileAttachment> EcReceiptFileAttachments { get; set; }
        [InverseProperty("Expense")]
        public virtual ICollection<EcReceiptFile> EcReceiptFiles { get; set; }
    }
}