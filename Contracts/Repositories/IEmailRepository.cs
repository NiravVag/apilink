using DTO.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Enums;
using DTO.RepoRequest.Email;

namespace Contracts.Repositories
{
    public interface IEmailRepository
    {
        /// <summary>
        /// Get Email recipient details
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<EmailRecipientRepoResponse> GetEmailRecipient(EmailRecipientRepoRequest request);

        /// <summary>
        /// Get internal default email recipients
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<List<MidEmailRecipientsInternalDefault>> GetInternalDefaultRecipientByEmailType(EmailType emailtypeid);

        /// <summary>
        /// Get customer email recipients
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<List<MidEmailRecipientsCusContactDefault>> GetCusDefaultRecipientByEmailType(EmailType emailtypeid);
    }
}
