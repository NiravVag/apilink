using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Notification")]
    public partial class MidNotification
    {
        public Guid Id { get; set; }
        public int NotifTypeId { get; set; }
        public int UserId { get; set; }
        public int LinkId { get; set; }
        public bool IsRead { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? EntityId { get; set; }
        public int? MessageId { get; set; }
        [StringLength(1000)]
        public string NotificationMessage { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("MidNotifications")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("MessageId")]
        [InverseProperty("MidNotifications")]
        public virtual MidNotificationMessage Message { get; set; }
        [ForeignKey("NotifTypeId")]
        [InverseProperty("MidNotifications")]
        public virtual MidNotificationType NotifType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("MidNotifications")]
        public virtual ItUserMaster User { get; set; }
    }
}