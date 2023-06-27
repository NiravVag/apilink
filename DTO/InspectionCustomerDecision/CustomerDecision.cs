using DTO.Common;
using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DTO.InspectionCustomerDecision
{
    public class CustomerDecisionRepo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CustomName { get; set; }

        public int? CustomerId { get; set; }

        public bool? IsDefault { get; set; }

        public int CusDecId { get; set; }
    }

    public class CustomerDecisionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CusDecId { get; set; }
    }

    public class CustomerDecisionResponseData
    {
        public int CustomerResultId { get; set; }
        public string Comments { get; set; }
    }

    public class CustomerDecisionSaveRequest
    {
        public int ReportId { get; set; }
        public int CustomerResultId { get; set; }
        public string Comments { get; set; }
        public bool sendEmailToFactoryContacts { get; set; }
        public int BookingId { get; set; }
        public bool? IsAutoCustomerDecision { get; set; }
    }
    public class CustomerDecisionListSaveRequest
    {
        public List<int> ReportIdList { get; set; }
        public int CustomerResultId { get; set; }
        public string Comments { get; set; }
        public bool sendEmailToFactoryContacts { get; set; }
        public int BookingId { get; set; }
        public bool? IsAutoCustomerDecision { get; set; }
    }

    public class CustomerDecisionSaveResponse
    {
        public CustomerDecisionSaveResponseResult Result { get; set; }
    }

    public class CustomerDecisionListResponse
    {
        public List<CustomerDecisionModel> CustomerDecisionList { get; set; }
        public CustomerDecisionListResponseResult Result { get; set; }
    }

    public class CustomerDecisionResponse
    {
        public CustomerDecisionResponseData CustomerDecision { get; set; }
        public CustomerDecisionResponseResult Result { get; set; }
    }

    public enum CustomerDecisionSaveResponseResult
    {
        success = 1,
        fail = 2,
        noemailconfiguration = 3,
        noEmailSubjectConfiguration = 4,
        noEmailBodyConfiguration = 5,
        noEmailRecipientsConfiguration = 6,
        multipleRuleFound = 7,
        noroleconfiguration = 8
    }

    public enum CustomerDecisionListResponseResult
    {
        success = 1,
        fail = 2,
        notfound = 3
    }

    public enum CustomerDecisionResponseResult
    {
        success = 1,
        fail = 2,
        notfound = 3
    }

    public class CustomerDecisionSummaryRequest
    {
        public int SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> CustomerList { get; set; }
        public IEnumerable<int> FactoryIdlst { get; set; }
        public int DateTypeid { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public IEnumerable<int?> Officeidlst { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public IEnumerable<int> ServiceTypelst { get; set; }
        public int AdvancedSearchtypeid { get; set; }
        public string AdvancedSearchtypetext { get; set; }
        public IEnumerable<int> SelectedCountryIdList { get; set; }
        public IEnumerable<int> SelectedBrandIdList { get; set; }
        public IEnumerable<int> SelectedDeptIdList { get; set; }
        public IEnumerable<int?> SelectedCollectionIdList { get; set; }
        public IEnumerable<int> SelectedBuyerIdList { get; set; }
        public List<int> StatusIdlst { get; set; }
        public List<int> FbReportResultList { get; set; }
        public int? CusDecisionGiven { get; set; }
        public List<int> BookingIds { get; set; }
    }

    public class CustomerDecisionSummaryResult
    {
        public int BookingId { get; set; }
        public string BookingNoCustomerNo { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? FactoryId { get; set; }
        public string FactoryName { get; set; }
        public string ServiceTypeName { get; set; }
        public string ServiceDate { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public bool HasCustomerDecisionRole { get; set; }
        public int TotalReportCount { get; set; }
        public bool HasCustomerDecisionConfig { get; set; }
        public int DecisionStatusCount { get; set; }
        public int ServiceTypeId { get; set; }
        public int StatusId { get; set; }
        public string CustomerBookingNo { get; set; }
        public List<CustomerDecisionProductList> ProductResultList { get; set; }
    }


    public class CustomerDecisionReponse
    {
        public List<CustomerDecisionSummaryResult> Data { get; set; }
        public CustomerDecisionResponseResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<InspectionStatus> InspectionStatuslst { get; set; }
    }

    public class CustomerDecisionCount
    {
        public int BookingId { get; set; }
        public int Count { get; set; }
        public int CustomerDecisionResultId { get; set; }
        public string CustomerDecisionResultName { get; set; }
        public int ReportId { get; set; }
        public string ProductId { get; set; }
        public int? ResultId { get; set; }
        public string ResultName { get; set; }
    }

    public class EditCustomerDecisionResponse
    {
        public BookingData BookingData { get; set; }
        public List<CustomerDecisionProductList> ProductList { get; set; }
        public CustomerDecisionResponseResult Result { get; set; }
    }

    public class CustomerDecisionProductList
    {
        public int BookingId { get; set; }
        public int? ReportId { get; set; }
        public int? ResportResultId { get; set; }
        public int CustomerDecisionResultId { get; set; }
        public string ProductId { get; set; }
        public List<string> ProductIdList { get; set; }
        public string ReportResultName { get; set; }
        public string CustomerDecisionName { get; set; }
        public string ResportResultColor { get; set; }
        public string CustomerDecisionResultColor { get; set; }
        public string CustomerDecisionComment { get; set; }
        public int CustomerDecisionResultCusDecId { get; set; }
        public string ProdDesc { get; set; }
        public int ProductRefId { get; set; }
        public int ContainerId { get; set; }
        public string ReportTitle { get; set; }
        public string ProductPhoto { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class CusDecisionProblematicRemarks
    {
        public int ReportId { get; set; }
        public string ProductId { get; set; }
        public string SubCat { get; set; }
        public string SubCat2 { get; set; }
        public string Remarks { get; set; }
        public string Result { get; set; }
    }

    public class CusDecisionProblematicRemarksResponse
    {
        public List<CusDecisionProblematicRemarks> Data { get; set; }
        public CustomerDecisionResponseResult Result { get; set; }
    }

    public class CustomerDecisionSummaryExport
    {
        [Description("Booking#")]
        public int BookingId { get; set; }
        [Description("Customer#")]
        public string CustomerBookingNo { get; set; }
        [Description("Customer")]
        public string Customer { get; set; }
        [Description("Supplier")]
        public string Supplier { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Service From Date")]
        public DateTime? ServiceFromDate { get; set; }
        [Description("Service To Date")]
        public DateTime? ServiceToDate { get; set; }
        [Description("Product Ref")]
        public string ProductName { get; set; }
        [Description("Product Desc")]
        public string ProductDesc { get; set; }
        [Description("po#")]
        public string PoNumber { get; set; }
        [Description("Report Result")]
        public string ReportResult { get; set; }
        [Description("Customer Decision")]
        public string CustomerDecision { get; set; }
        [Description("Customer Comment")]
        public string CustomerComment { get; set; }
        [Description("Department")]
        public string Department { get; set; }
        [Description("Collection")]
        public string Collection { get; set; }
        [Description("Buyer")]
        public string Buyer { get; set; }
        [Description("Brand")]
        public string Brand { get; set; }
        public int ReportId { get; set; }
    }
}
