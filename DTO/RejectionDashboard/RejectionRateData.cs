using DTO.CommonClass;
using DTO.InspectionCustomerDecision;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.RejectionDashboard
{
    public class RejectionRateData
    {
        public int? ReportId { get; set; }
        public double? InspectedQty { get; set; }
        public double? OrderQty { get; set; }
        public double? PresentedQty { get; set; }
        public int? ResultId { get; set; }
        public string ResultName { get; set; }
        public int? InspectionId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public int FactoryCountryId { get; set; }
        public string FactoryCountryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int CusDecisionId { get; set; }
        public int CustomerResultId { get; set; }
        public string CustomDecisionName { get; set; }
    }
    public class RejectionRateList
    {
        public List<RejectionRateReportResultList> RejectionRateReportResultLists { get; set; }
        public List<RejectionRateDecisionList> RejectionRateDecisionLists { get; set; }
    }

    public class RejectionRateReportResultList
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
        public double? PresentedQty { get; set; }
        public double? InspectedQty { get; set; }
        public double? OrderQty { get; set; }
        public int? ResultId { get; set; }
        public string ResultName { get; set; }
        public int TotalCount { get; set; }
    }

    public class RejectionRateDecisionList
    {
        public int FactoryCountryId { get; set; }
        public string FactoryCountryName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int TotalDecisionCount { get; set; }
        public int CustomerResultId { get; set; }
        public string CustomDecisionName { get; set; }

    }

    public class RejectionRateResponse
    {
        public RejectionRateItem Data { get; set; }
        public RejectionRateResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }

    public class RejectionRateItem
    {
        public RejectionRateList ResultDataList { get; set; }
        public List<RejectionRateGroupList> RejectionRateGroupList { get; set; }
        public List<CommonDataSource> ReportResultNameList { get; set; }
        public List<CustomerDecisionRepo> ReportDecisionNameList { get; set; }
    }

    public enum RejectionRateResult
    {
        Success = 1,
        NotFound = 2
    }

    public class RejectionRateGroupList
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
        [Description("Inspections")]
        public int InspectionCount { get; set; }
        [Description("Reports")]
        public int TotalCount { get; set; }
        [Description("Presented Qty")]
        public double? PresentedQty { get; set; }
        [Description("Inspected Qty")]
        public double? InspectedQty { get; set; }
        [Description("Order Qty")]
        public double? OrderQty { get; set; }
    }
    public class GroupByRequestFilter
    {
        public bool FactoryCountry { get; set; }
        public bool Supplier { get; set; }
        public bool Factory { get; set; }
        public bool Brand { get; set; }
    }
}
