using DTO.Common;
using DTO.OtherManday;
using DTO.Schedule;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Report
{
    public class InspectionOccupancyRepoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OfficeCountryId { get; set; }
        public string OfficeCountry { get; set; }
        public int? OfficeId { get; set; }
        public string Office { get; set; }
        public int EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public string OutSourceCompany { get; set; }
        public DateTime? JoinDate { get; set; }
        public int? OutSourceCompanyId { get; set; }
        public DateTime? RegisnDate { get; set; }
    }

    public class InspectionOccupancySummary
    {
        public string Name { get; set; }
        public string OfficeCountry { get; set; }
        public string Office { get; set; }
        public string EmployeeType { get; set; }
        public string OutsourceCompany { get; set; }
        public string JoinDate { get; set; }
        public int NumberOfWds { get; set; }
        public int MaxWds { get; set; }
        public int Leaves { get; set; }
        public int BankHolidays { get; set; }
        public int TotalActualWds { get; set; }
        public decimal UtilizationRate { get; set; }
        public int MaxCapacity { get; set; }
        public string ContractEnd { get; set; }
        public double? OtherManday { get; set; }
        public InspectionOccupancyCategory? InspectionOccupancyCategory { get; set; }
    }

    public class ExportInspectionOccupancySummary
    {
        [Description("Name")]
        public string Name { get; set; }
        [Description("Office Country")]
        public string OfficeCountry { get; set; }
        [Description("Office")]
        public string Office { get; set; }
        [Description("Employee Type")]
        public string EmployeeType { get; set; }
        [Description("Outsource Company")]
        public string OutsourceCompany { get; set; }
        [Description("Joint Date")]
        public string JoinDate { get; set; }
        [Description("Number Of WDs")]
        public int NumberOfWds { get; set; }
        [Description("Max WDs")]
        public int MaxWds { get; set; }
        [Description("Leaves")]
        public int Leaves { get; set; }
        [Description("Bank Holidays")]
        public int BankHolidays { get; set; }
        [Description("Total Actual WDs")]
        public int TotalActualWds { get; set; }
        [Description("Utilization Rate")]
        public double UtilizationRate { get; set; }
        [Description("Max Capacity")]
        public int MaxCapacity { get; set; }
        [Description("Contract End")]
        public string ContractEnd { get; set; }
        [Description("Other Manday")]
        public double? OtherManday { get; set; }
        public InspectionOccupancyCategory InspectionOccupancyCategory { get; set; }
    }

    public class InspectionOccupancySearchRequest
    {
        public int? OfficeId { get; set; }
        public int? OfficeCountryId { get; set; }
        public int? EmployeeType { get; set; }
        public int? OutSourceCompany { get; set; }

        public int? QA { get; set; }
        [Required]
        public DateObject FromDate { get; set; }
        [Required]
        public DateObject ToDate { get; set; }
        public bool UtilizationRate { get; set; }
        public IEnumerable<int> InspectionOccupancyCategories { get; set; }
        public int? Index { get; set; }

        public int? pageSize { get; set; }

    }

    public class InspectionOccupancySummaryResponse
    {
        public int? Index { get; set; }
        public int? PageSize { get; set; }
        public IEnumerable<InspectionOccupancySummary> Data { get; set; }
        public IEnumerable<InspectionOccupancyCategorySummary> StatusList { get; set; }
        public int TotalCount { get; set; }
        public InspectionOccupancyResult Result { get; set; }
    }

    public enum InspectionOccupancyResult
    {
        Success = 1,
        NotFound = 2
    }

    public class InspectionOccupancyCategorySummary
    {
        public InspectionOccupancyCategory InspectionOccupancyCategory { get; set; }
        public int Count { get; set; }
        public string Color { get; set; }
        public string Label { get; set; }
    }

    public enum InspectionOccupancyCategory
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public class ExportInspectionOccupancySummaryResponse
    {
        public List<ExportInspectionOccupancySummary> InspectionOccupancies { get; set; }
        public InspectionOccupancyResult Result { get; set; }
    }

    public class InspectionOccupancyData
    {
        public IEnumerable<ActualManday> ScheduleQcs { get; set; }
        public IEnumerable<HrLeave> HrLeaves { get; set; }
        public IEnumerable<OtherMandayDataRepo> OtherMandays { get; set; }
        public IEnumerable<InspectionOccupancyHolidayDto> Holidays { get; set; }
    }

    public class InspectionOccupancyHolidayDto
    {
        public DateTime HolidayDate { get; set; }
        public int? LocationId { get; set; }
        public int CountryId { get; set; }
    }
}
