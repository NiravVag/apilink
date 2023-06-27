using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_NotificationType")]
    public partial class MidNotificationType
    {
        public MidNotificationType()
        {
            MidNotifications = new HashSet<MidNotification>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Label { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("MidNotificationTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("NotifType")]
        public virtual ICollection<MidNotification> MidNotifications { get; set; }
    }
}