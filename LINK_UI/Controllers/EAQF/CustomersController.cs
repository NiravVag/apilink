using Contracts.Managers;
using DTO.Customer;
using LINK_UI.Controllers.EXTERNAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LINK_UI.Controllers.EAQF
{
    [Route("api/EAQF/[controller]")]
    [Authorize(Policy = "EAQFUserPolicy")]
    [ApiController]
    public class CustomersController : ExternalBaseController
    {
        private readonly ICustomerManager _manager = null;
        public CustomersController(ICustomerManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomer([FromBody] SaveEaqfCustomerRequest request)
        {
            var response = await _manager.SaveEAQFCustomer(request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, [FromBody] SaveEaqfCustomerRequest request)
        {
            var response = await _manager.UpdateEAQFCustomer(customerId, request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpPost("{customerId}/address")]
        public async Task<IActionResult> SaveCustomerAddress(int customerId, [FromBody] SaveEaqfCustomerAddressRequest request)
        {
            var response = await _manager.SaveEAQFCustomerAddress(customerId, request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpPut("{customerId}/address/{addressId}")]
        public async Task<IActionResult> UpdateCustomerAddress(int customerId, int addressId, [FromBody] UpdateEaqfCustomerAddressRequest request)
        {
            var response = await _manager.UpdateEAQFCustomerAddress(customerId, addressId, request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpPost("{customerId}/contact")]
        public async Task<IActionResult> SaveCustomerContact(int customerId, [FromBody] SaveEaqfCustomerContactRequest request)
        {
            var response = await _manager.SaveEAQFCustomerContact(customerId, request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpPut("{customerId}/contact/{contactId}")]
        public async Task<IActionResult> UpdateCustomerContact(int customerId, int contactId, [FromBody] SaveEaqfCustomerContactRequest request)
        {
            var response = await _manager.UpdateEAQFCustomerContact(customerId, contactId, request);
            return BuildCommonEaqfResponse(response);
        }

        [HttpGet("eaqfcustomer")]
        public async Task<IActionResult> GetCustomer(int Index, int PageSize, string CompanyName, string Email)
        {
            var response = await _manager.GetEAQFCustomer(Index, PageSize, CompanyName, Email);
            return BuildCommonEaqfResponse(response);
        }
    }
}
