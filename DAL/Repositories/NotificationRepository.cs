using Contracts.Repositories;
using DTO.Common;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class NotificationRepository : Repository, INotificationRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public NotificationRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        public async Task<IEnumerable<MidNotification>> GetNotifications(int userId)
        {
            return await _context.MidNotifications.Where(x => x.UserId == userId && !x.IsRead).ToListAsync();
        }      
    }
}
