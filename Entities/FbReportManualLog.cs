using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Manual_Log")]
    public partial class FbReportManualLog
    {
        public int Id { get; set; }
        public int? FbReportId { get; set; }
        public string FileUrl { get; set; }
        [StringLength(1000)]
        public string UniqueId { get; set; }
        [Required]
        [StringLength(500)]
        public string FileName { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("FbReportManualLogCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("FbReportManualLogDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("FbReportManualLogs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportManualLogs")]
        public virtual FbReportDetail FbReport { get; set; }
    }
}