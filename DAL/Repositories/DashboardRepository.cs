using Contracts.Repositories;
using DTO.Common;
using DTO.Dashboard;
using DTO.Kpi;
using DTO.Manday;
using DTO.User;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Repositories
{
    public class DashboardRepository : Repository, IDashboardRepository
    {
        public DashboardRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// Get the booking detail by customerdashboard filter request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Base booking detail</returns>
        public IQueryable<InspTransaction> GetBookingDetail(CustomerDashboardFilterRequest request)
        {
            var inspTransactions = _context.InspTransactions.Where(x => x.StatusId != (int)BookingStatus.Cancel);
            if (request != null)
            {
                if (request.CustomerId != null && request.CustomerId > 0)
                {
                    inspTransactions = inspTransactions.Where(x => x.CustomerId == request.CustomerId);
                }
                if (request.SupplierId != null && request.SupplierId > 0)
                {
                    inspTransactions = inspTransactions.Where(x => x.SupplierId == request.SupplierId);
                }
                if (request.FactoryId != null && request.FactoryId > 0)
                {
                    inspTransactions = inspTransactions.Where(x => x.FactoryId == request.FactoryId);
                }
                if (request.SelectedFactIdList != null && request.SelectedFactIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => request.SelectedFactIdList.Contains(x.FactoryId));
                }
                if (request.ServiceDateFrom != null && request.ServiceDateTo != null)
                {
                    //inspTransactions = inspTransactions.Where(x => !((x.ServiceDateFrom > request.ServiceDateTo.ToDateTime())
                    //                         || (x.ServiceDateTo < request.ServiceDateFrom.ToDateTime())));

                    inspTransactions = inspTransactions.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
                }

                if (request.StatusIdList != null && request.StatusIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => request.StatusIdList.Contains(x.StatusId));
                }

                if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranCuBrands.Any(y => request.SelectedBrandIdList.Contains(y.BrandId)));
                }

                if (request.SelectedBuyerIdList != null && request.SelectedBuyerIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranCuBuyers.Any(y => request.SelectedBuyerIdList.Contains(y.BuyerId)));
                }

                if (request.SelectedCollectionIdList != null && request.SelectedCollectionIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => request.SelectedCollectionIdList.Contains(x.CollectionId));
                }

                if (request.SelectedDeptIdList != null && request.SelectedDeptIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranCuDepartments.Any(y => request.SelectedDeptIdList.Contains(y.DepartmentId)));
                }

                if (request.ProdCategoryList != null && request.ProdCategoryList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspProductTransactions.Any(y => y.Active.Value && request.ProdCategoryList.Contains(y.Product.ProductCategory)));
                }

                if (request.ProductIdList != null && request.ProductIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspProductTransactions.Any(y => y.Active.Value && request.ProductIdList.Contains(y.ProductId)));
                }

                if (request.SelectedCountryIdList != null && request.SelectedCountryIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.Factory.SuAddresses.Any(y => request.SelectedCountryIdList.Contains(y.CountryId)));
                }
            }

            return inspTransactions;
            //.Select(x =>
            //new BookingDetail
            //{
            //    InspectionId = x.Id,
            //    CustomerId = x.CustomerId,
            //    SupplierId = x.SupplierId,
            //    FactoryId = x.FactoryId,
            //    CreationDate = x.CreatedOn.Value,
            //    ServiceDateFrom = x.ServiceDateFrom,
            //    ServiceDateTo = x.ServiceDateTo,
            //    StatusId = x.StatusId
            //}).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// Get the product count for the inspectionids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<InspCountryGeoCode>> GetInspCountryGeoCode(IEnumerable<int> lstinspid)
        {
            return await (from insp in _context.InspTransactions
                          join fact in _context.SuSuppliers on insp.FactoryId equals fact.Id
                          join sua in _context.SuAddresses on fact.Id equals sua.SupplierId
                          join con in _context.RefCountries on sua.CountryId equals con.Id
                          join reg in _context.RefProvinces on sua.RegionId equals reg.Id
                          where (lstinspid.Contains(insp.Id) && sua.AddressTypeId == (int)Supplier_Address_Type.HeadOffice)
                          select new InspCountryGeoCode
                          {
                              FactoryCountryName = con.CountryName,
                              FactoryCountryCode = con.Alpha2Code,
                              FactoryCountryId = sua.CountryId,
                              Latitude = con.Latitude,
                              Longitude = con.Longitude,
                              FactoryProvinceName = reg.ProvinceName,
                              FactoryProvinceId = reg.Id,
                              ProvinceLatitude = reg.Latitude,
                              ProvinceLongitude = reg.Longitude,
                              FactoryLatitude = sua.Latitude,
                              FactoryLongitude = sua.Longitude,
                              FactoryId = fact.Id,
                              FactoryName = fact.SupplierName
                          }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get inspection allocated
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<InspCountryGeoCode>> GetInspCountryGeoCodeAllocated(int customerid)
        {

            return await (from insp in _context.InspTransactions
                          join fact in _context.SuSuppliers on insp.FactoryId equals fact.Id
                          join sua in _context.SuAddresses on fact.Id equals sua.SupplierId
                          join con in _context.RefCountries on sua.CountryId equals con.Id
                          where (InspectedStatusList.Contains(insp.StatusId) && insp.CustomerId == customerid)
                          select new InspCountryGeoCode
                          {
                              FactoryCountryName = con.CountryName,
                              FactoryCountryId = sua.CountryId,
                              Latitude = con.Latitude,
                              Longitude = con.Longitude
                          }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the product count for the inspectionids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<int> GetProductCount(IEnumerable<int> inspectionIds)
        {
            return await _context.InspProductTransactions.Where(x => inspectionIds.Contains(x.InspectionId)
                        && x.Active.HasValue && x.Active.Value)
                        .AsNoTracking().Select(x => x.ProductId).Distinct().CountAsync();
        }

        /// <summary>
        /// Get the inspection mandays by inspectionids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public IQueryable<InspectionMandayDashboard> GetInspectionManDays(IEnumerable<int> inspectionIds)
        {
            return _context.QuQuotations.Where(x => x.IdStatus == (int)QuotationStatus.CustomerValidated).
                    SelectMany(x => x.QuQuotationInsps).Where(x => x.NoOfManDay.HasValue && inspectionIds.Contains(x.IdBooking))
                    .Select(x => new InspectionMandayDashboard()
                    {
                        MandayCount = x.NoOfManDay.Value,
                        ServiceDateTo = x.IdBookingNavigation.ServiceDateTo,
                        InspectionId = x.IdBooking
                    });
        }

        /// <summary>
        /// get the inspection actual count by inspection ids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public IQueryable<InspectionMandayDashboard> GetInspectionActualCount(IEnumerable<int> inspectionIds)
        {
            return _context.SchScheduleQcs.Where(x => x.Active && inspectionIds.Contains(x.BookingId))
                   .Select(x => new InspectionMandayDashboard()
                   {
                       ActualMandayCount = x.ActualManDay,
                       ServiceDateTo = x.Booking.ServiceDateTo,
                       InspectionId = x.BookingId
                   }).AsNoTracking();
        }

        public async Task<double> GetInspectionManDaysQuery(IQueryable<int> inspectionIds)
        {
            return await _context.QuQuotations.Where(x => x.IdStatus == (int)QuotationStatus.CustomerValidated).
                    SelectMany(x => x.QuQuotationInsps).Where(x => x.NoOfManDay.HasValue && inspectionIds.Contains(x.IdBooking))
                    .AsNoTracking().SumAsync(x => x.NoOfManDay.Value);
        }

        /// <summary>
        /// Get the API Result Analysis Dashboard
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectionStatusList"></param>
        /// <returns></returns>
        public IQueryable<FbReportDetail> GetAPIRADashboard(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList)
        {
            return _context.FbReportDetails.Where(x => x.Active.HasValue && x.Active.Value
                                            && x.ResultId != null && x.FbReportStatus == (int)FBStatus.ReportValidated
                                            && inspectionIds.Contains(x.InspectionId.Value) && inspectionStatusList.Contains(x.Inspection.StatusId));

        }

        public async Task<List<CustomerAPIRADashboardRepo>> GetAPIRADashboardByQuery(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList)
        {
            return await _context.InspProductTransactions.Where(x => inspectionIds.Contains(x.InspectionId)
                                    && x.Active.HasValue && x.Active.Value && inspectionStatusList.Contains(x.Inspection.StatusId)).
                                    Select(x => x.FbReport).Where(x => x.Active.HasValue && x.Active.Value
                                            && x.ResultId != null && x.FbReportStatus == (int)FBStatus.ReportValidated).Distinct().
                                    GroupBy(x => x.ResultId, p => p, (key, _data) =>
                                          new CustomerAPIRADashboardRepo
                                          {
                                              ResultId = key,
                                              TotalCount = _data.Count()
                                          }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the API Result Analysis Dashboard
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectionStatusList"></param>
        /// <returns></returns>
        public async Task<List<CustomerAPIRADashboardRepo>> GetQueriableAPIRADashboard(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList)
        {
            return await _context.FbReportDetails.Where(x => inspectionIds.Contains(x.InspectionId.GetValueOrDefault())
                                            && inspectionStatusList.Contains(x.Inspection.StatusId) &&
                                            x.Active.HasValue && x.Active.Value
                                            && x.ResultId != null && x.FbReportStatus == (int)FBStatus.ReportValidated).Distinct().
                                    GroupBy(x => x.ResultId, p => p, (key, _data) =>
                                          new CustomerAPIRADashboardRepo
                                          {
                                              ResultId = key,
                                              TotalCount = _data.Count()
                                          }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the FB Report Results Master Data
        /// </summary>
        /// <returns></returns>
        public List<FBReportResultData> GetFbReportResults()
        {
            return _context.FbReportResults.Where(x => x.Active.HasValue && x.Active.Value).
                        Select(x =>
                        new FBReportResultData
                        {
                            ResultId = x.Id,
                            ResultName = x.ResultName
                        }).AsNoTracking().ToList();
        }

        /// <summary>
        /// Get the customer result for the inspections
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectionStatusList"></param>
        /// <returns></returns>
        public IQueryable<InspRepCusDecision> GetCustomerResult(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList)
        {

            return _context.InspRepCusDecisions.Where(x => x.Active.Value && x.Report.Active.Value && inspectionIds.Contains(x.Report.InspectionId.Value)
                    && inspectionStatusList.Contains(x.Report.Inspection.StatusId));

        }
        /// <summary>
        /// Get the customer result for the inspections
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectionStatusList"></param>
        /// <returns></returns>
        public async Task<List<CustomerResultRepo>> GetQueryableCustomerResult(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList)
        {
            return await _context.FbReportDetails.Where(x => inspectionIds.Contains(x.InspectionId.GetValueOrDefault()) &&
                                inspectionStatusList.Contains(x.Inspection.StatusId) && x.Active.HasValue && x.Active.Value)
                                      .SelectMany(x => x.InspRepCusDecisions)
                                  .Select(x => new { x.CustomerResultId, x.ReportId })
                                    .GroupBy(x => x.CustomerResultId, p => p, (key, _data) =>
                                         new CustomerResultRepo
                                         {
                                             Id = key,
                                             TotalCount = _data.Select(x => x.ReportId).Distinct().Count()
                                         }).AsNoTracking().ToListAsync();


        }

        /// <summary>
        /// Get the customer Decision result for the inspections
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectionStatusList"></param>
        /// <returns></returns>
        public async Task<List<CustomerResultRepo>> GetCustomerDecisionResult(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList)
        {
            return await _context.FbReportDetails.Where(x => inspectionIds.Contains(x.Inspection.Id)
                            && inspectionStatusList.Contains(x.Inspection.StatusId) && x.FbReportStatus == (int)FBStatus.ReportValidated &&
                            x.Active.HasValue && x.Active.Value)
                            .Select(x => new
                            {
                                x.InspRepCusDecisions.FirstOrDefault().CustomerResultId,
                                x.InspRepCusDecisions.FirstOrDefault().ReportId
                            })
                            .GroupBy(x => x.CustomerResultId, p => p, (key, _data) =>
                                new CustomerResultRepo
                                {
                                    Id = key,
                                    TotalCount = _data.Select(x => x.ReportId).Distinct().Count()
                                }).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// Get the customer decision data
        /// </summary>
        /// <param name="resultIds"></param>
        /// <returns></returns>
        public async Task<List<CustomerResultMasterRepo>> GetCustomerResultAnalysis(List<int> resultIds)
        {
            //data to display the customer result in dashboard
            return await _context.RefInspCusDecisionConfigs.
                        Where(x => //x.Active.HasValue && x.Active.Value &&
                        resultIds.Contains(x.Id)).
                        Select(x => new CustomerResultMasterRepo
                        {
                            Id = x.Id,
                            CustomerDecisionId = x.CusDecId,
                            CustomDecisionName = x.CustomDecisionName,
                            CustomerDecisionName = x.CusDec.Name
                        }).AsNoTracking().ToListAsync();

        }
        /// <summary>
        /// Get the facory address by id
        /// </summary>
        /// <param name="factoryIds"></param>
        /// <returns></returns>
        public IQueryable<SuAddress> GetFactoryAddressById(IEnumerable<int> factoryIds)
        {
            return _context.SuAddresses.Where(x => factoryIds.Contains(x.SupplierId) && x.AddressTypeId == (int)SuAddressTypeEnum.Headoffice).AsNoTracking();
        }
        /// <summary>
        /// Get the facory address by id
        /// </summary>
        /// <param name="factoryIds"></param>
        /// <returns></returns>
        public IQueryable<SuAddress> GetQueryableFactoryAddressById(IQueryable<int> factoryIds)
        {
            return _context.SuAddresses.Where(x => factoryIds.Contains(x.SupplierId) && x.AddressTypeId == (int)SuAddressTypeEnum.Headoffice).AsNoTracking();
        }

        /// <summary>
        /// Get the Product Category data for the inspections
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public IQueryable<InspProductTransaction> GetProductCategoryDashboard(IQueryable<int> inspectionIds)
        {
            return _context.InspProductTransactions.Where(x => inspectionIds.Contains(x.Inspection.Id)).
                                    Where(x => x.Active.HasValue && x.Active.Value && x.Product.ProductCategory != null);
        }

        /// <summary>
        /// Get the inspection rejected details (take only failed results from summary)
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public IQueryable<FbReportInspSummary> GetCustomerInspectionReject(IQueryable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return _context.FbReportInspSummaries.
                Where(x => x.Active.HasValue && x.Active.Value && x.Name != null && x.ResultId == (int)FBReportResultEnum.Fail
                && inspectionIds.Contains(x.FbReportDetail.InspectionId.Value) && inspectedStatusIds.Contains(x.FbReportDetail.Inspection.StatusId));

        }

        public async Task<List<InspectionRejectDashboard>> GetCustomerInspectionRejectByQuery(IQueryable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value
                                                     && inspectionIds.Contains(x.InspectionId)
                                                     && inspectedStatusIds.Contains(x.Inspection.StatusId))
                              .Select(x => x.FbReport).Where(x => x.Active.HasValue && x.Active.Value).
                              SelectMany(x => x.FbReportInspSummaries).
                              Where(x => x.Active.HasValue && x.Active.Value && x.Name != null && x.ResultId == (int)FBReportResultEnum.Fail).
                              GroupBy(x => x.Name, p => p, (key, _data) =>
                                          new InspectionRejectDashboard
                                          {
                                              StatusName = key,
                                              TotalCount = _data.Count()
                                          }).AsNoTracking().OrderByDescending(y => y.TotalCount).
                                          Take(10).ToListAsync();
        }

        /// <summary>
        /// Get purchase order edt date by inspection ids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<CustomerETDDataRepo>> GetPOETDDateByInspectionId(IEnumerable<int> inspectionIds)
        {
            return await _context.InspTransactions.
                            Join(_context.InspPurchaseOrderTransactions, t => t.Id,
                            p => p.InspectionId, (t, p) =>
                            new CustomerETDDataRepo
                            {
                                InspectionId = t.Id,
                                EtdDate = p.Etd,
                                ServiceToDate = t.ServiceDateTo
                            }).Where(x => inspectionIds.Contains(x.InspectionId) && x.EtdDate.HasValue)
                            .Select(z => new { z.InspectionId, z.EtdDate, z.ServiceToDate })
                            .GroupBy(x => new { x.InspectionId, x.EtdDate, x.ServiceToDate }, (key, _data) =>
                            new CustomerETDDataRepo
                            {
                                InspectionId = key.InspectionId,
                                EtdDate = key.EtdDate,
                                ServiceToDate = key.ServiceToDate
                            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the bookingid count by bookingid,servicedatefrom and servicedateto
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns>give the booking id and count for servicedate changed for the booking id</returns>
        public async Task<List<SupplierBookingRevisionRepo>> GetSupplierRevisionData(IEnumerable<int> inspectionIds)
        {
            return await _context.InspTranStatusLogs.Where(x => inspectionIds.Contains(x.BookingId)).
                                        Select(z => new { z.BookingId, z.ServiceDateFrom, z.ServiceDateTo }).
                                        GroupBy(x => new { x.BookingId, x.ServiceDateFrom, x.ServiceDateTo }, (key, _data) => new SupplierBookingRevisionRepo
                                        {
                                            InspectionId = key.BookingId,
                                            BookingCount = _data.Count()
                                        }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the count of the quotation needs to be validated
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<int> GetPendingQuotations(int customerId)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdQuotationNavigation.CustomerId == customerId && (x.IdBookingNavigation.StatusId != (int)BookingStatus.Cancel)
                                    && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.SentToClient && x.IdQuotationNavigation.BillingPaidById == (int)QuotationPaidBy.customer).Select(x => x.IdQuotation).Distinct().CountAsync();
        }
        /// <summary>
        /// Get the count of the quotation which is validated
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<int> GetCompletedQuotations(int customerId)
        {
            return await _context.QuQuotations.Where(x => x.CustomerId == customerId
                                    && x.IdStatus == (int)QuotationStatus.CustomerValidated).AsNoTracking().CountAsync();
        }

        /// <summary>
        /// Get the man days group by service date
        /// </summary>
        /// <param name="ServiceDateFrom"></param>
        /// <param name="ServiceDateTo"></param>
        /// <returns>ServiceDate and SumofManDay between two dates</returns>
        public async Task<List<InspectionManDaysRepo>> GetInspectionManDays(DateTime ServiceDateFrom, DateTime ServiceDateTo, IEnumerable<int> inspectedStatusIds, int customerId)
        {
            return await _context.InspTransactions.
                            Join(_context.QuQuotationInsps, t => t.Id,
                            p => p.IdBooking, (t, p) => new { t, p }).
                            Where(x => x.p.NoOfManDay.HasValue && !((x.t.ServiceDateFrom > ServiceDateTo)
                                             || (x.t.ServiceDateTo < ServiceDateFrom))
                                             && x.p.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated
                                             && inspectedStatusIds.Contains(x.t.StatusId) && x.t.CustomerId == customerId).
                            Select(x => new { ServiceDateFrom = x.t.ServiceDateFrom, NoOfManDay = x.p.NoOfManDay }).
                            GroupBy(x => new { x.ServiceDateFrom, x.NoOfManDay }).
                            Select(grp =>
                            new InspectionManDaysRepo
                            {
                                ServiceDate = grp.Key.ServiceDateFrom,
                                ManDays = grp.Sum(x => x.NoOfManDay.Value)
                            }).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// Get the monthly inspection man days
        /// </summary>
        /// <param name="ServiceDateFrom"></param>
        /// <param name="ServiceDateTo"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionMonthlyManDaysRepo>> GetMonthlyInspectionManDays(DateTime ServiceDateFrom, DateTime ServiceDateTo, IEnumerable<int> inspectedStatusIds, int customerId)
        {
            return await _context.InspTransactions.
                                Join(_context.QuQuotationInsps, t => t.Id,
                                p => p.IdBooking, (t, p) => new { t, p }).
                                Where(x => x.p.NoOfManDay.HasValue && !((x.t.ServiceDateFrom > ServiceDateTo)
                                                 || (x.t.ServiceDateTo < ServiceDateFrom))
                                                 && x.p.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated
                                                 && inspectedStatusIds.Contains(x.t.StatusId) && x.t.CustomerId == customerId).
                                Select(x => new { ServiceDateFrom = x.t.ServiceDateFrom, NoOfManDay = x.p.NoOfManDay }).
                                GroupBy(x => new { x.ServiceDateFrom.Month, x.NoOfManDay }).
                                Select(grp =>
                                new InspectionMonthlyManDaysRepo
                                {
                                    Month = grp.Key.Month,
                                    ManDays = grp.Sum(x => x.NoOfManDay.Value)
                                }).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// GetInspectedBookingCount
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<int> GetInspectedBookingCount(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspTransactions.Where(x => inspectionIds.Contains(x.Id)
                                            && inspectedStatusIds.Contains(x.StatusId)
                                            ).AsNoTracking().CountAsync();
        }

        /// <summary>
        /// GetInspectedManDaysCount
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<double> GetInspectedManDaysCount(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspTransactions.Where(x => inspectionIds.Contains(x.Id)
                                            && inspectedStatusIds.Contains(x.StatusId)).
                                            SelectMany(x => x.QuQuotationInsps)
                                            .Where(x => x.NoOfManDay.HasValue && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated)
                                            .AsNoTracking().SumAsync(x => x.NoOfManDay.Value);
        }

        public async Task<UserStaffDetails> GetCSDetails(int customerId)
        {

            return await _context.ItUserMasters.Where(x => x.Active &&
                x.DaUserCustomerUsers.Any(y => y.Email && y.CustomerId == customerId &&
                y.UserType == (int)HRProfile.CS)).
                Select(y => new UserStaffDetails
                {
                    FullName = y.Staff != null ? (y.Staff.PersonName ?? "") : "",
                    Id = y.Id,
                    EmailAddress = y.Staff != null ? (y.Staff.CompanyEmail ?? "") : "",
                    StaffId = y.StaffId.HasValue ? y.StaffId.Value : 0,
                    MobileNumber = y.Staff != null ? (y.Staff.CompanyMobileNo ?? "") : "",
                }).AsNoTracking().FirstOrDefaultAsync();
        }
        /// <summary>
        /// Get PO PRoducts(ProductId,CombineProductId) for  the bookings
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<POProductsRepo>> GetPOProducts(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspTransactions.Where(x => inspectionIds.Contains(x.Id) && inspectedStatusIds.Contains(x.StatusId))
                .Select(x => new POProductsRepo
                {
                    ServiceDateTo = x.ServiceDateTo,
                    ContainerFbReportCount = x.InspContainerTransactions.Where(x => x.FbReportId.HasValue && x.FbReport.Active.Value && x.Active.Value).Select(x => x.FbReportId).Distinct().Count(),
                    ProductFbReportCount = x.InspProductTransactions.Where(x => x.FbReportId.HasValue && x.FbReport.Active.Value && x.Active.Value).Select(x => x.FbReportId).Distinct().Count()
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the report count for the inspectionids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<FbReportCustomerDashboard>> GetReportData(IEnumerable<int> inspectionIds)
        {
            return await _context.InspProductTransactions.Where(x => inspectionIds.Contains(x.InspectionId)
                        && x.Active.HasValue && x.Active.Value && x.FbReportId.HasValue && x.FbReport.Active.Value)
                        .Select(x => new FbReportCustomerDashboard
                        {
                            FbReportId = x.FbReportId,
                            ResultId = x.FbReport.ResultId
                        }).Distinct().AsNoTracking().ToListAsync();
        }

        //get failed result of report details from Fb_Report_Inspsummary
        public async Task<List<FBReportInspSubSummary>> GetFBInspSummaryResultbyReport(IEnumerable<int> fbReportIdList)
        {
            return await _context.FbReportInspSummaries.Where(x => x.Active.HasValue && x.Active == true && x.ResultId == (int)FBReportResult.Fail && fbReportIdList.Contains(x.FbReportDetailId) && x.FbReportInspsumTypeId == (int)InspSummaryType.Main).OrderBy(x => x.Sort ?? int.MaxValue)
                .Select(y => new FBReportInspSubSummary
                {
                    FBReportId = y.FbReportDetailId,
                    Name = y.Name,
                    Id = y.Id
                }).AsNoTracking().ToListAsync();
        }

        //Get booking manday by month
        public async Task<List<MandayYearChartItem>> GetMonthlyInspManDays(IEnumerable<int> bookingIds)
        {
            return await _context.QuQuotationInsps.Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated).
                Select(k => new { k.IdBookingNavigation.ServiceDateFrom.Year, k.IdBookingNavigation.ServiceDateFrom.Month, k.NoOfManDay }).
                GroupBy(x => new { x.Year, x.Month }, (key, group) => new MandayYearChartItem
                {
                    Year = key.Year,
                    Month = key.Month,
                    MonthManDay = group.Sum(k => k.NoOfManDay).GetValueOrDefault(),
                    MonthName = MonthData.GetValueOrDefault(key.Month)
                }).AsNoTracking().ToListAsync();
        }
        //Get booking manday by month
        public async Task<List<MandayYearChartItem>> GetQueryableMonthlyInspManDays(IQueryable<int> bookingIds)
        {
            var lstbooking = await bookingIds.ToListAsync();
            return await _context.QuQuotationInsps.Where(x => lstbooking.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated).
                Select(k => new { k.IdBookingNavigation.ServiceDateFrom.Year, k.IdBookingNavigation.ServiceDateFrom.Month, k.NoOfManDay }).
                GroupBy(x => new { x.Year, x.Month }, (key, group) => new MandayYearChartItem
                {
                    Year = key.Year,
                    Month = key.Month,
                    MonthManDay = group.Sum(k => k.NoOfManDay).GetValueOrDefault()
                }).AsNoTracking().ToListAsync();
        }
    }
}
