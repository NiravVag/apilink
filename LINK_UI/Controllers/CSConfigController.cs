using System.Threading.Tasks;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Contracts.Managers;
using DTO.References;
using DTO.Customer;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CSConfigController : ControllerBase
    {
        private readonly ICSConfigManager _csManager = null;
        private readonly ISharedInspectionManager _helper = null;
        public CSConfigController(ICSConfigManager csManager, ISharedInspectionManager helper)
        {
            _csManager = csManager;
            _helper = helper;
        }

        [HttpGet("getService")]
        [Right("customer-summary")]
        public async Task<ServiceResponse> GetService()
        {
            return await _csManager.GetService();
        }
        [HttpGet("getCustomerService")]
        [Right("customer-summary")]
        public async Task<CSResponse> GetCustomerService()
        {
            return await _csManager.GetCustomerService();
        }
        [HttpPost("saveCSConfigRegister")]
        [Right("customer-summary")]
        public Task<SaveCSConfResponse> Save([FromBody] SaveCSConfig request)
        {
            return _csManager.CSConfigSave(request);
        }
        [HttpPost("searchCSConfig")]
        [Right("customer-summary")]
        public CSConfigSearchResponse SearchCSConfig
            ([FromBody] CSConfigSearchRequest request)
        {
            return _csManager.GetAllCSConfig(request);
        }
        [HttpPost("deleteCSConfig")]
        [Right("customer-summary")]
        public async Task<CSConfigDeleteResponse> DeleteCSConfig([FromBody] CSConfigDelete id)
        {
            return await _csManager.DeleteCSConfig(id);
        }
        [HttpGet("editCSConfig/{id}")]
        [Right("customer-summary")]
        public async Task<EditCSConfigResponse> GetCSConfig(int id)
        {
            return await _csManager.GetEditCSConfig(id);
        }

        [HttpPost("getCustomerAllocation")]
        [Right("customer-summary")]
        public async Task<CSAllocationResponse> SearchCSAllocation([FromBody] CSAllocationSearchRequest request)
        {
            return await _csManager.GetCSAllocations(request);
        }

        [HttpPost("saveCustomerAllocation")]
        [Right("customer-summary")]
        public async Task<SaveCSAllocationResponse> SaveCSAllocation([FromBody] SaveCSAllocation request)
        {
            return await _csManager.SaveCSAllocationAsync(request);
        }

        [HttpGet("get-cs-allocation/{id}")]
        [Right("customer-summary")]
        public async Task<GetCSAllocationResponse> Get(int id)
        {
            return await _csManager.GetCSAllocation(id);
        }

        [HttpPost("deleteCustomerAllocations")]
        [Right("customer-summary")]
        public async Task<DeleteCSAllocationResponse> DeleteCSAllocations([FromBody] CSAllocationDeleteItem request)
        {
            return await _csManager.DeleteAllocationsAsync(request);
        }

        [HttpPost("Export-cs-allocation")]
        public async Task<IActionResult> ExportWorkLoadMatrixSummary(CSAllocationSearchRequest request)
        {
            var response = await _csManager.ExportCSAllocationSummary(request);
            if (response == null)
                return NotFound();
            var stream = _helper.GetAsStreamObject(response);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Workload_Matrix_summary.xlsx");
        }
    }
}
