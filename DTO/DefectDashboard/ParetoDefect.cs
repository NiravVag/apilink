using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DefectDashboard
{
    public class ParetoDefect
    {
        public string Color { get; set; }
        public string DefectName { get; set; }
        public int DefectCount { get; set; }
        public double Percentage { get; set; }
    }

    public class ParetoDefectExport
    {
        public string DefectName { get; set; }
        public int DefectCount { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public double Percentage { get; set; }
    }

    public class ParetoDefectRepo
    {
        public string DefectName { get; set; }
        //public int? DefectCount { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int ReportId { get; set; }
    }

    public class ParetoDefectExportResponse
    {
        public IEnumerable<ParetoDefectExport> ParetoList { get; set; }
        public DefectDashboardResult Result { get; set; }
        public DefectDashboardFilterExport RequestFilters { get; set; }
    }

    public class ParetoDefectResponse
    {
        public IEnumerable<ParetoDefect> ParetoList { get; set; }
        public DefectDashboardResult Result { get; set; }
    }
}
