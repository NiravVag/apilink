using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.Customer;
using DTO.References;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CustomerCheckPointController : ControllerBase
    {
        private readonly ICustomerCheckPointManager _ccpManager = null;

        public CustomerCheckPointController(ICustomerCheckPointManager ccpManager)
        {
            _ccpManager = ccpManager;
        }
        [HttpGet("getCheckPoint")]
        [Right("customer-checkpoint")]
        public async Task<CheckPointResponse> GetCheckPoint()
        {
            return await _ccpManager.GetCheckPointType();
        }
        [HttpGet()]
        [Right("customer-checkpoint")]
        public async Task<CustomerCheckPointGetResponse> GetCustomerCheckPointSummary()
        {
            return await _ccpManager.GetCustomerCheckPointSummary(0, 0);
        }
        [HttpGet("{cusId}/{serviceId}")]
        [Right("customer-checkpoint")]
        public async Task<CustomerCheckPointGetResponse> GetCustomerCPSummaryByCusId(int? cusId, int? serviceId)
        {
            return await _ccpManager.GetCustomerCheckPointSummary(cusId, serviceId);
        }
        [HttpPost()]
        [Right("customer-summary")]
        public async Task<CustomerCheckPointSaveResponse> Save([FromBody] CustomerCheckPointSaveRequest request)
        {
            return await _ccpManager.Save(request);
        }
        [HttpPut()]
        [Right("customer-summary")]
        public async Task<CustomerCheckPointSaveResponse> Update([FromBody] CustomerCheckPointSaveRequest request)
        {
            return await _ccpManager.Save(request);
        }
        [HttpDelete("{id}")]
        [Right("customer-summary")]
        public async Task<CustomerCheckPointDeleteResponse> Delete(int id)
        {
            return await _ccpManager.Delete(id);
        }

        [HttpGet("get-customer-check-point-list-by-service/{customerId}/{serviceId}")]
        [Right("customer-checkpoint")]
        public async Task<CommonCheckPointDataSourceResponse> GetCustomerCheckPointListByService(int customerId, int serviceId)
        {
            return await _ccpManager.GetCustomerCheckPointList(customerId, serviceId);
        }
    }
}