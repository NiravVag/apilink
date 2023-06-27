using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Dashboard;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class SupFactDashboardManager : ApiCommonData, ISupFactDashboardManager
    {
        readonly ISupFactDashboardRepository _repo = null;
        readonly IInspectionBookingManager _bookingManager = null;
        readonly IAPIUserContext _applicationContext = null;
        private readonly SupFactDashboardMap SupFactDashboardMap = null;

        public SupFactDashboardManager(ISupFactDashboardRepository repo, IInspectionBookingManager bookingManager,
            IAPIUserContext applicationContext)
        {
            _repo = repo;
            _bookingManager = bookingManager;
            _applicationContext = applicationContext;
            SupFactDashboardMap = new SupFactDashboardMap();
        }

        /// <summary>
        /// get booking id list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CommonDataListResponse> GetBookingDetails(SupFactDashboardModel request)
        {
            if (request == null)
                return new CommonDataListResponse() { Result = DataSourceResult.RequestNotCorrectFormat };

            var inspList = _bookingManager.GetAllInspections();

            //not equal to cancel status from booking
            inspList = inspList.Where(x => x.StatusId != (int)BookingStatus.Cancel);

            if (request.CustomerId > 0)
            {
                inspList = inspList.Where(x => x.CustomerId == request.CustomerId);
            }
            if (request.SupplierId > 0)
            {
                inspList = inspList.Where(x => x.SupplierId == request.SupplierId);
            }
            if (request.FactoryId > 0)
            {
                inspList = inspList.Where(x => x.FactoryId == request.FactoryId);
            }
            if (request.StatusIdList != null && request.StatusIdList.Any())
            {
                inspList = inspList.Where(x => request.StatusIdList.Contains(x.StatusId));
            }
            if (request.FromDate != null && request.ToDate != null)
            {
                inspList = inspList.Where(x => !((x.ServiceDateFrom > request.ToDate.ToDateTime())
                                         || (x.ServiceDateTo < request.FromDate.ToDateTime())));
            }

            if (inspList.Any())
            {
                //execute the data
                var BookingIdList = await inspList.Select(x => x.BookingId).ToListAsync();

                return new CommonDataListResponse() { IdList = BookingIdList, Result = DataSourceResult.Success };
            }
            else
            {
                return new CommonDataListResponse() { Result = DataSourceResult.CannotGetList };
            }
        }
        /// <summary>
        /// Get geo code for factory
        /// </summary>
        /// <param name="InspIdList"></param>
        /// <returns></returns>
        public async Task<FactMapGeoLocation> GetInspFactoryGeoCode(IEnumerable<int> InspIdList)
        {
            var response = new FactMapGeoLocation();

            var lstgeocode = await _repo.GetInspFactoryGeoCode(InspIdList);

            var grpByFactory = lstgeocode.GroupBy(p => p.FactoryId, (key, _data) => new InspFactoryGeoCode
            {
                FactoryId = key,
                FactoryName = _data.Where(x => x.FactoryId == key).Select(x => x.FactoryName).FirstOrDefault(),
                Latitude = _data.Where(x => x.FactoryId == key).Select(x => x.FactoryLatitude).FirstOrDefault(),
                Longitude = _data.Where(x => x.FactoryId == key).Select(x => x.FactoryLongitude).FirstOrDefault(),
                TotalCount = _data.Count()
            }).ToList();

            if (grpByFactory.Any(x => x.Latitude.HasValue && x.Longitude.HasValue))
            {
                response.FactoryGeoCode = grpByFactory.Where(x => x.Latitude.HasValue && x.Longitude.HasValue).ToList();
                response.Result = DashboardResult.Success;
            }
            else
            {
                response.Result = DashboardResult.CannotGetList;
            }

            return response;
        }

        /// <summary>
        /// get booking details
        /// </summary>
        /// <param name="InspIdList"></param>
        /// <returns></returns>
        public async Task<CusBookingDataResponse> GetCusBookingData(IEnumerable<int> InspIdList)
        {
            var response = new CusBookingDataResponse();

            var bookingList = await _repo.GetCusBookingDetails(InspIdList);

            if (bookingList.Any())
            {
                Random r = new Random();
                response.CusBookingDetails = bookingList.GroupBy(p => p.CustomerName, (key, _data) => new CustomerBookingModel
                {
                    CustomerName = key,
                    BookingCount = _data.Count(),
                    StatusColor = MandayDashboardColorList.GetValueOrDefault(r.Next(1, 8))
                }).ToList();

                response.Result = DashboardResult.Success;
            }
            else
            {
                response.Result = DashboardResult.CannotGetList;
            }
            return response;
        }

        /// <summary>
        /// get booking details
        /// </summary>
        /// <param name="InspIdList"></param>
        /// <returns></returns>
        public async Task<BookingDataResponse> GetBookingData(IEnumerable<int> InspIdList)
        {
                var response = new BookingDataResponse();

                var bookingList = await _repo.GetBookingDetails(InspIdList);

                var serviceTypeList = await _bookingManager.GetServiceTypeList(InspIdList);

                if (bookingList.Any())
                {
                    response.BookingDetails = bookingList.Select(x => SupFactDashboardMap.BookingDataMap(x, _applicationContext.UserId, serviceTypeList.ToList())).ToList();
                    response.Result = DashboardResult.Success;
                }
                else
                {
                    response.Result = DashboardResult.CannotGetList;
                }
                return response;
        }
    }
}
