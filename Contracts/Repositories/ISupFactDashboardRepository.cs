using DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ISupFactDashboardRepository
    {
        Task<List<FactoryGeoCode>> GetInspFactoryGeoCode(IEnumerable<int> InspIdList);

        Task<List<CustomerBookingModel>> GetCusBookingDetails(IEnumerable<int> InspIdList);

        Task<List<BookingDetailsRepo>> GetBookingDetails(IEnumerable<int> InspIdList);
    }
}
