using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.DynamicFields;
using DTO.FinanceDashboard;
using DTO.FullBridge;
using DTO.Invoice;
using DTO.Kpi;
using DTO.KPI;
using DTO.MasterConfig;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static BI.TenantProvider;

namespace BI
{
    public class KpiCustomManager : ApiCommonData, IKpiCustomManager
    {
        private readonly IKpiCustomRepository _repo = null;
        private readonly ICustomerRepository _customerRepo = null;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly IQuotationRepository _quotRepo = null;
        private readonly ICustomerDepartmentRepository _deptRepo = null;
        private readonly IFullBridgeRepository _fbRepo = null;
        private readonly IDynamicFieldManager _dynamicFieldManager = null;
        private readonly ILocationRepository _locationRepository = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IScheduleRepository _schRepo = null;
        private readonly ISupplierRepository _supplierRepository = null;
        private readonly IReferenceManager _referenceManager = null;
        private readonly IMandayManager _mandayManager = null;
        private readonly IInvoiceManager _invoiceManager = null;
        private readonly IHelper _helper = null;
        private readonly KpiCustomMap KpiCustomMap = null;
        private readonly ITenantProvider _tenant = null;
        private readonly IInvoicePreivewRepository _invoicePreivewRepository;
        private readonly IInspectionCustomerDecisionRepository _inspectionCustDecRepository;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IManagementDashboardRepository _managementDashboardRepo;
        private readonly ICustomerManager _customerManager;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IClaimRepository _claimRepo;
        private readonly IInvoiceDataAccessRepository _invoiceDataAccessRepository = null;
        private readonly IReportRepository _reportRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly IFinanceDashboardManager _financeManager;

        public KpiCustomManager(ICustomerRepository customerRepo, IKpiCustomRepository repo, IInspectionBookingManager inspManager, IQuotationRepository quotRepo,
            IAuditRepository auditRepository,
            ICustomerDepartmentRepository deptRepo,
            IFullBridgeRepository fbRepo,
            IInvoicePreivewRepository invoicePreivewRepository,
             IDynamicFieldManager dynamicFieldManager,
             ILocationRepository locationRepository,
             IInspectionBookingRepository inspRepo,
             IScheduleRepository schRepo,
             ISupplierRepository supplierRepository,
             IReferenceManager referenceManager,
             IMandayManager mandayManager,
             IInvoiceManager invoiceManager, IHelper helper,
             ITenantProvider tenant, IInspectionCustomerDecisionRepository inspectionCustDecRepository,
             IAPIUserContext applicationContext, IManagementDashboardRepository managementDashboardRepo,
             IInvoiceDataAccessRepository invoiceDataAccessRepository,
             ICustomerManager customerManager, IDashboardRepository dashboardRepository,
             IClaimRepository claimRepo, IReportRepository reportRepository, IFinanceDashboardManager financeManager)
        {
            _repo = repo;
            _customerRepo = customerRepo;
            _inspManager = inspManager;
            _quotRepo = quotRepo;
            _deptRepo = deptRepo;
            _fbRepo = fbRepo;
            _dynamicFieldManager = dynamicFieldManager;
            _locationRepository = locationRepository;
            _inspRepo = inspRepo;
            _schRepo = schRepo;
            _supplierRepository = supplierRepository;
            _referenceManager = referenceManager;
            _mandayManager = mandayManager;
            _invoiceManager = invoiceManager;
            _helper = helper;
            KpiCustomMap = new KpiCustomMap();
            _tenant = tenant;
            _invoicePreivewRepository = invoicePreivewRepository;
            _ApplicationContext = applicationContext;
            _managementDashboardRepo = managementDashboardRepo;
            _customerManager = customerManager;
            _dashboardRepository = dashboardRepository;
            _claimRepo = claimRepo;
            _invoiceDataAccessRepository = invoiceDataAccessRepository;
            _reportRepository = reportRepository;
            _auditRepository = auditRepository;
            _financeManager = financeManager;
        }

        public async Task<KpiResponse> GetKpisummary()
        {
            KpiResponse response = new KpiResponse();


            var customerByUserType = await _customerManager.GetCustomerByUserType(new CommonDataSourceRequest() { IsVirtualScroll = false });
            if (customerByUserType.Result == DTO.CommonClass.DataSourceResult.Success)
                response.CustomerList = customerByUserType.DataSourceList.ToList();

            var officeList = await _mandayManager.GetOfficeLocations();
            if (officeList != null && officeList.DataSourceList.Any())
            {
                response.OfficeList = officeList.DataSourceList.ToList();
            }
            return response;
        }

        //Product based report for ECI
        public async Task<ExportResult> ExportEciTemplate(KpiRequest request)
        {
            ExportResult result = new ExportResult();
            try
            {
                if (request != null)
                {
                    //fetch the booking details

                    var kpiBookingAndInvoice = await GetBookingDetails(request);

                    if (kpiBookingAndInvoice.BookingItems.Count() == 0)
                        return result = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = kpiBookingAndInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    var serviceTypeList = await _inspRepo.GetServiceType(bookingIds);
                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        serviceTypeList = serviceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = serviceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    var poDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the Product List for all the bookings
                    var productList = await _repo.GetProductListByBooking(bookingIds);

                    //fetch the dynamic fields for the customer and booking
                    var dfBookingFieldsResponse = await _dynamicFieldManager.GetBookingDFDataByBookingIds(bookingIds);

                    //get the quotation details by booking Ids
                    var quotationDetails = await _repo.GetClientQuotationByBooking(bookingIds);

                    //fetch quotation status log confirm date
                    var quotStatusLogs = await _repo.GetQuotationStatusLogById(bookingIds);

                    //get the distinct customer Ids
                    var customerIds = kpiBookingAndInvoice.BookingItems.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();

                    //factory country required for pending quotation 
                    var factoryCountryData = await _inspRepo.GetFactorycountryId(bookingIds);

                    //Fetch the customer Department for the Dept code
                    var customerDept = await _deptRepo.GetCustomerDepartmentsbyBooking(bookingIds);

                    //Get the shipment quantity from FB_Report_Qunatity table
                    var shipmentQty = await _repo.GetInspectionQuantities(bookingIds);

                    //add the shipment qunatity of the same products
                    var shipmentData = shipmentQty.GroupBy(p => new { p.ProductId.Id, p.BookingId }, p => p, (key, _data) =>
                   new BookingShipment
                   {
                       BookingId = _data.Where(x => x.ProductId.Id == key.Id).FirstOrDefault().BookingId,
                       ShipmentQty = _data.Where(x => x.ProductId.Id == key.Id && x.BookingId == key.BookingId).Sum(x => x.ShipmentQty),
                   }).ToList();

                    //fetch Merchandiser
                    var merchandiserList = await _repo.GetMerchandiserByBooking(bookingIds);

                    //Get the custom status for the customers
                    var customStatus = await _customerRepo.GetCustomStatusNameByCustomer(customerIds);

                    //fetch containerItems
                    var containerItems = await _repo.GetContainerItemsByReportId(bookingIds);

                    //Map the booking, quotation and dynamic fileds data
                    result.Data = KpiCustomMap.BookingquotationMapEci(kpiBookingAndInvoice.BookingItems, poDetails, productList, dfBookingFieldsResponse.bookingDFDataList, quotationDetails, bookingIds,
                        customerDept, shipmentData, customStatus, factoryCountryData, serviceTypeList, quotStatusLogs, merchandiserList, containerItems);
                }

                return result;

            }
            catch (Exception e)
            {
                return result = null;
            }
        }

        //Export the booking summary search to Excel
        public async Task<KpiBookingInvoiceResponse> GetBookingDetails(KpiRequest request)
        {
            var kpiBookingInvoiceResponse = new KpiBookingInvoiceResponse();
            //Get all the inspection based on the search criteria
            var response = _repo.GetAllInspections();

            if (!string.IsNullOrEmpty(request.InvoiceNo))
            {
                var invoiceSummaryResponse = await _invoiceManager.GetInvoiceBookingSearchSummary(request.InvoiceNo, (int)Service.InspectionId);

                if (invoiceSummaryResponse.Result == InvoiceSummaryResult.Success)
                {
                    kpiBookingInvoiceResponse.InvoiceBookingData = invoiceSummaryResponse.Data;

                    var bookingIdList = invoiceSummaryResponse.Data.Select(x => x.BookingId).Distinct().ToList();
                    response = response.Where(x => bookingIdList.Contains(x.BookingId));
                }
            }

            if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
            {
                //response = response.Where(x => !((x.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.ServiceDateTo < request.FromDate.ToDateTime())));
                response = response.Where(x => x.ServiceDateTo <= request.ToDate.ToDateTime() && x.ServiceDateTo >= request.FromDate.ToDateTime());
            }

            if (request.CustomerId != 0 && request.CustomerId != null)
            {
                response = response.Where(x => x.CustomerId == request.CustomerId);
            }

            if (request.CustomerIdList != null && request.CustomerIdList.Any())
            {
                response = response.Where(x => request.CustomerIdList.Contains(x.CustomerId.GetValueOrDefault()));
            }

            if (request.OfficeIdLst != null && request.OfficeIdLst.Any())
            {
                response = response.Where(x => request.OfficeIdLst.Contains(x.OfficeId.GetValueOrDefault()));
            }

            if (request.StatusIds != null && request.StatusIds.Any())
            {
                response = response.Where(x => request.StatusIds.Contains(x.StatusId));
            }
            else
            {
                response = response.Where(x => x.StatusId != (int)BookingStatus.Cancel);
            }

            if (request.BookingNo > 0)
            {
                var bookingNo = Convert.ToInt32(request.BookingNo.ToString().Trim());
                response = response.Where(x => x.BookingId == bookingNo);
            }

            var bookingDataList = await response.ToListAsync();

            if (bookingDataList != null && bookingDataList.Any())
            {
                var bookingIds = bookingDataList.Select(x => x.BookingId).Distinct().ToList();

                if (request.BrandIdList != null && request.BrandIdList.Any())
                {
                    var brandBookingIdData = await _inspRepo.GetBookingIdsByBrandsAndBookings(request.BrandIdList, bookingIds);
                    bookingIds = brandBookingIdData;
                    bookingDataList = bookingDataList.Where(x => brandBookingIdData.Contains(x.BookingId)).ToList();
                }

                if (request.DepartmentIdList != null && request.DepartmentIdList.Any())
                {
                    var deptBookingIdData = await _inspRepo.GetBookingIdsByDeptsAndBookings(request.DepartmentIdList, bookingIds);
                    bookingDataList = bookingDataList.Where(x => deptBookingIdData.Contains(x.BookingId)).ToList();
                }
            }

            kpiBookingInvoiceResponse.BookingItems = bookingDataList;

            return kpiBookingInvoiceResponse;
        }


        /// <summary>
        /// Get booking ids query with all the filters
        /// statusIds:- if pass the statusids in kpi request at that time pick the data based on status ids, if we are not pass the status ids then pick the not cancel data.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IQueryable<int> GetBookingIdAsQueryable(KpiRequest request)
        {
            var response = _repo.GetAllInspectionQuery();

            if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
            {
                response = response.Where(x => x.ServiceDateTo <= request.ToDate.ToDateTime() && x.ServiceDateTo >= request.FromDate.ToDateTime());
            }

            if (request.CustomerId > 0)
            {
                response = response.Where(x => x.CustomerId == request.CustomerId);
            }

            if (request.CustomerIdList != null && request.CustomerIdList.Any())
            {
                response = response.Where(x => request.CustomerIdList.Contains(x.CustomerId));
            }

            if (request.OfficeIdLst != null && request.OfficeIdLst.Any())
            {
                response = response.Where(x => request.OfficeIdLst.Contains(x.OfficeId.GetValueOrDefault()));
            }

            if (request.BookingNo > 0)
            {
                var bookingNo = Convert.ToInt32(request.BookingNo.ToString().Trim());
                response = response.Where(x => x.Id == bookingNo);
            }

            if (!string.IsNullOrEmpty(request.InvoiceNo))
            {
                response = response.Where(x => x.InvAutTranDetails.Any(y => y.InvoiceNo == request.InvoiceNo && y.ServiceId == (int)Service.InspectionId && y.InvoiceStatus != (int)InvoiceStatus.Cancelled));
            }

            if (request.IsInvoice)
            {
                response = response.Where(x => x.InvAutTranDetails.Any(y => y.ServiceId == (int)Service.InspectionId && y.InvoiceStatus != (int)InvoiceStatus.Cancelled));
            }

            if (request.TemplateId == (int)KPICustomTemplate.GapProductRefLevel)
            {
                response = response.Where(x => x.InspTranServiceTypes.Any(y => y.Active && y.ServiceTypeId != GapFlashProcessAuditServiceType));
            }

            if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
            {
                response = response.Where(x => x.InspTranServiceTypes.Any(y => y.Active && request.ServiceTypeIdLst.Contains(y.ServiceTypeId)));
            }

            if (request.BrandIdList != null && request.BrandIdList.Any())
            {
                response = response.Where(x => x.InspTranCuBrands.Any(y => y.Active && request.BrandIdList.Contains(y.BrandId)));
            }

            if (request.DepartmentIdList != null && request.DepartmentIdList.Any())
            {
                response = response.Where(x => x.InspTranCuDepartments.Any(y => y.Active && request.DepartmentIdList.Contains(y.DepartmentId)));
            }

            if (request.CountryIds != null && request.CountryIds.Any())
            {
                response = response.Where(x => x.Factory.SuAddresses.Any(y => y.AddressTypeId == (int)RefAddressTypeEnum.HeadOffice && request.CountryIds.Contains(y.CountryId)));
            }

            if (request.StatusIds != null && request.StatusIds.Any())//inspection qc summary, 
            {
                response = response.Where(x => request.StatusIds.Contains(x.StatusId));
            }
            else
            {
                response = response.Where(x => x.StatusId != (int)BookingStatus.Cancel);
            }

            return response.Select(x => x.Id);
        }


        public IQueryable<int> GetAuditIdAsQueryable(KpiRequest request)
        {
            var response = _repo.GetAllAuditQuery();

            if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
            {
                response = response.Where(x => x.ServiceDateTo <= request.ToDate.ToDateTime() && x.ServiceDateTo >= request.FromDate.ToDateTime());
            }

            if (request.CustomerId > 0)
            {
                response = response.Where(x => x.CustomerId == request.CustomerId);
            }

            if (request.CustomerIdList != null && request.CustomerIdList.Any())
            {
                response = response.Where(x => request.CustomerIdList.Contains(x.CustomerId));
            }

            if (request.OfficeIdLst != null && request.OfficeIdLst.Any())
            {
                response = response.Where(x => request.OfficeIdLst.Contains(x.OfficeId.GetValueOrDefault()));
            }

            if (request.BookingNo > 0)
            {
                var bookingNo = Convert.ToInt32(request.BookingNo.ToString().Trim());
                response = response.Where(x => x.Id == bookingNo);
            }

            if (!string.IsNullOrEmpty(request.InvoiceNo))
            {
                response = response.Where(x => x.InvAutTranDetails.Any(y => y.InvoiceNo == request.InvoiceNo && y.ServiceId == (int)Service.InspectionId && y.InvoiceStatus != (int)InvoiceStatus.Cancelled));
            }

            if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
            {
                response = response.Where(x => x.AudTranServiceTypes.Any(y => y.Active && request.ServiceTypeIdLst.Contains(y.ServiceTypeId)));
            }

            if (request.BrandIdList != null && request.BrandIdList.Any())
            {
                response = response.Where(x => request.BrandIdList.Contains(x.BrandId.GetValueOrDefault()));
            }

            if (request.DepartmentIdList != null && request.DepartmentIdList.Any())
            {
                response = response.Where(x => request.DepartmentIdList.Contains(x.DepartmentId.GetValueOrDefault()));
            }

            if (request.CountryIds != null && request.CountryIds.Any())
            {
                response = response.Where(x => x.Factory.SuAddresses.Any(y => y.AddressTypeId == (int)RefAddressTypeEnum.HeadOffice && request.CountryIds.Contains(y.CountryId)));
            }

            if (request.StatusIds != null && request.StatusIds.Any())//inspection qc summary, 
            {
                response = response.Where(x => request.StatusIds.Contains(x.StatusId));
            }
            else
            {
                response = response.Where(x => x.StatusId != (int)AuditStatus.Cancel);
            }

            return response.Select(x => x.Id);
        }

        //Report based template for Warehouse
        public async Task<ExportResult> ExportWarehouseTemplate(KpiRequest request)
        {
            ExportResult result = new ExportResult();
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    var kpiBookingAndInvoice = await GetBookingDetails(request);

                    if (kpiBookingAndInvoice.BookingItems.Count() == 0)
                        return result = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = kpiBookingAndInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    var serviceTypeList = await _inspRepo.GetServiceType(bookingIds);
                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        serviceTypeList = serviceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = serviceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    var poDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the Product List for all the bookings
                    var productList = await _repo.GetProductListByBooking(bookingIds);

                    //fetch quotation details
                    var quotDetails = await _repo.GetQuotationManDay(bookingIds);

                    //factory country required for pending quotation 
                    var factoryCountryData = await _inspRepo.GetFactorycountryId(bookingIds);

                    //Fetch the customer Department for the Dept code
                    var customerDept = await _deptRepo.GetCustomerDepartmentsbyBooking(bookingIds);

                    //fetch Qcnames from FB_Report_QCDetails
                    var reportIdlist = productList.Select(x => x.ReportId.GetValueOrDefault());
                    var qcNames = await _repo.GetFbQcNames(reportIdlist.ToList());

                    //Get Problematic remarks for reports
                    var reportRemarks = await _repo.GetFbProblematicRemarks(reportIdlist.ToList());

                    result.Data = KpiCustomMap.BookingquotationMapWarehouse(kpiBookingAndInvoice.BookingItems, poDetails, productList, quotDetails, customerDept, factoryCountryData, qcNames, reportRemarks);
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        //GIFI Customer KPI Template details of booking, quotation.
        public async Task<ExportResult> ExportGifiTemplate(KpiRequest request)
        {
            ExportResult result = new ExportResult();

            try
            {

                if (request != null)
                {
                    var dfBooking = new List<InspectionBookingDFData>();

                    //fetch the booking details
                    var kpiBookingAndInvoice = await GetBookingDetails(request);

                    if (kpiBookingAndInvoice.BookingItems.Count() == 0)
                        return result = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = kpiBookingAndInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    var serviceTypeList = await _inspRepo.GetServiceType(bookingIds);
                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        serviceTypeList = serviceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = serviceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    //po details
                    var poDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the Product List for all the bookings
                    var productList = await _repo.GetProductListByBooking(bookingIds);

                    //fetch the dynamic fields for the customer and booking
                    var dfBookingFieldsResponse = await _dynamicFieldManager.GetBookingDFDataByBookingIds(bookingIds);

                    //check dynamic fields if it success
                    if (dfBookingFieldsResponse.Result == InspectionBookingDFDataResult.Success)
                    {
                        dfBooking = dfBookingFieldsResponse.bookingDFDataList;
                    }

                    //factory country details
                    var factoryCountryData = await _inspRepo.GetFactorycountryId(bookingIds);

                    //get the quotation details by booking Ids
                    var quotationDetails = await _repo.GetClientQuotationByBooking(bookingIds);

                    //fetch quotation status log with confirm date
                    var quotStatusLogs = await _repo.GetQuotationStatusLogById(bookingIds);

                    //Fetch the customer buyer name
                    var customerBuyer = await _repo.GetCustomerBuyerbyBooking(bookingIds);

                    //get fb report id list from product list
                    var fbReportIdList = productList.Select(x => x.ReportId.GetValueOrDefault()).ToList();

                    //fetch fb other information
                    var fbOtherInformation = await _repo.GetFBOtherInformationList(fbReportIdList);

                    //fetch fb sample type
                    var fbSampleType = await _repo.GetFBSampleTypeList(fbReportIdList);

                    //get invoice number
                    var invoiceData = await _repo.GetInvoiceNoByBooking(bookingIds);

                    result.Data = KpiCustomMap.GIFITemplateBookingQuotationMap(kpiBookingAndInvoice.BookingItems, poDetails, productList.OrderBy(x => x.BookingId).ToList(), factoryCountryData, serviceTypeList.ToList()
                                            , quotationDetails, quotStatusLogs, customerBuyer, dfBooking, fbOtherInformation, fbSampleType, invoiceData);
                }

            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        //Adeo Customer KPI Template details based on Report
        public async Task<List<AdeoEanTemplate>> ExportAdeoEanCodeTemplate(KpiRequest request)
        {
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    var bookingIds = GetBookingIdAsQueryable(request);

                    if (bookingIds == null || !bookingIds.Any())
                    {
                        return null;
                    }

                    var productContainerData = await _repo.GetProductListByBooking(bookingIds);
                    productContainerData.AddRange(await _repo.GetContainerListByBooking(bookingIds));

                    var result = KpiCustomMap.BookingMapAdeoEanCode(productContainerData);

                    if (result == null || !result.Any())
                    {
                        return null;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        //Adeo Customer KPI Template details based on Product
        public async Task<List<AdeoFranceInspSummaryTemplate>> ExportAdeoFranceInspSummaryTemplateByEfCore(KpiRequest request)
        {
            KpiAdeoTemplateRequest mapRequest = new KpiAdeoTemplateRequest();
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    var bookingIdList = GetBookingIdAsQueryable(request);

                    if (bookingIdList == null || !bookingIdList.Any())
                    {
                        return null;
                    }

                    mapRequest.BookingData = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingIdList);
                    mapRequest.ProductData = await _repo.GetProductListByBooking(bookingIdList);
                    mapRequest.ProductData.AddRange(await _repo.GetContainerListByBooking(bookingIdList));

                    mapRequest.PoDetails = await _repo.GetBookingPoDetails(bookingIdList);

                    mapRequest.DeptList = await _repo.GetKPIBookingDepartmentDataEfCore(bookingIdList);

                    var result = KpiCustomMap.MapAdeoFranceInspSummary(mapRequest);

                    if (result == null || !result.Any())
                    {
                        return null;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        //Adeo Customer KPI Template details based on Factory
        public async Task<List<AdeoMonthInspSumbySubconFactoTemplate>> ExportAdeoMonthInspSumbySubconFactoTemplateByEfCore(KpiRequest request)
        {
            KpiAdeoTemplateRequest mapRequest = new KpiAdeoTemplateRequest();

            try
            {
                if (request != null)
                {
                    var bookingIdList = GetBookingIdAsQueryable(request);

                    if (bookingIdList == null || !bookingIdList.Any())
                    {
                        return null;
                    }

                    mapRequest.FactoryProductData = await _repo.GetAdeoFollowUpProductDataEfCore(bookingIdList);
                    mapRequest.FactoryProductData.AddRange(await _repo.GetAdeoFollowUpContainerDataEfCore(bookingIdList));

                    //Fetch the customer Department for the Dept code
                    mapRequest.DeptList = await _repo.GetKPIBookingDepartmentDataEfCore(bookingIdList);

                    //fetch Merchandiser
                    mapRequest.MerchandiserList = await _repo.GetMerchandiserByBooking(bookingIdList);

                    //Create a list of SupID and FactoryID to fetch the customer codes in one DB call
                    var customerIdList = mapRequest.FactoryProductData.Select(x => x.CustomerId).Distinct().ToList();
                    var supIds = new List<int>();
                    supIds.AddRange(mapRequest.FactoryProductData.Select(x => x.SupplierId).Distinct());
                    supIds.AddRange(mapRequest.FactoryProductData.Where(x => x.FactoryId > 0).Select(x => x.FactoryId.GetValueOrDefault()).Distinct());
                    mapRequest.SupCode = await _supplierRepository.GetSupplierCode(customerIdList, supIds);

                    var result = KpiCustomMap.MapAdeoInspSumByFactory(mapRequest);

                    if (result == null || !result.Any())
                    {
                        return null;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        //Adeo Customer KPI Template details based on Product for the QC's allocated
        public async Task<ExportResult> ExportAdeoInspSummaryByQATemplate(KpiRequest request)
        {
            ExportResult result = new ExportResult();
            var parameterList = new KPIMapParameters();
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    parameterList.BookingInvoice = await GetBookingDetails(request);

                    if (parameterList.BookingInvoice.BookingItems.Count() == 0)
                        return result = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        var serviceTypeList = await _inspRepo.GetServiceType(bookingIds);
                        serviceTypeList = serviceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = serviceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    //fetch the PO details for the bookings
                    parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the Product List for all the bookings
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);
                    parameterList.ProductList = parameterList.ProductList.Where(x => x.ReportResultId != (int)FBReportResult.Pass).ToList();

                    //Fetch the customer Department for the Dept code
                    parameterList.CustomerDept = await _deptRepo.GetCustomerDepartmentsbyBooking(bookingIds);

                    //fetch Customer contacts
                    var reportIdlist = parameterList.ProductList.Select(x => x.ReportId.GetValueOrDefault()).Distinct().ToList();
                    parameterList.CustomerContactData = await _repo.GetCustomerContactNames(bookingIds);


                    //Create a list of SupID and FactoryID to fetch the customer codes in one DB call
                    var customerIdList = parameterList.BookingInvoice.BookingItems.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();
                    var supIds = new List<int>();
                    supIds.AddRange(parameterList.BookingInvoice.BookingItems.Select(x => x.SupplierId.GetValueOrDefault()).Distinct());
                    parameterList.SupplierCode = await _supplierRepository.GetSupplierCode(customerIdList, supIds);

                    //Get Problematic remarks for reports
                    parameterList.ReportRemarks = await _repo.GetFbProblematicRemarks(reportIdlist);

                    //get the resinpection Ids for failed bookings
                    parameterList.ReInspectionList = await _repo.GetReinspectionBooking(bookingIds);

                    parameterList.CustomerDecision = await _repo.GetCustomerDecisionData(bookingIds);

                    result.Data = KpiCustomMap.BookingSummaryMapByReport(parameterList);
                }
            }
            catch (Exception ex)
            {
                return result = null;
            }
            return result;
        }

        //Adeo Customer KPI Template details based on product in the selected date range
        public async Task<List<AdeoInspSumOverallTemplate>> ExportAdeoInspSumOverallTemplateByEfCore(KpiRequest request)
        {
            var mapRequest = new KpiAdeoTemplateRequest();
            try
            {
                if (request != null)
                {
                    //Fetch only final random inspection and re inspection
                    request.ServiceTypeIdLst = new List<int>();
                    request.ServiceTypeIdLst.Add((int)InspectionServiceTypeEnum.FinalRandomInspection);
                    request.ServiceTypeIdLst.Add((int)InspectionServiceTypeEnum.FinalRandomReInspection);

                    var bookingIdList = GetBookingIdAsQueryable(request);

                    if (bookingIdList == null || !bookingIdList.Any())
                    {
                        return null;
                    }

                    mapRequest.RemarksData = await _repo.GetFbProblematicRemarksEfCore(bookingIdList);
                    mapRequest.FactoryProductData = await _repo.GetAdeoFollowUpProductDataEfCore(bookingIdList);
                    mapRequest.ServiceTypeData = await _inspRepo.GetServiceTypeByBookingQuery(bookingIdList);

                    mapRequest.QuotationData = await _repo.GetQuotationManDay(bookingIdList);
                    //Fetch the customer Department for the Dept code
                    mapRequest.DeptList = await _repo.GetKPIBookingDepartmentDataEfCore(bookingIdList);

                    //Create a list of SupID and FactoryID to fetch the customer codes in one DB call
                    var customerIdList = mapRequest.FactoryProductData.Select(x => x.CustomerId).Distinct().ToList();
                    var supIds = new List<int>();
                    supIds.AddRange(mapRequest.FactoryProductData.Select(x => x.SupplierId).Distinct());
                    mapRequest.SupCode = await _supplierRepository.GetSupplierCode(customerIdList, supIds);

                    //get invoice number
                    mapRequest.InvoiceData = await _repo.GetInvoiceNoByBooking(bookingIdList);

                    var result = KpiCustomMap.MapAdeoInspSumOverall(mapRequest);

                    if (result == null || !result.Any())
                    {
                        return null;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        //Adeo Customer KPI Template details for each product in the selected date range
        public async Task<List<AdeoFollowUpTemplate>> ExportAdeoInspFollowUpTemplateByEfCore(KpiRequest request)
        {
            KpiAdeoTemplateRequest mapRequest = new KpiAdeoTemplateRequest();
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    var requestFilters = GetBookingIdAsQueryable(request);

                    if (requestFilters == null || !requestFilters.Any())
                    {
                        return null;
                    }

                    mapRequest.BookingData = await _repo.GetBookingItemsbyBookingIdAsQuery(requestFilters);
                    mapRequest.ProductData = await _repo.GetProductListByBooking(requestFilters);
                    mapRequest.ProductData.AddRange(await _repo.GetContainerListByBooking(requestFilters));
                    mapRequest.PoDetails = await _repo.GetBookingPoDetails(requestFilters);

                    mapRequest.FactoryLocation = await _inspRepo.GetFactorycountryByBookingQuery(requestFilters);

                    //Create a list of SupID and FactoryID to fetch the customer codes in one DB call
                    var customerIdList = mapRequest.BookingData.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();
                    var supIds = mapRequest.BookingData.Select(x => x.SupplierId.GetValueOrDefault()).Distinct().ToList();
                    mapRequest.SupCode = await _supplierRepository.GetSupplierCode(customerIdList, supIds);

                    mapRequest.CusDecisionData = await _repo.GetDecisionDateByReport(requestFilters);

                    mapRequest.IcData = await _repo.GetICByReport(requestFilters);

                    mapRequest.HolidayList = await _repo.GetHolidaysByDateRange(request.FromDate.ToDateTime(), request.ToDate.ToDateTime());

                    var result = KpiCustomMap.MapAdeoInspFollowUp(mapRequest);

                    if (result == null || !result.Any())
                    {
                        return null;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }


        //Adeo Customer KPI Template details for each product for all the failed reports
        public async Task<List<AdeoFailedPoTemplate>> ExportAdeoFailedPoTemplateByEfCore(KpiRequest request)
        {
            KpiAdeoTemplateRequest mapRequest = new KpiAdeoTemplateRequest();
            try
            {
                if (request != null)
                {
                    var bookingIdList = GetBookingIdAsQueryable(request);

                    if (bookingIdList == null || !bookingIdList.Any())
                    {
                        return null;
                    }
                    mapRequest.BookingData = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingIdList);
                    mapRequest.RemarksData = await _repo.GetFbProblematicRemarksEfCore(bookingIdList);
                    mapRequest.FactoryProductData = await _repo.GetAdeoFailedProductDataByEfCore(bookingIdList);
                    mapRequest.FactoryProductData.AddRange(await _repo.GetAdeoFailedContainerDataByEfCore(bookingIdList));
                    mapRequest.PoDetails = await _repo.GetBookingPoDetails(bookingIdList);

                    //Fetch the customer Department for the Dept code
                    mapRequest.DeptList = await _repo.GetKPIBookingDepartmentDataEfCore(bookingIdList);

                    //Create a list of SupID and FactoryID to fetch the customer codes in one DB call
                    var customerIdList = mapRequest.FactoryProductData.Select(x => x.CustomerId).Distinct().ToList();
                    var supIds = new List<int>();
                    supIds.AddRange(mapRequest.FactoryProductData.Select(x => x.SupplierId).Distinct());
                    mapRequest.SupCode = await _supplierRepository.GetSupplierCode(customerIdList, supIds);

                    //get the resinpection Ids for failed bookings
                    mapRequest.ReinspectionData = await _repo.GetReinspectionBookingByBookingQuey(bookingIdList);

                    var result = KpiCustomMap.MapAdeoFailedData(mapRequest);

                    if (result == null || !result.Any())
                    {
                        return null;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        //Stokomani Customer KPI Template details for each report
        public async Task<ExportResult> ExportQcPerformanceBreakdownTemplate(KpiRequest request)
        {
            ExportResult result = new ExportResult();
            var parameterList = new KPIMapParameters();
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    parameterList.BookingInvoice = await GetBookingDetails(request);

                    if (parameterList.BookingInvoice.BookingItems.Count() == 0)
                        return result = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);
                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        parameterList.ServiceTypeList = parameterList.ServiceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = parameterList.ServiceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    //fetch the PO details for the bookings
                    parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the Product List for failed reports
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);

                    //Create a list of SupID and FactoryID to fetch the customer codes in one DB call
                    var customerIdList = parameterList.BookingInvoice.BookingItems.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();
                    var supIds = new List<int>();
                    supIds.AddRange(parameterList.BookingInvoice.BookingItems.Select(x => x.SupplierId.GetValueOrDefault()).Distinct());
                    parameterList.SupplierCode = await _supplierRepository.GetSupplierCode(customerIdList, supIds);

                    //factory country required for pending quotation 
                    parameterList.FactoryLocation = await _inspRepo.GetFactorycountryId(bookingIds);

                    //get the quotation details by booking Ids
                    parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingIds);

                    //fetch Merchandiser
                    parameterList.MerchandiserList = await _repo.GetMerchandiserByBooking(bookingIds);

                    //fetch Qcnames from FB_Report_QCDetails
                    var reportIdlist = parameterList.ProductList.Select(x => x.ReportId.GetValueOrDefault()).Distinct().ToList();
                    parameterList.QcNames = await _repo.GetFbQcNames(reportIdlist);

                    result.Data = KpiCustomMap.BookingSummaryMapByReport(parameterList);
                }
            }
            catch (Exception ex)
            {
                return result = null;
            }
            return result;
        }

        //Stokomani Customer KPI Template details for each report
        public async Task<ExportResult> ExportMonthlyOrderStatementTemplate(KpiRequest request)
        {
            ExportResult result = new ExportResult();
            var parameterList = new KPIMapParameters();
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    parameterList.BookingInvoice = await GetBookingDetails(request);

                    if (parameterList.BookingInvoice.BookingItems.Count() == 0)
                        return result = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);
                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        parameterList.ServiceTypeList = parameterList.ServiceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = parameterList.ServiceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    //fetch the PO details for the bookings
                    parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the Product List for failed reports
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);

                    //Create a list of SupID and FactoryID to fetch the customer codes in one DB call
                    var customerIdList = parameterList.BookingInvoice.BookingItems.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();
                    var supIds = new List<int>();
                    supIds.AddRange(parameterList.BookingInvoice.BookingItems.Select(x => x.SupplierId.GetValueOrDefault()).Distinct());
                    parameterList.SupplierCode = await _supplierRepository.GetSupplierCode(customerIdList, supIds);

                    //factory country required for pending quotation 
                    parameterList.FactoryLocation = await _inspRepo.GetFactorycountryId(bookingIds);

                    //get the quotation details by booking Ids
                    parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingIds);

                    //fetch Qcnames from FB_Report_QCDetails
                    var reportIdlist = parameterList.ProductList.Select(x => x.ReportId.GetValueOrDefault()).Distinct().ToList();
                    parameterList.QcNames = await _repo.GetFbQcNames(reportIdlist);

                    //Get the shipment quantity from FB_Report_Qunatity table
                    parameterList.FbBookingQuantity = await _repo.GetInspectionQuantities(bookingIds);

                    //Get the Supplier and Factory contact names
                    parameterList.ContactData = await _repo.GetContactNames(bookingIds);

                    result.Data = KpiCustomMap.BookingSummaryMapByReport(parameterList);
                }
            }
            catch (Exception ex)
            {
                return result = null;
            }
            return result;
        }

        //Report based on remarks
        public async Task<ExportResult> ExportAdeoSummaryRemarksTemplate(KpiRequest request)
        {
            ExportResult result = new ExportResult();
            var parameterList = new KPIMapParameters();
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    parameterList.BookingInvoice = await GetBookingDetails(request);

                    if (parameterList.BookingInvoice.BookingItems.Count() == 0)
                        return result = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);

                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        parameterList.ServiceTypeList = parameterList.ServiceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = parameterList.ServiceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    //fetch the PO details for the bookings
                    parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the Product List for all the bookings
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);

                    //Fetch the customer Department for the Dept code
                    parameterList.CustomerDept = await _deptRepo.GetCustomerDepartmentsbyBooking(bookingIds);

                    //fetch Customer contacts
                    var reportIdlist = parameterList.ProductList.Select(x => x.ReportId.GetValueOrDefault()).Distinct().ToList();
                    parameterList.CustomerContactData = await _repo.GetCustomerContactNames(bookingIds);

                    //get the quotation details by booking Ids
                    parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingIds);

                    //Create a list of SupID and FactoryID to fetch the customer codes in one DB call
                    var customerIdList = parameterList.BookingInvoice.BookingItems.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();
                    var supIds = new List<int>();
                    supIds.AddRange(parameterList.BookingInvoice.BookingItems.Select(x => x.SupplierId.GetValueOrDefault()).Distinct());
                    parameterList.SupplierCode = await _supplierRepository.GetSupplierCode(customerIdList, supIds);

                    //Get Problematic remarks for reports
                    parameterList.ReportRemarks = await _repo.GetFbProblematicRemarks(reportIdlist);

                    //get problematic remarks list by passing reportid list
                    parameterList.FBReportInspSubSummaryList = await _repo.GetFBInspSummaryResultbyReport(reportIdlist);

                    //get fb report comments list
                    parameterList.FbReportComments = await _repo.GetFBReportComments(reportIdlist);

                    result.Data = KpiCustomMap.BookingMapByProduct(parameterList);
                }
            }
            catch (Exception ex)
            {
                return result = null;
            }
            return result;
        }
        //ECI Customer KPI Template details of booking, fb details.
        public async Task<ExportResult> ExportECIRemarksTemplate(KpiRequest request)
        {
            var result = new ExportResult();
            var mapParameter = new KPIMapParameters();

            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    mapParameter.BookingInvoice = await GetBookingDetails(request);

                    if (mapParameter.BookingInvoice.BookingItems.Count() == 0)
                        return result = null;
                    //Pick only the booking Ids to fetch Products
                    var bookingIds = mapParameter.BookingInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    mapParameter.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);

                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        bookingIds = mapParameter.ServiceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId))
                                                    .Select(x => x.InspectionId).ToList();
                    }

                    //po details
                    mapParameter.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //get po ids from po details list
                    var poIdList = mapParameter.PoDetails.Select(x => x.PoTransactionId);

                    //Fetch the Product List for all the bookings
                    mapParameter.ProductList = await _repo.GetProductListByBooking(bookingIds);

                    //get fb report id list from product list
                    var fbReportIdList = mapParameter.ProductList.Where(x => x.ReportId > 0).Select(x => x.ReportId.GetValueOrDefault()).ToList();

                    //fetch the dynamic fields for the customer and booking
                    var dfBookingFieldsResponse = await _dynamicFieldManager.GetBookingDFDataByBookingIds(bookingIds);

                    //check dynamic fields if it success
                    if (dfBookingFieldsResponse.Result == InspectionBookingDFDataResult.Success)
                    {
                        mapParameter.BookingDFDataList = dfBookingFieldsResponse.bookingDFDataList;
                    }

                    //factory country details
                    mapParameter.FactoryLocation = await _inspRepo.GetFactorycountryId(bookingIds);

                    //Fetch the customer buyer name
                    mapParameter.CustomerBuyerList = await _repo.GetCustomerBuyerbyBooking(bookingIds);

                    //Get Problematic remarks for reports
                    mapParameter.ReportRemarks = await _repo.GetFbProblematicRemarks(fbReportIdList);

                    //get defect list by passing po id list
                    mapParameter.FBReportDefectsList = await _repo.GetFBDefects(poIdList);

                    mapParameter.FBReportInspSubSummaryList = await _repo.GetFBInspSummaryResult(fbReportIdList);

                    mapParameter.ContainerItems = await _repo.GetContainerItemsByReportId(bookingIds);

                    result = ECIRemarksTemplateCheck(mapParameter);

                }

            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        //loop the product level then by remark level for eci templates
        private ExportResult ECIRemarksTemplateCheck(KPIMapParameters parameters)
        {
            ExportResult response = new ExportResult();
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            ECIRemarkParameters eciRemarkParameters = new ECIRemarkParameters();

            int prevBookingId = 0;
            int prevProductId = 0;
            int? prevReportId = 0;

            var productList = parameters?.ProductList?.OrderBy(x => x.BookingId).ThenBy(x => x.ProductId).ThenBy(x => x.ReportId).ToList();

            foreach (var item in productList)
            {
                if (item != null && item.ProductName != null)
                {
                    eciRemarkParameters.TotalReports = 0;
                    eciRemarkParameters.ProductItem = item;

                    eciRemarkParameters.BookingDetails = parameters?.BookingInvoice?.BookingItems?.Where(x => x.BookingId == item.BookingId).FirstOrDefault();

                    var fbRemarkList = parameters?.ReportRemarks?.Where(x => x.ReportId == item.ReportId && x.ProductId == item.ProductId).ToList();

                    var fbCommonRemarkList = parameters?.ReportRemarks?.Where(x => x.ReportId == item.ReportId && x.ProductId == null).Select(x => x.Remarks).Distinct().ToList();

                    //logic to display quotation manday only on the first row if 1 booking has multiple reports
                    int servicetypeid = parameters?.ServiceTypeList?.Where(x => x.InspectionId == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.serviceTypeId).FirstOrDefault() ?? 0;

                    if (prevBookingId != item.BookingId)
                    {
                        eciRemarkParameters.TotalReports = servicetypeid != (int)InspectionServiceTypeEnum.Container ? parameters.ProductList?.Where(x => x.BookingId == item.BookingId && x.CombineProductId == 0).Count() +
                                                                             parameters.ProductList?.Where(x => x.BookingId == item.BookingId && x.CombineProductId != 0).Select(x => x.CombineProductId).Distinct().Count() :
                                                                             parameters?.ContainerItems?.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().Count() ?? 0;
                        prevBookingId = item.BookingId;
                    }

                    if (fbRemarkList.Any())
                    {
                        eciRemarkParameters.RemarkSerialNo = 0;

                        //remark serial number
                        if (prevProductId != item.ProductId)
                        {
                            eciRemarkParameters.RemarkSerialNo = 0;
                            prevProductId = item.ProductId;
                        }
                        foreach (var itemRemark in fbRemarkList)
                        {
                            eciRemarkParameters.RemarkSerialNo = eciRemarkParameters.RemarkSerialNo + 1;

                            //logic to display the report remarks only in the first product 
                            if (prevReportId != item.ReportId)
                            {
                                eciRemarkParameters.Remarks = fbCommonRemarkList.Any() ? string.Join(", ", fbCommonRemarkList) + ", " + itemRemark.Remarks : itemRemark.Remarks;
                                prevReportId = item.ReportId;
                            }
                            else
                            {
                                eciRemarkParameters.Remarks = itemRemark.Remarks;
                            }
                            eciRemarkParameters.RemarkResult = itemRemark.Result;

                            result.Add(KpiCustomMap.ECIRemarksTemplate(parameters, eciRemarkParameters));
                        }
                    }
                    else
                    {
                        //empty the remarks data
                        eciRemarkParameters.RemarkResult = string.Empty;
                        eciRemarkParameters.RemarkSerialNo = 0;
                        eciRemarkParameters.Remarks = string.Empty;

                        result.Add(KpiCustomMap.ECIRemarksTemplate(parameters, eciRemarkParameters));

                    }
                }
            }
            response.Data = result.OrderBy(x => x.BookingNo).ToList();
            return response;
        }

        //Method to fetch the details for insp defect template - loop product and then by defect
        public async Task<ExportResult> ExportInspDefectSummaryTemplate(KpiRequest request)
        {
            ExportResult result = new ExportResult();
            var parameterList = new KPIMapParameters();
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    parameterList.BookingInvoice = await GetBookingDetails(request);

                    if (parameterList.BookingInvoice.BookingItems.Count() == 0)
                        return result = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);

                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        parameterList.ServiceTypeList = parameterList.ServiceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = parameterList.ServiceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    //fetch the PO details for the bookings
                    parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the customer buyer name
                    parameterList.CustomerBuyerList = await _repo.GetCustomerBuyerbyBooking(bookingIds);

                    //Fetch the Product List for all the bookings
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);
                    parameterList.ProductList.AddRange(await _repo.GetContainerListByBooking(bookingIds));

                    //Fetch the customer Department for the Dept code
                    parameterList.CustomerDept = await _deptRepo.GetCustomerDepartmentsbyBooking(bookingIds);

                    //fetch Customer contacts
                    var reportIdlist = parameterList.ProductList.Select(x => x.ReportId.GetValueOrDefault()).Distinct().ToList();
                    parameterList.CustomerContactData = await _repo.GetCustomerContactNames(bookingIds);

                    //factory country required for pending quotation 
                    parameterList.FactoryLocation = await _inspRepo.GetFactorycountryId(bookingIds);

                    //get po ids from po details list
                    var poIdList = parameterList.PoDetails.Select(x => x.PoTransactionId);

                    //get defect list by passing po id list
                    parameterList.FBReportDefectsList = await _repo.GetFBDefects(poIdList);

                    result = DefectsTemplate(parameterList);
                }
            }
            catch (Exception ex)
            {
                return result = null;
            }
            return result;
        }


        /// <summary>
        /// Export inspection defect summary template by ef core logic
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectExportResult> ExportInspDefectSummaryTemplateByEfCore(KpiRequest request)
        {
            DefectExportResult result = new DefectExportResult();
            try
            {
                if (request != null)
                {
                    // fetch only the booking id list
                    var requestFilters = GetBookingIdAsQueryable(request);

                    // get defect list 
                    var defectList = await _repo.GetDefectSummaryDataEfCore(requestFilters);
                    var inspectionList = await _repo.GetKpiDefectInspectionData(requestFilters);
                    var poList = await _repo.GetKpiDefectPurchaseOrderData(requestFilters);
                    var buyerList = await _repo.GetKPIBookingBuyerDataEfCore(requestFilters);
                    var departmentList = await _repo.GetKPIBookingDepartmentDataEfCore(requestFilters);
                    var contactList = await _repo.GetKPIBookingCustomerContactsDataEfCore(requestFilters);

                    // map defect data with export template item
                    result = new DefectExportResult()
                    {
                        Data = new List<KpiDefectData>(),
                        BookingBuyer = buyerList,
                        BookingCustomerContacts = contactList,
                        BookingDepartments = departmentList
                    };

                    var groupedDefectsbyReport = defectList.OrderBy(x => x.BookingNo).GroupBy(x => x.ReportId).ToList();

                    foreach (var defectRow in groupedDefectsbyReport)
                    {
                        var inspection = inspectionList.FirstOrDefault(x => x.ReportId == defectRow.Key);
                        int defectId = 0;
                        foreach (var x in defectRow)
                        {
                            defectId++;
                            var po = poList.FirstOrDefault(y => y.PoId == x.PoId);
                            result.Data.Add(new KpiDefectData()
                            {
                                BookingNo = x.BookingNo,
                                BuyerName = string.Join(',', buyerList.Where(y => y.BookingId == x.BookingNo).Select(y => y.BuyerName).Distinct().ToArray()),
                                DeptCode = string.Join(',', departmentList.Where(y => y.BookingId == x.BookingNo).Select(y => y.DepartmentName).Distinct().ToArray()),
                                CustomerContact = contactList.Where(y => y.BookingId == x.BookingNo).Select(y => y.ContactName).FirstOrDefault(),
                                CustomerBookingNo = inspection.CustomerBookingNo,
                                CustomerName = inspection.CustomerName,
                                CollectionName = inspection.CollectionName,
                                Office = inspection.Office,
                                SupplierName = inspection.SupplierName,
                                FactoryName = inspection.FactoryName,
                                FactoryCountry = inspection.FactoryCountry,
                                ServiceTypeName = inspection.ServiceTypeName,
                                BookingStatus = inspection.BookingStatus,
                                InspectionStartDate = inspection.InspectionStartDate,
                                InspectionEndDate = inspection.InspectionEndDate,
                                Month = inspection.Month,
                                Year = inspection.Year,
                                PONumber = po?.PONumber,
                                ProductName = po?.ProductName,
                                ProductDescription = po?.ProductDescription,
                                FactoryRef = po?.FactoryRef,
                                ReportResult = x.ReportResult,
                                DefectId = defectId,
                                DefectDesc = x.DefectDesc,
                                CriticalDefect = x.CriticalDefect,
                                MajorDefect = x.MajorDefect,
                                MinorDefect = x.MinorDefect,
                                DefectCategory = x.DefectCategory,
                                ProductCategory = po?.ProductCategory,
                                ProductSubCategory = po?.ProductSubCategory,
                                ProductSubCategory2 = po?.ProductSubCategory2
                            });
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return result = null;
            }
        }



        //loop the product level then by defect level for insp defect templates
        private ExportResult DefectsTemplate(KPIMapParameters parameters)
        {
            ExportResult response = new ExportResult();
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            DefectParameters defectParameters = new DefectParameters();

            var productList = parameters?.ProductList?.OrderBy(x => x.BookingId).ThenBy(x => x.ProductId).ThenBy(x => x.ReportId).ToList();

            foreach (var item in productList)
            {
                if (item != null && item.ProductName != null)
                {
                    defectParameters.ProductItem = item;

                    defectParameters.BookingDetails = parameters?.BookingInvoice?.BookingItems?.Where(x => x.BookingId == item.BookingId).FirstOrDefault();
                    int servicetypeid = parameters?.ServiceTypeList?.Where(x => x.InspectionId == item.BookingId).Select(x => x.serviceTypeId).FirstOrDefault() ?? 0;
                    var fbDefects = servicetypeid != (int)InspectionServiceTypeEnum.Container ? parameters?.FBReportDefectsList?.Where(x => x.FBReportDetailId == item.ReportId && x.ProductId == item.ProductId).ToList() :
                        parameters?.FBReportDefectsList?.Where(x => x.FBReportDetailId == item.ReportId).ToList();

                    if (fbDefects.Any())
                    {
                        defectParameters.DefectSerialNo = 0;

                        foreach (var defect in fbDefects)
                        {
                            defectParameters.DefectSerialNo = defectParameters.DefectSerialNo + 1;
                            defectParameters.DefectData = defect;

                            result.Add(KpiCustomMap.BookingMapByDefect(parameters, defectParameters));
                        }
                    }
                }
            }
            response.Data = result.OrderBy(x => x.BookingNo).ToList();
            return response;
        }

        // Method to fetch the details for insp result template - loop based on report
        public async Task<DataTable> ExportInspResultSummaryTemplate(KpiRequest request)
        {

            var parameterList = new KPIReportResultMapParameters();
            try
            {
                if (request != null)
                {

                    var bookingRequestFilters = GetBookingIdAsQueryable(request);

                    parameterList.BookingItems = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingRequestFilters);

                    parameterList.CustomerDept = await _repo.GetKPIBookingDepartmentDataEfCore(bookingRequestFilters);

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceTypeByBookingQuery(bookingRequestFilters);

                    //fetch the PO details for the bookings
                    parameterList.PoDetails = await _repo.GetBookingPoDetails(bookingRequestFilters);

                    //Fetch the Product List for all the bookings
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingRequestFilters);

                    parameterList.ProductList.AddRange(await _repo.GetContainerListByBooking(bookingRequestFilters));

                    //fetch Customer contacts
                    var reportIdlist = parameterList.ProductList.Select(x => x.ReportId.GetValueOrDefault()).Distinct().ToList();

                    parameterList.CustomerContactData = await _repo.GetKPIBookingCustomerContactsDataEfCore(bookingRequestFilters);

                    //get the quotation details by booking Ids
                    parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingRequestFilters);

                    //factory country required for pending quotation 
                    parameterList.FactoryLocation = await _inspRepo.GetFactorycountryByBookingQuery(bookingRequestFilters);

                    //Fetch the customer buyer name
                    parameterList.CustomerBuyerList = await _repo.GetKPIBookingBuyerDataEfCore(bookingRequestFilters);

                    //fetch Merchandiser
                    parameterList.MerchandiserList = await _repo.GetMerchandiserByBooking(bookingRequestFilters);

                    //Fetch QC Names
                    parameterList.QcNames = await _repo.GetFbQcNames(reportIdlist);

                    //get invoice number
                    var invoiceData = await _repo.GetInvoiceNoByBooking(bookingRequestFilters);

                    parameterList.InvoiceBookingData = invoiceData.ConvertAll(x => new InvoiceBookingData
                    {
                        InvoiceNo = x.InvoiceNo,
                        InvoiceDate = x.InvoiceDate?.ToString(StandardDateFormat),
                        InvoieRemarks = x.InvoiceRemarks,
                        BookingId = x.BookingId.GetValueOrDefault(),
                        InspFees = x.InspFees.GetValueOrDefault(),
                        TravelFee = x.TravelFee.GetValueOrDefault(),
                        HotelFee = x.HotelFee.GetValueOrDefault(),
                        InvoiceTotal = x.TotalFee.GetValueOrDefault(),
                        InvoiceCurrency = x.CurrencyName,
                        OtherExpense = x.OtherFee.GetValueOrDefault(),
                        Discount = x.Discount.GetValueOrDefault(),
                        BilledTo = x.BilledTo
                    });


                    // get defect list by passing po id list
                    parameterList.FBReportInspSubSummaryList = await _repo.GetFBInspSummaryResultbyReport(reportIdlist);

                    parameterList.CustomerDecisionData = await _repo.GetDecisionDateByReportId(reportIdlist);

                    return ResultTemplate(parameterList);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        //loop the report level for insp result templates
        private DataTable ResultTemplate(KPIReportResultMapParameters parameters)
        {
            try
            {
                var response = new ExportReportResult();
                List<ReportResultTemplateItem> result = new List<ReportResultTemplateItem>();
                ResultParameters resultParameters = new ResultParameters();
                int bookingId = 0;

                var reportIdList = parameters?.ProductList?.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().ToList();

                if (reportIdList != null && reportIdList.Any())
                {
                    foreach (var reportId in reportIdList)
                    {
                        if (reportId != null)
                        {
                            resultParameters.ProductItem = parameters.ProductList?.Where(x => x.ReportId == reportId).ToList();

                            bookingId = resultParameters.ProductItem.Select(x => x.BookingId).FirstOrDefault();

                            resultParameters.BookingDetails = parameters?.BookingItems?.FirstOrDefault(x => x.BookingId == bookingId);

                            resultParameters.QuotationMandayDetails = parameters?.QuotationDetails?.MandayList?.FirstOrDefault(x => x.BookingId == bookingId);

                            result.Add(KpiCustomMap.BookingReportResultMap(parameters, resultParameters, reportId));

                        }
                    }
                }

                response.Data = result.OrderBy(x => x.BookingNo).ToList();
                response.ReportResultDataList = parameters?.FBReportInspSubSummaryList?
                               .Select(y => new FbReportInspectionResult
                               {
                                   ReportId = y.FBReportId,
                                   Name = y.Name,
                                   Result = y.Result
                               }).ToList();

                response.ResultName = parameters?.FBReportInspSubSummaryList?.
                    Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).Distinct().ToList();

                //convert the list to datatable
                var dtBookingTable = _helper.ConvertToDataTableWithCaption(response.Data.ToList());
                var removedColumnList = new List<string>() { "ReportId" };
                //map the booking dynamic fields with the datatable
                MapBookingReportResult(dtBookingTable, response.ResultName, response.ReportResultDataList, removedColumnList);

                return dtBookingTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable MapBookingReportResult(DataTable dtBookingTable, List<string> columns, List<FbReportInspectionResult> dataList, List<string> removedColumns)
        {
            if (columns != null && columns.Any())
            {

                //add the dynamic headers
                foreach (var efHeader in columns)
                {
                    dtBookingTable.Columns.Add(efHeader, typeof(string));
                }

                foreach (DataRow row in dtBookingTable.Rows)
                {
                    if (string.IsNullOrWhiteSpace(row["ReportId"].ToString()))
                        continue;

                    var reportResList = dataList.Where
                            (x => x.ReportId == Convert.ToInt32(row["ReportId"].ToString()));

                    if (reportResList.Any())
                    {
                        foreach (var efHeader in columns)
                        {
                            row[efHeader] = reportResList.FirstOrDefault
                               (x => x.Name == efHeader)?.Result;
                        }
                    }
                }
                // removed columns 
                foreach (var column in removedColumns)
                {
                    dtBookingTable.Columns.Remove(column);
                }
            }

            return dtBookingTable;
        }


        public DataTable MapAuditCheckpointResult(DataTable dtBookingTable, List<string> columns, List<FbReportCheckpointResult> dataList, List<string> removedColumns)
        {
            if (columns != null && columns.Any())
            {

                //add the dynamic headers
                foreach (var efHeader in columns)
                {
                    dtBookingTable.Columns.Add(efHeader, typeof(string));
                }

                foreach (DataRow row in dtBookingTable.Rows)
                {
                    if (string.IsNullOrWhiteSpace(row["AuditId"].ToString()))
                        continue;

                    var reportResList = dataList.Where
                            (x => x.AuditId == Convert.ToInt32(row["AuditId"].ToString()));

                    if (reportResList.Any())
                    {
                        foreach (var efHeader in columns)
                        {
                            row[efHeader] = reportResList.FirstOrDefault
                               (x => x.Name == efHeader)?.Result;
                        }
                    }
                }
                if (removedColumns != null)
                {
                    // removed columns 
                    foreach (var column in removedColumns)
                    {
                        dtBookingTable.Columns.Remove(column);
                    }
                }

            }

            return dtBookingTable;
        }

        //Template based on remarks
        public async Task<RemarksExportResult> ExportInspRemarksSummaryTemplate(KpiRequest request)
        {
            RemarksExportResult result = new RemarksExportResult();
            try
            {
                if (request != null)
                {

                    var bookingRequestFilters = GetBookingIdAsQueryable(request);
                    var remarksData = await _repo.GetReportRemarksDataEfCore(bookingRequestFilters);
                    var poDataList = await _repo.GetBookingPoDetails(bookingRequestFilters);
                    var buyerList = await _repo.GetKPIBookingBuyerDataEfCore(bookingRequestFilters);
                    var departmentList = await _repo.GetKPIBookingDepartmentDataEfCore(bookingRequestFilters);
                    var contactList = await _repo.GetKPIBookingCustomerContactsDataEfCore(bookingRequestFilters);

                    //Get the service Type for the bookings
                    var ServiceTypeList = await _inspRepo.GetServiceTypeByBookingQuery(bookingRequestFilters);

                    //Fetch the Product List for all the bookings
                    var ProductList = await _repo.GetProductListByBooking(bookingRequestFilters);

                    if (ServiceTypeList != null && ServiceTypeList.Any(x => x.serviceTypeId == (int)InspectionServiceTypeEnum.Container))
                    {
                        ProductList.AddRange(await _repo.GetContainerListByBooking(bookingRequestFilters));
                    }

                    // map defect data with export template item
                    result = new RemarksExportResult()
                    {
                        Data = new List<KpiReportRemarksTemplate>(),
                        BookingCustomerContacts = contactList,
                        BookingDepartments = departmentList,
                        PoList = poDataList
                    };

                    var groupedReportRemarks = remarksData.GroupBy(x => x.ReportId).ToList();

                    foreach (var reportRemarks in groupedReportRemarks)
                    {
                        int remarksNumber = 0;

                        foreach (var x in reportRemarks)
                        {

                            if (!string.IsNullOrEmpty(x.ReportRemarks) && !string.IsNullOrEmpty(x.FBRemarkResult) && x.FBRemarkResult.ToLower() != FBRemarkResult)
                            {
                                remarksNumber++;

                                // Add product specific filters

                                var reportProducts = ProductList.Where(z => z.ReportId == x.ReportId).ToList();

                                result.Data.Add(new KpiReportRemarksTemplate()
                                {
                                    BookingNo = x.BookingNo,
                                    CustomerBookingNo = x.CustomerBookingNo,
                                    CustomerName = x.CustomerName,
                                    BuyerName = string.Join(',', buyerList.Where(y => y.BookingId == x.BookingNo).Select(y => y.BuyerName).Distinct().ToArray()),
                                    CustomerContact = contactList.Where(y => y.BookingId == x.BookingNo).Select(y => y.ContactName).FirstOrDefault(),
                                    DeptCode = string.Join(',', departmentList.Where(y => y.BookingId == x.BookingNo).Select(y => y.DepartmentName).Distinct().ToArray()),
                                    CollectionName = x.CollectionName,
                                    Office = x.Office,
                                    SupplierName = x.SupplierName,
                                    FactoryName = x.FactoryName,
                                    FactoryCountry = x.FactoryCountry,
                                    ServiceTypeName = ServiceTypeList.Where(y => y.InspectionId == x.BookingNo).Select(y => y.serviceTypeName).FirstOrDefault(),
                                    BookingStatus = x.BookingStatus,
                                    InspectionStartDate = x.InspectionStartDate,
                                    InspectionEndDate = x.InspectionEndDate,
                                    Month = x.Month,
                                    Year = x.Year,
                                    PONumber = string.Join(',', poDataList.Where(y => reportProducts.Select(z => z.Id).Contains(y.ProductRefId)).Select(z => z.PoNumber).Distinct().ToArray()),
                                    ProductName = string.Join(',', reportProducts.Select(z => z.ProductName).Distinct().ToArray()),
                                    ProductDescription = string.Join(',', reportProducts.Select(z => z.ProductDescription).Distinct().ToArray()),
                                    FactoryRef = string.Join(',', reportProducts.Select(z => z.FactoryReference).Distinct().ToArray()),
                                    CombinedWith = reportProducts.Select(z => z.CombineProductId).FirstOrDefault() == 0 ? null :
                                            reportProducts.Select(z => z.CombineProductId).FirstOrDefault(),
                                    ReportResult = x.ReportResult,
                                    ReportRemarks = x.ReportRemarks,
                                    FBRemarkNumber = remarksNumber,
                                    RemarkCategory = x.RemarkCategory,
                                    FBRemarkResult = x.FBRemarkResult,
                                    RemarkSubCategory = x.RemarkSubCategory,
                                    RemarkSubCategory2 = x.RemarkSubCategory2,
                                    BillPaidBy = x.BillPaidBy == (int)QuotationPaidBy.customer ? QuotationPaidBy.customer.ToString() : QuotationPaidBy.supplier.ToString(),
                                    CustomerRemarkCodeReference = x.CustomerRemarkCodeReference,
                                    ProductCategory = string.Join(',', reportProducts.Select(z => z.ProductCategory).Distinct().ToArray()),
                                    ProductSubCategory = string.Join(',', reportProducts.Select(z => z.ProductSubCategory).Distinct().ToArray()),
                                    ProductSubCategory2 = string.Join(',', reportProducts.Select(z => z.ProductSubCategory2).Distinct().ToArray())
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return result = null;
            }
            return result;
        }


        //Template based on Product
        public async Task<ExportResult> ExportLiverpoolTemplate(KpiRequest request)
        {
            ExportResult result = new ExportResult();
            var parameterList = new KPIMapParameters();

            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    parameterList.BookingInvoice = await GetBookingDetails(request);

                    if (parameterList.BookingInvoice.BookingItems.Count() == 0)
                        return result = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);

                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        parameterList.ServiceTypeList = parameterList.ServiceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = parameterList.ServiceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    //fetch the PO details for the bookings
                    parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the Product List for all the bookings
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);

                    //Fetch the customer Department for the Dept code
                    parameterList.CustomerDept = await _deptRepo.GetCustomerDepartmentsbyBooking(bookingIds);

                    //fetch Customer contacts
                    var reportIdlist = parameterList.ProductList.Select(x => x.ReportId.GetValueOrDefault()).Distinct().ToList();
                    parameterList.CustomerContactData = await _repo.GetCustomerContactNames(bookingIds);

                    //get the quotation details by booking Ids
                    parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingIds);

                    //factory country required for pending quotation 
                    parameterList.FactoryLocation = await _inspRepo.GetFactorycountryId(bookingIds);

                    //Fetch the customer buyer name
                    parameterList.CustomerBuyerList = await _repo.GetCustomerBuyerbyBooking(bookingIds);

                    //Fetch customer brands
                    parameterList.CustomerBrandList = await _repo.GetCustomerBrandbyBooking(bookingIds);

                    if (string.IsNullOrEmpty(request.InvoiceNo))
                    {
                        var invoiceData = await _repo.GetInvoiceNoByBooking(bookingIds);
                        parameterList.BookingInvoice.InvoiceBookingData = invoiceData.ConvertAll(x => new InvoiceBookingData
                        {
                            InvoiceNo = x.InvoiceNo,
                            InvoiceDate = x.InvoiceDate?.ToString(StandardDateFormat),
                            InvoieRemarks = x.InvoiceRemarks,
                            BookingId = x.BookingId.GetValueOrDefault()
                        });
                    }
                    result.Data = KpiCustomMap.BookingMapAdeo(parameterList);
                }
            }
            catch (Exception ex)
            {
                return result = null;
            }
            return result;
        }

        //Template based on booking
        public async Task<List<ExpenseTemplateItem>> ExportInspExpenseSummaryTemplate(KpiRequest request)
        {
            List<ExpenseTemplateItem> result = new List<ExpenseTemplateItem>();
            var parameterList = new KPIExpenseMapParameters();
            KpiExpenseResultParameters resultParameters = new KpiExpenseResultParameters();

            try
            {
                if (request != null)
                {
                    var bookingRequestFilters = GetBookingIdAsQueryable(request);
                    parameterList.BookingItems = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingRequestFilters);
                    parameterList.CustomerDept = await _repo.GetKPIBookingDepartmentDataEfCore(bookingRequestFilters);

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceTypeByBookingQuery(bookingRequestFilters);

                    //fetch the PO details for the bookings
                    parameterList.PoDetails = await _repo.GetBookingPoDetails(bookingRequestFilters);

                    //Fetch the Product List for all the bookings
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingRequestFilters);

                    //fetch Customer contacts
                    var reportIdlist = parameterList.ProductList.Select(x => x.ReportId.GetValueOrDefault()).Distinct().ToList();
                    parameterList.CustomerContactData = await _repo.GetKPIBookingCustomerContactsDataEfCore(bookingRequestFilters);

                    //get the quotation details by booking Ids
                    parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingRequestFilters);

                    //factory country required for pending quotation 
                    parameterList.FactoryLocation = await _inspRepo.GetFactorycountryByBookingQuery(bookingRequestFilters);

                    //Fetch the customer buyer name
                    parameterList.CustomerBuyerList = await _repo.GetKPIBookingBuyerDataEfCore(bookingRequestFilters);

                    //container Items
                    parameterList.ContainerItems = await _repo.GetContainerItemsByReportId(bookingRequestFilters);

                    parameterList.ExtraFeeData = await _repo.GetExtraFeeByBooking(bookingRequestFilters);

                    //fetch Merchandiser
                    parameterList.MerchandiserList = await _repo.GetMerchandiserByBooking(bookingRequestFilters);

                    var containerreportidlist = parameterList.ContainerItems.Where(x => x.ReportId.HasValue).Select(x => x.ReportId.Value).Distinct().ToList();

                    if (containerreportidlist != null && containerreportidlist.Any())
                        reportIdlist.AddRange(containerreportidlist);

                    //Fetch QC Names
                    parameterList.QcNames = await _repo.GetFbQcNames(reportIdlist);

                    //get invoice number
                    var invoiceData = await _repo.GetInvoiceNoByBooking(bookingRequestFilters);
                    parameterList.InvoiceBookingData = invoiceData.ConvertAll(x => new InvoiceBookingData
                    {
                        InvoiceNo = x.InvoiceNo,
                        InvoiceDate = x.InvoiceDate?.ToString(StandardDateFormat),
                        InvoieRemarks = x.InvoiceRemarks,
                        BookingId = x.BookingId.GetValueOrDefault(),
                        InspFees = x.InspFees.GetValueOrDefault(),
                        TravelFee = x.TravelFee.GetValueOrDefault(),
                        HotelFee = x.HotelFee.GetValueOrDefault(),
                        InvoiceTotal = x.TotalFee.GetValueOrDefault(),
                        InvoiceCurrency = x.CurrencyName,
                        OtherExpense = x.OtherFee.GetValueOrDefault(),
                        Discount = x.Discount.GetValueOrDefault(),
                        BilledTo = x.BilledTo
                    });


                    int prevQuotationId = 0;

                    var bookingIds = parameterList.BookingItems.OrderBy(x => x.BookingId).Select(x => x.BookingId).ToList();

                    foreach (var bookingId in bookingIds)
                    {
                        var quotMandayList = parameterList.QuotationDetails?.MandayList?.OrderBy(x => x.QuotationId).ThenBy(x => x.BookingId);

                        resultParameters.BookingDetails = parameterList.BookingItems.FirstOrDefault(x => x.BookingId == bookingId);
                        resultParameters.ProductItem = parameterList.ProductList.Where(x => x.BookingId == bookingId).ToList();
                        resultParameters.QuotationMandayDetails = quotMandayList?.FirstOrDefault(x => x.BookingId == bookingId);
                        resultParameters.QuotationDetails = parameterList?.QuotationDetails?.QuotDetails?.FirstOrDefault(x => x.Booking.Any(y => y.IdBooking == bookingId));
                        resultParameters.ContainerItems = parameterList.ContainerItems.Where(x => x.BookingId == bookingId).ToList();
                        resultParameters.PrevQuotationId = prevQuotationId;
                        resultParameters.ExtraFeeDetails = parameterList.ExtraFeeData?.Where(x => x.BookingId == bookingId).ToList();

                        //fetch quotation id from manday list using booking id
                        int quotationId = resultParameters.QuotationMandayDetails?.QuotationId ?? 0;

                        if (prevQuotationId != quotationId)
                        {
                            //get other cost based on quotation id
                            resultParameters.OtherCost = parameterList?.QuotationDetails?.QuotDetails?.FirstOrDefault(x => x.QuotationId == quotationId)?.OtherCost ?? 0;
                            prevQuotationId = quotationId;
                            resultParameters.Discount = parameterList?.QuotationDetails?.MandayList?.FirstOrDefault(x => x.QuotationId == quotationId)?.Discount ?? 0;
                        }
                        else
                        {
                            resultParameters.OtherCost = 0;
                            resultParameters.Discount = 0;
                        }

                        result.Add(KpiCustomMap.BookingExpenseMapByResult(parameterList, resultParameters));
                    }
                }
            }
            catch (Exception ex)
            {
                return result = null;
            }
            return result;
        }

        //Template based on booking
        public async Task<ExportResult> ExportWeeklyBookingStatus(KpiRequest request)
        {
            ExportResult response = new ExportResult();
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            var parameterList = new KPIMapParameters();
            ResultParameters resultParameters = new ResultParameters();

            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    parameterList.BookingInvoice = await GetBookingDetails(request);

                    if (parameterList.BookingInvoice.BookingItems.Count() == 0)
                        return response = null;

                    //Pick only the booking Ids to fetch Products
                    var bookingIds = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);

                    if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                    {
                        parameterList.ServiceTypeList = parameterList.ServiceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                        bookingIds = parameterList.ServiceTypeList.Select(x => x.InspectionId).ToList();
                    }

                    //fetch the PO details for the bookings
                    parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

                    //Fetch the Product List for all the bookings
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);

                    //get the quotation details by booking Ids
                    parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingIds);

                    //fetch Merchandiser
                    parameterList.MerchandiserList = await _repo.GetMerchandiserByBooking(bookingIds);

                    foreach (var bookingId in bookingIds)
                    {
                        resultParameters.BookingDetails = parameterList.BookingInvoice.BookingItems.Where(x => x.BookingId == bookingId).FirstOrDefault();
                        resultParameters.ProductItem = parameterList.ProductList.Where(x => x.BookingId == bookingId).ToList();
                        resultParameters.QuotationMandayDetails = parameterList?.QuotationDetails?.MandayList?.Where(x => x.BookingId == bookingId).FirstOrDefault();
                        resultParameters.QuotationDetails = parameterList?.QuotationDetails?.QuotDetails?.Where(x => x.Booking.Any(y => y.IdBooking == bookingId)).FirstOrDefault();

                        result.Add(KpiCustomMap.BookingMapByResult(parameterList, resultParameters));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            response.Data = result;
            return response;
        }

        /// <summary>
        /// Get carrefour invoice based details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parameterList"></param>
        /// <param name="bookingRequestFilters"></param>
        /// <returns></returns>
        private async Task GetCarreFourInvoiceData(KpiRequest request, KpiCarrefourInvoiceParameters parameterList, IQueryable<int> bookingRequestFilters)
        {
            var invoiceData = new List<KpiInvoiceData>();

            //get the invoice data by invoice no
            if (string.IsNullOrEmpty(request.InvoiceNo))
            {
                invoiceData = await _repo.GetInvoiceNoByBooking(bookingRequestFilters);
                parameterList.ExtraFeeData = await _repo.GetExtraFeeByBooking(bookingRequestFilters);
            }
            else if (!string.IsNullOrEmpty(request.InvoiceNo))
            {
                invoiceData = await _repo.GetInvoiceDetailsByInvoiceNo(request.InvoiceNo);
                parameterList.ExtraFeeData = await _repo.GetExtraFeeByInvoiceNo(request.InvoiceNo);
            }

            if (invoiceData != null && invoiceData.Any())
            {
                parameterList.InvoiceBookingData = invoiceData.ConvertAll(x => new InvoiceBookingData
                {
                    InvoiceNo = x.InvoiceNo,
                    InspFees = Math.Round(x.InspFees.GetValueOrDefault() + (x.InspFees.GetValueOrDefault() * x.TaxValue.GetValueOrDefault()), 2),
                    TravelFee = Math.Round(x.TravelFee.GetValueOrDefault() + (x.TravelFee.GetValueOrDefault() * x.TaxValue.GetValueOrDefault()), 2),
                    HotelFee = Math.Round(x.HotelFee.GetValueOrDefault() + (x.HotelFee.GetValueOrDefault() * x.TaxValue.GetValueOrDefault()), 2),
                    OtherExpense = Math.Round(x.OtherFee.GetValueOrDefault() + (x.OtherFee.GetValueOrDefault() * x.TaxValue.GetValueOrDefault()), 2),
                    InvoiceTotal = x.TotalFee.GetValueOrDefault(),
                    InvoiceCurrency = x.CurrencyName,
                    BookingId = x.BookingId.GetValueOrDefault(),
                    Id = x.InvoiceId,
                    BilledTo = x.BilledTo,
                    BilledName = x.BilledName,
                    InvoieRemarks = x.InvoiceRemarks
                });
            }
        }

        //Inoivce - Customer KPI Template details of booking, invoice.
        public async Task<DataTable> ExportCarreFourInvoiceTemplate(KpiRequest request)
        {
            List<CarrefourInvoiceResponse> result = new List<CarrefourInvoiceResponse>();

            try
            {
                if (request != null)
                {
                    KpiCarrefourInvoiceParameters parameterList = new KpiCarrefourInvoiceParameters();
                    request.IsInvoice = true;
                    request.StatusIds = new List<int>() { (int)BookingStatus.Inspected , (int)BookingStatus.ReportSent };
                    // fetch only the booking id list
                    var bookingRequestFilters = GetBookingIdAsQueryable(request);

                    //get the booking base details
                    parameterList.BookingItems = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingRequestFilters);

                    //Get the carrefour invoice data
                    await GetCarreFourInvoiceData(request, parameterList, bookingRequestFilters);

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceTypeList(bookingRequestFilters);

                    //get the quotation details by booking Ids
                    parameterList.QuotationDetails = await _repo.GetQuotationByBookings(bookingRequestFilters);

                    //po details
                    parameterList.PoDetails = await _repo.GetBookingPoDetails(bookingRequestFilters);

                    //Fetch the Product List for all the bookings
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingRequestFilters);

                    //factory country details
                    parameterList.FactoryLocation = await _inspRepo.GetFactorycountryByBookingQuery(bookingRequestFilters);

                    //reschedule details
                    parameterList.RescheduleList = await _repo.GetRescheduleBookingDetails(bookingRequestFilters);

                    //fetch fb other information
                    parameterList.FbReportComments = await _repo.GetFBOtherInformationList(bookingRequestFilters);

                    //get defect list by passing po id list
                    parameterList.FBReportDefectsList = await _repo.GetFBDefects(bookingRequestFilters);

                    //Get Problematic remarks for reports
                    parameterList.ReportRemarks = await _repo.GetFbProblematicRemarks(bookingRequestFilters);

                    //Get the shipment quantity from FB_Report_Qunatity table
                    parameterList.FbBookingQuantity = await _repo.GetInspectionQuantities(bookingRequestFilters);

                    //get product weight details
                    parameterList.FBWeightList = await _repo.GetFBReportWeightDetails(bookingRequestFilters);

                    //get product dimention details
                    parameterList.FBDimentionList = await _repo.GetFBReportDimentionDetails(bookingRequestFilters);

                    //get qc names list
                    parameterList.QcNames = await _repo.GetFbQcNames(bookingRequestFilters);

                    //get supplier code list by customer list and supplier list
                    parameterList.SupplierCode = await _supplierRepository.GetSupplierCode(bookingRequestFilters);

                    //get result name list by passing reportid list
                    parameterList.FBReportInspSubSummaryList = await _repo.GetFBInspSummaryResultbyReport(bookingRequestFilters);

                    result = KpiCustomMap.MapCarrefourInvoiceTemplate(parameterList);

                    var reportResultDataList = parameterList?.FBReportInspSubSummaryList?
                                .Select(y => new FbReportInspectionResult
                                {
                                    ReportId = y.FBReportId,
                                    Name = y.Name,
                                    Result = y.Result
                                }).ToList();

                    var dtBookingTable = _helper.ConvertToDataTableWithCaption(result);
                    var removedColumnList = new List<string>() { "ReportId" };

                    MapCarreFourReportResult(dtBookingTable, reportResultDataList, removedColumnList);

                    return dtBookingTable;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return null;
        }

        /// <summary>
        /// Xerox Invoice template
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<XeroInvoiceResponse> ExportXeroxInvoiceTemplate(KpiRequest request)
        {
            XeroInvoiceResponse response = new XeroInvoiceResponse();
            List<XeroInvoiceData> result = new List<XeroInvoiceData>();
            try
            {
                if (request != null)
                {
                    // check invoice data access and display the data based onn that filter
                    if (_ApplicationContext.StaffId > 0)
                    {
                        var invoiceAccessData = await _invoiceDataAccessRepository.GetStaffInvoiceDataAccess(_ApplicationContext.StaffId);

                        if (invoiceAccessData == null)
                        {
                            response.Result = XeroInvoiceResponseResult.NoInvoiceAccess;
                            response.ResultData = null;
                            return response;
                        }

                        request = MapInvoiceAccessKpiRequest(request, invoiceAccessData);


                        // fetch only the booking id list
                        var bookingIds = GetBookingIdAsQueryable(request);

                        // fetch only the audit id list
                        var auditIds = GetAuditIdAsQueryable(request);

                        // Xero Inspection Fees
                        var xeroxInvoiceList1 = await _repo.GetXeroxInvoiceFirstSetItems(bookingIds, request.InvoiceTypeIdList);
                        // Xero Travel Fees
                        var xeroxInvoiceList2 = await _repo.GetXeroxInvoiceSecondSetItems(bookingIds, request.InvoiceTypeIdList);
                        // Xero Other Fees
                        var xeroxInvoiceList3 = await _repo.GetXeroxInvoiceThirdSetItems(bookingIds, request.InvoiceTypeIdList);
                        //var xeroxInvoiceList4 = await _repo.GetXeroxInvoiceFourthSetItems(bookingRequestFilters, InvoiceTypeAccessList);
                        var xeroxInvoiceList5 = new List<XeroInvoiceData>();
                        if (request.InvoiceTypeIdList.Contains((int)INVInvoiceType.PreInvoice))
                        {
                            var Request = request;
                            Request.StatusIds = InspectionStatusList;

                            // fetch with cancel booking id list
                            var BookingIds = GetBookingIdAsQueryable(Request);

                            // Xero Extra Fees
                            xeroxInvoiceList5 = await _repo.GetXeroInvoiceFifthSetItems(BookingIds);
                        }

                        // Audit Xero Inspection Fees
                        var auditXeroxInvoiceList1 = await _repo.GetAuditXeroInvoiceFirstSetItems(auditIds, request.InvoiceTypeIdList);
                        // Audit Xero Travel Fees
                        var auditXeroxInvoiceList2 = await _repo.GetAuditXeroxInvoiceSecondSetItems(auditIds, request.InvoiceTypeIdList);
                        // Audit Xero Other Fees
                        var auditXeroxInvoiceList3 = await _repo.GetAuditXeroxInvoiceThirdSetItems(auditIds, request.InvoiceTypeIdList);
                        var auditXeroxInvoiceList5 = new List<XeroInvoiceData>();
                        if (request.InvoiceTypeIdList.Contains((int)INVInvoiceType.PreInvoice))
                        {
                            var Request = request;
                            Request.StatusIds = AuditStatusList;

                            // fetch with cancel audit id list
                            var AuditIds = GetAuditIdAsQueryable(Request);

                            // Audit Xero Extra Fees
                            auditXeroxInvoiceList5 = await _repo.GetAuditXeroInvoiceFifthSetItems(AuditIds);
                        }

                        result.AddRange(xeroxInvoiceList1);
                        result.AddRange(auditXeroxInvoiceList1);
                        result.AddRange(xeroxInvoiceList2);
                        result.AddRange(auditXeroxInvoiceList2);
                        result.AddRange(xeroxInvoiceList3);
                        result.AddRange(auditXeroxInvoiceList3);
                        // result.AddRange(xeroxInvoiceList4);
                        result.AddRange(xeroxInvoiceList5);
                        result.AddRange(auditXeroxInvoiceList5);

                        if (result == null || !result.Any())
                        {
                            response.Result = XeroInvoiceResponseResult.CannotGetList;
                            response.ResultData = null;
                            return response;
                        }

                        var groupedData = result.GroupBy(x => new {
                            x.ContactName,
                            x.EmailAddress,
                            x.POAddressLine1,
                            x.POAddressLine2,
                            x.POAddressLine3,
                            x.POAddressLine4,
                            x.POCity,
                            x.PORegion,
                            x.POPostalCode,
                            x.POCountry,
                            x.InvoiceNumber,
                            x.Reference,
                            x.InvoiceDate,
                            x.DueDate,
                            x.InventoryItemCode,
                            x.Description,
                            x.Quantity,
                            x.Discount,
                            x.AccountCode,
                            x.TaxType,
                            x.TrackingName1,
                            x.TrackingOption1,
                            x.TrackingName2,
                            x.TrackingOption2,
                            x.Currency,
                            x.BrandingTheme,
                            x.AccountName
                        }).Select(g => new XeroInvoiceData
                        {
                            ContactName = g.Key.ContactName,
                            EmailAddress = g.Key.EmailAddress,
                            POAddressLine1 = g.Key.POAddressLine1,
                            POAddressLine2 = g.Key.POAddressLine2,
                            POAddressLine3 = g.Key.POAddressLine3,
                            POAddressLine4 = g.Key.POAddressLine4,
                            POCity = g.Key.POCity,
                            PORegion = g.Key.PORegion,
                            POPostalCode = g.Key.POPostalCode,
                            POCountry = g.Key.POCountry,
                            InvoiceNumber = g.Key.InvoiceNumber,
                            Reference = g.Key.Reference,
                            InvoiceDate = g.Key.InvoiceDate,
                            DueDate = g.Key.DueDate,
                            InventoryItemCode = g.Key.InventoryItemCode,
                            Description = g.Key.Description,
                            Quantity = g.Key.Quantity,
                            UnitAmount = g.Sum(y => y.UnitAmount),
                            Discount = g.Key.Discount,
                            AccountCode = g.Key.AccountCode,
                            TaxType = g.Key.TaxType,
                            TrackingName1 = g.Key.TrackingName1,
                            TrackingOption1 = g.Key.TrackingOption1,
                            TrackingName2 = g.Key.TrackingName2,
                            TrackingOption2 = g.Key.TrackingOption2,
                            Currency = g.Key.Currency,
                            BrandingTheme = g.Key.BrandingTheme,
                            AccountName = g.Key.AccountName
                        }).ToList();

                        var dtBookingTable = _helper.ConvertToDataTableWithCaption(groupedData);

                        response.Result = XeroInvoiceResponseResult.Success;
                        response.ResultData = dtBookingTable;
                        return response;
                    }
                    else
                    {
                        response.Result = XeroInvoiceResponseResult.StaffIsNotValid;
                        response.ResultData = null;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return null;
        }

        /// <summary>
        /// Xerox Expense template
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<XeroInvoiceResponse> ExportXeroExpenseTemplate(KpiRequest request)
        {
            XeroInvoiceResponse response = new XeroInvoiceResponse();
            List<XeroInvoiceData> result = new List<XeroInvoiceData>();
            try
            {
                if (request != null)
                {
                    if (_ApplicationContext.StaffId > 0)
                    {
                        var invoiceAccessData = await _invoiceDataAccessRepository.GetStaffInvoiceDataAccess(_ApplicationContext.StaffId);

                        if (invoiceAccessData == null)
                        {
                            response.Result = XeroInvoiceResponseResult.NoInvoiceAccess;
                            response.ResultData = null;
                            return response;
                        }

                        request = MapInvoiceAccessKpiRequest(request, invoiceAccessData);

                        // fetch only the booking id list
                        var bookingIds = GetBookingIdAsQueryable(request);
                        var expenseItems = await _repo.GetXeroExpenseItems(bookingIds);
                        result.AddRange(expenseItems);

                        // fetch only the audit id list
                        var auditIds = GetAuditIdAsQueryable(request);
                        var auditXeroExpenseItems = await _repo.GetAuditXeroExpenseItems(auditIds);
                        result.AddRange(auditXeroExpenseItems);

                        var nonInspectionXeroExpenseItems = await _repo.GetNonInspectionXeroExpenseItems(request);
                        result.AddRange(nonInspectionXeroExpenseItems);

                        if (result == null || !result.Any())
                        {
                            response.Result = XeroInvoiceResponseResult.CannotGetList;
                            response.ResultData = null;
                            return response;
                        }
                        var groupedData = result.GroupBy(x => new {
                            x.ContactName,
                            x.EmailAddress,
                            x.POAddressLine1,
                            x.POAddressLine2,
                            x.POAddressLine3,
                            x.POAddressLine4,
                            x.POCity,
                            x.PORegion,
                            x.POPostalCode,
                            x.POCountry,
                            x.InvoiceNumber,
                            x.Reference,
                            x.InvoiceDate,
                            x.DueDate,
                            x.InventoryItemCode,
                            x.Description,
                            x.Quantity,
                            x.Discount,
                            x.AccountCode,
                            x.TaxType,
                            x.TrackingName1,
                            x.TrackingOption1,
                            x.TrackingName2,
                            x.TrackingOption2,
                            x.Currency,
                            x.BrandingTheme,
                            x.AccountName
                        })
                        .Select(g => new XeroInvoiceData
                        {
                            ContactName = g.Key.ContactName,
                            EmailAddress = g.Key.EmailAddress,
                            POAddressLine1 = g.Key.POAddressLine1,
                            POAddressLine2 = g.Key.POAddressLine2,
                            POAddressLine3 = g.Key.POAddressLine3,
                            POAddressLine4 = g.Key.POAddressLine4,
                            POCity = g.Key.POCity,
                            PORegion = g.Key.PORegion,
                            POPostalCode = g.Key.POPostalCode,
                            POCountry = g.Key.POCountry,
                            InvoiceNumber = g.Key.InvoiceNumber,
                            Reference = g.Key.Reference,
                            InvoiceDate = g.Key.InvoiceDate,
                            DueDate = g.Key.DueDate,
                            InventoryItemCode = g.Key.InventoryItemCode,
                            Description = g.Key.Description,
                            Quantity = g.Key.Quantity,
                            UnitAmount = g.Sum(y => y.UnitAmount),
                            Discount = g.Key.Discount,
                            AccountCode = g.Key.AccountCode,
                            TaxType = g.Key.TaxType,
                            TrackingName1 = g.Key.TrackingName1,
                            TrackingOption1 = g.Key.TrackingOption1,
                            TrackingName2 = g.Key.TrackingName2,
                            TrackingOption2 = g.Key.TrackingOption2,
                            Currency = g.Key.Currency,
                            BrandingTheme = g.Key.BrandingTheme,
                            AccountName = g.Key.AccountName
                        }).ToList();

                        var dtBookingTable = _helper.ConvertToDataTableWithCaption(groupedData);
                        response.Result = XeroInvoiceResponseResult.Success;
                        response.ResultData = dtBookingTable;
                        return response;
                    }
                    else
                    {
                        response.Result = XeroInvoiceResponseResult.StaffIsNotValid;
                        response.ResultData = null;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return null;
        }

        public DataTable MapCarreFourReportResult(DataTable dtBookingTable, List<FbReportInspectionResult> dataList, List<string> removedColumns)
        {
            foreach (DataRow row in dtBookingTable.Rows)
            {

                var reportResList = dataList.Where
                        (x => x.ReportId == Convert.ToInt32(row["ReportId"].ToString()));

                if (reportResList.Any())
                {
                    row["AQLResult"] = reportResList.FirstOrDefault(x => x.Name.ToLower() == "workmanship")?.Result;
                    row["ProdConformityResult"] = reportResList.FirstOrDefault(x => x.Name.ToLower() == "product specifications")?.Result;
                    row["SizeSpecResult"] = reportResList.FirstOrDefault(x => x.Name.ToLower() == "product specifications")?.Result;
                    row["PackagingResult"] = reportResList.FirstOrDefault(x => x.Name.ToLower() == "packing")?.Result;
                    row["PackingResult"] = reportResList.FirstOrDefault(x => x.Name.ToLower() == "packing")?.Result;
                }
            }
            if (dtBookingTable.Columns.Count > 0)
            {
                // removed columns 
                foreach (var column in removedColumns)
                {
                    dtBookingTable.Columns.Remove(column);
                }
            }
            return dtBookingTable;
        }

        /// <summary>
        /// Get general invoice data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parameterList"></param>
        /// <param name="bookingRequestFilters"></param>
        /// <returns></returns>
        private async Task GetGeneralInvoiceData(KpiRequest request, KpiGeneralInvoiceParameters parameterList, IQueryable<int> bookingRequestFilters)
        {
            var invoiceData = new List<KpiInvoiceData>();

            //get the invoice data by invoice no
            if (string.IsNullOrEmpty(request.InvoiceNo))
            {
                invoiceData = await _repo.GetInvoiceNoByBooking(bookingRequestFilters);
                parameterList.ExtraFeeData = await _repo.GetExtraFeeByBooking(bookingRequestFilters);
            }
            else if (!string.IsNullOrEmpty(request.InvoiceNo))
            {
                invoiceData = await _repo.GetInvoiceDetailsByInvoiceNo(request.InvoiceNo);
                parameterList.ExtraFeeData = await _repo.GetExtraFeeByInvoiceNo(request.InvoiceNo);
            }

            if (invoiceData != null && invoiceData.Any())
            {
                parameterList.InvoiceBookingData = invoiceData.ConvertAll(x => new InvoiceBookingData
                {
                    InvoiceNo = x.InvoiceNo,
                    InspFees = Math.Round(x.InspFees.GetValueOrDefault() + (x.InspFees.GetValueOrDefault() * x.TaxValue.GetValueOrDefault()), 2),
                    TravelFee = Math.Round(x.TravelFee.GetValueOrDefault() + (x.TravelFee.GetValueOrDefault() * x.TaxValue.GetValueOrDefault()), 2),
                    HotelFee = Math.Round(x.HotelFee.GetValueOrDefault() + (x.HotelFee.GetValueOrDefault() * x.TaxValue.GetValueOrDefault()), 2),
                    OtherExpense = Math.Round(x.OtherFee.GetValueOrDefault() + (x.OtherFee.GetValueOrDefault() * x.TaxValue.GetValueOrDefault()), 2),
                    InvoiceTotal = x.TotalFee.GetValueOrDefault(),
                    InvoiceCurrency = x.CurrencyName,
                    BookingId = x.BookingId.GetValueOrDefault(),
                    Id = x.InvoiceId,
                    BilledTo = x.BilledTo,
                    InvoieRemarks = x.InvoiceRemarks
                });
            }
        }

        /// <summary>
        /// fetch the invoice details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataTable> GetGeneralInvoiceTemplate(KpiRequest request)
        {
            var response = new List<GeneralInvoiceResponse>();

            var parameterList = new KpiGeneralInvoiceParameters();

            try
            {
                if (request != null)
                {
                    // fetch only the booking id list
                    var bookingRequestFilters = GetBookingIdAsQueryable(request);

                    parameterList.BookingItems = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingRequestFilters);

                    await GetGeneralInvoiceData(request, parameterList, bookingRequestFilters);

                    //get the customer contact data
                    parameterList.CustomerContactData = await _repo.GetKPIBookingCustomerContactsDataEfCore(bookingRequestFilters);

                    //Fetch the customer Department for the Dept code
                    parameterList.CustomerDept = await _repo.GetKPIBookingDepartmentDataEfCore(bookingRequestFilters);

                    //Fetch customer brands
                    parameterList.CustomerBrandList = await _repo.GetCustomerBrandbyBookingQuery(bookingRequestFilters);

                    //Fetch the customer buyer name
                    parameterList.CustomerBuyerList = await _repo.GetCustomerBuyerbyBookingQuery(bookingRequestFilters);

                    //fetch merchandiser name
                    parameterList.CustomerMerchandiserList = await _repo.GetMerchandiserByBooking(bookingRequestFilters);

                    //get supplier code list by customer list and supplier list
                    parameterList.SupplierCode = await _supplierRepository.GetSupplierCode(bookingRequestFilters);

                    //get the product details
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingRequestFilters);

                    //get the PO details
                    parameterList.PoDetails = await _repo.GetBookingPoDetails(bookingRequestFilters);

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceTypeByBookingQuery(bookingRequestFilters);

                    //get the quotation details
                    parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingRequestFilters);

                    response = KpiCustomMap.BookingMapInvoice(parameterList);

                    var dtBookingTable = _helper.ConvertToDataTableWithCaption(response);

                    var removedColumnList = new List<string>() { "ReportId" };

                    RemoveColumGeneralInvoice(dtBookingTable, removedColumnList);

                    return dtBookingTable;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        /// <summary>
        /// Remove unnecessary columns from general invoice
        /// </summary>
        /// <param name="dtBookingTable"></param>
        /// <param name="removedColumns"></param>
        /// <returns></returns>
        private void RemoveColumGeneralInvoice(DataTable dtBookingTable, List<string> removedColumns)
        {
            // removed columns 
            foreach (var column in removedColumns)
            {
                dtBookingTable.Columns.Remove(column);
            }
        }

        //Product based report with isEcopack true
        public async Task<List<CarreFourECOPackTemplate>> ExportCarrefourEcopackTemplate(KpiRequest request)
        {
            List<CarreFourECOPackTemplate> result = new List<CarreFourECOPackTemplate>();
            var parameterList = new KpiCarreFourECOPackParameters();
            if (request != null)
            {
                // fetch only the booking id list
                var bookingRequestFilters = GetBookingIdAsQueryable(request);

                //get the booking base details
                parameterList.BookingItems = await _repo.GetBookingEcoPackbyBookingQuery(bookingRequestFilters);

                //Get the service Type for the bookings
                parameterList.ServiceTypeList = await _inspRepo.GetServiceTypeList(bookingRequestFilters);

                //fetch the PO details for the bookings
                parameterList.PoDetails = await _repo.GetBookingPoDetails(bookingRequestFilters);

                //Fetch the Product List for all the bookings
                parameterList.ProductList = await _repo.GetProductListForEcoPack(bookingRequestFilters);

                //Fetch the customer Department for the Dept code
                parameterList.CustomerDept = await _repo.GetKPIBookingDepartmentDataEfCore(bookingRequestFilters);

                //fetch the report packing battery data
                parameterList.ReportBatteryData = await _repo.GetReportBatteryInfo(bookingRequestFilters);

                //fetch the report packing data
                parameterList.ReportPackingData = await _repo.GetReportPackingInfo(bookingRequestFilters);

                //fetch Qc Name
                var reportIdlist = parameterList.ProductList.Select(x => x.ReportId.GetValueOrDefault()).Distinct().ToList();
                parameterList.QcNames = await _repo.GetFbQcNames(reportIdlist);

                result = KpiCustomMap.BookingMapByBatteryPacking(parameterList);
            }
            return result;
        }

        public async Task<ExportResult> ExportMdmDefectTemplate(KpiRequest request)
        {


            //convert the request object to DB request
            KpiDbRequest req = new KpiDbRequest()
            {
                FromDate = request.FromDate.ToDateTime(),
                ToDate = request.ToDate.ToDateTime(),
                CustomerId = request.CustomerId,
                TemplateId = request.TemplateId,
                InvoiceNo = request.InvoiceNo ?? "",
                OfficeIdList = request.OfficeIdLst != null && request.OfficeIdLst.Any() ? request.OfficeIdLst.ConvertAll(x => new CommonId { Id = x }) : null,
                ServiceTypeIdList = request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any() ? request.ServiceTypeIdLst.ConvertAll(x => new CommonId { Id = x }) : null,
                BrandIdList = request.BrandIdList != null && request.BrandIdList.Any() ? request.BrandIdList.ConvertAll(x => new CommonId { Id = x }) : null,
                DepartmentIdList = request.DepartmentIdList != null && request.DepartmentIdList.Any() ? request.DepartmentIdList.ConvertAll(x => new CommonId { Id = x }) : null,
                EntityId = _tenant.GetCompanyId()
            };

            var data = await _repo.GetMDMDefectData(req);

            var result = KpiCustomMap.MdmDefectSummaryMap(data);

            return new ExportResult
            {
                Data = result
            };

        }

        /// <summary>
        /// Get the inspection picking data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<InspectionPickingReport>> GetInspectionPickingData(KpiRequest request)
        {
            var inspectionPickingReportList = new List<InspectionPickingReport>();
            //get the inspection picking data (IQueryable)
            var inspectionPicking = _repo.GetInspectionPickingData();
            //get the booking details with filters applied
            var KpiBookingData = await GetBookingDetails(request);

            if (KpiBookingData.BookingItems != null && KpiBookingData.BookingItems.Any())
            {
                var bookingIds = KpiBookingData.BookingItems.Select(x => x.BookingId).ToList();

                //Get the service Type for the bookings
                if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
                {
                    var serviceTypeList = await _inspRepo.GetServiceType(bookingIds);
                    var serviceIdData = serviceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).Select(x => x.InspectionId).ToList();
                    KpiBookingData.BookingItems = KpiBookingData.BookingItems.Where(x => serviceIdData.Contains(x.BookingId)).ToList();
                }

                var filteredBookingIds = KpiBookingData.BookingItems.Select(x => x.BookingId).ToList();

                if (filteredBookingIds != null && filteredBookingIds.Any())
                {
                    inspectionPicking = inspectionPicking.Where(x => filteredBookingIds.Contains(x.InspectionId));

                    //execute the inspection picking list
                    var inspectionPickingList = await inspectionPicking.AsNoTracking().ToListAsync();

                    //after applying all filter check inspectionpickinglist has data or not
                    if (inspectionPickingList != null && inspectionPickingList.Any())
                    {

                        //take the distinct inspectionid and productid
                        var inspectionProducts = inspectionPickingList.GroupBy(x => new { x.ProductId, x.InspectionId }).
                                                 Select(x => new InspectionPickingProducts()
                                                 { ProductId = x.Key.ProductId, InspectionId = x.Key.InspectionId }).Distinct().OrderBy(x => x.InspectionId).ToList();

                        //loop through the productid and populate the inspection picking report list
                        foreach (var product in inspectionProducts)
                        {
                            var pickingList = inspectionPickingList.Where(x => x.InspectionId == product.InspectionId && x.ProductId == product.ProductId);
                            if (pickingList != null && pickingList.Any())
                            {
                                var pickingData = pickingList.FirstOrDefault();

                                var pickingReport = new InspectionPickingReport()
                                {
                                    ProductName = pickingData.ProductName,
                                    ProductCategory = pickingData.ProductCategory,
                                    ProductSubCategory = pickingData.ProductSubCategory,
                                    CustomerName = pickingData.CustomerName,
                                    SupplierName = pickingData.SupplierName,
                                    FactoryName = pickingData.FactoryName,
                                    InspectionId = pickingData.InspectionId,
                                    PoNumber = string.Join(",", pickingList.Select(x => x.PoNumber).Distinct()),
                                    ServiceDate = pickingData.ServiceDateFrom == pickingData.ServiceDateTo ? pickingData.ServiceDateFrom.ToString(StandardDateFormat)
                                                    : pickingData.ServiceDateFrom.ToString(StandardDateFormat) + " to " + pickingData.ServiceDateTo.ToString(StandardDateFormat),
                                    LabName = pickingData.LabAddressId != null ? string.Join(",", pickingList.Select(x => x.LabName).Distinct().ToList()) : pickingData.CustomerName,
                                    PickingQuantity = pickingList.Where(x => pickingData.PickingQuantity != null).Sum(x => x.PickingQuantity.Value)
                                };

                                inspectionPickingReportList.Add(pickingReport);

                            }
                        }
                    }
                }

            }

            return inspectionPickingReportList;

        }

        /// <summary>
        /// Export the inspection picking data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExportResult> ExportInspectionPickingData(KpiRequest request)
        {
            var parameterList = new KPIMapParameters();
            //get the inspection picking list
            var inspectionPickingList = await GetInspectionPickingData(request);
            //assign to parameter list
            if (inspectionPickingList != null && inspectionPickingList.Any())
            {
                parameterList.InspectionPickingList = inspectionPickingList;
            }
            //map the inspection picking export data
            var inspectionPickingResult = KpiCustomMap.InspectionPickingMap(parameterList);

            return new ExportResult
            {
                Data = inspectionPickingResult
            };
        }

        //Template based on remarks
        public async Task<List<KpiReportCommentsTemplate>> ExportInspCommentSummaryTemplate(KpiRequest request)
        {
            InspCommentSummaryMapRequest mapRequest = new InspCommentSummaryMapRequest();
            try
            {
                if (request != null)
                {

                    var bookingRequestFilters = GetBookingIdAsQueryable(request);
                    mapRequest.CommentData = await _repo.GetReportCommentsDataEfCore(bookingRequestFilters);
                    mapRequest.PoData = await _repo.GetBookingPoDetails(bookingRequestFilters);
                    mapRequest.BuyerData = await _repo.GetKPIBookingBuyerDataEfCore(bookingRequestFilters);
                    mapRequest.DeptData = await _repo.GetKPIBookingDepartmentDataEfCore(bookingRequestFilters);
                    mapRequest.ContactData = await _repo.GetKPIBookingCustomerContactsDataEfCore(bookingRequestFilters);
                    mapRequest.FactoryLocation = await _inspRepo.GetFactorycountryByBookingQuery(bookingRequestFilters);
                    mapRequest.BillPaidByData = await _repo.GetQuotationByBookings(bookingRequestFilters);

                    //Get the service Type for the bookings
                    mapRequest.ServiceTypeData = await _inspRepo.GetServiceTypeByBookingQuery(bookingRequestFilters);

                    //Fetch the Product List for all the bookings
                    mapRequest.ProductData = await _repo.GetProductListByBooking(bookingRequestFilters);

                    //fetch the merchandiser by bookings
                    mapRequest.MerchandiserData = await _repo.GetMerchandiserByBooking(bookingRequestFilters);

                    var customerIds = mapRequest.CommentData.Select(x => x.CustomerId).Distinct().ToList();
                    var supplierIds = mapRequest.CommentData.Select(x => x.SupplierId).Distinct().ToList();
                    var factoryIds = mapRequest.CommentData.Select(x => x.FactoryId).Distinct().ToList();
                    var supIds = new List<int>();
                    supIds.AddRange(supplierIds);
                    supIds.AddRange(factoryIds);
                    mapRequest.SupplierCode = await _supplierRepository.GetSupplierCode(customerIds, supIds);

                    if (mapRequest.ServiceTypeData != null && mapRequest.ServiceTypeData.Any(x => x.serviceTypeId == (int)InspectionServiceTypeEnum.Container))
                    {
                        mapRequest.ProductData.AddRange(await _repo.GetContainerListByBooking(bookingRequestFilters));
                    }

                    return KpiCustomMap.MapInspCommentSummary(mapRequest);

                }
            }
            catch (Exception ex)
            {
                return null;

            }

            return null;
        }

        public async Task<DataTable> ExportOrderStatusLog(KpiRequest request)
        {
            var parameterList = new KPIMapParameters();
            try
            {
                if (request != null)
                {
                    //fetch the booking details
                    var kpiBookingAndInvoice = await GetBookingDetails(request);

                    if (kpiBookingAndInvoice.BookingItems.Count == 0)
                        return null;

                    parameterList.BookingInvoice = kpiBookingAndInvoice;
                    //Pick only the booking Ids to fetch Products
                    var bookingIds = kpiBookingAndInvoice.BookingItems.Select(x => x.BookingId).ToList();

                    //Fetch service type list
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);
                    //Fetch customer brands
                    parameterList.CustomerBrandList = await _repo.GetCustomerBrandbyBooking(bookingIds);
                    //Fetch the customer Department for the Dept code
                    parameterList.CustomerDept = await _deptRepo.GetCustomerDepartmentsbyBooking(bookingIds);
                    //get product list
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);
                    //get po color list
                    parameterList.POColorList = await _inspRepo.GetPOColorTransactionsByBookingIds(bookingIds);
                    //get customer product category list
                    parameterList.CustomerProductCategoryList = await _customerRepo.GetCustomerProductCategoryListByBookingIds(bookingIds);
                    //get qc names 
                    var reportIdlist = parameterList.ProductList.Where(x => x.ReportId.HasValue && x.ReportId > 0).Select(x => x.ReportId.GetValueOrDefault());
                    parameterList.QcNames = await _repo.GetFbQcNames(reportIdlist.ToList());
                    //factory country 
                    parameterList.FactoryLocation = await _inspRepo.GetFactorycountryId(bookingIds);
                    parameterList.OfficeCountryList = await _locationRepository.GetCountriesByOffice(kpiBookingAndInvoice.BookingItems.Select(x => x.OfficeId.GetValueOrDefault()).Distinct().ToList());

                    //fetch inspectio report customer decision data by booking ids
                    parameterList.CustomerDecisionData = await _repo.GetDecisionDateByReport(bookingIds);

                    // get defect list by passing po id list
                    parameterList.FBReportInspSubSummaryList = await _repo.GetFBInspSummaryResultbyReport(reportIdlist);

                    if (string.IsNullOrEmpty(request.InvoiceNo))
                    {
                        var invoiceData = await _repo.GetInvoiceNoByBooking(bookingIds);
                        parameterList.BookingInvoice.InvoiceBookingData = invoiceData.ConvertAll(x => new InvoiceBookingData
                        {
                            Id = x.InvoiceId,
                            InvoiceNo = x.InvoiceNo,
                            InvoiceDate = x.InvoiceDate?.ToString(StandardDateFormat),
                            BookingId = x.BookingId.GetValueOrDefault(),
                            InvoiceCurrency = x.CurrencyName,
                            InvoiceTo = x.InvoiceName,
                            BilledTo = x.BilledTo,
                            UnitPrice = x.UnitPrice,
                            BillingManDays = x.ManDay,
                            InspFees = x.InspFees.GetValueOrDefault(),
                            TravelLandFee = x.TravelLandFee,
                            TravelAirFee = x.TravelAirFee,
                            HotelFee = x.HotelFee,
                            TravelFee = x.TravelFee.GetValueOrDefault(),
                            InvoiceMethod = x.InvoiceMethod
                        });
                    }
                    parameterList.BilledContactList = await _invoicePreivewRepository.GetInvoiceBilledContacts(parameterList.BookingInvoice.InvoiceBookingData.Select(x => x.Id));
                    var result = KpiCustomMap.MapOrderStatusLogSummary(parameterList);

                    //convert the list to datatable
                    var dtBookingTable = _helper.ConvertToDataTableWithCaption(result);

                    var removedColumnList = OrderStatusLogRemovedColumnList;

                    var reportResultDataList = parameterList?.FBReportInspSubSummaryList?
                               .Select(y => new FbReportInspectionResult
                               {
                                   ReportId = y.FBReportId,
                                   Name = y.Name,
                                   Result = y.Result
                               }).ToList();

                    var resultName = parameterList?.FBReportInspSubSummaryList?.
                        Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).Distinct().ToList();

                    //map the booking dynamic fields with the datatable
                    MapBookingReportResult(dtBookingTable, resultName, reportResultDataList, removedColumnList);
                    return dtBookingTable;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        //Export inspeciton data
        public async Task<DataTable> ExportInspectionData(KpiRequest request)
        {
            var parameterList = new KPIMapParameters();
            //fetch bookng data
            var kpiInvoiceAndBooking = await GetBookingDetails(request);
            var bookingIds = kpiInvoiceAndBooking.BookingItems.Select(x => x.BookingId).ToList();
            parameterList.BookingInvoice = kpiInvoiceAndBooking;
            //fetch product data by booking ids
            parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);
            //fetch the service types by booking ids
            parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);
            //fetch the customer brands by booking ids
            parameterList.CustomerBrandList = await _repo.GetCustomerBrandbyBooking(bookingIds);
            //fetch the customer department by booking ids
            parameterList.CustomerDept = await _deptRepo.GetCustomerDepartmentsbyBooking(bookingIds);

            //fetch the customer contact by booking ids
            parameterList.CustomerContactData = await _repo.GetCustomerContactNames(bookingIds);

            //fetch the merchandiser by booking ids
            parameterList.MerchandiserList = await _repo.GetMerchandiserByBooking(bookingIds);

            var customerIds = parameterList.BookingInvoice.BookingItems.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();
            var supplierIds = parameterList.BookingInvoice.BookingItems.Select(x => x.SupplierId.GetValueOrDefault()).Distinct().ToList();
            var factoryIds = parameterList.BookingInvoice.BookingItems.Select(x => x.FactoryId.GetValueOrDefault()).Distinct().ToList();
            var supIds = new List<int>();
            supIds.AddRange(supplierIds);
            supIds.AddRange(factoryIds);
            parameterList.SupplierCode = await _supplierRepository.GetSupplierCode(customerIds, supIds);

            //po details
            parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds.ToList());

            //get po ids from po details list
            var poIdList = parameterList.PoDetails.Select(x => x.PoTransactionId);

            //fetch fb report defect data by po ids
            parameterList.FBReportDefectsList = await _repo.GetFBDefects(poIdList);

            var reportIdlist = parameterList.ProductList.Where(x => x.ReportId.HasValue && x.ReportId > 0).Select(x => x.ReportId.GetValueOrDefault()).Distinct();
            //fetch fb report insp summary by report ids
            parameterList.FBReportInspSubSummaryList = await _repo.GetFBInspSummaryResultbyReport(reportIdlist);
            //fetch inspectio report customer decision data by booking ids
            parameterList.CustomerDecisionData = await _repo.GetDecisionDateByReport(bookingIds);
            //factory location by booking ids
            parameterList.FactoryLocation = await _inspRepo.GetFactorycountryId(bookingIds);

            parameterList.OfficeCountryList = await _locationRepository.GetCountriesByOffice(kpiInvoiceAndBooking.BookingItems.Select(x => x.OfficeId.GetValueOrDefault()).Distinct().ToList());
            //export result name of the fb report insp summary
            var resultName = parameterList?.FBReportInspSubSummaryList?.
                  Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).Distinct().ToList();

            var inspectionDataList = KpiCustomMap.MapInspectionDataSummary(parameterList);

            //convert the list to datatable
            var dataTable = _helper.ConvertToDataTableWithCaption(inspectionDataList);

            MapInspectionDataWithDynamicFields(dataTable, resultName);

            return dataTable;
        }

        public DataTable MapInspectionDataWithDynamicFields(DataTable dataTable, List<string> resultNameList)
        {
            if (resultNameList != null && resultNameList.Any())
            {
                int index = dataTable.Columns["ServiceToDate"].Ordinal;
                //add the dynamic headers
                foreach (var dfHeader in resultNameList)
                {
                    index++;
                    dataTable.Columns.Add(dfHeader, typeof(string));
                    dataTable.Columns[dfHeader]?.SetOrdinal(index);
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    var fbResultList = row["FbResult"] as List<FbReportInspSummaryResult>;
                    if (fbResultList.Any())
                    {
                        foreach (var dfHeader in resultNameList)
                        {
                            row[dfHeader] = fbResultList.FirstOrDefault(x => x.Name == dfHeader)?.Result;
                        }
                    }
                }
            }
            // removed columns 
            var removedColumnList = new List<string>() { "FbResult" };
            foreach (var column in removedColumnList)
            {
                dataTable.Columns.Remove(column);
            }
            return dataTable;
        }

        //Export customer cultura
        public async Task<List<CustomerCulturaTemplate>> ExportCustomerCultura(KpiRequest request)
        {
            var parameterList = new KPIMapParameters();

            //fetch the booking details
            parameterList.BookingInvoice = await GetBookingDetails(request);

            if (parameterList.BookingInvoice.BookingItems.Count == 0)
                return null;

            //Pick only the booking Ids to fetch Products
            var bookingIds = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).Distinct().ToList();

            //Get the service Type for the bookings
            parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);
            if (request.ServiceTypeIdLst != null && request.ServiceTypeIdLst.Any())
            {
                parameterList.ServiceTypeList = parameterList.ServiceTypeList.Where(x => request.ServiceTypeIdLst.Contains(x.serviceTypeId)).ToList();
                bookingIds = parameterList.ServiceTypeList.Select(x => x.InspectionId).ToList();
            }

            //fetch the PO details for the bookings
            parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds);

            //Fetch the Product List for failed reports
            parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);

            //get the quotation details by booking Ids
            parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingIds);

            //get the status log details by booking Ids
            parameterList.BookingStatusLogs = _managementDashboardRepo.GetBookingStatusLogs(bookingIds);

            var enumEntityName = (Company)_tenant.GetCompanyId();
            parameterList.EntityName = enumEntityName.ToString().ToUpper();

            return KpiCustomMap.MapCustomerCultura(parameterList);
        }

        //export eci
        public async Task<DataTable> ExportECITemplate(KpiRequest request)
        {
            try
            {
                if (request != null)
                {
                    var parameterList = new ECITemplateParameters();

                    // fetch only the booking id list
                    var bookingRequestFilters = GetBookingIdAsQueryable(request);

                    if (bookingRequestFilters.Count() == 0)
                        return null;

                    //get the booking base details
                    parameterList.BookingItems = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingRequestFilters);

                    //Get the service Type for the bookings
                    parameterList.ServiceTypeList = await _inspRepo.GetServiceTypeList(bookingRequestFilters);

                    //Fetch the customer Department for the Dept code
                    parameterList.CustomerDept = await _repo.GetKPIBookingDepartmentDataEfCore(bookingRequestFilters);

                    //Fetch the customer buyer name
                    parameterList.CustomerBuyerList = await _repo.GetKPIBookingBuyerDataEfCore(bookingRequestFilters);

                    //fetch merchandiser name
                    parameterList.CustomerMerchandiserList = await _repo.GetMerchandiserByBooking(bookingRequestFilters);

                    //po details
                    parameterList.PoDetails = await _repo.GetBookingPoDetails(bookingRequestFilters);

                    //Fetch the Product List for all the bookings
                    parameterList.ProductList = await _repo.GetProductListByBooking(bookingRequestFilters);

                    //factory country details
                    parameterList.FactoryLocation = await _inspRepo.GetFactorycountryByBookingQuery(bookingRequestFilters);

                    //get defect list by passing po id list
                    parameterList.FBReportDefectsList = await _repo.GetFBDefects(bookingRequestFilters);

                    //Get the shipment quantity from FB_Report_Qunatity table
                    parameterList.FbBookingQuantity = await _repo.GetInspectionQuantities(bookingRequestFilters);

                    //take the dynamic field response
                    var dfBookingFieldsResponse = await _dynamicFieldManager.GetBookingDFDataByBookings(bookingRequestFilters);
                    parameterList.BookingDFDataList = dfBookingFieldsResponse.bookingDFDataList;

                    //get result name list by passing reportid list
                    parameterList.FBReportInspSubSummaryList = await _repo.GetFBInspSummaryResultbyReport(bookingRequestFilters);

                    //Get Problematic remarks for reports
                    parameterList.ReportRemarks = await _repo.GetFbProblematicRemarks(bookingRequestFilters);
                    var reportRemarks = parameterList.ReportRemarks?.Where(x => x.Result?.ToLower() == "pending" || x.Result?.ToLower() == "fail")?.ToList();

                    var Data = KpiCustomMap.MapECITemplate(parameterList);
                    Data = Data?.OrderBy(x => x.BookingNo).ToList();

                    //convert the list to datatable
                    var dtBookingTable = _helper.ConvertToDataTableWithCaption(Data);

                    //map the booking dynamic fields with the datatable
                    MapBookingECIResult(dtBookingTable, parameterList.FBReportInspSubSummaryList, parameterList.FBReportDefectsList, reportRemarks);
                    return dtBookingTable;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable MapBookingECIResult(DataTable dtBookingTable, List<FBReportInspSubSummary> fbReportInspSubSummaryList, List<FBReportDefects> fBReportDefectsList, List<FbReportRemarks> reportRemarks)
        {
            if (fbReportInspSubSummaryList != null && fbReportInspSubSummaryList.Any())
            {
                var inspRepoColName = fbReportInspSubSummaryList.Where(x => !string.IsNullOrEmpty(x.Name) && !string.IsNullOrWhiteSpace(x.Name))?.Select(x => x.Name).Distinct().ToList();
                if (inspRepoColName != null && inspRepoColName.Any())
                {
                    foreach (var inspRepoHeader in inspRepoColName)
                    {
                        dtBookingTable.Columns.Add(inspRepoHeader, typeof(string));

                        if (inspRepoHeader != "Workmanship")
                        {
                            var fbReportInspIds = fbReportInspSubSummaryList.Where(x => x.Name == inspRepoHeader).Select(x => x.Id).ToList();
                            var remarksColName = reportRemarks?.Where(x => fbReportInspIds.Contains(x.InspSummaryId) && !string.IsNullOrEmpty(x.SubCategory) && !string.IsNullOrWhiteSpace(x.SubCategory))?.Select(x => x.SubCategory).Distinct().ToList();

                            if (remarksColName != null && remarksColName.Any())
                            {
                                foreach (var remarksHeader in remarksColName)
                                {
                                    dtBookingTable.Columns.Add(inspRepoHeader + " - " + remarksHeader, typeof(string));
                                }
                            }
                        }
                    }
                }

                dtBookingTable.Columns["Workmanship"]?.SetOrdinal(dtBookingTable.Columns.Count - 1);

                // add defect columns 
                var defectDescColName = fBReportDefectsList?.Where(x => !string.IsNullOrEmpty(x.DefectDesc) && !string.IsNullOrWhiteSpace(x.DefectDesc))?.Select(x => x.DefectDesc).Distinct().OrderBy(x => x).ToList();
                if (defectDescColName != null && defectDescColName.Any())
                {
                    foreach (var defectDescHeader in defectDescColName)
                    {
                        dtBookingTable.Columns.Add(defectDescHeader, typeof(int));
                    }
                }

                foreach (DataRow row in dtBookingTable.Rows)
                {
                    var reportResList = fbReportInspSubSummaryList.Where(x => x.FBReportId == Convert.ToInt32(row["ReportId"].ToString()))?.ToList();
                    if (reportResList != null && reportResList.Any() && inspRepoColName != null && inspRepoColName.Any())
                    {
                        foreach (var inspRepoHeader in inspRepoColName)
                        {
                            var fbReportInsp = reportResList.FirstOrDefault(x => x.Name == inspRepoHeader);

                            if (fbReportInsp != null)
                            {
                                switch (fbReportInsp.Result?.ToLower())
                                {
                                    case "pending":
                                        row[inspRepoHeader] = "Pending";
                                        break;
                                    case "pass":
                                        row[inspRepoHeader] = "Conform";
                                        break;
                                    case "fail":
                                        row[inspRepoHeader] = "Not Conform";
                                        break;
                                    case "missing":
                                        row[inspRepoHeader] = "Abortive";
                                        break;
                                }
                                if (inspRepoHeader != "Workmanship")
                                {
                                    var remarksRowName = reportRemarks?.Where(x => x.InspSummaryId == fbReportInsp.Id && !string.IsNullOrEmpty(x.SubCategory) && !string.IsNullOrWhiteSpace(x.SubCategory))?.ToList();

                                    var remarksColName = remarksRowName?.Select(x => x.SubCategory).Distinct().ToList();

                                    if (remarksColName != null && remarksColName.Any())
                                    {
                                        foreach (var remarksHeader in remarksColName)
                                        {
                                            string insSubCategoryHeader = inspRepoHeader + " - " + remarksHeader;
                                            row[insSubCategoryHeader] = remarksRowName?.FirstOrDefault(x => x.SubCategory == remarksHeader)?.Result;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var defectDescList = fBReportDefectsList?.Where(x => x.FBReportDetailId == Convert.ToInt32(row["ReportId"].ToString()) && x.ProductId == Convert.ToInt32(row["ProductId"].ToString()));
                    if (defectDescList != null && defectDescList.Any() && defectDescColName != null && defectDescColName.Any())
                    {
                        foreach (var defectDescHeader in defectDescColName)
                        {
                            var fBReportDefect = defectDescList.FirstOrDefault(x => x.DefectDesc == defectDescHeader);

                            if (fBReportDefect != null)
                            {
                                if (fBReportDefect.Critical > 0)
                                {
                                    row[defectDescHeader] = fBReportDefect?.Critical;
                                }
                                else if (fBReportDefect.Major > 0)
                                {
                                    row[defectDescHeader] = fBReportDefect?.Major;
                                }
                                else if (fBReportDefect.Minor > 0)
                                {
                                    row[defectDescHeader] = fBReportDefect?.Minor;
                                }
                            }
                        }
                    }
                }
            }

            // removed columns 
            var removedColumnList = new List<string>() { "ReportId", "ProductId" };
            foreach (var column in removedColumnList)
            {
                dtBookingTable.Columns.Remove(column);
            }

            //moved the column to the end
            dtBookingTable.Columns["CriticalDefect"]?.SetOrdinal(dtBookingTable.Columns.Count - 1);
            dtBookingTable.Columns["MajorDefect"]?.SetOrdinal(dtBookingTable.Columns.Count - 1);
            dtBookingTable.Columns["MinorDefect"]?.SetOrdinal(dtBookingTable.Columns.Count - 1);
            dtBookingTable.Columns["OrdnanceDeFabrication"]?.SetOrdinal(dtBookingTable.Columns.Count - 1);
            dtBookingTable.Columns["ShortDescription"]?.SetOrdinal(dtBookingTable.Columns.Count - 1);
            dtBookingTable.Columns["Season"]?.SetOrdinal(dtBookingTable.Columns.Count - 1);
            dtBookingTable.Columns["IRCode"]?.SetOrdinal(dtBookingTable.Columns.Count - 1);

            return dtBookingTable;
        }

        public async Task<List<KPITemplate>> GetTemplateList()
        {
            var data = _repo.GetTemplateList();

            return await data.Select(x => new KPITemplate
            {
                Id = x.Id,
                Name = x.Name,
                TypeId = x.TypeId
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<KPITemplate>> GetKpiTeamplate(KPITemplateRequest request)
        {
            var data = _repo.GetTemplateList();

            if (_ApplicationContext.UserType == UserTypeEnum.Customer)
                data = data.Where(x => x.IsDefaultCustomer.Value || x.RefKpiTeamplateCustomers.Any(y => y.UserTypeId.Value == (int)UserTypeEnum.Customer && y.CustomerId == (request.CustomerId > 0 ? request.CustomerId.GetValueOrDefault() : _ApplicationContext.CustomerId)));

            else if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
            {
                if (request.CustomerId != null && request.CustomerId > 0)
                    data = data.Where(x => x.IsDefault.Value || x.RefKpiTeamplateCustomers.Any(y => y.CustomerId == request.CustomerId));
                else
                    data = data.Where(x => x.IsDefault.Value);
            }

            return await data.Select(x => new KPITemplate
            {
                Id = x.Id,
                Name = x.Name,
                TypeId = x.TypeId
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ScheduleAnalysisTemplate>> ExportScheduleAnalysisTemplate(KpiRequest request)
        {
            var result = new List<ScheduleAnalysisTemplate>();
            var parameterList = new KPIMapParameters();
            if (request != null)
            {

                request.StatusIds = InspectionStatusList;
                //fetch the booking details
                parameterList.BookingInvoice = await GetBookingDetails(request);

                if (parameterList.BookingInvoice.BookingItems.Count == 0)
                    return null;

                //Pick only the booking Ids to fetch Products
                var bookingIds = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).ToList();

                //fetch the PO details for the bookings
                parameterList.PoDetails = await _repo.GetBookingPOTransactionDetails(bookingIds);

                //Fetch the Product List for all the bookings
                parameterList.ProductList = await _repo.GetProductListByBooking(bookingIds);

                var reportIds = parameterList.ProductList.Where(x => x.ReportId.HasValue && x.ReportId > 0).Select(x => x.ReportId.GetValueOrDefault()).ToList();

                //get qc names 
                parameterList.QcNames = await _repo.GetFbQcNames(reportIds);

                //get po color list
                parameterList.POColorList = await _inspRepo.GetPOColorTransactionsByBookingIds(bookingIds);

                //Fetch customer brands
                parameterList.CustomerBrandList = await _repo.GetCustomerBrandbyBooking(bookingIds);

                //Fetch the customer Department for the Dept code
                parameterList.CustomerDept = await _deptRepo.GetCustomerDepartmentsbyBooking(bookingIds);

                //factory country required for pending quotation 
                parameterList.FactoryLocation = await _inspRepo.GetFactorycountryId(bookingIds);

                //Get the service Type for the bookings
                parameterList.ServiceTypeList = await _inspRepo.GetServiceType(bookingIds);

                //fetch inspectio report customer decision data by booking ids
                parameterList.CustomerDecisionData = await _repo.GetDecisionDateByReport(bookingIds);

                //get the quotation details by booking Ids
                parameterList.QuotationDetails = await _repo.GetClientQuotationByBooking(bookingIds);

                parameterList.ExtraFeeData = await _repo.GetExtraFeeByBooking(bookingIds);

                parameterList.ScheduleQcList = await _schRepo.GetQCBookingDetails(bookingIds);

                parameterList.ExpenseData = await _repo.GetBookingExpense(bookingIds);

                var customerIds = parameterList.BookingInvoice.BookingItems.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();

                var supplierIds = parameterList.BookingInvoice.BookingItems.Select(x => x.SupplierId.GetValueOrDefault()).Distinct().ToList();

                var factoryIds = parameterList.BookingInvoice.BookingItems.Select(x => x.FactoryId.GetValueOrDefault()).Distinct().ToList();

                parameterList.SupplierGrades = await _supplierRepository.GetGradeByCustomersAndSuppliers(customerIds, supplierIds);

                parameterList.FactoryGrades = await _supplierRepository.GetGradeByCustomersAndSuppliers(customerIds, factoryIds);

                var bookingDynamicFieldResponse = await _dynamicFieldManager.GetBookingDFDataByBookingIds(bookingIds);

                parameterList.BookingDFDataList = bookingDynamicFieldResponse?.bookingDFDataList;

                //get invoice number
                var invoiceData = await _repo.GetInvoiceNoByBooking(bookingIds);

                parameterList.BookingInvoice.InvoiceBookingData = invoiceData.ConvertAll(x => new InvoiceBookingData
                {
                    Id = x.InvoiceId,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.InvoiceDate?.ToString(StandardDateFormat),
                    BookingId = x.BookingId.GetValueOrDefault(),
                    InvoiceCurrency = x.CurrencyName,
                    InvoiceTo = x.InvoiceName,
                    InvoiceTotal = x.TotalFee.GetValueOrDefault(),
                    InvoicePaymentStatus = x.InvoicePaymentStatus,
                    BilledToName = x.BilledToName
                });

                result = KpiCustomMap.MapScheduleAnalysis(parameterList);

            }
            return result;
        }

        public async Task<List<InspectionSummaryQCTemplate>> ExportInspectionSummaryQCTemplate(KpiRequest request)
        {
            if (request == null)
                return null;

            request.StatusIds = InspectionStatusList;

            var bookingIds = GetBookingIdAsQueryable(request);

            if (bookingIds == null || !bookingIds.Any())
                return null;

            var bookingItems = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingIds);

            var serviceTypeList = await _inspRepo.GetServiceTypeByBookingQuery(bookingIds);

            var factoryLocationData = await _inspRepo.GetFactorycountryByBookingQuery(bookingIds);

            var qcList = await _schRepo.GetQCBookingDetailsByBookingQuery(bookingIds);

            var claimList = await _repo.GetQcKpiBookingExpenseDetails(bookingIds);

            var invoiceList = await _repo.GetQcKpiBookingInvoiceDetails(bookingIds);

            var extraFeeList = await _repo.GetExtraFeeByBooking(bookingIds);

            var result = KpiCustomMap.MapInspectionSummaryQC(qcList, bookingItems, serviceTypeList, factoryLocationData, claimList, invoiceList, extraFeeList);

            if (result == null || !result.Any())
                return null;

            return result;
        }

        public async Task<DataTable> ExportCustomerMandaySummaryTemplate(KpiRequest request)
        {
            if (request != null)
            {

                request.StatusIds = KPICustomerMandayStatusList;

                var bookingIds = GetBookingIdAsQueryable(request);

                var bookings = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingIds);

                var bookingServiceTypes = await _inspRepo.GetBookingServiceTypes(bookingIds);

                var estimatedMandayDetails = await _dashboardRepository.GetInspectionManDays(bookingIds).AsNoTracking().ToListAsync();
                var actualMandayDetails = await _dashboardRepository.GetInspectionActualCount(bookingIds).ToListAsync();
                //var quantityDetails = await _repo.GetQuantityDetailsData(bookingIds);
                var productCounts = await _repo.GetProductsCountByInspectionIds(bookingIds);

                //
                var quantityDetailFromReports = await _repo.GetInspectionQuantities(bookingIds);

                //
                var factoryIds = bookings.Select(x => x.FactoryId.GetValueOrDefault()).Distinct().ToList();
                var factoryAddresses = await _supplierRepository.GetSupplierAddressBySupplierIds(factoryIds);

                //
                var result = KpiCustomMap.MapCustomerManday(bookings, quantityDetailFromReports, actualMandayDetails, estimatedMandayDetails, productCounts, factoryAddresses, bookingServiceTypes);

                var groupByRequestFilter = new GroupByCustomerMandayRequestFilter();
                if (request.CustomerMandayGroupByFields != null && request.CustomerMandayGroupByFields.Any())
                {
                    groupByRequestFilter.Customer = request.CustomerMandayGroupByFields.Any(x => x == CustomerMandayGroupByFields.Customer);
                    groupByRequestFilter.ServiceType = request.CustomerMandayGroupByFields.Any(x => x == CustomerMandayGroupByFields.ServiceType);
                    groupByRequestFilter.FactoryCountry = request.CustomerMandayGroupByFields.Any(x => x == CustomerMandayGroupByFields.FactoryCountry);
                    groupByRequestFilter.FactoryProvince = request.CustomerMandayGroupByFields.Any(x => x == CustomerMandayGroupByFields.FactoryProvince);
                    groupByRequestFilter.FactoryCity = request.CustomerMandayGroupByFields.Any(x => x == CustomerMandayGroupByFields.FactoryCity);
                }

                var customerMandayExportData = result.GroupBy(x => new
                {
                    FactoryCountry = groupByRequestFilter.FactoryCountry ? x.FactoryCountry : null,
                    FactoryCountryId = groupByRequestFilter.FactoryCountry ? x.CountryId : 0,
                    FactoryCity = groupByRequestFilter.FactoryCity ? x.FactoryCity : null,
                    FactoryCityId = groupByRequestFilter.FactoryCity ? x.FactoryCityId : 0,
                    FactoryProvince = groupByRequestFilter.FactoryProvince ? x.FactoryProvince : null,
                    FactoryProvinceId = groupByRequestFilter.FactoryProvince ? x.FactoryProvinceId : 0,
                    ServiceType = groupByRequestFilter.ServiceType ? x.ServiceType : null,
                    ServiceTypeId = groupByRequestFilter.ServiceType ? x.ServiceTypeId : 0,
                    Customer = groupByRequestFilter.Customer ? x.CustomerName : null,
                    CustomerId = groupByRequestFilter.Customer ? x.CustomerId : 0
                }).Select(x => new CustomerMandayExportData()
                {
                    FactoryCountry = x.Key.FactoryCountry,
                    FactoryCity = x.Key.FactoryCity,
                    FactoryProvince = x.Key.FactoryProvince,
                    ServiceType = x.Key.ServiceType,
                    Customer = x.Key.Customer,
                    Inspection = x.Select(x => x.InspectionId).Distinct().Count(),
                    Reports = x.Sum(y => y.ReportCount),
                    ProductCount = x.Sum(y => y.ProductCount),
                    ActualManday = x.Sum(y => y.ActualManday),
                    EstimatedManday = x.Sum(y => y.EstimatedManday),
                    InspectedQty = x.Sum(y => y.InspectedQty),
                    OrderQty = x.Sum(y => y.OrderQty),
                    PresentedQty = x.Sum(y => y.PresentedQty)
                }).ToList();

                //convert the list to datatable
                var dataTable = _helper.ConvertToDataTableWithCaption(customerMandayExportData);
                List<string> removeColumn = new List<string>();
                if (!groupByRequestFilter.Customer)
                    removeColumn.Add("Customer");
                if (!groupByRequestFilter.FactoryCountry)
                    removeColumn.Add("FactoryCountry");
                if (!groupByRequestFilter.FactoryProvince)
                    removeColumn.Add("FactoryProvince");
                if (!groupByRequestFilter.FactoryCity)
                    removeColumn.Add("FactoryCity");
                if (!groupByRequestFilter.ServiceType)
                    removeColumn.Add("ServiceType");

                _helper.RemoveCloumnToDataTable(dataTable, removeColumn);
                return dataTable;
            }

            return null;
        }

        public async Task<DataTable> ExportGapProductRefLevel(KpiRequest request)
        {

            if (request == null)
                return null;

            var bookingIds = GetBookingIdAsQueryable(request);

            var inspections = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingIds);

            var inspProducts = await _inspRepo.GetBookingQuantityDetails(bookingIds);

            var inspQuantities = await _repo.GetInspectionQuantities(bookingIds);

            var reportDetails = await _repo.GetGapKpiReportData(bookingIds);

            var reportIds = reportDetails.Select(x => x.ReportId).ToList();

            var reportDefects = await _repo.GetFBDefectsByReportIds(reportIds);

            var qcList = await _schRepo.GetQCBookingDetailsByBookingQuery(bookingIds);

            var brands = await _inspRepo.GetBrandBookingIdsByBookingIds(bookingIds);

            var bookingServiceTypes = await _inspRepo.GetBookingServiceTypes(bookingIds);

            var fbReportQualityPlans = await _reportRepository.GetFbReportQualityPlansByReportIds(reportIds);

            var customerIds = inspections.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();
            var supplierIds = inspections.Select(x => x.SupplierId.GetValueOrDefault()).Distinct().ToList();
            var factoryIds = inspections.Select(x => x.FactoryId.GetValueOrDefault()).Distinct().ToList();

            var supplierCodes = await _supplierRepository.GetSupplierCode(customerIds, supplierIds);

            var factoryCodes = await _supplierRepository.GetSupplierCode(customerIds, factoryIds);

            var factoryAddresses = await _supplierRepository.GetSupplierAddressBySupplierIds(factoryIds);

            var officeCountries = await _locationRepository.GetCountriesByOffice(inspections.Select(x => x.OfficeId.GetValueOrDefault()).Distinct().ToList());

            var purchaseOrders = await _inspRepo.GetPoNoListByBookingIds(bookingIds);

            var fbReportPackingSampleSize = await _reportRepository.GetFbReportPackingPackagingLabellingProducts(reportIds, new List<int>() { (int)FbReportPackageType.Workmanship });

            var result = KpiCustomMap.MapGapCustomerProductRefLevel(inspections, inspProducts, inspQuantities, reportDetails, reportDefects, qcList, brands, bookingServiceTypes, fbReportQualityPlans, supplierCodes, factoryCodes, factoryAddresses, officeCountries, purchaseOrders, fbReportPackingSampleSize);

            var resultSummaryList = await _repo.GetFBInspSummaryResultbyReport(reportIds);

            var resultNames = resultSummaryList.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).Distinct().ToList();

            var reportResultDataList = resultSummaryList?
                               .Select(y => new FbReportInspectionResult
                               {
                                   ReportId = y.FBReportId,
                                   Name = y.Name,
                                   Result = y.Result
                               }).ToList();

            var dtBookingTable = _helper.ConvertToDataTableWithCaption(result);
            var removedColumnList = new List<string>() { "ReportId" };
            return MapBookingReportResult(dtBookingTable, resultNames, reportResultDataList, removedColumnList);
        }

        /// <summary>
        /// Export AR Follow up Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataTable> ExportARFollowUpReport(KpiRequest request)
        {
            DataTable dt = new DataTable();
            List<InvoiceCommunication> invoiceCommunications = null;

            if (request != null)
            {
                var invoiceAccessData = await _invoiceDataAccessRepository.GetStaffInvoiceDataAccess(_ApplicationContext.StaffId);
                if (invoiceAccessData == null)
                    return null;

                request = MapInvoiceAccessKpiRequest(request, invoiceAccessData);

                var bookingIds = GetBookingIdAsQueryable(request);

                if (bookingIds == null || !bookingIds.Any())
                    return null;

                var invoiceDataQuery = _repo.GetInvoiceDetailsQueryable(bookingIds);

                if (request.InvoiceTypeIdList != null && request.InvoiceTypeIdList.Any())
                {
                    invoiceDataQuery = invoiceDataQuery.Where(x => request.InvoiceTypeIdList.Contains(x.InvoiceType.GetValueOrDefault()));
                }

                if (request.PaymentStatusIdList != null && request.PaymentStatusIdList.Any(x => x == (int)KpiInvoicePaymentStatus.Paid && x != (int)KpiInvoicePaymentStatus.Pending))
                {
                    invoiceDataQuery = invoiceDataQuery.Where(x => x.InvoicePaymentStatus.GetValueOrDefault() == (int)InvoicePaymentStatus.Paid);
                }
                else if (request.PaymentStatusIdList != null && request.PaymentStatusIdList.Any(x => x != (int)KpiInvoicePaymentStatus.Paid && x == (int)KpiInvoicePaymentStatus.Pending))
                {
                    invoiceDataQuery = invoiceDataQuery.Where(x => x.InvoicePaymentStatus.GetValueOrDefault() != (int)InvoicePaymentStatus.Paid);
                }

                //get invoice data list by booking ids
                var invoiceDataList = await invoiceDataQuery.Select(x => new KpiInvoiceData
                {
                    InvoiceId = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    BookingId = x.InspectionId,
                    InvoiceDate = x.InvoiceDate,
                    InvoiceRemarks = x.Remarks,
                    InspFees = x.InspectionFees,
                    TravelFee = x.TravelTotalFees,
                    HotelFee = x.HotelFees,
                    TotalFee = x.TotalInvoiceFees,
                    CurrencyName = x.InvoiceCurrencyNavigation.CurrencyName,
                    OtherFee = x.OtherFees,
                    BilledTo = x.InvoiceTo,
                    BilledToName = x.InvoicedName,
                    BilledAddress = x.InvoicedAddress,
                    InvocieBillingEntity = x.OfficeNavigation.Name,
                    Discount = x.Discount,
                    TaxValue = x.TaxValue,
                    BilledName = x.InvoiceToNavigation.Label,
                    PaymentTerms = x.PaymentTerms,
                    PaymentDuration = x.PaymentDuration,
                    InvoiceStatus = x.InvoiceStatusNavigation.Name,
                    PaymentDate = x.InvoicePaymentDate,
                    PaymentStatusName = x.InvoicePaymentStatusNavigation.Name,
                    CurrencyId = x.InvoiceCurrency,
                    InvoiceType = x.InvoiceTypeNavigation.Name

                }).AsNoTracking().ToListAsync();

                var invoiceBookingIds = invoiceDataQuery.Where(x => x.InspectionId != null).Select(x => x.InspectionId.GetValueOrDefault()).Distinct();

                //get the inspection booking list
                var bookingInspections = await _repo.GetBookingItemsbyBookingIdAsQuery(invoiceBookingIds);

                //get the factory id list
                var factoryIds = bookingInspections.Select(x => x.FactoryId.GetValueOrDefault()).Distinct().ToList();

                //get the factory address by factory id list
                var factoryAddressList = await _supplierRepository.GetSupplierAddressBySupplierIds(factoryIds);

                //get the booking brands
                var bookingBrands = await _inspRepo.GetBrandBookingIdsByBookingIds(invoiceBookingIds);

                //get the booking service types
                var bookingServiceTypes = await _inspRepo.GetBookingServiceTypes(invoiceBookingIds);

                //get the booking departments
                var bookingDepartments = await _deptRepo.GetCustomerDepartmentsbyBooking(invoiceBookingIds);

                //fetch the PO details for the bookings
                var poDetails = await _repo.GetBookingPoDetails(invoiceBookingIds);

                //get the quotation list by booking ids
                var quotationDetails = await _repo.GetQuotationByBookings(invoiceBookingIds);

                //get the invoice communication details by invoice nos
                if (invoiceDataList.Any())
                {
                    var invoiceNoList = invoiceDataList.Where(x => x.InvoiceNo != null).Select(x => x.InvoiceNo.Trim().ToLower()).Distinct().ToList();
                    invoiceCommunications = await _repo.GetInvoiceCommunicationByInvoiceNo(invoiceNoList);
                }

                //get the invoice credit data by booking ids
                var invoiceCreditData = await _claimRepo.GetInvoiceCreditDetailsByBookingIds(invoiceBookingIds);

                var extraFeeDataQuery = _repo.GetExtraFeeByBookingQuery(bookingIds);

                if (request.InvoiceTypeIdList != null && request.InvoiceTypeIdList.Any())
                {
                    extraFeeDataQuery = extraFeeDataQuery.Where(x => request.InvoiceTypeIdList.Contains(x.Invoice.InvoiceType.GetValueOrDefault()));
                }

                if (request.PaymentStatusIdList != null && request.PaymentStatusIdList.Any(x => x == (int)KpiInvoicePaymentStatus.Paid && x != (int)KpiInvoicePaymentStatus.Pending))
                {
                    extraFeeDataQuery = extraFeeDataQuery.Where(x => x.PaymentStatus.GetValueOrDefault() == (int)InvoicePaymentStatus.Paid);
                }
                else if (request.PaymentStatusIdList != null && request.PaymentStatusIdList.Any(x => x != (int)KpiInvoicePaymentStatus.Paid && x == (int)KpiInvoicePaymentStatus.Pending))
                {
                    extraFeeDataQuery = extraFeeDataQuery.Where(x => x.PaymentStatus.GetValueOrDefault() != (int)InvoicePaymentStatus.Paid);
                }

                //get the extra fee data list by booking ids
                var extraFeeDataList = await extraFeeDataQuery.Select(x => new KpiExtraFeeData
                {
                    BookingId = x.InspectionId,
                    InvoiceId = x.InvoiceId,
                    ExtraFee = x.TotalExtraFee,
                    BilledTo = x.BilledTo,
                    ExtraFeeInvoiceNo = x.ExtraFeeInvoiceNo,
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.InvoiceCurrency.CurrencyName,
                    ExtraFeeStatus = x.Status.Name,
                    BilledName = x.BilledToNavigation.Label,
                    PaymentTerms = x.PaymentTerms,
                    PaymentStatusName = x.PaymentStatusNavigation.Name,
                    PaymentDate = x.PaymentDate,
                    InvoiceDate = x.ExtraFeeInvoiceDate,
                    PaymentDuration = x.PaymentDuration,
                    BilledToName = x.BilledName,
                    BilledToAddress = x.BilledAddress,
                    BillingEntity = x.BillingEntity.Name
                }).AsNoTracking().ToListAsync();

                var extraFeeBookingIds = extraFeeDataQuery.Where(x => x.InspectionId != null).Select(x => x.InspectionId.GetValueOrDefault()).Distinct();

                //get the inspection booking list
                var extraFeeBookingInspections = await _repo.GetBookingItemsbyBookingIdAsQuery(extraFeeBookingIds);

                //get the factory id list
                var extraFeeFactoryIds = bookingInspections.Select(x => x.FactoryId.GetValueOrDefault()).Distinct().ToList();

                //get the factory address by factory id list
                var extraFeeFactoryAddressList = await _supplierRepository.GetSupplierAddressBySupplierIds(extraFeeFactoryIds);

                //get the booking brands
                var extraFeeBookingBrands = await _inspRepo.GetBrandBookingIdsByBookingIds(extraFeeBookingIds);

                //get the booking service types
                var extraFeeBookingServiceTypes = await _inspRepo.GetBookingServiceTypes(extraFeeBookingIds);

                //get the booking departments
                var extraFeeBookingDepartments = await _deptRepo.GetCustomerDepartmentsbyBooking(extraFeeBookingIds);

                //fetch the PO details for the bookings
                var extraFeePoDetails = await _repo.GetBookingPoDetails(extraFeeBookingIds);

                //get the quotation list by booking ids
                var extraFeeQuotationDetails = await _repo.GetQuotationByBookings(extraFeeBookingIds);

                //make the request to currency conversion usd for invoice
                var result = invoiceDataList.ConvertAll(x => new ExchangeCurrencyItem
                {
                    CurrencyId = x.CurrencyId.GetValueOrDefault(),
                    Fee = x.TotalFee.GetValueOrDefault(),
                    Id = x.BookingId.GetValueOrDefault()
                }).ToList();

                //get the usd currency conversion for invoice data
                var usdConversionData = await _financeManager.CurrencyConversionToUsd(result, (int)CurrencyMaster.USD);

                //make the request to currency conversion usd for extra fee
                var extraFeeResult = extraFeeDataList.ConvertAll(x => new ExchangeCurrencyItem
                {
                    ExtraFeeCurrencyId = x.CurrencyId.GetValueOrDefault(),
                    ExtraFee = x.ExtraFee.GetValueOrDefault(),
                    Id = x.BookingId.GetValueOrDefault()
                }).ToList();

                //get the usd currency conversion for extra fee
                var extraFeeUsdConversionData = await _financeManager.CurrencyConversionToUsd(extraFeeResult, (int)CurrencyMaster.USD);

                //get the entity name
                var masterConfigs = await _inspManager.GetMasterConfiguration();
                var entityName = masterConfigs.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.Entity && x.Active == true)?.Value;

                //map the AR Follow Up Report
                var data = KpiCustomMap.MapARFollowUpReport(bookingInspections, quotationDetails, poDetails, factoryAddressList, bookingBrands,
                    bookingDepartments, bookingServiceTypes, invoiceDataList, invoiceCreditData, extraFeeDataList, usdConversionData,
                    extraFeeUsdConversionData, entityName,
                    invoiceCommunications, extraFeeBookingInspections, extraFeeQuotationDetails, extraFeePoDetails, extraFeeFactoryAddressList, extraFeeBookingBrands,
                    extraFeeBookingDepartments, extraFeeBookingServiceTypes);

                data = data.OrderBy(x => x.ServiceToDate).ToList();

                dt = _helper.ConvertToDataTableWithCaption(data);

            }

            return dt;
        }

        public async Task<DataTable> ExportGapFlashProcessAudit(KpiRequest request)
        {
            if (request == null)
                return null;

            request.ServiceTypeIdLst = new List<int>() { GapFlashProcessAuditServiceType };

            var bookingIds = GetBookingIdAsQueryable(request);

            var inspections = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingIds);

            var dyanmicFieldsResponse = await _dynamicFieldManager.GetBookingDFDataByBookingIds(bookingIds);

            var bookingServiceTypes = await _inspRepo.GetBookingServiceTypes(bookingIds);

            var reportDetails = await _repo.GetGapKpiReportData(bookingIds);

            var reportInspSummary = await _repo.GetFBInspSummaryResultbyReport(reportDetails.Select(x => x.ReportId).ToList());

            var customerIds = inspections.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();
            var supplierIds = inspections.Select(x => x.SupplierId.GetValueOrDefault()).Distinct().ToList();
            var factoryIds = inspections.Select(x => x.FactoryId.GetValueOrDefault()).Distinct().ToList();

            var supplierCodes = await _supplierRepository.GetSupplierCode(customerIds, supplierIds);

            var factoryCodes = await _supplierRepository.GetSupplierCode(customerIds, factoryIds);

            var factoryAddresses = await _supplierRepository.GetSupplierAddressBySupplierIds(factoryIds);

            var qcList = await _schRepo.GetQCBookingDetailsByBookingQuery(bookingIds);

            var data = KpiCustomMap.MapGapFlashProcessAudit(inspections, reportDetails, dyanmicFieldsResponse.bookingDFDataList, bookingServiceTypes, qcList, supplierCodes, factoryCodes, factoryAddresses);

            var resultNames = reportInspSummary.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).Distinct().ToList();

            var reportResultDataList = reportInspSummary?
                               .Select(y => new FbReportInspectionResult
                               {
                                   ReportId = y.FBReportId,
                                   Name = y.Name,
                                   Result = y.ScoreValue
                               }).ToList();
            var dtBookingTable = _helper.ConvertToDataTableWithCaption(data);
            var removedColumnList = new List<string>() { "ReportId" };
            return MapBookingReportResult(dtBookingTable, resultNames, reportResultDataList, removedColumnList);
        }

        public async Task<DataTable> ExportGapProcessAudit(KpiRequest request)
        {
            if (request == null)
                return null;

            var auditIds = GetAuditIdAsQueryable(request);

            var audits = await _repo.GetAuditItemsbyAuditIdAsQuery(auditIds);

            var auditServiceTypes = await _auditRepository.GetServiceTypeDataByAudit(auditIds);

            var customerIds = audits.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();
            var supplierIds = audits.Select(x => x.SupplierId.GetValueOrDefault()).Distinct().ToList();
            var factoryIds = audits.Select(x => x.FactoryId.GetValueOrDefault()).Distinct().ToList();

            var supplierCodes = await _supplierRepository.GetSupplierCode(customerIds, supplierIds);

            var factoryCodes = await _supplierRepository.GetSupplierCode(customerIds, factoryIds);

            var factoryAddresses = await _supplierRepository.GetSupplierAddressBySupplierIds(factoryIds);

            var auditors = await _auditRepository.GetAuditorDataByAudit(auditIds);

            var fbReportCheckpoints = await _auditRepository.GetAuditFbReportCheckpointByAuditIds(auditIds);

            var data = KpiCustomMap.MapGapProcessAudit(audits, auditServiceTypes, auditors, supplierCodes, factoryCodes, factoryAddresses);


            var resultNames = fbReportCheckpoints.Where(x => !string.IsNullOrEmpty(x.ChekPointName)).Select(x => x.ChekPointName).Distinct().ToList();

            var reportResultDataList = fbReportCheckpoints?
                               .Select(y => new FbReportCheckpointResult
                               {
                                   AuditId = y.AuditId.GetValueOrDefault(),
                                   Name = y.ChekPointName,
                                   Result = y.ScoreValue
                               }).ToList();

            var dtBookingTable = _helper.ConvertToDataTableWithCaption(data);

            return MapAuditCheckpointResult(dtBookingTable, resultNames, reportResultDataList, null);
        }

        private KpiRequest MapInvoiceAccessKpiRequest(KpiRequest request, Entities.InvDaTransaction invoiceAccessData)
        {
            List<int> InvoiceTypeAccessList = new List<int>() { (int)INVInvoiceType.Monthly, (int)INVInvoiceType.PreInvoice };

            var customerIdAccess = invoiceAccessData?.InvDaCustomers.Where(x => x.Active).Select(x => x.CustomerId).Distinct().ToList();

            var officeIdAccess = invoiceAccessData?.InvDaOffices.Where(x => x.Active).Select(x => x.OfficeId).Distinct().ToList();

            var invoiceTypeAccess = invoiceAccessData?.InvDaInvoiceTypes.Where(x => x.Active).Select(x => x.InvoiceTypeId).Distinct().ToList();

            if (customerIdAccess != null && customerIdAccess.Any())
            {
                if (request.CustomerId > 0)
                {
                    List<int> customerList = new List<int>() { request.CustomerId.GetValueOrDefault() };
                    request.CustomerIdList = customerIdAccess.Intersect(customerList).ToList();
                    request.CustomerId = 0;
                }
                else
                {
                    request.CustomerIdList = customerIdAccess;
                }
            }

            if (officeIdAccess != null && officeIdAccess.Any())
            {
                if (request.OfficeIdLst != null && request.OfficeIdLst.Any())
                {
                    request.OfficeIdLst = officeIdAccess.Intersect(request.OfficeIdLst).ToList();
                }
                else
                {
                    request.OfficeIdLst = officeIdAccess;
                }
            }

            if (invoiceTypeAccess != null && invoiceTypeAccess.Any())
            {
                InvoiceTypeAccessList = invoiceTypeAccess;
            }

            if (request.InvoiceTypeIdList != null && request.InvoiceTypeIdList.Any())
            {
                InvoiceTypeAccessList = InvoiceTypeAccessList.Intersect(request.InvoiceTypeIdList).ToList();
            }
            request.InvoiceTypeIdList = InvoiceTypeAccessList;
            return request;
        }

        public async Task<DataTable> ExportExpenseInspectionSummaryTemplate(KpiRequest request)
        {
            if (request == null)
                return null;

            request.StatusIds = InspectionStatusList;

            var bookingIds = GetBookingIdAsQueryable(request);

            if (bookingIds == null || !bookingIds.Any())
                return null;

            var bookingItems = await _repo.GetBookingItemsbyBookingIdAsQuery(bookingIds);

            var serviceTypeList = await _inspRepo.GetServiceTypeByBookingQuery(bookingIds);

            var factoryLocationData = await _inspRepo.GetFactorycountryByBookingQuery(bookingIds);

            var qcList = await _schRepo.GetQCBookingDetailsByBookingQuery(bookingIds);

            var claimList = await _repo.GetQcKpiBookingExpenseDetails(bookingIds);

            var invoiceList = await _repo.GetQcKpiBookingInvoiceDetails(bookingIds);

            var extraFeeList = await _repo.GetExtraFeeByBooking(bookingIds);

            var result = KpiCustomMap.MapInspectionSummaryExpenses(qcList, bookingItems, serviceTypeList, factoryLocationData, claimList, invoiceList, extraFeeList);


            if (result == null || !result.Any())
                return null;

            var dataTable = _helper.ConvertToDataTableWithCaption(result);
            List<string> removeColumn = new List<string>() { "QcId" };

            _helper.RemoveCloumnToDataTable(dataTable, removeColumn);
            return dataTable;
        }
    }
}

