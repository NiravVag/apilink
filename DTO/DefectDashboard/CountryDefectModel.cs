using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DefectDashboard
{   

    public class CountryDefectModel
    {
        public string DefectName { get; set; }
        public int Count { get; set; }
        public string Color { get; set; }
        public IEnumerable<DefectCountModel> CountryDefectData { get; set; }
        //same defect name with different countries
    }

    public class CountryDefectChartResponse
    {
        public List<CountryDefectModel> Data { get; set; }
        public List<CountryModel> CountryList { get; set; }
        public DefectDashboardResult Result { get; set; }
    }

    public class CountryModel
    {
        public string CountryName { get; set; }
        public int CountryId { get; set; }
    }

    public class DefectCountModel
    {
        public string DefectName { get; set; }
        public int Count { get; set; }
        public int CountryId { get; set; }
    }

    public class CountryListModel
    {
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public int FactoryId { get; set; }
    }


    public class CountryReport
    {
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public int? ReportId { get; set; }
    }

    public class DefectCountryReport
    {
        public string DefectName { get; set; }
        public int Count { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int ReportId { get; set; }
    }

    public class DefectCountryChartExportResponse
    {
        public DefectDashboardResult Result { get; set; }
        public IEnumerable<CountryDefectModel> Data { get; set; }
        public List<CountryModel> CountryNameList { get; set; }
        public DefectDashboardFilterExport RequestFilters { get; set; }
    }
}
