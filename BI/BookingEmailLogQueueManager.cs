using AutoMapper.Configuration;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.EmailLog;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class BookingEmailLogQueueManager : IBookingEmailLogQueueManager
    {
        private readonly IBookingFbQueueRepository _repo = null;
        private readonly ITenantProvider _tenantProvider = null;
        private readonly IAPIUserContext _apiUserContext;        

        public BookingEmailLogQueueManager(IBookingFbQueueRepository repo, ITenantProvider tenantProvider, IAPIUserContext apiUserContext)
        {
            _repo = repo;
            _tenantProvider = tenantProvider;
            _apiUserContext = apiUserContext;
        }
        /// <summary>
        /// Add  Booking Email Log Queue
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> AddBookingFbQueueLog(BookingFbLogData request)
        {
            var entityId = _tenantProvider.GetCompanyId();
            var logBookingFbQueue = new LogBookingFbQueue()
            {
                BookingId = request.BookingId,
                FbBookingSyncType = (int)request.FbBookingSyncType,                
                Active = true,
                CreatedOn = DateTime.Now,
                TryCount = request.TryCount,
                EntityId = entityId,
                CreatedBy = _apiUserContext.UserId,
                Status = 1,
                IsMissionUpdated = request.IsMissionUpdated
            };

            _repo.AddEntity(logBookingFbQueue);
            await _repo.Save();
            return logBookingFbQueue.Id;
        }

        /// <summary>
        /// Update Email Log Queue
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task UpdateBookingFbQueueLog(LogBookingFbQueue request)
        {
            _repo.EditEntity(request);
            await _repo.Save();
        }

        /// <summary>
        /// Get Email Log Queue
        /// </summary>
        /// <param name="emailLogId"></param>
        /// <returns></returns>

        public async Task<LogBookingFbQueue> GetBookingFbQueueLogById(int emailLogId)
        {
            return await _repo.GetBookingFbLogById(emailLogId);
        }

    }
}
