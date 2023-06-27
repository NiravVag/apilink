using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.TravelTariff;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class TravelTariffManager : ApiCommonData, ITravelTariffManager
    {
        private readonly ITravelTariffRepository _repo = null;
        public readonly TravelTariffMap _travelRequestMap = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IUserRightsManager _userRightsManager = null;

        public TravelTariffManager(ITravelTariffRepository repo, IAPIUserContext applicationContext,
            ITenantProvider filterService, IUserRightsManager userRightsManager)
        {
            _repo = repo;
            _travelRequestMap = new TravelTariffMap();
            _applicationContext = applicationContext;
            _filterService = filterService;
            _userRightsManager = userRightsManager;
        }

        public async Task<TravelTariffSaveResponse> SaveTravelTariff(TravelTariffSaveRequest request)
        {
            try
            {
                var response = new TravelTariffSaveResponse();

                if (request.Id == 0)
                {
                    if (!await _repo.CheckTravelTarifExist(request))
                    {
                        var travelTariffEntity = _travelRequestMap.TravelTariffSaveRequestMap(
                                                  request, _applicationContext.UserId, _filterService.GetCompanyId());

                        _repo.AddEntity(travelTariffEntity);

                        await _repo.Save();

                        response.Result = TravelTariffResponseResult.Success;
                    }
                    else
                    {
                        response.Result = TravelTariffResponseResult.AllreadyExist;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<TravelTariffGetAllResponse> GetAllTravelTariffDetails(TravelTariffSearchRequest request)
        {
            var response = new TravelTariffGetAllResponse();

            if (request == null)
                return new TravelTariffGetAllResponse() { Result = TravelTariffGetAllResult.NotFound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value <= 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            var travelTariffs = _repo.GetTravelTariffList();

            if (request.StartDate != null && request.EndDate != null)
            {
                travelTariffs = travelTariffs.Where(x => !((x.StartDate > request.EndDate.ToDateTime()) || (x.EndDate < request.StartDate.ToDateTime())));
            }

            else if (request.StartDate != null && request.EndDate == null)
            {
                travelTariffs = travelTariffs.Where(x => x.StartDate >= request.StartDate.ToDateTime());
            }

            else if (request.StartDate == null && request.EndDate != null)
            {
                travelTariffs = travelTariffs.Where(x => x.EndDate <= request.EndDate.ToDateTime());
            }

            if (request.StartPort > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.StartPort == request.StartPort);
            }

            if (request.CountryId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.Town.County.City.Province.CountryId == request.CountryId);
            }

            if (request.ProvinceId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.Town.County.City.ProvinceId == request.ProvinceId);
            }

            if (request.CityId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.Town.County.CityId == request.CityId);
            }

            if (request.CountyId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.Town.CountyId == request.CountyId);
            }

            if (request.TownId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.TownId == request.TownId);
            }

            if (request.Status != null)
            {
                travelTariffs = travelTariffs.Where(x => x.Status == request.Status);
            }

            var totalCount = await travelTariffs.CountAsync();

            response.TravelTariffDetails = await travelTariffs.Skip(skip).Take(take)
                .Select(x => new TravelTariffSummaryData()
                {
                    Id = x.Id,
                    StartPortName = x.StartPortNavigation.StartPortName,
                    TownName = x.Town.TownName,
                    CountryName = x.Town.County.City.Province.Country.CountryName,
                    CityName = x.Town.County.City.CityName,
                    ProvinceName = x.Town.County.City.Province.ProvinceName,
                    CountyName = x.Town.County.CountyName,
                    StartDate = x.StartDate.Value.ToString(StandardDateFormat),
                    EndDate = x.EndDate.Value.ToString(StandardDateFormat),
                    TravelCurrency = x.TravelCurrencyNavigation.CurrencyName,
                    TravelTariff = x.TravelTariff,
                    Status = x.Status
                })
                .ToListAsync();
            response.TotalCount = totalCount;
            response.Index = request.Index.Value;
            response.PageSize = request.PageSize.Value;
            response.PageCount = (totalCount / request.PageSize.Value) + (totalCount % request.PageSize.Value > 0 ? 1 : 0);
            response.Result = TravelTariffGetAllResult.Success;
            return response;
        }

        public async Task<TravelTariffGetAllResponse> GetExportTravelTariffDetails(TravelTariffSearchRequest request)
        {
            var response = new TravelTariffGetAllResponse();

            if (request == null)
                return new TravelTariffGetAllResponse() { Result = TravelTariffGetAllResult.NotFound };


            var travelTariffs = _repo.GetTravelTariffList();

            if (request.StartPort > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.StartPort == request.StartPort);
            }

            if (request.StartDate != null && request.EndDate != null)
            {
                travelTariffs = travelTariffs.Where(x => !((x.StartDate > request.EndDate.ToDateTime()) || (x.EndDate < request.StartDate.ToDateTime())));
            }
            if (request.CountryId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.Town.County.City.Province.CountryId == request.CountryId);
            }

            if (request.ProvinceId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.Town.County.City.ProvinceId == request.ProvinceId);
            }

            if (request.CityId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.Town.County.CityId == request.CityId);
            }

            if (request.CountyId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.Town.CountyId == request.CountyId);
            }

            if (request.TownId > 0)
            {
                travelTariffs = travelTariffs.Where(x => x.TownId == request.TownId);
            }

            if (request.Status != null)
            {
                travelTariffs = travelTariffs.Where(x => x.Status == request.Status);
            }

            var totalCount = await travelTariffs.CountAsync();

            response.TravelTariffDetails = await travelTariffs
                .Select(x => new TravelTariffSummaryData()
                {
                    Id = x.Id,
                    StartPortName = x.StartPortNavigation.StartPortName,
                    TownName = x.Town.TownName,
                    CountryName = x.Town.County.City.Province.Country.CountryName,
                    CityName = x.Town.County.City.CityName,
                    ProvinceName = x.Town.County.City.Province.ProvinceName,
                    CountyName = x.Town.County.CountyName,
                    StartDate_Date = x.StartDate,
                    EndDate_Date = x.EndDate,
                    TravelCurrency = x.TravelCurrencyNavigation.CurrencyName,
                    TravelTariff = x.TravelTariff,
                    Status = x.Status
                })
                .ToListAsync();
            response.TotalCount = totalCount;
            response.Result = TravelTariffGetAllResult.Success;
            return response;
        }


        /// <summary>
        /// Update travel tariff details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TravelTariffSaveResponse> UpdateTravelTariff(TravelTariffSaveRequest request)
        {
            try
            {
                var response = new TravelTariffSaveResponse();

                if (request == null)
                {
                    return new TravelTariffSaveResponse() { Result = TravelTariffResponseResult.RequestNotCorrectFormat };
                }

                if (request.Id > 0)
                {

                    var travelTariff = await _repo.GetTravelTariff(request.Id);

                    if (travelTariff == null)
                    {
                        return new TravelTariffSaveResponse() { Result = TravelTariffResponseResult.NotFound };
                    }
                    else
                    {
                        if (!await _repo.CheckTravelTarifExist(request))
                        {
                            _travelRequestMap.TravelTariffUpdateRequestMap(request, _applicationContext.UserId, travelTariff);

                            // update if any auto qc travel expense is not configured in this date range.

                            var travelAutoExpenseList = await _repo.GetAutoQcFoodExpenseList(travelTariff.TownId, request.StartDate.ToDateTime(), request.EndDate.ToDateTime());

                            if (travelAutoExpenseList.Any())
                            {
                                foreach (var expense in travelAutoExpenseList)
                                {
                                    expense.IsTravelAllowanceConfigured = true;
                                    expense.TravelTariffCurrency = travelTariff.TravelCurrency;
                                    expense.TravelTariff = (double)travelTariff.TravelTariff;
                                }
                                _repo.EditEntities(travelAutoExpenseList);
                                await _repo.Save();
                            }




                            _repo.Save(travelTariff, true);
                            // update travel tariff task
                            await _userRightsManager.UpdateTask(travelTariff.Id, new[] { (int)TaskType.TravelTariffUpdate }, false);
                            response.Result = TravelTariffResponseResult.Success;
                        }
                        else
                        {
                            response.Result = TravelTariffResponseResult.AllreadyExist;
                        }
                    }
                }
                else
                {
                    return new TravelTariffSaveResponse() { Result = TravelTariffResponseResult.RequestNotCorrectFormat };
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get travel tariff details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<TravelTariffGetResponse> GetTravelTariffDetails(int Id)
        {
            var response = new TravelTariffGetResponse();
            try
            {

                var travelTariff = _repo.GetTravelTariffData(Id);

                var travelTariffData = await travelTariff.Select(x => new TravelTariffData
                {
                    Id = x.Id,
                    StartPortId = x.StartPort,
                    StartDate = Static_Data_Common.GetCustomDate(x.StartDate),
                    EndDate = Static_Data_Common.GetCustomDate(x.EndDate),
                    TownId = x.TownId,
                    TravelCurrency = x.TravelCurrency,
                    TravelTariff = x.TravelTariff,
                    CountyId = x.Town.CountyId,
                    CityId = x.Town.County.CityId,
                    CountryId = x.Town.County.City.Province.CountryId,
                    ProvinceId = x.Town.County.City.ProvinceId,
                }).FirstOrDefaultAsync();

                if (travelTariff == null || !travelTariff.Any())
                    response.Result = TravelTariffGetResult.NotFound;
                else
                {
                    response.TravelTariff = travelTariffData;
                    response.Result = TravelTariffGetResult.Success;
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Delete travel tariff details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<TravelTariffDeleteResponse> DeleteTravelTariff(int Id)
        {
            try
            {
                if (Id > 0)
                {
                    var travelTariff = await _repo.GetTravelTariff(Id);

                    if (travelTariff == null)
                    {
                        return new TravelTariffDeleteResponse() { Result = TravelTariffDeleteResult.NotFound };
                    }
                    else
                    {
                        travelTariff.Active = false;
                        travelTariff.DeletedOn = DateTime.Now;
                        travelTariff.DeletedBy = _applicationContext.UserId;
                        _repo.EditEntity(travelTariff);
                        await _repo.Save();
                    }

                    return new TravelTariffDeleteResponse { Id = Id, Result = TravelTariffDeleteResult.Success };
                }

                return new TravelTariffDeleteResponse { Id = Id, Result = TravelTariffDeleteResult.RequestNotCorrect };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataSourceResponse> GetStartPotList()
        {
            var data = await _repo.GetStartPotList();

            if (data != null && data.Any())
            {
                return new DataSourceResponse()
                {
                    DataSourceList = data,
                    Result = DataSourceResult.Success
                };
            }
            else
            {
                return new DataSourceResponse()
                {
                    DataSourceList = null,
                    Result = DataSourceResult.CannotGetList
                };
            }
        }
    }
}
