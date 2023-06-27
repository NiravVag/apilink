using DTO.Common;
using DTO.CommonClass;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Claim
{
    public class BookingClaimRequest
    {
        [Required]
        public int BookingId { get; set; }
        public List<int?> ReportId { get; set; }
    }

    public class BookingClaimData
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceDate { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string FactoryAddress { get; set; }
        public double? ReportPresentedQty { get; set; }
        public double? ReportInspectedQty { get; set; }
        public string Qc { get; set; }
        public string Cs { get; set; }
        public string ServiceFromDate { get; set; }
        public string ServiceToDate { get; set; }
    }

    public class BookingClaimRepoData
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime ServiceFromDate { get; set; }
        public DateTime ServiceToDate { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string FactoryAddress { get; set; }
        public string FactoryRegionalAddress { get; set; }
        public int ReportPresentedQty { get; set; }
        public int ReportInspectedQty { get; set; }
        public string Qc { get; set; }
        public string Cs { get; set; }
    }

    public class ClaimBookingResponse
    {
        public BookingClaimData Data { get; set; }
        public ClaimResult Result { get; set; }
    }

    public enum ClaimResult
    {
        Success = 1,
        Fail = 2,
        NotFound = 3
    }

    public class FbReportQuantityData
    {
        public int? InspectionId { get; set; }
        public int ReportId { get; set; }
        public double InspectedQty { get; set; }
        public double PresentedQty { get; set; }
        public double OrderQty { get; set; }
    }

    public class ClaimDetails
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string ClaimNo { get; set; }
        public IEnumerable<int> ReportIdList { get; set; }
        public DateObject ClaimDate { get; set; }
        public string RequestContactName { get; set; }
        public int? ClaimFromId { get; set; }
        public int? ReceivedFromId { get; set; }
        public int? ClaimSourceId { get; set; }
        public IEnumerable<int> DefectFamilyIdList { get; set; }
        public IEnumerable<int> ClaimDepartmentIdList { get; set; }
        public string ClaimDescription { get; set; }
        public IEnumerable<int> ClaimCustomerRequestIdList { get; set; }
        public int? PriorityId { get; set; }
        public IEnumerable<int> CustomerRequestRefundIdList { get; set; }
        public double? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public string Customercomment { get; set; }
        public bool? QcControlId { get; set; }
        public double? DefectPercentage { get; set; }
        public int? NoOfPieces { get; set; }
        public double? CompareToAQL { get; set; }
        public int? DefectDistributionId { get; set; }
        public string Color { get; set; }
        public string DefectCartonInspected { get; set; }
        public double? FobPrice { get; set; }
        public int? FobCurrencyId { get; set; }
        public string AnalyzerFeedback { get; set; }
        public double? RetailPrice { get; set; }
        public int? RetailCurrencyId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int? ClaimResultId { get; set; }
        public string ClaimRemarks { get; set; }
        public string ClaimRecommendation { get; set; }
        public IEnumerable<int> FinalDecisionIdList { get; set; }
        public IEnumerable<int> FinalRefundIdList { get; set; }
        public double? FinalAmount { get; set; }
        public int? FinalCurrencyId { get; set; }
        public string ClaimFinalRefundRemarks { get; set; }
        public double? RealInspectionFees { get; set; }
        public int? RealInspectionCurrencyId { get; set; }
        public int? FileTypeId { get; set; }
        public string FileDesc { get; set; }
        public IEnumerable<ClaimAttachmentList> Attachments { get; set; }
    }

    public class ClaimBookingData
    {
        public int CustomerId { get; set; }
        public string CustomerBookingNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMail { get; set; }
        public DateTime ServiceFromDate { get; set; }
        public DateTime ServiceToDate { get; set; }
        public string SupplierName { get; set; }

        public string SupplierContactName { get; set; }

        public string SupplierPhone { get; set; }

        public string SupplierAddress { get; set; }

        public string SupplierMail { get; set; }

        public int ServiceTypeId { get; set; }

        public string ServiceType { get; set; }

        public string FactoryName { get; set; }

        public string FactoryContactName { get; set; }

        public string FactoryPhone { get; set; }

        public string FactoryMail { get; set; }

        public string FactoryAddress { get; set; }

        public string FactoryRegionalAddress { get; set; }
        public bool IsChinaCountry { get; set; }
        public int? OfficeId { get; set; }
        //public InspTranFaContacts inspTranFaContacts { get; set; }
    }

    public class BookingClaimsResponse
    {
        public IEnumerable<BookingClaims> BookingClaims { get; set; }
        public DataSourceResult Result { get; set; }
    }
    public class BookingClaims
    {
        public int Id { get; set; }
        public string ClaimNo { get; set; }
    }

    public class ClaimData
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string ClaimNo { get; set; }
        public IEnumerable<int> ReportIdList { get; set; }
        public DateTime? ClaimDate { get; set; }
        public string RequestContactName { get; set; }
        public int? ClaimFromId { get; set; }
        public int? ReceivedFromId { get; set; }
        public int? ClaimSourceId { get; set; }
        public IEnumerable<int> DefectFamilyIdList { get; set; }
        public IEnumerable<int> ClaimDepartmentIdList { get; set; }
        public string ClaimDescription { get; set; }
        public IEnumerable<int> ClaimCustomerRequestIdList { get; set; }
        public int? PriorityId { get; set; }
        public string Priority { get; set; }
        public IEnumerable<int> CustomerRequestRefundIdList { get; set; }
        public double? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public string Customercomment { get; set; }
        public bool? QcControlId { get; set; }
        public double? DefectPercentage { get; set; }
        public int? NoOfPieces { get; set; }
        public double? CompareToAQL { get; set; }
        public int? DefectDistributionId { get; set; }
        public string Color { get; set; }
        public string DefectCartonInspected { get; set; }
        public double? FobPrice { get; set; }
        public int? FobCurrencyId { get; set; }
        public string AnalyzerFeedback { get; set; }
        public double? RetailPrice { get; set; }
        public int? RetailCurrencyId { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public int? ClaimResultId { get; set; }
        public string ClaimRemarks { get; set; }
        public string ClaimRecommendation { get; set; }
        public IEnumerable<int> FinalDecisionIdList { get; set; }
        public double? FinalAmount { get; set; }
        public int? FinalCurrencyId { get; set; }
        public RefCurrency FinalCurrency { get; set; }
        public string ClaimRefundRemarks { get; set; }
        public double? RealInspectionFees { get; set; }
        public int? RealInspectionCurrencyId { get; set; }
        public ICollection<ClmTranCustomerRequestRefund> ClmTranCustomerRequestRefunds { get; set; }
        public ICollection<ClmTranCustomerRequest> ClmTranCustomerRequests { get; set; }
        public ICollection<ClmTranDefectFamily> ClmTranDefectFamilies { get; set; }
        public ICollection<ClmTranDepartment> ClmTranDepartments { get; set; }
        public ICollection<ClmTranFinalDecision> ClmTranFinalDecisions { get; set; }
        public ICollection<ClmTranReport> ClmTranReports { get; set; }
        public IEnumerable<ClmRefResult> ClmTranFinalDecisionName { get; set; }
        public IEnumerable<ClmTranAttachment> ClmTranAttachments { get; set; }
        public virtual ICollection<ClmTranClaimRefund> ClmTranClaimRefunds { get; set; }
        public IEnumerable<ClmRefFileType> FileType { get; set; }
        public IEnumerable<ClaimAttachmentList> Attachments { get; set; }
    }

    public class FinalDecision
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ClaimAttachment
    {
        public int Id { get; set; }
        public int FileTypeId { get; set; }
        public IEnumerable<ClaimAttachmentList> Attachments { get; set; }
        public string FileDesc { get; set; }
    }
    public class ClaimAttachmentList
    {
        public int Id { get; set; }
        public int? FileTypeId { get; set; }
        public string FileTypeName { get; set; }
        public string uniqueld { get; set; }
        public string FileName { get; set; }
        public bool IsNew { get; set; }
        public string MimeType { get; set; }
        public string FileUrl { get; set; }
        public string FileDesc { get; set; }
    }

    public class InvoiceResponse
    {
        public InvoiceDetail InvoiceDetail { get; set; }
        public DataSourceResult Result { get; set; }
    }
    public class InvoiceDetail
    {
        public int? InspectionId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public DateTime? BilledDate { get; set; }
        public double? TotalInvoiceFees { get; set; }
        public int? InvoiceCurrency { get; set; }
        public string InvoiceCurrencyName { get; set; }
        public RefCurrency InvoiceCurrencyNavigation { get; set; }
    }

    public class ClaimDataSourceResponse
    {
        public IEnumerable<DataSource> DataSource { get; set; }

        public ClaimDataSourceResult Result { get; set; }
    }
    public enum ClaimDataSourceResult
    {
        Success = 1,
        CountryEmpty = 2,
        NotFound = 4,
        CustomerEmpty = 5,
    }
}
