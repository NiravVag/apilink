using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Components.Core.contracts;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.CancelBooking;
using DTO.Common;
using DTO.CommonClass;
using DTO.Eaqf;
using DTO.EmailLog;
using DTO.Inspection;
using DTO.MasterConfig;
using DTO.Quotation;
using Entities.Enums;
using LINK_UI.App_start;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CancelBookingController : ControllerBase
    {
        private readonly ICancelBookingManager _cbManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IInspectionBookingManager _manager = null;
        private readonly IEmailsManager _mailManager = null;
        private readonly IConfiguration _configuration = null;
        private readonly IQuotationManager _quotationManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private readonly IUserRightsManager _userManager = null;
        private readonly IEmailManager _emailManager;
        private readonly ITenantProvider _filterService = null;
        private readonly IEaqfEventUpdateManager _eaqfEventUpdate = null;
        public CancelBookingController(ICancelBookingManager cbManager, IAPIUserContext applicationContext,
            IInspectionBookingManager manager, IEmailsManager mailManager, IUserRightsManager userManager,
        IConfiguration configuration, IQuotationManager quotationManager, IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IEmailManager emailManager,
        ITenantProvider filterService, IEaqfEventUpdateManager eaqfEventUpdate)
        {
            _cbManager = cbManager;
            _ApplicationContext = applicationContext;
            _manager = manager;
            _mailManager = mailManager;
            _configuration = configuration;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _quotationManager = quotationManager;
            _userManager = userManager;
            _emailManager = emailManager;
            _filterService = filterService;
            _eaqfEventUpdate = eaqfEventUpdate;
        }

        #region CancelBooking
        [HttpGet("getReason/{id}")]
        [Right("cancel-booking")]
        public async Task<BookingCancelRescheduleResponse> GetReason(int id)
        {
            return await _cbManager.GetReason(id);
        }
        [HttpPost("saveCancelBooking")]
        [Right("cancel-booking")]
        public async Task<SaveCancelBookingResponse> SaveCancel([FromBody] SaveCancelBookingRequest request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            if (request == null)
                return new SaveCancelBookingResponse { Result = SaveCancelBookingResponseResult.RequestNotCorrectFormat };

            var fbToken = getFbToken();
            var response = await _cbManager.SaveBookingCancelDetails(request, fbToken);

            var masterConfigs = await _manager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            string baseUrl = _configuration["BaseUrl"];

            // Cancel the quotation if booking is cancelled and send the cancel quotation email and notifications
            if (response.QuotationData != null && response.QuotationData.Id > 0)
            {
                try
                {
                    var actionSendList = new Dictionary<QuotationStatus, Action<SendEmailRequest>>()
                    {
                        { QuotationStatus.Canceled, (emailRequest) => this.SendEmail("Emails/Quotation/QuotationCancelled", emailRequest, baseUrl, entityName) }
                    };

                    var requestManager = new SetStatusBusinessRequest
                    {
                        Id = response.QuotationData.Id,
                        CusComment = response.QuotationData.CusComment,
                        IdStatus = QuotationStatus.Canceled,
                        OnSendEmail = actionSendList.TryGetValue(QuotationStatus.Canceled, out Action<SendEmailRequest> post) ? post : null,
                        ApiRemark = response.QuotationData.ApiRemark,
                        Url = baseUrl + string.Format(_configuration["UrlQuotation"], response.QuotationData.Id, entityName),
                        ApiInternalRemark = response.QuotationData.ApiInternalRemark + "Inspection cancelled on " + DateTime.Now.ToString()
                    };

                    var result = await _quotationManager.SetStatus(requestManager);
                }
                catch (Exception e)
                {
                    response.Result = SaveCancelBookingResponseResult.QuotationNotCancelledError;
                }
            }

            //Notification
            try
            {
                if (response.Result == SaveCancelBookingResponseResult.Success && request.BookingId > 0)
                {
                    if (response.IsEaqf)
                    {
                        EAQFEventUpdate cancelRequest = new EAQFEventUpdate();
                        cancelRequest.BookingId = request.BookingId;
                        cancelRequest.ReasonTypeId = request.ReasonTypeId;
                        await _eaqfEventUpdate.UpdateRescheduleStatusToEAQF(cancelRequest, EAQFBookingEventRequestType.CancelStatus);
                    }

                    SaveInsepectionRequest objRequest = new SaveInsepectionRequest();
                    var notifRres = await _manager.BookingTaskNotification(request.BookingId, false, (int)BookingStatus.Cancel, objRequest);

                    BookingMailRequest mailData = await _manager.GetBookingMailDetail(request.BookingId, null, null);

                    var entityId = _filterService.GetCompanyId();
                    var _settings = _emailManager.GetMailSettingConfiguration(entityId);

                    var CCUserList = new List<string>();
                    var ToUserList = new List<string>();
                    mailData.quotationExists = notifRres.quotationExists;
                    mailData.SenderEmail = _settings.SenderEmail;
                    if (notifRres.UserList != null)
                    {
                        CCUserList.AddRange(notifRres.UserList.Select(x => x.EmailAddress));
                    }

                    if (notifRres.ToRecipients != null)
                    {
                        ToUserList.AddRange(notifRres.ToRecipients.Select(x => x.EmailAddress));

                        // Broadcast message to accounting
                        broadCastService.Broadcast(notifRres.ToRecipients.Select(x => x.Id), new DTO.Common.Notification
                        {
                            Title = "LINK Tasks Manager",
                            Message = $"Inspection Booking - {request.BookingId} {BookingStatus.Cancel}",
                            Url = baseUrl + string.Format(configuration["UrlBookingCancel"], request.BookingId, entityName),
                            TypeId = "Task"
                        });
                    }

                    if (!response.IsEaqf && notifRres.CustomerEmail != null)
                    {
                        ToUserList.Add(notifRres.CustomerEmail);
                    }

                    if (!response.IsEaqf && response.BookingType != (int)InspectionBookingTypeEnum.UnAnnounced)
                    {
                        ToUserList.Add(mailData.SupplierMail);
                        ToUserList.Add(mailData.FactoryMail);
                    }


                    // add accounting team in cc if invoice exist and its cancelled

                    if (response.CancelInvoiceData != null && response.CancelInvoiceData.InvoiceCancelUserEmail != null && response.CancelInvoiceData.InvoiceCancelUserEmail.Any())
                    {
                        CCUserList.AddRange(response.CancelInvoiceData.InvoiceCancelUserEmail?.Select(x => x.EmailAddress));
                    }

                    // send email

                    if (ToUserList.Any())
                    {
                        var emailQueueRequest = new EmailDataRequest
                        {
                            TryCount = 1,
                            Id = Guid.NewGuid()
                        };

                        var emailLogRequest = new EmailLogData()
                        {
                            ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().Aggregate((x, y) => x + ";" + y) : "",
                            Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().Aggregate((x, y) => x + ";" + y) : "",
                            TryCount = 1,
                            Status = (int)EmailStatus.NotStarted,
                            SourceName = "Cancel Booking",
                            SourceId = request.BookingId,
                            Subject = $"{entityName} Inspection Booking - {BookingStatus.Cancel} (INS - {request.BookingId}, Customer: {mailData.CustomerName}, Supplier: {mailData.SupplierName})",
                        };

                        emailLogRequest.Subject = mailData.BookingType == (int)InspectionBookingTypeEnum.UnAnnounced ? emailLogRequest.Subject + ", BookingType: " + mailData.BookingTypeValue + ")" : emailLogRequest.Subject + ")";

                        mailData.EntityName = entityName;
                        emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingCancel", (mailData, baseUrl + string.Format(configuration["UrlBookingCancel"], request.BookingId, entityName)));
                        PublishQueueMessage(emailQueueRequest, emailLogRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Result = SaveCancelBookingResponseResult.BookingSavedNotificationError;
            }
            return response;
        }
        #endregion CancelBooking
        #region RescheduleBooking

        [HttpGet("getRescheduleReason/{id}")]
        [Right("reschedule-booking")]
        public async Task<BookingCancelRescheduleResponse> GetRescheduleReason(int id)
        {
            return await _cbManager.GetRescheduleReason(id);
        }
        [HttpPost("saveReschedule")]
        [Right("reschedule-booking")]
        public async Task<SaveRescheduleResponse> SaveReschedule([FromBody] SaveRescheduleRequest request, [FromServices] IBroadCastService broadCastService, [FromServices] IConfiguration configuration)
        {
            if (request == null)
                return new SaveRescheduleResponse { Result = SaveRescheduleResult.RequestNotCorrectFormat };
            // Save request
            var fbToken = getFbToken();
            var response = await _cbManager.SaveRescheduleDetails(request, fbToken);

            //Notification
            try
            {
                if (response.Result == SaveRescheduleResult.Success && request.BookingId > 0)
                {
                    if (response.IsEaqf)
                    {
                        EAQFEventUpdate cancelRequest = new EAQFEventUpdate();
                        cancelRequest.ServiceFromDate = request.ServiceFromDate;
                        cancelRequest.ServiceToDate = request.ServiceToDate;
                        cancelRequest.BookingId = request.BookingId;
                        cancelRequest.ReasonTypeId = request.ReasonTypeId;
                        await _eaqfEventUpdate.UpdateRescheduleStatusToEAQF(cancelRequest, EAQFBookingEventRequestType.DateChange);
                    }

                    SaveInsepectionRequest objRequest = new SaveInsepectionRequest();
                    var notifRres = await _manager.BookingTaskNotification(request.BookingId, false, (int)BookingStatus.Rescheduled, objRequest);
                    BookingMailRequest mailData = await _manager.GetBookingMailDetail(request.BookingId, null, null);

                    var masterConfigs = await _manager.GetMasterConfiguration();
                    var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                    string baseUrl = _configuration["BaseUrl"];

                    var subjectDate = mailData.ServiceDateFrom != mailData.ServiceDateTo ? mailData.ServiceDateFrom + " - " + mailData.ServiceDateTo : mailData.ServiceDateFrom;

                    var entityId = _filterService.GetCompanyId();
                    var _settings = _emailManager.GetMailSettingConfiguration(entityId);
                    mailData.SenderEmail = _settings.SenderEmail;

                    var CCUserList = new List<string>();
                    var ToUserList = new List<string>();
                    mailData.quotationExists = notifRres.quotationExists;

                    if (notifRres.UserList != null)
                    {
                        CCUserList.AddRange(notifRres.UserList.Select(x => x.EmailAddress));
                    }

                    if (notifRres.ToRecipients != null)
                    {
                        ToUserList.AddRange(notifRres.ToRecipients.Select(x => x.EmailAddress));

                        // Broadcast message to accounting
                        broadCastService.Broadcast(notifRres.ToRecipients.Select(x => x.Id), new DTO.Common.Notification
                        {
                            Title = "LINK Tasks Manager",
                            Message = $"Inspection Booking - {request.BookingId} {BookingStatus.Rescheduled}",
                            Url = baseUrl + string.Format(configuration["UrlBookingRequest"], request.BookingId, entityName),
                            TypeId = "Task"
                        });
                    }

                    if (notifRres.CustomerEmail != null && response.IsEaqf)
                    {
                        ToUserList.Add(notifRres.CustomerEmail);
                    }
                    if (response.BookingType != (int)InspectionBookingTypeEnum.UnAnnounced && response.IsEaqf)
                    {
                        ToUserList.Add(mailData.SupplierMail);
                        ToUserList.Add(mailData.FactoryMail);
                    }

                    mailData.QuotationId = notifRres.QuotationId;

                    // send email
                    if (ToUserList.Any())
                    {
                        var emailQueueRequest = new EmailDataRequest
                        {
                            TryCount = 1,
                            Id = Guid.NewGuid()
                        };

                        var emailLogRequest = new EmailLogData()
                        {
                            ToList = (ToUserList != null && ToUserList.Count > 0) ? ToUserList.Distinct().Aggregate((x, y) => x + ";" + y) : "",
                            Cclist = (CCUserList != null && CCUserList.Count > 0) ? CCUserList.Distinct().Aggregate((x, y) => x + ";" + y) : "",
                            TryCount = 1,
                            Status = (int)EmailStatus.NotStarted,
                            SourceName = "Reschedule Booking",
                            SourceId = request.BookingId,
                            Subject = $"{entityName} Inspection Booking - {BookingStatus.Rescheduled} with New Service Date on {subjectDate} (INS No.- {request.BookingId}, Customer: {mailData.CustomerName}, Supplier: {mailData.SupplierName})"
                        };

                        emailLogRequest.Subject = mailData.BookingType == (int)InspectionBookingTypeEnum.UnAnnounced ? emailLogRequest.Subject + ", BookingType: " + mailData.BookingTypeValue + ")" : emailLogRequest.Subject + ")";

                        mailData.EntityName = entityName;
                        emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingReschedule", (mailData, baseUrl + string.Format(configuration["UrlBookingRequest"], request.BookingId, entityName)));
                        PublishQueueMessage(emailQueueRequest, emailLogRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Result = SaveRescheduleResult.BookingSavedNotificationError;
            }
            return response;
        }
        #endregion RescheduleBooking
        #region CancelRescheduleBooking
        [HttpGet("getCurrency")]
        [Right("cancelreschedule-booking")]
        public async Task<CurrencyResponse> GetCurrency()
        {
            return await _cbManager.GetCurrency();
        }
        [HttpGet("getCancelEditBookingDetails/{id},{bookingstatus}")]
        [Right("cancelreschedule-booking")]
        public async Task<EditCancelBookingResponse> GetCancelEditDetails(int id, int bookingstatus)
        {
            return await _cbManager.GetCancelBookingDetails(id, bookingstatus);
        }
        #endregion CancelRescheduleBooking

        private void SendEmail(string ViewPath, SendEmailRequest request, string baseUrl, string entityName)
        {
            string url = baseUrl + string.Format(_configuration["UrlQuotation"], request.Model.Id, entityName);

            var emailQueueRequest = new EmailDataRequest
            {
                TryCount = 1,
                Id = Guid.NewGuid()
            };

            var emailLogRequest = new EmailLogData()
            {
                ToList = (request.RecepientList != null && request.RecepientList.Any()) ? request.RecepientList.Distinct().Aggregate((x, y) => x + ";" + y) : "",
                Cclist = (request.RecepientList != null && request.RecepientList.Any()) ? request.RecepientList.Distinct().Aggregate((x, y) => x + ";" + y) : "",
                TryCount = 1,
                Status = (int)EmailStatus.NotStarted,
                SourceName = "Cancel Quotation",
                SourceId = request.Model.Id,
                Subject = request.Subject
            };

            emailLogRequest.Body = this.GetEmailBody(ViewPath, (request.Model, request.RecepitName, url));
            PublishQueueMessage(emailQueueRequest, emailLogRequest);
        }

        /// <summary>
        /// Save email data into log table and publish to queue
        /// </summary>
        /// <param name="emailQueueRequest"></param>
        /// <param name="emailLogRequest"></param>
        /// <returns></returns>
        private void PublishQueueMessage(EmailDataRequest emailQueueRequest, EmailLogData emailLogRequest)
        {
            if (!string.IsNullOrWhiteSpace(emailLogRequest.ToList) || !string.IsNullOrWhiteSpace(emailLogRequest.Cclist))
            {
                var resultId = _emailLogQueueManager.AddEmailLog(emailLogRequest).Result;
                emailQueueRequest.EmailQueueId = resultId;
                _rabbitMQClient.Publish<EmailDataRequest>(_configuration["EmailQueue"], emailQueueRequest);
            }
        }

        /// <summary>
        /// Get FB token based on the needs
        /// </summary>
        /// <returns></returns>
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
    }
}