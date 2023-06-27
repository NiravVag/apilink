using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface INotificationRepository : IRepository
    {
        /// <summary>
        /// Get notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<MidNotification>> GetNotifications(int userId);
    }
}
