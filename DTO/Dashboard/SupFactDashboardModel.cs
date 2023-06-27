using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dashboard
{
    public class SupFactDashboardModel
    {
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int? CustomerId { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public List<int?> StatusIdList { get; set; }
    }

    public class CustomerBookingModel
    {
        public string CustomerName { get; set; }
        public int BookingId { get; set; }
        public int BookingCount { get; set; }
        public string StatusColor { get; set; }
    }

    public class BookingDetailsRepo
    {
        public int BookingId { get; set; }
        public string CustomerName{ get; set; }
        public string FactoryName { get; set; }
        public string SupplierName { get; set; }
        public IEnumerable<string> ServiceType { get; set; }
        public DateTime ServiceFromDate { get; set; }
        public DateTime ServiceToDate { get; set; }
        public string CountryName { get; set; }
        public int? CreatedBy { get; set; }
    }

    public class BookingDataResponse
    {
        public DashboardResult Result { get; set; }
        public List<BookingDetails> BookingDetails { get; set; }
    }

    public class CusBookingDataResponse
    {
        public DashboardResult Result { get; set; }
        public List<CustomerBookingModel> CusBookingDetails { get; set; }
    }

    public class BookingDetails
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string FactoryName { get; set; }
        public string SupplierName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceFromDate { get; set; }
        public string ServiceToDate { get; set; }
        public string CountryName { get; set; }
        public bool IsEdit { get; set; }
    }


    public class FactoryGeoCode
    {
        public int TotalCount { get; set; }
        public string FactoryName { get; set; }
        public int FactoryId { get; set; }
        public decimal? FactoryLongitude { get; set; }
        public decimal? FactoryLatitude { get; set; }
    }
    public enum DashboardResult
    {
        Success = 1,
        CannotGetList = 2,
        Failed = 3,
        RequestNotCorrectFormat = 4,
    }

    public class FactMapGeoLocation
    {
        public List<InspFactoryGeoCode> FactoryGeoCode { get; set; }
        public DashboardResult Result { get; set; }
    }
}
