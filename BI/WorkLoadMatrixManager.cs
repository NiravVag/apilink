using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.WorkLoadMatrix;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class WorkLoadMatrixManager: IWorkLoadMatrixManager
    {
        private readonly IWorkLoadMatrixRepository _repo = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IProductManagementRepository _prodMgmtRepo = null;

        public WorkLoadMatrixManager(IWorkLoadMatrixRepository repo, ITenantProvider filterService, IAPIUserContext ApplicationContext,
            IProductManagementRepository prodMgmtRepo)
        {
            _repo = repo;
            _filterService = filterService;
            _ApplicationContext = ApplicationContext;
            _prodMgmtRepo = prodMgmtRepo;
        }
        public async Task<SaveWorkLoadMatrixResponse> SaveWorkLoadMatrix(SaveWorkLoadMatrixRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new SaveWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.RequestNotCorrectFormat };
                }

                if (await _repo.CheckIfWorkLoadMatrixAlreadyExists(request.ProductSubCategory3Id))
                {
                    return new SaveWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.AlreadyExists };
                }

                if (request.ProductSubCategory3Id > 0)
                {
                    var entityId = _filterService.GetCompanyId();

                    QuWorkLoadMatrix entity = new QuWorkLoadMatrix()
                    {
                        ProductSubCategory3Id = request.ProductSubCategory3Id,
                        PreparationTime = request.PreparationTime,
                        SampleSize8h = request.EightHourSampleSize,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = entityId
                    };

                    _repo.Save(entity, false);
                }

                return new SaveWorkLoadMatrixResponse
                {
                    Result = WorkLoadMatrixResult.Success
                };
            }
            catch (Exception ex)
            {
                return new SaveWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.Failure };
            }
        }

        public async Task<SaveWorkLoadMatrixResponse> UpdateWorkLoadMatrix(SaveWorkLoadMatrixRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new SaveWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.RequestNotCorrectFormat };
                }

                if (await _repo.CheckIfWorkLoadMatrixAlreadyExists(request.ProductSubCategory3Id, request.Id))
                {
                    return new SaveWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.AlreadyExists };
                }

                var entity = await _repo.GetWorkLoadMatrixById(request.Id);

                if (entity == null)
                {
                    return new SaveWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.NotFound };
                };

                entity.ProductSubCategory3Id = request.ProductSubCategory3Id;
                entity.PreparationTime = request.PreparationTime;
                entity.SampleSize8h = request.EightHourSampleSize;
                entity.UpdatedBy = _ApplicationContext.UserId;
                entity.UpdatedOn = DateTime.Now;

                _repo.Save(entity, true);

                return new SaveWorkLoadMatrixResponse
                {
                    Result = WorkLoadMatrixResult.Success
                };
            }
            catch (Exception ex)
            {
                return new SaveWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.Failure };
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DeleteWorkLoadMatrixResponse> DeleteWorkLoadMatrix(int id)
        {
            try
            {
                if (!(id > 0))
                {
                    return new DeleteWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.RequestNotCorrectFormat };
                }

                var entity = await _repo.GetWorkLoadMatrixById(id);

                if (entity == null)
                {
                    return new DeleteWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.NotFound };
                };

                entity.Active = false;
                entity.DeletedBy = _ApplicationContext.UserId;
                entity.DeletedOn = DateTime.Now;

                _repo.Save(entity, true);

                return new DeleteWorkLoadMatrixResponse
                {
                    Result = WorkLoadMatrixResult.Success
                };
            }
            catch (Exception ex)
            {
                return new DeleteWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.Failure };
            }
        }

        /// <summary>
        /// Get ProdSubCategory3 by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditWorkLoadMatrixResponse> GetEditWorkLoadMatrix(int id, bool workLoadMatrixNotConfigured)
        {
            if (!(id > 0))
            {
                return new EditWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.RequestNotCorrectFormat };
            }

            //workLoadMatrixNotConfigured true - fetch from Product SubCat3 table, if false fetch from workloadmatrix table
            var entity = workLoadMatrixNotConfigured ? await _repo.GetProductCategorySub3ById(id) : await _repo.GetWorkLoadMatrixEditDataById(id);

            if (entity == null)
            {
                return new EditWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.NotFound };
            };

            return new EditWorkLoadMatrixResponse
            {
                Data = entity,
                Result = WorkLoadMatrixResult.Success
            };
        }
        public async Task<WorkLoadMatrixSummaryResponse> GetWorkLoadMatrixSummary(WorkLoadMatrixSummaryRequest request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            if (request == null)
            {
                return new WorkLoadMatrixSummaryResponse { Result = WorkLoadMatrixResult.RequestNotCorrectFormat };
            }

            var data = request.WorkLoadMatrixNotConfigured ? _prodMgmtRepo.GetProdSubCat3WorkLoadNotConfiguredByEfCore() : _repo.GetWorkLoadMatrixByEfCore();

            if (request.ProductCategoryId > 0)
            {
                data = data.Where(x => x.ProdCategoryId == request.ProductCategoryId);
            }
            if (request.ProductSubCategoryId > 0)
            {
                data = data.Where(x => x.ProdSubCategoryId == request.ProductSubCategoryId);
            }
            if (request.ProductCategorySub2IdList != null && request.ProductCategorySub2IdList.Any())
            {
                data = data.Where(x => request.ProductCategorySub2IdList.Contains(x.ProdCategorySub2Id));
            }
            if (request.ProductCategorySub3IdList != null && request.ProductCategorySub3IdList.Any())
            {
                data = data.Where(x => request.ProductCategorySub3IdList.Contains(x.ProdCategorySub3Id));
            }

            if (data == null || !data.Any())
            {
                return new WorkLoadMatrixSummaryResponse { Result = WorkLoadMatrixResult.NotFound };
            }

            var totalCount = await data.CountAsync();

            var result = await data.Skip(skip).Take(take).OrderBy(x => x.ProdCategoryId).AsNoTracking().ToListAsync();

            return new WorkLoadMatrixSummaryResponse
            {
                Data = result,
                HasITRole = _ApplicationContext.RoleList.ToList().Contains((int)RoleEnum.IT_Team),
                Result = WorkLoadMatrixResult.Success,
                PageCount = (totalCount / request.PageSize.Value) + (totalCount % request.PageSize.Value > 0 ? 1 : 0),
                PageSize = request.PageSize.GetValueOrDefault(),
                TotalCount = totalCount,
                Index = request.Index.GetValueOrDefault()
            };
        }

        /// <summary>
        /// Get ProdSubCategory3 by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditWorkLoadMatrixResponse> GettWorkLoadMatrixDataByProdCatSub3Id(int prodCatSub3Id)
        {
            if (!(prodCatSub3Id > 0))
            {
                return new EditWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.RequestNotCorrectFormat };
            }

            //workLoadMatrixNotConfigured true - fetch from Product SubCat3 table, if false fetch from workloadmatrix table
            var entity = await _repo.GetWorkLoadMatrixByProductCatSub3Id(prodCatSub3Id);

            if (entity == null)
            {
                return new EditWorkLoadMatrixResponse { Result = WorkLoadMatrixResult.NotFound };
            };

            return new EditWorkLoadMatrixResponse
            {
                Data = entity,
                Result = WorkLoadMatrixResult.Success
            };
        }

        public async Task<List<ExportWorkLoadMatrixData>> ExportWorkLoadMatrixSummary(WorkLoadMatrixSummaryRequest request)
        {
            if (request == null)
            {
                return null;
            }

            var data = request.WorkLoadMatrixNotConfigured ? _prodMgmtRepo.GetProdSubCat3WorkLoadNotConfiguredByEfCore() : _repo.GetWorkLoadMatrixByEfCore();

            if (request.ProductCategoryId > 0)
            {
                data = data.Where(x => x.ProdCategoryId == request.ProductCategoryId);
            }
            if (request.ProductSubCategoryId > 0)
            {
                data = data.Where(x => x.ProdSubCategoryId == request.ProductSubCategoryId);
            }
            if (request.ProductCategorySub2IdList != null && request.ProductCategorySub2IdList.Any())
            {
                data = data.Where(x => request.ProductCategorySub2IdList.Contains(x.ProdCategorySub2Id));
            }
            if (request.ProductCategorySub3IdList != null && request.ProductCategorySub3IdList.Any())
            {
                data = data.Where(x => request.ProductCategorySub3IdList.Contains(x.ProdCategorySub3Id));
            }

            if (data == null || !data.Any())
            {
                return null;
            }

            //var totalCount = await data.CountAsync();

            var result = await data.AsNoTracking().ToListAsync();

            return result.ConvertAll(x => new ExportWorkLoadMatrixData
            {
                ProdCategoryName = x.ProdCategoryName,
                ProdSubCategoryName = x.ProdSubCategoryName,
                ProdCategorySub2Name = x.ProdCategorySub2Name,
                ProdCategorySub3Name = x.ProdCategorySub3Name,
                PreparationTime = x.PreparationTime,
                EightHourSampleSize = x.EightHourSampleSize
            }).ToList();
        }
    }
}
