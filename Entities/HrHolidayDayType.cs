using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_HolidayDayType")]
    public partial class HrHolidayDayType
    {
        public HrHolidayDayType()
        {
            HrHolidayEndDateTypeNavigations = new HashSet<HrHoliday>();
            HrHolidayStartDateTypeNavigations = new HashSet<HrHoliday>();
            HrLeaveIdTypeEndDateNavigations = new HashSet<HrLeave>();
            HrLeaveIdTypeStartDateNavigations = new HashSet<HrLeave>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Label { get; set; }
        public int? TypeTransId { get; set; }

        [InverseProperty("EndDateTypeNavigation")]
        public virtual ICollection<HrHoliday> HrHolidayEndDateTypeNavigations { get; set; }
        [InverseProperty("StartDateTypeNavigation")]
        public virtual ICollection<HrHoliday> HrHolidayStartDateTypeNavigations { get; set; }
        [InverseProperty("IdTypeEndDateNavigation")]
        public virtual ICollection<HrLeave> HrLeaveIdTypeEndDateNavigations { get; set; }
        [InverseProperty("IdTypeStartDateNavigation")]
        public virtual ICollection<HrLeave> HrLeaveIdTypeStartDateNavigations { get; set; }
    }
}