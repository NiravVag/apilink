using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Invoice;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class InvoiceDiscountManager : ApiCommonData, IInvoiceDiscountManager
    {
        private readonly IInvoiceDiscountRepository _repo = null;
        private readonly IAPIUserContext _apiUserContext = null;
        private readonly ITenantProvider _filterService=null;
        private readonly InvoiceDiscountMap _invoiceDiscountMap = null;

        public InvoiceDiscountManager(IInvoiceDiscountRepository repo, IAPIUserContext apiUserContext,ITenantProvider filterService)
        {
            _repo = repo;
            _apiUserContext = apiUserContext;
            _filterService = filterService;
            _invoiceDiscountMap = new InvoiceDiscountMap(); ;
        }

        /// <summary>
        /// get invoice discount types
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInvoiceDiscountTypes()
        {
            var invDisTypes = await _repo.GetInvoicDiscountTypes();
            return new DataSourceResponse() { Result = DataSourceResult.Success, DataSourceList = invDisTypes };
        }

        /// <summary>
        /// Get invoice discount summary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<InvoiceDiscountSearchResponse> GetInvoiceDiscountSummary(InvoiceDiscountSearchRequest request)
        {
            if (request == null)
                return new InvoiceDiscountSearchResponse() { Result = InvoiceDiscountResult.NotFound };
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;
            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;
            int take = request.PageSize.Value;

            var response = new InvoiceDiscountSearchResponse { Index = request.Index.Value, PageSize = request.PageSize.Value };
            var query = _repo.GetInvoiceDiscountSummary();
            if (request != null)
            {
                if (request.CustomerId.HasValue && request.CustomerId > 0)
                {
                    query = query.Where(x => x.CustomerId == request.CustomerId);
                }

                if (request.CountryId.HasValue && request.CountryId > 0)
                {
                    query = query.Where(x => x.InvDisTranCountries.Select(y => y.CountryId).Contains(request.CountryId.Value));
                }
                if (request.DiscountType.HasValue && request.DiscountType > 0)
                {
                    query = query.Where(x => x.DiscountType == request.DiscountType);
                }
                if (request.PeriodFrom != null)
                {
                    query = query.Where(x => x.PeriodFrom >= request.PeriodFrom.ToDateTime());
                }
                if (request.PeriodTo != null)
                {
                    query = query.Where(x => x.PeriodTo <= request.PeriodTo.ToDateTime());
                }
            }

            response.TotalCount = query.AsNoTracking().Count();
            if (response.TotalCount == 0)
            {
                response.Result = InvoiceDiscountResult.NotFound;
                return response;
            }

            var result = await query.Skip(skip).Take(take).Select(x => new InvoiceDiscountSummaryItem()
            {
                Id = x.Id,
                CreatedBy = x.CreatedByNavigation.FullName,
                Customer = x.Customer.CustomerName,
                DiscountType = x.DiscountTypeNavigation.Name,
                PeriodFrom = x.PeriodFrom.Value.ToString(StandardDateFormat),
                PeriodTo = x.PeriodTo.Value.ToString(StandardDateFormat),
                CreatedOn = x.CreatedOn.Value.ToString(StandardDateFormat),
                UpdatedBy = x.UpdatedByNavigation.FullName,
                UpdatedOn = x.UpdatedOn.Value.ToString(StandardDateFormat)
            }).AsSingleQuery().AsNoTracking().ToListAsync();

            var countries = await _repo.GetCountryByInvDisIds(result.Select(x => x.Id));
            result.ForEach(x =>
            {
                x.Country = string.Join(", ", countries.Where(y => y.DiscountId == x.Id).Select(z => z.Name));
            });

            if (result == null || !result.Any())
                return new InvoiceDiscountSearchResponse() { Result = InvoiceDiscountResult.NotFound };

            response.Data = result;
            response.PageCount = (response.TotalCount / request.PageSize.Value) + (response.TotalCount % request.PageSize.Value > 0 ? 1 : 0);
            response.Result = InvoiceDiscountResult.Success;
            return response;
        }

        /// <summary>
        /// Delete invoice discount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteInvoiceDiscountResponse> DeleteInvoiceDiscount(int id)
        {
            var data = await _repo.GetInvoiceDiscount(id);

            if (data != null)
            {
                data.DeletedBy = _apiUserContext.UserId;
                data.DeletedOn = DateTime.Now;
                data.Active = false;
            }

            _repo.Save(data, true);
            return new DeleteInvoiceDiscountResponse() { Result = InvoiceDiscountResult.Success };

        }

        /// <summary>
        /// Save Invoice Discount
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveInvoiceDiscountResponse> SaveInvoiceDiscount(SaveInvoiceDiscount request)
        {
            var userId = _apiUserContext.UserId;
            var periodFrom = request.PeriodFrom.ToDateTime();
            var periodTo = request.PeriodTo.ToDateTime();
            var entityId = _filterService.GetCompanyId();
            var isInvoiceDiscount = await _repo.CheckInvoiceDiscountPeriod(0, request.CustomerId, request.DiscountType, periodFrom, periodTo);
            if (isInvoiceDiscount)
            {
                return new SaveInvoiceDiscountResponse()
                {
                    Result = InvoiceDiscountResult.PeriodNotAvailable
                };

            }
            var entity = _invoiceDiscountMap.MapInvoiceDiscountEntity(request, userId, entityId);
            foreach (var item in request.CountryIds)
            {
                var invDisCountry = _invoiceDiscountMap.MapInvoiceDiscountCountry(item, userId);

                entity.InvDisTranCountries.Add(invDisCountry);

                _repo.AddEntity(invDisCountry);
            }

            AddInvoiceDiscountPeriods(entity, request.Limits, userId);
            _repo.Save(entity, false);
            return new SaveInvoiceDiscountResponse() { Result = InvoiceDiscountResult.Success };
        }

        /// <summary>
        /// Update Invoice Discount
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveInvoiceDiscountResponse> UpdateInvoiceDiscount(SaveInvoiceDiscount request)
        {
            var response = new SaveInvoiceDiscountResponse();

            var invoiceDiscount = await _repo.GetInvoiceDiscount(request.Id);
            if (invoiceDiscount == null)
            {
                response.Result = InvoiceDiscountResult.NotFound;
                return response;
            }
            var periodFrom = request.PeriodFrom.ToDateTime();
            var periodTo = request.PeriodTo.ToDateTime();
            var isInvoiceDiscount = await _repo.CheckInvoiceDiscountPeriod(invoiceDiscount.Id, request.CustomerId, request.DiscountType, periodFrom, periodTo);
            if (isInvoiceDiscount)
            {
                return new SaveInvoiceDiscountResponse()
                {
                    Result = InvoiceDiscountResult.PeriodNotAvailable
                };

            }
            var userId = _apiUserContext.UserId;
            //
            _invoiceDiscountMap.MapInvoiceDiscountEntity(invoiceDiscount, request, userId);
            var ids = new List<int>() { invoiceDiscount.Id };

            if (request.Limits != null && request.Limits.Any())
            {
                var invoiceDiscountPeriods = await _repo.GetInvoiceDiscountPeriods(ids);

                var invDisPeriodIds = request.Limits.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
                //Delete Limits
                var deleteInvDisPeriods = invoiceDiscountPeriods.Where(x => !invDisPeriodIds.Contains(x.Id));
                if (deleteInvDisPeriods.Any())
                {
                    deleteInvDisPeriods.ToList().ForEach(x =>
                    {
                        x.Active = false;
                        x.DeletedBy = userId;
                        x.DeletedOn = DateTime.Now;
                    });

                    _repo.EditEntities(deleteInvDisPeriods);
                }

                var newLimits = request.Limits.Where(x => x.Id <= 0);
                if (newLimits.Any())
                {
                    AddInvoiceDiscountPeriods(invoiceDiscount, newLimits, userId);
                }


                var lstLimitToEdit = new List<InvDisTranPeriodInfo>();
                foreach (var item in request.Limits.Where(x => x.Id > 0))
                {
                    var invDisPeriod = invoiceDiscountPeriods.FirstOrDefault(x => x.Id == item.Id);

                    if (invDisPeriod != null)
                    {
                        _invoiceDiscountMap.MapInvDisTranPeriod(invDisPeriod, item);
                        lstLimitToEdit.Add(invDisPeriod);
                    }
                    else
                        return new SaveInvoiceDiscountResponse { Result = InvoiceDiscountResult.Success };

                }

                if (lstLimitToEdit.Count > 0)
                    _repo.EditEntities(lstLimitToEdit);
            }


            if (request.CountryIds != null && request.CountryIds.Any())
            {
                var countries = await _repo.GetInvoiceDiscountCountry(ids);
                //Delete Country
                var invDisCountries = countries.Where(x => !request.CountryIds.Contains(x.CountryId));
                invDisCountries.ToList().ForEach(x =>
                {
                    x.Active = false;
                    x.DeletedBy = userId;
                    x.DeletedOn = DateTime.Now;
                });
                _repo.EditEntities(invDisCountries);

                foreach (var countryId in request.CountryIds)
                {
                    var country = countries.FirstOrDefault(x => x.CountryId == countryId);
                    if (country == null)
                    {
                        country = _invoiceDiscountMap.MapInvoiceDiscountCountry(countryId, userId);

                        invoiceDiscount.InvDisTranCountries.Add(country);

                        _repo.AddEntity(country);
                    }
                    else
                    {
                        if (!country.Active.HasValue || !country.Active.Value)
                        {
                            country.Active = true;
                            _repo.EditEntity(country);
                        }

                    }
                }
            }
            _repo.EditEntity(invoiceDiscount);

            await _repo.Save();
            response.Result = InvoiceDiscountResult.Success;
            return response;
        }
        /// <summary>
        /// Add invoice discount periods
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="limits"></param>
        /// <param name="userId"></param>
        private void AddInvoiceDiscountPeriods(InvDisTranDetail entity, IEnumerable<InvoiceDiscountLimit> limits, int userId)
        {
            foreach (var item in limits)
            {
                var invDisTranPeriodInfo = _invoiceDiscountMap.MapInvoiceDiscountPeriod(item, userId);

                entity.InvDisTranPeriodInfos.Add(invDisTranPeriodInfo);

                _repo.AddEntity(invDisTranPeriodInfo);
            }
        }

        /// <summary>
        /// Get Invoice Discount 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditInvoiceDiscountResponse> EditInvoiceDiscount(int id)
        {
            var invDisTran = await _repo.GetInvoiceDiscount(id);
            if (invDisTran == null)
                return new EditInvoiceDiscountResponse() { Result = InvoiceDiscountResult.NotFound };
            var invoiceDiscount = _invoiceDiscountMap.EditInvoiceDiscountMap(invDisTran);
            var ids = new List<int>() { invDisTran.Id };
            var countrys = await _repo.GetCountryByInvDisIds(ids);
            invoiceDiscount.Limits = (await _repo.GetInvoiceDiscountPeriods(ids)).Select(x => _invoiceDiscountMap.EditInvoiceDiscountPeriodMap(x));
            invoiceDiscount.CountryIds = countrys.Select(x => x.Id);

            return new EditInvoiceDiscountResponse()
            {
                InvoiceDiscount = invoiceDiscount,
                Result = InvoiceDiscountResult.Success
            };
        }

        /// <summary>
        /// get invoice discount types
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerBusinessCountries(CommonDataSourceRequest request)
        {
            var countries = await _repo.GetCustomerBussinessCountriesByCustomerId(request.CustomerId.Value);
            return new DataSourceResponse() { Result = DataSourceResult.Success, DataSourceList = countries };
        }
    }
}
