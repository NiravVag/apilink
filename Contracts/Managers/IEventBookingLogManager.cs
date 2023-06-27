using DTO.EventBookingLog;
using DTO.Quotation;
using DTO.TCF;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IEventBookingLogManager
    {
        Task<int> SaveLogInformation(EventBookingLogInfo request);
        Task<int> SaveQuotationStatusLog(List<QuTranStatusLog> quot, QuotationStatus status);
        Task<int> SaveFbBookingRequestLog(FBBookingLogInfo request);
        Task<int> SaveZohoRequestLog(ZohoRequestLogInfo request);
        Task<int> SaveTCFMasterRequestLog(TCFMasterRequestLogInfo request);
        Task<int> SaveAPIGatewayLog(ApigatewayLog request);
        Task<int> SaveEventLogInformation(EventLogRequest request);
    }
}
