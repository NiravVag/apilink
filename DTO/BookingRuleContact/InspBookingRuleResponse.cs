using System.Collections.Generic;

namespace DTO.BookingRuleContact
{
    public class InspBookingRuleResponse
	{
        public InspBookingRules BookingRuleList { get; set; }
        public BookingRuleResult Result { get; set; }
    }
    public enum BookingRuleResult
	{
        Success = 1,
        BookingRuleNotFound = 2
    }
}
