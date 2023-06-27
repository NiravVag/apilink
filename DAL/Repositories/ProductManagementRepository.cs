using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Contracts.Repositories;
using DTO.References;
using Entities;
using Microsoft.EntityFrameworkCore;
using DTO.ProductManagement;
using DTO.WorkLoadMatrix;

namespace DAL.Repositories
{
    public class ProductManagementRepository : Repository, IProductManagementRepository
    {
        public ProductManagementRepository(API_DBContext context) : base(context)
        {
        }
        #region Product Category
        public IEnumerable<RefProductCategory> GetProductCategories()
        {
            return _context.RefProductCategories.Where(x => x.Active).Include(y=>y.BusinessLine);
        }

        public RefProductCategory GetProductCategorybyId(int id)
        {
            return _context.RefProductCategories.Where(x => x.Id == id).Include(x => x.CuProducts)
                .Include(x => x.CuServiceTypes).Include(x => x.HrStaffProductCategories).Include(x => x.RefProductCategorySubs).FirstOrDefault();
        }

        /// <summary>
        /// get fb product category id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int?> GetFBProductCategorybyId(int id)
        {
            return await _context.RefProductCategories.Where(x => x.Id == id && x.Active)
             .Select(x => x.FbProductCategoryId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get fb product sub category id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int?> GetFBProductSubCategorybyId(int id)
        {
            return await _context.RefProductCategorySubs.Where(x => x.Id == id && x.Active)
             .Select(x => x.FbProductSubCategoryId).FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveProductCategory(int id)
        {
            var entity = await _context.RefProductCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;
            entity.Active = false;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;
        }

        public Task<int> SaveEditProductCategory(RefProductCategory entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveNewProductCategory(RefProductCategory entity)
        {
            _context.RefProductCategories.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<bool> IsProductCategoryAvailbyId(int id, int entityId)
        {
            return _context.RefProductCategories.AnyAsync(x => x.Id == id && x.Active && x.EntityId == entityId);
        }
        #endregion

        #region Product Sub Category
        public IEnumerable<RefProductCategorySub> GetProductSubCategories()
        {
            return _context.RefProductCategorySubs.Where(x => x.Active).Include(x => x.ProductCategory);
        }

        public RefProductCategorySub GetProductSubCategorybyId(int id)
        {
            return _context.RefProductCategorySubs.Where(x => x.Id == id).Include(x => x.CuProducts)
               .Include(x => x.RefProductCategorySub2S).Include(x => x.ProductCategory).FirstOrDefault();
        }

        public Task<int> SaveNewProductSubCategory(RefProductCategorySub entity)
        {
            _context.RefProductCategorySubs.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveEditProductSubCategory(RefProductCategorySub entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveProductSubCategory(int id)
        {
            var entity = await _context.RefProductCategorySubs.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;
            entity.Active = false;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;
        }

        public Task<bool> IsProductSubCategoryAvailbyId(int id, int productCategoryId, int entityId)
        {
            return _context.RefProductCategorySubs.AnyAsync(a => a.Id == id && a.Active && a.ProductCategoryId == productCategoryId && a.EntityId == entityId);
        }
        #endregion

        #region Product Category Sub2

        public RefProductCategorySub2 GetProductCategorySub2ById(int id)
        {
            return _context.RefProductCategorySub2S.Where(x => x.Id == id).Include(x => x.CuProducts)
               .Include(x => x.ProductSubCategory).ThenInclude(x => x.ProductCategory).FirstOrDefault();
        }

        public IEnumerable<RefProductCategorySub2> GetProductCategorySub2s()
        {
            return _context.RefProductCategorySub2S.Where(x => x.Active).Include(x => x.ProductSubCategory).ThenInclude(x => x.ProductCategory);
        }

        public Task<int> SaveNewProductCategorySub2(RefProductCategorySub2 entity)
        {
            _context.RefProductCategorySub2S.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveEditProductCategorySub2(RefProductCategorySub2 entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveProductCategorySub2(int id)
        {
            var entity = await _context.RefProductCategorySub2S.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;
            entity.Active = false;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;
        }

        public Task<bool> IsProductSub2CategoryAvailbyId(int id, int productSubCategoryId, int entityId)
        {
            return _context.RefProductCategorySub2S.AnyAsync(a => a.Id == id && a.Active && a.ProductSubCategoryId == productSubCategoryId && a.EntityId == entityId);
        }
        #endregion

        #region Product Category Sub3
        /// <summary>
        /// check if duplicate
        /// </summary>
        /// <param name="subCategory2Id"></param>
        /// <param name="subCategory3Name"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfSubCat3AlreadyExists(int subCategory2Id, string subCategory3Name)
        {
            return await _context.RefProductCategorySub3S.AnyAsync(x => x.Active && x.ProductSubCategory2Id == subCategory2Id && x.Name == subCategory3Name);
        }

        /// <summary>
        /// get the category sub 3 by Id for update
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RefProductCategorySub3> GetProdSubCat3(int id)
        {
            return await _context.RefProductCategorySub3S.Where(x => x.Active && x.Id == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// get the category sub 3 by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProdCatSub3Data> GetProdSubCat3ById(int id)
        {
            return await _context.RefProductCategorySub3S.Where(x => x.Active && x.Id == id)
                .Select(x => new ProdCatSub3Data
                {
                    Id = x.Id,
                    ProdCategoryId = x.ProductSubCategory2.ProductSubCategory.ProductCategoryId,
                    ProdSubCategoryId = x.ProductSubCategory2.ProductSubCategoryId,
                    ProdCategorySub2Id = x.ProductSubCategory2Id,
                    ProdCategorySub3 = x.Name
                }).FirstOrDefaultAsync();
        }
        /// <summary>
        /// get the data for the summary page
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProdCatSub3Data> GetProdSubCat3ByEfCore()
        {
            return _context.RefProductCategorySub3S.Where(x => x.Active)
                .Select(x => new ProdCatSub3Data
                {
                    Id = x.Id,
                    ProdCategoryId = x.ProductSubCategory2.ProductSubCategory.ProductCategoryId,
                    ProdCategoryName = x.ProductSubCategory2.ProductSubCategory.ProductCategory.Name,
                    ProdSubCategoryId = x.ProductSubCategory2.ProductSubCategoryId,
                    ProdSubCategoryName = x.ProductSubCategory2.ProductSubCategory.Name,
                    ProdCategorySub2Id = x.ProductSubCategory2Id,
                    ProdCategorySub2Name = x.ProductSubCategory2.Name,
                    ProdCategorySub3 = x.Name
                });
        }

        /// <summary>
        /// get the sub cat 3 data for which work load is not configured for the summary page
        /// </summary>
        /// <returns></returns>
        public IQueryable<WorkLoadMatrixData> GetProdSubCat3WorkLoadNotConfiguredByEfCore()
        {
            return _context.RefProductCategorySub3S.Where(x => x.Active && !x.QuWorkLoadMatrices.Any(x => x.Active.Value))
                .Select(x => new WorkLoadMatrixData
                {
                    ProdCategorySub3Id = x.Id,
                    ProdCategoryId = x.ProductSubCategory2.ProductSubCategory.ProductCategoryId,
                    ProdCategoryName = x.ProductSubCategory2.ProductSubCategory.ProductCategory.Name,
                    ProdSubCategoryId = x.ProductSubCategory2.ProductSubCategoryId,
                    ProdSubCategoryName = x.ProductSubCategory2.ProductSubCategory.Name,
                    ProdCategorySub2Id = x.ProductSubCategory2Id,
                    ProdCategorySub2Name = x.ProductSubCategory2.Name,
                    ProdCategorySub3Name = x.Name
                });
        }
        /// <summary>
        /// Check if Product cat sub 3 is mapped to any product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckIdProdCatSub3MappedToCustomerProduct(int id)
        {
            return await _context.CuProducts.AnyAsync(x => x.Active && x.ProductCategorySub3 == id);
        }
        #endregion
    }
}