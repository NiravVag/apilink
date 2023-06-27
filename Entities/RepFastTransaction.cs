using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REP_FAST_Transaction")]
    public partial class RepFastTransaction
    {
        public RepFastTransaction()
        {
            RepFastTranLogs = new HashSet<RepFastTranLog>();
        }

        public int Id { get; set; }
        public int? ReportId { get; set; }
        public int? BookingId { get; set; }
        public int? Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? TryCount { get; set; }
        [Column("IT_Notification")]
        public bool? ItNotification { get; set; }
        [StringLength(2000)]
        public string ReportLink { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("RepFastTransactions")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("ReportId")]
        [InverseProperty("RepFastTransactions")]
        public virtual FbReportDetail Report { get; set; }
        [ForeignKey("Status")]
        [InverseProperty("RepFastTransactions")]
        public virtual RepFastRefStatus StatusNavigation { get; set; }
        [InverseProperty("FastTran")]
        public virtual ICollection<RepFastTranLog> RepFastTranLogs { get; set; }
    }
}