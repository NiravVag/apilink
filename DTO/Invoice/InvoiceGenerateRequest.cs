using DTO.Common;
using DTO.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Invoice
{
    public class InvoiceGenerateRequest
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int? InvoicingRequest { get; set; }
        [Required]
        public DateObject RealInspectionFromDate { get; set; }
        [Required]
        public DateObject RealInspectionToDate { get; set; }
        public IEnumerable<int> BookingNoList { get; set; }
        public int InvoiceTo { get; set; }
        public bool IsTravelExpense { get; set; }
        public bool IsInspection { get; set; }
        public bool IsNewBookingInvoice { get; set; }
        public string InvoiceNumber { get; set; }
        public int? InvoiceType { get; set; }
        public int? BillingEntity { get; set; }
        public int? BankAccount { get; set; }
        public double? AdditionalTax  { get; set; }

        // group by options
        public int Service { get; set; }
        public IEnumerable<int> ServiceTypes { get; set; }
        public IEnumerable<int> FactoryCountryList { get; set; }
        public IEnumerable<int> BrandIdList { get; set; }
        public IEnumerable<int> DepartmentIdList { get; set; }
        public IEnumerable<int> BuyerIdList { get; set; }
        public IEnumerable<int> SupplierList { get; set; }
        public IEnumerable<int> CustomerContacts { get; set; }
        public IEnumerable<int> SplitInvoice { get; set; }
        public IEnumerable<int> ProductCategoryIdList { get; set; }
        public IEnumerable<int> ProductSubCategoryIdList { get; set; }
        public int? CurrencyId { get; set; }
        public double? ExchangeRate { get; set; }
        public InvoiceSupplierInfo SupplierInfo { get; set; }
        public bool IsFromQuotation { get; set; }
    }

    public class InvoiceSupplierInfo
    {
        public int SupplierId { get; set; }
        public string BilledName { get; set; }
        public string BillingAddress { get; set; }
        public IEnumerable<int> ContactPersonIdList { get; set; }
    }

    public class InvoiceBookingDetail
    {
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int FactoryCountryId { get; set; }
        public int? FactoryCountyId { get; set; }
        public int? FactoryProvinceId { get; set; }
        public int? FactoryCityId { get; set; }
        public string FactoryCountryName { get; set; }
        public string FactoryCountryCode { get; set; }
        public string CustomerBookingNo { get; set; }
        public IEnumerable<int> ServiceTypeIds { get; set; }
        public IEnumerable<int> BrandIds { get; set; }
        public IEnumerable<int> BuyerIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public IEnumerable<int> CustomerContactIds { get; set; }
        public IEnumerable<int> ProductCategoryIds { get; set; }
        public IEnumerable<int> ProductSubCategoryIds { get; set; }
        public int PriceCategoryId { get; set; }
        public DateTime ServiceFrom { get; set; }
        public DateTime ServiceTo { get; set; }
        public int BookingId { get; set; }
        public int AuditId { get; set; }
        public int? AuditBrandId { get; set; }
        public int? AuditDepartmentId { get; set; }
        public int OfficeId { get; set; }
        public int StatusId { get; set; }
        public string GroupBy { get; set; }
        public bool IsInvalid { get; set; }
        public CustomerPriceCardRepo RuleConfig { get; set; }
        public int? TotalStaff { get; set; }
    }

    public class InvoiceBookingFactoryDetails
    {
        public int InspectionId { get; set; }
        public int FactoryId { get; set; }
        public int FactoryCountryId { get; set; }
        public int FactoryProvinceId { get; set; }
        public int FactoryCityId { get; set; }
        public int? FactoryCountyId { get; set; }
        public string FactoryCountryName { get; set; }
        public string FactoryCountryCode { get; set; }
    }

    public class InvoiceDataForNewBooking
    {
        public int InvoiceId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? PaymentStatus { get; set; }
    }

    public enum InvoiceGenerationGroupBy
    {
        Supplier = 1,
        Service = 2,
        ServiceType = 3,
        Country = 4,
        Brand = 5,
        Department = 6,
        Buyer = 7,
        CustomerContact = 8,
        BookingNo = 9,
        ProductCategory = 10
    }

    public enum INVInvoiceType
    {
        Monthly = 1,
        PreInvoice = 2
    }

    public enum InvoiceTo
    {
        Customer = 1,
        Supplier = 2,
        Factory = 3
    }

    public enum InvoiceStatus
    {
        Created = 1,
        Modified = 2,
        Approved = 3,
        Cancelled = 4,
        Sent = 5
    }

    public enum InvoicePaymentStatus
    {
        NotPaid = 1,
        HalfPaid = 2,
        Paid = 3
    }
}
