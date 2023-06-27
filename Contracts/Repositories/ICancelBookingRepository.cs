using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICancelBookingRepository : IRepository
    {
        /// <summary>
        /// Get Currency
        /// </summary>
        /// <returns>Currency List</returns>
        Task<List<RefCurrency>> GetCurrencies();
        /// <summary>
        /// Get Booking Details in Cancel page by Booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Get Booking Detail </returns>
        Task<InspTransaction> GetCancelBookingDetails(int bookingId);
        /// <summary>
        /// Get reason Type
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Reason Type List</returns>
        Task<List<InspCancelReason>> GetBookingCancelReasons(int? customerId);
        /// <summary>
        /// Get Booking Details for cancel
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Booking Details</returns>
        Task<InspTransaction> GetBookingDetailsById(int bookingId);
        /// <summary>
        /// Save Cancel record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Cancel Id</returns>
        Task<int> SaveCancelDetail(InspTranCancel entity);
        /// <summary>
        /// Get reason Type
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Reason Type List</returns>
        Task<List<InspRescheduleReason>> GetBookingRescheduleReasons(int? customerId);
        /// <summary>
        /// Save Reschedule record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Reschedule Id</returns>
        int SaveReschedule(InspTranReschedule entity);
        /// <summary>
        /// Get Reschedule Details By Booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Reschedule Details</returns>
        Task<InspTranReschedule> GetRescheduleDetailsById(int bookingId);
        /// <summary>
        /// Get Cancel Details By Booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Cancel Details</returns>
        Task<InspTranCancel> GetCancelDetailsById(int bookingId);
        /// <summary>
        /// Update Cancel Details 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Cancel Id</returns>
        Task<int> EditCancel(InspTranCancel entity);
        /// <summary>
        /// Update Reschedule Details 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Reschedule Id</returns>
        Task<int> EditReschedule(InspTranReschedule entity);
        /// <summary>
        /// Quotation exists for booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>returns QuQuotationInspection table record have booking id</returns>
        Task<QuInspProduct> BookingQuotationExists(int bookingId);
        /// <summary>
        /// Update service date from and to in insp_transaction table from reschedule page
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>returns update success</returns>
        Task<int> UpdateBookingStatusServiceDate(InspTransaction entity);
        /// <summary>
        /// Get reason Type
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns>Reason Type List</returns>
        Task<InspCancelReason> GetBookingCancelReasonsById(int reasonId);
        Task<int> GetBookingQuotationDetails(int bookingId);
        Task<QuQuotation> GetQuotationData(int quotationId);
        Task<int> GetQuotationBookingCount(int quotationId);
        Task<AudTranCancelReschedule> GetAuditCancelDetailsById(int bookingId);
        Task<AudTranCancelReschedule> GetAuditRescheduleDetailsById(int bookingId);
    }
}
