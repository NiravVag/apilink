using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.EmailSend;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class EmailSubjectManager : ApiCommonData, IEmailSubjectManager
    {
        private readonly IEmailSubjectRepository _repo = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly EmailSubjectConfigMap _emailsubject = null;
        private ITenantProvider _filterService = null;

        public EmailSubjectManager(IEmailSubjectRepository repo, IAPIUserContext applicationContext, ITenantProvider filterService)
        {
            _repo = repo;
            _applicationContext = applicationContext;
            _emailsubject = new EmailSubjectConfigMap();
            _filterService = filterService;
        }
        /// <summary>
        /// Get field list
        /// </summary>
        /// <returns></returns>
        public async Task<PreDefinedColSourceResponse> GetFieldColumnList()
        {
            var data = await _repo.GetPreDefinedFieldColumnList();

            if (data != null)
            {
                return new PreDefinedColSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new PreDefinedColSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// save and update the email subject configuration 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SubConfigSaveResponse> Save(EmailSubjectConfig request)
        {
            if (request == null)
                return new SubConfigSaveResponse() { Result = Result.RequestNotCorrectFormat };

            int id = 0;

            //template display name will be - template name separated by request delimiter. 
            if (!string.IsNullOrEmpty(request.Delimiter))
            {
                request.TemplateDisplayName = string.Join(request.Delimiter, request.TemplateColumnList.Select(x => x.Name).ToList());
            }

            //exists
            var response = await IsExists(request);

            //exists template name and details 
            if (response.Result == Result.TemplateNameExists || response.Result == Result.TemplateFieldsExists)
            {
                return response;
            }
            else
            {
                //get the details email subject config
                var entity = await _repo.GetEmailSubConfigList(request.Id);

                if (request.Id > 0)
                {
                    //update
                    entity.TemplateName = request.TemplateName;
                    entity.UpdatedBy = _applicationContext.UserId;
                    entity.UpdatedOn = DateTime.Now;
                    entity.Id = request.Id;
                    entity.TemplateDisplayName = request.TemplateDisplayName;
                    entity.CustomerId = request.CustomerId;
                    entity.Active = true;
                    entity.Id = request.Id;
                    entity.EmailTypeId = request.EmailTypeId;
                    entity.DelimiterId = request.DelimiterId;
                    entity.ModuleId = request.ModuleId;

                    _repo.RemoveEntities(entity.EsSuTemplateDetails);

                    AddChildEmailSubject(request, entity);

                    _repo.EditEntity(entity);

                    await _repo.Save();

                    id = entity.Id;
                }
                else
                {
                    id = await AddEmailSubject(request);

                }
                return new SubConfigSaveResponse() { Id = id, Result = Result.Success };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<SubConfigSaveResponse> IsExists(EmailSubjectConfig request)
        {
            var isExistsTemplateName = await _repo.ExistsTemplateName(request);

            if (isExistsTemplateName)
            {
                return new SubConfigSaveResponse() { Result = Result.TemplateNameExists };
            }
            else
            {
                var ids = request.TemplateColumnList.Select(x => x.Id).ToList();

                var tempDetailsIdList = await _repo.ExistsTemplateDetails(request);

                var templateGroupList = tempDetailsIdList.GroupBy(
                                                p => p.TemplateId,
                                                (key, valueData) => new { tempId = key, fieldIdList = valueData.ToList() });

                foreach (var tempDetailItem in templateGroupList.OrderBy(x => x.tempId))
                {
                    //Where(x => x.TemplateId == tempDetailItem.tempId)
                    var idList = tempDetailItem.fieldIdList.Select(x => x.FieldId).ToList();

                    if (idList.Count == ids.Count)
                    {
                        var checkIdList = idList.Where(x => !ids.Contains((int)x)).ToList();

                        if (!checkIdList.Any())
                        {
                            return new SubConfigSaveResponse() { Result = Result.TemplateFieldsExists };
                        }
                    }
                }
            }
            return new SubConfigSaveResponse() { Result = Result.NoExists };
        }

        /// <summary>
        /// add email subject details
        /// </summary>
        /// <param name="request"></param>
        private async Task<int> AddEmailSubject(EmailSubjectConfig request)
        {
            //add
            EsSuTemplateMaster entity = new EsSuTemplateMaster()
            {
                TemplateName = request.TemplateName,
                TemplateDisplayName = request.TemplateDisplayName,
                CreatedOn = DateTime.Now,
                CreatedBy = _applicationContext.UserId,
                CustomerId = request.CustomerId,
                Active = true,
                EmailTypeId = request.EmailTypeId,
                DelimiterId = request.DelimiterId,
                ModuleId = request.ModuleId,
                EntityId = _filterService.GetCompanyId()
        };

            AddChildEmailSubject(request, entity);

            _repo.AddEntity(entity);

            await _repo.Save();

            return entity.Id;
        }

        /// <summary>
        /// add template detils
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddChildEmailSubject(EmailSubjectConfig request, EsSuTemplateMaster entity)
        {
            var i = 1;
            foreach (var templateItem in request.TemplateColumnList)
            {
                EsSuTemplateDetail childentity = new EsSuTemplateDetail()
                {
                    FieldId = templateItem.Id,
                    CreatedBy = _applicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    MaxChar = templateItem.MaxChar,
                    IsTitle=templateItem.IsTitle,
                    TitleCustomName=templateItem.TitleCustomName,
                    MaxItems=templateItem.MaxItems,
                    DateFormat=templateItem.DateFormat,
                    IsDateSeperator=templateItem.IsDateSeperator,
                    Sort = i++,
                };
                entity.EsSuTemplateDetails.Add(childentity);
                _repo.AddEntity(childentity);
            }
        }

        /// <summary>
        /// edit the details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<SubConfigEditResponse> Edit(int Id)
        {
            var response = new SubConfigEditResponse();

            var emailConfigData = await _repo.GetEmailSubConfig(Id);
            
            //get the email template column details
            var emailTemplateDetails = await _repo.GetEmailSubTemplateDetails(Id);
            
            //assign to email template column list
            if(emailTemplateDetails!=null && emailTemplateDetails.Any())
            {
                emailConfigData.TemplateColumnList = emailTemplateDetails;
            }

            //used in email send page
            response.IsUseByEmailSend = await _repo.GetSubjectFileNameDetails(Id);

            if (emailConfigData.Id > 0)
            {
                response.editDetails = emailConfigData;
                response.Result = Result.Success;
            }
            else
            {
                response.Result = Result.NotFound;
            }

            return response;
        }

        /// <summary>
        /// summary page search details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<EmailSubConfigSummaryResponse> Search(EmailSubConfigSummary request)
        {
            if (request == null)
                return new EmailSubConfigSummaryResponse() { Result = EmailSubConfigSummaryResult.RequestNotCorrectFormat };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 20;

            var response = new EmailSubConfigSummaryResponse { Index = request.Index.Value, PageSize = request.PageSize.Value };

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            var data = _repo.GetEmailSubjectTemplate();

            if (data == null)
                return new EmailSubConfigSummaryResponse() { Result = EmailSubConfigSummaryResult.NotFound };

            data = FilterSummaryData(data, request);

            var result = await data.Skip(skip).Take(take).AsNoTracking().ToListAsync();

            if (result == null || !result.Any())
                return new EmailSubConfigSummaryResponse() { Result = EmailSubConfigSummaryResult.NotFound };

            //get subject id or file id list
            var templateIdList = result.Select(x => x.SubConfigId).ToList();

            //get mapped email subject id list
            var EmailRuleTemplateIds = await _repo.GetEmailTemplateIdList(templateIdList);

            //union the file name ids and subject ids
            var EmailTemplateIdList = EmailRuleTemplateIds.Select(x => x.SubjectId).Distinct().Union(EmailRuleTemplateIds.Select(X => X.FileNameId).Distinct()).ToList();

            var res = result.Select(x => _emailsubject.TemplateMasterMap(x, EmailTemplateIdList));

            return new EmailSubConfigSummaryResponse
            {
                Result = EmailSubConfigSummaryResult.Success,
                TotalCount = await data.CountAsync(),
                Index = request.Index.Value,
                PageSize = request.PageSize.Value,
                PageCount = (response.TotalCount / request.PageSize.Value) + (response.TotalCount % request.PageSize.Value > 0 ? 1 : 0),
                Data = res.ToList()
            };
        }

        /// <summary>
        /// filter the data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private IQueryable<EmailSubConfigRepo> FilterSummaryData(IQueryable<EmailSubConfigRepo> data, EmailSubConfigSummary request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 20;

            //filter by customer id list
            if (request.CustomerIds != null && request.CustomerIds.Any())
            {
                data = data.Where(x => request.CustomerIds.Contains(x.CustomerId.GetValueOrDefault()));
            }

            //filter by template name
            if (request.TemplateName != null)
            {
                data = data.Where(x => EF.Functions.Like(x.TemplateName, $"%{request.TemplateName.Trim()}%"));
            }

            //filter by email type id
            if (request.EmailTypeId != null && request.EmailTypeId > 0)
            {
                data = data.Where(x => x.EmailTypeId == request.EmailTypeId);
            }

            //filter by module id
            if (request.ModuleId != null && request.ModuleId > 0)
            {
                data = data.Where(x => x.ModuleId == request.ModuleId);
            }
            return data;
        }

        /// <summary>
        /// delete the email send configuration
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> Delete(int Id)
        {
            //delete check map with email rule
            if (await _repo.IsEmailSubjectConfigWithEmailRule(Id))
            {
                return new DeleteResponse() { Result = Result.MappedEmailRule };
            }

            var entity = await _repo.GetEmailSubConfigList(Id);

            if (entity == null)
                return new DeleteResponse() { Result = Result.NotFound };

            entity.Active = false;
            entity.DeletedBy = _applicationContext.UserId;
            entity.DeletedOn = DateTime.Now;

            _repo.RemoveEntities(entity.EsSuTemplateDetails);

            await _repo.Save();

            return new DeleteResponse() { Result = Result.Success };
        }

        /// <summary>
        /// get emailtype list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEmailTypeList()
        {
            var data = await _repo.GetEmailTypeList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// get module list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetModuleList()
        {
            var data = await _repo.GetModuleList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }
    }
}
