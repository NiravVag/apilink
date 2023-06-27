
using DTO.Common;
using DTO.CommonClass;
using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DTO.Customer
{
    public class SaveComplaintResponse
    {
        public int Id { get; set; }
        public ComplaintResult Result { get; set; }
    }
    public class RemoveComplaintDetailResponse
    {
        public int Id { get; set; }
        public ComplaintResult Result { get; set; }
    }

    public class ComplaintDataResponse
    {
        public IEnumerable<CommonDataSource> ComplaintDataList { get; set; }
        public ComplaintResult Result { get; set; }
    }

    public enum ComplaintResult
    {
        Success = 1,
        CannotGetList = 2,
        Failed = 3,
        RequestNotCorrectFormat = 4,
        ServiceIdRequired = 5,
        ComplaintSavedNotificationError = 6,

    }


    public class BookingNo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class ComplaintBookingDataRepo
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
    }
    public class ComplaintBookingData
    {
        public int BookingNo { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int BookingStatus { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public string FirstProductName { get; set; }
        public string OfficeName { get; set; }
        public int BookingQty { get; set; }
    }
    public class ComplaintBookingDataResponse
    {
        public ComplaintBookingData Data { get; set; }
        public ComplaintResult Result { get; set; }
    }

    public class ComplaintBookingProductDataResponse
    {
        public IEnumerable<BookingProductinfo> Data { get; set; }
        public ComplaintResult Result { get; set; }
    }

    public class ComplaintSummaryRequest
    {
        public int SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        public int Datetypeid { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public int AdvancedSearchtypeId { get; set; }
        public string AdvancedsearchtypeText { get; set; }
        public int? ComplaintTypeId { get; set; }
        public int? ServiceId { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public int? CustomerId { get; set; }
    }

    public class ComplaintSummaryResult
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ComplaintTypeId { get; set; }
        public string ComplaintTypeName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string CreatedBy { get; set; }
        public string ComplaintDate { get; set; }
        public string ServiceDate { get; set; }
        public string BookingNoCustomerNo { get; set; }
    }

    public class ComplaintSummaryRepoResult
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ComplaintTypeId { get; set; }
        public string ComplaintTypeName { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public DateTime? ServiceDateFrom { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        public string CustomerBookingNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int StatusId { get; set; }
    }

    public class ComplaintSummaryResponse
    {
        public List<ComplaintSummaryResult> Data { get; set; }
        public ComplaintResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<InspectionStatus> InspectionStatuslst { get; set; }
    }
    public class ComplaintDetailedInfoResponse
    {
        public ComplaintDetailedInfo Data { get; set; }
        public ComplaintResult Result { get; set; }
    }
    public class ComplaintDetailedInfo
    {
        public int Id { get; set; }
        public int ComplaintTypeId { get; set; }
        public int? ServiceId { get; set; }
        public int? BookingNo { get; set; }
        public DateObject ComplaintDate { get; set; }
        public int RecipientTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int? CustomerId { get; set; }
        public int? CountryId { get; set; }
        public int? OfficeId { get; set; }
        public string Remarks { get; set; }
        public IEnumerable<ComplaintDetail> ComplaintDetails { get; set; }
        public IEnumerable<int> UserIds { get; set; }
    }
    public class ComplaintDetailedRepo
    {
        public int Id { get; set; }
        public int ComplaintTypeId { get; set; }
        public int? ServiceId { get; set; }
        public int? BookingNo { get; set; }
        public int? AuditId { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public int RecipientTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int? CustomerId { get; set; }
        public int? CountryId { get; set; }
        public int? OfficeId { get; set; }
        public string Remarks { get; set; }

    }

    public class ComplaintDetail
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public DateObject AnswerDate { get; set; }
        public string Description { get; set; }
        public string CorrectiveAction { get; set; }
        public string Remarks { get; set; }
    }
    public class ComplaintDetailRepo
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public DateTime? AnswerDate { get; set; }
        public string Description { get; set; }
        public string CorrectiveAction { get; set; }
        public string Remarks { get; set; }
    }

    public class DeleteComplaintResponse
    {
        public int Id { get; set; }
        public ComplaintResult Result { get; set; }
    }


    public class ExportComplaintSummaryRepoResult
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? AuditId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string Factory { get; set; }
        public string ComplaintTypeName { get; set; }
        public string ComplaintOffice { get; set; }
        public string ComplaintCountry { get; set; }
        public string ServiceName { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public DateTime? ServiceDateFrom { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        public string ServiceType { get; set; }
        public string RecipientType { get; set; }
        public string Department { get; set; }
        public string PersonInCharge { get; set; }
        public int? ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string Category { get; set; }
        public string ComplaintDescription { get; set; }
        public string CorrectiveAction { get; set; }
        public DateTime? AnswerDate { get; set; }
        public string Remarks { get; set; }
        public List<ExportComplaintDetailRepo> ComplaintDetails { get; set; }
    }
    public class ExportComplaintSummaryResult
    {
        [Description("Inspection#")]
        public int? BookingId { get; set; }
        [Description("Audit#")]
        public int? AuditId { get; set; }
        [Description("Customer")]
        public string CustomerName { get; set; }
        [Description("Complaint Type")]
        public string ComplaintTypeName { get; set; }
        [Description("Complaint Office")]
        public string ComplaintOffice { get; set; }
        [Description("Complaint Country")]
        public string ComplaintCountry { get; set; }
        [Description("Complaint Date")]
        public DateTime? ComplaintDate { get; set; }
        [Description("Service")]
        public string ServiceName { get; set; }
        [Description("Service Date")]
        public string ServiceDate { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }

        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Recipient Type")]
        public string RecipientType { get; set; }
        [Description("Department")]
        public string Department { get; set; }
        [Description("Person In Charge")]
        public string PersonInCharge { get; set; }
        [Description("Product Id/Style")]
        public int? ProductId { get; set; }
        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("Category")]
        public string Category { get; set; }
        [Description("Complaint Description")]
        public string ComplaintDescription { get; set; }
        [Description("Corrective Action")]
        public string CorrectiveAction { get; set; }
        [Description("Answer Date")]
        public DateTime? AnswerDate { get; set; }
        [Description("Remarks")]
        public string Remarks { get; set; }

        [Description("Comments")]
        public string Comments { get; set; }
    }
    public class ExportComplaintSummaryResponse
    {
        public List<ExportComplaintSummaryRepoResult> Data { get; set; }
        public ComplaintResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
    public class ExportComplaintDetailRepo
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public int? ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductDescription { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public DateTime? AnswerDate { get; set; }
        public string Description { get; set; }
        public string CorrectiveAction { get; set; }
        public string Remarks { get; set; }
    }
}
