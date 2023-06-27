using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Invoice;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerPriceCard
    {
        public int CustomerId { get; set; }
        public int? Id { get; set; }
        public int BillingMethodId { get; set; }
        public int BillingToId { get; set; }
        public int ServiceId { get; set; }
        public int? PriceComplexType { get; set; }
        public int CurrencyId { get; set; }
        public double UnitPrice { get; set; }
        public double? HolidayPrice { get; set; }
        public double? ProductPrice { get; set; }
        public int? FreeTravelKM { get; set; }
        public int? TariffTypeId { get; set; }
        public bool? TaxIncluded { get; set; }
        public bool? TravelIncluded { get; set; }
        public string Remarks { get; set; }
        public DateObject PeriodFrom { get; set; }
        public DateObject PeriodTo { get; set; }
        public IEnumerable<int> ServiceTypeIdList { get; set; }
        public IEnumerable<int> FactoryCountryIdList { get; set; }
        public IEnumerable<int> FactoryProvinceIdList { get; set; }
        public IEnumerable<int> FactoryCityIdList { get; set; }
        public IEnumerable<int> ProductCategoryIdList { get; set; }
        public IEnumerable<int> ProductSubCategoryIdList { get; set; }
        public IEnumerable<int> SupplierIdList { get; set; }
        public IEnumerable<int> DepartmentIdList { get; set; }
        public IEnumerable<int> BrandIdList { get; set; }
        public IEnumerable<int> BuyerIdList { get; set; }
        public IEnumerable<int> PriceCategoryIdList { get; set; }
        public IEnumerable<int> HolidayTypeIdList { get; set; }
        public int? MaxProductCount { get; set; }
        public bool? SampleSizeBySet { get; set; }
        public bool? PriceToEachProduct { get; set; }
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

        public bool? InvoiceRequestSelectAll { get; set; }
        public bool? SubCategorySelectAll { get; set; }
        public bool? IsSpecial { get; set; }
        public bool? IsInvoiceConfigured { get; set; }
        public int? InvoiceRequestType { get; set; }
        public string InvoiceRequestAddress { get; set; }
        public string InvoiceRequestBilledName { get; set; }

        public IEnumerable<PriceInvoiceRequest> InvoiceRequestList { get; set; }
        public IEnumerable<PriceSpecialRule> RuleList { get; set; }
        public IEnumerable<PriceSubCategory> SubCategoryList { get; set; }
        public IEnumerable<int> InvoiceRequestContact { get; set; }

        public int? BillingEntity { get; set; }
        public int? BankAccount { get; set; }

        public int? PaymentDuration { get; set; }
        public string PaymentTerms { get; set; }

        public string InvoiceNoDigit { get; set; }
        public string InvoiceNoPrefix { get; set; }

        public int? InvoiceInspFeeFrom { get; set; }
        public int? InvoiceHotelFeeFrom { get; set; }
        public int? InvoiceOtherFeeFrom { get; set; }
        public int? InvoiceDiscountFeeFrom { get; set; }
        public int? InvoiceTravelExpense { get; set; }
        public int? InvoiceOffice { get; set; }
        public IEnumerable<int> InspectionLocationList { get; set; }
        public int? BillQuantityType { get; set; }
        public int? InterventionType { get; set; }
        public double? MaxFeeStyle { get; set; }
        public int? BillFrequency { get; set; }
        public string InvoiceSubject { get; set; }
        public string CustomerSegment { get; set; }
        public string CustomerCountry { get; set; }

        public double? MandayProductivity { get; set; }
        public int? MandayReports { get; set; }
        public double? MandayBuffer { get; set; }
    }


    public class CustomerPriceCardRepo
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Id { get; set; }
        public int BillingMethodId { get; set; }
        public int BillingToId { get; set; }
        public int ServiceId { get; set; }
        public int CurrencyId { get; set; }
        public double UnitPrice { get; set; }
        public double? HolidayPrice { get; set; }
        public double? ProductPrice { get; set; }
        public int? FreeTravelKM { get; set; }
        public int? TariffTypeId { get; set; }
        public int? TravelMatrixTypeId { get; set; }
        public bool? TaxIncluded { get; set; }
        public bool? TravelIncluded { get; set; }
        public bool? PriceToEachProduct { get; set; }
        public string Remarks { get; set; }
        public DateObject PeriodFrom { get; set; }
        public DateObject PeriodTo { get; set; }
        public IEnumerable<int> ServiceTypeIdList { get; set; }
        public IEnumerable<int> FactoryCountryIdList { get; set; }
        public IEnumerable<int> FactoryProvinceIdList { get; set; }
        public IEnumerable<int> FactoryCityIdList { get; set; }
        public IEnumerable<int> ProductCategoryIdList { get; set; }
        public IEnumerable<int> ProductSubCategoryIdList { get; set; }
        public IEnumerable<int> SupplierIdList { get; set; }
        public IEnumerable<int> DepartmentIdList { get; set; }
        public IEnumerable<int> BrandIdList { get; set; }
        public IEnumerable<int> BuyerIdList { get; set; }
        public IEnumerable<int> PriceCategoryIdList { get; set; }
        public IEnumerable<int> HolidayTypeIdList { get; set; }

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

        public bool? InspRateBySamplingProRate { get; set; }
        public bool? InvoiceRequestSelectAll { get; set; }
        public bool? IsSpecial { get; set; }

        public bool? SubCategorySelectAll { get; set; }
        public bool? IsInvoiceConfigured { get; set; }
        public int? InvoiceRequestType { get; set; }
        public int? PriceComplexType { get; set; }
        public string InvoiceRequestAddress { get; set; }
        public string InvoiceRequestBilledName { get; set; }
        public IEnumerable<int> InvDepartmentIdList { get; set; }
        public IEnumerable<int> InvBrandIdList { get; set; }
        public IEnumerable<int> InvBuyerIdList { get; set; }
        public IEnumerable<PriceInvoiceRequest> InvoiceRequestList { get; set; }
        public IEnumerable<PriceSubCategory> SubCategory2List { get; set; }
        public IEnumerable<PriceSpecialRule> RuleList { get; set; }
        public IEnumerable<int> InvoiceRequestContact { get; set; }

        public int? BillingEntity { get; set; }
        public int? BankAccount { get; set; }

        public int? PaymentDuration { get; set; }
        public string PaymentTerms { get; set; }

        public string InvoiceNoDigit { get; set; }
        public string InvoiceNoPrefix { get; set; }

        public int? InvoiceInspFeeFrom { get; set; }
        public int? InvoiceHotelFeeFrom { get; set; }
        public int? InvoiceOtherFeeFrom { get; set; }
        public int? InvoiceDiscountFeeFrom { get; set; }
        public int? InvoiceTravelExpense { get; set; }

        public int? InvoiceOffice { get; set; }
        public int? BillQuantityType { get; set; }
        public int? InterventionType { get; set; }
        public double? MaxFeeStyle { get; set; }
        public int? BillFrequency { get; set; }
        public string InvoiceSubject { get; set; }

        public double? MandayProductivity { get; set; }
        public int? MandayReports { get; set; }
        public double? MandayBuffer { get; set; }
    }


    public class CombineBookingGroup
    {
        public int BookingId { get; set; }
        public int CombineGroupId { get; set; }
    }


    public class SaveCustomerPriceCardResponse
    {
        public int Id { get; set; }
        public ResponseResult Result { get; set; }
    }
    public class EditSaveCustomerPriceCardResponse
    {
        public CustomerPriceCard getData { get; set; }
        public ResponseResult Result { get; set; }
    }
    public enum ResponseResult
    {
        Success = 1,
        Failed = 2,
        RequestNotCorrectFormat = 3,
        NotFound = 4,
        Error = 5,
        Exists = 6,
        MoreRuleExists = 7,
        NoQuotationCommonDataMatch = 8
    }

    public enum UnitPriceResponseResult
    {
        Success = 1,
        NotFound = 2,
        MoreRuleExists = 3,
        SingleRuleExists = 4,
    }

    public class CustomerPriceCardSummary
    {
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public int CustomerId { get; set; }
        public int? BillingMethodId { get; set; }
        public int? BillingToId { get; set; }
        public int? ServiceId { get; set; }
        public bool? TaxIncluded { get; set; }
        public bool? TravelIncluded { get; set; }
        public DateObject PeriodFrom { get; set; }
        public DateObject PeriodTo { get; set; }
        public IEnumerable<int> ProductCategoryIdList { get; set; }
        public IEnumerable<int> ServiceTypeIdList { get; set; }
        public IEnumerable<int> CountryIdList { get; set; }
        public IEnumerable<int> DepartmentIdList { get; set; }
        public IEnumerable<int> PriceCategoryIdList { get; set; }
    }

    public class CustomerPriceCardSummaryItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string BillMethodName { get; set; }
        public string DepartmentName { get; set; }
        public string PriceCategory { get; set; }
        public string BillToName { get; set; }
        public string ServiceName { get; set; }
        public double UnitPrice { get; set; }
        public string CurrencyName { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string FactoryCountryList { get; set; }
        public string ServiceTypeList { get; set; }
        public string TravelIncluded { get; set; }
        public string TaxInclude { get; set; }
        public string Remarks { get; set; }
        public int? FreeTraveKM { get; set; }
        public string SupplierNameList { get; set; }
        public string ProductCategoryNameList { get; set; }
        public string FactoryProvinceList { get; set; }
        public DateTime? PeriodFromDate { get; set; }
        public DateTime? PeriodToDate { get; set; }
        public bool? TaxIncludedBool { get; set; }
        public bool? TravelIncludedBool { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedByName { get; set; }
        public string UpdatedOn { get; set; }

    }

    public class CustomerPriceCardSummaryResponse
    {
        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<CustomerPriceCardSummaryItem> GetData { get; set; }
        public ResponseResult Result { get; set; }
    }
    public class CustomerPriceCardDetails
    {
        public double UnitPrice { get; set; }
        public int Id { get; set; }
        public IEnumerable<int> CountryIdList { get; set; }
        public IEnumerable<int?> ProductCategoryIdList { get; set; }
        public IEnumerable<int> ServiceTypeIdList { get; set; }
        public IEnumerable<int> ProvinceIdList { get; set; }
        public IEnumerable<int> SupplierIdList { get; set; }
        public IEnumerable<int> BrandIdList { get; set; }
        public IEnumerable<int> BuyerIdList { get; set; }
        public IEnumerable<int> DepartmentIdList { get; set; }
        public IEnumerable<int> PriceCategoryList { get; set; }
        public IEnumerable<CuPrHolidayType> HolidayTypeList { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public int BillMethodId { get; set; }
        public int BillToId { get; set; }
        public int CurrencyId { get; set; }
        public int? TravelMatrixTypeId { get; set; }
        public double? HolidayPrice { get; set; }
        public bool? TaxIncluded { get; set; }
        public bool? TravelIncluded { get; set; }
        public string BillingMethodName { get; set; }
        public string BillingTo { get; set; }
        public string CurrencyName { get; set; }
        public bool isChecked { get; set; }
    }

    public class CustomerPriceHolidayType
    {
        public int Id { get; set; }
        public int PriceId { get; set; }
        public int HolidayInfoId { get; set; }
        public bool? Active { get; set; }
    }

    public class QuotationCustomerPriceCard
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string BillingPaidBy { get; set; }
        public string BillingMethodName { get; set; }
        public string ServiceTypeNames { get; set; }
        public string ProductCategoryNames { get; set; }
        public string FactoryCountryNames { get; set; }
        public string BrandNames { get; set; }
        public string BuyerNames { get; set; }
        public string PriceCategoryNames { get; set; }
        public string DepartmentNames { get; set; }
        public double UnitPrice { get; set; }
        public int? FreeTravelKM { get; set; }
        public string Remarks { get; set; }
        public string TaxIncluded { get; set; }
        public string TravelIncluded { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public int? TravelMatrixTypeId { get; set; }
        public int CurrencyId { get; set; }
        public int BillingPaidById { get; set; }
        public int BillingMethodId { get; set; }
        public string CurrencyName { get; set; }
        public string ProvinceNames { get; set; }
        public int? BillingEntityId { get; set; }
        public string PaymentTermsValue { get; set; }
        public int? PaymentTermsCount { get; set; }
    }

    public class QuotationCustomerPriceCardData
    {
        public string CustomerName { get; set; }
        public string BillingMethodName { get; set; }
        public string BillingPaidBy { get; set; }
        public int Id { get; set; }
        public IEnumerable<string> ServiceTypeNameList { get; set; }
        public IEnumerable<string> ProductCategoryNameList { get; set; }
        public IEnumerable<string> FactoryCountryNameList { get; set; }
        public IEnumerable<string> BrandNameList { get; set; }
        public IEnumerable<string> BuyerNameList { get; set; }
        public IEnumerable<string> PriceCategoryNameList { get; set; }
        public IEnumerable<string> DepartmentNameList { get; set; }
        public double UnitPrice { get; set; }
        public int? FreeTravelKM { get; set; }
        public string Remarks { get; set; }
        public bool? TaxIncluded { get; set; }
        public bool? TravelIncluded { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public int? TravelMatrixTypeId { get; set; }
        public int CurrencyId { get; set; }
        public int BillingMethodId { get; set; }
        public int BillingPaidById { get; set; }
        public string CurrencyName { get; set; }
        public IEnumerable<string> ProvinceNameList { get; set; }
        public int? BillingEntityId { get; set; }
        public string PaymentTermsValue { get; set; }
        public int? PaymentTermsCount { get; set; }
    }

    public class CustomerPriceCardRequest
    {
        public int BookingId { get; set; }
        public int? RuleId { get; set; }
        public int QuotationId { get; set; }
        public int BillMethodId { get; set; }
        public int BillPaidById { get; set; }
        public int? CurrencyId { get; set; }
    }
    public class UnitPriceCardRequest
    {
        public IEnumerable<int> BookingIds { get; set; }
        public int BillMethodId { get; set; }
        public int BillPaidById { get; set; }
        public int? CurrencyId { get; set; }
        public int? BillQuantityType { get; set; }
        public int? RuleId { get; set; }
        public int? InvoiceType { get; set; }
        public int? QuotationBillPaidBy { get; set; }       
    }

    public class CustomerPriceCardUnitPrice
    {
        public int BookingId { get; set; }
        public IEnumerable<int> PriceCardIdList { get; set; }
        public double? UnitPrice { get; set; }
        public int? BillQuantityType { get; set; }
        public double? TotalBillQuantity { get; set; }
        public string Remarks { get; set; }
        public IEnumerable<QuotationCustomerPriceCard> CustomerPriceCardDetails { get; set; }
        public UnitPriceResponseResult Result { get; set; }
    }

    public class BookingRuleInfo
    {
        public InvoiceGenerateRequest InvoiceGenerateRequest { get; set; }
        public IEnumerable<InvoiceBookingDetail> invoiceBookings { get; set; }
        public IEnumerable<CustomerPriceCardRepo> customerPriceCards { get; set; }

    }

    public class CustomerPriceCardUnitPriceResponse
    {
        public List<CustomerPriceCardUnitPrice> UnitPriceCardList { get; set; }
        public ResponseResult Result { get; set; }
    }

    public class ExportSummary
    {
        public string CustomerName { get; set; }
        public string SupplierNames { get; set; }
        public string BillMethod { get; set; }
        public string BillPaidBy { get; set; }
        public string Service { get; set; }
        public string ProductCategorys { get; set; }
        public string ServiceTypes { get; set; }
        public string FactoryCountry { get; set; }
        public string FactoryProvince { get; set; }
        public string CurrencyName { get; set; }
        public double UnitPrice { get; set; }
        public int? FreeTravelKM { get; set; }
        public string Remarks { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string TaxInclude { get; set; }
        public string TraveInclude { get; set; }
        public string Department { get; set; }
        public string Brand { get; set; }
        public string Buyer { get; set; }
        public string PriceCategory { get; set; }
        public string HolidayType { get; set; }
        public double? MinBillingFeePerDay { get; set; }
        public int? MaximumProductCount { get; set; }
        public string InvoiceRequest { get; set; }
        public string BillingEntity { get; set; }
        public string InvoiceBank { get; set; }
        public string BilledName { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string BillingAddress { get; set; }
        public string InvoiceDigitalNo { get; set; }
        public string InvoiceNoPrefix { get; set; }
        public string InvoiceOffice { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeValue { get; set; }
        public string PaymentTerms { get; set; }
        public string InspectionFee { get; set; }
        public string TravelExpense { get; set; }
        public string Discount { get; set; }
        public string OtherFee { get; set; }
        public string TariffType { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedByName { get; set; }
        public string UpdatedOn { get; set; }

    }
    public class PriceCard
    {
        public IEnumerable<CustomerPriceCardDetails> CustomerPriceCardDetails { get; set; }
        public ResponseResult Result { get; set; }
    }

    public class QuotationPriceCard
    {
        public QuotationCustomerPriceCard PriceCardDetails { get; set; }
        public ResponseResult Result { get; set; }
    }

    public class SamplingUnitPriceRequest
    {
        public int BookingId { get; set; }
        public int PriceCardId { get; set; }
    }

    public class SamplingUnitPriceResponse
    {
        public List<SamplingUnitPrice> SamplingUnitPriceList { get; set; }
        public SamplingUnitPriceResult Result { get; set; }
    }

    public class SamplingUnitPrice
    {
        public int BookingId { get; set; }
        public double UnitPrice { get; set; }
        public string Remarks { get; set; }
    }


    public enum SamplingUnitPriceResult
    {
        Success = 1,
        NotFound = 2
    }

    public enum InvoiceRequestType
    {
        Brand = 1,
        Department = 2,
        Buyer = 3,
        NotApplicable = 4
    }

    public enum InvoiceFeesFrom
    {
        Invoice = 1,
        Quotation = 2,
        Carrefour = 3,
        NotApplicable = 4
    }

    public enum PriceComplexType
    {
        Simple = 1,
        Complex = 2
    }

    public class PriceInvoiceRequest
    {
        public int Id { get; set; }
        public int? CuPriceCardId { get; set; }
        public string BilledName { get; set; }
        public string BilledAddress { get; set; }
        public int? DepartmentId { get; set; }
        public int? BrandId { get; set; }
        public int? BuyerId { get; set; }
        public int? ProductCategoryId { get; set; }
        public bool IsCommon { get; set; }
        public IEnumerable<int> InvoiceRequestContactList { get; set; }
    }


    public class PriceSubCategory
    {
        public int Id { get; set; }
        public int? CuPriceCardId { get; set; }
        public int? SubCategory2Id { get; set; }
        public int? MandayProductivity { get; set; }
        public double? UnitPrice { get; set; }
        public int? MandayReports { get; set; }
        public double? MandayBuffer { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategory2Name { get; set; }
        public double? AQL_QTY_8 { get; set; }
        public double? AQL_QTY_13 { get; set; }
        public double? AQL_QTY_20 { get; set; }
        public double? AQL_QTY_32 { get; set; }
        public double? AQL_QTY_50 { get; set; }
        public double? AQL_QTY_80 { get; set; }
        public double? AQL_QTY_125 { get; set; }
        public double? AQL_QTY_200 { get; set; }
        public double? AQL_QTY_315 { get; set; }
        public double? AQL_QTY_500 { get; set; }
        public double? AQL_QTY_800 { get; set; }
        public double? AQL_QTY_1250 { get; set; }
        public bool IsCommon { get; set; }
    }

    public class PriceSpecialRule
    {
        public int Id { get; set; }
        public int? CuPriceCardId { get; set; }

        public int? MandayProductivity { get; set; }
        public double? UnitPrice { get; set; }
        public int? MandayReports { get; set; }
        public int? MandayBuffer { get; set; }
        public int? PieceRate_Billing_Q_Start { get; set; }
        public int? Piecerate_Billing_Q_End { get; set; }
        public double? AdditionalFee { get; set; }
        public double? Piecerate_MinBilling { get; set; }
        public int? PerInterventionRange1 { get; set; }
        public int? PerInterventionRange2 { get; set; }
        public double? Max_Style_Per_Day { get; set; }
        public double? Max_Style_Per_Week { get; set; }
        public double? Max_Style_per_Month { get; set; }
        public double? Interventionfee { get; set; }
    }



    public class PriceInvoiceRequestContact
    {
        public int Id { get; set; }
        public int? CuPriceCardId { get; set; }
        public int? InvoiceRequestId { get; set; }
        public int? ContactId { get; set; }
        public bool? IsCommon { get; set; }
        public string Name { get; set; }
    }

    public class InvoiceRequestContactId
    {
        public int ContactId { get; set; }
        public int? InvoiceRequestId { get; set; }
    }

    public enum PriceBillingMethod
    {
        ManDay = 1,
        Sampling = 2,
        PieceRate = 3,
        Intervention = 4
    }

    public enum InterventionType
    {
        Range = 1,
        PerStyle = 2
    }

    public class ExportSummaryItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string BillMethod { get; set; }
        public string BillPaidBy { get; set; }
        public string Service { get; set; }
        public string CurrencyName { get; set; }
        public double UnitPrice { get; set; }
        public int? FreeTravelKM { get; set; }
        public string Remarks { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public bool? TaxInclude { get; set; }
        public bool? TraveInclude { get; set; }
        public double? MinBillingFeePerDay { get; set; }
        public int? MaximumProductCount { get; set; }
        public string InvoiceRequest { get; set; }
        public string BillingEntity { get; set; }
        public string InvoiceBank { get; set; }
        public string BilledName { get; set; }
        public string BillingAddress { get; set; }
        public string InvoiceDigitalNo { get; set; }
        public string InvoiceNoPrefix { get; set; }
        public string InvoiceOffice { get; set; }
        public string PaymentTypeValue { get; set; }
        public string PaymentTerms { get; set; }
        public string InspectionFee { get; set; }
        public string TravelExpense { get; set; }
        public string Discount { get; set; }
        public string OtherFee { get; set; }
        public string TariffType { get; set; }
        public int? MaxSampleSize { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedOn { get; set; }
        
    }

    public class ExportMapRequest
    {
        public List<CuPrCommonDataSource> SupData { get; set; }
        public List<CuPrCommonDataSource> ProductCategory { get; set; }
        public List<CuPrCommonDataSource> ProductSubCategory { get; set; }
        public List<CuPrCommonDataSource> ServiceType { get; set; }
        public List<CuPrCommonDataSource> CountryData { get; set; }
        public List<CuPrCommonDataSource> ProvinceData { get; set; }
        public List<CuPrCommonDataSource> CityData { get; set; }
        public List<CuPrCommonDataSource> DeptData { get; set; }
        public List<CuPrCommonDataSource> BuyerData { get; set; }
        public List<CuPrCommonDataSource> BrandData { get; set; }
        public List<CuPrCommonDataSource> PriceCategory { get; set; }
        public List<CuPrCommonDataSource> HolidayType { get; set; }
        public List<CuPrCommonDataSource> InspectionLocation { get; set; }
        public List<CuPrCommonDataSource> Contact { get; set; }
        public List<CuPrContactCommonDataSource> InvoiceContactList { get; set; }
    }

    public class CuPrCommonDataSource
    {
        public int Id { get; set; }
        public int PriceId { get; set; }
        public string Name { get; set; }
    }
    public class CuPrContactCommonDataSource
    {
        public int? InvoiceId { get; set; }
        public int? PriceId { get; set; }
        public string Name { get; set; }
        public int? ContactId { get; set; }
    }
}
