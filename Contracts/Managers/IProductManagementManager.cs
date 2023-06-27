using DTO.ProductManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.References;
using DTO.CommonClass;

namespace Contracts.Managers
{
    public interface IProductManagementManager
    {
        #region Product Category
        /// <summary>
        /// Get product categories
        /// </summary>
        /// <returns>ProductResponse</returns> 
        DataSourceResponse GetProductCategories();

        /// <summary>
        /// Get product category summary
        /// </summary>
        /// <returns>ProductResponse</returns> 
        ProductCategoryResponse GetProductCategorySummary();
        /// <summary>
        //  Save Product category
        //  </summary>
        //  <param name="product"></param>
        //  <returns></returns>
        Task<SaveProductCategoryResponse> SaveProductCategory(ProductCategory procategory, string fbToken);
        /// <summary>
        /// Get product category by id
        /// </summary>
        /// <returns>ProductResponse</returns> 
        EditProductCategoryResponse GetProductCategoryById(int? id);
        /// <summary>
        /// Delete product category by id
        /// </summary>
        /// <returns>ProductResponse</returns> 
        Task<DeleteProductCategoryResponse> RemoveProductCategory(int id);
        #endregion

        #region Product Sub Category
        /// <summary>
        /// Get product Sub categories
        /// </summary>
        /// <returns>ProductResponse</returns> 
        DataSourceResponse GetProductSubCategories();

        /// <summary>
        /// Get product Sub category summary
        /// </summary>
        /// <returns>ProductResponse</returns> 
        ProductSubCategoryResponse GetProductSubCategorySummary();
        /// <summary>
        //  Save Product Sub category
        //  </summary>
        //  <param name="product"></param>
        //  <returns></returns>
        Task<SaveProductSubCategoryResponse> SaveProductSubCategory(ProductSubCategory procategory, string fbToken);
        /// <summary>
        /// Get product Sub category by id
        /// </summary>
        /// <returns>ProductResponse</returns> 
        EditProductSubCategoryResponse GetProductSubCategoryById(int? id);
        /// <summary>
        /// Delete product Sub category by id
        /// </summary>
        /// <returns>ProductResponse</returns> 
        Task<DeleteProductSubCategoryResponse> RemoveProductSubCategory(int id);
        #endregion

        #region Product Category Sub2
        /// <summary>
        /// Get Product Category Sub2
        /// </summary>
        /// <returns>ProductResponse</returns> 
        IEnumerable<ProductCategorySub2> GetProductCategorySub2s();

        /// <summary>
        /// Get Product Category Sub2 summary
        /// </summary>
        /// <returns>ProductResponse</returns> 
        ProductCategorySub2Response GetProductCategorySub2Summary();

        /// <summary>
        /// Get Product Category Sub2 search summary
        /// </summary>
        /// <returns>ProductResponse</returns> 
        ProductCategorySub2SearchResponse GetProductCategorySub2SearchSummary(ProductCategorySub2SearchRequest request);

        /// <summary>
        ///  Save Product Category Sub2
        ///  </summary>
        ///  <param name="product"></param>
        ///  <returns></returns>
        Task<SaveProductCategorySub2Response> SaveProductCategorySub2(ProductCategorySub2 procategory, string fbToken);
        /// <summary>
        /// Get Product Category Sub2 by id
        /// </summary>
        /// <returns>ProductResponse</returns> 
        EditProductCategorySub2Response GetProductCategorySub2ById(int? id);
        /// <summary>
        /// Delete Product Category Sub2 by id
        /// </summary>
        /// <returns>ProductResponse</returns> 
        Task<DeleteProductCategorySub2Response> RemoveProductCategorySub2(int id);

        /// <summary>
        /// Get product Sub category by category
        /// </summary>
        /// <returns>ProductResponse</returns> 
        ProductSubCategoryResponse GetProductSubCategoryByCategory(int? id);

        /// <summary>
        /// Get product category sub2 by sub category
        /// </summary>
        /// <returns>ProductResponse</returns> 
        ProductCategorySub2Response GetProductCategorySub2BySubCategory(int? id);
        #endregion

        #region Product Category Sub3
        Task<SaveProductCategorySub3Response> SaveProdSubCategory3(SaveProductCategorySub3 request);
        Task<SaveProductCategorySub3Response> UpdateProdSubCategory3(SaveProductCategorySub3 request);
        Task<DeleteProdCategorySub3Response> DeleteProdSubCategory3(int id);
        Task<EditProdCategorySub3Response> GetProdSubCategory3ById(int id);
        Task<ProdCatSub3SummaryResponse> GetProdSubCategory3Summary(ProdCatSub3SummaryRequest request);
        Task<List<ExportProdCatSub3Data>> ExportProdSubCategory3Summary(ProdCatSub3SummaryRequest request);

        Task<ProductSubCategory3Response> GetProductCategorySub3List(int productSubCategory2Id);

        #endregion

       
    }
}
