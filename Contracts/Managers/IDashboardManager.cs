using DTO.Dashboard;
using DTO.Manday;
using DTO.MobileApp;
using DTO.User;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{

    public interface IDashboardManager
    {

        /// <summary>
        /// Get the booking details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<BookingDetail>> GetBookingDetails(CustomerDashboardFilterRequest request);

        /// <summary>
        /// Get the customer business overview data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CustomerBusinessOVDashboard> GetCustomerBusinessOverview(CustomerDashboardFilterRequest request);

        /// <summary>
        /// Get the API RA Dashboard
        /// </summary>
        /// <param name="inspectionids"></param>
        /// <returns></returns>
        Task<List<CustomerAPIRADashboard>> GetAPIRADashboard(CustomerDashboardFilterRequest request);
        Task<List<CustomerAPIRADashboard>> GetQueriableAPIRADashboard(IQueryable<int> inspectionids);

        /// <summary>
        /// Get the customer result dashboard
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        Task<List<CustomerResultDashboard>> GetCustomerResultDashBoard(CustomerDashboardFilterRequest request);
        Task<List<CustomerResultDashboard>> GetQueryableCustomerResultDashBoard(IQueryable<int> inspectionIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        Task<List<ProductCategoryDashboard>> GetProductCategoryDashboard(CustomerDashboardFilterRequest request);

        /// <summary>
        /// Get the inspection reject dashboard
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        Task<List<InspectionRejectDashboard>> GetInspectionRejectDashBoard(CustomerDashboardFilterRequest request);

        Task<SupplierPerformanceDashboard> GetSupplierPerformDashBoard(CustomerDashboardFilterRequest request);

        /// <summary>
        /// Get the quotation tasks
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<QuotationTaskData> GetQuotationTasks(int customerId);

        /// <summary>
        /// Get Man Day Data 
        /// </summary>
        /// <param name="manDaySearchData"></param>
        /// <returns></returns>
        Task<ManDayDashboard> GetManDaysData(int manDaySearchData, int customerId);

        Task<InspectionManDayOverview> GetInspectionManDayOverview(CustomerDashboardFilterRequest request);

        Task<UserStaffDetails> GetCustomerCsContact(int customerId);

        Task<MapGeoLocation> GetInspCountryGeoCode(DashboardMapFilterRequest request);

        Task<List<InspCountryGeoCode>> GetAllocatedInspCountryGeoCode();

        /// <summary>
        /// Get Man Day Data 
        /// </summary>
        /// <param name="manDaySearchData"></param>
        /// <returns></returns>
        Task<CustomerFactoryDashboard> GetInspectedBookingsByFactory(CustomerDashboardFilterRequest request);


        // <summary>
        /// Get customer dashboard details for mobile
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        Task<InspDashboardMobileResponse> GetMobileCustomerDashboard(InspDashboardMobileRequest request);
        /// <summary>
        /// manday count by year 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<MandayYearChartResponse> GetMandayYearChart(CustomerDashboardFilterRequest request);

        /// <summary>
        /// Get task list
        /// </summary>
        /// <returns></returns>
        Task<MobileTaskResponse> GetMobiletTaskList();

        IQueryable<SuAddress> GetFactoryAddressById(IEnumerable<int> factoryIds);

        Task<List<CustomerAPIRADashboard>> GetAPIRADashboardByQuery(IQueryable<int> inspectionids);

        Task<List<InspectionRejectDashboard>> GetInspectionRejectDashBoardByBookingQuery(IQueryable<int> inspectionIds);

        /// <summary>
        /// get customer decision count
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<CustomerDecisionCount> GetCustomerDecisionCount(int customerId);
    }
}
