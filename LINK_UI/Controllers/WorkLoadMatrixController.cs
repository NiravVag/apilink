using Contracts.Managers;
using DTO.WorkLoadMatrix;
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
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class WorkLoadMatrixController : ControllerBase
    {
        private readonly IWorkLoadMatrixManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;

        public  WorkLoadMatrixController(IWorkLoadMatrixManager manager, ISharedInspectionManager helper)
        {
            _manager = manager;
            _helper = helper;
        }

        [HttpPost("save")]
        public async Task<SaveWorkLoadMatrixResponse> SaveWorkLoadMatrix(SaveWorkLoadMatrixRequest request)
        {
            return await _manager.SaveWorkLoadMatrix(request);
        }

        [HttpPut("update")]
        public async Task<SaveWorkLoadMatrixResponse> UpdateWorkLoadMatrix(SaveWorkLoadMatrixRequest request)
        {
            return await _manager.UpdateWorkLoadMatrix(request);
        }

        [HttpDelete("delete/{id}")]
        public async Task<DeleteWorkLoadMatrixResponse> DeleteWorkLoadMatrix(int id)
        {
            return await _manager.DeleteWorkLoadMatrix(id);
        }

        [HttpGet("edit-work-load-matrix/{id}/{workLoadMatrixNotConfigured}")]
        public async Task<EditWorkLoadMatrixResponse> GetEditWorkLoadMatrix(int id, bool workLoadMatrixNotConfigured)
        {
            return await _manager.GetEditWorkLoadMatrix(id, workLoadMatrixNotConfigured);
        }

        [HttpPost("work-load-matrix-summary")]
        public async Task<WorkLoadMatrixSummaryResponse> GetWorkLoadMatrixSummary(WorkLoadMatrixSummaryRequest request)
        {
            return await _manager.GetWorkLoadMatrixSummary(request);
        }

        [HttpGet("work-load-matrix-by-prodCatSub3Id/{prodCatSub3Id}")]
        public async Task<EditWorkLoadMatrixResponse> GettWorkLoadMatrixDataByProdCatSub3Id(int prodCatSub3Id)
        {
            return await _manager.GettWorkLoadMatrixDataByProdCatSub3Id(prodCatSub3Id);
        }

        [HttpPost("Export-workload-matrix")]
        public async Task<IActionResult> ExportWorkLoadMatrixSummary(WorkLoadMatrixSummaryRequest request)
        {
            var response = await _manager.ExportWorkLoadMatrixSummary(request);
            if (response == null)
                return NotFound();
            var stream = _helper.GetAsStreamObject(response);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Workload_Matrix_summary.xlsx");
        }
    }
}
