using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.User;
using DTO.UserAccount;
using DTO.UserConfig;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class UserConfigController : ControllerBase
    {
        private readonly IUserConfigManager _manager = null;

        public UserConfigController(IUserConfigManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Right("User-config")]
        public async Task<UserConfigSaveResponse> Save(UserConfigSaveRequest request)
        {
            return await _manager.Save(request);
        }   
        [HttpGet("{userId}")]
        [Right("User-config")]
        public async Task<UserConfigEditResponse> Edit(int userId)
        {
            return await _manager.Edit(userId);
        }
    }
}