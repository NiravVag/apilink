using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_Report")]
    public partial class AudTranReport
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string UniqueId { get; set; }
        public int AuditId { get; set; }
        [StringLength(500)]
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool Active { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranReports")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTranReportCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudTranReportDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}