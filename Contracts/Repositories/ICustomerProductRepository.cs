using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.FullBridge;
using Entities;

namespace Contracts.Repositories
{
    public interface ICustomerProductRepository : IRepository
    {
        IQueryable<CuProduct> GetCustomerProducts();
        IQueryable<CustomerProductRepoExportData> GetCustomerProductsExportData();
        IEnumerable<CuProduct> GetCustomerProductsByCustomers(int customerId);
        CuProduct GetCustomerProductByID(int id);
        CuProduct GetProductsByCustomerAndProducts(int customerId, string productId);
        Task<SuSupplierCustomer> GetSupplierId(string SupplierCode);
        Task<int> AddCustomerProducts(CuProduct entity);
        Task<int> EditCustomerProducts(CuProduct entity);
        Task<int> RemoveCustomerProducts(int id);
        Task<IEnumerable<CuProductFileAttachment>> GetReceptFiles(int productId, IEnumerable<string> UniqueIdList);
        Task<CuProductFileAttachment> GetFile(int id);
        Task<int> RemoveProductFileAttachement(int id);
        CuProduct GetCustomerProductByProductID(string productID);

        CuProduct GetCustomerProductById(int id);
        IEnumerable<CuProduct> GetProductsByCustomerAndCategory(int customerId, int productCategoryId);
        Task<bool> CheckProductInspExists(int productid);
        Task<FBProductMasterData> GetFBCustomerProduct(int id, int? entityId);
        IQueryable<CuProduct> GetCustomerProductDataSource();
        Task<bool> CheckProductIsExistForThisCustomer(int customerId, string productId, int id);

        Task<int> GetProductIdByNameAndCustomer(int customerId, string productId);
        Task<IEnumerable<CommonDataSource>> GetFileTypeList();
        Task<List<CommonDataSource>> GetProductSubCategory2List(int? productSubCategoryId);
        CuProduct GetCustomerProductByEntityAndProductId(int id, int? entityId);

        Task<List<CustomerProductDetail>> GetProductsByProductIds(List<int> ProductIds);

        Task<List<string>> GetProductNameByProductIds(IEnumerable<int> productIds);

        Task<List<string>> GetProductImageUrls(int productId);

        Task<CustomerProductCategory> GetSubCategoryIdFromCustomerCategory(string subCategory);

        Task<CuProductType> GetSubCategoryTwoIdFromCustomerCategory(string productType);

        #region CustomerProductRef

        Task<CuProduct> GetProductRefByCustomer(int customerId, string productRef);
        Task<CUProductListResponse> GetProductFileList(int customerId, string productRef);
        Task<bool> CheckPOExist(int productId);
        IEnumerable<FileTypes> GetFilesByProductId(int productId);

        #endregion

        Task<List<CustomerProductDetail>> GetCustomerProductsByName(int customerId, List<string> productNameList);
        Task<bool> CheckProductExistsForCustomerId(int productid, int customerId);
        Task<List<FBProductMschartData>> GetFBProductMschart(int productId);
        IQueryable<CuProductMschartOcrMap> GetMSChartFileFormatByCustomer(int customerId);
    }
}
