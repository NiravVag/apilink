using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class EditAuditReportResponse
    {
    }
    public class AuditBasicDetails
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

        public int StatusId { get; set; }

        public string CustomerBookingNo { get; set; }

    }
    public class AuditBasicDetailsResponse
    {
        public AuditBasicDetails Data { get; set; }
        public AuditBasicDetailsResponseResult Result { get; set; }
    }
    public enum AuditBasicDetailsResponseResult
    {
        success=1,
        CannotGetAuditDetails = 2
    }
}
