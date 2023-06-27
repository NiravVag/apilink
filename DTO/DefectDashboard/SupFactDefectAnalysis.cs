using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DTO.DefectDashboard
{
    public class SupFactDefectAnalysis
    {
        public string SupOrFactName { get; set; }
        public int? SupOrFactId { get; set; }
        public int Critical { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int TotalDefect { get; set; }
        public int TotalReports { get; set; }
        public List<ReportDefectInfo> DefectReportInfo { get; set; }
        public List<int> DefectCriticalIds { get; set; }
        public IEnumerable<int> DefectMajorIds { get; set; }
        public IEnumerable<int> DefectMinorIds { get; set; }
        public IEnumerable<int> DefectPhotoIds { get; set; }

        public bool IsMajorShow { get; set; }
        public bool IsMinorShow { get; set; }
        public bool IsCriticalShow { get; set; }
    }
    public class ReportDefectInfo
    {
        public string ReportNo { get; set; }
        public string ReportLink { get; set; }
        public string FinalManualReportLink { get; set; }
    }

    public class DefectPerformanceFilter
    {
        public string DefectName { get; set; }
        //public int? DefectId { get; set; }
        public IEnumerable<int?> DefectSelected { get; set; }
        public int? TypeId { get; set; }
        public int SupOrFactId { get; set; }
        public int? DefectSelect { get; set; }
    }

    public class DefectPerformanceAnalysis
    {
        public DefectDashboardFilterRequest TopPerformanceFilter { get; set; }
        public DefectPerformanceFilter InnerPerformanceFilter { get; set; }
    }

    public class DefectPerformanceResponse
    {
        public DefectDashboardResult Result { get; set; }
        public IEnumerable<SupFactDefectAnalysis> PerformanceDefectList { get; set; }
    }

    public class DefectPerformanceExportResponse
    {
        public DefectDashboardFilterExport RequestFilters { get; set; }
        public DefectSubSupOrFactFilterExport RequestSubFilters { get; set; }
        public DefectDashboardResult Result { get; set; }
        public IEnumerable<SupFactDefectAnalysis> PerformanceDefectList { get; set; }
    }

    public class ReportDefectDetailsRepo
    {
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int? ReportId { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int? TotalDefects { get; set; }
        public int? TotalReports { get; set; }
        public string DefectName { get; set; }
        public List<string> DefectNameList { get; set; }
    }

    public class ReportDefectListRepo
    {
        public int ReportId { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public string DefectName { get; set; }
    }

    public class SupplierDefectAnalysisRepo
    {
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int? TotalDefects { get; set; }
        public int? TotalReports { get; set; }
        public List<int?> DefectCriticalIds { get; set; }
        public IEnumerable<int> DefectMajorIds { get; set; }
        public IEnumerable<int> DefectMinorIds { get; set; }
        public List<int> DefectPhotoIds { get; set; }
    }

    public class ReportSupplierDetails
    {
        public int SupplierId { get; set; }
        public int? ReportId { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int? FactoryId { get; set; }
        public string ReportNo { get; set; }
        public string ReportLink { get; set; }
        public string FinalManualReportLink { get; set; }
    }

    public class DefectCount
    {
        public int? Count { get; set; }
        public string DefectName { get; set; }
    }

    public class DefectCountResponse
    {
        public DefectDashboardResult Result { get; set; }
        public List<DefectCount> PerformanceDefectList { get; set; }
    }
    public class DefectPhoto
    {
        public string DefectPhotoPath { get; set; }
        public string Description { get; set; }
    }

    public class DefectPhotoResponse
    {
        public DefectDashboardResult Result { get; set; }
        public List<DefectPhoto> PerformanceDefectList { get; set; }
    }

    public class DefectPhotoRepo
    {
        public string DefectPhotoPath { get; set; }
        public string DefectName { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public string Description { get; set; }
    }

    public class DefectReportList
    {
        public int FactoryCountryId { get; set; }
        public string FactoryCountryName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int InspectionId { get; set; }
        public int ReportId { get; set; }
        public int ReportDefectId { get; set; }
        public string DefectName { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int? Critical { get; set; }
    }
    public class ReportDefectResponse
    {
        public List<ReportDefectData> Data { get; set; }
        public ReportDefectResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }

    public enum ReportDefectResult
    {
        Success = 1,
        NotFound = 2
    }

    public class ReportDefectData
    {
        public int FactoryCountryId { get; set; }
        public string FactoryCountryName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int InspectionCount { get; set; }
        public int ReportCount { get; set; }
        public int TotalDefectCount { get; set; }
        public List<ReportDefectCountData> Defects { get; set; }
    }

    public class ReportDefectCountData
    {
        public string DefectName { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int? Critical { get; set; }
        public int? DefectCount
        {
            get
            {
                return Major + Minor + Critical;
            }
        }
    }

    public class ExportDefectGroupList
    {
        public int FactoryCountryId { get; set; }
        public string FactoryCountryName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int InspectionCount { get; set; }
        public int ReportCount { get; set; }
        public int TotalDefectCount { get; set; }
    }

    public class ExportDefectList
    {
        public int FactoryCountryId { get; set; }
        [Description("Factory Country")]
        public string FactoryCountryName { get; set; }
        public int SupplierId { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        public int BrandId { get; set; }
        [Description("Brand")]
        public string BrandName { get; set; }
        [Description("Defect")]
        public string DefectName { get; set; }
        [Description("Critical")]
        public int Critical { get; set; }
        [Description("Major")]
        public int Major { get; set; }
        [Description("Minor")]
        public int Minor { get; set; }
        [Description("Total Defect")]
        public int DefectCount
        {
            get
            {
                return Critical + Major + Minor;
            }
        }
    }
}
