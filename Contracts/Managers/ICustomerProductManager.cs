using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.File;
using DTO.PurchaseOrder;
using DTO.References;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICustomerProductManager
    {
        Task<CustomerProductSearchResponse> SearchCustomerProducts(CustomerProductSearchRequest request);
        Task<CustomerProductExportDataResponse> CustomerProductsExportDetails(CustomerProductSearchRequest request);
        Task<SaveCustomerProductResponse> SaveCustomerProduct(CustomerProduct request);
        Task<CustomerProductDeleteResponse> RemoveCustomerProduct(int id);
        EditCustomerProductResponse GetEditCustomerProduct(int id);
        Task<SaveCustomerProductResponse> SaveCustomerProductList(List<CustomerProduct> requestList);
        IEnumerable<CustomerProduct> GetCustomerProductsByCustomer(int customerId);
        IEnumerable<CustomerProduct> GetProductsByCustomerAndCategory(int customerId, int productCategoryId);
        Task<CustomerProductDataSourceResponse> GetCustomerProductDataSourceList(CustomerProductDataSourceRequest request);
        Task<DataSourceResponse> GetFileTypeList();

        Task<CustomerProductDetailResponse> GetProductDetailsByCustomer(PoProductRequest poProductRequest);

        Task<List<string>> GetProductNameByProductIds(IEnumerable<int> productIds);

        Task<CustomerProductFileResponse> GetProductImageUrls(int productId);

        Task<POProductListResponse> GetCustomerProductDetailsDataSourceList(CustomerProductDetailsDataSourceRequest request);
        Task<CustomerProductRefResponse> SaveCustomerProductRef(CustomerProductRefRequest request);
        Task<CustomerProductRefResponse> UpdateCustomerProductRef(CustomerProductRefRequest request);
        Task<CustomerProductRefResponse> DeleteCustomerProductRef(string productReference);
        Task<SaveCustomerProductResponses> GetProductReferance(string productReference);
        Task<CustomerProductRefResponse> DeleteProductFileAttachments(string uniqueId);
        Task<UploadCustomerProductResponse> UploadProductAttachment(CustomerProductFileUpload customerProductFileUpload);
        Task<MSChartFileFormatResponse> GetMSChartFileFormatByCustomer(int customerId);
        Task<dynamic> GetOcrTableData(OcrTableRequest request);
    }
}
