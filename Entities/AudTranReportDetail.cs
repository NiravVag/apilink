using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_Report_Details")]
    public partial class AudTranReportDetail
    {
        public int Id { get; set; }
        [Column("Audit_Id")]
        public int AuditId { get; set; }
        [Column("ServiceDate_From", TypeName = "datetime")]
        public DateTime ServiceDateFrom { get; set; }
        [Column("ServiceDate_To", TypeName = "datetime")]
        public DateTime ServiceDateTo { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
        [StringLength(2000)]
        public string Comments { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranReportDetails")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("AudTranReportDetails")]
        public virtual ItUserMaster User { get; set; }
    }
}