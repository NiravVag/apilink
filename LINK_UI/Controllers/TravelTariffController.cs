using Contracts.Managers;
using DTO.CommonClass;
using DTO.TravelTariff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class TravelTariffController : ControllerBase
    {
        private readonly ITravelTariffManager _traveltariffManager = null;
        private readonly ISharedInspectionManager _helper = null;
        public TravelTariffController(ITravelTariffManager traveltariffManager, ISharedInspectionManager helper)
        {
            _traveltariffManager = traveltariffManager;
            _helper = helper;
        }

        [HttpPost("GetAllTravelTariff")]
        public async Task<TravelTariffGetAllResponse> GetAllTravelTariff(TravelTariffSearchRequest request)
        {
            return await _traveltariffManager.GetAllTravelTariffDetails(request);
        }

        [HttpGet("StartPotList")]
        public async Task<DataSourceResponse> Get()
        {
            return await _traveltariffManager.GetStartPotList();
        }


        [HttpGet("{id}")]
        public async Task<TravelTariffGetResponse> Get(int id)
        {
            return await _traveltariffManager.GetTravelTariffDetails(id);
        }

        [HttpPost]
        public async Task<TravelTariffSaveResponse> Post(TravelTariffSaveRequest model)
        {
            if (!ModelState.IsValid)
            {
                return new TravelTariffSaveResponse() { Result = TravelTariffResponseResult.RequestNotCorrectFormat };
            }
            return await _traveltariffManager.SaveTravelTariff(model);
        }

        [HttpPut("{id}")]
        public async Task<TravelTariffSaveResponse> Put(TravelTariffSaveRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new TravelTariffSaveResponse() { Result = TravelTariffResponseResult.RequestNotCorrectFormat };
            }
            return await _traveltariffManager.UpdateTravelTariff(request);
        }

        [HttpDelete("{id}")]
        public async Task<TravelTariffDeleteResponse> Delete(int id)
        {
            return await _traveltariffManager.DeleteTravelTariff(id);
        }

        [HttpPost("Export")]
        public async Task<IActionResult> ExportTravelTariff([FromBody] TravelTariffSearchRequest request)
        {
            if (request == null)
                return NotFound();
            var response = await _traveltariffManager.GetExportTravelTariffDetails(request);
            if (response == null)
                return NotFound();
            Stream stream = _helper.GetAsStreamObject(response.TravelTariffDetails);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BookingSummaryTemplate.xlsx");
        }
    }
}
