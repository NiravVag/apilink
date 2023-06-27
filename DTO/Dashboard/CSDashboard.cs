using DTO.Common;
using DTO.CommonClass;
using DTO.Kpi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.Dashboard
{
    public class CSDashboard
    {

    }

    public class CSDashboardModelRequest
    {
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> FactoryIdList { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        //[DateGreaterThan(otherPropertyName = "ServiceDateTo", ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_GREATER_REQ")]
        public DateObject ServiceDateFrom { get; set; }

        //[Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_TODATE_REQ")]
        [DateGreaterThanAttribute(otherPropertyName = "ServiceDateFrom", ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_TODATE_LESS_REQ")]
        public DateObject ServiceDateTo { get; set; }

        public IEnumerable<int?> DeptIdList { get; set; }
        public IEnumerable<int?> BrandIdList { get; set; }
        public IEnumerable<int?> BuyerIdList { get; set; }
        public IEnumerable<int> StatusIdList { get; set; }
        public List<int> CountryIdList { get; set; }
        public List<int> OfficeIdList { get; set; }
        public List<int?> ServiceTypeList { get; set; }
    }

    public class CSDashboardFilterModel
    {
        //public List<int> BookingIdList { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? EntityId { get; set; }
    }

    public class CSDashboardDBRequest
    {
        [MapData(Type = "IntList")]
        public List<CommonId> BookingIdList { get; set; }
        public int? EntityId { get; set; }
    }

    public class CSDashboardStatusDBRequest
    {
        [MapData(Type = "IntList")]
        public List<CommonId> BookingIdList { get; set; }
        public int? EntityId { get; set; }
        public int UserId { get; set; }
        [MapData(Type = "IntList")]
        public List<CommonId> RoleIdList { get; set; }
    }

    //public class GetNewBookingRelatedCount
    //{
    //    public int NewCount { get; set; }
    //    public string Name { get; set; }
    //}

    public enum CSDashboardResult
    {
        Success = 1,
        NoFound = 2,
        Failed = 3,
        RequestNotCorrectFormat = 4
    }

    public class GetNewBookingRelatedCountResponse
    {
        public CSDashboardResult Result { get; set; }
        public CSDashboardCountItem CSDashboardCountItem { get; set; }
        //public List<GetNewBookingRelatedCount> NewCountList {get;set;}

    }

    public class CSDashboardCountItem
    {
        public int CustomerCount { get; set; }
        public int SupplierCount { get; set; }
        public int FactoryCount { get; set; }
        public int BookingCount { get; set; }
        public int POCount { get; set; }
        public int ProductCount { get; set; }
    }

    public class CSDashboardItem
    {
        public string Name { get; set; }
        public double Count { get; set; }
        //public string Color { get; set; }
    }

    public class CSDashboardserviceTypeResponse
    {
        public List<CSDashboardItem> Data { get; set; }
        public CSDashboardResult Result { get; set; }
    }

    public class CSDashboardMandayByOfficeResponse
    {
        public List<CSDashboardItem> Data { get; set; }
        public CSDashboardResult Result { get; set; }
    }

    public class ServiceTypeData
    {
        public int ServiceTypeId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class MandayOfficeData
    {
        public string OfficeName { get; set; }
        public int MandayCount { get; set; }
    }

    public class DayReportCountRepo
    {
        public int FbReportCount { get; set; }
        public DateTime ServiceToDate { get; set; }
    }

    public class DayFBReportCount
    {
        public int Count { get; set; }
        public string Date { get; set; }
    }

    public class DayFBReportCountResponse
    {
        public List<DayFBReportCount> Data { get; set; }
        public CSDashboardResult Result { get; set; }
    }

    public class StatusListCountResponse
    {
        public CSDashboardResult Result { get; set; }
        public List<StatusTaskCountItem> BookingStatusCount { get; set; }
        public List<StatusTaskCountItem> QuotationStatusCount { get; set; }
        public List<StatusTaskCountItem> AllocationStatusCount { get; set; }
        public List<StatusTaskCountItem> ReportStatusCount { get; set; }

       public StatusListCountResponse()
        {
            QuotationStatusCount = new List<StatusTaskCountItem>();
        }
    }

    public class StatusTaskCountItem
    {
        public int StatusCount { get; set; }
        public int? TaskCount { get; set; }
        public string StatusName { get; set; }
        public string TaskName { get; set; }
        public string TaskLink { get; set; }
        public int Id { get; set; }
    }


    public class StatusTaskCountItemRepo
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public int ActionType { get; set; }
    }

    public class StatusTaskCountListRepo
    {
        public List<StatusTaskCountItemRepo> StatusCountList { get; set; }
        //public List<StatusTaskCountItemRepo> QuotationStatusCount { get; set; }
        //public List<StatusTaskCountItemRepo> AllocationStatusCount { get; set; }
        //public List<StatusTaskCountItemRepo> ReportStatusCount { get; set; }
    }

    public enum CSDashboardStatusCount
    {
        BookingStatusList = 1,
        BookingTaskList = 2,
        QuotationStatusList = 3,
        QuotationTaskList = 4,
        AllocationStatusList = 5,
        AllocationTaskList = 6,
        ReportStatusList = 7,
        ReportTaskList = 8
    }
}
