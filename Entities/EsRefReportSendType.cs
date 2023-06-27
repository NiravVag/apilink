using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_REF_Report_Send_Type")]
    public partial class EsRefReportSendType
    {
        public EsRefReportSendType()
        {
            EsDetails = new HashSet<EsDetail>();
            EsEmailReportTypeMaps = new HashSet<EsEmailReportTypeMap>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsRefReportSendTypes")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EsRefReportSendTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("ReportSendTypeNavigation")]
        public virtual ICollection<EsDetail> EsDetails { get; set; }
        [InverseProperty("ReportTypeNavigation")]
        public virtual ICollection<EsEmailReportTypeMap> EsEmailReportTypeMaps { get; set; }
    }
}