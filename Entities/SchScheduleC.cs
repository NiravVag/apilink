using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SCH_Schedule_CS")]
    public partial class SchScheduleC
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        [Column("CSId")]
        public int Csid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ServiceDate { get; set; }
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
        [Column("IsReportReviewCS")]
        public bool? IsReportReviewCs { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("SchScheduleCS")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("SchScheduleCCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("Csid")]
        [InverseProperty("SchScheduleCS")]
        public virtual HrStaff Cs { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SchScheduleCDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("SchScheduleCModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
    }
}