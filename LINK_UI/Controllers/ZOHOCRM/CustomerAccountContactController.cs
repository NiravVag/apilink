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
    [ApiController]
    [Authorize(Policy = "ZohoUserPolicy")]
    public class CustomerAccountContactController : ExternalBaseController
    {
        private readonly ICustomerContactManager _customerContactManager = null;
        private static IConfiguration _configuration = null;
        private readonly IEventBookingLogManager _eventLog = null;
        public CustomerAccountContactController(ICustomerContactManager customerContactManager,
                                                            IEventBookingLogManager eventLog, IConfiguration configuration)
        {
            _customerContactManager = customerContactManager;
            _configuration = configuration;
            _eventLog = eventLog;
        }

        /// <summary>
        /// Save the customer contact with zoho customerid and contactid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("customeraccount/{id}/customercontact/{contactId}")]
        [Right("customer-summary")]
        public async Task<IActionResult> SaveCustomerContact(string id, [FromBody] SaveZohoCrmCustomerContactDetails request, string contactId)
        {
            await _eventLog.SaveZohoRequestLog(new ZohoRequestLogInfo()
            {
                CustomerId = long.Parse(id),
                RequestUrl = "/customeraccount/{id}/customercontact/{contactId}",
                CreatedOn = DateTime.Now,
                CreatedBy = Convert.ToInt16(_configuration["ExternalAccessorEntityID"]),
                LogInformation = JsonConvert.SerializeObject(request)
            });
            //Map the zoho input to customercontactdetails
            var response = await _customerContactManager.SaveZohoCRMContact(request, id, contactId);
            return BuildCommonLinkAPIResponse(response);
        }

        /// <summary>
        /// Update the customer contact with zoho customerid and zoho contactid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("customeraccount/{id}/customercontact/{contactId}")]
        [Right("customer-summary")]
        public async Task<IActionResult> UpdateCustomerContact(string id, [FromBody] SaveZohoCrmCustomerContactDetails request, string contactId)
        {
            await _eventLog.SaveZohoRequestLog(new ZohoRequestLogInfo()
            {
                CustomerId = long.Parse(id),
                RequestUrl = "/customeraccount/{id}/customercontact/{contactId}",
                CreatedOn = DateTime.Now,
                CreatedBy = Convert.ToInt16(_configuration["ExternalAccessorEntityID"]),
                LogInformation = JsonConvert.SerializeObject(request)
            });
            //map the zoho input to customercontact details
            var response = await _customerContactManager.UpdateZohoCRMContact(request, id, contactId);
            return BuildCommonLinkAPIResponse(response);
        }

        /// <summary>
        /// Get the customer contact details by zoho customerid and zoho contactid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contactid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("customeraccount/{id}/customercontact/{contactId}")]
        [Right("customer-summary")]
        public async Task<IActionResult> GetCustomerContactDetails(long id, long contactId)
        {
            //map the zoho input to customercontact details
            var response = await _customerContactManager.GetCustomerContactByZohoData(id, contactId);
            return BuildCommonLinkAPIResponse(response);
        }

        /// <summary>
        /// get customer contact by customerid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("customeraccount/{id}/customercontact")]
        [Right("customer-summary")]
        public async Task<IActionResult> GetCustomerContactsByCustomerId(long id)
        {
            var response = await _customerContactManager.GetCustomerContactByZohoCustomerId(id);
            return BuildCommonLinkAPIResponse(response);
        }

        /// <summary>
        /// Get the customer contacts by customerid and contact emailid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("customeraccount/{id}/getcustomercontactbyemail/{email}")]
        [Right("customer-summary")]
        public async Task<IActionResult> GetCustomerContactByEmailAndId(long id, string email)
        {
            var response = await _customerContactManager.GetCustomerContactByEmailAndID(id, email);
            return BuildCommonLinkAPIResponse(response);
        }
    }
}