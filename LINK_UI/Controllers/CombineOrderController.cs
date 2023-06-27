using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Core.contracts;
using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DTO.CombineOrders;
using DTO.Common;
using DTO.EmailLog;
using DTO.Inspection;
using DTO.MasterConfig;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;

namespace LINK_UI.Controllers
{
    /// <summary>
    /// This API will handle Booking Combine Orders
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CombineOrderController : ControllerBase
    {
        private readonly ICombineOrdersManager _manager = null;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
        private static IConfiguration _configuration = null;
        private readonly IEmailManager _emailManager;
        private readonly ITenantProvider _filterService = null;
        public CombineOrderController(ICombineOrdersManager manager,
            IInspectionBookingManager inspManager, IAPIUserContext applicationContext,
            IRabbitMQGenericClient rabbitMQClient, IEmailLogQueueManager emailLogQueueManager, IConfiguration configuration, IEmailManager emailManager, ITenantProvider filterService)
        {
            _manager = manager;
            _inspManager = inspManager;
            _ApplicationContext = applicationContext;
            _rabbitMQClient = rabbitMQClient;
            _emailLogQueueManager = emailLogQueueManager;
            _configuration = configuration;
            _emailManager = emailManager;
            _filterService = filterService;
        }

        [HttpGet("{bookingid}")]
        [Right("edit-combineorders")]
        public async Task<CombineOrderSummaryResponse> GetCombineOrders(int bookingid)
        {
            return await _manager.GetCombineOrderDetails(bookingid);
        }

        [HttpGet("GetPoDetails/{bookingid}/{productrefId}")]
        [Right("edit-combineorders")]
        public async Task<PoDetailsResponse> GetPoDetails(int bookingid, int productrefId)
        {
            return await _manager.GetPoDetails(bookingid, productrefId);
        }

        [HttpGet("GetSamplingQuantity/{bookingid}/{productid}")]
        public async Task<int> GetSamplingQuantity(int bookingid, int productid)
        {
            return await _manager.GetSamplingQuantityByBookingProduct(bookingid, productid);
        }
        [HttpGet("GetCombineAqlQuantity/{bookingid}/{productid}")]
        public async Task<int?> GetCombineAqlQuantity(int bookingid, int productid)
        {
            return await _manager.GetCombineAqlQuantityByBookingProduct(bookingid, productid);
        }

        //[HttpPost("GetAqlQuantityByProducts/{bookingid}")]
        //public async Task<AqlProductListResponse> GetAqlQuantityByProducts(int bookingid, List<int> productIdList)
        //{
        //    return await _manager.GetAqlQuantityByProducts(bookingid, productIdList);
        //}

        [HttpPost("GetSamplingQuantity/{bookingid}/{aqlId}")]
        public async Task<CombineSamplingProductListResponse> GetCombineOrderSamplingQuantity(int bookingid, int? aqlId, List<CombineOrderSamplingData> combineOrders)
        {
            return await _manager.GetCombinedAQLQty(bookingid, aqlId, combineOrders);
        }
        [HttpPost("SaveCombineOrders/{bookingid}")]
        [Right("edit-combineorders")]
        public async Task<SaveCombineOrdersResponse> SaveCombineOrders(List<SaveCombineOrdersRequest> combineOrders, int bookingid, [FromServices] IConfiguration configuration)
        {
            var response = await _manager.SaveCombineOrders(combineOrders, bookingid);

            if (response.isEmailRequired)
            {
                BookingMailRequest mailData = await _inspManager.GetBookingMailDetail(response.Id, true, true);

                foreach (var item in mailData.InspectionPoList)
                {
                    //if the products are combined and AQL is not selected, make the first product in the list as parent product
                    var isAQLSelected = mailData.InspectionPoList.Where(z => z.CombineProductId == item.CombineProductId
                                            && z.CombineAqlQty.GetValueOrDefault() > 0).Count();

                    item.SampleQty = await GetSamplingQuantity(bookingid, item.Id);
                    item.CombineProductCount = item.CombineProductId > 0 ? mailData.InspectionPoList.Where(x => x.CombineProductId == item.CombineProductId).Count() : 1;
                    item.IsParentProduct = (item.CombineProductId.GetValueOrDefault() == 0) ? true : (item.CombineAqlQty != null &&
                                            item.CombineAqlQty != 0) ? true : (isAQLSelected == 0 && mailData.InspectionPoList.Where(z => z.CombineProductId == item.CombineProductId).FirstOrDefault().ProductId == item.ProductId ? true : false);
                }

                mailData.InspectionPoList.Select(x => GetSamplingQuantity(bookingid, x.Id));
                var CCUserList = new List<string>();
                var ToUserList = new List<string>();
                mailData.quotationExists = true;
                mailData.StatusName = "Modified";

                mailData.InspectionPoList = mailData.InspectionPoList.OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQty).ThenBy(x => x.ProductId).ToList();


                if (response.CcRecipients != null)
                {
                    CCUserList.AddRange(response.CcRecipients.Select(x => x.EmailAddress));
                }

                if (response.ToRecipients != null)
                {
                    ToUserList.AddRange(response.ToRecipients.Select(x => x.EmailAddress));
                }
                var masterConfigs = await _inspManager.GetMasterConfiguration();
                var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                string baseUrl = _configuration["BaseUrl"];
                //To user list .has to be mandatory if no data , send to Booking team.
                if (CCUserList.Count == 0 || ToUserList.Count == 0)
                {
                    var bookingTeamGroupEmail = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.BookingTeamGroupEmail).Select(x => x.Value).FirstOrDefault();

                    CCUserList.Add(bookingTeamGroupEmail);
                }
                var entityId = _filterService.GetCompanyId();
                var _settings = _emailManager.GetMailSettingConfiguration(entityId);
                mailData.SenderEmail = _settings.SenderEmail;

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
                    SourceName = "Combine Order",
                    SourceId = bookingid,
                    Subject = $"{entityName} Inspection Booking - combine products Modified (INS - {response.Id}, Customer: {mailData.CustomerName}, Supplier: {mailData.SupplierName})"
                };
                mailData.EntityName = entityName;
                emailLogRequest.Body = this.GetEmailBody("Emails/Booking/BookingCombineProduct", (mailData, baseUrl + string.Format(configuration["UrlBookingRequest"], response.Id, entityName)));

                await PublishQueueMessage(emailQueueRequest, emailLogRequest);
            }

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

        [Right("edit-combineorders")]
        [HttpPost("ExportCombineOrders/{bookingId}")]
        public async Task<IActionResult> ExportInspectionSearchSummary(int bookingId)
        {
            if (bookingId == 0)
                return NotFound();

            var response = await _manager.GetCombineOrderDetails(bookingId);

            if (response == null)
                return NotFound();

            return await this.FileAsync("CombineOrders", response.CombineOrdersList, Components.Core.entities.FileType.Excel);
        }
    }
}
