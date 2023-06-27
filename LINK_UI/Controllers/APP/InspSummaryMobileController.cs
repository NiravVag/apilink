using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BI.Maps.APP;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Inspection;
using DTO.MobileApp;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InspSummaryMobileController : ControllerBase
    {
        private readonly IInspectionBookingManager _manager = null;

        public InspSummaryMobileController(IInspectionBookingManager manager)
        {
            _manager = manager;
        }

        [HttpPost("getInspMobileSummary")]
        [Right("insp-summary")]
        public async Task<InspSummaryMobileResponse> GetInspSummary(InspSummaryMobileRequest request)
        {
            return await _manager.GetMobileInspSummary(request);
        }

        [Right("insp-summary")]
        [HttpGet("GetBookingProductStatus/{inspectionId}")]
        public async Task<BookingProductMobileResponse> GetBookingProductsAndStatus(int inspectionId)
        {
            return await _manager.GetMobileBookingProductsAndStatusTimeline(inspectionId);
        }

        [HttpGet("getInspMobileDetailSummary/{reportId}")]
        [Right("insp-summary")]
        public async Task<InspSummaryMobileDetaiResponse> GetInspDetailSummary(int reportId)
        {
             return await _manager.GetInspDetailMobileSummary(reportId);
        }

        [HttpPost("getMobileFactoryCountry")]
        [Right("insp-summary")]
        public async Task<FilterDataSourceResponse> GetMobileFactoryCountry(CommonCountrySourceRequest request)
        {
            return await _manager.GetMobileFactoryCountry(request);
        }

        [HttpPost("getMobileSupplier")]
        [Right("insp-summary")]
        public async Task<FilterDataSourceResponse> GetMobileSupplierFactory(CommonDataSourceRequest request)
        {
            return await _manager.GetMobileSupplierFactory(request);
        }

        [HttpPost("getMobileCustomer")]
        [Right("insp-summary")]
        public async Task<FilterDataSourceResponse> GetMobileCustomer(CommonDataSourceRequest request)
        {
            return await _manager.GetMobileCustomer(request);
        }

        [HttpPost("getMobileDepartment")]
        [Right("insp-summary")]
        public async Task<FilterDataSourceResponse> GetMobileDepartment(CommonCustomerSourceRequest request)
        {
            return await _manager.GetMobileDepartment(request);
        }

        [HttpPost("getMobileCollection")]
        [Right("insp-summary")]
        public async Task<FilterDataSourceResponse> GetMobileCollection(CommonCustomerSourceRequest request)
        {
            return await _manager.GetMobileCollection(request);
        }

        [HttpPost("getMobileBuyer")]
        [Right("insp-summary")]
        public async Task<FilterDataSourceResponse> GetMobileBuyer(CommonCustomerSourceRequest request)
        {
            return await _manager.GetMobileBuyer(request);
        }

        [HttpGet("getMobileStatusAndServiceType")]
        [Right("insp-summary")]
        public async Task<CommonFilterListResponse> GetMobileCommonFilter()
        {
            return await _manager.GetMobileCommonFilter();
        }
    }
}