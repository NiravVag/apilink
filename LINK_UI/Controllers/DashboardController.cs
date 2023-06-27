using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.Dashboard;
using DTO.Manday;
using DTO.User;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardManager _manager = null;
        public DashboardController(IDashboardManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Get the customer business overview data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("getcustomerbusinessoverview")]
        [Right("get-dashboard")]
        public async Task<CustomerBusinessOVDashboard> GetCustomerBusinessOverview(CustomerDashboardFilterRequest request)
        {
            return await _manager.GetCustomerBusinessOverview(request);
        }

        /// <summary>
        /// Get the base booking details with the filter request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("getBookingDetails")]
        [Right("get-dashboard")]
        public async Task<List<BookingDetail>> GetBookingDetails(CustomerDashboardFilterRequest request)
        {
            return await _manager.GetBookingDetails(request);
        }

        /// <summary>
        /// Get the API Result Analysis data
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        [HttpPost("getAPIRAResult")]
        [Right("get-dashboard")]
        public async Task<List<CustomerAPIRADashboard>> GetAPIRAResult(CustomerDashboardFilterRequest request)
        {
            return await _manager.GetAPIRADashboard(request);
        }

        /// <summary>
        /// Get the customer result analytics data
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        [HttpPost("getCustomerResult")]
        [Right("get-dashboard")]
        public async Task<List<CustomerResultDashboard>> GetCustomerResult(CustomerDashboardFilterRequest request)
        {
            return await _manager.GetCustomerResultDashBoard(request);
        }

        /// <summary>
        /// Get the inspection rejection result
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        [HttpPost("getinspectionreject")]
        [Right("get-dashboard")]
        public async Task<List<InspectionRejectDashboard>> GetInspectionReject(CustomerDashboardFilterRequest request)
        {
            return await _manager.GetInspectionRejectDashBoard(request);
        }

        /// <summary>
        /// Get the product category data by inspectionids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        [HttpPost("getproductcategoryData")]
        [Right("get-dashboard")]
        public async Task<List<ProductCategoryDashboard>> GetProductCategory(CustomerDashboardFilterRequest request)
        {
            return await _manager.GetProductCategoryDashboard(request);
        }

        /// <summary>
        /// Get the supplier performance data with the filter request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("getsupplierperformance")]
        [Right("get-dashboard")]
        public async Task<SupplierPerformanceDashboard> GetSupplierPerformance(CustomerDashboardFilterRequest request)
        {
            return await _manager.GetSupplierPerformDashBoard(request);
        }

        /// <summary>
        /// Get the pending quotation tasks
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("getquotationtasks/{customerId}")]
        [Right("get-dashboard")]
        public async Task<QuotationTaskData> GetQuotationTasks(int customerId)
        {
            return await _manager.GetQuotationTasks(customerId);
        }

        /// <summary>
        /// Get the man day data by type(daily,weekly,monthly)
        /// </summary>
        /// <param name="manDayType"></param>
        /// <returns></returns>
        [HttpGet("getmandaydata/{mandaytype}/{customerId}")]
        [Right("get-dashboard")]
        public async Task<ManDayDashboard> GetManDayData(int manDayType, int customerId)
        {
            return await _manager.GetManDaysData(manDayType, customerId);
        }

        /// <summary>
        /// Get the inspected bookings data
        /// </summary>
        /// <param name="inspectedIds"></param>
        /// <returns></returns>
        [HttpPost("getinspectedbookings")]
        [Right("get-dashboard")]
        public async Task<InspectionManDayOverview> GetInspectedBookings(CustomerDashboardFilterRequest request)
        {
            return await _manager.GetInspectionManDayOverview(request);
        }

        /// <summary>
        /// Get the customer cs contact
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        [HttpGet("getcustomercscontact/{customerid}")]
        [Right("get-dashboard")]
        public async Task<UserStaffDetails> GetCustomerCsContact(int customerid)
        {
            return await _manager.GetCustomerCsContact(customerid);
        }

        /// <summary>
        /// Get the country geo code 
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        [HttpPost("getcountrygeocode")]
        [Right("get-dashboard")]
        public async Task<MapGeoLocation> GetInspCountryGeoCode(DashboardMapFilterRequest dashboardMapFilterRequest)
        {
            return await _manager.GetInspCountryGeoCode(dashboardMapFilterRequest);
        }

        /// <summary>
        /// Get the country geo code  for allocated inspection
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        [HttpGet("getallocatedcountrygeocode")]
        [Right("get-dashboard")]
        public async Task<List<InspCountryGeoCode>> GetAllocatedInspCountryGeoCode()
        {
            return await _manager.GetAllocatedInspCountryGeoCode();
        }

        /// <summary>
        /// Get the inspected bookings data
        /// </summary>
        /// <param name="inspectedIds"></param>
        /// <returns></returns>
        [HttpPost("getinspectedbookingsByFactory")]
        [Right("get-dashboard")]
        public async Task<CustomerFactoryDashboard> GetInspectedBookingsByFactory(CustomerDashboardFilterRequest request)
        {
            return await _manager.GetInspectedBookingsByFactory(request);
        }
        [HttpPost("get-manday-count-by-year")]
        [Right("mandayDashboard")]
        public async Task<MandayYearChartResponse> GetMandayYearChart([FromBody] CustomerDashboardFilterRequest request)
        {
            return await _manager.GetMandayYearChart(request);
        }

        /// <summary>
        /// get customer decision count
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("getcustomerdecisioncount/{customerId}")]
        public async Task<CustomerDecisionCount> GetCustomerDecisionCount(int customerId)
        {
            return await _manager.GetCustomerDecisionCount(customerId);
        }
    }
}