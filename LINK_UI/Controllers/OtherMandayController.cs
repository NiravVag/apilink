using Contracts.Managers;
using DTO.CommonClass;
using DTO.OtherManday;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ApiUserPolicy")]
    [ApiController]
    public class OtherMandayController : ControllerBase
    {
        private readonly IOtherMandayManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;

        public OtherMandayController(IOtherMandayManager manager, ISharedInspectionManager helper)
        {
            _manager = manager;
            _helper = helper;
        }

        [HttpPost("save")]
        public async Task<SaveOtherMandayResponse> SaveOtherManday(SaveOtherMandayRequest request)
        {
            return await _manager.SaveOtherManday(request);
        }

        [HttpPut("update")]
        public async Task<SaveOtherMandayResponse> UpdateOtherManday(SaveOtherMandayRequest request)
        {
            return await _manager.UpdateOtherManday(request);
        }

        [HttpDelete("delete/{id}")]
        public async Task<DeleteOtherMandayResponse> DeleteOtherManday(int id)
        {
            return await _manager.DeleteOtherManday(id);
        }

        [HttpGet("edit-other-manday/{id}")]
        public async Task<EditOtherMandayResponse> GetEditOtherManday(int id)
        {
            return await _manager.GetEditOtherManday(id);
        }

        [HttpPost("other-manday-summary")]
        public async Task<OtherMandaySummaryResponse> GetOtherMandaySummary(OtherMandaySummaryRequest request)
        {
            return await _manager.GetOtherMandaySummary(request);
        }

        [HttpGet("getPurposeList")]
        public async Task<DataSourceResponse> GetPurposeList()
        {
            return await _manager.GetPurposeList();
        }

        [HttpPost("ExportOtherMandaySummary")]
        public async Task<IActionResult> ExportOtherMandaySummary(OtherMandaySummaryRequest request)
        {
            var response = await _manager.ExportOtherMandaySummary(request);
            if (response == null)
                return NotFound();
            var stream = _helper.GetAsStreamObject(response);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "other_manday.xlsx");
        }
    }
}
