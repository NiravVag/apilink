using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class NotificationResponse
    {
        public IEnumerable<NotificationModel> Data { get; set;  }
        public NotificationResult Result { get; set;  }
    }

    public class NotificationModel
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }

        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public int LinkId { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsRead { get; set; }
        public string TypeLabel { get; set; }
        public NotificationDateType DateType { get; set; }
    }

    public class NotificationSearchRequest
    {
        public int Skip { get; set; }
        public bool? IsUnread { get; set; }
        public NotificationFilterType? NotificationType { get; set; }
    }

    public enum NotificationResult
    {
        Success = 1,
        NotFound = 2
    }

    public enum NotificationDateType
    {
        Today = 1,
        Yesterday = 2,
        Older = 3
    }

    public enum NotificationFilterType
    {
        Leave = 1,
        Expense = 2,
        Inspection = 3,
        Quotation = 4,
        Booking = 5,
        Report = 6
    }
}
