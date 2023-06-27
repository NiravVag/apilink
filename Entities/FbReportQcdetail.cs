using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_QCDetails")]
    public partial class FbReportQcdetail
    {
        public int Id { get; set; }
        public int FbReportDetailId { get; set; }
        public int? QcId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("FbReportDetailId")]
        [InverseProperty("FbReportQcdetails")]
        public virtual FbReportDetail FbReportDetail { get; set; }
    }
}