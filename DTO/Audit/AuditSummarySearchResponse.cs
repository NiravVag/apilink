using DTO.Quotation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class AuditSummarySearchResponse
    {
        public IEnumerable<AuditItem> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<AuditStatus> AuditStatuslst { get; set; }

        public AuditSummarySearchResponseResult Result { get; set; }
    }
    public class AuditItem
    {
        public int AuditId { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public string ServiceType { get; set; }

        public string ServiceDateFrom { get; set; }

        public string ServiceDateTo { get; set; }

        public string PoNumber { get; set; }

        public string ReportNo { get; set; }

        public string Office { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int? BookingCreatedBy { get; set; }

        public QuotStatus QuotationStatus { get; set; }
        public string CustomerBookingNo { get; set; }
        public string FactoryCountry { get; set; }
        public string FactoryState { get; set; }
        public string FactoryCity { get; set; }
        public string Instruction { get; set; }
        public string EaqfReference { get; set; }
        public string Auditors { get; set; }
        public string SupplierCustomerCode { get; set; }
        public string FactoryCustomerCode { get; set; }
        public string CreatedOnEaqf { get; set; }
    }

    public class AuditRepoItem
    {
        public int AuditId { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public DateTime ServiceDateFrom { get; set; }

        public DateTime ServiceDateTo { get; set; }

        public string PoNumber { get; set; }

        public string ReportNo { get; set; }

        public string Office { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int? BookingCreatedBy { get; set; }

        public QuotStatus QuotationStatus { get; set; }
        public string CustomerBookingNo { get; set; }

        public string CreatedOnEaqf { get; set; }
    }

    public enum AuditSummarySearchResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }

}
