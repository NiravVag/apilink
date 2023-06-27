using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SCH_Schedule_QC_Report")]
    public partial class SchScheduleQcReport
    {
        public int Id { get; set; }
        public int PoProductId { get; set; }
        [Column("QC")]
        public int Qc { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SchScheduleQcReportCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SchScheduleQcReportDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("PoProductId")]
        [InverseProperty("SchScheduleQcReports")]
        public virtual InspPoTransaction PoProduct { get; set; }
        [ForeignKey("Qc")]
        [InverseProperty("SchScheduleQcReports")]
        public virtual HrStaff QcNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("SchScheduleQcReportUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}