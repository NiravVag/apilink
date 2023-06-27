using DTO.Common;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Schedule
{
    public class AllocationStaff
    {
        public DateTime ServiceDate { get; set; }
        public double ActualManDay { get; set; }
        public double AvailableManDay { get; set; }
        public IEnumerable<StaffSchedule> QC { get; set; }
        public IEnumerable<StaffSchedule> AddtionalQC { get; set; }
        public IEnumerable<StaffSchedule> CS { get; set; }
        public double ManDay { get; set; }
        public string Remarks { get; set; }
        public bool IsQcVisibility { get; set; }
        public IEnumerable<QcAutoExpense> QcAutoExpenseList { get; set; }
    }

    public class QcAutoExpense
    {
        public int? InspectionId { get; set; }
        public int? QcId { get; set; }
        public string QcName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public int? StartPortId { get; set; }
        public string StartPortName { get; set; }
        public int? FactoryTownId { get; set; }
        public string FactoryTownName { get; set; }
        public int? TripTypeId { get; set; }
        public string TripTypeName { get; set; }
        public double? TravelTariff { get; set; }
        public int? TravelTariffCurrency { get; set; }
        public double? FoodAllowance { get; set; }
        public int? FoodAllowanceCurrency { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string Comments { get; set; }
        public int? ExpenseStatus { get; set; }
    }

    public class ScheduleQuotationManDay
    {
        public double ManDay { get; set; }
        public int BookingId { get; set; }
        public DateTime ServiceDate { get; set; }
    }
    public class QCStaffInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string EmergencyCall { get; set; }
        public string Location { get; set; }
        public int LocationId { get; set; }
        public int ZoneId { get; set; }
        public int? StartPortId { get; set; }
        public string StartPortName { get; set; }
        public int EmployeeType { get; set; }
        public IEnumerable<SchScheduleQc> ScheduleQC { get; set; }
        public IEnumerable<HrLeave> LeaveQC { get; set; }
        public HrStaffPhoto StaffImage { get; set; }
    }

    public class AllocationStaffList
    {
        public IEnumerable<StaffSchedule> QCList { get; set; }
        public IEnumerable<StaffSchedule> AddtionalQCList { get; set; }
        public IEnumerable<StaffSchedule> CSList { get; set; }
    }

    public class AllocationStaffSearchRequest
    {
        [Required]
        public string ServiceDate { get; set; }
        [Required]
        public int BookingId { get; set; }
        [Required]
        public string Type { get; set; }
        public int? OfficeId { get; set; }
        public int? EntityId { get; set; }
        public int? EmployeeType { get; set; }
        public int? ZoneId { get; set; }
        public int? OutSourceCompany { get; set; }
        public int? StartPortId { get; set; }
        public int? MarketSegmentId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ExpertiseId { get; set; }
        public string SearchText { get; set; }

        public int? Skip { get; set; }
        public int? Take { get; set; }

    }
    public class ScheduleStaffItem
    {
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public int QCType { get; set; }
        public DateTime ServiceDate { get; set; }
        public int Id { get; set; }
        public string CompanyEmail { get; set; }
        public string Name { get; set; }
        public double ActualManDay { get; set; }
        public bool IsQcVisibility { get; set; }
        public string Email { get; set; }
        public string EmployeeTypeName { get; set; }
        public int CustomerId { get; set; }
        public string PayrollCurrency { get; set; }
        public bool IsChinaCountry { get; set; }
        public string StartPortName { get; set; }
    }

    public class BookingCsItem
    {
        public int BookingId { get; set; }

        public int CsId { get; set; }

    }
}
