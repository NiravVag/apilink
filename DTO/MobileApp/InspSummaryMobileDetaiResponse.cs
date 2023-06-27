using DTO.Inspection;
using DTO.InspectionCustomerDecision;
using DTO.Kpi;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class InspSummaryMobileDetaiResponse
    {
        public MobileResult meta { get; set; }
        public MobileInspectionReportDetail data { get; set; }
    }

    public class MobileInspectionReportDetail
    {
        public int? inspectionId { get; set; }
        public int reportNo { get; set; }
        public string reportTitle { get; set; }
        public int? supplierId { get; set; }
        public string supplierName { get; set; }
        public int? factoryId { get; set; }
        public string factoryName { get; set; }
        public string inspectionResult { get; set; }
        public int? inspectionResultId { get; set; }
        public string customerDecisionResult { get; set; }
        public int? customerDecisionResultId { get; set; }
        public string serviceType { get; set; }
        public string serviceDate { get; set; }
        public string bookingStatus { get; set; }
        public string poNumber { get; set; }
        public string destinationCountry { get; set; }
        public List<string> productImageUrl { get; set; }
        public int combinedProductsCount { get; set; }
        public bool isCombined { get; set; }
        public List<ProductData> combineProductList { get; set; }
        public List<FbResult> inspectionResultList { get; set; }
        public List<MobileDefectData> defectList { get; set; }
        public string reportLink { get; set; }
        public int? criticalCount { get; set; }
        public int? majorCount { get; set; }
        public int? minorCount { get; set; }
        public List<CusDecision> customerDecision { get; set; }
    }

    public class ProductData
    {
        public int key { get; set; }
        public string productRef { get; set; }
        public string productDesc { get; set; }
        public int criticalDefectsCount { get; set; }
        public int majorDefectsCount { get; set; }
        public int minorDefectsCount { get; set; }
        public double? orderQty { get; set; }
        public double? inspectedQty { get; set; }
        public double? presentedQty { get; set; }
        public string destinationCountry { get; set; }
        public string poNumber { get; set; }
        public List<MobileDefectData> defectList { get; set; }
    }

    public class FbResult
    {
        public int key { get; set; }
        public int? resultId { get; set; }
        public string fbResultName { get; set; }
        public string result { get; set; }
        public List<string> resultImage { get; set; }
        public int? imageCount { get; set; }
        public List<FbResult> subResultList { get; set; }
        public string remarks { get; set; }
    }

    public class MobileDefectData
    {
        public int key { get; set; }
        public string title { get; set; }
        public int count { get; set; }
        public int type { get; set; }
        public List<DefectImageData> defectImagedata { get; set; }
    }

    public class DefectImageData
    {
        public string defectImageUrl { get; set; }
        public string defectImageDesc { get; set; }
    }

    public class CusDecision
    {
        public int key { get; set; }
        public string title { get; set; }
        public int id { get; set; }
        public SvgColor svg { get; set; }
    }

    public class ReportData
    {
        public BookingData bookingData { get; set; }
        public IEnumerable<BookingProductsData> productDataList { get; set; }
        public IEnumerable<ReportCustomerDecision> cusDecision { get; set; }
        public IEnumerable<InspectionReportDefects> defects { get; set; }
        public IEnumerable<InspectionReportSummary> inspectionReportSummaries { get; set; }
        public List<FBReportInspSubSummary> fbReportInspSubSummaries { get; set; }
        public List<CustomerDecisionModel> customerDecisionModes { get; set; }
        public List<ReportDefectsImage> defectImageData { get; set; }
    }
}

