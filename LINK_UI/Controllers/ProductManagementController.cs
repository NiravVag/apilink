using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.ProductManagement;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DTO.References;
using System.Security.Claims;
using LINK_UI.App_start;
using Microsoft.Extensions.Configuration;
using DTO.CommonClass;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class ProductManagementController : ControllerBase
    {
        private IProductManagementManager _productManagementManager = null;
        private static IConfiguration _configuration = null;
        private readonly ISharedInspectionManager _helper = null;
        public ProductManagementController(IProductManagementManager productManagementManager, IConfiguration configuration, ISharedInspectionManager helper)
        {
            _productManagementManager = productManagementManager;
            _configuration = configuration;
            _helper = helper;
        }
        #region product category
        [Right("product-category")]
        [HttpGet("productcategory")]
        public ProductCategoryResponse GetProductCategorySummary()
        {
            return _productManagementManager.GetProductCategorySummary();
        }

        [Right("product-category")]
        [HttpPost("productcategory")]
        public async Task<SaveProductCategoryResponse> SaveProductCategoryAsync([FromBody]ProductCategory request)
        {
            string strFbToken = getFbToken();
            return await _productManagementManager.SaveProductCategory(request, strFbToken);
        }

        [Right("product-category")]
        [HttpGet("productcategory/{id}")]
        public EditProductCategoryResponse GetProductCategoryById(int? id)
        {
            return _productManagementManager.GetProductCategoryById(id);
        }

        [Right("product-category")]
        [HttpPut("productcategory")]
        public async Task<SaveProductCategoryResponse> UpdateProductCategory([FromBody]ProductCategory request)
        {
            string strFbToken = getFbToken();
            return await _productManagementManager.SaveProductCategory(request, strFbToken);
        }

        [Right("product-category")]
        [HttpDelete("productcategory/{id}")]
        public async Task<DeleteProductCategoryResponse> DeleteProductCategory(int id)
        {
            return await _productManagementManager.RemoveProductCategory(id);
        }

        [HttpGet("productcategorylist")]
        public DataSourceResponse GetProductCategories()
        {
            return  _productManagementManager.GetProductCategories();
        }

        /// <summary>
        /// Get FB token based on the needs
        /// </summary>
        /// <returns></returns>
        private string getFbToken()
        {
            var Fbclaims = new List<Claim>
            {
                new Claim("email",_configuration["FbAdminEmail"]),
                new Claim("firstname", _configuration["FbAdminUserName"]),
                new Claim("lastname", ""),
                new Claim("role", "admin"),
                new Claim("redirect", "")
            };
            return AuthentificationService.CreateFBToken(Fbclaims, _configuration["FBKey"]);
        }
        #endregion

        #region product sub category
        [Right("product-subcategory")]
        [HttpGet("productsubcategory")]
        public ProductSubCategoryResponse GetProductSubCategorySummary()
        {
            return _productManagementManager.GetProductSubCategorySummary();
        }

        [Right("product-subcategory")]
        [HttpPost("productsubcategory")]
        public async Task<SaveProductSubCategoryResponse> SaveProductSubCategoryAsync([FromBody]ProductSubCategory request)
        {
            string strFbToken = getFbToken();
            return await _productManagementManager.SaveProductSubCategory(request, strFbToken);
        }

        [Right("product-subcategory")]
        [HttpGet("productsubcategory/{id}")]
        public EditProductSubCategoryResponse GetProductSubCategoryById(int? id)
        {
            return _productManagementManager.GetProductSubCategoryById(id);
        }

        [Right("product-subcategory")]
        [HttpPut("productsubcategory")]
        public async Task<SaveProductSubCategoryResponse> UpdateProductSubCategory([FromBody]ProductSubCategory request)
        {
            string strFbToken = getFbToken();
            return await _productManagementManager.SaveProductSubCategory(request, strFbToken);
        }

        [Right("product-subcategory")]
        [HttpDelete("productsubcategory/{id}")]
        public async Task<DeleteProductSubCategoryResponse> DeleteProductSubCategory(int id)
        {
            return await _productManagementManager.RemoveProductSubCategory(id);
        }


        [HttpGet("productsubcategorylist")]
        public DataSourceResponse GetProductSubCategories()
        {
            return _productManagementManager.GetProductSubCategories();
        }
        #endregion

        #region product category sub2
        [Right("product-category-sub2-summary")]
        [HttpGet("productcategorysub2")]
        public ProductCategorySub2Response GetProductCategorySub2Summary()
        {
            return _productManagementManager.GetProductCategorySub2Summary();
        }

        [Right("product-category-sub2-summary")]
        [HttpPost("searchproductcategorysub2")]
        public ProductCategorySub2SearchResponse GetProductCategorySub2SearchSummary([FromBody] ProductCategorySub2SearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;
            return _productManagementManager.GetProductCategorySub2SearchSummary(request);
        }

        [Right("product-category-sub2-summary")]
        [HttpPost("productcategorysub2")]
        public async Task<SaveProductCategorySub2Response> SaveProductCategorySub2Async([FromBody]ProductCategorySub2 request)
        {
            string strFbToken = getFbToken();
            return await _productManagementManager.SaveProductCategorySub2(request, strFbToken);
        }

        [Right("product-category-sub2-summary")]
        [HttpGet("productcategorysub2/{id}")]
        public EditProductCategorySub2Response GetProductCategorySub2ById(int? id)
        {
            return _productManagementManager.GetProductCategorySub2ById(id);
        }

        [Right("product-category-sub2-summary")]
        [HttpPut("productcategorysub2")]
        public async Task<SaveProductCategorySub2Response> UpdateProductCategorySub2([FromBody]ProductCategorySub2 request)
        {
            string strFbToken = getFbToken();
            return await _productManagementManager.SaveProductCategorySub2(request, strFbToken);
        }

        [Right("product-category-sub2-summary")]
        [HttpDelete("productcategorysub2/{id}")]
        public async Task<DeleteProductCategorySub2Response> DeleteProductCategorySub2(int id)
        {
            return await _productManagementManager.RemoveProductCategorySub2(id);
        }

        [Right(new string[] { "product-subcategory", "product-category-sub2-summary" })]
        [HttpGet("productcategory/productsubcategories/{id}")]
        public ProductSubCategoryResponse GetSubByCategory(int id)
        {
            return _productManagementManager.GetProductSubCategoryByCategory(id);
        }

        [Right("product-category-sub2-summary")]
        [HttpGet("productsubcategory/productcategorysub2s/{id}")]
        public ProductCategorySub2Response GetTypeBySubCategory(int id)
        {
            return _productManagementManager.GetProductCategorySub2BySubCategory(id);
        }

        #endregion

        #region product category sub3

        [HttpPost("productcategorysub3s/save")]
        public async Task<SaveProductCategorySub3Response> SaveProdSubCategory3(SaveProductCategorySub3 request)
        {
            return await _productManagementManager.SaveProdSubCategory3(request);
        }

        [HttpPut("productcategorysub3s/update")]
        public async Task<SaveProductCategorySub3Response> UpdateProdSubCategory3(SaveProductCategorySub3 request)
        {
            return await _productManagementManager.UpdateProdSubCategory3(request);
        }

        [HttpDelete("delete-productcategorysub3/{id}")]
        public async Task<DeleteProdCategorySub3Response> DeleteProdSubCategory3(int id)
        {
            return await _productManagementManager.DeleteProdSubCategory3(id);
        }

        [HttpGet("product-category-sub3/{id}")]
        public async Task<EditProdCategorySub3Response> GetProductCategorySub3(int id)
        {
            return await _productManagementManager.GetProdSubCategory3ById(id);
        }

        [HttpPost("product-category-sub3")]
        public async Task<ProdCatSub3SummaryResponse> GetProductCategorySub3(ProdCatSub3SummaryRequest request)
        {
            return await _productManagementManager.GetProdSubCategory3Summary(request);
        }

        [HttpPost("Export-product-category-sub3")]
        public async Task<IActionResult> ExportProdCatSub3Summary(ProdCatSub3SummaryRequest request)
        {
            var response = await _productManagementManager.ExportProdSubCategory3Summary(request);
            if (response == null)
                return NotFound();
            var stream = _helper.GetAsStreamObject(response);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "prod_cat_sub3_summary.xlsx");
        }

        [HttpGet("product-category-sub3-list/{productSubCategory2Id}")]
        public async Task<ProductSubCategory3Response> GetProductCategorySub3List(int productSubCategory2Id)
        {
            return await _productManagementManager.GetProductCategorySub3List(productSubCategory2Id);
        }

        #endregion
    }
}