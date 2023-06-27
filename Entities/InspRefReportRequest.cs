using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_ReportRequest")]
    public partial class InspRefReportRequest
    {
        public InspRefReportRequest()
        {
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("ReportRequestNavigation")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}