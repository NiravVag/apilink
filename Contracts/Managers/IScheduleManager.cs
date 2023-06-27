using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.CommonClass;
using DTO.HumanResource;
using DTO.Inspection;
using DTO.Schedule;
using DTO.UserAccount;
using Entities;

namespace Contracts.Managers
{
    public interface IScheduleManager
    {
        /// <summary>
        /// get schedule allocation details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Return the booking result which is ready to schedule</returns>
        Task<ScheduleSearchResponse> GetScheduleDetails(ScheduleSearchRequest request);
        /// <summary>
        /// save the schedule allocation with QC,CS Product details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Return the booking id with result</returns>
        Task<SaveScheduleResponse> SaveSchedule(SaveScheduleRequest request, string fbToken);
        /// <summary>
        /// get booking details, service date range for QC, CS, Actual/Available Manday
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<ScheduleAllocation> GetBookingAllocation(int bookingId);
        /// <summary>
        /// get details for file save
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<List<ScheduleBookingItemExportSummarynew>> ExportSummary(IEnumerable<ScheduleBookingItem> request);

        /// <summary>
        /// get details for file save
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<List<ScheduleBookingItemExportSummaryProductLevel>> ExportSummaryProductLevel(IEnumerable<ScheduleBookingItem> data);
        /// <summary>
        /// get schedule records make as inactive by bookingid
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task UpdateScheduleOnReschedule(int bookingId);

        /// <summary>
        /// get schedule records make as inactive by bookingid
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<AllocationStaffList> GetQCList(AllocationStaffSearchRequest allocationStaffSearchRequest);

        /// <summary>
        /// get total man day, availale man day and Qcs on leave for a date range
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<MandayForecastResponse> GetManDayForecast(MandayForecastRequest request);

        /// <summary>
        /// get Qc on leave details
        /// </summary>
        /// <param name="date, location"></param>
        /// <returns></returns>
        Task<StaffLeaveInfoResponse> GetStaffDetailsWithLeave(string date, int locationId, int zoneid);

        /// <summary>
        /// get manday details by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<QuotScheduleMandayResponse> GetMandayDetails(int bookingId);

        /// <summary>
        /// save manday
        /// </summary>
        /// <param name="object manday list"></param>
        /// <returns></returns>
        Task<int> SaveManday(QuotScheduleManday data);

        /// <summary>
        /// qc names virtual scroll implementation
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetQcDataSource(CommonQcSourceRequest request);

        /// <summary>
        /// Update the scheduled QC and CS on booking reschedule
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateScheduleOnRescheduleToLesserDate(BookingDateInfo request, InspTransaction entity);

        /// <summary>
        /// calculate the actual manday
        /// </summary>
        /// <param name="QcId"></param>
        /// <param name="serviceDate"></param>
        /// <returns></returns>
        Task UpdateScheduleQcMandayOnCancelReschedule(int bookingId, bool isKeepQCForTravelExpense);

        /// <summary>
        /// QcForVisibility
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BookingDataQcVisibleResponse> GetQcVisibilityByBooking(QcVisibilityBookingRequest request);
        Task<int> UpdateQcVisibileData(BookingDataQcVisibleRequest request);
        Task<List<DuplicateTravelAllowance>> GetDuplicateTravelExpenseData(SaveScheduleRequest request);

        Task<ScheduleProductModelResponse> GetProductPODetails(int bookingId);
    }
}
