using DTO.EmailLog;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IEmailLogQueueManager
    {
        Task<int> AddEmailLog(EmailLogData request);
        Task<int> UpdateEmailLog(LogEmailQueue request);
        Task<LogEmailQueue> GetEmailLogById(int emailLogId);
    }
}
