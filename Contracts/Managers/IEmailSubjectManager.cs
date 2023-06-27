using DTO.CommonClass;
using DTO.EmailSend;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IEmailSubjectManager
    {
        Task<PreDefinedColSourceResponse> GetFieldColumnList();

        Task<SubConfigSaveResponse> Save(EmailSubjectConfig request);

        Task<SubConfigEditResponse> Edit(int Id);

        Task<EmailSubConfigSummaryResponse> Search(EmailSubConfigSummary request);

        Task<DeleteResponse> Delete(int Id);

        Task<DataSourceResponse> GetEmailTypeList();

        Task<DataSourceResponse> GetModuleList();
    }
}
