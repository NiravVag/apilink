using DTO.CustomerPriceCard;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Invoice
{
    public class InvoiceSamplingUnitPrice
    {
        public double UnitPrice { get; set; }
        public int TotalBookingQuantity { get; set; }
        public int? TotalSamplingSize { get; set; }
        public List<CustomerPriceBookingProducts> bookingProductList { get; set; }
    }
}
