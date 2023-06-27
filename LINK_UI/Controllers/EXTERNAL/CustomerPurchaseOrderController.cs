using Contracts.Managers;
using DTO.PurchaseOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LINK_UI.Controllers.EXTERNAL
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPurchaseOrderController : ExternalBaseController
    {
        private readonly IPurchaseOrderManager _manager = null;
        public CustomerPurchaseOrderController(IPurchaseOrderManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Save Customer Purchase order based on the valid Input Request
        /// </summary>
        /// <param name="request">Purchase order details</param>
        /// <returns></returns>
        [Authorize(Policy = "CflUserPolicy")]
        [HttpPost]
        public async Task<IActionResult> SaveCustomerPurchaseOrder([FromBody] CustomerPurchaseOrderDetails request)
        {
            var response = await _manager.SaveCustomerPurchaseDetails(request);
            return BuildCommonResponse(response.statusCode, response);
        }

        /// <summary>
        /// Get Customer Purchase order by pono
        /// </summary>
        /// <param name="pono"></param>
        /// <returns></returns>
        [Authorize(Policy = "CflUserPolicy")]
        [HttpGet("{pono}")]
        public async Task<IActionResult> GetCustomerPurchaseOrder(string pono)
        {
            var response = await _manager.GetPurchaseOrderDetails(pono);
            return BuildCommonResponse(response.statusCode, response);
        }

        /// <summary>
        /// Update Customer Purchase Order by Po No
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "CflUserPolicy")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomerPurchaseDetails([FromBody] CustomerPurchaseOrderDetails request)
        {
            var response = await _manager.UpdateCustomerPurchaseDetails(request);
            return BuildCommonResponse(response.statusCode, response);
        }

        /// <summary>
        /// Delete Customer Purchase order by PoNo
        /// </summary>
        /// <param name="pono">PO Number</param>
        /// <returns></returns>
        [Authorize(Policy = "CflUserPolicy")]
        [HttpDelete("{pono}")]
        public async Task<IActionResult> getCustomerPurchaseOrderbyPoNo(string pono)
        {
            var response = await _manager.Deletecustomerpurchaseorder(pono);
            return BuildCommonResponse(response.statusCode, response);
        }
    }
}
