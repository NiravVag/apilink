using DTO.Manday;
using DTO.Schedule;
using DTO.UtilizationDashboard;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IUtilizationDashboardRepository
    {
        Task<IEnumerable<QCStaffInfo>> GetQCListByLocationForForecast(List<int> location, int serviceId);
        IQueryable<BookingAuditItems> GetAllInspections();
        IQueryable<BookingAuditItems> GetAllAudits();
        Task<List<QuotationManday>> GetQuotationManDayAudit(List<int> auditIds);
        Task<List<LeaveData>> GetHrLeaves(DateTime startdate, DateTime enddate, List<int> officeIdList, int serviceId);
        Task<List<HrHoliday>> GetHolidaysByRange(DateTime startdate, DateTime enddate, List<int> officeIdList, List<int> countryIdList);
        Task<List<QuotationManday>> GetActualInspManDay(List<int> bookingIds);
        Task<List<QuotationManday>> GetMonthlyInspManDays(List<int> bookingIds, UtilizationDashboardRequest request);
    }
}
