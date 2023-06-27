using BI.Maps;
using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.EmailLog;
using DTO.EmailSend;
using DTO.EventBookingLog;
using DTO.FullBridge;
using DTO.Inspection;
using DTO.Invoice;
using DTO.MasterConfig;
using DTO.Quotation;
using DTO.RepoRequest.Enum;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BI.FBReportManager;

namespace BI
{
    public class EmailSendManager : ApiCommonData, IEmailSendManager
    {
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly IUserRepository _userRepo = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly ISupplierManager _suppliermanager = null;
        private readonly IEmailSendRepository _emailSendRepo = null;
        private readonly IFileManager _fileManager = null;
        private readonly EmailSendMap _emailsendmap = null;
        private readonly BookingMap _bookingmap = null;
        private readonly IKpiCustomRepository _kpiRepo = null;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IInvoicePreivewRepository _invoicePreivewRepository;
        private readonly IEmailConfigurationRepository _emailConfigurationRepository;
        private readonly ICustomerCheckPointRepository _customerCheckPointRepository;
        private readonly IConfiguration _configuration;
        private readonly IHelper _helper = null;
        private readonly FBSettings _fbSettings = null;
        private readonly IEventBookingLogManager _fbLog = null;
        private readonly ICustomerCheckPointManager _customerCheckPointManager = null;
        private readonly IDashboardRepository _dashboardRepo = null;

        public EmailSendManager(IInspectionBookingManager inspManager, IUserRepository userRepo, IAPIUserContext applicationContext,
            IInspectionBookingRepository inspRepo, ITenantProvider filterService, IInvoiceRepository invoiceRepository, IEmailConfigurationRepository emailConfigurationRepository, ICustomerCheckPointRepository customerCheckPointRepository,
            IOfficeLocationManager office, IQuotationRepository quotationRepository, IInvoicePreivewRepository invoicePreivewRepository, IConfiguration configuration,
            ISupplierManager suppliermanager, IEmailSendRepository emailSendRepo,
            IFileManager fileManager, IKpiCustomRepository kpiRepo, IHelper helper, IOptions<FBSettings> fbSettings, IEventBookingLogManager fbLog, ICustomerCheckPointManager customerCheckPointManager, IDashboardRepository dashboardRepo)
        {
            _inspManager = inspManager;
            _userRepo = userRepo;
            _applicationContext = applicationContext;
            _inspRepo = inspRepo;
            _office = office;
            _suppliermanager = suppliermanager;
            _emailSendRepo = emailSendRepo;
            _fileManager = fileManager;
            _emailsendmap = new EmailSendMap();
            _bookingmap = new BookingMap();
            _kpiRepo = kpiRepo;
            _invoiceRepository = invoiceRepository;
            _quotationRepository = quotationRepository;
            _invoicePreivewRepository = invoicePreivewRepository;
            _emailConfigurationRepository = emailConfigurationRepository;
            _customerCheckPointRepository = customerCheckPointRepository;
            _configuration = configuration;
            _customerCheckPointManager = customerCheckPointManager;
            _helper = helper;
            _fbSettings = fbSettings.Value;
            _fbLog = fbLog;
            _dashboardRepo = dashboardRepo;
        }

        /// <summary>
        /// get the inspection status
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetBookingStatusList()
        {
            return await _inspManager.GetBookingStatusList();
        }

        //Get the AE list
        public async Task<AeListResponse> GetAeList()
        {
            var res = await _userRepo.GetAEList();

            if (res == null || !res.Any())
            {
                return new AeListResponse { Result = EmailSendSummaryResponseResult.NotFound };
            }

            return new AeListResponse
            {
                Data = res.ToList(),
                Result = EmailSendSummaryResponseResult.Success
            };
        }

        //Search the email send summary
        public async Task<EmailSendSummaryResponse> GetEmailSendSummary(EmailSendSummaryRequest request)
        {
            if (request == null)
                return new EmailSendSummaryResponse() { Result = EmailSendSummaryResponseResult.NotFound };

            if (request.ServiceId == (int)Service.InspectionId)
            {
                var response = new EmailSendSummaryResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

                var deptIdList = new List<int>();
                var brandIdList = new List<int>();
                var customerIdList = new List<int?>();

                var bookIdwithDept = new List<int>();
                var newBookIdListWithoutDept = new List<int>();

                var bookIdWithBrand = new List<int>();
                var newBookIdListWithoutBrand = new List<int>();

                //filter data based on user type
                switch (_applicationContext.UserType)
                {
                    case UserTypeEnum.Customer:
                        {
                            request.CustomerId = request?.CustomerId != null && request?.CustomerId != 0 ? request?.CustomerId : _applicationContext.CustomerId;
                            break;
                        }
                    case UserTypeEnum.Factory:
                        {
                            request.FactoryIdlst = request.FactoryIdlst != null && request.FactoryIdlst.Any() ? request.FactoryIdlst : new List<int>().Append(_applicationContext.FactoryId);
                            break;
                        }
                    case UserTypeEnum.Supplier:
                        {
                            request.SupplierId = request.SupplierId != null && request.SupplierId != 0 ? request.SupplierId.Value : _applicationContext.SupplierId;
                            break;
                        }
                }

                if (request.Index == null || request.Index.Value <= 0)
                    request.Index = 1;

                if (request.pageSize == null || request.pageSize.Value == 0)
                    request.pageSize = 20;

                int skip = (request.Index.Value - 1) * request.pageSize.Value;

                int take = request.pageSize.Value;

                var cuslist = new List<int>();
                if (_applicationContext.UserType == UserTypeEnum.InternalUser)
                {
                    if (request.Officeidlst != null && !request.Officeidlst.Any())
                    {
                        var _cusofficelist = await _office.GetOfficesByUserIdAsync(_applicationContext.StaffId);
                        request.Officeidlst = _cusofficelist == null || !_cusofficelist.Any() ? request.Officeidlst : _cusofficelist.Where(x => request.Officeidlst.Contains(x.Id)).Select(x => (int?)x.Id).ToList();
                    }

                    if (request?.CustomerId > 0)
                        cuslist.Add(request.CustomerId.Value);
                }
                else
                {
                    if (request?.CustomerId > 0)
                        cuslist.Add(request.CustomerId.Value);
                }

                var data = _inspRepo.GetAllInspections();

                if (request != null && cuslist != null && cuslist.Count > 0)
                {
                    data = data.Where(x => cuslist.Contains(x.CustomerId.GetValueOrDefault()));
                }


                if (request != null && request.SupplierId != 0 && request.SupplierId != null)
                {
                    data = data.Where(x => x.SupplierId == request.SupplierId);
                }

                if (request != null && request.FactoryIdlst != null && request.FactoryIdlst.Any())
                {
                    data = data.Where(x => request.FactoryIdlst.ToList().Contains(x.FactoryId.GetValueOrDefault()));
                }

                if (request != null && request.Officeidlst != null && request.Officeidlst.Any()) //&& data.Any(x => x.OfficeId != null))
                {
                    data = data.Where(x => x.OfficeId != null && request.Officeidlst.ToList().Contains(x.OfficeId.Value));
                }

                if (request != null && request.StatusIdlst != null && request.StatusIdlst.Any())
                {
                    data = data.Where(x => request.StatusIdlst.ToList().Contains(x.StatusId));
                }

                //filter by collection
                if (request != null && request.SelectedCollectionIdList != null && request.SelectedCollectionIdList.Any())
                {
                    data = data.Where(x => request.SelectedCollectionIdList.Contains(x.CollectionId));
                }

                if (Enum.TryParse(request.DateTypeid.ToString(), out SearchType _datesearchtype))
                {
                    if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                    {
                        data = data.Where(x => !((x.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.ServiceDateTo < request.FromDate.ToDateTime())));

                    }
                }

                if (request.SearchTypeId == (int)SearchType.BookingNo)
                {
                    if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()) && int.TryParse(request.SearchTypeText?.Trim(), out int bookid))
                    {
                        data = data.Where(x => x.BookingId == bookid);
                    }
                }

                if (request.SearchTypeId == (int)SearchType.CustomerBookingNo)
                {
                    if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()))
                    {
                        data = data.Where(x => EF.Functions.Like(x.CustomerBookingNo.Trim(), $"%{request.SearchTypeText.Trim()}%"));
                    }
                }

                var bookingCustomerList = await data.Select(x => new BookingCustomerData
                {
                    BookingId = x.BookingId,
                    CustomerId = x.CustomerId
                }).Distinct().ToListAsync();

                var bookingIds = bookingCustomerList.Select(x => x.BookingId).ToList();

                //apply factory country filter
                if (request.SelectedCountryIdList != null && request.SelectedCountryIdList.Any())
                {
                    //take distinct factoryIds
                    var factoryIds = await data.Select(x => x.FactoryId).Distinct().ToListAsync();
                    //take factory address details by factoryids list
                    var factoryAddressList = await _suppliermanager.GetSupplierOrFactoryLocations(factoryIds);
                    //apply country filter and take factory ids
                    var filteredFactoryIds = factoryAddressList.Where(x => request.SelectedCountryIdList.Contains(x.CountryId)).
                                                                        Select(x => x.FactoryId).ToList();
                    //filter the booking by factory ids
                    data = data.Where(x => filteredFactoryIds.Contains(x.FactoryId.GetValueOrDefault()));
                }

                //Get the service Type for the bookings
                var serviceTypeList = await _inspRepo.GetServiceType(bookingIds);


                //Advance Search Start

                if (data == null)
                {
                    response.Result = EmailSendSummaryResponseResult.NotFound;
                    return response;
                }

                //Filter based on Service type
                if (request.ServiceTypelst != null && request.ServiceTypelst.Any())
                {
                    var bookingIdsByServiceType = serviceTypeList.Where(y => request.ServiceTypelst.Contains(y.serviceTypeId)).Select(x => x.InspectionId);

                    data = data.Where(x => bookingIdsByServiceType.Contains(x.BookingId));
                }


                if (request.UserIdList != null && request.UserIdList.Any())
                {
                    //user config list 
                    var AEConfigList = await _userRepo.GetAECustomerConfigList(request?.UserIdList);

                    //select dept id from userconfig list
                    var AEDeptIdList = AEConfigList?.Where(x => x.DepartmentId > 0).Select(x => x.DepartmentId.GetValueOrDefault()).Distinct().ToList();

                    //select brand id from user config list
                    var AEBrandIdList = AEConfigList?.Where(x => x.BrandId > 0).Select(x => x.BrandId.GetValueOrDefault()).Distinct().ToList();

                    //select distinct customer from user config list
                    var AEDistinctCusId = AEConfigList?.Select(z => z.CustomerId).Distinct().ToList();

                    //select ae customer id and dept id list
                    var AECustomerDeptList = AEConfigList.Where(x => x.DepartmentId > 0).Select(x => new CustomerDeptData
                    {
                        CustomerId = x.CustomerId,
                        DeptId = x.DepartmentId
                    }).ToList();

                    //select ae customer id and brand id list
                    var AECustomerBrandList = AEConfigList.Where(x => x.BrandId > 0).Select(x => new CustomerBrandData
                    {
                        CustomerId = x.CustomerId,
                        BrandId = x.BrandId
                    }).ToList();

                    //Apply Filter in booking data with AE configuration
                    if (AEDistinctCusId.Any())
                    {
                        data = data.Where(x => AEDistinctCusId.Contains(x.CustomerId.GetValueOrDefault()));
                    }

                    //common customer list from AE list and booking List
                    customerIdList = bookingCustomerList.Select(x => x.CustomerId).Intersect(AECustomerDeptList.Select(x => x.CustomerId)).ToList();

                    //get booking ids who has dept AE config
                    bookIdwithDept = bookingCustomerList.Where(x => customerIdList.Contains(x.CustomerId)).Select(x => x.BookingId).ToList();

                    // not exists in bookidwithdep
                    newBookIdListWithoutDept = bookingCustomerList.Where(x => !bookIdwithDept.Contains(x.BookingId)).Select(x => x.BookingId).ToList();

                    var brandCustomerIdList = new List<int?>();

                    //common brand customer list from AE list and booking List
                    brandCustomerIdList = bookingCustomerList.Select(x => x.CustomerId).Intersect(AECustomerBrandList.Select(x => x.CustomerId)).ToList();

                    //get customer id from brand
                    customerIdList = customerIdList.Union(brandCustomerIdList).ToList();

                    //get booking ids who has brand AE config
                    bookIdWithBrand = bookingCustomerList.Where(x => brandCustomerIdList.Contains(x.CustomerId)).Select(x => x.BookingId).ToList();

                    // not exists in bookidwithbrand
                    newBookIdListWithoutBrand = bookingCustomerList.Where(x => !bookIdWithBrand.Contains(x.BookingId)).Select(x => x.BookingId).ToList();


                    //add AE dept list to generic dept list to filter
                    if (AEDeptIdList.Any())
                        deptIdList.AddRange(AEDeptIdList);

                    //add AE brand list to generic brand list to filter
                    if (AEBrandIdList.Any())
                        brandIdList.AddRange(AEBrandIdList);

                }

                //assign the dept and customer from request
                if (request.SelectedDeptIdList != null && request.SelectedDeptIdList.Any() && request.CustomerId != null && request.CustomerId > 0)
                {
                    if (deptIdList.Any())
                    {
                        deptIdList = deptIdList.Intersect(request.SelectedDeptIdList).ToList();
                        if (!deptIdList.Any())
                        {
                            response.Result = EmailSendSummaryResponseResult.NotFound;
                            return response;
                        }
                    }
                    else
                        deptIdList = request.SelectedDeptIdList.ToList();

                    customerIdList.Add(request.CustomerId);
                }

                //assign the brand and customer from request
                if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
                {
                    if (brandIdList.Any())
                    {
                        brandIdList = brandIdList.Intersect(request.SelectedBrandIdList).ToList();
                        if (!brandIdList.Any())
                        {
                            response.Result = EmailSendSummaryResponseResult.NotFound;
                            return response;
                        }
                    }
                    else
                        brandIdList = request.SelectedBrandIdList.ToList();

                    //brandIdList.AddRange(request.SelectedBrandIdList);

                    customerIdList.Add(request.CustomerId);
                }

                //filter by buyer
                if (request.SelectedBuyerIdList != null && request.SelectedBuyerIdList.Any())
                {
                    //buyer filter from booking
                    var _bookingIds = await _inspRepo.GetBookingIdsByBuyersAndBookings(request.SelectedBuyerIdList, bookingIds);

                    //filter booking by buyer ids of booking id
                    data = data.Where(x => _bookingIds.Contains(x.BookingId));
                }
                //Advance Search end

                //if dept id list and customer id list has data
                if (deptIdList.Any() && customerIdList.Any())
                {
                    var _bookingIdswithFilterDept = new List<int>();

                    if (bookIdwithDept.Any())
                    {
                        //AE dept filter from booking
                        _bookingIdswithFilterDept = await _inspRepo.GetBookingIdsByDeptsAndBookings(deptIdList, bookIdwithDept);
                    }
                    else
                    {
                        //dept filter from booking
                        _bookingIdswithFilterDept = await _inspRepo.GetBookingIdsByDeptsAndBookings(deptIdList, bookingIds);
                    }

                    var finalBookidList = new List<int>();

                    //get booking id list of without config dept booking ids and filter by dept booking ids
                    finalBookidList = newBookIdListWithoutDept.Union(_bookingIdswithFilterDept).ToList();

                    //filter booking by dept ids of booking id
                    data = data.Where(x => finalBookidList.Contains(x.BookingId));
                }
                //if brand id list and customer id list has data
                if (brandIdList.Any() && customerIdList.Any())
                {
                    var _bookingIdswithFilterBrand = new List<int>();

                    if (bookIdWithBrand.Any())
                    {
                        //AE brand filter from booking
                        _bookingIdswithFilterBrand = await _inspRepo.GetBookingIdsByBrandsAndBookings(brandIdList, bookIdWithBrand);
                    }
                    else
                    {
                        //brand filter from booking
                        _bookingIdswithFilterBrand = await _inspRepo.GetBookingIdsByBrandsAndBookings(brandIdList, bookingIds);
                    }

                    var finalBookidList = new List<int>();

                    //get booking id list of without config brand booking ids and filter by brand booking ids
                    finalBookidList = newBookIdListWithoutBrand.Union(_bookingIdswithFilterBrand).ToList();

                    // filter booking by brand ids of booking id
                    data = data.Where(x => finalBookidList.Contains(x.BookingId));
                }

                var bookingIdList = await data.Select(x => x.BookingId).ToListAsync();


                //get report data to filter the report result 
                var reportData = await _inspRepo.GetInspectionReportData(bookingIdList);

                if (request != null && request.SelectedAPIResultIdList != null && request.SelectedAPIResultIdList.Any()) //&& data.Any(x => x.OfficeId != null))
                {

                    //data=data.Where(x=>x.Fbre)
                    reportData = reportData.Where(x => x.FBResultId != null && request.SelectedAPIResultIdList.ToList().Contains(x.FBResultId.Value)).ToList();

                    //get the bookingids filtered with report results
                    var filteredReportBookingIds = reportData.Select(x => x.BookingId).ToList();

                    //filter the bookings with the filtered report bookingids
                    if (filteredReportBookingIds != null && filteredReportBookingIds.Any())
                    {
                        data = data.Where(x => filteredReportBookingIds.Contains(x.BookingId));
                    }
                }

                var statusIds = await data.Select(x => x.StatusId).ToListAsync();

                // take total booking count after filter
                response.TotalCount = statusIds.Count;

                try
                {

                    if (response.TotalCount == 0)
                    {
                        response.Result = EmailSendSummaryResponseResult.NotFound;
                        return response;
                    }

                    // var statusItem=data.Select(x=>x.Status)

                    List<InspectionStatus> items = new List<InspectionStatus>();

                    items = await data.Select(x => new { x.StatusId, x.StatusName, x.BookingId, x.StatusPriority })
                               .GroupBy(p => new { p.StatusId, p.StatusName, p.StatusPriority }, p => p, (key, _data) =>
                             new InspectionStatus
                             {
                                 Id = key.StatusId,
                                 StatusName = key.StatusName,
                                 TotalCount = _data.Count(),
                                 Priority = key.StatusPriority,
                                 StatusId = key.StatusId
                             }).OrderBy(x => x.Priority).ToListAsync();

                    // Apply status color
                    items.ForEach(x => x.StatusColor = InspectionStatusColor.GetValueOrDefault(x.StatusId, ""));


                    var _statuslist = items.Select(x => _bookingmap.GetBookingStatusMap(x));

                    var result = await data.OrderByDescending(x => x.ServiceDateFrom).Skip(skip).Take(take).ToListAsync();

                    if (result == null || !result.Any())
                        return new EmailSendSummaryResponse() { Result = EmailSendSummaryResponseResult.NotFound };

                    //get the report ids
                    var reportIds = reportData.Where(y => y.ReportId != null).Select(x => x.ReportId).ToList();

                    //get the log email report email queue
                    var logBookingReportEmailQueues = await _emailSendRepo.GetLogBookingReportEmailQueues(bookingIds);

                    var emailLogIds = logBookingReportEmailQueues.Where(x => reportIds.Contains(x.ReportId.Value))
                        .Select(x => x.EmailLogId).Distinct().ToList();

                    //get the log email queues
                    var logEmailQueues = await _emailSendRepo.GetLogEmailQueues(emailLogIds);

                    //process the sucess report
                    var bookingSucessReportList = logBookingReportEmailQueues.Join(logEmailQueues, brequeue => brequeue.EmailLogId, logQueue => logQueue.EmailLogId,
                                                        (brequeue, logQueue) => new { brequeue, logQueue })
                                                        .GroupBy(x => new { x.brequeue.InspectionId })
                                                        .Select(x => new LogEmailSuccessReportCount
                                                        {
                                                            InspectionId = x.Key.InspectionId,
                                                            ReportCount = x.Select(y => y.brequeue.ReportId).Distinct().Count(),
                                                        }).ToList();


                    var _resultdata = result.Select(x => _emailsendmap.EmailSendSummaryMap(x, serviceTypeList, reportData.ToList(), bookingSucessReportList));


                    return new EmailSendSummaryResponse()
                    {
                        Result = EmailSendSummaryResponseResult.Success,
                        TotalCount = response.TotalCount,
                        Index = request.Index.Value,
                        PageSize = request.pageSize.Value,
                        PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                        Data = _resultdata.ToList(),
                        InspectionStatuslst = _statuslist
                    };
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return new EmailSendSummaryResponse() { Result = EmailSendSummaryResponseResult.NotFound };
        }

        /// <summary>
        /// get booking(container, non-container) and report details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<EmailSendBookingReportResponse> GetBookingReportDetails(BookingReportRequest request)
        {
            if (request == null)
                return new EmailSendBookingReportResponse() { Result = EmailSendResult.RequestNotCorrectFormat };

            var bookingReportData = new BookingReportDetails();

            var response = new EmailSendBookingReportResponse();

            //inspection  starts
            if (request.ServiceId == (int)Service.InspectionId)
            {
                //booking details

                //container details
                bookingReportData.InspectionContainerList = await _emailSendRepo.GetContainerDetails(request.BookingIdList);

                //get the booking po number list
                var bookingPoNumberList = await _inspRepo.GetPoNoListByBookingIds(request.BookingIdList);
                //assign po number list to inspection data
                if (bookingPoNumberList != null && bookingPoNumberList.Any())
                {
                    foreach (var inspectionContainerItem in bookingReportData.InspectionContainerList)
                    {
                        inspectionContainerItem.PoNumberList = bookingPoNumberList.Where(x => x.ContainerRefId == inspectionContainerItem.ContainerRefId).Select(x => x.PoNumber).ToList();
                    }
                }


                //get non container booking id list
                var nonContainerBookingIdList = request.BookingIdList.Except(bookingReportData.InspectionContainerList?.Select(x => x.BookingId).Distinct().ToList());

                if (nonContainerBookingIdList.Any())
                {
                    //product details
                    bookingReportData.InspectionProductList = await _emailSendRepo.GetProductDetails(nonContainerBookingIdList);
                }

                if (!bookingReportData.InspectionContainerList.Any() && !bookingReportData.InspectionProductList.Any())
                    return new EmailSendBookingReportResponse() { Result = EmailSendResult.NotFound };

                //get report ids from container list
                var _containerReportList = bookingReportData.InspectionContainerList?.Where(x => x.ReportId > 0).Select(x => (int)x.ReportId).ToList();

                //get product ids from product list
                var _productReportList = bookingReportData.InspectionProductList?.Where(x => x.ReportId > 0).Select(x => (int)x.ReportId).ToList();

                //if both list has value, union the report ids and get report details
                if (_productReportList != null && _productReportList.Any() && _containerReportList != null && _containerReportList.Any())
                {
                    //take both product and container report id common list
                    var _reportIdList = _productReportList.Union(_containerReportList);

                    //get report details
                    bookingReportData.ReportDetailsList = await _emailSendRepo.GetReportDetails(_reportIdList);
                }
                else if (_productReportList != null && _productReportList.Any())
                {
                    //get report details
                    bookingReportData.ReportDetailsList = await _emailSendRepo.GetReportDetails(_productReportList);
                }
                else if (_containerReportList != null && _containerReportList.Any())
                {
                    //get report details
                    bookingReportData.ReportDetailsList = await _emailSendRepo.GetReportDetails(_containerReportList);
                }

                var customerResultIds = bookingReportData.ReportDetailsList.Select(x=>x.CustomerDecisionResultId).Distinct().ToList();

                //take the customer result data from the resultids
                var customerResultAnalysis = await _dashboardRepo.GetCustomerResultAnalysis(customerResultIds);

                MapCustomerResultAnalysis(customerResultAnalysis, bookingReportData.ReportDetailsList);

                //get booking id list from product list
                var bookingIdList = bookingReportData.InspectionProductList?.Select(x => x.BookingId).Distinct().ToList();

                if (bookingIdList != null && bookingIdList.Any())
                {
                    foreach (var bookingId in bookingIdList)
                    {
                        //get distinct product id list
                        var productIdList = bookingReportData.InspectionProductList.Where(x => bookingId == x.BookingId).Select(x => x.ProductId).Distinct().ToList();

                        foreach (var productId in productIdList)
                        {
                            //product details - map the data
                            response.EmailSendList.Add(_emailsendmap.EmailSendBookingProductReportMap(bookingReportData, productId, bookingId));
                        }
                    }
                }

                //get container details to map
                response.EmailSendList.AddRange(bookingReportData.InspectionContainerList?.Select(x => _emailsendmap.EmailSendBookingContainerReportMap(bookingReportData, x.ContainerId)).ToList());

                var bookingIds = response.EmailSendList.Select(x => x.BookingId).ToList();

                var reportIds = response.EmailSendList.Select(x => x.ReportId).ToList();

                //get the log email report email queue
                var logBookingReportEmailQueues = _emailSendRepo.GetLogBookingReportEmailQueues();

                var logBookingReportEmailData = await logBookingReportEmailQueues.Where(x => bookingIds.Contains(x.InspectionId.Value)
                                                    && reportIds.Contains(x.ReportId.Value)).ToListAsync();

                var emailLogIds = logBookingReportEmailData.Select(x => x.EmailLogId).Distinct().ToList();

                //get the log email queues
                var logEmailQueues = await _emailSendRepo.GetLogEmailQueues(emailLogIds);

                //process the sucess report
                var bookingEmailReportList = logBookingReportEmailData.Join(logEmailQueues, brequeue => brequeue.EmailLogId, logQueue => logQueue.EmailLogId,
                                                    (brequeue, logQueue) => new { brequeue, logQueue })
                                                    .GroupBy(x => new { x.brequeue.InspectionId, x.brequeue.ReportId })
                                                    .Select(x => new LogEmailReportCount
                                                    {
                                                        InspectionId = x.Key.InspectionId,
                                                        ReportId = x.Key.ReportId,
                                                        ReportCount = x.Select(y => y.logQueue.EmailLogId).Count(),
                                                    }).ToList();

                response.EmailSendList = response.EmailSendList.Select(x => _emailsendmap.EmailBookingReportSendMap(x, bookingEmailReportList)).ToList();

                //order by booking then report ids
                response.EmailSendList = response.EmailSendList?.OrderBy(x => x.BookingId).ThenBy(x => x.ReportId).ThenByDescending(x => x.IsParentProduct).ToList();

                //report status color
                foreach (var emailSendItem in response.EmailSendList)
                {
                    if (bookingReportData.ReportDetailsList.Any(x => x.ReportId == emailSendItem.ReportId && x.ReportResult == FBReportResult.Pass.ToString()))
                    {
                        emailSendItem.ReportStatusColor = EmailSendReportResultColor.GetValueOrDefault((int)FBReportResult.Pass, "");
                    }
                    else if (bookingReportData.ReportDetailsList.Any(x => x.ReportId == emailSendItem.ReportId && x.ReportResult == FBReportResult.Fail.ToString()))
                    {
                        emailSendItem.ReportStatusColor = EmailSendReportResultColor.GetValueOrDefault((int)FBReportResult.Fail, "");
                    }
                    else if (bookingReportData.ReportDetailsList.Any(x => x.ReportId == emailSendItem.ReportId && x.ReportResult == FBReportResult.Pending.ToString()))
                    {
                        emailSendItem.ReportStatusColor = EmailSendReportResultColor.GetValueOrDefault((int)FBReportResult.Pending, "");
                    }
                    else
                    {
                        emailSendItem.ReportStatusColor = "";
                    }
                }

                //inspection  end
                response.Result = EmailSendResult.Success;
            }
            else if (request.ServiceId == (int)Service.AuditId)
            {
                //audit - will implement later
                response.Result = EmailSendResult.NotFound;
            }

            return response;

        }

        
        /// <summary>
        /// Get booking invoice details by invoice number list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<EmailSendInvoiceResponse> GetBookingInvoiceDetails(EmailRuleRequestByInvoiceNumbers request)
        {
            if (request == null)
                return new EmailSendInvoiceResponse() { Result = EmailSendResult.RequestNotCorrectFormat };

            var response = new EmailSendInvoiceResponse();
            response.EmailSendList = new List<EmailSendInvoice>();

            if (request.ServiceId == (int)Service.InspectionId)
            {
                var emailInvoiceDetails = await _emailSendRepo.GetEmailSendInvoiceList(request.InvoiceList);

                var distinctInvoices = emailInvoiceDetails.Select(x => x.InvoiceNo).Distinct().ToList();
                foreach (var item in distinctInvoices)
                {
                    var invoiceData = emailInvoiceDetails.FirstOrDefault(x => x.InvoiceNo == item);
                    response.EmailSendList.Add(new EmailSendInvoice()
                    {
                        InvoiceDate = invoiceData.InvoiceDate != null ? invoiceData.InvoiceDate.Value.ToString(StandardDateFormat) : "",
                        InvoiceId = invoiceData.InvoiceId,
                        InvoiceNo = invoiceData.InvoiceNo,
                        InvoiceTotal = Math.Round(emailInvoiceDetails.Where(x => x.InvoiceNo == item).Select(x => x.InvoiceTotal.GetValueOrDefault()).Sum(), 2, MidpointRounding.AwayFromZero),
                        InvoiceType = invoiceData.InvoiceType,
                        BilledName = invoiceData.BilledName,
                        BillTo = invoiceData.BillTo,
                        CurrencyCode = invoiceData.CurrencyCode,
                        InvoiceFileUrl = emailInvoiceDetails.FirstOrDefault(x => x.InvoiceNo == item && x.InvoiceFileUrl != null).InvoiceFileUrl
                    });
                }

                response.Result = EmailSendResult.Success;
            }

            return response;
        }

        /// <summary>
        /// delete the email send details - make inactive
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> Delete(int Id)
        {
            var entity = await _emailSendRepo.GetEmailSendData(Id);

            if (entity == null)
                return new DeleteResponse() { Result = Result.NotFound };

            entity.Active = false;
            entity.DeletedBy = _applicationContext.UserId;
            entity.DeletedOn = DateTime.Now;


            await _emailSendRepo.Save();

            return new DeleteResponse() { Result = Result.Success };
        }

        public async Task<DeleteResponse> DeleteInvoiceFile(int Id)
        {
            var entity = await _emailSendRepo.GetInvoiceSendData(Id);

            if (entity == null)
                return new DeleteResponse() { Result = Result.NotFound };

            entity.Active = false;
            entity.DeletedBy = _applicationContext.UserId;
            entity.DeletedOn = DateTime.Now;

            await _emailSendRepo.Save();
            return new DeleteResponse() { Result = Result.Success };
        }

        /// <summary>
        /// get email send file details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<EmailSendFileListResponse> GetEmailSendFileDetails(BookingReportRequest request)
        {
            var emailSendFileDetails = await _emailSendRepo.GetEmailFileList(request);

            if (emailSendFileDetails != null && emailSendFileDetails.Any())
            {
                var emailSendList = emailSendFileDetails.Select(x => _emailsendmap.EmailSendFileMap(x));

                return new EmailSendFileListResponse() { EmailSendFileList = emailSendList.ToList(), Result = EmailSendResult.Success };

            }
            else
                return new EmailSendFileListResponse() { Result = EmailSendResult.NotFound };
        }

        public async Task<EmailSendFileListResponse> GetInvoiceSendFileDetails(InvoiceSendFilesRequest request)
        {
            var emailSendFileDetails = await _emailSendRepo.GetInvoiceSendFileList(request);

            if (emailSendFileDetails != null && emailSendFileDetails.Any())
            {
                var emailSendList = emailSendFileDetails.Select(x => _emailsendmap.EmailSendFileMap(x));
                return new EmailSendFileListResponse() { EmailSendFileList = emailSendList.ToList(), Result = EmailSendResult.Success };
            }
            else
                return new EmailSendFileListResponse() { Result = EmailSendResult.NotFound };
        }

        /// <summary>
        /// save the file send details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<EmailSendFileUploadResponse> Save(EmailSendFileUpload model)
        {
            if (model.ServiceId == (int)Service.InspectionId)
            {
                if (model.BookingIds != null && model.BookingIds.Any())
                {
                    foreach (var bookingId in model.BookingIds)
                    {
                        if (model.ReportIds != null && model.ReportIds.Any())
                        {
                            foreach (var reportId in model.ReportIds)
                            {
                                var ESTranEntity = AddEntityEmailSend(model, bookingId);

                                ESTranEntity.ReportId = reportId;
                            }
                        }
                        else
                        {
                            AddEntityEmailSend(model, bookingId);
                        }
                    }
                }

                await _emailSendRepo.Save();

                return new EmailSendFileUploadResponse() { Result = EmailSendResult.Success };
            }
            else
            {
                return new EmailSendFileUploadResponse() { Result = EmailSendResult.NotFound };
            }
        }

        /// <summary>
        /// Save Invoice attachments
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<EmailSendFileUploadResponse> SaveInvoiceAttachments(InvoiceSendFileUpload model)
        {
            if (model.ServiceId == (int)Service.InspectionId)
            {
                if (model.InvoiceId > 0)
                {
                    AddEntityInvoiceSend(model);
                    await _emailSendRepo.Save();
                }
                return new EmailSendFileUploadResponse() { Result = EmailSendResult.Success };
            }
            else
            {
                return new EmailSendFileUploadResponse() { Result = EmailSendResult.NotFound };
            }
        }



        /// <summary>
        /// adding table to entity
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public EsTranFile AddEntityEmailSend(EmailSendFileUpload model, int bookingId)
        {
            var EStran = new EsTranFile()
            {
                Active = true,
                InspectionId = bookingId,
                FileTypeId = model.FileTypeId,
                FileName = model.FileName,
                FileLink = model.FileLink,
                CreatedOn = DateTime.Now,
                CreatedBy = _applicationContext.UserId,
                UniqueId = model.UniqueId
            };

            _emailSendRepo.AddEntity(EStran);

            return EStran;
        }

        public InvTranFile AddEntityInvoiceSend(InvoiceSendFileUpload model)
        {
            var invTranFile = new InvTranFile()
            {
                Active = true,
                FileName = model.FileName,
                FilePath = model.FileLink,
                FileType = model.FileTypeId,
                InvoiceId = model.InvoiceId,
                InvoiceNo = model.InvoiceNo,
                CreatedOn = DateTime.Now,
                CreatedBy = _applicationContext.UserId,
                UniqueId = model.UniqueId
            };

            _emailSendRepo.AddEntity(invTranFile);

            return invTranFile;
        }

        /// <summary>
        /// get file type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetFileTypeList()
        {
            var response = new DataSourceResponse();
            response.DataSourceList = await _emailSendRepo.GetFileTypeList();

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.Success };

            response.Result = DataSourceResult.Success;

            return response;
        }

        public async Task<DataSourceResponse> GetInvoiceFileTypeList()
        {
            var response = new DataSourceResponse();
            response.DataSourceList = await _emailSendRepo.GetInvoiceFileTypeList();

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.Success };

            response.Result = DataSourceResult.Success;

            return response;
        }

        /// <summary>
        /// Vaidate if the customer has any email rule and also can send multiple report emails
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<ReportSendTypeResponse> ValidateMultipleEmailSendByCustomer(int customerId, int serviceId)
        {
            var res = await _emailSendRepo.GetEmailDataByCustomer(customerId, serviceId);

            if (res == null || !res.Any())
            {
                return new ReportSendTypeResponse();
            }

            return new ReportSendTypeResponse { RuleFound = true, SendMultipleEmail = true };
        }

        public async Task<IEnumerable<int>> GetBookingNumbersByInvoice(List<string> InvoiceList)
        {
            return await _emailSendRepo.GetBookingNumbersbyInvoiceList(InvoiceList);
        }

        public async Task<IEnumerable<InvoiceBookingEmailSend>> GetBookingDataByInvoiceNoList(List<string> InvoiceList)
        {
            return await _emailSendRepo.GetBookingDataByInvoiceList(InvoiceList);
        }

        public async Task<FbReportRevisionNoResponse> SetFbReportVersion(int apiReportId, int fbReportId, int requestVersion, string fbToken)
        {

            try
            {
                string fbRequest = string.Empty;
                var fbBase = _fbSettings.BaseUrl;
                var objectRequest = new UpdateFbReportRevision()
                {
                    revision = requestVersion,
                };

                fbRequest = string.Format(_fbSettings.ReportUpdateUrl, fbReportId) + "/revision";

                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objectRequest, fbBase, fbToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var fbReport = await _emailSendRepo.GetFbReportInfo(apiReportId);

                    if (fbReport != null)
                    {
                        fbReport.RequestedReportRevision = requestVersion;
                        _emailSendRepo.Save(fbReport);
                        return new FbReportRevisionNoResponse() { ReportId = fbReportId, RevisionId = requestVersion, Result = FbReportRevisionNoRequestResult.Success };
                    }

                    return new FbReportRevisionNoResponse() { ReportId = fbReportId, RevisionId = requestVersion, Result = FbReportRevisionNoRequestResult.NotFound };
                }
                else
                {
                    return new FbReportRevisionNoResponse() { ReportId = fbReportId, RevisionId = requestVersion, Result = FbReportRevisionNoRequestResult.Failed };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckFbReportIsInvalidated(int fbReportId, string fbToken)
        {
            bool isReportStatusInvalidated = false;
            try
            {
                var reportInfo = await GetFullBridgeReportDetails(fbReportId, fbToken);

                if (reportInfo != null && reportInfo.status == "invalidated")
                {
                    isReportStatusInvalidated = true;
                }

                return isReportStatusInvalidated;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<FbReportMainDetail> GetFullBridgeReportDetails(int fbReportId, string fbToken)
        {
            try
            {
                FbReportMainDetail objReport = new FbReportMainDetail();

                var fbBase = _fbSettings.BaseUrl;
                var fbRequest = string.Format(_fbSettings.ReportDeleteRequestUrl, fbReportId);
                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, fbToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var reportData = httpResponse.Content.ReadAsStringAsync();
                    JObject reportDataJson = JObject.Parse(reportData.Result);
                    if (reportDataJson != null && reportDataJson.GetValue("data") != null)
                    {

                        objReport.reportTitle = reportDataJson["data"]["attributes"]["title"].ToString();
                        objReport.status = reportDataJson["data"]["attributes"]["status"].ToString();
                        objReport.statusPreparation = reportDataJson["data"]["attributes"]["statusPreparation"].ToString();
                        objReport.statusFilling = reportDataJson["data"]["attributes"]["statusFilling"].ToString();
                        objReport.statusReview = reportDataJson["data"]["attributes"]["statusReview"].ToString();

                        foreach (var item in reportDataJson.GetValue("included").ToArray())
                        {
                            if (item["type"].ToString() == "Mission")
                            {
                                objReport.missionTitle = item["attributes"]["reference"].ToString();
                                objReport.missionStatus = item["attributes"]["status"].ToString();
                            }
                        }

                    }

                    return objReport;

                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// get email rule list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<EmailRuleResponse> GetEmailRuleData(BookingReportRequest request)
        {
            //get booking details by booking ids
            var bookingList = await _emailSendRepo.GetBookingDetails(request.BookingIdList);
            //get the booking brands
            var bookingBrandsIdList = await _inspRepo.GetBrandBookingIdsByBookingIds(request.BookingIdList);
            //get the booking buyers
            var bookingBuyersIdList = await _inspRepo.GetBuyerBookingIdsByBookingIds(request.BookingIdList);
            //get the booking departments
            var bookingDepartmentIdList = await _inspRepo.GetDeptBookingIdsByBookingIds(request.BookingIdList);
            //get the booking product (productcategory,report result)
            var bookingProductReportResult = await _inspRepo.GetBookingProductAndReportResult(request.BookingIdList);
            //get the booking container report results
            var bookingContainerReportResult = await _inspRepo.GetContainerReportDataByBooking(request.BookingIdList.ToList());
            //get the booking service type list
            var bookingServiceTypeList = await _inspRepo.GetServiceType(request.BookingIdList);

            //get the customer list
            var customerList = bookingList.Select(x => x.CustomerId).ToList();

            var isAnyCustomerPreInvoiceRuleConfigured = false;
            if (request.EmailSendingtype == (int)EmailSendingType.InvoiceStatus && request.InvoiceType == (int)INVInvoiceType.PreInvoice)
            {
                isAnyCustomerPreInvoiceRuleConfigured = await _emailSendRepo.IsAnyCustomerPreInvoiceRuleConfigured(request.EmailSendingtype, customerList, (int)Service.InspectionId, request.InvoiceType);
            }

            //get the email config base data            
            var emailConfigBaseData = await _emailSendRepo.GetEmailConfigurationBaseDetails(request.EmailSendingtype, customerList, (int)Service.InspectionId, request.InvoiceType, isAnyCustomerPreInvoiceRuleConfigured);

            //take the email send details id
            var esDetailsId = emailConfigBaseData.Select(x => x.Id).ToList();

            //get the email send customer config data
            var esCustomerConfigData = await _emailSendRepo.GetESCustomerConfigDetails(esDetailsId);

            //get the email send customer contact data
            var esCustomerContactData = await _emailSendRepo.GetESCustomerContactDetails(esDetailsId);

            //get the email send service type data
            var esServiceTypeData = await _emailSendRepo.GetESServiceTypeDetails(esDetailsId);

            //get the email send factory country config data
            var esFactoryCountryData = await _emailSendRepo.GetESFactoryCountryDetails(esDetailsId);

            //get the email send supplier or factory config data
            var esSupplierOrFactoryData = await _emailSendRepo.GetESSupplierOrFactoryDetails(esDetailsId);

            //get the email send office config data
            var esOfficeDetailsData = await _emailSendRepo.GetESOfficeDetails(esDetailsId);

            //get the email send api contact config data
            var esApiContactData = await _emailSendRepo.GetESApiContactDetails(esDetailsId);

            //get the email send report result config data
            var esReportResultData = await _emailSendRepo.GetESReportResultDetails(esDetailsId);

            //get the email send product category config data
            var esProductCategoryData = await _emailSendRepo.GetESProductCategoryDetails(esDetailsId);

            //get the email send special rule config data
            var esSpecialRuleData = await _emailSendRepo.GetESSpecialRuleDetails(esDetailsId);

            var emailConfigList = new List<EmailSendConfigBooking>();

            foreach (var bookingItem in bookingList)
            {
                //get email configuration
                var emailConfigurationList = GetEmailConfigurationList(bookingItem.CustomerId, emailConfigBaseData, esCustomerConfigData, esCustomerContactData,
                    esServiceTypeData, esFactoryCountryData, esSupplierOrFactoryData, esOfficeDetailsData, esApiContactData, esReportResultData,
                    esProductCategoryData, esSpecialRuleData, request.EmailSendingtype, request.InvoiceType, isAnyCustomerPreInvoiceRuleConfigured);

                //assign the brand list

                bookingItem.BrandIdList = bookingBrandsIdList.Where(x => x.BookingId == bookingItem.BookingId).Select(x => x.BrandId).ToList();

                //assign the buyer list

                bookingItem.BuyerIdList = bookingBuyersIdList.Where(x => x.BookingId == bookingItem.BookingId).Select(x => x.BuyerId).ToList();

                //assign the department list

                bookingItem.DepartmentIdList = bookingDepartmentIdList.Where(x => x.BookingId == bookingItem.BookingId).Select(x => x.DeptId).ToList();

                //assign the service type list

                bookingItem.ServiceTypeIdList = bookingServiceTypeList.Where(x => x.InspectionId == bookingItem.BookingId).Select(x => x.serviceTypeId).ToList();

                //assign the report result list

                bookingItem.ProductCategoryIdList = bookingProductReportResult.Where(x => x.InspectionId == bookingItem.BookingId && x.ProductCategoryId != null).Select(x => x.ProductCategoryId.GetValueOrDefault(0)).ToList();

                bookingItem.NonContainerReportResultIds = bookingProductReportResult.Where(x => x.InspectionId == bookingItem.BookingId && x.ResultId != null).Select(x => x.ResultId.GetValueOrDefault(0)).ToList();

                //assign the container report result list

                bookingItem.ContainerReportResultIds = bookingProductReportResult.Where(x => x.InspectionId == bookingItem.BookingId && x.ResultId != null).Select(x => x.ResultId.GetValueOrDefault(0)).ToList();

                //get email config list by booking details
                emailConfigList.Add(GetEmailConfigurationListByBookingData(bookingItem, emailConfigurationList, 0));
            }
            return GetEmailRuleData(emailConfigList, request.BookingIdList.ToList());
        }

        /// <summary>
        /// Assign and get the email configuration list data for each customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="emailConfigBaseList"></param>
        /// <param name="emailCustomerConfigDetails"></param>
        /// <param name="emailCustomerContactDetails"></param>
        /// <param name="emailServiceTypeDetails"></param>
        /// <param name="emailFactoryCountryDetails"></param>
        /// <param name="esSupplierFactoryDetails"></param>
        /// <param name="esOfficeDetails"></param>
        /// <param name="esApiContacts"></param>
        /// <param name="esReportResultDetails"></param>
        /// <param name="esProductCategoryDetails"></param>
        /// <param name="esSpecialRuleDetails"></param>
        /// <returns></returns>
        public List<EmailSendConfigDetails> GetEmailConfigurationList(int customerId, List<EmailSendConfigBaseDetails> emailConfigData, List<EmailSendCustomerConfigDetails> emailCustomerConfigDetails, List<EmailSendCustomerContactDetails> emailCustomerContactDetails,
                    List<EmailSendServiceTypeDetails> emailServiceTypeDetails, List<EmailSendFactoryCountryDetails> emailFactoryCountryDetails, List<EmailSendSupplierFactoryDetails> esSupplierFactoryDetails,
                    List<EmailSendOfficeDetails> esOfficeDetails, List<ESApiContacts> esApiContacts, List<EmailSendResultDetails> esReportResultDetails, List<ESProductCategoryDetails> esProductCategoryDetails,
                    List<ESSpecialRuleDetails> esSpecialRuleDetails, int emailTypeId, int? invoiceType = null, bool isAnyCustomerPreInvoiceRuleConfigured = false)
        {
            List<EmailSendConfigDetails> emailConfigurationList = new List<EmailSendConfigDetails>();

            if ((emailTypeId == (int)EmailSendingType.InvoiceStatus && invoiceType == (int)INVInvoiceType.PreInvoice && isAnyCustomerPreInvoiceRuleConfigured)
                || (emailTypeId == (int)EmailSendingType.InvoiceStatus && invoiceType == (int)INVInvoiceType.Monthly) || (emailTypeId != (int)EmailSendingType.InvoiceStatus))
            {
                emailConfigData = emailConfigData.Where(x => x.CustomerId == customerId).ToList();
            }



            if (emailConfigData != null && emailConfigData.Any())
            {
                foreach (var emailData in emailConfigData)
                {
                    EmailSendConfigDetails emailSendConfigDetail = new EmailSendConfigDetails();
                    emailSendConfigDetail.Id = emailData.Id;
                    emailSendConfigDetail.CustomerId = emailData.CustomerId;
                    emailSendConfigDetail.ServiceId = emailData.ServiceId;
                    emailSendConfigDetail.Type = emailData.Type;
                    emailSendConfigDetail.ReportInEmail = emailData.ReportInEmail;
                    emailSendConfigDetail.ReportSendType = emailData.ReportSendType;

                    emailSendConfigDetail.BrandIds = emailCustomerConfigDetails.Where(x => x.EsDetailsId == emailData.Id && x.BrandId > 0).Select(x => x.BrandId.GetValueOrDefault()).ToList();
                    emailSendConfigDetail.DepartmentIds = emailCustomerConfigDetails.Where(x => x.EsDetailsId == emailData.Id && x.DepartmentId > 0).Select(x => x.DepartmentId.GetValueOrDefault()).ToList();
                    emailSendConfigDetail.BuyerIds = emailCustomerConfigDetails.Where(x => x.EsDetailsId == emailData.Id && x.BuyerId > 0).Select(x => x.BuyerId.GetValueOrDefault()).ToList();
                    emailSendConfigDetail.CollectionIds = emailCustomerConfigDetails.Where(x => x.EsDetailsId == emailData.Id && x.CollectionId > 0).Select(x => x.CollectionId.GetValueOrDefault()).ToList();

                    emailSendConfigDetail.BrandNameList = emailCustomerConfigDetails.Where(x => x.EsDetailsId == emailData.Id).Select(x => x.BrandName).ToList();
                    emailSendConfigDetail.DepartmentNameList = emailCustomerConfigDetails.Where(x => x.EsDetailsId == emailData.Id).Select(x => x.DepartmentName).ToList();
                    emailSendConfigDetail.BuyerNameList = emailCustomerConfigDetails.Where(x => x.EsDetailsId == emailData.Id).Select(x => x.BuyerName).ToList();
                    emailSendConfigDetail.CollectionNameList = emailCustomerConfigDetails.Where(x => x.EsDetailsId == emailData.Id).Select(x => x.CollectionName).ToList();

                    emailSendConfigDetail.ServiceTypeIds = emailServiceTypeDetails.Where(x => x.ESDetailId == emailData.Id && x.ServiceTypeId > 0).Select(x => x.ServiceTypeId).ToList();
                    emailSendConfigDetail.ServiceTypeNameList = emailServiceTypeDetails.Where(x => x.ESDetailId == emailData.Id).Select(x => x.ServiceTypeName).ToList();
                    emailSendConfigDetail.CustomerContactIds = emailCustomerContactDetails.Where(x => x.ESDetailId == emailData.Id && x.CustomerContactId > 0).Select(x => x.CustomerContactId).ToList();
                    emailSendConfigDetail.FactoryCountryIds = emailFactoryCountryDetails.Where(x => x.ESDetailId == emailData.Id && x.FactoryCountryId > 0).Select(x => x.FactoryCountryId).ToList();
                    emailSendConfigDetail.FactoryCountryNameList = emailFactoryCountryDetails.Where(x => x.ESDetailId == emailData.Id).Select(x => x.FactoryCountryName).ToList();
                    emailSendConfigDetail.SupplierOrFactoryIds = esSupplierFactoryDetails.Where(x => x.ESDetailId == emailData.Id && x.SupplierId > 0).Select(y => y.SupplierId).ToList();
                    emailSendConfigDetail.SupplierNameList = esSupplierFactoryDetails.Where(y => y.SupplierType == (int)Supplier_Type.Supplier_Agent && y.ESDetailId == emailData.Id).Select(y => y.SupplierName).ToList();
                    emailSendConfigDetail.FactoryNameList = esSupplierFactoryDetails.Where(y => y.SupplierType == (int)Supplier_Type.Factory && y.ESDetailId == emailData.Id).Select(y => y.SupplierName).ToList();
                    emailSendConfigDetail.OfficeIds = esOfficeDetails.Where(x => x.ESDetailId == emailData.Id && x.OfficeId > 0).Select(x => x.OfficeId).ToList();
                    emailSendConfigDetail.ApiContactIds = esApiContacts.Where(x => x.EsDetailId == emailData.Id && x.ApiContactId > 0).Select(x => x.ApiContactId).ToList();
                    emailSendConfigDetail.CustomerResultIds = esReportResultDetails.Where(z => z.ESDetailId == emailData.Id && z.CustomerResultId > 0).Select(y => y.CustomerResultId).ToList();
                    emailSendConfigDetail.ApiResultNameList = esReportResultDetails.Where(y => y.ESDetailId == emailData.Id).Select(y => y.ApiResultName).ToList();
                    emailSendConfigDetail.ApiResultIdList = esReportResultDetails.Where(y => y.ESDetailId == emailData.Id && y.ApiResultId > 0).Select(y => y.ApiResultId.GetValueOrDefault()).ToList();
                    emailSendConfigDetail.ProductCategoryIds = esProductCategoryDetails.Where(x => x.ESDetailId == emailData.Id && x.Id > 0).Select(x => x.Id).ToList();
                    emailSendConfigDetail.ProductCategoryNameList = esProductCategoryDetails.Where(x => x.ESDetailId == emailData.Id).Select(x => x.Name).ToList();
                    emailSendConfigDetail.SpecialRuleNameList = esSpecialRuleDetails.Where(x => x.ESDetailId == emailData.Id).Select(x => x.Name).ToList();

                    emailConfigurationList.Add(emailSendConfigDetail);
                }

            }

            return emailConfigurationList;

        }

        /// <summary>
        /// get email rule list
        /// </summary>
        /// <param name="emailConfigList"></param>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public EmailRuleResponse GetEmailRuleData(List<EmailSendConfigBooking> emailConfigList, List<int> bookingIdList)
        {
            var response = new EmailRuleResponse();

            var ruleBookingIdList = new List<RuleBookingIds>();

            foreach (var bookingId in bookingIdList)
            {
                var emailListPerBooking = emailConfigList.FirstOrDefault(x => x.BookingId == bookingId);

                var ruleIdList = emailListPerBooking.EmailSendConfigList?.Select(x => x.Id).ToList();

                ruleBookingIdList.Add(new RuleBookingIds()
                {
                    BookingId = bookingId,
                    RuleIds = ruleIdList
                });

                if (!ruleIdList.Any())
                {
                    response.BookingIdsWithoutRule.Add(bookingId);
                }
                else
                {
                    response.BookingIdsWithRule.Add(bookingId);
                }
            }

            var ruleIdCount = ruleBookingIdList.Where(x => response.BookingIdsWithRule.Contains(x.BookingId)).SelectMany(x => x.RuleIds).Distinct().Count();
            var bookingcount = response.BookingIdsWithRule.Count();

            //all booking has same rule
            if (ruleIdCount == 1 && bookingcount >= 1)
            {
                response.RuleId = ruleBookingIdList.Where(x => response.BookingIdsWithRule.Contains(x.BookingId)).SelectMany(x => x.RuleIds).FirstOrDefault();
                response.Result = EmailSendResult.OneRuleFound;
            }
            //one booking has multiple rule
            else if (ruleIdCount > 1 && bookingcount == 1)
            {
                //get rule list 
                response.EmailRuleList = emailConfigList.Where(x => response.BookingIdsWithRule.Contains(x.BookingId)).SelectMany(x => x.EmailSendConfigList.Select(z => _emailsendmap.EmailSendDetailsMap(z)).Distinct()).ToList();
                response.Result = EmailSendResult.MoreThanOneRuleFound;
            }
            else if (ruleIdCount > 1 && bookingcount > 1) // multiple booking with multiple rules
            {
                response.RuleId = 0;
                response.Result = EmailSendResult.EachBookingHasDifferentRule;
            }
            else // no rule found
            {
                response.Result = EmailSendResult.NoRuleFound;
            }

            return response;
        }

        /// <summary>
        /// get email config list by booking details
        /// </summary>
        /// <param name="bookingList"></param>
        /// <param name="emailConfigurationList"></param>
        /// <returns></returns>
        public EmailSendConfigBooking GetEmailConfigurationListByBookingData(BookingDetails bookingList, List<EmailSendConfigDetails>
            emailConfigurationList, int customerResultId)
        {

            List<int> SupplierOrFactoryList = new List<int>();

            if (bookingList.SupplierId > 0)
            {
                SupplierOrFactoryList.Add(bookingList.SupplierId);
            }
            if (bookingList.FactoryId > 0)
            {
                SupplierOrFactoryList.Add(bookingList.FactoryId.GetValueOrDefault());
            }

            //// get email configuration of api default contacts
            var EmailSendConfigBookingResponse = new EmailSendConfigBooking();

            if (emailConfigurationList.SelectMany(x => x.OfficeIds).Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.OfficeIds.Any()) || x.OfficeIds.Contains(bookingList.OfficeId)).ToList();
            }

            if (emailConfigurationList.SelectMany(x => x.FactoryCountryIds).Any() && bookingList.FactoryCountryId > 0)
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.FactoryCountryIds.Any()) || x.FactoryCountryIds.Contains(bookingList.FactoryCountryId)).ToList();
            }

            if (emailConfigurationList.SelectMany(x => x.ProductCategoryIds).Any() && bookingList.ProductCategoryIdList.Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.ProductCategoryIds.Any()) || x.ProductCategoryIds.Any(bookingList.ProductCategoryIdList.Contains)).ToList();
            }

            if (emailConfigurationList.SelectMany(x => x.BrandIds).Any() && bookingList.BrandIdList.Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.BrandIds.Any()) || x.BrandIds.Any(bookingList.BrandIdList.Contains)).ToList();
            }
            if (emailConfigurationList.SelectMany(x => x.DepartmentIds).Any() && bookingList.DepartmentIdList.Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.DepartmentIds.Any()) || x.DepartmentIds.Any(bookingList.DepartmentIdList.Contains)).ToList();
            }
            if (emailConfigurationList.SelectMany(x => x.BuyerIds).Any() && bookingList.BuyerIdList.Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.BuyerIds.Any()) || x.BuyerIds.Any(bookingList.BuyerIdList.Contains)).ToList();
            }
            if (emailConfigurationList.SelectMany(x => x.CollectionIds).Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.CollectionIds.Any()) || x.CollectionIds.Contains(bookingList.CollectionId.GetValueOrDefault())).ToList();
            }
            if (emailConfigurationList.SelectMany(x => x.SupplierOrFactoryIds).Any() && SupplierOrFactoryList.Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.SupplierOrFactoryIds.Any()) || x.SupplierOrFactoryIds.Any(SupplierOrFactoryList.Contains)).ToList();
            }
            if (emailConfigurationList.SelectMany(x => x.ServiceTypeIds).Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.ServiceTypeIds.Any()) || x.ServiceTypeIds.Any(bookingList.ServiceTypeIdList.Contains)).ToList();
            }

            // Added report result filter for non container 
            if (emailConfigurationList.SelectMany(x => x.ApiResultIdList).Any() && bookingList.NonContainerReportResultIds.Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.ApiResultIdList.Any()) || x.ApiResultIdList.Any(bookingList.NonContainerReportResultIds.Contains)).ToList();
            }

            // Added report result filter for  container 
            if (emailConfigurationList.SelectMany(x => x.ApiResultIdList).Any() && bookingList.ContainerReportResultIds.Any())
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.ApiResultIdList.Any()) || x.ApiResultIdList.Any(bookingList.ContainerReportResultIds.Contains)).ToList();
            }

            // Customer result filter
            if (emailConfigurationList.SelectMany(x => x.CustomerResultIds).Any() && customerResultId > 0)
            {
                emailConfigurationList = emailConfigurationList.Where(x => (!x.CustomerResultIds.Any()) || x.CustomerResultIds.Any(z => z == customerResultId)).ToList();
            }

            //assign booking id 
            EmailSendConfigBookingResponse.BookingId = bookingList.BookingId;
            EmailSendConfigBookingResponse.EmailSendConfigList = emailConfigurationList;

            return EmailSendConfigBookingResponse;
        }


        /// <summary>
        /// get email details by email rule id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<EmailPreviewResponse> FetchEmaildetailsbyEmailRule(EmailPreviewRequest request)
        {

            var response = new EmailPreviewResponse() { Result = EmailPreviewResponseResult.success };
            List<ReportDetailsRepo> reportData = new List<ReportDetailsRepo>();
            EmailRuleDataRepo ruleData = null;

            if (request.EmailRuleId > 0)
            {
                var ruleDataIqueryable = _emailSendRepo.GetEmailRuleData(request.EmailRuleId);

                if (request.EsTypeId > 0)
                {
                    ruleDataIqueryable = ruleDataIqueryable.Where(x => x.EmailSendType.GetValueOrDefault() == request.EsTypeId);
                }

                if (request.EsTypeId == (int)EmailSendingType.InvoiceStatus && request.InvoiceType > 0)
                {
                    ruleDataIqueryable = ruleDataIqueryable.Where(x => x.InvoiceType == request.InvoiceType);
                }
                ruleData = await ruleDataIqueryable.AsNoTracking().FirstOrDefaultAsync();

                if (ruleData != null)
                {

                    // take the email send expense id(rule id)
                    List<int> esDetailId = new List<int> { ruleData.RuleId };

                    //get the office details
                    var esOfficeDetails = await _emailSendRepo.GetESOfficeDetails(esDetailId);

                    if (esOfficeDetails != null && esOfficeDetails.Any())
                    {
                        ruleData.OfficeIdList = esOfficeDetails.Select(x => x.OfficeId).ToList();
                    }
                    //get the report result details
                    var esReportResultDetails = await _emailSendRepo.GetESReportResultDetails(esDetailId);

                    if (esReportResultDetails != null && esReportResultDetails.Any())
                    {
                        ruleData.CustomerDecisionResults = esReportResultDetails.Select(x => x.CustomerResultId).ToList();
                    }
                    //get the api contact details
                    var esApiContacts = await _emailSendRepo.GetESApiContactDetails(esDetailId);

                    if (esApiContacts != null && esApiContacts.Any())
                    {
                        ruleData.ApiContactEmailList = esApiContacts.Select(x => x.CompanyEmail).ToList();
                    }
                    //get the customer contact details
                    var esCustomerContacts = await _emailSendRepo.GetESCustomerContactDetails(esDetailId);

                    if (esCustomerContacts != null && esCustomerContacts.Any())
                    {
                        ruleData.CustomerContactEmailList = esCustomerContacts.Select(x => x.CustomerContactEmail).ToList();
                    }

                    bool isCustomerDecisionEmailRuleConfigured = false;
                    if (ruleData.CustomerId.HasValue)
                    {
                        var isCustomerDecisionCheckpoint = await _customerCheckPointRepository.IsCustomerCheckpointConfigured(ruleData.CustomerId.Value, (int)CheckPointTypeEnum.CustomerDecisionRequired);
                        if (isCustomerDecisionCheckpoint)
                        {
                            isCustomerDecisionEmailRuleConfigured = await _emailConfigurationRepository.IsEmailRuleExistByCustomerIdAndTypeId(ruleData.CustomerId.Value, (int)EmailSendingType.CustomerDecision);
                        }
                    }


                    var reciepientData = await _emailSendRepo.GetEmailRecipientDetails(request.EmailRuleId);

                    var bookingIds = request.EmailReportPreviewData.Select(x => x.BookingId).Distinct().ToList();

                    var bookingData = await _emailSendRepo.GetEmailInspectionDetails(bookingIds);

                    //get the booking department list
                    var bookingDepartments = await _inspRepo.GetDeptBookingIdsByBookingIds(bookingIds);
                    //get the booking brands
                    var bookingBrands = await _inspRepo.GetBrandBookingIdsByBookingIds(bookingIds);
                    //get the booking buyers
                    var bookingBuyers = await _inspRepo.GetBuyerBookingIdsByBookingIds(bookingIds);
                    //get the booking customer contacts
                    var bookingCustomerContacts = await _inspRepo.GetBookingCustomerContacts(bookingIds);
                    //get the booking supplier contacts
                    var bookingSupplierContacts = await _inspRepo.GetSupplierContactsByBookingIds(bookingIds);
                    //get the booking factory contacts
                    var bookingFactoryContacts = await _inspRepo.GetFactoryContactsByBookingIds(bookingIds);
                    //get booking merchandiser contact
                    var bookingMerchandiserContactList = await _inspRepo.GetMerchandiserContactsByBookingIds(bookingIds);

                    var customerDecisionResults = await _kpiRepo.GetCustomerDecisionData(bookingIds);
                    var bookingInvoiceList = await _invoiceRepository.GetInvoiceDetailByBookingIds(bookingIds);
                    var invoiceContacts = await _invoicePreivewRepository.GetInvoiceBilledContacts(bookingInvoiceList.Select(x => x.InvoiceId).ToList());
                    var bookingQuotations = await _quotationRepository.GetQuotationOtherCost(bookingIds);
                    //get purchase order colors by booking ids
                    var inspectionPOColors = await _inspRepo.GetPOColorTransactionsByBookingIds(bookingIds);

                    List<QuotationBookingContactRepo> quotationCustomerContacts = null;
                    List<QuotationBookingContactRepo> quotationSupplierContacts = null;
                    List<QuotationBookingContactRepo> quotationFactoryContacts = null;
                    List<QuotationBookingContactRepo> quotationInternalContacts = null;
                    if (bookingQuotations.Any())
                    {
                        var quotationIds = bookingQuotations.Select(x => x.QuotationId).ToList();
                        quotationCustomerContacts = await _quotationRepository.GetQuotationCustomerContactsById(quotationIds);
                        quotationSupplierContacts = await _quotationRepository.GetQuotationSupplierContactsById(quotationIds);
                        quotationFactoryContacts = await _quotationRepository.GetQuotationFactoryContactsById(quotationIds);
                        quotationInternalContacts = await _quotationRepository.GetQuotationInternalContactsById(quotationIds);
                    }

                    foreach (var booking in bookingData)
                    {
                        if (bookingDepartments != null && bookingDepartments.Any())
                        {
                            booking.Department = bookingDepartments.Where(x => x.BookingId == booking.InspectionID).Select(x => x.DeptName).ToList();
                            booking.DepartmentCode = bookingDepartments.Where(x => x.BookingId == booking.InspectionID).Select(x => x.DeptCode).ToList();
                        }
                        if (bookingBrands != null && bookingBrands.Any())
                        {
                            booking.Brand = bookingBrands.Where(x => x.BookingId == booking.InspectionID).Select(x => x.BrandName).ToList();

                        }
                        if (bookingBuyers != null && bookingBuyers.Any())
                        {
                            booking.Buyer = bookingBuyers.Where(x => x.BookingId == booking.InspectionID).Select(x => x.BuyerName).ToList();
                        }
                        if (bookingCustomerContacts != null && bookingCustomerContacts.Any())
                        {
                            booking.CustomerContact = bookingCustomerContacts.Where(x => x.BookingId == booking.InspectionID).Select(x => x.CustomerContactEmail).ToList();
                        }
                        if (bookingSupplierContacts != null && bookingSupplierContacts.Any())
                        {
                            booking.SupplierContact = bookingSupplierContacts.Where(x => x.InspectionId == booking.InspectionID).Select(x => x.ContactEmail).ToList();
                        }
                        if (bookingFactoryContacts != null && bookingFactoryContacts.Any())
                        {
                            booking.FactoryContact = bookingFactoryContacts.Where(x => x.InspectionId == booking.InspectionID).Select(x => x.ContactEmail).ToList();
                        }
                        if (bookingMerchandiserContactList != null && bookingMerchandiserContactList.Any())
                        {
                            booking.MerchandiserContactEmailList = bookingMerchandiserContactList.Where(x => x.BookingId == booking.InspectionID).Select(x => x.MerchandiserContactEmail).Distinct().ToList();
                        }
                        //set po colors
                        if (inspectionPOColors != null && inspectionPOColors.Any())
                        {
                            var poColors = inspectionPOColors.Where(x => x.BookingId == booking.InspectionID).ToList();
                            //set distinct color code
                            booking.ColorCode = poColors.Select(x => x.ColorCode).Distinct().ToList();
                            //set distinct color name
                            booking.ColorName = poColors.Select(x => x.ColorName).Distinct().ToList();
                        }

                        if (customerDecisionResults != null && customerDecisionResults.Any())
                        {
                            booking.CustomerDecisionResult = string.Join(", ", customerDecisionResults.Where(x => x.BookingId == booking.InspectionID).Select(y => y.CustomerDecisionName).Distinct());
                        }
                        if (bookingInvoiceList != null && bookingInvoiceList.Any())
                        {
                            var bookingInvoiceDetail = bookingInvoiceList.FirstOrDefault(x => x.InspectionId == booking.InspectionID);
                            if (bookingInvoiceDetail != null)
                            {
                                booking.Invoice = bookingInvoiceDetail.InvoiceNo;
                                booking.InvoiceDate = bookingInvoiceDetail.InvoiceDate?.ToString(StandardDateFormat);
                                booking.InvoicePostDate = bookingInvoiceDetail.InvoicePostDate?.ToString(StandardDateFormat);
                                booking.InvoiceContactEmailList = invoiceContacts.Where(x => x.InvoiceId == bookingInvoiceDetail.InvoiceId).Select(y => y.CustomerContactName).ToList();
                                booking.InvoiceBank = new InvoiceSendEmailBankDetails()
                                {
                                    AccountName = bookingInvoiceDetail.BankAccountName,
                                    AccountNumber = bookingInvoiceDetail.BankAccountNumber,
                                    BankAddress = bookingInvoiceDetail.BankAddress,
                                    BankName = bookingInvoiceDetail.BankName,
                                    SwiftCode = bookingInvoiceDetail.BankSwiftCode
                                };
                            }
                        }

                        if (bookingQuotations.Any())
                        {
                            var bookingQuotationIds = bookingQuotations.Where(x => x.BookingId == booking.InspectionID).Select(x => x.QuotationId).ToList();
                            if (quotationCustomerContacts != null && quotationCustomerContacts.Any())
                            {
                                booking.QuotationCustomerContactEmailList = quotationCustomerContacts.Where(x => bookingQuotationIds.Contains(x.QuotationId)).ToList();
                            }
                            if (quotationSupplierContacts != null && quotationSupplierContacts.Any())
                            {
                                booking.QuotationSupplierContactEmailList = quotationSupplierContacts.Where(x => bookingQuotationIds.Contains(x.QuotationId)).ToList();
                            }
                            if (quotationFactoryContacts != null && quotationFactoryContacts.Any())
                            {
                                booking.QuotationFactoryContactEmailList = quotationFactoryContacts.Where(x => bookingQuotationIds.Contains(x.QuotationId)).ToList();
                            }
                            if (quotationInternalContacts != null && quotationInternalContacts.Any())
                            {
                                booking.QuotationInternalContactEmailList = quotationInternalContacts.Where(x => bookingQuotationIds.Contains(x.QuotationId)).Select(x => x.Email).ToList();
                            }
                        }

                    }


                    var apiDefaultContacts = ruleData.OfficeIdList != null ? await _emailSendRepo.GetAPIDefaultContacts(ruleData.OfficeIdList) : null;

                    var toAddressList = reciepientData.Where(x => x.IsTo.GetValueOrDefault()).ToList();

                    var ccAddressList = reciepientData.Where(x => x.IsCc.GetValueOrDefault()).ToList();


                    var additionalEmailAddressList = await _emailConfigurationRepository.GetAdditionalEmailRecipientsByEmailDetailId(new List<int>() { request.EmailRuleId });

                    var additionalToList = additionalEmailAddressList.Where(x => x.RecipientId == (int)RecipientType.To).Select(y => y.Email).Distinct().ToList();
                    var additionalCcList = additionalEmailAddressList.Where(x => x.RecipientId == (int)RecipientType.Cc).Select(y => y.Email).Distinct().ToList();
                    var additionalBccList = additionalEmailAddressList.Where(x => x.RecipientId == (int)RecipientType.Bcc).Select(y => y.Email).Distinct().ToList();
                    response.EmailToList = GetEmailRecipients(toAddressList, ruleData, bookingData, apiDefaultContacts, additionalToList, false);

                    response.EmailCCList = GetEmailRecipients(ccAddressList, ruleData, bookingData, apiDefaultContacts, additionalCcList, true);

                    // for the bcc email address list                    
                    response.EmailBCCList = additionalBccList;

                    var reportIds = request.EmailReportPreviewData.Select(x => x.ReportId).Distinct().ToList();

                    var productData = await _emailSendRepo.GetNonContainerProductDetails(bookingIds);

                    //get the booking po number list
                    var bookingPoNumberList = await _inspRepo.GetPoNoListByBookingIds(bookingIds);

                    foreach (var bookingProduct in productData)
                    {
                        bookingProduct.PoNumberList = bookingPoNumberList.Where(x => x.ProductRefId == bookingProduct.ProductRefId).Select(x => x.PoNumber).ToList();
                    }

                    if (reportIds != null && reportIds.Any() && reportIds.Any(x => x > 0))
                        productData = productData.Where(x => reportIds.Contains(x.ReportId.GetValueOrDefault())).ToList();

                    var containerData = await _emailSendRepo.GetContainerDetails(request.EmailReportPreviewData.Select(x => x.BookingId).Distinct());

                    foreach (var bookingContainer in containerData)
                    {
                        bookingContainer.PoNumberList = bookingPoNumberList.Where(x => x.ContainerRefId == bookingContainer.ContainerRefId).Select(x => x.PoNumber).ToList();
                    }

                    if (reportIds != null && reportIds.Any() && reportIds.Any(x => x > 0))
                        containerData = containerData.Where(x => reportIds.Contains(x.ReportId.GetValueOrDefault())).ToList();

                    EmailInspectionDetail bookingItem = null;
                    var reportHeader = new List<string>();

                    if (request.EsTypeId > 0 && request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                    {
                        var invoiceList = request.EmailReportPreviewData.Select(x => x.InvoiceNo).Distinct().ToList();
                        var invoiceData = await _emailSendRepo.GetEmailSendInvoiceList(invoiceList);

                        foreach (var invoiceNo in invoiceList)
                        {
                            var invoice = invoiceData.FirstOrDefault(x => x.InvoiceNo == invoiceNo && x.InvoiceFileUrl != null);
                            reportData.Add(new ReportDetailsRepo()
                            {
                                ReportLink = invoice?.InvoiceFileUrl,
                                FinalManualReportPath = invoice?.InvoiceFileUrl,
                                ReportSummaryLink = invoice?.InvoiceFileUrl,
                                InvoiceNo = invoiceNo,
                                BookingId = invoice.BookingId,
                                TotalInvoiceFees = Math.Round(invoiceData.Where(x => x.InvoiceNo == invoiceNo).Select(y => y.InvoiceTotal.GetValueOrDefault()).Sum(), 2, MidpointRounding.AwayFromZero),
                                InvoiceCurrencyCode = invoice.CurrencyCode,
                            });
                        }
                    }

                    //productData = productData.Where(x => request.EmailReportPreviewData.Select(y => y.ReportId).Distinct().ToList().Contains(x.ReportId.GetValueOrDefault())).ToList();

                    // product report details
                    if (productData != null && productData.Any() && request.EsTypeId > 0 && request.EsTypeId != (int)EmailSendingType.InvoiceStatus)
                    {
                        reportData = await _emailSendRepo.GetReportDetails(request.EmailReportPreviewData.Select(x => x.ReportId).Distinct());
                    }
                    // container report details
                    else if (containerData != null && containerData.Any() && request.EsTypeId > 0 && request.EsTypeId != (int)EmailSendingType.InvoiceStatus)
                    {
                        reportData = await _emailSendRepo.GetContainerFbReportDetails(request.EmailReportPreviewData.Select(x => x.ReportId).Distinct());
                    }

                    var customerResultIds = reportData.Select(x => x.CustomerDecisionResultId).Distinct().ToList();

                    //take the customer result data from the resultids
                    var customerResultAnalysis = await _dashboardRepo.GetCustomerResultAnalysis(customerResultIds);

                    MapCustomerResultAnalysis(customerResultAnalysis, reportData);

                    if (request.EsTypeId > 0 && request.EsTypeId == (int)EmailSendingType.CustomerDecision && ruleData?.CustomerDecisionResults != null && ruleData.CustomerDecisionResults.Any())
                    {
                        var customerDecisionResultIds = ruleData?.CustomerDecisionResults.
                                                        Select(x => x.GetValueOrDefault()).ToList();
                        if (customerDecisionResultIds.Any())
                        {
                            reportData = reportData.Where(x => customerDecisionResultIds.Contains(x.CustomerDecisionResultId)).ToList();
                        }
                    }

                    SetReportResultColor(reportData);

                    bookingItem = bookingData.FirstOrDefault();

                    // set new line for po numbers 
                    if (productData != null && bookingItem != null)
                    {
                        var inspectionReportList = await _emailSendRepo.GetInspectionSummaryData(bookingItem.InspectionID);
                        var poTransactionsList = await _emailSendRepo.GetPoTransactionbyBooking(bookingItem.InspectionID);
                        reportHeader = inspectionReportList?.OrderBy(x => x.Name).Select(x => x.Name).Distinct().ToList();

                        foreach (var item in productData)
                        {
                            if (item.PoNumberList.Any())
                            {
                                item.PoNumber = string.Join("\n", item.PoNumberList);
                            }

                            //sgt starts
                            item.ServiceDate = bookingItem?.ServiceDate;
                            item.Country = bookingItem?.FactoryCountry;
                            item.ServiceType = bookingItem?.ServiceType;

                            var fbReportList = inspectionReportList?.OrderBy(x => x.Name).Where(x => x.FbReportDetailId == item.ReportId).Distinct();

                            //Report results count for bind dynamic header into table
                            item.ReportResultHeader = fbReportList?.Select(x => x.Name).ToList();
                            item.ReportResultData = fbReportList?.Select(x => x.Result).ToList();


                            var inspPOColorTransactions = poTransactionsList?.SelectMany(x =>
                            x.InspPurchaseOrderColorTransactions?.Where(x => x.ProductRefId == item.ProductRefId)).ToList();

                            item.Color = string.Join(", ", inspPOColorTransactions?
                                .Select(x => x.ColorName).ToList());
                            item.ColorCode = string.Join(", ", inspPOColorTransactions?.Select(x => x.ColorCode).ToList());
                            //sgt end
                        }
                    }

                    // set new line for po numbers with container data
                    if (containerData != null)
                    {
                        foreach (var item in containerData)
                        {
                            if (item.PoNumberList.Any())
                            {
                                item.PoNumber = string.Join("\n", item.PoNumberList);
                            }
                        }
                    }

                    //sgt starts
                    //defect details
                    if (reportData.Any())
                    {
                        var defectList = await _emailSendRepo.GetDefectData(bookingItem.InspectionID);

                        foreach (var item in reportData)
                        {
                            item.MajorDefects = string.Join(";\n", defectList?.Where(x => x.MajorDefect > 0 && x.ReportId == item.ReportId).GroupBy(x => new { x.DefectDesc, x.ReportId }).Select(x => x.Key.DefectDesc));
                            item.MinorDefects = string.Join(";\n", defectList?.Where(x => x.MinorDefect > 0 && x.ReportId == item.ReportId).GroupBy(x => new { x.DefectDesc, x.ReportId }).Select(x => x.Key.DefectDesc));
                            item.CriticalDefects = string.Join(";\n", defectList?.Where(x => x.CriticalDefect > 0 && x.ReportId == item.ReportId).GroupBy(x => new { x.DefectDesc, x.ReportId }).Select(x => x.Key.DefectDesc));

                            item.MajorDefect = defectList?.Where(x => x.ReportId == item.ReportId).Sum(x => x.MajorDefect);
                            item.MinorDefect = defectList?.Where(x => x.ReportId == item.ReportId).Sum(x => x.MinorDefect);
                            item.CriticalDefect = defectList?.Where(x => x.ReportId == item.ReportId).Sum(x => x.CriticalDefect);

                            item.CustomerDecisionFormatDate = item?.CustomerDecisionDate?.ToShortDateString();

                        }
                    }
                    var CustomerDecisionLatestDate = reportData?.Where(x => x.CustomerDecisionDate.HasValue).OrderByDescending(x => x.CustomerDecisionDate)
                      .Select(x => x.CustomerDecisionDate.Value.ToString(StandardDateFormat)).FirstOrDefault();

                    var CustomerDecisionLatestTime = reportData?.Where(x => x.CustomerDecisionDate.HasValue).Select(x => x.CustomerDecisionDate.Value.ToShortTimeString()).FirstOrDefault();

                    //sgt end

                    response.EmailBodyTempList = await GetEmailBodyList(ruleData, productData, reportData, containerData, bookingData, request);


                    //sgt starts
                    foreach (var item in response.EmailBodyTempList)
                    {
                        item.CustomerId = bookingItem?.CustomerId ?? 0;
                        item.Customer = bookingItem?.Customer;
                        item.Supplier = bookingItem?.Supplier;
                        item.Factory = bookingItem?.Factory;
                        if (request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                        {
                            var booking = bookingData.FirstOrDefault(x => x.Invoice == item.InvoiceNumber);
                            item.InvoiceBank = booking?.InvoiceBank;
                            if (bookingItem?.QuotationInternalContactEmailList != null)
                            {
                                item.QuotationInternalContact = string.Join("; ", booking?.QuotationInternalContactEmailList.Select(x => x));
                            }
                        }

                        item.UpdatedDate = CustomerDecisionLatestDate;
                        item.UpdatedTime = CustomerDecisionLatestTime;
                        //1 is static column
                        item.ColumnCount = reportHeader?.Count + 1 ?? 0;
                        item.ReportHeader = reportHeader;
                        if (isCustomerDecisionEmailRuleConfigured && isCustomerDecisionEmailRuleConfigured)
                        {

                            if (bookingIds.Any())
                            {
                                var masterConfigs = await _inspManager.GetMasterConfiguration();
                                var entityName = masterConfigs.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.Entity && x.Active == true)?.Value;
                                var baseUrl = _configuration["BaseUrl"];
                                if (bookingIds.Count > 1)
                                {
                                    var customerDecisionUrl = _configuration["UrlCustomerDecisionSummary"];
                                    var url = string.Concat(baseUrl, string.Format(customerDecisionUrl, entityName, WebUtility.UrlEncode("{" + $"\"bookingIds\": [{string.Join(",", bookingIds)}]" + "}")));
                                    item.CustomerDecisionUrl = url;
                                }
                                else
                                {
                                    var customerDecisionUrl = _configuration["UrlCustomerDecision"];
                                    item.CustomerDecisionUrl = string.Concat(baseUrl, string.Format(customerDecisionUrl, entityName, bookingIds.FirstOrDefault()));
                                }

                            }

                        }
                    }
                    //sgt end

                    // set report summary link is not available if the email body is empty
                    if (response.EmailBodyTempList.Count == 0 &&
                        (ruleData.ReportSendType.GetValueOrDefault() == (int)Email_Report_Send_Type.OneEmailWithReportSummary ||
                        ruleData.ReportSendType.GetValueOrDefault() == (int)Email_Report_Send_Type.OneEmailWithReportSummaryAndAllReports))
                    {
                        response.Result = EmailPreviewResponseResult.inspectionsummarylinknotavailable;
                    }
                }
                else
                {
                    response.Result = EmailPreviewResponseResult.emailrulenotvalid;
                }
            }
            else
            {
                response.Result = EmailPreviewResponseResult.emailrulenotvalid;
            }
            return response;
        }


        public async Task<int> AddOrUpdatedEmailInvoiceAttachment(InvoiceEmailAttachment invoiceEmailAttachmentRequest)
        {
            int successData = 0;
            if (invoiceEmailAttachmentRequest != null && invoiceEmailAttachmentRequest.InvoiceId > 0)
            {

                // delete invoice email attachment

                var invoiceAttachments = await _emailSendRepo.GetInvoiceEmailAttachment(invoiceEmailAttachmentRequest.InvoiceId.GetValueOrDefault());

                if (invoiceAttachments.Any())
                {
                    foreach (var item in invoiceAttachments)
                    {
                        item.Active = false;
                        item.DeletedBy = invoiceEmailAttachmentRequest.UserId;
                    }
                    _emailSendRepo.EditEntities(invoiceAttachments);
                }


                // Add invoice email attchment

                var emailInvoiceAttachmentFile = new InvTranFile()
                {
                    InvoiceId = invoiceEmailAttachmentRequest.InvoiceId,
                    UniqueId = invoiceEmailAttachmentRequest.UniqueId,
                    FileName = invoiceEmailAttachmentRequest.FileName,
                    FilePath = invoiceEmailAttachmentRequest.FilePath,
                    FileType = invoiceEmailAttachmentRequest.FileType,
                    Active = true,
                    CreatedBy = invoiceEmailAttachmentRequest.UserId,
                    CreatedOn = DateTime.Now
                };

                successData = await _emailSendRepo.AddInvoiceEmailAttachment(emailInvoiceAttachmentFile);
            }

            return successData;
        }


        private void SetReportResultColor(List<ReportDetailsRepo> reportData)
        {
            // set report Result color
            foreach (var item in reportData)
            {
                item.ServiceFrom = item.ServiceDateFrom.ToString(StandardDateFormat);
                if (item.ReportResult?.ToLower() == FBReportResult.Pass.ToString().ToLower())
                {
                    item.ReportResultColor = EmailSendReportResultColor.GetValueOrDefault((int)FBReportResult.Pass, "");
                }
                else if (item.ReportResult?.ToLower() == FBReportResult.Fail.ToString().ToLower())
                {
                    item.ReportResultColor = EmailSendReportResultColor.GetValueOrDefault((int)FBReportResult.Fail, "");
                }
                else if (item.ReportResult?.ToLower() == FBReportResult.Pending.ToString().ToLower())
                {
                    item.ReportResultColor = EmailSendReportResultColor.GetValueOrDefault((int)FBReportResult.Pending, "");
                }
                else if (item.ReportResult?.ToLower() == FBReportResult.Missing.ToString().ToLower())
                {
                    item.ReportResultColor = EmailSendReportResultColor.GetValueOrDefault((int)FBReportResult.Missing, "");
                }
                else
                {
                    item.ReportResultColor = "";
                }

                //customer decision result font color
                if (item.CustomerDecisionResultId == (int)InspCustomerDecisionEnum.Pass)
                {
                    item.CustomerDecisionResultColor = CustomerDecisionResultColor.GetValueOrDefault((int)InspCustomerDecisionEnum.Pass, "");
                }
                else if (item.CustomerDecisionResultId == (int)InspCustomerDecisionEnum.Fail)
                {
                    item.CustomerDecisionResultColor = CustomerDecisionResultColor.GetValueOrDefault((int)InspCustomerDecisionEnum.Fail, "");
                }
                else if (item.CustomerDecisionResultId == (int)InspCustomerDecisionEnum.Pending)
                {
                    item.CustomerDecisionResultColor = CustomerDecisionResultColor.GetValueOrDefault((int)InspCustomerDecisionEnum.Pending, "");
                }
                else if (item.CustomerDecisionResultId == (int)InspCustomerDecisionEnum.Derogated)
                {
                    item.CustomerDecisionResultColor = CustomerDecisionResultColor.GetValueOrDefault((int)InspCustomerDecisionEnum.Derogated, "");
                }
                else
                {
                    item.CustomerDecisionResultColor = "";
                }

                //set Report Link
                if (!string.IsNullOrEmpty(item.FinalManualReportPath))
                    item.ReportLink = item.FinalManualReportPath;
            }
        }

        /// <summary>
        /// Update all the booking status from report send option 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<bool> UpdateBookingStatusbyReportSent(List<int> bookingIds, int entityId, int createdBy)
        {

            var emailBookingReports = await _emailSendRepo.GetEmailBookingReportMaps(bookingIds);

            var bookingContainerReports = await _emailSendRepo.GetContainerBookingReportMaps(bookingIds, entityId);

            var bookingNonContainerReports = await _emailSendRepo.GetNonContainerBookingReportMaps(bookingIds, entityId);

            var bookingReports = bookingContainerReports.Concat(bookingNonContainerReports)
                                    .ToList();

            foreach (var bookingId in bookingIds)
            {
                var emailBookingReportList = emailBookingReports.Where(x => x.InspectionId == bookingId).
                                                 Select(x => x.ReportId).Distinct().ToList();

                var bookingReportList = bookingReports.Where(x => x.InspectionId == bookingId)
                                              .Select(x => x.ReportId).Distinct().ToList();

                // if we send email for all the reports then update the status
                if (bookingReportList.All(elem => emailBookingReportList.Contains(elem)))
                {
                    var bookingResult = await _inspManager.UpdateBookingStatus(bookingId, (int)BookingStatus.ReportSent);

                    if (bookingResult.Result == BookingStatusUpdateResponseResult.success)
                    {
                        InspTranStatusLog inspStatusLog = new()
                        {
                            CreatedBy = createdBy,
                            CreatedOn = DateTime.Now,
                            BookingId = bookingId,
                            StatusId = (int)BookingStatus.ReportSent,
                            StatusChangeDate = DateTime.Now,
                            EntityId = entityId
                        };

                        _emailSendRepo.AddEntity(inspStatusLog);
                        await _emailSendRepo.Save();
                    }
                }
            }

            return true;
        }

        public async Task<bool> UpdateInvoiceStatusbyInvoiceSend(List<int> bookingIds, int entityId, int createdBy)
        {
            var invoiceList = await _invoiceRepository.GetInvoiceListByBookingIds(bookingIds);

            if (invoiceList.Any())
            {
                foreach (var invoiceDetails in invoiceList)
                {
                    invoiceDetails.UpdatedBy = createdBy;
                    invoiceDetails.UpdatedOn = DateTime.Now;
                    invoiceDetails.InvoiceStatus = (int)InvoiceStatus.Sent;
                }
            }
            _emailSendRepo.EditEntities(invoiceList);
            await _emailSendRepo.Save();
            return true;
        }

        /// <summary>
        /// Get email body template list 
        /// </summary>
        /// <param name="ruleData"></param>
        /// <param name="productData"></param>
        /// <param name="reportData"></param>
        /// <param name="containerData"></param>
        /// <returns></returns>
        private async Task<List<EmailBody>> GetEmailBodyList(EmailRuleDataRepo ruleData, List<InspectionProductRepo> productData, List<ReportDetailsRepo> reportData, List<InspectionContainerRepo> containerData, List<EmailInspectionDetail> bookingData, EmailPreviewRequest request)
        {
            var emailSizeBodyList = new List<EmailBody>();
            var finalEmailBodyList = new List<EmailBody>();

            var emailBodyList = GetEmailBodyListByNoOfReports(ruleData, productData, reportData, containerData);

            if (emailBodyList.Any() && ruleData.ReportInEmail.GetValueOrDefault() == (int)ReportInEmail.Link_Attachment)
            {
                emailSizeBodyList = GetEmailBodyListbyEmailSize(emailBodyList, ruleData, productData, containerData, request);
            }

            if (emailSizeBodyList.Any())
            {
                finalEmailBodyList = await GetFinalEmailBodyList(emailSizeBodyList, ruleData, bookingData, productData, reportData, request);
            }
            else if (emailBodyList.Any())
            {
                finalEmailBodyList = await GetFinalEmailBodyList(emailBodyList, ruleData, bookingData, productData, reportData, request);
            }
            return finalEmailBodyList;
        }

        /// <summary>
        /// get Final Email body List
        /// </summary>
        /// <param name="emailBodyList"></param>
        /// <param name="ruleData"></param>
        /// <returns></returns>
        private async Task<List<EmailBody>> GetFinalEmailBodyList(List<EmailBody> emailBodyList, EmailRuleDataRepo ruleData, List<EmailInspectionDetail> bookingData, List<InspectionProductRepo> productData, List<ReportDetailsRepo> reportData, EmailPreviewRequest request)
        {
            var finalEmailBodyList = new List<EmailBody>();
            List<string> reportIds;
            foreach (var emailBodyGroup in emailBodyList.GroupBy(x => x.EmailId))
            {
                var emailBody = new EmailBody();
                emailBody.RecipientName = (!string.IsNullOrEmpty(ruleData.RecipientName)) ? ruleData.RecipientName : "All";
                emailBody.EmailValidOption = emailBodyGroup.FirstOrDefault().EmailValidOption;
                emailBody.EmailCount = emailBodyGroup.FirstOrDefault().EmailCount;
                foreach (var item in emailBodyGroup)
                {
                    emailBody.ReportList.AddRange(item.ReportList);
                    emailBody.ContainerList.AddRange(item.ContainerList);
                    emailBody.ProductList.AddRange(item.ProductList);
                    emailBody.InvoiceNumber = item.InvoiceNumber;
                }

                if (request.EsTypeId > 0 && request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                {
                    reportIds = emailBody.ReportList.Select(x => x.InvoiceNo).Distinct().ToList();
                }
                else
                {
                    reportIds = emailBody.ReportList.Select(x => x.ReportId.ToString()).Distinct().ToList();
                }

                emailBody.EmailSubject = await GetEmailSubjectOrFileName(ruleData, bookingData, productData, reportData, reportIds, EmailSendSubjectModule.Subject, request);


                if (!string.IsNullOrEmpty(emailBody.EmailCount) && (ruleData.NoOfReports.GetValueOrDefault() > 0 || ruleData.ReportInEmail.GetValueOrDefault() == (int)ReportInEmail.Link_Attachment))
                {
                    emailBody.EmailSubject = emailBody.EmailSubject + " " + emailBody.EmailCount;
                }

                emailBody.ReportSummaryLinkList = new List<ReportSummaryLink>();
                // if report summary is available
                if (ruleData.ReportSendType.GetValueOrDefault() == (int)Email_Report_Send_Type.OneEmailWithReportSummary ||
                            ruleData.ReportSendType.GetValueOrDefault() == (int)Email_Report_Send_Type.OneEmailWithReportSummaryAndAllReports)
                {
                    foreach (var summaryItem in emailBody.ReportList.GroupBy(x => x.ReportSummaryLink).ToList())
                    {
                        foreach (var summary in summaryItem)
                        {
                            emailBody.ReportSummaryLinkList.Add(
                               new ReportSummaryLink()
                               { BookingId = summary.BookingId, SummaryLink = summary.ReportSummaryLink });
                        }
                    }
                }

                emailBody.AttachmentList = new List<EmailAttachments>();
                string emailFileName = string.Empty;

                List<CommonReportLinkData> reportLinks = null;
                if (request.EsTypeId > 0 && request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                {
                    reportLinks = emailBody.ReportList.Select(x => new CommonReportLinkData { Id = x.InvoiceNo, ReportLink = x.ReportLink }).Distinct().ToList();
                }
                else
                {
                    reportLinks = emailBody.ReportList.Select(x => new CommonReportLinkData { Id = x.ReportId.ToString(), ReportLink = x.ReportLink }).Distinct().ToList();
                }

                foreach (var link in reportLinks)
                {
                    if (!string.IsNullOrWhiteSpace(link.ReportLink))
                    {
                        List<string> selectedReport = new List<string>() { link.Id };

                        emailFileName = await GetEmailSubjectOrFileName(ruleData, bookingData, productData, reportData, selectedReport, EmailSendSubjectModule.FileName, request);

                        string filVersion = string.Empty;

                        if (request.EsTypeId > 0 && request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                        {
                            emailFileName = emailFileName + ruleData.FileDelimeterName;
                        }
                        else
                        {
                            // Add report file version
                            var extraFileName = request.EmailReportPreviewData.Where(x => x.ReportId.ToString() == link.Id).Select(x => x.ExtraFileName).FirstOrDefault();
                            if (!string.IsNullOrEmpty(emailFileName) && !string.IsNullOrEmpty(extraFileName))
                            {
                                emailFileName = emailFileName + ruleData.FileDelimeterName + extraFileName;
                            }
                        }

                        if (emailFileName.Length > EmailFileNameMaxChar)
                        {
                            emailFileName = emailFileName.Substring(0, EmailFileNameMaxChar) + "...";
                        }
                        // Remove invalid character from the filename and apply -
                        emailFileName = Regex.Replace(emailFileName.Trim(), "[^A-Za-z0-9-_ ]+", "-");

                        if (!string.IsNullOrWhiteSpace(emailFileName))
                        {
                            if (request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                            {
                                link.ReportLink = GetInvoiceUrl(link.Id, emailFileName);
                            }
                            else
                            {
                                link.ReportLink = string.Concat(link.ReportLink, "/" + emailFileName);
                            }
                        }
                        else
                        {
                            if (request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                            {
                                link.ReportLink = GetInvoiceUrl(link.Id, null);
                            }
                        }

                        //map in the existing report list, bcz here is looping 
                        emailBody.ReportList.Where(x => request.EsTypeId == (int)EmailSendingType.InvoiceStatus ? x.InvoiceNo == link.Id : x.ReportId.ToString() == link.Id).ToList()
                            .ForEach(x =>
                            {
                                x.ReportLink = link.ReportLink;
                            });

                        if (ruleData.ReportInEmail.GetValueOrDefault() == (int)ReportInEmail.Link_Attachment && ruleData.ReportSendType.GetValueOrDefault() != (int)Email_Report_Send_Type.OneEmailWithReportSummary)
                        {
                            string mimeType = GetFileType(link.ReportLink);
                            var fileExtension = _fileManager.GetExtension(mimeType);

                            emailBody.AttachmentList.Add(
                            new EmailAttachments()
                            { FileLink = link.ReportLink, FileName = emailFileName + "." + fileExtension, FileType = mimeType });
                        }
                    }

                }


                if (emailBody.ReportSummaryLinkList.Any())
                {
                    if (ruleData.ReportInEmail.GetValueOrDefault() == (int)ReportInEmail.Link_Attachment &&
                   (ruleData.ReportSendType.GetValueOrDefault() == (int)Email_Report_Send_Type.OneEmailWithReportSummary ||
                    ruleData.ReportSendType.GetValueOrDefault() == (int)Email_Report_Send_Type.OneEmailWithReportSummaryAndAllReports))
                    {

                        var reportSummaryLinks = emailBody.ReportSummaryLinkList.Select(x => new { x.SummaryLink, x.BookingId }).Distinct().ToList();

                        foreach (var link in reportSummaryLinks)
                        {
                            if (!string.IsNullOrWhiteSpace(link.SummaryLink))
                            {

                                if (ruleData.ReportSendType.GetValueOrDefault() == (int)Email_Report_Send_Type.OneEmailWithReportSummaryAndAllReports)
                                {
                                    emailFileName = "#" + link.BookingId + "_Inspection Summary Report";
                                }

                                string mimeType = GetFileType(link.SummaryLink);

                                var fileExtension = _fileManager.GetExtension(mimeType);

                                if (emailFileName.Length > EmailFileNameMaxChar)
                                {
                                    emailFileName = emailFileName.Substring(0, EmailFileNameMaxChar) + "...";
                                }

                                emailBody.AttachmentList.Add(
                                    new EmailAttachments()
                                    { FileLink = link.SummaryLink, FileName = emailFileName + "." + fileExtension, FileType = mimeType });
                            }

                        }
                    }
                }


                // if report is available map the report
                if (reportIds.Count > 0)
                {
                    emailBody.ReportBookingList = new List<ReportBooking>();
                    foreach (var reportId in reportIds)
                    {
                        int bookingId = 0;
                        int inspectionReportId = 0;
                        string reportName = string.Empty;
                        int? reportVersion = null;
                        int? reportRevisison = null;

                        if (request.EsTypeId > 0 && request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                        {
                            bookingId = emailBody.ReportList.FirstOrDefault(x => x.InvoiceNo == reportId).BookingId;
                            reportName = emailBody.ReportList.Select(x => x.InvoiceNo).FirstOrDefault();
                            reportVersion = emailBody.ReportList.Select(x => x.ReportVersion).FirstOrDefault();
                            reportRevisison = emailBody.ReportList.Select(x => x.ReportRevision).FirstOrDefault();
                        }
                        else
                        {
                            bookingId = emailBody.ReportList.FirstOrDefault(x => x.ReportId.ToString() == reportId).BookingId;
                            inspectionReportId = Int32.Parse(reportId);
                            reportName = emailBody.ReportList.Select(x => x.ReportName).FirstOrDefault();
                            reportVersion = emailBody.ReportList.Select(x => x.ReportVersion).FirstOrDefault();
                            reportRevisison = emailBody.ReportList.Select(x => x.ReportRevision).FirstOrDefault();
                        }


                        emailBody.ReportBookingList.Add(new ReportBooking()
                        {
                            AuditId = 0,
                            InspectionId = bookingId,
                            ReportId = inspectionReportId,
                            ReportName = reportName,
                            ReportVersion = reportVersion,
                            ReportRevision = reportRevisison,
                            EsTypeId = request.EsTypeId
                        });
                    }
                }

                emailBody.EmailRuleData = ruleData;
                finalEmailBodyList.Add(emailBody);
            }

            return finalEmailBodyList;
        }

        /// <summary>
        /// get email body list by number of reports
        /// </summary>
        /// <param name="ruleData"></param>
        /// <param name="productData"></param>
        /// <param name="reportData"></param>
        /// <param name="containerData"></param>
        /// <returns></returns>
        private List<EmailBody> GetEmailBodyListByNoOfReports(EmailRuleDataRepo ruleData, List<InspectionProductRepo> productData, List<ReportDetailsRepo> reportData, List<InspectionContainerRepo> containerData)
        {
            var emailBodyList = new List<EmailBody>();
            var reportSendEmailBodyList = new List<EmailBody>();
            EmailBody emailBody = null;

            switch (ruleData.ReportSendType.GetValueOrDefault())
            {
                case (int)Email_Report_Send_Type.OneEmailWithAllReports:

                    foreach (var report in reportData)
                    {
                        emailBody = new EmailBody();
                        emailBody.EmailId = 1;
                        emailBody.ReportList = reportData.Where(x => x.ReportId == report.ReportId).ToList();
                        emailBody.ContainerList = containerData.Where(x => x.ReportId == report.ReportId).ToList();
                        emailBody.ProductList = productData.Where(x => x.ReportId == report.ReportId).ToList();
                        reportSendEmailBodyList.Add(emailBody);
                    }
                    break;
                case (int)Email_Report_Send_Type.OneEmailOneReport:

                    int index = 1;
                    foreach (var report in reportData)
                    {
                        emailBody = new EmailBody();
                        emailBody.EmailId = index;
                        emailBody.ReportList = reportData.Where(x => x.ReportId == report.ReportId).ToList();
                        emailBody.ContainerList = containerData.Where(x => x.ReportId == report.ReportId).ToList();
                        emailBody.ProductList = productData.Where(x => x.ReportId == report.ReportId).ToList();
                        reportSendEmailBodyList.Add(emailBody);
                        index++;
                    }

                    break;
                case (int)Email_Report_Send_Type.OneEmailWithSameReportResult:

                    int j = 1;
                    foreach (var report in reportData.Where(s => !string.IsNullOrWhiteSpace(s.ReportResult)).GroupBy(x => x.ReportResult).ToList())
                    {
                        foreach (var item in report)
                        {
                            emailBody = new EmailBody();
                            emailBody.EmailId = j;
                            emailBody.ReportList = reportData.Where(x => x.ReportId == item.ReportId).ToList();
                            emailBody.ContainerList = containerData.Where(x => x.ReportId == item.ReportId).ToList();
                            emailBody.ProductList = productData.Where(x => x.ReportId == item.ReportId).ToList();
                            reportSendEmailBodyList.Add(emailBody);
                        }
                        j++;
                    }

                    break;
                case (int)Email_Report_Send_Type.OneEmailWithReportSummary:
                    int k = 1;
                    foreach (var report in reportData.Where(s => !string.IsNullOrWhiteSpace(s.ReportSummaryLink)).GroupBy(x => x.ReportSummaryLink))
                    {
                        foreach (var item in report)
                        {
                            emailBody = new EmailBody();
                            emailBody.EmailId = k;
                            emailBody.ReportList = reportData.Where(x => x.ReportId == item.ReportId).ToList();
                            emailBody.ContainerList = containerData.Where(x => x.ReportId == item.ReportId).ToList();
                            emailBody.ProductList = productData.Where(x => x.ReportId == item.ReportId).ToList();
                            reportSendEmailBodyList.Add(emailBody);
                        }
                        k++;
                    }
                    break;
                case (int)Email_Report_Send_Type.OneEmailWithReportSummaryAndAllReports:
                    int z = 1;
                    foreach (var report in reportData.Where(s => !string.IsNullOrWhiteSpace(s.ReportSummaryLink)).GroupBy(x => x.ReportSummaryLink))
                    {
                        foreach (var item in report)
                        {
                            emailBody = new EmailBody();
                            emailBody.EmailId = z;
                            emailBody.ReportList = reportData.Where(x => x.ReportId == item.ReportId).ToList();
                            emailBody.ContainerList = containerData.Where(x => x.ReportId == item.ReportId).ToList();
                            emailBody.ProductList = productData.Where(x => x.ReportId == item.ReportId).ToList();
                            reportSendEmailBodyList.Add(emailBody);
                        }
                        z++;
                    }
                    break;

                case (int)Email_Report_Send_Type.OneEmailWithAllInvoice:

                    foreach (var report in reportData)
                    {
                        emailBody = new EmailBody();
                        emailBody.EmailId = 1;
                        emailBody.ReportList = reportData.Where(x => x.InvoiceNo == report.InvoiceNo).ToList();
                        emailBody.ContainerList = new List<InspectionContainerRepo>();
                        emailBody.ProductList = new List<InspectionProductRepo>();
                        reportSendEmailBodyList.Add(emailBody);
                    }
                    break;

                case (int)Email_Report_Send_Type.OneInvoicePerEmail:
                    int y = 1;
                    foreach (var report in reportData)
                    {
                        emailBody = new EmailBody();
                        emailBody.EmailId = y;
                        emailBody.ReportList = reportData.Where(x => x.InvoiceNo == report.InvoiceNo).ToList();
                        emailBody.InvoiceNumber = report.InvoiceNo;
                        emailBody.ContainerList = new List<InspectionContainerRepo>();
                        emailBody.ProductList = new List<InspectionProductRepo>();
                        reportSendEmailBodyList.Add(emailBody);
                        y++;
                    }
                    break;
            }

            if (ruleData.NoOfReports.GetValueOrDefault() > 0 && reportData.Count >= ruleData.NoOfReports.GetValueOrDefault())
            {
                int emailId = 1;

                foreach (var emailGroup in reportSendEmailBodyList.GroupBy(X => X.EmailId).ToList())
                {

                    var emailbyNoOfItems = emailGroup.Select((item, index) => new { item, index })
                              .GroupBy(pair => pair.index / ruleData.NoOfReports.GetValueOrDefault(), pair => pair.item)
                              .ToList();

                    foreach (var reports in emailbyNoOfItems)
                    {
                        foreach (var item in reports)
                        {
                            var reportIds = item.ReportList.Select(x => x.ReportId).Distinct().ToList();

                            emailBody = new EmailBody();
                            emailBody.EmailId = emailId;
                            emailBody.EmailCount = (emailbyNoOfItems.Count > 1 && ruleData.NoOfReports.GetValueOrDefault() > 1) ? emailId + "/" + emailbyNoOfItems.Count : string.Empty;
                            emailBody.ReportList = item.ReportList;
                            emailBody.ContainerList = containerData.Where(x => reportIds.Contains(x.ReportId.GetValueOrDefault())).ToList();
                            emailBody.ProductList = productData.Where(x => reportIds.Contains(x.ReportId.GetValueOrDefault())).ToList();
                            emailBodyList.Add(emailBody);
                        }

                        emailId++;
                    }
                }
            }
            else
            {
                emailBodyList = reportSendEmailBodyList;
            }

            return emailBodyList;
        }

        /// <summary>
        /// Get email body list by email size
        /// </summary>
        /// <param name="emailBodyList"></param>
        /// <param name="ruleData"></param>
        /// <param name="productData"></param>
        /// <param name="reportData"></param>
        /// <param name="containerData"></param>
        /// <returns></returns>
        private List<EmailBody> GetEmailBodyListbyEmailSize(List<EmailBody> emailBodyList, EmailRuleDataRepo ruleData,
            List<InspectionProductRepo> productData, List<InspectionContainerRepo> containerData, EmailPreviewRequest request)
        {
            var emailSizeBodyList = new List<EmailBody>();
            if (ruleData.ReportInEmail.GetValueOrDefault() == (int)ReportInEmail.Link_Attachment && ruleData.EmailSize > 0)
            {
                var emailSize = ruleData.EmailSize * 1000000; // in mb       

                int index = 1;
                long totalFileSize = 0;

                foreach (var groupEmail in emailBodyList.GroupBy(x => x.EmailId))
                {
                    var reportWithinEmailSizeLimit = new List<string>();
                    EmailBody emailBody = new EmailBody();
                    emailBody.EmailId = index;

                    foreach (var item in groupEmail)
                    {
                        if (item.ReportList.Count > 0)
                        {

                            // Add only one time if we have report summary link
                            if (ruleData.ReportSendType.GetValueOrDefault() == (int)Email_Report_Send_Type.OneEmailWithReportSummary ||
                                ruleData.ReportSendType.GetValueOrDefault() == (int)Email_Report_Send_Type.OneEmailWithReportSummaryAndAllReports)
                            {
                                totalFileSize += GetFileSize(item.ReportList.FirstOrDefault().ReportSummaryLink);
                            }

                            foreach (var report in item.ReportList)
                            {
                                var fileSize = GetFileSize(report.ReportLink);

                                totalFileSize += fileSize;

                                if (totalFileSize > emailSize)
                                {
                                    if (reportWithinEmailSizeLimit.Count == 0)
                                    {
                                        totalFileSize = 0;
                                        emailBody = new EmailBody();
                                        emailBody.EmailId = index;
                                        emailBody.EmailCount = string.Empty;
                                        emailBody.EmailValidOption = (int)EmailValidOption.EmailSizeExceed;
                                        emailBody.ReportList.Add(report);
                                        emailBody.ContainerList = containerData.Where(x => report.ReportId == x.ReportId).ToList();
                                        emailBody.ProductList = productData.Where(x => report.ReportId == x.ReportId).ToList();
                                        emailSizeBodyList.Add(emailBody);
                                        index++;
                                    }
                                    else if (reportWithinEmailSizeLimit.Count > 0)
                                    {

                                        emailBody = new EmailBody();
                                        emailBody.EmailId = index;
                                        emailBody.EmailCount = index.ToString();
                                        emailBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                                        emailBody.ReportList = groupEmail.SelectMany(x => x.ReportList).Where(x => reportWithinEmailSizeLimit.Contains(x.ReportId.ToString())).ToList();
                                        emailBody.ContainerList = containerData.Where(x => reportWithinEmailSizeLimit.Contains(x.ReportId.GetValueOrDefault().ToString())).ToList();
                                        emailBody.ProductList = productData.Where(x => reportWithinEmailSizeLimit.Contains(x.ReportId.GetValueOrDefault().ToString())).ToList();
                                        emailSizeBodyList.Add(emailBody);
                                        reportWithinEmailSizeLimit.Clear();
                                        totalFileSize = fileSize;
                                        index++;


                                        if (request.EsTypeId > 0 && request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                                        {
                                            if (groupEmail.SelectMany(x => x.ReportList).LastOrDefault().InvoiceNo == report.InvoiceNo)
                                            {
                                                emailBody = new EmailBody();
                                                emailBody.EmailId = index;
                                                emailBody.EmailCount = index.ToString();
                                                emailBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                                                emailBody.ReportList = item.ReportList.Where(x => report.InvoiceNo == x.InvoiceNo).ToList();
                                                emailBody.ContainerList = new List<InspectionContainerRepo>();
                                                emailBody.ProductList = new List<InspectionProductRepo>();
                                                emailSizeBodyList.Add(emailBody);
                                                totalFileSize = 0;
                                            }
                                            else
                                            {
                                                reportWithinEmailSizeLimit.Add(report.InvoiceNo);
                                            }
                                        }
                                        else
                                        {
                                            // check the report is last item then form one new email
                                            if (groupEmail.SelectMany(x => x.ReportList).LastOrDefault().ReportId == report.ReportId)
                                            {
                                                emailBody = new EmailBody();
                                                emailBody.EmailId = index;
                                                emailBody.EmailCount = index.ToString();
                                                emailBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                                                emailBody.ReportList = item.ReportList.Where(x => report.ReportId == x.ReportId).ToList();
                                                emailBody.ContainerList = containerData.Where(x => report.ReportId == x.ReportId.GetValueOrDefault()).ToList();
                                                emailBody.ProductList = productData.Where(x => report.ReportId == x.ReportId.GetValueOrDefault()).ToList();
                                                emailSizeBodyList.Add(emailBody);
                                                totalFileSize = 0;
                                            }
                                            else
                                            {
                                                reportWithinEmailSizeLimit.Add(report.ReportId.ToString());
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (request.EsTypeId > 0 && request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                                    {
                                        reportWithinEmailSizeLimit.Add(report.InvoiceNo);

                                        if (groupEmail.SelectMany(x => x.ReportList).LastOrDefault().InvoiceNo == report.InvoiceNo)
                                        {
                                            emailBody = new EmailBody();
                                            emailBody.EmailId = index;
                                            emailBody.EmailCount = index.ToString();
                                            emailBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                                            emailBody.ReportList = groupEmail.SelectMany(x => x.ReportList).Where(x => reportWithinEmailSizeLimit.Contains(x.InvoiceNo)).ToList();
                                            emailBody.ContainerList = new List<InspectionContainerRepo>();
                                            emailBody.ProductList = new List<InspectionProductRepo>();
                                            emailSizeBodyList.Add(emailBody);
                                            totalFileSize = 0;
                                            index++;
                                        }
                                    }
                                    else
                                    {
                                        reportWithinEmailSizeLimit.Add(report.ReportId.ToString());

                                        if (groupEmail.SelectMany(x => x.ReportList).LastOrDefault().ReportId == report.ReportId)
                                        {
                                            emailBody = new EmailBody();
                                            emailBody.EmailId = index;
                                            emailBody.EmailCount = index.ToString();
                                            emailBody.EmailValidOption = (int)EmailValidOption.EmailSuccess;
                                            emailBody.ReportList = groupEmail.SelectMany(x => x.ReportList).Where(x => reportWithinEmailSizeLimit.Contains(x.ReportId.ToString())).ToList();
                                            emailBody.ContainerList = containerData.Where(x => reportWithinEmailSizeLimit.Contains(x.ReportId.GetValueOrDefault().ToString())).ToList();
                                            emailBody.ProductList = productData.Where(x => reportWithinEmailSizeLimit.Contains(x.ReportId.GetValueOrDefault().ToString())).ToList();
                                            emailSizeBodyList.Add(emailBody);
                                            totalFileSize = 0;
                                            index++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (emailSizeBodyList.Any())
                {
                    var totalEmailCount = emailSizeBodyList.Where(x => x.EmailCount != "").GroupBy(x => x.EmailId).Count();

                    foreach (var data in emailSizeBodyList)
                    {
                        if (!string.IsNullOrEmpty(data.EmailCount) && totalEmailCount > 1)
                        {
                            data.EmailCount = data.EmailCount + "/" + totalEmailCount;
                        }
                        else
                        {
                            data.EmailCount = string.Empty;
                        }
                    }
                }
            }
            else
            {
                emailSizeBodyList = emailBodyList;
            }
            return emailSizeBodyList;
        }

        /// <summary>
        /// get file size
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private long GetFileSize(string url)
        {
            long result = 4 * 1024;
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                req.Method = "HEAD";
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    if (long.TryParse(resp.Headers.Get("Content-Length"), out long ContentLength))
                    {
                        result = ContentLength;
                    }
                }
            }
            catch (Exception ex)
            {
                result = 4 * 1024;
            }
            return result;
        }

        /// <summary>
        /// Get File type from the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetFileType(string url)
        {
            string result = "application/pdf";
            try
            {

                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                req.Method = "HEAD";
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    if (!string.IsNullOrWhiteSpace(resp.Headers.Get("Content-Type")))
                    {
                        result = resp.Headers.Get("Content-Type");
                    }
                }
            }
            catch (Exception ex)
            {
                result = "application/pdf";
            }

            return result;
        }

        /// <summary>
        /// Frame Email Recipients like TO and CC
        /// </summary>
        /// <param name="addressList"></param>
        /// <param name="ruleData"></param>
        /// <param name="bookingData"></param>
        /// <param name="apiDefaultContacts"></param>
        /// <returns></returns>

        private List<string> GetEmailRecipients(List<EsRecipientType> addressList, EmailRuleDataRepo ruleData, List<EmailInspectionDetail> bookingData, List<string> apiDefaultContacts, List<string> additionalEmails, bool isEmailToCC)
        {
            List<string> toRecepients = new List<string>();

            foreach (var item in addressList)
            {
                switch (item.RecipientTypeId)
                {
                    case (int)EmailRecipientType.Customer:
                        var customerContactList = bookingData.Where(x => x.CustomerContact != null).SelectMany(x => x.CustomerContact).ToList();
                        if (customerContactList != null && customerContactList.Any())
                            AddRecepientEmailAddresses(toRecepients, customerContactList);
                        break;

                    case (int)EmailRecipientType.Factory:
                        var factoryContactList = bookingData.Where(x => x.FactoryContact != null).SelectMany(x => x.FactoryContact).ToList();
                        if (factoryContactList != null && factoryContactList.Any())
                            AddRecepientEmailAddresses(toRecepients, factoryContactList);
                        break;

                    case (int)EmailRecipientType.Supplier:
                        var supplierContactList = bookingData.Where(x => x.SupplierContact != null).SelectMany(x => x.SupplierContact).ToList();
                        if (supplierContactList != null && supplierContactList.Any())
                            AddRecepientEmailAddresses(toRecepients, supplierContactList);
                        break;

                    case (int)EmailRecipientType.APITeam:
                        if (ruleData.ApiContactEmailList != null && ruleData.ApiContactEmailList.Any())
                        {
                            AddRecepientEmailAddresses(toRecepients, ruleData.ApiContactEmailList);
                        }

                        if (apiDefaultContacts != null && apiDefaultContacts.Any())
                        {
                            AddRecepientEmailAddresses(toRecepients, apiDefaultContacts);
                        }

                        break;
                    case (int)EmailRecipientType.Merchandiser:
                        var merchandiserContactList = bookingData.Where(x => x.MerchandiserContactEmailList != null).SelectMany(x => x.MerchandiserContactEmailList).ToList();
                        if (merchandiserContactList != null && merchandiserContactList.Any())
                            AddRecepientEmailAddresses(toRecepients, merchandiserContactList);
                        break;
                    case (int)EmailRecipientType.QuotationCustomerContact:
                        var quotationCustomerContactEmailList = bookingData.Where(x => x.QuotationCustomerContactEmailList != null).SelectMany(x => x.QuotationCustomerContactEmailList).ToList();
                        if (ruleData.EmailSendType == (int)EmailSendingType.InvoiceStatus)
                        {
                            quotationCustomerContactEmailList = quotationCustomerContactEmailList.Where(x => x.InvoiceEmail).ToList();
                        }
                        else
                        {
                            quotationCustomerContactEmailList = quotationCustomerContactEmailList.Where(x => x.IsEmail).ToList();
                        }
                        if (quotationCustomerContactEmailList != null && quotationCustomerContactEmailList.Any())
                            AddRecepientEmailAddresses(toRecepients, quotationCustomerContactEmailList.Select(x => x.Email).ToList());
                        break;
                    case (int)EmailRecipientType.QuotationSupplierContact:

                        var quotationSupplierContactEmailList = bookingData.Where(x => x.QuotationSupplierContactEmailList != null).SelectMany(x => x.QuotationSupplierContactEmailList).ToList();
                        if (ruleData.EmailSendType == (int)EmailSendingType.InvoiceStatus)
                        {
                            quotationSupplierContactEmailList = quotationSupplierContactEmailList.Where(x => x.InvoiceEmail).ToList();
                        }
                        else
                        {
                            quotationSupplierContactEmailList = quotationSupplierContactEmailList.Where(x => x.IsEmail).ToList();
                        }
                        if (quotationSupplierContactEmailList != null && quotationSupplierContactEmailList.Any())
                            AddRecepientEmailAddresses(toRecepients, quotationSupplierContactEmailList.Select(x => x.Email).ToList());
                        break;

                    case (int)EmailRecipientType.QuotationFactoryContact:

                        var quotationFactoryContactEmailList = bookingData.Where(x => x.QuotationFactoryContactEmailList != null).SelectMany(x => x.QuotationFactoryContactEmailList).ToList();
                        if (ruleData.EmailSendType == (int)EmailSendingType.InvoiceStatus)
                        {
                            quotationFactoryContactEmailList = quotationFactoryContactEmailList.Where(x => x.InvoiceEmail).ToList();
                        }
                        else
                        {
                            quotationFactoryContactEmailList = quotationFactoryContactEmailList.Where(x => x.IsEmail).ToList();
                        }
                        if (quotationFactoryContactEmailList != null && quotationFactoryContactEmailList.Any())
                            AddRecepientEmailAddresses(toRecepients, quotationFactoryContactEmailList.Select(x => x.Email).ToList());
                        break;
                    case (int)EmailRecipientType.QuotationInternalContact:

                        var quotationInternalContactEmailList = bookingData.Where(x => x.QuotationInternalContactEmailList != null).SelectMany(x => x.QuotationInternalContactEmailList).ToList();
                        if (quotationInternalContactEmailList != null && quotationInternalContactEmailList.Any())
                            AddRecepientEmailAddresses(toRecepients, quotationInternalContactEmailList);
                        break;
                    case (int)EmailRecipientType.InvoiceContact:

                        var invoiceContactEmailList = bookingData.Where(x => x.InvoiceContactEmailList != null).SelectMany(x => x.InvoiceContactEmailList).ToList();
                        if (invoiceContactEmailList != null && invoiceContactEmailList.Any())
                            AddRecepientEmailAddresses(toRecepients, invoiceContactEmailList);
                        break;
                    case (int)EmailRecipientType.CustomerContact:
                        if (ruleData.CustomerContactEmailList != null && ruleData.CustomerContactEmailList.Any())
                        {
                            AddRecepientEmailAddresses(toRecepients, ruleData.CustomerContactEmailList);
                        }
                        break;
                }
            }

            // Add current login user in the cc if email is send to customer
            if (isEmailToCC && _applicationContext.EmailId != null && new EmailAddressAttribute().IsValid(_applicationContext.EmailId))
            {
                toRecepients.Add(_applicationContext.EmailId);
            }

            //add additional email addresses
            if (additionalEmails != null && additionalEmails.Any())
            {
                AddRecepientEmailAddresses(toRecepients, additionalEmails);
            }

            return toRecepients.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
        }

        /// <summary>
        /// Add recepient email address 
        /// </summary>
        /// <param name="toRecepients"></param>
        /// <param name="recepientEmailAddresses"></param>
        private void AddRecepientEmailAddresses(List<string> toRecepients, List<string> recepientEmailAddresses)
        {
            if (toRecepients == null || recepientEmailAddresses == null)
                return;
            foreach (var recepientEmailAddress in recepientEmailAddresses)
            {
                if (recepientEmailAddress != null && new EmailAddressAttribute().IsValid(recepientEmailAddress))
                {
                    toRecepients.Add(recepientEmailAddress);
                }
            }
        }

        /// <summary>
        /// Get Email Subject based on the rule Configuration
        /// </summary>
        /// <param name="ruleData"></param>
        /// <param name="bookingData"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private async Task<string> GetEmailSubjectOrFileName(EmailRuleDataRepo ruleData, List<EmailInspectionDetail> bookingData,
            List<InspectionProductRepo> productData, List<ReportDetailsRepo> reportData, List<string> reportIds, EmailSendSubjectModule esModule, EmailPreviewRequest request)
        {
            StringBuilder strEmailSubject = new StringBuilder();
            string strResultData = string.Empty;
            string strDelimeterName = string.Empty;
            List<EmailRuleTemplateDetailsRepo> objTemplateDetailsList = null;
            int maxCount = 10;

            if (ruleData != null)
            {
                if (esModule == EmailSendSubjectModule.Subject)
                {
                    strDelimeterName = ruleData.SubjectDelimeterName;

                    if (ruleData.EmailSubjectId > 0)
                    {
                        objTemplateDetailsList = await _emailSendRepo.GetEmailRuleSubjectTemplateData(ruleData.EmailSubjectId);
                    }
                }
                else if (esModule == EmailSendSubjectModule.FileName)
                {
                    strDelimeterName = ruleData.FileDelimeterName;

                    if (ruleData.EmailFileId > 0)
                    {
                        // apply sorting
                        objTemplateDetailsList = await _emailSendRepo.GetEmailRuleFileTemplateData(ruleData.EmailFileId);
                    }
                }

                List<int> reportInspection = new List<int>();

                if (request.EsTypeId > 0 && request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                {
                    reportInspection = reportData.Where(x => reportIds.Contains(x.InvoiceNo)).Select(x => x.BookingId).Distinct().ToList();
                }
                else
                {
                    reportInspection = reportData.Where(x => reportIds.Contains(x.ReportId.ToString())).Select(x => x.BookingId).Distinct().ToList();
                }

                if (objTemplateDetailsList != null && objTemplateDetailsList.Any())
                {
                    foreach (var item in objTemplateDetailsList)
                    {

                        if (!String.IsNullOrEmpty(item.FieldName))
                        {
                            if (item.FieldIsText.GetValueOrDefault())
                            {
                                strEmailSubject.Append(item.FieldName).Append(strDelimeterName);
                            }
                            else
                            {
                                // booking fileds check   
                                if (EmailBookingFileds.Contains(item.FieldId))
                                {

                                    List<object> dataList = null;

                                    if (reportInspection.Any())
                                    {
                                        dataList = bookingData.Where(x => reportInspection.Contains(x.InspectionID)).
                                           Where(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null) != null).
                                           Select(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null)).Distinct().ToList();
                                    }
                                    else
                                    {
                                        dataList = bookingData.Where(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null) != null).
                                           Select(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null)).Distinct().ToList();
                                    }


                                    if (dataList.Any() && dataList[0] != null)
                                    {
                                        var valueType = dataList[0].GetType();
                                        var value = dataList[0];
                                        string strResult = string.Empty;

                                        if (valueType.Equals(typeof(System.String)) || valueType.Equals(typeof(System.Int32))
                                            || valueType.Equals(typeof(System.Decimal))
                                            || (value as IEnumerable<string>).Any())
                                        {

                                            if (value as IEnumerable<string> is null)
                                            {
                                                if (item.DateFormatId != null)
                                                {
                                                    if (item.MaxItem > 0)
                                                    {
                                                        maxCount = item.MaxItem.GetValueOrDefault();
                                                    }
                                                    strResult = ApplyDateFormat(dataList, strDelimeterName, item, maxCount);
                                                }
                                                else
                                                {
                                                    if (item.MaxItem.GetValueOrDefault() > 0)
                                                    {
                                                        maxCount = item.MaxItem.GetValueOrDefault();
                                                    }

                                                    List<string> strListTemp = new List<string>();

                                                    foreach (var newData in dataList)
                                                    {
                                                        if (newData != null)
                                                        {
                                                            strListTemp.Add(newData.ToString());
                                                        }
                                                    }

                                                    if (strListTemp.Count > 0)
                                                    {
                                                        var maxLevelItems = strListTemp.Distinct().Take(maxCount);
                                                        strResult = string.Join(strDelimeterName, maxLevelItems.ToArray());
                                                    }
                                                }
                                            }
                                            else if ((value as IEnumerable<string>).Any())
                                            {

                                                if (item.MaxItem.GetValueOrDefault() > 0)
                                                {
                                                    maxCount = item.MaxItem.GetValueOrDefault();
                                                }
                                                List<string> strListTemp = new List<string>();

                                                foreach (var newData in dataList)
                                                {
                                                    if (newData != null)
                                                    {
                                                        strListTemp.AddRange(newData as IEnumerable<string>);
                                                    }

                                                }

                                                if (strListTemp.Count > 0)
                                                {
                                                    var maxLevelItems = strListTemp.Distinct().Take(maxCount);
                                                    strResult = string.Join(strDelimeterName, maxLevelItems.ToArray());
                                                }
                                            }

                                            if (item.MaxChar.GetValueOrDefault() > 0)
                                            {
                                                strResult = strResult.Substring(0, Math.Min(strResult.Length, item.MaxChar.GetValueOrDefault()));
                                            }
                                            // update title name 
                                            if (item.IsTitle.GetValueOrDefault())
                                            {
                                                if (!string.IsNullOrWhiteSpace(item.TitleCustomName))
                                                {
                                                    strResult = item.TitleCustomName + ":" + strResult;
                                                }
                                                else
                                                {
                                                    strResult = item.FieldName + ":" + strResult;
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(strResult))
                                            {
                                                strEmailSubject.Append(strResult).Append(strDelimeterName);
                                            }
                                        }
                                    }
                                }

                                // product fileds check 
                                if (EmailProductFileds.Contains(item.FieldId))
                                {
                                    List<object> dataList = null;
                                    if (request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                                    {
                                        dataList = productData.Where(x =>
                                        reportInspection.Contains(x.BookingId)).
                                        Where(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null) != null).
                                        Select(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null)).Distinct().ToList();
                                    }
                                    else
                                    {
                                        dataList = productData.Where(x =>
                                            reportIds.Contains(x.ReportId.GetValueOrDefault().ToString())).
                                            Where(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null) != null).
                                            Select(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null)).Distinct().ToList();
                                    }


                                    if (dataList.Any() && dataList[0] != null)
                                    {
                                        var valueType = dataList[0].GetType();
                                        var value = dataList[0];
                                        string strResult = string.Empty;

                                        if (valueType.Equals(typeof(System.String)) || valueType.Equals(typeof(System.Int32))
                                            || valueType.Equals(typeof(System.Decimal))
                                            || (value as IEnumerable<string>).Any())
                                        {
                                            if (value as IEnumerable<string> is null)
                                            {
                                                if (item.DateFormatId != null)
                                                {
                                                    if (item.MaxItem.GetValueOrDefault() > 0)
                                                    {
                                                        maxCount = item.MaxItem.GetValueOrDefault();
                                                    }
                                                    strResult = ApplyDateFormat(dataList, strDelimeterName, item, maxCount);
                                                }
                                                else
                                                {
                                                    if (item.MaxItem.GetValueOrDefault() > 0)
                                                    {
                                                        maxCount = item.MaxItem.GetValueOrDefault();
                                                    }

                                                    List<string> strListTemp = new List<string>();

                                                    foreach (var newData in dataList)
                                                    {
                                                        if (newData != null)
                                                        {
                                                            strListTemp.Add(newData.ToString());
                                                        }
                                                    }

                                                    if (strListTemp.Count > 0)
                                                    {
                                                        var maxLevelItems = strListTemp.Distinct().Take(maxCount);
                                                        strResult = string.Join(strDelimeterName, maxLevelItems.ToArray());
                                                    }
                                                }
                                            }
                                            else if ((value as IEnumerable<string>).Any())
                                            {
                                                if (item.MaxItem.GetValueOrDefault() > 0)
                                                {
                                                    maxCount = item.MaxItem.GetValueOrDefault();
                                                }
                                                List<string> strListTemp = new List<string>();

                                                foreach (var newData in dataList)
                                                {
                                                    strListTemp.AddRange(newData as IEnumerable<string>);
                                                }

                                                if (strListTemp.Count > 0)
                                                {
                                                    var maxLevelItems = strListTemp.Distinct().Take(maxCount);
                                                    strResult = string.Join(strDelimeterName, maxLevelItems.ToArray());
                                                }
                                            }

                                            if (item.IsTitle.GetValueOrDefault())
                                            {
                                                if (!string.IsNullOrWhiteSpace(item.TitleCustomName))
                                                {
                                                    strResult = item.TitleCustomName + ":" + strResult;
                                                }
                                                else
                                                {
                                                    strResult = item.FieldName + ":" + strResult;
                                                }
                                            }

                                            if (item.MaxChar.GetValueOrDefault() > 0)
                                            {
                                                strResult = strResult.Substring(0, Math.Min(strResult.Length, item.MaxChar.GetValueOrDefault())) + "..";
                                            }

                                            if (!string.IsNullOrEmpty(strResult))
                                            {
                                                strEmailSubject.Append(strResult).Append(strDelimeterName);
                                            }
                                        }
                                    }

                                }

                                // report fileds check 
                                if (EmailReportFileds.Contains(item.FieldId))
                                {

                                    List<object> dataList = null;
                                    if (request.EsTypeId == (int)EmailSendingType.InvoiceStatus)
                                    {
                                        dataList = reportData.Where(x =>
                                        reportInspection.Contains(x.BookingId)).
                                        Where(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null) != null).
                                        Select(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null)).Distinct().ToList();
                                    }
                                    else
                                    {
                                        dataList = reportData.Where(x =>
                                            reportIds.Contains(x.ReportId.ToString())).
                                            Where(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null) != null).
                                            Select(x => x.GetType().GetProperty(item.FieldName).GetValue(x, null)).Distinct().ToList();
                                    }

                                    if (dataList.Any() && dataList[0] != null)
                                    {
                                        var valueType = dataList[0].GetType();
                                        var value = dataList[0];
                                        string strResult = string.Empty;

                                        if (valueType.Equals(typeof(System.String)) || valueType.Equals(typeof(System.Int32))
                                            || valueType.Equals(typeof(System.Decimal))
                                            || (value as IEnumerable<string>).Any())
                                        {
                                            if (value as IEnumerable<string> is null)
                                            {
                                                if (item.DateFormatId != null)
                                                {
                                                    if (item.MaxItem.GetValueOrDefault() > 0)
                                                    {
                                                        maxCount = item.MaxItem.GetValueOrDefault();
                                                    }
                                                    strResult = ApplyDateFormat(dataList, strDelimeterName, item, maxCount);
                                                }
                                                else
                                                {
                                                    if (item.MaxItem.GetValueOrDefault() > 0)
                                                    {
                                                        maxCount = item.MaxItem.GetValueOrDefault();
                                                    }

                                                    List<string> strListTemp = new List<string>();

                                                    foreach (var newData in dataList)
                                                    {
                                                        if (newData != null)
                                                        {
                                                            strListTemp.Add(newData.ToString());
                                                        }
                                                    }

                                                    if (strListTemp.Count > 0)
                                                    {
                                                        var maxLevelItems = strListTemp.Distinct().Take(maxCount);
                                                        strResult = string.Join(strDelimeterName, maxLevelItems.ToArray());
                                                    }
                                                }
                                            }
                                            else if ((value as IEnumerable<string>).Any())
                                            {
                                                if (item.MaxItem.GetValueOrDefault() > 0)
                                                {
                                                    maxCount = item.MaxItem.GetValueOrDefault();
                                                }
                                                List<string> strListTemp = new List<string>();

                                                foreach (var newData in dataList)
                                                {
                                                    strListTemp.AddRange(newData as IEnumerable<string>);
                                                }

                                                if (strListTemp.Count > 0)
                                                {
                                                    var maxLevelItems = strListTemp.Distinct().Take(maxCount);
                                                    strResult = string.Join(strDelimeterName, maxLevelItems.ToArray());
                                                }
                                            }

                                            if (item.IsTitle.GetValueOrDefault())
                                            {
                                                if (!string.IsNullOrWhiteSpace(item.TitleCustomName))
                                                {
                                                    strResult = item.TitleCustomName + ":" + strResult;
                                                }
                                                else
                                                {
                                                    strResult = item.FieldName + ":" + strResult;
                                                }
                                            }

                                            if (item.MaxChar.GetValueOrDefault() > 0)
                                            {
                                                strResult = strResult.Substring(0, Math.Min(strResult.Length, item.MaxChar.GetValueOrDefault())) + "..";
                                            }

                                            if (!string.IsNullOrEmpty(strResult))
                                            {
                                                strEmailSubject.Append(strResult).Append(strDelimeterName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (strEmailSubject.Length > 0)
                {
                    // remove last delimeter
                    strEmailSubject.Remove(strEmailSubject.Length - 1, 1);
                    strResultData = string.Join(strDelimeterName, strEmailSubject.ToString().Split(strDelimeterName).Distinct().ToArray());
                }
            }

            return strResultData;
        }

        /// <summary>
        /// Apply date format 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="strDelimeterName"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private string ApplyDateFormat(List<object> dataList, string strDelimeterName, EmailRuleTemplateDetailsRepo item, int maxCount)
        {

            DateTime dateFiledValue;
            string strResult = string.Empty;
            string DateFromatValue = string.Empty;
            string DateFromatValueNoSlash = string.Empty;
            List<string> strListTemp = new List<string>();
            if (dataList != null)
            {
                foreach (var date in dataList)
                {
                    if (date != null)
                    {
                        bool isSuccess = DateTime.TryParseExact(date.ToString(),
                                                       StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFiledValue);

                        if (isSuccess)
                        {
                            var day = dateFiledValue.Day < 10 ? "0" + dateFiledValue.Day : dateFiledValue.Day.ToString();
                            var month = dateFiledValue.Month < 10 ? "0" + dateFiledValue.Month : dateFiledValue.Month.ToString();

                            switch (item.DateFormatId.GetValueOrDefault())
                            {
                                case (int)EmailDateFormat.Format1:
                                    DateFromatValue = day + "/" + month + "/" + dateFiledValue.Year;
                                    DateFromatValueNoSlash = day + "" + month + "" + dateFiledValue.Year;
                                    break;

                                case (int)EmailDateFormat.Format2:
                                    DateFromatValue = month + "/" + day + "/" + dateFiledValue.Year;
                                    DateFromatValueNoSlash = month + "" + day + "" + dateFiledValue.Year;
                                    break;

                                case (int)EmailDateFormat.Format3:
                                    DateFromatValue = dateFiledValue.Year + "/" + month + "/" + day;
                                    DateFromatValueNoSlash = dateFiledValue.Year + "" + month + "" + day;
                                    break;

                                case (int)EmailDateFormat.Format4:
                                    DateFromatValue = day + "_" + dateFiledValue.ToString("MMM") + "_" + dateFiledValue.Year;
                                    DateFromatValueNoSlash = day + "" + dateFiledValue.ToString("MMM") + "" + dateFiledValue.Year;
                                    break;
                            }

                            DateTime datenewFiledValue;

                            DateTime.TryParseExact(DateFromatValue, item.DateFormat,
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out datenewFiledValue);


                            if (item.IsDateSeparator.GetValueOrDefault())
                            {
                                strResult = string.Join(strDelimeterName, datenewFiledValue.ToString(item.DateFormat));
                            }
                            else
                            {
                                strResult = string.Join(strDelimeterName, DateFromatValueNoSlash);
                            }
                            if (!string.IsNullOrEmpty(strResult))
                            {
                                strListTemp.Add(strResult);
                            }
                        }
                    }
                }
            }

            if (strListTemp.Count > 0)
            {
                var maxLevelItems = strListTemp.Distinct().Take(maxCount);
                strResult = string.Join(strDelimeterName, maxLevelItems.ToArray());
            }

            return strResult;
        }


        /// <summary>
        /// Get the email Send History Details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<EmailSendHistoryResponse> GetEmailSendHistory(int inspectionId, int reportId, int EmailTypeId)
        {
            //get the email log ids
            var emailHistoryRepo = await _emailSendRepo.GetEmailSendHistory(inspectionId, reportId, EmailTypeId);

            if (emailHistoryRepo != null && emailHistoryRepo.Any())
            {
                //get the emailhistory popup data
                List<EmailSendHistory> emailHistoryList = AddEmailHistoryList(emailHistoryRepo);

                return new EmailSendHistoryResponse() { Result = EmailSendHistoryResult.Success, EmailSendHistoryList = emailHistoryList };
            }


            return new EmailSendHistoryResponse() { Result = EmailSendHistoryResult.NotFound };
        }

        /// <summary>
        /// add the email history popup data from email history repo
        /// </summary>
        /// <param name="emailHistoryRepo"></param>
        /// <returns></returns>
        private List<EmailSendHistory> AddEmailHistoryList(List<EmailSendHistoryRepo> emailHistoryRepo)
        {
            var emailHistoryList = new List<EmailSendHistory>();

            foreach (var email in emailHistoryRepo)
            {
                string senddate = string.Empty;

                if (email.EmailSentOn.HasValue)
                {
                    senddate = email.EmailSentOn.Value.ToString(StandardDateTimeFormat, CultureInfo.InvariantCulture);
                }

                var emailSendHistory = new EmailSendHistory();
                emailSendHistory.EmailSentBy = email.EmailSentBy;
                emailSendHistory.EmailSentOn = senddate;
                //assign the email status text based on the status
                switch (email.EmailStatus)
                {
                    case (int)EmailStatus.Success:
                        emailSendHistory.EmailStatus = "Success";
                        break;
                    case (int)EmailStatus.Failure:
                        emailSendHistory.EmailStatus = "Failure";
                        break;

                }

                emailHistoryList.Add(emailSendHistory);
            }

            return emailHistoryList;
        }

        public async Task<AutoCustomerDecisionResponse> AutoCustomerDecisionList(AutoCustomerDecisionRequest request)
        {
            var response = new AutoCustomerDecisionResponse();
            var autoCustomerDecisionList = new List<AutoCustomerDecision>();
            if (request == null)
                return new AutoCustomerDecisionResponse { Result = AutoCustomerDecisionResult.RequestShouldNotBeEmpty };

            var checkpoint = await _customerCheckPointManager.GetCustomerCheckpoint(request.CustomerId, (int)Service.InspectionId, (int)CheckPointTypeEnum.AutoCustomerDecisionForPassReportResult);

            //get the email config base data
            var emailConfigBaseData = await _emailSendRepo.GetEmailConfigurationBaseDetails((int)EmailSendingType.ReportSend, new[] { request.CustomerId }.ToList(), (int)Service.InspectionId);
            var reportSendTypeIds = emailConfigBaseData.Select(x => x.ReportSendTypeId).Distinct().ToList();

            if (checkpoint != null && (reportSendTypeIds.Contains((int)Email_Report_Send_Type.OneEmailWithAllReports) ||
                reportSendTypeIds.Contains((int)Email_Report_Send_Type.OneEmailWithSameReportResult) ||
                reportSendTypeIds.Contains((int)Email_Report_Send_Type.OneEmailOneReport) ||
                reportSendTypeIds.Contains((int)Email_Report_Send_Type.OneEmailWithReportSummaryAndAllReports)))
            {
                var reportCustomerDecisionComment = await _emailSendRepo.GetSingleAsync<CuReportCustomerDecisionComment>(x => x.Active && x.CustomerId == request.CustomerId && x.ReportResult.ToLower() == FBReportResult.Pass.ToString().ToLower());
                var reportDetailData = _emailSendRepo.GetQueryable<FbReportDetail>(x => x.Active.Value && request.ReportIdList.Contains(x.Id) && x.ResultId == (int)FBReportResult.Pass &&
                                    request.BookingIdList.Contains(x.InspectionId.GetValueOrDefault()) && !x.InspRepCusDecisions.Any(y => y.Active.Value));

                //check if checkpoint brand is selected in booking
                if (checkpoint.BrandList.Any())
                {
                    reportDetailData = reportDetailData.Where(x => x.Inspection.InspTranCuBrands.Any(a => a.Active && checkpoint.BrandList.Contains(a.BrandId)));
                }

                //check if checkpoint dept is selected in booking
                if (checkpoint.DeptList.Any())
                {
                    reportDetailData = reportDetailData.Where(x => x.Inspection.InspTranCuDepartments.Any(a => a.Active && checkpoint.DeptList.Contains(a.DepartmentId)));
                }

                //check if checkpoint service type is selected in booking
                if (checkpoint.ServiceTypeList.Any())
                {
                    reportDetailData = reportDetailData.Where(x => x.Inspection.InspTranServiceTypes.Any(a => a.Active && checkpoint.ServiceTypeList.Contains(a.ServiceTypeId)));
                }

                //check if checkpoint factory country is selected in booking
                if (checkpoint.CountryIdList.Any())
                {
                    reportDetailData = reportDetailData.Where(x => x.Inspection.Supplier.SuAddresses.Any(a => a.AddressTypeId == (int)Supplier_Address_Type.HeadOffice && checkpoint.CountryIdList.Contains(a.CountryId)));
                }

                var reportDetailList = await reportDetailData.AsNoTracking().ToListAsync();

                if (reportDetailList == null || !reportDetailList.Any())
                    return new AutoCustomerDecisionResponse { Result = AutoCustomerDecisionResult.DataNotFound };

                var bookingIds = reportDetailData.Select(x => x.InspectionId.GetValueOrDefault()).Distinct().ToList();
                foreach (var bookingId in bookingIds)
                {
                    //report detail id list by booking id
                    var reportIds = reportDetailData.Where(x => x.InspectionId.GetValueOrDefault() == bookingId).Select(x => x.Id).Distinct().ToList();
                    autoCustomerDecisionList.Add(new AutoCustomerDecision
                    {
                        BookingId = bookingId,
                        ReportIdList = reportIds,
                        Comments = reportCustomerDecisionComment?.Comments
                    });
                }
                response.AutoCustomerDecisionList = autoCustomerDecisionList;
                response.Result = AutoCustomerDecisionResult.Success;
            }
            return response;
        }

        private string GetInvoiceUrl(string invoiceNo, string invoiceFileName)
        {
            var serverUrl = _configuration["ServerUrl"];
            var invoiceDownloadUrl = _configuration["InvoiceDownloadUrl"];

            string encryptedFileName = null;
            if (!string.IsNullOrWhiteSpace(invoiceFileName))
            {
                encryptedFileName = EncryptionDecryption.EncryptStringAES(invoiceFileName);
            }
            var encryptedInvoiceno = EncryptionDecryption.EncryptStringAES(invoiceNo);
            var invoiceUrl = string.Concat(serverUrl, string.Format(invoiceDownloadUrl, WebUtility.UrlEncode(encryptedInvoiceno), WebUtility.UrlEncode(encryptedFileName)));
            if (invoiceUrl.EndsWith('/'))
            {
                invoiceUrl = invoiceUrl.Remove(invoiceUrl.Length - 1);
            }

            return invoiceUrl;
        }

        private void MapCustomerResultAnalysis(List<DTO.Dashboard.CustomerResultMasterRepo> customerResultAnalysis, List<ReportDetailsRepo> reportDetailsList)
        {
            foreach (var reportDetail in reportDetailsList)
            {
                var customerResult = customerResultAnalysis.FirstOrDefault(x => x.Id == reportDetail.CustomerDecisionResultId);
                if (customerResult != null)
                {
                    if (!string.IsNullOrEmpty(customerResult.CustomDecisionName))
                        reportDetail.CustomerDecisionResult = customerResult.CustomDecisionName;
                    else
                        reportDetail.CustomerDecisionResult = customerResult.CustomerDecisionName;
                }
            }
        }
    }
}
