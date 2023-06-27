using DTO.EmailSend;
using DTO.EmailSendingDetails;
using DTO.InspectionCustomerDecision;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IEmailSendingDetailsManager
    {
        Task<EmailPreviewResponse> GetCustomerDecisionEmailConfigurationContacts(List<int> reportIdList, int bookingId, bool sendEmailToFactoryContacts, int customerResultId);
    }
}
