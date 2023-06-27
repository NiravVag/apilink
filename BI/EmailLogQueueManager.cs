using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.EmailLog;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class EmailLogQueueManager : IEmailLogQueueManager
    {
        private readonly IEmailLogQueueRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ITenantProvider _tenantProvider = null;
        private static IConfiguration _Configuration = null;

        public EmailLogQueueManager(IEmailLogQueueRepository repo,
            IAPIUserContext ApplicationContext, IConfiguration configuration, ITenantProvider tenantProvider)
        {
            _repo = repo;
            _ApplicationContext = ApplicationContext;
            _Configuration = configuration;
            _tenantProvider = tenantProvider;
        }
        /// <summary>
        /// Add Email Log Queue
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> AddEmailLog(EmailLogData request)
        {
            int userId = 0;
            int.TryParse(_Configuration["ExternalAccessorUserId"], out userId);
            var entityId = _tenantProvider.GetCompanyId();
            var emailData = new LogEmailQueue()
            {
                Id = request.Id,
                SourceId = request.SourceId,
                SourceName = request.SourceName,
                Subject = request.Subject,
                Body = request.Body,
                Cclist = request.Cclist,
                Bcclist = request.Bcclist,
                ToList = request.ToList,
                CreatedBy = _ApplicationContext.UserId != 0 ? _ApplicationContext.UserId : userId,
                Active = true,
                CreatedOn = DateTime.Now,
                TryCount = request.TryCount,
                Status = request.Status,
                EntityId = entityId
            };

            // Add email attachments
            if (request.FileList != null)
            {
                foreach (var item in request.FileList)
                {
                    var attachment = new LogEmailQueueAttachment()
                    {
                        GuidId = Guid.NewGuid(),
                        FileName = item.Name,
                        File = item.Content,
                        FileLink = item.FileLink,
                        FileUniqueId = item.FileUniqueId,
                        FileStorageType = item.FileStorageType,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId != 0 ? _ApplicationContext.UserId : userId,
                        CreatedOn = DateTime.Now,
                        EntityId = entityId
                    };

                    emailData.LogEmailQueueAttachments.Add(attachment);
                }
            }

            // Add email reports and booking data
            if (request.BookingReportList != null)
            {
                foreach (var item in request.BookingReportList)
                {
                    if (item.InspectionId > 0 || item.AuditId > 0)
                    {
                        var bookingReport = new LogBookingReportEmailQueue()
                        {
                            InspectionId = item.InspectionId > 0 ? item.InspectionId : null,
                            ReportId = item.ReportId > 0 ? item.ReportId : null,
                            EsTypeId = item.EsTypeId,
                            EntityId = entityId,
                            ReportRevision = item.ReportRevision,
                            ReportVersion = item.ReportVersion
                        };
                        emailData.LogBookingReportEmailQueues.Add(bookingReport);
                    }
                }
            }

            return await _repo.AddEmailLog(emailData);
        }

        /// <summary>
        /// Update Email Log Queue
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> UpdateEmailLog(LogEmailQueue request)
        {
            return await _repo.EditEmailLog(request);
        }

        /// <summary>
        /// Get Email Log Queue
        /// </summary>
        /// <param name="emailLogId"></param>
        /// <returns></returns>

        public async Task<LogEmailQueue> GetEmailLogById(int emailLogId)
        {
            return await _repo.GetEmailLogById(emailLogId);
        }
    }
}
