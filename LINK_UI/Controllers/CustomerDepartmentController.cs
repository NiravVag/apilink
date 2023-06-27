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
    public class CustomerDepartmentController : ControllerBase
    {
        private readonly ICustomerDepartmentManager _manager = null;

        public CustomerDepartmentController(ICustomerDepartmentManager manager)
        {
            _manager = manager;
        }

        [HttpGet("get/{id}")]
        [Right("customer-deparment")]
        public async Task<CustomerDepartmentResponse> CustomerDepartment(int id)
        {
            var response = await _manager.GetCustomerDepartments(id);
            return response;
        }

        
        [HttpPost("department/save")]
        [Right("customer-deparment")]
        public async Task<SaveCustomerDepartmentResponse> SaveDepartment([FromBody]SaveCustomerDepartmentRequest request)
        {
            return await _manager.Save(request);
        }

        [HttpGet("delete/{id}")]
        [Right("customer-summary")]
        public async Task<CustomerDepartmentDeleteResponse> DeleteCustomerContact(int id)
        {
            return await _manager.DeleteCustomerDepartment(id);
        }

        [HttpPost("dept-list-by-customer")]
        [Right("customer-summary")]
        public async Task<DataSourceResponse> GetDepartmentDataSource(CommonCustomerSourceRequest request)
        {
            return await _manager.GetDepartmentDataSource(request);
        }
    }
}