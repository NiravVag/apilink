using DTO.Common;
using DTO.References;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.Inspection
{
    public class SaveInsepectionRequest
    {

        public int Id { get; set; }

        public int? DraftInspectionId { get; set; }

        public Guid GuidId { get; set; }
        [StringLength(1500)]
        public string InternalReferencePo { get; set; }

        public bool IsEmailRequired { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EDIT_BOOKING.MSG_CUS_REQ")]
        public int CustomerId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EDIT_BOOKING.MSG_SUP_REQ")]
        public int SupplierId { get; set; }

        //[RequiredGreaterThanZero(ErrorMessage = "EDIT_BOOKING.MSG_FACT_REQ")]
        public int? FactoryId { get; set; }

        public int StatusId { get; set; }

        public bool IsFlexibleInspectionDate { get; set; }

        public int? SeasonId { get; set; }

        public int? SeasonYearId { get; set; }

        public int ServiceTypeId { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        // [DateShouldBeGreaterInNew(otherPropertyName = "Id", ErrorMessage = "EDIT_BOOKING.MSG_Service_FROMDATE_FUTURE_REQ")]
        public DateObject ServiceDateFrom { get; set; }

        [DateGreaterThanAttribute(otherPropertyName = "ServiceDateFrom", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        // [DateShouldBeGreaterInNew(otherPropertyName = "Id", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_FUTURE_REQ")]
        public DateObject ServiceDateTo { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        //[DateShouldBeGreaterInNew(otherPropertyName = "Id", ErrorMessage = "EDIT_BOOKING.LBL_Service_FIRST_FROMDATE")]
        public DateObject FirstServiceDateFrom { get; set; }

        [DateGreaterThanAttribute(otherPropertyName = "FirstServiceDateFrom", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_LESS_REQ")]
        // [DateShouldBeGreaterInNew(otherPropertyName = "Id", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_FUTURE_REQ")]
        public DateObject FirstServiceDateTo { get; set; }

        [StringLength(5000)]
        public string CusBookingComments { get; set; }
        [StringLength(5000)]
        public string ApiBookingComments { get; set; }
        [StringLength(5000)]
        public string InternalComments { get; set; }
        [StringLength(5000)]
        public string QCBookingComments { get; set; }

        public int? OfficeId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int EntityId { get; set; }

        [StringLength(200)]
        public string ApplicantName { get; set; }

        [StringLength(200)]
        public string ApplicantEmail { get; set; }

        [StringLength(200)]
        public string ApplicantPhoneNo { get; set; }

        public int? PreviousBookingNo { get; set; }
        public int? SplitPreviousBookingNo { get; set; }
        public int? ReinspectionType { get; set; }

        public bool? IsPickingRequired { get; set; }

        public int? inspectionPageType { get; set; }

        [RequiredList(ErrorMessage = "EDIT_BOOKING.MSG_BOOKING_SELECT_PO")]
        public List<SaveInspectionPOProductDetails> InspectionProductList { get; set; }

        public IEnumerable<BookingFileAttachment> InspectionFileAttachmentList { get; set; }

        public IEnumerable<InspectionDFTransactions> InspectionDFTransactions { get; set; }

        [RequiredList(ErrorMessage = "EDIT_BOOKING.MSG_CUS_CONTACT_REQ")]
        public IEnumerable<int> InspectionCustomerContactList { get; set; }

        //[RequiredList(ErrorMessage = "EDIT_BOOKING.MSG_FACT_CONTACT_REQ")]
        public IEnumerable<int?> InspectionFactoryContactList { get; set; }

        [RequiredList(ErrorMessage = "EDIT_BOOKING.MSG_CUS_SERVICETYPE_REQ")]
        public IEnumerable<int> InspectionServiceTypeList { get; set; }

        [RequiredList(ErrorMessage = "EDIT_BOOKING.MSG_SUP_CONTACT_REQ")]
        public IEnumerable<int> InspectionSupplierContactList { get; set; }

        public IEnumerable<int> InspectionCustomerBrandList { get; set; }

        public IEnumerable<int> InspectionCustomerBuyerList { get; set; }

        public IEnumerable<int> InspectionCustomerDepartmentList { get; set; }

        public IEnumerable<int> InspectionCustomerMerchandiserList { get; set; }

        public string CustomerBookingNo { get; set; }

        public bool IsSupplierOrFactoryEmailSend { get; set; }

        public bool IsCustomerEmailSend { get; set; }

        public int? collectionId { get; set; }

        public int? priceCategoryId { get; set; }

        public string CompassBookingNo { get; set; }

        public bool IsCombineRequired { get; set; }



        public int? HoldReasonTypeId { get; set; }

        public string HoldReason { get; set; }

        public int? BusinessLine { get; set; }

        public int? InspectionLocation { get; set; }

        public string ShipmentPort { get; set; }

        public DateObject ShipmentDate { get; set; }

        public string EAN { get; set; }

        public int? CuProductCategory { get; set; }

        public List<int?> ShipmentTypeIds { get; set; }

        public List<int> CsList { get; set; }

        public bool IsUpdateBookingDetail { get; set; }

        public int UserId { get; set; }
        public int UserType { get; set; }

        public int? BookingType { get; set; }

        public int? PaymentOptions { get; set; }
        public bool IsEaqf { get; set; }
        public int? ReportRequest { get; set; }
        public bool IsSameDayReport { get; set; }

        public bool? GAPDACorrelation { get; set; }
        public string GAPDAName { get; set; }
        public string GAPDAEmail { get; set; }

    }

    public class SaveInspectionPODetails
    {
        public int Id { get; set; }

        public int PoId { get; set; }

        public string PoName { get; set; }

        public string ProductName { get; set; }

        public int PoDetailId { get; set; }

        public int ProductId { get; set; }

        public string ProductDesc { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public int? ProductCategorySub2Id { get; set; }

        public int InspectionId { get; set; }

        public int Unit { get; set; }

        public int? UnitCount { get; set; }

        public int BookingQuantity { get; set; }

        public int ExistingBookingQuantity { get; set; }

        public int PoQuantity { get; set; }

        public int PoReminingQuantity { get; set; }

        public int? PickingQuantity { get; set; }

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

        public string ProductSubCategoryName { get; set; }

        public string ProductCategorySub2Name { get; set; }

        public string AqlName { get; set; }

        public string UnitName { get; set; }

        public int? DestinationCountryID { get; set; }

        public string FactoryReference { get; set; }

        public int? CombineProductId { get; set; }

        public int? AqlQuantity { get; set; }

        public int? CombineAqlQuantity { get; set; }

        public DateObject Etd { get; set; }

        public string Barcode { get; set; }

        public IEnumerable<ProductFileAttachment> ProductFileAttachments { get; set; }

        public IEnumerable<ProductCategorySub2> BookingCategorySub2ProductList { get; set; }

        public IEnumerable<InspectionPickingData> InspectionPickingList { get; set; }

        

    }

    public class SaveInspectionPOProductDetails
    {
        public int Id { get; set; }

        public int PoId { get; set; }

        public string PoName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDesc { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public int? ProductCategorySub2Id { get; set; }

        public int? ProductCategorySub3Id { get; set; }

        public int? InspectionId { get; set; }

        public int Unit { get; set; }

        public int? UnitCount { get; set; }

        public int? TotalBookingQuantity { get; set; }

        public string Remarks { get; set; }

        public int? Aql { get; set; }

        public int? Critical { get; set; }

        public int? Major { get; set; }

        public int? Minor { get; set; }

        public string ProductSubCategoryName { get; set; }

        public string ProductCategorySub2Name { get; set; }

        public string AqlName { get; set; }

        public string UnitName { get; set; }

        public string FactoryReference { get; set; }

        public int? CombineProductId { get; set; }

        public int? AqlQuantity { get; set; }

        public int? SampleType { get; set; }

        public int? CombineAqlQuantity { get; set; }

        public string Barcode { get; set; }

        public bool IsEcopack { get; set; }

        public bool IsDisplayMaster { get; set; }

        public int? ParentProductId { get; set; }

        public int? FbTemplateId { get; set; }

        public DateObject AsReceivedDate { get; set; }

        public DateObject TfReceivedDate { get; set; }

        public List<ProductFileAttachment> ProductFileAttachments { get; set; }

        public int PoTransactionId { get; set; }

        public int? ContainerId { get; set; }

        public int BookingQuantity { get; set; }

        public int? PoQuantity { get; set; }

        public int? PickingQuantity { get; set; }

        public int? DestinationCountryID { get; set; }

        public DateObject Etd { get; set; }

        public string CustomerReferencePo { get; set; }

        public bool? IsPoProductVisible { get; set; }

        public int ColorTransactionId { get; set; }

        public string ColorCode { get; set; }

        public string ColorName { get; set; }
        public bool? IsGoldenSampleAvailable { get; set; }
        [StringLength(500)]
        public string GoldenSampleComments { get; set; }
        public bool? IsSampleCollection { get; set; }
        [StringLength(500)]
        public string SampleCollectionComments { get; set; }
        public int? ProductionStatus { get; set; }
        public int? PackingStatus { get; set; }

        public List<SaveInspectionPickingDetails> SaveInspectionPickingList { get; set; }

    }

    public class SaveInspectionPickingDetails
    {
        public int Id { get; set; }
        public int? LabOrCustomerId { get; set; }
        public int? LabOrCustomerAddressId { get; set; }
        public int PickingQuantity { get; set; }
        public string Remarks { get; set; }
        public int? LabType { get; set; }
        //public List<int> LabOrCustomerContactIds { get; set; }
        public List<SavePickingContact> PickingContactList { get; set; }
        public string LabOrCustomerName { get; set; }
        public string LabOrCustomerAddressName { get; set; }
        public string LabOrCustomerContactName { get; set; }

    }

    public class SavePickingContact
    {
        public int Id { get; set; }

        public int LabOrCustomerContactId { get; set; }

        public string LabOrCustomerContactName { get; set; }

    }


    public class PoProductData
    {
        public int poId { get; set; }
        public int productId { get; set; }
    }

    public class PoProductMappedRequest
    {
        public int poId { get; set; }
        public int productId { get; set; }
    }

    public class PoProductNotMappedData
    {
        public int poId { get; set; }
        public int productId { get; set; }
    }

    public class PoProductMappedData
    {
        public int poId { get; set; }
        public int productId { get; set; }
    }

    public class BookingPoSearchData
    {
        public List<int> PoList { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }

    }

    public class BookingCustomerServiceTypeData
    {
        public int CustomerId { get; set; }
        public int ServiceTypeId { get; set; }
    }
    public class ProductTranData
    {
        public string Unit { get; set; }
        public string ProductCode { get; set; }
        public int BookingId { get; set; }
        public int? FbReportId { get; set; }
        public int ProductRefId { get; set; }
    }
    public class InspectionCsData
    {
        public int? InspectionId { get; set; }
        public int? CsId { get; set; }
        public string CsName { get; set; }
    }

}
