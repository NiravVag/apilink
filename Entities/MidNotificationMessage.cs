using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Notification_Message")]
    public partial class MidNotificationMessage
    {
        public MidNotificationMessage()
        {
            MidNotifications = new HashSet<MidNotification>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("Message")]
        public virtual ICollection<MidNotification> MidNotifications { get; set; }
    }
}