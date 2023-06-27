using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Leave")]
    public partial class HrLeave
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        [Column("LeaveType_Id")]
        public int LeaveTypeId { get; set; }
        [Column("Date_Begin", TypeName = "datetime")]
        public DateTime DateBegin { get; set; }
        [Column("Date_End", TypeName = "datetime")]
        public DateTime DateEnd { get; set; }
        [Column("Number_Of_Days")]
        public double NumberOfDays { get; set; }
        [StringLength(200)]
        public string Comments { get; set; }
        [Column("Checked_Id")]
        public int? CheckedId { get; set; }
        [StringLength(100)]
        public string Comments1 { get; set; }
        [Column("Approved_Id")]
        public int? ApprovedId { get; set; }
        [StringLength(100)]
        public string Comments2 { get; set; }
        public int? Status { get; set; }
        [Column("Approved_Leave_days")]
        public double? ApprovedLeaveDays { get; set; }
        [Column("Leave_application_date", TypeName = "datetime")]
        public DateTime? LeaveApplicationDate { get; set; }
        public int IdTypeStartDate { get; set; }
        public int IdTypeEndDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CancelledOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RejectedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("ApprovedId")]
        [InverseProperty("HrLeaveApproveds")]
        public virtual ItUserMaster Approved { get; set; }
        [ForeignKey("CheckedId")]
        [InverseProperty("HrLeaveCheckeds")]
        public virtual ItUserMaster Checked { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("HrLeaves")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("IdTypeEndDate")]
        [InverseProperty("HrLeaveIdTypeEndDateNavigations")]
        public virtual HrHolidayDayType IdTypeEndDateNavigation { get; set; }
        [ForeignKey("IdTypeStartDate")]
        [InverseProperty("HrLeaveIdTypeStartDateNavigations")]
        public virtual HrHolidayDayType IdTypeStartDateNavigation { get; set; }
        [ForeignKey("LeaveTypeId")]
        [InverseProperty("HrLeaves")]
        public virtual HrLeaveType LeaveType { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrLeaves")]
        public virtual HrStaff Staff { get; set; }
        [ForeignKey("Status")]
        [InverseProperty("HrLeaves")]
        public virtual HrLeaveStatus StatusNavigation { get; set; }
    }
}