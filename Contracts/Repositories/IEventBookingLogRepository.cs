using DTO.Inspection;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IEventBookingLogRepository : IRepository
    {
        Task<int> SaveLogInformation(EventBookingLog entity);
        List<BookingLogStatus> GetLogStatusByBooking(int bookingId);
        Task<int> SaveQuotationStatusLog(List<QuTranStatusLog> entity);
        Task<int> SaveFbBookingLog(FbBookingRequestLog entity);
        Task<int> SaveZohoBookingLog(ZohoRequestLog entity);
        Task<int> SaveTCFMasterRequestLog(TcfMasterDataLog entity);
        Task<int> SaveAPIGatewayLog(ApigatewayLog entity);
        Task<int> SaveEventLogInformation(EventLog entity);
        List<BookingLogStatus> GetLogStatusByBooking(List<int> bookingIds);
        List<BookingLogStatus> GetAuditLogStatusByBooking(List<int> bookingIds);
    }
}
