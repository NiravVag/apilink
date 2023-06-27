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
    public class BookingFbQueueRepository : Repository, IBookingFbQueueRepository
    {
        public BookingFbQueueRepository(API_DBContext dBContext) : base(dBContext)
        {

        }

        public async Task<LogBookingFbQueue> GetBookingFbLogById(int emailLogId)
        {
            return await _context.LogBookingFbQueues.FirstOrDefaultAsync(x => x.Id == emailLogId);
        }
    }
}
