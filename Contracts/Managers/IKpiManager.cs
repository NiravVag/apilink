using DTO.KPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IKpiManager
    {
        /// <summary>
        /// GetModuleList
        /// </summary>
        /// <returns></returns>
        Task<ModuleListResponse> GetModuleList();

        /// <summary>
        /// Get SubModule List
        /// </summary>
        /// <param name="idmodule"></param>
        /// <returns></returns>
        Task<ModuleListResponse> GetSubModuleList(int idmodule);

        /// <summary>
        /// Get column list
        /// </summary>
        /// <param name="idSubModule"></param>
        /// <returns></returns>
        Task<KpiColumnListResponse> GetColumnList(int idSubModule);

        /// <summary>
        /// Get clumns by module
        /// </summary>
        /// <param name="idModule"></param>
        /// <returns></returns>
        Task<KpiColumnListResponse> GetColumnListByModule(int idModule);

        /// <summary>
        /// Get filter List
        /// </summary>
        /// <param name="idSubModule"></param>
        /// <returns></returns>
        Task<KpiFilterListResponse> GetFilterList(int idSubModule);

        /// <summary>
        /// Get filter list by module
        /// </summary>
        /// <param name="idModule"></param>
        /// <returns></returns>
        Task<KpiFilterListResponse> GetFilterListByModule(int idModule);


        /// <summary>
        /// Get templates for current user
        /// </summary>
        /// <param name="idSubModule"></param>
        /// <returns></returns>
        Task<KpiTemplateListResponse> GetTemplateList(int? idSubModule);

        /// <summary>
        /// Get Template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<KpiTemplateItemResponse> GetTemplate(int id);

        /// <summary>
        /// Get columns by id Template
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns></returns>
        Task<KpiTemplateColumnListResponse> GetTemplateColumnList(int idTemplate);

        /// <summary>
        /// Get Filter Templates
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns></returns>
        Task<KpiTemplateFilterListResponse> GetTemplateFilterList(int idTemplate);

        /// <summary>
        /// Get data source by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<KpiDataSourceResponse> GetDataSource(int id);

        /// <summary>
        /// Get data lazy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldName"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        Task<IEnumerable<object>> GetDataLazy(int id, string fieldName, string term);

        /// <summary>
        /// Save Template
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<KpiSavetemplateResponse> SaveTemplate(KpiTemplateRequest request);

        /// <summary>
        /// Search templates
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<KpiTemplateListResponse> SearchTemplates(KpiTemplateListRequest request);

        /// <summary>
        /// Get view template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<KpiTemplateViewResponse> GetViewTemplate(int id);

        /// <summary>
        /// View Result
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ViewDataResponse> ViewResult(KpiTemplateViewRequest request);

        /// <summary>
        /// GetColumns ByTemplate
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Task<IEnumerable<TemplateColumn>> GetColumnListByTemplate(int templateId);

        /// <summary>
        /// Delete Template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeleteTemplateResponse> DeleteTemplate(int id);

    }
}
