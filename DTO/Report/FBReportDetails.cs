using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Report
{
    public class FBReportDetails
    {
        public int? ReportId { get; set; }

        public int? FbReportId { get; set; }

        public string StatusName { get; set; }

        public string FillingStatus { get; set; }

        public string ReviewStatus { get; set; }

        public int? FillingStatusId { get; set; }

        public int? ReviewStatusId { get; set; }

        public int? ReportStatusId { get; set; }

        public double? InspectedQuantity { get; set; }

        public string FinalReportPath { get; set; }

        public string OverAllResult { get; set; }

        public string ReportTitle { get; set; }

        public string ReportResult { get; set; }

        public string ReportStatus { get; set; }

        public string ReportPath { get; set; }

        public string FinalManualReportPath { get; set; }

        public int BookingId { get; set; }
    }

    public enum DynamicDropDownSourceType
    {
        AuditProductCategory = 10
    }
}
