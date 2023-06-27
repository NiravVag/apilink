using DTO.Common;
using DTO.Manday;
using DTO.Quotation;
using DTO.Schedule;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.UtilizationDashboard
{
    public class UtilizationDashboard
    {
        public string Office { get; set; }
        public int? HourMandDays { get; set; }
        public int WorkDays { get; set; }
        public double Leaves { get; set; }
        public int MaxPotential { get; set; }
        public int OutsourceMandays { get; set; }
        public double OutsourceMandaysPercentage { get; set; }
        public double UtilizationRateLastYear { get; set; }
        public double UtilizationRateCurrentYear { get; set; }
        public double UtilizationPercentage { get; set; }
    }

    public class UtilizationDashboardRequest
    {
        [Required]
        public DateObject ServiceDateFrom { get; set; }
        [Required]
        public DateObject ServiceDateTo { get; set; }
        public List<int> OfficeIdList { get; set; }
        public int ServiceId { get; set; }
        public List<int> CountryIdList { get; set; }
    }

    public class UtilizationResponse
    {
        public List<UtilizationDashboard> Data { get; set; }
        public UtilizationGraphData GraphData { get; set; }
        public UtilizationDashboardResult Result { get; set; }
    }

    public class UtilizationExportResponse
    {
        public List<UtilizationDashboard> Data { get; set; }
        public UtilizationDashboardResult Result { get; set; }
        public MandayDashboardRequestExport RequestFilter { get; set; }
    }

    public enum UtilizationDashboardResult
    {
        Success = 1,
        Fail = 2,
        NotFound = 3
    }

    public class BookingAuditItems
    {
        public int Id { get; set; }
        public int? OfficeId { get; set; }
        public int StatusId { get; set; }
        public string Office { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public int? FactoryId { get; set; }
    }

    public class LeaveData
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public double NoOfDays { get; set; }
    }

    public class UtilizationData
    {
        public List<QuotationManday> Mandays { get; set; }
        //public int NoOfWorkingDays { get; set; }
        //public QcLeaveData QcDetails { get; set; }
        public List<LeaveData> LeaveCount { get; set; }
        public List<QCStaffInfo> QcList { get; set; }
        public List<DateTime> HoidayListDates { get; set; }
        public List<DateTime> SearchDateRange { get; set; }
        public List<HrHoliday> HrHolidayList { get; set; }
    }

    public class UtilizationDataMapRequest
    {
        public List<QuotationManday> Mandays { get; set; }
        public int NoOfWorkingDays { get; set; }
        public int LocationId { get; set; }
        public List<LeaveData> LeaveCount { get; set; }
        public List<QCStaffInfo> QcList { get; set; }
        public List<HrHoliday> HrHolidayList { get; set; }
        public double UtilizationRateCurrentYear { get; set; }
        public double UtilizationRateLastYear { get; set; }
    }

    public class QcLeaveData
    {
        public List<StaffSchedule> AvailableQc { get; set; }
        public List<StaffSchedule> UnAvailableQc { get; set; }
    }

    public class UtilizationGraphData
    {
        public double TotalUtilization { get; set; }
        public List<GradingData> GradingData { get; set; }
    }

    public class GradingData
    {
        public string Title { get; set; }
        public string color { get; set; }
        public int LowScore { get; set; }
        public int HighScore { get; set; }
    }

    public class QuotationManday
    {
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public DateTime ServiceDate { get; set; }
        public int BookingId { get; set; }
        public double? ManDay { get; set; }
        public int StaffId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int StaffType { get; set; }
        public int? factoryId { get; set; }
    }

    public class UtilizationMandayYearChartItem
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public double MonthManDay { get; set; }
        public string MonthName { get; set; }
    }
}
