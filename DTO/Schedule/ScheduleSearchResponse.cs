using DTO.CommonClass;
using DTO.Inspection;
using DTO.Quotation;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DTO.Schedule
{
    public class ScheduleSearchResponse
    {
        public IEnumerable<ScheduleBookingItem> Data { get; set; }

        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<InspectionStatus> InspectionStatuslst { get; set; }
        public ScheduleSearchResponseResult Result { get; set; }
        public InternalUserRoleAccessSchedule InternalUserRole { get; set; }
        public double MandayCount { get; set; }
    }
    public class ScheduleBookingItem
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ServiceType { get; set; }
        public DateTime? ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string ServiceDate { get; set; }
        public double? ManDay { get; set; }
        public double? CalculatedWorkingHours { get; set; }
        public IEnumerable<ServiceDateQCNames> ServiceDateQCNames { get; set; }
        public IEnumerable<ServiceDateCSNames> ServiceDateCSNames { get; set; }
        public IEnumerable<ServiceDateQCNames> ServiceDateAdditionalQCNames { get; set; }
        public IEnumerable<ScheduleQuotationManDay> quotationMandayListByDate { get; set; }
        public string StatusName { get; set; }
        public int? StatusId { get; set; }
        public string Office { get; set; }
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public string FactoryAddress { get; set; }
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public string ZoneName { get; set; }
        public string TownName { get; set; }
        public double ActualManDay { get; set; }
        public int Productcount { get; set; }
        public int ReportCount { get; set; }
        public bool IsMandayButtonVisible { get; set; }
        public string FirstServiceDate { get; set; }
        public int SampleSize { get; set; }
        public double ReportSampleSize { get; set; }
        public string QuotationStatus { get; set; }
        public bool ShowAddButton { get; set; }
        public double PlannedManday { get; set; }
        public int AssignedManday { get; set; }
        public bool HasQcVisible { get; set; }
        public bool QcVisibleToEmail { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public IEnumerable<string> ProductSubCategory2 { get; set; }
        public string ProductId { get; set; }
        public bool IsMSChartProduct { get; set; }
        public List<InspectionPOColorTransaction> POColorTransactions { get; set; }
        public List<ScheduleProductsData> InspectionProducts { get; set; }
        public string Season { get; set; }
        public string Brands { get; set; }
        public List<InspectionPOExportData> InspectionPOs { get; set; }
        //public string PONumber { get; set; }
        public string FactoryCountry { get; set; }
        public string ReportNumber { get; set; }
        public List<ReportDetails> ReportTitleList { get; set; }
        public string CSNames { get; set; }
        public string FactoryContact { get; set; }
        public DateTime? CreateDate { get; set; }
        public double SuggestedManday { get; set; }
        public bool? IsEAQF { get; set; }
        public int CuProductId { get; set; }
    }

    public class ScheduleBookingInfo
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string ProductCategory { get; set; }
        public string StatusName { get; set; }
        public int StatusPriority { get; set; }
        public string Office { get; set; }
        public int OfficeId { get; set; }
        public CuCustomer Customer { get; set; }
        public int StatusId { get; set; }
        public double ActualManDay { get; set; }
        public int CombineProductcount { get; set; }
        public int NonCombineProductcount { get; set; }
        public DateTime? FirstServiceDateFrom { get; set; }
        public DateTime? FirstServiceDateTo { get; set; }
        public string FactoryRegionalName { get; set; }
        public string CustomerBookingNo { get; set; }
        public string ProductSubCategory { get; set; }
        public string Season { get; set; }
        public int? Year { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsEAQF { get; set; }
    }

    public class StaffLeavesDate
    {
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
    }

    public enum ScheduleSearchResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
    //are we using
    public class InternalUserRoleAccessSchedule
    {
        public bool IsBookingRequestRole { get; set; }
        public bool IsBookingConfirmRole { get; set; }
        public bool IsBookingVerifyRole { get; set; }
    }

    public class ScheduleBookingItemExportSummarynew
    {
        [Description("Booking No")]
        public int? BookingId { get; set; }
        [Description("Province")]
        public string ProvinceName { get; set; }
        [Description("City")]
        public string CityName { get; set; }
        [Description("County")]
        public string CountyName { get; set; }
        [Description("Zone")]
        public string Zone { get; set; }
        [Description("Town")]
        public string TownName { get; set; }
        [Description("Quotation Status")]
        public string QuotationStatus { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Service From Date")]
        public DateTime? ServiceFromDate { get; set; }
        [Description("Service To Date")]
        public DateTime? ServiceToDate { get; set; }
        [Description("Schedule Date")]
        public DateTime? ScheduleDate { get; set; }
        [Description("Allocated QC")]
        public double ScheduleManday { get; set; }
        [Description("Actual Man Day")]
        public double ActualManday { get; set; }
        [Description("Estimated Man Day")]
        public double QuotationManday { get; set; }
        [Description("Planned Man Day")]
        public double PlannedManday { get; set; }
        [Description("Suggested Man Day")]
        public double SuggestedManday { get; set; }
        [Description("Calculated Total Working Hours")]
        public double CalculatedTotalWorkingHours { get; set; }
        [Description("QC")]
        public string QCName { get; set; }
        [Description("Addtional QC")]
        public string AdditionalQCName { get; set; }
        [Description("Report Checker")]
        public string CSName { get; set; }
        [Description("Customer")]
        public string CustomerName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Factory Address")]
        public string FactoryAddress { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Item")]
        public int ProductCount { get; set; }
        [Description("Report")]
        public int ReportCount { get; set; }
        [Description("Booking Sample Size")]
        public int SampleSize { get; set; }
        [Description("Report Sample Size")]
        public double ReportSampleSize { get; set; }
        [Description("First Service Date")]
        public string FirstServiceDate { get; set; }
        [Description("QC Visibility")]
        public string QCVisibility { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Sub Category")]
        public string ProductSubCategory { get; set; }
        [Description("Product Name")]
        public string ProductSubCategory2 { get; set; }
        [Description("Office")]
        public string Office { get; set; }
        [Description("CS Name")]
        public string CSNames { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
        [Description("Inspection Create date")]
        public DateTime? CreateDate { get; set; }
    }

    public class ScheduleBookingItemExportSummaryProductLevel
    {
        [Description("Booking No")]
        public int? BookingId { get; set; }
        [Description("Report No")]
        public string ReportTitle { get; set; }
        [Description("PO Number")]
        public string PoNumber { get; set; }
        [Description("Product Ref")]
        public string ProductRef { get; set; }
        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("Order Qty")]
        public int? OrderQty { get; set; }
        [Description("Unit")]
        public string Unit { get; set; }
        [Description("Color Code")]
        public string ColorCode { get; set; }
        [Description("Color Name")]
        public string ColorName { get; set; }
        [Description("MS Chart")]
        public string MSChart { get; set; }
        [Description("Brand")]
        public string Brand { get; set; }
        [Description("Season")]
        public string Season { get; set; }
        [Description("Inspection Status")]
        public string InspectionStatus { get; set; }
        [Description("Province")]
        public string ProvinceName { get; set; }
        [Description("City")]
        public string CityName { get; set; }
        [Description("County")]
        public string CountyName { get; set; }
        [Description("Zone")]
        public string Zone { get; set; }
        [Description("Town")]
        public string TownName { get; set; }
        [Description("Quotation Status")]
        public string QuotationStatus { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Service From Date")]
        public DateTime? ServiceFromDate { get; set; }
        [Description("Service To Date")]
        public DateTime? ServiceToDate { get; set; }
        [Description("Actual Man Day")]
        public double ActualManday { get; set; }
        [Description("Estimate Man Day (Quotation MD)")]
        public double QuotationManday { get; set; }
        [Description("Planned Man Day")]
        public double PlannedManday { get; set; }
        [Description("Suggested Man Day")]
        public double SuggestedManday { get; set; }
        [Description("QC")]
        public string QCName { get; set; }
        [Description("Addtional QC")]
        public string AdditionalQCName { get; set; }
        [Description("Report Checker")]
        public string CSName { get; set; }
        [Description("CS Name")]
        public string CSNames { get; set; }
        [Description("Customer")]
        public string CustomerName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Factory Address")]
        public string FactoryAddress { get; set; }
        [Description("Factory Contact")]
        public string FactoryContact { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Booking Sample Size")]
        public int SampleSize { get; set; }
        [Description("Report Sample Size")]
        public double ReportSampleSize { get; set; }
        [Description("First Service Request Date")]
        public string FirstServiceDate { get; set; }
        [Description("QC Visibility")]
        public string QCVisibility { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Sub Category")]
        public string ProductSubCategory { get; set; }
        [Description("Product Name")]
        public string ProductSubCategory2 { get; set; }
        [Description("Office")]
        public string Office { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
        [Description("Inspection Create date")]
        public DateTime? CreateDate { get; set; }
    }

    public class QCBookingInfo
    {
        public string ServiceDate { get; set; }
        public string QCName { get; set; }
        public string CSName { get; set; }
        public string AdditionalQCName { get; set; }
        public double ActualManDay { get; set; }
    }

    public class QCInfo
    {
        public string QCName { get; set; }
        public double ActualManDay { get; set; }
        public double TotalBooking { get; set; }
    }
    public class CSInfo
    {
        public string CSName;
        public double ActualManDay;
    }
    public class ServiceDateQCNames
    {
        public IEnumerable<QCInfo> QCInfo;
        public string ServiceDate;
    }
    public class ServiceDateCSNames
    {
        public IEnumerable<CSInfo> CSInfo;
        public string ServiceDate;
    }

    public class MandayForecastItem
    {
        public string Date { get; set; }
        public string DayOftheWeek { get; set; }
        public string Location { get; set; }
        public double ManDaycount { get; set; }
        public int AvailableQcCount { get; set; }
        public int QcOnLeaveCount { get; set; }
        public string Color { get; set; }
    }

    public class MandayForecastResponse
    {
        public List<MandayForecastItem> Data { get; set; }
        public IEnumerable<CommonDataSource> DataSourceList { get; set; } //Office Location List
        public List<ManDayScheduleLocName> LocationName { get; set; }
        public ScheduleSearchResponseResult Result { get; set; }
    }

    public class ManDayScheduleLocName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ZoneId { get; set; }
        public int officeId { get; set; }
    }
    public class QuotScheduleManday
    {
        public int BookingId { get; set; }
        public double? TotalManday { get; set; }
        public double? TravelManday { get; set; }
        public List<QuotationManday> MandayList { get; set; }
        public double? SuggestedManday { get; set; }
    }

    public class QuotScheduleMandayResponse
    {
        public QuotScheduleManday Data { get; set; }
        public ScheduleSearchResponseResult Result { get; set; }
    }
    public class BookingDataQcVisibleResponse
    {
        public IEnumerable<BookingDataQcVisible> Data { get; set; }
        public ScheduleSearchResponseResult Result { get; set; }
    }
    public class BookingDataQcVisibleRequest
    {
        public IEnumerable<BookingDataQcVisible> BookingDataQcVisible { get; set; }
    }

    public class BookingDataQcVisible
    {
        public int BookingId { get; set; }
        public string ServiceDate { get; set; }
        public bool IsQcVisibility { get; set; }

    }

    public class ReportDetails
    {
        public int? ReportMapId { get; set; }
        public string ReportTitle { get; set; }
        public int ProductId { get; set; }
    }
}
