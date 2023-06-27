using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Report
{
    public class CustomerReportDetailsRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ProductRef { get; set; }
        public string Po { get; set; }
        public int InspectionNo { get; set; }
        public string ReportNo { get; set; }
    }

    public class CustomerReportDetails
    {
        public string ReportNo { get; set; }
        public string PO { get; set; }
        public string ProductRef { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public string ProductType { get; set; }
        public string InspectionType { get; set; }
        public int InspectionCount { get; set; }
        public string InspectionFromDate { get; set; }
        public string InspectionToDate { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string FactoryName { get; set; }
        public string FactoryCode { get; set; }
        public string Brand { get; set; }
        public double PresentedQty { get; set; }
        public double InspectedQty { get; set; }
        public string Unit { get; set; }
        public string FactoryCountry { get; set; }
        public string FactoryCountryCode { get; set; }
        public string Season { get; set; }
        public int? Year { get; set; }
        public string ReportResult { get; set; }
        public string CustomerDecision { get; set; }
        public string CustomerDecisionComment { get; set; }
        public string CustomerDecisionDate { get; set; }
        public List<CustomerReportCheckpoints> Checkpoints { get; set; }
        public List<CustomerReportDefects> Defects { get; set; }
    }

    public class CustomerReportCheckpoints
    {
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class CustomerReportDefects
    {
        public string DefectCode { get; set; }
        public string DefectFamily { get; set; }
        public string DefectName { get; set; }
        public int? MarjorCount { get; set; }
        public int? MinorCount { get; set; }
        public string Position { get; set; }
    }

    public class CustomerReportDetailsResponse
    {
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public List<CustomerReportDetails> customerReportDetails { get; set; }
        public List<string> errors { get; set; }

        public CustomerReportDetailsResponse()
        {
            this.errors = new List<string>();
        }
    }
    public class CustomerReportDetailsRepo
    {
        public int ReportId { get; set; }
        public int BookingId { get; set; }
        public string ReportNo { get; set; }
        public DateTime? ServiceFromDate { get; set; }
        public DateTime? ServiceToDate { get; set; }
        public int FactoryId { get; set; }
        public string ReportResult { get; set; }
    }

    public class CustomerReportInspectionRepo
    {
        public int InspectionId { get; set; }
        public string FactoryName { get; set; }
        public string FactoryCode { get; set; }
        public int FactoryId { get; set; }
        public string InspectionType { get; set; }
        public int? ReInspectionType { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string Season { get; set; }
        public int? Year { get; set; }

    }

    public class CustomerReportFbProductDetails
    {
        public int FbReportId { get; set; }
        public int InspectionId { get; set; }
        public string ReportNo { get; set; }
        public DateTime? ServiceFromDate { get; set; }
        public DateTime? ServiceToDate { get; set; }
        public string ReportResult { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? ProductSubCategory2Id { get; set; }
        public string Unit { get; set; }
    }
}
