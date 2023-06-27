using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Components.Core.contracts;
using Components.Web;
using Contracts.Managers;
using DTO.Expense;
using DTO.HumanResource;
using Entities.Enums;
using LINK_UI.FileModels;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO.Common;
using Components.Core.entities.Emails;
using LINK_UI.App_start;
using Microsoft.Extensions.Configuration;
using DTO.EmailLog;
using RabbitMQUtility;
using DTO.File;
using FileResult = DTO.File.FileResult;
using BI.Utilities;
using System.Net.Http;
using System.Net;
using DTO.User;
using DTO.MasterConfig;
using static BI.TenantProvider;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseManager _manager = null;
        private readonly IExchangeRateManager _currencyManager = null;
        private readonly IFileManager _fileManager = null;
        private readonly IHostingEnvironment _hosting = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private static IConfiguration _configuration = null;
        private readonly IHelper _helper = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly ITenantProvider _filterService = null;
        private readonly ISharedInspectionManager _sharedInspManager = null;

        public ExpenseController(IExpenseManager manager, IExchangeRateManager currencyManager, IFileManager fileManager, IHostingEnvironment hosting,
             IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IConfiguration configuration,
             IHelper helper, IAPIUserContext ApplicationContext, IInspectionBookingManager inspManager, ITenantProvider filterService,
             ISharedInspectionManager sharedInspManager)
        {
            _manager = manager;
            _currencyManager = currencyManager;
            _fileManager = fileManager;
            _hosting = hosting;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _configuration = configuration;
            _helper = helper;
            _ApplicationContext = ApplicationContext;
            _inspManager = inspManager;
            _filterService = filterService;
            _sharedInspManager = sharedInspManager;
        }

        [HttpGet("expense-claim")]
        [Right("expense-claim")]
        public async Task<ExpenseClaimResponse> GetExpenseClaimList()
        {
            var response = await _manager.GetExpenseClaim(User.Identity.Name, null);

            return response;
        }
        [HttpGet("expense-claim/{id}")]
        [Right("expense-claim")]
        public async Task<ExpenseClaimResponse> GetExpenseClaim(int id)
        {
            var response = await _manager.GetExpenseClaim(User.Identity.Name, id);

            return response;
        }

        [HttpGet("cities/{term}")]
        [Right("expense-claim")]
        public async Task<ClaimCitiesResponse> GetCities(string term)
        {
            return await _manager.GetClaimCities(term);
        }

        [HttpGet("cities")]
        [Right("expense-claim")]
        public ClaimCitiesResponse GetCities()
        {
            return new ClaimCitiesResponse();
        }

        [HttpGet("currency-rate/{targetId}/{currencyId}/{date}")]
        [Right("expense-claim")]
        public async Task<string> GetExchangeRate(int targetId, int currencyId, string date)
        {
            if (targetId == 0)
                return "1";

            if (currencyId == 0)
                return "1";

            if (string.IsNullOrEmpty(date))
                return "1";

            string[] dateTab = date.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (dateTab.Length < 3)
                return "1";

            var entityId = _filterService.GetCompanyId();

            var exChangeRateType = ExhangeRateTypeEnum.ExpenseClaim;

            return await _currencyManager.GetExchangeRate(targetId, currencyId, new DateObject { Day = Convert.ToInt32(dateTab[0]), Month = Convert.ToInt32(dateTab[1]), Year = Convert.ToInt32(dateTab[2]) }, exChangeRateType);
        }

        [Right("expense-claim")]
        [HttpGet("file/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var file = await _manager.GetFile(id);

            if (file.Result == DTO.File.FileResult.NotFound || file.Content == null)
                return NotFound();

            return File(file.Content, file.MimeType); // returns a FileStreamResult
        }


        [Right("expense-claim")]
        [HttpPost("expense-claim/save")]
        public async Task<SaveExpenseClaimResponse> SaveExpenseClaim([FromBody] ExpenseClaim request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            SaveExpenseClaimResponse response = new SaveExpenseClaimResponse();

            if (request == null)
                return new SaveExpenseClaimResponse { Result = SaveExpenseClaimResult.RequestIncorrect };

            //If user has outsourceaccounting role and going to create the expense claim
            if (_ApplicationContext.UserType == UserTypeEnum.OutSource && _ApplicationContext.RoleList.Contains((int)RoleEnum.OutsourceAccounting) && request.Id == 0)
            {
                //Create the expense claim by each qc
                response = await _manager.AddOutSourceQCExpenseClaim(request);

                if (response.Result == SaveExpenseClaimResult.Success && response.ExpenseClaimList != null && response.UserList != null)
                {
                    foreach (var expenseClaim in response.ExpenseClaimList)
                    {
                        await SendExpenseClaimEmail(broadCastService, configuration, response.UserList.ToList(), expenseClaim);
                    }

                }
            }
            else
            {
                response = await _manager.SaveExpenseClaim(request);

                if (response.Result == SaveExpenseClaimResult.Success &&
                    response.ExpenseClaim != null && response.UserList != null && !response.ExpenseClaim.IsAutoExpense.GetValueOrDefault())
                {
                    await SendExpenseClaimEmail(broadCastService, configuration, response.UserList.ToList(), response.ExpenseClaim);
                }
            }



            return response;
        }

        /// <summary>
        /// Send the expense claim email
        /// </summary>
        /// <param name="broadCastService"></param>
        /// <param name="configuration"></param>
        /// <param name="userList"></param>
        /// <param name="expenseClaim"></param>
        /// <returns></returns>
        private async Task SendExpenseClaimEmail(IBroadCastService broadCastService, IConfiguration configuration,
                                List<User> userList, ExpenseClaim expenseClaim)
        {
            var clailUserList = new List<string>();

            var masterConfigs = await _inspManager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            string url = _configuration["BaseUrl"] + string.Format(configuration["UrlExpenseClaim"], expenseClaim.Id, entityName);

            foreach (var user in userList)
            {
                clailUserList.Add(user.EmailAddress);

                // Broadcast message to accounting
                broadCastService.Broadcast(user.Id, new DTO.Common.Notification
                {
                    Title = "LINK Tasks Manager",
                    Message = $"New Expense claim from {user.FullName}",
                    Url = url,
                    TypeId = "Task"
                });
            }

            // send email
            var emailQueueRequest = new EmailDataRequest
            {
                TryCount = 1,
                Id = Guid.NewGuid()
            };

            var emailLogRequest = new EmailLogData()
            {
                ToList = (clailUserList != null && clailUserList.Count > 0) ? clailUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                Cclist = expenseClaim?.StaffEmail,
                TryCount = 1,
                Status = (int)EmailStatus.NotStarted,
                SourceName = "Save Expense Claim",
                SourceId = expenseClaim.Id,
                Subject = $"Expense Claim {expenseClaim.Status} - {expenseClaim.ClaimNo}"
            };

            emailLogRequest.Body = this.GetEmailBody("Emails/ExpenseClaim",
                        (expenseClaim, (userList.Count() == 1 ? userList.FirstOrDefault() : null), url));

            await PublishQueueMessage(emailQueueRequest, emailLogRequest);
        }


        [Right("expense-claim")]
        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public bool UploadAttachedFiles()
        {

            if (Request.Form.Files != null && Request.Form.Files.Any())
            {
                var dict = new Dictionary<Guid, byte[]>();

                foreach (var file in Request.Form.Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            dict.Add(new Guid(file.Name), fileBytes);
                        }
                    }
                }

                _manager.UploadFiles(dict).Wait();
                return true;
            }
            return false;
        }

        [Right("expenseclaim-list")]
        [HttpGet("expense-summary")]
        public async Task<ExpenseClaimSummaryResponse> GetExpenseSummary()
        {
            return await _manager.GetExpenseSummary();
        }


        [Right("expenseclaim-list")]
        [HttpPost("expense-list")]
        public async Task<ExpenseClaimListResponse> GetExpenseClaimList(ExpenseClaimListRequest request)
        {
            if (request.StartDate == null)
                return new ExpenseClaimListResponse { Result = ExpenseClaimListResult.StartDateRequired };
            if (request.EndDate == null)
                return new ExpenseClaimListResponse { Result = ExpenseClaimListResult.EndDateRequired };

            return await _manager.GetExpenseClaimList(request);
        }

        [Right("expense-claim")]
        [HttpGet("food-allowance/{date}/{countryId}")]
        public async Task<decimal> GetFoodAllowance(string date, int countryId)
        {
            string[] dateTab = date.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (dateTab.Length < 3)
                return 0;

            return await _currencyManager.GetFoodAllowance(new DateObject { Day = Convert.ToInt32(dateTab[0]), Month = Convert.ToInt32(dateTab[1]), Year = Convert.ToInt32(dateTab[2]) }, countryId);
        }

        [Right("expense-claim")]
        [HttpGet("status/{id}/{idStatus}/{expenseType}")]
        public async Task<bool> SetStatus(int id, int idStatus, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration, 
            bool expenseType)
        {
            var response = await _manager.SetExpenseStatus(id, idStatus);

            if (response.Result == SetExpenseStatusResult.Success && response.Data != null && !expenseType)
            {
                var emailRequestList = new List<(string, object, EmailRequest)>();

                var masterConfigs = await _inspManager.GetMasterConfiguration();
                var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                string url = _configuration["BaseUrl"] + string.Format(configuration["UrlExpenseClaim"], response.Data.Id, entityName);

                if (idStatus != (int)ExpenseClaimStatus.Cancelled)
                {
                    // Broadcast message to user
                    broadCastService.Broadcast(response.UserIds, new DTO.Common.Notification
                    {
                        Title = "LINK Notification Manager",
                        Message = $"Expense claim {response.Data.Status} - {response.Data.ClaimNo}",
                        Url = url,
                        TypeId = "Notification"
                    });
                }

                if (idStatus == (int)ExpenseClaimStatus.Checked && response.ManagerUserId > 0)
                {
                    // Send email and task for manager to approve
                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = response.ManagerEmail,
                        Cclist = response.Data.StaffEmail,
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Expense Claim Status",
                        SourceId = response.Data.Id,
                        Subject = $"Expense Claim {response.Data.Status} - {response.Data.ClaimNo}",
                    };

                    emailLogRequest.Body = this.GetEmailBody("Emails/ExpenseEmailToApproveOrPay",
                                 (response.Data,
                     response.ManagerName, url));

                    await PublishQueueMessage(emailQueueRequest, emailLogRequest);

                    // Broadcast message to manager
                    broadCastService.Broadcast(response.ManagerUserId, new DTO.Common.Notification
                    {
                        Title = "LINK Tasks Manager",
                        Message = $"Expense claim Checked - {response.Data.Name}",
                        Url = url,
                        TypeId = "Task"
                    });
                }
                else if (idStatus == (int)ExpenseClaimStatus.Approved && response.ClaimUserList != null && response.ClaimUserList.Any())
                {
                    // Send one email to Claim Users 

                    var lstUserClaims = new List<string>();

                    foreach (var user in response.ClaimUserList)
                    {
                        lstUserClaims.Add(user.EmailAddress);

                        // Broadcast message to accounting
                        broadCastService.Broadcast(user.Id, new DTO.Common.Notification
                        {
                            Title = "LINK Tasks Manager",
                            Message = $"Expense claim Approved - {response.Data.Name}",
                            Url = url,
                            TypeId = "Task"
                        });
                    }

                    // send email                    
                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = response.Data.StaffEmail,
                        Cclist = (lstUserClaims != null && lstUserClaims.Count > 0) ? lstUserClaims.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Expense Claim Status",
                        SourceId = response.Data.Id,
                        Subject = $"Expense Claim {response.Data.Status} - {response.Data.ClaimNo}"
                    };

                    emailLogRequest.Body = this.GetEmailBody("Emails/ExpenseStatus",
                               (response.Data, url));

                    await PublishQueueMessage(emailQueueRequest, emailLogRequest);

                }
                else if (idStatus == (int)ExpenseClaimStatus.Paid)
                {
                    // Send one email to Claim Users 

                    var lstUserClaims = new List<string>();

                    foreach (var user in response.ClaimUserList)
                        lstUserClaims.Add(user.EmailAddress);

                    // send email                    
                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = response.Data.StaffEmail,
                        Cclist = (lstUserClaims != null && lstUserClaims.Count > 0) ? lstUserClaims.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Expense Claim Status",
                        SourceId = response.Data.Id,
                        Subject = $"Expense Claim {response.Data.Status} - {response.Data.ClaimNo}",
                    };

                    emailLogRequest.Body = this.GetEmailBody("Emails/ExpenseStatus",
                                (response.Data, url));

                    await PublishQueueMessage(emailQueueRequest, emailLogRequest);

                }
                else if (idStatus == (int)ExpenseClaimStatus.Cancelled && response.Data != null && !response.Data.IsAutoExpense.GetValueOrDefault())
                {
                    // Send one email to Claim Users 

                    var lstUserClaims = new List<string>();

                    foreach (var user in response.ClaimUserList)
                        lstUserClaims.Add(user.EmailAddress);

                    // send email                    
                    var emailQueueRequest = new EmailDataRequest
                    {
                        TryCount = 1,
                        Id = Guid.NewGuid()
                    };

                    var emailLogRequest = new EmailLogData()
                    {
                        ToList = (lstUserClaims != null && lstUserClaims.Count > 0) ? lstUserClaims.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                        Cclist = response.Data.StaffEmail,
                        TryCount = 1,
                        Status = (int)EmailStatus.NotStarted,
                        SourceName = "Expense Claim Status",
                        SourceId = response.Data.Id,
                        Subject = $"Expense Claim {response.Data.Status} - {response.Data.ClaimNo}",
                    };

                    emailLogRequest.Body = this.GetEmailBody("Emails/ExpenseCancel",
                               (response.Data, url));

                    await PublishQueueMessage(emailQueueRequest, emailLogRequest);

                }

            }
            return response.Result == SetExpenseStatusResult.Success;
        }



        [Right("expense-claim")]
        [HttpPost("reject/{id}")]
        public async Task<bool> SetStatus(int id, ExpenseReject request, [FromServices] IConfiguration configuration, [FromServices] IBroadCastService broadCastService)
        {
            var response = await _manager.Reject(id, request.Comment);

            if (response.Result == SetExpenseStatusResult.Success && response.Data != null)
            {
                var masterConfigs = await _inspManager.GetMasterConfiguration();
                var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                string url = _configuration["BaseUrl"] + string.Format(configuration["UrlExpenseClaim"], response.Data.Id, entityName);

                // send email
                var emailQueueRequest = new EmailDataRequest
                {
                    TryCount = 1,
                    Id = Guid.NewGuid()
                };

                var emailLogRequest = new EmailLogData()
                {
                    ToList = response.Data.StaffEmail,
                    TryCount = 1,
                    Status = (int)EmailStatus.NotStarted,
                    SourceName = "Reject Expense",
                    SourceId = response.Data.Id,
                    Subject = $"Expense Claim {response.Data.Status} - {response.Data.ClaimNo}",
                };

                emailLogRequest.Body = this.GetEmailBody("Emails/ExpenseStatus",
                           (response.Data, url));

                await PublishQueueMessage(emailQueueRequest, emailLogRequest);

                if (response.UserIds != null && response.UserIds.Any())
                {
                    // Broadcast message to user
                    broadCastService.Broadcast(response.UserIds, new Notification
                    {
                        Title = "LINK Notification Manager",
                        Message = $"Expense claim {response.Data.Status} - {response.Data.ClaimNo}",
                        Url = url,
                        TypeId = "Notification"
                    });
                }
            }
            return response.Result == SetExpenseStatusResult.Success;
        }


        [Right("expense-claim")]
        [HttpPost("exportSummary")]
        public async Task<IActionResult> GetExportSummary([FromBody] ExpenseClaimListRequest request)
        {
            request.Index = 1;
            request.pageSize = 999999;
            string standarddateformat = "dd/MM/yyyy";
            var response = await _manager.GetExpenseClaimList(request);

            if (response.Result != ExpenseClaimListResult.Success)
                return NotFound();

            var model = new ExpenseSummaryModel
            {
                Items = response.ExpenseClaimGroupList.SelectMany(x => x.Items),
                StartDate = request.StartDate.ToDateTime().ToString(standarddateformat),
                EndDate = request.EndDate.ToDateTime().ToString(standarddateformat),
                LocationName = request.LocationValues == null ? "" : string.Join(",", request.LocationValues.Select(x => x.Name)),
                Employes = request.EmployeeValues == null ? "" : string.Join(",", request.EmployeeValues.Select(x => x.StaffName)),
                StatusList = request.StatusValues,
                DateType = request.IsClaimDate ? "Claim date" : "Expense date"
            };

            return await this.FileAsync("ExpenseSummary", model, Components.Core.entities.FileType.Excel);
        }

        [Right("expense-claim")]
        [HttpGet("export-claim/{id}")]
        public async Task<IActionResult> GetExportClaim(int id)
        {
            var response = await _manager.GetExpenseClaim(User.Identity.Name, id);

            if (response.Result != ExpenseClaimResult.Success)
                return NotFound();

            var model = new ExpenseClaimModel
            {
                Item = response.ExpenseClaim,
                Total = response.ExpenseClaim.ExpenseList.Sum(x => x.Amount)
            };

            var sheet = await this.GetFileBytesAsync("ExpenseClaim", model, Components.Core.entities.FileType.Excel);

            using (MemoryStream zipToOpen = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    //Add xlsx
                    await AddFileToZip(archive, sheet, $"{model.Item.ClaimNo}.xlsx");

                    var data = response.ExpenseClaim.ExpenseList.SelectMany(x => x.Files).ToList();

                    foreach (var file in data)
                    {
                        FileResponse fileContent = new FileResponse();

                        if (string.IsNullOrEmpty(file.Uniqueld) && string.IsNullOrEmpty(file.FileUrl))
                        {
                            fileContent = await _manager.GetFile(file.Id);
                        }

                        if (!string.IsNullOrEmpty(file.Uniqueld))
                        {
                            try
                            {
                                var httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, file.FileUrl, null, "");

                                if (httpResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    using (var stream = await httpResponse.Content.ReadAsStreamAsync())
                                    {
                                        using (MemoryStream mstream = new MemoryStream())
                                        {
                                            stream.CopyTo(mstream);

                                            fileContent = new FileResponse
                                            {
                                                Content = mstream.ToArray(),
                                                Result = FileResult.Success,
                                                MimeType = _fileManager.GetMimeType(Path.GetExtension(file.FileName)),
                                            };
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                fileContent.Result = FileResult.NotFound;
                            }
                        }

                        if (fileContent.Result != DTO.File.FileResult.Success || fileContent.Content == null)
                            continue;

                        await AddFileToZip(archive, fileContent.Content, file.FileName);
                    }
                }

                zipToOpen.Close();

                return File(zipToOpen.ToArray(), "application/zip");
            }

        }

        private async Task AddFileToZip(ZipArchive archive, byte[] fileContent, string fileName)
        {
            ZipArchiveEntry readmeEntry = archive.CreateEntry(fileName);



            using (var stream = readmeEntry.Open())
            {
                using (var streamContent = new MemoryStream(fileContent))
                    await streamContent.CopyToAsync(stream);

            }
        }

        [Right("expenseclaim-list")]
        [HttpGet("expense-claimtypelist")]
        public async Task<ExpenseClaimTypeResponse> GetExpenseClaimTypeList()
        {
            return await _manager.GetExpenseClaimTypeList();
        }

        [Right("expenseclaim-list")]
        [HttpGet("getbookingdetail/{claimTypeId}/{expenseId}/{isEdit}")]
        public async Task<ExpenseBookingDetailResponse> GetBookingDetail(int claimTypeId, int? expenseId, bool isEdit)
        {
            ExpenseBookingDetailResponse response = null;

            if (_ApplicationContext.UserType == UserTypeEnum.OutSource && _ApplicationContext.RoleList.Contains((int)RoleEnum.OutsourceAccounting))
                response = await _manager.GetOutSourceQCBookingDetails(expenseId);
            else
                response = await _manager.GetExpenseBookingDetails(claimTypeId, expenseId, isEdit);


            return response;
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


        [Right("expense-claim")]
        [HttpPost("vocherSummary")]
        public async Task<IActionResult> ExportVocherSummary(ExpenseClaimListRequest request)
        {
            var response = await _manager.ExportVocherSummary(request);

            if (response == null || !response.ClaimData.Any())
                return NotFound();

            return await this.FileAsync("KPI/Expense/VoucherExpenseClaimTemplate", response, Components.Core.entities.FileType.Excel);
        }

        [Right("expense-claim")]
        [HttpPost("ExpenseKpiSummary")]
        public async Task<IActionResult> ExportExpenseKpiSummary(ExpenseClaimListRequest request)
        {
            if (request.ExportType == (int)ExportExpenseType.ExpenseSummaryKPI)
            {
                var response = await _manager.ExportExpenseKpiSummary(request);

                if (response == null || !response.Data.Any())
                    return NotFound();

                return await this.FileAsync("KPI/Expense/ExpenseSummaryTemplate", response, Components.Core.entities.FileType.Excel);
            }
            else
            {
                var response = await _manager.ExportExpenseDetailKpiSummary(request);

                if (response.Result != ExpenseSummaryDetailKpiListResult.Success)
                    return NotFound();

                var stream = _sharedInspManager.GetAsStreamObject(response.ExpenseSummaryDetailKpiList);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "expensesummarydetailkpi.xls");
            }
        }

        [Right("expense-claim")]
        [HttpPost("CheckPendingExpenseExist")]
        public async Task<bool> CheckPendingExpenseExist(PendingExpenseRequest request)
        {
            return await _manager.CheckPendingExpenseExist(request);
        }
        [Right("expenseclaim-list")]
        [HttpPost("expense-food-amount")]
        public async Task<ExpenseFoodClaimResponse> GetExpenseFoodAmount(ExpenseFoodClaimRequest request)
        {
            return await _manager.GetExpenseFoodAmount(request);
        }

        [Right("expense-claim")]
        [HttpPost("update-status")]
        public async Task<bool> SetStatusList(List<ExpenseClaimUpdateStatus> expenseClaimUpdateStatus,
             [FromServices] IConfiguration configuration, [FromServices] IBroadCastService broadCastService)
        {
            bool res = false;
            foreach (var item in expenseClaimUpdateStatus)
            {
                if (item.Id > 0 && item.NextStatusId > 0)
                {
                    res = await SetStatus(item.Id, item.NextStatusId, broadCastService, configuration, item.ExpenseType);
                }
            }
            return res;
        }

        [Right("expense-claim")]
        [HttpPost("pending-expense-configure")]
        public async Task<PendingBookingExpenseResponse> GetFoodOrTravelPendingExpense(List<PendingBookingExpenseRequest> request)
        {
            return await _manager.GetFoodOrTravelPendingExpenseBookingIdList(request);
        }
    }
}