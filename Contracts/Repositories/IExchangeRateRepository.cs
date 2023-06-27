using DTO.FinanceDashboard;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IExchangeRateRepository : IRepository
    {
        /// <summary>
        /// Get Rate type List
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EmExchangeRateType>> GetRateTypeList();

        /// <summary>
        /// Get Exchange rate list
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        Task<IEnumerable<EmExchangeRate>> GetExchangeRateList(int currencyId, DateTime fromDate, DateTime toDate, int exchangeTypeId);


        /// <summary>
        /// Get exchange data by one date
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="date"></param>
        /// <param name="exchangeTypeId"></param>
        /// <returns></returns>
        Task<IEnumerable<EmExchangeRate>> GetExchangeRateList(int currencyId, DateTime date,  int exchangeTypeId);

        /// <summary>
        /// Get conversion list
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<IEnumerable<EmExchangeRate>> GetConversionList(IEnumerable<int> idList);

        /// <summary>
        /// Get exchanges
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="currencyId"></param>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<EmExchangeRate>> GetExchanges(int targetId, int currencyId, DateTime date, int type);

        /// <summary>
        /// Get food allowance
        /// </summary>
        /// <param name="date"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<decimal> GetFoodAllowance(DateTime date, int countryId);
        
        /// <summary>
        /// Fetch the exchange rates for different currency to USD
        /// </summary>
        /// <param name="currencyList"></param>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<EmExchangeRate>> GetExchangesList(List<ExchangeCurrency> currencyList, DateTime date, int type);
    }
}
