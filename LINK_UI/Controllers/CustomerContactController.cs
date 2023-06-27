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
using DTO.UserAccount;
using static DTO.Common.ApiCommonData;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CustomerContactController : ControllerBase
    {
        private readonly ICustomerContactManager _manager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private static IConfiguration _configuration = null;

        public CustomerContactController(ICustomerContactManager manager, IRabbitMQGenericClient rabbitMQClient, IConfiguration configuration)
        {
            _manager = manager;
            _rabbitMQClient = rabbitMQClient;
            _configuration = configuration;
        }

        [HttpPost("customercontactsummary")]
        [Right("customer-summary")]
        public async Task<CustomerContactSummaryResponse> CustomerContactSummary([FromBody] CustomerContactSummaryRequest request)
        {
            var response = await _manager.GetCustomerContactSummary(request);
            return response;
        }

        [HttpPost("search")]
        [Right("customer-summary")]
        public CustomerContactSearchResponse CustomerContactSearch([FromBody] CustomerContactSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null || request.pageSize == 0)
                request.pageSize = 10;
            return _manager.GetCustomerContactData(request);

        }

        [HttpGet("deletecontact/{id}")]
        [Right("customer-summary")]
        public async Task<CustomerContactDeleteResponse> DeleteCustomerContact(int id)
        {
            return await _manager.DeleteCustomerContact(id);
        }

        [HttpGet("edit/{id}")]
        [Right("customer-summary")]
        public async Task<EditCustomerContactResponse> GetCustomerContactDetails(int id)
        {
            return await _manager.GetEditCustomerContact(id);
        }

        [HttpGet("add/{id}")]
        [Right("customer-summary")]
        public async Task<CustomerContactResponse> GetCustomerContactDetails1(int id)
        {
            return await _manager.GetCustomerContact(id);
        }

        [HttpPost("save")]
        [Right("customer-summary")]
        public async Task<SaveCustomerContactResponse> Save([FromBody] CustomerContactDetails request)
        {
            var response = await _manager.Save(request);
            if (response != null && response.Id > 0)
            {
                //add the customer details
                UpdateCustomerContactToTCF(request.ContactServiceList, response.Id, MasterDataType.CustomerContactCreation);
            }
            return response;
        }


        private async void UpdateCustomerContactToTCF(IEnumerable<int> ContactServiceList, int customerId, MasterDataType masterDataMap)
        {
            //push the customer account to FB if selected api service is TCF
            if (ContactServiceList != null && ContactServiceList.Contains((int)Service.Tcf))
            {
                var tcfCustomerContactRequest = new MasterDataRequest()
                {
                    Id = Guid.NewGuid(),
                    SearchId = customerId,
                    ExternalClient = ExternalClient.TCF,
                    MasterDataType = masterDataMap
                };
                await _rabbitMQClient.Publish<MasterDataRequest>(_configuration["AccountQueue"], tcfCustomerContactRequest);
            }
        }

        [HttpGet("GetContactBrandByCusId/{cusId}")]
        [Right("customer-summary")]
        public async Task<EditCustomerContactResponse> GetContactBrand(int cusId)
        {
            return await _manager.GetContactBrandByCusId(cusId);
        }

        [HttpPost("GetCustomerContactDataSourceList")]
        public async Task<DataSourceResponse> GetCustomerContactDataSourceList(CustomerContactDataSourceRequest request)
        {
            return await _manager.GetCustomerContactDataSourceList(request);
        }


        [HttpGet("GetCustomerContactByBooking/{bookingId}")]
        public async Task<DataSourceResponse> GetCustomerContactByBooking(int bookingId)
        {
            return await _manager.GetCustomerContactByBooking(bookingId);
        }


        [HttpGet("GetCustomerContactByServiceAndBooking/{bookingId}/{serviceId}")]
        public async Task<DataSourceResponse> GetCustomerContactByServiceAndBooking(int bookingId, int serviceId)
        {
            return await _manager.GetCustomerContactByBookingAndService(bookingId, serviceId);
        }


        [HttpGet("get-customer-contact-by-customerId/{customerId}")]
        public async Task<CustomerContactSummaryResponse> GetCustomerContactByCustomerId(int customerId)
        {
            return await _manager.GetCustomerContactsByCustomerId(customerId);
        }

        [HttpPost("create-customer-contact-credential")]
        public async Task<SaveUserResponse> CreateCustomerContactUserCredentials(CustomerContactUserRequest request)
        {
            return await _manager.CreateCustomerContactUserCredential(request, CustomerContactCredentialsFrom.ContactPage);
        }
    }
}