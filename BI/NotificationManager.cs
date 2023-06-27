using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.User;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly UserMap UserMap = null;
        private readonly ITenantProvider _filterService = null;

        public NotificationManager(INotificationRepository repo, IAPIUserContext applicationContext, ITenantProvider filterService)
        {
            _repo = repo;
            _ApplicationContext = applicationContext;
            UserMap = new UserMap();
            _filterService = filterService;
        }

        //Create Notification
        public async Task AddNotification(NotificationType notificationType, int bookingId, IEnumerable<int> idUserList, string message = null, int? notificationMessage = null)
        {
            if (idUserList != null && idUserList.Any())
            {
                foreach (int idUser in idUserList)
                {
                    // Add new notification for user request
                    var notification = new MidNotification
                    {
                        Id = Guid.NewGuid(),
                        IsRead = false,
                        LinkId = bookingId,
                        UserId = idUser,
                        NotifTypeId = (int)notificationType,
                        CreatedOn = DateTime.Now,
                        EntityId = _filterService.GetCompanyId(),
                        MessageId = notificationMessage,
                        NotificationMessage = message
                    };

                    _repo.AddEntity(notification);
                }

                //Save
                await _repo.Save();

            }
        }

        public async Task<NotificationResponse> GetNotifications()
        {
            var notifications = await _repo.GetNotifications(_ApplicationContext.UserId);

            if (notifications == null || !notifications.Any())
                return new NotificationResponse { Result = NotificationResult.NotFound };

            return new NotificationResponse
            {
                Data = notifications.Select(UserMap.GetNotification),
                Result = NotificationResult.Success
            };
        }
    }
}
