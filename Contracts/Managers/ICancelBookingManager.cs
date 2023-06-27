using DTO.CancelBooking;
using DTO.Eaqf;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICancelBookingManager
    {
        /// <summary>
        /// Get Currency
        /// </summary>
        /// <returns>List Of Currency</returns>
        Task<CurrencyResponse> GetCurrency();
        /// <summary>
        /// Get Booking Details in Cancel page by Booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="bookingStatus"></param>
        /// <returns>Get Booking Detail </returns>
        Task<EditCancelBookingResponse> GetCancelBookingDetails(int bookingId, int bookingStatus);
        /// <summary>
        /// Get Response by Booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Get Response list </returns>
        Task<BookingCancelRescheduleResponse> GetReason(int bookingId);
        /// <summary>
        /// Insert the cancel Booking and status update in booking table
        /// </summary>
        /// <param name="request"></param>
        /// <returns> Insert Response  </returns>
        Task<SaveCancelBookingResponse> SaveBookingCancelDetails(SaveCancelBookingRequest request, string fbToken);
        /// <summary>
        /// Get Response by Booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Get Response list </returns>
        Task<BookingCancelRescheduleResponse> GetRescheduleReason(int bookingId);
        /// <summary>
        /// Insert the Reschedule Booking and status update in booking table
        /// </summary>
        /// <param name="request"></param>
        /// <returns> Insert Response  </returns>
        Task<SaveRescheduleResponse> SaveRescheduleDetails(SaveRescheduleRequest request, string fbToken);
        /// <summary>
        /// Get Cancel Details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns> Get Cancel Details </returns>
        Task<SaveCancelBookingRequest> GetCancelDetails(int bookingId);
		/// <summary>
		/// Get Reschedule Details
		/// </summary>
		/// <param name="bookingId"></param>
		/// <returns> Get Reschedule Details </returns>
		Task<SaveRescheduleRequest> GetRescheduleDetails(int bookingId);

    }
}
