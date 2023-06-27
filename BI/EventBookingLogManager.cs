using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.EventBookingLog;
using DTO.Quotation;
using DTO.TCF;
using Entities;
using Entities.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class EventBookingLogManager : IEventBookingLogManager
    {
        private readonly IEventBookingLogRepository _eventLogRepository = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ITenantProvider _filterService = null;

        public EventBookingLogManager(IEventBookingLogRepository eventLogRepository, IAPIUserContext ApplicationContext, ITenantProvider filterService)
        {
            _eventLogRepository = eventLogRepository;
            _ApplicationContext = ApplicationContext;
            _filterService = filterService;
        }

        public async Task<int> SaveLogInformation(EventBookingLogInfo request)
        {
            var eventBookingLog = new EventBookingLog()
            {
                AuditId = request.AuditId,
                BookingId = request.BookingId,
                QuotationId = request?.QuotationId ?? 0,
                StatusId = request.StatusId,
                LogInformation = request.LogInformation,
                CreatedBy = request.UserId > 0 ? request.UserId.Value : _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                EntityId = _filterService.GetCompanyId()
            };
            return await _eventLogRepository.SaveLogInformation(eventBookingLog);
        }

        public async Task<int> SaveEventLogInformation(EventLogRequest request)
        {
            var eventBookingLog = new EventLog()
            {
                Name = request.Name,
                Message = request.Message,
                CreatedTime = request.CreatedTime,
                EventId = request.EventId,
                LogLevel = request.LogLevel,
                Exception = request.Exception,
                //  ResponseTime = request.ResponseTime
            };
            return await _eventLogRepository.SaveEventLogInformation(eventBookingLog);
        }

        /// <summary>
        /// Save FB Log Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> SaveFbBookingRequestLog(FBBookingLogInfo request)
        {
            var fbLog = new FbBookingRequestLog()
            {
                ServiceId = request.ServiceId,
                LogInformation = request.LogInformation,
                MissionId = request.MissionId,
                AccountId = request.AccountId,
                MissionProductId = request.MissionProductId,
                BookingId = request.BookingId,
                ReportId = request.ReportId,
                RequestUrl = request.RequestUrl,
                CreatedOn = DateTime.Now,
                CreatedBy = request.CreatedBy != -1 ? _ApplicationContext.UserId : request.CreatedBy,
                EntityId = _filterService.GetCompanyId()
            };
            return await _eventLogRepository.SaveFbBookingLog(fbLog);
        }

        /// <summary>
        /// Save Zoho Log Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> SaveZohoRequestLog(ZohoRequestLogInfo request)
        {
            var zohoLog = new ZohoRequestLog()
            {
                LogInformation = request.LogInformation,
                CustomerId = request.CustomerId,
                RequestUrl = request.RequestUrl,
                CreatedOn = DateTime.Now,
                CreatedBy = request.CreatedBy
            };
            return await _eventLogRepository.SaveZohoBookingLog(zohoLog);
        }

        public async Task<int> SaveQuotationStatusLog(List<QuTranStatusLog> quot, QuotationStatus status)
        {
            List<QuTranStatusLog> log = new List<QuTranStatusLog>();
            var entityId = _filterService.GetCompanyId();
            foreach (var item in quot)
            {
                var data = new QuTranStatusLog()
                {
                    QuotationId = item.QuotationId,
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    StatusId = item.StatusId == 0 ? (int)QuotationStatus.QuotationCreated : item.StatusId,
                    BookingId = item.BookingId,
                    AuditId = item.AuditId,
                    StatusChangeDate = item.StatusChangeDate,
                    EntityId = entityId
                };

                log.Add(data);
            }

            return await _eventLogRepository.SaveQuotationStatusLog(log);
        }

        /// <summary>
        /// Save the TCF Master Data Log
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> SaveTCFMasterRequestLog(TCFMasterRequestLogInfo request)
        {
            var tcfMasterRequestLog = new TcfMasterDataLog()
            {
                AccountId = request.AccountId,
                DataType = request.DataType,
                RequestUrl = request.RequestUrl,
                LogInformation = request.LogInformation,
                ResponseMessage = request.ResponseMessage,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now
            };
            return await _eventLogRepository.SaveTCFMasterRequestLog(tcfMasterRequestLog);
        }

        /// <summary>
        /// Save the Common API Request Log (it is generic function but currently using only for tcf system)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> SaveAPIGatewayLog(ApigatewayLog request)
        {
            var apiGatewayLog = new ApigatewayLog()
            {
                RequestUrl = request.RequestUrl,
                LogInformation = request.LogInformation,
                ResponseMessage = request.ResponseMessage,
                RequestBaseUrl = request.RequestBaseUrl,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now
            };
            return await _eventLogRepository.SaveAPIGatewayLog(apiGatewayLog);
        }
    }
}
