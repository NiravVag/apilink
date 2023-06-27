using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_QUOTATION")]
    public partial class QuQuotation
    {
        public QuQuotation()
        {
            QuInspProducts = new HashSet<QuInspProduct>();
            QuPdfversions = new HashSet<QuPdfversion>();
            QuQuotationAudMandays = new HashSet<QuQuotationAudManday>();
            QuQuotationAudits = new HashSet<QuQuotationAudit>();
            QuQuotationContacts = new HashSet<QuQuotationContact>();
            QuQuotationCustomerContacts = new HashSet<QuQuotationCustomerContact>();
            QuQuotationFactoryContacts = new HashSet<QuQuotationFactoryContact>();
            QuQuotationInspMandays = new HashSet<QuQuotationInspManday>();
            QuQuotationInsps = new HashSet<QuQuotationInsp>();
            QuQuotationPdfVersions = new HashSet<QuQuotationPdfVersion>();
            QuQuotationSupplierContacts = new HashSet<QuQuotationSupplierContact>();
            QuTranStatusLogs = new HashSet<QuTranStatusLog>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public int ServiceId { get; set; }
        public int BillingMethodId { get; set; }
        public int BillingPaidById { get; set; }
        public int CustomerId { get; set; }
        [Required]
        [StringLength(200)]
        public string CustomerLegalName { get; set; }
        public int SupplierId { get; set; }
        [Required]
        [StringLength(200)]
        public string SupplierLegalName { get; set; }
        public int FactoryId { get; set; }
        [Required]
        [StringLength(200)]
        public string LegalFactoryName { get; set; }
        [Required]
        [StringLength(600)]
        public string FactoryAddress { get; set; }
        public int OfficeId { get; set; }
        public double InspectionFees { get; set; }
        public double EstimatedManday { get; set; }
        public int CurrencyId { get; set; }
        public double? TravelCostsAir { get; set; }
        public double? TravelCostsLand { get; set; }
        public double? TravelCostsHotel { get; set; }
        public double? OtherCosts { get; set; }
        public double? Discount { get; set; }
        public double TotalCost { get; set; }
        public string ApiRemark { get; set; }
        public string CustomerRemark { get; set; }
        public int IdStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public string ApiInternalRemark { get; set; }
        public int? BillingEntity { get; set; }
        public int? PaymentTerms { get; set; }
        public int? ValidatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ValidatedOn { get; set; }
        public int? EntityId { get; set; }
        public int? RuleId { get; set; }
        [StringLength(200)]
        public string PaymentTermsValue { get; set; }
        public int? PaymentTermsCount { get; set; }

        [ForeignKey("BillingEntity")]
        [InverseProperty("QuQuotations")]
        public virtual RefBillingEntity BillingEntityNavigation { get; set; }
        [ForeignKey("BillingMethodId")]
        [InverseProperty("QuQuotations")]
        public virtual QuBillMethod BillingMethod { get; set; }
        [ForeignKey("BillingPaidById")]
        [InverseProperty("QuQuotations")]
        public virtual QuPaidBy BillingPaidBy { get; set; }
        [ForeignKey("CountryId")]
        [InverseProperty("QuQuotations")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("CurrencyId")]
        [InverseProperty("QuQuotations")]
        public virtual RefCurrency Currency { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("QuQuotations")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("QuQuotations")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryId")]
        [InverseProperty("QuQuotationFactories")]
        public virtual SuSupplier Factory { get; set; }
        [ForeignKey("IdStatus")]
        [InverseProperty("QuQuotations")]
        public virtual QuStatus IdStatusNavigation { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("QuQuotations")]
        public virtual RefLocation Office { get; set; }
        [ForeignKey("PaymentTerms")]
        [InverseProperty("QuQuotations")]
        public virtual RefInvoiceType PaymentTermsNavigation { get; set; }
        [ForeignKey("RuleId")]
        [InverseProperty("QuQuotations")]
        public virtual CuPrDetail Rule { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("QuQuotations")]
        public virtual RefService Service { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("QuQuotationSuppliers")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("ValidatedBy")]
        [InverseProperty("QuQuotations")]
        public virtual ItUserMaster ValidatedByNavigation { get; set; }
        [InverseProperty("IdQuotationNavigation")]
        public virtual ICollection<QuInspProduct> QuInspProducts { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<QuPdfversion> QuPdfversions { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<QuQuotationAudManday> QuQuotationAudMandays { get; set; }
        [InverseProperty("IdQuotationNavigation")]
        public virtual ICollection<QuQuotationAudit> QuQuotationAudits { get; set; }
        [InverseProperty("IdQuotationNavigation")]
        public virtual ICollection<QuQuotationContact> QuQuotationContacts { get; set; }
        [InverseProperty("IdQuotationNavigation")]
        public virtual ICollection<QuQuotationCustomerContact> QuQuotationCustomerContacts { get; set; }
        [InverseProperty("IdQuotationNavigation")]
        public virtual ICollection<QuQuotationFactoryContact> QuQuotationFactoryContacts { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<QuQuotationInspManday> QuQuotationInspMandays { get; set; }
        [InverseProperty("IdQuotationNavigation")]
        public virtual ICollection<QuQuotationInsp> QuQuotationInsps { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<QuQuotationPdfVersion> QuQuotationPdfVersions { get; set; }
        [InverseProperty("IdQuotationNavigation")]
        public virtual ICollection<QuQuotationSupplierContact> QuQuotationSupplierContacts { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<QuTranStatusLog> QuTranStatusLogs { get; set; }
    }
}