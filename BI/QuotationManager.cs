using BI.Maps;
using BI.Maps.APP;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using DTO.DataAccess;
using DTO.DynamicFields;
using DTO.Eaqf;
using DTO.EventBookingLog;
using DTO.File;
using DTO.Inspection;
using DTO.Invoice;
using DTO.MasterConfig;
using DTO.MobileApp;
using DTO.Quotation;
using DTO.RepoRequest.Enum;
using DTO.Supplier;
using DTO.User;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static BI.TenantProvider;

namespace BI
{
    public class QuotationManager : ApiCommonData, IQuotationManager
    {
        private readonly ILocationManager _locManager = null;
        private readonly IReferenceManager _referenceManager = null;
        private readonly IQuotationRepository _QuotationRepository = null;
        private readonly IOfficeLocationManager _officeLocationManager = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly ICustomerContactRepository _customerContactRepository = null;
        private readonly ISupplierRepository _supplierRepository = null;
        private readonly IHumanResourceRepository _hrReposiotry = null;
        private readonly IUserRightsManager _userManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ICombineOrdersManager _combinedOrdersManager = null;
        private readonly IQuotationPDF _quotationPDF = null;
        private readonly IBroadCastService _brodcastService = null;
        private readonly ICustomerManager _customerManager = null;
        private IDictionary<QuotationStatus, Func<SetStatusBusinessRequest, Task<SetStatusQuotationResponse>>> _dictStatuses = null;
        private readonly IInspectionBookingRepository _insprepo = null;
        private readonly IAuditRepository _audprepo = null;
        private readonly ILogger _logger = null;
        private static IConfiguration _Configuration = null;
        private readonly IDynamicFieldManager _dynamicFieldManager = null;
        private readonly IFBInternalReportRepository _fBInternalReportRepository = null;
        private readonly ICustomerPriceCardManager _customerPriceCardManager = null;
        private readonly IEventBookingLogManager _eventBookingLog = null;
        private readonly ICustomerDepartmentRepository _deptRepo = null;
        private readonly IInspectionBookingManager _bookingManager = null;
        private readonly ICustomerPriceCardRepository _customerPriceCardRepository = null;
        private readonly IKpiCustomRepository _customKpiRepo = null;
        private readonly QuotationMap QuotationMap = null;
        private readonly CustomerPriceCardMap CustomerPriceCardMap = null;
        private readonly QuotationMobileMap QuotationMobileMap = null;
        private ITenantProvider _filterService = null;
        private readonly IUserConfigRepository _userConfigRepo = null;
        private readonly IInvoiceManager _invoiceManager = null;
        private readonly ITravelMatrixRepository _travelMatrixRepository = null;
        private readonly IEmailManager _emailManager;
        private readonly ISupplierManager _supplierManager = null;
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly IManualInvoiceManager _manualInvoiceManager;
        private readonly IReferenceRepository _referenceRepository = null;
        private readonly ICustomerCheckPointRepository _customerCheckPointRepository;

        public QuotationManager(ILocationManager locManager,
            IReferenceManager referenceManager,
            IQuotationRepository quotationRepository,
            IOfficeLocationManager officeLocationManager,
            ICustomerRepository customerRepository,
            ICustomerContactRepository customerContactRepository,
            ISupplierRepository supplierRepository,
            IHumanResourceRepository hrReposiotry,
            IAPIUserContext applicationContext,
            IUserRightsManager userManager,
            ICombineOrdersManager combinedOrderManager,
            IQuotationPDF quotationPDF,
            IBroadCastService brodcastService,
             ICustomerManager customerManager,
             ILogger<QuotationManager> logger,
             IInspectionBookingRepository insprepo,
             IAuditRepository auditrepo,
            IConfiguration configuration,
            IDynamicFieldManager dynamicFieldManager,
            IFBInternalReportRepository fBInternalReportRepository,
            ICustomerPriceCardManager customerPriceCardManager,
            IEventBookingLogManager eventBookingLog,
            ICustomerDepartmentRepository deptRepo,
            IInspectionBookingManager bookingManager,
            ICustomerPriceCardRepository customerPriceCardRepository,
            IKpiCustomRepository customKpiRepo,
            ICustomerCheckPointRepository customerCheckPointRepository,
             ITenantProvider filterService,
             IUserConfigRepository userConfigRepo,
             IInvoiceManager invoiceManager,
             ITravelMatrixRepository travelMatrixRepository,
             IEmailManager emailManager,
             ISupplierManager supplierManager,
             IInvoiceRepository invoiceRepository,
             IManualInvoiceManager manualInvoiceManager,
              IReferenceRepository referenceRepository
            )
        {
            _locManager = locManager;
            _referenceManager = referenceManager;
            _QuotationRepository = quotationRepository;
            _officeLocationManager = officeLocationManager;
            _customerRepository = customerRepository;
            _customerContactRepository = customerContactRepository;
            _supplierRepository = supplierRepository;
            _hrReposiotry = hrReposiotry;
            _ApplicationContext = applicationContext;
            _userManager = userManager;
            _combinedOrdersManager = combinedOrderManager;
            _quotationPDF = quotationPDF;
            _brodcastService = brodcastService;
            _customerManager = customerManager;
            _logger = logger;
            _insprepo = insprepo;
            _audprepo = auditrepo;
            _Configuration = configuration;
            _dynamicFieldManager = dynamicFieldManager;
            _fBInternalReportRepository = fBInternalReportRepository;
            _customerPriceCardManager = customerPriceCardManager;
            _eventBookingLog = eventBookingLog;
            _deptRepo = deptRepo;
            _dictStatuses = new Dictionary<QuotationStatus, Func<SetStatusBusinessRequest, Task<SetStatusQuotationResponse>>>() {
                                    { QuotationStatus.ManagerApproved, ToManagerApprove },
                                    { QuotationStatus.Canceled, ToCancelQuotation },
                                    { QuotationStatus.AERejected, ToCSRejected },
                                    { QuotationStatus.ManagerRejected, ToManagerRejected },
                                    { QuotationStatus.CustomerValidated, ToCustomerValidated },
                                    { QuotationStatus.CustomerRejected,  ToCustomerReject },
                                    { QuotationStatus.SentToClient, ToSend },
                               };
            _bookingManager = bookingManager;
            _customerPriceCardRepository = customerPriceCardRepository;
            _customKpiRepo = customKpiRepo;
            QuotationMap = new QuotationMap();
            CustomerPriceCardMap = new CustomerPriceCardMap();
            QuotationMobileMap = new QuotationMobileMap();
            _filterService = filterService;
            _userConfigRepo = userConfigRepo;
            _invoiceManager = invoiceManager;
            _travelMatrixRepository = travelMatrixRepository;
            _emailManager = emailManager;
            _supplierManager = supplierManager;
            _manualInvoiceManager = manualInvoiceManager;
            _invoiceRepository = invoiceRepository;
            _referenceRepository = referenceRepository;
            _customerCheckPointRepository = customerCheckPointRepository;
        }

        public void UpdateEmailState(Guid id, int emailState, bool isTask)
        {
            var task = _hrReposiotry.GetSingle<MidTask>(x => x.Id == id);

            // TODO TASK OR NOTIFICATION

            if (task != null)
            {
                task.EmailState = emailState;
                task.UpdatedBy = _ApplicationContext?.UserId;
                task.UpdatedOn = DateTime.Now;
                _hrReposiotry.Save(task);
            }
        }

        public async Task<QuotationSummaryResponse> GetQuotationSummary()
        {
            var response = new QuotationSummaryResponse();

            var customerByUserType = await _customerManager.GetCustomerByUserType(new CommonDataSourceRequest() { IsVirtualScroll = false });
            if (customerByUserType.Result == DataSourceResult.Success)
                response.CustomerList = customerByUserType.DataSourceList.ToList();
            //OfficeList 
            response.OfficeList = _officeLocationManager.GetAllOffices();

            if (response.OfficeList == null || !response.OfficeList.Any())
                return new QuotationSummaryResponse { Result = QuotationSummaryResult.CannotFindOfficeList };

            // StatusList 
            var statusList = await _QuotationRepository.GetStatusList();

            if (statusList == null || !statusList.Any())
                return new QuotationSummaryResponse { Result = QuotationSummaryResult.CannotFindStatusList };

            response.StatusList = statusList.Select(QuotationMap.GetStatus);

            //Service List 
            response.ServiceList = await _referenceManager.GetServices();

            if (response.ServiceList == null || !response.ServiceList.Any())
                return new QuotationSummaryResponse { Result = QuotationSummaryResult.CannotFindServiceList };

            response.Result = QuotationSummaryResult.Success;

            return response;
        }

        //Create New Requst For Quotation Export
        private QuotationSummaryRepoRequest CreateNewRequstForQuotation(QuotationSummaryGenRequest request)
        {
            if (_ApplicationContext.UserType != UserTypeEnum.InternalUser)
            {
                var statusidlist = new int[] { (int)QuotationStatus.CustomerValidated, (int)QuotationStatus.CustomerRejected, (int)QuotationStatus.SentToClient };

                request.Statusidlst = request.Statusidlst != null && request.Statusidlst.Count() == 0 ? statusidlist : statusidlist.Where(x => request.Statusidlst.Any(y => y == x)).ToArray();
            }
            else
            {
                var accesslocidlist = _ApplicationContext.LocationList != null ? _ApplicationContext.LocationList.ToList() : new List<int>();

                if (request.Officeidlst != null && request.Officeidlst.Any())
                {
                    request.Officeidlst = accesslocidlist.Where(x => request.Officeidlst.Any(y => x == y)).ToList();
                }
                else
                {
                    request.Officeidlst = accesslocidlist;
                }
            }
            //filter data by user type
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        request.Customerid = request?.Customerid == null || request?.Customerid == 0 ? _ApplicationContext.CustomerId : request?.Customerid.Value;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        request.Factoryidlst = request.Factoryidlst != null && request.Factoryidlst.Count() > 0 ? request.Factoryidlst : new List<int>().Append(_ApplicationContext.FactoryId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        request.Supplierid = request?.Supplierid == null || request?.Supplierid == 0 ? _ApplicationContext.SupplierId : request?.Supplierid.Value;
                        break;
                    }
            }

            var newRequest = new QuotationSummaryRepoRequest
            {
                Customerid = request.Customerid,
                Factoryidlst = request.Factoryidlst,
                Fromdate = request.Fromdate?.ToDateTime(),
                Todate = request.Todate?.ToDateTime(),
                Supplierid = request.Supplierid,
                Statusidlst = request.Statusidlst,
                Index = request.Index,
                ServiceId = request.ServiceId,
                Officeidlst = request.Officeidlst,
                No = request.Searchtypetext,
                SearchTypeId = request.Searchtypeid,
                BillPaidBy = _ApplicationContext.UserType == UserTypeEnum.Customer ? (int)QuotationPaidBy.customer : 0,
                ServiceTypelst = request.ServiceTypelst,
                AdvancedSearchtypeid = request.AdvancedSearchtypeid.Trim().ToLower(),
                AdvancedSearchtypetext = request.AdvancedSearchtypetext?.Trim(),
                DateTypeId = request.Datetypeid,
                DeptIdList = request.DeptIdList,
                BrandIdList = request.BrandIdList,
                BuyerIdList = request.BuyerIdList
            };
            return newRequest;
        }
        //map quoation billing data
        private List<Tuple<int, string, string>> GetQuotationBillData(IEnumerable<QuotationInspAuditExportRepo> quAuditDetails,
                                               IEnumerable<SupplierAddressData> factAddressList, List<CustomerAccountingAddress> customeraddress,
                                              List<SupplierAddress> supplieraddress, List<Tuple<int, string>> lstcuscontact,
                                              List<Tuple<int, string>> lstsupcontact, List<Tuple<int, string>> lstfactcontact)
        {
            var quobillings = new List<Tuple<int, string, string>>();
            foreach (var quotation in quAuditDetails)
            {
                var Accountingaddress = "";
                var contactlst = new List<string>();

                // first check the accounting address , if not exists take the home address.
                switch (quotation.BillPaidById)
                {
                    case (int)QuotationPaidBy.customer:
                        {
                            if (customeraddress.Where(x => x.AddressType == (int)RefAddressTypeEnum.Accounting).Any())
                            {
                                Accountingaddress = customeraddress.Where(x => x.AddressType == (int)RefAddressTypeEnum.Accounting && x.CustomerId == quotation.CustomerId).Select(x => x.EnglishAddress).FirstOrDefault();
                            }
                            else
                            {
                                Accountingaddress = customeraddress.Where(x => x.AddressType == (int)RefAddressTypeEnum.HeadOffice && x.CustomerId == quotation.CustomerId).Select(x => x.EnglishAddress)?.FirstOrDefault();
                            }

                            contactlst = lstcuscontact.Where(x => x.Item1 == quotation.QuotationId).Select(x => x.Item2).ToList();

                            break;
                        }
                    case (int)QuotationPaidBy.supplier:
                        {
                            if (supplieraddress.Where(x => x.SupplierAddresstype == (int)SuAddressTypeEnum.Accounting).Any())
                            {
                                Accountingaddress = supplieraddress.Where(x => x.SupplierAddresstype == (int)SuAddressTypeEnum.Accounting && x.SupplierId == quotation.SupplierId).Select(x => x.Address).FirstOrDefault();
                            }
                            else
                            {
                                Accountingaddress = supplieraddress.Where(x => x.SupplierAddresstype == (int)SuAddressTypeEnum.Headoffice && x.SupplierId == quotation.SupplierId).Select(x => x.Address)?.FirstOrDefault();
                            }

                            contactlst = lstsupcontact.Where(x => x.Item1 == quotation.QuotationId).Select(x => x.Item2).ToList();
                            break;
                        }
                    case (int)QuotationPaidBy.factory:
                        {
                            Accountingaddress = quotation.FactoryAddress;
                            contactlst = lstfactcontact.Where(x => x.Item1 == quotation.QuotationId).Select(x => x.Item2).ToList();
                            break;
                        }

                }
                quotation.BillPaidByAddress = Accountingaddress;

                var contact = string.Join(',', contactlst);
                quotation.BillPaidByContact = contact;

                quobillings.Add(new Tuple<int, string, string>(quotation.QuotationId, Accountingaddress, contact));
            }
            return quobillings;
        }

        //Export Quotation Audit details
        public async Task<QuotationExportDataResponse> GetQuotationSummaryAuditExportDetails(QuotationSummaryGenRequest request)
        {
            var newRequest = CreateNewRequstForQuotation(request);

            var quAudit = GetQuotationAuditExport(newRequest);

            var quIds = quAudit.GroupBy(x => x.IdQuotation).Select(x => x.Key).Distinct();
            var BookingIds = quAudit.GroupBy(x => x.IdBooking).Select(x => x.Key).Distinct();
            var factoryIds = await quAudit.GroupBy(x => x.IdQuotationNavigation.FactoryId).Select(x => x.Key).Distinct().ToListAsync();
            var customerIds = await quAudit.GroupBy(x => x.IdQuotationNavigation.CustomerId).Select(x => x.Key).Distinct().ToListAsync();
            var supplierIds = await quAudit.GroupBy(x => x.IdQuotationNavigation.SupplierId).Select(x => x.Key).Distinct().ToListAsync();

            //get the Quotation Audit details
            var quAuditDetails = await _QuotationRepository.GetQuotationAuditExport(quIds);

            //get the customer,supplier and factory Address
            var factAddressList = await _supplierRepository.GetSupplierAddressDataByIds(factoryIds);
            var customeraddress = await _customerRepository.GetCustomerAddressByCusIds(customerIds);
            var supplieraddress = await _supplierRepository.GetSupplierOfficeAddressBySupplierIds(supplierIds);

            //get the customer,supplier and factory contacts
            var lstcuscontact = await _QuotationRepository.GetQuotationCustomerContactsByIds(quIds);
            var lstsupcontact = await _QuotationRepository.GetQuotationSupplierContactsByIds(quIds);
            var lstfactcontact = await _QuotationRepository.GetQuotationFactoryContactsByIds(quIds);




            var quobillings = GetQuotationBillData(quAuditDetails, factAddressList, customeraddress, supplieraddress, lstcuscontact,
                                                            lstsupcontact, lstfactcontact);

            var quotationAuditDetais = await _QuotationRepository.GetQuotationAuditDetails(quIds);

            var quotationAuditInvoice = await _QuotationRepository.InvoiceDetailsByAuditId(BookingIds);

            var quotationAuditReport = await _QuotationRepository.GetQuotationAuditReportDetails(BookingIds);



            var response = new QuotationExportDataResponse();

            var enumEntityName = (Company)_filterService.GetCompanyId();
            string entityName = enumEntityName.ToString().ToUpper();
            int loginUserTypeId = (int)_ApplicationContext.UserType;


            //get customer id list
            var customerIdList = quAuditDetails?.Select(x => x.CustomerId).Distinct().ToList();

            //get supplier id list
            var supplierIdList = quAuditDetails?.Select(x => x.SupplierId).Distinct().ToList();

            //get supplier code list
            var supplierCodeList = await _supplierManager.GetSupplierCode(customerIdList, supplierIdList);


            response.QuotationAuditExportList = QuotationMap.GetQuotationSummaryAuditExport(quAuditDetails,
                                       quobillings, factAddressList, quotationAuditDetais, quotationAuditReport, quotationAuditInvoice, entityName,
                                       loginUserTypeId, supplierCodeList);

            if (response.QuotationAuditExportList == null || !response.QuotationAuditExportList.Any())
                response.Result = QuotationExportResult.NotFound;
            else
                response.Result = QuotationExportResult.Success;
            return response;
        }

        //Export Quotation inspection details
        public async Task<QuotationExportDataResponse> GetQuotationSummaryInspExportDetails(QuotationSummaryGenRequest request)
        {
            bool isInternalUser = _ApplicationContext.UserType == UserTypeEnum.InternalUser;
            var newRequest = CreateNewRequstForQuotation(request);

            var quInspProd = GetQuotationInspExport(newRequest);

            var quIds = quInspProd.GroupBy(x => x.IdQuotation).Select(x => x.Key).Distinct();
            var BookingIds = quInspProd.GroupBy(x => x.ProductTran.InspectionId).Select(x => x.Key).Distinct();
            var factoryIds = await quInspProd.GroupBy(x => x.IdQuotationNavigation.FactoryId).Select(x => x.Key).Distinct().ToListAsync();
            var customerIds = await quInspProd.GroupBy(x => x.IdQuotationNavigation.CustomerId).Select(x => x.Key).Distinct().ToListAsync();
            var supplierIds = await quInspProd.GroupBy(x => x.IdQuotationNavigation.SupplierId).Select(x => x.Key).Distinct().ToListAsync();

            //get the Quotation product details
            var quInspProdDetails = await _QuotationRepository.GetQuotationInspProductExport(quIds);

            //get the customer,supplier and factory Address
            var factAddressList = await _supplierRepository.GetSupplierAddressDataByIds(factoryIds);
            var customeraddress = await _customerRepository.GetCustomerAddressByCusIds(customerIds);
            var supplieraddress = await _supplierRepository.GetSupplierOfficeAddressBySupplierIds(supplierIds);

            //get the customer,supplier and factory contacts
            var lstcuscontact = await _QuotationRepository.GetQuotationCustomerContactsByIds(quIds);
            var lstsupcontact = await _QuotationRepository.GetQuotationSupplierContactsByIds(quIds);
            var lstfactcontact = await _QuotationRepository.GetQuotationFactoryContactsByIds(quIds);


            var quobillings = GetQuotationBillData(quInspProdDetails, factAddressList, customeraddress, supplieraddress, lstcuscontact,
                                                            lstsupcontact, lstfactcontact);

            //fetch the invoice man day from quotation insp
            var quotationInvoices = await _QuotationRepository.GetquotationInvoiceList(quIds);
            //fetch the invoice details
            var invoicedetails = await _QuotationRepository.InvoiceDetailsBybookingdId(BookingIds);
            var bookingServiceTypeList = await _fBInternalReportRepository.GetServiceTypeList(BookingIds);

            //get brand list
            var brandList = await _insprepo.GetBrandBookingIdsByBookingQuery(BookingIds);

            //get buyer list
            var buyerList = await _insprepo.GetBuyerBookingIdsByBookingQuery(BookingIds);

            //get dept list
            var departmentList = await _insprepo.GetDeptBookingIdsByBookingQuery(BookingIds);

            //get the po data mapped with the booking products
            var bookingProductsPoData = await _QuotationRepository.GetBookingProductsPoListByProductRefIds(BookingIds);

            var enumEntityName = (Company)_filterService.GetCompanyId();
            string entityName = enumEntityName.ToString().ToUpper();
            int loginUserTypeId = (int)_ApplicationContext.UserType;

            //get supplier code  - pass cus id sup id

            //get customer id list
            var customerIdList = quInspProdDetails?.Select(x => x.CustomerId).Distinct().ToList();

            //get supplier id list
            var supplierIdList = quInspProdDetails?.Select(x => x.SupplierId).Distinct().ToList();

            //get supplier code list
            var supplierCodeList = await _supplierManager.GetSupplierCode(customerIdList, supplierIdList);

            var response = new QuotationExportDataResponse();
            response.QuotationInspProdExportList = QuotationMap.GetQuotationSummaryInspExport(quInspProdDetails,
                                     bookingServiceTypeList, quotationInvoices, quobillings,
                                      invoicedetails, factAddressList, bookingProductsPoData, brandList, buyerList, departmentList, isInternalUser, entityName,
                                      loginUserTypeId, supplierCodeList);




            if (response.QuotationInspProdExportList == null || !response.QuotationInspProdExportList.Any())
                response.Result = QuotationExportResult.NotFound;
            else
                response.Result = QuotationExportResult.Success;

            return response;
        }

        public async Task<QuotationDataSummaryResponse> GetQuotationList(QuotationSummaryGenRequest request, bool Isexpot = false)
        {
            QuotationItemRepoResponse quotationItemData = new QuotationItemRepoResponse();
            if (request.Index == 0)
                request.Index = 1;

            if (request.PageSize == 0)
                request.PageSize = 10;

            //fetch ADEO customer IDs from appsettings
            int adeoCustomerId = 0;
            var customQuotationCustomerIds = _Configuration["CustomerAdeo"].Split(',').Where(str => int.TryParse(str, out adeoCustomerId)).Select(str => adeoCustomerId).ToList();


            //if external user , take only customer confirmed , rejected , sent 
            if (_ApplicationContext.UserType != UserTypeEnum.InternalUser)
            {
                var statusidlist = new int[] { (int)QuotationStatus.CustomerValidated, (int)QuotationStatus.CustomerRejected, (int)QuotationStatus.SentToClient };

                request.Statusidlst = request.Statusidlst != null && request.Statusidlst.Count() == 0 ? statusidlist : statusidlist.Where(x => request.Statusidlst.Any(y => y == x)).ToArray();
            }
            else
            {
                var accesslocidlist = _ApplicationContext.LocationList != null ? _ApplicationContext.LocationList.ToList() : new List<int>();

                if (request.Officeidlst != null && request.Officeidlst.Any())
                {
                    request.Officeidlst = accesslocidlist.Where(x => request.Officeidlst.Any(y => x == y)).ToList();
                }
                else
                {
                    request.Officeidlst = accesslocidlist;
                }
            }
            //filter data by user type
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        request.Customerid = request?.Customerid == null || request?.Customerid == 0 ? _ApplicationContext.CustomerId : request?.Customerid.Value;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        request.Factoryidlst = request.Factoryidlst != null && request.Factoryidlst.Count() > 0 ? request.Factoryidlst : new List<int>().Append(_ApplicationContext.FactoryId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        request.Supplierid = request?.Supplierid == null || request?.Supplierid == 0 ? _ApplicationContext.SupplierId : request?.Supplierid.Value;
                        break;
                    }
            }

            var newRequest = new QuotationSummaryRepoRequest
            {
                Customerid = request.Customerid,
                Factoryidlst = request.Factoryidlst,
                Fromdate = request.Fromdate?.ToDateTime(),
                Todate = request.Todate?.ToDateTime(),
                Supplierid = request.Supplierid,
                Statusidlst = request.Statusidlst,
                Index = request.Index,
                PageSize = request.PageSize,
                Skip = (request.Index - 1) * request.PageSize,
                ServiceId = request.ServiceId,
                Officeidlst = request.Officeidlst,
                No = request.Searchtypetext,
                SearchTypeId = request.Searchtypeid,
                BillPaidBy = _ApplicationContext.UserType == UserTypeEnum.Customer ? (int)QuotationPaidBy.customer : 0,
                ServiceTypelst = request.ServiceTypelst,
                AdvancedSearchtypeid = request.AdvancedSearchtypeid.Trim().ToLower(),
                AdvancedSearchtypetext = request.AdvancedSearchtypetext?.Trim(),
                DateTypeId = request.Datetypeid,
                BrandIdList = request.BrandIdList,
                BuyerIdList = request.BuyerIdList,
                DeptIdList = request.DeptIdList,
                IsEAQF = request.IsEAQF
            };

            if (request.Searchtypeid == (int)SearchType.CustomerBookingNo && request.ServiceId == Service.AuditId)
            {
                return new QuotationDataSummaryResponse { Result = QuotationDataSummaryResult.NotFound };
            }

            IQueryable<int> QuIds;

            if (request.ServiceId == Service.InspectionId)
            {
                QuIds = GetQuotationInspId(newRequest);
            }
            else
            {
                QuIds = GetQuotationAuditId(newRequest);
            }

            if (!QuIds.Any())
            {
                return new QuotationDataSummaryResponse { Result = QuotationDataSummaryResult.NotFound };
            }


            var data = _QuotationRepository.GetQuotationItemByBookingAndAudit(QuIds);

            if (data == null)
                return new QuotationDataSummaryResponse { Result = QuotationDataSummaryResult.NotFound };

            int totalCount = await data.CountAsync();

            var statusItems = await data.Select(x => new { x.StatusId, x.StatusName }).GroupBy(p => new { p.StatusId, p.StatusName }, p => p, (key, _data) =>
                new QuotationSummaryStatus
                {
                    Id = key.StatusId,
                    StatusName = key.StatusName,
                    TotalCount = _data.Count()
                }).OrderBy(x => x.Id).ToListAsync();

            var quotationItems = await data.Skip(newRequest.Skip).Take(newRequest.PageSize).ToListAsync();

            var quotationIds = quotationItems.Select(x => x.QuotationId).ToList();

            //fetch the invoice details from quotation insp
            IEnumerable<QuotationInsp> quotationInsp = null;
            IEnumerable<QuInspProduct> quotationInspProd = null;
            IEnumerable<QuotationInvoiceItem> quotationAuditandInvoices = null;
            List<BookingBrandAccess> brandList = null;
            List<BookingDeptAccess> departmentList = null;

            if (quotationIds != null && request.ServiceId == Service.AuditId)
            {
                quotationAuditandInvoices = await _QuotationRepository.GetQuotationAuditandInvoice(quotationIds.ToList());
            }
            else if (quotationIds != null && request.ServiceId == Service.InspectionId)
            {
                quotationInsp = await _QuotationRepository.GetQuotationInspList(quotationIds);
                quotationInspProd = await _QuotationRepository.GetQuotationInspProdList(quotationIds);

                var bookingIdList = quotationInsp.Select(x => x.BookingId).ToList();

                //get brand list
                brandList = await _insprepo.GetBrandBookingIdsByBookingIds(bookingIdList);

                //get dept list
                departmentList = await _insprepo.GetDeptBookingIdsByBookingIds(bookingIdList);
            }

            var _statusList = statusItems.Select(x => QuotationMap.GetQuotationStatuswithColor(x));

            //get customer id list
            var customerIdList = quotationItems?.Select(x => x.CustomerId).Distinct().ToList();

            //get supplier id list
            var supplierIdList = quotationItems?.Select(x => x.SupplierId).Distinct().ToList();

            //get supplier code list
            var supplierCodeList = await _supplierManager.GetSupplierCode(customerIdList, supplierIdList);

            IEnumerable<QuotationItem> items = null;
            if (quotationIds != null && request.ServiceId == Service.InspectionId)
            {
                items = quotationItems.Select(x => QuotationMap.GetQuotationInspItem(x, GetAbilities, customQuotationCustomerIds,
               quotationInsp, quotationInspProd, brandList, departmentList, supplierCodeList));
            }
            else if (quotationIds != null && request.ServiceId == Service.AuditId)
            {
                items = quotationItems.Select(x => QuotationMap.GetQuotatioAuditItem(x, GetAbilities, customQuotationCustomerIds,
              quotationAuditandInvoices, supplierCodeList));
            }


            return new QuotationDataSummaryResponse
            {
                Result = QuotationDataSummaryResult.Success,
                Data = items != null ? items.OrderByDescending(x => x.QuotationId) : items,
                Index = request.Index,
                PageSize = request.PageSize,
                PageCount = (totalCount / request.PageSize) + (totalCount % request.PageSize > 0 ? 1 : 0),
                TotalCount = totalCount,
                StatusList = _statusList
            };


        }
        private IQueryable<QuInspProduct> GetQuotationInspExport(QuotationSummaryRepoRequest request)
        {
            var dataInsp = _QuotationRepository.GetQuotationInspProductList(request);

            if (!string.IsNullOrWhiteSpace(request.No) && Convert.ToInt32(request.No) > 0)
            {
                if (request.SearchTypeId == (int)SearchType.CustomerBookingNo)
                    dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.CustomerBookingNo == request.No);
                else if (request.SearchTypeId == (int)SearchType.BookingNo)
                    dataInsp = dataInsp.Where(x => x.ProductTran.InspectionId == Convert.ToInt32(request.No));
                else if (request.SearchTypeId == (int)SearchType.QuotationNo)
                    dataInsp = dataInsp.Where(x => x.IdQuotationNavigation.Id == Convert.ToInt32(request.No));
            }
            if (request.Fromdate != null || request.Todate != null)
            {

                if (request.DateTypeId.Trim().ToLower() == "servicedate")
                {
                    if (request.Fromdate != null)
                        dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.ServiceDateFrom >= request.Fromdate.Value);

                    if (request.Todate != null)
                        dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.ServiceDateTo <= request.Todate.Value);
                }
                else if (request.DateTypeId.Trim().ToLower() == "quotationdate")
                {
                    if (request.Fromdate != null)
                        dataInsp = dataInsp.Where(x => x.IdQuotationNavigation.CreatedDate >= request.Fromdate.Value);

                    if (request.Todate != null)
                        dataInsp = dataInsp.Where(x => x.IdQuotationNavigation.CreatedDate <= request.Todate.Value);
                }


            }
            if (dataInsp.Any() && request.BillPaidBy == (int)QuotationPaidBy.customer)
            {
                dataInsp = dataInsp.Where(x => x.IdQuotationNavigation.BillingPaidById == request.BillPaidBy);
            }

            if (!string.IsNullOrEmpty(request.AdvancedSearchtypetext?.Trim()))
            {
                if (request.AdvancedSearchtypeid.Trim().ToLower() == "factoryref")
                {
                    dataInsp = dataInsp.Where(x => EF.Functions.Like(x.ProductTran.Product.FactoryReference.Trim(), $"%{request.AdvancedSearchtypetext}%"));
                }

                else
                {
                    dataInsp = dataInsp.Where(x => x.ProductTran.Product != null && EF.Functions.Like(x.ProductTran.Product.ProductId.Trim(), $"%{request.AdvancedSearchtypetext}%"));
                }
            }

            if (request.ServiceTypelst != null && request.ServiceTypelst.Any())
            {
                dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.InspTranServiceTypes.Any(y => request.ServiceTypelst.Contains(y.ServiceTypeId)));
            }

            //filter by brand id list
            if (request.BrandIdList != null && request.BrandIdList.Any())
            {
                dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.InspTranCuBrands.Any(y => y.Active && request.BrandIdList.Contains(y.BrandId)));
            }

            //filter by dept id list
            if (request.DeptIdList != null && request.DeptIdList.Any())
            {
                dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.InspTranCuDepartments.Any(y => y.Active && request.DeptIdList.Contains(y.DepartmentId)));
            }

            //filter by buyer id list
            if (request.BuyerIdList != null && request.BuyerIdList.Any())
            {
                dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.InspTranCuBuyers.Any(y => y.Active && request.BuyerIdList.Contains(y.BuyerId)));
            }

            return dataInsp;
        }

        private IQueryable<int> GetQuotationInspId(QuotationSummaryRepoRequest request)
        {
            var dataInsp = _QuotationRepository.GetQuotationInspProductList(request);

            if (!string.IsNullOrWhiteSpace(request.No) && Convert.ToInt32(request.No) > 0)
            {
                if (request.SearchTypeId == (int)SearchType.CustomerBookingNo)
                    dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.CustomerBookingNo == request.No);
                else if (request.SearchTypeId == (int)SearchType.BookingNo)
                    dataInsp = dataInsp.Where(x => x.ProductTran.InspectionId == Convert.ToInt32(request.No));
                else if (request.SearchTypeId == (int)SearchType.QuotationNo)
                    dataInsp = dataInsp.Where(x => x.IdQuotationNavigation.Id == Convert.ToInt32(request.No));
            }
            if (request.Fromdate != null || request.Todate != null)
            {

                if (request.DateTypeId.Trim().ToLower() == "servicedate")
                {
                    dataInsp = dataInsp.Where(x => !((x.ProductTran.Inspection.ServiceDateFrom > request.Todate.Value) || (x.ProductTran.Inspection.ServiceDateTo < request.Fromdate.Value)));
                }
                else if (request.DateTypeId.Trim().ToLower() == "quotationdate")
                {
                    if (request.Fromdate != null)
                        dataInsp = dataInsp.Where(x => x.IdQuotationNavigation.CreatedDate >= request.Fromdate.Value);

                    if (request.Todate != null)
                        dataInsp = dataInsp.Where(x => x.IdQuotationNavigation.CreatedDate <= request.Todate.Value);
                }


            }
            if (dataInsp.Any() && request.BillPaidBy == (int)QuotationPaidBy.customer)
            {
                dataInsp = dataInsp.Where(x => x.IdQuotationNavigation.BillingPaidById == request.BillPaidBy);
            }

            if (request.IsEAQF.GetValueOrDefault())
            {
                dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.IsEaqf == request.IsEAQF.GetValueOrDefault());
            }

            if (!string.IsNullOrEmpty(request.AdvancedSearchtypetext?.Trim()))
            {
                if (request.AdvancedSearchtypeid.Trim().ToLower() == "factoryref")
                {
                    dataInsp = dataInsp.Where(x => EF.Functions.Like(x.ProductTran.Product.FactoryReference.Trim(), $"%{request.AdvancedSearchtypetext}%"));
                }

                else
                {
                    dataInsp = dataInsp.Where(x => x.ProductTran.Product != null && EF.Functions.Like(x.ProductTran.Product.ProductId.Trim(), $"%{request.AdvancedSearchtypetext}%"));
                }
            }

            if (request.ServiceTypelst != null && request.ServiceTypelst.Any())
            {
                dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.InspTranServiceTypes.Any(y => request.ServiceTypelst.Contains(y.ServiceTypeId)));
            }

            //filter by brand id list
            if (request.BrandIdList != null && request.BrandIdList.Any())
            {
                dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.InspTranCuBrands.Any(y => y.Active && request.BrandIdList.Contains(y.BrandId)));
            }

            //filter by dept id list
            if (request.DeptIdList != null && request.DeptIdList.Any())
            {
                dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.InspTranCuDepartments.Any(y => y.Active && request.DeptIdList.Contains(y.DepartmentId)));
            }

            //filter by buyer id list
            if (request.BuyerIdList != null && request.BuyerIdList.Any())
            {
                dataInsp = dataInsp.Where(x => x.ProductTran.Inspection.InspTranCuBuyers.Any(y => y.Active && request.BuyerIdList.Contains(y.BuyerId)));
            }

            var data = dataInsp.GroupBy(x => x.IdQuotation).Select(x => x.Key).Distinct();
            return data;
        }
        private IQueryable<QuQuotationAudit> GetQuotationAuditExport(QuotationSummaryRepoRequest request)
        {
            var dataAudits = _QuotationRepository.GetQuotationAuditList(request);
            if (!string.IsNullOrWhiteSpace(request.No) && Convert.ToInt32(request.No) > 0)
            {
                if (request.SearchTypeId == (int)SearchType.BookingNo)
                    dataAudits = dataAudits.Where(x => x.IdBooking == Convert.ToInt32(request.No));
                else if (request.SearchTypeId == (int)SearchType.QuotationNo)
                    dataAudits = dataAudits.Where(x => x.IdQuotationNavigation.Id == Convert.ToInt32(request.No));
            }

            if (request.Fromdate != null || request.Todate != null)
            {
                if (request.DateTypeId.Trim().ToLower() == "servicedate")
                {
                    if (request.Fromdate != null)
                        dataAudits = dataAudits.Where(x => x.IdBookingNavigation.ServiceDateFrom >= request.Fromdate.Value);

                    if (request.Todate != null)
                        dataAudits = dataAudits.Where(x => x.IdBookingNavigation.ServiceDateTo <= request.Todate.Value);
                }
                else if (request.DateTypeId.Trim().ToLower() == "quotationdate")
                {
                    if (request.Fromdate != null)
                        dataAudits = dataAudits.Where(x => x.IdQuotationNavigation.CreatedDate >= request.Fromdate.Value);

                    if (request.Todate != null)
                        dataAudits = dataAudits.Where(x => x.IdQuotationNavigation.CreatedDate <= request.Todate.Value);

                }
            }

            //filter by brand id list
            if (request.BrandIdList != null && request.BrandIdList.Any())
            {
                dataAudits = dataAudits.Where(x => request.BrandIdList.Contains(x.IdBookingNavigation.BrandId.GetValueOrDefault()));
            }

            //filter by dept id list
            if (request.DeptIdList != null && request.DeptIdList.Any())
            {
                dataAudits = dataAudits.Where(x => request.DeptIdList.Contains(x.IdBookingNavigation.DepartmentId.GetValueOrDefault()));
            }
            return dataAudits;
        }
        private IQueryable<int> GetQuotationAuditId(QuotationSummaryRepoRequest request)
        {
            var dataAudits = _QuotationRepository.GetQuotationAuditList(request);
            if (!string.IsNullOrWhiteSpace(request.No) && Convert.ToInt32(request.No) > 0)
            {
                if (request.SearchTypeId == (int)SearchType.BookingNo)
                    dataAudits = dataAudits.Where(x => x.IdBooking == Convert.ToInt32(request.No));
                else if (request.SearchTypeId == (int)SearchType.QuotationNo)
                    dataAudits = dataAudits.Where(x => x.IdQuotationNavigation.Id == Convert.ToInt32(request.No));
            }

            if (request.Fromdate != null || request.Todate != null)
            {
                if (request.DateTypeId.Trim().ToLower() == "servicedate")
                {
                    if (request.Fromdate != null)
                        dataAudits = dataAudits.Where(x => x.IdBookingNavigation.ServiceDateFrom >= request.Fromdate.Value);

                    if (request.Todate != null)
                        dataAudits = dataAudits.Where(x => x.IdBookingNavigation.ServiceDateTo <= request.Todate.Value);
                }
                else if (request.DateTypeId.Trim().ToLower() == "quotationdate")
                {
                    if (request.Fromdate != null)
                        dataAudits = dataAudits.Where(x => x.IdQuotationNavigation.CreatedDate >= request.Fromdate.Value);

                    if (request.Todate != null)
                        dataAudits = dataAudits.Where(x => x.IdQuotationNavigation.CreatedDate <= request.Todate.Value);

                }
            }

            //filter by brand id list
            if (request.BrandIdList != null && request.BrandIdList.Any())
            {
                dataAudits = dataAudits.Where(x => request.BrandIdList.Contains(x.IdBookingNavigation.BrandId.GetValueOrDefault()));
            }

            //filter by dept id list
            if (request.DeptIdList != null && request.DeptIdList.Any())
            {
                dataAudits = dataAudits.Where(x => request.DeptIdList.Contains(x.IdBookingNavigation.DepartmentId.GetValueOrDefault()));
            }

            var data = dataAudits.GroupBy(x => x.IdQuotation).Select(x => x.Key).Distinct();
            return data;
        }




        private async Task<BookingDateChangeInfo> VerifyServiceDateChange(QuotationDetails data)
        {
            try
            {
                List<int> LstBookingId = data.OrderList.Select(x => x.Id).ToList();
                List<BookingDate> lstbookingDate = new List<BookingDate>();
                if (data.Service?.Id == (int)Service.InspectionId)
                {
                    lstbookingDate = await _insprepo.getListInspBookingDateDetails(LstBookingId);
                }
                else if (data.Service?.Id == (int)Service.AuditId)
                {
                    lstbookingDate = await _audprepo.getListAuditBookingDateDetails(LstBookingId);
                }
                else
                {
                    return new BookingDateChangeInfo() { Result = BookingDateChangeInfoResult.ServiceNotFound };
                }
                if (!lstbookingDate.Any())
                {
                    return new BookingDateChangeInfo() { Result = BookingDateChangeInfoResult.NodateFound };
                }
                foreach (var order in data.OrderList)
                {
                    // get all the dates from service date
                    var bookingDate = lstbookingDate.Where(x => x.BookingId == order.Id).FirstOrDefault();

                    if (bookingDate == null || bookingDate.ServiceDateTo == null || bookingDate.ServiceDateFrom == null)
                        return new BookingDateChangeInfo() { Result = BookingDateChangeInfoResult.NodateFound };

                    var bookingDaterange = Enumerable.Range(0, 1 + bookingDate.ServiceDateTo.Subtract(bookingDate.ServiceDateFrom).Days)
                                       .Select(offset => bookingDate.ServiceDateFrom.AddDays(offset)).ToArray();
                    foreach (var quoservicedate in order.QuotationMandayList)
                    {


                        // verify the date include in quotation booking man day rate
                        if (!bookingDaterange.Any(x => x.Date
                        == DateTime.ParseExact(quoservicedate.ServiceDate, StandardDateFormat, CultureInfo.InvariantCulture).Date))
                        {
                            var lstpreviousdate = order.QuotationMandayList.Select(x => DateTime.ParseExact(quoservicedate.ServiceDate,
                                  StandardDateFormat, CultureInfo.InvariantCulture).Date).ToList();
                            return new BookingDateChangeInfo()
                            {
                                BookingId = order.Id,
                                ServiceDateFrom = bookingDate.ServiceDateFrom.ToString(StandardDateFormat),
                                ServiceDateTo = bookingDate.ServiceDateTo.ToString(StandardDateFormat),
                                PreviousServiceDateFrom = lstpreviousdate.Min().ToString(StandardDateFormat),
                                PreviousServiceDateTo = lstpreviousdate.Max().ToString(StandardDateFormat),
                                Result = BookingDateChangeInfoResult.DateChanged
                            };
                        }
                    }
                }
                return new BookingDateChangeInfo() { Result = BookingDateChangeInfoResult.Verified };
            }
            catch (Exception ex)
            {
                return new BookingDateChangeInfo() { Result = BookingDateChangeInfoResult.Error };
            }
        }

        public async Task<QuotationResponse> GetQuotation(int? id)
        {
            var response = new QuotationResponse();

            // Current Quotation
            if (id != null && id.Value > 0)
            {
                response.Model = await GetQuotationDetails(id.Value);

                if (response.Model == null)
                    return new QuotationResponse { Result = QuotationResult.CurrentQuotationNotFound };
            }
            else
            {
                // if new quotation and user has not quotation role, no need to show  screen 
                if (!_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.QuotationRequest))
                    return new QuotationResponse { Result = QuotationResult.NoAccess };
            }


            // country list
            response.CountryList = _locManager.GetCountries();

            if (response.CountryList == null || !response.CountryList.Any())
                return new QuotationResponse { Result = QuotationResult.CannotFindCountryList };

            //Service List 
            response.ServiceList = await _referenceManager.GetServices();

            if (response.ServiceList == null || !response.ServiceList.Any())
                return new QuotationResponse { Result = QuotationResult.CannotFindServiceList };

            //Billing Methods
            var billingMethods = await _QuotationRepository.GetBillMethodList();

            if (billingMethods == null || !billingMethods.Any())
                return new QuotationResponse { Result = QuotationResult.CannotFindBillingMethodList };

            response.BillingMethodList = billingMethods.Select(QuotationMap.GetBillingMethod);

            //Bill Paid By 
            var billPaidByList = await _QuotationRepository.GetPaidByList();

            if (billPaidByList == null || !billPaidByList.Any())
                return new QuotationResponse { Result = QuotationResult.CannotFindBillPaidByList };

            response.BillPaidByList = billPaidByList.Select(QuotationMap.GetBillPaidBy);

            //Billing Entities
            var billingEntities = await _QuotationRepository.GetBillingEntities();

            if (billingEntities == null || !billingEntities.Any())
                return new QuotationResponse { Result = QuotationResult.CannotFindBillingEntities };

            response.BillingEntities = billingEntities;

            var paymentTerms = await _customerManager.GetInvoiceType();

            if (paymentTerms == null || !paymentTerms.CustomerSource.Any())
                return new QuotationResponse { Result = QuotationResult.CannotFindPaymentTerms };

            response.PaymentTermList = paymentTerms.CustomerSource;

            var paymentTermsValue = await _referenceRepository.GetInvoicePaymentTypeList();

            response.PaymentTermsValueList = paymentTermsValue;

            if (id != null && id.Value > 0)
            {
                //CustomerList
                var custList = await _customerRepository.GetEditCustomerListByCountryAndService(response.Model.Country.Id, response.Model.Service.Id);

                if (custList == null || !custList.Any())
                    return new QuotationResponse { Result = QuotationResult.CannotGetCustList };

                response.CustomerList = custList.Select(x => QuotationMap.GetDataSource(x, response.Model.Service.Id));

                //Supplier List
                var suppList = await _supplierRepository.GetSupplierByCustomerId(response.Model.Customer.Id);

                if (suppList == null || !suppList.Any())
                    return new QuotationResponse { Result = QuotationResult.CannotGetSuppList };

                response.SupplierList = suppList.Select(QuotationMap.GetDataSource);

                // factory list
                var factoryList = await _supplierRepository.GetFactoryBySupplierId(response.Model.Supplier.Id);

                if (factoryList == null || !factoryList.Any())
                    return new QuotationResponse { Result = QuotationResult.CannotGetFactoryList };

                response.FactoryList = factoryList.Select(QuotationMap.GetDataSource);
            }

            //OfficeList 
            var office = _officeLocationManager.GetOffices();

            if (office == null || !office.Any())
                return new QuotationResponse { Result = QuotationResult.CannotFindOfficeList };
            response.OfficeList = office.Select(x => QuotationMap.GetDataSource(x));

            //it's billed to contact mandatory when master config is configured, for this reason the master config is returned
            var masterConfig = await _userConfigRepo.GetMasterConfigurationByTypeIds(new List<int>() { (int)EntityConfigMaster.PreInvoiceContactMandatoryInQuotation });
            bool isPreInvoiceContactMandatoryInQuotation = false;
            if (masterConfig.Any())
            {
                var preInvoiceContactMandatoryInQuotation = masterConfig.FirstOrDefault()?.Value;
                bool.TryParse(preInvoiceContactMandatoryInQuotation?.ToLower(), out isPreInvoiceContactMandatoryInQuotation);
            }

            response.IsPreInvoiceContactMandatoryInQuotation = isPreInvoiceContactMandatoryInQuotation;

            // currency List 
            response.CurrencyList = _referenceManager.GetCurrencies();

            if (response.CurrencyList == null || !response.CurrencyList.Any())
                return new QuotationResponse { Result = QuotationResult.CannotFindCurrencies };

            response.Abilities = GetAbilities(id == null ? (QuotationStatus?)null : response.Model.StatusId);
            response.Result = QuotationResult.Success;
            return response;
        }

        private IEnumerable<QuotationAbility> GetAbilities(QuotationStatus? status)
        {
            var abilities = new List<QuotationAbility>();


            if (_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.QuotationRequest))
            {
                if (status != null)
                {
                    // Quotation => Can Cancl an any times
                    if (status.Value != QuotationStatus.Canceled)
                        abilities.Add(QuotationAbility.CanCancel);

                    // Quotation => can Save => if stattus in verified, cofirmed, or rejected
                    if ((new QuotationStatus[] { QuotationStatus.QuotationCreated, QuotationStatus.QuotationVerified, QuotationStatus.ManagerRejected, QuotationStatus.AERejected
                        //, QuotationStatus.CSRejectedAfterCustomerRejected
                    }).Contains(status.Value))
                        abilities.Add(QuotationAbility.CanSave);
                }
                else
                    abilities.Add(QuotationAbility.CanSave);
            }
            if (_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.QuotationSend))
            {
                if (status != null)
                {
                    // Quotation => can Save => if stattus in verified, cofirmed, or rejected
                    if ((new QuotationStatus[] { QuotationStatus.QuotationVerified, QuotationStatus.ManagerApproved,QuotationStatus.CustomerRejected, QuotationStatus.SentToClient,QuotationStatus.CustomerValidated
                    }).Contains(status.Value))
                        abilities.Add(QuotationAbility.CanSave);
                }
            }

            if (status != null)
            {

                // QuotationManager =>  Can Approve / reject if status is Pending
                if (_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.QuotationManager) && status == QuotationStatus.QuotationCreated)
                    abilities.Add(QuotationAbility.CanApprove);

                //QuotationConfirmation => can Confirm if status is approved  
                //if (_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.QuotationConfirmation) && status == QuotationStatus.Approved)
                //  abilities.Add(QuotationAbility.CanConfirm);

                //QuotationSend => Can send if status is approved or confirmed
                if (_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.QuotationSend) && (new QuotationStatus[] { QuotationStatus.ManagerApproved, QuotationStatus.QuotationVerified, QuotationStatus.CustomerRejected, QuotationStatus.SentToClient }).Contains(status.Value))
                    abilities.Add(QuotationAbility.CanSend);

                // QuotationConfirmation => Can customerConfirm or reject if status is sent
                if (_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.QuotationConfirmation) && status.Value == QuotationStatus.SentToClient)
                    abilities.Add(QuotationAbility.CanCustConfirm);
            }

            return abilities;
        }

        public async Task<QuotationDetails> GetQuotationDetails(int id)
        {

            var quotation = await _QuotationRepository.GetQuotation(id);
            //booking id get from above variable and pass to below to simplify
            var PoDetails = await _QuotationRepository.GetBookingPoDetails(id);
            var bookingIds = PoDetails.QuQuotationInsps.Select(x => x.IdBooking);
            IEnumerable<PoDetails> productDetails = null;
            if (quotation.ServiceId == (int)Service.InspectionId)
            {
                productDetails = await _insprepo.GetBookingPOTransactionDetails(bookingIds.ToList());
            }


            if (quotation == null || PoDetails == null || (quotation.ServiceId == (int)Service.InspectionId && productDetails == null))
                return null;

            var quot = QuotationMap.GetQuotation(quotation, PoDetails, productDetails?.ToList());
            var bookingContainerList = await _insprepo.GetBookingContainer(bookingIds);
            // add check - booking is invoiced
            quot.IsBookingInvoiced = await _insprepo.IsAnyOneBookingInvoiced(bookingIds);

            if (bookingContainerList != null && bookingContainerList.Any())
            {
                var totalContainers = bookingContainerList.Where(x => x.ContainerId.HasValue).Select(x => x.ContainerId).Distinct().Count();
                quot.TotalContainers = totalContainers;
            }

            if (quotation.ServiceId == (int)Service.InspectionId && quotation.QuInspProducts.Any())
            {
                //get list of inspection booking id mapped with quoation
                quot.BookingNo = string.Join(", ", PoDetails.QuQuotationInsps.Select(x => x.IdBooking).ToList());

                //get top 1 active service type abbreviation for inspection. if abbreviation has value show or else show service type name
                quot.ServiceTypeAbbreviation = !string.IsNullOrWhiteSpace(quotation.QuQuotationInsps.FirstOrDefault().IdBookingNavigation.InspTranServiceTypes
                                                .Where(x => x.Active).FirstOrDefault()?.ServiceType?.Abbreviation)
                                               ?
                                              quotation.QuQuotationInsps.FirstOrDefault().IdBookingNavigation.InspTranServiceTypes
                                                .Where(x => x.Active).FirstOrDefault()?.ServiceType?.Abbreviation
                                               :
                                               quotation.QuQuotationInsps.FirstOrDefault().IdBookingNavigation.InspTranServiceTypes
                                                .Where(x => x.Active).FirstOrDefault()?.ServiceType?.Name;

                quot.ETD = string.Join(", ", quotation.QuInspProducts
                            .SelectMany(x => x.ProductTran.InspPurchaseOrderTransactions.Where(y => y.Etd != null)
                                  .Select(y => y.Etd.Value.ToString(StandardDateFormat))).Distinct());

                quot.InspecCreatedDate = string.Join(", ", quotation.QuInspProducts
                        .Select(x => ((TimeSpan)(x.ProductTran.Inspection.ServiceDateTo - x.ProductTran.Inspection.ServiceDateFrom)).Days == 0 ?
                        x.ProductTran.Inspection.ServiceDateTo.ToString(StandardDateFormat)
                        : $"{x.ProductTran.Inspection.ServiceDateFrom.ToString(StandardDateFormat)} " +
                        $"- {x.ProductTran.Inspection.ServiceDateTo.ToString(StandardDateFormat)}").Distinct());

                var data = quotation.QuInspProducts.GroupBy(x => x.ProductTran.InspectionId);

                var list = new List<Order>();
                int gapCustomerId = 0;
                // Dynamic Field Data
                var customQuotationCustomerIds = _Configuration["CustomerGAP"].Split(',').Where(str => int.TryParse(str, out gapCustomerId)).Select(str => gapCustomerId).ToList();

                List<InspectionBookingDFData> inspectionBookingDFData = null;
                if (customQuotationCustomerIds.Contains(quotation.CustomerId))
                {
                    var bookingDFDataList = await _dynamicFieldManager.GetBookingDFDataByBookingIds(bookingIds);
                    if (bookingDFDataList != null && bookingDFDataList.Result == InspectionBookingDFDataResult.Success)
                    {
                        inspectionBookingDFData = bookingDFDataList.bookingDFDataList;
                    }
                }

                foreach (var item in data)
                {
                    list.Add(await GetInspection(item.First().ProductTran.Inspection, false, quotation.QuInspProducts, true, inspectionBookingDFData));
                }

                quot.OrderList = list;

                var inspmanday = await _QuotationRepository.GetQuotationInspManday(id);
                if (quot.OrderList != null && inspmanday != null && inspmanday.Any())
                {
                    var lstquoorder = new List<Order>();
                    foreach (var order in quot.OrderList)
                    {
                        order.QuotationMandayList = inspmanday.Where(x => x.BookingId == order.Id).Select(y => QuotationMap.GetQuQuotationInspMandayDTO(y));
                        lstquoorder.Add(order);
                    }
                    quot.OrderList = lstquoorder;
                }
            }
            else
            {
                //get list of audit booking id mapped with quoation
                quot.BookingNo = string.Join(", ", PoDetails.QuQuotationAudits.Select(x => x.IdBooking).ToList());

                //get top 1 active service type abbreviation for audit. if abbreviation has value show or else show service type name
                quot.ServiceTypeAbbreviation = !string.IsNullOrWhiteSpace(quotation.QuQuotationAudits.FirstOrDefault().IdBookingNavigation.AudTranServiceTypes
                                                .Where(x => x.Active).FirstOrDefault().ServiceType.Abbreviation)
                                               ?
                                              quotation.QuQuotationAudits.FirstOrDefault().IdBookingNavigation.AudTranServiceTypes
                                                .Where(x => x.Active).FirstOrDefault().ServiceType.Abbreviation
                                               :
                                               quotation.QuQuotationAudits.FirstOrDefault().IdBookingNavigation.AudTranServiceTypes
                                                .Where(x => x.Active).FirstOrDefault().ServiceType.Name;


                var audmanday = await _QuotationRepository.GetQuotationAudManday(id);
                if (quot.OrderList != null && audmanday != null && audmanday.Any())
                {
                    var lstquoorder = new List<Order>();
                    foreach (var order in quot.OrderList.ToList())
                    {
                        order.QuotationMandayList = audmanday.Where(x => x.BookingId == order.Id).Select(y => QuotationMap.GetQuQuotationAuditMandayDTO(y)).ToList();
                        lstquoorder.Add(order);
                    }
                    quot.OrderList = lstquoorder;
                }

                quot.InspecCreatedDate = string.Join(", ", quotation.QuQuotationAudits.Select(x => ((TimeSpan)(x.IdBookingNavigation.ServiceDateTo - x.IdBookingNavigation.ServiceDateFrom)).Days == 0 ?
                x.IdBookingNavigation.ServiceDateTo.ToString(StandardDateFormat)
                : $"{x.IdBookingNavigation.ServiceDateFrom.ToString(StandardDateFormat)} - {x.IdBookingNavigation.ServiceDateTo.ToString(StandardDateFormat)}")
                    .Distinct());
            }

            quot.EntityMasterConfigs = await _userConfigRepo.GetMasterConfiguration();
            var entityName = quot.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            quot.EntityName = entityName;

            var entityId = _filterService.GetCompanyId();
            var _settings = _emailManager.GetMailSettingConfiguration(entityId);
            quot.SenderEmail = _settings.SenderEmail;

            // if no pdf generated - create the file
            if (string.IsNullOrEmpty(await _QuotationRepository.GetquotationPdfPath(quot.Id)))
            {
                var document = _quotationPDF.CreateDocument(quot);
                SavePdfReferenceToCloudAndUpdatewithQuotation(document, quot.Id);
            }

            var pdfHistoryList = await _QuotationRepository.GetquotationPdfHistoryList(quot.Id);
            if (pdfHistoryList.Any())
            {
                //GetPdf Versions
                quot.QuotationPDFList = pdfHistoryList
                                         .Select(x => new QuotationPDFVersion
                                         {
                                             Id = x.Id,
                                             FileName = x.FileName,
                                             SendDate = x.UploadDate.GetValueOrDefault().ToString(StandardDateFormat),
                                             FileLink = x.FileUrl
                                         }).ToList();
            }

            return quot;

        }

        public async Task<QuotationDataSourceResponse> GetCustomerList(int countryId, int serviceId)
        {
            var data = await _customerRepository.GetCustomerListByCountryAndService(countryId, serviceId);

            if (data == null || !data.Any())
                return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.NotFound };

            return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.Success, DataSource = data };
        }

        public async Task<QuotationDataSourceResponse> GetSupplierList(int customerId)
        {
            var data = await _supplierRepository.GetSupplierByCustomerId(customerId);

            if (data == null || !data.Any())
                return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.NotFound };


            return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.Success, DataSource = data.Select(QuotationMap.GetDataSource) };
        }

        public async Task<QuotationContactListResponse> GetCustomerContactList(int customerId)
        {
            var data = await _customerContactRepository.GetCustomerContacts(customerId);
            data = data.Where(x => x.Active == true).ToList();

            if (data == null || !data.Any())
                return new QuotationContactListResponse { Result = QuotationContactListResult.NotFound };


            return new QuotationContactListResponse { Result = QuotationContactListResult.Success, Data = data.Select(QuotationMap.GetContact) };
        }


        public async Task<QuotationDataSourceResponse> GetFactoryList(int supplierId)
        {
            var data = await _supplierRepository.GetFactoryBySupplierId(supplierId);

            if (data == null || !data.Any())
                return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.NotFound };


            return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.Success, DataSource = data.Select(QuotationMap.GetDataSource) };
        }


        public async Task<QuotationContactListResponse> GetSupplierContactList(int supplierId, int customerId)
        {
            var data = await _supplierRepository.GetSuppliercontactById(supplierId, customerId);

            if (data == null || !data.Any())
                return new QuotationContactListResponse { Result = QuotationContactListResult.NotFound };


            return new QuotationContactListResponse { Result = QuotationContactListResult.Success, Data = data.Select(x => QuotationMap.GetContact(x, QuotationEntityType.Supplier)) };
        }

        public async Task<QuotationContactListResponse> GetFactoryContactList(int factoryId, int customerId)
        {
            var data = await _supplierRepository.GetSuppliercontactById(factoryId, customerId);

            if (data == null || !data.Any())
                return new QuotationContactListResponse { Result = QuotationContactListResult.NotFound };


            return new QuotationContactListResponse { Result = QuotationContactListResult.Success, Data = data.Select(x => QuotationMap.GetContact(x, QuotationEntityType.Factory)) };
        }

        //get staff list with quoatation and ae access by location and customer
        public async Task<QuotationContactListResponse> GetInternalContactList(int locationId, int customerId)
        {
            //get staff list config with customer in dausercustomer table
            var customerAEList = await _userManager.GetAEByCustomerAndLocation(new List<int>() { locationId }, new List<int>() { customerId });

            //get staff list from hrstaff table whose has quotation profile
            var data = await _hrReposiotry.GetStaffByOfficeIdAndProfile(new List<int>() { (int)HRProfile.Quotation });

            //customerAEList and data both empty 
            if ((customerAEList == null || !customerAEList.Any()) && (data == null || !data.Any()))
                return new QuotationContactListResponse { Result = QuotationContactListResult.NotFound };

            //map the hrstaff list
            var staffHRList = data.Select(QuotationMap.GetContact);

            //map the customer ae list
            var staffAEList = customerAEList.Select(QuotationMap.GetAEContact);

            //get staffids from staffHRList not in staffaelist
            var staffHRIds = (staffHRList.Select(x => x.ContactId)).Except(staffAEList.Select(x => x.ContactId));

            //select staffhrlist with staffids
            staffHRList = staffHRList.Where(x => staffHRIds.Contains(x.ContactId));

            return new QuotationContactListResponse { Result = QuotationContactListResult.Success, Data = staffHRList.Concat(staffAEList) };
        }

        public async Task<QuotationOrderListResponse> GetOrders(FilterOrderRequest request)
        {
            QuotationOrderListResponse response = new QuotationOrderListResponse();
            request.OfficeIds = _ApplicationContext.LocationList != null ? _ApplicationContext.LocationList : new int[] { };
            response = request.ServiceId == Service.AuditId ? await GetAudits(request) : await GetInspections(request);

            if (response != null && response.Result == OrderListResult.Success)
            {
                var bookinid = response.Data.Select(x => x.Id).ToList();
                var mandayresponse = await QuotationManday(new QuotationMandayRequest() { BookingId = bookinid, service = request.ServiceId });
                if (mandayresponse.MandayResult == QuotationMandayResult.Success && mandayresponse.QuotationMandaysList != null && mandayresponse.QuotationMandaysList.Any())
                {
                    var lstquoorder = new List<Order>();
                    foreach (var order in response.Data)
                    {
                        order.QuotationMandayList = mandayresponse.QuotationMandaysList.Where(x => x.BookingId == order.Id).ToList();
                        lstquoorder.Add(order);
                    }
                    response.Data = lstquoorder;
                }
            }
            return response;
        }
        /// <summary>
        /// get status of booking from quotation
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetBookingStatusList(int quotationId)
        {
            return await _QuotationRepository.GetquotationBookingStatus(quotationId);
        }

        public async Task<SaveQuotationResponse> SaveQuotation(SaveQuotationRequest request)
        {
            SaveQuotationResponse response = new SaveQuotationResponse();

            //Check role
            if (_ApplicationContext.RoleList == null || !(_ApplicationContext.RoleList.Contains((int)RoleEnum.QuotationRequest) ||
                                                           _ApplicationContext.RoleList.Contains((int)RoleEnum.QuotationSend)))
                return new SaveQuotationResponse { Result = SaveQuotationResult.NoAccess };

            var verifydatechange = await VerifyServiceDateChange(request.Model);
            //verify the service date change in booking module
            if (verifydatechange.Result != BookingDateChangeInfoResult.Verified)
                return new SaveQuotationResponse { Result = SaveQuotationResult.ServiceDateChanged, ServiceDateChangeInfo = verifydatechange };

            //service id greater than zero and orderlist atleast have one record
            if (request.Model != null && request.Model.Service != null && request.Model.Service.Id > 0 && request.Model.OrderList.Any())
            {

                //if inspection or audit quotation exists, bookingIdList has exists ids 
                var bookingIdList = await IsQuotationExists(request.Model.Service.Id, request.Model.OrderList.Select(x => x.Id), request.Model.Id);

                //inspection or audit quotation exists method will return 
                if (bookingIdList.Any())
                    return new SaveQuotationResponse { Result = SaveQuotationResult.QuotationExists, BookingOrAuditNos = bookingIdList };
            }
            await _eventBookingLog.SaveLogInformation(new EventBookingLogInfo()
            {
                Id = 0,
                AuditId = 0,
                BookingId = 0,
                QuotationId = request.Model.Id,
                //skipclientconfirmation = true then status is update to customer validated. this handle in editquotation method.
                StatusId = !request.Model.skipclientconfirmation ? (int)request.Model.StatusId : (int)QuotationStatus.CustomerValidated,
                LogInformation = JsonConvert.SerializeObject(request.Model)
            });

            //Quotation
            if (request.Model.Id > 0)
                response = await EditQuotation(request);
            else
                response = await AddQuotation(request);

            //factorybookinginfolist atleast have one record
            if (request.FactoryBookingInfoList.Any())
            {
                //update quotation suggested manday
                await UpdateQuotationSuggestedManday(request.FactoryBookingInfoList);
            }

            //quotation status log
            var data = await _QuotationRepository.GetQuotationDataForStatusLogs(response.Item.Id);
            data.ForEach(c => c.StatusChangeDate = DateTime.Now);
            await _eventBookingLog.SaveQuotationStatusLog(data, response.Item.StatusId);

            // 1 Generate PDF FILE
            var document = _quotationPDF.CreateDocument(response.Item);

            //2 Save PDF FILE to cloud
            if (document.Result == DTO.File.FileResult.Success)
            {
                SavePdfReferenceToCloudAndUpdatewithQuotation(document, response.Item.Id);
            }

            return response;
        }




        public string SavePdfReferenceToCloudAndUpdatewithQuotation(FileResponse document, int QuotationId)
        {
            var pdfentity = new QuQuotationPdfVersion
            {
                UniqueId = Guid.NewGuid().ToString(),
                FileName = document.Name,
                UploadDate = DateTime.Now,
                QuotationId = QuotationId,
                UserId = _ApplicationContext.UserId,
                SendToClient = false
            };
            // upload file to cloud
            var multipartContent = new MultipartFormDataContent();
            var byteContent = new ByteArrayContent(document.Content);
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
            multipartContent.Add(byteContent, "files", pdfentity.UniqueId);

            using (var httpClient = new HttpClient())
            {
                int ContainerId = (int)FileContainerList.DevContainer;
                if (!Convert.ToBoolean(_Configuration["IsDevelopment_Enviornment"]))
                {
                    ContainerId = (int)FileContainerList.QuotationPdf;
                }
                int EntityId = _filterService.GetCompanyId();
                var cloudFileUrl = _Configuration["FileServer"] + "savefile/" + ContainerId.ToString() + "/" + EntityId.ToString();

                HttpResponseMessage dataResponse = httpClient.PostAsync(cloudFileUrl, multipartContent).Result;

                if (dataResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = dataResponse.Content.ReadAsStringAsync().Result;

                    if (!string.IsNullOrEmpty(result))
                    {
                        var fileResultData = JsonConvert.DeserializeObject<FileUploadResponse>(result);

                        if (fileResultData != null && fileResultData.FileUploadDataList != null
                            && fileResultData.FileUploadDataList.FirstOrDefault() != null
                            && fileResultData.FileUploadDataList.FirstOrDefault().Result == FileUploadResponseResult.Sucess)
                        {
                            pdfentity.FileUrl = fileResultData.FileUploadDataList.FirstOrDefault().FileCloudUri;
                        }
                    }
                }
            }
            _QuotationRepository.Save(pdfentity, false);

            return pdfentity.FileUrl;
        }



        public async Task<string> GetFactoryAddress(int factoryId)
        {
            var factoryAddress = await _supplierRepository.GetSupplierAddressDataList(new int[] { factoryId });

            var address = factoryAddress.FirstOrDefault();

            if (address == null)
                return null;

            if (address.CountryId == (int)CountryEnum.China)
                return $"{address.Address} {address.RegionalLanguageName} . {address.CountryName}/{address.RegionName}/{address.CityName}/{address.CountyName}";

            return $"{address.Address}. {address.CountryName}/{address.RegionName}/{address.CityName}/{address.CountyName}";
        }

        private async Task<SaveQuotationResponse> EditQuotation(SaveQuotationRequest saveRequest)
        {
            var request = saveRequest.Model;
            var quotation = await _QuotationRepository.GetQuotation(request.Id);

            if (quotation == null)
                return new SaveQuotationResponse { Result = SaveQuotationResult.NotFound };

            var oldStatus = quotation.IdStatus;
            // If is rejected or  confirmed , then can forward to manager and user has quotation access.
            bool sendNotif = ((new int[] { (int)QuotationStatus.QuotationVerified, (int)QuotationStatus.AERejected, (int)QuotationStatus.ManagerRejected }).Contains(quotation.IdStatus)
                                && _ApplicationContext.RoleList.Contains((int)RoleEnum.QuotationRequest));

            if (sendNotif)
            {
                if (request.IsToForward)
                    quotation.IdStatus = (int)QuotationStatus.QuotationCreated;
                else
                    quotation.IdStatus = (int)QuotationStatus.QuotationVerified;
            }

            if (request.skipclientconfirmation)
                quotation.IdStatus = (int)QuotationStatus.CustomerValidated;


            quotation.BillingMethodId = request.BillingMethod.Id;
            quotation.BillingPaidById = request.BillingPaidBy.Id;
            quotation.CustomerLegalName = request.CustomerLegalName;
            quotation.SupplierLegalName = request.SupplierLegalName;
            quotation.LegalFactoryName = request.LegalFactoryName;
            quotation.InspectionFees = request.InspectionFees;
            quotation.EstimatedManday = request.EstimatedManday;
            quotation.CurrencyId = request.Currency.Id;
            quotation.TravelCostsAir = request.TravelCostsAir;
            quotation.TravelCostsLand = request.TravelCostsLand;
            quotation.TravelCostsHotel = request.TravelCostsHotel;
            quotation.OtherCosts = request.OtherCosts;
            quotation.Discount = request.Discount;
            quotation.TotalCost = request.TotalCost;
            quotation.ApiRemark = request.ApiRemark;
            quotation.CustomerRemark = request.CustomerRemark;
            quotation.FactoryAddress = request.FactoryAddress;
            quotation.ApiInternalRemark = request.ApiInternalRemark;
            quotation.BillingEntity = request.BillingEntity;
            quotation.PaymentTerms = request.PaymentTerm;
            quotation.RuleId = request.RuleId;
            quotation.OfficeId = request.Office.Id;
            quotation.PaymentTermsValue = request.PaymentTermsValue;
            quotation.PaymentTermsCount = request.PaymentTermsCount;

            // CustomerContact
            _QuotationRepository.RemoveEntities(quotation.QuQuotationCustomerContacts);

            foreach (var item in request.CustomerContactList)
            {
                if (item.Email || item.Quotation || item.InvoiceEmail)
                {
                    var custContact = new QuQuotationCustomerContact
                    {
                        IdContact = item.ContactId,
                        Email = item.Email,
                        Quotation = item.Quotation,
                        InvoiceEmail = item.InvoiceEmail
                    };
                    quotation.QuQuotationCustomerContacts.Add(custContact);
                    _QuotationRepository.AddEntity(custContact);
                }
            }

            //SupplierContact
            _QuotationRepository.RemoveEntities(quotation.QuQuotationSupplierContacts);

            foreach (var item in request.SupplierContactList)
            {
                if (item.Email || item.Quotation || item.InvoiceEmail)
                {
                    var suppContact = new QuQuotationSupplierContact
                    {
                        IdContact = item.ContactId,
                        Email = item.Email,
                        Quotation = item.Quotation,
                        InvoiceEmail = item.InvoiceEmail
                    };
                    quotation.QuQuotationSupplierContacts.Add(suppContact);
                    _QuotationRepository.AddEntity(suppContact);
                }
            }

            //FactoryContact
            _QuotationRepository.RemoveEntities(quotation.QuQuotationFactoryContacts);

            foreach (var item in request.FactoryContactList)
            {
                if (item.Email || item.Quotation || item.InvoiceEmail)
                {
                    var factContact = new QuQuotationFactoryContact
                    {
                        IdContact = item.ContactId,
                        Email = item.Email,
                        Quotation = item.Quotation,
                        InvoiceEmail = item.InvoiceEmail
                    };
                    quotation.QuQuotationFactoryContacts.Add(factContact);
                    _QuotationRepository.AddEntity(factContact);
                }
            }

            //Internal Contact
            _QuotationRepository.RemoveEntities(quotation.QuQuotationContacts);

            foreach (var item in request.InternalContactList)
            {
                var contact = new QuQuotationContact
                {
                    IdContact = item.ContactId,
                    Email = item.Email,
                    Quotation = item.Quotation
                };
                quotation.QuQuotationContacts.Add(contact);
                _QuotationRepository.AddEntity(contact);
            }

            //order list 
            if (request.Service.Id == (int)Service.AuditId)
            {
                _QuotationRepository.RemoveEntities(quotation.QuQuotationAudMandays);
                _QuotationRepository.RemoveEntities(quotation.QuQuotationAudits);
            }
            else
            {
                _QuotationRepository.RemoveEntities(quotation.QuInspProducts);
                _QuotationRepository.RemoveEntities(quotation.QuQuotationInsps);
            }
            foreach (var manDayInsp in quotation.QuQuotationInspMandays.Where(x => x.Active.HasValue && x.Active.Value))
            {
                manDayInsp.Active = false;
                manDayInsp.DeletedDate = DateTime.Now;
            }
            foreach (var item in request.OrderList)
            {
                if (request.Service.Id == (int)Service.InspectionId && item.ProductList != null && item.ProductList.Any())
                {
                    var productList = item.ProductList.GroupBy(x => x.InspPoId);

                    foreach (var product in productList)
                    {
                        var itemProduct = new QuInspProduct
                        {
                            ProductTranId = product.Key,
                            AqlLevelDesc = product.First().AqlLevelDescription,
                            SampleQty = product.First().SampleQty
                        };

                        quotation.QuInspProducts.Add(itemProduct);
                        _QuotationRepository.AddEntity(itemProduct);
                    }
                    var quotationinsp = new QuQuotationInsp
                    {
                        IdBooking = item.Id,
                        InspFees = item.orderCost.InspFees,
                        NoOfManDay = item.orderCost.NoOfManday ?? 0,
                        TravelAir = item.orderCost.TravelAir,
                        TravelHotel = item.orderCost.TravelHotel,
                        TravelLand = item.orderCost.TravelLand,
                        UnitPrice = item.orderCost.UnitPrice,
                        TotalCost = (item.orderCost.InspFees ?? 0) + (item.orderCost.TravelAir ?? 0) + (item.orderCost.TravelLand ?? 0) + (item.orderCost.TravelHotel ?? 0),
                        NoOfTravelManDay = item.orderCost.TravelManday,
                        TravelDistance = item.orderCost.TravelDistance,
                        TravelTime = item.orderCost.TravelTime,
                        CalculatedWorkingHrs = item.orderCost.CalculatedWorkingHours,
                        CalculatedWorkingManDay = item.orderCost.CalculatedManday,
                        Quantity = item.orderCost.Quantity,
                        BilledQtyType = item.orderCost.BilledQtyType
                    };
                    quotation.QuQuotationInsps.Add(quotationinsp);
                    _QuotationRepository.AddEntity(quotationinsp);
                }
                else
                {
                    var quotationAudit = new QuQuotationAudit
                    {
                        IdBooking = item.Id,
                        InspFees = item.orderCost.InspFees,
                        NoOfManDay = item.orderCost.NoOfManday ?? 0,
                        TravelAir = item.orderCost.TravelAir,
                        TravelHotel = item.orderCost.TravelHotel,
                        TravelLand = item.orderCost.TravelLand,
                        UnitPrice = item.orderCost.UnitPrice,
                        TotalCost = (item.orderCost.InspFees ?? 0) + (item.orderCost.TravelAir ?? 0) + (item.orderCost.TravelLand ?? 0) + (item.orderCost.TravelHotel ?? 0),
                        NoOfTravelManDay = item.orderCost.TravelManday,
                        TravelDistance = item.orderCost.TravelDistance,
                        TravelTime = item.orderCost.TravelTime
                    };

                    quotation.QuQuotationAudits.Add(quotationAudit);
                    _QuotationRepository.AddEntity(quotationAudit);
                }
                //update quotation manday
                AddQuotationManday(item, quotation, request.Service.Id);
            }
            await _QuotationRepository.Save();

            var model = await GetQuotationDetails(quotation.Id);

            if (sendNotif)
            {
                if (saveRequest.OnSendEmail != null)
                {
                    try
                    {
                        IEnumerable<User> userList = null;

                        //Get product category details
                        var productCategoryList = model?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetProductCategoryDetails(model?.OrderList?.Select(x => x.Id)) : null;
                        //Get Department details
                        var departmentData = model?.Service?.Id == (int)Service.InspectionId ?
                                                        await _insprepo.GetBookingDepartmentList(model?.OrderList?.Select(x => x.Id)) : null;
                        //Get Brand details
                        var brandData = model?.Service?.Id == (int)Service.InspectionId ?
                                                        await _insprepo.GetBookingBrandList(model?.OrderList?.Select(x => x.Id)) : null;

                        //factory country 
                        int? factoryCountryId = null;
                        if (model?.Factory?.Id > 0)
                        {
                            var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(model.Factory.Id);
                            if (factoryCountryData.Result == SupplierListResult.Success)
                                factoryCountryId = factoryCountryData.countryId;
                        }

                        var userAccessFilter = new UserAccess
                        {
                            OfficeId = model.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).FirstOrDefault(),
                            ServiceId = model.Service.Id,
                            CustomerId = model.Customer.Id,
                            RoleId = (int)RoleEnum.QuotationManager,
                            ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                            DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                            BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                            FactoryCountryId = factoryCountryId
                        };
                        if ((request.StatusId == QuotationStatus.QuotationVerified || request.StatusId == QuotationStatus.AERejected || request.StatusId == QuotationStatus.ManagerRejected) && model.StatusId == QuotationStatus.QuotationCreated)
                        {
                            userList = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                        }
                        else if ((request.StatusId == QuotationStatus.AERejected || request.StatusId == QuotationStatus.ManagerRejected) && model.StatusId == QuotationStatus.QuotationVerified)
                        {
                            userAccessFilter.RoleId = (int)RoleEnum.QuotationSend;
                            userList = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                        }

                        var removetasktype = model.StatusId == QuotationStatus.QuotationVerified ? TaskType.QuotationSent : TaskType.QuotationModify;
                        if (oldStatus == (int)QuotationStatus.AERejected && model.StatusId == QuotationStatus.QuotationVerified)
                        {
                            removetasktype = TaskType.QuotationModify;
                        }
                        // Remove old Tasks related to this quotation 
                        await RemoveTask(removetasktype, model.Id);


                        if (userList != null && userList.Any())
                        {
                            string contactName = "All";

                            if (userList.Count() == 1)
                                contactName = userList.First().FullName;

                            userAccessFilter.RoleId = (int)RoleEnum.QuotationRequest;
                            //quotationUserList 
                            var quotUserList = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                            // Send Emails 
                            var emailRequest = new SendEmailRequest
                            {
                                Subject = $"Quotation Pending - {(model.Service.Id == (int)Service.InspectionId ? "INS" : "AUD")}#{(model.OrderList.Any() ? string.Join(", ", model.OrderList.Select(x => x.Id).ToList()) : "")} / {model.ServiceTypeAbbreviation} / Customer : {model.Customer.Name}, Supplier : {model.Supplier.Name}, {(model.Service.Id == (int)Service.InspectionId ? $"Product Ref : {model.ProductRef}, PO : {model.PoNO}, " : "")}Factory country : {model.FactoryCountry} ",
                                Model = model,
                                RecepientList = userList.Select(x => x.EmailAddress),
                                CcList = quotUserList == null || !quotUserList.Any() ? null : quotUserList.Select(x => x.EmailAddress),
                                RecepitName = contactName
                            };

                            saveRequest.OnSendEmail(emailRequest);

                            var tasktype = model.StatusId == QuotationStatus.QuotationCreated ? TaskType.QuotationToApprove : TaskType.QuotationSent;

                            //Add Task
                            await AddTask(tasktype, model.Id, userList.Select(x => x.Id), new Notification
                            {
                                Title = "LINK Tasks Manager",
                                Message = emailRequest.Subject,
                                Url = saveRequest.Url,
                                TypeId = "Task"
                            });

                            if (quotUserList != null && quotUserList.Any())
                            {
                                // Add Notifications
                                await AddNotification(NotificationType.QuotationModified, model.Id, quotUserList.Select(x => x.Id), new Notification
                                {
                                    Title = "LINK Notification Manager",
                                    Message = emailRequest.Subject,
                                    Url = saveRequest.Url,
                                    TypeId = "Notification"
                                });
                            }

                        }

                        return new SaveQuotationResponse
                        {
                            Item = model,
                            Result = SaveQuotationResult.Success
                        };
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());

                        return new SaveQuotationResponse
                        {
                            Item = model,
                            Result = SaveQuotationResult.SuccessWithBrodcastError
                        };
                    }
                }
            }

            return new SaveQuotationResponse
            {
                Item = model,
                Result = SaveQuotationResult.Success
            };
        }
        /// <summary>
        /// Add quoation manday entity to main(quotation) entity
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private void AddQuotationManday(Order request, QuQuotation entity, int serviceid)
        {
            int userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
            if (request.QuotationMandayList != null && request.QuotationMandayList.Any())
            {
                foreach (var item in request.QuotationMandayList)
                {
                    if (serviceid == (int)Service.InspectionId)
                    {
                        var entityinspManday = QuotationMap.GetQuQuotationInspManday(item, userId);
                        entity.QuQuotationInspMandays.Add(entityinspManday);
                        _QuotationRepository.AddEntity(entityinspManday);
                    }
                    else if (serviceid == (int)Service.AuditId)
                    {
                        var entityaudManday = QuotationMap.GetQuQuotationAuditManday(item, userId);
                        entity.QuQuotationAudMandays.Add(entityaudManday);
                        _QuotationRepository.AddEntity(entityaudManday);
                    }
                }
            }
        }

        private async Task<SaveQuotationResponse> AddQuotation(SaveQuotationRequest saveRequest)
        {
            var request = saveRequest.Model;
            var customer = _customerRepository.GetCustomerByID(request.Customer.Id);

            if (customer == null)
                return new SaveQuotationResponse { Result = SaveQuotationResult.CustomerNotFound };

            QuotationStatus quotStatus = QuotationStatus.QuotationCreated;

            // update the status based on the check box value
            if (request.IsToForward)
                quotStatus = QuotationStatus.QuotationCreated;
            else
                quotStatus = QuotationStatus.QuotationVerified;

            if (request.SkipQuotationSentToClient)
            {
                quotStatus = QuotationStatus.CustomerValidated;
            }

            if (saveRequest.isCallFromEAQF)
            {
                quotStatus = QuotationStatus.CustomerValidated;
            }

            var _entityId = _filterService.GetCompanyId();

            var entity = new QuQuotation
            {
                CountryId = request.Country.Id,
                ServiceId = request.Service.Id,
                BillingMethodId = request.BillingMethod.Id,
                BillingPaidById = request.BillingPaidBy.Id,
                CurrencyId = request.Currency.Id,
                CustomerId = request.Customer.Id,
                ApiRemark = request.ApiRemark,
                CustomerLegalName = request.CustomerLegalName,
                CustomerRemark = request.CustomerRemark,
                Discount = request.Discount,
                EstimatedManday = request.EstimatedManday,
                FactoryId = request.Factory.Id,
                InspectionFees = request.InspectionFees,
                LegalFactoryName = request.LegalFactoryName,
                OfficeId = request.Office.Id,
                OtherCosts = request.OtherCosts,
                SupplierId = request.Supplier.Id,
                SupplierLegalName = request.SupplierLegalName,
                TotalCost = request.TotalCost,
                TravelCostsAir = request.TravelCostsAir,
                TravelCostsHotel = request.TravelCostsHotel,
                TravelCostsLand = request.TravelCostsLand,
                FactoryAddress = request.FactoryAddress,
                IdStatus = (int)quotStatus,
                CreatedDate = DateTime.Now,
                ApiInternalRemark = request.ApiInternalRemark,
                BillingEntity = request.BillingEntity,
                PaymentTerms = request.PaymentTerm,
                EntityId = _entityId,
                RuleId = request.RuleId,
                PaymentTermsValue = request.PaymentTermsValue,
                PaymentTermsCount = request.PaymentTermsCount
            };
            if (request.CustomerContactList != null && request.CustomerContactList.Count() > 0)
            {
                // CustomerContact
                foreach (var item in request.CustomerContactList)
                {
                    if (item.Email || item.Quotation || item.InvoiceEmail)
                    {
                        var custContact = new QuQuotationCustomerContact
                        {
                            IdContact = item.ContactId,
                            Email = item.Email,
                            Quotation = item.Quotation,
                            InvoiceEmail = item.InvoiceEmail
                        };
                        entity.QuQuotationCustomerContacts.Add(custContact);
                        _QuotationRepository.AddEntity(custContact);
                    }
                }
            }

            if (request.SupplierContactList != null && request.SupplierContactList.Count() > 0)
            {
                //SupplierContact
                foreach (var item in request.SupplierContactList)
                {
                    if (item.Email || item.Quotation || item.InvoiceEmail)
                    {
                        var suppContact = new QuQuotationSupplierContact
                        {
                            IdContact = item.ContactId,
                            Email = item.Email,
                            Quotation = item.Quotation,
                            InvoiceEmail = item.InvoiceEmail
                        };

                        entity.QuQuotationSupplierContacts.Add(suppContact);
                        _QuotationRepository.AddEntity(suppContact);
                    }
                }
            }

            if (request.FactoryContactList != null && request.FactoryContactList.Count() > 0)
            {
                //FactoryContact
                foreach (var item in request.FactoryContactList)
                {
                    if (item.Email || item.Quotation || item.InvoiceEmail)
                    {
                        var factContact = new QuQuotationFactoryContact
                        {
                            IdContact = item.ContactId,
                            Email = item.Email,
                            Quotation = item.Quotation,
                            InvoiceEmail = item.InvoiceEmail
                        };
                        entity.QuQuotationFactoryContacts.Add(factContact);
                        _QuotationRepository.AddEntity(factContact);
                    }
                }
            }

            if (request.InternalContactList != null && request.InternalContactList.Count() > 0)
            {
                //Internal Contact
                foreach (var item in request.InternalContactList)
                {
                    if (item.Email || item.Quotation)
                    {
                        var contact = new QuQuotationContact
                        {
                            IdContact = item.ContactId,
                            Email = item.Email,
                            Quotation = item.Quotation
                        };
                        entity.QuQuotationContacts.Add(contact);
                        _QuotationRepository.AddEntity(contact);
                    }
                }
            }

            //order list 
            foreach (var item in request.OrderList)
            {
                item.UserId = request.UserId;
                if (request.Service.Id == (int)Service.InspectionId && item.ProductList != null && item.ProductList.Any())
                {

                    var productList = item.ProductList.GroupBy(x => x.InspPoId);

                    foreach (var product in productList)
                    {
                        var itemProduct = new QuInspProduct
                        {
                            ProductTranId = product.Key,
                            AqlLevelDesc = product.First().AqlLevelDescription,
                            SampleQty = product.First().SampleQty
                        };

                        entity.QuInspProducts.Add(itemProduct);
                        _QuotationRepository.AddEntity(itemProduct);
                    }


                    var quotationinsp = new QuQuotationInsp
                    {
                        IdBooking = item.Id,
                        InspFees = item.orderCost.InspFees,
                        NoOfManDay = item.orderCost.NoOfManday,
                        TravelAir = item.orderCost.TravelAir,
                        TravelHotel = item.orderCost.TravelHotel,
                        TravelLand = item.orderCost.TravelLand,
                        UnitPrice = item.orderCost.UnitPrice,
                        TotalCost = (item.orderCost.InspFees ?? 0) + (item.orderCost.TravelAir ?? 0) + (item.orderCost.TravelLand ?? 0) + (item.orderCost.TravelHotel ?? 0),
                        NoOfTravelManDay = item.orderCost.TravelManday,
                        TravelDistance = item.orderCost.TravelDistance,
                        TravelTime = item.orderCost.TravelTime,
                        CalculatedWorkingHrs = item.orderCost.CalculatedWorkingHours,
                        CalculatedWorkingManDay = item.orderCost.CalculatedManday,
                        Quantity = item.orderCost.Quantity,
                        BilledQtyType = item.orderCost.BilledQtyType
                    };
                    entity.QuQuotationInsps.Add(quotationinsp);
                    _QuotationRepository.AddEntity(quotationinsp);
                }
                else
                {

                    var quotationAudit = new QuQuotationAudit
                    {
                        IdBooking = item.Id,
                        InspFees = item.orderCost.InspFees,
                        NoOfManDay = item.orderCost.NoOfManday ?? 0,
                        TravelAir = item.orderCost.TravelAir,
                        TravelHotel = item.orderCost.TravelHotel,
                        TravelLand = item.orderCost.TravelLand,
                        UnitPrice = item.orderCost.UnitPrice,
                        TotalCost = (item.orderCost.InspFees ?? 0) + (item.orderCost.TravelAir ?? 0) + (item.orderCost.TravelLand ?? 0) + (item.orderCost.TravelHotel ?? 0),
                        NoOfTravelManDay = item.orderCost.TravelManday,
                        TravelDistance = item.orderCost.TravelDistance,
                        TravelTime = item.orderCost.TravelTime
                    };

                    entity.QuQuotationAudits.Add(quotationAudit);
                    _QuotationRepository.AddEntity(quotationAudit);
                }
                //add quoation manday insert
                AddQuotationManday(item, entity, request.Service.Id);
            }
            try
            {
                _QuotationRepository.Save(entity, false);
            }
            catch (Exception ex)
            {

                throw ex;
            }




            if (entity.Id <= 0)
                return new SaveQuotationResponse
                {
                    Result = SaveQuotationResult.CannotSave
                };

            if (saveRequest.isCallFromEAQF)
            {
                return new SaveQuotationResponse
                {
                    Item = new QuotationDetails() { Id = entity.Id },
                    Result = SaveQuotationResult.Success
                };
            }

            var model = await GetQuotationDetails(entity.Id);

            try
            {
                //Get all distinct user with "quotation approve" access based on booking location and office control access
                IEnumerable<User> userList = null;

                //Get product category details
                var productCategoryList = model?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetProductCategoryDetails(model?.OrderList?.Select(x => x.Id)) : null;
                //Get Department details
                var departmentData = model?.Service?.Id == (int)Service.InspectionId ?
                                                await _insprepo.GetBookingDepartmentList(model?.OrderList?.Select(x => x.Id)) : null;
                //Get Brand details
                var brandData = model?.Service?.Id == (int)Service.InspectionId ?
                                                await _insprepo.GetBookingBrandList(model?.OrderList?.Select(x => x.Id)) : null;

                //factory country 
                int? factoryCountryId = null;
                if (model?.Factory?.Id > 0)
                {
                    var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(model.Factory.Id);
                    if (factoryCountryData.Result == SupplierListResult.Success)
                        factoryCountryId = factoryCountryData.countryId;
                }

                var userAccessFilter = new UserAccess
                {
                    OfficeId = model.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).FirstOrDefault(),
                    ServiceId = model.Service.Id,
                    CustomerId = model.Customer.Id,
                    RoleId = (int)RoleEnum.QuotationManager,
                    ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                    DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    FactoryCountryId = factoryCountryId
                };
                if (model.StatusId == QuotationStatus.QuotationCreated)
                {
                    userList = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                }
                else
                {
                    userAccessFilter.RoleId = (int)RoleEnum.QuotationSend;
                    userList = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                }
                userAccessFilter.RoleId = (int)RoleEnum.QuotationRequest;
                //quotationUserList 
                var quotUserList = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                //remove pending quotation request
                if (request.OrderList != null && request.OrderList.Any())
                {
                    foreach (var item in request.OrderList)
                    {
                        // Remove old Tasks related to this booking 
                        await RemoveTask(TaskType.QuotationPending, item.Id);
                    }

                }

                if (userList != null && userList.Any())
                {
                    string contactName = "All";

                    if (userList.Count() == 1)
                        contactName = userList.First().FullName;

                    if (saveRequest.OnSendEmail != null)
                    {
                        // Send Emails 
                        var emailRequest = new SendEmailRequest
                        {
                            Subject = $"Quotation Created - {(model.Service.Id == (int)Service.InspectionId ? "INS" : "AUD")}#{(model.OrderList.Any() ? string.Join(", ", model.OrderList.Select(x => x.Id).ToList()) : "")} / { model.ServiceTypeAbbreviation } / Customer : {model.Customer.Name}, Supplier : {model.Supplier.Name}, {(model.Service.Id == (int)Service.InspectionId ? $"Product Ref : {model.ProductRef}, PO : {model.PoNO}, " : "")}Factory country : {model.FactoryCountry} ",
                            Model = model,
                            RecepientList = userList.Select(x => x.EmailAddress),
                            CcList = quotUserList == null || !quotUserList.Any() ? null : quotUserList.Select(x => x.EmailAddress),
                            RecepitName = contactName
                        };

                        saveRequest.OnSendEmail(emailRequest);

                        var tasktype = model.StatusId == QuotationStatus.QuotationCreated ? TaskType.QuotationToApprove : TaskType.QuotationSent;
                        var masterConfigs = await _userConfigRepo.GetMasterConfiguration();
                        var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                        //Add Task
                        await AddTask(tasktype, model.Id, userList.Select(x => x.Id), new Notification
                        {
                            Title = "LINK Tasks Manager",
                            Message = emailRequest.Subject,
                            Url = string.Format(saveRequest.Url, model.Id, entityName),
                            TypeId = "Task"
                        });

                        if (quotUserList != null && quotUserList.Any())
                        {
                            // Add Notifications
                            await AddNotification(NotificationType.QuotationAdd, model.Id, quotUserList.Select(x => x.Id), new Notification
                            {
                                Title = "LINK Notification Manager",
                                Message = emailRequest.Subject,
                                Url = string.Format(saveRequest.Url, model.Id, entityName),
                                TypeId = "Notification"
                            });
                        }

                    }
                }

                // get quotation from database
                return new SaveQuotationResponse
                {
                    Item = model,
                    Result = entity.Id > 0 ? SaveQuotationResult.Success : SaveQuotationResult.CannotSave
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                // get quotation from database
                return new SaveQuotationResponse
                {
                    Item = model,
                    Result = SaveQuotationResult.SuccessWithBrodcastError
                };
            }
        }

        private async Task<QuotationOrderListResponse> GetInspections(FilterOrderRequest request)
        {
            var data = await _QuotationRepository.GetInspectionList(request);

            if (data == null || !data.Any())
                return new QuotationOrderListResponse { Result = OrderListResult.NotFound };

            var list = new List<Order>();
            // Dynamic Field Data
            int gapCustomerId = 0;
            // Dynamic Field Data
            var customQuotationCustomerIds = _Configuration["CustomerGAP"].Split(',').Where(str => int.TryParse(str, out gapCustomerId)).Select(str => gapCustomerId).ToList();

            List<InspectionBookingDFData> inspectionBookingDFData = null;
            if (customQuotationCustomerIds.Contains(request.CustomerId))
            {
                var bookingDFDataList = await _dynamicFieldManager.GetBookingDFDataByBookingIds(data.Select(x => x.Id).ToList());
                if (bookingDFDataList != null && bookingDFDataList.Result == InspectionBookingDFDataResult.Success)
                {
                    inspectionBookingDFData = bookingDFDataList.bookingDFDataList;
                }
            }
            foreach (var item in data)
            {
                list.Add(await GetInspection(item, true, null, false, inspectionBookingDFData));
            }


            return new QuotationOrderListResponse { Result = OrderListResult.Success, Data = list };
        }

        private async Task<Order> GetInspection(InspTransaction entity, bool searchCombine, IEnumerable<QuInspProduct> inspProductList, bool isEdit, List<InspectionBookingDFData> dFData)
        {
            var rulePriceConfig = new CustomerPriceCardDetails();

            List<int> prevBookingNoList = new List<int>();

            var bookingProductCategory = await _insprepo.GetProductCategoryDetails(new[] { entity.Id });

            string productCategory = string.Join(",", bookingProductCategory.Select(y => y.ProductCategoryName).Distinct().ToArray());

            var containerList = new List<BookingContainersData>();
            if (entity.InspTranServiceTypes.Any(x => x.Active && x.ServiceTypeId == (int)InspectionServiceTypeEnum.Container))
            {
                var bookingContainerResponse = await _bookingManager.GetBookingContainers(entity.Id);
                if (bookingContainerResponse.Result == BookingProductsResponseResult.Success)
                    containerList = bookingContainerResponse.BookingContainerList;
            }

            //get all the previous booking value untill the prev booking value is null
            if (entity.PreviousBookingNo > 0 && entity.PreviousBookingNo != entity.Id)
            {
                int? prevBookingNo = entity.PreviousBookingNo.GetValueOrDefault();
                prevBookingNoList.Add(entity.PreviousBookingNo.GetValueOrDefault());

                //loops untill the prev booking no is null
                do
                {
                    prevBookingNo = await _insprepo.GetPreviousBookingNumber(prevBookingNo.GetValueOrDefault());

                    if (prevBookingNo > 0)
                        prevBookingNoList.Add(prevBookingNo.GetValueOrDefault());
                } while (prevBookingNo > 0 && prevBookingNo != entity.Id && prevBookingNoList.IndexOf(prevBookingNo.GetValueOrDefault()) == -1);

            }

            var order = QuotationMap.GetInspection(entity, inspProductList, productCategory, isEdit, containerList, dFData);
            order.PreviousBookingNo = prevBookingNoList.Distinct().ToList();

            return order;
        }

        private async Task<QuotationOrderListResponse> GetAudits(FilterOrderRequest request)
        {
            var data = await _QuotationRepository.GetAuditList(request);

            if (data == null || !data.Any())
                return new QuotationOrderListResponse { Result = OrderListResult.NotFound };

            return new QuotationOrderListResponse { Result = OrderListResult.Success, Data = data.Select(QuotationMap.GetAudit) };
        }

        private async Task SendEMailAndBrodcast(QuotationDetails model, Func<Task<IEnumerable<User>>> roleTO, Func<Task<IEnumerable<User>>> roleCC, SetStatusBusinessRequest request, string subject, TaskType taskType, TaskType oldtask, NotificationType notificationType, IEnumerable<FileResponse> fileList = null, Func<Task<IEnumerable<User>>> roleSchedule = null)
        {
            int locationId = model.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).First();
            var userToList = await roleTO();
            var userCCList = await roleCC();
            // Remove old Tasks related to this quotation 
            if (oldtask != TaskType.None)
            {
                await RemoveTask(oldtask, request.Id);
            }
            if (userToList != null && userToList.Any())
            {
                string contactName = "All";

                if (userToList.Count() == 1)
                    contactName = userToList.First().FullName;

                if (request.OnSendEmail != null)
                {
                    // Send Emails 
                    var emailRequest = new SendEmailRequest
                    {
                        Subject = subject,
                        Model = model,
                        RecepientList = userToList.Select(x => x.EmailAddress),
                        CcList = userCCList == null || !userCCList.Any() ? null : userCCList.Select(x => x.EmailAddress),
                        RecepitName = contactName,
                        FileList = fileList
                    };

                    //Add Task
                    if (taskType != TaskType.None)
                    {
                        // create booking task for schedule inspection
                        if (TaskType.ScheduleInspection != taskType)
                        {
                            await AddTask(taskType, model.Id, userToList.Select(x => x.Id), new Notification
                            {
                                Title = "LINK Tasks Manager",
                                Message = emailRequest.Subject,
                                Url = request.Url,
                                TypeId = "Task"
                            });
                        }
                        else
                        {
                            // if service is inspection and booking is confirmed then send the notification to schedule user 
                            if (model.Service.Id == (int)Service.InspectionId && roleSchedule != null)
                            {
                                var scheduleUserList = await roleSchedule();
                                if (scheduleUserList != null && scheduleUserList.Any())
                                {
                                    foreach (var order in model.OrderList)
                                    {
                                        if (order.StatusId == (int)BookingStatus.Confirmed)
                                        {
                                            await AddTask(taskType, order.Id, scheduleUserList.Select(x => x.Id), new Notification
                                            {
                                                Title = "LINK Tasks Manager",
                                                Message = "Inspection booking to be Scheduled INS-" + order.Id,
                                                Url = request.Url,
                                                TypeId = "Task"
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (userCCList != null && userCCList.Any())
                    {
                        // Add Notifications
                        await AddNotification(notificationType, model.Id, userCCList.Select(x => x.Id), new Notification
                        {
                            Title = "LINK Notification Manager",
                            Message = emailRequest.Subject,
                            Url = request.Url,
                            TypeId = "Notification"
                        });
                    }

                    request.OnSendEmail(emailRequest);

                }

            }
        }


        public async Task<SetStatusQuotationResponse> SetStatus(SetStatusBusinessRequest request)
        {
            if (_dictStatuses.TryGetValue(request.IdStatus, out Func<SetStatusBusinessRequest, Task<SetStatusQuotationResponse>> func))
            {
                SetStatusQuotationResponse res = await func(request);

                ////quotation status log
                var data = await _QuotationRepository.GetQuotationDataForStatusLogs(request.Id);
                if (request.IdStatus == QuotationStatus.CustomerValidated)
                {
                    data.ForEach(c => c.StatusChangeDate = request.ConfirmDate.ToDateTime());
                }
                else
                {
                    data.ForEach(c => c.StatusChangeDate = DateTime.Now);
                }
                await _eventBookingLog.SaveQuotationStatusLog(data, request.IdStatus);

                return res;
            }
            return new SetStatusQuotationResponse { Result = SetStatusQuotationResult.StatusNotConfigued };
        }


        private async Task<SetStatusQuotationResponse> ToManagerApprove(SetStatusBusinessRequest request)
        {

            var response = await CheckCommonStatus(request, RoleEnum.QuotationManager, QuotationStatus.ManagerApproved, QuotationStatus.QuotationCreated);

            if (response.Result == SetStatusQuotationResult.Success)
            {
                try
                {
                    int locationId = response.Item.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).FirstOrDefault();
                    int customerId = response.Item.Customer.Id;

                    //Get product category details
                    var productCategoryList = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetProductCategoryDetails(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Department details
                    var departmentData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingDepartmentList(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Brand details
                    var brandData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingBrandList(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    var bookingIds = response.Item.OrderList.Select(x => x.Id).ToList();
                    var poDetails = await _insprepo.GetBookingPOTransactionDetails(bookingIds);


                    //factory country 
                    int? factoryCountryId = null;
                    if (response?.Item?.Factory?.Id > 0)
                    {
                        var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(response.Item.Factory.Id);
                        if (factoryCountryData.Result == SupplierListResult.Success)
                            factoryCountryId = factoryCountryData.countryId;
                    }

                    var toUserAccessFilter = new UserAccess
                    {
                        OfficeId = locationId,
                        ServiceId = response.Item.Service.Id,
                        CustomerId = customerId,
                        RoleId = (int)RoleEnum.QuotationSend,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };
                    var ccUserAccessFilter = new UserAccess
                    {
                        OfficeId = locationId,
                        ServiceId = response.Item.Service.Id,
                        CustomerId = customerId,
                        RoleId = (int)RoleEnum.QuotationManager,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };



                    //email subject frame
                    string subject = $"Quotation Manager Approved - {(response.Item.Service.Id == (int)Service.InspectionId ? "INS" : "AUD")}#{(response.Item.OrderList.Any() ? string.Join(", ", response.Item.OrderList.Select(x => x.Id).ToList()) : "")} / {response.Item.ServiceTypeAbbreviation} / Customer : {response.Item.Customer.Name}, Supplier : {response.Item.Supplier.Name}, {(response.Item.Service.Id == (int)Service.InspectionId ? $"Product Ref : {response.Item.ProductRef}, PO : {response.Item.PoNO}, " : "")}Factory country : {response.Item.FactoryCountry}";

                    await SendEMailAndBrodcast(response.Item,
                        () => _userManager.GetUserListByRoleOfficeServiceCustomer(toUserAccessFilter),
                        () => _userManager.GetUserListByRoleOfficeServiceCustomer(ccUserAccessFilter),
                        request, subject,
                        TaskType.QuotationSent, TaskType.QuotationToApprove, NotificationType.QuotationToApprove);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    response.Result = SetStatusQuotationResult.SuccessButErrorBrodcast;
                }

            }

            return response;
        }


        private async Task<SetStatusQuotationResponse> ToCancelQuotation(SetStatusBusinessRequest request)
        {
            // var quotation = await  GetQuotationDetails(request.Id);
            var statusId = request.IdStatus;

            var response = await CheckCommonStatus(request, RoleEnum.QuotationRequest, QuotationStatus.Canceled, null);
            if (response.Result == SetStatusQuotationResult.Success)
            {
                try
                {
                    var tasktype = TaskType.None;
                    var quotationTaskTypes = new List<int>() { (int)TaskType.QuotationModify, (int)TaskType.QuotationPending, (int)TaskType.QuotationSent, (int)TaskType.QuotationToApprove, (int)TaskType.QuotationCustomerConfirmed, (int)TaskType.QuotationCustomerReject };
                    //get old task 
                    var taskList = await _QuotationRepository.GetQueryable<MidTask>(x => quotationTaskTypes.Contains(x.TaskTypeId) && x.LinkId == request.Id && !x.IsDone).ToListAsync();

                    if (taskList != null && taskList.Any())
                        tasktype = (TaskType)taskList.OrderBy(x => x.ReportTo).First().TaskTypeId;

                    //Get product category details
                    var productCategoryList = response.Item.Service.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetProductCategoryDetails(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Department details
                    var departmentData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingDepartmentList(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Brand details
                    var brandData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingBrandList(response?.Item?.OrderList?.Select(x => x.Id)) : null;

                    //factory country 
                    int? factoryCountryId = null;
                    if (response?.Item?.Factory?.Id > 0)
                    {
                        var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(response.Item.Factory.Id);
                        if (factoryCountryData.Result == SupplierListResult.Success)
                            factoryCountryId = factoryCountryData.countryId;
                    }

                    var userAccessFilter = new UserAccess
                    {
                        OfficeId = response.Item.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).FirstOrDefault(),
                        ServiceId = response.Item.Service.Id,
                        CustomerId = response.Item.Customer.Id,
                        RoleId = (int)RoleEnum.QuotationSend,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };

                    if (taskList.Any())
                    {
                        taskList.ForEach(x =>
                        {
                            x.IsDone = true;
                            x.UpdatedBy = _ApplicationContext.UserId;
                            x.UpdatedOn = DateTime.Now;
                        });

                        _QuotationRepository.EditEntities(taskList);
                    }


                    //get distinct service date(if from and to dates are equal show one date) if multiple order show as comma seprated
                    string date = BookingServiceDateCommaSeperateForQuotation(response?.Item?.OrderList);

                    //show supplier name in 25 char with ...
                    string supplierName = SupplierNameFormatForEmail(response?.Item?.Supplier?.Name);
                    //email subject frame
                    string subject = $"Quotation Canceled - {(response.Item.Service.Id == (int)Service.InspectionId ? "INS" : "AUD")}#{(response.Item.OrderList.Any() ? string.Join(", ", response.Item.OrderList.Select(x => x.Id).ToList()) : "")} / {response.Item.ServiceTypeAbbreviation} / {supplierName}, Insp. Date : {date}, {(response.Item.Service.Id == (int)Service.InspectionId ? $"Product Ref : {response.Item.ProductRef}, PO : {response.Item.PoNO}, " : "")}Factory country : {response.Item.FactoryCountry}";

                    await SendEMailAndBrodcast(response.Item,
                        async () =>
                        {
                            var lst = new List<User>();
                            if ((new QuotationStatus[] { QuotationStatus.CustomerValidated, QuotationStatus.CustomerRejected, QuotationStatus.SentToClient }).Contains(statusId))
                            {
                                lst.AddRange(response.Item.CustomerContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                                lst.AddRange(response.Item.SupplierContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                                lst.AddRange(response.Item.FactoryContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                                lst.AddRange(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
                                return lst;
                            }
                            else if (statusId == QuotationStatus.QuotationCreated)
                            {
                                userAccessFilter.RoleId = (int)RoleEnum.QuotationManager;
                                return await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                            }
                            else
                            {
                                return await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                            }
                        },
                        async () =>
                        {
                            var lst = new List<User>();
                            if ((new QuotationStatus[] { QuotationStatus.CustomerValidated, QuotationStatus.CustomerRejected, QuotationStatus.SentToClient }).Contains(statusId))
                            {
                                userAccessFilter.RoleId = (int)RoleEnum.QuotationSend;
                                lst.AddRange(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
                                userAccessFilter.RoleId = (int)RoleEnum.QuotationRequest;
                                lst.AddRange(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
                                return lst;
                            }
                            else
                            {
                                userAccessFilter.RoleId = (int)RoleEnum.QuotationRequest;
                                return await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                            }

                        },
                         request, subject,
                         TaskType.None, tasktype,
                         NotificationType.QuotationCanceled);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    response.Result = SetStatusQuotationResult.SuccessButErrorBrodcast;
                }
            }
            return response;
        }

        private async Task<SetStatusQuotationResponse> ToCSRejected(SetStatusBusinessRequest request)
        {
            var response = await CheckCommonStatus(request, RoleEnum.QuotationSend, QuotationStatus.AERejected, QuotationStatus.QuotationVerified, QuotationStatus.CustomerRejected, QuotationStatus.ManagerApproved);

            if (response.Result == SetStatusQuotationResult.Success)
            {
                try
                {
                    int customerId = response.Item.Customer.Id;
                    int locationId = response.Item.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).FirstOrDefault();

                    //Get product category details
                    var productCategoryList = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetProductCategoryDetails(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Department details
                    var departmentData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingDepartmentList(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Brand details
                    var brandData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingBrandList(response?.Item?.OrderList?.Select(x => x.Id)) : null;

                    //factory country 
                    int? factoryCountryId = null;
                    if (response?.Item?.Factory?.Id > 0)
                    {
                        var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(response.Item.Factory.Id);
                        if (factoryCountryData.Result == SupplierListResult.Success)
                            factoryCountryId = factoryCountryData.countryId;
                    }

                    var toUserAccessFilter = new UserAccess
                    {
                        OfficeId = locationId,
                        ServiceId = response.Item.Service.Id,
                        CustomerId = customerId,
                        RoleId = (int)RoleEnum.QuotationRequest,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };
                    var ccUserAccessFilter = new UserAccess
                    {
                        OfficeId = locationId,
                        ServiceId = response.Item.Service.Id,
                        CustomerId = customerId,
                        RoleId = (int)RoleEnum.QuotationSend,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };
                    //email subject frame
                    string subject = $"Quotation AE Rejected - {(response.Item.Service.Id == (int)Service.InspectionId ? "INS" : "AUD")}#{(response.Item.OrderList.Any() ? string.Join(", ", response.Item.OrderList.Select(x => x.Id).ToList()) : "")} / {response.Item.ServiceTypeAbbreviation} / Customer : {response.Item.Customer.Name}, Supplier : {response.Item.Supplier.Name}, {(response.Item.Service.Id == (int)Service.InspectionId ? $"Product Ref : {response.Item.ProductRef}, PO : {response.Item.PoNO}, " : "")}Factory country : {response.Item.FactoryCountry}";

                    await SendEMailAndBrodcast(response.Item,
                        () => _userManager.GetUserListByRoleOfficeServiceCustomer(toUserAccessFilter),
                        () => _userManager.GetUserListByRoleOfficeServiceCustomer(ccUserAccessFilter),
                        request, subject,
                        TaskType.QuotationModify, TaskType.QuotationSent, NotificationType.QuotationRejected);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    response.Result = SetStatusQuotationResult.SuccessButErrorBrodcast;
                }
            }

            return response;

        }

        private async Task<SetStatusQuotationResponse> ToManagerRejected(SetStatusBusinessRequest request)
        {
            var response = await CheckCommonStatus(request, RoleEnum.QuotationManager, QuotationStatus.ManagerRejected, QuotationStatus.QuotationCreated);

            if (response.Result == SetStatusQuotationResult.Success)
            {
                try
                {
                    int customerId = response.Item.Customer.Id;
                    int locationId = response.Item.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).FirstOrDefault();

                    //Get product category details
                    var productCategoryList = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetProductCategoryDetails(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Department details
                    var departmentData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingDepartmentList(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Brand details
                    var brandData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingBrandList(response?.Item?.OrderList?.Select(x => x.Id)) : null;

                    //factory country 
                    int? factoryCountryId = null;
                    if (response?.Item?.Factory?.Id > 0)
                    {
                        var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(response.Item.Factory.Id);
                        if (factoryCountryData.Result == SupplierListResult.Success)
                            factoryCountryId = factoryCountryData.countryId;
                    }

                    var toUserAccessFilter = new UserAccess
                    {
                        OfficeId = locationId,
                        ServiceId = response.Item.Service.Id,
                        CustomerId = customerId,
                        RoleId = (int)RoleEnum.QuotationRequest,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };
                    var ccUserAccessFilter = new UserAccess
                    {
                        OfficeId = locationId,
                        ServiceId = response.Item.Service.Id,
                        CustomerId = customerId,
                        RoleId = (int)RoleEnum.QuotationManager,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };
                    //email subject frame
                    string subject = $"Quotation Manager Rejected - {(response.Item.Service.Id == (int)Service.InspectionId ? "INS" : "AUD")}#{(response.Item.OrderList.Any() ? string.Join(", ", response.Item.OrderList.Select(x => x.Id).ToList()) : "")} / {response.Item.ServiceTypeAbbreviation} / Customer : {response.Item.Customer.Name}, Supplier : {response.Item.Supplier.Name}, {(response.Item.Service.Id == (int)Service.InspectionId ? $"Product Ref : {response.Item.ProductRef}, PO : {response.Item.PoNO}, " : "")}Factory country : {response.Item.FactoryCountry}";

                    await SendEMailAndBrodcast(response.Item,
                        () => _userManager.GetUserListByRoleOfficeServiceCustomer(toUserAccessFilter),
                        () => _userManager.GetUserListByRoleOfficeServiceCustomer(ccUserAccessFilter),
                        request, subject,
                        TaskType.QuotationModify, TaskType.QuotationToApprove, NotificationType.QuotationRejected);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    response.Result = SetStatusQuotationResult.SuccessButErrorBrodcast;
                }
            }

            return response;
        }

        //private async Task<SetStatusQuotationResponse> ToRejectFromCustomerRejectedQuotation(SetStatusBusinessRequest request)
        //{
        //    return await CheckCommonStatus(request, RoleEnum.QuotationSend, QuotationStatus.CSRejectedAfterCustomerRejected, QuotationStatus.CustomerRejected);
        //}

        private async Task RemoveTask(TaskType taskType, int idQuotation)
        {
            var taskList = await _QuotationRepository
                .GetQueryable<MidTask>(x => x.TaskTypeId == (int)taskType && x.LinkId == idQuotation && !x.IsDone)
                .ToListAsync();

            if (taskList != null && taskList.Count > 0)
            {
                foreach (var task in taskList)
                {
                    task.IsDone = true;
                    task.UpdatedOn = DateTime.Now;
                    task.UpdatedBy = _ApplicationContext?.UserId;
                    _QuotationRepository.EditEntity(task);
                }
                await _QuotationRepository.Save();
            }
        }

        private async Task AddNotification(NotificationType notificationType, int idQuotation, IEnumerable<int> idUserList, Notification notificationEntity)
        {
            if (idUserList != null && idUserList.Any())
            {
                foreach (int idUser in idUserList.Where(x => x > 0))
                {
                    // Add new notification for user request
                    var notification = new MidNotification
                    {
                        Id = Guid.NewGuid(),
                        IsRead = false,
                        LinkId = idQuotation,
                        UserId = idUser,
                        NotifTypeId = (int)notificationType,
                        CreatedOn = DateTime.Now,
                        EntityId = _filterService.GetCompanyId()
                    };

                    _QuotationRepository.AddEntity(notification);
                }

                //Save
                await _QuotationRepository.Save();

                //Brodcast
                _brodcastService.Broadcast(idUserList, notificationEntity);
            }
        }

        private async Task AddTask(TaskType taskType, int idQuotation, IEnumerable<int> idUserList, Notification notificationEntity)
        {
            if (idUserList != null && idUserList.Any())
            {
                foreach (int idUser in idUserList.Where(x => x > 0))
                {
                    // Add new notification for user request
                    var task = new MidTask
                    {
                        Id = Guid.NewGuid(),
                        LinkId = idQuotation,
                        UserId = _ApplicationContext.UserId,
                        IsDone = false,
                        TaskTypeId = (int)taskType,
                        ReportTo = idUser,
                        CreatedBy = _ApplicationContext?.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = _filterService.GetCompanyId()
                    };

                    _QuotationRepository.AddEntity(task);
                }

                //Save
                await _QuotationRepository.Save();

                //Brodcast
                _brodcastService.Broadcast(idUserList, notificationEntity);
            }
        }

        private async Task<SetStatusQuotationResponse> ToSend(SetStatusBusinessRequest request)
        {

            var response = await CheckCommonStatus(request, RoleEnum.QuotationSend, QuotationStatus.SentToClient, QuotationStatus.ManagerApproved, QuotationStatus.QuotationVerified, QuotationStatus.SentToClient, QuotationStatus.CustomerRejected);
            if (response.Result == SetStatusQuotationResult.Success)
            {
                try
                {
                    //get distinct service date(if from and to dates are equal show one date) if multiple order show as comma seprated
                    string date = BookingServiceDateCommaSeperateForQuotation(response?.Item?.OrderList);

                    //show supplier name in 25 char with ...
                    string supplierName = SupplierNameFormatForEmail(response?.Item?.Supplier?.Name);

                    //email subject frame
                    string subject = $" {response.Item.EntityName} Quotation - {(response.Item.Service.Id == (int)Service.InspectionId ? "INS" : "AUD")}#{(response.Item.OrderList.Any() ? string.Join(", ", response.Item.OrderList.Select(x => x.Id).ToList()) : "")} / {response.Item.ServiceTypeAbbreviation} / {supplierName}, Insp. Date : {date}, {(response.Item.Service.Id == (int)Service.InspectionId ? $"Product Ref : {response.Item.ProductRef}, PO : {response.Item.PoNO}, " : "")}Factory country : {response.Item.FactoryCountry}";


                    // check cloud link is exist for quotation pdf if not create and get
                    QuQuotationPdfVersion pdfFile = null;
                    FileResponse document = null;
                    List<FileResponse> objFileList = new List<FileResponse>();

                    if (response.Item != null)
                    {
                        pdfFile = await _QuotationRepository.GetquotationPdfFile(response.Item.Id);

                        if (pdfFile == null)
                        {
                            document = _quotationPDF.CreateDocument(response.Item);
                            pdfFile = new QuQuotationPdfVersion();
                            pdfFile.FileName = document.Name;
                            pdfFile.FileUrl = SavePdfReferenceToCloudAndUpdatewithQuotation(document, response.Item.Id);
                        }

                        if (pdfFile != null && !string.IsNullOrEmpty(pdfFile.FileUrl))
                        {
                            objFileList.Add(new FileResponse()
                            {
                                FileLink = pdfFile.FileUrl,
                                FileStorageType = (int)FileStorageType.Link,
                                Name = pdfFile.FileName,
                                MimeType = "application/pdf"
                            });
                            // update status sento client
                            var pdfFileRef = await _QuotationRepository.GetquotationPdfFile(response.Item.Id);
                            if (pdfFileRef != null)
                            {
                                pdfFileRef.SendToClient = true;
                                _QuotationRepository.Save(pdfFileRef);
                            }
                        }
                    }

                    //remove duplicated new task
                    await RemoveTask(TaskType.QuotationCustomerConfirmed, request.Id);
                    await RemoveTask(TaskType.QuotationCustomerReject, request.Id);

                    int externaluserid = 0;
                    UserTypeEnum usertype = UserTypeEnum.InternalUser;
                    switch ((QuotationPaidBy)response.Item.BillingPaidBy.Id)
                    {
                        case QuotationPaidBy.customer:
                            {
                                externaluserid = response.Item.Customer.Id;
                                usertype = UserTypeEnum.Customer;
                                break;
                            };
                        case QuotationPaidBy.supplier:
                            {
                                externaluserid = response.Item.Supplier.Id;
                                usertype = UserTypeEnum.Supplier;
                                break;
                            };
                        case QuotationPaidBy.factory:
                            {
                                externaluserid = response.Item.Factory.Id;
                                usertype = UserTypeEnum.Factory;
                                break;
                            };
                        default:
                            {
                                break;
                            }

                    }

                    //Get product category details
                    var productCategoryList = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetProductCategoryDetails(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Department details
                    var departmentData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingDepartmentList(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Brand details
                    var brandData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingBrandList(response?.Item?.OrderList?.Select(x => x.Id)) : null;

                    //factory country 
                    int? factoryCountryId = null;
                    if (response?.Item?.Factory?.Id > 0)
                    {
                        var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(response.Item.Factory.Id);
                        if (factoryCountryData.Result == SupplierListResult.Success)
                            factoryCountryId = factoryCountryData.countryId;
                    }


                    var userAccessFilter = new UserAccess
                    {
                        OfficeId = response.Item.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).FirstOrDefault(),
                        ServiceId = response.Item.Service.Id,
                        CustomerId = response.Item.Customer.Id,
                        RoleId = (int)RoleEnum.QuotationConfirmation,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };

                    var externaluserids = await _userManager.GetExternalUserListByRole((int)RoleEnum.QuotationConfirmation, externaluserid, usertype);
                    await SendEMailAndBrodcast(response.Item,
                        async () =>
                        {
                            var lst = new List<User>();
                            lst.AddRange(response.Item.CustomerContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                            lst.AddRange(response.Item.SupplierContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                            lst.AddRange(response.Item.FactoryContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                            lst.AddRange(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
                            if (externaluserids != null && externaluserids.Any())// when send to external user , check the confirm role and send the notification
                                lst.AddRange(externaluserids);
                            return lst;
                        }, async () =>
                        {
                            userAccessFilter.RoleId = (int)RoleEnum.QuotationSend;
                            return await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                        },
                         request, subject,
                         TaskType.QuotationCustomerConfirmed, TaskType.QuotationSent,
                         NotificationType.QuotationSent,
                         objFileList);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    response.Result = SetStatusQuotationResult.SuccessButErrorBrodcast;
                }

            }

            return response;
        }

        private async Task<SetStatusQuotationResponse> ToCustomerValidated(SetStatusBusinessRequest request)
        {
            var response = await CheckCommonStatus(request, RoleEnum.QuotationConfirmation, QuotationStatus.CustomerValidated, QuotationStatus.SentToClient);

            if (response.Result == SetStatusQuotationResult.Success)
            {
                try
                {

                    // check cloud link is exist for quotation pdf if not create and get
                    QuQuotationPdfVersion pdfFile = null;
                    FileResponse document = null;
                    List<FileResponse> objFileList = new List<FileResponse>();

                    if (response.Item != null)
                    {
                        pdfFile = await _QuotationRepository.GetquotationPdfFile(response.Item.Id);

                        if (pdfFile == null)
                        {
                            document = _quotationPDF.CreateDocument(response.Item);
                            pdfFile = new QuQuotationPdfVersion();
                            pdfFile.FileName = document.Name;
                            pdfFile.FileUrl = SavePdfReferenceToCloudAndUpdatewithQuotation(document, response.Item.Id);
                        }

                        if (pdfFile != null && !string.IsNullOrEmpty(pdfFile.FileUrl))
                        {
                            objFileList.Add(new FileResponse()
                            {
                                FileLink = pdfFile.FileUrl,
                                FileStorageType = (int)FileStorageType.Link,
                                Name = pdfFile.FileName,
                                MimeType = "application/pdf"
                            });
                        }
                    }


                    //get distinct service date(if from and to dates are equal show one date) if multiple order show as comma seprated
                    string date = BookingServiceDateCommaSeperateForQuotation(response?.Item?.OrderList);

                    //show supplier name in 25 char with ...
                    string supplierName = SupplierNameFormatForEmail(response?.Item?.Supplier?.Name);

                    var enumEntityName = (Company)_filterService.GetCompanyId();
                    string entityName = enumEntityName.ToString().ToUpper();

                    //email subject frame
                    string subject = $" Quotation Confirmed for {entityName} - {(response.Item.Service.Id == (int)Service.InspectionId ? "INS" : "AUD")}#{(response.Item.OrderList.Any() ? string.Join(", ", response.Item.OrderList.Select(x => x.Id).ToList()) : "")} / {response.Item.ServiceTypeAbbreviation} / {supplierName}, Insp. Date : {date}, {(response.Item.Service.Id == (int)Service.InspectionId ? $"Product Ref : {response.Item.ProductRef}, PO : {response.Item.PoNO}, " : "")}Factory country : {response.Item.FactoryCountry}";

                    //Get product category details
                    var productCategoryList = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetProductCategoryDetails(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Department details
                    var departmentData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingDepartmentList(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Brand details
                    var brandData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingBrandList(response?.Item?.OrderList?.Select(x => x.Id)) : null;

                    //factory country 
                    int? factoryCountryId = null;
                    if (response?.Item?.Factory?.Id > 0)
                    {
                        var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(response.Item.Factory.Id);
                        if (factoryCountryData.Result == SupplierListResult.Success)
                            factoryCountryId = factoryCountryData.countryId;
                    }

                    var toUserAccessFilter = new UserAccess
                    {
                        OfficeId = response.Item.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).FirstOrDefault(),
                        ServiceId = response.Item.Service.Id,
                        CustomerId = response.Item.Customer.Id,
                        RoleId = (int)RoleEnum.InspectionVerified,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };

                    // Email and broadcast 
                    await SendEMailAndBrodcast(response.Item,
                      async () =>
                      {
                          var lstUser = new List<User>();
                          //lstUser.AddRange(await _userManager.GetUserListByRoleOfficeServiceCustomer(toUserAccessFilter));
                          //toUserAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                          lstUser.AddRange(await _userManager.GetUserListByRoleOfficeServiceCustomer(toUserAccessFilter));
                          return lstUser;
                      },
                      async () =>
                      {
                          var lst = new List<User>();
                          lst.AddRange(response.Item.CustomerContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                          lst.AddRange(response.Item.SupplierContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                          lst.AddRange(response.Item.FactoryContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                          if (response != null && response.Item != null && response.Item.PaymentTerm.HasValue && response.Item.PaymentTerm.Value == (int)INVInvoiceType.PreInvoice)
                          {
                              lst.AddRange(await _userManager.GetUserListByRole((int)RoleEnum.Accounting, toUserAccessFilter.OfficeId));
                          }
                          return lst;
                      },
                       request, subject,
                       TaskType.ScheduleInspection, TaskType.QuotationCustomerConfirmed,
                       NotificationType.QuotationCustomerConfirmed,
                       objFileList,
                       async () =>
                       {
                           toUserAccessFilter.RoleId = (int)RoleEnum.InspectionScheduled;
                           return await _userManager.GetUserListByRoleOfficeServiceCustomer(toUserAccessFilter);
                       });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    response.Result = SetStatusQuotationResult.SuccessButErrorBrodcast;
                }
            }


            return response;
        }

        private async Task<SetStatusQuotationResponse> ToCustomerReject(SetStatusBusinessRequest request)
        {

            var response = await CheckCommonStatus(request, RoleEnum.QuotationConfirmation, QuotationStatus.CustomerRejected, QuotationStatus.SentToClient);

            if (response.Result == SetStatusQuotationResult.Success)
            {
                try
                {
                    //Get product category details
                    var productCategoryList = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetProductCategoryDetails(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Department details
                    var departmentData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingDepartmentList(response?.Item?.OrderList?.Select(x => x.Id)) : null;
                    //Get Brand details
                    var brandData = response?.Item?.Service?.Id == (int)Service.InspectionId ?
                                                    await _insprepo.GetBookingBrandList(response?.Item?.OrderList?.Select(x => x.Id)) : null;

                    //factory country 
                    int? factoryCountryId = null;
                    if (response?.Item?.Factory?.Id > 0)
                    {
                        var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(response.Item.Factory.Id);
                        if (factoryCountryData.Result == SupplierListResult.Success)
                            factoryCountryId = factoryCountryData.countryId;
                    }

                    var userAccessFilter = new UserAccess
                    {
                        OfficeId = response.Item.OrderList.Where(x => x.LocationId != null).Select(x => x.LocationId.Value).FirstOrDefault(),
                        ServiceId = response.Item.Service.Id,
                        CustomerId = response.Item.Customer.Id,
                        RoleId = (int)RoleEnum.QuotationSend,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentData != null && departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandData != null && brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        FactoryCountryId = factoryCountryId
                    };

                    //get distinct service date(if from and to dates are equal show one date) if multiple order show as comma seprated
                    string date = BookingServiceDateCommaSeperateForQuotation(response?.Item?.OrderList);

                    //show supplier name in 25 char with ...
                    string supplierName = SupplierNameFormatForEmail(response?.Item?.Supplier?.Name);

                    //email subject frame
                    string subject = $"Quotation Customer Rejected - {(response.Item.Service.Id == (int)Service.InspectionId ? "INS" : "AUD")}#{(response.Item.OrderList.Any() ? string.Join(", ", response.Item.OrderList.Select(x => x.Id).ToList()) : "")} / {response.Item.ServiceTypeAbbreviation} / {supplierName}, Insp. Date : {date}, {(response.Item.Service.Id == (int)Service.InspectionId ? $"Product Ref : {response.Item.ProductRef}, PO : {response.Item.PoNO}," : "")}Factory country : {response.Item.FactoryCountry}";

                    // Email and broadcast 
                    await SendEMailAndBrodcast(response.Item,
                      () => _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter),
                      async () =>
                      {
                          var lst = new List<User>();
                          lst.AddRange(response.Item.CustomerContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                          lst.AddRange(response.Item.SupplierContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                          lst.AddRange(response.Item.FactoryContactList.Where(x => x.Email).Select(x => new User { Id = 0, EmailAddress = x.ContactEmail, FullName = x.ContactName }));
                          return lst;
                      },
                       request, subject,
                       TaskType.QuotationCustomerReject, TaskType.QuotationCustomerConfirmed,
                       NotificationType.QuotationCustomerReject
                       );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    response.Result = SetStatusQuotationResult.SuccessButErrorBrodcast;
                }
            }


            return response;
        }

        private async Task<SetStatusQuotationResponse> CheckCommonStatus(SetStatusBusinessRequest request, RoleEnum role, QuotationStatus newStatus, params QuotationStatus[] oldStatus)
        {

            //Check role
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Contains((int)role))
                return new SetStatusQuotationResponse { Result = SetStatusQuotationResult.NoAccess };

            // Get Quotation 
            var quotation = await GetQuotationDetails(request.Id);

            if (quotation == null)
                return new SetStatusQuotationResponse { Result = SetStatusQuotationResult.QuotationNotFound };


            if (oldStatus != null && oldStatus.Any())
            {
                //Check old status must be Checked
                if (!oldStatus.Contains(quotation.StatusId))
                    return new SetStatusQuotationResponse { Result = SetStatusQuotationResult.NoAccess };
            }

            // no validation for cancelled quotations.
            if (request.IdStatus != QuotationStatus.Canceled)
            {
                var verifydatechange = await VerifyServiceDateChange(quotation);
                //verify the service date change in booking module
                if (verifydatechange.Result != BookingDateChangeInfoResult.Verified)
                    return new SetStatusQuotationResponse { Result = SetStatusQuotationResult.ServiceDateChanged, ServiceDateChangeInfo = verifydatechange };

                //check the inspection booking status before sent to client
                //if (request.IdStatus == QuotationStatus.SentToClient && quotation.Service.Id == (int)Service.InspectionId)
                //{
                //    //quotation can't send to client , if the status is verified or rescheduled.
                //    if (quotation.OrderList.Any(x => new int[] { (int)BookingStatus.Verified, (int)BookingStatus.Rescheduled, (int)BookingStatus.Cancel }.Contains(x.StatusId)))
                //    {
                //        return new SetStatusQuotationResponse { Result = SetStatusQuotationResult.BookingStatusNotConfirmed };
                //    }
                //}
            }
            var objSetStatus = new SetStatusRequest
            {
                Id = request.Id,
                StatusId = (int)newStatus,
                ApiRemark = request.ApiRemark,
                CusComment = request.CusComment,
                ApiInternalRemark = request.ApiInternalRemark
            };
            if (newStatus == QuotationStatus.CustomerValidated)
            {
                if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                {
                    TimeSpan time = DateTime.Now.TimeOfDay;
                    objSetStatus.ValidatedOn = request.ConfirmDate.ToDateTime() + time;
                }
                else
                    objSetStatus.ValidatedOn = DateTime.Now;
                objSetStatus.ValidatedById = _ApplicationContext.UserId;
            }
            //Update status 
            bool result = await _QuotationRepository.SetStatus(objSetStatus);

            //after assign new quotation , need to set status id
            quotation.StatusId = newStatus;

            if (!result)
                return new SetStatusQuotationResponse { Result = SetStatusQuotationResult.QuotationNotFound, Item = quotation };




            var response = new SetStatusQuotationResponse
            {
                Result = SetStatusQuotationResult.Success,
                Item = quotation,
                //ManagerList = await _insprepo.GetUserListByRoleCustomer((int)RoleEnum.QuotationManager, quotation.Office.Id), //unused code
                TaskId = Guid.NewGuid()



            };


            return response;
        }

        public async Task<FileResponse> GetQuotationVersion(Guid id)
        {
            var file = await _QuotationRepository.GetSingleAsync<QuPdfversion>(x => x.GuidId == id);

            if (file == null)
                return new FileResponse { Result = FileResult.NotFound };

            return new FileResponse
            {
                Name = file.FileName,
                Content = file.File,
                MimeType = "application/pdf",
                Result = FileResult.Success
            };

        }
        public async Task<QuotationMandayResponse> QuotationManday(QuotationMandayRequest request)
        {
            QuotationMandayResponse quotationMandayResponse = new QuotationMandayResponse();
            List<BookingDate> bookingDateList = new List<BookingDate>();
            List<QuotationManday> quotationMandays = new List<QuotationManday>();

            foreach (var id in request.BookingId)
            {
                BookingDate bookingDate = null;
                if (request.service == Service.InspectionId)
                {
                    bookingDate = await _insprepo.getInspBookingDateDetails(id);
                }
                else if (request.service == Service.AuditId)
                {
                    bookingDate = await _audprepo.getAuditBookingDateDetails(id);
                }
                if (bookingDate != null)
                {
                    bookingDateList.Add(bookingDate);
                    var listDate = Enumerable.Range(0, 1 + bookingDate.ServiceDateTo.Subtract(bookingDate.ServiceDateFrom).Days)
                                 .Select(offset => bookingDate.ServiceDateFrom.AddDays(offset)).ToArray();
                    var manday = listDate.Select(x => QuotationMap.getQuotationManDay(x.Date, id)).ToList();
                    quotationMandays.AddRange(manday);
                }
            }
            if (!quotationMandays.Any())
                return new QuotationMandayResponse() { MandayResult = QuotationMandayResult.NotFound, QuotationMandaysList = null };

            quotationMandayResponse.MandayResult = QuotationMandayResult.Success;
            quotationMandayResponse.QuotationMandaysList = quotationMandays;

            return quotationMandayResponse;
            //await GetDateRangeManday(serviceDateRangeList);
        }
        /* If reschedule happens need to update existing service date as inactive */
        public async Task UpdateQuotationServiceDate(BookingDateInfo request, InspTransaction entity)
        {
            try
            {
                //if request and entity as same date we should not update quotation date
                if (!(request.ServiceFromDate.ToDateTime() == entity.ServiceDateFrom &&
                       request.ServiceToDate.ToDateTime() == entity.ServiceDateTo))
                {
                    bool quotationExists = await _QuotationRepository.QuotationInspExists(request.BookingId);
                    if (quotationExists)
                    {
                        var quotationInspDateMandayList = await _QuotationRepository.GetQuotationInspManDay(request.BookingId);

                        if (quotationInspDateMandayList != null && quotationInspDateMandayList.Count() > 0)
                        {
                            var quotationRecord = quotationInspDateMandayList.FirstOrDefault();
                            //if data exist make as inactive 
                            foreach (QuQuotationInspManday manDayInsp in quotationInspDateMandayList)
                            {
                                manDayInsp.Active = false;
                                manDayInsp.DeletedDate = DateTime.Now;
                            }
                            _QuotationRepository.EditEntities(quotationInspDateMandayList);

                            AddQuotationServiceDate(quotationInspDateMandayList, request, quotationRecord.QuotationId);
                            UpdateQuotationAPIRemarkData(request, quotationRecord);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Existing data sum the manday and join the remarks update in new service date data first row. And insert a new service date in QuQuotationInspManday 
        private void AddQuotationServiceDate(IEnumerable<QuQuotationInspManday> quotationInspDateMandayList, BookingDateInfo request, int quotationId)
        {
            try
            {
                double? totalManday = quotationInspDateMandayList.Select(x => x.NoOfManday).Sum();
                var remarks = string.Join(", ", quotationInspDateMandayList.Where(x => x.Remarks != null).Select(x => x.Remarks).ToList());

                DateTime[] dateList = Enumerable.Range(0, 1 + request.ServiceToDate.ToDateTime().Subtract(request.ServiceFromDate.ToDateTime()).Days)
                     .Select(offset => request.ServiceFromDate.ToDateTime().AddDays(offset)).ToArray();

                foreach (DateTime serviceDate in dateList)
                {
                    var dateManday = new QuQuotationInspManday
                    {
                        BookingId = request.BookingId,
                        QuotationId = quotationId,
                        NoOfManday = totalManday,
                        Remarks = remarks,
                        ServiceDate = serviceDate,
                        CreatedBy = _ApplicationContext.UserId,
                        Active = true
                    };
                    remarks = null;
                    totalManday = 0;
                    _QuotationRepository.AddEntity(dateManday);
                }
            }
            catch (Exception ex)
            {

            }
        }
        //Append -  API remark column text 
        private void UpdateQuotationAPIRemarkData(BookingDateInfo request, QuQuotationInspManday objQuotation)
        {
            try
            {
                string ServiceDateFormat = request.ServiceFromDate.ToDateTime().ToShortDateString() == request.ServiceToDate.ToDateTime().ToShortDateString() ?
                                        request.ServiceFromDate.ToDateTime().ToShortDateString() + "."
                                        : (request.ServiceFromDate.ToDateTime().ToShortDateString() + " - "
                                        + request.ServiceToDate.ToDateTime().ToShortDateString() + ".");

                var apiRemark = objQuotation.Quotation.ApiRemark != null ? objQuotation.Quotation.ApiRemark : "";

                objQuotation.Quotation.ApiRemark = apiRemark + " Booking #" + request.BookingId + " rescheduled to " + ServiceDateFormat;

                _QuotationRepository.EditEntity(objQuotation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetQuotationIdByAuditid(int AuditbookingId)
        {
            return await _QuotationRepository.GetQuotationIdByAuditid(AuditbookingId);
        }

        // product sample qty from booking and check with quoation sample qty
        public async Task<bool> CheckQuotationSampleQtyAndBookingSampleQtyAreEqual(IEnumerable<QuotProduct> quotProducts)
        {
            //get list of insp po trans id 
            var inspPoTranIds = quotProducts.Select(x => x.InspPoId).ToList();

            //get insppotransaction list by insp po id
            var inspPoTranList = await _insprepo.GetInspPoDetails(inspPoTranIds);

            //sum the aql quantity not combined product - insppotransaction
            var aqlQtyBookingTotal = inspPoTranList.Where(x => !(x.CombineProductId > 0) && !(x.CombineAqlQuantity > 0) && (x.AqlQuantity > 0))
                                        .Sum(x => x.AqlQuantity);

            //sum the combine aql quantity for combined product insppotransaction
            var combineQtyBookingTotal = inspPoTranList.Where(x => (x.CombineProductId > 0) && (x.CombineAqlQuantity > 0))
                                          .Sum(x => x.CombineAqlQuantity);

            //sum the sample qty quoation product table
            var quotationSampleQtyTotal = quotProducts.Where(x => x.SampleQty > 0).Sum(x => x.SampleQty);

            //return the quotation qty and booking qty equal or not
            return (quotationSampleQtyTotal == (combineQtyBookingTotal + aqlQtyBookingTotal));
        }

        //get distinct service date(if from and to dates are equal show one date) if multiple order show as comma seprated
        private string BookingServiceDateCommaSeperateForQuotation(IEnumerable<Order> orderList)
        {
            string DateFormat = string.Empty;

            if (orderList.Any())
            {
                // from and to date null check in the order list and select the date(using timespan(internal time between dates) we are showing date as one or two dates)
                DateFormat = string.Join(", ", orderList.Where(x =>
                            DateTime.ParseExact(x.InspectionTo, StandardDateFormat, CultureInfo.InvariantCulture).Date != null &&
                            DateTime.ParseExact(x.InspectionFrom, StandardDateFormat, CultureInfo.InvariantCulture).Date != null)
                        .Select(x => ((TimeSpan)
                            (DateTime.ParseExact(x.InspectionTo, StandardDateFormat, CultureInfo.InvariantCulture) -
                            DateTime.ParseExact(x.InspectionFrom, StandardDateFormat, CultureInfo.InvariantCulture))).Days == 0 ?
                              x.InspectionFrom : $"{x.InspectionFrom} - {x.InspectionTo}").Distinct());
            }
            return DateFormat;
        }

        //show supplier name in 25 char with ... for customer email
        private string SupplierNameFormatForEmail(string supplierName)
        {
            return (!string.IsNullOrWhiteSpace(supplierName) ?
                        supplierName.Substring(0, Math.Min(supplierName.Length, 25)) + (supplierName.Length > 25 ?
                        "..." : "") : "");
        }

        //Return booking ids if inspection or audit quotation exists 
        private async Task<IEnumerable<int>> IsQuotationExists(int serviceId, IEnumerable<int> bookingOrAuditIds, int quotationId)
        {
            return (serviceId == (int)Service.AuditId
                ?
                await _QuotationRepository.IsAuditQuotationExists(bookingOrAuditIds, quotationId)
                :
                await _QuotationRepository.IsBookingQuotationExists(bookingOrAuditIds, quotationId));
        }

        public async Task<ClientQuotationResponse> GetClientQuotation(int quotationId)
        {
            ClientQuotationResponse response = new ClientQuotationResponse();
            try
            {
                //Fetch the quotation details for the quotationID and customerID if not cancelled
                var quotation = await _QuotationRepository.GetClientQuotation(quotationId);

                //Get the booking ID from Quotaiton details
                var bookingId = quotation.Booking.Select(x => x.IdBooking).FirstOrDefault();

                //Fetch the PO and product details for the booking 
                var bookingdetails = await _insprepo.GetProductListByBookingForClientQuotation(bookingId);

                //factory country required for pending quotation 
                //var list = new[] { bookingId };
                //var factoryCountryData = await _insprepo.GetFactorycountryId(list);
                //var inspectionLocation = factoryCountryData.FirstOrDefault();

                //Create a list of SupID and FactoryID to fetch the customer codes in one DB call
                var customerIdList = new[] { quotation.CustomerId }.ToList();

                var supIds = new List<int>();
                supIds.Add(quotation.FactoryId);
                supIds.Add(quotation.SupplierId);
                var supcode = await _supplierRepository.GetSupplierCode(customerIdList, supIds);

                //get the customer contact
                //var cusContact = await _insprepo.GetBookingCustomerContact(bookingId);
                //quotation.CustomerContact = string.Join(", ", cusContact);

                var list = new[] { bookingId };
                var dept = await _deptRepo.GetCustomerDepartmentsbyBooking(list.ToList());
                quotation.DepartmentName = string.Join(", ", dept.Select(x => x.Name));

                //Get the distinct number of products for the booking
                int prodQty = bookingdetails.Select(x => x.ProductId).Distinct().Count();

                quotation.ProductCategory = bookingdetails.Select(x => x.ProductCategory).FirstOrDefault();

                quotation.AQL = string.Join(", ", bookingdetails.Select(x => x.AQL).Distinct()).TrimEnd(", ".ToCharArray());

                quotation.TravelCost = quotation.TravelCostAir.GetValueOrDefault() + quotation.TravelCostLand.GetValueOrDefault();

                //Get the total quotation price
                double totalPrice = quotation.QuotationPrice + quotation.Transportcost.GetValueOrDefault() + quotation.TravelCost.GetValueOrDefault() + quotation.HotelCost.GetValueOrDefault() + quotation.OtherCost.GetValueOrDefault();

                //Calculate the Sampling size for each PO
                var distinctPO = bookingdetails.Select(x => x.PoId).Distinct();
                Dictionary<int, int> sampleQty = new Dictionary<int, int>();

                var combineAQLQuantity = bookingdetails.Where(x => x.CombineProductId != null).Sum(x => x.CombineAqlQty);

                var nonCombineAQLQuantity = bookingdetails.Where(x => x.CombineProductId == null).Sum(x => x.AqlQty);

                var totalSamplingQuantity = combineAQLQuantity.GetValueOrDefault(0) + nonCombineAQLQuantity.GetValueOrDefault(0);

                //Group by the PO
                var items = bookingdetails.GroupBy(p => p.PoId, p => p, (key, _data) =>
                    new QuotationBookingItem
                    {
                        BookingId = _data.Where(x => x.PoId == key).FirstOrDefault().BookingId,
                        PONumber = _data.Where(x => x.PoId == key).FirstOrDefault().PONumber,
                        POQuantity = _data.Where(x => x.PoId == key).Sum(x => x.POQuantity),
                        ServiceFromDate = _data.Where(x => x.PoId == key).FirstOrDefault().ServiceFromDate,
                        ServiceToDate = _data.Where(x => x.PoId == key).FirstOrDefault().ServiceToDate,
                        //(quantity for that PO / total PO Quantity combined ) * total cost
                        CostPerOrder = GetCostPerOrder(key, bookingdetails, totalPrice, true, 0),//(_data.Where(x => x.PONumber == key).Sum(x => x.POQuantity) / bookingdetails.Sum(x => x.POQuantity) ) * (quotation.QuotationUnitPrice + quotation.Transportcost.GetValueOrDefault() + quotation.TravelCost.GetValueOrDefault() + quotation.HotelCost.GetValueOrDefault() + quotation.OtherCost.GetValueOrDefault())
                        Manday = GetCostPerOrder(key, bookingdetails, totalPrice, false, quotation.ManDay)
                    }).ToList();

                response.QuotationDetails = QuotationMap.ClientQuotationMap(quotation, items, supcode, prodQty, totalSamplingQuantity);
                response.Result = ClinetQuotationResult.Success;
                return response;
            }
            catch (Exception e)
            {
                response.Result = ClinetQuotationResult.Failure;
                return response;
            }
        }

        //Calculate the Cost Per PO
        private double GetCostPerOrder(int PoNumber, IEnumerable<ClientQuotationBookingItem> productList, double totalPrice, bool costPerOrder, double manday)
        {
            double costperorder = 0;

            //get the total cost of all the POs
            double totalqty = productList.Sum(x => x.POQuantity);

            //get the sum of the quantity for a particular PO
            var poquantity = productList.Where(x => x.PoId == PoNumber).Sum(x => x.POQuantity);

            if (costPerOrder)
            {
                //Cost Per PO = (sum of quantity per PO/ Total cost of all POs ) * total quotation proce
                costperorder = (poquantity / totalqty) * totalPrice;
                return Math.Round(costperorder, 2);
            }

            else
            {
                //Cost Per PO = (sum of quantity per PO/ Total cost of all POs ) * total quotation proce
                costperorder = (poquantity / totalqty) * manday;
                return Math.Round(costperorder, 2);

            }
        }

        //Save Invoice Details
        public async Task<SaveQuotationResponse> SaveInvoice(InvoiceRequest request)
        {
            SaveQuotationResponse res = new SaveQuotationResponse();
            try
            {
                if (request != null)
                {
                    if (request.Service == (int)Service.InspectionId)
                    {
                        var response = await _QuotationRepository.GetquotationInsp(request.QuotationId);

                        response.InvoiceNo = request.InvoiceNo;
                        response.InvoiceDate = request.InvoiceDate.ToDateTime();
                        response.InvoiceRemarks = request.InvoiceRemarks;
                        response.UpdatedBy = _ApplicationContext.UserId;
                        response.UpdatedOn = DateTime.Now;

                        await _QuotationRepository.Save();

                        res.Result = SaveQuotationResult.Success;
                    }

                    else
                    {
                        var response = await _QuotationRepository.GetquotationAudit(request.QuotationId);

                        response.InvoiceNo = request.InvoiceNo;
                        response.InvoiceDate = request.InvoiceDate.ToDateTime();
                        response.InvoiceRemarks = request.InvoiceRemarks;
                        response.UpdatedBy = _ApplicationContext.UserId;
                        response.UpdatedOn = DateTime.Now;

                        await _QuotationRepository.Save();

                        res.Result = SaveQuotationResult.Success;
                    }
                }
            }

            catch (Exception e)
            {
                res.Result = SaveQuotationResult.CannotSave;
            }

            return res;
        }

        //Fetch Saved Invoice Details with quotation Id
        public async Task<QuotationEditInvoiceItem> GetInvoice(int quotationId, int serviceId)
        {
            QuotationEditInvoiceItem response = new QuotationEditInvoiceItem();
            var list = new[] { quotationId };
            if (serviceId == (int)Service.InspectionId)
            {
                var data = _QuotationRepository.GetquotationInsp(list.ToList()).Result.FirstOrDefault();
                var date = data.QuoInvoiceDate?.GetCustomDate();
                response.InvoiceDate = date;
                response.InvoiceNo = data.QuoInvoiceNo;
                response.InvoiceREmarks = data.QuoInvoiceREmarks;
            }

            else
            {
                var data = _QuotationRepository.GetquotationAudit(list.ToList()).Result.FirstOrDefault();
                var date = data.QuoInvoiceDate?.GetCustomDate();
                response.InvoiceDate = date;
                response.InvoiceNo = data.QuoInvoiceNo;
                response.InvoiceREmarks = data.QuoInvoiceREmarks;
            }
            return response;
        }

        /// <summary>
        /// get Customer Price related data
        /// </summary>
        /// <param name="ruleConfigs"></param>
        /// <returns></returns>
        private async Task<PriceCard> GetCustomerPriceData(List<CustomerPriceCardRepo> ruleConfigs)
        {
            var cuPriceDetails = ruleConfigs.Select(x => new CustomerPriceCardDetails
            {
                Id = x.Id,
                BillMethodId = x.BillingMethodId,
                UnitPrice = x.UnitPrice,
                BillToId = x.BillingToId
            }).ToList();

            if (!cuPriceDetails.Any())
                return new PriceCard() { Result = ResponseResult.NotFound };

            //if rule count is 1 return success, more than one rule we should not return the unit price
            if (cuPriceDetails.Count == 1)
            {
                return new PriceCard()
                {
                    CustomerPriceCardDetails = cuPriceDetails,
                    Result = ResponseResult.Success
                };
            }
            else if (cuPriceDetails.Count > 1)
            {

                return new PriceCard() { CustomerPriceCardDetails = cuPriceDetails, Result = ResponseResult.MoreRuleExists };
            }

            return new PriceCard() { Result = ResponseResult.NotFound };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <param name="billMethod"></param>
        /// <param name="billPaidBy"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public async Task<PriceCard> GetCustomerCardDetailsByBookingDetails(
                             IEnumerable<BookingDetail> bookingFilters, int billMethod, int billPaidBy, int? currencyId)
        {

            ////fetch (country customer supplier province  servicetype, service from and to)  details by booking id
            //var getBookingFilters = await _bookingManager.GetBookingData(bookingIds);


            if (bookingFilters != null)
            {
                var bookingIds = bookingFilters.Select(x => x.BookingId);

                //get product category details by booking ids
                var productCategoryDetails = await _bookingManager.GetProductCategoryDetails(bookingFilters?.Select(x => x.BookingId));

                var bookingData = bookingFilters.FirstOrDefault();

                if (bookingData == null)
                    return new PriceCard() { Result = ResponseResult.NotFound };
                //    foreach (var bookingData in getBookingFilters)

                //get config detils
                var cardDetails = _customerPriceCardRepository.GetCustomerUnitPriceByCustomerIdServiceId(bookingData.CustomerId, (int)Service.InspectionId);

                if (cardDetails == null || !cardDetails.Any())
                    return new PriceCard() { Result = ResponseResult.NotFound };

                var getData = GetCustomerPriceCardRule(bookingData, cardDetails, productCategoryDetails, billMethod, billPaidBy, currencyId);

                var cuPriceCardIdList = getData.Select(x => x.Id).ToList();
                var holidayTypeList = await _customerPriceCardRepository.GetHolidayTypeList(cuPriceCardIdList);

                foreach (var item in getData)
                {
                    item.HolidayTypeList = holidayTypeList.Where(x => x.Id == item.Id).ToList();
                }

                if (getData == null || !getData.Any())
                    return new PriceCard() { Result = ResponseResult.NotFound };

                //if rule count is 1 return success, more than one rule we should not return the unit price
                if (getData.Count() == 1)
                {
                    return new PriceCard()
                    {
                        CustomerPriceCardDetails = getData,
                        Result = ResponseResult.Success
                    };
                }
                else if (getData.Count() > 1)
                {

                    return new PriceCard() { CustomerPriceCardDetails = getData, Result = ResponseResult.MoreRuleExists };
                }

            }
            return new PriceCard() { Result = ResponseResult.NotFound };

            //GetCustomerPriceCardRule - get rule id 
        }


        /*filter(country customer supplier province  servicetype, service from and to product category billing method paid by) 
        * the customer price card rule by booking details
        */
        private IEnumerable<CustomerPriceCardDetails> GetCustomerPriceCardRule(BookingDetail entity, IQueryable<CuPrDetail> cardDetails,
                                                    IEnumerable<BookingProductCategory> productCategory, int billMethod, int billPaidBy, int? currencyId)
        {
            cardDetails = cardDetails.Where(x => !((x.PeriodFrom > entity.ServiceTo) ||
                                                        (x.PeriodTo < entity.ServiceFrom)));

            //booking supplier id contains in price card record
            if (entity.SupplierId > 0)
            {
                cardDetails = cardDetails.Where(x => (!x.CuPrSuppliers.Any()) || (x.CuPrSuppliers.Any(y => y.Active.Value && y.SupplierId == entity.SupplierId)));
            }

            //quotation bill method equal to price card bill method
            if (billMethod > 0)
            {
                cardDetails = cardDetails.Where(x => (x.BillingMethodId == 0) || x.BillingMethodId == billMethod);
            }

            //quotation currency equal to price card currency
            if (currencyId != null && currencyId > 0 && cardDetails.Select(x => x.CurrencyId) != null && cardDetails.Any(x => x.CurrencyId > 0))
            {
                cardDetails = cardDetails.Where(x => (x.CurrencyId == 0) || x.CurrencyId == currencyId);
            }

            //quotation bill paidby equal to price card bill paidby
            if (billPaidBy > 0)
            {
                cardDetails = cardDetails.Where(x => x.BillingToId == 0 || x.BillingToId == billPaidBy);
            }

            //booking ServiceTypeIdList contains in price card record
            if (entity.ServiceTypeIds != null && entity.ServiceTypeIds.Any())
            {
                cardDetails = cardDetails.Where(x => (!x.CuPrServiceTypes.Any(y => y.Active.Value)) || x.CuPrServiceTypes.Any(y => y.Active.Value && entity.ServiceTypeIds.Contains(y.ServiceTypeId)));
            }

            //booking CountryIdList contains in price card record
            if (entity.CountryIds != null && entity.CountryIds.Any())
            {
                cardDetails = cardDetails.Where(x => !x.CuPrCountries.Any(y => y.Active.Value) || x.CuPrCountries.Any(y => y.Active.Value && entity.CountryIds.Contains(y.FactoryCountryId)));

            }

            //booking ProvinceIdList contains in price card record
            if (entity.CountryIds != null && entity.ProvinceIds != null && entity.CountryIds.Any() && entity.ProvinceIds.Any())
            //cardDetails.Where(x => x.CountryIdList.Any()).Count() == CountryCount &&
            {
                cardDetails = cardDetails.Where(x => !x.CuPrProvinces.Any() || x.CuPrProvinces.Any(y => y.Active.Value && entity.ProvinceIds.Contains(y.FactoryProvinceId)));
            }

            //booking product category contains in price card record

            var bookingProductCategoryIds = productCategory.Where(x => x.BookingId == entity.BookingId).Select(x => x.ProductCategoryId).Distinct();

            if (bookingProductCategoryIds != null && bookingProductCategoryIds.Any())
                cardDetails = cardDetails.Where(x => (!x.CuPrProductCategories.Any(y => y.Active.Value)) || (x.CuPrProductCategories.Any(y => y.Active.Value && bookingProductCategoryIds.Contains(y.ProductCategoryId))));


            //booking BrandList contains in price card record
            if (entity.BrandIds != null && entity.BrandIds.Any())
            {
                cardDetails = cardDetails.Where(x => !x.CuPrBrands.Any(y => y.Active.Value) || x.CuPrBrands.Any(y => y.Active.Value && entity.BrandIds.Contains(y.BrandId.GetValueOrDefault())));
            }

            //booking BrandList contains in price card record
            if (entity.BuyerIds != null && entity.BuyerIds.Any())
            {
                cardDetails = cardDetails.Where(x => !x.CuPrBuyers.Any(y => y.Active.Value) || x.CuPrBuyers.Any(y => y.Active.Value && entity.BuyerIds.Contains(y.BuyerId.GetValueOrDefault())));
            }

            //booking DepartmentList contains in price card record
            if (entity.DepartmentIds != null && entity.DepartmentIds.Any())
            {
                cardDetails = cardDetails.Where(x => !x.CuPrDepartments.Any(y => y.Active.Value) || x.CuPrDepartments.Any(y => y.Active.Value && entity.DepartmentIds.Contains(y.DepartmentId.GetValueOrDefault())));
            }

            //booking pricecategorylist contains in price card record
            if (entity.PriceCategoryId != null && entity.PriceCategoryId > 0)
            {
                cardDetails = cardDetails.Where(x => (!x.CuPrPriceCategories.Any(y => y.Active.Value) || x.CuPrPriceCategories.Any(y => y.Active.Value && y.PriceCategoryId.GetValueOrDefault() == entity.PriceCategoryId)));
            }

            var data = cardDetails.AsNoTracking().ToList();

            return data.Select(x => new CustomerPriceCardDetails
            {
                Id = x.Id,
                BillMethodId = x.BillingMethodId,
                UnitPrice = x.UnitPrice
            }).ToList();
        }

        //get unit price and booking id details by booking id, billmethod and bill paid by 
        public async Task<CustomerPriceCardUnitPriceResponse> GetCustomerPriceCardUnitPriceData(UnitPriceCardRequest request)
        {
            try
            {
                if (request == null)
                    return new CustomerPriceCardUnitPriceResponse() { Result = ResponseResult.RequestNotCorrectFormat };

                var response = new CustomerPriceCardUnitPriceResponse
                {
                    UnitPriceCardList = new List<CustomerPriceCardUnitPrice>()
                };

                if (request.BookingIds != null)
                {

                    var bookingRuleInfoList = await GetBookingAndRuleList(request.BookingIds, request.BillPaidById, request.BillMethodId);

                    if (bookingRuleInfoList != null)
                    {
                        bool unitPriceZeroAmount = false;
                        if (bookingRuleInfoList?.InvoiceGenerateRequest?.CustomerId > 0)
                        {
                            var checkPoint = await _customerCheckPointRepository.GetCustomerCheckpoint(bookingRuleInfoList.InvoiceGenerateRequest.CustomerId, (int)Service.InspectionId, (int)CheckPointTypeEnum.HidePricecardAmountforCustomerinQuotation);
                            unitPriceZeroAmount = checkPoint != null && checkPoint.Id > 0;
                        }
                        foreach (var bookingOrder in bookingRuleInfoList.invoiceBookings)
                        {

                            var invoiceList = new List<InvoiceDetail>();

                            var ruleConfigs = await _invoiceManager.GetRuleConfigListbyBookingFilter(bookingOrder, bookingRuleInfoList.customerPriceCards);

                            // Removed this filter. when user changes the filter like bill methods,currency billto then rule need to fetch and updates.

                            if (request.RuleId > 0)
                                ruleConfigs = ruleConfigs.Where(x => x.Id == request.RuleId.GetValueOrDefault()).ToList();

                            var customerPriceCard = await GetCustomerPriceData(ruleConfigs);

                            if (customerPriceCard != null && customerPriceCard.Result == ResponseResult.Success &&
                                   customerPriceCard.CustomerPriceCardDetails != null)
                            {
                                var customerPriceCardDetails = customerPriceCard.CustomerPriceCardDetails.FirstOrDefault();
                                if (customerPriceCardDetails != null)
                                {

                                    var ruleConfig = ruleConfigs.FirstOrDefault(x => x.Id == customerPriceCardDetails.Id);

                                    if (request.BillQuantityType > 0)
                                        ruleConfig.BillQuantityType = request.BillQuantityType;
                                    // set default invoice fees from option - only from quotation even if it is from diffrent rule
                                    ruleConfig.InvoiceInspFeeFrom = (int)InvoiceFeesFrom.Invoice;

                                    if (request.InvoiceType > 0)
                                        bookingRuleInfoList.InvoiceGenerateRequest.InvoiceType = request.InvoiceType;

                                    // set Rule Config
                                    bookingOrder.RuleConfig = ruleConfig;
                                    var quotationBooking = new List<QuotationBooking>();
                                    var invoiceBookingList = new List<InvoiceBookingDetail>() { bookingOrder };

                                    // get invoice list by price calculations
                                    invoiceList = await _invoiceManager.GetInvoiceListbyPriceCalculations(invoiceBookingList, invoiceList, ruleConfig, bookingRuleInfoList.InvoiceGenerateRequest, quotationBooking);

                                    // map unitprice bookingid and return
                                    CustomerPriceCardUnitPrice objUnitPrice = new CustomerPriceCardUnitPrice()
                                    {
                                        UnitPrice = unitPriceZeroAmount && customerPriceCardDetails.BillToId == (int)QuotationPaidBy.customer ? 0 : invoiceList.Where(x => x.InspectionId == bookingOrder.BookingId && x.UnitPrice > 0).Select(x => x.UnitPrice).FirstOrDefault(),
                                        PriceCardIdList = customerPriceCard.CustomerPriceCardDetails.Select(x => x.Id),
                                        BillQuantityType = invoiceList.Where(x => x.InspectionId == bookingOrder.BookingId).Select(x => x.BilledQuantityType).FirstOrDefault(),
                                        TotalBillQuantity = invoiceList.Where(x => x.InspectionId == bookingOrder.BookingId).Select(x => x.BilledQuantity).FirstOrDefault(),
                                        BookingId = bookingOrder.BookingId,
                                        Remarks = invoiceList.Where(x => x.InspectionId == bookingOrder.BookingId && x.UnitPrice > 0).Select(x => x.Remarks).FirstOrDefault(),
                                    };

                                    response.UnitPriceCardList.Add(objUnitPrice);

                                    objUnitPrice.Result = UnitPriceResponseResult.SingleRuleExists;
                                }
                            }
                            else if (customerPriceCard != null && customerPriceCard.CustomerPriceCardDetails != null &&
                                                                    customerPriceCard.Result == ResponseResult.MoreRuleExists)
                            {
                                CustomerPriceCardUnitPrice objUnitPrice = new CustomerPriceCardUnitPrice()
                                {
                                    UnitPrice = null,
                                    BookingId = bookingOrder.BookingId,
                                    PriceCardIdList = customerPriceCard.CustomerPriceCardDetails.Select(x => x.Id),
                                };

                                response.UnitPriceCardList.Add(objUnitPrice);
                                objUnitPrice.Result = UnitPriceResponseResult.MoreRuleExists;
                            }
                        }
                    }






                    //if unit price list has data
                    if (response.UnitPriceCardList.Any())
                    {
                        ExportMapRequest mapRequest = new ExportMapRequest();
                        //take price id list
                        var priceIdList = response.UnitPriceCardList.SelectMany(x => x.PriceCardIdList).ToList();

                        //take customer price card detail info for multipricecard popup
                        var customerPriceCardDetail = await _customerPriceCardRepository.GetCustomerPriceCardData(priceIdList);

                        mapRequest.ProductCategory = await _customerPriceCardRepository.GetPrProductCategories(priceIdList);

                        mapRequest.ServiceType = await _customerPriceCardRepository.GetPrServiceTypes(priceIdList);

                        mapRequest.CountryData = await _customerPriceCardRepository.GetPrCountries(priceIdList);

                        mapRequest.DeptData = await _customerPriceCardRepository.GetPrDepartment(priceIdList);

                        mapRequest.BuyerData = await _customerPriceCardRepository.GetPrBuyer(priceIdList);

                        mapRequest.BrandData = await _customerPriceCardRepository.GetPrBrand(priceIdList);

                        mapRequest.PriceCategory = await _customerPriceCardRepository.GetPrPriceCategory(priceIdList);

                        mapRequest.ProvinceData = await _customerPriceCardRepository.GetPrProvince(priceIdList);

                        foreach (var unitPriceCard in response.UnitPriceCardList)
                        {
                            unitPriceCard.CustomerPriceCardDetails = customerPriceCardDetail.Where(x => unitPriceCard.PriceCardIdList.Contains(x.Id)).
                                                                         Select(x => CustomerPriceCardMap.GetPriceCardQuotationMap(x, mapRequest));
                        }

                        response.Result = ResponseResult.Success;

                        if (!CheckQuotationCommonData(response.UnitPriceCardList))
                            response.Result = ResponseResult.NoQuotationCommonDataMatch;
                    }
                    else
                    {
                        response.Result = ResponseResult.NotFound;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //throw ex;
                return new CustomerPriceCardUnitPriceResponse() { Result = ResponseResult.Error };
            }
        }

        private async Task<BookingRuleInfo> GetBookingAndRuleList(IEnumerable<int> bookingIds, int billPaidby, int billingMethod)
        {
            BookingRuleInfo bookingRuleInfo = new BookingRuleInfo();
            // create invoice instance 
            InvoiceGenerateRequest requestDto = new InvoiceGenerateRequest() { InvoiceTo = billPaidby, IsFromQuotation = true };
            requestDto.BookingNoList = bookingIds;


            // get the booking info 
            var bookingList = await _invoiceManager.GetInspectioDatabyInvoiceRequest(requestDto, new List<int> { });

            // set the booking related info
            if (bookingList.FirstOrDefault() != null)
            {
                requestDto.CustomerId = bookingList.FirstOrDefault().CustomerId;
            }

            if (bookingList.Any())
            {
                var insepectionMinDate = bookingList.Select(x => x.ServiceFrom).Min();
                var inspectionMaxDate = bookingList.Select(x => x.ServiceTo).Max();
                requestDto.RealInspectionFromDate = insepectionMinDate.GetCustomDate();
                requestDto.RealInspectionToDate = inspectionMaxDate.GetCustomDate();

                var ruleConfigList = await _invoiceManager.GetPriceCardRuleList(requestDto);

                // added billing method check
                if (billingMethod > 0)
                {
                    ruleConfigList = ruleConfigList.Where(x => x.BillingMethodId == billingMethod).ToList();
                }

                await _invoiceManager.SetRuleDataList(ruleConfigList);

                bookingRuleInfo.customerPriceCards = ruleConfigList;
            }

            bookingRuleInfo.InvoiceGenerateRequest = requestDto;
            bookingRuleInfo.invoiceBookings = bookingList;
            return bookingRuleInfo;
        }

        /// <summary>
        /// Check the Quotation Common Data among the price card rules selected for each booking
        /// </summary>
        /// <param name="unitPriceCardList"></param>
        /// <returns></returns>
        private bool CheckQuotationCommonData(List<CustomerPriceCardUnitPrice> unitPriceCardList)
        {
            int? billingMethodId = null;
            int? billingPaidById = null;
            int? currencyId = null;
            if (unitPriceCardList != null && unitPriceCardList.Count() > 0)
            {
                var singleRuleUnitPriceCard = unitPriceCardList.FirstOrDefault(x => x.Result == UnitPriceResponseResult.SingleRuleExists);

                if (singleRuleUnitPriceCard != null)
                {
                    billingMethodId = singleRuleUnitPriceCard.CustomerPriceCardDetails.FirstOrDefault()?.BillingMethodId;
                    billingPaidById = singleRuleUnitPriceCard.CustomerPriceCardDetails.FirstOrDefault()?.BillingPaidById;
                    currencyId = singleRuleUnitPriceCard.CustomerPriceCardDetails.FirstOrDefault()?.CurrencyId;
                    var remainingPriceCardList = unitPriceCardList.Where(x => x.BookingId != singleRuleUnitPriceCard.BookingId).ToList();

                    foreach (var priceCardData in remainingPriceCardList)
                    {
                        if (!(priceCardData.CustomerPriceCardDetails.Any(x => x.BillingMethodId == billingMethodId
                                                     && x.BillingPaidById == billingPaidById && x.CurrencyId == currencyId)))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //get customer price card details to show in quotation
        public async Task<QuotationPriceCard> GetCustomerPriceCardData(CustomerPriceCardRequest request)
        {
            try
            {
                var response = new QuotationPriceCard();

                int ruleId = 0;

                if (request == null)
                {
                    return new QuotationPriceCard() { Result = ResponseResult.RequestNotCorrectFormat };
                }

                // set if the Rule is valid
                if (request.RuleId > 0)
                {
                    ruleId = request.RuleId.GetValueOrDefault();
                }
                else
                {
                    // fetch if the Rule is not exist
                    var bookingRuleInfoList = await GetBookingAndRuleList(new int[] { request.BookingId }, request.BillPaidById, request.BillMethodId);

                    if (bookingRuleInfoList != null)
                    {
                        var bookingOrder = bookingRuleInfoList.invoiceBookings.FirstOrDefault();

                        if (bookingOrder != null)
                        {
                            var ruleConfigs = await _invoiceManager.GetRuleConfigListbyBookingFilter(bookingOrder, bookingRuleInfoList.customerPriceCards);

                            // set even we have multiple Rule
                            ruleId = ruleConfigs.Select(x => x.Id).FirstOrDefault();

                            if (ruleId > 0 && request.QuotationId > 0)
                            {
                                // update the Rule with Quotation if the data is found

                                var quotationData = await _QuotationRepository.GetOnlyQuotation(request.QuotationId);

                                if (quotationData != null)
                                {
                                    quotationData.RuleId = ruleId;

                                    _QuotationRepository.Save(quotationData);
                                }
                            }
                        }
                    }
                }
                if (ruleId > 0)
                {

                    var customerPriceCard = await _customerPriceCardRepository.GetQuotationCustomerPriceCardData(ruleId);

                    if (customerPriceCard != null)
                    {

                        var priceIdList = new[] { ruleId }.ToList();

                        ExportMapRequest mapRequest = new ExportMapRequest();

                        mapRequest.ProductCategory = await _customerPriceCardRepository.GetPrProductCategories(priceIdList);

                        mapRequest.ServiceType = await _customerPriceCardRepository.GetPrServiceTypes(priceIdList);

                        mapRequest.CountryData = await _customerPriceCardRepository.GetPrCountries(priceIdList);

                        mapRequest.DeptData = await _customerPriceCardRepository.GetPrDepartment(priceIdList);

                        mapRequest.BuyerData = await _customerPriceCardRepository.GetPrBuyer(priceIdList);

                        mapRequest.BrandData = await _customerPriceCardRepository.GetPrBrand(priceIdList);

                        mapRequest.PriceCategory = await _customerPriceCardRepository.GetPrPriceCategory(priceIdList);

                        mapRequest.ProvinceData = await _customerPriceCardRepository.GetPrProvince(priceIdList);
                        response.PriceCardDetails = CustomerPriceCardMap.GetPriceCardQuotationMap(customerPriceCard, mapRequest);
                        response.Result = ResponseResult.Success;
                    }
                    else
                    {
                        return new QuotationPriceCard() { Result = ResponseResult.NotFound };
                    }
                }
                else
                {
                    return new QuotationPriceCard() { Result = ResponseResult.NotFound };
                }

                return response;
            }
            catch (Exception ex)
            {
                return new QuotationPriceCard() { Result = ResponseResult.Error };
            }
        }


        /// <summary>
        /// Get the sampling unit price
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="customerPriceCard"></param>
        /// <returns></returns>
        public async Task<double> GetSamplingUnitPrice(int bookingId, int priceId)
        {
            #region VariableDeclaration

            double unitPrice = 0;
            int maxProductCount = 0;
            bool sampleSizeBySet = false;
            int rowIndex = 0;
            List<int> combineGroupIds = new List<int>();
            List<CustomerPriceBookingProduct> bookingProductList = new List<CustomerPriceBookingProduct>();

            #endregion

            //Get the Inspection Product Transaction Data
            var productTransactions = await _insprepo.GetBookingProductDetails(bookingId);
            //Get the customer price card sampling data
            var customerPriceCardSampling = await _customerPriceCardRepository.GetCustomerPriceCardDetail(priceId);
            if (productTransactions != null && productTransactions.Any() && customerPriceCardSampling != null)
            {
                //Sort the product transaction data by product name
                productTransactions = productTransactions.OrderBy(x => x.Name).ToList();

                //check if samplesizebyset enabled or not
                if (customerPriceCardSampling.SampleSizeBySet != null)
                    sampleSizeBySet = customerPriceCardSampling.SampleSizeBySet.GetValueOrDefault();

                //check if the maxproductcount is there for the rule.if yes assign the value
                if (customerPriceCardSampling.MaxProductCount != null)
                {
                    maxProductCount = customerPriceCardSampling.MaxProductCount.GetValueOrDefault();
                }

                //take the product length
                var productLength = productTransactions.Count();

                if (maxProductCount == 0)
                {
                    maxProductCount = productLength;
                }

                //loop through the product transaction list
                foreach (var productTransaction in productTransactions)
                {
                    //check if the product is combined and add it into the customer booking product list and add combineproductid into combinegroupids list.
                    //so the next time we wont add product belongs to the same group
                    if (productTransaction.CombineProductId != null && !combineGroupIds.Contains(productTransaction.CombineProductId.GetValueOrDefault()))
                    {
                        //take combine aql quantity.since combine aql quantity will be updated for one product per combine group
                        var combineAQLQuantity = productTransactions.Where(x => x.CombineProductId == productTransaction.CombineProductId
                                                       && x.CombinedAQLQuantity > 0).Select(x => x.CombinedAQLQuantity).FirstOrDefault();
                        //add the customer price booking product
                        bookingProductList.Add(AddCustomerPriceCardTransaction(productTransaction, sampleSizeBySet, combineAQLQuantity));
                        //add the combine product id into the conbinegroupids list
                        combineGroupIds.Add(productTransaction.CombineProductId.GetValueOrDefault());
                    }
                    else if (productTransaction.CombineProductId == null)
                    {
                        //add the customer price booking product
                        bookingProductList.Add(AddCustomerPriceCardTransaction(productTransaction, sampleSizeBySet, null));
                    }

                    //update row read in the product list
                    rowIndex = rowIndex + 1;

                    //calculate the unit price if booking product list count equals to the maxproductcount(if it is set)
                    //or all the rows in the product list has been read
                    if ((maxProductCount > 0 && bookingProductList.Count() == maxProductCount) || (productLength - rowIndex) == 0)
                    {
                        //calculate the unit price with sampling data
                        unitPrice = unitPrice + CalculateUnitPrice(bookingProductList, customerPriceCardSampling);
                        //clear the product list so that the list will be fresh for the next cycle.
                        bookingProductList.Clear();
                    }

                }

                //if the calculate unit price less than the min billing day take the min billing day
                if (customerPriceCardSampling.MinBillingDay != null && unitPrice < customerPriceCardSampling.MinBillingDay)
                    unitPrice = customerPriceCardSampling.MinBillingDay.GetValueOrDefault();

            }
            return unitPrice;
        }

        /// <summary>
        /// Instantiate the customer PriceCard Booking Product
        /// </summary>
        /// <param name="productTransaction"></param>
        /// <param name="sampleSizeBySet"></param>
        /// <param name="combineAQLQuantity"></param>
        /// <returns></returns>
        private CustomerPriceBookingProduct AddCustomerPriceCardTransaction(CustomerPriceBookingProductRepo productTransaction, bool sampleSizeBySet, int? combineAQLQuantity)
        {
            CustomerPriceBookingProduct bookingProduct = new CustomerPriceBookingProduct();
            bookingProduct.Id = productTransaction.Id;
            bookingProduct.Name = productTransaction.Name;

            //if the product is combine assign the conbined aql quantity
            if (productTransaction.CombineProductId != null && combineAQLQuantity != null && combineAQLQuantity > 0)
                bookingProduct.SamplingSize = combineAQLQuantity.GetValueOrDefault();
            //if the product is non combined add the aql quantity
            else if (productTransaction.CombineProductId == null && productTransaction.AQLQuantity != null && productTransaction.AQLQuantity > 0)
                bookingProduct.SamplingSize = productTransaction.AQLQuantity.GetValueOrDefault();

            //if samplesizebyset is enabled calculate samplingsize with unitcount value
            if (sampleSizeBySet && productTransaction.UnitCount != null && productTransaction.UnitCount > 0)
                bookingProduct.SamplingSize = bookingProduct.SamplingSize * productTransaction.UnitCount.GetValueOrDefault();

            return bookingProduct;
        }

        /// <summary>
        /// Calculate the unit price
        /// </summary>
        /// <param name="bookingProductList"></param>
        /// <param name="customerPriceCardSampling"></param>
        /// <returns></returns>
        private double CalculateUnitPrice(List<CustomerPriceBookingProduct> bookingProductList, CustomerPriceCardRepo customerPriceCardSampling)
        {
            double unitPrice = 0;

            //calculate the total sample size
            var totalSamplingSize = bookingProductList.Sum(x => x.SamplingSize);
            //assign the max sample size
            var maxSampleSize = customerPriceCardSampling.MaxSampleSize.GetValueOrDefault();

            if (totalSamplingSize > 0 && maxSampleSize > 0)
            {
                //if total sampling size less than max sample size
                if (totalSamplingSize <= maxSampleSize)
                {
                    //take the unit price for the total sampling size
                    unitPrice = unitPrice + GetQuantitySamplingValue(totalSamplingSize, customerPriceCardSampling);
                }
                //if total sampling size greater than max sample size
                else if (totalSamplingSize > maxSampleSize)
                {
                    //if additional sample size is configured
                    if (customerPriceCardSampling.AdditionalSampleSize != null && customerPriceCardSampling.AdditionalSampleSize > 0)
                    {
                        //take the unit price for the max sample size
                        unitPrice = unitPrice + GetQuantitySamplingValue(maxSampleSize, customerPriceCardSampling);

                        //take the additional sample size
                        var additionalSampleSize = customerPriceCardSampling.AdditionalSampleSize.GetValueOrDefault();

                        //calculate the remaining sample size
                        int remainingSampleSize = totalSamplingSize - maxSampleSize;

                        //take no of times additional sample size available in the remaining samplesize.
                        int additionSampleSizeCount = remainingSampleSize / additionalSampleSize;

                        //if additional sampleprice greater than zero
                        if (customerPriceCardSampling.AdditionalSamplePrice != null && customerPriceCardSampling.AdditionalSamplePrice > 0)
                        {
                            //calculate the no of times additional sample size involved * additional sample price
                            //add it into the existing unit price
                            unitPrice = unitPrice + (additionSampleSizeCount * customerPriceCardSampling.AdditionalSamplePrice.GetValueOrDefault());
                        }

                        //calculate the remaining sample size again
                        remainingSampleSize = remainingSampleSize - (additionSampleSizeCount * additionalSampleSize);

                        //if it is there additional one more additional sample price to existing unit price
                        if (remainingSampleSize > 0)
                            unitPrice = unitPrice + customerPriceCardSampling.AdditionalSamplePrice.GetValueOrDefault();

                    }
                    //if additional sample size not configured
                    else
                    {
                        //calculate no of times maxsamplesize available with the total sampling size
                        int sampleSizeCount = totalSamplingSize / maxSampleSize;
                        int remainingSampleSize = 0;
                        //take the unit price for the maximum sample size
                        unitPrice = unitPrice + GetQuantitySamplingValue(maxSampleSize, customerPriceCardSampling);
                        //if sample size greater than one
                        if (sampleSizeCount > 1)
                        {
                            //multiply unitprice with no of samplesize involved in the total sampling size
                            unitPrice = unitPrice * sampleSizeCount;
                            //calc remaining sample size
                            remainingSampleSize = totalSamplingSize - (maxSampleSize * sampleSizeCount);
                        }
                        else
                        {
                            //calc remaining sample size
                            remainingSampleSize = totalSamplingSize - maxSampleSize;
                        }
                        //if remaining sample size is there then take the remaining sample size price and add it to existing unit price

                        if (remainingSampleSize > 0)
                            unitPrice = unitPrice + GetQuantitySamplingValue(remainingSampleSize, customerPriceCardSampling);

                    }
                }
            }


            return unitPrice;
        }

        /// <summary>
        /// Get the quantity sampling price
        /// </summary>
        /// <param name="totalSamplingSize"></param>
        /// <param name="customerPriceCardSampling"></param>
        /// <returns></returns>
        private double GetQuantitySamplingValue(int totalSamplingSize, CustomerPriceCardRepo customerPriceCardSampling)
        {
            if (totalSamplingSize >= 1 && totalSamplingSize <= 8 && customerPriceCardSampling.Quantity8 != null && customerPriceCardSampling.Quantity8 > 0)
                return customerPriceCardSampling.Quantity8.GetValueOrDefault();

            else if (totalSamplingSize >= 9 && totalSamplingSize <= 13 && customerPriceCardSampling.Quantity13 != null && customerPriceCardSampling.Quantity13 > 0)
                return customerPriceCardSampling.Quantity13.GetValueOrDefault();

            else if (totalSamplingSize >= 14 && totalSamplingSize <= 20 && customerPriceCardSampling.Quantity20 != null && customerPriceCardSampling.Quantity20 > 0)
                return customerPriceCardSampling.Quantity20.GetValueOrDefault();

            else if (totalSamplingSize >= 21 && totalSamplingSize <= 32 && customerPriceCardSampling.Quantity32 != null && customerPriceCardSampling.Quantity32 > 0)
                return customerPriceCardSampling.Quantity32.GetValueOrDefault();

            else if (totalSamplingSize >= 33 && totalSamplingSize <= 50 && customerPriceCardSampling.Quantity50 != null && customerPriceCardSampling.Quantity50 > 0)
                return customerPriceCardSampling.Quantity50.GetValueOrDefault();

            else if (totalSamplingSize >= 51 && totalSamplingSize <= 80 && customerPriceCardSampling.Quantity80 != null && customerPriceCardSampling.Quantity80 > 0)
                return customerPriceCardSampling.Quantity80.GetValueOrDefault();

            else if (totalSamplingSize >= 81 && totalSamplingSize <= 125 && customerPriceCardSampling.Quantity125 != null && customerPriceCardSampling.Quantity125 > 0)
                return customerPriceCardSampling.Quantity125.GetValueOrDefault();

            else if (totalSamplingSize >= 126 && totalSamplingSize <= 200 && customerPriceCardSampling.Quantity200 != null && customerPriceCardSampling.Quantity200 > 0)
                return customerPriceCardSampling.Quantity200.GetValueOrDefault();

            else if (totalSamplingSize >= 201 && totalSamplingSize <= 315 && customerPriceCardSampling.Quantity315 != null && customerPriceCardSampling.Quantity315 > 0)
                return customerPriceCardSampling.Quantity315.GetValueOrDefault();

            else if (totalSamplingSize >= 316 && totalSamplingSize <= 500 && customerPriceCardSampling.Quantity500 != null && customerPriceCardSampling.Quantity500 > 0)
                return customerPriceCardSampling.Quantity500.GetValueOrDefault();

            else if (totalSamplingSize >= 501 && totalSamplingSize <= 800 && customerPriceCardSampling.Quantity800 != null && customerPriceCardSampling.Quantity800 > 0)
                return customerPriceCardSampling.Quantity800.GetValueOrDefault();

            else if (totalSamplingSize >= 801 && totalSamplingSize <= 1250 && customerPriceCardSampling.Quantity1250 != null && customerPriceCardSampling.Quantity1250 > 0)
                return customerPriceCardSampling.Quantity1250.GetValueOrDefault();
            return 0;
        }

        /// <summary>
        /// Check public holiday exists between two date ranges
        /// </summary>
        /// <param name="inspectionDateList"></param>
        /// <returns></returns>
        private async Task<bool> CheckPublicHolidayExists(InvoiceBookingDetail bookingDetail)
        {
            if (bookingDetail != null)
            {
                var holidayData = await _QuotationRepository.GetHolidayInfo(bookingDetail.ServiceFrom, bookingDetail.ServiceTo,
                                                                                                  bookingDetail.OfficeId);
                if (holidayData != null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get the holiday unit price if holiday types are configured
        /// </summary>
        /// <param name="customerPriceCardDetails"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        private async Task<double> GetHolidayUnitPrice(IEnumerable<CustomerPriceHolidayType> holidayTypeList, InvoiceBookingDetail bookingFilters, double holidayPrice)
        {
            double unitPrice = 0;

            if (bookingFilters != null)
            {
                var bookingDetail = bookingFilters;

                //if holiday type configured other than public holidays(Sun,Mon..) then booking service dates matching with configured days
                if (holidayTypeList.Any(x => x.Active.HasValue && x.Active.Value &&
                                                                  x.HolidayInfoId != (int)CustomerPriceHolidayInfoEnum.PublicHoliday))
                {

                    var priceCardHolidayList = holidayTypeList.Where(x => x.Active.HasValue && x.Active.Value &&
                                                                  x.HolidayInfoId != (int)CustomerPriceHolidayInfoEnum.PublicHoliday);

                    //take the day list between the booking servicedates
                    List<int> dayList = new List<int>();
                    for (DateTime currentDate = bookingDetail.ServiceFrom; currentDate <= bookingDetail.ServiceTo; currentDate = currentDate.AddDays(1))
                    {
                        dayList.Add((int)currentDate.DayOfWeek + 1);
                    }

                    //if configured days matched with the booking service dates then add the unit price
                    if (priceCardHolidayList.Any(x => dayList.Contains(x.HolidayInfoId)))
                        unitPrice = holidayPrice;
                }
                //if holiday type configured with public holidays check if it is available with the holiday calendar if yes add it to the unit price
                else if (holidayTypeList.Any(x => x.Active.HasValue && x.Active.Value &&
                                                                x.HolidayInfoId == (int)CustomerPriceHolidayInfoEnum.PublicHoliday))
                {
                    var isHolidayExists = await CheckPublicHolidayExists(bookingDetail);
                    if (isHolidayExists)
                        unitPrice = holidayPrice;
                }
            }

            return unitPrice;
        }

        /// <summary>
        /// Get Sampling unit price data for the bookingids
        /// </summary>
        /// <param name="samplingUnitPriceRequest"></param>
        /// <returns></returns>
        public async Task<SamplingUnitPriceResponse> GetSamplingUnitPriceByBooking(List<SamplingUnitPriceRequest> samplingUnitPriceRequest)
        {
            var response = new SamplingUnitPriceResponse();

            if (samplingUnitPriceRequest != null && samplingUnitPriceRequest.Any())
            {
                response.SamplingUnitPriceList = new List<SamplingUnitPrice>();

                var bookingIds = samplingUnitPriceRequest.Select(x => x.BookingId);

                if (bookingIds != null)
                {
                    var bookingRuleInfoList = await GetBookingAndRuleList(bookingIds, 0, 0);

                    if (bookingRuleInfoList != null)
                    {
                        foreach (var samplingBookingData in samplingUnitPriceRequest)
                        {

                            var invoiceList = new List<InvoiceDetail>();

                            var bookingOrder = bookingRuleInfoList.invoiceBookings.FirstOrDefault(x => x.BookingId == samplingBookingData.BookingId);

                            var ruleConfigs = await _invoiceManager.GetRuleConfigListbyBookingFilter(bookingOrder, bookingRuleInfoList.customerPriceCards);

                            var ruleConfig = ruleConfigs.FirstOrDefault(x => x.Id == samplingBookingData.PriceCardId);

                            if (bookingOrder != null && ruleConfig != null)
                            {
                                // set default invoice fees from option - only from quotation even if it is from diffrent rule
                                ruleConfig.InvoiceInspFeeFrom = (int)InvoiceFeesFrom.Invoice;

                                bookingOrder.RuleConfig = ruleConfig;
                                var quotationBooking = new List<QuotationBooking>();
                                var invoiceBookingList = new List<InvoiceBookingDetail>() { bookingOrder };

                                // get invoice list by price calculations
                                invoiceList = await _invoiceManager.GetInvoiceListbyPriceCalculations(invoiceBookingList, invoiceList, ruleConfig, bookingRuleInfoList.InvoiceGenerateRequest, quotationBooking);
                                if (invoiceList.Any())
                                {
                                    SamplingUnitPrice samplingUnitPrice = new SamplingUnitPrice();
                                    samplingUnitPrice.BookingId = samplingBookingData.BookingId;
                                    samplingUnitPrice.UnitPrice = invoiceList.Where(x => x.InspectionId == bookingOrder.BookingId && x.UnitPrice > 0)
                                                                  .Select(x => x.UnitPrice.GetValueOrDefault()).FirstOrDefault();
                                    samplingUnitPrice.Remarks = invoiceList.Where(x => x.InspectionId == bookingOrder.BookingId)
                                                                  .Select(x => x.Remarks).FirstOrDefault();
                                    response.SamplingUnitPriceList.Add(samplingUnitPrice);
                                }
                            }
                        }

                    }
                }

                if (response.SamplingUnitPriceList != null && response.SamplingUnitPriceList.Count > 0)
                {
                    response.Result = SamplingUnitPriceResult.Success;
                }
                else
                {
                    response.Result = SamplingUnitPriceResult.NotFound;
                }
            }

            return response;
        }
        /* If reschedule happens need to add new service date in quotation*/
        public async Task UpdateQuotationServiceDateReschdule(BookingDateInfo request, InspTransaction entity, int userId)
        {
            try
            {
                //if request and entity as same date we should not update quotation date
                if (!(request.ServiceFromDate.ToDateTime() == entity.ServiceDateFrom &&
                       request.ServiceToDate.ToDateTime() == entity.ServiceDateTo))
                {
                    bool quotationExists = await _QuotationRepository.QuotationInspExists(request.BookingId);

                    if (quotationExists)
                    {
                        DateTime[] dateList = { };

                        //to date check
                        if (request.ServiceToDate.ToDateTime() > entity.ServiceDateTo)
                        {
                            dateList = Enumerable.Range(1, 0 + request.ServiceToDate.ToDateTime().Subtract(entity.ServiceDateTo).Days)
                   .Select(offset => entity.ServiceDateTo.AddDays(offset)).ToArray();
                        }

                        if (dateList.Any())
                        {
                            var quotationInspDateMandayList = await _QuotationRepository.GetQuotationInspManDay(request.BookingId);

                            if (quotationInspDateMandayList != null && quotationInspDateMandayList.Count() > 0)
                            {
                                var quotationRecord = quotationInspDateMandayList.FirstOrDefault();

                                foreach (DateTime serviceDate in dateList)
                                {
                                    var dateManday = new QuQuotationInspManday
                                    {
                                        BookingId = request.BookingId,
                                        QuotationId = quotationRecord.QuotationId,
                                        NoOfManday = 0,
                                        Remarks = null,
                                        ServiceDate = serviceDate,
                                        CreatedBy = userId,
                                        Active = true
                                    };
                                    //entity add
                                    _QuotationRepository.AddEntity(dateManday);
                                }
                                //Quotation Remarks update
                                UpdateQuotationAPIRemarkData(request, quotationRecord);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //get the pending quotations for the mobile app
        public async Task<List<MobileInspQuotationData>> GetMobileQuotationData(InspectionSummarySearchRequest request)
        {
            var data = _QuotationRepository.GetMobileQuotationDetails();
            var isNotPendingRequest = false;
            // filter data based on user type
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        request.CustomerId = request?.CustomerId != null && request?.CustomerId != 0 ? request?.CustomerId : _ApplicationContext.CustomerId;
                        data = data.Where(x => x.BilledTo == (int)QuotationPaidBy.customer);
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        request.FactoryIdlst = request.FactoryIdlst != null && request.FactoryIdlst.Count() > 0 ? request.FactoryIdlst : new List<int>().Append(_ApplicationContext.FactoryId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        request.SupplierId = request.SupplierId != null && request.SupplierId != 0 ? request.SupplierId.Value : _ApplicationContext.SupplierId;
                        break;
                    }
            }

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 20;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            if (request != null && request.quotationId != null && request.quotationId != 0)
            {
                data = data.Where(x => x.QuotationId == request.quotationId);
            }

            if (request != null && request.CustomerId != 0 && request.CustomerId != null)
            {
                data = data.Where(x => x.CustomerId == request.CustomerId);
            }

            if (request != null && request.SupplierId != 0 && request.SupplierId != null)
            {
                data = data.Where(x => x.SupplierId == request.SupplierId);
            }

            if (request != null && request.FactoryIdlst != null && request.FactoryIdlst.Count() > 0)
            {
                data = data.Where(x => request.FactoryIdlst.ToList().Contains(x.FactoryId.GetValueOrDefault()));
            }

            if (request != null && request.StatusIdlst != null && request.StatusIdlst.Count() > 0)
            {
                data = data.Where(x => request.StatusIdlst.ToList().Contains(x.BookingStatusId));
            }
            if (request != null && request.QuotationsStatusIdlst != null && request.QuotationsStatusIdlst.Any())
            {
                data = data.Where(x => request.QuotationsStatusIdlst.ToList().Contains(x.QuotationStatusId));
                isNotPendingRequest = request.QuotationsStatusIdlst.Any(x => x != (int)QuotationStatus.SentToClient);
            }
            if (Enum.TryParse(request.DateTypeid.ToString(), out SearchType _datesearchtype))
            {
                if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                {
                    switch (_datesearchtype)
                    {
                        case SearchType.ServiceDate:
                            {
                                if (isNotPendingRequest)//don't set the date for pending request 
                                    data = data.Where(x => !((x.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.ServiceDateTo < request.FromDate.ToDateTime())));
                                break;
                            }
                    }
                }
            }

            if (request.SearchTypeId == (int)SearchType.BookingNo)
            {
                if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()) && int.TryParse(request.SearchTypeText?.Trim(), out int bookid))
                {
                    data = data.Where(x => x.BookingId == bookid);
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()) && int.TryParse(request.SearchTypeText?.Trim(), out int quotId))
                {
                    data = data.Where(x => x.QuotationId == quotId);
                }
            }

            //execute the data - IEnumerable
            var list = await data.OrderByDescending(x => x.ServiceDateFrom).Skip(skip).Take(request.pageSize.Value).ToListAsync();

            //fetch the bookingIds
            var bookingIdList = list.Select(x => x.BookingId).Distinct().ToList();

            //Get the product List
            var productList = await _insprepo.GetProductListByBooking(bookingIdList);

            //fetch data from quotation status log to get the customer approved date of the quotation
            var quotApprovalData = await _customKpiRepo.GetQuotationStatusLogById(bookingIdList);

            var result = QuotationMobileMap.MapQuotationProducts(list, productList, quotApprovalData, request.Index.GetValueOrDefault());

            return result;
        }

        public async Task<DataSourceResponse> GetBillPaidByList()
        {
            var response = new DataSourceResponse();

            //Bill Paid By 
            var billPaidByList = await _QuotationRepository.GetPaidByList();

            if (billPaidByList == null || !billPaidByList.Any())
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };

            response.DataSourceList = billPaidByList.Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Label
            });

            response.Result = DataSourceResult.Success;

            return response;
        }

        public async Task<QuotationSummaryStatusResponse> GetQuotationStatusColor()
        {
            var response = new QuotationSummaryStatusResponse();

            //Bill Paid By 
            var statusList = await _QuotationRepository.GetStatusList();

            if (statusList == null || !statusList.Any())
                return new QuotationSummaryStatusResponse { Result = QuotationSummaryStatusResult.CannotGetList };

            response.QuotationStatusList = statusList.Select(QuotationMap.GetQuotationStatuswithColor).ToList();

            response.Result = QuotationSummaryStatusResult.Success;

            return response;
        }

        //get the pending quotation for the mobile app
        public async Task<InspQuotationMobileResponse> GetMobilePendingQuotation(InspSummaryMobileRequest request)
        {
            var response = new InspQuotationMobileResponse();
            try
            {
                if (_ApplicationContext.RoleList.Contains((int)RoleEnum.QuotationConfirmation))
                {
                    InspectionSummarySearchRequest inspRequest = RequestMobileMap.MapInspRequest(request);
                    response.data = await GetMobileQuotationData(inspRequest);
                    response.meta = new MobileResult { success = true, message = "" };
                }
                else
                {
                    response.meta = new MobileResult { success = true, message = "no data found." };
                }
            }

            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Quotation fetch failed." };
            }

            return response;
        }
        //get the quotation status for mobile
        public FilterDataSourceResponse GetMobileQuotationStatus()
        {
            var response = new FilterDataSourceResponse();

            try
            {
                var _key = 1;

                response.data = MobileQuotationStatus.Select(x => new FilterDataSource
                {
                    key = _key++,
                    id = x.Key,
                    name = x.Value
                }).ToList();

                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "not Found" };
            }

            return response;
        }

        //check if the customer has skip quotation sent to client checkpoint configured
        public async Task<bool> GetSkipQuotationSentToClientCheckpoint(QuotCheckpointRequest request)
        {
            bool brandCheckpointExists = true;
            bool deptCheckpointExists = true;
            bool serviceTypeCheckpointExists = true;

            //fetch the checkpoint if "skip quotation sent to client" exists
            var checkpointData = await _QuotationRepository.GetCheckpointByCustomerId(request.CustomerId);

            if (checkpointData != null && checkpointData.CheckpointId > 0)
            {
                checkpointData.BrandIdList = await _QuotationRepository.GetCheckpointBrandByCheckpointId(checkpointData.CheckpointId);

                checkpointData.DeptList = await _QuotationRepository.GetCheckpointDepartmentByCheckpointId(checkpointData.CheckpointId);

                checkpointData.ServiceTypeIdList = await _QuotationRepository.GetCheckpointServiceTypeByCheckpointId(checkpointData.CheckpointId);

                var bookingDataList = await _QuotationRepository.GetBookingDetails(request.BookingIdList);

                var brandList = await _insprepo.GetBrandBookingIdsByBookingIds(request.BookingIdList);

                var deptList = await _insprepo.GetDeptBookingIdsByBookingIds(request.BookingIdList);

                var serviceTypeList = await _insprepo.GetServiceType(request.BookingIdList);

                foreach (var bookingData in bookingDataList)
                {

                    //check if checkpoint brand is selected in booking
                    if (checkpointData.BrandIdList.Any())
                    {
                        brandCheckpointExists = brandList.Where(x => x.BookingId == bookingData.BookingId).Any(y => checkpointData.BrandIdList.Any(z => z == y.BrandId));
                    }

                    //check if checkpoint dept is selected in booking
                    if (checkpointData.DeptList.Any())
                    {
                        deptCheckpointExists = deptList.Where(x => x.BookingId == bookingData.BookingId).Any(y => checkpointData.DeptList.Any(z => z == y.DeptId));
                    }

                    //check if checkpoint service type is selected in booking
                    if (checkpointData.ServiceTypeIdList.Any())
                    {
                        serviceTypeCheckpointExists = serviceTypeList.Where(x => x.InspectionId == bookingData.BookingId).Any(y => checkpointData.ServiceTypeIdList.Any(z => z == y.serviceTypeId));
                    }

                    if (brandCheckpointExists && deptCheckpointExists && serviceTypeCheckpointExists)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<string> GetquotationPdfPath(int quotationId)
        {
            return await _QuotationRepository.GetquotationPdfPath(quotationId);
        }

        public async Task<CalculatedWorkingHoursResponse> GetCalculatedWorkingManday(int bookingId)
        {
            var serviceTypeId = await _insprepo.GetBookingServiceTypes(bookingId);

            if (!Static_Data_Common.ContainerServiceList.Contains(serviceTypeId.FirstOrDefault()))
            {

                var productList = await _QuotationRepository.GetQuotationBookingProductsByBookingIds(new[] { bookingId });

                if (productList.Any(x => !(x.ProdSubCategory3Id > 0)))
                {
                    return new CalculatedWorkingHoursResponse { Result = CalculateWorkingHoursResult.ProdCatSub3NotMapped };
                }

                if (productList.Any(x => x.ProdUnit != (int)BookingProductUnitType.Pcs))
                {
                    return new CalculatedWorkingHoursResponse { Result = CalculateWorkingHoursResult.UnitNotPcs };
                }

                var cusReqIndex = await _QuotationRepository.GetCustomerRequirementIndex(bookingId);

                var res = QuotationMap.CalWorkingHours(productList, cusReqIndex);

                // res = res + 0.5;//based on formula

                return new CalculatedWorkingHoursResponse
                {
                    CalculatedWorkingHours = Math.Round(res, 2),
                    CalculatedManday = Math.Round(res / 8, 2),
                    Result = CalculateWorkingHoursResult.Success
                };
            }
            else
            {
                return new CalculatedWorkingHoursResponse
                {
                    CalculatedWorkingHours = 0,
                    CalculatedManday = 0,
                    Result = CalculateWorkingHoursResult.Success
                };
            }
        }


        //get travel matrix info based on price card data
        public async Task<QuotationTravelMatrixResponse> GetTravelMatrixData(TravelMatrixRequest request)
        {
            try
            {
                int ruleId = 0;

                if (request == null)
                {
                    return new QuotationTravelMatrixResponse() { Result = TravelMatrixResponseResult.NotFound };
                }

                // set if the Rule is valid
                if (request.RuleId > 0)
                {
                    ruleId = request.RuleId.GetValueOrDefault();
                }
                else
                {
                    // fetch if the Rule is not exist
                    var bookingRuleInfoList = await GetBookingAndRuleList(new int[] { request.BookingId }, request.BillPaidById, request.BillMethodId);

                    if (bookingRuleInfoList != null)
                    {
                        var bookingOrder = bookingRuleInfoList.invoiceBookings.FirstOrDefault();

                        if (bookingOrder != null)
                        {
                            var ruleConfigs = await _invoiceManager.GetRuleConfigListbyBookingFilter(bookingOrder, bookingRuleInfoList.customerPriceCards);

                            // set even we have multiple Rule
                            ruleId = ruleConfigs.Select(x => x.Id).FirstOrDefault();

                            if (ruleId > 0 && request.QuotationId > 0)
                            {
                                // update the Rule with Quotation if the data is found

                                var quotationData = await _QuotationRepository.GetOnlyQuotation(request.QuotationId);

                                if (quotationData != null)
                                {
                                    quotationData.RuleId = ruleId;

                                    _QuotationRepository.Save(quotationData);
                                }
                            }
                            else
                            {
                                return new QuotationTravelMatrixResponse() { Result = TravelMatrixResponseResult.NotFound };
                            }
                        }
                    }
                }

                if (ruleId > 0)
                {
                    //get customer price card details
                    var priceCardDetail = await _customerPriceCardRepository.GetQuotationPriceCard(ruleId);

                    //if price card success
                    if (priceCardDetail != null)
                    {

                        // factory county, price card matrix type and currency valid check
                        if (priceCardDetail.CurrencyId <= 0 || priceCardDetail.TravelMatrixTypeId == null
                                || priceCardDetail.TravelMatrixTypeId <= 0)
                        {
                            return new QuotationTravelMatrixResponse() { Result = TravelMatrixResponseResult.NotFound }; //RequestNotCorrectFormat 
                        }
                        else
                        {
                            var quotationMatrixRequest = MatrixMapRequest(request, priceCardDetail);

                            ///get the executed travel matrix search list
                            var matrixList = await GetTravelMatrixList(quotationMatrixRequest);

                            if (matrixList.Count() > 1) //travel matrix duplicate found
                            {
                                return new QuotationTravelMatrixResponse() { Result = TravelMatrixResponseResult.MoreMatrixExists };
                            }
                            else if (matrixList.Count() == 1)
                            {
                                var travelMatrixDetails = matrixList.FirstOrDefault();
                                //append the travel configured data with the remarks
                                travelMatrixDetails.Remarks = travelMatrixDetails.Remarks + GetTravelMatrixRemarks(request);

                                return new QuotationTravelMatrixResponse()
                                {
                                    TravelMatrixDetails = matrixList.FirstOrDefault(),

                                    Result = TravelMatrixResponseResult.Success
                                };
                            }
                            else
                            {
                                return new QuotationTravelMatrixResponse() { Result = TravelMatrixResponseResult.NotFound };
                            }
                        }
                    }
                    else
                    {
                        return new QuotationTravelMatrixResponse() { Result = TravelMatrixResponseResult.PriceCardNotCorrect };
                    }
                }
                else
                {
                    return new QuotationTravelMatrixResponse() { Result = TravelMatrixResponseResult.PriceCardNotCorrect };
                }


            }
            catch (Exception)
            {
                return new QuotationTravelMatrixResponse() { Result = TravelMatrixResponseResult.Error };
            }
        }

        public async Task<List<TravelMatrixSearch>> GetTravelMatrixList(QuotationMatrixRequest request)
        {
            //get the travel matrix query
            var travelMatrixQuery = _travelMatrixRepository.GetTravelMatrixData(request);

            //apply the travel matrix county id
            if (request.CountyId > 0)
                travelMatrixQuery = travelMatrixQuery.Where(x => x.CountyId == request.CountyId);
            //apply the travel matrix city id
            else if (request.CityId > 0)
                travelMatrixQuery = travelMatrixQuery.Where(x => x.CityId == request.CityId && x.CountyId == null);
            //apply the travel matrix province id
            else if (request.ProvinceId > 0)
                travelMatrixQuery = travelMatrixQuery.Where(x => x.ProvinceId == request.ProvinceId && x.CityId == null
                                                                            && x.CountyId == null);

            return await travelMatrixQuery.AsNoTracking().ToListAsync();
        }

        private CustomerPriceCardRequest PriceCardRequestMap(TravelMatrixRequest request)
        {
            return new CustomerPriceCardRequest()
            {
                BillMethodId = request.BillMethodId,
                BillPaidById = request.BillPaidById,
                CurrencyId = request.CurrencyId,
                BookingId = request.BookingId
            };
        }

        private QuotationMatrixRequest MatrixMapRequest(TravelMatrixRequest request, QuotationCustomerPriceCard priceCardDetails)
        {
            return new QuotationMatrixRequest()
            {
                CountyId = request.CountyId,
                CityId = request.CityId,
                ProvinceId = request.ProvinceId,
                CurrencyId = priceCardDetails.CurrencyId,
                MatrixTypeId = priceCardDetails.TravelMatrixTypeId,
                customerId = request.customerId
            };
        }

        private string GetTravelMatrixRemarks(TravelMatrixRequest request)
        {
            TravelMatrixConfig travelMatrixConfig = 0;
            if (request.CountyId > 0)
                travelMatrixConfig = TravelMatrixConfig.County;
            else if (request.CityId > 0)
                travelMatrixConfig = TravelMatrixConfig.City;
            else if (request.ProvinceId > 0)
                travelMatrixConfig = TravelMatrixConfig.Province;

            return "Travel Matrix Configured by " + travelMatrixConfig.ToString();
        }

        /// <summary>
        /// get travel include value from curprdetails table
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public async Task<PriceCardTravelResponse> GetPriceCardTravel(int ruleId)
        {
            var response = new PriceCardTravelResponse();

            if (ruleId > 0)
            {
                var isTravelInclude = await _QuotationRepository.GetPriceCardTravel(ruleId);

                if (isTravelInclude != null)
                {
                    response.IsTravelInclude = isTravelInclude.Value;
                    response.Result = PriceCardTravelResult.Success;
                }
                else
                {
                    response.Result = PriceCardTravelResult.NotFound;
                }
            }
            else
            {
                response.Result = PriceCardTravelResult.NotFound;
            }
            return response;
        }
        /// <summary>
        /// get calculated working manday and save the details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<CalculatedWorkingHoursResponse> SaveWorkingManday(int bookingId)
        {
            var response = await GetCalculatedWorkingManday(bookingId);

            if (response.Result == CalculateWorkingHoursResult.Success)
            {
                //get quotation insp data
                var quotationInsp = await _QuotationRepository.GetQuotationInsp(bookingId);

                quotationInsp.CalculatedWorkingManDay = response.CalculatedManday;
                quotationInsp.CalculatedWorkingHrs = response.CalculatedWorkingHours;
                quotationInsp.UpdatedOn = DateTime.Now;
                quotationInsp.UpdatedBy = _ApplicationContext.UserId;

                _QuotationRepository.Save(quotationInsp, true);

                response.SaveResult = CalculateManDaySaveResult.Success;
            }
            return response;
        }

        /// <summary>
        /// Get booking invoice details 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<object> GetEaqfInspectionInvoiceDetails(string bookingIds)
        {
            var response = new GetEaqfInspectionBookingInvoiceResponse();
            try
            {

                if (string.IsNullOrWhiteSpace(bookingIds))
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Booking Id is not valid" });
                }

                List<int> bookingList;
                try
                {
                    bookingList = bookingIds.Split(',').Select(int.Parse).Distinct().ToList();
                }
                catch (Exception ex)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Booking Id is not valid" });
                }

                if (bookingList == null || !bookingList.Any())
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Booking Id is not valid" });
                }

                var invoiceDataList = await _insprepo.GetEaqfBookingInvoiceDetails(bookingList);

                var invoiceNoList = invoiceDataList.Select(x => x.InvoiceNo).ToList();

                // update invoice pdf url
                if (invoiceNoList.Any())
                {
                    var invoiceFileDataList = await _insprepo.GetEaqfBookingInvoiceFileDetails(invoiceNoList);

                    foreach (var item in invoiceDataList)
                    {
                        item.InvoicePdfUrl = invoiceFileDataList.FirstOrDefault(x => x.InvoiceNo == item.InvoiceNo)?.InvoicePdfUrl;
                    }
                }

                if (!invoiceDataList.Any())
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = BadRequest,
                        statusCode = HttpStatusCode.BadRequest,
                        data = invoiceDataList
                    };
                }
                response.TotalCount = invoiceDataList.Count();
                response.EaqfBookingInvoiceData = invoiceDataList;
                return new EaqfGetSuccessResponse()
                {
                    message = Success,
                    statusCode = HttpStatusCode.OK,
                    data = response
                };
            }
            catch
            {
                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { InternalServerError });
            }
        }

        /// <summary>
        /// Get Audit booking invoice details 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<object> GetAuditEaqfInspectionInvoiceDetails(string bookingIds)
        {
            var response = new GetEaqfInspectionBookingInvoiceResponse();
            try
            {

                if (string.IsNullOrWhiteSpace(bookingIds))
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Booking Id is not valid" });
                }

                List<int> bookingList;
                try
                {
                    bookingList = bookingIds.Split(',').Select(int.Parse).Distinct().ToList();
                }
                catch (Exception ex)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Booking Id is not valid" });
                }

                if (bookingList == null || !bookingList.Any())
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Booking Id is not valid" });
                }

                var invoiceDataList = await _audprepo.GetAuditEaqfBookingInvoiceDetails(bookingList);

                var invoiceNoList = invoiceDataList.Select(x => x.InvoiceNo).ToList();

                // update invoice pdf url
                if (invoiceNoList.Any())
                {
                    var invoiceFileDataList = await _insprepo.GetEaqfBookingInvoiceFileDetails(invoiceNoList);

                    foreach (var item in invoiceDataList)
                    {
                        item.InvoicePdfUrl = invoiceFileDataList.FirstOrDefault(x => x.InvoiceNo == item.InvoiceNo)?.InvoicePdfUrl;
                    }
                }

                if (!invoiceDataList.Any())
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = BadRequest,
                        statusCode = HttpStatusCode.BadRequest,
                        data = invoiceDataList
                    };
                }
                response.TotalCount = invoiceDataList.Count();
                response.EaqfBookingInvoiceData = invoiceDataList;
                return new EaqfGetSuccessResponse()
                {
                    message = Success,
                    statusCode = HttpStatusCode.OK,
                    data = response
                };
            }
            catch
            {
                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { InternalServerError });
            }
        }

        public EaqfErrorResponse BuildCommonEaqfResponse(HttpStatusCode statusCode, string message, List<string> errors)
        {
            return new EaqfErrorResponse()
            {
                errors = errors,
                statusCode = statusCode,
                message = message
            };
        }

        public async Task<object> SaveEAQFQuotationAndInvoice(SaveQuotationEaqfRequest request)
        {
            var quotation = await SaveEaqfQuotation(request);
            var invoice = await _manualInvoiceManager.SaveEAQFManualInvoice(request);

            if (quotation != null)
            {
                System.Reflection.PropertyInfo pi = quotation.GetType().GetProperty("statusCode");
                if (pi != null)
                {
                    var statusCode = (HttpStatusCode)(pi.GetValue(quotation, null));
                    if (statusCode != HttpStatusCode.OK)
                    {
                        return quotation;
                    }
                }
            }
            else
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.InternalServerError, "Internal server error", new List<string>() { "Internal server error" });
                // return BuildCommonEaqfErrorResponse(HttpStatusCode.InternalServerError, "Internal server error", new List<string>() { "Internal server error" });
            }

            if (invoice != null)
            {
                var invoiceStatusCodePropertry = invoice.GetType().GetProperty("statusCode");
                if (invoiceStatusCodePropertry != null)
                {
                    var statusCode = (HttpStatusCode)(invoiceStatusCodePropertry.GetValue(invoice, null));
                    if (statusCode != HttpStatusCode.OK)
                    {
                        return invoice;
                    }
                }
            }
            else
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.InternalServerError, "Internal server error", new List<string>() { "Internal server error" });
            }


            var piObj = quotation.GetType().GetProperty("data");
            if (piObj != null)
            {
                var eaqfGetSuccessResponse = (SaveQuotationEaqfResponse)(piObj.GetValue(quotation, null));
                var invoiceObj = invoice.GetType().GetProperty("data");
                if (invoiceObj != null)
                {
                    var invoiceSuccessResposne = (EAQFInvoiceResponse)(invoiceObj.GetValue(invoice, null));
                    eaqfGetSuccessResponse.InvoiceId = invoiceSuccessResposne.InvoiceId;
                    eaqfGetSuccessResponse.InvoiceNo = invoiceSuccessResposne.InvoiceNo;
                    piObj.SetValue(quotation, eaqfGetSuccessResponse);
                }
            }

            return quotation;
        }
        public async Task<object> SaveEaqfQuotation(SaveQuotationEaqfRequest request)
        {
            // Add validation 
            if (request == null)
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Request can not be empty" });
            }

            if (request.Service != (int)(Service.InspectionId) && request.Service != (int)(Service.AuditId))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Service is invalid" });
            }

            if (string.IsNullOrEmpty(request.CurrencyCode))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Currency Code is not valid" });
            }

            if (request.UserId <= 0)
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "User is not valid" });
            }

            if (request.OrderDetails == null || !request.OrderDetails.Any())
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Order details is Required" });
            }

            //only one order fee comes in when create quotation
            if (request.OrderDetails.Count(x => x.OrderType == "orderfee") != 1)
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Accept only one order fee in order details" });
            }

            if (request.OrderDetails.Any(x => x.OrderType == "orderfee" && x.Manday <= 0))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Manday is required in order details" });
            }

            if (request.OrderDetails.Any(x => x.OrderType == "orderfee" && x.Amount <= 0))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Order fee is required in order details" });
            }

            if (request.OrderDetails.Any(x => x.OrderType == "extrafee" && x.Amount <= 0))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Extra fee is required in order details" });
            }

            if (request.OrderDetails.Any(x => x.OrderType == "discount" && x.Amount <= 0))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Discount is required in order details" });
            }

            double totalFees = 0;
            double orderFees = 0;
            double otherFees = 0;
            double discount = 0;

            if (request.OrderDetails.Any(x => x.OrderType == "orderfee"))
            {
                orderFees = request.OrderDetails.Where(x => x.OrderType == "orderfee").Select(x => x.Manday * x.Amount).Sum();
            }
            if (request.OrderDetails.Any(x => x.OrderType == "otherfee"))
            {
                otherFees = request.OrderDetails.Where(x => x.OrderType == "otherfee").Select(x => x.Amount).Sum();
            }

            if (request.OrderDetails.Any(x => x.OrderType == "discount"))
            {
                discount = request.OrderDetails.Where(x => x.OrderType == "discount").Select(x => x.Amount).Sum();
            }

            totalFees = orderFees + otherFees;

            if (totalFees < discount)
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Total discount is not greater than the total fees in order details" });
            }

            if (request.BookingId > 0)
            {
                await _eventBookingLog.SaveLogInformation(new EventBookingLogInfo()
                {
                    Id = 0,
                    AuditId = null,
                    BookingId = null,
                    QuotationId = null,
                    UserId = request.UserId,
                    StatusId = (int)QuotationStatus.CustomerValidated,
                    LogInformation = JsonConvert.SerializeObject(request)
                });

                if (request.Service == (int)(Service.InspectionId))
                {
                    var bookingIdList = await IsQuotationExists(request.Service, new List<int>() { request.BookingId }, 0);

                    //inspection or audit quotation exists method will return 
                    if (bookingIdList.Any())
                    {
                        return new EaqfGetSuccessResponse()
                        {
                            statusCode = HttpStatusCode.OK,
                            data = new SaveQuotationEaqfResponse()
                            {
                                InspectionId = request.BookingId,
                                QuotationId = bookingIdList.FirstOrDefault()
                            }
                        };
                    }

                    // Get booking Info by inspection id
                    var bookingDetail = await _insprepo.GetInspectionBookingDetails(request.BookingId);

                    if (bookingDetail != null)
                    {
                        //get the factory address list
                        var factoryAddress = await _supplierRepository.GetSupplierHeadOfficeAddress(bookingDetail.FactoryId.GetValueOrDefault());
                        var currencyData = await _referenceManager.GetCurrencyData(request.CurrencyCode);

                        if (currencyData == null)
                        {
                            return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Currency data not found" });
                        }

                        var customerData = await _referenceManager.GetCustomerData(bookingDetail.CustomerId);

                        if (customerData == null)
                        {
                            return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer is not exist for this booking" });
                        }

                        var factoryCountryId = factoryAddress != null ? factoryAddress.countryId : 0;

                        if (!customerData.ItUserMasters.Any(x => x.Id == request.UserId))
                        {
                            return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "User id is not mapped with this Booking customer" });
                        }

                        // set factory country in customer business country list if not added already
                        if (!customerData.CuCustomerBusinessCountries.Any(x => x.BusinessCountryId == factoryCountryId))
                        {
                            customerData.CuCustomerBusinessCountries.
                                Add(new CuCustomerBusinessCountry() { BusinessCountryId = factoryCountryId });
                            // save customer business country
                            _hrReposiotry.Save(customerData);
                        }

                        SaveQuotationRequest quotationRequest = new SaveQuotationRequest();
                        quotationRequest.Model = new QuotationDetails();
                        quotationRequest.Model.Country = new DTO.Location.Country() { Id = factoryCountryId };
                        quotationRequest.Model.Service = new DTO.References.Service() { Id = (int)(Service.InspectionId) };
                        quotationRequest.Model.BillingMethod = new BillingMethod() { Id = (int)BillingMethodEnum.ManDay };
                        quotationRequest.Model.BillingPaidBy = new BillPaidBy() { Id = (int)QuotationPaidBy.customer };
                        quotationRequest.Model.Currency = new DTO.References.Currency() { Id = currencyData.Id };
                        quotationRequest.Model.Customer = new DataSource() { Id = bookingDetail.CustomerId };
                        quotationRequest.Model.CustomerLegalName = bookingDetail.CustomerName;
                        quotationRequest.Model.Supplier = new DataSource() { Id = bookingDetail.SupplierId };
                        quotationRequest.Model.SupplierLegalName = bookingDetail.SupplierName;
                        quotationRequest.Model.Factory = new DataSource() { Id = bookingDetail.FactoryId.GetValueOrDefault() };
                        quotationRequest.Model.LegalFactoryName = bookingDetail.FactoryName;
                        quotationRequest.Model.FactoryAddress = factoryAddress != null ? factoryAddress.Address : "";
                        quotationRequest.Model.Office = new DataSource() { Id = bookingDetail.OfficeId.GetValueOrDefault() };

                        var orderFeeData = request.OrderDetails.FirstOrDefault(x => x.OrderType == "orderfee");
                        quotationRequest.Model.InspectionFees = (orderFeeData.Manday * orderFeeData.Amount);
                        quotationRequest.Model.EstimatedManday = orderFeeData.Manday;
                        quotationRequest.Model.Discount = request.OrderDetails.Where(x => x.OrderType == "discount").Select(x => x.Amount).Sum();
                        quotationRequest.Model.OtherCosts = request.OrderDetails.Where(x => x.OrderType == "otherfee").Select(y => y.Manday * y.Amount).Sum();
                        quotationRequest.Model.TotalCost = ((quotationRequest.Model.InspectionFees + quotationRequest.Model.OtherCosts) - quotationRequest.Model.Discount).GetValueOrDefault();
                        quotationRequest.Model.StatusId = QuotationStatus.CustomerValidated;
                        quotationRequest.Model.CreatedDate = DateTime.Now.ToString();
                        quotationRequest.Model.ApiInternalRemark = "Quotation created from EAQF System";
                        quotationRequest.Model.PaymentTerm = (int)INVInvoiceType.PreInvoice;
                        quotationRequest.Model.UserId = request.UserId;
                        quotationRequest.isCallFromEAQF = true;

                        var filterRequest = new FilterOrderRequest();
                        filterRequest.BookingIds = new List<int>() { request.BookingId };
                        var inspectionOrderList = await GetInspections(filterRequest);
                        if (inspectionOrderList.Result == OrderListResult.Success && inspectionOrderList.Data.FirstOrDefault() != null)
                        {
                            List<QuotationManday> listOfMandays = new List<QuotationManday>();
                            BookingDate bookingDate = await _insprepo.getInspBookingDateDetails(request.BookingId);
                            if (bookingDate != null)
                            {
                                var listDate = Enumerable.Range(0, 1 + bookingDate.ServiceDateTo.Subtract(bookingDate.ServiceDateFrom).Days)
                                             .Select(offset => bookingDate.ServiceDateFrom.AddDays(offset)).ToArray();
                                var manday = listDate.Select(x => QuotationMap.getQuotationManDay(x.Date, request.BookingId)).ToList();
                                listOfMandays.AddRange(manday);
                                if (listOfMandays.FirstOrDefault() != null)
                                {
                                    listOfMandays.FirstOrDefault().ManDay = orderFeeData.Manday;
                                }
                                if (inspectionOrderList.Data.FirstOrDefault() != null)
                                {
                                    inspectionOrderList.Data.FirstOrDefault().orderCost = new OrderCost()
                                    {
                                        InspFees = quotationRequest.Model.InspectionFees,
                                        UnitPrice = orderFeeData.Amount,
                                        NoOfManday = orderFeeData.Manday
                                    };
                                    inspectionOrderList.Data.FirstOrDefault().QuotationMandayList = listOfMandays;
                                    quotationRequest.Model.OrderList = inspectionOrderList.Data;
                                }
                            }
                            var response = await AddQuotation(quotationRequest);
                            if (response.Result == SaveQuotationResult.Success)
                            {
                                return new EaqfGetSuccessResponse()
                                {
                                    message = "Success",
                                    statusCode = HttpStatusCode.OK,
                                    data = new SaveQuotationEaqfResponse()
                                    {
                                        InspectionId = request.BookingId,
                                        QuotationId = response.Item.Id
                                    }
                                };
                            }
                            else
                            {
                                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Failed" });
                            }
                        }
                        else
                        {
                            return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Inspection order list not found" });
                        }
                    }
                    else
                    {
                        return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Inspection data not found." });
                    }
                }
                else if (request.Service == (int)(Service.AuditId))
                {

                    var bookingIdList = await IsQuotationExists(request.Service, new List<int>() { request.BookingId }, 0);

                    //inspection or audit quotation exists method will return 
                    if (bookingIdList.Any())
                    {
                        return new EaqfGetSuccessResponse()
                        {
                            statusCode = HttpStatusCode.OK,
                            data = new SaveQuotationEaqfResponse()
                            {
                                InspectionId = request.BookingId,
                                QuotationId = bookingIdList.FirstOrDefault()
                            }
                        };
                    }

                    // Get booking Info by inspection id
                    var bookingDetail = await _audprepo.GetAuditBookingDetails(request.BookingId);

                    if (bookingDetail != null)
                    {
                        //get the factory address list
                        var factoryAddress = await _supplierRepository.GetSupplierHeadOfficeAddress(bookingDetail.FactoryId.GetValueOrDefault());
                        var currencyData = await _referenceManager.GetCurrencyData(request.CurrencyCode);

                        if (currencyData == null)
                        {
                            return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Currency data not found" });
                        }

                        var customerData = await _referenceManager.GetCustomerData(bookingDetail.CustomerId);

                        if (customerData == null)
                        {
                            return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer is not exist for this booking" });
                        }

                        var factoryCountryId = factoryAddress != null ? factoryAddress.countryId : 0;

                        if (!customerData.ItUserMasters.Any(x => x.Id == request.UserId))
                        {
                            return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "User id is not mapped with this Booking customer" });
                        }

                        // set factory country in customer business country list if not added already
                        if (!customerData.CuCustomerBusinessCountries.Any(x => x.BusinessCountryId == factoryCountryId))
                        {
                            customerData.CuCustomerBusinessCountries.
                                Add(new CuCustomerBusinessCountry() { BusinessCountryId = factoryCountryId });
                            // save customer business country
                            _hrReposiotry.Save(customerData);
                        }

                        SaveQuotationRequest quotationRequest = new SaveQuotationRequest();
                        quotationRequest.Model = new QuotationDetails();
                        quotationRequest.Model.Country = new DTO.Location.Country() { Id = factoryCountryId };
                        quotationRequest.Model.Service = new DTO.References.Service() { Id = (int)(Service.AuditId) };
                        quotationRequest.Model.BillingMethod = new BillingMethod() { Id = (int)BillingMethodEnum.ManDay };
                        quotationRequest.Model.BillingPaidBy = new BillPaidBy() { Id = (int)QuotationPaidBy.customer };
                        quotationRequest.Model.Currency = new DTO.References.Currency() { Id = currencyData.Id };
                        quotationRequest.Model.Customer = new DataSource() { Id = bookingDetail.CustomerId };
                        quotationRequest.Model.CustomerLegalName = bookingDetail.CustomerName;
                        quotationRequest.Model.Supplier = new DataSource() { Id = bookingDetail.SupplierId };
                        quotationRequest.Model.SupplierLegalName = bookingDetail.SupplierName;
                        quotationRequest.Model.Factory = new DataSource() { Id = bookingDetail.FactoryId.GetValueOrDefault() };
                        quotationRequest.Model.LegalFactoryName = bookingDetail.FactoryName;
                        quotationRequest.Model.FactoryAddress = factoryAddress != null ? factoryAddress.Address : "";
                        quotationRequest.Model.Office = new DataSource() { Id = bookingDetail.OfficeId.GetValueOrDefault() };

                        var orderFeeData = request.OrderDetails.FirstOrDefault(x => x.OrderType == "orderfee");
                        quotationRequest.Model.InspectionFees = (orderFeeData.Manday * orderFeeData.Amount);
                        quotationRequest.Model.EstimatedManday = orderFeeData.Manday;
                        quotationRequest.Model.Discount = request.OrderDetails.Where(x => x.OrderType == "discount").Select(x => x.Amount).Sum();
                        quotationRequest.Model.OtherCosts = request.OrderDetails.Where(x => x.OrderType == "otherfee").Select(y => y.Manday * y.Amount).Sum();
                        quotationRequest.Model.TotalCost = ((quotationRequest.Model.InspectionFees + quotationRequest.Model.OtherCosts) - quotationRequest.Model.Discount).GetValueOrDefault();
                        quotationRequest.Model.StatusId = QuotationStatus.CustomerValidated;
                        quotationRequest.Model.CreatedDate = DateTime.Now.ToString();
                        quotationRequest.Model.ApiInternalRemark = "Quotation created from EAQF System";
                        quotationRequest.Model.PaymentTerm = (int)INVInvoiceType.PreInvoice;
                        quotationRequest.Model.UserId = request.UserId;
                        quotationRequest.isCallFromEAQF = true;

                        //var filterRequest = new FilterOrderRequest();
                        //filterRequest.BookingIds = new List<int>() { request.BookingId };
                        var inspectionOrderList = new QuotationOrderListResponse { Result = OrderListResult.Success };
                        inspectionOrderList.Data = new List<Order>() { new Order() { } };
                        //if (inspectionOrderList.Result == OrderListResult.Success && inspectionOrderList.Data.FirstOrDefault() != null)
                        //{
                        List<QuotationManday> listOfMandays = new List<QuotationManday>();
                        BookingDate bookingDate = await _audprepo.getAuditBookingDateDetails(request.BookingId);
                        if (bookingDate != null)
                        {
                            var listDate = Enumerable.Range(0, 1 + bookingDate.ServiceDateTo.Subtract(bookingDate.ServiceDateFrom).Days)
                                         .Select(offset => bookingDate.ServiceDateFrom.AddDays(offset)).ToArray();
                            var manday = listDate.Select(x => QuotationMap.getQuotationManDay(x.Date, request.BookingId)).ToList();
                            listOfMandays.AddRange(manday);
                            if (listOfMandays.FirstOrDefault() != null)
                            {
                                listOfMandays.FirstOrDefault().ManDay = orderFeeData.Manday;
                            }
                            inspectionOrderList.Data.FirstOrDefault().Id = request.BookingId;
                            inspectionOrderList.Data.FirstOrDefault().orderCost =
                                new OrderCost()
                                {

                                    InspFees = quotationRequest.Model.InspectionFees,
                                    UnitPrice = orderFeeData.Amount,
                                    NoOfManday = orderFeeData.Manday
                                };


                            inspectionOrderList.Data.FirstOrDefault().QuotationMandayList = listOfMandays;
                            quotationRequest.Model.OrderList = inspectionOrderList.Data;

                        }
                        var response = await AddQuotation(quotationRequest);
                        if (response.Result == SaveQuotationResult.Success)
                        {
                            return new EaqfGetSuccessResponse()
                            {
                                message = "Success",
                                statusCode = HttpStatusCode.OK,
                                data = new SaveQuotationEaqfResponse()
                                {
                                    InspectionId = request.BookingId,
                                    QuotationId = response.Item.Id
                                }
                            };
                        }
                        else
                        {
                            return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Failed" });
                        }
                    }
                    else
                    {
                        return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Inspection data not found." });
                    }
                }
                else
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Inspection Id is not valid." });
                }
            }
            else
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Inspection Id is not valid." });
            }
        }

        public EaqfErrorResponse BuildCommonEaqfErrorResponse(HttpStatusCode statusCode, string message, List<string> errors)
        {
            return new EaqfErrorResponse()
            {
                errors = errors,
                statusCode = statusCode,
                message = message
            };
        }

        public async Task<FactoryBookingInfoResponse> FactoryBookingInfo(FactoryBookingInfoRequest request)
        {
            if (request == null || request.FactoryId <= 0 || !request.BookingIds.Any())
                return new FactoryBookingInfoResponse() { Result = FactoryBookingInfoResult.RequestNotCorrectFormat };

            var inspServiceDatelst = new List<DateTime>();
            var inspServiceDates = await _insprepo.getListInspBookingDateDetails(request.BookingIds);
            foreach (var inspServiceDate in inspServiceDates)
            {
                for (var dt = inspServiceDate.ServiceDateFrom; dt <= inspServiceDate.ServiceDateTo; dt = dt.AddDays(1))
                {
                    if (!inspServiceDatelst.Contains(dt))
                        inspServiceDatelst.Add(dt);
                }
            }

            var inspectionQuery = _insprepo.GetAllInspectionsQuery();
            int i = 0;
            foreach (var serviceDatelst in inspServiceDatelst)
            {
                if (i == 0)
                {
                    inspectionQuery = inspectionQuery.Where(y => !((y.ServiceDateFrom > serviceDatelst) || (y.ServiceDateTo < serviceDatelst)));
                }
                else
                {
                    var inspectionDateQuery = _insprepo.GetAllInspectionsQuery();
                    inspectionDateQuery = inspectionDateQuery.Where(y => !((y.ServiceDateFrom > serviceDatelst) || (y.ServiceDateTo < serviceDatelst)));

                    inspectionQuery = inspectionQuery.Union(inspectionDateQuery);
                }
                i++;
            }
            if (request.FactoryId > 0)
                inspectionQuery = inspectionQuery.Where(x => x.FactoryId == request.FactoryId);

            var bookingIds = inspectionQuery.Select(x => x.Id);

            // get the booking list by booking ids
            var bookinglst = await _insprepo.GetBookingData(bookingIds);
            if (bookinglst == null || !bookinglst.Any())
                return new FactoryBookingInfoResponse() { Result = FactoryBookingInfoResult.NotFound };

            //get the service type list by booking ids
            var serviceTypelst = await _insprepo.GetServiceTypeByBookingQuery(bookingIds);

            var productlst = await _insprepo.GetScheduleProductListByBookingQuery(bookingIds);
            var containerlst = await _insprepo.GetScheduleContainerListByBookingQuery(bookingIds);

            //get quotation data by booking number list
            var quotationlst = await _invoiceRepository.GetQuotationDataByBookingIdsList(bookingIds.ToList());

            var invoicelst = new List<InvoiceDetail>();

            //fetch Carrefour customer IDs from appsettings
            int carrefourCustomerId = 0;
            var carrefourCustomerIds = _Configuration["CustomerCarrefour"].Split(',').Where(str => int.TryParse(str, out carrefourCustomerId)).Select(str => carrefourCustomerId).ToList();

            if (bookinglst.Any(x => carrefourCustomerIds.Contains(x.CustomerId)))
            {
                var bookingRuleInfoList = await GetBookingAndRuleList(bookingIds, 0, 0);
                if (bookingRuleInfoList != null)
                {
                    var ruleConfigDictionary = new Dictionary<int, CustomerPriceCardRepo>();
                    foreach (var bookingOrder in bookingRuleInfoList.invoiceBookings)
                    {
                        var ruleConfigs = await _invoiceManager.GetRuleConfigListbyBookingFilter(bookingOrder, bookingRuleInfoList.customerPriceCards);
                        var ruleConfig = await _invoiceManager.GetRuleConfigData(bookingOrder, ruleConfigs);
                        if (ruleConfig != null && ruleConfig.InvoiceRequestType != null && ruleConfig.InvoiceInspFeeFrom == (int)InvoiceFeesFrom.Carrefour)
                        {
                            bookingOrder.RuleConfig = ruleConfig;
                            //  Add rule config to dictionary for process later
                            if (!ruleConfigDictionary.ContainsKey(ruleConfig.Id))
                            {
                                ruleConfigDictionary.Add(ruleConfig.Id, ruleConfig);
                            }
                            bookingOrder.IsInvalid = false;
                        }
                        else
                        {
                            // Error: Cannot find rule for this order
                            bookingOrder.IsInvalid = true;
                        }
                    }

                    var bookingList = bookingRuleInfoList.invoiceBookings.Where(o => !o.IsInvalid).ToList();
                    if (bookingList.Any())
                    {
                        // Billing Method per rule config
                        foreach (var ruleConfig in ruleConfigDictionary.Values)
                        {
                            var invoiceDetails = new List<InvoiceDetail>();
                            var quotationBooking = new List<QuotationBooking>();
                            invoiceDetails = await _invoiceManager.GetInvoiceListbyPriceCalculations(bookingList, invoiceDetails, ruleConfig, bookingRuleInfoList.InvoiceGenerateRequest, quotationBooking, true);
                            invoicelst.AddRange(invoiceDetails);
                        }
                    }
                }
            }

            var factoryBookingInfolst = bookinglst.OrderBy(x => x.ServiceFrom).ThenBy(x => x.ServiceTo).Select(x => QuotationMap.FactoryBookingInfo(x, serviceTypelst, productlst, containerlst, quotationlst, invoicelst));

            if (!factoryBookingInfolst.Any())
                return new FactoryBookingInfoResponse() { Result = FactoryBookingInfoResult.NotFound };

            return new FactoryBookingInfoResponse()
            {
                Result = FactoryBookingInfoResult.Success,
                FactoryBookingInfolst = factoryBookingInfolst
            };
        }

        private async Task UpdateQuotationSuggestedManday(IEnumerable<FactoryBookingInfo> factoryBookingInfoList)
        {
            var bookingIds = factoryBookingInfoList.Select(x => x.BookingId).ToList();
            var quotationInspList = await _QuotationRepository.GetListAsync<QuQuotationInsp>(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled);
            if (quotationInspList.Any())
            {
                foreach (var quotationInsp in quotationInspList)
                {
                    var factoryBookingInfo = factoryBookingInfoList.FirstOrDefault(x => x.BookingId == quotationInsp.IdBooking);
                    if (factoryBookingInfo != null)
                        quotationInsp.SuggestedManday = factoryBookingInfo.SuggestedManday;
                }
                _QuotationRepository.SaveList(quotationInspList, true);
            }
        }
    }
}