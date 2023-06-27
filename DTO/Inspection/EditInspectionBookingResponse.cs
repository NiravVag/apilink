using DTO.CombineOrders;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.InspectionPicking;
using DTO.References;
using DTO.Supplier;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class EditInspectionBookingResponse
    {
        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public IEnumerable<SupplierItem> FactoryList { get; set; }

        public UserTypeEnum UserType { get; set; }

        public bool IsBookingRequestRole { get; set; }

        public bool IsBookingConfirmRole { get; set; }

        public bool IsBookingVerifyRole { get; set; }

        public bool IsBookingInvoiced { get; set; }

        public double TotalMandays { get; set; }

        public InspectionBookingDetails BookingDetails { get; set; }

        public EditCustomerServiceConfigData BookingServiceConfig { get; set; }

        public EditInspectionBookingResult Result { get; set; }
    }

    public class PickingAndCombineOrderResponse
    {
        
        public InspectionPickingSummaryResponse PickingResponse { get; set; }

        public CombineOrderSummaryResponse CombineOrderResponse { get; set; }

        public PickingAndCombineOrderResult Result { get; set; }
    }
    public enum PickingAndCombineOrderResult
    {
        Success = 1,
        CannotGetPickingList = 2,
        CannotGetCombineOrderList = 3,
    }

    public class EditInspectionFactDetails
    {
        public IEnumerable<suppliercontact> FactoryContactList { get; set; }

        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public string FactoryAddress { get; set; }

        public string FactoryRegionalAddress { get; set; }

        public string FactoryCode { get; set; }

        public string PhoneNumber { get; set; }

        public int? OfficeId { get; set; }

        public EditInspectionBookingResult Result { get; set; }

        public IEnumerable<CustomerCS> AuditCS { get; set; }
    }

    public class EditInspectionBookingSupDetails
    {
        public IEnumerable<SupplierItem> FactoryList { get; set; }

        public IEnumerable<suppliercontact> SupplierContactList { get; set; }

        public string SupplierCode { get; set; }

        public string SupplierPhoneNumber { get; set; }

        public EditInspectionBookingResult Result { get; set; }
    }

    public enum EditInspectionBookingResult
    {
        Success = 1,
        CannotGetCustomerList = 2,
        CannotGetCustomerBrandList = 3,
        CannotGetCustomerDepartmentList = 4,
        CannotGetCustomerContactList = 5,
        CannotGetSeasonList = 6,
        CannotGetServiceTypeList = 7,
        CannotGetSelectSeasonList = 9,
        CannotGetOfficeList = 10,
        CannotGetUnitList = 11,
        CannotGetEvalutionRoundList = 12,
        CannotGetSupplierList = 13,
        CannotGetfactoryList = 14,
        CannotGetFabricCategoryList = 15,
        CannotGetProcessTypeList = 16,
        CannotGetProductionStatusList = 17,
        CannotGetProductionQuantityList = 18,
        GetAuditDetailsByCustomerIdSuccess = 19,
        GetSupplierDetailsBySupplierCUstomerIdSuccess = 20,
        CannotGetSupplierContactList = 21,
        CannotGetSupplierDetails = 22,
        GetFactoryDetailsByIdSuccess = 23,
        CannotGetFactoryContactList = 24,
        CannotGetFactoryDetails = 25,
        CanotGetCustomerDetails = 26,
        CannotGetBookingDetails = 27,
        CannotGetBookingRule = 28,
        CannotGetContactDetails = 29,
        GetContactDetailsSuccess = 30
    }
    public class PriceCategoryRequest
    {
        public int CustomerId { get; set; }
        public int? PriceCategoryId { get; set; }
        public IEnumerable<int?> ProductSubCategory2IdList { get; set; }
    }
    public class PriceCategoryResponse
    {
        public string PriceCategoryName { get; set; }
        public int PriceCategoryId { get; set; }
        public PriceCategoryResult Result { get; set; }
    }
    public enum PriceCategoryResult
    {
        Success = 1,
        MultiplePriceCategory = 2,
        MismatchPriceCategory = 3,
        NodataFound = 4,
        RequestNotCorrectFormat = 5,
        SelectPriceCategory = 6
    }
    public class PriceCategoryDetails
    {
        public string PriceCategoryName { get; set; }
        public int PriceCategoryId { get; set; }
    }

    public class AddInspectionResponse
    {
        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public IEnumerable<SupplierItem> FactoryList { get; set; }

        public UserTypeEnum UserType { get; set; }

        public bool IsBookingRequestRole { get; set; }

        public bool IsBookingConfirmRole { get; set; }

        public bool IsBookingVerifyRole { get; set; }

        public AddInspectionBookingResult Result { get; set; }
    }

    public enum AddInspectionBookingResult
    {
        Success = 1,
        CannotGetSupplierList=2,
        CannotGetFactoryList = 3

    }

    public class InspectionPOProductDetails
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

        public int PoTransactionId { get; set; }

        public int ProductRefId { get; set; }

        public int? PoId { get; set; }

        public string PoName { get; set; }

        public int BookingQuantity { get; set; }

        public int ExistingBookingQuantity { get; set; }

        public int? PoQuantity { get; set; }

        public int PoReminingQuantity { get; set; }

        public int? PickingQuantity { get; set; }

        public int? DestinationCountryID { get; set; }

        public string DestinationCountryName { get; set; }

        public int? ContainerId { get; set; }

        public DateObject ETD { get; set; }

        public DateTime? ETDDate { get; set; }

        public string CustomerReferencePo { get; set; }

        public string BaseCustomerReferencePo { get; set; }

        public List<ProductFileAttachmentRepsonse> ProductFileAttachments { get; set; }

        public List<CommonDataSource> BookingCategorySubProductList { get; set; }

        public List<CommonDataSource> BookingCategorySub2ProductList { get; set; }

        public List<CommonDataSource> BookingCategorySub3ProductList { get; set; }

        public List<SaveInspectionPickingDetails> SaveInspectionPickingList { get; set; }

        public bool IsEcopack { get; set; }

        public bool IsDisplayMaster { get; set; }

        public int? FBTemplateId { get; set; }

        public string FbTemplateName { get; set; }

        public int? ParentProductId { get; set; }

        public string ParentProductName { get; set; }

        public bool? IsNewProduct { get; set; }

        public int ColorTransactionId { get; set; }

        public string ColorCode { get; set; }

        public string ColorName { get; set; }

        public DateObject AsReceivedDate { get; set; }

        public DateObject TfReceivedDate { get; set; }

        public int ProductImageCount { get; set; }
        public string UnitNameCount { get; set; }

    }

    public class InspectionPickingDetails
    {
        public int Id { get; set; }
        public int? labId { get; set; }
        public string labName { get; set; }

        public string labAddress { get; set; }

        public int? labAddressId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerAddressId { get; set; }
        public string CustomerAddress { get; set; }
        public int PoTransactionId { get; set; }
        public int? InspectionId { get; set; }

        public int PickingQuantity { get; set; }
        public string Remarks { get; set; }
    }

    public class InspectionPickingContactDetails
    {
        public int Id { get; set; }
        public int InspectionPickingId { get; set; }
        public int? LabContactId { get; set; }
        public int? CustomerContactId { get; set; }
        public string LabContactName { get; set; }
        public string CustomerContactName { get; set; }
    }
}
