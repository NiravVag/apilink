using DTO.CommonClass;
using DTO.EmailSend;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IEmailSubjectRepository : IRepository
    {
        Task<IEnumerable<PreDefinedFieldColumn>> GetPreDefinedFieldColumnList();

        Task<EsSuTemplateMaster> GetEmailSubConfigList(int emailConfigId);

        Task<SubConfigEdit> GetEmailSubConfig(int emailConfigId);

        Task<List<SubConfigColumnEdit>> GetEmailSubTemplateDetails(int emailConfigId);

        Task<bool> ExistsTemplateName(EmailSubjectConfig request);

        Task<List<TemplateDetailsRepo>> ExistsTemplateDetails(EmailSubjectConfig request);

        IQueryable<EmailSubConfigRepo> GetEmailSubjectTemplate();

        Task<List<TemplateDetailRepo>> GetEmailSubjectTemplateDetails(IEnumerable<int> subConfigIds);

        Task<IEnumerable<CommonDataSource>> GetModuleList();

        Task<IEnumerable<CommonDataSource>> GetEmailTypeList();

        Task<bool> IsEmailSubjectConfigWithEmailRule(int emailSubjectId);

        Task<bool> GetSubjectFileNameDetails(int id);

        Task<List<EmailTemplate>> GetEmailTemplateIdList(List<int> templateIdList);
    }
}
