using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REP_CUS_Decision")]
    public partial class InspRepCusDecision
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int CustomerResultId { get; set; }
        public string Comments { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public bool? IsAutoCustomerDecision { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspRepCusDecisions")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerResultId")]
        [InverseProperty("InspRepCusDecisions")]
        public virtual RefInspCusDecisionConfig CustomerResult { get; set; }
        [ForeignKey("ReportId")]
        [InverseProperty("InspRepCusDecisions")]
        public virtual FbReportDetail Report { get; set; }
    }
}