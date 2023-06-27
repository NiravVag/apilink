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
    public class CustomerCollectionController : ControllerBase
    {
        private readonly ICustomerCollectionManager _manager = null;

        public CustomerCollectionController(ICustomerCollectionManager manager)
        {
            _manager = manager;
        }

        [HttpPost("get")]
        [Right("customer-collection")]
        public async Task<CustomerCollectionDetails> CustomerCollections([FromBody]CustomerCollectionListSummary request)
        {
            return await _manager.GetCustomerCollectionList(request);
        }

        [HttpPost("save")]
        [Right("customer-collection")]
        public async Task<SaveCustomerResponse> Save([FromBody] CustomerCollectionDetails request)
        {
            return await _manager.Save(request);
        }

        [HttpPost("collection-list-by-customer")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetCollectionDataSource(CommonCustomerSourceRequest request)
        {
            return await _manager.GetCollectionDataSource(request);
        }
    }
}