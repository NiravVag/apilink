using DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class SaveDraftInsepectionResponse
    {
        public SaveDraftInspectionResult Result { get; set; }
        public int DraftInspectionId { get; set; }
    }

    public enum SaveDraftInspectionResult
    {
        Success=1,
        Failure=2
    }


    public class DraftInspectionRequest
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public DateObject ServiceDateFrom { get; set; }
        public DateObject ServiceDateTo { get; set; }
        public int? BrandId { get; set; }
        public int? DepartmentId { get; set; }
        public string BookingInfo { get; set; }
        public bool? IsReInspectionDraft { get; set; }
        public bool? IsReBookingDraft { get; set; }
        public int? PreviousBookingNo { get; set; }
    }

    public class SaveDraftInsepectionRequest
    {

        public int Id { get; set; }

        public string InternalReferencePo { get; set; }

        public bool IsEmailRequired { get; set; }

        public int? CustomerId { get; set; }

        public int? SupplierId { get; set; }

        public int? FactoryId { get; set; }

        public int? StatusId { get; set; }

        public bool IsFlexibleInspectionDate { get; set; }

        public int? SeasonId { get; set; }

        public int? SeasonYearId { get; set; }

        public int? ServiceTypeId { get; set; }

        public DateObject ServiceDateFrom { get; set; }

        public DateObject ServiceDateTo { get; set; }

        public DateObject FirstServiceDateFrom { get; set; }

        public DateObject FirstServiceDateTo { get; set; }

        public string CusBookingComments { get; set; }
        public string ApiBookingComments { get; set; }
        public string InternalComments { get; set; }
        public string QCBookingComments { get; set; }

        public int? OfficeId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? EntityId { get; set; }

        public string ApplicantName { get; set; }

        public string ApplicantEmail { get; set; }

        public string ApplicantPhoneNo { get; set; }

        public int? PreviousBookingNo { get; set; }
        public int? SplitPreviousBookingNo { get; set; }
        public int? ReinspectionType { get; set; }

        public bool? IsPickingRequired { get; set; }

        public int? inspectionPageType { get; set; }

        public List<SaveDraftInspectionPOProductDetails> InspectionProductList { get; set; }

        public IEnumerable<BookingFileAttachment> InspectionFileAttachmentList { get; set; }

        public IEnumerable<InspectionDFTransactions> InspectionDFTransactions { get; set; }

        public IEnumerable<int> InspectionCustomerContactList { get; set; }

        public IEnumerable<int?> InspectionFactoryContactList { get; set; }

        public IEnumerable<int> InspectionServiceTypeList { get; set; }

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

        public List<int?> CsList { get; set; }
    }

    public class SaveDraftInspectionPOProductDetails
    {
        public int? Id { get; set; }

        public int? PoId { get; set; }

        public string PoName { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDesc { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public int? ProductCategorySub2Id { get; set; }

        public int? ProductCategorySub3Id { get; set; }

        public int? InspectionId { get; set; }

        public int? Unit { get; set; }

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

        public int? PoTransactionId { get; set; }

        public int? ContainerId { get; set; }

        public int? BookingQuantity { get; set; }

        public int? PoQuantity { get; set; }

        public int? PickingQuantity { get; set; }

        public int? DestinationCountryID { get; set; }

        public DateObject Etd { get; set; }

        public string CustomerReferencePo { get; set; }

        public bool? IsPoProductVisible { get; set; }

        public int? ColorTransactionId { get; set; }

        public string ColorCode { get; set; }

        public string ColorName { get; set; }

    }

}
