using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EmailLogQueueRepository : Repository, IEmailLogQueueRepository
    {
        public EmailLogQueueRepository(API_DBContext context) : base(context)
        {
        }
        /// <summary>
        /// Adding email log 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddEmailLog(LogEmailQueue entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        /// <summary>
        /// Update email log
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> EditEmailLog(LogEmailQueue entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        /// <summary>
        /// Get Email Log
        /// </summary>
        /// <param name="logEmailId"></param>
        /// <returns></returns>
        public async Task<LogEmailQueue> GetEmailLogById(int emailLogId)
        {
            return await _context.LogEmailQueues.
                           Include(x => x.LogEmailQueueAttachments).
                           Include(x => x.LogBookingReportEmailQueues).
                               Where(x => x.Id == emailLogId && x.Active)
                               .FirstOrDefaultAsync();
        }
    }
}
