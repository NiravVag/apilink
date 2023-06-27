using DTO.Audit;
using DTO.Common;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.AuditReport
{
    public class AuditCusReportBookingDetails
    {
        public int AuditId { get; set; }
        public string ReportNo { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public string ServiceDate { get; set; }
        public string ServiceType { get; set; }
        public string OfficeName { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public int? Reportid { get; set; }
        public string MimeType { get; set; }
        public string pathextension { get; set; }
        public string CustomerBookingNo { get; set; }
        public string ReportUrl { get; set; }
        public string FbReportUrl { get; set; }
        public string ReportFileUniqueId { get; set; }
        public string ReportFileName { get; set; }
        public string FactoryCountry { get; set; }
        public int? Fbreportid { get; set; }
    }
    public class AuditRepoCusReportBookingDetails
    {
        public int AuditId { get; set; }
        public string ReportNo { get; set; }
        public string Customer { get; set; }
        public int CustomerId { get; set; }
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        public string Factory { get; set; }
        public int FactoryId { get; set; }
        public DateTime ServiceFromDate { get; set; }
        public DateTime ServiceToDate { get; set; }
        public string officeName { get; set; }
        public int? officeId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CustomerBookingNo { get; set; }
        public int? ReportId { get; set; }
        public string ReportUrl { get; set; }
    }
    public class AuditCusReportBookingDetailsResponse
    {
        public IEnumerable<AuditCusReportBookingDetails> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<AuditStatus> AuditStatuslst { get; set; }

        public AuditCusReportBookingDetailsResult Result { get; set; }
    }
    public enum AuditCusReportBookingDetailsResult
    {
        Success = 1,
        NotFound = 2,
        Error = 3,
        RequestError = 4
    }
    public class AuditCusReportBookingDetailsRequest
    {
        public int SearchTypeId { get; set; }

        public string SearchTypeText { get; set; }

        public int? CustomerId { get; set; }

        public int? SupplierId { get; set; }

        public IEnumerable<int> FactoryIdlst { get; set; }

        public IEnumerable<int> StatusIdlst { get; set; }

        public IEnumerable<int> ServiceTypelst { get; set; }

        public int DateTypeid { get; set; }

        public DateObject FromDate { get; set; }

        public DateObject ToDate { get; set; }

        public IEnumerable<int?> Officeidlst { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }
        public IEnumerable<int> FactoryCountryIdList { get; set; }

    }
    public class AuditServiceType
    {
        public int bookingid { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public bool Active { get; set; }
    }
    public class AuditcusReport
    {
        public int AuditId { get; set; }
        public int ReportId { get; set; }
        public string filename { get; set; }
        public string fileUrl { get; set; }
        public string ReportFileUniqueId { get; set; }
        public int? FbReportid { get; set; }
    }
    public class AuditFactoryCountry
    {
        public int FactoryCountryId { get; set; }
        public int AuditId { get; set; }
        public string CountryName { get; set; }
    }
}
