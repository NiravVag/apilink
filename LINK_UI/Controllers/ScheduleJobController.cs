using BI.Utilities;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.Common;
using DTO.DataAccess;
using DTO.EmailLog;
using DTO.File;
using DTO.Kpi;
using DTO.ScheduleJob;
using Entities;
using Entities.Enums;
using LINK_UI.App_start;
using LINK_UI.Controllers.EXTERNAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class ScheduleJobController : ExternalBaseController
    {
        private readonly IScheduleJobManager _scheduleJobManager = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private static IConfiguration _configuration = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IUserRightsManager _userRightManager = null;
        private readonly IKpiCustomManager _kpiCustomManager = null;
        private readonly ISharedInspectionManager _sharedInspectionManager = null;
        private readonly IHelper _helper = null;
        private readonly ITenantProvider _filterService = null;
        public ScheduleJobController(IScheduleJobManager scheduleJobManager,
            IEmailLogQueueManager emailLogQueueManager, IRabbitMQGenericClient rabbitMQClient, IUserRightsManager userRightManager,
            IConfiguration configuration, IKpiCustomManager kpiCustomManager, ISharedInspectionManager sharedInspectionManager, ITenantProvider filterService, IHelper helper)
        {
            _scheduleJobManager = scheduleJobManager;
            _emailLogQueueManager = emailLogQueueManager;
            _rabbitMQClient = rabbitMQClient;
            _configuration = configuration;
            _userRightManager = userRightManager;
            _kpiCustomManager = kpiCustomManager;
            _sharedInspectionManager = sharedInspectionManager;
            _filterService = filterService;
            _helper = helper;
        }

        [HttpGet("getCulturaPackingInfo")]
        public async Task<bool> Get()
        {
            return await _scheduleJobManager.SaveCulturaPackingInfo();
        }

        [HttpGet("GetScheduleAutoQcExpense")]
        public async Task<bool> GetScheduleAutoQcExpense()
        {
            return await _scheduleJobManager.SaveScheduleAutoQcExpenseInfo(new List<int> { }, new List<int> { });
        }

        [HttpGet("SendTravelTariffEmail")]
        public async Task<bool> SendTravelTariffEmail()
        {

            var travelTariffList = await _scheduleJobManager.GetInActivatTravelTariffList();

            var userAccessFilter = new UserAccess
            {
                RoleId = (int)RoleEnum.AutoQCExpenseAccounting
            };

            var userListByRoleAccess = await _userRightManager.GetUserListByRoleOffice(userAccessFilter);

            string emailAddressList = string.Join(";", userListByRoleAccess.
                                      Where(x => x.EmailAddress != null).Select(x => x.EmailAddress).Distinct().ToList());

            if (travelTariffList.Any())
            {
                var response = new ScheduleTravelTariffEmailResponse();
                response.TravelTariffList = travelTariffList;
                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };

                var emailLogRequest = new EmailLogData()
                {
                    ToList = emailAddressList,
                    Cclist = emailAddressList,
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "EmailJob - Travel Tariff ",
                    Subject = $"Travel Tariff Missing"
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/TravelTariff/TravelTariff", response);
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }

            return true;
        }


        [HttpGet("SendCarrefourDailyResultEmail")]
        public async Task<bool> SendCarrefourDailyResultEmail()
        {

            var jobConfigurationList = await _scheduleJobManager.getJobConfigurationList();
            var jobConfigurationData = jobConfigurationList.FirstOrDefault(x => x.Type == (int)ScheduleOptions.ScheduleCarrefourDailyResult);

            if (jobConfigurationData != null)
            {
                var response = new ScheduleJobCarrfourDailyResultEmailResponse();
                response.JobDataList = new List<JobConfiguration>() { jobConfigurationData };
                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };

                string emailToAddressList = jobConfigurationData.To.ToString();

                string emailCCAddressList = jobConfigurationData.Cc.ToString();

                var emailLogRequest = new EmailLogData()
                {
                    ToList = emailToAddressList,
                    Cclist = emailCCAddressList,
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "EmailJob - Carrfour daily Result ",
                    Subject = $"API Daily Report List"
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/EmailJob/CarrefourDailyResult", response);

                var fileCloudUrl = await UploadCarrefourDailyResultToCloudAndReturnFileUrl(jobConfigurationData);

                if (!string.IsNullOrWhiteSpace(fileCloudUrl))
                {
                    emailLogRequest.FileList = new List<FileResponse>()
                    {
                        new FileResponse()
                        {
                            FileLink = fileCloudUrl,
                            MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            FileStorageType= (int)FileStorageType.Link,
                            Name = "CarrefourDailyResultTemplate.xlsx"
                        }
                    };
                }

                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }

            return true;
        }

        private byte[] ToByteArray(Stream stream)
        {
            using (stream)
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    stream.CopyTo(memStream);
                    return memStream.ToArray();
                }
            }
        }

        [HttpGet("GetScheduleQcExpenseEmail")]
        public async Task<bool> GetScheduleQcExpenseEmail()
        {

            var qcExpenseList = await _scheduleJobManager.GetQcExpenseList();
            var tomorrowDate = DateTime.Today.AddDays(1).ToString(ApiCommonData.StandardDateFormat);

            var userAccessFilter = new UserAccess
            {
                RoleId = (int)RoleEnum.AutoQCExpenseAccounting
            };

            var userListByRoleAccess = await _userRightManager.GetUserListByRoleOffice(userAccessFilter);

            string emailAddressList = string.Join(";", userListByRoleAccess.
                                      Where(x => x.EmailAddress != null).Select(x => x.EmailAddress).Distinct().ToList());

            if (qcExpenseList.Any())
            {

                var groupExpensebyQc = qcExpenseList.GroupBy(x => new { x.QcId }).Select(y => y).ToList();

                foreach (var groupItems in groupExpensebyQc)
                {
                    var qcEmail = groupItems.FirstOrDefault()?.QcEmail;
                    if (!string.IsNullOrEmpty(qcEmail))
                    {
                        var expenseList = new List<QcExpenseEmailData>();

                        foreach (var expenseItem in groupItems)
                        {
                            expenseList.Add(expenseItem);
                        }

                        var response = new QcExpenseEmailResponse();
                        response.QcExpenseEmailList = expenseList;

                        var emailQueueRequest = new EmailDataRequest
                        {
                            TryCount = 1,
                            Id = Guid.NewGuid()
                        };

                        var emailLogRequest = new EmailLogData()
                        {
                            ToList = qcEmail,
                            Cclist = emailAddressList,
                            TryCount = 1,
                            Status = (int)EmailStatus.NotStarted,
                            SourceName = "EmailJob - Qc Expense ",
                            Subject = $"Inspection details of Tomorrow  (" + tomorrowDate + ")"
                        };

                        emailLogRequest.Body = this.GetEmailBody("Emails/QcExpense/QcExpenseData", response);
                        await PublishQueueMessage(emailQueueRequest, emailLogRequest);
                    }
                }
            }

            return true;
        }

        [HttpPost("GetQcPendingExpenseData")]
        public async Task<QcPendingExpenseResponse> GetQcPendingExpenseData(QcPendingExpenseRequest request)
        {
            var response = new QcPendingExpenseResponse();
            int skip = (request.Index.Value - 1) * request.PageSize.Value;
            int take = request.PageSize.Value;

            var pendingExpenseList = await _scheduleJobManager.GetPendingQcExpenseList(request);
            response.TotalCount = pendingExpenseList.Count;
            var pendingExpenseListData = pendingExpenseList.Skip(skip).Take(take).ToList();

            if (pendingExpenseList.Any())
            {
                response.Result = QcPendingExpenseDataResult.Success;
                response.QcPendingExpenseData = pendingExpenseListData;
            }
            else
            {
                response.Result = QcPendingExpenseDataResult.NotFound;
                response.QcPendingExpenseData = new List<QcPendingExpenseData>();
            }
            response.Index = request.Index.Value;
            response.PageSize = request.PageSize.Value;
            response.PageCount = (response.TotalCount / request.PageSize.Value) + (response.TotalCount % request.PageSize.Value > 0 ? 1 : 0);


            return response;
        }

        [HttpPost("SaveQcPendingExpenseData")]
        public async Task<bool> SaveQcPendingExpenseData(List<QcPendingExpenseData> pendingExpense)
        {
            if (pendingExpense.Any())
            {
                var travelExpenseIds = pendingExpense.Where(x => x.ExpenseTypeId == (int)AutoQcExpenseType.TravellAllowance).Select(x => x.Id).ToList();
                var foodExpenseIds = pendingExpense.Where(x => x.ExpenseTypeId == (int)AutoQcExpenseType.FoodAllowance).Select(x => x.Id).ToList();
                return await _scheduleJobManager.SaveScheduleAutoQcExpenseInfo(travelExpenseIds, foodExpenseIds);
            }
            return false;
        }
        [HttpGet("SendClaimReminderEmail")]
        public async Task<bool> SendClaimReminderEmail()
        {
            //Claim Reminder Analyze email 
            var claimQuery = await _scheduleJobManager.GetAllClaims();

            var response = new ClaimReminderEmailResponse();

            await SendReminderEmailByRegisteredStatus(claimQuery, response);
            await SendReminderEmailByAnalyzedStatus(claimQuery, response);
            await SendReminderEmailByValidatedStatus(claimQuery, response);

            return true;
        }

        [HttpGet("PushReportInfoToFastReport")]
        public async Task<bool> PushReportInfoToFastReport()
        {
            var fbToken = getFbToken();
            var response = await _scheduleJobManager.PushReportInfoToFastReport(fbToken);
            return response.Result;
        }

        [HttpGet("UploadInspectionAttachementsAsZipToCloud")]
        public async Task UploadInspectionAttachementsAsZipToCloud()
        {
            await _scheduleJobManager.UploadInspectionAttachementsAsZipToCloud(null);
        }

        [HttpGet("SendSkipMSchartEmail")]
        public async Task<bool> SendSkipMSchartEmail()
        {
            var jobConfigurationList = await _scheduleJobManager.getJobConfigurationList();
            jobConfigurationList = jobConfigurationList.Where(x => x.Type == (int)ScheduleOptions.ScheduleMissedMSchart).ToList();

            if (jobConfigurationList.Any())
            {
                string emailToAddressList = string.Join(",", jobConfigurationList.Where(x => x.To != null).Select(x => x.To).Distinct().ToList());
                string emailCCAddressList = string.Join(";", jobConfigurationList.Where(x => x.Cc != null).Select(x => x.Cc).Distinct().ToList());

                var inspectionList = await _scheduleJobManager.ScheduleSkipMSchart();

                inspectionList = inspectionList.Where(x => x.ProductList != null && x.ProductList.Any()).ToList();

                var bookingIds = inspectionList.Select(x => x.InspectionId).Distinct().ToList();

                var inspectionCsList = await _scheduleJobManager.GetBookingCSItemList(bookingIds);

                var customerServiceList = inspectionCsList.Select(x => x.CsId).Distinct().ToList();

                if (customerServiceList != null && customerServiceList.Any())
                {
                    foreach (var customerServiceId in customerServiceList)
                    {

                        var customerServiceBookings = inspectionCsList.Where(x => x.CsId == customerServiceId).Select(x => x.BookingId).ToList();
                        var csInspectionList = inspectionList.Where(x => customerServiceBookings.Contains(x.InspectionId)).ToList();

                        var response = _scheduleJobManager.ScheduleSkipMSchartForCustomer(csInspectionList);
                        if (response.InspectionList != null && response.InspectionList.Any())
                        {
                            var emailQueueRequest = new EmailDataRequest
                            {
                                TryCount = 1,
                                Id = Guid.NewGuid()
                            };

                            var emailLogRequest = new EmailLogData()
                            {
                                ToList = emailToAddressList,
                                Cclist = emailCCAddressList,
                                TryCount = 1,
                                Status = (int)EmailStatus.NotStarted,
                                SourceName = "EmailMSchart - Missed MSchart",
                                Subject = $"Inspection Schedule ({response.FromDate} - {response.ToDate}) for MS chart missed -{response.CustomerName}"
                            };
                            emailLogRequest.Body = this.GetEmailBody("Emails/ScheduleSkipMSchart/ScheduleSkipMSchart", response);
                            await PublishQueueMessage(emailQueueRequest, emailLogRequest);
                        }
                    }
                }
            }

            return true;
        }

        [HttpGet("ScheduleBookingCs")]
        public async Task<bool> ScheduleBookingCs()
        {
            List<string> jobConfigurationToList = new List<string>();
            List<string> jobConfigurationCcList = new List<string>();
            var jobConfigurationList = await _scheduleJobManager.GetJobConfigurations(new List<int>() { (int)ScheduleOptions.ScheduleBookingCS });

            if (jobConfigurationList != null && jobConfigurationList.Any())
            {
                jobConfigurationToList = jobConfigurationList.Where(x => x.To != null).Select(x => x.To).Distinct().ToList();
                jobConfigurationCcList = jobConfigurationList.Where(x => x.Cc != null).Select(x => x.Cc).Distinct().ToList();
            }
            var staffs = await _scheduleJobManager.ScheduleBookingCS();

            foreach (var staff in staffs)
            {
                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };

                jobConfigurationToList.Add(staff.CSEmail);

                var emailLogRequest = new EmailLogData()
                {
                    ToList = string.Join(";", jobConfigurationToList),
                    Cclist = string.Join(";", jobConfigurationCcList),
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "",
                    Subject = $"Report checking checklist ({staff.ScheduleDate}) - {staff.CSName}"
                };
                emailLogRequest.Body = this.GetEmailBody("Emails/Scheduling/ScheduleEmailToCS", staff);
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
                jobConfigurationToList.Remove(staff.CSEmail);
            }
            return true;

        }

        [HttpGet("SchedulePlanningForCSEmail")]
        public async Task<IActionResult> SchedulePlanningForCSEmail(string offices = "", string configureId = "")
        {
            var response = await _scheduleJobManager.SchedulePlanningForCS(offices, configureId);
            if (response.statusCode == HttpStatusCode.OK)
            {
                var data = response.data;
                var fileCloudUrl = await UploadSchedulePlanningForCSResultToCloudAndReturnFileUrl(response.data);
                if (!string.IsNullOrWhiteSpace(fileCloudUrl))
                {
                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = data.JobConfiguration.To,
                        Cclist = data.JobConfiguration.Cc,
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Schedule Planning For CS",
                        Subject = data.EntityName + "updated schedule planning from " + data.FromDate.ToString("dd/MM/yyyy") + " to " + data.ToDate.ToString("dd/MM/yyyy") + " - " + data.JobConfiguration.Name
                    };
                    emailLogRequest.Body = this.GetEmailBody("Emails/SchedulePlanningForCS/SchedulePlanningForCS", response.data);
                    emailLogRequest.FileList = new List<FileResponse>()
                    {
                        new FileResponse()
                        {
                            FileLink = fileCloudUrl,
                            MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            FileStorageType= (int)FileStorageType.Link,
                            Name = data.JobConfiguration.FileName + "_" + data.FromDate.ToString("ddMMyyyy") + "_" + data.ToDate.ToString("ddMMyyyy") + ".xlsx"
                        }
                    };
                    await PublishQueueMessage(emailQueueRequest, emailLogRequest);
                }
                else
                {
                    response.errors = new List<string>() { "no record found" };
                    response.message = "Not Found";
                    response.statusCode = HttpStatusCode.NotFound;
                    response.data = null;
                }
            }

            return BuildCommonResponse(response.statusCode, response);
        }

        [HttpGet("BBGInitialBookingExtractEmail")]
        public async Task<IActionResult> BBGInitialBookingExtractEmail()
        {
            var response = await _scheduleJobManager.BBGInitialBookingExtractEmail();
            if (response.statusCode == HttpStatusCode.OK)
            {
                var data = response.data;
                var fileCloudUrl = await UploadBBGInitialBookingExtractResultToCloudAndReturnFileUrl(response.data);
                if (!string.IsNullOrWhiteSpace(fileCloudUrl))
                {
                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = data.JobConfiguration.To,
                        Cclist = data.JobConfiguration.Cc,
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "BBG Initial Booking Extract",
                        Subject = "Initial booking extract " + data.FromDate.ToString("dd/MM/yyyy") + " - " + data.ToDate.ToString("dd/MM/yyyy")
                    };
                    emailLogRequest.Body = this.GetEmailBody("Emails/BBGInitialBookingExtract/BBGInitialBookingExtract", response.data);
                    emailLogRequest.FileList = new List<FileResponse>()
                    {
                        new FileResponse()
                        {
                            FileLink = fileCloudUrl,
                            MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            FileStorageType= (int)FileStorageType.Link,
                            Name = data.JobConfiguration.FileName + " " + data.FromDate.ToString("ddMMyyyy") + "-" + data.ToDate.ToString("ddMMyyyy") + ".xlsx"
                        }
                    };
                    await PublishQueueMessage(emailQueueRequest, emailLogRequest);
                }
                else
                {
                    response.errors = new List<string>() { "no record found" };
                    response.message = "Not Found";
                    response.statusCode = HttpStatusCode.NotFound;
                    response.data = null;
                }
            }
            return BuildCommonResponse(response.statusCode, response);
        }

        /// <summary>
        /// Publish to Queue
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

        private async Task SendReminderEmailByRegisteredStatus(IQueryable<ClmTransaction> claimQuery, ClaimReminderEmailResponse response)
        {
            var claimRegisterData = await _scheduleJobManager.GetClaimMailDetail(claimQuery, (int)ClaimStatus.Registered);
            if (claimRegisterData != null && claimRegisterData.Any())
            {
                var claimRegisteredUserIds = claimRegisterData.Select(x => x.CreatedBy).Distinct().ToList();
                var claimRegistersEmails = await _scheduleJobManager.GetUserEmailsByIds(claimRegisteredUserIds);

                var claimRegistersOfficeIds = claimRegisterData.Select(x => x.OfficeId).Distinct().ToList();
                foreach (var claimRegistersOfficeId in claimRegistersOfficeIds)
                {
                    var userAccessFilter = new UserAccess
                    {
                        OfficeId = claimRegistersOfficeId,
                        ServiceId = null,
                        RoleId = (int)RoleEnum.ClaimAnalyze
                    };

                    var userListByRoleAccess = await _userRightManager.GetUserListByRoleOffice(userAccessFilter);
                    var claimRegistersEmailAddressList = userListByRoleAccess.Select(x => x.EmailAddress).Distinct().ToList();

                    response.ScheduleClaimReminderEmailList = claimRegisterData;

                    string sourceName = "Claim Reminder Analyze Email";
                    string subject = $"Claim Reminder for Request status";

                    string body = this.GetEmailBody("Emails/Claim/ClaimReminderAnalyzed", response);
                    await CommonSendReminderEmail(claimRegistersEmailAddressList, claimRegistersEmails, sourceName, subject, body);
                }
            }
        }

        private async Task SendReminderEmailByAnalyzedStatus(IQueryable<ClmTransaction> claimQuery, ClaimReminderEmailResponse response)
        {
            var claimAnalyzeData = await _scheduleJobManager.GetClaimMailDetail(claimQuery, (int)ClaimStatus.Analyzed);
            if (claimAnalyzeData != null && claimAnalyzeData.Any())
            {
                var claimAnalyzedUserIds = claimAnalyzeData.Select(x => x.CreatedBy).Distinct().ToList();
                var claimAnalyzedEmails = await _scheduleJobManager.GetUserEmailsByIds(claimAnalyzedUserIds);

                var userByRole = await _userRightManager.GetRoleAccessUserList((int)RoleEnum.ClaimValidate);
                var claimAnalyzedEmailAddressList = userByRole.Where(x => x.EmailAddress != null).Select(x => x.EmailAddress).Distinct().ToList();

                response.ScheduleClaimReminderEmailList = claimAnalyzeData;

                string sourceName = "Claim Reminder Validate Email";
                string subject = $"Claim Reminder for Analyzed status";
                string body = this.GetEmailBody("Emails/Claim/ClaimReminderValidateEmail", response);

                await CommonSendReminderEmail(claimAnalyzedEmailAddressList, claimAnalyzedEmails, sourceName, subject, body);
            }
        }

        private async Task SendReminderEmailByValidatedStatus(IQueryable<ClmTransaction> claimQuery, ClaimReminderEmailResponse response)
        {
            var claimValidateData = await _scheduleJobManager.GetClaimMailDetail(claimQuery, (int)ClaimStatus.Validated);
            if (claimValidateData != null && claimValidateData.Any())
            {
                var claimValidatedUserIds = claimValidateData.Select(x => x.CreatedBy).Distinct().ToList();
                var claimValidatedEmails = await _scheduleJobManager.GetUserEmailsByIds(claimValidatedUserIds);
                var claimValidatedOfficeIds = claimValidateData.Select(x => x.OfficeId).Distinct().ToList();
                foreach (var claimValidatedOfficeId in claimValidatedOfficeIds)
                {
                    var userAccessFilter = new UserAccess
                    {
                        OfficeId = claimValidatedOfficeId,
                        ServiceId = null,
                        RoleId = (int)RoleEnum.ClaimAnalyze
                    };

                    var userListByRoleAccess = await _userRightManager.GetUserListByRoleOffice(userAccessFilter);
                    var claimValidatedEmailAddressList = userListByRoleAccess.Select(x => x.EmailAddress).Distinct().ToList();

                    response.ScheduleClaimReminderEmailList = claimValidateData;

                    string sourceName = "Claim Reminder Close Email";
                    string subject = $"Claim Reminder for Analyzed status";
                    string body = this.GetEmailBody("Emails/Claim/ClaimReminderCloseEmail", response);
                    await CommonSendReminderEmail(claimValidatedEmailAddressList, claimValidatedEmails, sourceName, subject, body);
                }
            }
        }

        private async Task CommonSendReminderEmail(List<string> ToUserEmailList, List<string> CCUserEmailList, string SourceName, string Subject, string Body)
        {
            try
            {
                var CCUserList = new List<string>();
                var ToUserList = new List<string>();

                if (ToUserEmailList != null)
                {
                    ToUserList.AddRange(ToUserEmailList);
                }
                if (CCUserEmailList != null)
                {
                    CCUserList.AddRange(CCUserEmailList);
                }

                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };
                var emailLogRequest = new EmailLogData()
                {
                    ToList = (ToUserList.Count > 0) ? ToUserList.Distinct().Aggregate((x, y) => x + ";" + y) : "",
                    Cclist = (CCUserList.Count > 0) ? CCUserList.Distinct().Aggregate((x, y) => x + ";" + y) : "",
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = SourceName,
                    Subject = Subject
                };

                emailLogRequest.Body = Body;

                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string getFbToken()
        {
            var Fbclaims = new List<Claim>
            {
                new Claim("email",_configuration["FbAdminEmail"]),
                new Claim("firstname", _configuration["FbAdminUserName"]),
                new Claim("lastname", ""),
                new Claim("role", "admin"),
                new Claim("redirect", "")
            };
            return AuthentificationService.CreateFBToken(Fbclaims, _configuration["FBKey"]);

        }

        private async Task<string> UploadCarrefourDailyResultToCloudAndReturnFileUrl(JobConfiguration jobConfiguration)
        {
            DateTime startDate;
            // Attachment data part
            if (jobConfiguration.StartDate != null)
            {
                startDate = DateTime.ParseExact(jobConfiguration.StartDate.Value.ToShortDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                startDate = DateTime.Now;
            }

            var endDate = startDate.AddDays(-1);
            if (startDate.DayOfWeek.ToString().ToLower() == ApiCommonData.Monday.ToLower())
                startDate = startDate.AddDays(-3);
            else
                startDate = startDate.AddDays(-1);

            var fromDate = new DateObject(startDate.Year, startDate.Month, startDate.Day);
            var toDate = new DateObject(endDate.Year, endDate.Month, endDate.Day);

            KpiRequest kpiRequest = new KpiRequest() { CustomerId = (int)CustomerEnum.CarrfourCustomer, TemplateId = (int)TemplateEnum.CarrefourDailyResultTemplate, FromDate = fromDate, ToDate = toDate };

            var reportCarrefourDailyResultResponse = await _kpiCustomManager.ExportInspResultSummaryTemplate(kpiRequest);
            var stream = _sharedInspectionManager.GetAsStreamObjectAndLoadDataTable(reportCarrefourDailyResultResponse);
            var fileData = File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CarrefourDailyResultTemplate.xlsx");


            // upload file to cloud
            string strReportPath = "";
            var multipartContent = new MultipartFormDataContent();
            var byteData = ToByteArray(fileData.FileStream);
            var byteContent = new ByteArrayContent(byteData);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            multipartContent.Add(byteContent, "files", Guid.NewGuid().ToString());

            int ContainerId = (int)FileContainerList.DevContainer;
            if (!Convert.ToBoolean(_configuration["IsDevelopment_Enviornment"]))
            {
                ContainerId = (int)FileContainerList.InspectionReport;
            }

            int EntityId = _filterService.GetCompanyId();
            var cloudFileUrl = _configuration["FileServer"] + "savefile/" + ContainerId.ToString() + "/" + EntityId.ToString();

            using (var httpClient = new HttpClient())
            {

                HttpResponseMessage dataResponse = httpClient.PostAsync(cloudFileUrl, multipartContent).Result;
                var StatusCode = dataResponse.StatusCode;
                if (StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = dataResponse.Content.ReadAsStringAsync().Result;

                    if (!string.IsNullOrEmpty(result))
                    {
                        var fileResultData = JsonConvert.DeserializeObject<FileUploadResponse>(result);

                        if (fileResultData != null && fileResultData.FileUploadDataList != null
                            && fileResultData.FileUploadDataList.FirstOrDefault() != null
                            && fileResultData.FileUploadDataList.FirstOrDefault().Result == FileUploadResponseResult.Sucess)
                        {
                            strReportPath = fileResultData.FileUploadDataList.FirstOrDefault().FileCloudUri;
                        }
                    }
                }
            }

            return strReportPath;
        }

        private async Task<string> UploadSchedulePlanningForCSResultToCloudAndReturnFileUrl(SchedulePlanningForCS response)
        {
            var schedulePlanningForCSFileData = await _scheduleJobManager.SchedulePlanningForCSFileData(response);
            if (!schedulePlanningForCSFileData.Any())
                return null;

            var fileName = response.JobConfiguration.FileName + "_" + response.FromDate.ToString("ddMMyyyy") + "_" + response.ToDate.ToString("ddMMyyyy") + ".xlsx";
            Stream stream = _sharedInspectionManager.GetAsStreamObject(schedulePlanningForCSFileData);
            var fileData = File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

            // upload file to cloud
            string strReportPath = "";
            var multipartContent = new MultipartFormDataContent();
            var byteData = ToByteArray(fileData.FileStream);
            var byteContent = new ByteArrayContent(byteData);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            multipartContent.Add(byteContent, "files", Guid.NewGuid().ToString());

            int ContainerId = (int)FileContainerList.DevContainer;
            if (!Convert.ToBoolean(_configuration["IsDevelopment_Enviornment"]))
            {
                ContainerId = (int)FileContainerList.ScheduleJob;
            }

            int EntityId = _filterService.GetCompanyId();
            
            var cloudFileUrl = _configuration["FileServer"] + $"savefile/{ContainerId}/{EntityId}";

            using (var httpClient = new HttpClient())
            {

                HttpResponseMessage dataResponse = httpClient.PostAsync(cloudFileUrl, multipartContent).Result;
                var StatusCode = dataResponse.StatusCode;
                if (StatusCode == HttpStatusCode.OK)
                {
                    string result = dataResponse.Content.ReadAsStringAsync().Result;

                    if (!string.IsNullOrEmpty(result))
                    {
                        var fileResultData = JsonConvert.DeserializeObject<FileUploadResponse>(result);

                        if (fileResultData != null && fileResultData.FileUploadDataList != null && fileResultData.FileUploadDataList.First().Result == FileUploadResponseResult.Sucess)
                        {
                            strReportPath = fileResultData.FileUploadDataList.First().FileCloudUri;
                        }
                    }
                }
            }

            return strReportPath;
        }

        private async Task<string> UploadBBGInitialBookingExtractResultToCloudAndReturnFileUrl(InitialBookingExtract responseData)
        {
            var fromDate = new DateObject(responseData.FromDate.Year, responseData.FromDate.Month, responseData.FromDate.Day);
            var toDate = new DateObject(responseData.ToDate.Year, responseData.ToDate.Month, responseData.ToDate.Day);

            KpiRequest kpiRequest = new KpiRequest() { CustomerIdList = responseData.CustomerIds, TemplateId = (int)KPICustomTemplate.InspectionData, FromDate = fromDate, ToDate = toDate };
            var initialBookingExtractResultResponse = await _kpiCustomManager.ExportInspectionData(kpiRequest);

            if (initialBookingExtractResultResponse == null)
                return null;

            EnumerableRowCollection<DataRow> query = from row in initialBookingExtractResultResponse.AsEnumerable()
                                                     orderby DateTime.ParseExact(row.Field<string>("ServiceToDate"), ApiCommonData.StandardDateFormat, null) ascending
                                                     select row;
            initialBookingExtractResultResponse = query.AsDataView().ToTable();

            string fileName = responseData.JobConfiguration.FileName +" " + responseData.FromDate.ToString("ddMMyyyy") + "-" + responseData.ToDate.ToString("ddMMyyyy") + ".xlsx";
            Stream stream = _sharedInspectionManager.GetAsStreamObjectAndLoadDataTable(initialBookingExtractResultResponse);
            var fileData = File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

            // upload file to cloud
            string strFilePath = "";
            var multipartContent = new MultipartFormDataContent();
            var byteData = ToByteArray(fileData.FileStream);
            var byteContent = new ByteArrayContent(byteData);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            multipartContent.Add(byteContent, "files", Guid.NewGuid().ToString());

            int ContainerId = (int)FileContainerList.DevContainer;
            if (!Convert.ToBoolean(_configuration["IsDevelopment_Enviornment"]))
            {
                ContainerId = (int)FileContainerList.ScheduleJob;
            }

            int EntityId = _filterService.GetCompanyId();

            var cloudFileUrl = _configuration["FileServer"] + $"savefile/{ContainerId}/{EntityId}";

            using (var httpClient = new HttpClient())
            {

                HttpResponseMessage dataResponse = httpClient.PostAsync(cloudFileUrl, multipartContent).Result;
                var StatusCode = dataResponse.StatusCode;
                if (StatusCode == HttpStatusCode.OK)
                {
                    string result = dataResponse.Content.ReadAsStringAsync().Result;

                    if (!string.IsNullOrEmpty(result))
                    {
                        var fileResultData = JsonConvert.DeserializeObject<FileUploadResponse>(result);

                        if (fileResultData != null && fileResultData.FileUploadDataList != null && fileResultData.FileUploadDataList.First().Result == FileUploadResponseResult.Sucess)
                        {
                            strFilePath = fileResultData.FileUploadDataList.First().FileCloudUri;
                        }
                    }
                }
            }

            return strFilePath;
        }
    }
}