using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IEmailLogQueueRepository
    {
        Task<int> AddEmailLog(LogEmailQueue entity);
        Task<int> EditEmailLog(LogEmailQueue entity);
        Task<LogEmailQueue> GetEmailLogById(int emailLogId);
    }
}
