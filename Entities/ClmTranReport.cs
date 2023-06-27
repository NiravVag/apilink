using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_TRAN_Reports")]
    public partial class ClmTranReport
    {
        public int Id { get; set; }
        public int? ClaimId { get; set; }
        public int? ReportId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("ClaimId")]
        [InverseProperty("ClmTranReports")]
        public virtual ClmTransaction Claim { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ClmTranReportCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("ClmTranReportDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ReportId")]
        [InverseProperty("ClmTranReports")]
        public virtual FbReportDetail Report { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("ClmTranReportUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}