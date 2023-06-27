using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.Maps;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.DataAccess;
using DTO.Eaqf;
using DTO.EventBookingLog;
using DTO.FullBridge;
using DTO.HumanResource;
using DTO.Inspection;
using DTO.Quotation;
using DTO.RepoRequest.Enum;
using DTO.Schedule;
using DTO.UserAccount;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class ScheduleManager : ApiCommonData, IScheduleManager
    {
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ICustomerManager _customerManager = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly IScheduleRepository _repo = null;
        private readonly ILogger<ScheduleManager> _logger = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IFBReportManager _fbReportManager = null;
        private readonly IInspectionBookingManager _bookingmanager = null;
        private readonly IEventBookingLogManager _eventBookingLog = null;
        private readonly IHumanResourceRepository _hrRepo = null;
        private readonly IUserRightsManager _userRightsManager = null;
        private readonly IOfficeLocationRepository _officeRepo = null;
        private readonly IOfficeLocationManager _officeManager = null;
        private readonly IQuotationRepository _quotRepo = null;
        private readonly ICustomerCheckPointManager _checkpointManager = null;
        private readonly IInspectionCertificateRepository _certificateRepo = null;
        private readonly ILocationRepository _locationRepo = null;
        readonly IQCBlockManager _qcBlockmanager = null;
        private readonly ISharedInspectionManager _sharedInspection = null;
        private readonly BookingMap BookingMap = null;
        private readonly ScheduleMap ScheduleMap = null;
        private readonly QuotationMap QuotationMap = null;
        private readonly ITenantProvider _filterService = null;
        private readonly ITravelTariffRepository _travelRepo = null;
        private readonly IFullBridgeManager _fullBridgeManager = null;
        private readonly IEaqfEventUpdateManager _eaqfEventUpdate = null;

        public ScheduleManager
            (
            IScheduleRepository repo,
            ICustomerManager customerManager,
            IAPIUserContext applicationContext,
            IInspectionBookingRepository bookingRepository,
            IOfficeLocationManager office,
            ILogger<ScheduleManager> logger,
            IFBReportManager fbReportManager,
            IEventBookingLogManager eventBookingLog,
            IHumanResourceRepository hrRepo,
            IUserRightsManager userRightsManager,
            IOfficeLocationRepository officeRepo,
            IOfficeLocationManager officeManager,
            IQuotationRepository quotRepo,
            ICustomerCheckPointManager checkpointManager,
            IInspectionCertificateRepository certificateRepo,
            ILocationRepository locationRepo,
            IQCBlockManager qcBlockmanager,
            ISharedInspectionManager sharedInspection,
            ITenantProvider filterService,
            ITravelTariffRepository travelRepo,
            IFullBridgeManager fullBridgeManager,
            IEaqfEventUpdateManager eaqfEventUpdate
            )
        {
            _repo = repo;
            _customerManager = customerManager;
            _office = office;
            _ApplicationContext = applicationContext;
            _logger = logger;
            _inspRepo = bookingRepository;
            _fbReportManager = fbReportManager;
            _eventBookingLog = eventBookingLog;
            _hrRepo = hrRepo;
            _userRightsManager = userRightsManager;
            _officeManager = officeManager;
            _officeRepo = officeRepo;
            _quotRepo = quotRepo;
            _checkpointManager = checkpointManager;
            _certificateRepo = certificateRepo;
            _qcBlockmanager = qcBlockmanager;
            _locationRepo = locationRepo;
            _sharedInspection = sharedInspection;
            BookingMap = new BookingMap();
            ScheduleMap = new ScheduleMap();
            QuotationMap = new QuotationMap();
            _filterService = filterService;
            _travelRepo = travelRepo;
            _fullBridgeManager = fullBridgeManager;
            _eaqfEventUpdate = eaqfEventUpdate;
        }

        /// <summary>
        /// Get Schedule summary details based on search filters
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScheduleSearchResponse> GetScheduleDetails(ScheduleSearchRequest request)
        {
            if (request == null)
                return new ScheduleSearchResponse() { Result = ScheduleSearchResponseResult.NotFound };

            var response = new ScheduleSearchResponse { };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

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

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetScheduleInspectionQueryRequestMap(request);

            //get the booking data query
            var data = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);


            if (request.QuotationsStatusIdlst != null && request.QuotationsStatusIdlst.Any())
            {
                data = data.Where(x => x.QuQuotationInsps.Any(y => request.QuotationsStatusIdlst.Contains(y.IdQuotationNavigation.IdStatus)));
            }

            var scheduleData = data.Select(x => new ScheduleBookingInfo
            {
                BookingId = x.Id,
                CustomerId = x.CustomerId,
                SupplierId = x.SupplierId,
                FactoryId = x.FactoryId,
                CustomerName = x.Customer.CustomerName,
                SupplierName = x.Supplier.SupplierName,
                FactoryName = x.Factory.SupplierName,
                FactoryRegionalName = x.Factory.LocalName,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo,
                StatusName = x.Status.Status,
                StatusPriority = x.Status.Priority,
                Office = x.Office.LocationName,
                OfficeId = x.OfficeId.GetValueOrDefault(),
                Customer = x.Customer,
                StatusId = x.StatusId,
                FirstServiceDateFrom = x.FirstServiceDateFrom,
                FirstServiceDateTo = x.FirstServiceDateTo,
                CustomerBookingNo = x.CustomerBookingNo,
                ProductCategory = x.ProductCategory.Name,
                ProductSubCategory = x.ProductSubCategory.Name,
                Year = x.SeasonYear.Year,
                Season = x.Season.Season.Name,
                CreateDate = x.CreatedOn,
                IsEAQF = x.IsEaqf
            }).OrderBy(x => x.ServiceDateFrom).ThenBy(x => x.ServiceDateTo);


            var result = await scheduleData.
                Skip(skip).Take(take).AsNoTracking().ToListAsync();

            response.TotalCount = await scheduleData.AsNoTracking().CountAsync();

            var allBookingIdAsQuery = data.Select(x => x.Id);

            // factory country required for pending quotation 
            var factoryDetails = await _inspRepo.GetFactorycountryByBookingQuery(allBookingIdAsQuery);

            //Get the Quotation Status for the booking using POTransactionsID
            var quotationDetails = await _inspRepo.GetBookingQuotationDetailsbybookingIdQuery(allBookingIdAsQuery);

            //Get the service Type for the bookings
            var serviceTypeList = await _inspRepo.GetServiceTypeByBookingQuery(allBookingIdAsQuery);

            //get the booking factory contacts
            var factoryContacts = await _inspRepo.GetFactoryContactsByBookingIds(allBookingIdAsQuery.ToList());

            var customerIdList = result.Select(x => x.CustomerId).Distinct().ToList();
            var cuCheckpointData = await _checkpointManager.GetCheckPointListByCustomer(customerIdList, (int)Service.InspectionId);

            var bookingBrand = await _inspRepo.GetBrandBookingIdsByBookingQuery(allBookingIdAsQuery);
            var bookingDept = await _inspRepo.GetDeptBookingIdsByBookingQuery(allBookingIdAsQuery);

            var checkpointIdList = cuCheckpointData.Where(x => x.CheckpointTypeId == (int)CheckPointTypeEnum.QuotationRequired).Select(x => x.Id).Distinct().ToList();

            var serviceTypeCheckpointData = await _checkpointManager.GetCheckPointServiceTypeList(checkpointIdList);

            //Get quotation manday for total planned manday between service dates
            var quotationMandayByDates = new List<ScheduleQuotationManDay>();
            if (request.FromDate != null && request.ToDate != null)
            {
                quotationMandayByDates = await _repo.GetQuotationManDaybyBookingQuery(allBookingIdAsQuery, request.FromDate.ToDateTime(), request.ToDate.ToDateTime());
            }

            //Get the allocated staff for every booking
            var QcList = await _repo.GetQCBookingDetailsByBookingQuery(allBookingIdAsQuery);
            var CsList = await _repo.GetCSBookingDetailsByBookingQuery(allBookingIdAsQuery);
            try
            {
                if (response.TotalCount == 0)
                {
                    response.Result = ScheduleSearchResponseResult.NotFound;
                    return response;
                }

                var items = new List<InspectionStatus>();
                items = await scheduleData.Select(x => new { x.StatusId, x.StatusName, x.BookingId, x.StatusPriority })
                       .GroupBy(p => new { p.StatusId, p.StatusName, p.StatusPriority }, p => p, (key, _data) =>
                     new InspectionStatus
                     {
                         Id = key.StatusId,
                         StatusName = key.StatusName,
                         TotalCount = _data.Count(),
                         Priority = key.StatusPriority,
                     }).OrderBy(x => x.Priority).AsNoTracking().ToListAsync();

                var productList = await _inspRepo.GetScheduleProductListByBookingQuery(allBookingIdAsQuery);
                var ContainerList = await _inspRepo.GetScheduleContainerListByBookingQuery(allBookingIdAsQuery);
                var inspectionPOColorTransactions = await _inspRepo.GetPOColorTransactionsByBookingIds(allBookingIdAsQuery);
                var inspectionPurchaseOrders = await _inspRepo.GetBookingPoListByBookingQuery(allBookingIdAsQuery);
                if (result == null || !result.Any())
                    return new ScheduleSearchResponse() { Result = ScheduleSearchResponseResult.NotFound };

                //get product report id list
                var productReportMapIdList = productList.Where(x => x.ReportId > 0).Select(x => x.ReportId).Distinct().ToList();

                //get container report id list
                var containerReportMapIdList = ContainerList.Where(x => x.ReportId > 0).Select(x => x.ReportId).Distinct();

                //merge product and container id list
                var fbReportIdList = productReportMapIdList.Concat(containerReportMapIdList);

                //get report details
                var reportList = await _fullBridgeManager.GetFbReportTitleListByReportIds(fbReportIdList);

                //get booking cs name list
                var csNameList = await _inspRepo.GetInspectionTransCSList(allBookingIdAsQuery);

                var bookingIds = result.Select(x => x.BookingId).ToList();
                //get the booking product category details
                var bookingProductCategoryData = await _inspRepo.GetProductCategoryDetails(bookingIds);

                var _resultdata = result.Select(x =>
                    ScheduleMap.GetInspectionSearchResult(x, items, serviceTypeList, factoryDetails, QcList, CsList,
                                 quotationDetails, cuCheckpointData, quotationMandayByDates, productList, ContainerList,
                                 bookingBrand, bookingDept, serviceTypeCheckpointData, bookingProductCategoryData, inspectionPurchaseOrders, inspectionPOColorTransactions,
                                 reportList, csNameList, factoryContacts));

                var _statuslist = items.Select(x => BookingMap.GetBookingStatusMap(x));

                var totalPlannedManday = quotationMandayByDates.Any() ? quotationMandayByDates.Sum(x => x.ManDay) : quotationDetails?.OrderByDescending(x => x.quotCreatedDate).Select(x => x.Manday.GetValueOrDefault()).FirstOrDefault() ?? 0;

                return new ScheduleSearchResponse()
                {
                    Result = ScheduleSearchResponseResult.Success,
                    TotalCount = response.TotalCount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                    InspectionStatuslst = _statuslist,
                    Data = _resultdata.OrderBy(x => x.ServiceDateFrom).ThenBy(x => x.ProvinceName).ThenBy(x => x.CityName).ThenBy(x => x.FactoryName).ThenBy(x => x.CustomerName).ToList(),
                    MandayCount = totalPlannedManday
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adding status log for schedule 
        /// </summary>
        /// <param name="entity"></param>
        private void AddInspectionStatusLog(InspTransaction entity)
        {
            entity.InspTranStatusLogs.Add(new InspTranStatusLog()
            {
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                StatusId = entity.StatusId,
                ServiceDateFrom = entity.ServiceDateFrom,
                ServiceDateTo = entity.ServiceDateTo,
                StatusChangeDate = DateTime.Now,
                EntityId = _filterService.GetCompanyId()
            });
        }

        public async Task<SaveScheduleResponse> SaveSchedule(SaveScheduleRequest request, string fbToken)
        {
            SaveScheduleResponse response = new SaveScheduleResponse();
            if (request != null)
            {
                // Save Master Data to FB from API.
                try
                {
                    // check booking is not processed by any other user

                    if (!await _inspRepo.CheckBookingIsProcessed(request.BookingId))
                    {
                        if (await _fbReportManager.SaveMasterDataToFB(request, fbToken, false))
                        {
                            try
                            {
                                // Save or Update Booking Schedule allocation details.
                                if (!request.AllocationCSQCStaff.SelectMany(x => x.QC).Any())
                                {
                                    int fbMissionId = 0;
                                    var fbDeleteStatus = await _fbReportManager.DeleteFBMission(request.BookingId, fbMissionId, fbToken);

                                    if (fbDeleteStatus.Result == SaveMissionResponseResult.FBReportAlreadyProcessed || fbDeleteStatus.Result == SaveMissionResponseResult.MissionCompleted)
                                    {
                                        var booking = await _repo.GetInspectionByID(request.BookingId);
                                        booking.IsProcessing = false;
                                        await _inspRepo.EditInspectionBooking(booking);

                                        response.Result = SaveScheduleResponseResult.ReportProcessedAlready;
                                        return response;
                                    }
                                }
                                RemoveCSAdditionalQC(request);
                                // Adding qc expense details per day
                                await AddORUpdateQcExpenseDetails(request);
                                await AddUpdateScheduleDetails(request, (int)QCType.QC);
                                await AddUpdateScheduleDetails(request, (int)QCType.AdditionalQC);
                                AddUpdateCSScheduleDetails(request);
                                response.BookingStatus = await UpdateBookingStatus(request);
                            }
                            catch (Exception ex)
                            {
                                await UpdateBookingProcessOver(request.BookingId);
                                response.Result = SaveScheduleResponseResult.SaveUnsuccessful;
                                return response;
                            }
                        }
                    }
                    else
                    {
                        response.Result = SaveScheduleResponseResult.BookingProcessAlready;
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    await UpdateBookingProcessOver(request.BookingId);
                    response.StatusMessageList = ex.Source.Split("\n");
                    response.Result = SaveScheduleResponseResult.SaveFBDataFailure;
                    return response;
                }

            }

            response.Result = SaveScheduleResponseResult.Success;
            return response;
        }

        /// <summary>
        /// Remove Additional QC and CS if all QC are removed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private void RemoveCSAdditionalQC(SaveScheduleRequest request)
        {
            try
            {
                var listNotHasQC = request.AllocationCSQCStaff.Where(x => x.QC.Count() == 0).ToList();
                var listHasQC = request.AllocationCSQCStaff.Where(x => x.QC.Count() > 0);

                foreach (var item in listNotHasQC)
                {
                    item.CS = Enumerable.Empty<StaffSchedule>();
                    item.AdditionalQC = Enumerable.Empty<StaffSchedule>();
                }
                if (listNotHasQC.Count() > 0)
                {
                    listNotHasQC.AddRange(listHasQC);
                    request.AllocationCSQCStaff = listNotHasQC;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Update Booking status based on the schedule allocation.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<int> UpdateBookingStatus(SaveScheduleRequest request)
        {
            try
            {
                SaveInsepectionRequest objRequest = new SaveInsepectionRequest();

                var booking = await _repo.GetInspectionByID(request.BookingId);
                int updateBooking = 0;
                booking.ScheduleComments = request.Comment;
                // set booking schedule process over
                booking.IsProcessing = false;

                if (!request.AllocationCSQCStaff.Any(x => x.QC.Any()))
                {
                    booking.StatusId = (int)BookingStatus.Confirmed;

                    await UpdateTask(request.BookingId, new[] { (int)TaskType.ScheduleInspection }, true, false);
                }
                else
                {
                    booking.StatusId = (int)BookingStatus.AllocateQC;

                    if (booking.IsEaqf.GetValueOrDefault())
                    {
                        EAQFEventUpdate cancelRequest = new EAQFEventUpdate();
                        cancelRequest.BookingId = request.BookingId;
                        cancelRequest.StatusId = booking.StatusId;
                        await _eaqfEventUpdate.UpdateRescheduleStatusToEAQF(cancelRequest, EAQFBookingEventRequestType.AddStatus);
                    }
                    await UpdateTask(booking.Id, new[] { (int)TaskType.ScheduleInspection }, false, true);

                }

                AddInspectionStatusLog(booking);

                var successResponse = await _inspRepo.EditInspectionBooking(booking);
                if (successResponse == 1)
                    updateBooking = booking.StatusId;
                else
                    updateBooking = 0;
                return updateBooking;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update the task 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="typeIdList"></param>
        /// <param name="oldTaskDoneValue"></param>
        /// <param name="newTaskDoneValue"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MidTask>> UpdateTask(int bookingId, IEnumerable<int> typeIdList, bool oldTaskDoneValue, bool newTaskDoneValue)
        {
            IEnumerable<MidTask> getTasks = await _inspRepo.GetTask(bookingId, typeIdList, oldTaskDoneValue);
            foreach (var task in getTasks)
            {
                if (task != null)
                {
                    task.IsDone = newTaskDoneValue;
                    task.UpdatedBy = _ApplicationContext?.UserId;
                    task.UpdatedOn = DateTime.Now;
                    //_repo.Save(task, true);
                }
            }
            _repo.SaveList(getTasks);
            return getTasks;
        }

        /// <summary>
        /// update booking process over.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        private async Task<int> UpdateBookingProcessOver(int bookingId)
        {
            try
            {
                var booking = await _repo.GetInspectionByID(bookingId);
                booking.IsProcessing = false;
                var successResponse = await _inspRepo.EditInspectionBooking(booking);
                return successResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ScheduleAllocation> GetBookingAllocation(int bookingId)
        {
            ScheduleAllocation response = new ScheduleAllocation();
            List<AllocationStaff> _allocationStaffList = new List<AllocationStaff>();
            List<int> prevBookingNoList = new List<int>();
            try
            {
                //Get booking Details based on booking number
                var bookingDetails = await _repo.GetBookingDetails(bookingId);

                var bookingIdList = new[] { bookingId }.ToList();

                var serviceTypelist = await _inspRepo.GetServiceType(bookingIdList);
                bookingDetails.ServiceTypeList = serviceTypelist.ToList();

                bookingDetails.ProductList = await _inspRepo.GetScheduleProductListByBooking(bookingIdList);

                bookingDetails.ContainerList = await _inspRepo.GetScheduleContainerListByBooking(bookingIdList);

                //get product category details
                var productCategoryList = await _inspRepo.GetProductCategoryDetails(new int[] { bookingId });

                if (bookingDetails == null)
                {
                    response.Result = ScheduleAllocationResponseResult.QCListNotAvailable;
                    return response;
                }

                //get all the previous booking value untill the prev booking value is null
                if (bookingDetails.PreviousBookingNo > 0 && bookingDetails.PreviousBookingNo != bookingDetails.BookingNo)
                {
                    int? prevBookingNo = bookingDetails.PreviousBookingNo.GetValueOrDefault();
                    prevBookingNoList.Add(bookingDetails.PreviousBookingNo.GetValueOrDefault());

                    //loops untill the prev booking no is null
                    do
                    {
                        prevBookingNo = await _inspRepo.GetPreviousBookingNumber(prevBookingNo.GetValueOrDefault());

                        if (prevBookingNo > 0)
                            prevBookingNoList.Add(prevBookingNo.GetValueOrDefault());
                    } while (prevBookingNo > 0 && prevBookingNo != bookingDetails.BookingNo && prevBookingNoList.IndexOf(prevBookingNo.GetValueOrDefault()) == -1);

                }

                var fromDate = bookingDetails.ServiceDateFrom;
                var toDate = bookingDetails.ServiceDateTo;

                var quotationManday = await _repo.GetQuotationManDay(bookingId);

                var quotationTravelManday = await _quotRepo.GetQuotationTravelManDay(bookingId);

                //Get the Date range between service date from and service date to
                var listDate = Enumerable.Range(0, 1 + toDate.Subtract(fromDate).Days)
                             .Select(offset => fromDate.AddDays(offset)).ToArray();

                //Get the allocated QCs and the other bookings allocated to them during the date range
                var qcDetails = await _repo.GetQcListbyServiceDate(fromDate, toDate, bookingId);

                //Get the allocated report checkers and the other bookings allocated to them during the date range
                var csDetails = await _repo.GetCSListbyServiceDate(fromDate, toDate, bookingId);


                var qcCountByLocation = _ApplicationContext.LocationList != null ?
                    await _repo.GetQCCountbyLocation(_ApplicationContext.LocationList) : 0;

                var qcLeavesByLocation = _ApplicationContext.LocationList != null ?
                    await _repo.GetQCStaffLeaves(_ApplicationContext.LocationList) : Enumerable.Empty<StaffLeavesDate>();

                var qcActualManDayOnServiceDates = _ApplicationContext.LocationList != null ?
                    await _repo.GetActualQcCountOnDate(fromDate, toDate, _ApplicationContext.LocationList.ToList()) : Enumerable.Empty<ActualManDayDateCount>();

                var qcAcutualMandayList = await _repo.GetQCActualManDayByServiceDates(fromDate, toDate, bookingId);

                var qcAutoTravelExpenses = await _repo.GetQCAutoTravelExpenseList(bookingId);

                foreach (DateTime item in listDate)
                {

                    var allocatedStaff = await CreateScheduleAllocationDetails(item, quotationManday,
                    qcDetails, csDetails, bookingId, qcCountByLocation, qcLeavesByLocation, qcActualManDayOnServiceDates, qcAcutualMandayList);

                    allocatedStaff.QcAutoExpenseList = new List<QcAutoExpense>();

                    if (qcAutoTravelExpenses.Any())
                    {
                        var qcExpenseListPerDay = qcAutoTravelExpenses.Where(x => x.ServiceDate == item.Date).ToList();

                        if (qcExpenseListPerDay.Any())
                        {
                            allocatedStaff.QcAutoExpenseList = qcExpenseListPerDay;
                        }
                    }

                    _allocationStaffList.Add(allocatedStaff);
                }

                //Get the service Type for the bookings
                var list = new[] { bookingId };
                var serviceTypeList = await _inspRepo.GetServiceType(list);


                //Get factory details
                var factoryDetails = await _inspRepo.GetFactorycountryId(list);
                var factory = factoryDetails.FirstOrDefault();

                var entityFeatureIsExist = await _inspRepo.GetEntityFeatureIsExist(factory.FactoryCountryId);

                var isBookingInvoiced = await _inspRepo.IsBookingInvoiced(bookingId);

                return new ScheduleAllocation
                {
                    ServiceDateFrom = fromDate.ToString(StandardDateFormat),
                    ServiceDateTo = toDate.ToString(StandardDateFormat),
                    CustomerName = bookingDetails.CustomerName,
                    CustomerId = bookingDetails.CustomerId,
                    SupplierName = bookingDetails.SupplierName.Equals(bookingDetails.RegionalSupplierName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(bookingDetails.RegionalSupplierName) ? bookingDetails.SupplierName : bookingDetails.SupplierName + " (" + bookingDetails.RegionalSupplierName + ")",
                    FactoryName = bookingDetails.FactoryName.Equals(bookingDetails.RegionalFactoryName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(bookingDetails.RegionalFactoryName) ? bookingDetails.FactoryName : bookingDetails.FactoryName + " (" + bookingDetails.RegionalFactoryName + ")",
                    BookingComments = bookingDetails.BookingComments,
                    BookingNo = bookingDetails.BookingNo,
                    BookingStatus = bookingDetails.BookingStatus,
                    AllocationCSQCStaff = _allocationStaffList,
                    Comment = bookingDetails.Comment,
                    QuotationManDay = quotationManday.Sum(x => x.ManDay.GetValueOrDefault()),
                    ServiceType = serviceTypeList.FirstOrDefault().serviceTypeName,
                    CountryName = factory.CountryName,
                    CountryId = factory.FactoryCountryId,
                    ProvinceName = factory.ProvinceName,
                    CityName = factory.CityName,
                    CountyName = factory.CountyName,
                    TownId = factory.TownId,
                    TownName = factory.TownName,
                    FactoryAddress = factory.FactoryAdress,
                    RegionalAddress = factory.FactoryRegionalAddress,
                    TotalProducts = bookingDetails.ProductList.Select(x => x.ProductId).Distinct().Count(),
                    TotalReports = bookingDetails.ServiceTypeList.Any(x => ContainerServiceList.Contains(x.serviceTypeId)) ?
                                        bookingDetails.ContainerList.Select(x => x.ContainerId).Count() :
                                        bookingDetails.ProductList.Where(x => x.CombineProductId == null).Count() +
                                        bookingDetails.ProductList.Where(x => x.CombineProductId != null).Select(x => x.CombineProductId).Distinct().Count(),
                    TotalSamplingSize = GetSamplingSize(bookingDetails.ProductList),
                    TotalCombineCount = GetCombineProductCount(bookingDetails.ProductList),
                    TotalContainers = (bookingDetails.ContainerList != null && bookingDetails.ContainerList.Count() > 0) ?
                                        bookingDetails.ContainerList.Select(x => x.ContainerId).Count() : 0,
                    TravelManday = quotationTravelManday?.TravelManDay.GetValueOrDefault() ?? 0,
                    Result = _allocationStaffList != null ? ScheduleAllocationResponseResult.Success : ScheduleAllocationResponseResult.QCListNotAvailable,
                    PreviousBookingNoList = prevBookingNoList.Distinct().ToList(),
                    ActualManday = _allocationStaffList.Sum(x => x.ActualManDay),
                    ProductCategory = string.Join(", ", productCategoryList.Select(x => x.ProductCategoryName).Distinct().ToList()),
                    ProductSubCategory = string.Join(", ", productCategoryList.Select(x => x.ProductCategorySubName).Distinct().ToList()),
                    ProductSubCategory2 = string.Join(", ", productCategoryList.Select(x => x.ProductCategorySub2Name).Distinct().ToList()),
                    IsEntityLevelAutoQcExpenseEnabled = entityFeatureIsExist,
                    IsServiceTypeLevelAutoQcExpenseEnabled = serviceTypeList.FirstOrDefault().IsAutoQCExpenseClaim,
                    IsBookingInvoiced = isBookingInvoiced,
                    SuggestedManday = quotationTravelManday?.SuggestedManday
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get remaining man day for QC on particular date
        /// </summary>
        /// <param name="_datetime"></param>
        /// <returns></returns>
        private double AvailableManDay(DateTime _datetime, int qcCountByLocation, IEnumerable<StaffLeavesDate> staffLeaves, IEnumerable<ActualManDayDateCount> actualManDayCount)
        {
            try
            {
                double availableManDay = 0;
                if (_ApplicationContext.LocationList != null)
                {

                    int QCLeaveCount = staffLeaves.
                                                 Count(z => _datetime >= z.LeaveStartDate.Date &&
                                                 _datetime <= z.LeaveEndDate.Date);

                    double assignedQCCount = actualManDayCount.Where(x => x.ServiceDate.Date == _datetime).
                                             Select(x => x.ActualManDayCount).FirstOrDefault();

                    availableManDay = qcCountByLocation - (QCLeaveCount + (double)Math.Round(assignedQCCount, 0));
                }
                return availableManDay;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// get QC/CS list which is not leave on same date and assigned QC count on same day for QC list
        /// </summary>
        /// <param name="staffList"></param>
        /// <param name="profileId"></param>
        /// <param name="serviceDate"></param>
        /// <returns></returns>
        private List<StaffSchedule> getStaffList(IEnumerable<QCStaffInfo> staffList, int? profileId, DateTime serviceDate)
        {
            List<StaffSchedule> _staffList = new List<StaffSchedule>();

            foreach (var user in staffList)
            {
                if (profileId != null)
                {
                    //added to avoid mapping the user with multiple leave records having 1 day leave in the service
                    //date range (service date: 3-5, leave on 3 and 5)
                    var leave = user.LeaveQC.Where(x => x.DateBegin.Date == serviceDate || x.DateEnd.Date == serviceDate
                    || (serviceDate > x.DateBegin.Date && serviceDate < x.DateEnd.Date));

                    if (leave.Count(z => z.Status != (int)Entities.Enums.LeaveStatus.Cancelled && z.Status != (int)Entities.Enums.LeaveStatus.Rejected) <= 0
                        || (leave.Count(z => !(serviceDate >= z.DateBegin.Date && serviceDate <= z.DateEnd.Date)) > 0))
                    {
                        _staffList.Add(ScheduleMap.MapStaff(user));
                    }
                }
                //for CS and AE as there is no Leave concept
                else
                {
                    _staffList.Add(ScheduleMap.MapStaff(user));
                }
            }
            return _staffList;
        }
        /// <summary>
        /// add or update qc details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="QcType"></param>
        /// <returns></returns>
        private async Task AddUpdateScheduleDetails(SaveScheduleRequest request, int QcType)
        {
            var entity = _repo.GetQCDetails(request.BookingId, QcType).Result;
            foreach (var itemAllocationCSQCStaff in request.AllocationCSQCStaff)
            {
                var QC = QcType == (int)QCType.QC ? itemAllocationCSQCStaff.QC
              : itemAllocationCSQCStaff.AdditionalQC;

                var QCList = QC.Select(x => x.StaffID);

                var QCIdtoAddList = new List<SchScheduleQc>();

                var QCIdInList = entity.Where(x => QCList.Contains(x.Qcid) && x.ServiceDate.Date == itemAllocationCSQCStaff.ServiceDate.Date).ToList();
                var QCIdNotInList = entity.Where(x => !QCList.Contains(x.Qcid) && x.ServiceDate.Date == itemAllocationCSQCStaff.ServiceDate.Date);

                // Remove if data does not exist in the db.

                foreach (var item in QCIdNotInList)
                {
                    item.DeletedOn = DateTime.Now;
                    item.DeletedBy = _ApplicationContext.UserId;
                    item.Active = false;
                    item.ActualManDay = 0;
                    _repo.Save(item, true);
                }
                // Update if data already exist in the db

                if (QCList != null && QCList.Any())
                {
                    foreach (var id in QCList)
                    {
                        var leader = itemAllocationCSQCStaff.QC.FirstOrDefault(x => x.StaffID == id);

                        if (!QCIdInList.Any() || QCIdInList.Count(x => x.Qcid == id &&
                            x.ServiceDate.Date == itemAllocationCSQCStaff.ServiceDate.Date) == 0)
                        {
                            var QCIdtoAdd = new SchScheduleQc();

                            QCIdtoAdd.Qcid = id;
                            QCIdtoAdd.Qctype = QcType;
                            QCIdtoAdd.Active = true;
                            QCIdtoAdd.CreatedBy = _ApplicationContext.UserId;
                            QCIdtoAdd.CreatedOn = DateTime.Now;
                            QCIdtoAdd.BookingId = request.BookingId;
                            QCIdtoAdd.ServiceDate = itemAllocationCSQCStaff.ServiceDate;
                            QCIdtoAdd.ActualManDay = (double)itemAllocationCSQCStaff.ActualManDay;
                            QCIdtoAdd.QcLeader = QcType == (int)QCType.QC ? leader.isLeader : false;
                            QCIdtoAdd.IsVisibleToQc = itemAllocationCSQCStaff.IsQcVisibility;
                            QCIdtoAddList.Add(QCIdtoAdd);
                        }

                        //update QC Leader
                        else if (QcType == (int)QCType.QC)
                        {
                            try
                            {
                                var data = QCIdInList.FirstOrDefault(x => x.Qcid == id);

                                if (data.QcLeader != leader.isLeader)
                                {
                                    data.ModifiedBy = _ApplicationContext.UserId;
                                    data.ModifiedOn = DateTime.Now;
                                    data.QcLeader = leader.isLeader;
                                    data.ServiceDate = itemAllocationCSQCStaff.ServiceDate;

                                    _repo.Save(data, true);
                                }

                                //Update IsQcVisibility
                                data.ModifiedBy = _ApplicationContext.UserId;
                                data.ModifiedOn = DateTime.Now;
                                data.IsVisibleToQc = itemAllocationCSQCStaff.IsQcVisibility;
                                _repo.Save(data, true);
                            }
                            catch (Exception e)
                            {
                                var ex = e;
                            }
                        }
                    }

                    if (QCIdtoAddList != null && QCIdtoAddList.Any())
                    {
                        _repo.SaveList(QCIdtoAddList, false);
                    }

                    QCIdtoAddList.AddRange(QCIdNotInList.ToList());

                    if (QcType == (int)QCType.QC)
                        //qc actualmanday logic
                        await ActualMandayCalculation(QCIdtoAddList);
                }
            }
        }

        /// <summary>
        /// Add or update or remove qc expense details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        private async Task AddORUpdateQcExpenseDetails(SaveScheduleRequest request)
        {

            if (request.AllocationCSQCStaff != null && request.AllocationCSQCStaff.Any())
            {
                var expenseQcList = request.AllocationCSQCStaff.SelectMany(x => x.QcAutoExpenses.Select(x => x.QcId)).ToList();

                // take the range of dates 
                List<DateTime> serviceDates = request.AllocationCSQCStaff.Select(x => x.ServiceDate.Date).ToList();
                DateTime startDate = serviceDates.Min();
                DateTime endDate = serviceDates.Max();

                var inspectionLocation = await _repo.GetInspectionLocation(request.BookingId);

                // get Qc Expense List

                var qcExpenseList = await _repo.GetAutoQcExpenseListByQcList(expenseQcList, request.BookingId);

                // travel expense details 
                var qcAutoTravelExpenses = await _repo.GetQCAutoTravelExpensesByInspectionId(request.BookingId);
                var travelAllowanceList = await _repo.GetTravelTariffData(startDate, endDate);

                // food expense details
                var qcAutoFoodExpenses = await _repo.GetQCAutoFoodExpensesByInspectionId(request.BookingId);

                var existingQcFoodExpenseNotRemovedFromRequest = qcAutoFoodExpenses.Where(x => !expenseQcList.Contains(x.QcId));

                var qcRemovedList = existingQcFoodExpenseNotRemovedFromRequest.Select(x => x.QcId).Distinct().ToList();

                expenseQcList = expenseQcList.Concat(qcRemovedList).Distinct().ToList();

                var qcAutoFoodExpensebyQcList = await _repo.GetQCAutoFoodExpenses(expenseQcList, startDate, endDate);
                var foodAllowanceList = await _repo.GetFoodAllowanceData(startDate, endDate);

                foreach (var qcItemPerDay in request.AllocationCSQCStaff)
                {
                    await AddOrUpdateTravelExpense(qcItemPerDay, qcAutoTravelExpenses, travelAllowanceList, request, qcExpenseList);
                    await AddOrUpdateFoodExpense(qcItemPerDay, qcAutoFoodExpenses, foodAllowanceList,
                        request, inspectionLocation, qcAutoFoodExpensebyQcList, qcExpenseList);
                }
            }
        }

        private async Task AddOrUpdateTravelExpense(SaveAllocationStaff qcItemPerDay,
            IEnumerable<EcAutQcTravelExpense> qcAutoTravelExpenses, List<EcAutTravelTariff> travelAllowanceList, SaveScheduleRequest request,
            List<EcExpencesClaim> qcExpenseList)
        {

            var QCList = qcItemPerDay.QcAutoExpenses.Select(x => x.QcId).ToList();

            var qcTravelExpenseNewList = new List<EcAutQcTravelExpense>();
            var existingQcTravelExpenseNotRemoved = qcAutoTravelExpenses.Where(x => QCList.Contains(x.QcId) && x.ServiceDate == qcItemPerDay.ServiceDate.Date).ToList();
            var existingQcTravelExpenseNotIntheRequest = qcAutoTravelExpenses.Where(x => !QCList.Contains(x.QcId) && x.ServiceDate == qcItemPerDay.ServiceDate.Date);

            var qcRemovedList = existingQcTravelExpenseNotIntheRequest.Select(x => x.QcId).Distinct().ToList();
            var expenseIdList = new List<int>();
            var expenseClaimDetails = qcExpenseList.Where(x => qcRemovedList.Contains(x.StaffId)).SelectMany(x => x.EcExpensesClaimDetais).ToList();

            // remove or inactive 
            foreach (var item in existingQcTravelExpenseNotIntheRequest)
            {

                // set expense claim details inactive
                var activeTravelExpenseClaimDetails = expenseClaimDetails.Where(x => item.Id == x.QcTravelExpenseId && x.Active.Value).ToList();
                if (activeTravelExpenseClaimDetails.Any())
                {
                    foreach (var expenseClaim in activeTravelExpenseClaimDetails)
                    {
                        // remove expense claim if the expense is in pending status
                        if (activeTravelExpenseClaimDetails.FirstOrDefault()?.Expense?.StatusId == (int)ExpenseClaimStatus.Pending)
                        {
                            expenseClaim.Active = false;
                            expenseClaim.Description = "Qc Removed";
                            expenseIdList.Add(expenseClaim.ExpenseId);
                        }
                    }
                }

                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _ApplicationContext.UserId;
                item.Active = false;
                _repo.Save(item, true);
            }


            // Update if data already exist in the db

            if (qcItemPerDay.QcAutoExpenses != null && qcItemPerDay.QcAutoExpenses.Any())
            {
                foreach (var expense in qcItemPerDay.QcAutoExpenses)
                {
                    // Add new qc travel expense
                    if (!existingQcTravelExpenseNotRemoved.Any()
                        || (!existingQcTravelExpenseNotRemoved.Any(x => x.QcId == expense.QcId && x.ServiceDate == qcItemPerDay.ServiceDate.Date)))
                    {

                        var travelAllowance = await CalculateTravelAllowance(travelAllowanceList, qcItemPerDay, expense);

                        qcTravelExpenseNewList.Add(new EcAutQcTravelExpense()
                        {
                            StartPort = expense.StartPortId,
                            ServiceDate = qcItemPerDay.ServiceDate.Date,
                            FactoryTown = expense.FactoryTownId,
                            EntityId = _filterService.GetCompanyId(),
                            TravelTariff = travelAllowance.TravelAllowance,
                            TravelTariffCurrency = travelAllowance.TravelAllowanceCurrency,
                            TripType = expense.TripTypeId,
                            Active = true,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            InspectionId = request.BookingId,
                            QcId = expense.QcId,
                            Comments = expense.Comments,
                            IsExpenseCreated = false,
                            IsTravelAllowanceConfigured = travelAllowance.IsTravelAllowanceConfigured
                        });
                    }
                    // update Qc expense
                    else if (expense.QcId > 0)
                    {
                        try
                        {
                            var existingTravelExpenseRow = existingQcTravelExpenseNotRemoved.
                                       FirstOrDefault(x => x.QcId == expense.QcId && x.ServiceDate == qcItemPerDay.ServiceDate);

                            if (existingTravelExpenseRow != null)
                            {
                                if (existingTravelExpenseRow.StartPort != expense.StartPortId ||
                                   existingTravelExpenseRow.FactoryTown != expense.FactoryTownId || existingTravelExpenseRow.TripType != expense.TripTypeId)
                                {
                                    var travelAllowance = await CalculateTravelAllowance(travelAllowanceList, qcItemPerDay, expense);

                                    existingTravelExpenseRow.StartPort = expense.StartPortId;
                                    existingTravelExpenseRow.FactoryTown = expense.FactoryTownId;
                                    existingTravelExpenseRow.TripType = expense.TripTypeId;

                                    existingTravelExpenseRow.TravelTariff = travelAllowance.TravelAllowance;
                                    existingTravelExpenseRow.TravelTariffCurrency = travelAllowance.TravelAllowanceCurrency;
                                    existingTravelExpenseRow.IsTravelAllowanceConfigured = travelAllowance.IsTravelAllowanceConfigured;

                                    // update existing travel expense details

                                    var travelExpenseClaimDetails = qcExpenseList.Where(x => x.StaffId == expense.QcId).SelectMany(x => x.EcExpensesClaimDetais).ToList();

                                    foreach (var claimDetails in travelExpenseClaimDetails)
                                    {
                                        if (claimDetails.Active.GetValueOrDefault() && claimDetails.QcTravelExpenseId == existingTravelExpenseRow.Id)
                                        {
                                            var actualAmount = existingTravelExpenseRow.TravelTariff.GetValueOrDefault();
                                            var amount = Math.Round(claimDetails.ExchangeRate.GetValueOrDefault() * actualAmount * 100) / 100;
                                            claimDetails.AmmountHk = actualAmount;
                                            claimDetails.Amount = amount;
                                            claimDetails.TripType = expense.TripTypeId;
                                            claimDetails.Description = "Amount is updated based on the Schedule Allocation change";
                                            _repo.Save(claimDetails, true);
                                        }
                                    }

                                    existingTravelExpenseRow.Comments = expense.Comments;
                                    existingTravelExpenseRow.UpdatedBy = _ApplicationContext.UserId;
                                    existingTravelExpenseRow.UpdatedOn = DateTime.Now;
                                    _repo.Save(existingTravelExpenseRow, true);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }

                // Add new items 
                if (qcTravelExpenseNewList.Any())
                {
                    _repo.SaveList(qcTravelExpenseNewList, false);
                }

            }
            if (expenseIdList.Any())
            {
                setCancelExpenseWhileQcReomvedFromSchedule(qcExpenseList, expenseIdList);
            }
        }

        /// <summary>
        /// Set cancel status for previous expense if the status in pending
        /// </summary>
        /// <param name="qcExpenseList"></param>
        /// <param name="expenseIdList"></param>
        private void setCancelExpenseWhileQcReomvedFromSchedule(List<EcExpencesClaim> qcExpenseList, List<int> expenseIdList)
        {
            foreach (var expenseId in expenseIdList)
            {
                var expenseDataByQc = qcExpenseList.FirstOrDefault(x => x.Id == expenseId);

                // check any cliam details not active
                if (expenseDataByQc != null && !expenseDataByQc.EcExpensesClaimDetais.Any(x => x.Active.Value) && expenseDataByQc.StatusId == (int)ExpenseClaimStatus.Pending) // if status pending
                {
                    expenseDataByQc.StatusId = (int)ExpenseClaimStatus.Cancelled; // update to cancelled. 
                }
                _repo.Save(expenseDataByQc, true);
            }
        }

        private async Task AddOrUpdateFoodExpense(SaveAllocationStaff qcItemPerDay,
           IEnumerable<EcAutQcFoodExpense> qcAutoFoodExpenses, List<EcFoodAllowance> foodAllowanceList,
           SaveScheduleRequest request, int? inspectionLocation, List<EcAutQcFoodExpense> qcAutoFoodExpensebyQcList, List<EcExpencesClaim> qcExpenseList)
        {

            var QCList = qcItemPerDay.QcAutoExpenses.Select(x => x.QcId).ToList();

            var qcFoodExpenseNewList = new List<EcAutQcFoodExpense>();
            var existingQcFoodExpenseNotRemoved = qcAutoFoodExpenses.Where(x => QCList.Contains(x.QcId) && x.ServiceDate == qcItemPerDay.ServiceDate.Date).ToList();
            var existingQcFoodExpenseNotRemovedFromRequest = qcAutoFoodExpenses.Where(x => !QCList.Contains(x.QcId) && x.ServiceDate == qcItemPerDay.ServiceDate.Date);

            var qcRemovedList = existingQcFoodExpenseNotRemovedFromRequest.Select(x => x.QcId).Distinct().ToList();
            var expenseIdList = new List<int>();
            var expenseClaimDetails = qcExpenseList.Where(x => qcRemovedList.Contains(x.StaffId)).SelectMany(x => x.EcExpensesClaimDetais).ToList();

            // remove or inactive 
            foreach (var item in existingQcFoodExpenseNotRemovedFromRequest)
            {
                // set expense claim details inactive
                var activeFoodExpenseClaimDetails = expenseClaimDetails.Where(x => item.Id == x.QcFoodExpenseId && x.Active.Value).ToList();
                foreach (var expenseClaim in activeFoodExpenseClaimDetails)
                {
                    // remove expense claim details if the expense is in pending status
                    if (activeFoodExpenseClaimDetails.FirstOrDefault()?.Expense?.StatusId == (int)ExpenseClaimStatus.Pending)
                    {
                        expenseClaim.Active = false;
                        expenseClaim.Description = "Qc Removed";
                        expenseIdList.Add(expenseClaim.ExpenseId);
                    }
                }
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _ApplicationContext.UserId;
                item.Active = false;
                _repo.Save(item, true);

                // update qc other booking on same day
                var qcOtherExpenseBookingsOnSameDay = qcAutoFoodExpensebyQcList.FirstOrDefault(x => x.InspectionId != request.BookingId
                     && x.QcId == item.QcId && x.ServiceDate == qcItemPerDay.ServiceDate && x.FoodAllowance > 0);

                if (qcOtherExpenseBookingsOnSameDay != null)
                {
                    var foodAllowanceData = foodAllowanceList.
                           FirstOrDefault(x =>
                             !((x.StartDate > qcItemPerDay.ServiceDate) || (x.EndDate < qcItemPerDay.ServiceDate)) &&
                            x.CountryId == qcOtherExpenseBookingsOnSameDay.FactoryCountry);

                    if (foodAllowanceData != null)
                    {
                        qcOtherExpenseBookingsOnSameDay.FoodAllowance = (double)foodAllowanceData.FoodAllowance;
                        qcOtherExpenseBookingsOnSameDay.FoodAllowanceCurrency = foodAllowanceData.CurrencyId;
                        qcOtherExpenseBookingsOnSameDay.IsFoodAllowanceConfigured = true;
                    }
                    _repo.Save(qcOtherExpenseBookingsOnSameDay, true);
                }
            }

            // Update if data already exist in the db

            if (qcItemPerDay.QcAutoExpenses != null && qcItemPerDay.QcAutoExpenses.Any())
            {
                foreach (var expense in qcItemPerDay.QcAutoExpenses)
                {

                    // Add new qc expense
                    if (!existingQcFoodExpenseNotRemoved.Any()
                        || (!existingQcFoodExpenseNotRemoved.Any(x => x.QcId == expense.QcId && x.ServiceDate == qcItemPerDay.ServiceDate.Date)))
                    {

                        var foodAllowance = CalculateFoodAllowance(foodAllowanceList, qcAutoFoodExpensebyQcList,
                            inspectionLocation, qcItemPerDay, expense);

                        qcFoodExpenseNewList.Add(new EcAutQcFoodExpense()
                        {
                            ServiceDate = qcItemPerDay.ServiceDate.Date,
                            EntityId = _filterService.GetCompanyId(),
                            FoodAllowance = (double)foodAllowance.FoodAllowance,
                            FoodAllowanceCurrency = foodAllowance.FoodAllowanceCurrency,
                            Active = true,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now,
                            InspectionId = request.BookingId,
                            QcId = expense.QcId,
                            Comments = expense.Comments,
                            IsExpenseCreated = false,
                            IsFoodAllowanceConfigured = foodAllowance.IsFoodAllowanceConfigured,
                            FactoryCountry = expense.CountryId
                        });
                    }
                    // update Qc expense
                    else if (expense.QcId > 0)
                    {
                        try
                        {
                            var existingExpenseRow = existingQcFoodExpenseNotRemoved.
                                       FirstOrDefault(x => x.QcId == expense.QcId && x.ServiceDate == qcItemPerDay.ServiceDate);

                            if (existingExpenseRow != null)
                            {

                                var foodAllowance = foodAllowanceList.
                                                      FirstOrDefault(x =>
                                                        !((x.StartDate > qcItemPerDay.ServiceDate) || (x.EndDate < qcItemPerDay.ServiceDate)) &&
                                                       x.CountryId == expense.CountryId);

                                if (foodAllowance != null)
                                {
                                    existingExpenseRow.FoodAllowance = (double)foodAllowance.FoodAllowance;
                                    existingExpenseRow.FoodAllowanceCurrency = foodAllowance.CurrencyId;
                                    existingExpenseRow.IsFoodAllowanceConfigured = true;

                                    existingExpenseRow.Comments = expense.Comments;
                                    existingExpenseRow.UpdatedBy = _ApplicationContext.UserId;
                                    existingExpenseRow.UpdatedOn = DateTime.Now;

                                    // update existing food expense details

                                    var travelExpenseClaimDetails = qcExpenseList.Where(x => x.StaffId == expense.QcId).SelectMany(x => x.EcExpensesClaimDetais).ToList();

                                    foreach (var claimDetails in travelExpenseClaimDetails)
                                    {
                                        if (claimDetails.QcFoodExpenseId == existingExpenseRow.Id)
                                        {
                                            // calculate amount by exchange rate
                                            var actualAmount = existingExpenseRow.FoodAllowance.GetValueOrDefault();
                                            var amount = Math.Round(claimDetails.ExchangeRate.GetValueOrDefault() * actualAmount * 100) / 100;
                                            claimDetails.AmmountHk = actualAmount;
                                            claimDetails.Amount = amount;
                                            claimDetails.Description = "Amount is updated based on the Schedule Allocation change";
                                            _repo.Save(claimDetails, true);
                                        }
                                    }
                                    _repo.Save(existingExpenseRow, true);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
                // Add new items 
                if (qcFoodExpenseNewList.Any())
                {
                    _repo.SaveList(qcFoodExpenseNewList, false);
                }

            }
            if (expenseIdList.Any())
            {
                setCancelExpenseWhileQcReomvedFromSchedule(qcExpenseList, expenseIdList);
            }
        }

        /// <summary>
        /// Get Duplicate travel expense data.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<DuplicateTravelAllowance>> GetDuplicateTravelExpenseData(SaveScheduleRequest request)
        {
            List<DuplicateTravelAllowance> duplicateTravelAllowances = new List<DuplicateTravelAllowance>();

            if (request.AllocationCSQCStaff != null && request.AllocationCSQCStaff.Any())
            {
                var expenseQcList = request.AllocationCSQCStaff.SelectMany(x => x.QcAutoExpenses.Select(x => x.QcId)).ToList();

                // take the range of dates 
                List<DateTime> serviceDates = request.AllocationCSQCStaff.Select(x => x.ServiceDate.Date).ToList();
                DateTime startDate = serviceDates.Min();
                DateTime endDate = serviceDates.Max();

                var qcAutoExpensebyQcList = await _repo.GetQCAutoTravelExpenses(expenseQcList, startDate, endDate);

                if (qcAutoExpensebyQcList.Any())
                {
                    // day by day schedule
                    foreach (var qcItemPerDay in request.AllocationCSQCStaff)
                    {

                        if (qcItemPerDay.QcAutoExpenses != null && qcItemPerDay.QcAutoExpenses.Any())
                        {
                            // per day - qc list check
                            foreach (var expense in qcItemPerDay.QcAutoExpenses)
                            {
                                if (expense.TripTypeId != (int)TripType.NoTrip)
                                {
                                    // check qc has already travel allowance
                                    var travelTariff = qcAutoExpensebyQcList.FirstOrDefault(x => x.QcId == expense.QcId && x.InspectionId != request.BookingId &&
                                         x.ServiceDate == qcItemPerDay.ServiceDate.Date &&
                                         x.StartPort == expense.StartPortId && x.FactoryTown == expense.FactoryTownId && x.TravelTariff > 0);

                                    if (travelTariff != null)
                                    {
                                        duplicateTravelAllowances.Add(new DuplicateTravelAllowance()
                                        {
                                            BookingId = travelTariff.InspectionId.GetValueOrDefault(),
                                            FactoryTown = expense.FactoryTownName,
                                            StartPort = expense.StartPortName,
                                            QcName = expense.QcName,
                                            ServiceDate = qcItemPerDay.ServiceDate.ToString(StandardDateFormat)
                                        });
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return duplicateTravelAllowances;
        }


        /// <summary>
        /// Calculate travel allowance data
        /// </summary>
        /// <param name="travelAllowanceList"></param>
        /// <param name="qcItemPerDay"></param>
        /// <param name="expense"></param>
        /// <returns></returns>
        private async Task<TravelAllowanceData> CalculateTravelAllowance(List<EcAutTravelTariff> travelAllowanceList,
           SaveAllocationStaff qcItemPerDay, QcAutoExpense expense)
        {

            var travelAllowance = new TravelAllowanceData()
            {
                TravelAllowance = 0,
                TravelAllowanceCurrency = null,
                IsTravelAllowanceConfigured = false
            };

            var travelAllowanceData = travelAllowanceList.FirstOrDefault(x =>
                                                                   !((x.StartDate > qcItemPerDay.ServiceDate)
                                                                   || (x.EndDate < qcItemPerDay.ServiceDate)) &&
                                                                  x.StartPort == expense.StartPortId && x.TownId == expense.FactoryTownId);

            // travel tariff is not configured - create it.
            if (travelAllowanceData == null)
            {
                travelAllowance.TravelAllowance = 0;
                travelAllowance.TravelAllowanceCurrency = null;
                travelAllowance.IsTravelAllowanceConfigured = false;

                //// check travel tariff is not exist then create it
                //if (!await _travelRepo.CheckTravelStartPortEndPortExist(expense.StartPortId.GetValueOrDefault(), expense.FactoryTownId.GetValueOrDefault()))
                //{
                //    var travelTariffData = new EcAutTravelTariff()
                //    {
                //        StartPort = expense.StartPortId.GetValueOrDefault(),
                //        TownId = expense.FactoryTownId.GetValueOrDefault(),
                //        TravelTariff = 0,
                //        Active = true,
                //        CreatedBy = _filterService.GetCompanyId(),
                //        CreatedOn = DateTime.Now,
                //        EntityId = _filterService.GetCompanyId(),
                //        Status = false
                //    };

                //    var tariffId = await _travelRepo.AddTravelTariff(travelTariffData);

                //    // tariff save success then create the task who has auto qc expense account role

                //    if (tariffId > 0)
                //    {
                //        var userAccessFilter = new UserAccess
                //        {
                //            RoleId = (int)RoleEnum.AutoQCExpenseAccounting
                //        };

                //        var userListByRoleAccess = await _userRightsManager.GetUserListByRoleOffice(userAccessFilter);

                //        await _userRightsManager.AddTask(TaskType.TravelTariffUpdate, tariffId, userListByRoleAccess.Select(x => x.Id));
                //    }
                //}
            }
            else
            {
                travelAllowance.TravelAllowance = 0;
                travelAllowance.TravelAllowanceCurrency = travelAllowanceData.TravelCurrency;
                travelAllowance.IsTravelAllowanceConfigured = true;

                if (expense.TripTypeId == (int)TripType.SigleTrip)
                {
                    travelAllowance.TravelAllowance = travelAllowanceData.TravelTariff;
                }
                else if (expense.TripTypeId == (int)TripType.DoubleTrip)
                {
                    travelAllowance.TravelAllowance = (travelAllowanceData.TravelTariff * 2);
                }
            }

            return travelAllowance;
        }

        /// <summary>
        /// Calculate Food Allowance Data.
        /// </summary>
        /// <param name="foodAllowanceList"></param>
        /// <param name="qcAutoExpensebyQcList"></param>
        /// <param name="inspectionLocation"></param>
        /// <param name="qcItemPerDay"></param>
        /// <param name="expense"></param>
        /// <returns></returns>
        private FoodAllowanceData CalculateFoodAllowance(List<EcFoodAllowance> foodAllowanceList,
            List<EcAutQcFoodExpense> qcAutoFoodExpensebyQcList, int? inspectionLocation, SaveAllocationStaff qcItemPerDay, QcAutoExpense expense)
        {

            var foodAllowanceData = foodAllowanceList.
                                       FirstOrDefault(x =>
                                         !((x.StartDate > qcItemPerDay.ServiceDate) || (x.EndDate < qcItemPerDay.ServiceDate)) &&
                                        x.CountryId == expense.CountryId);

            if (foodAllowanceData == null)
            {
                return new FoodAllowanceData()
                {
                    FoodAllowance = 0,
                    FoodAllowanceCurrency = null,
                    IsFoodAllowanceConfigured = false
                };
            }
            // check already qc has food allowance on same day if not then update it             
            else if (!qcAutoFoodExpensebyQcList.Any(x => x.QcId == expense.QcId && x.ServiceDate == qcItemPerDay.ServiceDate.Date && x.FoodAllowance > 0))
            {
                if ((inspectionLocation == null || inspectionLocation == (int)InspectionLocation.Factory) ||
                    (inspectionLocation == (int)InspectionLocation.Platform && qcAutoFoodExpensebyQcList.
                    Count(x => x.QcId == expense.QcId && x.ServiceDate == qcItemPerDay.ServiceDate.Date) >= 3))
                {
                    return new FoodAllowanceData()
                    {
                        FoodAllowance = foodAllowanceData.FoodAllowance,
                        FoodAllowanceCurrency = foodAllowanceData.CurrencyId,
                        IsFoodAllowanceConfigured = true
                    };
                }
                else
                {
                    return new FoodAllowanceData()
                    {
                        FoodAllowance = 0,
                        FoodAllowanceCurrency = foodAllowanceData.CurrencyId,
                        IsFoodAllowanceConfigured = true
                    };
                }
            }
            else
            {
                return new FoodAllowanceData()
                {
                    FoodAllowance = 0,
                    FoodAllowanceCurrency = foodAllowanceData.CurrencyId,
                    IsFoodAllowanceConfigured = true
                };
            }
        }



        /// <summary>
        /// add or update cs schedule details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private void AddUpdateCSScheduleDetails(SaveScheduleRequest request)
        {

            var entity = _repo.GetCSDetails(request.BookingId).Result;
            foreach (var itemAllocationCSQCStaff in request.AllocationCSQCStaff)
            {
                var CSIdtoRemove = new List<SchScheduleC>();
                var CSIdtoAdd = new List<SchScheduleC>();
                var CSStaffList = itemAllocationCSQCStaff.CS;
                var CSList = CSStaffList.Select(x => x.StaffID);

                var CSIdInList = entity.Where(x => CSList.Contains(x.Csid) && x.ServiceDate.Date == itemAllocationCSQCStaff.ServiceDate.Date);
                var CSIdNotInList = entity.Where(x => !CSList.Contains(x.Csid) && x.ServiceDate.Date == itemAllocationCSQCStaff.ServiceDate.Date);

                // Remove if data does not exist in the db.
                foreach (var item in CSIdNotInList)
                {
                    item.DeletedOn = DateTime.Now;
                    item.DeletedBy = _ApplicationContext.UserId;
                    item.Active = false;
                    CSIdtoRemove.Add(item);
                }

                _repo.SaveList(CSIdtoRemove, true);

                // Update if data already exist in the db

                if (CSList != null && CSList.Count() > 0)
                {
                    // Add if data is new it means id = 0;
                    foreach (var id in CSList)
                    {
                        if (!CSIdInList.Any() || CSIdInList.Where(x => x.Csid == id &&
                        x.ServiceDate.Date == itemAllocationCSQCStaff.ServiceDate.Date).Count() == 0)
                        {
                            CSIdtoAdd.Add(new SchScheduleC()
                            {
                                Csid = id,
                                Active = true,
                                CreatedBy = _ApplicationContext.UserId,
                                CreatedOn = DateTime.Now,
                                BookingId = request.BookingId,
                                ServiceDate = itemAllocationCSQCStaff.ServiceDate
                            });
                        }
                    }
                    _repo.SaveList(CSIdtoAdd, false);
                }
            }
        }


        public async Task<List<ScheduleBookingItemExportSummarynew>> ExportSummary(IEnumerable<ScheduleBookingItem> data)
        {
            return ScheduleMap.MapExportSummary(data);
        }

        public async Task<List<ScheduleBookingItemExportSummaryProductLevel>> ExportSummaryProductLevel(IEnumerable<ScheduleBookingItem> data)
        {
            return ScheduleMap.MapExportSummaryProductLevel(data);
        }
        //Get schedule QC and CS records make as inactive by bookingid
        public async Task UpdateScheduleOnReschedule(int bookingId)
        {
            try
            {
                //get QC list from sch_schedule_qc table using booking id
                var qcList = await _repo.GetQCDetails(bookingId);

                //get CS list from sch_schedule_cs table using booking id
                var csList = await _repo.GetCSDetails(bookingId);

                //expense details update as inactive
                await UpdateQcExpenseDetails(qcList);

                if (qcList != null && qcList.Count() > 0)
                {
                    foreach (var qcItem in qcList)
                    {
                        qcItem.Active = false;
                        qcItem.DeletedBy = _ApplicationContext.UserId;
                        qcItem.DeletedOn = DateTime.Now;
                    }
                    _repo.EditEntities(qcList);
                }
                if (csList != null && csList.Count() > 0)
                {
                    foreach (var csItem in csList)
                    {
                        csItem.Active = false;
                        csItem.DeletedBy = _ApplicationContext.UserId;
                        csItem.DeletedOn = DateTime.Now;
                    }
                    _repo.EditEntities(csList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //fetch the QC or Report checker List on click
        public async Task<AllocationStaffList> GetQCList(AllocationStaffSearchRequest request)
        {
            if (request == null)
                return new AllocationStaffList() { };

            if (!request.Skip.HasValue)
                request.Skip = 0;

            if (!request.Take.HasValue || request.Take.Value == 0)
                request.Take = 10;

            var response = new AllocationStaffList();

            DateTime serviceDate = new DateTime();
            if (DateTime.TryParse(request.ServiceDate, out serviceDate))
            {
                IEnumerable<DateTime> dateList = new DateTime[] { serviceDate };

                if (request.Type.ToLower() == HRProfile.Inspector.ToString().ToLower())
                {

                    var qcList = await GetQCStaffs(request, dateList);

                    if (qcList != null)
                    {
                        response.QCList = getStaffList(qcList, (int)QCType.QC, serviceDate);
                    }

                    response.AddtionalQCList = response.QCList;
                }
                else if (request.Type.ToLower() == HRProfile.ReportChecker.ToString().ToLower())
                {
                    var CSList = await getUserListByUserAccess(request.BookingId, request.OfficeId);
                    response.CSList = getStaffList(CSList, null, serviceDate);
                }

            }

            return response;
        }

        //get qc list 
        private async Task<IEnumerable<QCStaffInfo>> GetQCStaffs(AllocationStaffSearchRequest request, IEnumerable<DateTime> serviceDateList)
        {
            //get blocked QC list by Booking Id
            var QCBlockIdList = await _qcBlockmanager.GetQCBlockIdList(request.BookingId);

            var qcList = _repo.GetQCList();
            if (_ApplicationContext.LocationList != null && _ApplicationContext.LocationList.Any())
                qcList = qcList.Where(x => _ApplicationContext.LocationList.Contains(x.LocationId.Value));

            if (QCBlockIdList != null && QCBlockIdList.Any())
                qcList = qcList.Where(x => !QCBlockIdList.Contains(x.Id));

            if (request.EntityId.HasValue && request.EntityId > 0)
                qcList = qcList.Where(x => x.HrEntityMaps.Any(y => y.EntityId == request.EntityId));

            if (request.EmployeeType.HasValue && request.EmployeeType > 0)
                qcList = qcList.Where(x => x.EmployeeTypeId == request.EmployeeType);

            if (request.OfficeId.HasValue && request.OfficeId > 0)
                qcList = qcList.Where(x => x.LocationId == request.OfficeId);

            if (request.OutSourceCompany.HasValue && request.OutSourceCompany.Value > 0)
                qcList = qcList.Where(x => x.HroutSourceCompanyId == request.OutSourceCompany);

            if (request.ProductCategoryId.HasValue && request.ProductCategoryId.Value > 0)
                qcList = qcList.Where(x => x.HrStaffProductCategories.Any(x => x.ProductCategoryId == request.ProductCategoryId));

            if (request.ExpertiseId.HasValue && request.ExpertiseId.Value > 0)
                qcList = qcList.Where(x => x.HrStaffExpertises.Any(x => x.ExpertiseId == request.ExpertiseId));

            if (request.MarketSegmentId.HasValue && request.MarketSegmentId.Value > 0)
                qcList = qcList.Where(x => x.HrStaffMarketSegments.Any(x => x.MarketSegmentId == request.MarketSegmentId));

            if (request.ZoneId.HasValue && request.ZoneId.Value > 0)
                qcList = qcList.Where(x => x.CurrentCounty.ZoneId == request.ZoneId);

            if (request.StartPortId.HasValue && request.StartPortId.Value > 0)
                qcList = qcList.Where(x => x.StartPortId == request.StartPortId);

            if (!string.IsNullOrEmpty(request.SearchText) && !string.IsNullOrWhiteSpace(request.SearchText))
            {
                qcList = qcList.Where(x => x.PersonName.Contains(request.SearchText));
            }

            var result = await qcList.Skip(request.Skip.Value).Take(request.Take.Value).Select(x => new QCStaffInfo
            {
                Id = x.Id,
                Name = x.PersonName,
                Location = x.Location.LocationName,
                EmployeeType = x.EmployeeTypeId,
                EmailAddress = x.EmaiLaddress,
                EmergencyCall = x.EmergencyCall,
                StartPortId = x.StartPortNavigation.Id,
                StartPortName = x.StartPortNavigation.StartPortName,
                LeaveQC = x.HrLeaves,
                ScheduleQC = x.SchScheduleQcs
            }).AsNoTracking().ToListAsync();

            result.AsParallel().ForAll(x =>
            {
                x.ScheduleQC = x.ScheduleQC.Where(y => y.Active && serviceDateList.Contains(y.ServiceDate));
            });

            return result;
        }

        //get user list who has report checker role
        private async Task<IEnumerable<QCStaffInfo>> getUserListByUserAccess(int bookingId, int? officeId)
        {
            try
            {
                //get product category Details by bookingi id
                var productCategoryList = await _inspRepo.GetProductCategoryDetails(new[] { bookingId });

                //get booking data by bookingi id
                var bookingData = await _inspRepo.GetBookingTransaction(bookingId);

                var userAccess = new UserAccess()
                {
                    RoleId = (int)RoleEnum.ReportChecker,
                    OfficeId = bookingData.OfficeId.GetValueOrDefault(),
                    ServiceId = (int)Service.InspectionId,
                    CustomerId = bookingData.CustomerId,
                    StaffOfficeId = officeId,
                    ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                             productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>()
                };

                //report checker profile based on access
                var userList = await _userRightsManager.GetUserListByReportCheckerProfile(userAccess);

                return userList.Select(x => ScheduleMap.MapStaffData(x));

                //get report checked based on access
                //var userList =  await _userRightsManager.GetRoleAccessUserList((int)RoleEnum.ReportChecker);
                //return userList.Select(x => ScheduleMap.MapStaffData(x));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Calculate the Sample Size
        private int GetSamplingSize(IEnumerable<ScheduleProductsData> entity)
        {
            int sampleSize = 0;
            foreach (var item in entity)
            {
                if (item.CombineProductId != null)
                {
                    sampleSize = sampleSize + item.CombineAqlQuantity.GetValueOrDefault();
                }
                else if (item.CombineProductId == null)
                {
                    sampleSize = sampleSize + item.AqlQuantity.GetValueOrDefault();
                }
            }
            return sampleSize;
        }

        //Calculate the Combine Product count
        private int GetCombineProductCount(IEnumerable<ScheduleProductsData> entity)
        {
            var data = entity.Where(x => x.CombineProductId.HasValue);
            var item = data.GroupBy(p => p.CombineProductId, p => p, (key, _data) =>
                     new
                     {
                         Combinecount = _data.Where(x => x.CombineProductId == key).Select(x => x.CombineProductId).FirstOrDefault()
                     }).ToList();
            return item.Count();
        }

        private async Task<AllocationStaff> CreateScheduleAllocationDetails(DateTime item, IEnumerable<DTO.Quotation.QuotationManday> quotationManday, List<StaffScheduleQcRepo> qcDetails,
            List<StaffScheduleRepo> csDetailsList, int bookingId, int qcCountByLocation, IEnumerable<StaffLeavesDate> staffLeaves, IEnumerable<ActualManDayDateCount> actualManDayCount, IEnumerable<QcActualManDayRepo> qcActualMandayList)
        {
            AllocationStaff _allocationData = new AllocationStaff();
            double totalQcActualManday = 0;
            _allocationData.ServiceDate = item.Date;

            // set Available manday
            _allocationData.AvailableManDay = AvailableManDay(item.Date, qcCountByLocation, staffLeaves, actualManDayCount);
            _allocationData.ServiceDate = item.Date;

            if (quotationManday.Any())
            {
                //quotation estimated Manday and remarks
                _allocationData.ManDay = quotationManday.Any(x => x.ServiceDate
                                              == item.ToString(StandardDateFormat)) ? quotationManday.FirstOrDefault(x => x.ServiceDate
                                              == item.ToString(StandardDateFormat)).ManDay.GetValueOrDefault() : 0;
                _allocationData.Remarks = quotationManday.Any(x => x.ServiceDate
                                             == item.ToString(StandardDateFormat)) ? quotationManday.FirstOrDefault(x => x.ServiceDate
                                             == item.ToString(StandardDateFormat)).Remarks : null;

            }
            if (qcDetails != null && qcDetails.Any() && csDetailsList != null && csDetailsList.Any())
            {
                //Fetch Allocated QC on specific service date with booking
                var qcList = qcDetails.Where(x => x.ServiceDate == item.Date && x.BookingId == bookingId && x.QcType == (int)QCType.QC);
                _allocationData.QC = qcList.Select(x => ScheduleMap.MapAllocatedQcStaff(x, item, qcDetails));

                //Fetch Allocated Additional QC on specific service date with booking
                var additionalQcList = qcDetails.Where(x => x.ServiceDate == item.Date && x.BookingId == bookingId && x.QcType == (int)QCType.AdditionalQC);
                _allocationData.AddtionalQC = additionalQcList.Select(x => ScheduleMap.MapAllocatedQcStaff(x, item, qcDetails));

                //Fetch allocated Report Checker on specific service date with booking
                var csList = csDetailsList.Where(x => x.ServiceDate == item && x.BookingId == bookingId);
                _allocationData.CS = csList.Select(x => ScheduleMap.MapAllocatedCSStaff(x, item, csDetailsList));

                // Get total actual many day of Qc List for particaular date
                var distinctQcList = qcList.Select(x => x.StaffID).Distinct().ToList();
                totalQcActualManday = (double)Math.Round(qcActualMandayList.
                           Where(x => x.ServiceDate.Date == item.Date && distinctQcList.Contains(x.QcId)).Select(x => x.ActualManDay).Sum(), 1);
                _allocationData.ActualManDay = (double)Math.Round(totalQcActualManday, 1);
                _allocationData.IsQcVisibility = qcDetails.Where(x => x.ServiceDate == item.Date && x.BookingId == bookingId && x.QcType == (int)QCType.QC).Select(y => y.IsQcVisibility).FirstOrDefault();
            }

            return _allocationData;
        }

        //fetch data for Man Day forecast Page
        public async Task<MandayForecastResponse> GetManDayForecast(MandayForecastRequest request)
        {
            MandayForecastResponse response = new MandayForecastResponse();

            response.Data = new List<MandayForecastItem>();

            response.DataSourceList = _officeManager.GetLocationList().Result.DataSourceList;

            var _zoneData = _locationRepo.GetZoneDataSource();

            if (request.Officeidlst.Count() == 0 || request.Officeidlst == null)
            {
                //if user select only zone then apply office filter based on zone
                if (request.ZoneIdlst != null && request.ZoneIdlst.Any())
                {
                    var _officeIds = _zoneData.Where(x => request.ZoneIdlst.Contains(x.Id) && x.LocationId != null).Select(x => x.LocationId ?? default).Distinct().ToList().AsEnumerable();
                    request.Officeidlst = _officeIds;
                }
                else
                    request.Officeidlst = response.DataSourceList.Select(x => x.Id);
            }
            else
            {
                foreach (var _officeId in request.Officeidlst)
                {
                    var _zoneIds = _zoneData.Where(x => x.LocationId == _officeId).Select(x => x.Id).ToList();

                    if (_zoneIds.Where(x => request.ZoneIdlst.Contains(x)).Count() > 0)
                    {
                        continue;
                    }

                    request.ZoneIdlst.AddRange(_zoneIds);
                }
            }

            try
            {
                if (request.FromDate != null && request.ToDate != null)
                {
                    //get the individual dates from a date range
                    var dateList = Enumerable.Range(0, request.ToDate.ToDateTime().Subtract(request.FromDate.ToDateTime()).Days + 1)
                             .Select(d => request.FromDate.ToDateTime().AddDays(d));

                    //Get all the QCs from HR Staff
                    var QCList = await _repo.GetQCListByLocationForForecast(request.Officeidlst, dateList);

                    //get the quotation man day for the date range
                    var quotationData = _quotRepo.GetQuotationMandayByDate(dateList);
                    List<Manday> quotation = new List<Manday>();

                    //Filter by Zone
                    if (request.ZoneIdlst != null && request.ZoneIdlst.Any())
                    {
                        var bookingIdList = quotationData.Select(x => x.BookingId).Distinct().ToList();

                        var factoryDetails = await _inspRepo.GetFactorycountryId(bookingIdList);

                        List<ZoneManday> zoneBookings = factoryDetails.Where(x => request.ZoneIdlst.Contains(x.FactoryZoneId))
                           .Select(x => new ZoneManday
                           {
                               BookingId = x.BookingId,
                               ZoneId = x.FactoryZoneId,
                           }).ToList();

                        quotation = await quotationData.Where(x => zoneBookings.Select(y => y.BookingId).Contains(x.BookingId)).ToListAsync();

                        foreach (var item in quotation)
                        {
                            item.ZoneId = zoneBookings.Where(x => x.BookingId == item.BookingId).Select(x => x.ZoneId).FirstOrDefault();
                        }

                    }
                    else
                    {
                        quotation = await quotationData.ToListAsync();
                    }


                    if (QCList != null)
                    {
                        // zoneids from qclist zone 
                        var zoneids = QCList.Select(x => x.ZoneId).Distinct().ToList();

                        var zoneItemsById = await _locationRepo.GetZoneByIds(zoneids);

                        foreach (var date in dateList)
                        {
                            //get the list by removing the QC's on leave
                            var data = getStaffList(QCList, (int)QCType.QC, date);

                            var items = QCList.GroupBy(p => new { p.LocationId, date }, (key, _data) =>
                            new MandayForecastItem
                            {
                                Date = date.ToString(StandardDateFormat, CultureInfo.InvariantCulture),
                                DayOftheWeek = date.ToString(Day_DateFormat),
                                Location = _data.Where(x => x.LocationId == key.LocationId).Select(x => x.Location).FirstOrDefault(),
                                AvailableQcCount = data.Where(x => x.LocationId == key.LocationId).Count(),
                                ManDaycount = quotation.Where(x => x.ServiceDate == date && x.OfficeId == key.LocationId).Select(x => x.NoOfManday).Sum(),
                                QcOnLeaveCount = QCList.Where(x => x.LocationId == key.LocationId).Count() - data.Where(x => x.LocationId == key.LocationId).Count(),
                                Color = GetDataColor(quotation.Where(x => x.ServiceDate == date && x.OfficeId == key.LocationId).Select(x => x.NoOfManday).Sum(), data.Where(x => x.LocationId == key.LocationId).Count())
                            });

                            response.Data.AddRange(items);
                            if (request.ZoneIdlst.Count() > 0 && request.ZoneIdlst != null)
                            {


                                items = QCList.Where(x => request.ZoneIdlst.Contains(x.ZoneId)).GroupBy(p => new { p.ZoneId, date }, (key, _data) =>
                                  new MandayForecastItem
                                  {
                                      Date = date.ToString(StandardDateFormat, CultureInfo.InvariantCulture),
                                      DayOftheWeek = date.ToString(Day_DateFormat),
                                      Location = zoneItemsById.Where(x => x.Id == key.ZoneId).Select(x => x.Name).FirstOrDefault(),
                                      AvailableQcCount = data.Where(x => x.ZoneId == key.ZoneId).Count(),
                                      ManDaycount = quotation.Where(x => x.ServiceDate == date && x.ZoneId == key.ZoneId).Select(x => x.NoOfManday).Sum(),
                                      QcOnLeaveCount = QCList.Where(x => x.ZoneId == key.ZoneId).Count() - data.Where(x => x.ZoneId == key.ZoneId).Count(),
                                      Color = GetDataColor(quotation.Where(x => x.ServiceDate == date && x.ZoneId == key.ZoneId).Select(x => x.NoOfManday).Sum(), data.Where(x => x.ZoneId == key.ZoneId).Count())
                                  });
                                response.Data.AddRange(items);
                            }
                        }
                    }
                }
                //Allocated location based on sum of QC Availability and QC LeaveCount
                response.Data = response.Data.OrderByDescending(x => x.AvailableQcCount + x.QcOnLeaveCount).ToList();
                var allocatedLocation = response.Data.Select(x => x.Location).Distinct().ToList();

                List<ManDayScheduleLocName> emptyLocation = new List<ManDayScheduleLocName>();
                if (request.Officeidlst.Count() > 0 && request.Officeidlst != null)
                {
                    var res = await _officeRepo.GetLocationListByIds(request.Officeidlst.ToList());
                    response.LocationName = (from loc in allocatedLocation
                                             join locDetails in res on loc equals locDetails.Name
                                             select new ManDayScheduleLocName
                                             {
                                                 Id = locDetails.Id,
                                                 Name = locDetails.Name,
                                                 ZoneId = 0,
                                                 officeId = locDetails.Id
                                             }).ToList();
                    emptyLocation = res.Where(x => !allocatedLocation.Contains(x.Name))
                        .Select(x => new ManDayScheduleLocName { Id = x.Id, ZoneId = 0, Name = x.Name, officeId = x.Id }).ToList();
                }
                if (request.ZoneIdlst.Count() > 0 && request.ZoneIdlst != null)
                {
                    var _zoneFilterData = _zoneData.Where(x => request.ZoneIdlst.Contains(x.Id)).Select(x => new ManDayScheduleLocName
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ZoneId = x.Id,
                        officeId = x.LocationId.GetValueOrDefault()
                    }).OrderBy(x => x.Name).ToList();

                    response.LocationName.AddRange(_zoneFilterData);

                    //kept the empty location keet it last
                    response.LocationName.AddRange(emptyLocation);

                }
                else
                {
                    response.LocationName.AddRange(emptyLocation);
                }

                response.Result = ScheduleSearchResponseResult.Success;
            }

            catch (Exception e)
            {
                response.Result = ScheduleSearchResponseResult.NotFound;
            }

            return response;
        }

        //Get the color based on the QC availability
        private string GetDataColor(double totalManday, int availableManday)
        {
            if (availableManday == 0)
            {
                return ReportResult.Red.ToString();
            }

            double num = availableManday - totalManday;
            double num2 = num / availableManday;
            var res = num2 * 100;

            if (res > 20) //|| availableManday == totalManday
            {
                return ReportResult.Limegreen.ToString();
            }

            else if (res < 10)
            {
                return ReportResult.Red.ToString();
            }

            else if (res < 20)
            {
                return ReportResult.Orange.ToString();
            }

            return null;
        }

        //get the staff details on leave for a date
        public async Task<StaffLeaveInfoResponse> GetStaffDetailsWithLeave(string date, int locationId, int zoneid)
        {
            StaffLeaveInfoResponse response = new StaffLeaveInfoResponse();

            try
            {
                if (date != null && locationId > 0)
                {
                    if (DateTime.TryParseExact(date.Replace("-", "/"), StandardDateFormat, new CultureInfo("en-us"), DateTimeStyles.None, out DateTime servicedate))
                    {
                        response.Data = await _hrRepo.GetStaffWithLeave(servicedate, locationId, zoneid);
                        response.Result = LeaveResult.Success;
                    }
                }
            }
            catch (Exception e)
            {
                response.Result = LeaveResult.CannotFindLeaveRequest;
            }

            return response;
        }

        public async Task<QuotScheduleMandayResponse> GetMandayDetails(int bookingId)
        {
            var response = new QuotScheduleMandayResponse();
            var data = new List<QuotationManday>();
            try
            {
                if (bookingId > 0)
                {
                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionScheduled))
                    {
                        var mandayByDate = await _quotRepo.GetQuotationInspManDay(bookingId);

                        var quotManday = await _quotRepo.GetQuotationTravelManDay(bookingId);
                        if (mandayByDate != null && mandayByDate.Any())
                        {
                            data = mandayByDate.Select(y => QuotationMap.GetQuQuotationInspMandayDTO(y)).ToList();
                        }

                        var item = new QuotScheduleManday()
                        {
                            BookingId = quotManday.BookingId,
                            TotalManday = quotManday.ManDay,
                            TravelManday = quotManday.TravelManDay,
                            MandayList = data.ToList(),
                            SuggestedManday = quotManday.SuggestedManday
                        };
                        response.Data = item;
                        response.Result = ScheduleSearchResponseResult.Success;
                    }
                }

                if (data == null || !data.Any())
                {
                    response.Result = ScheduleSearchResponseResult.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.Result = ScheduleSearchResponseResult.Other;
            }
            return response;
        }

        public async Task<int> SaveManday(QuotScheduleManday data)
        {
            var response = await _quotRepo.GetQuotationInspManDay(data.BookingId);
            var res = new List<QuQuotationInspManday>();
            try
            {
                foreach (var item in data.MandayList)
                {
                    DateTime date = new DateTime();
                    if (DateTime.TryParseExact(item.ServiceDate, StandardDateFormat, new CultureInfo("en-us"), DateTimeStyles.None, out date))
                    {
                        var result = response.Where(x => x.ServiceDate == date).FirstOrDefault();
                        result.NoOfManday = item.ManDay;
                        result.Remarks = item.Remarks;
                        result.UpdatedBy = _ApplicationContext.UserId;
                        result.UpdatedDate = DateTime.Now;

                        res.Add(result);
                    }

                }

                _repo.SaveList(res, true);
                return (int)ScheduleSearchResponseResult.Success;
            }
            catch (Exception ex)
            {
                return (int)ScheduleSearchResponseResult.Other;
            }
        }

        //get qc by qc name substring
        public async Task<DataSourceResponse> GetQcDataSource(CommonQcSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _repo.GetQcDataSource();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Staff.PersonName != null && EF.Functions.Like(x.Staff.PersonName, $"%{request.SearchText.Trim()}%"));
                }

                // filter by country ids
                if (request.Qcids != null && request.Qcids.Any())
                {
                    data = data.Where(x => request.Qcids.Contains(x.StaffId));
                }

                if (request.OfficeCountryIds != null && request.OfficeCountryIds.Any())
                {
                    data = data.Where(x => request.OfficeCountryIds.Contains(x.Staff.NationalityCountryId.GetValueOrDefault()));
                }

                var qcList = await data
                    .Select(x => new CommonDataSource
                    {
                        Id = x.StaffId,
                        Name = x.Staff.PersonName
                    }).Skip(request.Skip).Take(request.Take).ToListAsync();

                if (qcList == null || !qcList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = qcList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Update the schedule when the new serviceTo date is less than the old serviceTo date
        public async Task UpdateScheduleOnRescheduleToLesserDate(BookingDateInfo request, InspTransaction entity)
        {
            try
            {
                //get the reschedule date list from the range
                var rescheduleDateList = Enumerable.Range(0, 1 + request.ServiceToDate.ToDateTime().Subtract(request.ServiceFromDate.ToDateTime()).Days)
                               .Select(offset => request.ServiceFromDate.ToDateTime().AddDays(offset)).ToArray();

                //get the initial date list from the range
                var serviceDateList = Enumerable.Range(0, 1 + entity.ServiceDateTo.Subtract(entity.ServiceDateFrom).Days)
                            .Select(offset => entity.ServiceDateFrom.AddDays(offset)).ToArray();

                //get the dates for which quotation and qc to be updated
                var datesToRemove = serviceDateList.Except(rescheduleDateList);

                if (datesToRemove != null && datesToRemove.Any())
                {
                    //get QC list from sch_schedule_qc table using booking id
                    var qcList = await _repo.GetQCDetails(entity.Id);
                    qcList = qcList.Where(x => datesToRemove.Contains(x.ServiceDate)).ToList();

                    //expense details update as inactive
                    await UpdateQcExpenseDetails(qcList);

                    //get CS list from sch_schedule_cs table using booking id
                    var csList = await _repo.GetCSDetails(entity.Id);
                    csList = csList.Where(x => datesToRemove.Contains(x.ServiceDate)).ToList();

                    if (qcList != null && qcList.Any())
                    {
                        foreach (var qcItem in qcList)
                        {
                            qcItem.Active = false;
                            qcItem.DeletedBy = _ApplicationContext.UserId;
                            qcItem.DeletedOn = DateTime.Now;
                            qcItem.ActualManDay = 0;
                        }
                        _repo.SaveList(qcList);

                        await ActualMandayCalculation(qcList);
                    }
                    if (csList != null && csList.Any())
                    {
                        foreach (var csItem in csList)
                        {
                            csItem.Active = false;
                            csItem.DeletedBy = _ApplicationContext.UserId;
                            csItem.DeletedOn = DateTime.Now;
                        }
                        _repo.EditEntities(csList);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// update the schedule table on booking reschedule and cancel
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task UpdateScheduleQcMandayOnCancelReschedule(int bookingId, bool isKeepQCForTravelExpense)
        {
            var qcList = await _repo.GetQCDetails(bookingId);

            if (qcList != null && qcList.Any())
            {
                foreach (var qc in qcList)
                {
                    qc.DeletedOn = DateTime.Now;
                    qc.DeletedBy = _ApplicationContext.UserId;
                    qc.Active = isKeepQCForTravelExpense;
                    qc.ActualManDay = 0;

                    qc.KeepQcforTravelExpense = isKeepQCForTravelExpense;

                    _repo.Save(qc, true);

                }
                await ActualMandayCalculation(qcList);
            }
        }

        /// <summary>
        /// manday logic - calculate man day for different booking on same day and same QC. 
        /// </summary>
        /// <param name="QcId"></param>
        /// <param name="serviceDate"></param>
        /// <returns></returns>
        public async Task ActualMandayCalculation(List<SchScheduleQc> qcList)
        {
            var qcidList = qcList.Select(x => x.Qcid).Distinct().ToList();
            var dateList = qcList.Select(x => x.ServiceDate).Distinct().ToList();

            List<SchScheduleQc> QCIdModify = new List<SchScheduleQc>();

            //fetch number of booking per qc & date
            var qcData = await _repo.GetQcManDayData(qcidList, dateList);
            var qcidlist = qcData.Select(x => x.QcId).Distinct().ToList();
            var servicedateslst = qcData.Select(x => x.ServiceDate).Distinct().ToList();
            var AllQcList = await _repo.GetQcListManDay(qcidlist, servicedateslst);
            var scheduleQcCustomerFactoryDetails = await _repo.GetQCBookingCustomerFactory(qcidlist);
            foreach (var item in qcData)
            {
                bool firstIndexCalcManDay = false;

                //manday calc
                var manDayCalc = item.TotalBooking > 0 ?
                                (double)Math.Round(decimal.Divide(1, item.TotalBooking), 2) : 1;

                //when sum the manday for one QC. it has to 1(one). so we are doing below logic
                var mandayCalOneItem = (manDayCalc + (1 - (manDayCalc * item.TotalBooking)));
                var qcCustomerFactoryData = scheduleQcCustomerFactoryDetails.Where(x => x.CustomerId == item.CustomerId && x.FactoryId == item.FactoryId && x.QcId == item.QcId).Select(y => y.Id).ToList();
                var QcList = AllQcList.Where(x => x.Qcid == item.QcId && x.ServiceDate == item.ServiceDate && qcCustomerFactoryData.Contains(x.Id)).ToList();
                //update the manday in table
                foreach (var qc in QcList)
                {
                    qc.ModifiedOn = DateTime.Now;
                    qc.ModifiedBy = _ApplicationContext.UserId;
                    if (!firstIndexCalcManDay)
                    {
                        qc.ActualManDay = mandayCalOneItem;
                    }
                    else
                    {
                        qc.ActualManDay = manDayCalc;
                    }
                    firstIndexCalcManDay = true;
                }
                QCIdModify.AddRange(QcList);
            }
            _repo.SaveList(QCIdModify, true);
        }
        //get qc visible false
        public async Task<BookingDataQcVisibleResponse> GetQcVisibilityByBooking(QcVisibilityBookingRequest request)
        {
            try
            {
                var response = new BookingDataQcVisibleResponse();

                var data = await _repo.GetQCBookingVisibleDetails(request.BookingIdlst);

                var result = data.GroupBy(p => new { p.BookingId, p.ServiceDate, p.IsVisibleToQc }, p => p,
                    (key, _data) => new BookingDataQcVisible
                    {
                        BookingId = key.BookingId,
                        ServiceDate = key.ServiceDate.ToString(StandardDateFormat),
                        IsQcVisibility = key.IsVisibleToQc.GetValueOrDefault()
                    }).OrderBy(x => x.BookingId).ToList();


                if (result == null || !result.Any())
                    response.Result = ScheduleSearchResponseResult.NotFound;
                else
                {
                    response.Data = result;
                    response.Result = ScheduleSearchResponseResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///UpdateQcVisibileData
        /// </summary>
        /// <param name="BookingDataQcVisible"></param>
        /// <returns></returns>
        public async Task<int> UpdateQcVisibileData(BookingDataQcVisibleRequest request)
        {
            var bookingIds = request.BookingDataQcVisible.Select(x => x.BookingId).ToList();
            var data = await _repo.GetQCBookingVisibleDetails(bookingIds);

            List<SchScheduleQc> QCVisibleData = new List<SchScheduleQc>();
            try
            {

                foreach (var item in request.BookingDataQcVisible)
                {
                    DateTime date = new DateTime();
                    if (DateTime.TryParseExact(item.ServiceDate, StandardDateFormat, new CultureInfo("en-us"), DateTimeStyles.None, out date))
                    {
                        var qcVisibleData = data.Where(x => x.BookingId == item.BookingId && x.ServiceDate == date);

                        foreach (var visibledata in qcVisibleData)
                        {
                            visibledata.IsVisibleToQc = true;
                        }
                        QCVisibleData.AddRange(qcVisibleData);
                    }
                }
                _repo.SaveList(QCVisibleData, true);
                return (int)ScheduleSearchResponseResult.Success;
            }
            catch (Exception)
            {

                return (int)ScheduleSearchResponseResult.Other;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<ScheduleProductModelResponse> GetProductPODetails(int bookingId)
        {
            var productData = await _repo.GetProductPODetails(bookingId);

            var POData = await _repo.GetPODetails(bookingId);

            var response = new ScheduleProductModelResponse();

            if (productData != null && productData.Any())
            {
                response.ScheduleProductModel = productData.Select(x => ScheduleMap.MaproductData(x, POData)).ToList();
                response.Result = SaveScheduleResponseResult.Success;
            }
            else
            {
                response.Result = SaveScheduleResponseResult.NotFound;
            }

            return response;
        }

        /// <summary>
        /// update QC expense details
        /// </summary>
        /// <param name="scheduleQCList"></param>
        /// <returns></returns>
        private async Task UpdateQcExpenseDetails(List<SchScheduleQc> scheduleQCList)
        {
            var bookingId = scheduleQCList.Select(x => x.BookingId).FirstOrDefault();

            if (bookingId > 0)
            {
                var qcAutoTravelExpenses = await _repo.GetQCAutoTravelExpensesByInspectionId(bookingId);
                var qcAutoFoodExpenses = await _repo.GetQCAutoFoodExpensesByInspectionId(bookingId);

                var scheduleDate = scheduleQCList.Select(x => x.ServiceDate).Distinct().ToList();

                //get expense by schedule date 
                qcAutoTravelExpenses = qcAutoTravelExpenses.Where(x => scheduleDate.Contains(x.ServiceDate.GetValueOrDefault())).ToList();
                qcAutoFoodExpenses = qcAutoFoodExpenses.Where(x => scheduleDate.Contains(x.ServiceDate.GetValueOrDefault())).ToList();

                // inactive the record
                foreach (var item in qcAutoTravelExpenses)
                {
                    item.DeletedOn = DateTime.Now;
                    item.DeletedBy = _ApplicationContext.UserId;
                    item.Active = false;
                }

                _repo.EditEntities(qcAutoTravelExpenses);

                foreach (var item in qcAutoFoodExpenses)
                {
                    item.DeletedOn = DateTime.Now;
                    item.DeletedBy = _ApplicationContext.UserId;
                    item.Active = false;
                }
                _repo.EditEntities(qcAutoFoodExpenses);
                await _repo.Save();
            }
        }
    }
}