using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Components.Core.contracts;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.Audit;
using DTO.CommonClass;
using DTO.EmailLog;
using DTO.Expense;
using DTO.HumanResource;
using DTO.Location;
using LINK_UI.App_start;
using LINK_UI.FileModels;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQUtility;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class HumanResourceController : ControllerBase
    {
        private IHumanResourceManager _manager = null;
        private ILocationManager _locationManager = null;
        private IFileManager _fileManager = null;
        private ILogger _logger = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private static IConfiguration _configuration = null;

        public HumanResourceController(IHumanResourceManager manager, ILocationManager locationManager,
            IFileManager fileManager, ILogger<HumanResourceController> logger,
            IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IConfiguration configuration)
        {
            _manager = manager;
            _locationManager = locationManager;
            _fileManager = fileManager;
            _logger = logger;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _configuration = configuration;
        }


        [HttpGet()]
        [Right("staff-summary")]
        public async Task<StaffSummaryResponse> GetStaffSummary()
        {
            var response = await _manager.GetStaffSummary();
            _logger.LogInformation("Get staff summary ");
            return response;
        }

        [HttpGet("positions")]
        public async Task<StaffSummaryResponse> GetPositionList()
        {
            return await _manager.GetPositions();
        }

        [HttpPost("[action]")]
        [Right("staff-summary")]
        public async Task<StaffSearchResponse> Search([FromBody] StaffSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return await _manager.GetStaffData(request);
        }

        [Right("rate-matrix")]
        [HttpPost("export")]
        public async Task<IActionResult> ExportStaff([FromBody] StaffSearchRequest request)
        {

            request.Index = 1;
            request.pageSize = 999999;

            var response = await _manager.GetStaffData(request);

            if (response.Result != StaffSearchResult.Success)
                return NotFound();

            return await this.FileAsync("StaffDetails", response.Data, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("staff/delete")]
        [Right("staff-summary")]
        public async Task<StaffDeleteResponse> DeleteStaff([FromBody] StaffDeleteRequest request)
        {
            return await _manager.DeleteStaff(request);
        }

        [Right("staff-summary")]
        [HttpGet("staff/edit/{id}")]
        public async Task<EditStaffResponse> GetStaff(int id)
        {
            return await _manager.GetEditStaff(id);
        }

        [Right("staff-summary")]
        [HttpGet("staff/bookingstaffdetails/{id}")]
        public async Task<BookingStaffInfo> GetStaffInfoDetails(int id)
        {
            return await _manager.GetStaffInfoByStaffId(id);
        }


        [Right("staff-summary")]
        [HttpGet("staff/edit")]
        public async Task<EditStaffResponse> GetStaff()
        {
            return await _manager.GetEditStaff(null);
        }

        [Right("staff-summary")]
        [HttpGet("staff/subdepts/{id}")]
        public SubDepartmentResponse GetSubDepartments(int id)
        {
            return _manager.GetSubDepartments(id);
        }

        [Right("staff-summary")]
        [HttpGet("staff/states/{id}")]
        public StatesResponse GetStates(int id)
        {
            return _locationManager.GetStates(id);
        }

        [Right("staff-summary")]
        [HttpGet("staff/cities/{id}")]
        public CitiesResponse GetCities(int id)
        {
            return _locationManager.GetCities(id);
        }

        [Right("staff-summary")]
        [HttpPost("staff/save")]
        public async Task<SaveStaffResponse> SaveStaff([FromBody] StaffDetails request)
        {
            request.UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return await _manager.SaveStaff(request);
        }

        [Right("staff-summary")]
        [HttpGet("staff/photo/{id}")]
        public async Task<HrPhotoResponse> GetPicture(int id)
        {
            return await _manager.GetPhotoAsync(id);
        }

        [Right("holiday-master", "show-holiday")]
        [HttpGet("holiday-master")]
        public HolidayMasterResponse GetHolidayMasterSummary()
        {
            return _manager.GetHolidayDataMaster();
        }

        [Right("holiday-master", "show-holiday")]
        [HttpPost("holidays/search")]
        public async Task<HolidayDetailsResponse> GetHolidaysDetails(HolidayDetaisRequest request)
        {
            if (request.Index == null)
                request.Index = 1;

            if (request.PageSize == null)
                request.PageSize = 10;

            return await _manager.GetHolidayDetails(request);
        }

        [Right("holiday-master")]
        [HttpPost("holiday/edit")]
        public async Task<EditHolidayResponse> AddOrEditHoliday(EditHolidayRequest request)
        {
            return await _manager.AddOrEditHoliday(request);
        }

        [Right("holiday-master")]
        [HttpPost("holiday/delete")]
        public async Task<EditHolidayResponse> DeleteHoliday([FromBody] DeleteHolidayRequest request)
        {
            return await _manager.DeleteHoliday(request);
        }

        [Right("office-control")]
        [HttpGet("OfficesControl")]
        public async Task<OfficesControlResponse> GetOfficesControl()
        {
            return await _manager.GetOfficesControl();
        }

        [Right("office-control")]
        [HttpGet("OfficesControl/{id}")]
        public async Task<OfficeControlStaffResponse> GetOfficesControlByStaffId(int id)
        {
            return await _manager.GetOfficesControlByStaffId(id);
        }

        [Right("office-control")]
        [HttpPost("OfficesControl/save")]
        public async Task<SaveOfficeControlResponse> SaveOfficeControl([FromBody] SaveOfficeControlRequest request)
        {
            return await _manager.SaveOfficeControl(request);
        }

        [Right("staff-summary")]
        [HttpGet("leave-request")]
        public async Task<LeaveResponse> GetLeaveRequest()
        {
            return await _manager.GetLeaveRequest(null);
        }

        [Right("staff-summary")]
        [HttpGet("leave-request/{id}")]
        public async Task<LeaveResponse> GetLeaveRequestBydId(int id)
        {
            return await _manager.GetLeaveRequest(id);
        }

        [Right("staff-summary")]
        [HttpPost("leave/save")]

        public async Task<SaveLeaveRequestResponse> SaveLeaveRequest(LeaveRequest request, [FromServices] IBroadCastService broadCastService)
        {
            if (request.StartDate == null)
                return new SaveLeaveRequestResponse { Result = SaveLeaveRequestResult.StartDateIsRequired };

            if (request.EndDate == null)
                return new SaveLeaveRequestResponse { Result = SaveLeaveRequestResult.EndDateIsRequired };

            if (request.LeaveTypeId <= 0)
                return new SaveLeaveRequestResponse { Result = SaveLeaveRequestResult.LeaveTypeIsRequired };

            var response = await _manager.SaveLeaveRequest(request);


            if (response.Result == SaveLeaveRequestResult.Success && response.SendNotification && response.EmailModel != null)
            {
                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };

                // send email
                var emailLogRequest = new EmailLogData()
                {
                    ToList = response.EmailModel.RecepientEmail,
                    Cclist = response.EmailModel.RecepientCCEmail,
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "Leave Request",
                    SourceId = response.Id,
                    Subject = $"Leave Request : {response.EmailModel.SenderName}"
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/LeaveRequest", response.EmailModel);

                await PublishQueueMessage(emailQueueRequest, emailLogRequest);

                // Broadcast message to manager
                broadCastService.Broadcast(response.EmailModel.RecepiendUserId, new DTO.Common.Notification
                {
                    Title = "LINK Tasks Manager",
                    Message = $"Leave request from {response.EmailModel.SenderName}",
                    Url = response.EmailModel.WebSite,
                    TypeId = "Task"
                });

            }

            return response;
        }

        [Right("leave-summary")]
        [HttpGet("leave-summary")]
        public async Task<LeaveSummaryResponse> GetLeaveSummary()
        {
            return await _manager.GetLeaveSummary();

        }

        [Right("leave-summary")]
        [HttpPost("leave-summary")]
        public async Task<LeaveSummaryDataResponse> GetLeaveDataSummary(LeaveSummaryRequest request)
        {
            return await _manager.GetLeaveDataSummary(request);
        }

        [Right("leave-summary")]
        [HttpPost("leave/export")]
        public async Task<IActionResult> ExportDataSummary(LeaveSummaryRequest request)
        {
            request.Index = 1;
            request.PageSize = 99999;

            var response = await _manager.GetLeaveDataSummary(request);

            if (response.Result != LeaveSummaryDataResult.Success)
                return NotFound();

            var model = new LeaveSummaryModel { Data = response.Data, Request = request };

            return await this.FileAsync("LeaveSummary", model, Components.Core.entities.FileType.Excel);
        }


        [Right("leave-summary")]
        [HttpGet("leave/status/{id}/{idStatus}")]
        public async Task<bool> SetStatus(int id, int idStatus, [FromServices] IBroadCastService broadCastService)
        {
            var email = await _manager.SetLeaveStatus(id, (Entities.Enums.LeaveStatus)idStatus, "");

            if (email != null)
            {
                EmailRequest emailRequest = null;

                if (idStatus == (int)Entities.Enums.LeaveStatus.Approved)
                {
                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = email.ReceipentEmail,
                        Cclist = (email.LeaveStaffList != null && email.LeaveStaffList.Count() > 0) ? email.LeaveStaffList.Select(x => x.StaffEmail).Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Leave Approved",
                        SourceId = id,
                        Subject = $"Leave Approved from {email.StartDate} to {email.EndDate} "
                    };

                    emailLogRequest.Body = this.GetEmailBody("Emails/LeaveApprove", email);

                    await PublishQueueMessage(emailQueueRequest, emailLogRequest);

                    // Broadcast message to user
                    broadCastService.Broadcast(email.UserId, new DTO.Common.Notification
                    {
                        Title = "LINK Notification Manager",
                        Message = $"Leave Approved from {email.StartDate} to {email.EndDate}",
                        Url = email.Url,
                        TypeId = "Notification"
                    });

                    // broadcast to Hr Leave users 
                    foreach (var item in email.LeaveStaffList)
                    {
                        foreach (var user in item?.UserIdList)
                        {
                            broadCastService.Broadcast(email.UserId, new DTO.Common.Notification
                            {
                                Title = "LINK Notification Manager",
                                Message = $"Leave Approved For {email.StaffName} from {email.StartDate} to {email.EndDate}",
                                Url = email.Url,
                                TypeId = "Notification"
                            });
                        }
                    }
                }
                else if (idStatus == (int)Entities.Enums.LeaveStatus.Cancelled)
                {
                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = email.ReceipentEmail,
                        Cclist = email.ReceipentCCEmail,
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Leave Cancelled",
                        SourceId = id,
                        Subject = $"Leave Cancelled - {email.UserName} ",
                    };

                    if (email.IsCancelledAfterApproval)
                        emailLogRequest.Body = this.GetEmailBody("Emails/LeaveCancelAfterApproval", email);
                    else
                        emailLogRequest.Body = this.GetEmailBody("Emails/LeaveCancel", email);

                    await PublishQueueMessage(emailQueueRequest, emailLogRequest);

                    // Broadcast message to user
                    broadCastService.Broadcast(email.UserId, new DTO.Common.Notification
                    {
                        Title = "LINK Notification Manager",
                        Message = $"Leave cancelled - {email.UserName} ",
                        Url = email.Url,
                        TypeId = "Notification"
                    });

                }


            }

            return true;
        }

        [Right("leave-summary")]
        [HttpPost("leave/reject/{id}")]
        public async Task<bool> SetStatus(int id, ExpenseReject request, [FromServices] IBroadCastService broadCastService)
        {
            var email = await _manager.SetLeaveStatus(id, Entities.Enums.LeaveStatus.Rejected, request.Comment);

            if (email != null)
            {
                // send email
                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };

                var emailLogRequest = new EmailLogData()
                {
                    ToList = email.ReceipentEmail,
                    Cclist = email.ReceipentCCEmail != null ? email.ReceipentCCEmail : "",
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "Leave Rejected",
                    SourceId = id,
                    Subject = $"Leave Rejected from {email.StartDate} to {email.EndDate} ",
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/LeaveReject", email);

                await PublishQueueMessage(emailQueueRequest, emailLogRequest);


                // Broadcast message to manager
                broadCastService.Broadcast(email.UserId, new DTO.Common.Notification
                {
                    Title = "LINK Notification Manager",
                    Message = $"Leave Rejected from {email.StartDate} to {email.EndDate}",
                    Url = email.Url,
                    TypeId = "Notification"
                });
            }

            return true;
        }

        [Right("leave-summary")]
        [HttpPost("leave/days")]
        public async Task<LeaveDaysResponse> GetLeaveDays(LeaveDaysRequest request)
        {
            return await _manager.GetLeaveDays(request);
        }

        [HttpGet("GetAuditor")]
        [Right("staff-summary")]
        public async Task<AuditorResponse> GetAuditor()
        {
            return await _manager.GetAuditors();
        }

        [Right("staff-summary")]
        [HttpGet("GetUserGender")]
        public async Task<StaffGenderResponse> GetUserGender()
        {
            return await _manager.GetGender();
        }

        [HttpGet("profileList")]
        [Right("staff-summary")]
        public async Task<HRProfileResponse> GetProfileList()
        {
            return await _manager.GetProfileList();
        }
        /// <summary>
        /// Save email data into log table and publish to queue
        /// </summary>
        /// <param name="emailQueueRequest"></param>
        /// <param name="emailLogRequest"></param>
        /// <returns></returns>
        private async Task PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest)
        {
            var resultId = await _emailLogQueueManager.AddEmailLog(emailLogRequest);
            emailQueueRequest.EmailQueueId = resultId;
            await _rabbitMQClient.Publish<EmailDataRequest>(_configuration["EmailQueue"], emailQueueRequest);
        }

        [HttpPost("get-staff-details")]
        public async Task<DataSourceResponse> GetStaffDataSource(StaffDataSourceRequest request)
        {
            return await _manager.GetStaffDataSource(request);
        }

        [HttpGet("get-status-list")]
        public async Task<DataSourceResponse> GetStaffStatusList()
        {
            return await _manager.GetStaffStatusList();
        }

        [HttpGet("get-social-insurance-type-list")]
        public async Task<DataSourceResponse> GetSocialInsuranceTypeList()
        {
            return await _manager.GetSocialInsuranceTypeList();
        }

        [HttpGet("get-band-list")]
        public async Task<DataSourceResponse> GetBandList()
        {
            return await _manager.GetBandList();
        }

        [HttpPost("get-huko-location-list")]
        public async Task<DataSourceResponse> GetHukoLocationList(CommonDataSourceRequest request)
        {
            return await _manager.GetHukoLocationList(request);
        }

        [HttpGet("{idOffice}/staffs")]
        public async Task<StaffSearchResponse> GetStaffsByOffice(int idOffice)
        {
            return await _manager.GetStaffsByOffice(idOffice);
        }

        [HttpPost("get-staff-summary")]        
        public async Task<DataSourceResponse> GetStaffUserDataSource([FromBody] UserDataSourceRequest request)
        {
            return await _manager.GetStaffUserDataSource(request);
        }

        [Right("staff-summary")]
        [HttpPost("save-hr-outsource-company")]
        public async Task<SaveHROutSourceResponse> SaveHROutSourceCompany(SaveHROutSourceRequest request)
        {
            return await _manager.SaveHROutSourceCompany(request);
        }

        [Right("staff-summary")]
        [HttpPost("get-hr-outsource-company-list")]
        public async Task<HROutSourceCompanyListResponse> GetHROutSourceCompanyList(HROutSourceCompanyRequest request)
        {
            return await _manager.GetHROutSourceCompanyList(request);
        }

        [Right("staff-summary")]
        [HttpGet("get-all-hr-outsource-company-list")]
        public async Task<HROutSourceCompanyListResponse> GetHROutSourceCompanyList()
        {
            return await _manager.GetHROutSourceCompanyList();
        }

        [Right("staff-summary")]
        [HttpPost("get-stafflist-by-company")]
        public async Task<HRStaffResponse> GetStaffListByOutSourceCompany(List<int> companyIds)
        {
            return await _manager.GetStaffListByOutSourceCompanyIds(companyIds);
        }

        [Right("staff-summary")]
        [HttpGet("get-hr-payroll-company-list")]
        public async Task<HRPayrollCompanyListResponse> GetHRPayrollCompanyList()
        {
            return await _manager.GetHRPayrollCompanyList();
        }
    }
}