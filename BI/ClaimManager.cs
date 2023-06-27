using BI.Maps;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Claim;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.RepoRequest.Enum;
using DTO.Supplier;
using DTO.User;
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
    public class ClaimManager : ApiCommonData, IClaimManager
    {
        private readonly IClaimRepository _repo = null;
        private readonly IScheduleRepository _schRepo = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IUserRepository _userRepo = null;
        private readonly IManualInvoiceManager _manualInvoiceManager;
        private readonly ClaimMap _clmMap = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private IFileManager _fileManager = null;
        private readonly ICustomerManager _customerManager = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly ISupplierManager _suppliermanager = null;
        private IHumanResourceRepository _hrRepository = null;
        private readonly IReportRepository _reportRepository = null;
        private readonly ISharedInspectionRepo _sharedInspectionRepo = null;
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly IInspectionBookingRepository _inspectionBookingRepository = null;

        public ClaimManager(IClaimRepository repo, IScheduleRepository schRepo, IInspectionBookingRepository inspRepo, IUserRepository userRepo,
            IManualInvoiceManager manualInvoiceManager, IReportRepository reportRepository, ISharedInspectionRepo sharedInspectionRepo, IInvoiceRepository invoiceRepository,
            IAPIUserContext applicationContext, ITenantProvider filterService, IFileManager fileManager, ICustomerManager customerManager, IOfficeLocationManager office,
            ISupplierManager suppliermanager, IHumanResourceRepository hrRepository, IInspectionBookingRepository inspectionBookingRepository)
        {
            _repo = repo;
            _schRepo = schRepo;
            _inspRepo = inspRepo;
            _userRepo = userRepo;
            _manualInvoiceManager = manualInvoiceManager;
            _clmMap = new ClaimMap();
            _filterService = filterService;
            _ApplicationContext = applicationContext;
            _fileManager = fileManager;
            _customerManager = customerManager;
            _office = office;
            _suppliermanager = suppliermanager;
            _hrRepository = hrRepository;
            _reportRepository = reportRepository;
            _sharedInspectionRepo = sharedInspectionRepo;
            _invoiceRepository = invoiceRepository;
            _inspectionBookingRepository = inspectionBookingRepository;
        }


        public async Task<ClaimSummaryResponse> GetClaimSummary()
        {
            var response = new ClaimSummaryResponse();
            var statusList = await _repo.GetClaimStatusList();

            if (statusList == null || !statusList.Any())
                return new ClaimSummaryResponse { Result = ClaimSummaryResult.CannotFindStatusList };

            response.StatusList = statusList.Select(_clmMap.GetClaimStatus);

            response.Result = ClaimSummaryResult.Success;
            return response;
        }

        public async Task<DataSourceResponse> GetBookingIdDataSource(CommonBookingIdSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _repo.GetBookingIdDataSource();

                data = data.Where(x => InspectedStatusList.Contains(x.StatusId));

                if (!string.IsNullOrEmpty(request.SearchText) && int.Parse(request.SearchText) > 0)
                {
                    data = data.Where(x => x.Id == int.Parse(request.SearchText));
                }

                // filter by country ids
                if (request.Ids != null && request.Ids.Any())
                {
                    data = data.Where(x => request.Ids.Contains(x.Id));
                }

                //var bookingIdList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();                

                var items = await data.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Id.ToString()
                }).Skip(request.Skip).Take(request.Take).ToListAsync();

                if (items == null || !items.Any())
                    response.Result = DataSourceResult.CannotGetList;

                response.DataSourceList = items;
                response.Result = DataSourceResult.Success;

                return response;

            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<DataSourceResponse> GetReportListByBooking(int bookingId)
        {
            var reportList = await _inspRepo.GetInspectionReportData(new List<int> { bookingId }.ToList());

            var items = reportList.Select(x => new CommonDataSource
            {
                Id = x.ReportId.GetValueOrDefault(),
                Name = x.ReportTitle
            }).ToList();

            if (items == null || !items.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = items,
                Result = DataSourceResult.Success
            };
        }

        public async Task<ClaimBookingResponse> GetBookingData(BookingClaimRequest request)
        {
            var bookingData = await _repo.GetBookingData(request.BookingId);

            var qcIds = await _hrRepository.GetFBQCIdList(request.BookingId);
            var qcNameList = await _hrRepository.GetQCNameList(qcIds);

            var csIds = await _hrRepository.GetFBCSIds(request.BookingId);
            var csNameList = await _hrRepository.GetCSNameList(csIds);

            var fbReportData = await _repo.GetFbQuantityData(request.BookingId);

            if (request.ReportId != null && request.ReportId.Any())
            {
                fbReportData = fbReportData.Where(x => request.ReportId.Contains(x.ReportId)).ToList();
            }

            var res = new BookingClaimData
            {
                CustomerId = bookingData.CustomerId,
                CustomerName = bookingData.CustomerName,
                SupplierName = bookingData.SupplierName,
                FactoryName = bookingData.FactoryName,
                FactoryAddress = bookingData.FactoryAddress + (string.IsNullOrEmpty(bookingData.FactoryRegionalAddress) ? "" : ("( " + bookingData.FactoryRegionalAddress + " )")),
                ServiceDate = bookingData?.ServiceFromDate == bookingData?.ServiceToDate ? bookingData?.ServiceFromDate.ToString(StandardDateFormat) : string.Join(" - ", bookingData?.ServiceFromDate.ToString(StandardDateFormat), bookingData?.ServiceToDate.ToString(StandardDateFormat)),
                Qc = qcNameList != null && qcNameList.Any() ? string.Join(", ", qcNameList.Select(x => x).Distinct()) : "",
                Cs = csNameList != null && csNameList.Any() ? string.Join(", ", csNameList.Select(x => x).Distinct()) : "",
                ReportInspectedQty = fbReportData != null && fbReportData.Any() ? fbReportData.Sum(x => x.InspectedQty) : null,
                ReportPresentedQty = fbReportData != null && fbReportData.Any() ? fbReportData.Sum(x => x.PresentedQty) : null,
                ServiceFromDate = bookingData.ServiceFromDate.ToString(StandardDateFormat),
                ServiceToDate = bookingData.ServiceToDate.ToString(StandardDateFormat)
            };

            if (res == null)
            {
                return new ClaimBookingResponse { Result = ClaimResult.NotFound };
            }

            return new ClaimBookingResponse
            {
                Data = res,
                Result = ClaimResult.Success
            };
        }
        public async Task<DataSourceResponse> GetClaimFromList()
        {
            var claimFromList = await _repo.GetClaimFromList();

            if (claimFromList == null || !claimFromList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = claimFromList,
                Result = DataSourceResult.Success
            };
        }
        public async Task<DataSourceResponse> GetReceivedFromList()
        {
            var receivedFromList = await _repo.GetReceivedFromList();

            if (receivedFromList == null || !receivedFromList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = receivedFromList,
                Result = DataSourceResult.Success
            };
        }
        public async Task<DataSourceResponse> GetClaimSourceList()
        {
            var claimSourceList = await _repo.GetClaimSourceList();

            if (claimSourceList == null || !claimSourceList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = claimSourceList,
                Result = DataSourceResult.Success
            };
        }

        public async Task<DataSourceResponse> GetFbDefectList()
        {
            var defectCategoryList = await _repo.GetFbDefectList();

            if (defectCategoryList == null || !defectCategoryList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = defectCategoryList,
                Result = DataSourceResult.Success
            };
        }

        public async Task<DataSourceResponse> GetClaimDepartmentList()
        {
            var claimDepartmentList = await _repo.GetClaimDepartmentList();

            if (claimDepartmentList == null || !claimDepartmentList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = claimDepartmentList,
                Result = DataSourceResult.Success
            };
        }

        public async Task<DataSourceResponse> GetClaimCustomerRequestList()
        {
            var claimCustomerRequestList = await _repo.GetClaimCustomerRequestList();

            if (claimCustomerRequestList == null || !claimCustomerRequestList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = claimCustomerRequestList,
                Result = DataSourceResult.Success
            };
        }

        public async Task<DataSourceResponse> GetClaimPriorityList()
        {
            var claimPriorityList = await _repo.GetClaimPriorityList();

            if (claimPriorityList == null || !claimPriorityList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = claimPriorityList,
                Result = DataSourceResult.Success
            };
        }
        public async Task<DataSourceResponse> GetClaimRefundTypeList()
        {
            var claimRefundTypeList = await _repo.GetClaimRefundTypeList();

            if (claimRefundTypeList == null || !claimRefundTypeList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = claimRefundTypeList,
                Result = DataSourceResult.Success
            };
        }

        public async Task<DataSourceResponse> GetClaimDefectDistributionList()
        {
            var claimdefectDistributionList = await _repo.GetClaimDefectDistributionList();

            if (claimdefectDistributionList == null || !claimdefectDistributionList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = claimdefectDistributionList,
                Result = DataSourceResult.Success
            };
        }

        public async Task<DataSourceResponse> GetClaimResultList()
        {
            var claimResultList = await _repo.GetClaimResultList();

            if (claimResultList == null || !claimResultList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = claimResultList,
                Result = DataSourceResult.Success
            };
        }

        public async Task<DataSourceResponse> GetClaimFinalResultList()
        {
            var finalResultList = await _repo.GetClaimFinalResultList();

            if (finalResultList == null || !finalResultList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = finalResultList,
                Result = DataSourceResult.Success
            };
        }
        public async Task<DataSourceResponse> GetCurrencies()
        {
            var claimCurrencyList = await _repo.GetCurrencies();

            if (claimCurrencyList == null || !claimCurrencyList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = claimCurrencyList,
                Result = DataSourceResult.Success
            };
        }
        public async Task<DataSourceResponse> GetClaimFileTypeList()
        {
            var finalResultList = await _repo.GetClaimFileTypeList();

            if (finalResultList == null || !finalResultList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = finalResultList,
                Result = DataSourceResult.Success
            };
        }

        public async Task<SaveClaimResponse> SaveClaim(ClaimDetails request)
        {
            try
            {
                var response = new SaveClaimResponse();
                var userId = _ApplicationContext.UserId;
                if (request.Id == 0)
                {
                    //var existCalims = 
                    var existCalims = await _repo.GetClaimByBookingId(request.BookingId);
                    var claimIds = existCalims.Select(x => x.Id).ToList();
                    var claimReportList = await _repo.GetClaimReportList(claimIds);
                    var reportIds = request.ReportIdList;
                    var existingReports = claimReportList.Where(x => reportIds.Contains(x.ReportId.GetValueOrDefault())).ToList();
                    if (existingReports.Count == 0)
                    {
                        request.ClaimNo = "CLM_" + DateTime.Now.ToString("yyyyddMMhhmmss");
                        ClmTransaction entity = _clmMap.MapClaimEntity(request, _filterService.GetCompanyId(), userId);

                        if (entity == null)
                            return new SaveClaimResponse { Result = SaveClaimResult.CannotMapRequestToEntites };

                        response.Id = _repo.AddClaim(entity);

                        if (response.Id == 0)
                            return new SaveClaimResponse { Result = SaveClaimResult.CannotAddClaim };

                        response.Result = SaveClaimResult.Success;

                        return response;
                    }
                    else
                    {
                        return new SaveClaimResponse { Result = SaveClaimResult.ClaimReportsExist };
                    }
                }
                else
                {
                    var entity = await _repo.GetClaimDetails(request.Id);

                    if (entity == null)
                        return new SaveClaimResponse { Result = SaveClaimResult.CurrentClaimNotFound };

                    _clmMap.UpdateEnity(entity, request, userId);

                    int re = await _repo.EditClaim(entity);
                    if (re > 0)
                        return new SaveClaimResponse { Id = entity.Id, Result = SaveClaimResult.Success };
                }
                return response;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public async Task<EditClaimResponse> GetEditClaim(int id)
        {
            var response = new EditClaimResponse();
            try
            {
                //Claim
                if (id > 0)
                {
                    var claimData = await _repo.GetClaimDetailsByClaimId(id);

                    if (claimData == null)
                        return null;

                    var claimFileTypeList = await _repo.GetClaimFileTypeList();

                    var claimDetail = _clmMap.GetClaimDetails(claimData, claimFileTypeList, (x) => _fileManager.GetMimeType(x));
                    response.Claim = claimDetail;

                    response.BookingIdList = new CommonDataSource { Id = response.Claim.BookingId, Name = response.Claim.BookingId.ToString() };

                    var reportList = GetReportListByBooking(claimDetail.BookingId);
                    if (reportList.Result.DataSourceList != null)
                    {
                        response.ReportList = reportList.Result.DataSourceList;
                    }
                    var defectFamilyList = GetFbDefectList();
                    if (defectFamilyList.Result.DataSourceList != null)
                    {
                        response.DefectFamilyList = defectFamilyList.Result.DataSourceList;
                    }
                    var claimDepartmentList = GetClaimDepartmentList();
                    if (claimDepartmentList.Result.DataSourceList != null)
                    {
                        response.ClaimDepartmentList = claimDepartmentList.Result.DataSourceList;
                    }
                    var claimCustomerRequestList = GetClaimCustomerRequestList();
                    if (claimCustomerRequestList.Result.DataSourceList != null)
                    {
                        response.ClaimCustomerRequestList = claimCustomerRequestList.Result.DataSourceList;
                    }
                    var customerRequestRefundList = GetClaimRefundTypeList();
                    if (customerRequestRefundList.Result.DataSourceList != null)
                    {
                        response.CustomerRequestRefundList = customerRequestRefundList.Result.DataSourceList;
                    }
                    if (response.Claim == null)
                        return new EditClaimResponse { Result = EditClaimResult.CannotGetClaim };
                }
                response.Result = EditClaimResult.Success;
            }
            catch (Exception ex)
            {
            }
            return response;
        }

        public async Task<BookingClaimsResponse> GetClaimsByBookingId(int bookingId)
        {
            var claims = await _repo.GetClaimByBookingId(bookingId);
            if (claims.Count > 0)
            {
                return new BookingClaimsResponse
                {
                    BookingClaims = claims,
                    Result = DataSourceResult.Success
                };
            }
            else
            {
                return new BookingClaimsResponse { Result = DataSourceResult.CannotGetList };
            }
        }
        public async Task<CustomerKamDetail> GetCustomerKAMDetails(int customerId)
        {
            return await _repo.GetCustomerKAMDetails(customerId);
        }
        public async Task<ClaimEmailRequest> GetClaimMailDetail(int claimId, int bookingId, IEnumerable<int> reportIdList, bool? isEdit)
        {
            var data = new ClaimEmailRequest();
            try
            {
                var user = await _repo.GetUserName(_ApplicationContext.UserId);
                var claim = await _repo.GetClaimDetailsByClaimId(claimId);

                data.ClaimNo = claim.ClaimNo;
                data.ClaimDate = (DateTime)claim.ClaimDate;
                data.StatusName = claim.StatusName;
                data.StatusId = claim.StatusId.GetValueOrDefault();
                data.UserName = user.FullName;
                data.InspectionNo = bookingId;
                data.Priority = claim.Priority;
                var claimFinalDecisions = claim.ClmTranFinalDecisions.Select(x => x.FinalDecision.GetValueOrDefault()).Distinct().ToList();
                var finalDecisionNames = await _repo.GetClaimFinalResultListByIds(claimFinalDecisions);

                data.FinalDecision = string.Join(",", finalDecisionNames);
                data.FinalDecisionName = finalDecisionNames;
                data.FinalAmount = claim.FinalAmount;
                data.FinalCurrencyName = claim.FinalCurrency?.CurrencyName;
                data.ClaimRefundRemarks = claim.ClaimRefundRemarks;
                // Booking detail
                var response = await _repo.GetBookingResponse(bookingId);
                var bookingIds = new List<int> { bookingId };
                var productList = await _inspectionBookingRepository.GetProductListByBookingByPO(bookingIds);
                if (response.ServiceTypeId == (int)InspectionServiceTypeEnum.Container && reportIdList != null && reportIdList.Any())
                    productList = productList.Where(x => reportIdList.Contains(x.FbContainerReportId)).ToList();
                else if (reportIdList != null && reportIdList.Any())
                    productList = productList.Where(x => reportIdList.Contains(x.FbReportId)).ToList();

                if (response != null)
                    data = _clmMap.MapClaimForEmail(data, response, productList, (int)CountryEnum.China);
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public async Task<User> GetUserInfo(int userId)
        {
            return await _userRepo.GetUserInfo(userId);
        }

        public async Task<InvoiceResponse> GetClaimInvoiceByBooking(int bookingId)
        {
            var invoice = await _repo.GetClaimInvoiceDetails(bookingId);
            var invoiceData = _clmMap.MapClaimInvoiceDetails(invoice);
            if (invoiceData != null)
            {
                return new InvoiceResponse
                {
                    InvoiceDetail = invoiceData,
                    Result = DataSourceResult.Success
                };
            }
            else
            {
                return new InvoiceResponse { Result = DataSourceResult.CannotGetList };
            }
        }

        public async Task<ClaimSearchResponse> GetClaimDetails(ClaimSearchRequest request)
        {
            if (request == null)
                return new ClaimSearchResponse() { Result = ClaimSearchResponseResult.NotFound };

            var response = new ClaimSearchResponse { };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            // get the claim Query 
            var claimQuery = await _repo.GetAllClaimsQuery();
            var claimData = GetClaimQuerywithRequestFilters(request, claimQuery);

            var claimStatusList = await GetClaimStatusList(claimData);
            claimStatusList.ForEach(x => x.StatusColor = ClaimSummaryStatusColor.GetValueOrDefault(x.StatusId, ""));

            var data = claimData.Select(x => new ClaimItem
            {
                ClaimId = x.Id,
                ClaimNo = x.ClaimNo,
                ClaimDate = x.ClaimDate.Value.ToString(StandardDateFormat),
                BookingId = x.InspectionNo.GetValueOrDefault(),
                CustomerId = x.InspectionNoNavigation.CustomerId,
                SupplierId = x.InspectionNoNavigation.SupplierId,
                CustomerName = x.InspectionNoNavigation.Customer.CustomerName,
                SupplierName = x.InspectionNoNavigation.Supplier.SupplierName,
                InspectionDate = x.InspectionNoNavigation.ServiceDateFrom == x.InspectionNoNavigation.ServiceDateTo ? x.InspectionNoNavigation.ServiceDateFrom.ToString(StandardDateFormat) : string.Join(" - ", x.InspectionNoNavigation.ServiceDateFrom.ToString(StandardDateFormat), x.InspectionNoNavigation.ServiceDateTo.ToString(StandardDateFormat)),
                ServiceDateFrom = x.InspectionNoNavigation.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = x.InspectionNoNavigation.ServiceDateTo.ToString(StandardDateFormat),
                StatusName = x.Status.Name,
                StatusColor = ClaimSummaryStatusColor.GetValueOrDefault(x.Status.Id, ""),
                Office = x.InspectionNoNavigation.Office.LocationName,
                StatusId = x.StatusId,
                FinalDecision = x.ClmTranFinalDecisions.Select(x => x.FinalDecisionNavigation.Name).FirstOrDefault()
            });

            var result = await data.Skip(skip).Take(take).AsNoTracking().ToListAsync();

            response.TotalCount = await claimData.AsNoTracking().CountAsync();

            try
            {
                if (response.TotalCount == 0)
                {
                    response.Result = ClaimSearchResponseResult.NotFound;
                    return response;
                }

                return new ClaimSearchResponse()
                {
                    Result = ClaimSearchResponseResult.Success,
                    TotalCount = response.TotalCount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                    ClaimStatuslst = claimStatusList,
                    Data = result,
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IQueryable<ClmTransaction> GetClaimQuerywithRequestFilters(ClaimSearchRequest request, IQueryable<ClmTransaction> claimQuery)
        {
            //search by inspection booking no/po no/customer booking no
            if (!string.IsNullOrWhiteSpace(request.SearchTypeText?.Trim()) && (Enum.TryParse(request.SearchTypeId.ToString(), out SearchType _numberSearchTypeEnum)))
            {
                switch (_numberSearchTypeEnum)
                {
                    case SearchType.BookingNo:
                        {
                            if (int.TryParse(request.SearchTypeText?.Trim(), out int bookid))
                                claimQuery = claimQuery.Where(x => x.InspectionNo == bookid);
                            break;
                        }
                }
            }

            //filter by creation date or service date or firstservicedate
            if (Enum.TryParse(request.DateTypeid.ToString(), out SearchType _datesearchtype) && (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null))
            {
                switch (_datesearchtype)
                {
                    case SearchType.ClaimDate:
                        {
                            claimQuery = claimQuery.Where(x => EF.Functions.DateDiffDay(request.FromDate.ToDateTime(), x.ClaimDate) >= 0 &&
                                                 EF.Functions.DateDiffDay(x.ClaimDate, request.ToDate.ToDateTime()) >= 0);
                            break;
                        }
                    case SearchType.ServiceDate:
                        {
                            claimQuery = claimQuery.Where(x => !((x.InspectionNoNavigation.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.InspectionNoNavigation.ServiceDateTo < request.FromDate.ToDateTime())));
                            break;
                        }
                }
            }

            if (request.CustomerId > 0)
            {
                claimQuery = claimQuery.Where(x => x.InspectionNoNavigation.CustomerId == request.CustomerId);
            }

            //apply supplier filter
            if (request.SupplierId > 0)
            {
                claimQuery = claimQuery.Where(x => x.InspectionNoNavigation.SupplierId == request.SupplierId);
            }

            if (request.StatusIdlst != null && request.StatusIdlst.Any())
            {
                claimQuery = claimQuery.Where(x => request.StatusIdlst.ToList().Contains(x.StatusId.Value));
            }

            if (request.Officeidlst != null && request.Officeidlst.Any())
            {
                claimQuery = claimQuery.Where(x => x.InspectionNoNavigation.OfficeId != null && request.Officeidlst.ToList().Contains(x.InspectionNoNavigation.OfficeId.Value));
            }

            if (request?.CountryId > 0)
            {
                claimQuery = claimQuery.Where(x => x.InspectionNoNavigation.Factory.SuAddresses.Any(y => y.CountryId == request.CountryId));
            }

            return claimQuery;
        }

        public async Task<List<ClaimItemExportSummary>> ExportSummary(IEnumerable<ClaimExportItem> data)
        {
            return _clmMap.MapExportSummary(data);
        }
        public async Task<ClaimCancelResponse> CancelClaim(int id)
        {
            var claimData = await _repo.GetClaimDetails(id);
            if (claimData != null)
            {
                claimData.DeletedBy = _ApplicationContext.UserId;
                claimData.DeletedOn = DateTime.Now;
                claimData.StatusId = (int)ClaimStatus.Cancelled;
                int re = await _repo.EditClaim(claimData);
                return new ClaimCancelResponse { Id = claimData.Id, Result = ClaimCancelResult.Success };
            }
            return new ClaimCancelResponse { Id = 0, Result = ClaimCancelResult.NotFound };
        }

        private async Task<List<ClaimsStatus>> GetClaimStatusList(IQueryable<ClmTransaction> claimData)
        {
            return await claimData.Select(x => new { x.StatusId, x.Status.Name, x.Id })
                   .GroupBy(p => new { p.StatusId, p.Name }, p => p, (key, _data) =>
                  new ClaimsStatus
                  {
                      Id = key.StatusId.GetValueOrDefault(),
                      StatusName = key.Name,
                      TotalCount = _data.Count(),
                      StatusId = key.StatusId.GetValueOrDefault()
                  }).OrderBy(x => x.StatusId).ToListAsync();
        }

        public async Task<ClaimSearchExportResponse> GetExportClaimDetails(ClaimSearchRequest request)
        {
            try
            {
                if (request == null)
                    return new ClaimSearchExportResponse() { Result = ClaimSearchResponseResult.NotFound };

                var response = new ClaimSearchExportResponse { };

                if (request.Index == null || request.Index.Value <= 0)
                    request.Index = 1;

                if (request.pageSize == null || request.pageSize.Value == 0)
                    request.pageSize = 10;

                int skip = (request.Index.Value - 1) * request.pageSize.Value;

                int take = request.pageSize.Value;

                // get the claim Query 
                var claimQuery = await _repo.GetAllClaimsQuery();
                var claimData = GetClaimQuerywithRequestFilters(request, claimQuery);

                var data = claimData.Select(x => new ClaimExportItem
                {
                    ClaimId = x.Id,
                    ClaimNo = x.ClaimNo,
                    ClaimDate = x.ClaimDate,
                    Inspection = x.InspectionNo,
                    CustomerName = x.InspectionNoNavigation.Customer.CustomerName,
                    ClaimSource = x.ClaimSourceNavigation.Name,
                    ClaimReceivedFrom = x.ReceivedFromNavigation.Name,
                    ClaimDescription = x.ClaimDescription,
                    CustomerRequestPriority = x.CustomerPriorityNavigation.Name,
                    Amount = x.CustomerReqRefundAmount,
                    CustomerReqRefundCurrency = x.CustomerReqRefundCurrencyNavigation.CurrencyCodeA,
                    SupplierName = x.InspectionNoNavigation.Supplier.SupplierName,
                    ServiceDateFrom = x.InspectionNoNavigation.ServiceDateFrom,
                    ServiceDateTo = x.InspectionNoNavigation.ServiceDateTo,
                    Office = x.InspectionNoNavigation.Office.LocationName,
                    OfficeCountry = x.InspectionNoNavigation.Office.City.Province.Country.CountryName,
                    InspectionTransaction = x.InspectionNoNavigation,
                    Remarks = x.CustomerComments,
                    AnalyzerFeedback = x.AnalyzerFeedback,
                    Color = x.Color,
                    FobPrice = x.FobPrice,
                    FobCurrency = x.FobCurrencyNavigation.CurrencyCodeA,
                    RetailPrice = x.RetailPrice,
                    RetailCurrency = x.RetailCurrencyNavigation.CurrencyCodeA,
                    ClaimRecommendation = x.ClaimRecommendation,
                    ClaimValidateResult = x.ClaimValidateResult,
                    ValidatorReviewComment = x.ClaimRemarks,
                    FinalAmount = x.ClaimRefundAmount,
                    FinalCurrency = x.ClaimRefundCurrencyNavigation.CurrencyCodeA,
                    CreatedBy = x.CreatedByNavigation.FullName,
                    CreatedDate = x.CreatedOn,
                    AnalyzedBy = x.AnalyzedByNavigation.FullName,
                    AnalyzedDate = x.AnalyzedOn,
                    ValidatedBy = x.ValidatedByNavigation.FullName,
                    ValidatedDate = x.ValidatedOn,
                    ClosedBy = x.ClosedByNavigation.FullName,
                    ClosedDate = x.ClosedOn,
                    ProductCategory = x.InspectionNoNavigation.ProductCategory.Name,
                    ProductSubCategory = x.InspectionNoNavigation.ProductSubCategory.Name,
                    Factory = x.InspectionNoNavigation.Factory.SupplierName,
                    RealInspectionFees = x.RealInspectionFees,
                    RealInspectionFeesCurrency = x.RealInspectionFeesCurrencyNavigation.CurrencyCodeA,
                    ClmTranReports = x.ClmTranReports,
                    ClmTranCustomerRequests = x.ClmTranCustomerRequests,
                    ClmTranCustomerRequestRefunds = x.ClmTranCustomerRequestRefunds,
                    ClmTranDefectFamilies = x.ClmTranDefectFamilies,
                    ClmTranDepartments = x.ClmTranDepartments,
                    ClmTranFinalDecisions = x.ClmTranFinalDecisions,
                    ClmTranClaimRefunds = x.ClmTranClaimRefunds,
                    StatusName = x.Status.Name
                });

                var result = await data.Skip(skip).Take(take).AsNoTracking().ToListAsync();

                response.TotalCount = await data.AsNoTracking().CountAsync();

                var allClaimIdAsQuery = result.Select(x => x.ClaimId);

                var allInspectionIds = result.Select(x => x.Inspection.GetValueOrDefault()).ToList();

                var reportList = await _inspRepo.GetInspectionReportData(allInspectionIds);

                var defectCategoryList = await _repo.GetFbDefectList();

                var claimDepartmentList = await _repo.GetClaimDepartmentList();

                var claimCustomerRequestList = await _repo.GetClaimCustomerRequestList();

                var claimRefundTypeList = await _repo.GetClaimRefundTypeList();

                var claimResultList = await _repo.GetClaimResultList();

                var finalResultList = await _repo.GetClaimFinalResultList();

                var qcDetails = await _repo.GetQcDetails(allClaimIdAsQuery);

                var productList = await _inspRepo.GetProductListByBooking(allInspectionIds);

                var invoiceList = await _repo.GetClaimInvoiceDetailByBookingIds(allInspectionIds);

                var countryList = await _inspRepo.GetFactorycountryId(allInspectionIds);

                var fbReportQuantityData = await _repo.GetFbQuantityDataByBookingIds(allInspectionIds);


                var _resultdata = result.Select(x =>
                _clmMap.GetClaimSearchResult(x, defectCategoryList, claimCustomerRequestList, claimDepartmentList, claimRefundTypeList, claimResultList,
                    finalResultList, qcDetails, reportList, productList, invoiceList, countryList, fbReportQuantityData)).ToList();

                if (response.TotalCount == 0)
                {
                    response.Result = ClaimSearchResponseResult.NotFound;
                    return response;
                }

                return new ClaimSearchExportResponse()
                {
                    Result = ClaimSearchResponseResult.Success,
                    TotalCount = response.TotalCount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                    Data = _resultdata
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// pending claim data summary 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PendingClaimSummaryResponse> GetPendingClaims(PendingClaimSearchRequest request)
        {
            if (request == null)
                return new PendingClaimSummaryResponse() { Result = CreditNoteResult.NotFound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;
            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            int take = request.pageSize.Value;
            var response = new PendingClaimSummaryResponse();
            var data = _repo.GetPendingClaims();
            if (request.CustomerId.HasValue && request.CustomerId.Value > 0)
            {
                data = data.Where(x => x.CustomerId == request.CustomerId);
            }

            if (request.FromDate != null && request.ToDate != null)
            {
                var claimFromDate = request.FromDate.ToDateTime();
                var claimToDate = request.ToDate.ToDateTime();
                data = data.Where(x => x.ClaimDate >= claimFromDate && x.ClaimDate <= claimToDate);
            }
            if (request.SearchTypeId.HasValue && request.SearchTypeId.Value > 0)
            {
                switch (request.SearchTypeId.Value)
                {
                    case (int)SearchType.BookingNo:
                        if (!string.IsNullOrEmpty(request.SearchTypeText))
                            data = data.Where(x => x.InspectionId.HasValue && x.InspectionId.Value.ToString().Contains(request.SearchTypeText));
                        break;
                    case (int)SearchType.ClaimNo:
                        if (!string.IsNullOrEmpty(request.SearchTypeText))
                            data = data.Where(x => x.ClaimNo.Contains(request.SearchTypeText));
                        break;
                }
            }


            if (request.OfficeId.HasValue && request.OfficeId.Value > 0)
            {
                data = data.Where(x => x.OfficeId == request.OfficeId);
            }

            response.TotalCount = await data.AsNoTracking().CountAsync();

            if (response.TotalCount == 0)
            {
                response.Result = CreditNoteResult.NotFound;
                return response;
            }
            var result = await data.Skip(skip).Take(take).AsNoTracking().ToListAsync();

            var bookingIds = data.Where(x => x.InspectionId > 0).Select(x => x.InspectionId.Value).Distinct().ToList();
            var invoices = await _invoiceRepository.GetInvoiceListByBookingId(bookingIds);
            var serviceTypeList = await _inspectionBookingRepository.GetServiceType(bookingIds);
            var products = await _reportRepository.GetProductListByBooking(bookingIds);
            response.Data = result.Select(x => _clmMap.MapPendingClaimSummary(x, products, invoices, serviceTypeList)).ToList();
            response.Index = request.Index.Value;
            response.PageSize = request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
            response.Result = CreditNoteResult.Success;
            response.IsAccountingCreditNoteRole = _ApplicationContext.RoleList.Contains((int)RoleEnum.AccountingCreditNote);
            return response;
        }

        /// <summary>
        /// save credit note 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveCreditNoteResponse> SaveCreditNote(SaveCreditNote request)
        {
            var response = new SaveCreditNoteResponse();
            //check credit number exist or not
            var isCreditNoExist = await this.CheckCreditNumberExist(request.CreditNo);
            if (isCreditNoExist)
            {
                response.Result = CreditNoteResult.CreditNoAlreadyExist;
                return response;
            }
            var userId = _ApplicationContext.UserId;
            var entityId = _filterService.GetCompanyId();
            //map credit entity
            var invCreTransaction = _clmMap.MapCreditEntity(request, userId, entityId);
            _repo.AddEntity<InvCreTransaction>(invCreTransaction);
            //if contact persion available in request
            if (request.ContactPersons != null && request.ContactPersons.Any())
            {
                foreach (var contactPerson in request.ContactPersons)
                {
                    var invCreTranContact = _clmMap.MapCreditContactEntity(contactPerson, userId);
                    invCreTransaction.InvCreTranContacts.Add(invCreTranContact);
                    _repo.AddEntity(invCreTranContact);
                }
            }
            //if credit claim items availabe 
            if (request.SaveCreditNotes != null && request.SaveCreditNotes.Any())
            {
                //credit claim items 
                foreach (var creditNoteItem in request.SaveCreditNotes)
                {
                    var claimDetail = _clmMap.MapInvCreTranClaimDetailEntity(creditNoteItem, userId);
                    invCreTransaction.InvCreTranClaimDetails.Add(claimDetail);
                    _repo.AddEntity(claimDetail);
                }
            }


            await _repo.Save();

            response.Result = CreditNoteResult.Success;
            return response;
        }
        /// <summary>
        /// get pending claim data 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<GetPendingClaimResponse> GetPendingClaimData(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
                return new GetPendingClaimResponse() { Result = CreditNoteResult.NotFound };
            var data = await _repo.GetPendingClaims().Where(x => ids.Contains(x.ClaimId)).AsNoTracking().ToListAsync();
            if (!data.Any())
                return new GetPendingClaimResponse() { Result = CreditNoteResult.NotFound };

            if (data.Select(x => x.CustomerId).Distinct().Count() > 1)
            {
                return new GetPendingClaimResponse() { Result = CreditNoteResult.SameCustomer };
            }

            var bookingIds = data.Where(x => x.InspectionId > 0).Select(x => x.InspectionId.Value).Distinct().ToList();
            var products = await _reportRepository.GetProductListByBooking(bookingIds);
            var invoices = await _invoiceRepository.GetInvoiceDetailByBookingIds(bookingIds);
            var result = data.Select(x => _clmMap.MapGetPendingClaimData(x, products, invoices));
            var claim = data.FirstOrDefault();

            return new GetPendingClaimResponse()
            {
                Result = CreditNoteResult.Success,
                Data = result,
                CustomerId = claim?.CustomerId,
                CustomerName = claim?.Customer,
                IsAccountingCreditNoteRole = _ApplicationContext.RoleList.Contains((int)RoleEnum.AccountingCreditNote)
            };
        }

        /// <summary>
        /// Credit note summary 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreditNoteSummaryResponse> GetCreditNoteSummary(CreditNoteSearchRequest request)
        {
            if (request == null)
                return new CreditNoteSummaryResponse() { Result = CreditNoteResult.NotFound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;
            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;
            int take = request.PageSize.Value;
            var response = new CreditNoteSummaryResponse();
            var data = _repo.GetCreditNotes();

            if (!string.IsNullOrWhiteSpace(request.CreditNo))
            {
                request.CreditNo = request.CreditNo.Trim();
                data = data.Where(x => x.CreditNo.Trim() == request.CreditNo);
            }


            if (request.FromDate != null && request.ToDate != null)
            {
                var fromDate = request.FromDate.ToDateTime();
                var toDate = request.ToDate.ToDateTime();
                data = data.Where(x => x.CreditDate >= fromDate && x.CreditDate <= toDate);
            }

            if (request.CreditType.HasValue && request.CreditType.Value > 0)
                data = data.Where(x => x.CreditTypeId == request.CreditType);

            response.TotalCount = await data.AsNoTracking().CountAsync();
            if (response.TotalCount == 0)
            {
                response.Result = CreditNoteResult.NotFound;
                return response;
            }

            var result = await data.Skip(skip).Take(take).AsNoTracking().ToListAsync();
            var creditClaimDetails = await _repo.GetCreditNoteTransactionDetails(result.Select(x => x.Id).ToList());
            response.Data = result.Select(x => _clmMap.MapCreditNoteSummary(x, creditClaimDetails)).ToList();
            response.Index = request.Index.Value;
            response.PageSize = request.PageSize.Value;
            response.PageCount = (response.TotalCount / request.PageSize.Value) + (response.TotalCount % request.PageSize.Value > 0 ? 1 : 0);
            response.Result = CreditNoteResult.Success;
            response.IsAccountingCreditNoteRole = _ApplicationContext.RoleList.Contains((int)RoleEnum.AccountingCreditNote);
            return response;
        }

        /// <summary>
        /// check credit number exist or not
        /// </summary>
        /// <param name="creditNo"></param>
        /// <returns></returns>
        public async Task<bool> CheckCreditNumberExist(string creditNo)
        {
            //credit number check in exist invoice tables
            var isCreditNoExist = await _manualInvoiceManager.CheckInvoiceNumberExist(creditNo);
            //if credit number is not available then check to credit table
            if (!isCreditNoExist)
                //credit number check to credit table
                isCreditNoExist = await _repo.IsCreditNoExist(creditNo);
            return isCreditNoExist;
        }

        //get credit type list 
        public async Task<DataSourceResponse> GetCreditTypeList()
        {
            var finalResultList = await _repo.GetCreditTypeList();

            if (finalResultList == null || !finalResultList.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = finalResultList,
                Result = DataSourceResult.Success
            };
        }

        /// <summary>
        /// Get Credit note 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditCreditNoteResponse> EditCreditNote(int id)
        {
            var response = new EditCreditNoteResponse();
            //get credit note by id
            var invCreTransaction = await _repo.GetSingleAsync<InvCreTransaction>(x => x.Id == id && x.Active.HasValue && x.Active.Value);
            if (invCreTransaction == null)
            {
                response.Result = CreditNoteResult.NotFound;
                return response;
            }
            //get credit claim details
            var claimDetails = await _repo.GetCreditClaimDetailsByCreditIds(new List<int?>() { invCreTransaction.Id });
            //if claim details available
            if (claimDetails.Any())
            {
                var inspection = await _sharedInspectionRepo.GetInspectionsQuery(claimDetails.FirstOrDefault().InspectionId).FirstOrDefaultAsync();
                var products = await _reportRepository.GetProductListByBooking(claimDetails.Select(y => y.InspectionId).ToList());
                var contacts = await _repo.GetCreditContacts(new List<int?>() { invCreTransaction.Id });
                response.CreditNote = _clmMap.MapEditCreditNote(invCreTransaction, contacts.Select(y => y.CustomerContactId), inspection.CustomerId);
                response.CreditNote.SaveCreditNotes = claimDetails.Select(x => _clmMap.MapEditCreditNoteItem(x, products));
            }
            response.IsAccountingCreditNoteRole = _ApplicationContext.RoleList.Contains((int)RoleEnum.AccountingCreditNote);
            response.Result = CreditNoteResult.Success;
            return response;
        }

        public async Task<SaveCreditNoteResponse> UpdateCreditNote(SaveCreditNote request)
        {
            var response = new SaveCreditNoteResponse();
            var invCreTransaction = await _repo.GetSingleAsync<InvCreTransaction>(x => x.Id == request.Id);
            if (invCreTransaction == null)
            {
                response.Result = CreditNoteResult.NotFound;
                return response;
            }
            if (invCreTransaction.CreditNo != request.CreditNo)
            {
                if (await this.CheckCreditNumberExist(request.CreditNo))
                {
                    response.Result = CreditNoteResult.CreditNoAlreadyExist;
                    return response;
                }
            }

            var userId = _ApplicationContext.UserId;
            _clmMap.MapCreditNoteEntity(invCreTransaction, request, userId);
            _repo.EditEntity(invCreTransaction);

            var claimDetails = await _repo.GetCreditTranClaimDetails(new List<int>() { invCreTransaction.Id });
            var contactPersons = await _repo.GetCreditContacts(new List<int?>() { invCreTransaction.Id });

            var creditClaimIds = request.SaveCreditNotes.Select(x => x.Id);

            if (request.SaveCreditNotes != null && request.SaveCreditNotes.Any())
            {
                //Delete Claims
                var deleteCreditClaims = claimDetails.Where(x => !creditClaimIds.Contains(x.Id));
                if (deleteCreditClaims.Any())
                {
                    deleteCreditClaims.ToList().ForEach(x =>
                    {
                        x.Active = false;
                        x.DeletedBy = userId;
                        x.DeletedOn = DateTime.Now;
                    });

                    _repo.EditEntities(deleteCreditClaims);
                }

                var lstCreditClaim = new List<InvCreTranClaimDetail>();
                foreach (var item in request.SaveCreditNotes.Where(x => x.Id > 0))
                {
                    var creditClaim = claimDetails.FirstOrDefault(x => x.Id == item.Id);

                    if (creditClaim != null)
                    {
                        _clmMap.MapCreditNoteItemEntity(creditClaim, item, userId);
                        lstCreditClaim.Add(creditClaim);
                    }
                    else
                        return new SaveCreditNoteResponse { Result = CreditNoteResult.NotFound };
                }

                if (lstCreditClaim.Count > 0)
                    _repo.EditEntities(lstCreditClaim);
            }

            if (request.ContactPersons != null && request.ContactPersons.Any())
            {
                var deleteContactPersons = contactPersons.Where(x => !request.ContactPersons.Contains(x.CustomerContactId.Value));
                if (deleteContactPersons.Any())
                {
                    deleteContactPersons.ToList().ForEach(x =>
                    {
                        x.Active = false;
                        x.DeletedBy = userId;
                        x.DeletedOn = DateTime.Now;
                    });

                    _repo.EditEntities(deleteContactPersons);
                }

                var dbContactPersonIds = contactPersons.Select(x => x.CustomerContactId);
                var newContactPersonIds = request.ContactPersons.Where(x => !dbContactPersonIds.Contains(x));
                foreach (var contactPersonId in newContactPersonIds)
                {
                    var creditContactPerson = _clmMap.MapCreditContactEntity(contactPersonId, userId);
                    _repo.AddEntity(creditContactPerson);
                    invCreTransaction.InvCreTranContacts.Add(creditContactPerson);
                }
            }
            else
            {
                contactPersons.ToList().ForEach(x =>
                {
                    x.Active = false;
                    x.DeletedBy = userId;
                    x.DeletedOn = DateTime.Now;
                });
                _repo.EditEntities(contactPersons);
            }

            await _repo.Save();
            response.Result = CreditNoteResult.Success;
            return response;
        }

        public async Task<DeleteCreditNoteResponse> DeleteCreditNote(int id)
        {
            var invCreTransaction = await _repo.GetSingleAsync<InvCreTransaction>(x => x.Id == id);
            if (invCreTransaction == null)
            {
                return new DeleteCreditNoteResponse() { Result = CreditNoteResult.NotFound };
            }
            // delete inv cre transaction
            invCreTransaction.Active = false;
            invCreTransaction.DeletedBy = _ApplicationContext.UserId;
            invCreTransaction.DeletedOn = DateTime.Now;

            var tranClaims = await _repo.GetCreditTranClaimDetails(new List<int>() { invCreTransaction.Id });
            if (tranClaims.Any(x => x.Active == true))
            {
                tranClaims.ForEach(x =>
                {
                    x.Active = false;
                    x.DeletedBy = _ApplicationContext.UserId;
                    x.DeletedOn = DateTime.Now;
                });

                _repo.EditEntities(tranClaims);
            }

            var contacts = await _repo.GetCreditContacts(new List<int?>() { invCreTransaction.Id });
            if (contacts.Any(x => x.Active == true))
            {
                contacts.ForEach(x =>
                {
                    x.Active = false;
                    x.DeletedBy = _ApplicationContext.UserId;
                    x.DeletedOn = DateTime.Now;
                });

                _repo.EditEntities(contacts);
            }

            _repo.EditEntity(invCreTransaction);
            await _repo.Save();

            return new DeleteCreditNoteResponse() { Result = CreditNoteResult.Success };
        }
        public async Task<List<ExportCreditNoteSummary>> ExportCreditNoteSummary(CreditNoteSearchRequest request)
        {
            if (request == null)
                return null;

            var data = _repo.GetExportCreditNotes();
            if (!string.IsNullOrEmpty(request.CreditNo))
                data = data.Where(x => x.CreditNo.Contains(request.CreditNo));

            if (request.FromDate != null && request.ToDate != null)
            {
                var fromDate = request.FromDate.ToDateTime();
                var toDate = request.ToDate.ToDateTime();
                data = data.Where(x => x.CreditDate >= fromDate && x.CreditDate <= toDate);
            }

            if (request.CreditType.HasValue && request.CreditType.Value > 0)
                data = data.Where(x => x.CreditTypeId == request.CreditType);

            var result = await data.AsNoTracking().OrderBy(x => x.CreditNo).Select(x => _clmMap.MapExportCreditNoteSummary(x)).ToListAsync();
            return result;
        }

        /// <summary>
        /// get the credit note numbers
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCreditNoteNos()
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _repo.GetCreditNotes();

                var items = await data.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.CreditNo
                }).ToListAsync();

                if (items == null || !items.Any())
                    response.Result = DataSourceResult.CannotGetList;

                response.DataSourceList = items;
                response.Result = DataSourceResult.Success;

                return response;

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
