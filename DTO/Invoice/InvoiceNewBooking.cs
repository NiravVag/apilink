using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Invoice
{
    public class InvoiceNewBookingResponse
    {
        public List<InvoiceNewBookingDetail> BookingList { get; set; }
        public InvoiceNewBookingResult Result { get; set; }
    }

    public enum InvoiceNewBookingResult
    {
        success = 1,
        failure = 2,
        nodata = 3
    }

    public class InvoiceNewBookingDetailRepo
    {
        public int BookingId { get; set; }
        public int BookingQuantity { get; set; }
        public int StatusId { get; set; }
        public int? InvoiceStatus { get; set; }
        public DateTime ServiceFromDate { get; set; }
        public DateTime ServiceToDate { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int PriceCategoryId { get; set; }

        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string PriceCategoryName { get; set; }
        public string ServiceTypeName { get; set; }
    }

    public class InvoiceNewBookingDetail
    {
        public int BookingId { get; set; }
        public int BookingQuantity { get; set; }

        public string ServiceDate { get; set; }

        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int PriceCategoryId { get; set; }


        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string PriceCategoryName { get; set; }
        public string ServiceTypeName { get; set; }
    }

    public class NewBookingInvoiceSearch
    {
        public int? BookingNumber { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int? ServiceId { get; set; }
        public int BilledTo { get; set; }
        public int InvoiceType { get; set; }
        public DateObject BookingStartDate { get; set; }
        public DateObject BookingEndDate { get; set; }
    }
}
