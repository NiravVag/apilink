using DTO.Inspection;
using DTO.Report;
using Entities;
using System;
using System.Collections.Generic;

namespace DTO.Schedule
{
    public class ScheduleAllocation
    {
        public int BookingNo { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int BookingStatus { get; set; }
        public IEnumerable<AllocationStaff> AllocationCSQCStaff { get; set; }
        public double QuotationManDay { get; set; }
        public string Comment { get; set; }
        public string BookingComments { get; set; }
        public string ServiceType { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public int TownId { get; set; }
        public string TownName { get; set; }
        public string FactoryAddress { get; set; }
        public string RegionalAddress { get; set; }
        public int TotalProducts { get; set; }
        public int TotalReports { get; set; }
        public int TotalSamplingSize { get; set; }
        public int TotalCombineCount { get; set; }
        public int TotalContainers { get; set; }
        public double? TravelManday { get; set; }
        public ScheduleAllocationResponseResult Result { get; set; }
        public List<int> PreviousBookingNoList { get; set; }
        public double ActualManday { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
        public bool IsEntityLevelAutoQcExpenseEnabled { get; set; }
        public bool IsServiceTypeLevelAutoQcExpenseEnabled { get; set; }
        public bool IsBookingInvoiced { get; set; }
        public double? SuggestedManday { get; set; }
    }
    public enum ScheduleAllocationResponseResult
    {
        Success = 1,
        CannotGet = 2,
        QCListNotAvailable = 3
    }

    public class AllocationBookingItem
    {
        public int BookingNo { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string RegionalSupplierName { get; set; }
        public string RegionalFactoryName { get; set; }
        public int BookingStatus { get; set; }
        public string Comment { get; set; }
        public string BookingComments { get; set; }
        public IEnumerable<ScheduleProductsData> ProductList { get; set; }
        public IEnumerable<ScheduleContainersRepo> ContainerList { get; set; }
        public List<ServiceTypeList> ServiceTypeList { get; set; }
        public int? PreviousBookingNo { get; set; }
        public bool IsAutoQCExpenseClaim { get; set; }
    }

    public class PoTransactionDetails
    {
        public int PoTranId { get; set; }
        public double ProductId { get; set; }
        public string PoNo { get; set; }
        public int BookingId { get; set; }
        public int? CombineProductId { get; set; }
        public int? CombineAqlQty { get; set; }
        public int? AqlQty { get; set; }
        public int? ReportId { get; set; }
    }

    public class QCBookings
    {
        public int QCId { get; set; }
        public int BookingId { get; set; }
        public int BookingStatus { get; set; }
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public string FactoryRegionalName { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
