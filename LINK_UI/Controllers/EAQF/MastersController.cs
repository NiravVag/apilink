using Contracts.Managers;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.ProductManagement;
using DTO.References;
using LINK_UI.Controllers.EXTERNAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace LINK_UI.Controllers.EAQF
{
    [Route("api/EAQF/[controller]")]
    [Authorize(Policy = "EAQFUserPolicy")]
    [ApiController]
    public class MastersController : ExternalBaseController
    {
        private readonly IReferenceManager _refManager = null;
        public MastersController(IReferenceManager refManager)
        {
            _refManager = refManager;
        }
        /// <summary>
        /// GetProductCategory
        /// </summary>
        /// <param name="productline"></param>
        /// <returns></returns>

        [HttpGet("productcategory")]
        public async Task<IActionResult> GetProductCategory(string ProductLineName, int ProductLineId)
        {
            var response = await _refManager.GetEAQFProductsCategories(ProductLineName, ProductLineId);
            return BuildCommonEaqfResponse(response);
        }
        /// <summary>
        /// GetBusinessLines
        /// </summary>
        /// <returns></returns>

        [HttpGet("productline")]

        public async Task<IActionResult> GetBusinessLines()
        {
            var response = new EaqfGetSuccessResponse();
            try
            {
                var data = await _refManager.GetBusinessLines();
                if (data == null)
                {
                    var eaqfResponse = BuildEaqfErrorResponse(System.Net.HttpStatusCode.BadRequest, ApiCommonData.BadRequest, new List<string>() { ApiCommonData.BadRequest });
                    return BuildCommonEaqfResponse(eaqfResponse);
                }
                else
                {
                    response.statusCode = System.Net.HttpStatusCode.OK;
                    response.message = ApiCommonData.Success;
                    response.data = data.DataSourceList;
                    return BuildCommonEaqfResponse(response);
                }
            }
            catch
            {
                var eaqfErrorResponse = BuildEaqfErrorResponse(System.Net.HttpStatusCode.BadRequest, ApiCommonData.BadRequest, new List<string>() { ApiCommonData.BadRequest });
                return BuildCommonEaqfResponse(eaqfErrorResponse);
            }
        }
        /// <summary>
        /// GetAPIServices
        /// </summary>
        /// <returns></returns>

        [HttpGet("servicecategories")]
        public async Task<IActionResult> GetAPIServices()
        {
            var response = new EaqfGetSuccessResponse();
            try
            {
                var data = await _refManager.GetAPIServices();
                if (data == null)
                {
                    var eaqfResponse = BuildEaqfErrorResponse(System.Net.HttpStatusCode.BadRequest, ApiCommonData.BadRequest, new List<string>() { ApiCommonData.BadRequest });
                    return BuildCommonEaqfResponse(eaqfResponse);
                }
                else
                {
                    response.statusCode = System.Net.HttpStatusCode.OK;
                    response.message = ApiCommonData.Success;
                    response.data = data.DataSourceList;
                    return BuildCommonEaqfResponse(response);
                }
            }
            catch
            {
                var eaqfErrorResponse = BuildEaqfErrorResponse(System.Net.HttpStatusCode.BadRequest, ApiCommonData.BadRequest, new List<string>() { ApiCommonData.BadRequest });
                return BuildCommonEaqfResponse(eaqfErrorResponse);
            }
        }
        /// <summary>
        /// GetServices
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>

        [HttpGet("services")]
        public async Task<IActionResult> GetServices(int CustomerId, string ServiceCategoryName, int ServiceCategoryId)
        {
            var response = await _refManager.GetEAQFServices(CustomerId, ServiceCategoryName, ServiceCategoryId);
            return BuildCommonEaqfResponse(response);
        }
        /// <summary>
        /// GetProductType
        /// </summary>
        /// <param name="productCategoryName"></param>
        /// <param name="productSubCategoryName"></param>
        /// <returns></returns>

        [HttpGet("producttype")]
        public async Task<IActionResult> GetProductType(string productcategoryname, string productsubcategoryname)
        {
            var response = await _refManager.GetEAQFProductType(productcategoryname, productsubcategoryname);
            return BuildCommonEaqfResponse(response);
        }

        [NonAction]
        public EaqfErrorResponse BuildEaqfErrorResponse(HttpStatusCode statusCode, string message, List<string> errors)
        {
            return new EaqfErrorResponse()
            {
                errors = errors,
                statusCode = statusCode,
                message = message
            };
        }
    }


}
