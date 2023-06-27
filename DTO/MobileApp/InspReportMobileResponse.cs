using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class InspReportMobileResponse
    {
        public MobileResult meta { get; set; }
        public List<MobileInspectionReportData> data { get; set; }
    }

    public class MobileInspectionReportData
    {
        public int key { get; set; }
        public int? supplierId { get; set; }
        public string supplierName { get; set; }
        public int totalReports { get; set; }
        public int totalProducts { get; set; }
        public int totalInspections { get; set; }
    }
}
