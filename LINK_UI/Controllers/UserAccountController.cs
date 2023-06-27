using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DTO.UserAccount;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Contracts.Managers;
using LINK_UI.App_start;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using DTO.Common;
using BI.Utilities;
using DTO.FullBridge;
using Microsoft.Extensions.Options;
using DTO.CommonClass;
using RabbitMQUtility;
using Entities.Enums;
using DTO.Master;
using System.Linq;
using DTO.UserProfile;
using DTO.User;
using Components.Core.entities.Emails;
using DTO.EmailLog;
using DTO.MasterConfig;
using Components.Web;
using System.Net;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class UserAccountController : ControllerBase
    {
        private IUserAccountManager _manager = null;
        private static IConfiguration _configuration = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly IHelper _helper = null;
        private readonly FBSettings _fbSettings = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEventBookingLogManager _eventLog = null;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;

        public UserAccountController(IUserAccountManager manager, IConfiguration configuration,
            IRabbitMQGenericClient rabbitMQClient, IOptions<FBSettings> fbSettings, IAPIUserContext applicationContext, IHelper helper,
            IEventBookingLogManager eventLog, IInspectionBookingManager inspManager, IEmailLogQueueManager emailLogQueueManager)
        {
            _manager = manager;
            _configuration = configuration;
            _applicationContext = applicationContext;
            _helper = helper;
            _fbSettings = fbSettings.Value;
            _rabbitMQClient = rabbitMQClient;
            _eventLog = eventLog;
            _inspManager = inspManager;
            _emailLogQueueManager = emailLogQueueManager;
        }

        [HttpGet]
        [Right("user-account-summary")]
        public UserAccountResponse GetUserAccountSummary()
        {
            return _manager.GetUserAccountSummary();
        }

        [HttpGet("getTokentoFB")]
        [Right("user-account-summary")]
        public UserFBToken GetUserTokenToFB()
        {
            UserFBToken response = new UserFBToken();
            var Fbclaims = new List<Claim>
            {
                new Claim("email",_applicationContext.EmailId),
                new Claim("firstname", _applicationContext.UserName),
                new Claim("lastname", string.Empty),
                new Claim("role", "Inspector"),
                new Claim("redirect", "")
            };
            string rsaPrivateKey = _configuration["FBKey"];
            var FBToken = AuthentificationService.CreateFBToken(Fbclaims, rsaPrivateKey);
            response.Token = FBToken;
            response.ReportUrl = string.Concat(_fbSettings.BaseUrl, "/", _fbSettings.ReportUrl);
            response.ReportsUrl = string.Concat(_fbSettings.BaseUrl, "/", _fbSettings.ReportsUrl);
            response.MissionUrl = string.Concat(_fbSettings.BaseUrl, "/", _fbSettings.MissionUrl);
            response.Result = UserAccountFBResult.Success;
            return response;
        }


        [HttpPost]
        [Right("user-account-summary")]
        public async Task<UserAccountSearchResponse> GetSearchUserAccount([FromBody] UserAccountSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return await _manager.GetUserAccountSearchSummary(request);

        }
        [HttpPost("getuserdatasource")]
        public async Task<DataSourceResponse> GetUserDataSource(UserDataSourceRequest request)
        {
            return await _manager.GetUserDataSource(request);
        }

        [HttpPost("edit")]
        [Right("edit-user-account")]
        public async Task<EditUserAccountResponse> GetUserAccountDetail([FromBody] UserAccountSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return await _manager.GetUserAccountDetail(request);
        }

        [HttpPost("save")]
        [Right("edit-user-account")]
        public async Task<SaveUserAccountResponse> SaveUserAccount([FromBody] UserAccountItem request)
        {
            var response = await _manager.SaveUserAccount(request);

            if (response.Id > 0)
            {
                UpdateUserDetailsToTCF(response.ServiceIds, response.Id, MasterDataType.UserCreation);
            }

            return response;
        }

        private async void UpdateUserDetailsToTCF(List<int> apiServiceIds, int userId, MasterDataType masterDataMap)
        {
            bool isTCFUser = false;
            if (apiServiceIds != null && apiServiceIds.Contains((int)Service.Tcf))
                isTCFUser = true;

            //push the supplier account to FB if selected api service is not TCF
            if (isTCFUser)
            {
                var tcfSupplierRequest = new MasterDataRequest()
                {
                    Id = Guid.NewGuid(),
                    SearchId = userId,
                    ExternalClient = ExternalClient.TCF,
                    MasterDataType = masterDataMap
                };

                await _rabbitMQClient.Publish<MasterDataRequest>(_configuration["AccountQueue"], tcfSupplierRequest);
            }
        }

        [HttpPut("save")]
        [Right("edit-user-account")]
        public async Task<SaveUserAccountResponse> UpdateUserAccount([FromBody] UserAccountItem request)
        {
            var response = await _manager.SaveUserAccount(request);

            if (response.Id > 0)
            {
                UpdateUserDetailsToTCF(response.ServiceIds, response.Id, MasterDataType.UserCreation);
            }

            return response;
        }

        [Right("edit-user-account")]
        [HttpDelete("{id}")]
        public async Task<DeleteUserAccountResponse> DeleteProductSubCategory(int id)
        {
            return await _manager.RemoveUserAccount(id);
        }
        [HttpGet("loggedUserRoleExists/{roleId}")]
        [Right("edit-user-account")]
        public bool LoggedUserRoleExists(int roleId)
        {
            return _manager.LoggedUserRoleExists(roleId);
        }

        [HttpGet("getUserName/{id}")]
        [Right("edit-user-account")]
        public async Task<UserNameResponse> GetUserName(int id)
        {
            return await _manager.GetUserName(id);
        }

        [HttpPost("save-user")]
        [Right("edit-user-account")]
        public async Task<SaveUserResponse> SaveUserFromSupplier([FromBody] UserAccountItem request)
        {
            var response = await _manager.AddUserDetails(request);
            if (response.Id > 0)
            {
                UpdateUserDetailsToTCF(request.ApiServiceIds, response.Id, MasterDataType.UserCreation);
                await SendEmail(response);
            }

            return response;
        }

        [HttpGet("user-detail/{contactId}/{usertypeId}")]
        [Right("edit-user-account")]
        public async Task<SaveUserResponse> GetLoginUserDetail(int contactId, int usertypeId)
        {
            return await _manager.GetLoginUserDetail(contactId, usertypeId);
        }

        [HttpGet("roles")]
        public async Task<RolesResponse> GetRoles()
        {
            return await _manager.GetRoles();
        }

        [HttpGet("get-user-applicant-details")]
        public async Task<UserApplicantDetails> GetUserApplicantDetails()
        {
            return await _manager.GetUserApplicantDetails();
        }

        private async Task SendEmail(SaveUserResponse response)
        {
            var masterConfigs = await _inspManager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            var data = await _manager.GetUserCredentialsMailDetail(response.Id);

            if(data.Result == UserCredentialsMailTemplateResult.Success)
            {
                var mailData = data.UserCredentialsMailTemplate;
                string baseUrl = _configuration["BaseUrl"];
                mailData.BaseUrl = baseUrl;
                mailData.Username = response.UserName;
                mailData.Password = response.Password;
                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };

                var emailLogRequest = new EmailLogData()
                {
                    ToList = _applicationContext.EmailId,
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "User Credentials",
                    SourceId = response.Id,
                    Subject = $"{mailData.UserType} {mailData.Name} contact’s ({mailData.ContactName}) user credentials",
                };
                emailLogRequest.Body = this.GetEmailBody("Emails/User/UserCredentials", (mailData, baseUrl + string.Format(_configuration["UrlSupplierRequest"], response.Id, entityName)));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
        }

        private async Task PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest)
        {
            var resultId = await _emailLogQueueManager.AddEmailLog(emailLogRequest);
            emailQueueRequest.EmailQueueId = resultId;
            await _rabbitMQClient.Publish<EmailDataRequest>(_configuration["EmailQueue"], emailQueueRequest);
        }

        [HttpGet("forgotpassword/{username}")]
        [Right("forgotpassword")]
        [AllowAnonymous]
        public async Task<ForgotPasswordResponse> ForgotPassword(string username)
        {
            var response = await _manager.ForgotPassword(username);
            if (response.Result == ForgotPasswordResult.Success)
                await this.SendForgotPasswordEmail(response);
            return response;
        }

        private async Task SendForgotPasswordEmail(ForgotPasswordResponse response)
        {
            var mailData = response;
            string baseUrl = _configuration["BaseUrl"];
            mailData.Link = string.Concat(baseUrl , string.Format(_configuration["UrlRecoveryEmail"]) + "?username=" + WebUtility.UrlEncode(EncryptionDecryption.EncryptStringAES(response.UserName)));
            var emailQueueRequest = new EmailDataRequest
            {
                TryCount = 1,
                Id = Guid.NewGuid()
            };

            var emailLogRequest = new EmailLogData()
            {
                ToList = response.EmailId,
                TryCount = 1,
                Status = (int)EmailStatus.NotStarted,
                SourceName = "Recovery Email",
                SourceId = response.Id,
                Subject = $"Set your New Link QMS Password",
            };
            emailLogRequest.Body = this.GetEmailBody("Emails/User/RecoveryEmail", (mailData, baseUrl + string.Format(_configuration["UrlRecoveryEmail"], response.Id)));
            await PublishQueueMessage(emailQueueRequest, emailLogRequest);
        }
    }
}