using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Customer;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CustomerBrandController : ControllerBase
    {

        private readonly ICustomerBrandManager _manager = null;

        public CustomerBrandController(ICustomerBrandManager manager)
        {
            _manager = manager;
        }

        [HttpGet("get/{id}")]
        [Right("customer-summary")]
        public async Task<CustomerBrandDetails> CustomerBrands(int id)
        {
            var response = await _manager.GetCustomerBrands(id);
            return response;
        }

        [HttpPost("save")]
        [Right("customer-summary")]
        public async Task<SaveCustomerResponse> Save([FromBody] CustomerBrandDetails request)
        {
            return await _manager.Save(request);
        }

        [HttpPost("brand-list-by-customer")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetBrandDataSource(CommonCustomerSourceRequest request)
        {
            return await _manager.GetBrandDataSource(request);
        }
    }
}
