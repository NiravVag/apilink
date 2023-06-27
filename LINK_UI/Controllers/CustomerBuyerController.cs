using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.Customer;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Entities.Enums;
using DTO.Master;
using RabbitMQUtility;
using Microsoft.Extensions.Configuration;
using DTO.CommonClass;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CustomerBuyerController : ControllerBase
    {
        private readonly ICustomerBuyerManager _manager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly ITenantProvider _filterService = null;
        private static IConfiguration _configuration = null;

        public CustomerBuyerController(ICustomerBuyerManager manager, IRabbitMQGenericClient rabbitMQClient, ITenantProvider filterService, IConfiguration configuration)
        {
            _manager = manager;
            _rabbitMQClient = rabbitMQClient;
            _filterService = filterService;
            _configuration = configuration;
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Right("customer-buyer")]
        public async Task<CustomerBuyerResponse> CustomerBuyer(int id)
        {
            var response = await _manager.GetCustomerBuyers(id);
            return response;
        }

        // POST api/<controller>
        [HttpPost]
        [Right("customer-buyer")]
        public async Task<SaveCustomerBuyerResponse> Save([FromBody]SaveCustomerBuyerRequest request)
        {
            var response = await _manager.Save(request);
            if (response.Id > 0)
            {
                //if any of buyer data belongs to TCF then process the buyer info to the Queue
                if (request.buyerList.Any(x => x.apiServiceIds.Contains((int)Service.Tcf)))
                    UpdateBuyerDetailsToTCF(request.customerValue);
            }
            return response;
        }

        private async void UpdateBuyerDetailsToTCF(int customerId)
        {
            var tcfBuyerRequest = new MasterDataRequest()
            {
                Id = Guid.NewGuid(),
                SearchId = customerId,
                ExternalClient = ExternalClient.TCF,
                MasterDataType = MasterDataType.BuyerCreation,
                EntityId=_filterService.GetCompanyId()
            };
            await _rabbitMQClient.Publish<MasterDataRequest>(_configuration["AccountQueue"], tcfBuyerRequest);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Right("customer-buyer")]
        public async Task<CustomerBuyerDeleteResponse> DeleteCustomerContact(int id)
        {
            return await _manager.DeleteCustomerBuyer(id);
        }

        [HttpPost("buyer-list-by-customer")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetBuyerDataSource(CommonCustomerSourceRequest request)
        {
            return await _manager.GetBuyerDataSource(request);
        }


        [HttpPost("GetBuyerDataSourceList")]
        public async Task<DataSourceResponse> GetBuyerDataSourceList(BuyerDataSourceRequest request)
        {
            return await _manager.GetBuyerDataSourceList(request);
        }
    }
}
