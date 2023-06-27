using DTO.BookingRuleContact;
using DTO.DataAccess;
using DTO.Inspection;
using System.Threading.Tasks;

namespace Contracts.Managers
{
	public interface IInspBookingRuleContactManager
	{
		///// <summary>
		///// Get Booking Contact Information
		///// </summary>
		///// <param name="factoryId"></param>
		///// <param name=""></param>
		///// <returns>InspBookingContactResponse</returns>
		//Task<InspBookingContactResponse> GetInspBookingContactInformation(int factoryId, int officeId);

        /// <summary>
        /// GetBookingContactInformation
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<BookingContactResponse> GetBookingContactInformation(int factoryId, int customerId);

        /// <summary>
        /// Get Booking Rule
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="factoryId"></param>
        /// <returns>InspBookingRuleResponse</returns>
        Task<InspBookingRuleResponse> GetInspBookingRules(int customerId, int? factoryId);
        /// <summary>
        /// Get Holiday List(public holiday+weekends)
        /// </summary>
        /// <param name="factoryCountryId"></param>
        /// <param name="factoryId"></param>
        /// <returns>InspBookingRuleResponse</returns>
        Task<InspectionHolidaySummaryList> GetInspBookingHolidaysDate(int factoryCountryId, int factoryId);
        /// <summary>
        /// GetInspBookingContactInformation
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="officeId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<InspBookingContactResponse> GetInspBookingContactInformation(UserAccess userAccessFilter);
    }
}
