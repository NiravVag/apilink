using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.Maps;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.DataAccess;
using DTO.EventBookingLog;
using DTO.HumanResource;
using DTO.Inspection;
using DTO.QcDashboard;
using DTO.Quotation;
using DTO.RepoRequest.Enum;
using DTO.Schedule;
using DTO.UserAccount;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class QcDashboardManager : IQcDashboardManager
    {
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IQcDashboardRepository _repo = null;
        private readonly IScheduleRepository _scheduleRepo = null;
        private readonly ILogger<ScheduleManager> _logger = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IInspectionBookingManager _bookingmanager = null;
        private readonly IEventBookingLogManager _eventBookingLog = null;
        private readonly IHumanResourceRepository _hrRepo = null;
        private readonly IUserRightsManager _userRightsManager = null;
        private readonly QcDashboardMap QcDashboardMap = null;
        private const int QcDashboardDays = 4;

        public QcDashboardManager
            (
            IQcDashboardRepository repo,
            IAPIUserContext applicationContext,
            IScheduleRepository scheduleRepo,
            ILogger<ScheduleManager> logger,
            IInspectionBookingRepository bookingRepository,
            IFBReportManager fbReportManager,
            IInspectionBookingManager bookingmanager,
            IEventBookingLogManager eventBookingLog,
            IHumanResourceRepository hrRepo,
            IUserRightsManager userRightsManager
            )
        {
            _repo = repo;
            _ApplicationContext = applicationContext;
            _scheduleRepo = scheduleRepo;
            _logger = logger;
            _inspRepo = bookingRepository;
            _bookingmanager = bookingmanager;
            _eventBookingLog = eventBookingLog;
            _hrRepo = hrRepo;
            _userRightsManager = userRightsManager;
            QcDashboardMap = new QcDashboardMap();
        }

        ///// <summary>
        ///// Get the Qc Schedule Details
        ///// </summary>
        ///// <returns></returns>
        public async Task<QcDashboardCalendarResponse> GetQcScheduleDetails()
        {
            var todayDate = DateTime.Now.Date;
            DateTime fromdate = DateTime.Now.Date.AddDays(-4);
            DateTime Todate = DateTime.Now.Date.AddDays(4);
            var calendarDates = getCalendarDays(todayDate.AddDays(-4).Date, todayDate.AddDays(4).Date);
            var _qcId = _ApplicationContext.StaffId;

            //get qc scheduled details along with the booking information
            var qcBookingsByServiceDate = await _scheduleRepo.GetQCBookingbyServiceDate(_qcId, fromdate, Todate);
            //take the booking ids
            var qcBookingIds = qcBookingsByServiceDate.Select(x => x.BookingId).Distinct().ToList();
            //get factory address details
            var factoryDetails = await _inspRepo.GetFactorycountryId(qcBookingIds);

            //take the booking allocated count details
            var todayCount = qcBookingsByServiceDate.Count(x => x.ScheduledDate.Date == todayDate.Date);
            var tommorowCount = qcBookingsByServiceDate.Count(x => x.ScheduledDate.Date == todayDate.AddDays(1).Date);
            var _upcomingAllocatedCount = qcBookingsByServiceDate.Count(x => x.ScheduledDate.Date >= todayDate.AddDays(2).Date);
            //map the scheduled data for the one week data
            var _resultdata = calendarDates.Select(x => QcDashboardMap.GetQcScheduleResultData(x, qcBookingsByServiceDate, factoryDetails));

            try
            {
                return new QcDashboardCalendarResponse()
                {
                    Result = QcDashboardResponseResult.Success,
                    TodayCount = todayCount,
                    TomorrowCount = tommorowCount,
                    UpcomingAllocatedCount = _upcomingAllocatedCount,
                    QcCalendar = _resultdata
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the Qc DashboardCount Details
        /// </summary>
        ///<param name="request"></param>
        /// <returns></returns>
        public async Task<QcDashboardCountResponse> GetQcDashboardCountDetails(QcDashboardSearchRequest request)
        {
            if (request.serviceDateFrom?.ToDateTime() == null && request.serviceDateTo?.ToDateTime() == null)
            {
                return new QcDashboardCountResponse() { Result = QcDashboardResponseResult.NotFound };
            }

            var _qcId = _ApplicationContext.StaffId;

            var data = await _repo.GetScheduelQcDetails(_qcId, request.serviceDateFrom.ToDateTime(), request.serviceDateTo.ToDateTime());


            var _customerCount = data.Select(x => x.CustomerId).Distinct().Count();
            var _factoryCount = data.Select(x => x.FactoryId).Distinct().Count();
            var _inspectionCount = data.Select(x => x.BookingId).Distinct().Count();


            var reports = await _repo.GetQcReportsDetails(_qcId, data.Select(x => x.BookingId).Distinct().ToList());
            var _reportCount = reports.Count();
            try
            {
                return new QcDashboardCountResponse()
                {
                    Result = QcDashboardResponseResult.Success,
                    customerCount = _customerCount,
                    factoryCount = _factoryCount,
                    inspectionCount = _inspectionCount,
                    reportCount = _reportCount,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Get the Qc Rejection Details
        /// </summary>
        ///<param name="request"></param>
        /// <returns></returns>
        public async Task<QcRejectionReportsResponse> GetQcRejectionDetails(QcDashboardSearchRequest request)
        {

            if (request.serviceDateFrom?.ToDateTime() == null && request.serviceDateTo?.ToDateTime() == null)
            {
                return new QcRejectionReportsResponse() { Result = QcDashboardResponseResult.NotFound };
            }

            var _qcId = _ApplicationContext.StaffId;

            var data = await _repo.GetScheduelQcDetails(_qcId, request.serviceDateFrom.ToDateTime(), request.serviceDateTo.ToDateTime());

            var _inspectedBookingCount = data.Select(x => x.BookingId).Distinct().Count();

            var _rejectionCount = await _repo.GetAllRejectionReport(request.serviceDateFrom.ToDateTime(), request.serviceDateTo.ToDateTime());

            var _rejectionQcCount = await _repo.GetQcRejectionReport(_qcId, request.serviceDateFrom.ToDateTime(), request.serviceDateTo.ToDateTime());

            var _rejectionPercentage = _inspectedBookingCount != 0 ? Math.Round((double)(100 * _rejectionCount) / _inspectedBookingCount) : 0;

            var _qcRejectionPercentage = _rejectionCount != 0 ? Math.Round((double)(100 * _rejectionQcCount) / _rejectionCount, 1) : 0;


            try
            {
                return new QcRejectionReportsResponse()
                {
                    Result = QcDashboardResponseResult.Success,
                    InspectedBooking = _inspectedBookingCount,
                    RejectionBooking = _rejectionCount,
                    QcRejectionBooking = _rejectionQcCount,
                    RejectionPercentage = _rejectionPercentage,
                    QcRejectionPercentage = _qcRejectionPercentage
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Get the Qc Productivity Details
        /// </summary>
        ///<param name="request"></param>
        /// <returns></returns>
        public async Task<QcDashboardReportsResponse> GetQcProductivityDetails(QcDashboardSearchRequest request)
        {
            var _qcId = _ApplicationContext.StaffId;
            if (request.serviceDateFrom?.ToDateTime() == null && request.serviceDateTo?.ToDateTime() == null)
            {
                return new QcDashboardReportsResponse() { Result = QcDashboardResponseResult.NotFound };
            }

            var data = await _repo.GetScheduelQcDetails(_qcId, request.serviceDateFrom.ToDateTime(), request.serviceDateTo.ToDateTime());

            var reports = await _repo.GetQcReportsDetails(_qcId, data.Select(x => x.BookingId).Distinct().ToList());

            var _resultData =
               (from i in reports
                group i by i.ServiceDateTo into g
                select new QcReportscount()
                {
                    ServiceDate = g.Key,
                    ReportCount = g.Count()
                }).OrderBy(x => x.ServiceDate).ToList();
            try
            {
                return new QcDashboardReportsResponse()
                {
                    Result = QcDashboardResponseResult.Success,
                    QcReportscount = _resultData
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Get the Qc Calendar Days
        /// </summary>
        ///<param name="_fromDate"></param>
        ///<param name="_toDate"></param>
        /// <returns></returns>
        private List<DateTime> getCalendarDays(DateTime _fromDate, DateTime _toDate)
        {
            List<DateTime> _calendarDays = new List<DateTime>();

            int i = 0;
            while (i < 9)
            {
                _calendarDays.Add(_toDate.AddDays(-i));
                if (_fromDate >= _toDate.AddDays(-i))
                    break;
                i++;
            }
            return _calendarDays.OrderBy(x => x.Date).ToList();
        }

    }
}