using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_Details")]
    public partial class CuPrDetail
    {
        public CuPrDetail()
        {
            CuPrBrands = new HashSet<CuPrBrand>();
            CuPrBuyers = new HashSet<CuPrBuyer>();
            CuPrCities = new HashSet<CuPrCity>();
            CuPrCountries = new HashSet<CuPrCountry>();
            CuPrDepartments = new HashSet<CuPrDepartment>();
            CuPrHolidayTypes = new HashSet<CuPrHolidayType>();
            CuPrInspectionLocations = new HashSet<CuPrInspectionLocation>();
            CuPrPriceCategories = new HashSet<CuPrPriceCategory>();
            CuPrProductCategories = new HashSet<CuPrProductCategory>();
            CuPrProductSubCategories = new HashSet<CuPrProductSubCategory>();
            CuPrProvinces = new HashSet<CuPrProvince>();
            CuPrServiceTypes = new HashSet<CuPrServiceType>();
            CuPrSuppliers = new HashSet<CuPrSupplier>();
            CuPrTranSpecialRules = new HashSet<CuPrTranSpecialRule>();
            CuPrTranSubcategories = new HashSet<CuPrTranSubcategory>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvTranInvoiceRequestContacts = new HashSet<InvTranInvoiceRequestContact>();
            InvTranInvoiceRequests = new HashSet<InvTranInvoiceRequest>();
            QuQuotations = new HashSet<QuQuotation>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int BillingMethodId { get; set; }
        public int BillingToId { get; set; }
        public int ServiceId { get; set; }
        public int CurrencyId { get; set; }
        public double UnitPrice { get; set; }
        public bool? TaxIncluded { get; set; }
        public bool? TravelIncluded { get; set; }
        [Column("FreeTravelKM")]
        public int? FreeTravelKm { get; set; }
        [StringLength(3000)]
        public string Remarks { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PeriodFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PeriodTo { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? TravelMatrixTypeId { get; set; }
        public double? HolidayPrice { get; set; }
        public double? ProductPrice { get; set; }
        public bool? PriceToEachProduct { get; set; }
        public bool? InvoiceRequestSelectAll { get; set; }
        public bool? IsInvoiceConfigured { get; set; }
        public int? InvoiceRequestType { get; set; }
        public string InvoiceRequestAddress { get; set; }
        [StringLength(2000)]
        public string InvoiceRequestBilledName { get; set; }
        public int? BillingEntity { get; set; }
        public int? BankAccount { get; set; }
        public int? PaymentDuration { get; set; }
        [StringLength(100)]
        public string PaymentTerms { get; set; }
        [StringLength(100)]
        public string InvoiceNoDigit { get; set; }
        [StringLength(100)]
        public string InvoiceNoPrefix { get; set; }
        public int? InvoiceInspFeeFrom { get; set; }
        [Column("InvoiceTMFeeFrom")]
        public int? InvoiceTmfeeFrom { get; set; }
        public int? InvoiceHotelFeeFrom { get; set; }
        public int? InvoiceOtherFeeFrom { get; set; }
        public int? InvoiceDiscountFeeFrom { get; set; }
        public int? InvoiceOffice { get; set; }
        public int? MaxProductCount { get; set; }
        public bool? SampleSizeBySet { get; set; }
        public double? MinBillingDay { get; set; }
        public int? MaxSampleSize { get; set; }
        public int? AdditionalSampleSize { get; set; }
        public double? AdditionalSamplePrice { get; set; }
        public double? Quantity8 { get; set; }
        public double? Quantity13 { get; set; }
        public double? Quantity20 { get; set; }
        public double? Quantity32 { get; set; }
        public double? Quantity50 { get; set; }
        public double? Quantity80 { get; set; }
        public double? Quantity125 { get; set; }
        public double? Quantity200 { get; set; }
        public double? Quantity315 { get; set; }
        public double? Quantity500 { get; set; }
        public double? Quantity800 { get; set; }
        public double? Quantity1250 { get; set; }
        public int? EntityId { get; set; }
        public int? BilledQuantityType { get; set; }
        public double? MaxFeeStyle { get; set; }
        [StringLength(1000)]
        public string InvoiceSubject { get; set; }
        public int? BillingFreequency { get; set; }
        [Column("ManDay_Productivity")]
        public double? ManDayProductivity { get; set; }
        [Column("Manday_ReportCount")]
        public int? MandayReportCount { get; set; }
        [Column("Manday_Buffer")]
        public double? MandayBuffer { get; set; }
        public bool? SubCategorySelectAll { get; set; }
        public bool? IsSpecial { get; set; }
        public int? InterventionType { get; set; }
        public int? PriceComplexType { get; set; }

        [ForeignKey("BankAccount")]
        [InverseProperty("CuPrDetails")]
        public virtual InvRefBank BankAccountNavigation { get; set; }
        [ForeignKey("BilledQuantityType")]
        [InverseProperty("CuPrDetails")]
        public virtual InspRefQuantityType BilledQuantityTypeNavigation { get; set; }
        [ForeignKey("BillingEntity")]
        [InverseProperty("CuPrDetails")]
        public virtual RefBillingEntity BillingEntityNavigation { get; set; }
        [ForeignKey("BillingFreequency")]
        [InverseProperty("CuPrDetails")]
        public virtual InvRefBillingFreequency BillingFreequencyNavigation { get; set; }
        [ForeignKey("BillingMethodId")]
        [InverseProperty("CuPrDetails")]
        public virtual QuBillMethod BillingMethod { get; set; }
        [ForeignKey("BillingToId")]
        [InverseProperty("CuPrDetails")]
        public virtual QuPaidBy BillingTo { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CurrencyId")]
        [InverseProperty("CuPrDetails")]
        public virtual RefCurrency Currency { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuPrDetails")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuPrDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("InterventionType")]
        [InverseProperty("CuPrDetails")]
        public virtual InvRefInterventionType InterventionTypeNavigation { get; set; }
        [ForeignKey("InvoiceDiscountFeeFrom")]
        [InverseProperty("CuPrDetailInvoiceDiscountFeeFromNavigations")]
        public virtual InvRefFeesFrom InvoiceDiscountFeeFromNavigation { get; set; }
        [ForeignKey("InvoiceHotelFeeFrom")]
        [InverseProperty("CuPrDetailInvoiceHotelFeeFromNavigations")]
        public virtual InvRefFeesFrom InvoiceHotelFeeFromNavigation { get; set; }
        [ForeignKey("InvoiceInspFeeFrom")]
        [InverseProperty("CuPrDetailInvoiceInspFeeFromNavigations")]
        public virtual InvRefFeesFrom InvoiceInspFeeFromNavigation { get; set; }
        [ForeignKey("InvoiceOffice")]
        [InverseProperty("CuPrDetails")]
        public virtual InvRefOffice InvoiceOfficeNavigation { get; set; }
        [ForeignKey("InvoiceOtherFeeFrom")]
        [InverseProperty("CuPrDetailInvoiceOtherFeeFromNavigations")]
        public virtual InvRefFeesFrom InvoiceOtherFeeFromNavigation { get; set; }
        [ForeignKey("InvoiceRequestType")]
        [InverseProperty("CuPrDetails")]
        public virtual InvRefRequestType InvoiceRequestTypeNavigation { get; set; }
        [ForeignKey("InvoiceTmfeeFrom")]
        [InverseProperty("CuPrDetailInvoiceTmfeeFromNavigations")]
        public virtual InvRefFeesFrom InvoiceTmfeeFromNavigation { get; set; }
        [ForeignKey("PriceComplexType")]
        [InverseProperty("CuPrDetails")]
        public virtual CuPrRefComplexType PriceComplexTypeNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("CuPrDetails")]
        public virtual RefService Service { get; set; }
        [ForeignKey("TravelMatrixTypeId")]
        [InverseProperty("CuPrDetails")]
        public virtual InvTmType TravelMatrixType { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("CuPrice")]
        public virtual ICollection<CuPrBrand> CuPrBrands { get; set; }
        [InverseProperty("CuPrice")]
        public virtual ICollection<CuPrBuyer> CuPrBuyers { get; set; }
        [InverseProperty("CuPr")]
        public virtual ICollection<CuPrCity> CuPrCities { get; set; }
        [InverseProperty("CuPr")]
        public virtual ICollection<CuPrCountry> CuPrCountries { get; set; }
        [InverseProperty("CuPrice")]
        public virtual ICollection<CuPrDepartment> CuPrDepartments { get; set; }
        [InverseProperty("CuPrice")]
        public virtual ICollection<CuPrHolidayType> CuPrHolidayTypes { get; set; }
        [InverseProperty("CuPrice")]
        public virtual ICollection<CuPrInspectionLocation> CuPrInspectionLocations { get; set; }
        [InverseProperty("CuPrice")]
        public virtual ICollection<CuPrPriceCategory> CuPrPriceCategories { get; set; }
        [InverseProperty("CuPr")]
        public virtual ICollection<CuPrProductCategory> CuPrProductCategories { get; set; }
        [InverseProperty("CuPr")]
        public virtual ICollection<CuPrProductSubCategory> CuPrProductSubCategories { get; set; }
        [InverseProperty("CuPr")]
        public virtual ICollection<CuPrProvince> CuPrProvinces { get; set; }
        [InverseProperty("CuPr")]
        public virtual ICollection<CuPrServiceType> CuPrServiceTypes { get; set; }
        [InverseProperty("CuPr")]
        public virtual ICollection<CuPrSupplier> CuPrSuppliers { get; set; }
        [InverseProperty("CuPrice")]
        public virtual ICollection<CuPrTranSpecialRule> CuPrTranSpecialRules { get; set; }
        [InverseProperty("CuPrice")]
        public virtual ICollection<CuPrTranSubcategory> CuPrTranSubcategories { get; set; }
        [InverseProperty("Rule")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("CuPriceCard")]
        public virtual ICollection<InvTranInvoiceRequestContact> InvTranInvoiceRequestContacts { get; set; }
        [InverseProperty("CuPriceCard")]
        public virtual ICollection<InvTranInvoiceRequest> InvTranInvoiceRequests { get; set; }
        [InverseProperty("Rule")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
    }
}