using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.StartingPort;
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
    public class StartingPortManager: IStartingPortManager
    {
        private readonly IStartingPortRepository _repo = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly StartingPortMap _startingPortMap = null;
        private readonly ITenantProvider _filterService = null;
        public StartingPortManager(IStartingPortRepository repo, IAPIUserContext applicationContext, ITenantProvider filterService)
        {
            _repo = repo;
            _applicationContext = applicationContext;
            _startingPortMap = new StartingPortMap();
            _filterService = filterService;
        }

        public async Task<StartingPortSaveResponse> SaveStartingPort(StartingPortRequest request)
        {
            try
            {
                var response = new StartingPortSaveResponse();

                if (request == null)
                {
                    return new StartingPortSaveResponse() { Result = StartingPortResult.RequestNotCorrectFormat };
                }

                if (request.Id == 0)
                {
                    if (await _repo.CheckifStartingPortDataExistsByName(request.Id, request.StartPortName))
                    {
                        response.Result = StartingPortResult.AlreadyExists;
                    }
                    else
                    {
                        var startPortEntity = _startingPortMap.MapSaveRequest(request, _applicationContext.UserId, _filterService.GetCompanyId());

                        _repo.AddEntity(startPortEntity);

                        await _repo.Save();

                        response.Result = StartingPortResult.Success;

                    }
                }

                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StartingPortSaveResponse> UpdateStartingPort(StartingPortRequest request)
        {
            try
            {
                var response = new StartingPortSaveResponse();

                if (request == null)
                {
                    return new StartingPortSaveResponse() { Result = StartingPortResult.RequestNotCorrectFormat };
                }

                if (request.Id > 0)
                {

                    if (await _repo.CheckifStartingPortDataExistsByName(request.Id, request.StartPortName))
                    {
                        response.Result = StartingPortResult.AlreadyExists;
                    }
                    else
                    {
                        var startingPortEntity = await _repo.GetStartingPortDataById(request.Id);

                        if (startingPortEntity == null)
                        {
                            return new StartingPortSaveResponse() { Result = StartingPortResult.NotFound };
                        }

                        var startPortEntity = _startingPortMap.MapUpdateRequest(startingPortEntity, request, _applicationContext.UserId);

                        _repo.Save(startPortEntity, true);

                        response.Result = StartingPortResult.Success;

                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StartingPortSaveResponse> DeleteStartingPort(int Id)
        {
            try
            {
                if (Id > 0)
                {
                    var startingPortEntity = await _repo.GetStartingPortDataById(Id);

                    if (startingPortEntity == null)
                    {
                        return new StartingPortSaveResponse() { Result = StartingPortResult.NotFound };
                    }
                    else
                    {
                        startingPortEntity.Active = false;
                        startingPortEntity.DeletedOn = DateTime.Now;
                        startingPortEntity.DeletedBy = _applicationContext.UserId;
                        _repo.EditEntity(startingPortEntity);
                        await _repo.Save();
                    }

                    return new StartingPortSaveResponse { Id = Id, Result = StartingPortResult.Success };
                }

                return new StartingPortSaveResponse { Id = Id, Result = StartingPortResult.RequestNotCorrectFormat };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StartingPortResponse> GetStartingPortSummary(StartingPortSearchRequest request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            var data = _repo.GetAllStartingPort();

            if (request.StartingPortId > 0)
                data = data.Where(x => x.Id == request.StartingPortId);

            if (data == null || !data.Any())
            {
                return new StartingPortResponse { Result = StartingPortResult.NotFound };
            }

            var totalCount = await data.CountAsync();

            var hasItRole = _applicationContext.RoleList.Any(x => x == (int)RoleEnum.IT_Team);

            var summaryItem = await data.Select(x => new StartingPortSummaryData
            {
                CityId = x.CityId.GetValueOrDefault(),
                CityName = x.City.CityName,
                StartingPortId = x.Id,
                StartingPortName = x.StartPortName,
                HasItRole = hasItRole
            }).AsNoTracking().OrderBy(x => x.StartingPortId).Skip(skip).Take(take).ToListAsync();

            return new StartingPortResponse
            {
                Data = summaryItem.ToList(),
                Result = StartingPortResult.Success,
                PageCount = (totalCount / request.PageSize.Value) + (totalCount % request.PageSize.Value > 0 ? 1 : 0),
                PageSize = request.PageSize.GetValueOrDefault(),
                TotalCount = totalCount,
                Index = request.Index.GetValueOrDefault()
            };
        }

        public async Task<StartingPortEditResponse> GetStartingPortDetails(int Id)
        {
            var response = new StartingPortEditResponse();
            try
            {

                var startingPortEntity = await _repo.GetStartingPortDataById(Id);

                if (startingPortEntity == null)
                {
                    return new StartingPortEditResponse()
                    {
                        Data = null,
                        Result = StartingPortResult.NotFound
                    };
                }

                response.Data = _startingPortMap.MapStartingPortData(startingPortEntity);
                response.Result = StartingPortResult.Success;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
    }
}
