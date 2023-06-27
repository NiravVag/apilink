using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_Status")]
    public partial class InspStatus
    {
        public InspStatus()
        {
            InspBookingEmailConfigurations = new HashSet<InspBookingEmailConfiguration>();
            InspCuStatuses = new HashSet<InspCuStatus>();
            InspTranStatusLogs = new HashSet<InspTranStatusLog>();
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Status { get; set; }
        public bool? Active { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }
        public int Priority { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("InspStatuses")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("BookingStatus")]
        public virtual ICollection<InspBookingEmailConfiguration> InspBookingEmailConfigurations { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<InspCuStatus> InspCuStatuses { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<InspTranStatusLog> InspTranStatusLogs { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}