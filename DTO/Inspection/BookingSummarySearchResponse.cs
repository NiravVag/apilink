using DTO.Quotation;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{


    public class BookingSummarySearchResponse
    {
        public IEnumerable<BookingItem> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<InspectionStatus> InspectionStatuslst { get; set; }

        public IEnumerable<QuotationSummaryStatus> QuotationStatuslst { get; set; }

        public BookingSummarySearchResponseResult Result { get; set; }
        public InternalUserRoleAccess InternalUserRole { get; set; }
    }
    public class BookingItem
    {
        public int BookingId { get; set; }

        public int? CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int? SupplierId { get; set; }
        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public string ServiceType { get; set; }

        public int ServiceTypeId { get; set; }

        public string ServiceDateFrom { get; set; }

        public string ServiceDateTo { get; set; }

        public string ServiceDateFromEaqf { get; set; }

        public string ServiceDateToEaqf { get; set; }

        public string FirstServiceDateFrom { get; set; }

        public string FirstServiceDateTo { get; set; }

        public string PoNumber { get; set; }

        public string ReportNo { get; set; }

        public string Office { get; set; }

        public int? OfficeId { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int? QuotationStatusId { get; set; }

        public string QuotationStatusName { get; set; }

        public int? BookingCreatedBy { get; set; }

        public bool IsPicking { get; set; }

        public bool? IsEAQF { get; set; }

        public string ProductCategory { get; set; }

        public int? PreviousBookingNo { get; set; }

        public int? CountryId { get; set; }
        public string CSName { get; set; }
        public bool IsSplitBookingButtonVisible { get; set; }
        public bool IsPickingButtonVisible { get; set; }
        public bool? IsCombineVisible { get; set; }

        public string ApplyDate { get; set; }

        public InspStatus Status { get; set; }

        public CuCustomer Customer { get; set; }

        public string CustomerBookingNo { get; set; }

        public string ReportSummaryLink { get; set; }

        public List<int> DeptIds { get; set; }

        public string FactoryState { get; set; }

        public string FactoryCity { get; set; }

        public string FactoryCountry { get; set; }

        public string BookingAPIComments { get; set; }

        public int ReportCount { get; set; }

        public string DeptNames { get; set; }

        public string BrandNames { get; set; }

        public string BuyerNames { get; set; }

        public string PriceCategoryName { get; set; }

        public string CollectionName { get; set; }

        public string Barcode { get; set; }

        public bool IsEcoPack { get; set; }

        public string FactoryCountryName { get; set; }

        public string BookingCreatedByName { get; set; }

        public int BookingCreatedByType { get; set; }

        public int ProductCount { get; set; }

        public string BookingCreatedFirstName { get; set; }
        public bool IsCustomerLogin { get; set; }

        public string ProductRefId { get; set; }

        public string CancelType { get; set; }

        public string CancelReason { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductType { get; set; }
        public int? ReportRequest { get; set; }
        public bool? IsSameDayReport { get; set; }
        public string Instructions { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedDateEaqf { get; set; }
        public int? BookingType { get; set; }

    }

    public class InspectionBookingItems
    {
        public int BookingId { get; set; }
        public int? CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int? SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ServiceType { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public DateTime? FirstServiceDateFrom { get; set; }
        public DateTime? FirstServiceDateTo { get; set; }
        public string Office { get; set; }
        public int? OfficeId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int StatusPriority { get; set; }
        public int? BookingCreatedBy { get; set; }
        public string ProductCategory { get; set; }
        public int? PreviousBookingNo { get; set; }
        public bool IsSplitBookingButtonVisible { get; set; }
        public bool IsPicking { get; set; }

        public bool? IsEAQF { get; set; }
        public DateTime ApplyDate { get; set; }
        public InspStatus Status { get; set; }
        public CuCustomer Customer { get; set; }
        public string CustomerBookingNo { get; set; }
        public string BookingAPiRemarks { get; set; }
        public ICollection<InspTranCuDepartment> DeptCode { get; set; }
        public string PriceCategoryName { get; set; }
        public string CollectionName { get; set; }
        public int? CollectionId { get; set; }
        public int? PriceCategoryId { get; set; }
        public int UserTypeId { get; set; }
        public bool? IsCombineRequired { get; set; }
        public string CreatedByStaff { get; set; }
        public string CreatedByCustomer { get; set; }
        public string CreatedBySupplier { get; set; }
        public string CreatedByFactory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductType { get; set; }
        public int? ReportRequest { get; set; }
        public bool? IsSameDayReport { get; set; }
        public string Instructions { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedOnEaqf { get; set; }
        public int? BookingType { get; set; }
    }

    public class BookingDataRepo
    {
        public int BookingNo { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int BookingStatus { get; set; }
        public string RegionalSupplierName { get; set; }
        public string RegionalFactoryName { get; set; }
        public string Office { get; set; }
        public string Status { get; set; }
    }

    public class ReportVersionData
    {
        public int ReportId { get; set; }
        public int? ReportVersion { get; set; }
        public int? ReportRevision { get; set; }
    }
    public class BookingProductinfo
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ProductId { get; set; }
        public string PoNumbers { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int? BookingQuantity { get; set; }
        public string UnitName { get; set; }

    }

    public enum BookingSummarySearchResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }

    public class ReportSummaryResponse
    {
        public IEnumerable<ReportItem> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<InspectionStatus> InspectionStatuslst { get; set; }

        public ReportSummaryResponseResult Result { get; set; }

    }
    public class ReportItem
    {
        public int BookingId { get; set; }

        public int? CustomerID { get; set; }
        public int? FactoryId { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public string ProductCategory { get; set; }

        public string ServiceType { get; set; }

        public string ServiceDateFrom { get; set; }

        public string ServiceDateTo { get; set; }

        public string PoNumber { get; set; }

        public int PoDetailId { get; set; }

        public string ReportNo { get; set; }

        public string Office { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int? BookingCreatedBy { get; set; }

        public bool IsPicking { get; set; }

        public int? PreviousBookingNo { get; set; }
        public int? CountryId { get; set; }

        public int FbMissionId { get; set; }

        public string MissionStatus { get; set; }

        public bool IsCsReport { get; set; }

        public IEnumerable<ProductItem> ReportProducts { get; set; }
    }

    public class ProductItem
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public string PoNumber { get; set; }
        public int PoDetailId { get; set; }
        public string QcName { get; set; }
        public int FbReportId { get; set; }
        public string MissionTitle { get; set; }
        public string ReportTitle { get; set; }
        public string FillingStatus { get; set; }
        public string ReviewStatus { get; set; }
        public string ReportLink { get; set; }
        public int? CombineAqlQuantity { get; set; }
        public int CombineProductCount { get; set; }
        public bool IsParentProduct { get; set; }
        public string ReportStatus { get; set; }
        public int? CombineProductId { get; set; }
        public string ColorCode { get; set; }
        public string ReportStatusColor { get; set; }
        public string FillingStatusColor { get; set; }
        public string ReviewStatusColor { get; set; }
    }

    public enum ReportSummaryResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }

    public class InternalUserRoleAccess
    {
        public bool IsBookingRequestRole { get; set; }
        public bool IsBookingConfirmRole { get; set; }
        public bool IsBookingVerifyRole { get; set; }
        public bool IsQuotationRequestRole { get; set; }
    }
    public class CustomerCSLocation
    {
        public int CustomerId { get; set; }
        public string CsName { get; set; }
        public int? CsId { get; set; }
        public string CscompanyPhone { get; set; }
        public IEnumerable<HrOfficeControl> LocationList { get; set; }
    }

    public class PoDetails
    {
        public int PoTransactionId { get; set; }
        public int BookingId { get; set; }
        public string PoNumber { get; set; }
        public int? PickingQuantity { get; set; }
        public int QuotationStatus { get; set; }
        public string FactoryReference { get; set; }
        public string ProductId { get; set; }
        public string QuotationStatusName { get; set; }
        public int ProductTransId { get; set; }
        public int? ReportId { get; set; }
        public string Barcode { get; set; }
        public bool? IsEcoPack { get; set; }
        public DateTime quotCreatedDate { get; set; }
        public double? CalculatedWorkingHours { get; set; }
        public double? Manday { get; set; }
        public int? CombineProductId { get; set; }
        public double? SuggestedManday { get; set; }
    }

    public class FactoryCountry
    {
        public int BookingId { get; set; }
        public int FactoryCountryId { get; set; }
        public string FactoryAdress { get; set; }
        public string FactoryRegionalAddress { get; set; }
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public int TownId { get; set; }
        public string TownName { get; set; }
        public string ZoneName { get; set; }
        public int FactoryProvinceId { get; set; }
        public int FactoryCityId { get; set; }
        public int FactoryCountyId { get; set; }
        public int FactoryZoneId { get; set; }
        public int IsAutoQcExpenseClaim { get; set; }
    }
    public class BookingProductCategory
    {
        public int BookingId { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductCategorySubName { get; set; }
        public string ProductCategorySub2Name { get; set; }
        public int? ProductCategorySubId { get; set; }
        public int? ProductCategorySub2Id { get; set; }
        public int? ProductCategorySub3Id { get; set; }
        public string ProductCategorySub3Name { get; set; }
    }

    public class BookingProductCategoryData
    {
        public int BookingId { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
    }

    public class BookingDetail
    {
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public IEnumerable<int> CountryIds { get; set; }
        public IEnumerable<int> ProvinceIds { get; set; }
        public IEnumerable<int> ServiceTypeIds { get; set; }
        public IEnumerable<int> BrandIds { get; set; }
        public IEnumerable<int> BuyerIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public int PriceCategoryId { get; set; }
        public DateTime ServiceFrom { get; set; }
        public DateTime ServiceTo { get; set; }
        public int BookingId { get; set; }
        public int OfficeId { get; set; }
        public string CustomerName { get; set; }
        public string FactoryName { get; set; }
        public string StatusName { get; set; }
    }

    public class CSConfigDetail
    {
        public int? CustomerId { get; set; }
        public string CsName { get; set; }
        public int CsId { get; set; }
        public int? OfficeId { get; set; }
        public int? BrandId { get; set; }
        public int? DepartmentId { get; set; }
    }

    public class BookingCustomerData
    {
        public int? CustomerId { get; set; }
        public int BookingId { get; set; }
    }

    public class CustomerDeptData
    {
        public int? CustomerId { get; set; }
        public int? DeptId { get; set; }
    }

    public class CustomerBrandData
    {
        public int? CustomerId { get; set; }
        public int? BrandId { get; set; }
    }

    public class InspectionBookingApplicantItems
    {
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicatPhoneNo { get; set; }
        public int? CreatedBy { get; set; }
    }

    public class BookingReportData
    {
        public int BookingId { get; set; }
        public int? ReportId { get; set; }
        public int ServiceId { get; set; }
        public int? FactoryId { get; set; }
        public int? FBResultId { get; set; }
        public string ReportTitle { get; set; }
    }

}
