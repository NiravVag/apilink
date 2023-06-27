using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.Customer;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DTO.CommonClass;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CustomerServiceConfigController : ControllerBase
    {

        private readonly ICustomerServiceConfigManager _manager = null;

        public CustomerServiceConfigController(ICustomerServiceConfigManager manager)
        {
            _manager = manager;
        }


        [HttpPost("customerserviceconfig")]
        [Right("customer-summary")]
        public Task<CustomerServiceConfigResponse> EditCustomerServiceConfigSearch([FromBody]CustomerServiceConfigRequest request)
        {
            if ( request.index == null)
                request.index = 0;
            if (request.pageSize == null || request.pageSize == 0)
                request.pageSize = 10;
            var response= _manager.GetEditCustomerServiceConfigSummary(request);
            return response;

        }

        [HttpPost("search")]
        [Right("customer-summary")]
        public Task<CustomerServiceConfigSearchResponse> CustomerServiceConfigSearch([FromBody]CustomerServiceConfigSearchRequest request)
        {
            if (request.index == null)
                request.index = 0;
            if (request.pageSize == null || request.pageSize == 0)
                request.pageSize = 10;
            return _manager.GetCustomerServiceConfigData(request);

        }

        [HttpGet()]
        [Right("customer-summary")]
        public async Task<CustomerServConfigSummaryResponse> GetCustomerServiceConfigSummary()
        {
            var response = await _manager.GetCustomerServiceConfigSummary();
            return response;
        }

        [HttpGet("edit/{id}")]
        [Right("customer-summary")]
        public EditCustomerServiceConfigResponse GetCustomerServiceconfigDetails(int id)
        {
            var response= _manager.GetEditCustomerServiceConfig(id);
            return response;
        }

        [HttpGet("getserviceconfigmaster")]
        [Right("customer-summary")]
        public async Task<CustomerServiceConfigMasterResponse> GetServiceConfigMaster(int id)
        {
            var response = await _manager.GetCustomerServiceConfigMaster();
            return response;
        }

        [HttpPost("save")]
        [Right("customer-summary")]
        public async Task<SaveCustomerServiceConfigResponse> Save([FromBody] EditCustomerServiceConfigData request)
        {
            return await _manager.Save(request);
        }

        [HttpGet("delete/{id}")]
        [Right("customer-summary")]
        public async Task<CustomerServiceConfigDeleteResponse> DeleteCustomerService(int id)
        {
            return await _manager.DeleteCustomerService(id);
        }

        [HttpGet("getservicelevelpickfirst")]
        [Right("customer-summary")]
        public async Task<CustomerServicePickResponse> GetServiceLevelPick()
        {
            return await _manager.GetLevelPickFirst();
        }

        [HttpGet("getserviceType/{customerId}/{serviceId}")]
        public async Task<DataSourceResponse> GetServiceconfig(int customerId, int serviceId)
        {
            return await _manager.GetServiceconfig(customerId, serviceId);
        }
    }
}