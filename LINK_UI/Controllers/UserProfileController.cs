using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.UserConfig;
using DTO.UserProfile;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileManager _manager = null;

        public UserProfileController(IUserProfileManager manager)
        {
            _manager = manager;
        }

        [HttpGet("{userId}")]
        [Right("User-profile")]
        public async Task<UserProfileResponse> Edit(int userId)
        {
            if(userId <= 0)
            {
                return new UserProfileResponse { Result = UserProfileResult.InvalidUserId };
            }

            return await _manager.GetUserProfileSummary(userId);
        }

        [HttpPost]
        [Right("User-profile")]
        public async Task<UserProfileSaveResponse> Save(UserProfileRequest request)
        {
            if(request.UserId <= 0)
            {
                return new UserProfileSaveResponse { Result = UserProfileResponseResult.RequestNotCorrectFormat };
            }

            return await _manager.Save(request);
        }
    }
}