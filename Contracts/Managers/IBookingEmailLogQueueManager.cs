using DTO.EmailLog;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IBookingEmailLogQueueManager
    {
        Task<int> AddBookingFbQueueLog(BookingFbLogData request);
        Task UpdateBookingFbQueueLog(LogBookingFbQueue request);
        Task<LogBookingFbQueue> GetBookingFbQueueLogById(int emailLogId);

    }
}