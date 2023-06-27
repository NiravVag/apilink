using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.EmailConfiguration;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class EmailConfigManager : IEmailConfigurationManager
    {
        readonly IEmailConfigurationRepository _emailConfigRepo = null;
        readonly IHumanResourceManager _humanResourceManager = null;
        readonly ICustomerContactManager _customerContactManager = null;
        readonly IAPIUserContext _ApplicationContext = null;
        private ITenantProvider _filterService = null;
        private readonly IEmailSendRepository _emailSendRepo = null;

        private readonly EmailConfigurationMap _emailconfigmap = null;
        public EmailConfigManager(IEmailConfigurationRepository emailConfigRepo, IHumanResourceManager humanResourceManager,
            ICustomerContactManager customerContactManager, IAPIUserContext applicationContext, ITenantProvider filterService, IEmailSendRepository emailSendRepo)
        {
            _emailConfigRepo = emailConfigRepo;
            _emailSendRepo = emailSendRepo;
            _humanResourceManager = humanResourceManager;
            _customerContactManager = customerContactManager;
            _ApplicationContext = applicationContext;
            _emailconfigmap = new EmailConfigurationMap();
            _filterService = filterService;
        }

        /// <summary>
        /// get special rule list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetSpecialRuleList()
        {
            var response = new DataSourceResponse();
            try
            {
                response.DataSourceList = await _emailConfigRepo.GetSpecialRuleList();

                if (!response.DataSourceList.Any())
                    return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

                response.Result = DataSourceResult.Success;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        /// <summary>
        /// get report in email list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetReportInEmailList()
        {
            var response = new DataSourceResponse();

            response.DataSourceList = await _emailConfigRepo.GetReportInEmailList();

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            response.Result = DataSourceResult.Success;

            return response;
        }

        /// <summary>
        /// get email size list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEmailSizeList()
        {
            var response = new DataSourceResponse();

            response.DataSourceList = await _emailConfigRepo.GetEmailSizeList();

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            response.Result = DataSourceResult.Success;

            return response;
        }

        /// <summary>
        /// get report send type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetReportSendTypeListByEmailType(int emailTypeId)
        {
            var response = new DataSourceResponse();

            response.DataSourceList = await _emailConfigRepo.GetReportSendTypeListByEmailType(emailTypeId);

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            response.Result = DataSourceResult.Success;

            return response;
        }

        /// <summary>
        /// get template display name
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetTemplateMasterList(EmailSubRequest request)
        {
            var response = new DataSourceResponse();

            var subjectDataList = _emailConfigRepo.GetTemplateMasterList();

            if (request.CustomerId != null && request.CustomerId > 0)
            {
                subjectDataList = subjectDataList.Where(x => x.CustomerId == null || x.CustomerId == request.CustomerId);
            }
            else
            {
                subjectDataList = subjectDataList.Where(x => x.CustomerId == null || x.CustomerId > 0);
            }
            if (request.EmailTypeId != null && request.EmailTypeId > 0)
            {
                subjectDataList = subjectDataList.Where(x => x.EmailTypeId == request.EmailTypeId);
            }

            var result = await subjectDataList.ToListAsync();

            response.DataSourceList = result.Select(x => _emailconfigmap.EmailSubjectDataMap(x));

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            response.Result = DataSourceResult.Success;

            return response;
        }
        /// <summary>
        /// get staff name list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetStaffNameList()
        {
            var response = new DataSourceResponse();

            response.DataSourceList = await _humanResourceManager.GetStaffNameList();

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            response.Result = DataSourceResult.Success;

            return response;
        }

        /// <summary>
        /// get customer contact name list
        /// </summary>
        /// <returns></returns>
        //public async Task<DataSourceResponse> GetCustomerContactNameList()
        //{
        //    var response = new DataSourceResponse();

        //    response.DataSourceList = await _customerContactManager.GetCustomerContactNameList();

        //    if (!response.DataSourceList.Any())
        //        return new DataSourceResponse() { Result = DataSourceResult.Success };

        //    response.Result = DataSourceResult.Success;

        //    return response;
        //}

        /// <summary>
        /// get email send type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEmailSendTypeList()
        {
            var response = new DataSourceResponse();

            response.DataSourceList = await _emailConfigRepo.GetEmailSendTypeList();

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.Success };

            response.Result = DataSourceResult.Success;

            return response;
        }

        /// <summary>
        /// add the email config
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<EmailConfigSaveReponse> AddEmailConfig(EmailConfiguration model)
        {
            var esDetailEntity = new EsDetail()
            {
                ServiceId = model.ServiceId,
                Active = true,
                CreatedOn = DateTime.Now,
                CreatedBy = _ApplicationContext.UserId,
                CustomerId = model.CustomerId,
                EmailSize = model.EmailSizeId,
                NoOfReports = model.NumberOfReports,
                RecipientName = model.RecipientName,
                ReportSendType = model.ReportSendTypeId,
                EmailSubject = model.EmailSubjectId,
                ReportInEmail = model.ReportInEmailId,
                TypeId = model.TypeId,
                FileNameId = model.FileNameId,
                EntityId = _filterService.GetCompanyId(),
                IsPictureFileInEmail = model.IsPictureFileInEmail,
                InvoiceTypeId = model.InvoiceTypeId
            };

            AddChildTable(model, esDetailEntity);

            _emailConfigRepo.AddEntity(esDetailEntity);

            await _emailConfigRepo.Save();

            return new EmailConfigSaveReponse() { Id = esDetailEntity.Id, Result = EmailConfigResult.Success };
        }

        /// <summary>
        /// update the email config
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<EmailConfigSaveReponse> EditEmailConfig(EmailConfiguration model)
        {
            var emailSendData = await _emailConfigRepo.GetEmailSendDetails(model.Id);

            emailSendData.CustomerId = model.CustomerId;
            emailSendData.ServiceId = model.ServiceId;
            emailSendData.EmailSubject = model.EmailSubjectId;
            emailSendData.ReportSendType = model.ReportSendTypeId;
            emailSendData.ReportInEmail = model.ReportInEmailId;
            emailSendData.RecipientName = model.RecipientName;
            emailSendData.NoOfReports = model.NumberOfReports;
            emailSendData.Active = true;
            emailSendData.FileNameId = model.FileNameId;
            emailSendData.EmailSize = model.EmailSizeId;
            emailSendData.TypeId = model.TypeId;
            emailSendData.IsPictureFileInEmail = model.IsPictureFileInEmail;
            emailSendData.UpdatedOn = DateTime.Now;
            emailSendData.UpdatedBy = _ApplicationContext.UserId;
            emailSendData.InvoiceTypeId = model.InvoiceTypeId;
            _emailConfigRepo.RemoveEntities(emailSendData.EsSpecialRules);
            _emailConfigRepo.RemoveEntities(emailSendData.EsFaCountryConfigs);
            _emailConfigRepo.RemoveEntities(emailSendData.EsSupFactConfigs);
            _emailConfigRepo.RemoveEntities(emailSendData.EsResultConfigs);
            _emailConfigRepo.RemoveEntities(emailSendData.EsProductCategoryConfigs);
            _emailConfigRepo.RemoveEntities(emailSendData.EsServiceTypeConfigs);
            _emailConfigRepo.RemoveEntities(emailSendData.EsOfficeConfigs);
            _emailConfigRepo.RemoveEntities(emailSendData.EsCuContacts);
            _emailConfigRepo.RemoveEntities(emailSendData.EsApiContacts);
            _emailConfigRepo.RemoveEntities(emailSendData.EsCuConfigs);
            _emailConfigRepo.RemoveEntities(emailSendData.EsRecipientTypes);

            //additional email recipient updates
            var dbAdditionalEmailRecipient = await _emailConfigRepo.GetAdditionalEmailRecipientsByEmailDetailId(emailSendData.Id);
            if (dbAdditionalEmailRecipient.Any())
            {
                _emailConfigRepo.RemoveEntities(dbAdditionalEmailRecipient);
            }

            AddChildTable(model, emailSendData);

            _emailConfigRepo.EditEntity(emailSendData);

            await _emailConfigRepo.Save();

            return new EmailConfigSaveReponse() { Id = emailSendData.Id, Result = EmailConfigResult.Success };
        }

        /// <summary>
        /// save and update details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<EmailConfigSaveReponse> Save(EmailConfiguration model)
        {
            if (model == null)
                return new EmailConfigSaveReponse() { Result = EmailConfigResult.RequestNotCorrectFormat };

            //same request if exists in DB
            if (await IsEmailConfigurationExists(model))
            {
                return new EmailConfigSaveReponse() { Result = EmailConfigResult.Exists };
            }

            //update
            if (model.Id > 0)
            {
                return await EditEmailConfig(model);
            }
            else
            {
                //save
                return await AddEmailConfig(model);
            }
        }

        /// <summary>
        /// add the details to email send child table
        /// </summary>
        /// <param name="model"></param>
        /// <param name="esDetailEntity"></param>
        private void AddChildTable(EmailConfiguration model, EsDetail esDetailEntity)
        {
            //API contact id list added to table 
            if (model.APIContactIdList != null && model.APIContactIdList.Any())
            {
                foreach (var apiContact in model.APIContactIdList)
                {
                    var itemAPIContact = new EsApiContact
                    {
                        ApiContactId = apiContact
                    };

                    esDetailEntity.EsApiContacts.Add(itemAPIContact);
                    _emailConfigRepo.AddEntity(itemAPIContact);
                }
            }

            if (model.AdditionalEmailRecipients != null && model.AdditionalEmailRecipients.Any())
            {
                foreach (var additionalEmailRecipient in model.AdditionalEmailRecipients)
                {
                    var eSAdditionalRecipient = new EsAdditionalRecipient
                    {
                        AdditionalEmail = additionalEmailRecipient.Email,
                        Recipient = additionalEmailRecipient.RecipientId,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };
                    esDetailEntity.EsAdditionalRecipients.Add(eSAdditionalRecipient);
                    _emailConfigRepo.AddEntity(eSAdditionalRecipient);
                }
            }

            if (model.CustomerContactIdList != null && model.CustomerContactIdList.Any())
            {
                foreach (var customerContactId in model.CustomerContactIdList)
                {

                    var itemCustomerContact = new EsCuContact
                    {
                        CustomerContactId = customerContactId
                    };

                    esDetailEntity.EsCuContacts.Add(itemCustomerContact);
                    _emailConfigRepo.AddEntity(itemCustomerContact);
                }
            }

            if (model.OfficeIdList != null && model.OfficeIdList.Any())
            {
                foreach (var officeId in model.OfficeIdList)
                {
                    var itemofficeConfig = new EsOfficeConfig
                    {
                        OfficeId = officeId
                    };

                    esDetailEntity.EsOfficeConfigs.Add(itemofficeConfig);
                    _emailConfigRepo.AddEntity(itemofficeConfig);
                }
            }
            if (model.ServiceTypeIdList != null && model.ServiceTypeIdList.Any())
            {
                foreach (var serviceTypeId in model.ServiceTypeIdList)
                {
                    var itemServiceType = new EsServiceTypeConfig
                    {
                        ServiceTypeId = serviceTypeId
                    };

                    esDetailEntity.EsServiceTypeConfigs.Add(itemServiceType);
                    _emailConfigRepo.AddEntity(itemServiceType);
                }
            }

            if (model.ProductCategoryIdList != null && model.ProductCategoryIdList.Any())
            {
                foreach (var productCategoryId in model.ProductCategoryIdList)
                {

                    var itemProductCategory = new EsProductCategoryConfig
                    {
                        ProductCategoryId = productCategoryId
                    };

                    esDetailEntity.EsProductCategoryConfigs.Add(itemProductCategory);
                    _emailConfigRepo.AddEntity(itemProductCategory);
                }

            }

            if (model.ApiResultIdList != null && model.ApiResultIdList.Any())
            {
                foreach (var APIResultId in model.ApiResultIdList)
                {
                    var itemResult = new EsResultConfig
                    {
                        ApiResultId = APIResultId
                    };

                    esDetailEntity.EsResultConfigs.Add(itemResult);
                    _emailConfigRepo.AddEntity(itemResult);
                }

            }

            if (model.CusDecisionIdList != null && model.CusDecisionIdList.Any())
            {
                foreach (var cusDecisionId in model.CusDecisionIdList)
                {
                    var itemResult = new EsResultConfig
                    {
                        CustomerResultId = cusDecisionId
                    };

                    esDetailEntity.EsResultConfigs.Add(itemResult);
                    _emailConfigRepo.AddEntity(itemResult);
                }

            }

            if (model.SupplierIdList != null && model.SupplierIdList.Any())
            {
                foreach (var supplierId in model.SupplierIdList)
                {
                    var itemSupConfig = new EsSupFactConfig
                    {
                        SupplierOrFactoryId = supplierId
                    };

                    esDetailEntity.EsSupFactConfigs.Add(itemSupConfig);
                    _emailConfigRepo.AddEntity(itemSupConfig);
                }

            }
            if (model.FactoryIdList != null && model.FactoryIdList.Any())
            {
                foreach (var factoryId in model.FactoryIdList)
                {
                    var itemFactConfig = new EsSupFactConfig
                    {
                        SupplierOrFactoryId = factoryId
                    };

                    esDetailEntity.EsSupFactConfigs.Add(itemFactConfig);
                    _emailConfigRepo.AddEntity(itemFactConfig);
                }
            }

            if (model.FactoryCountryIdList != null && model.FactoryCountryIdList.Any())
            {
                foreach (var factCountryId in model.FactoryCountryIdList)
                {
                    var itemFactCountryConfig = new EsFaCountryConfig
                    {
                        FactoryCountryId = factCountryId
                    };

                    esDetailEntity.EsFaCountryConfigs.Add(itemFactCountryConfig);
                    _emailConfigRepo.AddEntity(itemFactCountryConfig);
                }
            }
            if (model.SpecialRuleIdList != null && model.SpecialRuleIdList.Any())
            {
                foreach (var specialRuleId in model.SpecialRuleIdList)
                {
                    var itemSpecialRule = new EsSpecialRule
                    {
                        SpecialRuleId = specialRuleId,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    esDetailEntity.EsSpecialRules.Add(itemSpecialRule);
                    _emailConfigRepo.AddEntity(itemSpecialRule);
                }
            }

            if (model.DepartmentIdList != null && model.DepartmentIdList.Any())
            {
                foreach (var deptId in model.DepartmentIdList)
                {
                    var itemDepartment = new EsCuConfig
                    {
                        DepartmentId = deptId
                    };

                    esDetailEntity.EsCuConfigs.Add(itemDepartment);
                    _emailConfigRepo.AddEntity(itemDepartment);
                }
            }

            if (model.BuyerIdList != null && model.BuyerIdList.Any())
            {
                foreach (var buyerId in model.BuyerIdList)
                {
                    var itemBuyer = new EsCuConfig
                    {
                        BuyerId = buyerId
                    };

                    esDetailEntity.EsCuConfigs.Add(itemBuyer);
                    _emailConfigRepo.AddEntity(itemBuyer);
                }
            }

            if (model.CollectionIdList != null && model.CollectionIdList.Any())
            {
                foreach (var collectionId in model.CollectionIdList)
                {
                    var itemCollection = new EsCuConfig
                    {
                        CollectionId = collectionId
                    };

                    esDetailEntity.EsCuConfigs.Add(itemCollection);
                    _emailConfigRepo.AddEntity(itemCollection);
                }

            }
            if (model.BrandIdList != null && model.BrandIdList.Any())
            {
                foreach (var brandId in model.BrandIdList)
                {
                    var itemBrand = new EsCuConfig
                    {
                        BrandId = brandId
                    };

                    esDetailEntity.EsCuConfigs.Add(itemBrand);
                    _emailConfigRepo.AddEntity(itemBrand);
                }
            }

            //to id list added to recipient type table
            if (model.ToIdList != null && model.ToIdList.Any())
            {
                foreach (var toId in model.ToIdList)
                {
                    var itemToData = new EsRecipientType
                    {
                        RecipientTypeId = toId,
                        IsTo = true,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId
                    };

                    esDetailEntity.EsRecipientTypes.Add(itemToData);
                    _emailConfigRepo.AddEntity(itemToData);
                }
            }

            //CC id list added to recipient type table
            if (model.CCIdList != null && model.CCIdList.Any())
            {
                foreach (var ccId in model.CCIdList)
                {
                    var itemCCData = new EsRecipientType
                    {
                        RecipientTypeId = ccId,
                        IsCc = true,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId
                    };

                    esDetailEntity.EsRecipientTypes.Add(itemCCData);
                    _emailConfigRepo.AddEntity(itemCCData);
                }
            }
        }

        /// <summary>
        /// get the email config details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EmailEditResponse> EditDetails(int id)
        {
            //edit email rule config changes
            List<int> esDetailsId = new List<int> { id };
            var emailSendDetails = await _emailConfigRepo.GetEditEmailSendDetail(id);

            if (emailSendDetails != null)
            {
                //get the email config customer config data(brand,department,buyer,collection)
                var esCustomerConfigData = await _emailSendRepo.GetESCustomerConfigDetails(esDetailsId);

                //get the email config  customer contact data
                var esCustomerContactData = await _emailSendRepo.GetESCustomerContactDetails(esDetailsId);

                //get the email config  api contact data
                var esApiContactData = await _emailSendRepo.GetESApiContactDetails(esDetailsId);

                //get the email config  api report result data
                var esReportResultData = await _emailSendRepo.GetESReportResultDetails(esDetailsId);

                //get the email config  product category data
                var esProductCategoryData = await _emailSendRepo.GetESProductCategoryDetails(esDetailsId);

                //get the email config  special rule data
                var esReportResultDetails = await _emailSendRepo.GetESSpecialRuleDetails(esDetailsId);

                //get the email config factory country data
                var esFactoryCountryData = await _emailSendRepo.GetESFactoryCountryDetails(esDetailsId);

                //get the email config office details data
                var esOfficeDetailsData = await _emailSendRepo.GetESOfficeDetails(esDetailsId);

                //get the email config service type data
                var esServiceTypeData = await _emailSendRepo.GetESServiceTypeDetails(esDetailsId);

                //get the email config special rule data
                var esSpecialRuleData = await _emailSendRepo.GetESSpecialRuleDetails(esDetailsId);

                //get the email config supplier or factory details
                var esSupplierOrFactoryData = await _emailSendRepo.GetESSupplierOrFactoryDetails(esDetailsId);

                //get the email config recipient data
                var esRecipientData = await _emailSendRepo.GetESRecipientDetails(esDetailsId);

                var additionalEmailRecipients = await _emailConfigRepo.GetAdditionalEmailRecipientsByEmailDetailId(esDetailsId);

                //assign the customer config data
                if (esCustomerConfigData.Any())
                {
                    emailSendDetails.BrandIdList = esCustomerConfigData.Where(x => x.BrandId != null).Select(x => x.BrandId).ToList();
                    emailSendDetails.DepartmentIdList = esCustomerConfigData.Where(x => x.DepartmentId != null).Select(x => x.DepartmentId).ToList();
                    emailSendDetails.BuyerIdList = esCustomerConfigData.Where(x => x.BuyerId != null).Select(x => x.BuyerId).ToList();
                    emailSendDetails.CollectionIdList = esCustomerConfigData.Where(x => x.CollectionId != null).Select(x => x.CollectionId).ToList();                    
                }

                // assign the customer contact data
                emailSendDetails.CustomerContactIdList = esCustomerContactData.Select(x => x.CustomerContactId).ToList();


                // assign the api contact data
                emailSendDetails.APIContactIdList = esApiContactData.Select(x => x.ApiContactId).ToList();

                //assing the result data
                emailSendDetails.CusDecisionIdList = esReportResultData.Where(y => y.CustomerResultId != null).Select(y => y.CustomerResultId).ToList();
                emailSendDetails.ApiResultIdList = esReportResultData.Where(y => y.ApiResultId != null).Select(y => y.ApiResultId).ToList();

                //assing the factory country id list
                emailSendDetails.FactoryCountryIdList = esFactoryCountryData.Select(x => x.FactoryCountryId).ToList();

                //assing the office details
                emailSendDetails.OfficeIdList = esOfficeDetailsData.Select(x => x.OfficeId).ToList();

                //assing the product category data
                emailSendDetails.ProductCategoryIdList = esProductCategoryData.Select(x => x.Id).ToList();

                //assign the service type data
                emailSendDetails.ServiceTypeIdList = esServiceTypeData.Select(x => x.ServiceTypeId).ToList();

                //assign the special rule data
                emailSendDetails.SpecialRuleIdList = esSpecialRuleData.Select(x => x.Id).ToList();

                //assign the supplier or factory data
                emailSendDetails.SupplierIdList = esSupplierOrFactoryData.Where(a => a.SupplierId > 0 && a.SupplierType == (int)Supplier_Type.Supplier_Agent).Select(x => x.SupplierId).ToList();
                emailSendDetails.FactoryIdList = esSupplierOrFactoryData.Where(a => a.SupplierId > 0 && a.SupplierType == (int)Supplier_Type.Factory).Select(x => x.SupplierId).ToList();

                //assign the recipient data
                emailSendDetails.ToIdList = esRecipientData.Where(z => z.IsToValue.HasValue && z.IsToValue.Value && z.RecipientId > 0).Select(x => x.RecipientId).ToList();
                emailSendDetails.CCIdList = esRecipientData.Where(z => z.IsCCValue.HasValue && z.IsCCValue.Value && z.RecipientId > 0).Select(x => x.RecipientId).ToList();
                emailSendDetails.AdditionalEmailRecipients = additionalEmailRecipients;                 
            }

            if (emailSendDetails.Id <= 0)
                return new EmailEditResponse() { Result = EmailConfigResult.NotFound };

            return new EmailEditResponse() { EmailSendDetails = emailSendDetails, Result = EmailConfigResult.Success };
        }


        /// <summary>
        /// summary page search details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<EmailConfigurationSummaryResponse> Search(EmailConfigurationSummary request)
        {
            if (request == null)
                return new EmailConfigurationSummaryResponse() { Result = EmailConfigResult.RequestNotCorrectFormat };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 20;

            var response = new EmailConfigurationSummaryResponse { Index = request.Index.Value, PageSize = request.PageSize.Value };

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            var data = _emailConfigRepo.GetEmailConfigDetails();

            if (data == null)
                return new EmailConfigurationSummaryResponse() { Result = EmailConfigResult.NotFound };

            var filterEmailConfigList = await FilterSummaryData(data, request);

            var result = await filterEmailConfigList.EmailConfigSearchRepo.Skip(skip).Take(take).AsNoTracking().ToListAsync();

            if (result == null || !result.Any())
                return new EmailConfigurationSummaryResponse() { Result = EmailConfigResult.NotFound };

            var res = result.Select(x => _emailconfigmap.EmailConfigMap(filterEmailConfigList, x));

            return new EmailConfigurationSummaryResponse
            {
                Result = EmailConfigResult.Success,
                TotalCount = await filterEmailConfigList.EmailConfigSearchRepo.CountAsync(),
                Index = request.Index.Value,
                PageSize = request.PageSize.Value,
                PageCount = (response.TotalCount / request.PageSize.Value) + (response.TotalCount % request.PageSize.Value > 0 ? 1 : 0),
                Data = res.ToList()
            };
        }

        /// <summary>
        /// delete the email configuration
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<EmailConfigurationDeleteResponse> Delete(int Id)
        {
            var entity = await _emailConfigRepo.GetEmailSendDetails(Id);

            if (entity == null)
                return new EmailConfigurationDeleteResponse() { Result = EmailConfigResult.NotFound };

            entity.Active = false;
            entity.DeletedOn = DateTime.Now;
            entity.DeletedBy = _ApplicationContext.UserId;
            _emailConfigRepo.RemoveEntities(entity.EsApiContacts);
            _emailConfigRepo.RemoveEntities(entity.EsCuConfigs);
            _emailConfigRepo.RemoveEntities(entity.EsCuContacts);
            _emailConfigRepo.RemoveEntities(entity.EsFaCountryConfigs);
            _emailConfigRepo.RemoveEntities(entity.EsOfficeConfigs);
            _emailConfigRepo.RemoveEntities(entity.EsProductCategoryConfigs);
            _emailConfigRepo.RemoveEntities(entity.EsResultConfigs);
            _emailConfigRepo.RemoveEntities(entity.EsServiceTypeConfigs);
            _emailConfigRepo.RemoveEntities(entity.EsSpecialRules);
            _emailConfigRepo.RemoveEntities(entity.EsSupFactConfigs);
            _emailConfigRepo.RemoveEntities(entity.EsRecipientTypes);

            await _emailConfigRepo.Save();

            return new EmailConfigurationDeleteResponse() { Result = EmailConfigResult.Success };
        }

        /// <summary>
        /// check is email config same rule exists or not
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<bool> IsEmailConfigurationExists(EmailConfiguration request)
        {
            //get child table details with main data exists 
            var emailConfigList = await _emailConfigRepo.IsExists(request);

            var recordExists = false;

            if (emailConfigList.Any())
            {
                //check product category is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(y => y.EsProductCategoryConfigs).Any() && (request.ProductCategoryIdList != null && request.ProductCategoryIdList.Any()))
                {
                    emailConfigList = emailConfigList.Where(x => x.EsProductCategoryConfigs.Any(y => request.ProductCategoryIdList.Contains(y.ProductCategoryId)));
                }
                //check country is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(y => y.EsFaCountryConfigs).Any() && (request.FactoryCountryIdList != null && request.FactoryCountryIdList.Any()))
                {
                    emailConfigList = emailConfigList.Where(x => x.EsFaCountryConfigs.Any(y => request.FactoryCountryIdList.Contains(y.FactoryCountryId)));
                }
                //check supplier is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsSupFactConfigs).Any() && (request.SupplierIdList != null && request.SupplierIdList.Any()))
                {
                    emailConfigList = emailConfigList.Where(x => x.EsSupFactConfigs.Any(y => request.SupplierIdList.Contains(y.SupplierOrFactoryId)));
                }
                //check servicetype is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsServiceTypeConfigs).Any() && (request.ServiceTypeIdList != null && request.ServiceTypeIdList.Any()))
                {
                    emailConfigList = emailConfigList.Where(x => x.EsServiceTypeConfigs.Any(y => request.ServiceTypeIdList.Contains(y.ServiceTypeId)));
                }
                //check brand is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsCuConfigs).Any() && request.BrandIdList != null && request.BrandIdList.Any())
                {
                    emailConfigList = emailConfigList.Where(x => x.EsCuConfigs.Any(y => y.BrandId > 0 && request.BrandIdList.Contains(y.BrandId.Value)));
                }
                //check dept is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsCuConfigs).Any() && request.DepartmentIdList != null && request.DepartmentIdList.Any())
                {
                    emailConfigList = emailConfigList.Where(x => x.EsCuConfigs.Any(y => y.DepartmentId > 0 && request.DepartmentIdList.Contains(y.DepartmentId.Value)));
                }
                //check buyer is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsCuConfigs).Any() && request.BuyerIdList != null && request.BuyerIdList.Any())
                {
                    emailConfigList = emailConfigList.Where(x => x.EsCuConfigs.Any(y => y.BuyerId > 0 && request.BuyerIdList.Contains(y.BuyerId.Value)));
                }
                //check collection is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsCuConfigs).Any() && request.CollectionIdList != null && request.CollectionIdList.Any())
                {
                    emailConfigList = emailConfigList.Where(x => x.EsCuConfigs.Any(y => y.CollectionId > 0 && request.CollectionIdList.Contains(y.CollectionId.Value)));
                }
                ////check api contact is available or not
                //if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsApiContacts).Any() && request.APIContactIdList != null && request.APIContactIdList.Any())
                //{
                //    emailConfigList = emailConfigList.Where(x => x.EsApiContacts.Any(y => request.APIContactIdList.Contains(y.ApiContactId)));
                //}

                ////check customer contact is available or not
                //if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsCuContacts).Any() && request.CustomerContactIdList != null && request.CustomerContactIdList.Any())
                //{
                //    emailConfigList = emailConfigList.Where(x => x.EsCuContacts.Any(y => request.CustomerContactIdList.Contains(y.CustomerContactId)));
                //}

                //check office is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsOfficeConfigs).Any() && request.OfficeIdList != null && request.OfficeIdList.Any())
                {
                    emailConfigList = emailConfigList.Where(x => x.EsOfficeConfigs.Any(y => y.OfficeId > 0 && request.OfficeIdList.Contains(y.OfficeId.Value)));
                }
                //check customer decision is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsResultConfigs).Any() && request.CusDecisionIdList != null && request.CusDecisionIdList.Any())
                {
                    emailConfigList = emailConfigList.Where(x => x.EsResultConfigs.Any(y => y.CustomerResultId > 0 && request.CusDecisionIdList.Contains(y.CustomerResultId.Value)));
                }
                //check api result is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsResultConfigs).Any() && request.ApiResultIdList != null && request.ApiResultIdList.Any())
                {
                    emailConfigList = emailConfigList.Where(x => x.EsResultConfigs.Any(y => y.ApiResultId > 0 && request.ApiResultIdList.Contains(y.ApiResultId.Value)));
                }
                //check special rule is available or not
                if (emailConfigList.Any() && emailConfigList.SelectMany(z => z.EsSpecialRules).Any() && request.SpecialRuleIdList != null && request.SpecialRuleIdList.Any())
                {
                    emailConfigList = emailConfigList.Where(x => x.EsSpecialRules.Any(y => y.SpecialRuleId > 0 && request.SpecialRuleIdList.Contains(y.SpecialRuleId.Value)));
                }

                //if data exists return true
                if (emailConfigList.Any())
                {
                    recordExists = true;
                }
            }

            return recordExists;
        }

        /// <summary>
        /// search - filter data by request model 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<EmailConfigParameterMap> FilterSummaryData(IQueryable<EmailConfigSearchRepo> data, EmailConfigurationSummary request)
        {
            var parametersMap = new EmailConfigParameterMap();

            //filter by customer id
            if (request.CustomerId != null && request.CustomerId > 0)
            {
                data = data.Where(x => request.CustomerId == x.CustomerId);
            }

            //filter by service id
            if (request.ServiceId != null && request.ServiceId > 0)
            {
                data = data.Where(x => request.ServiceId == x.ServiceId);
            }

            //filter by email type id
            if (request.TypeId != null && request.TypeId > 0)
            {
                data = data.Where(x => request.TypeId == x.EmailTypeId);
            }

            //get email config id list
            var _emailConfigIds = await data.Select(x => x.EmailConfigId).ToListAsync();

            //get factory country data by email config ids
            parametersMap.EmailConfigFactoryCountryRepo = await _emailConfigRepo.GetEmailConfigFactoryCountryDetails(_emailConfigIds);

            //get service type data by email config ids
            parametersMap.EmailConfigServiceTypeRepo = await _emailConfigRepo.GetEmailConfigServiceTypeDetails(_emailConfigIds);

            //get office list by email config ids
            parametersMap.EmailConfigOfficeRepo = await _emailConfigRepo.GetEmailConfigOfficeDetails(_emailConfigIds);

            //get department list by email config ids
            parametersMap.EmailConfigDeptRepo = await _emailConfigRepo.GetEmailConfigDeptDetails(_emailConfigIds);

            //get brand list by email config ids
            parametersMap.EmailConfigBrandRepo = await _emailConfigRepo.GetEmailConfigBrandDetails(_emailConfigIds);

            //get product category data by email config ids
            parametersMap.EmailConfigProductCategoryRepo = await _emailConfigRepo.GetEmailConfigProductCategoryDetails(_emailConfigIds);

            //get api result list by email config ids 
            parametersMap.EmailConfigResultRepo = await _emailConfigRepo.GetEmailConfigAPIResultDetails(_emailConfigIds);

            // filter by factory country list
            if (request.FactoryCountryIdList != null && request.FactoryCountryIdList.Any())
            {
                var filteredEmailConfig = parametersMap.EmailConfigFactoryCountryRepo.Where(x => request.FactoryCountryIdList.Contains(x.FactoryCountryId)).Select(x => x.EmailConfigId).ToList();

                data = data.Where(x => filteredEmailConfig.Contains(x.EmailConfigId));
            }
            //else{}

            //filter by serivce type id list
            if (request.ServiceTypeIdList != null && request.ServiceTypeIdList.Any())
            {
                var filteredEmailConfig = parametersMap.EmailConfigServiceTypeRepo.Where(x => request.ServiceTypeIdList.Contains(x.ServiceTypeId)).Select(x => x.EmailConfigId).ToList();

                data = data.Where(x => filteredEmailConfig.Contains(x.EmailConfigId));
            }

            //filter by office id list
            if (request.OfficeIdList != null && request.OfficeIdList.Any())
            {
                var filteredEmailConfig = parametersMap.EmailConfigOfficeRepo.Where(x => x.OfficeId > 0 && request.OfficeIdList.Contains(x.OfficeId.Value)).Select(x => x.EmailConfigId).ToList();

                data = data.Where(x => filteredEmailConfig.Contains(x.EmailConfigId));
            }

            //filter by department id list
            if (request.DepartmentIdList != null && request.DepartmentIdList.Any())
            {
                var filteredEmailConfig = parametersMap.EmailConfigDeptRepo.Where(x => request.DepartmentIdList.Contains(x.DepartmentId)).Select(x => x.EmailConfigId).ToList();

                data = data.Where(x => filteredEmailConfig.Contains(x.EmailConfigId));
            }

            //filter by brand id list
            if (request.BrandIdList != null && request.BrandIdList.Any())
            {
                var filteredEmailConfig = parametersMap.EmailConfigBrandRepo.Where(x => request.BrandIdList.Contains(x.BrandId)).Select(x => x.EmailConfigId).ToList();

                data = data.Where(x => filteredEmailConfig.Contains(x.EmailConfigId));
            }

            //filter by product category id list
            if (request.ProductCategoryIdList != null && request.ProductCategoryIdList.Any())
            {
                var filteredEmailConfig = parametersMap.EmailConfigProductCategoryRepo.Where(x => request.ProductCategoryIdList.Contains(x.ProductCategoryId)).Select(x => x.EmailConfigId).ToList();

                data = data.Where(x => filteredEmailConfig.Contains(x.EmailConfigId));
            }

            //filter by api result id list
            if (request.ResultIdList != null && request.ResultIdList.Any())
            {
                var filteredEmailConfig = parametersMap.EmailConfigResultRepo.Where(x => request.ResultIdList.Contains(x.ResultId)).Select(x => x.EmailConfigId).ToList();

                data = data.Where(x => filteredEmailConfig.Contains(x.EmailConfigId));
            }

            //assign filter main table data
            parametersMap.EmailConfigSearchRepo = data;

            return parametersMap;
        }

        /// <summary>
        /// get file name from email subject config
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetFileNameList(EmailSubRequest request)
        {
            var response = new DataSourceResponse();

            var filelist = _emailConfigRepo.GetFileNameList();

            if (request.CustomerId != null && request.CustomerId > 0)
            {
                filelist = filelist.Where(x => x.CustomerId == null || x.CustomerId == request.CustomerId);
            }
            else
            {
                filelist = filelist.Where(x => x.CustomerId == null || x.CustomerId > 0);
            }
            if (request.EmailTypeId != null && request.EmailTypeId > 0)
            {
                filelist = filelist.Where(x => x.EmailTypeId == request.EmailTypeId);
            }

            var result = await filelist.ToListAsync();

            response.DataSourceList = result.Select(x => _emailconfigmap.EmailSubjectDataMap(x));

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            response.Result = DataSourceResult.Success;

            return response;
        }

        /// <summary>
        /// get recipient type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetRecipientTypeList(int emailTypeId)
        {
            var response = new DataSourceResponse();

            response.DataSourceList = await _emailConfigRepo.GetRecipientList(emailTypeId);

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            response.Result = DataSourceResult.Success;

            return response;
        }

        public async Task<DataSourceResponse> GetEsRefRecipientList()
        {
            var response = new DataSourceResponse();
            response.DataSourceList = await _emailConfigRepo.GetEsRefRecipientList();

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            response.Result = DataSourceResult.Success;

            return response;
        }
    }
}
