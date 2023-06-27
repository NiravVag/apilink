using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Status")]
    public partial class FbStatus
    {
        public FbStatus()
        {
            AudTransactionFbfillingStatusNavigations = new HashSet<AudTransaction>();
            AudTransactionFbreportStatusNavigations = new HashSet<AudTransaction>();
            AudTransactionFbreviewStatusNavigations = new HashSet<AudTransaction>();
            AudTransactionFillingStatusNavigations = new HashSet<AudTransaction>();
            FbReportDetailFbFillingStatusNavigations = new HashSet<FbReportDetail>();
            FbReportDetailFbReportStatusNavigations = new HashSet<FbReportDetail>();
            FbReportDetailFbReviewStatusNavigations = new HashSet<FbReportDetail>();
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        public int Type { get; set; }
        [StringLength(200)]
        public string StatusName { get; set; }
        [Column("FBStatusName")]
        [StringLength(200)]
        public string FbstatusName { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("Type")]
        [InverseProperty("FbStatuses")]
        public virtual FbStatusType TypeNavigation { get; set; }
        [InverseProperty("FbfillingStatusNavigation")]
        public virtual ICollection<AudTransaction> AudTransactionFbfillingStatusNavigations { get; set; }
        [InverseProperty("FbreportStatusNavigation")]
        public virtual ICollection<AudTransaction> AudTransactionFbreportStatusNavigations { get; set; }
        [InverseProperty("FbreviewStatusNavigation")]
        public virtual ICollection<AudTransaction> AudTransactionFbreviewStatusNavigations { get; set; }
        [InverseProperty("FillingStatusNavigation")]
        public virtual ICollection<AudTransaction> AudTransactionFillingStatusNavigations { get; set; }
        [InverseProperty("FbFillingStatusNavigation")]
        public virtual ICollection<FbReportDetail> FbReportDetailFbFillingStatusNavigations { get; set; }
        [InverseProperty("FbReportStatusNavigation")]
        public virtual ICollection<FbReportDetail> FbReportDetailFbReportStatusNavigations { get; set; }
        [InverseProperty("FbReviewStatusNavigation")]
        public virtual ICollection<FbReportDetail> FbReportDetailFbReviewStatusNavigations { get; set; }
        [InverseProperty("FbMissionStatusNavigation")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}