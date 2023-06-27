using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Status")]
    public partial class FbReportStatus
    {
        public FbReportStatus()
        {
            InspPoTransactionFbFillingStatusNavigations = new HashSet<InspPoTransaction>();
            InspPoTransactionFbReportStatusNavigations = new HashSet<InspPoTransaction>();
            InspPoTransactionFbReviewStatusNavigations = new HashSet<InspPoTransaction>();
        }

        public int Id { get; set; }
        public int Type { get; set; }
        [StringLength(200)]
        public string StatusName { get; set; }
        [Column("FBName")]
        [StringLength(200)]
        public string Fbname { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("Type")]
        [InverseProperty("FbReportStatuses")]
        public virtual FbStatusType TypeNavigation { get; set; }
        [InverseProperty("FbFillingStatusNavigation")]
        public virtual ICollection<InspPoTransaction> InspPoTransactionFbFillingStatusNavigations { get; set; }
        [InverseProperty("FbReportStatusNavigation")]
        public virtual ICollection<InspPoTransaction> InspPoTransactionFbReportStatusNavigations { get; set; }
        [InverseProperty("FbReviewStatusNavigation")]
        public virtual ICollection<InspPoTransaction> InspPoTransactionFbReviewStatusNavigations { get; set; }
    }
}