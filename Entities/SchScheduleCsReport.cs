using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SCH_Schedule_CS_Report")]
    public partial class SchScheduleCsReport
    {
        public int Id { get; set; }
        public int PoProductId { get; set; }
        [Column("CS")]
        public int Cs { get; set; }
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
        [InverseProperty("SchScheduleCsReportCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("Cs")]
        [InverseProperty("SchScheduleCsReports")]
        public virtual HrStaff CsNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SchScheduleCsReportDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("PoProductId")]
        [InverseProperty("SchScheduleCsReports")]
        public virtual InspPoTransaction PoProduct { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("SchScheduleCsReportUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}