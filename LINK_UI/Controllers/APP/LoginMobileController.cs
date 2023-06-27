using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BI.Maps.APP;
using Contracts.Managers;
using DTO.MobileApp;
using LINK_UI.App_start;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginMobileController : ControllerBase
    {
        private IUserRightsManager _manager = null;
        private IHumanResourceManager _hrManager = null;

        public LoginMobileController(IUserRightsManager manager, IHumanResourceManager _hrManager)
        {
            _manager = manager;
            _hrManager = _hrManager;
        }

        [HttpPost("[action]")]
        public async Task<LoginMobileResponse> SignIn(LoginMobileRequest request)
        {
            var response = new LoginMobileResponse();
            try
            {
                if (request.userName == "")
                    return new LoginMobileResponse();

                if (request.password == "")
                    return new LoginMobileResponse();

                var result = await _manager.SignIn(request.userName, request.password, request.isEncrypt);

                AuthentificationService.SetToken(result);

                response.data = LoginMobileMap.MapLoginResponse(result); //,image)
                response.meta = new MobileResult { success = true, message = "Login successful" };
            }
            catch(Exception e)
            {
                response.meta = new MobileResult { success = false, message = "Login failed" };
            }
            return response;
        }
    }
}