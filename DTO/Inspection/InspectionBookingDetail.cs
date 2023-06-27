using DTO.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionBookingDetail
    {
        public int InspectionId { get; set; }
        public string InternalReferencePo { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int? SeasonId { get; set; }
        public int? SeasonYearId { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public DateTime? FirstServiceDateFrom { get; set; }
        public DateTime? FirstServiceDateTo { get; set; }
        public string CusBookingComments { get; set; }
        public string ApiBookingComments { get; set; }
        public string InternalComments { get; set; }
        public string QCBookingComments { get; set; }
        public int? OfficeId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int EntityId { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantPhoneNo { get; set; }
        public int? PreviousBookingNo { get; set; }
        public int? ReInspectionType { get; set; }
        public bool? IsPickingRequired { get; set; }
        public bool? IsCombineRequired { get; set; }
        public bool? IsCombineDone { get; set; }
        public bool? isPickingDone { get; set; }
        public string CustomerBookingNo { get; set; }
        public int? CreatedUserType { get; set; }
        public int? CollectionId { get; set; }
        public int? PriceCategoryId { get; set; }
        public string CollectionName { get; set; }
        public string PriceCategoryName { get; set; }
        public string CompassBookingNo { get; set; }
        public int? BusinessLine { get; set; }
        public int? InspectionLocation { get; set; }
        public bool? IsASReceived { get; set; }
        public DateTime? AsReceivedDate { get; set; }
        public bool? IsTFReceived { get; set; }
        public DateTime? TfReceivedDate { get; set; }
        public string ShipmentPort { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string EAN { get; set; }
        public int? CuProductCategory { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public Guid GuidId { get; set; }
        public int? CustomerSeasonId { get; set; }
        public int? SeasonYear { get; set; }
        public int? SplitPreviousBookingNo { get; set; }
        public string ProductCategoryName { get; set; }
        public int? BookingType { get; set; }
        public string BookingTypeName { get; set; }
        public int? PaymentOptions { get; set; }
        public string GappaymentSupportingDocument { get; set; }
        public string ReInspectionTypeName { get; set; }
        public bool? IsEaqf { get; set; }
        public bool? GAPDACorrelation { get; set; }
        public string GAPDAName { get; set; }
        public string GAPDAEmail { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SeasonName { get; set; }  
        public string SeasonYearName { get; set; }
        public int ServiceTypeId { get; set; }
    }

    public class InspectionProductDetail
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }

        public string ProductDesc { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public int? ProductCategorySub2Id { get; set; }

        public int? ProductCategorySub3Id { get; set; }

        public int InspectionId { get; set; }

        public int Unit { get; set; }

        public int? UnitCount { get; set; }

        public int TotalBookingQuantity { get; set; }

        public string Remarks { get; set; }

        public int? Aql { get; set; }

        public int? Critical { get; set; }

        public int? Major { get; set; }

        public int? Minor { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool? Active { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductSubCategoryName { get; set; }

        public string ProductCategorySub2Name { get; set; }

        public string ProductCategorySub3Name { get; set; }

        public string AqlName { get; set; }

        public string UnitName { get; set; }

        public int? AqlQuantity { get; set; }

        public int? SampleType { get; set; }

        public int? CombineSamplingSize { get; set; }

        public int? CombineGroupId { get; set; }

        public int? ReportId { get; set; }

        public string FactoryReference { get; set; }

        public string Barcode { get; set; }

        public bool? IsEcopack { get; set; }

        public bool? IsDisplayMaster { get; set; }

        public int? FBTemplateId { get; set; }

        public string FbTemplateName { get; set; }

        public int? ParentProductId { get; set; }

        public string ParentProductName { get; set; }

        public bool? IsNewProduct { get; set; }

        public DateTime? AsReceivedDate { get; set; }

        public DateTime? TfReceivedDate { get; set; }

        public int ProductImageCount { get; set; }

    }

    public class InspectionPODetail
    {
        public int Id { get; set; }

        public int ProductRefId { get; set; }

        public int? PoId { get; set; }

        public int? ProductId { get; set; }

        public string PoName { get; set; }

        public int InspectionId { get; set; }

        public int BookingQuantity { get; set; }

        public int ExistingBookingQuantity { get; set; }

        public int? PoQuantity { get; set; }

        public int PoReminingQuantity { get; set; }

        public int? PickingQuantity { get; set; }

        public string Remarks { get; set; }

        public int? DestinationCountryID { get; set; }

        public string DestinationCountryName { get; set; }

        public int? ContainerId { get; set; }

        public int? ReportId { get; set; }

        public DateObject ETD { get; set; }

        public DateTime? ETDDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool? Active { get; set; }

        public string CustomerReferencePo { get; set; }

        public string BaseCustomerReferencePo { get; set; }
    }

    public class InspectionHoldReasons
    {
        public int Id { get; set; }
        public int? ReasonType { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public int? InspectionId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class InspectionCancelReason
    {
        public int InspectionId { get; set; }
        public string CancelType { get; set; }
        public string CancelReason { get; set; }
    }

    public class InspectionPoQuantityDetails
    {
        public int ProductId { get; set; }
        public int PoId { get; set; }
        public int PoQuantity { get; set; }
    }

    public class InspectionProductSubCategory
    {
        public int? ProductCategoryId { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public string ProductSubCategoryName { get; set; }

    }

    public class InspectionProductSubCategory2
    {
        public int? ProductSubCategoryId { get; set; }

        public int? ProductSubCategory2Id { get; set; }

        public string ProductSubCategory2Name { get; set; }
    }

    public class InspectionProductSubCategory3
    {
        public int? ProductSubCategory2Id { get; set; }

        public int? ProductSubCategory3Id { get; set; }

        public string ProductSubCategory3Name { get; set; }
    }

    public class InspectionProductAndReport
    {
        public int InspectionId { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ResultId { get; set; }
    }

    public class InspectionContainerAndReport
    {
        public int InspectionId { get; set; }

        public int? ContainerId { get; set; }

        public int? ResultId { get; set; }
    }
    public class InspectionPoNumberList
    {
        public int InspectionId { get; set; }
        public int ProductRefId { get; set; }
        public int? ContainerRefId { get; set; }
        public string PoNumber { get; set; }

    }

    public class InspectionSupplierFactoryContacts
    {
        public int? InspectionId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }

    }
}

