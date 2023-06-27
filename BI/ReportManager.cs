using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Inspection;
using DTO.RepoRequest.Enum;
using DTO.Report;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class ReportManager : ApiCommonData, IReportManager
    {
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IReportRepository _repo = null;
        private readonly ILogger<ScheduleManager> _logger = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly ICustomerManager _customerManager = null;
        private readonly IFullBridgeRepository _fbRepo = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IHumanResourceRepository _humanResourceRepository = null;
        private readonly IScheduleRepository _scheduleRepo = null;
        private readonly ISharedInspectionManager _sharedInspection = null;
        private readonly ReportMap ReportMap = null;
        private readonly BookingMap BookingMap = null;
        private readonly IKpiCustomRepository _kpiCustomRepository = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IOtherMandayRepository _otherMandayRepository = null;
        private readonly IInspectionCustomerDecisionRepository _inspectionCustomerDecisionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ISupplierManager _supplierManager = null;

        public ReportManager
            (
            IOtherMandayRepository otherMandayRepository,
            IReportRepository repo,
            IAPIUserContext applicationContext,
            ILogger<ScheduleManager> logger,
            IOfficeLocationManager office,
            ICustomerManager customerManager,
            IFullBridgeRepository fbRepo,
            IInspectionBookingRepository inspRepo,
            IScheduleRepository scheduleRepo,
            IHumanResourceRepository humanResourceRepository,
            ISharedInspectionManager sharedInspection,
            IKpiCustomRepository kpiCustomRepository,
            IInspectionCustomerDecisionRepository inspectionCustomerDecisionRepository,
            ICustomerRepository customerRepository,
            IInvoiceRepository invoiceRepository,
            ISupplierManager supplierManager,
            ITenantProvider filterService)
        {
            _ApplicationContext = applicationContext;
            _repo = repo;
            _logger = logger;
            _office = office;
            _customerManager = customerManager;
            _fbRepo = fbRepo;
            _inspRepo = inspRepo;
            _scheduleRepo = scheduleRepo;
            _humanResourceRepository = humanResourceRepository;
            _sharedInspection = sharedInspection;
            ReportMap = new ReportMap();
            BookingMap = new BookingMap();
            _kpiCustomRepository = kpiCustomRepository;
            _filterService = filterService;
            _otherMandayRepository = otherMandayRepository;
            _inspectionCustomerDecisionRepository = inspectionCustomerDecisionRepository;
            _customerRepository = customerRepository;
            _invoiceRepository = invoiceRepository;
            _supplierManager = supplierManager;
        }

        /// <summary>
        /// Map the initial booking summary request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<InspectionSummarySearchRequest> GetInspectionSummaryRequest(InspectionSummarySearchRequest request)
        {
            //filter data based on user type
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        request.CustomerId = request?.CustomerId != null && request?.CustomerId != 0 ? request?.CustomerId : _ApplicationContext.CustomerId;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        request.FactoryIdlst = request.FactoryIdlst != null && request.FactoryIdlst.Any() ? request.FactoryIdlst : new List<int>().Append(_ApplicationContext.FactoryId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        request.SupplierId = request.SupplierId != null && request.SupplierId != 0 ? request.SupplierId.Value : _ApplicationContext.SupplierId;
                        break;
                    }
            }

            request.CustomerList = new List<int>();

            //if logged in user type is internal user
            if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
            {
                var _cusofficelist = await _office.GetOnlyOfficeIdsByUser(_ApplicationContext.StaffId);

                if (_cusofficelist.Any())
                {
                    if (request.Officeidlst != null && request.Officeidlst.Any())
                    {
                        request.Officeidlst = _cusofficelist.Where(x => request.Officeidlst.Contains(x)).Select(x => (int?)x).ToList();
                    }
                    else
                    {
                        request.Officeidlst = _cusofficelist.Select(x => (int?)x).ToList();
                    }
                }

                if (request.CustomerId > 0)
                    request.CustomerList.Add(request.CustomerId.Value);
            }
            else
            {
                if (request.CustomerId > 0)
                    request.CustomerList.Add(request.CustomerId.Value);
            }

            return request;

        }

        /// <summary>
        /// Execute the booking query and get the inspection status list
        /// </summary>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private async Task<List<InspectionStatus>> GetInspectionStatusList(IQueryable<InspTransaction> bookingData)
        {
            return await bookingData.Select(x => new { x.StatusId, x.Status.Status, x.Id, x.Status.Priority })
                   .GroupBy(p => new { p.StatusId, p.Status, p.Priority }, p => p, (key, _data) =>
                 new InspectionStatus
                 {
                     Id = key.StatusId,
                     StatusName = key.Status,
                     TotalCount = _data.Count(),
                     Priority = key.Priority
                 }).OrderBy(x => x.Priority).ToListAsync();
        }

        /// <summary>
        /// If api call is for filling&review then apply qc filter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private IQueryable<InspTransaction> ApplyQcFilterForBooking(InspectionSummarySearchRequest request, IQueryable<InspTransaction> bookingData)
        {
            if (request.CallingFrom == (int)PageTypeSummary.FillingReview && _ApplicationContext.UserProfileList.Contains((int)HRProfile.Inspector))
            {
                // fetch the staff based on the staff profiles
                var qcStaffList = _humanResourceRepository.GetStaffIdsByProfileAndParentStaff(_ApplicationContext.StaffId, (int)HRProfile.Inspector);
                if (_ApplicationContext.RoleList != null && !_ApplicationContext.RoleList.Contains((int)RoleEnum.ReportChecker))
                {
                    bookingData = bookingData.Where(x => x.SchScheduleQcs.Any(y => y.Active && qcStaffList.Contains(y.Qcid)));
                }
                else
                {
                    bookingData = bookingData.Where(x => x.SchScheduleQcs.Any(y => y.Active && qcStaffList.Contains(y.Qcid)) || x.SchScheduleCS.Any(y => y.Active && _ApplicationContext.StaffId == y.Csid));
                }

            }

            return bookingData;
        }

        /// <summary>
        /// Get the inspection booking items
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private async Task<List<CustomerReportBookingValues>> GetInspectionBookingData(IQueryable<InspTransaction> bookingData, int skip, int take)
        {
            return await bookingData.Select(x => new CustomerReportBookingValues
            {
                BookingId = x.Id,
                CustomerBookingNo = x.CustomerBookingNo,
                CustomerId = x.CustomerId,
                FactoryId = x.FactoryId,
                SupplierId = x.SupplierId,
                missionId = x.FbMissionId,
                OfficeId = x.OfficeId,
                Office = x.Office.LocationName,
                CustomerName = x.Customer.CustomerName,
                SupplierName = x.Supplier.SupplierName,
                FactoryName = x.Factory.SupplierName,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo,
                StatusId = x.StatusId,
                StatusName = x.Status.Status,
                StatusPriority = x.Status.Priority,
                IsPicking = x.IsPickingRequired,
                IsEAQF = x.IsEaqf,
                PreviousBookingNo = x.PreviousBookingNo,
                Status = x.Status,
                MissionStatus = x.FbMissionStatusNavigation.StatusName,
                BookingType = x.BookingType,
                ReportDate = x.InspTranStatusLogs.Where(z => z.StatusId == (int)BookingStatus.ReportSent).OrderBy(z => z.CreatedOn).Select(z => z.StatusChangeDate).FirstOrDefault()
            }).OrderByDescending(x => x.ServiceDateFrom).Skip(skip).Take(take).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Inspection search function for customerreport,fillingreview and fullbridge
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CustomerReportSummaryResponse> GetAllInspectionReports(InspectionSummarySearchRequest request)
        {
            if (request == null)
                return new CustomerReportSummaryResponse() { Result = CustomerReportSummaryResponseResult.NotFound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            var response = new BookingSummarySearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            //get the booking summary request
            request = await GetInspectionSummaryRequest(request);

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetInspectionQueryRequestMap(request);

            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            //apply qc staff id filter if calling  page is filling and review
            bookingData = ApplyQcFilterForBooking(request, bookingData);

            //apply df filter if calling page is filling and review
            bookingData = ApplyDfFilterForBooking(request, bookingData);

            //assign the total count
            response.TotalCount = await bookingData.Select(x => x.Id).CountAsync();

            //execute the status list in the booking summary page
            var inspectionStatusList = await GetInspectionStatusList(bookingData);

            if (request.CallingFrom == (int)PageTypeSummary.CustomerReport)
            {
                inspectionStatusList.ForEach(x => x.StatusColor = BookingSummaryInspectionStatusColor.GetValueOrDefault(x.StatusId, ""));
            }
            else
            {
                inspectionStatusList.ForEach(x => x.StatusColor = InspectionStatusColor.GetValueOrDefault(x.StatusId, ""));
            }

            var inspectionBookingItems = await GetInspectionBookingData(bookingData, skip, take);

            if (inspectionBookingItems == null || !inspectionBookingItems.Any())
                return new CustomerReportSummaryResponse() { Result = CustomerReportSummaryResponseResult.NotFound };

            var bookingIds = inspectionBookingItems.Select(x => x.BookingId).ToList();

            var inspectionCsList = await _inspRepo.GetInspectionTransCsDetails(bookingIds);

            //get the po details
            var poDetails = await _inspRepo.GetBookingPOTransactionDetails(bookingIds);

            //get container details
            var containerList = await _inspRepo.GetBookingContainer(bookingIds);

            //Get the service Type for the bookings
            var serviceTypeList = await _inspRepo.GetServiceType(bookingIds);

            //Get the inspected status list
            var inspectedStatusList = InspectedStatusList.Select(x => x);

            var bookingReportSummaryLink = await _repo.GetBookingReportSummaryLinkData(bookingIds, inspectedStatusList);

            //get customer id list
            var customerIdList = inspectionBookingItems?.Where(x => x.CustomerId > 0).Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();

            //get supplier id list
            var supplierIdList = inspectionBookingItems?.Where(x => x.SupplierId > 0).Select(x => x.SupplierId.GetValueOrDefault()).Distinct().ToList();

            //get supplier code list
            var supplierCodeList = await _supplierManager.GetSupplierCode(customerIdList, supplierIdList);

            var _resultdata = inspectionBookingItems.Select(x => ReportMap.GetInspectionReportData(x, inspectionStatusList, serviceTypeList, bookingReportSummaryLink, poDetails,
                containerList, inspectionCsList, supplierCodeList));

            IEnumerable<InspectionStatus> _statuslist;

            if (request.CallingFrom == (int)PageTypeSummary.CustomerReport)
            {
                _statuslist = inspectionStatusList.Select(x => BookingMap.GetBookingStatuswithColor(x));
            }
            else
            {
                _statuslist = inspectionStatusList.Select(x => BookingMap.GetBookingStatusMap(x));
            }

            return new CustomerReportSummaryResponse()
            {
                Result = CustomerReportSummaryResponseResult.Success,
                TotalCount = response.TotalCount,
                Index = request.Index.Value,
                PageSize = request.pageSize.Value,
                PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                Data = _resultdata,
                InspectionStatuslst = _statuslist
            };


        }

        //Get Products based on booking on booking expand in the UI
        public async Task<IEnumerable<ReportProductItem>> GetProductsByBooking(int bookingId)
        {
            var res = await _repo.GetProductsByBooking(bookingId);
            var poList = await _repo.GetPONumbersbyBooking(bookingId);
            var reportIds = res.Select(x => x.ApiReportId).ToList();
            var fbReportInfoList = await _fbRepo.GetFbReportStatusListCustomerReportbyBooking(reportIds);
            var qcList = await _repo.GetQCDetails(bookingId);
            var data = res.Select(x => ReportMap.GetProductList(x, fbReportInfoList, res, qcList, poList)).OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ThenBy(x => x.ProductName);
            return data;
        }


        //Get Products based on booking on booking expand in the UI
        public async Task<IEnumerable<ReportProductItem>> GetContainersByBooking(int bookingId)
        {
            var res = await _repo.GetContainersByBooking(bookingId);
            var reportId = res.Select(x => x.ApiReportId).ToList();
            var poList = await _repo.GetPONumbersbyBooking(bookingId);
            var fbReportInfoList = await _fbRepo.GetFbReportStatusListCustomerReportbyBooking(reportId);
            var qcList = await _repo.GetQCDetails(bookingId);
            var data = res.Select(x => ReportMap.GetProductList(x, fbReportInfoList, res, qcList, poList)).OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ThenBy(x => x.ProductName);
            return data;
        }

        /// <summary>
        /// Export report data (filling review/customer report)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExportReportData> ExportReportDataSummary(InspectionSummarySearchRequest request)
        {
            ExportReportData reportData = new ExportReportData();

            var containerServiceType = (int)InspectionServiceTypeEnum.Container;

            //get the booking summary request
            request = await GetInspectionSummaryRequest(request);

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetInspectionQueryRequestMap(request);

            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            //apply qc staff id filter if calling  page is filling and review
            bookingData = ApplyQcFilterForBooking(request, bookingData);

            //apply df filter if calling page is filling and review
            bookingData = ApplyDfFilterForBooking(request, bookingData);

            var bookingIds = bookingData.Select(x => x.Id);
            var inspectionCsList = await _inspRepo.GetInspectionTransCsDetails(bookingIds.ToList());

            //get the non container booking query
            var nonContainerBookingIds = bookingData.Where(x => x.InspTranServiceTypes.Any(y => y.Active && y.ServiceTypeId != containerServiceType)).
                       Select(x => x.Id);

            //get the inspection base data for the non container booking data along with the report details
            var productTransactionList = await _repo.GetBookingProductList(nonContainerBookingIds);

            //take container booking ids
            var containerBookingIds = bookingData.Where(x => x.InspTranServiceTypes.Any(y => y.Active && y.ServiceTypeId == containerServiceType)).
                      Select(x => x.Id);

            //get the inspection booking data for the container service bookings
            var containerTransactionList = await _repo.GetBookingContainerList(containerBookingIds);

            //get the fb report details for the bookingids
            var fbReportDetails = await _repo.GetExportFBReportData(bookingIds);

            //get the servicetypelist
            var serviceTypeList = await _inspRepo.GetServiceTypeList(bookingIds);

            //get the po details
            var poList = await _repo.GetPoListByBookingIds(bookingIds);

            //join the product vise booking data +container wise booking data
            var reportDataList = productTransactionList.Union(containerTransactionList).ToList();

            if (request.CallingFrom == (int)PageTypeSummary.CustomerReport)
            {
                //reportDataList - get cus id and sup id to get supplier code and map

                //get customer id list
                var customerIdList = reportDataList?.Select(x => x.CustomerId).Distinct().ToList();

                //get supplier id list
                var supplierIdList = reportDataList?.Select(x => x.SupplierId).Distinct().ToList();

                //get supplier code
                var supplierCodeList = await _supplierManager.GetSupplierCode(customerIdList, supplierIdList);

                //take the customer report data
                reportData.customerReportData = ReportMap.MapExportCustomerReport(reportDataList, serviceTypeList.ToList(), fbReportDetails, poList, supplierCodeList);
            }
            if (request.CallingFrom == (int)PageTypeSummary.FillingReview)
            {
                //take the filling report data
                reportData.fillingReportData = ReportMap.MapExportFillingReport(reportDataList, serviceTypeList.ToList(), fbReportDetails, poList, inspectionCsList.ToList());
            }

            return reportData;

        }

        //booking status update to report sent status table
        public async Task<CustomerReportSummaryResponseResult> BookingStatusUpdate(BookingStatusRequest request)
        {
            if (request == null)
                return CustomerReportSummaryResponseResult.Other;

            InspTranStatusLog inspStatusLog = new InspTranStatusLog()
            {
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                BookingId = request.BookingId,
                StatusId = (int)BookingStatus.ReportSent,
                StatusChangeDate = request.ReportDate.ToDateTime(),
                EntityId = _filterService.GetCompanyId()
            };

            //get booking details by booking id
            var bookingData = await _inspRepo.GetBookingTransaction(request.BookingId);

            bookingData.StatusId = (int)BookingStatus.ReportSent;

            //insert the new record in tran status log
            bookingData.InspTranStatusLogs.Add(inspStatusLog);
            _repo.AddEntity(inspStatusLog);

            //update the booking status in insp_transation table
            var bookingSuccess = await _inspRepo.EditInspectionBooking(bookingData);

            if (bookingSuccess > 0)
            {
                return CustomerReportSummaryResponseResult.Success;
            }
            else
                return CustomerReportSummaryResponseResult.Other;

        }
        /// <summary>
        /// Update custom report to fb report detail page and create log dat
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UploadCustomReportResponse> UpdateCustomReport(UploadCustomReportRequest request)
        {
            UploadCustomReportResponse response = new UploadCustomReportResponse() { Result = UploadCustomReportResult.NotSaved };
            if (request != null)
            {
                //get the fb report detail entity
                var fbReportDetail = await _repo.GetFBReportDetail(request.ApiReportId);
                if (fbReportDetail != null)
                {
                    //update the cloud url 
                    fbReportDetail.FinalManualReportPath = request.FileUrl;
                    //already exists deactivate the log
                    if (fbReportDetail.FbReportManualLogs != null && fbReportDetail.FbReportManualLogs.Count > 0)
                    {
                        foreach (var reportLog in fbReportDetail.FbReportManualLogs)
                        {
                            reportLog.Active = false;
                            reportLog.DeletedBy = _ApplicationContext.UserId;
                            reportLog.DeletedOn = DateTime.Now;
                            _repo.EditEntity(reportLog);
                        }
                    }
                    //create the new log
                    if (fbReportDetail.FbReportManualLogs != null)
                    {
                        var reportManualLog = new FbReportManualLog()
                        {
                            FbReportId = request.ApiReportId,
                            FileUrl = request.FileUrl,
                            FileName = request.FileName,
                            UniqueId = request.UniqueId,
                            Active = true,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            EntityId = _filterService.GetCompanyId()
                        };
                        fbReportDetail.FbReportManualLogs.Add(reportManualLog);
                    }

                    _repo.Save(fbReportDetail);

                    response.Result = UploadCustomReportResult.Success;

                }
            }

            return response;
        }

        /// <summary>
        /// get the inspection occupancy summary data 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<InspectionOccupancySummaryResponse> GetInspectionOccupancySummary(InspectionOccupancySearchRequest request)
        {
            if (request == null)
                return new InspectionOccupancySummaryResponse() { Result = InspectionOccupancyResult.NotFound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            var response = new InspectionOccupancySummaryResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            //get the staff data
            var inspectionOccupancies = _humanResourceRepository.GetInspectionOccupanceQuery();
            var fromDate = request.FromDate.ToDateTime();
            var toDate = request.ToDate.ToDateTime();

            //filter the data
            if (request.EmployeeType.HasValue && request.EmployeeType > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.EmployeeTypeId == request.EmployeeType);

            if (request.OfficeCountryId.HasValue && request.OfficeCountryId > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.OfficeCountryId == request.OfficeCountryId);

            if (request.OfficeId.HasValue && request.OfficeId > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.OfficeId == request.OfficeId);

            if (request.OutSourceCompany.HasValue && request.OutSourceCompany.Value > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.OutSourceCompanyId == request.OutSourceCompany);

            if (request.QA.HasValue && request.QA.Value > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.Id == request.QA);


            response.TotalCount = await inspectionOccupancies.AsNoTracking().CountAsync();
            if (response.TotalCount == 0)
            {
                response.Result = InspectionOccupancyResult.NotFound;
                return response;
            }


            //utilization rate is true, then calculate the utilization rate
            if (request.UtilizationRate)
            {
                //get leaves, holidays, schedule qc, other man day data
                InspectionOccupancyData inspectionOccupancyRelatedData = await GetInspectionOccupancyRelatedData(inspectionOccupancies.Select(x => x.Id), fromDate, toDate);
                //if inspection occupenacy categories available in filter
                if (request.InspectionOccupancyCategories != null && request.InspectionOccupancyCategories.Any())
                {
                    if (inspectionOccupancyRelatedData != null)
                    {
                        var inspectionOccupancyList = await inspectionOccupancies.AsNoTracking().ToListAsync();
                        //map the inspection occupancy result
                        var inspectionOccupanySummary = inspectionOccupancyList
                            .Select(x => ReportMap.MapInspectionOccupancies(x, inspectionOccupancyRelatedData, fromDate, toDate, request.UtilizationRate));

                        //filter the inspection occupancy category
                        var inspectionOccupancyCategoryFilterSummary = inspectionOccupanySummary.Where((Func<InspectionOccupancySummary, bool>)(x => request.InspectionOccupancyCategories.Contains((int)x.InspectionOccupancyCategory)));

                        response.TotalCount = inspectionOccupancyCategoryFilterSummary.Count();
                        response.Data = inspectionOccupancyCategoryFilterSummary.Skip(skip).Take(take).ToList();
                    }

                }
                else
                {
                    //only show the data and calculate the utilization rate
                    var result = await inspectionOccupancies.Skip(skip).Take(take).AsNoTracking().ToListAsync();
                    if (inspectionOccupancyRelatedData != null)
                        response.Data = result.Select(x => ReportMap.MapInspectionOccupancies(x, inspectionOccupancyRelatedData, fromDate, toDate, request.UtilizationRate)).ToList();
                }

                //get the inspection occupancy statuses
                response.StatusList = GetInspectionUtilizationRate(inspectionOccupancies, inspectionOccupancyRelatedData, fromDate, toDate);
            }
            else
            {
                //only show the data 
                var result = await inspectionOccupancies.Skip(skip).Take(take).AsNoTracking().ToListAsync();
                //get leaves, holidays and schedule qc data
                var inspectionOccupancyRelatedData = await GetInspectionOccupancyRelatedData(result.Select(x => x.Id), fromDate, toDate);
                if (inspectionOccupancyRelatedData != null)
                    response.Data = result.Select(x => ReportMap.MapInspectionOccupancies(x, inspectionOccupancyRelatedData, fromDate, toDate, request.UtilizationRate)).ToList();
            }
            response.Result = InspectionOccupancyResult.Success;
            response.Index = request.Index;
            response.PageSize = request.pageSize;

            return response;
        }

        /// <summary>
        /// get the leaves, holidays and schedule qc and other manday data, 
        /// </summary>
        /// <param name="staffIds"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private async Task<InspectionOccupancyData> GetInspectionOccupancyRelatedData(IEnumerable<int> staffIds, DateTime fromDate, DateTime toDate)
        {
            if (!staffIds.Any())
                return null;
            var otherMandays = await _otherMandayRepository.GetOtherMandayByEfCore().Where(x => x.QcId.HasValue && staffIds.Contains(x.QcId.Value)).ToListAsync();
            var scheduleQcResult = await _scheduleRepo.GetSchuduleQcs(staffIds, fromDate, toDate);
            var staffleaves = await _humanResourceRepository.GetStaffLeavesByStaffIdAndDateRange(staffIds, fromDate, toDate);
            var holidays = await GetHolidaysByRange(fromDate, toDate);
            return new InspectionOccupancyData()
            {
                ScheduleQcs = scheduleQcResult,
                HrLeaves = staffleaves,
                OtherMandays = otherMandays,
                Holidays = holidays
            };
        }

        private async Task<List<InspectionOccupancyHolidayDto>> GetHolidaysByRange(DateTime fromDate, DateTime toDate)
        {
            var holidays = await _kpiCustomRepository.GetHolidaysByRange(fromDate, toDate);
            if (holidays == null)
                return null;

            var holidayList = new List<InspectionOccupancyHolidayDto>();
            foreach (var _holi in holidays)
            {
                if (_holi.EndDate != null && _holi.StartDate != null && _holi.EndDate.Value != _holi.StartDate.Value)
                {
                    var daterange = Enumerable.Range(0, _holi.EndDate.Value.Subtract(_holi.StartDate.Value).Days + 1).Select(d => _holi.StartDate.Value.AddDays(d));
                    if (daterange != null || daterange.Count() > 0)
                    {

                        var holidayDates = daterange.Select(x => new InspectionOccupancyHolidayDto()
                        {
                            CountryId = _holi.CountryId,
                            LocationId = _holi.LocationId,
                            HolidayDate = x
                        });
                        holidayList.AddRange(holidayDates);
                    }

                }
                else if (_holi.EndDate != null && _holi.StartDate != null && _holi.EndDate.Value == _holi.StartDate.Value)
                {
                    holidayList.Add(new InspectionOccupancyHolidayDto()
                    {
                        CountryId = _holi.CountryId,
                        LocationId = _holi.LocationId,
                        HolidayDate = _holi.StartDate.Value
                    });
                }
            }

            return holidayList;
        }

        /// <summary>
        /// calculate the utilization rate based on the leaves, schedule qcs, holidays, from date, to date
        /// </summary>
        /// <param name="items"></param>
        /// <param name="inspectionOccupancyData"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private IEnumerable<InspectionOccupancyCategorySummary> GetInspectionUtilizationRate(IEnumerable<InspectionOccupancyRepoItem> items, InspectionOccupancyData inspectionOccupancyData, DateTime fromDate, DateTime toDate)
        {
            var result = items.Select(x =>
              {
                  //get the staff working days
                  var numberOfWorkingDays = inspectionOccupancyData.ScheduleQcs.Count(y => y.QcId == x.Id);
                  var maxWorkingDays = 0;
                  var bankHolidays = 0;
                  //check join date is between requested from date and to date
                  if (x.JoinDate >= fromDate && x.JoinDate <= toDate)
                  {
                      //request to date - join date 
                      maxWorkingDays = (toDate.Date - x.JoinDate.Value.Date).Days;
                      bankHolidays = inspectionOccupancyData.Holidays.Where(y => y.HolidayDate >= x.JoinDate && (y.LocationId == null || y.LocationId == x.OfficeId) && y.CountryId == x.OfficeCountryId).Select(a => a.HolidayDate).Distinct().Count();
                  }
                  else if (x.JoinDate <= fromDate)
                  {
                      //request to date - from date
                      maxWorkingDays = (toDate.Date - fromDate.Date).Days;
                      bankHolidays = inspectionOccupancyData.Holidays.Where(y => (y.LocationId == null || y.LocationId == x.OfficeId) && y.CountryId == x.OfficeCountryId).Select(a => a.HolidayDate).Distinct().Count();
                  }
                  //calculates the leaves
                  var leaves = inspectionOccupancyData.HrLeaves.Count(x => x.StaffId == x.Id);
                  //calculate total working days
                  var totalActualWds = maxWorkingDays - leaves - bankHolidays;
                  //calcualate the utilization rate
                  decimal utilizationRate = 0;
                  if (totalActualWds > 0)
                      utilizationRate = (Math.Round((decimal)numberOfWorkingDays / (decimal)totalActualWds, 2) * 100);

                  InspectionOccupancyCategory inspectionOccupancyCategory;
                  //based on utiliaztion rate we are categorizing the data
                  if (utilizationRate <= 30)
                  {
                      inspectionOccupancyCategory = InspectionOccupancyCategory.Low;
                  }
                  else if (utilizationRate > 30 && utilizationRate <= 60)
                  {
                      inspectionOccupancyCategory = InspectionOccupancyCategory.Medium;
                  }
                  else
                  {
                      inspectionOccupancyCategory = InspectionOccupancyCategory.High;
                  }

                  return new
                  {
                      x.Id,
                      UtilizationCategory = inspectionOccupancyCategory
                  };
              });

            //calculate the count based on category
            return result.GroupBy(y => y.UtilizationCategory).Select(y => new InspectionOccupancyCategorySummary()
            {
                InspectionOccupancyCategory = y.Key,
                Color = InspectionOccupancyCategoryColor.GetValueOrDefault((int)y.Key, ""),
                Label = InspectionOccupancyCategoryLabel.GetValueOrDefault((int)y.Key, ""),
                Count = y.Count()
            }).OrderBy(x => x.InspectionOccupancyCategory).ToList();
        }

        /// <summary>
        /// export the inspection occupancy summary method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExportInspectionOccupancySummaryResponse> ExportInspectionOccupanySummary(InspectionOccupancySearchRequest request)
        {
            if (request == null)
                return new ExportInspectionOccupancySummaryResponse() { Result = InspectionOccupancyResult.NotFound };

            //feth the data
            var inspectionOccupancies = _humanResourceRepository.GetInspectionOccupanceQuery();
            var fromDate = request.FromDate.ToDateTime();
            var toDate = request.ToDate.ToDateTime();

            //filter the data
            if (request.EmployeeType.HasValue && request.EmployeeType > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.EmployeeTypeId == request.EmployeeType);

            if (request.OfficeCountryId.HasValue && request.OfficeCountryId > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.OfficeCountryId == request.OfficeCountryId);

            if (request.OfficeId.HasValue && request.OfficeId > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.OfficeId == request.OfficeId);

            if (request.OutSourceCompany.HasValue && request.OutSourceCompany.Value > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.OutSourceCompanyId == request.OutSourceCompany);

            if (request.QA.HasValue && request.QA.Value > 0)
                inspectionOccupancies = inspectionOccupancies.Where(x => x.Id == request.QA);

            //select staff id
            var qcIds = inspectionOccupancies.Select(x => x.Id);
            //fetch the schedule data based on requested from and to date
            var scheduleQcResult = await _scheduleRepo.GetSchuduleQcs(qcIds, fromDate, toDate);
            //fetch the staff leaves data based on requested from and to date
            var staffleaves = await _humanResourceRepository.GetStaffLeavesByStaffIdAndDateRange(qcIds, fromDate, toDate);
            //fetch the holiday data based on requested from and to date
            var holidays = await _kpiCustomRepository.GetHolidaysByRange(fromDate, toDate);
            //fetch the other man day data
            var otherMandays = await _otherMandayRepository.GetOtherMandayByEfCore().Where(x => x.QcId.HasValue && qcIds.Contains(x.QcId.Value)).ToListAsync();

            var result = await inspectionOccupancies.AsNoTracking().ToListAsync();

            return new ExportInspectionOccupancySummaryResponse()
            {
                InspectionOccupancies = result.Select(x => ReportMap.MapExportInspectionOccupancies(x, holidays, staffleaves, scheduleQcResult, otherMandays, fromDate, toDate)).ToList(),
                Result = InspectionOccupancyResult.Success
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CustomerReportDetailsResponse> GetCustomerReportDetails(CustomerReportDetailsRequest request)
        {
            try
            {
                // get the customer id
                var customerId = _ApplicationContext.CustomerId;
                if (customerId <= 0)
                    return new CustomerReportDetailsResponse()
                    {
                        errors = new List<string>() { string.Format(CustomerReportDetailsErrorMessages.IsRequired, "Customer Id") },
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Failed"
                    };

                if (string.IsNullOrWhiteSpace(request.FromDate))
                    return new CustomerReportDetailsResponse()
                    {
                        errors = new List<string>() { string.Format(CustomerReportDetailsErrorMessages.IsRequired, "From Date") },
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Failed"
                    };
                if (string.IsNullOrWhiteSpace(request.ToDate))
                    return new CustomerReportDetailsResponse()
                    {
                        errors = new List<string>() { string.Format(CustomerReportDetailsErrorMessages.IsRequired, "To Date") },
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Failed"
                    };

                if (!DateTime.TryParseExact(request.FromDate, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime serviceFromDate))
                    return new CustomerReportDetailsResponse()
                    {
                        errors = new List<string>() { string.Format(CustomerReportDetailsErrorMessages.InvalidDateFormat, "From") },
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Failed"
                    };

                if (!DateTime.TryParseExact(request.ToDate, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime serviceToDate))
                    return new CustomerReportDetailsResponse()
                    {
                        errors = new List<string>() { string.Format(CustomerReportDetailsErrorMessages.InvalidDateFormat, "To") },
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Failed"
                    };

                if (serviceFromDate > serviceToDate)
                    return new CustomerReportDetailsResponse()
                    {
                        errors = new List<string>() { CustomerReportDetailsErrorMessages.InvalidDateRange },
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Failed"
                    };

                var sharedFbReportDetailsModel = _sharedInspection.GetCustomerReportInspectionQueryRequestMap(customerId, serviceFromDate, serviceToDate, request);

                var fbReportDetailsQuery = _sharedInspection.GetAllFbReportDetails();
                fbReportDetailsQuery = _sharedInspection.GetFbReportDetailswithRequestFilters(sharedFbReportDetailsModel, fbReportDetailsQuery);
                var fbReportDetailsData = fbReportDetailsQuery.Select(x => new CustomerReportDetailsRepo()
                {
                    BookingId = x.Inspection.Id,
                    ReportNo = x.ReportTitle,
                    ReportId = x.Id,
                    ServiceFromDate = x.ServiceFromDate,
                    ServiceToDate = x.ServiceToDate,
                    ReportResult = x.Result.ResultName,
                    FactoryId = x.Inspection.FactoryId.GetValueOrDefault()
                });

                var inspectionIds = fbReportDetailsData.Select(x => x.BookingId);
                var reportIds = fbReportDetailsData.Select(x => x.ReportId);
                //get product list list by reportIds
                var productList = await _repo.GetProductListByReportIds(reportIds, request.ProductRef, request.Po);
                //get po list by report ids
                var poList = await _repo.GetPoListByReportIds(reportIds);
                //get customer decisions by inspection ids
                var customerDecisions = await _inspectionCustomerDecisionRepository.GetCustomerDecisionByEfCoreReportIds(reportIds);
                //get inspection report summaries by report ids
                var fbInspSummaries = await _kpiCustomRepository.GetFBInspSummaryResultbyReportIds(reportIds);
                //get defects by inspection ids
                var fbDefects = await _kpiCustomRepository.GetFBDefectsByReportIds(reportIds);
                //get report quantities by inspection ids
                var fbReportQuantities = await _kpiCustomRepository.GetInspectionQuantitiesByReportIds(inspectionIds);
                //get brand list by booking ids
                var brands = await _inspRepo.GetBrandBookingIdsByBookingQuery(inspectionIds);
                //get customer product categories by product sub category
                var customerProductCategories = await _customerRepository.GetCustomerProductCategoryByProductSubCategoryIds(productList.Select(x => x.ProductSubCategoryId.GetValueOrDefault()).Distinct().ToList());
                //get customer product types by product sub category 2
                var customerProductTypes = await _customerRepository.GetCustomerProductTypeByProductCategoryIds(productList.Select(x => x.ProductSubCategory2Id.GetValueOrDefault()).Distinct().ToList());
                //get factory country by factory ids
                var factoryCountries = await _invoiceRepository.GetBookingFactoryDetails(fbReportDetailsData.Select(x => x.FactoryId).Distinct());

                var inspections = await _inspRepo.GetCustomerReportInspectionDetails(inspectionIds);
                var fbReportData = await fbReportDetailsData.AsNoTracking().ToListAsync();
                var result = ReportMap.MapCustomerReportDetails(inspections, fbReportData, productList, poList, customerDecisions, fbReportQuantities, fbInspSummaries, fbDefects, brands, customerProductCategories, customerProductTypes, factoryCountries);
                if (!result.Any())
                {
                    return new CustomerReportDetailsResponse()
                    {
                        statusCode = HttpStatusCode.NotFound,
                        message = "Success",
                        customerReportDetails = result,
                        errors = new List<string>() { "No record Found" }
                    };
                }
                return new CustomerReportDetailsResponse()
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Success",
                    customerReportDetails = result
                };
            }
            catch (Exception ex)
            {
                return new CustomerReportDetailsResponse()
                {
                    errors = new List<string>() { CustomerReportDetailsErrorMessages.InternalServerError },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Failed"
                };
            }

        }

        /// <summary>
        /// If api call is for filling and review then apply df filter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private IQueryable<InspTransaction> ApplyDfFilterForBooking(InspectionSummarySearchRequest request, IQueryable<InspTransaction> bookingData)
        {
            var inspectionDfTransactionlst = request.InspectionDfTransactions.Where(x => x.ControlConfigurationId > 0).ToList();

            foreach (var inspectionDfTransaction in inspectionDfTransactionlst)
            {
                if (request.CallingFrom == (int)PageTypeSummary.FillingReview && inspectionDfTransaction.Value != null)
                {
                    bookingData = bookingData.Where(x => x.InspDfTransactions.Any(y => y.Active && inspectionDfTransaction.ControlConfigurationId == y.ControlConfigurationId && inspectionDfTransaction.Value == y.Value));
                }
            }
            return bookingData;
        }

    }
}
