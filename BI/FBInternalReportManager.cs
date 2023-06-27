using BI.Maps;
using BI.Maps.APP;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.FBInternalReport;
using DTO.Inspection;
using DTO.MobileApp;
using DTO.RepoRequest.Enum;
using DTO.Report;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class FBInternalReportManager : ApiCommonData, IFBInternalReportManager
    {
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IFBInternalReportRepository _repo = null;
        private readonly ILogger<ScheduleManager> _logger = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly ICustomerManager _customerManager = null;
        private readonly IFullBridgeRepository _fbRepo = null;
        private readonly IHumanResourceManager _hrManager = null;
        private readonly IInspectionBookingRepository _repoInspection = null;
        private readonly FBInternalReportMap _fbinternalmap = null;
        private readonly BookingMap _bookingmap = null;
        private readonly InspReportMobileMap InspReportMobileMap = null;
        private readonly IUserConfigRepository _userConfigRepo = null;
        public FBInternalReportManager
            (
            IFBInternalReportRepository repo,
            IAPIUserContext applicationContext,
            ILogger<ScheduleManager> logger,
            IOfficeLocationManager office,
            ICustomerManager customerManager,
            IFullBridgeRepository fbRepo,
            IHumanResourceManager hrManager,
            IInspectionBookingRepository repoInspection, IUserConfigRepository userConfigRepo)
        {
            _ApplicationContext = applicationContext;
            _repo = repo;
            _logger = logger;
            _office = office;
            _customerManager = customerManager;
            _fbRepo = fbRepo;
            _hrManager = hrManager;
            _repoInspection = repoInspection;
            _fbinternalmap = new FBInternalReportMap();
            _bookingmap = new BookingMap();
            InspReportMobileMap = new InspReportMobileMap();
            _userConfigRepo = userConfigRepo;
        }
        /// <summary>
        /// Export Internal FB Summary Reports
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InternalFBReportItem>> ExportInternalFBReports(InspectionSummarySearchRequest request)
        {
            InternalFBReportSummaryResponse res = new InternalFBReportSummaryResponse();

            // get all the inspection reports based on the filters
            var response = await GetAllInspectionReportProducts(request);

            // get only booking ids
            var bookingIds = response.Data.Select(x => x.BookingId);

            // get product list based on the booking ids
            var productList = await _repoInspection.GetProductListByBookingByPO(bookingIds.ToList());

            foreach (var product in productList)
            {
                if (product.FbReportId == 0)
                {
                    product.FbReportId = product.FbContainerReportId != 0 ? product.FbContainerReportId : 0;
                }
            }

            // apply order by logic for combine process.
            productList = productList.OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ThenBy(x => x.ProductName).ToList();

            // get report Id from the product list
            var reportIds = productList.Select(x => x.FbReportId);

            // get report data based on report ids
            var fbReportInfoList = await _fbRepo.GetFbReportStatusListCustomerReportbyBooking(reportIds.ToList());

            // mapping the products data with right format
            var data = productList.Select(x => _fbinternalmap.GetProductList(x,
                fbReportInfoList.Where(z => z.ReportId == x.FbReportId).FirstOrDefault(), productList));

            var result = response.Data.Select(x => _fbinternalmap.MapInternalProductsToBooking(x, data));

            return result;
        }

        /// <summary>
        /// Set office and customer Filters
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<List<int>> setOfficeAndCustomerFilter(InspectionSummarySearchRequest request)
        {
            var cuslist = new List<int>();

            if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
            {
                if (request.Officeidlst != null && request.Officeidlst.Count() == 0)
                {
                    var _cusofficelist = _office.GetOfficesByUserId(_ApplicationContext.StaffId);

                    request.Officeidlst = _cusofficelist == null || _cusofficelist.Count() == 0 ? request.Officeidlst : _cusofficelist.Select(x => (int?)x.Id);
                }
                if (request?.CustomerId == null)
                {
                    var customerresponse = await _customerManager.GetCustomersByUserId(_ApplicationContext.StaffId);

                    if (customerresponse != null && customerresponse.Count() != 0)
                        cuslist.AddRange(customerresponse.Select(x => x.Id));
                }
            }
            else
            {
                if (request?.CustomerId > 0)
                    cuslist.Add(request.CustomerId.Value);
            }

            return cuslist;
        }

        private IQueryable<InternalReportBookingValues> GetAllInspectionReportProductsFilter(InspectionSummarySearchRequest request, IEnumerable<FBReportDetails> reportdetails, List<int> customerlst)
        {

            // get all the inspection Reports
            var data = _repo.GetAllInspectionsReports();

            if (request != null && customerlst != null && customerlst.Count() > 0)
            {
                data = data.Where(x => customerlst.Contains(x.CustomerId.Value));
            }

            // set all the global filters
            if (request != null && request.SupplierId != 0 && request.SupplierId != null)
            {
                data = data.Where(x => x.SupplierId == request.SupplierId);
            }

            if (request != null && request.FactoryIdlst != null && request.FactoryIdlst.Count() > 0)
            {
                data = data.Where(x => request.FactoryIdlst.ToList().Contains(x.FactoryId.Value));
            }

            if (request != null && request.Officeidlst != null && request.Officeidlst.Count() > 0 && data.Any(x => x.OfficeId != null))
            {
                data = data.Where(x => x.OfficeId != null && request.Officeidlst.ToList().Contains(x.OfficeId));
            }

            if (request != null && request.StatusIdlst != null && request.StatusIdlst.Count() > 0)
            {
                data = data.Where(x => request.StatusIdlst.ToList().Contains(x.StatusId));
            }

            if (Enum.TryParse(request.SearchTypeId.ToString(), out CustomerReportSearchType _seachtypeenum))
            {
                if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()))
                {
                    switch (_seachtypeenum)
                    {
                        case CustomerReportSearchType.BookingNo:
                            {
                                data = data.Where(x => x.BookingId == int.Parse(request.SearchTypeText));
                                break;
                            }

                        case CustomerReportSearchType.ReportNo:
                            {
                                if (reportdetails != null)
                                {
                                    var filteredData = reportdetails.Where(z => z.ReportTitle.Contains(request.SearchTypeText)).Select(x => x.BookingId).Distinct();
                                    data = data.Where(y => filteredData.ToList().Contains(y.BookingId));
                                }
                                break;
                            }
                        case CustomerReportSearchType.CustomerBookingNo:
                            {
                                data = data.Where(x => EF.Functions.Like(x.CustomerBookingNo.Trim(), $"%{request.SearchTypeText.Trim()}%"));
                                break;
                            }
                    }
                }
                if (Enum.TryParse(request.DateTypeid.ToString(), out SearchType _datesearchtype))
                {
                    if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                    {
                        data = data.Where(x => !(x.ServiceDateFrom > (request.ToDate.ToDateTime()) || x.ServiceDateTo < (request.FromDate.ToDateTime())));
                    }

                }
            }
            return data;
        }

        /// <summary>
        /// Fetch All the inspection reports
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<InternalFBReportSummaryResponse> GetAllInspectionReportProducts(InspectionSummarySearchRequest request)
        {

            if (request == null)
                return new InternalFBReportSummaryResponse() { Result = InternalReportSummaryResponseResult.NotFound };

            var response = new InternalFBReportSummaryResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            // set skip and take values from request
            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            int take = request.pageSize.Value;

            // set Office and Customer Filter
            var customerList = await setOfficeAndCustomerFilter(request);

            // get FB Report Info List
            var fbReportInfoList = await _fbRepo.GetFbReportStatusCustomerReport();

            var data = GetAllInspectionReportProductsFilter(request, fbReportInfoList, customerList);

            // get only booking ids
            var bookingIds = await data.Select(x => x.BookingId).ToListAsync();

            // get service type list based on the selective booking ids.
            var serviceTypeList = await _repo.GetServiceType(bookingIds);

            response.TotalCount = bookingIds.Count();

            try
            {

                if (response.TotalCount == 0)
                {
                    response.Result = InternalReportSummaryResponseResult.NotFound;
                    return response;
                }

                var statusItem = data.Select(x => x.Status);

                var items = await data.GroupBy(p => p.StatusId, p => p, (key, _data) =>
                  new InspectionStatus
                  {
                      Id = key,
                      StatusName = _data.Where(x => x.StatusId == key).Select(x => x.Status.Status).FirstOrDefault(),
                      TotalCount = _data.Count()
                  }).ToListAsync();

                // execute the final result.
                var result = await data.Skip(skip).Take(take).ToListAsync();

                if (result == null || !result.Any())
                    return new InternalFBReportSummaryResponse() { Result = InternalReportSummaryResponseResult.NotFound };

                var _resultdata = result.Select(x => _fbinternalmap.GetInspectionReportResult(x, items, serviceTypeList));

                var _statuslist = items.Select(x => _bookingmap.GetBookingStatuswithColor(x));

                return new InternalFBReportSummaryResponse()
                {
                    Result = InternalReportSummaryResponseResult.Success,
                    TotalCount = response.TotalCount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                    Data = _resultdata,
                    InspectionStatuslst = _statuslist

                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get Booking Products and report details based on booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IQueryable<InternalReportProductItem>> GetProductsByBooking(int bookingId)
        {
            var res = _repo.GetProductsByBooking(bookingId);
            var reportId = res.Select(x => x.FbReportId).FirstOrDefault();
            var fbReportInfoList = await _fbRepo.GetFbReportStatusCustomerReportbyBooking(reportId);
            var data = res.Select(x => _fbinternalmap.GetProductList(x, fbReportInfoList, res));
            return data;
        }
        /// <summary>
        /// Generate inspection detail pdf
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        public async Task<QCInspectionDetailsPDF> GetQCInspectionDetails(int inspectionID)
        {
            string staffName = string.Empty;
            //Get the staff base information by staff id
            var staff = await _hrManager.GetStaffByStaffId(_ApplicationContext.StaffId);
            if (staff != null)
            {
                //if logged in user is inspector then show the staffname
                if (staff.HrStaffProfiles != null)
                {
                    var profileList = staff.HrStaffProfiles.ToList();
                    if (profileList.Where(x => x.ProfileId == (int)HRProfile.Inspector).Count() > 0)
                    {
                        staffName = staff.StaffName;
                    }
                }
            }

            //Get the inspection transaction base data
            var inspTransactions = _repo.GetQCInspectionDetails(inspectionID);

            var bookingIdList = new int[] { inspectionID }.ToList();
            inspTransactions.FactoryContacts = await _repoInspection.GetFactoryContactsByBookingIds(bookingIdList);

            //get brand details
            var brandList = await _repoInspection.GetBookingBrandList(bookingIdList);

            //get department details
            var deptList = await _repoInspection.GetBookingDepartmentList(bookingIdList);

            var inspectionCsList = await _repoInspection.GetInspectionTransCSList(inspectionID);

            List<QCInspectionProductDetails> inspPOTransactions = null;
            if (inspTransactions.BussinessLine == (int)BusinessLine.HardLine)
            {
                inspPOTransactions = _repo.GetQCInspectionProductDetails(inspectionID).ToList();
            }
            else if (inspTransactions.BussinessLine == (int)BusinessLine.SoftLine)
            {
                inspPOTransactions =await _repo.GetQCInspectionProductSoftlineDetails(inspectionID);
            }

            //Get the inspection product data
            var containerList = await _repoInspection.GetBookingContainer(bookingIdList);

            var masterConfigs = await GetMasterConfiguration();
            //Map the inspection details and product details for pdf
            var result = _fbinternalmap.MapQCInspectionDetailPDF(inspTransactions, inspPOTransactions, staffName, containerList, brandList, deptList, inspectionCsList);
            result.EntityMasterConfigs = masterConfigs;
            return result;
        }

        public async Task<IEnumerable<QcPickingData>> GetQcPickingDetails(int bookingId)
        {
            var bookingData = await _repo.GetQcPickingDetails(bookingId).ToListAsync();

            var poTranIds = bookingData.Select(x => x.PoTransId);
            var addressData = await _repo.GetLabAddress(poTranIds).ToListAsync();

            var cusAddressData = await _repo.GetCusAddress(poTranIds).ToListAsync();

            var items = addressData.Where(x => x.Lab).GroupBy(p => p.AddressId, (key, _data) =>

                   new PickingLabAddressItem
                   {
                       PoTransId = _data.Where(x => x.AddressId == key).Select(x => x.PoTransId).FirstOrDefault(),
                       LabAddress = _data.Where(x => x.AddressId == key).Select(x => x.LabAddress).FirstOrDefault(),
                       LabName = _data.Where(x => x.AddressId == key).Select(x => x.LabName).FirstOrDefault(),
                       ContactName = _data.Where(x => x.AddressId == key).Select(x => x.ContactName).Distinct().ToList(),
                       Telephone = _data.Where(x => x.AddressId == key).Select(x => x.Telephone).Distinct().ToList(),
                       Email = _data.Where(x => x.AddressId == key).Select(x => x.Email).Distinct().ToList(),
                       PickingQuantity = _data.Where(x => x.AddressId == key).Select(x => x.PickingQuantity).FirstOrDefault(),
                       RegionalAddress = _data.Where(x => x.AddressId == key).Select(x => x.RegionalAddress).FirstOrDefault(),
                       AddressId = _data.Where(x => x.AddressId == key).Select(x => x.AddressId).FirstOrDefault(),
                       Lab = true
                   }).ToList();

            var items2 = cusAddressData.Where(x => x.Customer).GroupBy(p => p.AddressId, (key, _data) =>
                  new PickingLabAddressItem
                  {
                      PoTransId = _data.Where(x => x.AddressId == key).Select(x => x.PoTransId).FirstOrDefault(),
                      LabAddress = _data.Where(x => x.AddressId == key).Select(x => x.LabAddress).FirstOrDefault(),
                      LabName = _data.Where(x => x.AddressId == key).Select(x => x.LabName).FirstOrDefault(),
                      ContactName = _data.Where(x => x.AddressId == key).Select(x => x.ContactName).Distinct().ToList(),
                      Telephone = _data.Where(x => x.AddressId == key).Select(x => x.Telephone).Distinct().ToList(),
                      Email = _data.Where(x => x.AddressId == key).Select(x => x.Email).Distinct().ToList(),
                      PickingQuantity = _data.Where(x => x.AddressId == key).Select(x => x.PickingQuantity).FirstOrDefault(),
                      RegionalAddress = _data.Where(x => x.AddressId == key).Select(x => x.RegionalAddress).FirstOrDefault(),
                      AddressId = _data.Where(x => x.AddressId == key).Select(x => x.AddressId).FirstOrDefault()
                  }).ToList();

            var addressList = items.Concat(items2);

            var productList = await _repo.GetPickingProducts(poTranIds).ToListAsync();

            var customerId = bookingData.Distinct().Select(x => x.CustomerId);

            IEnumerable<CustomerCSLocation> CSLocation = await _repoInspection.GetCSLocationList(customerId);

            var staff = _ApplicationContext.UserProfileList.Contains(4) ? await _hrManager.GetStaffByStaffId(_ApplicationContext.StaffId) : null;

            var result = addressList.Select(x => _fbinternalmap.MapPickingProducts(x, bookingData, productList, CSLocation, staff != null ? staff.StaffName : ""));

            return result;
            //IEnumerable<CustomerCSLocation> CSLocation = await _repo.GetCSLocationList(distinctCusId);
        }

        //get the report details for mobile app
        public async Task<List<MobileInspectionReportData>> GetMobileReportSummary(InspectionSummarySearchRequest request)
        {
            //page size is same as the number of bookings to be fetched
            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 20;
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            //page index - 1 * 10 (pagesize) + 1
            var inspReportKey = ((request.Index.GetValueOrDefault() - 1) * PageSize) + 1;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            int take = request.pageSize.Value;

            IEnumerable<FBReportDetails> reportdetails = null;
            if (request.SearchTypeId == (int)CustomerReportSearchType.ReportNo)
                reportdetails = await _fbRepo.GetFbReportStatusCustomerReport();

            // set Office and Customer Filter
            var customerList = await setOfficeAndCustomerFilter(request);

            // get all the inspection reports based on the filters            
            var response = GetAllInspectionReportProductsFilter(request, reportdetails, customerList);

            // get only booking ids
            var bookingIds = await response.Where(x => x.StatusId != (int)BookingStatus.Cancel).Select(x => x.BookingId).ToListAsync();

            // get product list based on the booking ids
            var productList = await _repoInspection.GetProductListByBookingByPO(bookingIds);

            //group the data based on the supplier Id
            var result = await response.Select(x => new { x.SupplierId, x.BookingId, x.SupplierName }).GroupBy(p => new { p.SupplierId, p.SupplierName }, p => p, (key, _data) =>
                   new MobileInspectionReportData
                   {
                       supplierId = key.SupplierId,
                       supplierName = key.SupplierName,
                       totalInspections = _data.Select(y => y.BookingId).Count()
                   }).Skip(skip).Take(take).AsNoTracking().ToListAsync();


            foreach (var item in result.ToList())
            {
                item.totalReports = productList.Where(x => x.SupplierId == item.supplierId && x.FbReportId > 0).Select(x => x.FbReportId).Distinct().Count();
                item.totalProducts = productList.Where(x => x.SupplierId == item.supplierId && x.FbReportId > 0).Select(x => x.ProductId).Distinct().Count();
                item.key = inspReportKey++;
            }
            return result;
        }

        public async Task<List<MobileInspectionReportProducData>> GetMobileReportSummaryDetails(InspectionSummarySearchRequest request)
        {
            //page size is same as the number of bookings to be fetched

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 20;
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            int take = request.pageSize.Value;

            IEnumerable<FBReportDetails> reportdetails = null;
            if (request.SearchTypeId == (int)CustomerReportSearchType.ReportNo)
                reportdetails = await _fbRepo.GetFbReportStatusCustomerReport();

            // set Office and Customer Filter
            var customerList = await setOfficeAndCustomerFilter(request);

            // get all the inspection reports based on the filters            
            var response = GetAllInspectionReportProductsFilter(request, reportdetails, customerList);

            // get only booking ids
            var bookingIds = await response.Select(x => x.BookingId).ToListAsync();

            // get product list based on the booking ids
            var productList = _repoInspection.GetProductListByBookingByPO(bookingIds.ToList()).Result.ToList();

            //fetch only the products with report
            productList = productList.Where(x => x.FbReportId > 0).ToList();

            var result = InspReportMobileMap.InspReportMap(productList, request.Index.GetValueOrDefault(), request.pageSize.GetValueOrDefault(), skip, take);

            return result;
        }

        //inspection reports data for the mobile app
        public async Task<InspReportMobileResponse> GetMobileInspectionReportSummary(InspSummaryMobileRequest request)
        {
            var response = new InspReportMobileResponse();

            try
            {
                InspectionSummarySearchRequest inspRequest = RequestMobileMap.MapInspRequest(request);
                response.data = await GetMobileReportSummary(inspRequest);
                response.meta = new MobileResult { success = true, message = "" };
            }

            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Inspection Summary fetch failed." };
            }
            return response;
        }

        //fetch the product and status timeline for the mobile app
        public async Task<InspReportProductsMobileResponse> GetMobileBookingProductsAndStatus(InspSummaryMobileRequest request)
        {
            var response = new InspReportProductsMobileResponse();
            try
            {
                InspectionSummarySearchRequest inspRequest = RequestMobileMap.MapInspRequest(request);
                response.data = await GetMobileReportSummaryDetails(inspRequest);
                response.meta = new MobileResult { success = true, message = "" };
            }

            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Booking products fetch failed." };
            }

            return response;
        }

        public async Task<IEnumerable<EntMasterConfig>> GetMasterConfiguration()
        {
            return await _userConfigRepo.GetMasterConfiguration();
        }
    }
}
