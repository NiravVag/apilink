using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Reviewer")]
    public partial class FbReportReviewer
    {
        public int Id { get; set; }
        public int? ReviewerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int FbReportId { get; set; }

        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportReviewers")]
        public virtual FbReportDetail FbReport { get; set; }
    }
}