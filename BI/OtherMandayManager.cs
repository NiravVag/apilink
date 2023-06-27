using System.Threading.Tasks;
using Contracts.Managers;
using DTO.OtherManday;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;
using Entities;
using DTO.Common;
using BI.Maps;
using Microsoft.EntityFrameworkCore;
using DTO.CommonClass;

namespace BI
{
    public class OtherMandayManager : IOtherMandayManager
    {
        private readonly IOtherMandayRepository _repo = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly OtherMandayMap _map = null;

        public OtherMandayManager(IOtherMandayRepository repo, ITenantProvider filterService, IAPIUserContext ApplicationContext)
        {
            _repo = repo;
            _filterService = filterService;
            _ApplicationContext = ApplicationContext;
            _map = new OtherMandayMap();
        }
        public async Task<SaveOtherMandayResponse> SaveOtherManday(SaveOtherMandayRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new SaveOtherMandayResponse { Result = OtherMandayResult.RequestNotCorrectFormat };
                }

                if (await _repo.CheckIfOtherMandayAlreadyExists(request))
                {
                    return new SaveOtherMandayResponse { Result = OtherMandayResult.AlreadyExists };
                }

                var entityId = _filterService.GetCompanyId();

                OmDetail entity = new OmDetail()
                {
                    CustomerId = request.CustomerId > 0 ? request.CustomerId : null,
                    OfficeCountryId = request.OfficeCountryId,
                    QcId = request.QcId,
                    OperationalCountryId = request.OperationalCountryId,
                    PurposeId = request.PurposeId,
                    ServiceDate = request.ServiceDate.ToDateTime(),
                    ManDay = request.Manday,
                    Remarks = request.Remarks,
                    Active = true,
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    EntityId = entityId
                };

                _repo.Save(entity, false);

                return new SaveOtherMandayResponse
                {
                    Result = OtherMandayResult.Success
                };
            }
            catch (Exception ex)
            {
                return new SaveOtherMandayResponse { Result = OtherMandayResult.Failure };
            }
        }
        public async Task<SaveOtherMandayResponse> UpdateOtherManday(SaveOtherMandayRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new SaveOtherMandayResponse { Result = OtherMandayResult.RequestNotCorrectFormat };
                }
                //we can update manday and remarks
                //if (await _repo.CheckIfOtherMandayAlreadyExists(request))
                //{
                //    return new SaveOtherMandayResponse { Result = OtherMandayResult.AlreadyExists };
                //}

                var entity = await _repo.GetOtherMandayDataById(request.Id);

                if (entity == null)
                {
                    return new SaveOtherMandayResponse { Result = OtherMandayResult.NotFound };
                };

                entity.CustomerId = request.CustomerId > 0 ? request.CustomerId : null;
                entity.OfficeCountryId = request.OfficeCountryId;
                entity.OperationalCountryId = request.OperationalCountryId;
                entity.PurposeId = request.PurposeId;
                entity.ManDay = request.Manday;
                entity.Remarks = request.Remarks;
                entity.QcId = request.QcId;
                entity.ServiceDate = request.ServiceDate.ToDateTime();
                entity.UpdatedBy = _ApplicationContext.UserId;
                entity.UpdatedOn = DateTime.Now;

                _repo.Save(entity, true);

                return new SaveOtherMandayResponse
                {
                    Result = OtherMandayResult.Success
                };
            }
            catch (Exception ex)
            {
                return new SaveOtherMandayResponse { Result = OtherMandayResult.Failure };
            }
        }
        public async Task<DeleteOtherMandayResponse> DeleteOtherManday(int id)
        {
            try
            {
                if (!(id > 0))
                {
                    return new DeleteOtherMandayResponse { Result = OtherMandayResult.RequestNotCorrectFormat };
                }

                var entity = await _repo.GetOtherMandayDataById(id);

                if (entity == null)
                {
                    return new DeleteOtherMandayResponse { Result = OtherMandayResult.NotFound };
                };

                entity.Active = false;
                entity.DeletedBy = _ApplicationContext.UserId;
                entity.DeletedOn = DateTime.Now;

                _repo.Save(entity, true);

                return new DeleteOtherMandayResponse
                {
                    Result = OtherMandayResult.Success
                };
            }
            catch (Exception ex)
            {
                return new DeleteOtherMandayResponse { Result = OtherMandayResult.Failure };
            }
        }
        public async Task<EditOtherMandayResponse> GetEditOtherManday(int id)
        {
            if (!(id > 0))
            {
                return new EditOtherMandayResponse { Result = OtherMandayResult.RequestNotCorrectFormat };
            }

            var entity = await _repo.GetOtherMandayEditDataById(id);

            if (entity == null)
            {
                return new EditOtherMandayResponse { Result = OtherMandayResult.NotFound };
            };

            var data = _map.MapOtherMandayData(entity);

            return new EditOtherMandayResponse
            {
                Data = data,
                Result = OtherMandayResult.Success
            };
        }
        public async Task<OtherMandaySummaryResponse> GetOtherMandaySummary(OtherMandaySummaryRequest request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            if (request == null)
            {
                return new OtherMandaySummaryResponse { Result = OtherMandayResult.RequestNotCorrectFormat };
            }

            var data = _repo.GetOtherMandayByEfCore();

            if (request.CustomerId > 0)
            {
                data = data.Where(x => x.CustomerId == request.CustomerId);
            }
            if (request.OfficeCountryId > 0)
            {
                data = data.Where(x => x.OfficeCountryId == request.OfficeCountryId);
            }
            if (request.OperationalCountryId > 0)
            {
                data = data.Where(x => x.OperationalCountryId == request.OperationalCountryId);
            }
            if ((request.ServiceFromDate?.ToDateTime() != null && request.ServiceToDate?.ToDateTime() != null))
            {
                data = data.Where(x => !((x.ServiceDate > request.ServiceToDate.ToDateTime()) || (x.ServiceDate < request.ServiceFromDate.ToDateTime())));
            }
            if (request.QcId > 0)
            {
                data = data.Where(x => x.QcId == request.QcId);
            }

            if (data == null || !data.Any())
            {
                return new OtherMandaySummaryResponse { Result = OtherMandayResult.NotFound };
            }

            var totalCount = await data.CountAsync();

            var result = await data.Skip(skip).Take(take).OrderBy(x => x.CustomerId).AsNoTracking().ToListAsync();

            var entity = result.Select(x => _map.MapOtherMandayData(x));

            return new OtherMandaySummaryResponse
            {
                Data = entity.ToList(),
                Result = OtherMandayResult.Success,
                PageCount = (totalCount / request.PageSize.Value) + (totalCount % request.PageSize.Value > 0 ? 1 : 0),
                PageSize = request.PageSize.GetValueOrDefault(),
                TotalCount = totalCount,
                Index = request.Index.GetValueOrDefault()
            };
        }

        public async Task<DataSourceResponse> GetPurposeList()
        {
            var data = await _repo.GetPurposeList();

            if (data == null || !data.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }

        public async Task<List<ExportOtherMandayData>> ExportOtherMandaySummary(OtherMandaySummaryRequest request)
        {
            if (request == null)
            {
                return null;
            }

            var data = _repo.GetOtherMandayByEfCore();

            if (request.CustomerId > 0)
            {
                data = data.Where(x => x.CustomerId == request.CustomerId);
            }
            if (request.OfficeCountryId > 0)
            {
                data = data.Where(x => x.OfficeCountryId == request.OfficeCountryId);
            }
            if (request.OperationalCountryId > 0)
            {
                data = data.Where(x => x.OperationalCountryId == request.OperationalCountryId);
            }
            if ((request.ServiceFromDate?.ToDateTime() != null && request.ServiceToDate?.ToDateTime() != null))
            {
                data = data.Where(x => !((x.ServiceDate > request.ServiceToDate.ToDateTime()) || (x.ServiceDate < request.ServiceFromDate.ToDateTime())));
            }
            if (request.QcId > 0)
            {
                data = data.Where(x => x.QcId == request.QcId);
            }

            if (data == null || !data.Any())
            {
                return null;
            }

            var totalCount = await data.CountAsync();

            var result = await data.OrderBy(x => x.CustomerId).AsNoTracking().ToListAsync();

            var entity = result.Select(x => _map.MapExportOtherMandayData(x));

            return entity.ToList();
        }
    }
}
