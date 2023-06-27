using Components.Core.entities.Emails;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Components.Core.contracts
{
    public interface IEmailManager
    {
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void SendEmail(EmailRequest request, int entityId);

        bool SendEmail(EmailInfoRequest request, int entityId);

        /// <summary>
        /// Send multiple emails in async 
        /// </summary>
        /// <param name="emailList"></param>
        void SendEmails(IEnumerable<EmailRequest> emailList);

        SmtpSettings GetMailSettingConfiguration(int entityId);

    }
}
