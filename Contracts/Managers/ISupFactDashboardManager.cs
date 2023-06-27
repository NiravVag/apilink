using DTO.CommonClass;
using DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ISupFactDashboardManager
    {
        Task<CommonDataListResponse> GetBookingDetails(SupFactDashboardModel request);

        Task<FactMapGeoLocation> GetInspFactoryGeoCode(IEnumerable<int> InspIdList);

        Task<BookingDataResponse> GetBookingData(IEnumerable<int> InspIdList);

        Task<CusBookingDataResponse> GetCusBookingData(IEnumerable<int> InspIdList);
    }
}
