using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using System.Threading.Tasks;
using DTO.ProductManagement;
using System.Linq;
using DTO.WorkLoadMatrix;

namespace Contracts.Repositories
{
    public interface IProductManagementRepository : IRepository
    {
        #region Product Category
        /// <summary>
        /// Get all product categories
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefProductCategory> GetProductCategories();
        /// <summary>
        /// Get ProductDetails By Id
        /// </summary>
        /// <param name="product id"></param>
        /// <returns>RefProductCategory</returns>
        RefProductCategory GetProductCategorybyId(int id);

        /// <summary>
        /// Save New Product category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>id</returns>
        Task<int> SaveNewProductCategory(RefProductCategory entity);

        /// <summary>
        /// Save Edit Product category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> SaveEditProductCategory(RefProductCategory entity);

        /// <summary>
        /// Delete Product category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> RemoveProductCategory(int id);

        /// <summary>
        /// Check active Product Category is available for provided id and entityId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        Task<bool> IsProductCategoryAvailbyId(int id, int entityId);
        #endregion

        #region Product Sub Category
        /// <summary>
        /// Get all product sub categories
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefProductCategorySub> GetProductSubCategories();
        /// <summary>
        /// Get ProductDetails By Id
        /// </summary>
        /// <param name="product id"></param>
        /// <returns>RefProductCategory</returns>
        RefProductCategorySub GetProductSubCategorybyId(int id);

        /// <summary>
        /// Save New Product sub category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>id</returns>
        Task<int> SaveNewProductSubCategory(RefProductCategorySub entity);

        /// <summary>
        /// Save Edit Product sub category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> SaveEditProductSubCategory(RefProductCategorySub entity);

        /// <summary>
        /// Delete Product sub category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> RemoveProductSubCategory(int id);

        Task<bool> IsProductSubCategoryAvailbyId(int id, int productCategoryId, int entityId);
        #endregion

        #region Product Category Sub 2
        /// <summary>
        /// Get all ProductCategorySub2
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefProductCategorySub2> GetProductCategorySub2s();
		/// <summary>
		/// Get ProductCategorySub2 By Id
		/// </summary>
		/// <param name="product id"></param>
		/// <returns>RefProductCategory</returns>
		RefProductCategorySub2 GetProductCategorySub2ById(int id);

		/// <summary>
		/// Save New ProductCategorySub2
		/// </summary>
		/// <param name="entity"></param>
		/// <returns>id</returns>
		Task<int> SaveNewProductCategorySub2(RefProductCategorySub2 entity);

		/// <summary>
		/// Save Edit ProductCategorySub2
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		Task<int> SaveEditProductCategorySub2(RefProductCategorySub2 entity);

		/// <summary>
		/// Delete ProductCategorySub2
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		Task<bool> RemoveProductCategorySub2(int id);

        Task<bool> IsProductSub2CategoryAvailbyId(int id, int productSubCategoryId, int entityId);
        #endregion


        Task<int?> GetFBProductCategorybyId(int id);
        Task<int?> GetFBProductSubCategorybyId(int id);

        #region Prod Sub Cat3
        Task<bool> CheckIfSubCat3AlreadyExists(int subCategory2Id, string subCategory3Name);
        Task<RefProductCategorySub3> GetProdSubCat3(int id);
        Task<ProdCatSub3Data> GetProdSubCat3ById(int id);
        IQueryable<ProdCatSub3Data> GetProdSubCat3ByEfCore();
        IQueryable<WorkLoadMatrixData> GetProdSubCat3WorkLoadNotConfiguredByEfCore();
        Task<bool> CheckIdProdCatSub3MappedToCustomerProduct(int id);
        #endregion
    }
}