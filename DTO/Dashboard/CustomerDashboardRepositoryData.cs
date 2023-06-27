using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dashboard
{

    public class CustomerResultRepo
    {
        public int Id { get; set; }
        public int TotalCount { get; set; }
    }

    public class CustomerResultMasterRepo
    {
        public int Id { get; set; }
        public int CustomerDecisionId { get; set; }
        public string CustomDecisionName { get; set; }
        public string CustomerDecisionName { get; set; }
    }

    public class CustomerETDDataRepo
    {
        public int InspectionId { get; set; }
        public DateTime? EtdDate { get; set; }
        public DateTime ServiceToDate { get; set; }
    }

    public class CustomerAPIRADashboardRepo
    {
        public int? ResultId { get; set; }
        public int TotalCount { get; set; }
    }

   

    public class InspectionManDaysRepo
    {
        public DateTime ServiceDate { get; set; }
        public double ManDays { get; set; }
    }

    public class InspectionMonthlyManDaysRepo
    {
        public int Month { get; set; }
        public double ManDays { get; set; }
    }

    public class InspectionWeeklyManDaysRepo
    {
        public int WeekNumber { get; set; }
        public double ManDays { get; set; }
    }

    public class SupplierBookingRevisionRepo
    {
        public int InspectionId { get; set; }
        public int BookingCount { get; set; }
    }

    public class POProductsRepo
    {
        public DateTime? ServiceDateTo { get; set; }
        public int ProductFbReportCount { get; set; }
        public int ContainerFbReportCount { get; set; }
    }

    public class ProductCategoryDashboardRepo
    {
        public int? Id { get; set; }
        public int TotalCount { get; set; }
    }

    public class InspCountryGeoCode
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string FactoryCountryName { get; set; }
        public string FactoryCountryCode { get; set; }
        public int FactoryCountryId { get; set; }
        public int TotalCount { get; set; }
        public string FactoryProvinceName { get; set; }
        public int FactoryProvinceId { get; set; }
        public decimal? ProvinceLongitude { get; set; }
        public decimal? ProvinceLatitude { get; set; }
        public string FactoryName { get; set; }
        public int FactoryId { get; set; }
        public decimal? FactoryLongitude { get; set; }
        public decimal? FactoryLatitude { get; set; }
    }
    public class InspProvinceGeoCode
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string FactoryProvinceName { get; set; }
        public int FactoryProvinceId { get; set; }
        public int TotalCount { get; set; }
    }

    public class InspFactoryGeoCode
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string FactoryName { get; set; }
        public int FactoryId { get; set; }
        public int TotalCount { get; set; }
    }

    public class MapGeoLocation
    {

        public List<InspCountryGeoCode> CountryGeoCode { get; set; }
        public MapGeoLocationResult CountryGeoCodeResult { get; set; }
        public List<InspProvinceGeoCode> ProvinceGeoCode { get; set; }
        public MapGeoLocationResult ProvinceGeoCodeResult { get; set; }
        public List<InspFactoryGeoCode> FactoryGeoCode { get; set; }
    }
    public enum MapGeoLocationResult
    {
        Success = 1,
        Failure = 2
    }
    //public class BookingDetailRepo
    //{
    //    public int InspectionId { get; set; }
    //    public int CustomerId { get; set; }
    //    public int SupplierId { get; set; }
    //    public int FactoryId { get; set; }
    //    public DateTime CreationDate { get; set; }
    //    public DateTime ServiceDateFrom { get; set; }
    //    public DateTime ServiceDateTo { get; set; }
    //}
}
