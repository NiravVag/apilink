using Contracts.Managers;
using DTO.Customer;
using DTO.EventBookingLog;
using LINK_UI.Controllers.EXTERNAL;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ZohoUserPolicy")]
    public class CustomerAccountController : ExternalBaseController
    {

        private readonly ICustomerManager _manager = null;
        private readonly IEventBookingLogManager _eventLog = null;
        private static IConfiguration _configuration = null;
        public CustomerAccountController(ICustomerManager manager,
                                                   IEventBookingLogManager eventLog, IConfiguration configuration)
        {
            _manager = manager;
            _eventLog = eventLog;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCRMCustomer([FromBody] SaveCustomerCrmRequest request)
        {
            await _eventLog.SaveZohoRequestLog(new ZohoRequestLogInfo()
            {
                CustomerId = request.ZohoCustomerId,
                RequestUrl = "/api/CustomerAccount",
                CreatedOn = DateTime.Now,
                CreatedBy = Convert.ToInt16(_configuration["ExternalAccessorEntityID"]),
                LogInformation = JsonConvert.SerializeObject(request)
            });
            var response = await _manager.SaveZohoCRMCustomer(request);
            return BuildCommonLinkAPIResponse(response);
        }

        [HttpGet("{id}")]
        [Right("customer-summary")]
        public async Task<IActionResult> GetCustomerDetails(long id)
        {
            var response = await _manager.GetCustomerByZohoId(id);
            return BuildCommonLinkAPIResponse(response);
        }

        [HttpGet("getcustomerbyglcode/{glCode}")]
        [Right("customer-summary")]
        public async Task<IActionResult> GetCustomerDetailsbyGLCode(string glCode)
        {
            var response = await _manager.GetCustomerByGLCode(glCode);
            return BuildCommonLinkAPIResponse(response);
        }

        [HttpPut("{id}")]
        [Right("customer-summary")]
        public async Task<IActionResult> Update([FromBody] SaveCustomerCrmRequest request, long id)
        {
            await _eventLog.SaveZohoRequestLog(new ZohoRequestLogInfo()
            {
                CustomerId = request.ZohoCustomerId,
                RequestUrl = "/api/CustomerAccount/{id}",
                CreatedOn = DateTime.Now,
                CreatedBy = Convert.ToInt16(_configuration["ExternalAccessorEntityID"]),
                LogInformation = JsonConvert.SerializeObject(request)
            });
            //Map CustomerDetails as per zoho crm input
            var response = await _manager.UpdateZohoCustomer(request, id);
            return BuildCommonLinkAPIResponse(response);

        }
    }
}