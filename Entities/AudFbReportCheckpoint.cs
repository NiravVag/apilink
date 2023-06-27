using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Aud_FB_Report_Checkpoints")]
    public partial class AudFbReportCheckpoint
    {
        public int Id { get; set; }
        public int? AuditId { get; set; }
        [StringLength(1000)]
        public string ChekPointName { get; set; }
        [StringLength(100)]
        public string ScoreValue { get; set; }
        [StringLength(100)]
        public string ScorePercentage { get; set; }
        [StringLength(100)]
        public string Grade { get; set; }
        [StringLength(4000)]
        public string Remarks { get; set; }
        [StringLength(500)]
        public string Major { get; set; }
        [StringLength(500)]
        public string Minor { get; set; }
        [StringLength(500)]
        public string ZeroTolerance { get; set; }
        [StringLength(500)]
        public string MaxPoint { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        [StringLength(500)]
        public string Critical { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudFbReportCheckpoints")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudFbReportCheckpointCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudFbReportCheckpointDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}