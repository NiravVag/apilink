using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REP_FAST_TRAN_Log")]
    public partial class RepFastTranLog
    {
        public int Id { get; set; }
        public int? FastTranId { get; set; }
        public int? ReportId { get; set; }
        public int? Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public string ErrorLog { get; set; }

        [ForeignKey("FastTranId")]
        [InverseProperty("RepFastTranLogs")]
        public virtual RepFastTransaction FastTran { get; set; }
        [ForeignKey("ReportId")]
        [InverseProperty("RepFastTranLogs")]
        public virtual FbReportDetail Report { get; set; }
        [ForeignKey("Status")]
        [InverseProperty("RepFastTranLogs")]
        public virtual RepFastRefStatus StatusNavigation { get; set; }
    }
}