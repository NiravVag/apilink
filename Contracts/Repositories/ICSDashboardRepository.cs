using DTO.Common;
using DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICSDashboardRepository : IRepository
    {
        Task<List<CSDashboardCountItem>> GetCountNewDetails(CSDashboardFilterModel request);

        Task<List<CSDashboardItem>> GetServiceTypeList(CSDashboardDBRequest request);

        Task<List<CSDashboardItem>> GetMandayByOfficeList(CSDashboardDBRequest request);

        Task<List<DayReportCountRepo>> GetDayFBReportCountList(CSDashboardDBRequest request);

        //StatusTaskCountListRepo GetStatusTaskCountList(CSDashboardStatusDBRequest request);

        Task<List<StatusTaskCountItemRepo>> GetStatusTaskCountList(CSDashboardStatusDBRequest request);

        Task<List<BookingDetail>> GetBookingDetail(CSDashboardModelRequest request, IAPIUserContext applicationContext);
    }
}
