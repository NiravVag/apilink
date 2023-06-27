using DTO.Audit;
using DTO.CommonClass;
using DTO.Dashboard;
using DTO.Manday;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IMandayRepository
    {
        IQueryable<AuditResponseManday> GetAllAudits();
        Task<List<InspectionMandayDashboard>> GetAuditManDays(IEnumerable<int> auditIds);
        Task<List<CommonDataSource>> GetServices();
        Task<List<CommonDataSource>> GetOfficeLocations();
        Task<List<MandayYearChartItem>> GetMonthlyAuditManDays(IEnumerable<int> auditIds);
        Task<List<MandayYearChartItem>> GetMonthlyInspManDays(IEnumerable<int> bookingIds);
        IQueryable<MandayCustomerChart> GetCustomerInspManDays(IQueryable<int> bookingIds);
        IQueryable<MandayCustomerChart> GetCustomerAuditManDays(IEnumerable<int> bookingIds);
        Task<List<MandayCountryChart>> GetCountryInspManDays(IEnumerable<int> bookingIds, int countryId);
        Task<List<MandayCountryChart>> GetCountryAuditManDays(IEnumerable<int> bookingIds, int countryId);
        Task<List<EmployeeTypes>> GetMandayInspByEmployeeType(IEnumerable<int> bookingIds, DateTime serviceDateFrom, DateTime serviceDateTo);
        Task<List<EmployeeTypes>> GetMandayAuditByEmployeeType(IEnumerable<int> bookingIds);
        Task<List<MandayCountryChartExport>> GetCountryInspManDaysExport(IEnumerable<int> bookingIds, bool isCountrySelected);
        Task<List<MandayCountryChartExport>> GetCountryAuditManDaysExport(IEnumerable<int> bookingIds, bool isCountrySelected);
        Task<List<MandayCustomerChartData>> GetCustomerInspReportsData(IEnumerable<int> bookingIds);
        Task<List<MandayYearChartItem>> GetMonthlyInspActualManDays(IEnumerable<int> bookingIds);
        IQueryable<MandayCustomerChart> GetCustomerActualManDays(IQueryable<int> bookingIds);
        Task<List<MandayCountryChart>> GetCountryActualInspManDays(IEnumerable<int> bookingIds, int countryId);
        Task<List<EmployeeTypes>> GetActualMandayInspByEmployeeType(IEnumerable<int> bookingIds, DateTime serviceDateFrom, DateTime serviceDateTo);
        Task<List<MandayCountryChartExport>> GetCountryInspActualManDaysExport(IEnumerable<int> bookingIds, bool isCountrySelected);
    }
}
