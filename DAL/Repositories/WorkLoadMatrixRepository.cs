using Contracts.Repositories;
using DTO.WorkLoadMatrix;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class WorkLoadMatrixRepository : Repository, IWorkLoadMatrixRepository
    {
        public WorkLoadMatrixRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// check if duplicate
        /// </summary>
        /// <param name="subCategory3Id"></param>
        /// <param name="preparationTime"></param>
        /// <param name="sampleSize8h"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfWorkLoadMatrixAlreadyExists(int subCategory3Id,int workloadmatrixid=0)
        {
            var workload = _context.QuWorkLoadMatrices.Where(x=>x.Active.HasValue && x.Active.Value);
            if(workloadmatrixid>0)
            {
                workload = workload.Where(x => x.Id != workloadmatrixid);
            }
            return await workload.AnyAsync(x => x.ProductSubCategory3Id == subCategory3Id);
        }
        /// <summary>
        /// get the workloadmatrix by Id for update
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QuWorkLoadMatrix> GetWorkLoadMatrixById(int id)
        {
            return await _context.QuWorkLoadMatrices.Where(x => x.Active.Value && x.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get the category sub 3 by work load matrix Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WorkLoadMatrixData> GetWorkLoadMatrixEditDataById(int id)
        {
            return await _context.QuWorkLoadMatrices.Where(x => x.Active.Value && x.Id == id)
                .Select(x => new WorkLoadMatrixData
                {
                    Id = x.Id,
                    ProdCategoryId = x.ProductSubCategory3.ProductSubCategory2.ProductSubCategory.ProductCategoryId,
                    ProdSubCategoryId = x.ProductSubCategory3.ProductSubCategory2.ProductSubCategoryId,
                    ProdCategorySub2Id = x.ProductSubCategory3.ProductSubCategory2Id,
                    ProdCategorySub3Id = x.ProductSubCategory3Id,
                    PreparationTime = x.PreparationTime,
                    EightHourSampleSize = x.SampleSize8h
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get the category sub 3 by category sub3 Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WorkLoadMatrixData> GetProductCategorySub3ById(int id)
        {
            return await _context.RefProductCategorySub3S.Where(x => x.Active && x.Id == id)
                .Select(x => new WorkLoadMatrixData
                {
                    ProdCategoryId = x.ProductSubCategory2.ProductSubCategory.ProductCategoryId,
                    ProdSubCategoryId = x.ProductSubCategory2.ProductSubCategoryId,
                    ProdCategorySub2Id = x.ProductSubCategory2Id,
                    ProdCategorySub3Id = x.Id
                }).FirstOrDefaultAsync();
        }
        /// <summary>
        /// get the data for the summary page
        /// </summary>
        /// <returns></returns>
        public IQueryable<WorkLoadMatrixData> GetWorkLoadMatrixByEfCore()
        {
            return _context.QuWorkLoadMatrices.Where(x => x.Active.Value)
                .Select(x => new WorkLoadMatrixData
                {
                    Id = x.Id,
                    ProdCategoryId = x.ProductSubCategory3.ProductSubCategory2.ProductSubCategory.ProductCategoryId,
                    ProdCategoryName = x.ProductSubCategory3.ProductSubCategory2.ProductSubCategory.ProductCategory.Name,
                    ProdSubCategoryId = x.ProductSubCategory3.ProductSubCategory2.ProductSubCategoryId,
                    ProdSubCategoryName = x.ProductSubCategory3.ProductSubCategory2.ProductSubCategory.Name,
                    ProdCategorySub2Id = x.ProductSubCategory3.ProductSubCategory2Id,
                    ProdCategorySub2Name = x.ProductSubCategory3.ProductSubCategory2.Name,
                    ProdCategorySub3Id = x.ProductSubCategory3Id,
                    ProdCategorySub3Name = x.ProductSubCategory3.Name,
                    PreparationTime = x.PreparationTime,
                    EightHourSampleSize = x.SampleSize8h
                });
        }

        /// <summary>
        /// get the work load matrix data by prodCatSub3 Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WorkLoadMatrixData> GetWorkLoadMatrixByProductCatSub3Id(int prodCatSub3Id)
        {
            return await _context.QuWorkLoadMatrices.Where(x => x.Active.Value && x.ProductSubCategory3Id == prodCatSub3Id)
                .Select(x => new WorkLoadMatrixData
                {
                    ProdCategorySub3Id = x.ProductSubCategory3Id,
                    PreparationTime = x.PreparationTime,
                    EightHourSampleSize = x.SampleSize8h
                }).FirstOrDefaultAsync();
        }

        public async Task<List<WorkLoadMatrixData>> GetWorkLoadMatrixByProductCatSub3List(List<int> prodCatSub3List)
        {
            return await _context.QuWorkLoadMatrices.Where(x => x.Active.Value && x.ProductSubCategory3Id != null
                 && prodCatSub3List.Contains(x.ProductSubCategory3Id.Value))
                .Select(x => new WorkLoadMatrixData
                {
                    ProdCategorySub3Id = x.ProductSubCategory3Id,
                    PreparationTime = x.PreparationTime,
                    EightHourSampleSize = x.SampleSize8h
                }).AsNoTracking().ToListAsync();
        }
    }
}
