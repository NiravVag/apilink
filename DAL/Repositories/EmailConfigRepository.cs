using Contracts.Repositories;
using DTO.CommonClass;
using DTO.EmailConfiguration;
using DTO.EmailSend;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EmailConfigRepository : Repository, IEmailConfigurationRepository
    {
        public EmailConfigRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// get special rule list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetSpecialRuleList()
        {
            return await _context.EsRefSpecialRules.Where(x => x.Active.Value)
                      .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        public async Task<IEnumerable<CommonDataSource>> GetEsRefRecipientList()
        {
            return await _context.EsRefRecipients.Where(x => x.Active.Value).Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get report in email list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetReportInEmailList()
        {
            return await _context.EsRefReportInEmails.Where(x => x.Active.Value)
                      .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        /// <summary>
        /// get email size list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetEmailSizeList()
        {
            return await _context.EsRefEmailSizes.Where(x => x.Active.Value)
                      .Select(x => new CommonDataSource
                      { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailDetailId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EsAdditionalRecipient>> GetAdditionalEmailRecipientsByEmailDetailId(int emailDetailId)
        {
            return await _context.EsAdditionalRecipients.Where(x => x.EmailDetailId == emailDetailId).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailDetailId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AdditionalEmailRecipient>> GetAdditionalEmailRecipientsByEmailDetailId(List<int> emailDetailIds)
        {
            return await _context.EsAdditionalRecipients.Where(x => emailDetailIds.Contains(x.EmailDetailId.GetValueOrDefault()))
                .Select(y => new AdditionalEmailRecipient()
                {
                    Id = y.Id,
                    Email = y.AdditionalEmail,
                    RecipientId = y.Recipient,
                    RecipientType = y.RecipientNavigation.Name
                }).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailDetailId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EsAdditionalRecipient>> GetAdditionalEmailRecipientByEmailDetailId(int emailDetailId)
        {
            return await _context.EsAdditionalRecipients.Where(x => x.EmailDetailId == emailDetailId).ToListAsync();
        }
        /// <summary>
        /// get report send type list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetReportSendTypeListByEmailType(int emailTypeId)
        {
            return await _context.EsEmailReportTypeMaps.Where(x => x.EmailType == emailTypeId && x.Active)
                    .Select(x => new CommonDataSource
                    { Id = x.ReportTypeNavigation.Id, Name = x.ReportTypeNavigation.Name }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get template master list
        /// </summary>
        /// <returns></returns>
        public IQueryable<EmailSubjectRepo> GetTemplateMasterList()
        {
            return _context.EsSuTemplateMasters.Where(x => x.Active.Value && x.ModuleId == (int)EmailSendSubjectModule.Subject)
                      .Select(x => new EmailSubjectRepo
                      { Id = x.Id, Name = x.TemplateDisplayName, CustomerId = x.CustomerId, EmailTypeId = x.EmailTypeId });
        }

        /// <summary>
        /// get email send type list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetEmailSendTypeList()
        {
            return await _context.EsTypes
                      .Select(x => new CommonDataSource
                      { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        /// <summary>
        /// get email details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EsDetail> GetEmailSendDetails(int id)
        {
            return await _context.EsDetails.Where(x => x.Active.Value && x.Id == id)
                .Include(x => x.EsApiContacts)
                .Include(x => x.EsCuConfigs)
                .Include(x => x.EsCuContacts)
                .Include(x => x.EsFaCountryConfigs)
                .Include(x => x.EsOfficeConfigs)
                .Include(x => x.EsProductCategoryConfigs)
                .Include(x => x.EsResultConfigs)
                .Include(x => x.EsServiceTypeConfigs)
                .Include(x => x.EsSpecialRules)
                .Include(x => x.EsSupFactConfigs)
                .Include(x => x.EsRecipientTypes)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// get email details to edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditEmailConfiguration> GetEditEmailSendDetail(int id)
        {
            return await _context.EsDetails.Where(x => x.Active.Value && x.Id == id)
                .Select(x => new EditEmailConfiguration
                {
                    CustomerId = x.CustomerId,
                    EmailSizeId = x.EmailSize,
                    Id = x.Id,
                    NumberOfReports = x.NoOfReports,
                    RecipientName = x.RecipientName,
                    ReportInEmailId = x.ReportInEmail,
                    ReportSendTypeId = x.ReportSendType,
                    ReportSubjectId = x.EmailSubject,
                    ServiceId = x.ServiceId,
                    TypeId = x.TypeId,
                    EmailSubjectId = x.EmailSubject,
                    FileNameId = x.FileNameId,
                    isPictureFileInEmail = x.IsPictureFileInEmail,
                    InvoiceTypeId = x.InvoiceTypeId
                }).FirstOrDefaultAsync();
        }


        /// <summary>
        /// get email details as iqueryable for search
        /// </summary>
        /// <returns></returns>
        public IQueryable<EmailConfigSearchRepo> GetEmailConfigDetails()
        {
            return _context.EsDetails.Where(x => x.Active.Value)
                .Select(x => new EmailConfigSearchRepo
                {
                    CustomerName = x.Customer.CustomerName,
                    EmailConfigId = x.Id,
                    CustomerId = x.CustomerId,
                    ServiceName = x.Service.Name,
                    ServiceId = x.ServiceId,
                    ReportInEmail = x.ReportInEmailNavigation.Name,
                    ReportSendType = x.ReportSendTypeNavigation.Name,
                    EmailTypeName = x.Type.Name,
                    EmailTypeId = x.TypeId,
                    CreatedBy = x.CreatedByNavigation.FullName,
                    CreatedOn = x.CreatedOn,
                    UpdatedBy = x.UpdatedByNavigation.FullName,
                    UpdatedOn = x.UpdatedOn
                    //ServiceTypeId = x.EsServiceTypeConfigs.Select(y => y.ServiceTypeId),
                    //ServiceTypeName = x.EsServiceTypeConfigs.Select(y => y.ServiceType.Name)
                }).OrderBy(x => x.CustomerName);
        }

        /// <summary>
        /// get email config factory country details
        /// </summary>
        /// <param name="ESDetailsIdList"></param>
        /// <returns></returns>
        public async Task<List<EmailConfigFactoryCountryRepo>> GetEmailConfigFactoryCountryDetails(List<int> ESDetailsIdList)
        {
            return await _context.EsFaCountryConfigs.Where(x => ESDetailsIdList.Contains(x.EsDetailsId))
                .Select(x => new EmailConfigFactoryCountryRepo
                {
                    EmailConfigId = x.EsDetailsId,
                    FactoryCountryId = x.FactoryCountry.Id,
                    FactoryCountryName = x.FactoryCountry.CountryName
                }).ToListAsync();
        }
        /// <summary>
        /// get email config service type details
        /// </summary>
        /// <param name="ESDetailsIdList"></param>
        /// <returns></returns>
        public async Task<List<EmailConfigServiceTypeRepo>> GetEmailConfigServiceTypeDetails(List<int> ESDetailsIdList)
        {
            return await _context.EsServiceTypeConfigs.Where(x => ESDetailsIdList.Contains(x.EsDetailsId))
                .Select(x => new EmailConfigServiceTypeRepo
                {
                    EmailConfigId = x.EsDetailsId,
                    ServiceTypeId = x.ServiceType.Id,
                    ServiceTypeName = x.ServiceType.Name
                }).ToListAsync();
        }
        /// <summary>
        /// get email config office details
        /// </summary>
        /// <param name="ESDetailsIdList"></param>
        /// <returns></returns>
        public async Task<List<EmailConfigOfficeRepo>> GetEmailConfigOfficeDetails(List<int> ESDetailsIdList)
        {
            return await _context.EsOfficeConfigs.Where(x => ESDetailsIdList.Contains(x.EsDetailsId))
                .Select(x => new EmailConfigOfficeRepo
                {
                    EmailConfigId = x.EsDetailsId,
                    OfficeId = x.OfficeId,
                    OfficeName = x.Office.LocationName
                }).ToListAsync();
        }

        /// <summary>
        /// get email config department details
        /// </summary>
        /// <param name="ESDetailsIdList"></param>
        /// <returns></returns>
        public async Task<List<EmailConfigDeptRepo>> GetEmailConfigDeptDetails(List<int> ESDetailsIdList)
        {
            return await _context.EsCuConfigs.Where(x => ESDetailsIdList.Contains(x.EsDetailsId) && x.DepartmentId > 0)
                .Select(x => new EmailConfigDeptRepo
                {
                    EmailConfigId = x.EsDetailsId,
                    DepartmentId = x.Department.Id,
                    DepartmentName = x.Department.Name
                }).ToListAsync();
        }

        /// <summary>
        /// email config brand list details
        /// </summary>
        /// <param name="ESDetailsIdList"></param>
        /// <returns></returns>
        public async Task<List<EmailConfigBrandRepo>> GetEmailConfigBrandDetails(List<int> ESDetailsIdList)
        {
            return await _context.EsCuConfigs.Where(x => ESDetailsIdList.Contains(x.EsDetailsId) && x.BrandId > 0)
                .Select(x => new EmailConfigBrandRepo
                {
                    EmailConfigId = x.EsDetailsId,
                    BrandId = x.Brand.Id,
                    BrandName = x.Brand.Name
                }).ToListAsync();
        }

        /// <summary>
        /// get email config product category details
        /// </summary>
        /// <param name="ESDetailsIdList"></param>
        /// <returns></returns>
        public async Task<List<EmailConfigProductCategoryRepo>> GetEmailConfigProductCategoryDetails(List<int> ESDetailsIdList)
        {
            return await _context.EsProductCategoryConfigs.Where(x => ESDetailsIdList.Contains(x.EsDetailsId))
                .Select(x => new EmailConfigProductCategoryRepo
                {
                    EmailConfigId = x.EsDetailsId,
                    ProductCategoryId = x.ProductCategory.Id,
                    ProductCategoryName = x.ProductCategory.Name
                }).ToListAsync();
        }
        /// <summary>
        /// get api result details
        /// </summary>
        /// <param name="ESDetailsIdList"></param>
        /// <returns></returns>
        public async Task<List<EmailConfigResultRepo>> GetEmailConfigAPIResultDetails(List<int> ESDetailsIdList)
        {
            return await _context.EsResultConfigs.Where(x => ESDetailsIdList.Contains(x.EsDetailsId) && x.ApiResultId > 0)
                .Select(x => new EmailConfigResultRepo
                {
                    EmailConfigId = x.EsDetailsId,
                    ResultId = x.ApiResult.Id,
                    ResultName = x.ApiResult.ResultName
                }).ToListAsync();
        }

        /// <summary>
        /// get data for exists check
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EsDetail>> IsExists(EmailConfiguration request)
        {
            return await _context.EsDetails
                .Include(x => x.EsApiContacts)
                .Include(x => x.EsCuConfigs)
                .Include(x => x.EsCuContacts)
                .Include(x => x.EsFaCountryConfigs)
                .Include(x => x.EsOfficeConfigs)
                .Include(x => x.EsProductCategoryConfigs)
                .Include(x => x.EsResultConfigs)
                .Include(x => x.EsServiceTypeConfigs)
                .Include(x => x.EsSpecialRules)
                .Include(x => x.EsSupFactConfigs)
                .Where(x => x.Active.Value && x.Id != request.Id &&
                      x.CustomerId == request.CustomerId && x.ServiceId == request.ServiceId && x.TypeId == request.TypeId && x.InvoiceTypeId == request.InvoiceTypeId
                      && x.ReportSendType == request.ReportSendTypeId).ToListAsync();
        }

        /// <summary>
        /// get file name list
        /// </summary>
        /// <returns></returns>
        public IQueryable<EmailSubjectRepo> GetFileNameList()
        {
            return _context.EsSuTemplateMasters.Where(x => x.Active.Value && x.ModuleId == (int)EmailSendSubjectModule.FileName)
                      .Select(x => new EmailSubjectRepo
                      { Id = x.Id, CustomerId = x.CustomerId, Name = x.TemplateDisplayName, EmailTypeId = x.EmailTypeId });
        }

        /// <summary>
        /// get recipient type list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetRecipientList(int emailTypeId)
        {
            return await _context.EsRuleRecipientEmailTypeMaps.Where(x => x.EmailTypeId == emailTypeId && x.RecipientType.Active.Value && x.Active.Value)
                      .Select(x => new CommonDataSource
                      { Id = x.RecipientType.Id, Name = x.RecipientType.Name }).OrderBy(y => y.Id).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// is any email rule exist by customer id and type id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="emailTypeId"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailRuleExistByCustomerIdAndTypeId(int customerId, int emailTypeId)
        {
            return await _context.EsDetails.AsNoTracking().AnyAsync(x => x.CustomerId == customerId && x.Active == true && x.TypeId == emailTypeId);
        }
    }
}
