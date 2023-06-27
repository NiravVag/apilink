using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DefectDashboard
{
    public class DefectYearCountDataModel
    {
        public string DefectName { get; set; }
        public int DefectYearCount { get; set; }
        public string Color { get; set; }
        public List<DefectMonth> DefectMonthList { get; set; }
    }

    public class DefectMonth
    {
        public int Month { get; set; }
        public int Year { get; set; }
        //public int? Critical { get; set; }
        //public int? Major { get; set; }
        //public int? Minor { get; set; }
        public string MonthName { get; set; }
        public int? DefectMonthCount { get; set; }
        public string DefectName { get; set; }
    }

    public class DefectMonthRepo
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int? Critical { get; set; }
    }

    public class InspectionMonthRepo
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int ReportId { get; set; }
    }

    public class DefectReportRepo
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Critical { get; set; }
        public int ReportId { get; set; }
    }



    public class DefectYearCountResponse
    {
        public List<DefectMonthRepo> DefectCountList { get; set; }
        public DefectDashboardResult Result { get; set; }
        public List<int> ReportIdList { get; set; }
        public List<DefectYear> MonthXAxis { get; set; }
    }

    public class DefectYearInnerCountResponse
    {
        public List<DefectYearCountDataModel> DefectCountList { get; set; }
        public DefectDashboardResult Result { get; set; }
        public List<DefectYear> MonthXAxis { get; set; }
    }

    public class DefectYear
    {
        public int Year { get; set; }
        public int Month { get; set; }
        //public string MonthName { get; set; }
    }

    public class DefectMonthExport
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public string MonthName { get; set; }
    }

    public class DefectYearExportResponse
    {
        public IEnumerable<DefectMonthExport> MonthDefectData { get; set; }
        public DefectDashboardFilterExport RequestFilters { get; set; }
        public DefectDashboardResult Result { get; set; }

        //public int Total { get; set; }
    }

}
