using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface INotificationManager
    {
        Task AddNotification(NotificationType notificationType, int bookingId, IEnumerable<int> idUserList, string message = null, int? notificationMessage = null);
    }
}
