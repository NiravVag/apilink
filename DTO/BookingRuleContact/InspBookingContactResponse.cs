using System.Collections.Generic;

namespace DTO.BookingRuleContact
{
    public class InspBookingContactResponse
	{
        //public IEnumerable<InspBookingContacts> BookingContactList { get; set; }
        public IEnumerable<DTO.User.UserStaffDetails> BookingContactList { get; set; }
        public BookingContactResult Result { get; set; }
    }

    public class BookingContactResponse
    {
        public BookingContactModel BookingContact { get; set; }
        //public DTO.User.User BookingContact { get; set; }
        public BookingContactResult Result { get; set; }
    }

    public enum BookingContactResult
	{
		GetContactDetailsSuccess = 1,
		CannotGetOfficeList = 2,
		CannotGetContactDetails = 3
	}
}
