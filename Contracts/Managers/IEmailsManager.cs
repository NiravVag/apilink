using DTO.Email;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
   public interface IEmailsManager
    {
        /// <summary>
        /// Get default email recipients
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<DeafultEmailRecipientResponse> GetInternalDefaultRecipientByEmailType(EmailType emailtypeid);

        /// <summary>
        /// Get Inspection email recipients
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<InspEmailRecipientResponse> GetInspEmailRecipient(InspEmailRecipientRequest request);

        /// <summary>
        /// Get Audit email recipients
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<AudEmailRecipientResponse> GetAudEmailRecipient(AudEmailRecipientRequest request);
    }
}
