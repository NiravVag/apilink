using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_Status")]
    public partial class AudStatus
    {
        public AudStatus()
        {
            AudBookingEmailConfigurations = new HashSet<AudBookingEmailConfiguration>();
            AudTranStatusLogs = new HashSet<AudTranStatusLog>();
            AudTransactions = new HashSet<AudTransaction>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Status { get; set; }
        public bool? Active { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("AudStatuses")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("AuditStatus")]
        public virtual ICollection<AudBookingEmailConfiguration> AudBookingEmailConfigurations { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<AudTranStatusLog> AudTranStatusLogs { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
    }
}