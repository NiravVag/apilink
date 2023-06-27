using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SCH_Schedule_QC")]
    public partial class SchScheduleQc
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        [Column("QCId")]
        public int Qcid { get; set; }
        [Column("QCType")]
        public int Qctype { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ServiceDate { get; set; }
        public double ActualManDay { get; set; }
        [Column("QC_Leader")]
        public bool? QcLeader { get; set; }
        [Column("IsVisibleToQC")]
        public bool? IsVisibleToQc { get; set; }
        [Column("KeepQCForTravelExpense")]
        public bool? KeepQcforTravelExpense { get; set; }
        [Column("IsReportFilledQC")]
        public bool? IsReportFilledQc { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("SchScheduleQcs")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("SchScheduleQcCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SchScheduleQcDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("SchScheduleQcModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("Qcid")]
        [InverseProperty("SchScheduleQcs")]
        public virtual HrStaff Qc { get; set; }
        [ForeignKey("Qctype")]
        [InverseProperty("SchScheduleQcs")]
        public virtual SchQctype QctypeNavigation { get; set; }
    }
}