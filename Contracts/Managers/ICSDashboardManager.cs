using DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICSDashboardManager
    {
        Task<GetNewBookingRelatedCountResponse> GetCountNewBookingRelatedDetails(CSDashboardModelRequest request);

        Task<CSDashboardserviceTypeResponse> GetServiceTypeList(CSDashboardModelRequest request);

        Task<CSDashboardMandayByOfficeResponse> GetMandayByOfficeList(CSDashboardModelRequest request);

        Task<DayFBReportCountResponse> GetReportCountByDayList(CSDashboardModelRequest request);

        Task<StatusListCountResponse> GetStatusByLoggedUserList(CSDashboardModelRequest request);
    }
}

