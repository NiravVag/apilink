using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_REF_Report_In_Email")]
    public partial class EsRefReportInEmail
    {
        public EsRefReportInEmail()
        {
            EsDetails = new HashSet<EsDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsRefReportInEmails")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [InverseProperty("ReportInEmailNavigation")]
        public virtual ICollection<EsDetail> EsDetails { get; set; }
    }
}