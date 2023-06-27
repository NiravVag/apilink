using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class InspQuotationMobileResponse
    {
        public MobileResult meta { get; set; }
        public List<MobileInspQuotationData> data { get; set; }
    }

    public class MobileInspQuotationData
    {
        public int key { get; set; }
        public int quotationId { get; set; }
        public string supplier { get; set; }
        public string factory { get; set; }
        public string serviceDate { get; set; }
        public string bookingId { get; set; } //multiple
        public string productRef { get; set; }
        public string productDesc { get; set; }
        public int? bookingQty { get; set; }
        public int? samplingQty { get; set; }
        public double manDay { get; set; }
        public string poNumber { get; set; }
        public double unitPrice { get; set; }
        public double serviceFee { get; set; }
        public double travelAir { get; set; }
        public double travelLand { get; set; }
        public double hotel { get; set; }
        public double totalPrice { get; set; }
        public double otherCost { get; set; }
        public bool isApproved { get; set; }
        public string approvedDate { get; set; }
        public string currency { get; set; }
        public int combinedProductCount { get; set; }
        public int statusId { get; set; }
        public double discount { get; set; }
    }

    public class ProductList
    {
        public int productRef { get; set; }
        public int bookingQty { get; set; }
        public int sampleQty { get; set; }
        public int CombineId { get; set; }
        public bool isParentProduct { get; set; }
    }

    public class MobilePendingQuotation
    {
        public int QuotationId { get; set; }
        public int CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int QuotationStatusId { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int BookingId { get; set; }
        public double ManDay { get; set; }
        public double UnitPrice { get; set; }
        public double ServiceFee { get; set; }
        public double TravelAir { get; set; }
        public double TravelLand { get; set; }
        public double Hotel { get; set; }
        public double TotalPrice { get; set; }
        public double OtherCost { get; set; }
        public string Currency { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public int BookingStatusId { get; set; }
        public int BilledTo { get; set; }
        public double? Discount { get; set; }
    }
}

