using DTO.CommonClass;
using DTO.Invoice;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ITravelMatrixManager
    {
        /// <summary>
        /// get list from InvTmTypes(invoice travel matrix type) table
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetTravelMatrixTypes();
        /// <summary>
        /// save and update details with exists check
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TravelMatixSaveResponse> Save(IEnumerable<TravelMatrix> model);
        /// <summary>
        /// search the travel martix data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TravelMatrixSearchResponse> Search(TravelMatrixSummary request);
        /// <summary>
        /// get province list with country id
        /// </summary>
        /// <param name="countryIds"></param>
        /// <returns></returns>
        Task<AreaDataResponse> GetProvinceLists(IEnumerable<int> countryIds);
        /// <summary>
        /// get city list with province id
        /// </summary>
        /// <param name="cityIds"></param>
        /// <returns></returns>
        Task<AreaDataResponse> GetCityLists(IEnumerable<int> cityIds);
        /// <summary>
        /// get county list with city id
        /// </summary>
        /// <param name="countyIds"></param>
        /// <returns></returns>
        Task<AreaDataResponse> GetCountyLists(IEnumerable<int> countyIds);
        /// <summary>
        /// delete the travel matrix details logically
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<TravelMatixDeleteResponse> Delete(IEnumerable<int?> ids);
        /// <summary>
        /// export data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IEnumerable<TravelMatrixExportSummary> ExportSummary(IEnumerable<TravelMatrixSearch> data);    

        Task<DataSourceResponse> GetCountyListByCountry(int CountryId, string CountyName);

        Task<List<TravelMatrixSearch>> GetTravelMatrixList(QuotationMatrixRequest request);
    }
}
