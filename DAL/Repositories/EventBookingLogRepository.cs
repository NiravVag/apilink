using Contracts.Repositories;
using DTO.Common;
using DTO.Inspection;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EventBookingLogRepository : Repository, IEventBookingLogRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public EventBookingLogRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        public async Task<int> SaveLogInformation(EventBookingLog entity)
        {
            _context.EventBookingLogs.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity.Id;
            else
                return 0;
        }

        public async Task<int> SaveEventLogInformation(EventLog entity)
        {
            _context.EventLogs.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity.Id;
            else
                return 0;
        }

        public async Task<int> SaveFbBookingLog(FbBookingRequestLog entity)
        {
            _context.FbBookingRequestLogs.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity.Id;
            else
                return 0;
        }

        /// <summary>
        /// Get Event booking Logs by booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public List<BookingLogStatus> GetLogStatusByBooking(int bookingId)
        {
            return _context.InspTranStatusLogs.Where(y => y.BookingId == bookingId)
            .Select(z => new BookingLogStatus()
            {
                StatusId = z.StatusId,
                BookingId = z.BookingId,
                CreatedDate = z.StatusChangeDate
            }).ToList();
        }

        public List<BookingLogStatus> GetLogStatusByBooking(List<int> bookingIds)
        {
            return _context.InspTranStatusLogs.Where(y => bookingIds.Contains(y.BookingId))
            .Select(z => new BookingLogStatus()
            {
                StatusId = z.StatusId,
                BookingId = z.BookingId,
                CreatedDate = z.StatusChangeDate
            }).ToList();
        }

        public List<BookingLogStatus> GetAuditLogStatusByBooking(List<int> bookingIds)
        {
            return _context.AudTranStatusLogs.Where(y => bookingIds.Contains(y.AuditId))
            .Select(z => new BookingLogStatus()
            {
                StatusId = z.StatusId,
                BookingId = z.AuditId,
                CreatedDate = z.CreatedOn
            }).ToList();
        }

        public async Task<int> SaveQuotationStatusLog(List<QuTranStatusLog> entity)
        {
            foreach (var item in entity)
            {
                _context.QuTranStatusLogs.Add(item);
            }
            if (await _context.SaveChangesAsync() > 0)
                return 1;
            else
                return 0;

        }

        public async Task<int> SaveZohoBookingLog(ZohoRequestLog entity)
        {
            _context.ZohoRequestLogs.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity.Id;
            else
                return 0;
        }

        /// <summary>
        /// Save the TCF Master Data Log
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> SaveTCFMasterRequestLog(TcfMasterDataLog entity)
        {
            _context.TcfMasterDataLogs.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity.Id;
            else
                return 0;
        }

        /// <summary>
        /// Save the Commmon API Request Log (it is generic function but currently using only for tcf system)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> SaveAPIGatewayLog(ApigatewayLog entity)
        {
            _context.ApigatewayLogs.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity.Id;
            else
                return 0;
        }
    }
}
