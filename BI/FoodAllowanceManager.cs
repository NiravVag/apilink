using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Expense;
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
    public class FoodAllowanceManager : IFoodAllowanceManager
    {
        private readonly IFoodAllowanceRepository _repo = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly FoodAllowanceMap _foodAllowanceMap = null;
        private readonly ITenantProvider _filterService = null;

        public FoodAllowanceManager(IFoodAllowanceRepository repo, IAPIUserContext applicationContext, ITenantProvider filterService)
        {
            _repo = repo;
            _applicationContext = applicationContext;
            _foodAllowanceMap = new FoodAllowanceMap();
            _filterService = filterService;
        }
        public async Task<SaveResponse> Save(FoodAllowance request)
        {
            var response = new SaveResponse();

            if (await _repo.CheckFoodAllowanceExist(request.Id, request.CountryId, request.StartDate.ToDateTime(), request.EndDate.ToDateTime()))
            {
                response.Result = FoodAllowanceResult.AlreadyExists;
                return response;
            }
            else
            {
                var entity = new EcFoodAllowance
                {
                    CountryId = request.CountryId,
                    StartDate = request.StartDate.ToDateTime(),
                    EndDate = request.EndDate.ToDateTime(),
                    FoodAllowance = (decimal)request.FoodAllowanceValue,
                    CurrencyId = request.CurrencyId,
                    UserId = _applicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    Active = true,
                    EntityId = _filterService.GetCompanyId()
                };

                _repo.Save(entity, false);
                response.Result = FoodAllowanceResult.Success;
            }
            return response;
        }

        public async Task<SaveResponse> Update(FoodAllowance request)
        {
            var response = new SaveResponse();

            if (await _repo.CheckFoodAllowanceExist(request.Id, request.CountryId, request.StartDate.ToDateTime(), request.EndDate.ToDateTime()))
            {
                response.Result = FoodAllowanceResult.AlreadyExists;
                return response;
            }

            var data = await _repo.GetFoodAllowance(request.Id);

            if (data != null)
            {
                data.CountryId = request.CountryId;
                data.StartDate = request.StartDate.ToDateTime();
                data.EndDate = request.EndDate.ToDateTime();
                data.FoodAllowance = (decimal)request.FoodAllowanceValue;
                data.CurrencyId = request.CurrencyId;
                data.UpdatedBy = _applicationContext.UserId;
                data.UpdatedOn = DateTime.Now;

                // update if any auto qc food expense is not configured in this date range.

                var foodAutoExpenseList = await _repo.GetAutoQcFoodExpenseList(data.CountryId, data.StartDate, data.EndDate);

                if (foodAutoExpenseList.Any())
                {
                    foreach (var expense in foodAutoExpenseList)
                    {
                        expense.IsFoodAllowanceConfigured = true;
                        expense.FoodAllowanceCurrency = data.CurrencyId;
                        expense.FoodAllowance = (double)request.FoodAllowanceValue;
                    }
                    _repo.EditEntities(foodAutoExpenseList);
                    await _repo.Save();

                }
                _repo.Save(data, true);
                response.Result = FoodAllowanceResult.Success;



            }
            else
            {
                response.Result = FoodAllowanceResult.NotFound;
            }

            return response;
        }

        public async Task<SaveResponse> Delete(int id)
        {
            var data = await _repo.GetFoodAllowance(id);

            if (data != null)
            {
                data.DeletedBy = _applicationContext.UserId;
                data.DeletedOn = DateTime.Now;
                data.Active = false;
            }

            _repo.Save(data, true);
            return new SaveResponse { Result = FoodAllowanceResult.Success };

        }

        public async Task<FoodAllowanceSummaryResponse> GetFoodAllowanceSummary(FoodAllowanceSummaryRequest request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;
            var data = _repo.GetFoodAllowanceSummary();

            if (request.CountryId > 0)
                data = data.Where(x => x.CountryId == request.CountryId);

            if (request.StartDate != null && request.EndDate != null)
            {
                data = data.Where(x => !((x.StartDate > request.EndDate.ToDateTime()) || (x.EndDate < request.StartDate.ToDateTime())));
            }

            if (data == null || !data.Any())
            {
                return new FoodAllowanceSummaryResponse { Result = FoodAllowanceResult.NotFound };
            }

            var totalCount = await data.CountAsync();

            var res = await data.AsNoTracking().Skip(skip).Take(take).ToListAsync();

            var hasItRole = _applicationContext.RoleList.Any(x => x == (int)RoleEnum.IT_Team);

            var result = res.Select(x => _foodAllowanceMap.MapFoodAllowanceSummary(x, hasItRole));

            return new FoodAllowanceSummaryResponse
            {
                Data = result.ToList(),
                Result = FoodAllowanceResult.Success,
                PageCount = (totalCount / request.PageSize.Value) + (totalCount % request.PageSize.Value > 0 ? 1 : 0),
                PageSize = request.PageSize.GetValueOrDefault(),
                TotalCount = totalCount,
                Index = request.Index.GetValueOrDefault()
            };
        }

        public async Task<FoodAllowanceEditResponse> EditFoodAllowance(int id)
        {
            var data = _repo.GetFoodAllowanceSummary();

            data = data.Where(x => x.Id == id);

            if (data == null || !data.Any())
            {
                return new FoodAllowanceEditResponse { Result = FoodAllowanceResult.NotFound };
            }

            var res = await data.AsNoTracking().ToListAsync();

            var result = res.Select(x => _foodAllowanceMap.MapFoodAllowanceEdit(x));

            return new FoodAllowanceEditResponse
            {
                Data = result.ToList(),
                Result = FoodAllowanceResult.Success
            };
        }
    }
}
