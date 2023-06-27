using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DTO.RoleRight;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DTO.References;
using Contracts.Managers;
using DTO.CommonClass;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class RoleRightController : ControllerBase
    {
        public IRoleRightManager _manager = null;
        public RoleRightController(IRoleRightManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [Right("role-right-summary")]
        public RoleRightResponse GetRoleRightSummary()
        {
            return _manager.GetRoleRightSummary();
        }

        [HttpGet("{id}")]
        [Right("role-right-summary")]
        public RoleRightSearchResponse GetRoleRightByRole(int id)
        {
            return _manager.GetRoleRightSearch(id);
        }

        [HttpPut()]
        [Right("role-right-summary")]
        public async Task<SaveRoleRightResponse> SaveRoleRight([FromBody] RoleRightRequest request)
        {
            var response = await _manager.SaveRoleRight(request);
            return response;
        }

        [HttpGet("getRoleList/{userId}")]
        [Right("role-right-summary")]
        public async Task<DataSourceResponse> RoleList(int userId)
        {
            return await _manager.GetRoleList(userId);
        }
    }
}