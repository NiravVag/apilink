using DTO.Common;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ScheduleJob
{
    public class ScheduleJobRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ScheduleJobResponse
    {
        public int BookingId { get; set; }
        public int ReportId { get; set; }
        public DateTime InspectionStartDate { get; set; }
        public DateTime InspectionEndDate { get; set; }
        public string ReportNumber { get; set; }
        public string InspectionDate { get; set; }
        public string ProductRefernce { get; set; }
        public string Ean { get; set; }
        public string MeasuredLength { get; set; }
        public string MeasuredWeight { get; set; }
        public string MeasuredHeight { get; set; }
        public string MeasuredPackingWeight { get; set; }
        public string MeasuredPackingWeightUnit { get; set; }
    }

    public class ScheduleTravelTariffEmail
    {
        public int Id { get; set; }
        public string StartPortName { get; set; }
        public string FactoryTown { get; set; }
        public string TravelTariffUrl { get; set; }
    }

    public class ScheduleTravelTariffEmailResponse
    {
        public List<ScheduleTravelTariffEmail> TravelTariffList { get; set; }
    }

    public class ScheduleJobCarrfourDailyResultEmailResponse
    {
        public List<JobConfiguration> JobDataList { get; set; }
    }

    public class ScheduleJobLogRequest
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? ReportId { get; set; }
        public int? ScheduleType { get; set; }
        public string FileName { get; set; }
    }

    public class CulturaPackingSettings
    {
        public string StartDate { get; set; }
        public string FileName { get; set; }
        public int ScheduleInterval { get; set; }
        public string FolderPath { get; set; }
    }

    public enum ScheduleType
    {
        Cultura = 1
    }


    public class StartPortCity
    {
        public int? StartPortId { get; set; }
        public int? CityId { get; set; }
    }

    public class FactoryTownCity
    {
        public int? TownId { get; set; }
        public int? CityId { get; set; }
    }

    public class QcExpenseEmailData
    {
        public int? QcId { get; set; }
        public string QcName { get; set; }
        public string QcEmail { get; set; }
        public string CustomerName { get; set; }
        public int BookingId { get; set; }
        public string Style { get; set; }
        public int OrderQty { get; set; }
        public string ServiceType { get; set; }
        public string FactoryENAddress { get; set; }
        public string FactoryLocalAddress { get; set; }
        public double? TravelFee { get; set; }
        public string TripMode { get; set; }
        public string StartPort { get; set; }
        public string EndPort { get; set; }
        public double? FoodAllowance { get; set; }
        public bool IsChinaCountry { get; set; }

        public string Currency { get; set; }
    }

    public class QcPendingExpenseData
    {
        public int Id { get; set; }
        public int? QcId { get; set; }
        // travel or food allowance 
        public int ExpenseTypeId { get; set; }
        public int? PayrollCurrency { get; set; }
        public string QcName { get; set; }
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string StartPort { get; set; }
        public string EndPort { get; set; }
        public string FactoryTown { get; set; }
        public string FactoryProvince { get; set; }
        public string FactoryCity { get; set; }
        public string FactoryCounty { get; set; }
        public string FactoryCountry { get; set; }
        public double FoodAllowance { get; set; }
        public double TravelAllowance { get; set; }
        public string TripType { get; set; }
        public bool Status { get; set; }
        public bool IsExpenseSelected { get; set; }
        public DateTime ServiceDate { get; set; }
    }

    public class QcPendingExpenseResponse
    {
        public List<QcPendingExpenseData> QcPendingExpenseData { get; set; }
        public QcPendingExpenseDataResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }


    public class SaveQcPendingExpenseResponse
    {
        public QcPendingExpenseDataResult Result { get; set; }
    }

    public enum SaveQcPendingExpenseResponseResult
    {
        Success = 1,
        Failure = 2
    }

    public enum QcPendingExpenseDataResult
    {
        Success = 1,
        NotFound = 2,
        Failure = 3
    }

    public class QcPendingExpenseRequest
    {
        public DateObject StartDate { get; set; }
        public DateObject EndDate { get; set; }
        public IEnumerable<int> OfficeIdList { get; set; }
        public IEnumerable<int> QcIdList { get; set; }
        public int SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        public int Datetypeid { get; set; }
        public int? StatusId { get; set; }
        public int? ExpenseTypeId { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }

    public class QcExpenseEmailResponse
    {
        public List<QcExpenseEmailData> QcExpenseEmailList { get; set; }
    }

    public enum AutoQcPendingStatus
    {
        Configured = 1,
        NotConfigured = 2
    }

    public enum AutoQcExpenseType
    {
        TravellAllowance = 1,
        FoodAllowance = 2
    }

    public class ScheduleClaimReminderEmail
    {
        public InspTransaction InspectionTransaction { get; set; }
        public int ClaimId { get; set; }
        public string ClaimNo { get; set; }
        public DateTime? ClaimDate { get; set; }
        public int? BookingId { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string StatusName { get; set; }
        public int? StatusId { get; set; }
        public string Office { get; set; }
        public int? OfficeId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int? CreatedBy { get; set; }

    }

    public class ScheduleClaimReminderEmailItem
    {
        public int ClaimId { get; set; }
        public string ClaimNo { get; set; }
        public string ClaimURL { get; set; }
        public string InspectionURL { get; set; }
        public string ClaimDate { get; set; }
        public int? BookingId { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string InspectionDate { get; set; }
        public string ServiceDate { get; set; }
        public string StatusName { get; set; }
        public int? StatusId { get; set; }
        public string Office { get; set; }
        public int OfficeId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int CreatedBy { get; set; }

    }
    public class ClaimReminderEmailResponse
    {
        public List<ScheduleClaimReminderEmailItem> ScheduleClaimReminderEmailList { get; set; }
    }

    public class PushReportInfoToFastReportResponse
    {
        public bool Result { get; set; }
    }

    public class RequestZIPFile
    {
        public int Container { get; set; }
        public int EntityId { get; set; }
        public List<ZIPFileData> ZIPFileDataList { get; set; }
    }
    public class ZIPFileData
    {
        public string UniqueId { get; set; }
        public string FileName { get; set; }
    }

    public class ZipFileUploadResponse
    {
        public ZipFileUploadData FileUploadData { get; set; }
    }

    public class ZipFileUploadData
    {
        public string FileName { get; set; }
        public string FileCloudUri { get; set; }
        public string FileUniqueId { get; set; }
        public ZipFileUploadResponseResult Result { get; set; }
    }

    public class SchedulePlanningForCSResponse
    {
        public SchedulePlanningForCS data { get; set; }
        public List<string> errors { get; set; }
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }

    public class SchedulePlanningForCS
    {
        public JobConfiguration JobConfiguration { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<int> OfficeIdList { get; set; }
        public string EntityName { get; set; }
        public DateTime SendDate { get; set; }
        public int IntervalDay { get; set; }
    }

    public class SchedulePlanningFileDateForCS
    {
        [Description("Week")]
        public int Week { get; set; }
        [Description("DayNo")]
        public string DayNo { get; set; }
        [Description("Apply Date")]
        public string ApplyDate { get; set; }
        [Description("Service Date")]
        public string ServiceDate { get; set; }
        [Description("Insp No")]
        public int BookingId { get; set; }
        [Description("Product Reference")]
        public string PoNo { get; set; }
        [Description("IsMSChart")]
        public string IsMSChart { get; set; }
        [Description("Product Name")]
        public string ProductSubCategory2 { get; set; }
        [Description("Color Name")]
        public string ColorName { get; set; }
        [Description("QTY")]
        public int? BookingQuantity { get; set; }
        [Description("Unit")]
        public string UnitNameCount { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Product Category")]
        public string ProductSubCategory { get; set; }
        [Description("QC")]
        public string QCNames { get; set; }
        [Description("Report Checker")]
        public string CSNames { get; set; }
        [Description("Customer")]
        public string Customer { get; set; }
        [Description("Brand")]
        public string Brand { get; set; }
        [Description("Supplier")]
        public string Supplier { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }
        [Description("Address")]
        public string FactoryAddress { get; set; }
        [Description("Chn Address")]
        public string FactoryChinaAddress { get; set; }
        [Description("Order Status")]
        public string Status { get; set; }
        [Description("Office")]
        public string Office { get; set; }
    }

    public class InitialBookingExtractResponse
    {
        public InitialBookingExtract data { get; set; }
        public List<string> errors { get; set; }
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }

    public class InitialBookingExtract
    {
        public JobConfiguration JobConfiguration { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime SendDate { get; set; }
        public int IntervalDay { get; set; }
        public List<int> CustomerIds { get; set; }
    }

    public enum ZipFileUploadResponseResult
    {
        Sucess = 1,
        Failure = 2,
        FileSizeExceed = 3
    }

    public enum ScheduleOptions
    {
        ScheduleQcEmail = 1,
        ScheduleFbReport = 2,
        ScheduleCulturaPackingInfo = 3,
        ScheduleTravelTariffEmail = 4,
        ScheduleAutoQcExpense = 5,
        ScheduleQcInspectionExpenseEmail = 6,
        ScheduleClaimReminderEmail = 7,
        ScheduleFastReport = 8,
        ScheduleCarrefourDailyResult = 9,
        ScheduleMissedMSchart = 11,
        ScheduleBookingCS = 12,
        SchedulePlanningForCS = 13,
        BBGInitialBookingExtract = 14
    }
}
