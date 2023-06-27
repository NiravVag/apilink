using DTO.CommonClass;
using DTO.Invoice;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ITravelMatrixRepository : IRepository
    {
        /// <summary>
        /// get list from InvTmTypes(invoice travel matrix type) table
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetTravelMatrixTypes();
        /// <summary>
        /// get active data from invtmdetails table
        /// </summary>
        /// <returns></returns>
        IQueryable<TravelMatrixSearch> GetTravelMatrixData();
        //IQueryable<InvTmDetail> GetTravelMatrixData();
        /// <summary>
        /// get area details like country province city county
        /// </summary>
        /// <returns></returns>
        IQueryable<AreaDetails> GetDefaultAreaData();

        IQueryable<AreaDetails> GetDefaultAreaDataByCity();

        /// <summary>
        /// get all the active travel matrix details
        /// </summary>
        /// <returns></returns>
        IQueryable<TravelMatrixData> GetAllTravelMatrix(ExistsTravelMatrixRequest request);
        /// <summary>
        /// pass the country id list and get province data with parent id(country)
        /// </summary>
        /// <param name="countryIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ParentDataSource>> GetProvinceLists(IEnumerable<int> countryIds);
        /// <summary>
        /// pass the province id list and get city data with parent id(province)
        /// </summary>
        /// <param name="provinceIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ParentDataSource>> GetCityLists(IEnumerable<int> provinceIds);
        /// <summary>
        /// pass the county id list and get county data with parent id(county)
        /// </summary>
        /// <param name="cityIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ParentDataSource>> GetCountyLists(IEnumerable<int> cityIds);
        /// <summary>
        /// get details with list of ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<InvTmDetail>> GetTravelMatrixDetails(IEnumerable<int?> ids);
        /// <summary>
        /// get details with left join of two query 
        /// </summary>
        /// <param name="tmData"></param>
        /// <param name="areaData"></param>
        /// <returns></returns>
        IQueryable<TravelMatrixSearch> GetTravelMatrixDefaultDataByCounty(IQueryable<TravelMatrixSearch> tmData, IQueryable<AreaDetails> areaData);

        IQueryable<TravelMatrixSearch> GetTravelMatrixDefaultDataByCity(IQueryable<TravelMatrixSearch> tmData, IQueryable<AreaDetails> areaData);

        /// <summary>
        /// get travel matrix details by county, matrix type and source currency
        /// </summary>
        /// <param name="matrixRequest"></param>
        /// <returns></returns>
        Task<IEnumerable<TravelMatrixSearch>> GetMatrixData(QuotationMatrixRequest matrixRequest);
        /// <summary>
        /// Get County List by Country Name
        /// </summary>
        /// <param name="cityIds"></param>
        /// <param name="countyName"></param>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetCountyListsByCountyName(IEnumerable<int> cityIds, string countyName);

        IQueryable<TravelMatrixSearch> GetTravelMatrixData(QuotationMatrixRequest matrixRequest);
    }
}
