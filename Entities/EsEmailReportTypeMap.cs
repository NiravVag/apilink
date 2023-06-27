using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_Email_Report_Type_Map")]
    public partial class EsEmailReportTypeMap
    {
        public int Id { get; set; }
        public int EmailType { get; set; }
        public int ReportType { get; set; }
        public bool Active { get; set; }

        [ForeignKey("EmailType")]
        [InverseProperty("EsEmailReportTypeMaps")]
        public virtual EsType EmailTypeNavigation { get; set; }
        [ForeignKey("ReportType")]
        [InverseProperty("EsEmailReportTypeMaps")]
        public virtual EsRefReportSendType ReportTypeNavigation { get; set; }
    }
}