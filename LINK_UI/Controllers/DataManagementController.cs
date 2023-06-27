using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.Common;
using DTO.CommonClass;
using DTO.DataManagement;
using DTO.EmailLog;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/data-management")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class DataManagementController : ControllerBase
    {
        private readonly IDataManagementManager _manager = null;
        private readonly IHumanResourceManager _humanResourceManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private static IConfiguration _configuration = null;
        private readonly IUserRightsManager _userRightManager = null;

        public DataManagementController(IDataManagementManager manager, IAPIUserContext ApplicationContext, IHumanResourceManager humanResourceManager, IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IConfiguration configuration,
            IUserRightsManager userRightManager)
        {
            _manager = manager;
            _humanResourceManager = humanResourceManager;
            _emailLogQueueManager = emailLogQueueManager;
            _rabbitMQClient = rabbitMQClient;
            _ApplicationContext = ApplicationContext;
            _configuration = configuration;
            _userRightManager = userRightManager;
        }



        [HttpGet("modules")]
        //[Right("edit-audit")]
        public async Task<ModuleListResponse> GetMainModules()
        {
            return await _manager.GetModules(null);
        }

        [HttpGet("{idParent}/modules")]
        //[Right("edit-audit")]
        public async Task<ModuleListResponse> GetModules(int idParent)
        {
            return await _manager.GetModules(idParent);
        }

        [HttpPost("search")]
        public async Task<DataManagementListResponse> Search([FromBody] DataManagementListRequest request)
        {
            return await _manager.SearchDMDetail(request);
        }

        [HttpGet("{id}")]
        public async Task<DataManagementItemResponse> GetItem(int id)
        {
            var response = await _manager.GetItem(id);

            if (response.Result == DataManagementItemResult.Success)
            {
                if (!response.Item.CanEdit)
                    return new DataManagementItemResponse
                    {
                        Result = DataManagementItemResult.NotAuthorized
                    };
            }

            return response;
        }

        [HttpPost("save")]
        public async Task<DataManagmentEmailResponse> Save([FromBody] SaveDataManagementRequest request, [FromServices] IConfiguration configuration)
        {
            var response = await _manager.Save(request);

            if (response.Result == DataManagmentEmailResult.Success && request.Positions != null && request.Offices != null)
            {
                var users = await _humanResourceManager.GetEmailsByPositionsAndOffices(request.Positions, request.Offices, request.CountryIds);
                var CCUserList = new List<string>();

                if (users != null && users.Any())
                {
                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    string subject = "Data management - New file uploaded– ";

                    if (request.IdCustomer != null && request.IdCustomer.Value > 0)
                        subject = subject + request.IdCustomer.Value;

                    subject = subject + response.DMEmailData.FileHierarchyName;
                    response.DMEmailData.BaseUrl = _configuration["BaseUrl"];

                    //get users who has TechnicalTeamManagement role
                    var TechRoleUserList = await _userRightManager.GetRoleAccessUserList((int)RoleEnum.TechnicalTeamManagement);
                    var TechRoleEmailList = TechRoleUserList.Where(x => x.EmailAddress != null).Select(x => x.EmailAddress).Distinct().ToList();

                    if (!string.IsNullOrWhiteSpace(_ApplicationContext.EmailId))
                    {
                        CCUserList.Add(_ApplicationContext.EmailId);
                    }

                    CCUserList.AddRange(TechRoleEmailList);

                    // send email
                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = string.Join(";", users),
                        Cclist = string.Join(";", CCUserList.Distinct()),
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Data Management Save",
                        SourceId = response.DMEmailData.Id,
                        Subject = subject
                    };

                    emailLogRequest.Body = this.GetEmailBody("Emails/DataManagementSave", response.DMEmailData);

                    await PublishQueueMessage(emailQueueRequest, emailLogRequest, configuration);
                }
            }

            return response;
        }

        /// <summary>
        /// get rights
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("rights")]
        public async Task<DataManagementRightResponse> GetRights([FromBody] DataManagementRightRequest request)
        {
            if (request == null)
                return new DataManagementRightResponse
                {
                    Result = DataManagementRightResult.RequestRequired
                };

            return await _manager.GetRights(request);
        }


        /// <summary>
        /// get rights
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("save-rights")]
        public async Task<DataManagementRightResponse> SaveRights([FromBody] SaveDataManagementRightRequest request)
        {
            if (request == null || request.RightRequest == null)
                return new DataManagementRightResponse
                {
                    Result = DataManagementRightResult.RequestRequired
                };

            if (request.RightRequest.IdRole == null && request.RightRequest.IdStaff == null)
                return new DataManagementRightResponse
                {
                    Result = DataManagementRightResult.IdStaffOrIdRoleRequired
                };

            return await _manager.SaveRights(request);
        }

        /// <summary>
        /// Save email data into log table and publish to queue
        /// </summary>
        /// <param name="emailQueueRequest"></param>
        /// <param name="emailLogRequest"></param>
        /// <returns></returns>
        private async Task PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest, IConfiguration confiuration)
        {
            var resultId = await _emailLogQueueManager.AddEmailLog(emailLogRequest);
            emailQueueRequest.EmailQueueId = resultId;
            await _rabbitMQClient.Publish<EmailDataRequest>(confiuration["EmailQueue"], emailQueueRequest);
        }

        [HttpGet("get-module-list")]
        public async Task<DMModuleResponse> GetModuleList()
        {
            return await _manager.GetModuleList();
        }

        [HttpGet("delete/{id}")]
        public async Task<DataManagementDeleteResponse> DeleteDataManagement(int id)
        {
            return await _manager.DeleteDataManagement(id);
        }

        [HttpPost("getUserRightSummary")]
        public async Task<DMUserRightResponse> DMUserManagementSummary(DMUserManagementSummaryRequest request)
        {
            return await _manager.GetDMUserManagementSummary(request);
        }

        /// <summary>
        /// get rights
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("update-rights/{id}")]
        public async Task<DataManagementRightResponse> UpdateRights(int id, [FromBody] SaveDataManagementRightRequest request)
        {
            if (request == null || request.RightRequest == null)
                return new DataManagementRightResponse
                {
                    Result = DataManagementRightResult.RequestRequired
                };

            if (request.RightRequest.IdRole == null && request.RightRequest.IdStaff == null)
                return new DataManagementRightResponse
                {
                    Result = DataManagementRightResult.IdStaffOrIdRoleRequired
                };

            return await _manager.UpdateRights(id, request);
        }

        [HttpGet("getEditDMUserManagement/{id}")]
        public async Task<DMUserManagementDataEditResponse> GetEditUserManagement(int id)
        {
            return await _manager.EditDMUserManagement(id);
        }

        [HttpDelete("deleteDMUserManagement/{id}")]
        public async Task<DeleteDMUserManagementResponse> DeleteDMUserManagement(int id)
        {
            return await _manager.DeleteDMUserManagement(id);
        }

        [HttpGet("getModulesByDmRoleId/{id}")]
        public async Task<ModuleListResponse> GetModulesByDmRoleId(int id)
        {
            return await _manager.GetModulesByDmRoleId(id);
        }
    }
}
