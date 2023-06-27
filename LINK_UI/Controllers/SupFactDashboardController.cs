using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Dashboard;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class SupFactDashboardController : ControllerBase
    {

        readonly ISupFactDashboardManager _manager = null;
        public SupFactDashboardController(ISupFactDashboardManager manager)
        {
            _manager = manager;
        }

        [HttpPost("get-Booking-data")]
        [Right("sup-dashboard")]
        public async Task<CommonDataListResponse> GetBookingData(SupFactDashboardModel request)
        {
            return await _manager.GetBookingDetails(request);
        }

        [HttpPost("get-factory-geo-code")]
        [Right("sup-dashboard")]
        public async Task<FactMapGeoLocation> GetInspFactoryGeoCode(IEnumerable<int> InspIdList)
        {
            return await _manager.GetInspFactoryGeoCode(InspIdList);
        }

        [HttpPost("get-customer-data")]
        [Right("sup-dashboard")]
        public async Task<CusBookingDataResponse> GetCusBookingData(IEnumerable<int> InspIdList)
        {
            return await _manager.GetCusBookingData(InspIdList);
        }

        [HttpPost("get-booking-details")]
        [Right("sup-dashboard")]
        public async Task<BookingDataResponse> GetBookingDetails(IEnumerable<int> InspIdList)
        {
            return await _manager.GetBookingData(InspIdList);
        }
    }
}