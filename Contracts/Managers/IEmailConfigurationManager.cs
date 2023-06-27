using DTO.CommonClass;
using DTO.EmailConfiguration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IEmailConfigurationManager
    {

        Task<DataSourceResponse> GetSpecialRuleList();

        Task<DataSourceResponse> GetReportInEmailList();

        Task<DataSourceResponse> GetEmailSizeList();

        Task<DataSourceResponse> GetReportSendTypeListByEmailType(int emailTypeId);

        Task<DataSourceResponse> GetTemplateMasterList(EmailSubRequest request);

        Task<DataSourceResponse> GetStaffNameList();

        Task<EmailConfigSaveReponse> Save(EmailConfiguration model);

        Task<EmailEditResponse> EditDetails(int id);

        Task<DataSourceResponse> GetEmailSendTypeList();

        Task<EmailConfigurationDeleteResponse> Delete(int Id);

        Task<EmailConfigurationSummaryResponse> Search(EmailConfigurationSummary request);

        Task<DataSourceResponse> GetFileNameList(EmailSubRequest request);

        Task<DataSourceResponse> GetRecipientTypeList(int emailTypeId);
        Task<DataSourceResponse> GetEsRefRecipientList();
    }
}
