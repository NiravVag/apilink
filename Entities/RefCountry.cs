using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Country")]
    public partial class RefCountry
    {
        public RefCountry()
        {
            AudBookingContacts = new HashSet<AudBookingContact>();
            AudBookingEmailConfigurations = new HashSet<AudBookingEmailConfiguration>();
            AudBookingRules = new HashSet<AudBookingRule>();
            CompComplaints = new HashSet<CompComplaint>();
            CuAddresses = new HashSet<CuAddress>();
            CuCheckPointsCountries = new HashSet<CuCheckPointsCountry>();            
            CuCustomerBusinessCountries = new HashSet<CuCustomerBusinessCountry>();
            CuCustomerSalesCountries = new HashSet<CuCustomerSalesCountry>();
            CuCustomers = new HashSet<CuCustomer>();
            CuPrCountries = new HashSet<CuPrCountry>();
            CuPurchaseOrderDetails = new HashSet<CuPurchaseOrderDetail>();
            DaUserByFactoryCountries = new HashSet<DaUserByFactoryCountry>();
            EcAutQcFoodExpenses = new HashSet<EcAutQcFoodExpense>();
            EcExpencesClaims = new HashSet<EcExpencesClaim>();
            EcFoodAllowances = new HashSet<EcFoodAllowance>();
            EntFeatureDetails = new HashSet<EntFeatureDetail>();
            EsFaCountryConfigs = new HashSet<EsFaCountryConfig>();
            HrHolidays = new HashSet<HrHoliday>();
            HrStaffOpCountries = new HashSet<HrStaffOpCountry>();
            HrStaffs = new HashSet<HrStaff>();
            InspBookingEmailConfigurations = new HashSet<InspBookingEmailConfiguration>();
            InspBookingRules = new HashSet<InspBookingRule>();
            InspLabAddresses = new HashSet<InspLabAddress>();
            InspPurchaseOrderTransactions = new HashSet<InspPurchaseOrderTransaction>();
            InvDisTranCountries = new HashSet<InvDisTranCountry>();
            InvManTransactions = new HashSet<InvManTransaction>();
            InvTmDetails = new HashSet<InvTmDetail>();
            OmDetailOfficeCountries = new HashSet<OmDetail>();
            OmDetailOperationalCountries = new HashSet<OmDetail>();
            QuQuotations = new HashSet<QuQuotation>();
            RefBudgetForecasts = new HashSet<RefBudgetForecast>();
            RefCountryLocations = new HashSet<RefCountryLocation>();
            RefLocationCountries = new HashSet<RefLocationCountry>();
            RefProvinces = new HashSet<RefProvince>();
            SuAddresses = new HashSet<SuAddress>();
        }

        public int Id { get; set; }
        [Column("Country_Code")]
        public int? CountryCode { get; set; }
        [StringLength(2)]
        public string Alpha2Code { get; set; }
        [Column("Area_id")]
        public int? AreaId { get; set; }
        [Required]
        [Column("Country_Name")]
        [StringLength(50)]
        public string CountryName { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }
        [Column("FB_CountryId")]
        public int? FbCountryId { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal? Longitude { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal? Latitude { get; set; }

        [ForeignKey("AreaId")]
        [InverseProperty("RefCountries")]
        public virtual RefArea Area { get; set; }
        [InverseProperty("FactoryCountry")]
        public virtual ICollection<AudBookingContact> AudBookingContacts { get; set; }
        [InverseProperty("FactoryCountry")]
        public virtual ICollection<AudBookingEmailConfiguration> AudBookingEmailConfigurations { get; set; }
        [InverseProperty("FactoryCountry")]
        public virtual ICollection<AudBookingRule> AudBookingRules { get; set; }
        [InverseProperty("CountryNavigation")]
        public virtual ICollection<CompComplaint> CompComplaints { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<CuAddress> CuAddresses { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<CuCheckPointsCountry> CuCheckPointsCountries { get; set; }   
        [InverseProperty("BusinessCountry")]
        public virtual ICollection<CuCustomerBusinessCountry> CuCustomerBusinessCountries { get; set; }
        [InverseProperty("SalesCountry")]
        public virtual ICollection<CuCustomerSalesCountry> CuCustomerSalesCountries { get; set; }
        [InverseProperty("BusinessCountryNavigation")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
        [InverseProperty("FactoryCountry")]
        public virtual ICollection<CuPrCountry> CuPrCountries { get; set; }
        [InverseProperty("DestinationCountry")]
        public virtual ICollection<CuPurchaseOrderDetail> CuPurchaseOrderDetails { get; set; }
        [InverseProperty("FactoryCountry")]
        public virtual ICollection<DaUserByFactoryCountry> DaUserByFactoryCountries { get; set; }
        [InverseProperty("FactoryCountryNavigation")]
        public virtual ICollection<EcAutQcFoodExpense> EcAutQcFoodExpenses { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaims { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<EcFoodAllowance> EcFoodAllowances { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<EntFeatureDetail> EntFeatureDetails { get; set; }
        [InverseProperty("FactoryCountry")]
        public virtual ICollection<EsFaCountryConfig> EsFaCountryConfigs { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<HrHoliday> HrHolidays { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<HrStaffOpCountry> HrStaffOpCountries { get; set; }
        [InverseProperty("NationalityCountry")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
        [InverseProperty("FactoryCountry")]
        public virtual ICollection<InspBookingEmailConfiguration> InspBookingEmailConfigurations { get; set; }
        [InverseProperty("FactoryCountry")]
        public virtual ICollection<InspBookingRule> InspBookingRules { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<InspLabAddress> InspLabAddresses { get; set; }
        [InverseProperty("DestinationCountry")]
        public virtual ICollection<InspPurchaseOrderTransaction> InspPurchaseOrderTransactions { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<InvDisTranCountry> InvDisTranCountries { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<InvTmDetail> InvTmDetails { get; set; }
        [InverseProperty("OfficeCountry")]
        public virtual ICollection<OmDetail> OmDetailOfficeCountries { get; set; }
        [InverseProperty("OperationalCountry")]
        public virtual ICollection<OmDetail> OmDetailOperationalCountries { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<RefBudgetForecast> RefBudgetForecasts { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<RefCountryLocation> RefCountryLocations { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<RefLocationCountry> RefLocationCountries { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<RefProvince> RefProvinces { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<SuAddress> SuAddresses { get; set; }
    }
}