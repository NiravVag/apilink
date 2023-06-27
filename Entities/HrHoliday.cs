using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Holiday")]
    public partial class HrHoliday
    {
        public int Id { get; set; }
        [Column("Country_Id")]
        public int CountryId { get; set; }
        [Required]
        [Column("Holiday_Name")]
        [StringLength(200)]
        public string HolidayName { get; set; }
        [Column("location_id")]
        public int? LocationId { get; set; }
        [Column("recurrence_type")]
        public int RecurrenceType { get; set; }
        [Column("start_date", TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column("end_date", TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        public int? EntityId { get; set; }
        public int? HolidayId { get; set; }
        public int? StartDateType { get; set; }
        public int? EndDateType { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("HrHolidays")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("EndDateType")]
        [InverseProperty("HrHolidayEndDateTypeNavigations")]
        public virtual HrHolidayDayType EndDateTypeNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("HrHolidays")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("HrHolidays")]
        public virtual RefLocation Location { get; set; }
        [ForeignKey("StartDateType")]
        [InverseProperty("HrHolidayStartDateTypeNavigations")]
        public virtual HrHolidayDayType StartDateTypeNavigation { get; set; }
    }
}