using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_FabricControlmadeWith")]
    public partial class FbReportFabricControlmadeWith
    {
        public int Id { get; set; }
        public int? ReportDetailsId { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("ReportDetailsId")]
        [InverseProperty("FbReportFabricControlmadeWiths")]
        public virtual FbReportDetail ReportDetails { get; set; }
    }
}