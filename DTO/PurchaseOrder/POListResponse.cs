using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class PoListResponse
    {
       public IEnumerable<PoDataSource> PoDataSource { get; set; }
       public PoListResult Result { get; set; }
    }

    public class PoDataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum PoListResult
    {
        Success=1,
        NotFound=2
    }
    public class AutoPoNumber
    {
        public int customerId { get; set; }
        public string poname { get; set; }
    }

    public class POProductDataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int? PoId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> CustomerIds { get; set; }
        public int filterPoProduct { get; set; }
        public POProductDataSourceRequest()
        {
            this.SearchText = "";
            this.Skip = 0;
            this.Take = 10;
        }
    }

    public enum FilterPoProductEnum
    {
        ProductByPo=1,
        ProductByCustomer=2
    }

    public class PODataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? PoId { get; set; }
    }


    public class POProductDataRequest
    {
        public int? PoId { get; set; }
        public int? SupplierId { get; set; }
    }

    public class POBookingRepo
    {
        public int PoId { get; set; }
        public int BookingNumber { get; set; }
        public int StatusId { get; set; }
    }
    public class PoBookingDetailRepo
    {
        public string StatusName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int BookingNumber { get; set; }
        public int StatusId { get; set; }
        public DateTime ServiceFromDate { get; set; }
        public DateTime ServiceToDate { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string StatusColor { get; set; }
    }
    public class PoBookingDetailsResponse
    {
        public List<PoBookingDetailRepo> PoBookingDetails { get; set; }
        public PoListResult Result { get; set; }
    }
}
