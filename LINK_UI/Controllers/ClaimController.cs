using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.Claim;
using DTO.Common;
using DTO.CommonClass;
using DTO.DataAccess;
using DTO.EmailLog;
using DTO.MasterConfig;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimManager _manager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private static IConfiguration _configuration = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly IUserRightsManager _userRightManager = null;
        private readonly ISharedInspectionManager _helper = null;
        private readonly IInspectionBookingManager _inspManager = null;
        public ClaimController(IClaimManager manager, IConfiguration configuration, IAPIUserContext applicationContext, IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IUserRightsManager userRightManager, ISharedInspectionManager helper, IInspectionBookingManager inspManager)
        {
            _manager = manager;
            _configuration = configuration;
            _applicationContext = applicationContext;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _userRightManager = userRightManager;
            _helper = helper;
            _inspManager = inspManager;
        }

        [HttpGet("claim-summary")]
        public async Task<ClaimSummaryResponse> GetClaimSummary()
        {
            return await _manager.GetClaimSummary();
        }

        [HttpPost("ClaimSummary")]
        [Right("claim-summary")]
        public async Task<ClaimSearchResponse> GetClaimDetails([FromBody] ClaimSearchRequest request)
        {
            var res = await _manager.GetClaimDetails(request);
            return res;
        }

        [Right("claim-summary")]
        [HttpPost("ExportClaimSummary")]
        public async Task<IActionResult> ExportClaimSummary([FromBody] ClaimSearchRequest request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.pageSize = PageSize;
            var response = await _manager.GetExportClaimDetails(request);
            var data = await _manager.ExportSummary(response.Data);
            if (response.Result != ClaimSearchResponseResult.Success)
                return NotFound();

            var stream = _helper.GetAsStreamObject(data);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClaimSummary.xls");
        }

        [HttpPost("get-booking-no")]
        public async Task<DataSourceResponse> GetBookingIdDataSource(CommonBookingIdSourceRequest request)
        {
            return await _manager.GetBookingIdDataSource(request);
        }

        [HttpGet("get-report-list/{bookingId}")]
        public async Task<DataSourceResponse> GetReportListByBooking(int bookingId)
        {
            return await _manager.GetReportListByBooking(bookingId);
        }

        [HttpPost("get-booking-data")]
        public async Task<ClaimBookingResponse> GetBookingData(BookingClaimRequest request)
        {
            return await _manager.GetBookingData(request);
        }

        [HttpGet("get-claim-from-list")]
        public async Task<DataSourceResponse> GetClaimFromList()
        {
            return await _manager.GetClaimFromList();
        }

        [HttpGet("get-received-from-list")]
        public async Task<DataSourceResponse> GetReceivedFromList()
        {
            return await _manager.GetReceivedFromList();
        }

        [HttpGet("get-claim-source-list")]
        public async Task<DataSourceResponse> GetClaimSourceList()
        {
            return await _manager.GetClaimSourceList();
        }

        [HttpGet("get-fb-defect-list")]
        public async Task<DataSourceResponse> GetFbDefectList()
        {
            return await _manager.GetFbDefectList();
        }

        [HttpGet("get-claim-department-list")]
        public async Task<DataSourceResponse> GetClaimDepartmentList()
        {
            return await _manager.GetClaimDepartmentList();
        }

        [HttpGet("get-claim-customer-request-list")]
        public async Task<DataSourceResponse> GetClaimCustomerRequestList()
        {
            return await _manager.GetClaimCustomerRequestList();
        }

        [HttpGet("get-claim-priority-list")]
        public async Task<DataSourceResponse> GetClaimPriorityList()
        {
            return await _manager.GetClaimPriorityList();
        }

        [HttpGet("get-claim-refund-type-list")]
        public async Task<DataSourceResponse> GetClaimRefundTypeList()
        {
            return await _manager.GetClaimRefundTypeList();
        }

        [HttpGet("get-claim-defect-distribution-list")]
        public async Task<DataSourceResponse> GetClaimDefectDistributionList()
        {
            return await _manager.GetClaimDefectDistributionList();
        }

        [HttpGet("get-claim-result-list")]
        public async Task<DataSourceResponse> GetClaimResultList()
        {
            return await _manager.GetClaimResultList();
        }

        [HttpGet("get-final-result-list")]
        public async Task<DataSourceResponse> GetClaimFinalResultList()
        {
            return await _manager.GetClaimFinalResultList();
        }

        [HttpGet("get-currency-list")]
        public async Task<DataSourceResponse> GetClaimCurrencies()
        {
            return await _manager.GetCurrencies();
        }

        [HttpGet("get-file-type-list")]
        public async Task<DataSourceResponse> GetFileTypeList()
        {
            return await _manager.GetClaimFileTypeList();
        }

        [HttpGet("claim/edit/{id}")]
        public async Task<EditClaimResponse> GetClaim(int id)
        {
            return await _manager.GetEditClaim(id);
        }

        [HttpPost("claim/save")]
        public async Task<SaveClaimResponse> SaveClaim([FromBody] ClaimDetails request, [FromServices] IConfiguration configuration)
        {
            var response = await _manager.SaveClaim(request);

            try
            {
                // if booking update success - send email and notications
                if (response.Result == SaveClaimResult.Success && response.Id > 0)
                {
                    await SendEmailAndNotifications(request, response, configuration);
                }
            }
            catch (Exception)
            {
                response.Result = SaveClaimResult.ClaimSavedNotificationError;
            }
            return response;
        }
        private async Task SendEmailAndNotifications(ClaimDetails request, SaveClaimResponse response, IConfiguration configuration)
        {
            ClaimEmailRequest mailData = await _manager.GetClaimMailDetail(response.Id, request.BookingId, request.ReportIdList, request.Id > 0 ? true : false);
            var customerKamDetail = await _manager.GetCustomerKAMDetails(mailData.CustomerId);
            var currentUser = await _manager.GetUserInfo(_applicationContext.UserId);

            var masterConfigs = await _inspManager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            string baseUrl = _configuration["BaseUrl"];

            var userAccessFilter = new UserAccess
            {
                OfficeId = mailData.OfficeId.GetValueOrDefault(),
                ServiceId = null,
                RoleId = (int)RoleEnum.ClaimAnalyze
            };
            var userListByRoleAccess = await _userRightManager.GetUserListByRoleOffice(userAccessFilter);

            var emailAddressList = userListByRoleAccess.Where(x => x != null).Select(x => x.EmailAddress).Distinct().ToList();
            var CCUserList = new List<string>();
            var ToUserList = new List<string>();

            mailData.InspectionURL = baseUrl + string.Format(configuration["UrlBookingRequest"], mailData.InspectionNo, entityName);
            //
            var priority = mailData.Priority != null ? String.Format("Priority : {0}", mailData.Priority) : "";


            var emailQueueRequest = new EmailDataRequest
            {
                TryCount = 1,
                Id = Guid.NewGuid()
            };
            if (mailData.StatusId == (int)ClaimStatus.Registered)
            {
                if (customerKamDetail != null)
                {
                    ToUserList.Add(customerKamDetail.Email);
                }
                if (emailAddressList != null)
                {
                    ToUserList.AddRange(emailAddressList);
                }
                if (currentUser.EmailAddress != null)
                {
                    CCUserList.Add(currentUser.EmailAddress);
                }
                var emailLogRequest = new EmailLogData()
                {
                    ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                    Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "Save Claim",
                    SourceId = response.Id,
                    //Subject = $"New Claim - #{mailData.ClaimNo} {mailData.StatusName} for {mailData.CustomerName}. {priority}"
                    Subject = $"New Claim -- {mailData.CustomerName}, {mailData.ProductRef}, {priority}, {mailData.ClaimNo}"
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/Claim/ClaimRequest", (mailData, baseUrl + string.Format(configuration["UrlClaimRequest"], response.Id, entityName)));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            else if (mailData.StatusId == (int)ClaimStatus.Analyzed)
            {
                var claimValidateRoleUsers = await _userRightManager.GetRoleAccessUserList((int)RoleEnum.ClaimValidate);
                var claimValidateRoleEmailList = claimValidateRoleUsers.Where(x => x.EmailAddress != null).Select(x => x.EmailAddress).Distinct().ToList();
                if (claimValidateRoleEmailList != null)
                {
                    ToUserList.AddRange(claimValidateRoleEmailList);
                }
                if (customerKamDetail != null)
                {
                    CCUserList.Add(customerKamDetail.Email);
                }
                if (emailAddressList != null)
                {
                    CCUserList.AddRange(emailAddressList);
                }
                var emailLogRequest = new EmailLogData()
                {
                    ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                    Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "Save Claim",
                    SourceId = response.Id,
                    //Subject = $"Claim Analyzed --{mailData.ClaimNo}, {mailData.CustomerName} , {priority}"
                    Subject = $"Claim Analyzed --{mailData.CustomerName}, {mailData.ProductRef}, {priority}, {mailData.ClaimNo}"
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/Claim/ClaimAnalyzed", (mailData, baseUrl + string.Format(configuration["UrlClaimRequest"], response.Id, entityName)));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            else if (mailData.StatusId == (int)ClaimStatus.Validated)
            {
                var claimValidateRoleUsers = await _userRightManager.GetRoleAccessUserList((int)RoleEnum.ClaimValidate);
                var claimValidateRoleEmailList = claimValidateRoleUsers.Where(x => x.EmailAddress != null).Select(x => x.EmailAddress).Distinct().ToList();
                if (claimValidateRoleEmailList != null)
                {
                    CCUserList.AddRange(claimValidateRoleEmailList);
                }
                if (customerKamDetail != null)
                {
                    ToUserList.Add(customerKamDetail.Email);
                }
                if (emailAddressList != null)
                {
                    ToUserList.AddRange(emailAddressList);
                }
                var emailLogRequest = new EmailLogData()
                {
                    ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                    Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "Save Claim",
                    SourceId = response.Id,
                    //Subject = $"Claim Validated --{mailData.ClaimNo}, {mailData.CustomerName} , priority: {mailData.Priority}"
                    Subject = $"Claim Validated --{mailData.CustomerName}, {mailData.ProductRef}, {priority}, {mailData.ClaimNo}"
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/Claim/ClaimValidated", (mailData, baseUrl + string.Format(configuration["UrlClaimRequest"], response.Id, entityName)));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            else if (mailData.StatusId == (int)ClaimStatus.Closed)
            {
                var claimAccountingRoleUsers = await _userRightManager.GetRoleAccessUserList((int)RoleEnum.ClaimAccounting);
                var claimAccountingRoleEmailList = claimAccountingRoleUsers.Where(x => x.EmailAddress != null).Select(x => x.EmailAddress).Distinct().ToList();
                if (claimAccountingRoleEmailList != null)
                {
                    if (mailData.FinalDecisionName.Contains("Accepted with Financial Impact"))
                    {
                        CCUserList.AddRange(claimAccountingRoleEmailList);
                    }
                }
                if (customerKamDetail != null)
                {
                    ToUserList.Add(customerKamDetail.Email);
                }
                if (emailAddressList != null)
                {
                    ToUserList.AddRange(emailAddressList);
                }
                var emailLogRequest = new EmailLogData()
                {
                    ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                    Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "Save Claim",
                    SourceId = response.Id,
                    //Subject = $"Claim Closed --{mailData.ClaimNo}, {mailData.CustomerName} , {priority}",
                    Subject = $"Claim Closed --{mailData.CustomerName}, {mailData.ProductRef}, {priority}, {mailData.ClaimNo}"
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/Claim/ClaimAccountig", (mailData, baseUrl + string.Format(configuration["UrlClaimRequest"], response.Id, entityName)));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }

        }
        private async Task PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest)
        {
            var resultId = await _emailLogQueueManager.AddEmailLog(emailLogRequest);
            emailQueueRequest.EmailQueueId = resultId;
            await _rabbitMQClient.Publish<EmailDataRequest>(_configuration["EmailQueue"], emailQueueRequest);
        }

        [HttpGet("get-claims-list/{bookingId}")]
        public async Task<BookingClaimsResponse> GetClaimListByBooking(int bookingId)
        {
            return await _manager.GetClaimsByBookingId(bookingId);
        }

        [HttpGet("get-invoice-detail/{bookingId}")]
        public async Task<InvoiceResponse> GetInvoiceByBooking(int bookingId)
        {
            return await _manager.GetClaimInvoiceByBooking(bookingId);
        }

        [HttpGet("cancelClaim/{id}")]
        public async Task<ClaimCancelResponse> CancelClaim(int id)
        {
            return await _manager.CancelClaim(id);
        }

        [Right("claim-summary")]
        [HttpPost("pending-claim-summary")]
        public async Task<PendingClaimSummaryResponse> GetPendingClaims([FromBody] PendingClaimSearchRequest request)
        {
            return await _manager.GetPendingClaims(request);
        }

        [Right("claim-summary")]
        [HttpPost("save-credit-note")]
        public async Task<SaveCreditNoteResponse> SaveCreditNote([FromBody] SaveCreditNote request)
        {
            return await _manager.SaveCreditNote(request);
        }

        [Right("claim-summary")]
        [HttpPost("get-pending-claim")]
        public async Task<GetPendingClaimResponse> GetPendingClaim([FromBody] IEnumerable<int> ids)
        {
            return await _manager.GetPendingClaimData(ids);
        }

        [Right("claim-summary")]
        [HttpPost("credit-note-summary")]
        public async Task<CreditNoteSummaryResponse> GetCreditNoteSummary([FromBody] CreditNoteSearchRequest request)
        {
            return await _manager.GetCreditNoteSummary(request);
        }

        [Right("claim-summary")]
        [HttpGet("credit-type-list")]
        public async Task<DataSourceResponse> GetCreditTypeList()
        {
            return await _manager.GetCreditTypeList();
        }

        [Right("claim-summary")]
        [HttpGet("check-creditno-exist/{creditNo}")]
        public async Task<bool> CheckCreditNoExist(string creditNo)
        {
            return await _manager.CheckCreditNumberExist(creditNo);
        }


        [HttpGet("credit/{id}")]
        public async Task<EditCreditNoteResponse> GetCreditNote(int id)
        {
            return await _manager.EditCreditNote(id);
        }

        [HttpPut("update-credit-note")]
        public async Task<SaveCreditNoteResponse> UpdateCreditNote(SaveCreditNote request)
        {
            return await _manager.UpdateCreditNote(request);
        }

        [HttpDelete("delete-credit-note/{id}")]
        public async Task<DeleteCreditNoteResponse> DeleteCreditNote(int id)
        {
            return await _manager.DeleteCreditNote(id);
        }

        [HttpPost("export-credit-note")]
        public async Task<IActionResult> ExportCreditNoteSummary(CreditNoteSearchRequest request)
        {
            var result = await _manager.ExportCreditNoteSummary(request);
            if (result == null)
                return NotFound();

            var stream = _helper.GetAsStreamObject(result);
            if (stream == null)
                return NotFound();

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CreditNoteSummary.xls");
        }
        [HttpGet("get-credit-no")]
        public async Task<DataSourceResponse> GetCreditNos()
        {
            return await _manager.GetCreditNoteNos();
        }
    }
}
