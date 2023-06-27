using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Components.Web;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.Master;
using DTO.ProductManagement;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQUtility;
using DTO.PurchaseOrder;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class CustomerProductController : ControllerBase
    {
        private readonly ICustomerProductManager _manager = null;
        private readonly IReferenceManager _refManager = null;
        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
        private readonly ITenantProvider _filterService = null;
        private static IConfiguration _configuration = null;
        private readonly ISharedInspectionManager _helper = null;

        public CustomerProductController(ICustomerProductManager manager, IReferenceManager refManager, IRabbitMQGenericClient rabbitMQClient,
            ITenantProvider filterService, IConfiguration configuration, ISharedInspectionManager helper)
        {
            _manager = manager;
            _refManager = refManager;
            _rabbitMQClient = rabbitMQClient;
            _filterService = filterService;
            _configuration = configuration;
            _helper = helper;
        }

        [HttpGet("getProductsCategory")]
        [Right("customer-productsummary")]
        public async Task<ProductCategoryResponse> GetProductCategory()
        {
            var response = await _refManager.GetProductCategories();
            return response;
        }

        [HttpGet("productscategorybycustomer/{id}")]
        [Right("customer-productsummary")]
        public async Task<IEnumerable<CustomerProduct>> GetProductCategoryByCustomer(int id)
        {
            var response = _manager.GetCustomerProductsByCustomer(id);
            return response;
        }

        [HttpGet("productsbycustomerandcategory/{id}/{productcategoryid}")]
        [Right("customer-productsummary")]
        public IEnumerable<CustomerProduct> GetProductsByCustomerAndCategory(int id, int productCategoryId)
        {
            var response = _manager.GetProductsByCustomerAndCategory(id, productCategoryId);
            return response;
        }

        [HttpGet("getProductSubCategory/{id}")]
        [Right("customer-productsummary")]
        public async Task<ProductSubCategoryResponse> GetProductSubCategory(int id)
        {
            var response = await _refManager.GetProductSubCategories(id);
            return response;
        }

        [HttpGet("getProductCategorySub2/{id}")]
        [Right("customer-productsummary")]
        public async Task<ProductCategorySub2Response> GetProductCategorySub2(int id)
        {
            var response = await _refManager.GetProductCategorySub2(id);
            return response;
        }

        // GET: api/Product
        [HttpPost("search")]
        [Right("customer-productsummary")]
        public async Task<CustomerProductSearchResponse> Get(CustomerProductSearchRequest request)
        {
            var response = await _manager.SearchCustomerProducts(request);
            return response;
        }

        //Export Customer Products
        [HttpPost("export-customerproducts")]
        [Right("customer-productsummary")]
        public async Task<IActionResult> ExportCustomerProducts(CustomerProductSearchRequest request)
        {
            var response = await _manager.CustomerProductsExportDetails(request);
            if (response == null || response.Result == CustomerProductSearchResult.NotFound)
                return NotFound();

            if (request.isStyle)
            {
                return await this.FileAsync("CustomerStyles", response.CustomerProductExportData, Components.Core.entities.FileType.Excel);
            }
            return await this.FileAsync("CustomerProducts", response.CustomerProductExportData, Components.Core.entities.FileType.Excel);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        [Right("customer-productsummary")]
        public EditCustomerProductResponse Get(int id)
        {
            return _manager.GetEditCustomerProduct(id);
        }

        // POST: api/Product
        [HttpPost]
        [Right("edit-customer-product")]
        public async Task<SaveCustomerProductResponse> SaveCustomerProducts([FromBody] CustomerProduct request)
        {
            var response = await _manager.SaveCustomerProduct(request);
            if (response.Id > 0)
            {
                //create the product in the FB
                UpdateProductDetailsToFB(request.ApiServiceIds, response.Id, MasterDataType.ProductCreation);

                UpdateProductDetailsToTCF(request.ApiServiceIds, response.Id, MasterDataType.ProductCreation);
            }
            return response;
        }

        /// <summary>
        /// Add or update the products in the FB
        /// </summary>
        /// <param name="apiServiceIds"></param>
        /// <param name="productId"></param>
        /// <param name="masterDataMap"></param>
        private async void UpdateProductDetailsToFB(IEnumerable<int> apiServiceIds, int productId, MasterDataType masterDataMap)
        {
            bool isTCFProduct = false;
            //if only one api service configured for the product and it is tcf then we should not push to fb
            if (apiServiceIds != null && apiServiceIds.Count() == 1 && apiServiceIds.Contains((int)Service.Tcf))
                isTCFProduct = true;

            //push the product account to FB if selected api service is not in TCF
            if (!isTCFProduct)
            {
                var fbProductRequest = new MasterDataRequest()
                {
                    Id = Guid.NewGuid(),
                    SearchId = productId,
                    ExternalClient = ExternalClient.FullBridge,
                    MasterDataType = masterDataMap,
                    EntityId= _filterService.GetCompanyId()
                };
                await _rabbitMQClient.Publish<MasterDataRequest>(_configuration["AccountQueue"], fbProductRequest);
            }
        }

        private async void UpdateProductDetailsToTCF(IEnumerable<int> apiServiceIds, int productId, MasterDataType masterDataMap)
        {
            //push the customer account to FB if selected api service is TCF
            if (apiServiceIds != null && apiServiceIds.Contains((int)Service.Tcf))
            {
                var tcfCustomerContactRequest = new MasterDataRequest()
                {
                    Id = Guid.NewGuid(),
                    SearchId = productId,
                    ExternalClient = ExternalClient.TCF,
                    MasterDataType = masterDataMap,
                    EntityId = _filterService.GetCompanyId()
                };
                await _rabbitMQClient.Publish<MasterDataRequest>(_configuration["AccountQueue"], tcfCustomerContactRequest);
            }
        }

        // POST: api/Product
        [HttpPost("savecustomerproductlist")]
        [Right("edit-customer-product")]
        public async Task<SaveCustomerProductResponse> SaveCustomerProductList([FromBody] List<CustomerProduct> request)
        {
            var response = await _manager.SaveCustomerProductList(request);
            return response;
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        [Right("edit-customer-product")]
        public async Task<SaveCustomerProductResponse> UpdateCustomerProducts(int id, [FromBody] CustomerProduct request)
        {
            var response = await _manager.SaveCustomerProduct(request);
            return response;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Right("customer-productsummary")]
        public async Task<CustomerProductDeleteResponse> DeleteCustomerProduct(int id)
        {
            return await _manager.RemoveCustomerProduct(id);
        }

        [HttpPost("GetProductCategoryDataSource")]
        public async Task<DataSourceResponse> GetProductCategoryDataSource(ProductCategoryDataSourceRequest request)
        {
            return await _refManager.GetProductCategorySourceList(request);
        }

        [HttpPost("GetProductSubCategoryDataSource")]
        public async Task<DataSourceResponse> GetProductSubCategoryDataSource(ProductSubCategoryDataSourceRequest request)
        {
            return await _refManager.GetProductSubCategorySourceList(request);
        }

        [HttpPost("get-product-subcategory2-details")]
        public async Task<DataSourceResponse> GetProductSubCategory2DataSource(ProductSubCategory2DataSourceRequest request)
        {
            return await _refManager.GetProductSubCategory2SourceList(request);
        }
        [HttpPost("CustomerProductDataSourceList")]
        [Right("supplier")]
        public async Task<CustomerProductDataSourceResponse> GetCustomerProductDataSourceList(CustomerProductDataSourceRequest request)
        {
            return await _manager.GetCustomerProductDataSourceList(request);
        }

        [HttpPost("CustomerProductDetailsDataSourceList")]
        [Right("supplier")]
        public async Task<POProductListResponse> GetCustomerProductExtendedDataSourceList(CustomerProductDetailsDataSourceRequest request)
        {
            return await _manager.GetCustomerProductDetailsDataSourceList(request);
        }


        [HttpGet("fileTypeList")]
        public async Task<DataSourceResponse> GetFileTypeList()
        {
            return await _manager.GetFileTypeList();
        }

        [HttpPost("products-by-customer")]
        [Right("customer-productsummary")]
        public async Task<CustomerProductDetailResponse> GetProductsByCustomer(PoProductRequest poProductRequest)
        {
            return await _manager.GetProductDetailsByCustomer(poProductRequest);
        }

        [HttpPost("get-product-subcategory3-details")]
        public async Task<DataSourceResponse> GetProductSubCategory3DataSource(ProductSubCategory3DataSourceRequest request)
        {
            return await _refManager.GetProductSubCategory3SourceList(request);
        }

        [HttpGet("get-product-file-urls/{productId}")]
        public async Task<CustomerProductFileResponse> GetProductFileUrls(int productId)
        {
            return await _manager.GetProductImageUrls(productId);
        }

        [HttpGet("msChartFileFormatByCustomer/{customerId}")]
        public async Task<MSChartFileFormatResponse> GetMSChartFileFormatByCustomer(int customerId)
        {
            return await _manager.GetMSChartFileFormatByCustomer(customerId);
        }

        [HttpPost("OcrTableData")]
        public async Task<dynamic> GetOcrTableData([FromBody] OcrTableRequest request)
        {
            return await _manager.GetOcrTableData(request);
        }

        [HttpPost("export-ocr")]
        public async Task<IActionResult> ExportCustomerProducts(List<ExportOcrData> exportOcrData)
        {
            if (exportOcrData == null || !exportOcrData.Any())
                return NotFound();
            Stream stream = _helper.GetAsStreamObject(exportOcrData);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportOcr.xlsx");
        }
    }
}
