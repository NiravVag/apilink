using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DA_UserRoleNotificationByOffice")]
    public partial class DaUserRoleNotificationByOffice
    {
        public int Id { get; set; }
        [Column("DAUserCustomerId")]
        public int DauserCustomerId { get; set; }
        public int? OfficeId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("DaUserRoleNotificationByOffices")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DauserCustomerId")]
        [InverseProperty("DaUserRoleNotificationByOffices")]
        public virtual DaUserCustomer DauserCustomer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DaUserRoleNotificationByOffices")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("DaUserRoleNotificationByOffices")]
        public virtual RefLocation Office { get; set; }
    }
}