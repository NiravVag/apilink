using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.MobileApp;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BI.Maps.APP;
using Entities.Enums;

namespace LINK_UI.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InspDashboardMobileController : ControllerBase
    {
        private readonly IDashboardManager _manager = null;
        private readonly IUserRightsManager _userRightsManager = null;

        public InspDashboardMobileController(IDashboardManager manager, IUserRightsManager userRightsManager)
        {
            _manager = manager;
            _userRightsManager = userRightsManager;
        }

        [HttpPost("getCustomerDashboard")]
        [Right("get-dashboard")]
        public async Task<InspDashboardMobileResponse> GetCustomerDashboard(InspDashboardMobileRequest request)
        {
            return await _manager.GetMobileCustomerDashboard(request);
        }

        [HttpGet("tasks")]
        public async Task<MobileTaskResponse> GetTaskList()
        {
            return await _manager.GetMobiletTaskList();            
        }
    }
}