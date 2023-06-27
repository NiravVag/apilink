using Contracts.Repositories;
using DTO.CommonClass;
using DTO.EmailSend;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EmailSubjectRepository : Repository, IEmailSubjectRepository
    {
        public EmailSubjectRepository(API_DBContext context) : base(context)
        {

        }
        /// <summary>
        /// get field name list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PreDefinedFieldColumn>> GetPreDefinedFieldColumnList()
        {
            return await _context.EsSuPreDefinedFields.Where(x => x.Active.HasValue && x.Active.Value).
                Select(x => new PreDefinedFieldColumn
                {
                    Id = x.Id,
                    Name = x.FieldAliasName,
                    MaxChar = x.MaxChar,
                    DataType=x.DataType,
                    IsText = x.IsText
                }).AsNoTracking().OrderBy(x=>x.Name).ToListAsync();
        }

        /// <summary>
        /// edit the email subject config details 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<EsSuTemplateMaster> GetEmailSubConfigList(int emailConfigId)
        {
            return await _context.EsSuTemplateMasters.Where(x => x.Active.Value && x.Id == emailConfigId)
                .Include(x => x.EsSuTemplateDetails)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// view the data email subject config
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<SubConfigEdit> GetEmailSubConfig(int emailConfigId)
        {
            return await _context.EsSuTemplateMasters.Where(x => x.Active.Value && x.Id == emailConfigId).
                Select(x => new SubConfigEdit
                {
                    CustomerId = x.CustomerId,
                    Id = x.Id,
                    TemplateDisplayName = x.TemplateDisplayName,
                    TemplateName = x.TemplateName,
                    EmailTypeId = x.EmailTypeId,
                    DelimiterId = x.DelimiterId,
                    Delimiter=x.Delimiter.Name,
                    ModuleId = x.ModuleId,
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the email subject template details
        /// </summary>
        /// <param name="emailConfigId"></param>
        /// <returns></returns>
        public async Task<List<SubConfigColumnEdit>> GetEmailSubTemplateDetails(int emailConfigId)
        {
            return await _context.EsSuTemplateDetails.Where(x => x.TemplateId == emailConfigId).
                        Select(y => new SubConfigColumnEdit() 
                            {
                                Id = y.FieldId,
                                Name = y.Field.FieldAliasName,
                                MaxChar = y.MaxChar,
                                IsTitle = y.IsTitle,
                                TitleCustomName = y.TitleCustomName,
                                MaxItems = y.MaxItems,
                                DateFormat = y.DateFormat,
                                IsDateSeperator = y.IsDateSeperator,
                                IsText = y.Field.IsText,
                                DataType = y.Field.DataType
                        }).ToListAsync();
        }

        /// <summary>
        /// exists template name 
        /// </summary>
        /// <param name="emailConfigId"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public async Task<bool> ExistsTemplateName(EmailSubjectConfig request)
        {
            return await _context.EsSuTemplateMasters.Where(x => x.Active.Value && x.Id != request.Id && x.TemplateName == request.TemplateName
            && x.EmailTypeId == request.EmailTypeId && x.ModuleId == request.ModuleId && x.CustomerId == request.CustomerId).AnyAsync();
        }

        /// <summary>
        /// exists template details 
        /// </summary>
        /// <param name="emailConfigId"></param>
        /// <returns></returns>
        public async Task<List<TemplateDetailsRepo>> ExistsTemplateDetails(EmailSubjectConfig request)
        {
            return await _context.EsSuTemplateDetails.Where(x => x.TemplateId != request.Id && x.Template.CustomerId == request.CustomerId
            && x.Template.EmailTypeId == request.EmailTypeId && x.Template.ModuleId == request.ModuleId)
                .Select(x => new TemplateDetailsRepo
                {
                    TemplateId = x.TemplateId,
                    FieldId = x.FieldId
                })
                .ToListAsync();
        }

        /// <summary>
        /// get template master details
        /// </summary>
        /// <returns></returns>
        public IQueryable<EmailSubConfigRepo> GetEmailSubjectTemplate()
        {
            return _context.EsSuTemplateMasters.Where(x => x.Active.Value)
                .Select(x => new EmailSubConfigRepo
                {
                    CustomerName = x.Customer.CustomerName,
                    SubConfigId = x.Id,
                    TemplateDisplayName = x.TemplateDisplayName,
                    CustomerId = x.CustomerId,
                    ModuleId = x.ModuleId,
                    EmailTypeId = x.EmailTypeId,
                    TemplateName = x.TemplateName,
                    EmailType=x.EmailType.Name,
                    ModuleName=x.Module.Name
                });
        }

        /// <summary>
        /// get template details by subconfig ids
        /// </summary>
        /// <param name="subConfigIds"></param>
        /// <returns></returns>
        public async Task<List<TemplateDetailRepo>> GetEmailSubjectTemplateDetails(IEnumerable<int> subConfigIds)
        {
            return await _context.EsSuTemplateDetails.Select(x => new TemplateDetailRepo
            {
                FieldName = x.Field.FieldAliasName,
                TemplateId = x.TemplateId
            }).ToListAsync();
        }

        /// <summary>
        /// get module list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetModuleList()
        {
            return await _context.EsSuModules.Where(x => x.Active.Value).
                Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        /// <summary>
        /// get email type list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetEmailTypeList()
        {
            return await _context.EsTypes.Where(x => x.Active).
                Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        /// <summary>
        /// email subject exists or not
        /// </summary>
        /// <param name="emailSubjectId"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailSubjectConfigWithEmailRule(int emailSubjectId)
        {
            return await _context.EsDetails.Where(x => x.Active.Value && x.EmailSubject == emailSubjectId).AnyAsync();
        }

        /// <summary>
        /// email subject and file name details
        /// </summary>
        /// <param name="emailConfigId"></param>
        /// <returns></returns>
        public async Task<bool> GetSubjectFileNameDetails(int id)
        {
            return await _context.EsDetails.AnyAsync(x => x.EmailSubject == id || x.FileNameId == id);
        }

        /// <summary>
        /// get email subject id list which in mapped in email rule
        /// </summary>
        /// <param name="subjectIdList"></param>
        /// <returns></returns>
        public async Task<List<EmailTemplate>> GetEmailTemplateIdList(List<int> templateIdList)
        {
            return await _context.EsDetails.Where(x => x.Active.Value && (templateIdList.Contains(x.EmailSubject.GetValueOrDefault()) ||
            templateIdList.Contains(x.FileNameId.GetValueOrDefault())))
                .Select(x => new EmailTemplate()
                {
                    SubjectId = x.EmailSubject.GetValueOrDefault(),
                    FileNameId = x.FileNameId.GetValueOrDefault()
                }).ToListAsync();
        }
    }
}
