using DTO.EmailSend;
using DTO.EmailSendingDetails;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IEmailSendingDetailsRepository : IRepository
    {
        Task<List<CustomerDecisionEmailData>> GetCustomerDecisionEmailData(List<int> reportIdList);
        Task<List<EmailSendingDetails>> GetEmailConfiguration(int typeId, int customerId, int serviceId);
        Task<List<string>> GetAPIDefaultContacts(int officeId);
        Task<List<string>> GetBookingFactoryEmailContacts(int bookingId);
        Task<List<string>> GetBookingSupplierEmailContacts(int bookingId);
        Task<List<string>> GetBookingCustomerEmailContacts(int bookingId);
        Task<List<CustomerDecisionEmailContainerData>> GetContainerCustomerDecisionEmailData(List<int> reportIdList);
        Task<List<ReportDetailsRepo>> GetCusDecisionReportDetails(IEnumerable<int> reportIdList);
    }
}
