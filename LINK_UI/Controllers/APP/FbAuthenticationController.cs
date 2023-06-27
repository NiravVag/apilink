using Contracts.Managers;
using DTO.User;
using LINK_UI.App_start;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shyjus.BrowserDetection;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LINK_UI.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "MobileUserFbPolicy")]
    public class FbAuthenticationController : ControllerBase
    {

        private readonly IUserRightsManager _manager = null;
        private readonly IConfiguration _configuration = null;

        public FbAuthenticationController(IUserRightsManager manager, IConfiguration configuration)
        {
            _manager = manager;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn([FromBody] MobileAppSignInRequest request)
        {
            if (request.UserName == string.Empty)
                return BadRequest(new MobileAppSignInResponse() { StatusCode = (int)HttpStatusCode.BadRequest, Message = DTO.User.SignInResult.LoginEmpty.ToString() });

            if (request.Password == string.Empty)
                return BadRequest(new MobileAppSignInResponse { StatusCode = (int)HttpStatusCode.BadRequest, Message = DTO.User.SignInResult.PasswordEmpty.ToString() });

            var responseData = new MobileAppSignInResponse();
            var userData = await _manager.SignIn(request.UserName, request.Password, false);

            if (userData != null && userData.Result == DTO.User.SignInResult.Success && userData.User != null)
            {
                responseData.StatusCode = (int)HttpStatusCode.OK;
                responseData.Message = userData.Result.ToString();
                responseData.Token = getFbToken(userData.User);
            }
            else if (userData != null && userData.Result != DTO.User.SignInResult.Success)
            {
                responseData.StatusCode = (int)HttpStatusCode.Unauthorized;
                responseData.Message = userData.Result.ToString();
                responseData.Token = null;
                return Unauthorized(responseData);
            }

            return Ok(responseData);
        }

        private string getFbToken(User userData)
        {
            var Fbclaims = new List<Claim>
            {
                new Claim("email",userData.EmailAddress),
                new Claim("firstname", userData.FullName),
                new Claim("lastname", ""),
                new Claim("role", "inspector"),
                new Claim("redirect", "")
            };
            return AuthentificationService.CreateFBToken(Fbclaims, _configuration["FBKey"]);
        }
    }
}
