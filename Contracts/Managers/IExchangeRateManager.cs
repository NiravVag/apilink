using DTO.ExchangeRate;
using DTO.HumanResource;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.Common;
using DTO.FinanceDashboard;

namespace Contracts.Managers
{
    public interface IExchangeRateManager
    {
        /// <summary>
        /// Get exchange Rate Summary
        /// </summary>
        /// <returns></returns>
        Task<ExchangeRateResponse> GetExchangeRateSummary();

        /// <summary>
        /// get Exchange result
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ExchangeDataResponse> GetExchangeResult(ExchangeDataRequest request);

        /// <summary>
        /// Save Exchange Rates
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<SaveExchangeRateResponse> SaveExchangeRate(SaveConversionRequest request);

        /// <summary>
        /// Get matrix rate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<RateMatrixResponse> GetMatrixRate(RateMatrixRequest request);

        /// <summary>
        /// Excel export
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        Task<MatrixDataExport> GetMatrixRateExport(int currencyId, DateObject fromDate, DateObject toDate, int typeId);

        /// <summary>
        /// Get Exchange rate between two currencies
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="currencyId"></param>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<string> GetExchangeRate(int targetId, int currencyId, DateObject date, ExhangeRateTypeEnum type);

        /// <summary>
        /// Get food Allowance
        /// </summary>
        /// <param name="date"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<decimal> GetFoodAllowance(DateObject date, int countryId);

        /// <summary>
        /// Get Exchange rate between two currencies
        /// </summary>
        /// <param name="currencyList"></param>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Dictionary<int, double>> GetExchangeRateList(List<ExchangeCurrency> currencyList, DateTime date, ExhangeRateTypeEnum type);


    }
}
