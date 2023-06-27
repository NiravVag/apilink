using DTO.DynamicFields;
using DTO.MobileApp;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DTO.Inspection
{

    public class BookingProductAndReportDataResponse
    {
        public BookingData BookingData { get; set; }
        public FBReport ReportData { get; set; }
        public List<ReportProductsData> ReportProducts { get; set; }
        public List<BookingContainersReportData> ReportContainers { get; set; }
        public BookingDataResponseResult Result { get; set; }
    }

    public enum BookingDataResponseResult
    {
        Success = 1,
        Failure = 2,
        NotAvailable = 3
    }

    public class BookingProductsResponse
    {
        public List<BookingProductsData> BookingProductsList { get; set; }
        public List<BookingRepoStatus> BookingStatusList { get; set; }
        public string ProductUpdatedDate { get; set; }
        public BookingProductsResponseResult Result { get; set; }
    }
    public class MobileBookingProducts
    {
        public List<MobileInspectionReportProducData> BookingProductsList { get; set; }
        public List<BookingRepoStatus> BookingStatusList { get; set; }
        public string ProductUpdatedDate { get; set; }
        public BookingProductsResponseResult Result { get; set; }
    }

    public class BookingContainerResponse
    {
        public List<BookingContainersData> BookingContainerList { get; set; }
        public List<BookingRepoStatus> BookingStatusList { get; set; }
        public BookingProductsResponseResult Result { get; set; }
    }

    public class BookingProductPOResponse
    {
        public List<BookingPoDataRequest> BookingProductPoList { get; set; }
        public BookingProductPOResponseResult Result { get; set; }
    }


    public class BookingContainerProductResponse
    {
        public List<BookingProductsData> BookingProductList { get; set; }
        public BookingProductPOResponseResult Result { get; set; }
    }

    public class BookingProductDataResponse
    {
        public List<BookingProductsData> BookingProductsList { get; set; }
        public BookingProductDataResponseResult Result { get; set; }
    }

    public enum BookingProductsResponseResult
    {
        Success = 1,
        Failure = 2,
        NotAvailable = 3
    }

    public enum BookingProductDataResponseResult
    {
        Success = 1,
        Failure = 2,
        NotAvailable = 3
    }

    public enum BookingProductPOResponseResult
    {
        Success = 1,
        Failure = 2,
        NotAvailable = 3
    }

    public class InspectionReportSummaryRepsonse
    {
        public List<InspectionReportSummary> InspectionReportSummaryList { get; set; }
        public InspectionReportSummaryRepsonseResult Result { get; set; }
    }

    public enum InspectionReportSummaryRepsonseResult
    {
        Success = 1,
        Failure = 2,
        NotAvailable = 3
    }

    public class InspectionDefectsRepsonse
    {
        public List<InspectionReportDefects> InspectionDefectList { get; set; }
        public InspectionDefectsRepsonseResult Result { get; set; }
    }

    public enum InspectionDefectsRepsonseResult
    {
        Success = 1,
        Failure = 2,
        NotAvailable = 3
    }

    public class InspectionReportSummary
    {
        public int Id { get; set; }
        public int FbReportDetailId { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public int? ResultId { get; set; }
        public string Remarks { get; set; }
        public List<string> Photos { get; set; }
        public int PhotoCount { get; set; }
        public int ProblematicRemarksCount { get; set; }
    }

    public class InspectionReportDefects
    {
        public int Id { get; set; }
        public int FbReportDetailId { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int ProductRefId { get; set; }
    }

    public class ReportDefectsImage
    {
        public int DefectId { get; set; }
        public string Image { get; set; }
        public string Desc { get; set; }
    }

    public class FBReport
    {
        public string ReportTitle { get; set; }

        public string ReportPath { get; set; }

        public string FinalManualReportPath { get; set; }

        public string ReportResult { get; set; }

        public int? ReportResultId { get; set; }

        public string ReportPhoto { get; set; }

        public string ReportStatus { get; set; }

        public List<string> AdditionalPhotos { get; set; }

        public string CustomerDecisionStatus { get; set; }

        public string CustomerDecisionComments { get; set; }

        public int CustomerResultId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string InspectionDate { get; set; }

        public int ReportId { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalPresentedQty { get; set; }
        public double TotalInspecttedQty { get; set; }

    }
    public class FBReport_Detail
    {
        public string ReportTitle { get; set; }
        public int FBReportId { get; set; }
        public int? FBReportMapId { get; set; }
        public string MissionTitle { get; set; }
        public string OverAllResult { get; set; }
    }


    public class ReportCustomerDecision
    {
        public string CustomerDecisionStatus { get; set; }
        public string CustomerDecisionCustomStatus { get; set; }
        public string Comments { get; set; }
        public int CustomerResultId { get; set; }
        public int ReportId { get; set; }
    }

    public class ReportCustomerDecisionResponse
    {
        public int CustomerResultId { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public ReportCustomerDecisionResponseResult Result { get; set; }

    }

    public enum ReportCustomerDecisionResponseResult
    {
        success = 1,
        fail = 2

    }


    public class BookingData
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public string InspectionType { get; set; }

        public int InspectionTypeId { get; set; }

        public int CombinedCount { get; set; }

        public string SupplierName { get; set; }

        public string SupplierAddress { get; set; }

        public string FactoryName { get; set; }

        public string FactoryAddress { get; set; }

        public bool IsCustomerCheckPoint { get; set; }

        public int supplierId { get; set; }

        public int? factoryId { get; set; }

        public IEnumerable<int> ServiceTypeIds { get; set; }

        public IEnumerable<int> BrandIds { get; set; }

        public IEnumerable<int> DepartmentIds { get; set; }

        public string CustomerName { get; set; }

        public bool CustomerDecisionCheckpointExists { get; set; }

        public bool IsContainer { get; set; }
        public bool HideMultiSelectCustomerDecisionCheckPointExists { get; set; }
    }



    public class ReportProductsData
    {
        public int? ProductId { get; set; }

        public int InspectionPoId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public double? BookingQuantity { get; set; }

        public double? InspectedQuantity { get; set; }

        public double? PresentedQuantity { get; set; }

        public int? Minor { get; set; }

        public int? Major { get; set; }

        public int? Critical { get; set; }

        public string DestinationCountry { get; set; }

        public int? CombineProductId { get; set; }

        public int? CombineProductCount { get; set; }

        public int? CombineAql { get; set; }

        public int? ReportId { get; set; }
        public int ContainerId { get; set; }
        public List<string> PoNumber { get; set; }
    }

    public class BookingQuantityData
    {
        public int ProductRefId { get; set; }
        public int BookingId { get; set; }
        public double BookingQuantity { get; set; }
        public int ProductId { get; set; }
        public int? CombineProductId { get; set; }
        public int? AqlQuantity { get; set; }
        public int? CombineAqlQuantity { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public string ProductSubCategory { get; set; }
        public int? ProductSub2CategoryId { get; set; }
        public string ProductSub2Category { get; set; }
        public int? ContainerId { get; set; }
        public int UnitId { get; set; }
        public int? FbReportId { get; set; }
    }

    public class BookingReportQuantityData
    {
        public int BookingId { get; set; }
        public int? ReportId { get; set; }
        public string ReportName { get; set; }
        public double? PresentedQuantity { get; set; }
        public double? InspectedQuantity { get; set; }
        public double? OrderQuantity { get; set; }
    }

    public class BookingProductsData
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string ProductId { get; set; }

        public string PoNumber { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public string ProductDescription { get; set; }

        public string ProductCategory { get; set; }

        public string ProductSubCategory { get; set; }

        public string ProductSubCategory2 { get; set; }

        public string FactoryReference { get; set; }

        public string ProductImageUrl { get; set; }

        public int? BookingQuantity { get; set; }

        public int? BookingStatus { get; set; }

        public string BookingStatusName { get; set; }

        public double? InspectedQuantity { get; set; }

        public string InspectionDate { get; set; }

        public int? ReportId { get; set; }

        public int? ContainerId { get; set; }

        public string ReportStatus { get; set; }

        public string ReportResult { get; set; }

        public int? ReportResultId { get; set; }

        public string ReportPath { get; set; }

        public string FinalManualReportPath { get; set; }

        public DateTime? ServiceStartDate { get; set; }

        public DateTime? ServiceEndDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? CombineProductId { get; set; }

        public int? CombineProductCount { get; set; }

        public int? CombineAqlQuantity { get; set; }

        public bool IsParentProduct { get; set; }

        public bool IsPlaceHolderVisible { get; set; }

        public DateTime? Etd { get; set; }

        public DateTime? SRDate { get; set; }

        public double? PresentedQuantity { get; set; }

        public int? Minor { get; set; }

        public int? Major { get; set; }

        public int? Critical { get; set; }

        public string DestinationCountry { get; set; }

        public IEnumerable<string> AdditionalPhotos { get; set; }

        public string ReportNo { get; set; }
        public int? AqlQuantity { get; set; }

        public IEnumerable<string> PoNumberList { get; set; }

        public int PoNumberCount { get; set; }
        public int PoNumberCountDisplay { get; set; }
        public string PoNumberShow { get; set; }
        public bool Selected { get; set; }
        public string ProductDescTrim { get; set; }
        public bool IsProductDescTooltipShow { get; set; }
        public string ReportTitle { get; set; }
        public int Unit { get; set; }
        public int? UnitCount { get; set; }
        public string UnitName { get; set; }
        public bool IsMSChart { get; set; }
    }

    public class ScheduleProductsData
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }

        public bool IsMSChart { get; set; }

        public int? CombineProductId { get; set; }

        public int? CombineAqlQuantity { get; set; }

        public int? AqlQuantity { get; set; }
        public string ProductDescription { get; set; }
        public int? OrderQty { get; set; }
        public string Unit { get; set; }
        public int? ReportId { get; set; }
        public int ProductRefId { get; set; }
        public double? InspectedQty { get; set; }
    }


    public class BookingProductsExportData
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string ProductId { get; set; }

        public string PoNumber { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public string ProductDescription { get; set; }

        public string ProductCategory { get; set; }

        public string ProductSubCategory { get; set; }

        public string ProductSubCategory2 { get; set; }

        public string FactoryReference { get; set; }

        public string ProductImageUrl { get; set; }

        public int? BookingQuantity { get; set; }

        public int? BookingStatus { get; set; }

        public int FbReportId { get; set; }

        public int FbContainerReportId { get; set; }

        public int? ContainerId { get; set; }

        public DateTime? ServiceStartDate { get; set; }

        public DateTime? ServiceEndDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? CombineProductId { get; set; }

        public int? CombineProductCount { get; set; }

        public int? CombineAqlQuantity { get; set; }

        public bool IsParentProduct { get; set; }

        public bool IsPlaceHolderVisible { get; set; }

        public DateTime? Etd { get; set; }

        public DateTime? SRDate { get; set; }

        public string DestinationCountry { get; set; }

        public int? AqlQty { get; set; }

        public string Barcode { get; set; }

        public bool? IsNewProduct { get; set; }

        public bool? IsEcoPack { get; set; }

        public double? Critical { get; set; }

        public double? Major { get; set; }

        public double? Minor { get; set; }

        public int? PickingQty { get; set; }

        public string Unit { get; set; }

        public string Remarks { get; set; }

        public bool? DisplayMaster { get; set; }

        public string DisplayChild { get; set; }

        public string Aql { get; set; }

        public int? ProductSerialNo { get; set; }
    }

    public class BookingContainersData
    {
        public int Id { get; set; }

        public int? ContainerId { get; set; }

        public int BookingId { get; set; }

        public int? TotalBookingQuantity { get; set; }

        public int? BookingStatus { get; set; }

        public double? InspectedQuantity { get; set; }

        public string InspectionDate { get; set; }

        public int? ReportId { get; set; }

        public int? ApiReportId { get; set; }

        public string ReportStatus { get; set; }

        public string ContainerName { get; set; }

        public string ReportResult { get; set; }

        public int? ReportResultId { get; set; }

        public string ReportPath { get; set; }

        public string FinalManualReportLink { get; set; }

        public int? ReportStatusId { get; set; }

        public string ReportTitle { get; set; }

        public DateTime? ServiceStartDate { get; set; }

        public DateTime? ServiceEndDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsPlaceHolderVisible { get; set; }

        public string ReportResultColor { get; set; }

        public string ReportStatusColor { get; set; }

        public string ReviewResultColor { get; set; }

        public string FillingStatusColor { get; set; }

        public string QCName { get; set; }

        public string ReviewStatus { get; set; }

        public string FillingStatus { get; set; }

        public string ContainerSize { get; set; }

        public string FinalReportLink { get; set; }

    }

    public class BookingContainersRepo
    {
        public int Id { get; set; }

        public int? ContainerId { get; set; }

        public int BookingId { get; set; }

        public int? TotalBookingQuantity { get; set; }

        public int? BookingStatus { get; set; }

        public double? InspectedQuantity { get; set; }

        public string InspectionDate { get; set; }

        public int? ReportId { get; set; }

        public int? ApiReportId { get; set; }

        public string ReportStatus { get; set; }

        public string ContainerName { get; set; }

        public string ContainerSize { get; set; }

        public string ReportResult { get; set; }

        public int? ReportResultId { get; set; }

        public string ReportPath { get; set; }

        public string FinalManualReportPath { get; set; }

        public string ReportTitle { get; set; }

        public DateTime? ServiceStartDate { get; set; }

        public DateTime? ServiceEndDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsPlaceHolderVisible { get; set; }

        public int? ReportStatusId { get; set; }

        public int? ReviewResultId { get; set; }

        public int? FillingStatusId { get; set; }

        public string ReviewStatus { get; set; }

        public string FillingStatus { get; set; }

        public string FinalReportLink { get; set; }

    }

    public class ScheduleContainersRepo
    {
        public int Id { get; set; }
        public int? ContainerId { get; set; }
        public int BookingId { get; set; }
        public int? ReportId { get; set; }
    }


    public class BookingContainersReportData
    {
        public int Id { get; set; }

        public int InspectionPoId { get; set; }

        public int? ContainerId { get; set; }

        public int BookingId { get; set; }

        public int? TotalBookingQuantity { get; set; }

        public string InspectionDate { get; set; }

        public int? ReportId { get; set; }

        public string ContainerName { get; set; }

        public string ContainerSize { get; set; }

        public bool IsPlaceHolderVisible { get; set; }

        public int? InspectedQuantity { get; set; }

        public int? PresentedQuantity { get; set; }

        public int? Minor { get; set; }

        public int? Major { get; set; }

        public int? Critical { get; set; }

        public string DestinationCountry { get; set; }

    }


    public class BookingPoData
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string PoNumber { get; set; }

        public int? BookingQuantity { get; set; }

        public double? InspectedQuantity { get; set; }

        public DateTime? Etd { get; set; }

        public string DestinationCountry { get; set; }

        public DateTime? SRDate { get; set; }

        public bool IsPlaceHolderVisible { get; set; }
    }


    public class BookingPoDataRequest
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string PoNumber { get; set; }

        public int? BookingQuantity { get; set; }

        public double? InspectedQuantity { get; set; }

        public string Etd { get; set; }

        public string DestinationCountry { get; set; }

        public DateTime? SRDate { get; set; }

        public bool IsPlaceHolderVisible { get; set; }
    }

    public class BookingRepoStatus
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string IconType { get; set; }
        public string StatusDate { get; set; }
        public string StatusDesc { get; set; }
    }

    public class BookingLogStatus
    {
        public int? StatusId { get; set; }

        public int? BookingId { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class BookingExportData
    {
        public IEnumerable<string> bookingDFHeaderList { get; set; }
        public IEnumerable<BookingExportItem> bookingList { get; set; }
    }

    public class BookingExportItem
    {
        public int BookingId { get; set; }

        public string ProductId { get; set; }

        public string PoNumber { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public string ProductDescription { get; set; }

        public string ProductSubCategory { get; set; }

        public string ProductSubCategory2 { get; set; }

        public string FactoryReference { get; set; }

        public string ProductImageUrl { get; set; }

        public int? BookingQuantity { get; set; }

        public int? BookingStatus { get; set; }

        public double? InspectedQuantity { get; set; }

        public string InspectionDate { get; set; }

        public int? ReportId { get; set; }

        public string ReportStatus { get; set; }

        public string ReportResult { get; set; }

        public int? ReportResultId { get; set; }

        public string ReportPath { get; set; }

        public string ServiceStartDate { get; set; }

        public string ServiceEndDate { get; set; }

        public string InspectionFromDate { get; set; }

        public string InspectionToDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? CombineProductId { get; set; }

        public int? CombineProductCount { get; set; }

        public int? CombineAqlQuantity { get; set; }

        public bool IsParentProduct { get; set; }

        public bool IsPlaceHolderVisible { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public string ServiceType { get; set; }

        public string Office { get; set; }

        public string StatusName { get; set; }

        public int StatusId { get; set; }

        public string ApplyDate { get; set; }

        public string FirstServiceDateFrom { get; set; }

        public string FirstServiceDateTo { get; set; }

        public string QuotationStatusName { get; set; }

        public string CustomerBookingNo { get; set; }

        public string ProductCategory { get; set; }

        public string CSName { get; set; }

        public string Etd { get; set; }

        public string SRDate { get; set; }

        public int? QuotaitonManDay { get; set; }

        public List<int> DeptIds { get; set; }

        public string FactoryState { get; set; }

        public string FactoryCity { get; set; }

        public string FactoryCountry { get; set; }

        public string APIBookingRemarks { get; set; }

        public bool IsPickingRequired { get; set; }

        public string DestinationCountry { get; set; }

        public string ContainerName { get; set; }

        public List<InspectionBookingDFData> bookingDFList { get; set; }

        public string BrandNames { get; set; }

        public string DeptNames { get; set; }

        public string BuyerNames { get; set; }

        public string PriceCategoryName { get; set; }

        public string CollectionName { get; set; }

        public string Barcode { get; set; }

        public bool? IsEcoPack { get; set; }

        public bool IsPicking { get; set; }

        public string FactoryCountryName { get; set; }

        public string BookingCreatedByName { get; set; }

    }

    public class BookingContainer
    {
        public int BookingId { get; set; }
        public int? ContainerId { get; set; }
        public int ContainerRefId { get; set; }
        public int? ReportId { get; set; }
    }

    public class BookingInfo
    {
        public string CustomerBookingNo { get; set; }
    }

    public class BookingInformation
    {
        public BookingDataResponseResult Result { get; set; }
        public BookingInfo BookingInfo { get; set; }
    }

    public class BookingBrandAccess
    {
        public int BookingId { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
    }

    public class InspectionPriceProductCategory
    {
        public int BookingId { get; set; }
        public string ProductCategoryName { get; set; }
        public int? ProductCategoryId { get; set; }
    }

    public class InspectionPriceProductSubCategory
    {
        public int BookingId { get; set; }
        public string ProductSubCategoryName { get; set; }
        public int? ProductSubCategoryId { get; set; }
    }

    public class BookingDeptAccess
    {
        public int BookingId { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string DeptCode { get; set; }
    }

    public class BookingCustomerContactAccess
    {
        public int BookingId { get; set; }
        public int CustomerContactId { get; set; }
        public string CustomerContactName { get; set; }
        public string CustomerContactEmail { get; set; }
    }

    public class BookingServiceType
    {
        public int BookingNo { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
    }

    public class BookingBuyerAccess
    {
        public int BookingId { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
    }

    public class ProductValidationResponse
    {
        public ProductValidationResult Result { get; set; }
    }

    public enum ProductValidationResult
    {
        Success = 1,
        QuotationExists = 2,
        PickingExists = 3,
        PickingAndQuotationExists = 4,
        Fail = 5
    }

    public class BookingMerchandiserContactList
    {
        public int BookingId { get; set; }
        public int MerchandiserContactId { get; set; }
        public string MerchandiserContactName { get; set; }
        public string MerchandiserContactEmail { get; set; }
    }





}

