using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO;
using DTO.Common;
using DTO.CommonClass;
using DTO.User;
using LINK_UI.App_start;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shyjus.BrowserDetection;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRightsManager _manager = null;

        private readonly IBrowserDetector _browserDetector;

        static IHostingEnvironment _env = null;
        static IConfiguration _configuration = null;

        public UserController(IUserRightsManager manager, IBrowserDetector browserDetector, IHostingEnvironment env, IConfiguration configuration)
        {
            _manager = manager;
            _browserDetector = browserDetector;
            _env = env;
            _configuration = configuration;
        }

        /// <summary>
        /// Login service
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<SignInResponse> SignIn([FromBody]SignInRequest request)
        {
            if (request.UserName == "")
                return new SignInResponse { Result = DTO.User.SignInResult.LoginEmpty };

            if (request.Password == "")
                return new SignInResponse { Result = DTO.User.SignInResult.PasswordEmpty };

            var response = await _manager.SignIn(request.UserName, request.Password, request.IsEncrypt);

            if (response.Result != DTO.User.SignInResult.Success)
                return response;
            // added login log
            var clientLocation = getLocationDetails();
            clientLocation.LoginTime = DateTime.Now;
            await AddLoginLog(response.User.Id, clientLocation);

            AuthentificationService.SetToken(response);
            return response;
        }
        [HttpGet("TranslateFile/{lang}")]
        public PhysicalFileResult TranslateFile(string lang)
        {
            var basePath = _env.WebRootPath;// _ApplicationContext.AppBaseUrl;
            var translateFolderPath = _configuration["TranslateFolderPath"];
            var filePath = string.Concat(basePath, translateFolderPath, lang + ".json");
            return new PhysicalFileResult(filePath, "application/json");
        }
        [Authorize]
        [HttpGet("SignOut/{id}")]
        public async Task<int> GetSignOut(int id)
        {
            // added login log
            var clientLocation = getLocationDetails();
            clientLocation.LogoutTime = DateTime.Now;
            await AddLoginLog(id, clientLocation);
            return 1;
        }

        [Authorize]
        [HttpGet("EntityAccess/{id}")]
        public async Task<List<CommonDataSource>> GetUserEntityAccess(int id)
        {
            return await _manager.GetUserEntityAccess(id);
        }

        [Authorize]
        [HttpPost("EntityRoleAccess")]
        public async Task<UserEntityAccess> GetEntityAccess(UserEntityAccessRequest request)
        {
            return await _manager.GetUserEntityRoleAccess(request.Id, request.EntityId);
        }

        [Authorize]
        [HttpGet("tasks")]
        public async Task<TaskResponse> GetTaskList()
        {
            return await _manager.GetTaskList();
        }

        [Authorize]
        [HttpPost("tasklist")]
        public async Task<TaskResponse> TaskListData(TaskSearchRequest request)
        {
            return await _manager.TaskListData(request);
        }

        [Authorize]
        [HttpPost("notifications")]
        public async Task<NotificationResponse> GetNotificationList(NotificationSearchRequest request)
        {
            return await _manager.GetNotifications(request);
        }

        [Authorize]
        [HttpGet("donetask/{id}")]
        public async Task<TaskResponse> DoneTask(Guid Id)
        {
            return await _manager.DoneTask(Id);
        }

        [Authorize]
        [HttpGet("readnotif/{id}")]
        public async Task<NotificationResponse> ReadNotification(Guid Id)
        {
            return await _manager.ReadNotification(Id);
        }

        [Authorize]
        [HttpGet("alldonetask")]
        public async Task<TaskResponse> AllDoneTask()
        {
            return await _manager.AllDoneTask();
        }

        [Authorize]
        [HttpGet("readallnotif")]
        public async Task<NotificationResponse> ReadAllNotification()
        {
            return await _manager.ReadAllNotification();
        }

        [Authorize]
        [HttpPost("notdonetaskcount")]
        public async Task<NotDoneTaskResponse> NotDoneTaskCount(TaskSearchRequest request)
        {
            return await _manager.NotDoneTaskCount(request);
        }

        [Authorize]
        [HttpPost("unreadnotifcount")]
        public async Task<UnReadNotificationResponse> UnReadNotificationCount(NotificationSearchRequest request)
        {
            return await _manager.UnReadNotificationCount(request);
        }

        [Authorize]
        [HttpGet("[action]")]
        public IActionResult Test()
        {
            return new ObjectResult(true);
        }

        [HttpPost("subscribe/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public void Subscribe(int id, [FromBody] PushSubscriptionModel sub, [FromServices] IBroadCastService broadCastService)
        {
            broadCastService.Subscribe(id, sub);
        }

        [HttpPost("unsubscribe{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public void Unsubscribe(int id, [FromBody] PushSubscriptionModel sub, [FromServices] IBroadCastService broadCastService)
        {
            broadCastService.Unsubscribe(id, sub);
        }


        [AllowAnonymous]
        [HttpPost("broadcast/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public bool Broadcast(int id, [FromBody] Notification notification, [FromServices] IBroadCastService broadCastService)
        {
            return broadCastService.Broadcast(id, notification);
        }


        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        public async Task<ChangePasswordResponse> ChangePassword([FromBody]ChangePasswordRequest request)
        {
            var response = await _manager.ChangePassword(request);
            return response;
        }

        /// <summary>
        /// Get Request Location information
        /// </summary>
        /// <returns></returns>
        private DeviceLocation getLocationDetails()
        {
            IPAddress[] address = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            return new DeviceLocation()
            {
                DeviceType = _browserDetector?.Browser?.DeviceType,
                OsVersion = _browserDetector?.Browser?.OS,
                BrowserName = _browserDetector?.Browser?.Name,
                IpAddress = address[1]?.ToString()
            };
        }
        /// <summary>
        /// Add login and Logout log
        /// </summary>
        /// <param name="userItId"></param>
        /// <returns></returns>
        private async Task AddLoginLog(int userItId, DeviceLocation clientLocation)
        {
            clientLocation.UserItId = userItId;
            await _manager.SaveLoginLog(clientLocation);
        }
        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("resetpassword")]
        public async Task<ResetPasswordResponse> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var response = await _manager.ResetPassword(request);
            return response;
        }
        [Authorize]
        [HttpGet("righttypelst")]
        public async Task<RightTypeResponse> GetRightTypeList()
        {
            return await _manager.GetRightTypeList();
        }
    }
}