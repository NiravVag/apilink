using DTO.Common;
using DTO.CommonClass;
using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inspection
{
    public class InspectionBookingDetails
    {
        public int Id { get; set; }

        public Guid GuidId { get; set; }
        [StringLength(1500)]
        public string InternalReferencePo { get; set; }

        public bool IsEmailRequired { get; set; }

        public bool IsFlexibleInspectionDate { get; set; }

        public int CustomerId { get; set; }

        public int SupplierId { get; set; }

        public int? FactoryId { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int? SeasonId { get; set; }

        public int? SeasonYearId { get; set; }

        public int? productCategoryId { get; set; }

        public int ServiceTypeId { get; set; }

        public DateObject ServiceDateFrom { get; set; }

        public DateObject ServiceDateTo { get; set; }

        public DateObject FirstServiceDateFrom { get; set; }

        public DateObject FirstServiceDateTo { get; set; }

        [StringLength(1500)]
        public string CusBookingComments { get; set; }
        [StringLength(1500)]
        public string ApiBookingComments { get; set; }
        [StringLength(1500)]
        public string InternalComments { get; set; }
        [StringLength(1500)]
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
        public List<int> PreviousBookingNoList { get; set; }
        public int? ReinspectionType { get; set; }

        public bool? isPickingRequired { get; set; }

        public bool? isCombineRequired { get; set; }

        public bool? isCombineDone { get; set; }

        public bool? isPickingDone { get; set; }

        public IEnumerable<InspectionPODetails> InspectionPoList { get; set; }

        public List<InspectionPOProductDetails> InspectionProductList { get; set; }

        public IEnumerable<BookingFileAttachment> InspectionFileAttachmentList { get; set; }

        public IEnumerable<int> InspectionCustomerContactList { get; set; }

        public IEnumerable<int?> InspectionFactoryContactList { get; set; }

        public IEnumerable<int> InspectionServiceTypeList { get; set; }

        public IEnumerable<int> InspectionSupplierContactList { get; set; }

        public IEnumerable<int> InspectionCustomerBrandList { get; set; }

        public IEnumerable<int> InspectionCustomerBuyerList { get; set; }

        public IEnumerable<int> InspectionCustomerDepartmentList { get; set; }

        public IEnumerable<int> InspectionCustomerMerchandiserList { get; set; }

        public IEnumerable<InspectionDFTransactions> InspectionDfTransactions { get; set; }

        public IEnumerable<InspectionDFDDLTransactions> InspectionDfDDLTransactions { get; set; }

        public string CustomerBookingNo { get; set; }

        public string HoldReason { get; set; }

        public string HoldReasonType { get; set; }

        public int TotalNumberofReports { get; set; }

        public int? CreatedUserType { get; set; }

        public int? CollectionId { get; set; }

        public int? PriceCategoryId { get; set; }

        public string CollectionName { get; set; }

        public string PriceCategoryName { get; set; }

        public string CompassBookingNo { get; set; }

        public string CSNames { get; set; }

        public List<int> CsList { get; set; }

        public int? BusinessLine { get; set; }

        public int? InspectionLocation { get; set; }

        public string ShipmentPort { get; set; }

        public DateObject ShipmentDate { get; set; }

        public string EAN { get; set; }

        public int? CuProductCategory { get; set; }

        public List<int?> ShipmentTypeList { get; set; }

        public int? BookingType { get; set; }

        public string BookingTypeName { get; set; }

        public int? PaymentOptions { get; set; }

        public string GappaymentSupportingDocument { get; set; }

        public bool PoProductDependentFilter { get; set; }
        public bool IsEaqf { get; set; }

        public bool? GAPDACorrelation { get; set; }
        public string GAPDAName { get; set; }
        public string GAPDAEmail { get; set; }

    }

    public class BookingDateInfo
    {
        public int BookingId { get; set; }
        public DateObject ServiceFromDate { get; set; }
        public DateObject ServiceToDate { get; set; }
    }

    public class ApplicantStaffResponse
    {
        public BookingStaffInfo ApplicantInfo { get; set; }
        public ApplicantStaffResponseResult Result { get; set; }
    }

    public enum ApplicantStaffResponseResult
    {
        Success = 1,
        Fail = 2,
        NoValueFound = 3,
        NotFound = 4
    }

    public class InspectionPOColorTransaction
    {
        public int ColorTransactionId { get; set; }

        public int? PoTransactionId { get; set; }        

        public int? ProductRefId { get; set; }
        public int? ProductId { get; set; }
        public string ColorCode { get; set; }

        public string ColorName { get; set; }

        public int? BookingQuantity { get; set; }

        public int? PickingQuantity { get; set; }
        public int BookingId { get; set; }

        
    }

    public enum BusinessLine
    {
        HardLine = 1,
        SoftLine = 2
    }
}
