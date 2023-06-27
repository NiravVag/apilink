using Contracts.Managers;
using DTO.CustomerProducts;
using DTO.PurchaseOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers.EXTERNAL
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerProductRefController : ExternalBaseController
    {
        private readonly ICustomerProductManager _manager = null;
        private readonly IProductManagementManager _productmanager = null;
        public CustomerProductRefController(ICustomerProductManager manager, IProductManagementManager productManagementManager)
        {
            _manager = manager;
            _productmanager = productManagementManager;
        }

        [Authorize(Policy = "CflUserPolicy")]
        [HttpPost]
        public async Task<IActionResult> SaveCustomerProductRef([FromBody] CustomerProductRefRequest request)
        {
            var response = await _manager.SaveCustomerProductRef(request);
            return BuildCommonResponse(response.statusCode, response);
        }

        [Authorize(Policy = "CflUserPolicy")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomerProductRef([FromBody] CustomerProductRefRequest request)
        {
            var response = await _manager.UpdateCustomerProductRef(request);
            return BuildCommonResponse(response.statusCode, response);
        }

        [Authorize(Policy = "CflUserPolicy")]
        [HttpDelete("{productReference}")]
        public async Task<IActionResult> RemoveCustomerProductbyProductRef(string productReference)
        {
            var response = await _manager.DeleteCustomerProductRef(productReference);
            return BuildCommonResponse(response.statusCode, response);
        }

        [Authorize(Policy = "CflUserPolicy")]
        [HttpGet("{productreference}")]
        public async Task<IActionResult> GetProductReferanceDetail(string productReference)
        {
            var response = await _manager.GetProductReferance(productReference);
            return BuildCommonResponse(response.statusCode, response);
        }

        [Authorize(Policy = "CflUserPolicy")]
        [HttpDelete("DeleteProductFileAttachments/{uniqueId}")]
        public async Task<IActionResult> DeleteProductFileAttachments(string uniqueId)
        {
            var response = await _manager.DeleteProductFileAttachments(uniqueId);
            return BuildCommonResponse(response.statusCode, response);
        }

        /// <summary>
        ///Upload Customer Product File 
        /// </summary>
        /// <param name="productreference"></param>
        /// <returns></returns>

        [Authorize(Policy = "CflUserPolicy")]
        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadPurchaseOrderDetails([FromForm] CustomerProductFileUpload request)
        {
            var response = await _manager.UploadProductAttachment(request);
            return BuildCommonResponse(response.statusCode, response);
        }
    }
}
