using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class ClientQuotationResponse
    {
        public QuotationExport QuotationDetails { get; set; }
        public ClinetQuotationResult Result { get; set; }
    }

    public enum ClinetQuotationResult
    {
        Success = 1,
        Failure = 2
    }

    public class QuotationExport
    {
        public int QuotationId { get; set; }
        public int BookingId { get; set; }
        public string QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string SupplierCode { get; set; }
        public string FactoryCode { get; set; }
        public string InspectionLocation { get; set; }
        public string ServiceFromDate { get; set; }
        public string ServiceToDate { get; set; }
        public string ProductDescription { get; set; }
        public string Sampling { get; set; }
        public int ProductQuantity { get; set; }
        public double QuotationUnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public double? TravelCost { get; set; }
        public double? Transportcost { get; set; }
        public double? HotelCost { get; set; }
        public double? OtherCost { get; set; }
        public double ManDay { get; set; }
        public double TotalCost { get; set; }
        public string CustomeServiceTypeName { get; set; }
        public string DepartmentName { get; set; }
        public int? TotalSamplingSize { get; set; }
        public List<QuotationBookingItem> PoInformation { get; set; }
    }

    public class ClientQuotationItem
    {
        public int QuotationId { get; set; }
        public ICollection<QuQuotationInsp> Booking { get; set; }
        public DateTime QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int FactoryId { get; set; }
        public string SupplierName { get; set; }
        public string FatoryName { get; set; }
        public ICollection<SuAddress> InspectionLocation { get; set; }
        public double QuotationPrice { get; set; }
        public double? TravelCostAir { get; set; }
        public double? TravelCostLand { get; set; }
        public double? TravelCost { get; set; }
        public double? Transportcost { get; set; }
        public double? HotelCost { get; set; }
        public double? OtherCost { get; set; }
        public double ManDay { get; set; }
        public string AQL { get; set; }
        public string DepartmentName { get; set; }
        public string ProductCategory { get; set; }
        public string QuotationAPIcomment { get; set; }
        public int BillPaidBy { get; set; }
        public DateTime ReConfirmDate { get; set; }
        public ICollection<CuServiceType> CuServiceType { get; set; }
        public int InspServiceTypeId { get; set; }

        public string CurrencyName { get; set; }
        public string BillPaidByName { get; set; }
        public double UnitPrice { get; set; }


    }


    public class ExpenseClientQuotationItem
    {
        public int QuotationId { get; set; }
        public double QuotationPrice { get; set; }
        public double? TravelCostAir { get; set; }
        public double? TravelCostLand { get; set; }
        public double? TravelCost { get; set; }
        public double? Transportcost { get; set; }
        public double? HotelCost { get; set; }
        public double? OtherCost { get; set; }
        public double ManDay { get; set; }
        public int BillPaidBy { get; set; }
        public string CurrencyName { get; set; }
        public string BillPaidByName { get; set; }
        public ICollection<QuQuotationInsp> Booking { get; set; }

    }
    public class QuotationBookingItem
    {
        public int BookingId { get; set; }
        public DateTime? ServiceFromDate { get; set; }
        public DateTime? ServiceToDate { get; set; }
        public string PONumber { get; set; }
        public int POQuantity { get; set; }
        public int SamplingSize { get; set; }
        public double? CostPerOrder { get; set; }
        public double? Manday { get; set; }
        public string InspectionLocation { get; set; }
    }

    public class ClientQuotationBookingItem
    {
        public int BookingId { get; set; }
        public int PoId { get; set; }
        public int ProductId { get; set; }
        public int? CombineProductId { get; set; }
        public int? CombineAqlQty { get; set; }
        public int? AqlQty { get; set; }
        public DateTime? ServiceFromDate { get; set; }
        public DateTime? ServiceToDate { get; set; }
        public string PONumber { get; set; }
        public int POQuantity { get; set; }
        public int SamplingSize { get; set; }
        public string AQL { get; set; }
        public double? CostPerOrder { get; set; }
        public string ProductCategory { get; set; }
        public int? Aql { get; set; }
        public int Critical { get; set; }
        public int Minor { get; set; }
        public int Major { get; set; }
    }

    public class SupplierCode
    {
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public string Code { get; set; }
    }
}
