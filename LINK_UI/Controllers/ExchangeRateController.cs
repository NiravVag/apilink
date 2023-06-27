using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Core.contracts;
using Contracts.Managers;
using DTO.ExchangeRate;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Components.Web;
using Components.Core.entities;
using Microsoft.AspNetCore.Authorization;
using DTO.HumanResource;
using DTO.Common;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateManager _manager = null;

        public ExchangeRateController(IExchangeRateManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Right("edit-exchange")]
        public async Task<ExchangeRateResponse> ExchangeRateSummary()
        {
            var response = await _manager.GetExchangeRateSummary();

            return response;
        }

        /// <summary>
        /// Search Exchange
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Right("edit-exchange")]
        public async Task<ExchangeDataResponse> Search([FromBody]ExchangeDataRequest request)
        {
            if (request.Currency == null || request.Currency.Id <= 0)
                return new ExchangeDataResponse { Result = ExchangeDataResult.TargetCurrencyRequired };

            if (request.FromDate == null)
                return new ExchangeDataResponse { Result = ExchangeDataResult.BeginDateRequired };

            if (request.ToDate == null)
                return new ExchangeDataResponse { Result = ExchangeDataResult.EndDateRequired };

            if (request.ExchangeType == null || request.ExchangeType.Id <= 0)
                return new ExchangeDataResponse { Result = ExchangeDataResult.ExchangeTypeRequired };


            return await _manager.GetExchangeResult(request);
        }

        /// <summary>
        /// Search Exchange
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Right("edit-exchange")]
        public async Task<SaveExchangeRateResponse> Save([FromBody] SaveConversionRequest request)
        {
            return await _manager.SaveExchangeRate(request);
        }

        /// <summary>
        /// Search matrix
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Right("rate-matrix")]
        public async Task<RateMatrixResponse> GetMatrix([FromBody] RateMatrixRequest request)
        {
            return await _manager.GetMatrixRate(request);
        }

        [Right("rate-matrix")]
        [HttpGet("export/{id}/{fromDate}/{toDate}/{type}")]
        public async Task<IActionResult> GetFile(int id, string fromDate, string toDate, int type)
        {
            if (string.IsNullOrEmpty(fromDate))
                return NotFound();

            if (string.IsNullOrEmpty(toDate))
                return NotFound();

            if (id <= 0)
                return NotFound();

            if (type <= 0)
                return NotFound();

            var tabFrom = fromDate.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (tabFrom.Length < 3)
                return NotFound();

            var beginDate = new DateObject { Day = Convert.ToInt32(tabFrom[0]), Month = Convert.ToInt32(tabFrom[1]), Year = Convert.ToInt32(tabFrom[2]) };

            var tabTo = toDate.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (tabTo.Length < 3)
                return NotFound();

            var endDate = new DateObject { Day = Convert.ToInt32(tabTo[0]), Month = Convert.ToInt32(tabTo[1]), Year = Convert.ToInt32(tabTo[2]) };


            var model = await _manager.GetMatrixRateExport(id, beginDate, endDate, type);

            if (model == null || !model.Data.Any())
                return NotFound();


            return await this.FileAsync("CurrencyMatrixFormula", model, Components.Core.entities.FileType.Excel);
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            return await this.FileAsync("Test", Components.Core.entities.FileType.Excel);
        }
    }
}