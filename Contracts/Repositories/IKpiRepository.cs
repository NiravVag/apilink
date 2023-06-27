using DTO.KPI;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IKpiRepository : IRepository
    {
        /// <summary>
        /// Get Modules
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ApModule>> GetModules();

        /// <summary>
        /// Get sub modules
        /// </summary>
        /// <param name="idmodule"></param>
        /// <returns></returns>
        Task<IEnumerable<ApSubModule>> GetSubModules(int idmodule);


        /// <summary>
        /// Get submodule List
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<IEnumerable<ApSubModule>> GetSubModuleList(IEnumerable<int> idList);

        /// <summary>
        /// Get Columns
        /// </summary>
        /// <param name="idSubModule"></param>
        /// <returns></returns>
        Task<IEnumerable<KpiColumnItem>> GetColumns(int idSubModule);

        /// <summary>
        /// Get columns by module
        /// </summary>
        /// <param name="idModule"></param>
        /// <returns></returns>
        Task<IEnumerable<KpiColumnItem>> GetColumnsByModule(int idModule);

        /// <summary>
        /// Get columns By Id Template
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns></returns>
        Task<IEnumerable<TemplateColumn>> GetColumnsByTemplate(int idTemplate);

        /// <summary>
        /// Get Filters
        /// </summary>
        /// <param name="idSubModule"></param>
        /// <returns></returns>
        Task<IEnumerable<KpiFilterItem>> GetFilters(int idSubModule);

        /// <summary>
        /// Get filters by module
        /// </summary>
        /// <param name="idModule"></param>
        /// <returns></returns>
        Task<IEnumerable<KpiFilterItem>> GetFiltersByModule(int idModule);

        /// <summary>
        /// Get templates
        /// </summary>
        /// <param name="idSubModule"></param>
        /// <returns></returns>
        Task<IEnumerable<KpiTemplateItem>> GetTemplates(int? idSubModule = null);


        /// <summary>
        /// Get Template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<KpiTemplate> GetTemplate(int id );

        /// <summary>
        /// Get Template columns
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns></returns>
        Task<IEnumerable<KpiTemplateColumn>> GetTemplateColumns(int idTemplate);

        /// <summary>
        /// Get Template Filters
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns></returns>
        Task<IEnumerable<KpiTemplateColumn>> GetTemplateFilters(int idTemplate);

        /// <summary>
        /// Get data source by name
        /// </summary>
        /// <param name="dataSourceName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> GetDataSourceByName(string dataSourceName, string fieldName, string term, IDictionary<string, object> parameters = null);

        /// <summary>
        /// Get column or filter by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<KpiTemplateColumn> GetFieldById(int id);


        /// <summary>
        /// Se
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(IEnumerable<KpiTemplateItem>, int)> SearchTemplates(KpiTemplateListRequest request);

        /// <summary>
        /// Get view template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<KpiTemplateView> GetViewTemplate(int id);

        /// <summary>
        /// dataSourceResult
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ViewData> DataSourceResult(KpiTemplateViewRequest request);

    }
}
