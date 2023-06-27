using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DTO.Common.Static_Data_Common;
using Components.Core.entities;
using System.IO;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]

    public class TravelMatrixController : ControllerBase
    {
        private readonly ITravelMatrixManager _travelMatrixManager = null;
        private readonly ISharedInspectionManager _helper = null;

        public TravelMatrixController(ITravelMatrixManager travelMatrixManager, ISharedInspectionManager helper)
        {
            _travelMatrixManager = travelMatrixManager;
            _helper = helper;
        }
        [HttpGet("travelMatrixTypes")]
        public async Task<DataSourceResponse> GetTravelMatrixTypes()
        {
            return await _travelMatrixManager.GetTravelMatrixTypes();
        }

        [HttpPost("save")]
        public async Task<TravelMatixSaveResponse> Save(IEnumerable<TravelMatrix> model)
        {
            return await _travelMatrixManager.Save(model);
        }

        [HttpPost("search")]
        public async Task<TravelMatrixSearchResponse> Search(TravelMatrixSummary request)
        {
            return await _travelMatrixManager.Search(request);
        }
        [HttpPost("getProvinceLists")]
        public async Task<AreaDataResponse> GetProvinceLists(IEnumerable<int> countryIds)
        {
            return await _travelMatrixManager.GetProvinceLists(countryIds);
        }

        [HttpPost("getCityLists")]
        public async Task<AreaDataResponse> GetCityLists(IEnumerable<int> provinceIds)
        {
            return await _travelMatrixManager.GetCityLists(provinceIds);
        }

        [HttpPost("getCountyLists")]
        public async Task<AreaDataResponse> GetCountyLists(IEnumerable<int> cityIds)
        {
            return await _travelMatrixManager.GetCountyLists(cityIds);
        }


        [HttpPost("delete")]
        public async Task<TravelMatixDeleteResponse> Delete(IEnumerable<int?> ids)
        {
            return await _travelMatrixManager.Delete(ids);
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportData(TravelMatrixSummary request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            Stream stream = null;

            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.PageSize = PageSize;
            var response = await _travelMatrixManager.Search(request);
            var data = _travelMatrixManager.ExportSummary(response.GetData);
            if (response == null || (response.Result != TravelMatrixResponseResult.DefaultData && response.Result != TravelMatrixResponseResult.Success))
                return NotFound();

            stream = _helper.GetAsStreamObject(data);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TravelMatrixData.xlsx");

        }


        [HttpGet("getCountyListByCountry/{countryId}/{countyName}")]
        public async Task<DataSourceResponse> GetCountyListByCountry(int countryId, string countyName)
        {
            return await _travelMatrixManager.GetCountyListByCountry(countryId, countyName);
        }
    }
}