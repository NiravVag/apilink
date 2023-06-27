using DTO.Common;
using System;
using System.Collections.Generic;

namespace DTO.BookingRuleContact
{
    public class InspBookingRules
	{
		public int Id { get; set; }

		public int LeadDays { get; set; }

		public string BookingRule { get; set; }

		public int? FactoryCountryId { get; set; }

		public string ServiceLeadDaysMessage { get; set; }

		public IEnumerable<DateObject> Holidays { get; set; }
        public IEnumerable<DateTime> HolidaysDate { get; set; }
    }
}
