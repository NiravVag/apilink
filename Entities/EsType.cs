using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_Type")]
    public partial class EsType
    {
        public EsType()
        {
            EsDetails = new HashSet<EsDetail>();
            EsEmailReportTypeMaps = new HashSet<EsEmailReportTypeMap>();
            EsRuleRecipientEmailTypeMaps = new HashSet<EsRuleRecipientEmailTypeMap>();
            EsSuTemplateMasters = new HashSet<EsSuTemplateMaster>();
            LogBookingReportEmailQueues = new HashSet<LogBookingReportEmailQueue>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }

        [InverseProperty("Type")]
        public virtual ICollection<EsDetail> EsDetails { get; set; }
        [InverseProperty("EmailTypeNavigation")]
        public virtual ICollection<EsEmailReportTypeMap> EsEmailReportTypeMaps { get; set; }
        [InverseProperty("EmailType")]
        public virtual ICollection<EsRuleRecipientEmailTypeMap> EsRuleRecipientEmailTypeMaps { get; set; }
        [InverseProperty("EmailType")]
        public virtual ICollection<EsSuTemplateMaster> EsSuTemplateMasters { get; set; }
        [InverseProperty("EsType")]
        public virtual ICollection<LogBookingReportEmailQueue> LogBookingReportEmailQueues { get; set; }
    }
}