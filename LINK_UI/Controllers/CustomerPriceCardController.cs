using Components.Web;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CustomerPriceCardController : ControllerBase
    {
        private readonly ICustomerPriceCardManager _customerPriceCardManager = null;

        public CustomerPriceCardController(ICustomerPriceCardManager customerPriceCardManager)
        {
            _customerPriceCardManager = customerPriceCardManager;
        }

        // POST: CustomerPriceCard/CuPrDetails
        [HttpPost]
        public async Task<SaveCustomerPriceCardResponse> Save(CustomerPriceCard request)
        {
            return await _customerPriceCardManager.Save(request);
        }
        
        //GET: CustomerPriceCard/Edit/{id}
        [HttpGet("{id}")]
        public async Task<EditSaveCustomerPriceCardResponse> Edit(int id)
        {
            return await _customerPriceCardManager.Edit(id);
        }

        [HttpPost("getData")]
        public async Task<CustomerPriceCardSummaryResponse> GetData(CustomerPriceCardSummary request)
        {
            return await _customerPriceCardManager.GetData(request);
        }

        [HttpGet("delete/{id}")]
        public async Task<ResponseResult> Delete(int id)
        {
            return await _customerPriceCardManager.Delete(id);
        }      

        [HttpPost("exportData")]
        public async Task<IActionResult> ExportData(CustomerPriceCardSummary request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.pageSize = PageSize;
            var response = await _customerPriceCardManager.ExportSummary(request);
            if (response == null || !response.Any())
                return NotFound();
            return await this.FileAsync("CustomerPriceCardSummary", response, Components.Core.entities.FileType.Excel);
        }

        [HttpGet("getcustomerpriceholidaylist")]
        public async Task<DataSourceResponse> GetCustomerPriceHolidayList()
        {
            var response = await _customerPriceCardManager.GetCustomerPriceHolidayList();
            return response;
        }
    }
}
