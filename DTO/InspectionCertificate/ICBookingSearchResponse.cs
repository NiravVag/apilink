using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{
    public class ICBookingSearchResponse
    {
        public int BookingNumber { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ServiceFromDate { get; set; }
        public string ServiceToDate { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public bool EnableCheckbox { get; set; }
        public string BookingStatus { get; set; }
        public string ServiceType { get; set; }
        public bool IsExpand { get; set; }
        public int? BusinessLine { get; set; }
        public List<ICBookingSearchProductResponse> ProductList { get; set; }
    }
    public class ICBookingSearchRepoResponse
    {
        public int BookingNumber { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public DateTime ServiceFromDate { get; set; }
        public DateTime ServiceToDate { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public CuCustomer Customer { get; set; }
        public string BookingStatus { get; set; }
        public int? BusinessLine { get; set; }
    }
    public class CustomerDecisionBookId
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
    }
    public class QuantityPoId
    {
        public int InspPoTransactionId { get; set; }
        public double? Quantity { get; set; }
        public double TotalICQty { get; set; }
        public double PresentedQty { get; set; }
    }
    public class ICBookingResponse
    {
        public List<ICBookingSearchResponse> BookingList { get; set; }
        public ICBookingSearchResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
    public class ProductBookingICResponse
    {
        public int Id { get; set; }
        public int? BookingProductId { get; set; }
        public int ShipmentQty { get; set; }
        public int BookingNumber { get; set; }
        public string PONo { get; set; }
        public int POId { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string DestinationCountry { get; set; }
        public string Unit { get; set; }
        public double RemainingQty { get; set; }
        public double PresentedQty { get; set; }
        public int TotalICQty { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public int? PoColorId { get; set; }
        public int? BusinessLine { get; set; }
    }
    public class ICBookingProductResponse
    {
        public List<ProductBookingICResponse> ProductBookingList { get; set; }
        public ICBookingSearchResult Result { get; set; }
    }
    public enum ICBookingSearchResult
    {
        Success = 1,
        Failure = 2,
        RequestNotCorrectFormat = 3,
        NoDataFound = 4,
        BookingNosRequired = 5
    }
    public class ICBookingProductFB
    {
        public int FBReportId { get; set; }
        public double FBPresentedQty { get; set; }
        public int? FBStatus { get; set; }
        public int inspoTransid { get; set; }
        public int? InsPOColorTransId { get; set; }
    }
    public class ICBookingProductRequest
    {
        public IEnumerable<int> BookingIdList { get; set; }
        public IEnumerable<int> ProductIdList { get; set; }
    }
    public class FBReportCustomerDecision
    {
        public int ReportId { get; set; }
        public int CustomerDecisionId { get; set; }
    }

    public class ICBookingModel
    {
        public int BookingId { get; set; }
    }
}
