using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Report_CustomerDecisionComment")]
    public partial class CuReportCustomerDecisionComment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [StringLength(1000)]
        public string ReportResult { get; set; }
        public string Comments { get; set; }
        public bool Active { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuReportCustomerDecisionComments")]
        public virtual CuCustomer Customer { get; set; }
    }
}