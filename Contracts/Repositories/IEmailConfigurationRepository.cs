using DTO.CommonClass;
using DTO.EmailConfiguration;
using DTO.EmailSend;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IEmailConfigurationRepository : IRepository
    {
        Task<IEnumerable<CommonDataSource>> GetEsRefRecipientList();
        Task<IEnumerable<CommonDataSource>> GetSpecialRuleList();

        Task<IEnumerable<CommonDataSource>> GetReportInEmailList();

        Task<IEnumerable<CommonDataSource>> GetEmailSizeList();

        Task<IEnumerable<CommonDataSource>> GetReportSendTypeListByEmailType(int emailTypeId);

        IQueryable<EmailSubjectRepo> GetTemplateMasterList();

        Task<EsDetail> GetEmailSendDetails(int id);

        Task<EditEmailConfiguration> GetEditEmailSendDetail(int id);

        Task<IEnumerable<CommonDataSource>> GetEmailSendTypeList();

        IQueryable<EmailConfigSearchRepo> GetEmailConfigDetails();

        Task<List<EmailConfigFactoryCountryRepo>> GetEmailConfigFactoryCountryDetails(List<int> ESDetailsIdList);

        Task<List<EmailConfigServiceTypeRepo>> GetEmailConfigServiceTypeDetails(List<int> ESDetailsIdList);

        Task<List<EmailConfigOfficeRepo>> GetEmailConfigOfficeDetails(List<int> ESDetailsIdList);

        Task<List<EmailConfigDeptRepo>> GetEmailConfigDeptDetails(List<int> ESDetailsIdList);

        Task<List<EmailConfigProductCategoryRepo>> GetEmailConfigProductCategoryDetails(List<int> ESDetailsIdList);

        Task<List<EmailConfigBrandRepo>> GetEmailConfigBrandDetails(List<int> ESDetailsIdList);

        Task<List<EmailConfigResultRepo>> GetEmailConfigAPIResultDetails(List<int> ESDetailsIdList);

        Task<IEnumerable<EsDetail>> IsExists(EmailConfiguration request);

        IQueryable<EmailSubjectRepo> GetFileNameList();

        Task<IEnumerable<CommonDataSource>> GetRecipientList(int emailTypeId);

        Task<IEnumerable<EsAdditionalRecipient>> GetAdditionalEmailRecipientsByEmailDetailId(int emailDetailId);
        Task<IEnumerable<AdditionalEmailRecipient>> GetAdditionalEmailRecipientsByEmailDetailId(List<int> emailDetailIds);

        Task<bool> IsEmailRuleExistByCustomerIdAndTypeId(int customerId, int emailTypeId);
    }
}
