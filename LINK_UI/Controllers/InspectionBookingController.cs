using BI.Maps;
using Components.Core.contracts;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.CancelBooking;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.DataAccess;
using DTO.Eaqf;
using DTO.EmailLog;
using DTO.EntPages;
using DTO.FullBridge;
using DTO.Inspection;
using DTO.MasterConfig;
using DTO.OfficeLocation;
using DTO.Supplier;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InspectionBookingController : ControllerBase
    {
        private readonly IInspectionBookingManager _manager = null;
        private readonly ILogger<InspectionBookingController> _logger;
        private readonly ICustomerManager _customerManager = null;

        private readonly ICustomerServiceConfigManager _customerServiceManager = null;
        private readonly ISharedInspectionManager _helper = null;

        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ISupplierManager _suppliermanager = null;
        private static IConfiguration _configuration = null;
        private readonly IDocumentManager _documentManager = null;
        private readonly IHostingEnvironment _env;
        private readonly IUserRightsManager _userManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private readonly BookingMap _bookingmap = null;
        private readonly IEmailManager _emailManager;
        private readonly ITenantProvider _filterService = null;
        private readonly IEaqfEventUpdateManager _eaqfEventUpdate = null;
        private readonly IBookingEmailLogQueueManager _bookingLogQueueManager;

        public InspectionBookingController(IInspectionBookingManager manager, ICustomerManager customerManager, IConfiguration configuration,
            ICustomerServiceConfigManager customerServiceManager, IAPIUserContext applicationContext, ISharedInspectionManager helper,
            ISupplierManager suppliermanager, IUserRightsManager userManager, ILogger<InspectionBookingController> logger, IBookingEmailLogQueueManager bookingEmailLogQueueManager,
            IDocumentManager documentManager, IHostingEnvironment env, IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IEmailManager emailManager,
            ITenantProvider filterService, IEaqfEventUpdateManager eaqfEventUpdate)
        {
            _suppliermanager = suppliermanager;
            _manager = manager;
            _customerManager = customerManager;
            _customerServiceManager = customerServiceManager;

            _userManager = userManager;

            _ApplicationContext = applicationContext;
            _helper = helper;
            _logger = logger;
            _configuration = configuration;
            _documentManager = documentManager;
            _env = env;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _bookingmap = new BookingMap();
            _emailManager = emailManager;
            _filterService = filterService;
            _eaqfEventUpdate = eaqfEventUpdate;
            _bookingLogQueueManager = bookingEmailLogQueueManager;
        }

        [HttpGet("add")]

        public async Task<EditInspectionBookingResponse> Add()
        {
            return await _manager.AddInspectionData();
        }

        [Right("edit-booking")]
        [HttpGet("Getbookingsummary")]
        public async Task<BookingSummaryResponse> Getbookingsummary()
        {
            return await _manager.GetBookingSummary();
        }

        [HttpGet("EditBooking/{id}")]
        [Right("edit-booking")]
        public async Task<EditInspectionBookingResponse> EditBooking(int id)
        {
            return await _manager.EditInspectionData(id);
        }

        [HttpGet("POListByCustomerandProducts/{id}/{productcategoryid}/{supplierId}")]
        [Right("edit-booking")]
        public BookingPOListResponse GetPOListByCustomerandProducts(int id, int productCategoryId, int supplierId)
        {
            return _manager.GetPOListByCustomerAndProducts(id, productCategoryId, supplierId);
        }


        [HttpGet("GetPickingAndCombineOrders/{id}")]
        [Right("edit-booking")]
        public Task<PickingAndCombineOrderResponse> GetPickingAndCombineOrders(int id)
        {
            return _manager.GetPickingAndCombineOrders(id);
        }

        [HttpGet("get-draft-bookings")]
        [Right("edit-booking")]
        public Task<DraftInspectionResponse> GetInspectionDraftBooking()
        {
            return _manager.GetInspectionDraftByUserId();
        }

        [HttpGet("remove-draft-booking/{draftInspectionId}")]
        [Right("edit-booking")]
        public Task<DeleteDraftInspectionResponse> RemoveInspectionDraftBooking(int draftInspectionId)
        {
            return _manager.RemoveInspectionDraft(draftInspectionId);
        }

        // POST: api/InspectionBooking
        [HttpPost("save")]
        [Right("edit-booking")]
        public async Task<SaveInspectionBookingResponse> SaveInspectionBooking([FromBody] SaveInsepectionRequest request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            request.UserType = (int)_ApplicationContext.UserType;

            // Save or update Booking details 
            var response = await _manager.SaveInspectionBooking(request);

            try
            {
                // if booking update success - send email and notications
                if (response.Result == SaveInspectionBookingResult.Success && response.Id > 0)
                {

                    if (!request.IsUpdateBookingDetail && request.IsEaqf)
                    {
                        EAQFEventUpdate cancelRequest = new EAQFEventUpdate();
                        cancelRequest.BookingId = response.Id;
                        cancelRequest.StatusId = request.StatusId;
                        await _eaqfEventUpdate.UpdateRescheduleStatusToEAQF(cancelRequest, EAQFBookingEventRequestType.AddStatus);
                    }

                    if (response.IsTechincalDocumentsAddedOrRemoved)
                    {
                        await PublishFileAttachmentQueueMessage(response.Id);
                    }

                    if (request.Id > 0 && response.IsMissionCreated && response.IsTechnicalDoucmentSync) // this queue only for mission url when update method is called at that time we are publish the queue
                    {
                        var fbBookingFbLog = new BookingFbLogData()
                        {
                            BookingId = response.Id,
                            FbBookingSyncType = FbBookingSyncType.InspectionUpdation,
                            TryCount = 1
                        };
                        await PublishFbInspectionBookingQueue(fbBookingFbLog);
                    }


                    await SendEmailAndNotifications(request, response, broadCastService, configuration);


                }
            }
            catch (Exception)
            {
                response.Result = SaveInspectionBookingResult.BookingSavedNotificationError;
            }
            return response;
        }

        /// <summary>
        /// push the file attachment data to the queue
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        private async Task PublishFileAttachmentQueueMessage(int bookingId)
        {
            FileAttachmentToZipRequest fileAttachmentToZipRequest = new FileAttachmentToZipRequest();
            fileAttachmentToZipRequest.InspectionId = bookingId;
            fileAttachmentToZipRequest.EntityId = _filterService.GetCompanyId();

            var emailLogRequest = new EmailLogData()
            {
                TryCount = 1,
                Status = (int)EmailStatus.NotStarted,
                SourceName = "Save Booking Files as Zip",
                SourceId = bookingId,

            };

            await _emailLogQueueManager.AddEmailLog(emailLogRequest);

            await _rabbitMQClient.Publish<FileAttachmentToZipRequest>(_configuration["ProcessZipFileQueue"], fileAttachmentToZipRequest);
        }

        [HttpPost("save-draft-inspection-booking")]
        [Right("edit-booking")]
        public async Task<SaveDraftInsepectionResponse> SaveDraftInspectionBooking(DraftInspectionRequest request)
        {
            return await _manager.SaveDraftInspectionBooking(request);
        }

        [HttpPost("confirmemail")]
        [Right("edit-booking")]
        public async Task<SaveInspectionBookingResponse> ConfirmEmail([FromBody] SaveInsepectionRequest request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {

            // send Confirm email
            var response = new SaveInspectionBookingResponse();
            response.Id = request.Id;
            response.Result = SaveInspectionBookingResult.Success;
            try
            {
                if (request.StatusId == (int)BookingStatus.Confirmed || request.StatusId == (int)BookingStatus.AllocateQC)
                {
                    if (response.Result == SaveInspectionBookingResult.Success && response.Id > 0)
                    {
                        await SendConfirmationEmail(request, response, configuration);
                    }
                }
            }
            catch (Exception)
            {
                response.Result = SaveInspectionBookingResult.BookingSavedNotificationError;
            }
            return response;
        }

        /// <summary>
        /// Send email and notifications
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="broadCastService"></param>
        /// <returns></returns>
        private async Task SendEmailAndNotifications(SaveInsepectionRequest request, SaveInspectionBookingResponse response, IBroadCastService broadCastService, IConfiguration configuration)
        {
            SetInspNotifyResponse notifRres = null;

            var updateDataHoldBooking = request.StatusId == (int)BookingStatus.Hold && request.IsUpdateBookingDetail;

            if (!updateDataHoldBooking)
                notifRres = await _manager.BookingTaskNotification(response.Id, response.isCombineOrderDataChanged, request.StatusId, request);

            BookingMailRequest mailData = await _manager.GetBookingMailDetail(response.Id, request.IsEmailRequired, request.Id > 0 ? true : false);

            var masterConfigs = await _manager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            string baseUrl = _configuration["BaseUrl"];

            var CCUserList = new List<string>();
            var ToUserList = new List<string>();

            if (notifRres != null)
            {
                mailData.quotationExists = notifRres.quotationExists;
                mailData.StatusName = notifRres.StatusName;

                if (notifRres.UserList != null)
                {
                    CCUserList.AddRange(notifRres.UserList.Select(x => x.EmailAddress));
                }

                if (notifRres.ToRecipients != null)
                {
                    ToUserList.AddRange(notifRres.ToRecipients.Select(x => x.EmailAddress));
                    // Broadcast message to accounting
                    broadCastService.Broadcast(notifRres.ToRecipients.Select(x => x.Id), new Notification
                    {
                        Title = "LINK Tasks Manager",
                        Message = $"Inspection Booking - {response.Id} {notifRres.StatusName}",
                        Url = baseUrl + string.Format(configuration["UrlBookingRequest"], response.Id, entityName),
                        TypeId = "Task"
                    });
                }

                if (notifRres.CustomerEmail != null && !request.IsEaqf)
                {
                    ToUserList.Add(notifRres.CustomerEmail);
                }
            }

            if (!request.IsEaqf && response.isCombineOrderDataChanged)
            {
                ToUserList.Add(mailData.SupplierMail);
            }
            else if (!request.IsEaqf && (BookingStatus)request.StatusId != BookingStatus.Verified && request.IsSupplierOrFactoryEmailSend
                && request.BookingType != (int)InspectionBookingTypeEnum.UnAnnounced)
            {
                ToUserList.Add(mailData.SupplierMail);
                ToUserList.Add(mailData.FactoryMail);
            }

            //To user list .has to be mandatory if no data , send to Booking team.
            if (CCUserList.Count == 0 || ToUserList.Count == 0)
            {
                var bookingTeamGroupEmail = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.BookingTeamGroupEmail).Select(x => x.Value).FirstOrDefault();
                CCUserList.Add(bookingTeamGroupEmail);
            }
            var entityId = _filterService.GetCompanyId();
            var _settings = _emailManager.GetMailSettingConfiguration(entityId);

            mailData.SenderEmail = _settings.SenderEmail;
            // check cc is exist in to addrees - if available remove from cc list
            CCUserList.RemoveAll(ccemail => ToUserList.Contains(ccemail));
            ToUserList.RemoveAll(temail => temail == null);
            CCUserList.RemoveAll(ccemail => ccemail == null);

            if (!request.IsEaqf && request.FactoryId > 0)
            {
                var ccEmails = await _manager.GetCCEmailConfigurationEmailsByCustomer(request.CustomerId, request.FactoryId.GetValueOrDefault(), request.StatusId);
                if (ccEmails != null && ccEmails.Any())
                {
                    CCUserList.AddRange(ccEmails);
                }
            }

            var emailQueueRequest = new EmailDataRequest
            {
                TryCount = 1,
                Id = Guid.NewGuid()
            };

            var subjectStatusName = updateDataHoldBooking ? "Modified" : notifRres.StatusName;

            var emailLogRequest = new EmailLogData()
            {
                ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                TryCount = 1,
                Status = (int)EmailStatus.NotStarted,
                SourceName = "Save Booking",
                SourceId = response.Id,
                Subject = $"{entityName} Inspection Booking - {subjectStatusName} (INS - {response.Id}, Customer: {mailData.CustomerName}, Supplier: {mailData.SupplierName}"
            };

            emailLogRequest.Subject = mailData.BookingType == (int)InspectionBookingTypeEnum.UnAnnounced ? emailLogRequest.Subject + ", BookingType: " + mailData.BookingTypeValue + ")" : emailLogRequest.Subject + ")";


            var isSplitBooking = false;
            var urlBookingRequest = baseUrl + string.Format(configuration["UrlBookingRequest"], response.Id, entityName);
            var urlSplitBookingCancel = baseUrl + string.Format(configuration["UrlSplitBookingCancel"], response.Id, entityName);
            mailData.EntityName = entityName;
            if (request.StatusId == (int)BookingStatus.Received && !isSplitBooking)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingRequest", (mailData, urlBookingRequest));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);

            }
            else if (request.StatusId == (int)BookingStatus.Received && isSplitBooking)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Booking/SplitBooking", (mailData, urlSplitBookingCancel));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);

            }
            else if (request.StatusId == (int)BookingStatus.Verified)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingRequest", (mailData, urlBookingRequest));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            else if (request.StatusId == (int)BookingStatus.Confirmed)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingConfirm", (mailData, urlBookingRequest));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);

            }
            else if (request.StatusId == (int)BookingStatus.Hold && !request.IsUpdateBookingDetail)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingHold", (mailData, urlBookingRequest));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
            else if (updateDataHoldBooking)
            {
                emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingRequest", (mailData, urlBookingRequest));
                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }
        }


        private async Task SendConfirmationEmail(SaveInsepectionRequest request, SaveInspectionBookingResponse response, IConfiguration configuration)
        {
            var notifRres = await _manager.GetConfirmEmailUsers(response.Id, request);

            BookingMailRequest mailData = await _manager.GetBookingMailDetail(response.Id, request.IsEmailRequired, request.Id > 0 ? true : false);

            var CCUserList = new List<string>();
            var ToUserList = new List<string>();


            mailData.quotationExists = notifRres.quotationExists;
            mailData.StatusName = notifRres.StatusName;

            if (notifRres.UserList != null)
            {
                CCUserList.AddRange(notifRres.UserList.Select(x => x.EmailAddress));
            }

            if (notifRres.ToRecipients != null)
            {
                ToUserList.AddRange(notifRres.ToRecipients.Select(x => x.EmailAddress));
            }

            if (notifRres.CustomerEmail != null && !request.IsEaqf)
            {
                ToUserList.Add(notifRres.CustomerEmail);
            }
            if (request.IsSupplierOrFactoryEmailSend && !request.IsEaqf && request.BookingType != (int)InspectionBookingTypeEnum.UnAnnounced)
            {
                ToUserList.Add(mailData.SupplierMail);
                ToUserList.Add(mailData.FactoryMail);
            }

            var masterConfigs = await _manager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            string baseUrl = _configuration["BaseUrl"];
            //To user list .has to be mandatory if no data , send to Booking team.
            if (CCUserList.Count == 0 || ToUserList.Count == 0)
            {
                var bookingTeamGroupEmail = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.BookingTeamGroupEmail).Select(x => x.Value).FirstOrDefault();
                CCUserList.Add(bookingTeamGroupEmail);
            }

            // check cc is exist in to addrees - if available remove from cc list
            CCUserList.RemoveAll(ccemail => ToUserList.Contains(ccemail));
            ToUserList.RemoveAll(temail => temail == null);
            CCUserList.RemoveAll(ccemail => ccemail == null);

            var emailQueueRequest = new EmailDataRequest
            {
                TryCount = 1,
                Id = Guid.NewGuid()
            };

            var emailLogRequest = new EmailLogData()
            {
                ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                TryCount = 1,
                Status = (int)EmailStatus.NotStarted,
                SourceName = "Save Booking",
                SourceId = response.Id,
                Subject = $"{entityName} Inspection Booking - {notifRres.StatusName} (INS - {response.Id}, Customer: {mailData.CustomerName}, Supplier: {mailData.SupplierName})"
            };

            emailLogRequest.Subject = mailData.BookingType == (int)InspectionBookingTypeEnum.UnAnnounced ? emailLogRequest.Subject + ", BookingType: " + mailData.BookingTypeValue + ")" : emailLogRequest.Subject + ")";

            emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingConfirm", (mailData, baseUrl + string.Format(configuration["UrlBookingRequest"], response.Id, entityName)));
            await PublishQueueMessage(emailQueueRequest, emailLogRequest);
        }

        /// <summary>
        /// Save email data into log table and publish to queue
        /// </summary>
        /// <param name="emailQueueRequest"></param>
        /// <param name="emailLogRequest"></param>
        /// <returns></returns>
        private async Task PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest)
        {
            if (!string.IsNullOrWhiteSpace(emailLogRequest.ToList) || !string.IsNullOrEmpty(emailLogRequest.Cclist))
            {
                var resultId = await _emailLogQueueManager.AddEmailLog(emailLogRequest);
                emailQueueRequest.EmailQueueId = resultId;
                await _rabbitMQClient.Publish<EmailDataRequest>(_configuration["EmailQueue"], emailQueueRequest);
            }

        }

        [HttpPost("cancelbooking")]
        [Right("edit-booking")]
        public async Task<SaveInspectionBookingResponse> CancelInspectionBooking([FromBody] SplitBooking request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            var response = await _manager.CancelInspectionBooking(request);

            // Send mail
            try
            {

                if (response.Result == SaveInspectionBookingResult.Success)
                {
                    var masterConfigs = await _manager.GetMasterConfiguration();
                    var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                    string baseUrl = _configuration["BaseUrl"];

                    BookingMailRequest mailData = await _manager.GetBookingMailDetail(response.Id, request.IsEmailRequired, null);
                    if (mailData != null)
                        mailData.BookingComment = request.SplitBookingComments;
                    var cancelPODetail = _bookingmap.MapPOBookingList(request.SplitBookingProductList.ToList());
                    //var cancelPORequest = _bookingmap.MapPOCancelBooking(request.BookingData.InspectionPoList.ToList());
                    //cancelPODetail.RemoveAll(x => cancelPORequest.Exists(y => y.PoDetailId == x.PoDetailId));
                    mailData.CancelPoList = cancelPODetail;

                    //get product details
                    var productCategoryList = await _manager.GetProductCategoryDetails(new[] { response.Id });
                    //Get Department details
                    var departmentData = await _manager.GetBookingDepartmentList(new[] { response.Id });
                    //Get Brand details
                    var brandData = await _manager.GetBookingBrandList(new[] { response.Id });

                    //factory country 
                    int? factoryCountryId = null;
                    if (request.BookingData != null && request.BookingData.FactoryId.HasValue)
                    {
                        var factoryCountryData = await _suppliermanager.GetSupplierHeadOfficeAddress(request.BookingData.FactoryId.Value);
                        if (factoryCountryData.Result == SupplierListResult.Success)
                            factoryCountryId = factoryCountryData.countryId;
                    }

                    var userAccessFilter = new UserAccess
                    {
                        OfficeId = request.BookingData.OfficeId != null ? request.BookingData.OfficeId.Value : 0,
                        ServiceId = (int)Service.InspectionId,
                        CustomerId = request.BookingData.CustomerId,
                        RoleId = (int)RoleEnum.InspectionRequest,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };

                    var users = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                    userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                    var toRecipients = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                    var CCUserList = new List<string>();
                    var ToUserList = new List<string>();

                    if (users.Count() > 0)
                    {
                        foreach (var user in users)
                        {
                            CCUserList.Add(user.EmailAddress);
                        }
                    }

                    if (toRecipients.Count() > 0)
                    {
                        foreach (var user in toRecipients)
                        {
                            ToUserList.Add(user.EmailAddress);

                            // Broadcast message to accounting
                            broadCastService.Broadcast(user.Id, new Notification
                            {
                                Title = "LINK Tasks Manager",
                                Message = $"Inspection Booking - {response.Id} {"Modified"}",
                                Url = baseUrl + string.Format(configuration["UrlBookingRequest"], response.Id, entityName),
                                TypeId = "Task"
                            });
                        }
                    }

                    if (CCUserList.Count == 0)
                    {
                        var bookingTeamGroupEmail = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.BookingTeamGroupEmail).Select(x => x.Value).FirstOrDefault();
                        CCUserList.Add(bookingTeamGroupEmail);
                    }
                    if (!request.BookingData.IsEaqf && request.BookingData.BookingType != (int)InspectionBookingTypeEnum.UnAnnounced)
                    {
                        ToUserList.Add(mailData.SupplierMail);
                        ToUserList.Add(mailData.FactoryMail);
                    }


                    if (ToUserList.Count() > 0)
                    {

                        var emailQueueRequest = new EmailDataRequest
                        {
                            TryCount = 1,
                            Id = Guid.NewGuid()
                        };

                        var emailLogRequest = new EmailLogData()
                        {
                            ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                            Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                            TryCount = 1,
                            Status = (int)EmailStatus.NotStarted,
                            SourceName = "Split Booking",
                            SourceId = response.Id,
                            Subject = $"{entityName} Inspection Booking - {"Modified"} (INS - {response.Id}, Customer: {mailData.CustomerName}, Supplier: {mailData.SupplierName})"
                        };

                        emailLogRequest.Subject = mailData.BookingType == (int)InspectionBookingTypeEnum.UnAnnounced ? emailLogRequest.Subject + ", BookingType: " + mailData.BookingTypeValue + ")" : emailLogRequest.Subject + ")";

                        if (request.BookingData.StatusId == (int)BookingStatus.Received
                                    || request.BookingData.StatusId == (int)BookingStatus.Verified || request.BookingData.StatusId == (int)BookingStatus.Confirmed)
                        {
                            emailLogRequest.Body = this.GetEmailBody("Emails/Booking/SplitBooking", (mailData, baseUrl + string.Format(configuration["UrlSplitBookingCancel"], response.Id, entityName)));
                            await PublishQueueMessage(emailQueueRequest, emailLogRequest);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Result = SaveInspectionBookingResult.BookingSavedNotificationError;
            }
            return response;
        }

        [HttpPost("newbooking")]
        [Right("edit-booking")]
        public async Task<SaveInspectionBookingResponse> NewInspectionBooking([FromBody] SplitBooking request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            var isEdit = request.BookingId > 0 ? true : false;
            var response = await _manager.NewInspectionBooking(request);

            //Notification
            try
            {
                if (response.Result == SaveInspectionBookingResult.Success && response.Id > 0)
                {
                    SaveInsepectionRequest objRequest = new SaveInsepectionRequest();
                    var notifRres = await _manager.BookingTaskNotification(response.Id, response.isCombineOrderDataChanged, (int)BookingStatus.Received, objRequest);
                    BookingMailRequest mailData = await _manager.GetBookingMailDetail(response.Id, request.IsEmailRequired, isEdit);

                    var masterConfigs = await _manager.GetMasterConfiguration();
                    var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                    string baseUrl = _configuration["BaseUrl"];

                    var CCUserList = new List<string>();
                    var ToUserList = new List<string>();

                    if (notifRres.UserList.Count() > 0)
                    {
                        CCUserList.AddRange(notifRres.UserList.Select(x => x.EmailAddress));
                    }

                    if (notifRres.ToRecipients.Count() > 0)
                    {
                        ToUserList.AddRange(notifRres.ToRecipients.Select(x => x.EmailAddress));

                        // Broadcast message to accounting
                        broadCastService.Broadcast(notifRres.ToRecipients.Select(x => x.Id), new Notification
                        {
                            Title = "LINK Tasks Manager",
                            Message = $"Inspection Booking - {response.Id} {notifRres.StatusName}",
                            Url = baseUrl + string.Format(configuration["UrlBookingRequest"], response.Id, entityName),
                            TypeId = "Task"
                        });
                    }

                    if (!request.BookingData.IsEaqf && notifRres.CustomerEmail != null)
                    {
                        ToUserList.Add(notifRres.CustomerEmail);
                    }

                    if (CCUserList.Count == 0)
                    {
                        var bookingTeamGroupEmail = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.BookingTeamGroupEmail).Select(x => x.Value).FirstOrDefault();
                        CCUserList.Add(bookingTeamGroupEmail);
                    }


                    if (!request.BookingData.IsEaqf && request.BookingData.BookingType != (int)InspectionBookingTypeEnum.UnAnnounced)
                    {
                        ToUserList.Add(mailData.SupplierMail);
                        ToUserList.Add(mailData.FactoryMail);
                    }


                    if (ToUserList.Count() > 0)
                    {

                        var emailQueueRequest = new EmailDataRequest
                        {
                            TryCount = 1,
                            Id = Guid.NewGuid()
                        };

                        var emailLogRequest = new EmailLogData()
                        {
                            ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                            Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().ToList().Aggregate((x, y) => x + ";" + y) : "",
                            TryCount = 1,
                            Status = (int)EmailStatus.NotStarted,
                            SourceName = "Split Booking",
                            SourceId = response.Id,
                            Subject = $"{entityName} Inspection Booking - {notifRres.StatusName} (INS - {response.Id}, Customer: {mailData.CustomerName}, Supplier: {mailData.SupplierName})"
                        };

                        if (mailData.BookingType == (int)InspectionBookingTypeEnum.UnAnnounced)
                            emailLogRequest.Subject = emailLogRequest.Subject + ", BookingType:" + mailData.BookingTypeValue;

                        if (request.BookingData.StatusId == (int)BookingStatus.Received
                                    || request.BookingData.StatusId == (int)BookingStatus.Verified || request.BookingData.StatusId == (int)BookingStatus.Confirmed)
                        {
                            emailLogRequest.Body = this.GetEmailBody("Emails/Booking/SplitBooking", (mailData, baseUrl + string.Format(configuration["UrlSplitBookingCancel"], response.Id, entityName)));
                            await PublishQueueMessage(emailQueueRequest, emailLogRequest);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Result = SaveInspectionBookingResult.BookingSavedNotificationError;
            }
            return response;
        }

        [HttpGet("updatebookigstatus/{bookingid}/{statusid}")]
        [Right("edit-booking")]
        public async Task<BookingStatusUpdateResponse> UpdateBookigStatus(int bookingid, int statusid)
        {
            return await _manager.UpdateBookingStatus(bookingid, statusid);
        }

        //commented this api since this is no longer needed

        //// GET: api/Product/5
        //[HttpGet("{id}")]
        //[Right("edit-booking")]
        //public async Task<GetInspectionResponse> Get(int id)
        //{
        //    return await _manager.GetInspection(id);
        //}


        [HttpGet("GetBookingDetailsByCustomerId/{id}")]
        [Right("edit-booking")]
        public async Task<BookingCustomerDetails> GetBookingDetailsByCustomerId(int id)
        {
            return await _manager.GetBookingDetailsByCustomerId(id);
        }

        [HttpGet("GetServiceInspection/{id}")]
        [Right("edit-booking")]
        public async Task<CustomerServiceTypeResponse> GetServiceInspection(int id)
        {
            return await _customerManager.GetCustomerInspectionServiceType(id);
        }


        [HttpGet("GetBookingOffice")]
        [Right("edit-booking")]
        public OfficeSummaryResponse GetBookingOffice()
        {
            return _manager.GetBookingOffice();
        }

        [HttpPost("SearchInspection")]
        [Right("edit-booking")]
        public async Task<BookingSummarySearchResponse> SearchInspection([FromBody] InspectionSummarySearchRequest request)
        {
            return await _manager.GetAllInspectionsData(request);
        }

        [HttpPost("SearchInspectionReports")]
        [Right("edit-booking")]
        public async Task<ReportSummaryResponse> SearchInspectionReports([FromBody] InspectionSummarySearchRequest request)
        {
            _logger.LogInformation("SearchInspectionReports");
            return await _manager.GetAllInspectionReportProducts(request);
        }

        [Right("edit-booking")]
        [HttpPost("ExportInspectionSearchSummary")]
        public async Task<IActionResult> ExportInspectionSearchSummary([FromBody] InspectionSummarySearchRequest request)
        {
            if (request == null)
                return NotFound();
            var response = await _manager.ExportReportDataSummary(request);
            if (response == null)
                return NotFound();
            Stream stream = _helper.GetAsStreamObjectAndLoadDataTable(response.bookingList);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BookingSummaryTemplate.xlsx");
        }


        [HttpGet("GetFactoryDetailsById/{cusid},{factid},{bookingId}")]
        [Right("edit-booking")]
        public async Task<EditInspectionFactDetails> GetFactoryDetailsById(int cusid, int factid, int? bookingId)
        {
            return await _manager.GetFactoryDetailsByCustomerIdFactoryId(cusid, factid, bookingId);
        }

        [HttpGet("GetFactoryDetailsById/{factid}")]
        [Right("edit-booking")]
        public async Task<EditInspectionFactDetails> GetFactoryDetailsByFactoryId(int factid)
        {
            return await _manager.GetFactoryDetailsByCustomerIdFactoryId(null, factid, null);
        }

        [HttpGet("GetSupplierDetailsById/{cusid},{supid},{bookingId}")]
        [Right("edit-booking")]
        public async Task<EditInspectionBookingSupDetails> GetSupplierDetailsById(int cusid, int supid, int? bookingId)
        {
            return await _manager.GetSupplierDetailsByCustomerIdSupplierId(cusid, supid, bookingId);
        }

        [HttpGet("GetSupplierDetailsById/{supid}")]
        [Right("edit-booking")]
        public async Task<EditInspectionBookingSupDetails> GetSupplierDetailsBySupplierId(int supid)
        {
            return await _manager.GetSupplierDetailsByCustomerIdSupplierId(null, supid, null);
        }


        [HttpGet("GetInspectionBookingsummary")]
        [Right("edit-booking")]
        public async Task<BookingSummaryResponse> GetInspectionBookingSummary()
        {
            return await _manager.GetBookingSummary();
        }

        [HttpPost("productattached/{bookingid}")]
        [Right("edit-booking")]
        public bool UploadProductAttachedFiles(int bookingid)
        {
            if (Request.Form.Keys != null && Request.Form.Keys.Any())
            {
                var dict = new Dictionary<string, byte[]>();

                foreach (var key in Request.Form.Keys)
                {
                    dict.Add(key, null);
                }

                _manager.UploadProductFiles(dict, bookingid).Wait();
                return true;
            }
            return false;
        }

        [HttpGet("getunitdetails")]
        [Right("edit-booking")]
        public async Task<UnitDetailsResponse> GetUnitDetails()
        {
            return await _manager.GetUnits();
        }

        [HttpGet("productsbycustomerpoandcategory/{id}/{poid}")]
        [Right("edit-booking")]
        public IEnumerable<CustomerProduct> GetProductsByPOAndCustomer(int id, int poid)
        {
            var response = _manager.GetProductsByCustomerPOAndCategory(id, poid);
            return response;
        }

        [HttpGet("getaqlbyservicetype/{customerId}/{serviceTypeId}")]
        [Right("edit-booking")]
        public EditCustomerServiceConfigResponse GetServiceConfigDetails(int customerId, int serviceTypeId)
        {
            var response = _customerServiceManager.ServiceByCustomerAndServiceTypeID(customerId, serviceTypeId);
            return response;
        }

        [Right("edit-booking")]
        [HttpGet("file/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var file = await _manager.GetFile(id);

            if (file.Result == DTO.File.FileResult.NotFound)
                return NotFound();

            return File(file.Content, file.MimeType); // returns a FileStreamResult
        }



        [HttpGet("GetReInspectionServiceType/{id}")]
        [Right("edit-booking")]
        public async Task<CustomerServiceTypeResponse> GetReInspectionServiceType(int id)
        {
            return await _customerManager.GetReInspectionServiceType(id);
        }

        [HttpGet("GetReInspectionServiceTypes")]
        [Right("edit-booking")]
        public ReInspectionTypeResponse GetReInspectionServiceTypes()
        {
            return _manager.GetReInspectionTypes();
        }
        [HttpPost("getcustomercontactsbybrandordept")]
        [Right("edit-booking")]
        public Task<BookingCustomerContactDetails> GetCustomerContacts(BookingCustomerContactRequest request)
        {
            return _manager.GetCustomerContacts(request);
        }
        [HttpPost("isHolidayExists")]
        [Right("edit-booking")]
        public Task<Boolean> isHolidayExists(HolidayRequest request)
        {
            return _manager.IsHolidayExists(request);
        }
        [HttpGet("GetFactoryDetailsByCusSupId/{cusid},{supid}")]
        [Right("edit-booking")]
        public async Task<SupplierListResponse> GetFactoryDetailsByCusSupId(int cusid, int supid)
        {
            return await _suppliermanager.GetFactorysByUserType(cusid, supid);
        }

        [Right("edit-booking")]
        [HttpGet("fileTerms")]
        public async Task<IActionResult> GetInspBookingTermsFile()
        {
            var filepath = _env.WebRootPath;
            var masterConfigs = await _manager.GetMasterConfiguration();
            var inspBookTerms = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.InspBookTerms).Select(x => x.Value).FirstOrDefault();

            var file = _documentManager.GetFileData(filepath, inspBookTerms);

            if (file == null || file.Result == DTO.File.FileResult.NotFound || file.Content == null)
                return NotFound();

            return File(file.Content, file.MimeType); // returns a FileStreamResult
        }

        [Right("edit-booking")]
        [HttpGet("GetBookingProductsReports/{bookingId}/{reportId}/{containerId}")]
        public async Task<BookingProductAndReportDataResponse> GetBookingAndProductsAndReports(int bookingId, int reportId, int containerId)
        {
            var data = await _manager.GetBookingAndProductsAndReports(bookingId, reportId, containerId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetBookingProductStatus/{bookingId}")]
        public async Task<BookingProductsResponse> GetBookingProductsAndStatus(int bookingId)
        {
            var data = await _manager.GetBookingProductsAndStatus(bookingId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetBookingProducts/{bookingId}")]
        public async Task<BookingProductDataResponse> GetBookingProducts(int bookingId)
        {
            var data = await _manager.GetBookingProducts(bookingId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetBookingContainerStatus/{bookingId}")]
        public async Task<BookingContainerResponse> GetBookingContainerAndStatus(int bookingId)
        {
            var data = await _manager.GetBookingContainersAndStatus(bookingId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetBookingContainers/{bookingId}")]
        public async Task<BookingContainerResponse> GetBookingContainers(int bookingId)
        {
            var data = await _manager.GetBookingContainers(bookingId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetInspectionSummary/{reportId}")]
        public async Task<InspectionReportSummaryRepsonse> GetInspectionSummaryList(int reportId)
        {
            var data = await _manager.GetInspectionSummary(reportId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetInspectionDefects/{reportId}")]
        public async Task<InspectionDefectsRepsonse> GetInspectionDefectList(int reportId)
        {
            var data = await _manager.GetInspectionDefects(reportId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetInspectionDefects/{reportId}/{productRefId}")]
        public async Task<InspectionDefectsRepsonse> GetInspectionDefectListByInspectionAndReport(int reportId, int productRefId)
        {
            var data = await _manager.GetInspectionDefectsByReportandInspection(reportId, productRefId);
            return data;
        }


        [Right("edit-booking")]
        [HttpGet("GetInspectionDefectsByContainer/{reportId}/{containerRefId}")]
        public async Task<InspectionDefectsRepsonse> GetInspectionDefectListByInspectionAndContainer(int reportId, int containerRefId)
        {
            var data = await _manager.GetInspectionDefectsByReportandContainer(reportId, containerRefId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetBookingPoListbyProduct/{bookingId}/{productRefId}")]
        public async Task<BookingProductPOResponse> GetBookingPoListbyProduct(int bookingId, int productRefId)
        {
            var data = await _manager.GetBookingProductPoList(bookingId, productRefId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetBookingPoListbyContainerAndProduct/{bookingId}/{containerRefId}/{productRefId}")]
        public async Task<BookingProductPOResponse> GetBookingPoListbyContainerAndProduct(int bookingId, int containerRefId, int productRefId)
        {
            var data = await _manager.GetBookingProductPoList(bookingId, containerRefId, productRefId);
            return data;
        }

        [Right("edit-booking")]
        [HttpGet("GetBookingProductListbyContainer/{bookingId}/{containerRefId}")]
        public async Task<BookingContainerProductResponse> GetBookingProductListbyContainer(int bookingId, int containerRefId)
        {
            var data = await _manager.GetBookingContainerProductList(bookingId, containerRefId);
            return data;
        }

        [HttpGet("getfbReportTemplateList")]
        [Right("edit-booking")]
        public async Task<DataSourceResponse> GetFbReportTemplatDetails()
        {
            return await _manager.GetFbTemplateList();
        }

        [HttpGet("getBookingInfo/{bookingId}")]
        public async Task<BookingInformation> GetBookingInfo(int bookingId)
        {
            return await _manager.GetBookingInformation(bookingId);
        }
        [HttpPost("getPriceCategory")]
        public async Task<PriceCategoryResponse> GetPriceCategoryByCustomerIdPCSub2Id(PriceCategoryRequest request)
        {
            return await _manager.GetPriceCategoryByCustomerIdPCSub2Id(request);
        }

        [HttpGet("bookingProductValidationInfo/{bookingId}/{poTranId}/{productId}")]
        public async Task<ProductValidationResponse> BookingProductValidationInfo(int bookingId, int poTranId, int productId)
        {
            return await _manager.BookingProductValidationInfo(bookingId, poTranId, productId);
        }

        [HttpGet("booking-applicant-details")]
        public async Task<ApplicantStaffResponse> GetStaffInfoDetails()
        {
            return await _manager.GetApplicantInfoById();
        }

        [Right("edit-booking")]
        [HttpPost("ExportInspectionProductSummary")]
        public async Task<IActionResult> ExportInspectionProductSummary([FromBody] int bookingId)
        {
            if (bookingId <= 0)
                return NotFound();

            var response = await _manager.ExportBookingProductSummary(bookingId);
            if (response == null || !response.Any())
                return NotFound();
            return await this.FileAsync("BookingProductExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpGet("booking-status-data")]
        public async Task<DataSourceResponse> GetBookingStatusList()
        {
            return await _manager.GetBookingStatusList();
        }
        [HttpPost("cs-names")]
        public async Task<CSConfigResponse> GetCSNames(UserAccess userAccess)
        {
            return await _manager.getCSNames(userAccess);
        }

        [HttpPost("cs-list")]
        public async Task<CSConfigListResponse> GetCSList(UserAccess userAccess)
        {
            return await _manager.getCSList(userAccess);
        }

        [HttpPost("bookingProductValidation")]
        public async Task<List<ProductValidateData>> BookingProductValidation(List<ProductValidateData> request)
        {
            return await _manager.BookingProductValidation(request);
        }

        [Right("edit-booking")]
        [HttpGet("Getbookingsummarystatus")]
        public async Task<BookingSummaryStatusResponse> GetBookingSummaryStatus()
        {
            return await _manager.GetBookingSummaryStatus();
        }

        [Right("edit-booking")]
        [HttpGet("GetAEUserList")]
        public async Task<DataSourceResponse> GetAEUserList()
        {
            return await _manager.GetAEUserList();
        }

        [HttpGet("GetEditCustomerDetails/{id}/{bookingId}")]
        [Right("edit-booking")]
        public async Task<EditBookingCustomerDetails> GetEditCustomerDetails(int id, int bookingId)
        {
            return await _manager.GetEditBookingDetailByCustomerId(id, bookingId);
        }

        [HttpGet("GetEditBookingOffice/{bookingId}")]
        [Right("edit-booking")]
        public async Task<DataSourceResponse> GetEditBookingOffice(int bookingId)
        {
            return await _manager.GetEditBookingOffice(bookingId);
        }

        [HttpGet("GetEditBookingUnit/{bookingId}")]
        [Right("edit-booking")]
        public async Task<DataSourceResponse> GetEditBookingUnit(int bookingId)
        {
            return await _manager.GetEditBookingUnit(bookingId);
        }

        [HttpGet("GetHoldReasonTypes")]
        [Right("edit-booking")]
        public async Task<DataSourceResponse> GetHoldReasonTypes()
        {
            return await _manager.GetHoldReasonTypes();
        }

        [HttpGet("booking-inspection-locations/{bookingId}")]
        [Right("edit-booking")]
        public async Task<DataSourceResponse> GetEditBookingInspectionLocations(int bookingId)
        {
            return await _manager.GetEditBookingInspectionLocations(bookingId);
        }

        [HttpGet("booking-shipment-types/{bookingId}")]
        [Right("edit-booking")]
        public async Task<DataSourceResponse> GetEditBookingShipmentTypes(int bookingId)
        {
            return await _manager.GetEditBookingShipmentTypes(bookingId);
        }

        [HttpGet("booking-cu-product-category/{customerId}/{bookingId}")]
        [Right("edit-booking")]
        public async Task<DataSourceResponse> GetEditBookingCuProductCategory(int customerId, int bookingId)
        {
            return await _manager.GetEditBookingCuProductCategory(customerId, bookingId);
        }

        [HttpGet("get-booking-season-config/{customerId}/{bookingId}")]
        public async Task<DataSourceResponse> GetEditBookingCustomerSeason(int customerId, int bookingId)
        {
            return await _manager.GetEditBookingCustomerSeason(customerId, bookingId);
        }

        [HttpGet("booking-business-lines/{bookingId}")]
        [Right("edit-booking")]
        public async Task<DataSourceResponse> GetEditBookingBusinessLines(int bookingId)
        {
            return await _manager.GetEditBookingBusinessLines(bookingId);
        }
        [HttpGet("GetBookingInfoDetails/{bookingId}")]
        public async Task<BookingDataInfoResponse> GetBookingInfoDetails(int bookingId)
        {
            return await _manager.GetBookingInfoDetails(bookingId);
        }
        [HttpPost("GetInspectionPickingExists")]
        public async Task<bool> GetInspectionPickingExists(InspectionPickingExistRequest request)
        {
            return await _manager.GetInspectionPickingExists(request);
        }

        [HttpGet("get-inspproduct-base-detail/{bookingId}")]
        [Right("edit-booking")]
        public async Task<InspectionProductBaseDetailResponse> GetInspectionProductBaseDetail(int bookingId)
        {
            return await _manager.GetInspectionProductBaseDetails(bookingId);
        }

        [HttpPost("get-entpagefield-access")]
        public async Task<EntPageFieldAccessResponse> GetEntPageFieldAccess(EntPageRequest request)
        {
            return await _manager.GetEntPageFieldAccess(request);
        }

        [HttpPost("save-master-contact")]
        public async Task<SaveMasterContactResponse> SaveMasterContact(SaveMasterContactRequest request)
        {
            return await _manager.SaveMasterContact(request);
        }

        [HttpGet("get-booking-attachment/{bookingId}")]
        public async Task<BookingFileZipResponse> GetBookingAttachment(int bookingId)
        {
            return await _manager.GetBookingFileAttachment(bookingId);
        }

        [HttpPost("get-po-product-details")]
        [Right("edit-booking")]
        public async Task<POProductDetailResponse> GetPoProductDetails(PoProductDetailRequest poProductDetailRequest)
        {
            return await _manager.GetPoProductDetails(poProductDetailRequest);
        }

        [HttpPost("get-booking-po-product-details")]
        [Right("edit-booking")]
        public async Task<BookingPOProductListResponse> GetPoProductDetails(BookingPOProductDataSourceRequest request)
        {
            return await _manager.GetPoProductListBooking(request);
        }

        [HttpGet("get-purchase-order-sample-file/{typeId}")]
        public async Task<IActionResult> GetPurchaseOrderSampleFile(int typeId)
        {
            string purchaseOrderFile = "";

            var filepath = _env.WebRootPath;
            var masterConfigs = await _manager.GetMasterConfiguration();

            if (typeId == (int)PurchaseOrderSampleFile.ImportPOSampleFile)
                purchaseOrderFile = masterConfigs.Where(x => x.EntityId == _filterService.GetCompanyId()
                                        && x.Type == (int)EntityConfigMaster.ImportPurchaseOrderUpload).Select(x => x.Value).FirstOrDefault();

            else if (typeId == (int)PurchaseOrderSampleFile.ImportPODateFormat)
                purchaseOrderFile = masterConfigs.Where(x => x.EntityId == _filterService.GetCompanyId()
                                       && x.Type == (int)EntityConfigMaster.ImportPurchaseOrderDateFormat).Select(x => x.Value).FirstOrDefault();

            var file = _documentManager.GetFileData(filepath, purchaseOrderFile);

            if (file == null || file.Result == DTO.File.FileResult.NotFound || file.Content == null)
                return NotFound();

            return File(file.Content, file.MimeType);
        }

        [HttpPost("get-customer-contacts-address-list")]
        [Right("edit-booking")]
        public Task<BookingCustomerContactDetails> GetCustomerContactsAndAddressList(BookingCustomerContactRequest request)
        {
            return _manager.GetCustomerContacts(request);
        }


        /// <summary>
        /// publish the fb inspection booking queue
        /// </summary>
        /// <param name="bookingFbLog"></param>
        /// <returns></returns>
        private async Task PublishFbInspectionBookingQueue(BookingFbLogData bookingFbLog)
        {
            var resultId = await _bookingLogQueueManager.AddBookingFbQueueLog(bookingFbLog);
            var fbBookingRequest = new FbBookingRequest()
            {
                Id = Guid.NewGuid(),
                ResultId = resultId,
                TryCount = 1
            };
            await _rabbitMQClient.Publish<FbBookingRequest>(_configuration["FBBookingSyncQueue"], fbBookingRequest);
        }
    }
}
