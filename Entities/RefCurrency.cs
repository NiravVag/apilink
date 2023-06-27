using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Currency")]
    public partial class RefCurrency
    {
        public RefCurrency()
        {
            AudTranCancelReschedules = new HashSet<AudTranCancelReschedule>();
            ClmTransactionClaimRefundCurrencyNavigations = new HashSet<ClmTransaction>();
            ClmTransactionCustomerReqRefundCurrencyNavigations = new HashSet<ClmTransaction>();
            ClmTransactionFobCurrencyNavigations = new HashSet<ClmTransaction>();
            ClmTransactionRealInspectionFeesCurrencyNavigations = new HashSet<ClmTransaction>();
            ClmTransactionRetailCurrencyNavigations = new HashSet<ClmTransaction>();
            CuPrDetails = new HashSet<CuPrDetail>();
            EcAutQcFoodExpenses = new HashSet<EcAutQcFoodExpense>();
            EcAutQcTravelExpenses = new HashSet<EcAutQcTravelExpense>();
            EcAutTravelTariffs = new HashSet<EcAutTravelTariff>();
            EcExpensesClaimDetaiCurrencies = new HashSet<EcExpensesClaimDetai>();
            EcExpensesClaimDetaiPayrollCurrencies = new HashSet<EcExpensesClaimDetai>();
            EcFoodAllowances = new HashSet<EcFoodAllowance>();
            EmExchangeRateCurrencyId1Navigations = new HashSet<EmExchangeRate>();
            EmExchangeRateCurrencyid2Navigations = new HashSet<EmExchangeRate>();
            HrStaffHistories = new HashSet<HrStaffHistory>();
            HrStaffPayrollCurrencies = new HashSet<HrStaff>();
            HrStaffPreferCurrencies = new HashSet<HrStaff>();
            HrStaffSalaryCurrencies = new HashSet<HrStaff>();
            InspTranCancels = new HashSet<InspTranCancel>();
            InspTranReschedules = new HashSet<InspTranReschedule>();
            InvAutTranDetailInvoiceCurrencyNavigations = new HashSet<InvAutTranDetail>();
            InvAutTranDetailPriceCardCurrencyNavigations = new HashSet<InvAutTranDetail>();
            InvCreTransactions = new HashSet<InvCreTransaction>();
            InvExfTransactionCurrencies = new HashSet<InvExfTransaction>();
            InvExfTransactionInvoiceCurrencies = new HashSet<InvExfTransaction>();
            InvManTransactions = new HashSet<InvManTransaction>();
            InvRefBanks = new HashSet<InvRefBank>();
            InvTmDetailSourceCurrencies = new HashSet<InvTmDetail>();
            InvTmDetailTravelCurrencies = new HashSet<InvTmDetail>();
            QuQuotations = new HashSet<QuQuotation>();
            RefBudgetForecasts = new HashSet<RefBudgetForecast>();
            RefLocationDefaultCurrencies = new HashSet<RefLocation>();
            RefLocationMasterCurrencies = new HashSet<RefLocation>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(3)]
        public string CurrencyCodeA { get; set; }
        public int? CurrencyCodeN { get; set; }
        public short? MinorUnit { get; set; }
        [Required]
        [StringLength(50)]
        public string CurrencyName { get; set; }
        [Required]
        [StringLength(50)]
        public string Comment { get; set; }
        public bool Active { get; set; }

        [InverseProperty("Currency")]
        public virtual ICollection<AudTranCancelReschedule> AudTranCancelReschedules { get; set; }
        [InverseProperty("ClaimRefundCurrencyNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionClaimRefundCurrencyNavigations { get; set; }
        [InverseProperty("CustomerReqRefundCurrencyNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionCustomerReqRefundCurrencyNavigations { get; set; }
        [InverseProperty("FobCurrencyNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionFobCurrencyNavigations { get; set; }
        [InverseProperty("RealInspectionFeesCurrencyNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionRealInspectionFeesCurrencyNavigations { get; set; }
        [InverseProperty("RetailCurrencyNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionRetailCurrencyNavigations { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("FoodAllowanceCurrencyNavigation")]
        public virtual ICollection<EcAutQcFoodExpense> EcAutQcFoodExpenses { get; set; }
        [InverseProperty("TravelTariffCurrencyNavigation")]
        public virtual ICollection<EcAutQcTravelExpense> EcAutQcTravelExpenses { get; set; }
        [InverseProperty("TravelCurrencyNavigation")]
        public virtual ICollection<EcAutTravelTariff> EcAutTravelTariffs { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetaiCurrencies { get; set; }
        [InverseProperty("PayrollCurrency")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetaiPayrollCurrencies { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<EcFoodAllowance> EcFoodAllowances { get; set; }
        [InverseProperty("CurrencyId1Navigation")]
        public virtual ICollection<EmExchangeRate> EmExchangeRateCurrencyId1Navigations { get; set; }
        [InverseProperty("Currencyid2Navigation")]
        public virtual ICollection<EmExchangeRate> EmExchangeRateCurrencyid2Navigations { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<HrStaffHistory> HrStaffHistories { get; set; }
        [InverseProperty("PayrollCurrency")]
        public virtual ICollection<HrStaff> HrStaffPayrollCurrencies { get; set; }
        [InverseProperty("PreferCurrency")]
        public virtual ICollection<HrStaff> HrStaffPreferCurrencies { get; set; }
        [InverseProperty("SalaryCurrency")]
        public virtual ICollection<HrStaff> HrStaffSalaryCurrencies { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<InspTranCancel> InspTranCancels { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<InspTranReschedule> InspTranReschedules { get; set; }
        [InverseProperty("InvoiceCurrencyNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetailInvoiceCurrencyNavigations { get; set; }
        [InverseProperty("PriceCardCurrencyNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetailPriceCardCurrencyNavigations { get; set; }
        [InverseProperty("CurrencyNavigation")]
        public virtual ICollection<InvCreTransaction> InvCreTransactions { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<InvExfTransaction> InvExfTransactionCurrencies { get; set; }
        [InverseProperty("InvoiceCurrency")]
        public virtual ICollection<InvExfTransaction> InvExfTransactionInvoiceCurrencies { get; set; }
        [InverseProperty("CurrencyNavigation")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("AccountCurrencyNavigation")]
        public virtual ICollection<InvRefBank> InvRefBanks { get; set; }
        [InverseProperty("SourceCurrency")]
        public virtual ICollection<InvTmDetail> InvTmDetailSourceCurrencies { get; set; }
        [InverseProperty("TravelCurrency")]
        public virtual ICollection<InvTmDetail> InvTmDetailTravelCurrencies { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<RefBudgetForecast> RefBudgetForecasts { get; set; }
        [InverseProperty("DefaultCurrency")]
        public virtual ICollection<RefLocation> RefLocationDefaultCurrencies { get; set; }
        [InverseProperty("MasterCurrency")]
        public virtual ICollection<RefLocation> RefLocationMasterCurrencies { get; set; }
    }
}
