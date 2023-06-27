using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class InspReportProductsMobileResponse
    {
        public MobileResult meta { get; set; }
        public List<MobileInspectionReportProducData> data { get; set; }
    }

    public class MobileInspectionReportProducData
    {
        public int key { get; set; } 
        public string ProductName { get; set; }
        public string PoNumber { get; set; }
        public int ReportId { get; set; }
        public string ReportNo { get; set; }
        public string ProductDescription { get; set; }
        public int CombineProductCount { get; set; }
        public string InspectionDate { get; set; }
        public string ProductImageUrl { get; set; }
        public string ReportUrl { get; set; }
        public int? ReportResultId { get; set; }
        public string ReportSummaryUrl { get; set; }
        public int? BookingStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ServiceStartDate { get; set; }
        public DateTime? ServiceEndDate { get; set; }
        public int? CombineProductId { get; set; }
        public int? CombineAqlQuantity { get; set; }
    }
}

