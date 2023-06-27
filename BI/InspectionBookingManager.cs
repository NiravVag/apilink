using AutoMapper;
using BI.Maps;
using BI.Maps.APP;
using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.BookingRuleContact;
using DTO.CancelBooking;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.DataAccess;
using DTO.DynamicFields;
using DTO.Eaqf;
using DTO.EntPages;
using DTO.EventBookingLog;
using DTO.File;
using DTO.FullBridge;
using DTO.HumanResource;
using DTO.Inspection;
using DTO.Invoice;
using DTO.MasterConfig;
using DTO.MobileApp;
using DTO.OfficeLocation;
using DTO.PurchaseOrder;
using DTO.Quotation;
using DTO.RepoRequest.Enum;
using DTO.Report;
using DTO.SamplingQuantity;
using DTO.Supplier;
using DTO.User;
using DTO.WorkLoadMatrix;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static BI.TenantProvider;
using static DTO.Common.Static_Data_Common;
using static DTO.Inspection.BookingSummaryResponse;

namespace BI
{
    public class InspectionBookingManager : ApiCommonData, IInspectionBookingManager
    {
        private readonly IInspectionBookingRepository _repo = null;
        private readonly IReferenceRepository _referenceRepo = null;
        private readonly ICustomerManager _customerManager = null;
        private readonly IPurchaseOrderRepository _poRepo = null;
        private readonly ICustomerContactManager _customerContactRepository = null;
        private readonly ICustomerContactRepository _customerContactRepo = null;
        private readonly IReferenceManager _referencemanager = null;
        private readonly ISupplierManager _suppliermanager = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly IFileManager _fileManager = null;
        private readonly IMapper _mapper;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ICustomerProductRepository _productRepo = null;
        private readonly ICombineOrdersManager _combineOrdermanager = null;
        private readonly ICancelBookingRepository _cancelBookingRepository = null;
        private readonly IInspBookingRuleContactManager _bookingManager = null;
        private readonly IInspectionPickingManager _pickingManager = null;
        private readonly IEmailsManager _mailManager = null;
        private readonly ILogger _logger = null;
        private readonly IEventBookingLogManager _eventBookingLog = null;
        private readonly IHumanResourceRepository _humanResourceRepository = null;
        private readonly IUserRightsManager _userManager = null;
        private readonly IInspBookingRuleContactManager _inspBookingRuleContactManager = null;
        private readonly ICustomerCheckPointRepository _customerCheckPointRepository = null;
        private IDictionary<BookingStatus, Func<int, bool, SaveInsepectionRequest, Task<SetInspNotifyResponse>>> _dictStatuses = null;
        private bool isEdit;
        private readonly IScheduleRepository _schRepo = null;
        private readonly IQuotationRepository _quotationRepository = null;
        private readonly IHelper _helper = null;
        private readonly ICustomerRepository _customerRepo = null;
        private readonly ISupplierRepository _supplierRepo = null;
        private readonly FBSettings _fbSettings = null;
        private readonly ICustomerServiceConfigManager _customerServiceManager = null;
        private readonly IFullBridgeManager _fbManager = null;
        private readonly IReportRepository _reportRepository = null;
        private readonly IEventBookingLogRepository _eventLogRepo = null;
        private readonly IUserRepository _userRepo = null;
        private readonly IDynamicFieldManager _dynamicFieldManager = null;
        private readonly IFullBridgeRepository _fbRepo = null;
        private readonly IHostingEnvironment _env;

        private readonly IInspectionCustomerDecisionManager _customerDecisionManager = null;
        private readonly ILocationManager _locationManager = null;
        private readonly ICustomerDepartmentManager _cusDeptManager = null;
        private readonly ICustomerCollectionManager _cusCollectionManager = null;
        private readonly ICustomerBuyerManager _cusBuyerManager = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IInspectionPickingRepository _pickingRepository = null;
        private readonly IOfficeLocationRepository _officeLocationRepository = null;
        private readonly ISharedInspectionManager _sharedInspection = null;
        private readonly BookingMap _bookingmap = null;
        private readonly QuotationMap _quotationmap = null;
        private readonly ReferenceMap _refmap = null;
        private readonly CustomerMap _customermap = null;
        private readonly InspSummaryMobileMap _inspsummobilemap = null;
        private readonly InspReportMobileMap _inspreportmobilemap = null;
        private readonly IScheduleManager _scheduleManager = null;
        private readonly IWorkLoadMatrixRepository _workLoadMatrixRepository = null;
        private readonly IUserConfigRepository _userConfigRepo = null;
        private readonly ISupplierManager _supplierManager = null;
        private readonly IHumanResourceManager _humanResourceManager = null;
        private readonly IProductManagementRepository _productManagementRepository = null;
        private readonly IUserAccountRepository _userAccountRepository = null;
        private readonly IManualInvoiceRepository _manualInvoiceRepo = null;

        public InspectionBookingManager
            (IInspectionBookingRepository repo,
            IFileManager fileManager,
            IMapper mapper,
            ICustomerManager customerManager,
            IReferenceManager referencemanager, ISupplierManager suppliermanager,
            ICustomerContactManager customercontact, IOfficeLocationManager office,
            ICustomerRepository customerRepo,
            ISupplierRepository supplierRepo,
            IPurchaseOrderRepository poRepo,
            IReferenceRepository referenceRepo,
            ICustomerProductRepository productRepo,
            ICombineOrdersManager combineOrdermanager,
            IAPIUserContext applicationContext,
            ICancelBookingRepository cancelBookingRepository,
            IInspBookingRuleContactManager bookingManager,
            IInspectionPickingManager pickingManager,
            IEmailsManager mailManager,
            ILogger<InspectionBookingManager> logger,
            IEventBookingLogManager eventBookingLog,
            IHumanResourceRepository humanResourceRepository,
            IUserRightsManager userManager,
            ICustomerContactRepository customerContactRepo,
            IInspBookingRuleContactManager inspBookingRuleContactManager,
            ICustomerCheckPointRepository customerCheckPointRepository,
            IScheduleRepository scheuleRepository,
            IQuotationRepository quotationRepository,
            IHelper helper,
            IOptions<FBSettings> fbSettings,
            ICustomerServiceConfigManager customerServiceManager,
            IFullBridgeManager fbManager,
            IReportRepository reportRepository,
            IEventBookingLogRepository eventLogRepo,
            IUserRepository userRepo,
            IDynamicFieldManager dynamicFieldManager,
            IFullBridgeRepository fbRepo,
             IHostingEnvironment env,
            IInspectionCustomerDecisionManager customerDecisionManager,
            ILocationManager locationManager,
            ICustomerDepartmentManager cusDeptManager,
            ICustomerCollectionManager cusCollectionManager,
            ICustomerBuyerManager cusBuyerManager,
            IInspectionPickingRepository pickingRepository,
            IOfficeLocationRepository officeLocationRepository,
            ISharedInspectionManager sharedInspection,
            ITenantProvider filterService,
            IScheduleManager scheduleManager,
            IWorkLoadMatrixRepository workLoadMatrixRepository,
            IUserConfigRepository userConfigRepo,
            ISupplierManager supplierManager,
            ILocationRepository locationRepository,
            IHumanResourceManager humanResourceManager,
            IUserAccountRepository userAccountRepository,
            IProductManagementRepository productManagementRepository,
            IManualInvoiceRepository manualInvoiceRepo
            )
        {
            _repo = repo;
            _fileManager = fileManager;
            _referencemanager = referencemanager;
            _suppliermanager = suppliermanager;
            _customerManager = customerManager;
            _customerContactRepository = customercontact;
            _office = office;
            _mapper = mapper;
            _ApplicationContext = applicationContext;
            _poRepo = poRepo;
            _referenceRepo = referenceRepo;
            _productRepo = productRepo;
            _combineOrdermanager = combineOrdermanager;
            _cancelBookingRepository = cancelBookingRepository;
            _bookingManager = bookingManager;
            _pickingManager = pickingManager;
            _mailManager = mailManager;
            _logger = logger;
            _eventBookingLog = eventBookingLog;
            _humanResourceRepository = humanResourceRepository;
            _userManager = userManager;
            _customerContactRepo = customerContactRepo;
            _inspBookingRuleContactManager = inspBookingRuleContactManager;
            _customerCheckPointRepository = customerCheckPointRepository;
            _schRepo = scheuleRepository;
            _helper = helper;
            _customerRepo = customerRepo;
            _supplierRepo = supplierRepo;
            _fbSettings = fbSettings.Value;
            _quotationRepository = quotationRepository;
            _customerServiceManager = customerServiceManager;
            _fbManager = fbManager;
            _eventLogRepo = eventLogRepo;
            _reportRepository = reportRepository;
            _userRepo = userRepo;
            _dynamicFieldManager = dynamicFieldManager;
            _fbRepo = fbRepo;
            _env = env;
            _customerDecisionManager = customerDecisionManager;
            _locationManager = locationManager;
            _cusDeptManager = cusDeptManager;
            _cusCollectionManager = cusCollectionManager;
            _cusBuyerManager = cusBuyerManager;
            _pickingRepository = pickingRepository;
            _officeLocationRepository = officeLocationRepository;
            _sharedInspection = sharedInspection;
            _bookingmap = new BookingMap();
            _quotationmap = new QuotationMap();
            _refmap = new ReferenceMap();
            _customermap = new CustomerMap();
            _inspsummobilemap = new InspSummaryMobileMap();
            _inspreportmobilemap = new InspReportMobileMap();
            _scheduleManager = scheduleManager;
            _workLoadMatrixRepository = workLoadMatrixRepository;
            _userConfigRepo = userConfigRepo;
            _filterService = filterService;
            _supplierManager = supplierManager;

            _humanResourceManager = humanResourceManager;
            _userAccountRepository = userAccountRepository;
            _dictStatuses = new Dictionary<BookingStatus, Func<int, bool, SaveInsepectionRequest, Task<SetInspNotifyResponse>>>() {
                                    { BookingStatus.Received, ToRequestBooking},
                                    { BookingStatus.Verified, ToVerifyBooking},
                                    { BookingStatus.Confirmed, ToConfirmBooking },
                                    { BookingStatus.AllocateQC, ToScheduleBooking },
                                    { BookingStatus.Cancel, ToCancelBooking },
                                    { BookingStatus.Hold, ToHoldBooking },
                                    { BookingStatus.Rescheduled, ToRescheduleBooking }
                               };
            _productManagementRepository = productManagementRepository;
            _manualInvoiceRepo = manualInvoiceRepo;
        }

        /// <summary>
        /// Check the combine data is available for the booking
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool CheckCombineDataExists(InspTransaction entity)
        {
            bool isCombineDone = false;
            if (entity.InspProductTransactions != null && entity.InspProductTransactions.Any())
            {
                //take combined product where combine product is not null and combineaqlquantity greater than zero
                var combineProducts = entity.InspProductTransactions.Where(x => x.CombineProductId != null && x.CombineAqlQuantity > 0 && x.Active.HasValue && x.Active.Value).ToList();
                //if combine data is there
                if (combineProducts != null && combineProducts.Any())
                    return !isCombineDone;

            }
            return isCombineDone;
        }


        /// <summary>
        /// To save and update inspection booking details 
        /// </summary>
        /// <param name="request">InspectionBookingDetails</param>
        /// <returns>saved item id</returns>
        public async Task<SaveInspectionBookingResponse> SaveInspectionBooking(SaveInsepectionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotFound };
                }

                var response = new SaveInspectionBookingResponse();
                var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

                // Add  Booking Log information - in the Event booking log table.

                await _eventBookingLog.SaveLogInformation(new EventBookingLogInfo()
                {
                    Id = 0,
                    AuditId = 0,
                    BookingId = request.Id,
                    StatusId = request.StatusId,
                    LogInformation = JsonConvert.SerializeObject(request),
                    UserId = userId
                });

                if (request.Id == 0) // add new booking 
                {
                    InspTransaction entity = _mapper.Map<InspTransaction>(request);
                    entity.EntityId = _filterService.GetCompanyId();

                    if (entity == null)
                    {
                        return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotFound };
                    }

                    //assign the default office id based on the entity
                    if (_ApplicationContext.UserType == UserTypeEnum.Customer
                            && request.StatusId == (int)BookingStatus.Received && entity.OfficeId == null)
                    {
                        var masterConfings = await GetMasterConfiguration();
                        var masterOfficeId = masterConfings.Where(x => x.Type == (int)EntityConfigMaster.DefaultEntityBasedOffice).Select(x => x.Value).FirstOrDefault();
                        entity.OfficeId = Convert.ToInt32(masterOfficeId);
                    }

                    if (request.InspectionProductList == null)
                    {
                        return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotFound };
                    }

                    // if any total booking product quantity is zero
                    if (request.InspectionProductList.Any(x => x.TotalBookingQuantity == 0))
                    {
                        return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingQuantityZero };
                    }
                    if ((int)UserTypeEnum.InternalUser != request.UserType)// FOR EXTERNAL USER
                    {

                        InspBookingRuleResponse _bookingRule = await _inspBookingRuleContactManager.GetInspBookingRules(request.CustomerId, request.FactoryId);

                        if (_bookingRule != null && _bookingRule?.Result == BookingRuleResult.Success)
                        {
                            InspBookingRules _insBookRule = _bookingRule.BookingRuleList;
                            if (_insBookRule != null && !request.IsEaqf)
                            {
                                if (!IsServiceDateExistsWithLeadDays(_insBookRule, request.ServiceDateFrom.ToDateTime().Date))
                                {
                                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.BookingDatesIncorrect };
                                }
                                if (!IsServiceDateExistsWithLeadDays(_insBookRule, request.ServiceDateTo.ToDateTime().Date))
                                {
                                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.BookingDatesIncorrect };
                                }
                            }
                        }

                    }

                    entity.CreatedBy = userId;
                    //Add the status log in internal comments for users visibility
                    entity.InternalComments = request.InternalComments + (BookingStatus.Received) + " on " + DateTime.Now.ToString(StandardDateFormat) + "\n";

                    UpdateProductCategoryDataToInspection(request.InspectionProductList.FirstOrDefault(), entity);

                    await MapProductToPurchaseOrder(request);

                    await AddSupplierOrFactoryToPurchaseOrderOnBooking(request);

                    await AddInspectionPOProductList(request, entity);

                    AddInspectionCustomerContactList(request, entity);

                    AddInspectionFactoryContactList(request, entity);

                    AddInspectionSupplierContactList(request, entity);

                    AddInspectionServiceTypeList(request, entity);

                    AddInspectionCustomerBrandList(request, entity);

                    AddInspectionCustomerBuyerList(request, entity);

                    AddInspectionCustomerDepartmentList(request, entity);

                    AddInspectionStatusLog(request, entity);

                    response.IsTechincalDocumentsAddedOrRemoved = true;

                    AddFiles(request.InspectionFileAttachmentList, entity, request.UserId);

                    AddInspectionDFTransactionList(request.InspectionDFTransactions, entity, false);

                    AddInspectionCustomerMerchandiserList(request, entity);

                    //add inspection shipment types
                    AddInspectionShipmentTypes(request, entity);

                    //add the cs list
                    AddCsList(request, entity);

                    var Id = await _repo.AddInspectionBooking(entity);

                    if (request.DraftInspectionId != null)
                    {
                        var inspTransactionDraft = await _repo.GetInspectionDraftById(request.DraftInspectionId.GetValueOrDefault());

                        inspTransactionDraft.InspectionId = entity.Id;

                        _repo.EditEntity(entity);

                        await _repo.Save();
                    }

                    if (Id > 0)
                    {
                        response.Id = entity.Id;
                    }

                    if (entity.Id == 0)
                    {
                        return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotSaved };
                    }

                    response.Result = SaveInspectionBookingResult.Success;

                    //map inspection service type to customer
                    await MapInspectionServiceTypeToCustomer(request, response);

                    return response;
                }

                else // update booking details
                {
                    var entity = await _repo.GetInspectionByID(request.Id);
                    //get the current status
                    var dbCurrentStatus = entity.StatusId;

                    if (entity == null)
                        return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotFound };

                    // set internal comments
                    setInternalComments(request, entity);

                    //Update only the service dates and comments during Booking Confirmation
                    if (request.StatusId == (int)BookingStatus.Confirmed && (entity.StatusId == (int)BookingStatus.Verified ||
                        entity.StatusId == (int)BookingStatus.Hold || entity.StatusId == (int)BookingStatus.Rescheduled))
                    {

                        // only confirmed role user can update this 
                        if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionConfirmed))
                        {
                            // update supplier and factory contacts while confirm the booking
                            if (request.InspectionFactoryContactList != null)
                            {
                                this.UpdateInspectionFactoryContactList(request, entity);
                            }
                            if (request.InspectionSupplierContactList != null)
                            {
                                this.UpdateInspectionSupplierContactList(request, entity);
                            }
                        }

                        var entityBooking = new InspTransaction()
                        {
                            ServiceDateFrom = entity.ServiceDateFrom,
                            ServiceDateTo = entity.ServiceDateTo
                        };

                        await this.UpdateConfirmInspectionBooking(entity, request, userId, response);


                        var bookingInfo = new BookingDateInfo()
                        {
                            BookingId = request.Id,
                            ServiceFromDate = request.ServiceDateFrom,
                            ServiceToDate = request.ServiceDateTo
                        };

                        //update quotation service date if we change the booking service date 
                        await UpdateQuotationServiceDate(bookingInfo, entityBooking);

                        return response;

                    }

                    //Update only the status and hold reason.
                    if (request.StatusId == (int)BookingStatus.Hold && (entity.StatusId == (int)BookingStatus.Verified || entity.StatusId == (int)BookingStatus.Confirmed ||

                        entity.StatusId == (int)BookingStatus.AllocateQC || entity.StatusId == (int)BookingStatus.Rescheduled))
                    {
                        await this.UpdateHoldInspectionBooking(entity, request, userId, response);
                        return response;
                    }

                    this.UpdateInspectionBooking(entity, request, userId);

                    response.IsTechincalDocumentsAddedOrRemoved = CheckTechnicalDocListUpdated(request, entity);

                    if (!response.IsTechincalDocumentsAddedOrRemoved)
                    {
                        response.IsTechnicalDoucmentSync = entity.InspTranFileAttachments.Any(x => x.Active && request.InspectionFileAttachmentList.Any(y => y.Id == x.Id && y.IsReportSendToFB != x.IsReportSendToFb));
                    }
                    else
                    {
                        response.IsTechnicalDoucmentSync = response.IsTechincalDocumentsAddedOrRemoved;
                    }

                    if (request.InspectionProductList != null && request.InspectionProductList.Any())
                    {
                        await MapProductToPurchaseOrder(request);

                        await AddSupplierOrFactoryToPurchaseOrderOnBooking(request);

                        response.isCombineOrderDataChanged = await UpdateInspectionPODetails(request, entity, entity.StatusId);

                        //check combine information exists for booking if status is verified
                        if (dbCurrentStatus == (int)BookingStatus.Received && request.StatusId == (int)BookingStatus.Verified
                                                        && request.IsCombineRequired && !CheckCombineDataExists(entity))
                            return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.CombineInformationNotFound };


                    }
                    if (request.InspectionCustomerContactList != null)
                    {
                        this.UpdateInspectionCustomerContactList(request, entity);
                    }
                    if (request.InspectionFactoryContactList != null)
                    {
                        this.UpdateInspectionFactoryContactList(request, entity);
                    }
                    if (request.InspectionSupplierContactList != null)
                    {
                        this.UpdateInspectionSupplierContactList(request, entity);
                    }
                    if (request.InspectionServiceTypeList != null)
                    {
                        this.UpdateInspectionServiceTypeList(request, entity);
                    }
                    if (request.InspectionCustomerBuyerList != null)
                    {
                        this.UpdateInspectionCustomerBuyerList(request, entity);
                    }
                    if (request.InspectionCustomerBrandList != null)
                    {
                        this.UpdateInspectionCustomerBrandList(request, entity);
                    }
                    if (request.InspectionCustomerDepartmentList != null)
                    {
                        this.UpdateInspectionCustomerDepartmentList(request, entity);
                    }
                    if (request.InspectionCustomerMerchandiserList != null)
                    {
                        this.UpdateInspectionCustomerMerchandiserList(request, entity);
                    }

                    if (request.ShipmentTypeIds != null && request.ShipmentTypeIds.Any())
                    {
                        this.UpdateInspectionShipmentTypes(request, entity);
                    }

                    //update the cs list
                    if (request.CsList != null && request.CsList.Any())
                    {
                        UpdateInspectionCs(request, entity);
                    }


                    if (request.InspectionDFTransactions != null && request.InspectionDFTransactions.Any())
                        UpdateInspectionDfTransaction(request, entity);

                    AddInspectionStatusLog(request, entity);

                    AddAttachments(request, entity);

                    await _repo.EditInspectionBooking(entity);

                    await UpdateQuotationOffice(request.Id, request.OfficeId.GetValueOrDefault());

                    response.Id = entity.Id;
                    response.IsMissionCreated = entity.FbMissionId.HasValue;
                    response.Result = SaveInspectionBookingResult.Success;

                    //map the inspeciton service type to customer
                    await MapInspectionServiceTypeToCustomer(request, response);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check techical document list is updated or not
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool CheckTechnicalDocListUpdated(SaveInsepectionRequest request, InspTransaction entity)
        {
            var isTechincalDocumentsAddedOrRemoved = false;

            //take new file ids
            var fileIds = request.InspectionFileAttachmentList.Where(x => x.Id > 0).Select(x => x.Id);

            //take existing db fileids but removed in request
            var existingFilesNotAvailableinEntity = entity.InspTranFileAttachments.
                                                        Where(x => !fileIds.Contains(x.Id) && x.Active).ToList();

            var newFiles = request.InspectionFileAttachmentList.Where(x => x.Id == 0);

            if (existingFilesNotAvailableinEntity.Any() || (newFiles != null && newFiles.Any()))
            {
                isTechincalDocumentsAddedOrRemoved = true;
            }

            return isTechincalDocumentsAddedOrRemoved;
        }

        /// <summary>
        /// save or update the inspection draft booking
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveDraftInsepectionResponse> SaveDraftInspectionBooking(DraftInspectionRequest request)
        {
            SaveDraftInsepectionResponse response = new SaveDraftInsepectionResponse() { Result = SaveDraftInspectionResult.Failure };

            //add inspection draft booking data
            if (request.Id == 0)
            {
                //map the save inspection draft booking
                var inspTransactionDraft = _bookingmap.MapSaveInspectionDraftBooking(request);
                inspTransactionDraft.Active = true;
                inspTransactionDraft.CreatedBy = _ApplicationContext.UserId;
                inspTransactionDraft.CreatedOn = DateTime.Now;
                inspTransactionDraft.EntityId = _filterService.GetCompanyId();

                _repo.AddEntity(inspTransactionDraft);

                await _repo.Save();

                if (inspTransactionDraft.Id > 0)
                {
                    response.Result = SaveDraftInspectionResult.Success;
                    response.DraftInspectionId = inspTransactionDraft.Id;
                }
            }
            else if (request.Id > 0)
            {
                var inspTransactionDraft = await _repo.GetInspectionDraftById(request.Id);
                //map the update inspection draft booking
                _bookingmap.MapUpdateInspectionDraftBooking(request, inspTransactionDraft);

                inspTransactionDraft.UpdatedBy = _ApplicationContext.UserId;
                inspTransactionDraft.UpdatedOn = DateTime.Now;

                _repo.EditEntity(inspTransactionDraft);

                await _repo.Save();

                if (inspTransactionDraft.Id > 0)
                {
                    response.Result = SaveDraftInspectionResult.Success;
                    response.DraftInspectionId = inspTransactionDraft.Id;
                }

            }

            return response;

        }

        /// <summary>
        /// Get the inspection draft data by userid
        /// </summary>
        /// <returns></returns>
        public async Task<DraftInspectionResponse> GetInspectionDraftByUserId()
        {

            var response = new DraftInspectionResponse() { Result = DraftInspectionResult.NotFound };

            //get the inspection draft list by user id
            var inspectionDraftDataList = await _repo.GetInspectionDraftByUserId(_ApplicationContext.UserId);

            if (inspectionDraftDataList.Any())
            {
                //map the draft inspeciton list
                var draftInspectionList = _bookingmap.MapInspectionDraftList(inspectionDraftDataList);
                response.InspectionData = draftInspectionList;
                response.Result = DraftInspectionResult.Success;
            }

            return response;
        }

        /// <summary>
        /// remove the inspection draft(update the draft data to be inactive)
        /// </summary>
        /// <param name="draftInspectionId"></param>
        /// <returns></returns>
        public async Task<DeleteDraftInspectionResponse> RemoveInspectionDraft(int draftInspectionId)
        {
            var response = new DeleteDraftInspectionResponse() { Result = DeleteDraftInspectionResult.NotFound };

            //get the inspection draft data by id
            var draftInspectionData = await _repo.GetInspectionDraftById(draftInspectionId);

            //update the inspection draft to be inactive
            if (draftInspectionData != null)
            {
                draftInspectionData.Active = false;
                draftInspectionData.DeletedBy = _ApplicationContext.UserId;
                draftInspectionData.DeletedOn = DateTime.Now;
                _repo.EditEntity(draftInspectionData);
                await _repo.Save();
                response.Result = DeleteDraftInspectionResult.DeleteSuccess;
            }
            else
                response.Result = DeleteDraftInspectionResult.NotFound;

            return response;
        }

        /// <summary>
        /// Add the cs ist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="inspEntity"></param>
        private void AddCsList(SaveInsepectionRequest request, InspTransaction inspEntity)
        {
            if (request.CsList != null && request.CsList.Any())
            {
                foreach (var cs in request.CsList)
                {
                    var csData = new InspTranC()
                    {
                        CsId = cs,
                        Active = true,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    inspEntity.InspTranCS.Add(csData);
                }
            }
        }

        /// <summary>
        /// Map the inspection service type to customer if it is not mapped already
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<bool> MapInspectionServiceTypeToCustomer(SaveInsepectionRequest request, SaveInspectionBookingResponse response)
        {
            if (response.Result == SaveInspectionBookingResult.Success)
            {
                if (request.ServiceTypeId > 0)
                {
                    if (!await _customerServiceManager.CheckServiceTypeMappedWithCustomer(request.CustomerId, request.ServiceTypeId))
                    {
                        var cuServiceType = MapCustomerServiceType(request);
                        _repo.AddEntity(cuServiceType);
                        await _repo.Save();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Map the customer service type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private CuServiceType MapCustomerServiceType(SaveInsepectionRequest request)
        {
            return new CuServiceType
            {
                ServiceId = (int)Service.InspectionId,
                ServiceTypeId = request.ServiceTypeId,
                CustomerId = request.CustomerId,
                Active = true,
                CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                EntityId = _filterService.GetCompanyId(),
            };
        }

        /// <summary>
        /// Check service date exists with holidays and leaddays
        /// </summary>
        /// <param name="InsBookRule"></param>
        /// <param name="ServiceDate"></param>
        private bool IsServiceDateExistsWithLeadDays(InspBookingRules InsBookRule, DateTime ServiceDate)
        {
            bool _isServiceDate = true;
            int _leadDays = 0;
            var _tdyDate = DateTime.Now.Date;
            _leadDays = InsBookRule.LeadDays;

            if (InsBookRule.Holidays != null && InsBookRule.Holidays.Count() > 0)
            {
                var _holidaysDate = InsBookRule.Holidays.Select(x => x.ToDateTime().Date);
                if (_holidaysDate.Contains(ServiceDate))//check service date on holidays
                {
                    _isServiceDate = false;
                }
                else                //extends leaddays with holidaya and check service date
                {
                    int count = 0;
                    var _leadDate = _tdyDate.AddDays(_leadDays);
                    for (var i = DateTime.Now.Date; i < _leadDate.Date; i = i.AddDays(1))
                    {
                        if (_holidaysDate.Contains(i))
                        {
                            count++;
                        }
                    }
                    if (ServiceDate <= _tdyDate.AddDays(_leadDays + count))
                    {
                        _isServiceDate = false;
                    }
                }
            }
            else
            {
                if (ServiceDate <= _tdyDate.AddDays(_leadDays))//check service date on lead days 
                {
                    _isServiceDate = false;
                }
            }
            return _isServiceDate;
        }

        /// <summary>
        /// Add attachment files
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddAttachments(SaveInsepectionRequest request, InspTransaction entity)
        {
            //attched files
            request.InspectionFileAttachmentList = request?.InspectionFileAttachmentList ?? new HashSet<BookingFileAttachment>();
            //take the new files
            var newfiles = request.InspectionFileAttachmentList.Where(x => x.Id == 0);
            var existingFiles = request.InspectionFileAttachmentList.Where(x => x.Id > 0);
            var existingFileIds = existingFiles.Select(x => x.Id);
            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

            if (entity.InspTranFileAttachments.Any())
            {
                //take the removed files
                var filesToremove = entity.InspTranFileAttachments.Where(x => !existingFileIds.Contains(x.Id) && x.Active).ToList();

                //remove the inspection files in db entity
                RemoveInspectionFilesDBEntity(filesToremove, userId);

                //Update the inspection files entity
                UpdateInspectionFilesInDBEntity(request.InspectionFileAttachmentList, entity.InspTranFileAttachments);

                //if any files added or removed then need to reset the file data for zip processing and
                //remove the zip attachment
                if (newfiles.Any() || filesToremove.Any())
                {
                    ResetAttachmentForZipProcessing(request.InspectionFileAttachmentList, entity.InspTranFileAttachments);
                    RemoveZipAttachment(entity);
                }
            }

            AddFiles(newfiles, entity, request.UserId);
        }

        /// <summary>
        /// Remove inspection files from the db entity
        /// </summary>
        /// <param name="filesToremove"></param>
        private void RemoveInspectionFilesDBEntity(List<InspTranFileAttachment> filesToremove, int? userId)
        {
            var lstremove = new List<InspTranFileAttachment>();
            foreach (var fileItem in filesToremove)
            {
                fileItem.Active = false;
                fileItem.DeletedBy = userId;
                fileItem.DeletedOn = DateTime.Now;
                lstremove.Add(fileItem);
            }
            if (lstremove.Count > 0)
                _repo.EditEntities(lstremove);
        }

        /// <summary>
        /// Update the inspection files in the db entity
        /// </summary>
        /// <param name="requestFileAttachments"></param>
        /// <param name="entityFileAttachments"></param>
        private void UpdateInspectionFilesInDBEntity(IEnumerable<BookingFileAttachment> requestFileAttachments, IEnumerable<InspTranFileAttachment> entityFileAttachments)
        {
            foreach (var item in entityFileAttachments.Where(x => x.Active))
            {
                var requestFileData = requestFileAttachments.FirstOrDefault(x => x.uniqueld == item.UniqueId);

                item.IsbookingEmailNotification = requestFileData.IsBookingEmailNotification;
                item.IsReportSendToFb = requestFileData.IsReportSendToFB;
            }
        }

        /// <summary>
        /// Reset the file attachment to process the zip file
        /// </summary>
        /// <param name="requestFileAttachments"></param>
        /// <param name="entityFiles"></param>
        private void ResetAttachmentForZipProcessing(IEnumerable<BookingFileAttachment> requestFileAttachments, IEnumerable<InspTranFileAttachment> entityFiles)
        {
            //on update inspection booking reset zip status and ziptrycount to 0
            foreach (var item in entityFiles.Where(x => x.Active))
            {
                item.ZipStatus = (int)ZipStatus.Pending;
                item.ZipTryCount = 0;
            }

        }

        /// <summary>
        /// Remove the zip attachment
        /// </summary>
        /// <param name="entity"></param>
        private void RemoveZipAttachment(InspTransaction entity)
        {
            var inspTranZipAttachment = entity.InspTranFileAttachmentZips.FirstOrDefault(x => x.Active.HasValue && x.Active.Value);
            if (inspTranZipAttachment != null)
            {
                inspTranZipAttachment.Active = false;
                _repo.EditEntity(inspTranZipAttachment);
            }
        }

        private void setInternalComments(SaveInsepectionRequest request, InspTransaction entity)
        {

            string comment = null;
            //check if service date is modified
            if (request.ServiceDateFrom.ToDateTime() != entity.ServiceDateFrom || request.ServiceDateTo.ToDateTime() != entity.ServiceDateTo)
            {
                if (request.ServiceDateFrom.ToDateTime() != request.ServiceDateTo.ToDateTime())
                    comment = " ( Service Date - " + request.ServiceDateFrom.ToDateTime().ToString(StandardDateFormat) + " - " + request.ServiceDateTo.ToDateTime().ToString(StandardDateFormat) + " )";

                else
                    comment = " ( Service Date - " + request.ServiceDateFrom.ToDateTime().ToString(StandardDateFormat) + " )";
            }

            //Do not update Service Date comment of the booking if modified in Verified status
            if (request.StatusId == (int)BookingStatus.Verified && entity.StatusId == (int)BookingStatus.Verified)
            {
                comment = null;
            }

            //Add the status log in internal comments for users visibility
            if (entity.StatusId != request.StatusId)
            {
                request.InternalComments = request.InternalComments + (BookingStatus)request.StatusId + " on " + DateTime.Now.ToString(StandardDateFormat) + comment + "\n";
            }

            else
            {
                request.InternalComments = request.InternalComments + "Modified on " + DateTime.Now.ToString(StandardDateFormat) + comment + "\n";
            }
        }

        /// <summary>
        /// Remove the booking products,po,color information from the booking data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        private void RemoveBookingProducts(SaveInsepectionRequest request, InspTransaction bookingEntity)
        {
            var lstPOTransactionsToremove = new List<InspPurchaseOrderTransaction>();
            var lstProductTransactionsToremove = new List<InspProductTransaction>();
            var lstPoColorTransactionsToremove = new List<InspPurchaseOrderColorTransaction>();
            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
            if (bookingEntity != null)
            {

                if (request.BusinessLine == (int)BusinessLine.SoftLine)
                {

                    foreach (var poProductData in request.InspectionProductList)
                    {
                        //take the product transaction
                        var productTransactionEntity = bookingEntity.InspProductTransactions.FirstOrDefault(x => x.Id == poProductData.Id);

                        //if product transaction is not null
                        if (productTransactionEntity != null)
                        {
                            //take the po transaction
                            var poTransactionEntity = productTransactionEntity.InspPurchaseOrderTransactions.FirstOrDefault(x => x.Id == poProductData.PoTransactionId);

                            //take the po color transaction
                            var colorTransactionEntity = poTransactionEntity.InspPurchaseOrderColorTransactions.FirstOrDefault(x => x.Id == poProductData.ColorTransactionId);

                            //if color transaction is not null remove the color transaction
                            if (colorTransactionEntity != null)
                            {
                                colorTransactionEntity.DeletedBy = userId;
                                colorTransactionEntity.DeletedOn = DateTime.Now;
                                colorTransactionEntity.Active = false;
                                lstPoColorTransactionsToremove.Add(colorTransactionEntity);
                            }

                            //if there is no color transaction available under the purchase order then remove the po
                            if (!poTransactionEntity.InspPurchaseOrderColorTransactions.Any(x => x.Active.HasValue && x.Active.Value))
                            {
                                poTransactionEntity.DeletedBy = userId;
                                poTransactionEntity.DeletedOn = DateTime.Now;
                                poTransactionEntity.Active = false;
                                lstPOTransactionsToremove.Add(poTransactionEntity);
                            }

                            //if there is no purchase order under product then remove the product transaction
                            if (!productTransactionEntity.InspPurchaseOrderTransactions.Any(x => x.Active.HasValue && x.Active.Value))
                            {
                                productTransactionEntity.DeletedBy = userId;
                                productTransactionEntity.DeletedOn = DateTime.Now;
                                productTransactionEntity.Active = false;
                                lstProductTransactionsToremove.Add(productTransactionEntity);
                            }
                        }

                    }


                }
                else
                {
                    foreach (var poProductData in request.InspectionProductList)
                    {
                        //take the product transaction
                        var productTransactionEntity = bookingEntity.InspProductTransactions.FirstOrDefault(x => x.Id == poProductData.Id);

                        if (productTransactionEntity != null)
                        {

                            var poTransactionEntity = productTransactionEntity.InspPurchaseOrderTransactions.FirstOrDefault(x => x.Id == poProductData.PoTransactionId);

                            if (poTransactionEntity != null)
                            {
                                poTransactionEntity.DeletedBy = userId;
                                poTransactionEntity.DeletedOn = DateTime.Now;
                                poTransactionEntity.Active = false;
                                lstPOTransactionsToremove.Add(poTransactionEntity);
                            }
                            //if there is no po transactin under the product then remove the product
                            if (!productTransactionEntity.InspPurchaseOrderTransactions.Any(x => x.Active.HasValue && x.Active.Value))
                            {
                                productTransactionEntity.DeletedBy = userId;
                                productTransactionEntity.DeletedOn = DateTime.Now;
                                productTransactionEntity.Active = false;
                                lstProductTransactionsToremove.Add(productTransactionEntity);
                            }
                        }

                    }
                }

                _repo.EditEntities(lstPOTransactionsToremove);
                _repo.EditEntities(lstProductTransactionsToremove);
                _repo.EditEntities(lstPoColorTransactionsToremove);

            }
        }


        public async Task<BookingMailRequest> GetBookingMailDetail(int bookingId, bool? isEmailRequired, bool? isEdit, int? userId = 0)
        {
            var bookingEmailRequest = new BookingMailRequest();
            var bookingMapEmailData = new BookingMapEmailData();

            try
            {
                var user = await _repo.GetUserName(userId > 0 ? userId.GetValueOrDefault() : _ApplicationContext.UserId);
                bookingEmailRequest.UserName = user.FullName;
                bookingEmailRequest.BookingId = bookingId;

                var bookingDetail = await _repo.GetInspectionBookingDetails(bookingId);

                // Booking detail

                if (bookingDetail != null)
                {

                    bookingMapEmailData.BookingDetail = bookingDetail;

                    //get the ae user email list
                    // var customerList = new[] { bookingDetail.CustomerId };
                    //var AEUser = await _userRepo.GetAEByCustomer(customerList.ToList());
                    //bookingEmailRequest.AEUserEmail = string.Join(", ", AEUser.Select(x => x.EmailAddress));

                    //get the service types
                    bookingMapEmailData.ServiceTypes = await _repo.GetBookingServiceTypes(new[] { bookingId });

                    //get the booking hold reasons
                    bookingMapEmailData.BookingHoldReasons = await _repo.GetInspectionHoldReasons(bookingId);

                    //get the brand list
                    bookingMapEmailData.BrandList = await _repo.GetBookingBrandList(new[] { bookingId });

                    //get the department list
                    bookingMapEmailData.DepartmentList = await _repo.GetBookingDepartmentList(new[] { bookingId });

                    //get the buyer list
                    bookingMapEmailData.BuyerList = await _repo.GetBookingBuyerList(new[] { bookingId });

                    //get the factory contact list
                    bookingMapEmailData.Factcontactlist = await _repo.GetFactoryContactsByBookingIds(new[] { bookingId }.ToList());

                    if (bookingDetail.FactoryId > 0)
                    {
                        //get the factory address list
                        bookingMapEmailData.FactoryAddress = await _supplierRepo.GetSupplierHeadOfficeAddress(bookingDetail.FactoryId.GetValueOrDefault());

                        //get the factory contacts
                        bookingMapEmailData.FactoryContacts = await _supplierRepo.GetSupplierContactById(bookingDetail.FactoryId.GetValueOrDefault());
                    }

                    //get the supplier address
                    bookingMapEmailData.SupplierAddress = await _supplierRepo.GetSupplierHeadOfficeAddress(bookingDetail.SupplierId);

                    //get the supplier contacts
                    bookingMapEmailData.SupplierContacts = await _supplierRepo.GetSupplierContactById(bookingDetail.SupplierId);


                    //get the inspection supplier contacts
                    bookingMapEmailData.InspSupplierContacts = await _repo.GetSupplierContactsByBookingIds(new[] { bookingId }.ToList());

                    //get the season details
                    bookingMapEmailData.CustomerSeasonData = await _referenceRepo.GetSeasonById(bookingDetail.CustomerSeasonId);

                    //map the booking details
                    bookingEmailRequest = _bookingmap.MapBookingForEmail(bookingEmailRequest, bookingMapEmailData, (int)CountryEnum.China);

                    //fetch the total containers
                    var bookingContainers = await _repo.GetBookingContainer(new[] { bookingId });
                    var totalContainers = bookingContainers.Where(x => x.ContainerId.HasValue).Select(x => x.ContainerId).Distinct().Count();
                    bookingEmailRequest.TotalContainers = totalContainers;

                    //get the product transaction bookingEmailRequest
                    var productTransactions = await _repo.GetProductTransactionList(bookingId);

                    //get the purchase order bookingEmailRequest
                    var purchaseOrderTransactions = await _repo.GetPurchaseOrderTransactionList(bookingId);

                    if (bookingEmailRequest.BusinessLine == (int)BusinessLine.SoftLine)
                    {
                        var _poColorTransactionList = await _repo.GetPOColorTransactions(bookingId);
                        bookingEmailRequest.InspectionPoList = GetColorPOProductDetails(productTransactions, purchaseOrderTransactions, _poColorTransactionList);
                    }
                    else if (bookingEmailRequest.BusinessLine == (int)BusinessLine.HardLine)
                    {
                        bookingEmailRequest.InspectionPoList = GetPODetails(productTransactions, purchaseOrderTransactions);
                    }

                    //get the inspection file attachments
                    bookingEmailRequest.InspectionFileAttachments = await _repo.GetBookingMappedFiles(bookingId);

                    bookingEmailRequest.SplitBookingId = bookingDetail.SplitPreviousBookingNo == null ? 0 : bookingDetail.SplitPreviousBookingNo.Value;

                    //Get product category details
                    var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
                    //Get Department details
                    var departmentbookingEmailRequest = await _repo.GetBookingDepartmentList(new[] { bookingId });
                    //Get Brand details
                    var brandbookingEmailRequest = await _repo.GetBookingBrandList(new[] { bookingId });

                    var userAccessFilter = new UserAccess
                    {
                        OfficeId = bookingDetail.OfficeId != null ? bookingDetail.OfficeId.Value : 0,
                        ServiceId = (int)Service.InspectionId,
                        CustomerId = bookingDetail.CustomerId,
                        RoleId = (int)RoleEnum.InspectionVerified,
                        ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                        DepartmentIds = departmentbookingEmailRequest.Any() ? departmentbookingEmailRequest?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandbookingEmailRequest.Any() ? brandbookingEmailRequest?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>()
                    };

                    // set AE User 
                    var AEUser = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                    bookingEmailRequest.AEUserEmail = string.Join(", ", AEUser.Select(x => x.EmailAddress));

                    //Get the user details based on the DA_UserCustomer and DA_UserRoleNotificationByOffice tables
                    InspBookingContactResponse bookingContact = await _bookingManager.GetInspBookingContactInformation(userAccessFilter);

                    if (bookingContact != null && bookingContact.BookingContactList != null && bookingContact.BookingContactList.Any())
                    {
                        string apiUserMail = "";
                        string apiUserContactMail = "";
                        foreach (var staff in bookingContact.BookingContactList)
                        {
                            if (staff.MobileNumber != null)
                                apiUserMail += staff.FullName + " (" + staff.EmailAddress + " / " + staff.MobileNumber + "), ";

                            else
                                apiUserMail += staff.FullName + " (" + staff.EmailAddress + "), ";
                            apiUserContactMail += staff.EmailAddress + ";";
                        }

                        bookingEmailRequest.ApiUserMail = apiUserMail;
                        bookingEmailRequest.ApiUserContactMail = apiUserContactMail;
                    }

                    // Edit booking
                    if (isEmailRequired != null)
                    {
                        bookingEmailRequest.IsEmailRequired = isEmailRequired.Value;
                    }

                    // Booking cancel
                    var cancelbookingEmailRequest = await _cancelBookingRepository.GetCancelDetailsById(bookingId);
                    if (cancelbookingEmailRequest != null)
                    {
                        bookingEmailRequest.ReasonType = cancelbookingEmailRequest.ReasonType?.Reason;
                        bookingEmailRequest.Comment = cancelbookingEmailRequest.Comments;
                        bookingEmailRequest.CanceledBy = cancelbookingEmailRequest.CreatedByNavigation?.FullName;
                    }

                    // Booking reschedule
                    var schedulebookingEmailRequest = await _cancelBookingRepository.GetRescheduleDetailsById(bookingId);
                    if (schedulebookingEmailRequest != null)
                    {
                        //bookingEmailRequest.RescheduleServiceDateFrom = schedulebookingEmailRequest.ServiceFromDate.ToString(StandardDateFormat);
                        //bookingEmailRequest.RescheduleServiceDateTo = schedulebookingEmailRequest.ServiceToDate.ToString(StandardDateFormat);
                        bookingEmailRequest.RescheduleServiceDateFrom = schedulebookingEmailRequest.Inspection.InspTranStatusLogs.OrderByDescending(x => x.StatusChangeDate).Skip(1).Select(x => x.ServiceDateFrom).First().GetValueOrDefault().ToString(StandardDateFormat);
                        bookingEmailRequest.RescheduleServiceDateTo = schedulebookingEmailRequest.Inspection.InspTranStatusLogs.OrderByDescending(x => x.StatusChangeDate).Skip(1).Select(x => x.ServiceDateTo).First().GetValueOrDefault().ToString(StandardDateFormat);
                        bookingEmailRequest.ReasonType = schedulebookingEmailRequest.ReasonType.Reason;
                        bookingEmailRequest.Comment = schedulebookingEmailRequest.Comments;
                    }
                    if (bookingEmailRequest.StatusId == (int)BookingStatus.Cancel)
                        bookingEmailRequest.StatusName = "Cancelled";
                    else if (bookingEmailRequest.StatusId == (int)BookingStatus.Rescheduled)
                        bookingEmailRequest.StatusName = "Rescheduled";
                    else if (bookingEmailRequest.StatusId == (int)BookingStatus.Received && isEdit.HasValue && isEdit.Value)
                        bookingEmailRequest.StatusName = "Modified";

                    // file path set for special note in confirmed e-mail
                    if (bookingEmailRequest.StatusId == (int)BookingStatus.Confirmed)
                    {
                        var filepath = _ApplicationContext.AppBaseUrl; //_env.WebRootPath;

                        var masterConfings = await GetMasterConfiguration();
                        var inspConfirmTermsEnglish = masterConfings.Where(x => x.Type == (int)EntityConfigMaster.InspConfirmTermsEnglish).Select(x => x.Value).FirstOrDefault();
                        var inspConfirmTermsChinese = masterConfings.Where(x => x.Type == (int)EntityConfigMaster.InspConfirmTermsChinese).Select(x => x.Value).FirstOrDefault();

                        bookingEmailRequest.InspConfirmEngDocPath = string.Concat(filepath, inspConfirmTermsEnglish);
                        bookingEmailRequest.InspConfirmCnDocPath = string.Concat(filepath, inspConfirmTermsChinese);

                        bookingEmailRequest.InspectionConfirmFooter = masterConfings.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.InspectionConfirmFooter && x.CountryId != (int)CountryEnum.China)?.Value;

                        bookingEmailRequest.InspectionConfirmFooter = bookingEmailRequest.InspectionConfirmFooter.Replace("{docpath}", bookingEmailRequest.InspConfirmEngDocPath);

                        if (bookingEmailRequest.IsChinaCountry)
                        {
                            bookingEmailRequest.InspectionConfirmChineseFooter = masterConfings.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.InspectionConfirmFooter && x.CountryId == (int)CountryEnum.China)?.Value;
                            bookingEmailRequest.InspectionConfirmChineseFooter = bookingEmailRequest.InspectionConfirmChineseFooter?.Replace("{docpath}", bookingEmailRequest.InspConfirmEngDocPath);
                        }

                    }

                    if (bookingEmailRequest.StatusId == (int)BookingStatus.Rescheduled)
                    {
                        var masterConfings = await GetMasterConfiguration();

                        bookingEmailRequest.InspectionRescheduleEnglishFooter = masterConfings.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.InspectionRescheduleFooter && x.CountryId != (int)CountryEnum.China)?.Value;

                        if (bookingEmailRequest.IsChinaCountry)
                        {
                            bookingEmailRequest.InspectionRescheduleChineseFooter = masterConfings.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.InspectionRescheduleFooter && x.CountryId == (int)CountryEnum.China)?.Value;
                        }
                    }
                    bookingEmailRequest.IsAfter48Hours = (DateTime.Today - bookingDetail.ServiceDateFrom).TotalDays > BookingRuleDays ? true : false;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get booking model for email");
            }
            return bookingEmailRequest;
        }

        /// <summary>
        /// GetPODetails
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns>BookingMailRequest</returns>
        private List<InspectionPOProductItem> GetPODetails(List<InspectionProductDetail> productList, List<InspectionPODetail> purchaseOrderDetails)
        {
            List<InspectionPOProductItem> ponoList = new List<InspectionPOProductItem>();



            foreach (var item in productList)
            {

                var poTransactions = purchaseOrderDetails.Where(x => x.ProductRefId == item.Id).ToList();
                var ponumber = String.Join(",", poTransactions?.Select(x => x.PoName).ToArray());
                var destinationCountry = String.Join(",", poTransactions?.Select(x => x.DestinationCountryName)?.Distinct().ToArray());
                var bookingQty = poTransactions?.Select(x => x.BookingQuantity)?.Sum();
                var pickingQty = poTransactions?.Select(x => x.PickingQuantity)?.Sum();

                var product = new InspectionPOProductItem
                {
                    Id = item.ProductId,
                    PoNo = ponumber,
                    ProductId = item.ProductName,
                    ProductDesc = item.ProductDesc,
                    BookingQty = bookingQty ?? 0,
                    PickingQty = pickingQty ?? 0,
                    Remarks = item.Remarks,
                    CombineAqlQty = item.CombineSamplingSize,
                    CombineProductId = item.CombineGroupId,
                    AqlLevel = item.AqlName,
                    DestinationCountry = destinationCountry,
                    SubCategory2 = item.ProductCategorySub2Name,
                    Unit = item.UnitName
                };
                ponoList.Add(product);
            }
            return ponoList;
        }
        /// <summary>
        /// get color, po, product details
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="POColorTransactionList"></param>
        /// <returns></returns>
        private List<InspectionPOProductItem> GetColorPOProductDetails(List<InspectionProductDetail> productList,
               List<InspectionPODetail> purchaseOrderDetails, List<InspectionPOColorTransaction> POColorTransactionList)
        {
            List<InspectionPOProductItem> colorCodeList = new();

            foreach (var item in POColorTransactionList)
            {
                var purchaseOrderData = purchaseOrderDetails.Where(x => x.Id == item.PoTransactionId).FirstOrDefault();
                var productData = productList?.Where(x => x.Id == item.ProductRefId && purchaseOrderData.Id == item.PoTransactionId).FirstOrDefault();

                if (productData != null)
                {
                    var colorByProduct = new InspectionPOProductItem
                    {
                        Id = productData.ProductId,
                        PoNo = purchaseOrderData?.PoName,
                        ProductId = productData.ProductName,
                        ProductDesc = productData.ProductDesc,
                        BookingQty = item.BookingQuantity.GetValueOrDefault(),
                        Remarks = productData.Remarks,
                        CombineAqlQty = productData.CombineSamplingSize,
                        CombineProductId = productData.CombineGroupId,
                        AqlLevel = productData.AqlName,
                        DestinationCountry = purchaseOrderData?.DestinationCountryName,
                        SubCategory2 = productData.ProductCategorySub2Name,
                        ColorName = item.ColorName,
                        ColorCode = item.ColorCode,
                        Unit = productData.UnitName
                    };
                    colorCodeList.Add(colorByProduct);
                }


            }
            return colorCodeList;
        }

        /// <summary>
        /// Get the file attachment details
        /// </summary>
        /// <param name="inspectionFileAttachments"></param>
        /// <returns></returns>
        private List<InspectionFileAttachments> GetInspectionFileDetails(List<InspTranFileAttachment> inspectionFileAttachments)
        {
            List<InspectionFileAttachments> fileAttachments = new List<InspectionFileAttachments>();

            foreach (var inspectionFileAttachment in inspectionFileAttachments)
            {
                var fileAttachment = new InspectionFileAttachments()
                {
                    FileName = inspectionFileAttachment.FileName,
                    FileUrl = inspectionFileAttachment.FileUrl,
                    IsBookingEmailNotification = inspectionFileAttachment.IsbookingEmailNotification.GetValueOrDefault()
                };

                fileAttachments.Add(fileAttachment);
            }

            return fileAttachments;
        }

        /// <summary>
        /// New booking 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<SaveInspectionBookingResponse> NewInspectionBooking(SplitBooking request)
        {
            try
            {
                var response = new SaveInspectionBookingResponse();

                if (request == null)
                {
                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotFound };
                }

                InspTransaction entity = _mapper.Map<InspTransaction>(request.BookingData);

                if (entity == null)
                {
                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotFound };
                }

                // check booking products not available
                if (request.BookingData == null || request.BookingData.InspectionProductList == null)
                {
                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.BookingProductsNotAvailable };
                }

                // Add Booking Log information - in the Event booking log table.
                await _eventBookingLog.SaveLogInformation(new EventBookingLogInfo()
                {
                    Id = 0,
                    AuditId = 0,
                    BookingId = 0,
                    StatusId = (int)BookingStatus.Received,
                    LogInformation = JsonConvert.SerializeObject(request)
                });

                var inspectionID = request.BookingData.SplitPreviousBookingNo.GetValueOrDefault();

                if (inspectionID < 0)
                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotFound };

                var bookingEntity = await _repo.GetInspectionBaseTransaction(inspectionID);

                //// remove po data from the existing inspection
                RemoveBookingProducts(request.BookingData, bookingEntity);

                foreach (var poProductData in request.BookingData.InspectionProductList)
                {
                    poProductData.Id = 0;
                    poProductData.PoTransactionId = 0;
                    poProductData.ColorTransactionId = 0;
                }

                UpdateProductCategoryDataToInspection(request.BookingData.InspectionProductList.FirstOrDefault(), entity);

                await AddInspectionPOProductList(request.BookingData, entity);

                AddInspectionCustomerContactList(request.BookingData, entity);

                AddInspectionFactoryContactList(request.BookingData, entity);

                AddInspectionSupplierContactList(request.BookingData, entity);

                AddInspectionServiceTypeList(request.BookingData, entity);

                AddInspectionCustomerBrandList(request.BookingData, entity);

                AddInspectionCustomerBuyerList(request.BookingData, entity);

                AddInspectionCustomerDepartmentList(request.BookingData, entity);

                AddFiles(request.BookingData.InspectionFileAttachmentList, entity);

                entity.StatusId = (int)BookingStatus.Received;
                entity.EntityId = _filterService.GetCompanyId();

                // add status log table
                entity.InspTranStatusLogs.Add(new InspTranStatusLog()
                {
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    StatusId = (int)BookingStatus.Received,
                    ServiceDateFrom = request.BookingData.ServiceDateFrom.ToDateTime(),
                    ServiceDateTo = request.BookingData.ServiceDateTo.ToDateTime(),
                    StatusChangeDate = DateTime.Now,
                    EntityId = entity.EntityId
                });

                var Id = await _repo.AddInspectionBooking(entity);

                if (Id > 0)
                {
                    response.Id = entity.Id;
                }

                if (entity.Id == 0)
                {
                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotSaved };
                }

                response.Result = SaveInspectionBookingResult.Success;


                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cancel booking 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<SaveInspectionBookingResponse> CancelInspectionBooking(SplitBooking request)
        {
            try
            {
                var response = new SaveInspectionBookingResponse();

                if (request == null)
                {
                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotFound };
                }

                var inspectionID = request.BookingData.Id;

                var bookingEntity = await _repo.GetInspectionBaseTransaction(inspectionID);

                if (bookingEntity == null)
                    return new SaveInspectionBookingResponse { Result = SaveInspectionBookingResult.InspectionBookingIsNotFound };

                RemoveBookingProducts(request.BookingData, bookingEntity);

                await _repo.EditInspectionBooking(bookingEntity);
                response.Id = bookingEntity.Id;
                response.Result = SaveInspectionBookingResult.Success;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Update the booking status by booking id and status id 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public async Task<BookingStatusUpdateResponse> UpdateBookingStatus(int bookingId, int statusId)
        {
            var bookingEntity = await _repo.GetBookingTransaction(bookingId);

            var response = new BookingStatusUpdateResponse();

            if (bookingEntity == null)
            {
                response.Result = BookingStatusUpdateResponseResult.failed;
                return response;
            }
            // update the status
            bookingEntity.StatusId = statusId;

            await _repo.EditInspectionBooking(bookingEntity);

            response.Result = BookingStatusUpdateResponseResult.success;

            return response;
        }


        //public async Task<BookingPoProductResponse> GetPurchaseOrderProductsByPoNumber(int? poid, int supplierId)
        //{
        //    var response = new BookingPoProductResponse();

        //    if (poid != null)
        //    {
        //        var purchaseOrder = _poRepo.GetPurchaseOrderItemsById(poid);

        //        if (purchaseOrder == null)
        //        {
        //            response.Data = null;
        //            return new BookingPoProductResponse { Result = BookingPoProductResult.NotFound };
        //        }
        //        else
        //        {
        //            var data = await MapPoProducts(purchaseOrder, supplierId);
        //            response.Data = data.OrderBy(x => x.ProductName).ToList();
        //        }
        //    }
        //    response.Result = BookingPoProductResult.Success;
        //    return response;
        //}

        /// <summary>
        /// Map the po products
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        //private async Task<List<BookingPoProduct>> MapPoProducts(CuPurchaseOrder entity, int supplierId)
        //{

        //    List<BookingPoProduct> listPOProducts = new List<BookingPoProduct>();

        //    if (entity.CuPurchaseOrderDetails != null && entity.CuPurchaseOrderDetails.Any())
        //    {
        //        var purchaserOrderDetails = entity.CuPurchaseOrderDetails.Where(x => x.SupplierId == supplierId && x.Active.HasValue && x.Active.Value);
        //        foreach (var item in purchaserOrderDetails)
        //        {

        //            var bookingProduct = new BookingPoProduct();
        //            bookingProduct.Id = item.Id;
        //            bookingProduct.PoId = item.PoId;
        //            bookingProduct.ProductId = item.ProductId;
        //            bookingProduct.Pono = entity.Pono;
        //            bookingProduct.ProductCategoryName = item.Product?.ProductCategoryNavigation?.Name;
        //            bookingProduct.ProductCategorySub2Name = item.Product?.ProductCategorySub2Navigation?.Name;
        //            bookingProduct.ProductName = item.Product.ProductId;
        //            bookingProduct.ProductDesc = item.Product.ProductDescription;
        //            bookingProduct.ProductRemarks = item.Product.Remarks;
        //            bookingProduct.ProductCategoryId = item.Product.ProductCategory;
        //            bookingProduct.ProductSubCategoryId = item.Product.ProductSubCategory;
        //            bookingProduct.ProductCategoryName = item.Product?.ProductCategoryNavigation?.Name;
        //            bookingProduct.ProductSubCategoryName = item.Product.ProductSubCategoryNavigation?.Name;
        //            bookingProduct.ProductQuantity = item.Quantity;
        //            bookingProduct.IsNewProduct = item.Product?.IsNewProduct != null ? item.Product?.IsNewProduct : false;
        //            bookingProduct.Selected = false;
        //            bookingProduct.ProductCategorySub2List = null;
        //            bookingProduct.ProductCategorySub2Id = item.Product.ProductCategorySub2;
        //            bookingProduct.ProductCategoryMapped = item?.Product?.ProductCategory != null ? true : false;
        //            bookingProduct.ProductSubCategoryMapped = item?.Product?.ProductSubCategory != null ? true : false;
        //            bookingProduct.ProductCategorySub2Mapped = item?.Product?.ProductCategorySub2 != null ? true : false;
        //            bookingProduct.ProductFileAttachments = item.Product.CuProductFileAttachments.Where(x => x.Active && x.FileTypeId.HasValue && x.FileTypeId.Value == (int)ProductRefFileType.Pictures).Select(x => new ProductFileAttachmentRepsonse()
        //            {
        //                FileName = x.FileName,
        //                Id = x.Id,
        //                IsNew = false,
        //                FileUrl = x.FileUrl,
        //                uniqueld = x.UniqueId,
        //                MimeType = ""
        //            });

        //            //Load only the mapped product category data
        //            if (item.Product != null && item.Product.ProductSubCategoryNavigation != null)
        //            {
        //                List<DTO.References.ProductSubCategory> productSubCategoryList = new List<DTO.References.ProductSubCategory>();
        //                DTO.References.ProductSubCategory productSubCategory = new DTO.References.ProductSubCategory();
        //                productSubCategory.Id = item.Product.ProductSubCategoryNavigation.Id;
        //                productSubCategory.Name = item.Product.ProductSubCategoryNavigation.Name;
        //                productSubCategoryList.Add(productSubCategory);
        //                bookingProduct.ProductCategorySubList = productSubCategoryList;
        //            }

        //            var objListInternal = new List<CommonDataSource>();
        //            if (item.Product != null && item.Product.ProductCategorySub2Navigation != null)
        //            {
        //                var productSubCategory2 = new CommonDataSource();
        //                productSubCategory2.Id = item.Product.ProductCategorySub2Navigation.Id;
        //                productSubCategory2.Name = item.Product.ProductCategorySub2Navigation.Name;
        //                objListInternal.Add(productSubCategory2);
        //            }
        //            else if (item.Product != null && item.Product.ProductCategorySub2Navigation == null && item.Product.ProductSubCategoryNavigation != null)
        //            {
        //                objListInternal = item.Product.ProductSubCategoryNavigation.RefProductCategorySub2S.OrderBy(x => x.Name).Select(x => new CommonDataSource() { Id = x.Id, Name = x.Name }).ToList();
        //            }

        //            bookingProduct.ProductCategorySub2List = objListInternal;

        //            bookingProduct.DestinationCountryID = item.DestinationCountryId;
        //            bookingProduct.FactoryReference = item.Product?.FactoryReference;
        //            bookingProduct.ETD = Static_Data_Common.GetCustomDate(item.Etd);
        //            bookingProduct.Barcode = item.Product?.Barcode;
        //            bookingProduct.CustomerReferencePo = item.Po?.CustomerReferencePo;

        //            listPOProducts.Add(bookingProduct);


        //        }
        //    }
        //    return listPOProducts;
        //}

        public async Task<BookingSummaryResponse> GetBookingSummary()
        {
            var response = new BookingSummaryResponse();

            //customer list
            var cuslistresponse = await _customerManager.GetCustomersByUserType();
            if (cuslistresponse == null || cuslistresponse.Result != CustomerSummaryResult.Success || cuslistresponse.CustomerList == null)
                return new BookingSummaryResponse() { Result = BookingSummaryResponseResult.failed };
            response.CustomerList = cuslistresponse.CustomerList;

            //office list
            response.OfficeList = await _office.GetOfficesAsync();
            if (UserTypeEnum.InternalUser == _ApplicationContext.UserType)//logic for location configuration
            {
                var _cusofficelist = await _office.GetOfficesByUserIdAsync(_ApplicationContext.StaffId);
                response.OfficeList = _cusofficelist == null || _cusofficelist.Count() == 0 ? response.OfficeList : _cusofficelist;
            }


            var statuslist = await _repo.GetBookingStatus();
            if (statuslist != null && statuslist.Count == 0)
                return new BookingSummaryResponse() { Result = BookingSummaryResponseResult.failed };
            response.StatusList = statuslist.OrderBy(y => y.Priority).Select(x => _bookingmap.GetInspectionStatus(x));

            var _quotationStatusList = await _quotationRepository.GetStatusList();

            if (_quotationStatusList != null && _quotationStatusList.ToList().Count == 0)
                return new BookingSummaryResponse() { Result = BookingSummaryResponseResult.failed };
            response.QuotationStatusList = _quotationStatusList.Select(x => _quotationmap.GetStatus(x));

            if (UserTypeEnum.InternalUser != _ApplicationContext.UserType)//logic for location configuration
            {
                //supplier list
                var supplierlistresponse = await _suppliermanager.GetSuppliersByUserType(null);
                if (supplierlistresponse.Result == SupplierListResult.Success && supplierlistresponse.Data.Count() > 0)
                    response.SupplierList = supplierlistresponse.Data;

                //factory list
                var factlistresponse = await _suppliermanager.GetFactorysByUserType(null, null);
                if (factlistresponse.Result == SupplierListResult.Success && factlistresponse.Data.Count() > 0)
                    response.FactoryList = factlistresponse.Data;
            }
            //AE List
            var _AEList = await _userRepo.GetAEList();
            if (_AEList == null && _AEList.Count == 0)
                return new BookingSummaryResponse() { Result = BookingSummaryResponseResult.failed };
            var list = _AEList.GroupBy(p => p.Id, (key, _data) =>

                   new AECustomerList
                   {
                       Id = key,
                       FullName = _data.Where(x => x.Id == key).Select(x => x.FullName).FirstOrDefault(),
                       Customerid = _data.Where(x => x.Id == key && x.Customerid > 0).Select(x => x.Customerid).ToList()
                   });
            response.AEList = list;

            response.Result = BookingSummaryResponseResult.success;
            return response;
        }

        /// <summary>
        /// Get the booking status and quotation status master data
        /// </summary>
        /// <returns></returns>
        public async Task<BookingSummaryStatusResponse> GetBookingSummaryStatus()
        {
            var response = new BookingSummaryStatusResponse();
            //get the booking status
            var statuslist = await _repo.GetBookingStatus();
            if (statuslist != null && statuslist.Any())
            {
                response.StatusList = statuslist.OrderBy(y => y.Priority).Select(x => _bookingmap.GetInspectionStatus(x)).ToList();
                response.Result = BookingSummaryStatusResponseResult.Success;
            }
            //get the quotaiton status
            var quotationStatus = await _quotationRepository.GetStatusList();
            var quotationStatusList = quotationStatus.ToList();

            if (quotationStatusList != null && quotationStatusList.Any())
            {
                response.QuotationStatusList = quotationStatusList.Select(x => _quotationmap.GetStatus(x)).ToList();
                response.Result = BookingSummaryStatusResponseResult.Success;
            }

            if (response.StatusList == null || !response.StatusList.Any())
                response.Result = BookingSummaryStatusResponseResult.StatusListNotFound;

            if (response.QuotationStatusList == null || !response.QuotationStatusList.Any())
                response.Result = BookingSummaryStatusResponseResult.QuotationStatusListNotFound;


            return response;
        }

        /// <summary>
        /// Get the ae user list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetAEUserList()
        {
            var aeList = await _userRepo.GetAEUserList();
            if (aeList != null && aeList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.Success, DataSourceList = aeList };

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Set booking role 
        /// </summary>
        /// <param name="bookingDetails"></param>
        private void setBookingRole(EditInspectionBookingResponse bookingDetails)
        {
            if (UserTypeEnum.InternalUser == _ApplicationContext.UserType)//logic for location configuration
            {
                bookingDetails.IsBookingRequestRole = false;
                bookingDetails.IsBookingConfirmRole = false;
                bookingDetails.IsBookingVerifyRole = false;

                foreach (var role in _ApplicationContext.RoleList)
                {

                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionRequest))
                    {
                        bookingDetails.IsBookingRequestRole = true;
                    }

                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionConfirmed))
                    {
                        bookingDetails.IsBookingConfirmRole = true;
                    }

                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionVerified))
                    {
                        bookingDetails.IsBookingVerifyRole = true;
                    }
                }
            }
        }

        /// <summary>
        /// Get booking role 
        /// </summary>
        /// <param name="bookingDetails"></param>
        private InternalUserRoleAccess getBookingRole()
        {
            InternalUserRoleAccess roleDetails = new InternalUserRoleAccess();
            if (UserTypeEnum.InternalUser == _ApplicationContext.UserType)//logic for location configuration
            {
                //roleDetails.IsBookingRequestRole = false;
                //roleDetails.IsBookingConfirmRole = false;
                //roleDetails.IsBookingVerifyRole = false;
                if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionRequest))
                {
                    roleDetails.IsBookingRequestRole = true;
                }
                if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionConfirmed))
                {
                    roleDetails.IsBookingConfirmRole = true;
                }
                if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionVerified))
                {
                    roleDetails.IsBookingVerifyRole = true;
                }
                if (_ApplicationContext.RoleList.Contains((int)RoleEnum.QuotationRequest))
                {
                    roleDetails.IsQuotationRequestRole = true;
                }
            }
            return roleDetails;
        }

        public async Task<List<int>> GetPreviousBookingNoList(InspectionBookingDetail bookingDetail)
        {
            List<int> prevBookingNoList = new List<int>();

            //get all the previous booking value untill the prev booking value is null
            if (bookingDetail.PreviousBookingNo > 0 && bookingDetail.PreviousBookingNo != bookingDetail.InspectionId)
            {
                int? prevBookingNo = bookingDetail.PreviousBookingNo.GetValueOrDefault();
                prevBookingNoList.Add(bookingDetail.PreviousBookingNo.GetValueOrDefault());

                //loops untill the prev booking no is null
                do
                {
                    prevBookingNo = await _repo.GetPreviousBookingNumber(prevBookingNo.GetValueOrDefault());

                    if (prevBookingNo > 0)
                        prevBookingNoList.Add(prevBookingNo.GetValueOrDefault());
                } while (prevBookingNo > 0 && prevBookingNo != bookingDetail.InspectionId && prevBookingNoList.IndexOf(prevBookingNo.GetValueOrDefault()) == -1);

            }

            return prevBookingNoList;
        }

        /// <summary>
        /// Edit Inspection Booking Data
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="inspectionPageType"></param>
        /// <returns></returns>
        public async Task<EditInspectionBookingResponse> EditInspectionData(int bookingId)
        {
            #region Initialization
            var response = new EditInspectionBookingResponse() { BookingDetails = new InspectionBookingDetails() };

            List<InspectionProductSubCategory> productSubCategoryList = new List<InspectionProductSubCategory>();

            List<InspectionProductSubCategory2> productSubCategory2List = new List<InspectionProductSubCategory2>();

            List<InspectionProductSubCategory3> productSubCategory3List = new List<InspectionProductSubCategory3>();

            var inspectionPickingData = new List<InspectionPickingDetails>();

            var inspectionPickingContacts = new List<InspectionPickingContactDetails>();

            #endregion

            if (bookingId > 0)
            {
                //get the booking detail
                var bookingDetail = await _repo.GetInspectionBookingDetails(bookingId);

                if (bookingDetail == null)
                    return new EditInspectionBookingResponse() { Result = EditInspectionBookingResult.CannotGetBookingDetails };

                //get the previous booking no list
                List<int> prevBookingNoList = await GetPreviousBookingNoList(bookingDetail);

                // get quotation manday by booking id
                double? totalManDays = await _repo.GetQuotationManDayByBooking(bookingId);
                response.TotalMandays = totalManDays.GetValueOrDefault(0);

                //get the booking mapped departments
                var departments = await _repo.GetBookingMappedDepartments(bookingId);

                //get the booking mapped brands
                var brands = await _repo.GetBookingMappedBrands(bookingId);

                List<int?> _officeId = new List<int?>();
                if (bookingDetail.OfficeId.HasValue)
                {
                    _officeId.Add(bookingDetail.OfficeId);
                }

                var userAccessrequest = new UserAccess
                {
                    OfficeIds = _officeId.Any() ? _officeId.AsEnumerable() : Enumerable.Empty<int?>(),
                    CustomerId = bookingDetail.CustomerId,
                    DepartmentIds = departments.Any() ? departments.Select(x => (int?)x).Distinct() : Enumerable.Empty<int?>(),
                    BrandIds = brands.Any() ? brands.Select(x => (int?)x).Distinct() : Enumerable.Empty<int?>()
                };

                ////get the booking mapped cs names
                //var csNameList = await getCSNames(userAccessrequest);

                //var csList = await getCSList(userAccessrequest);

                //get the product transaction list
                var productTransactions = await _repo.GetProductTransactionList(bookingId);

                //get the po transaction list
                var poTransactions = await _repo.GetPurchaseOrderTransactionList(bookingId);

                //get the po color transaction list
                var poColorTransactions = await _repo.GetPOColorTransactions(bookingId);

                var inspectionCsList = await _repo.GetInspectionTransCSList(bookingId);

                //get the booking mapped service type list
                var serviceTypeList = await _repo.GetServiceType(new[] { bookingId });

                //get the booking mapped hold reason
                var holdReason = await _repo.GetInspectionHoldReasons(bookingId);

                //get the booking mapped customer contacts
                var customerContacts = await _repo.GetBookingMappedCustomerContacts(bookingId);

                //get the booking mapped supplier contacts
                var supplierContacts = await _repo.GetBookingMappedSupplierContacts(bookingId);

                //get the booking mapped factory contacts
                var factoryContacts = await _repo.GetBookingMappedFactoryContacts(bookingId);

                //get the booking mapped buyers
                var buyers = await _repo.GetBookingMappedBuyers(bookingId);

                //get the booking mapped merchandisers
                var merchandisers = await _repo.GetBookingMappedMerchandisers(bookingId);

                //get the booking mapped shipment types
                var shipmentTypes = await _repo.GetBookingMappedShipmentTypes(bookingId);

                //get the booking mapped containers
                var containers = await _repo.GetBookingMappedContainers(bookingId);

                //get the booking mapped dynamic data
                var inspectionDFData = await _repo.GetBookingMappedDFTransactions(bookingId);

                ////get the booking mapped files
                var inspectionFiles = await _repo.GetBookingMappedFiles(bookingId);

                if (productTransactions != null && productTransactions.Any())
                {
                    //take the product category ids if product subcategory is null for the product
                    var productCategoryIds = productTransactions.Where(x => x.ProductSubCategoryId == null).Select(x => x.ProductCategoryId).ToList();
                    //take the product sub category list
                    if (productCategoryIds != null && productCategoryIds.Any())
                        productSubCategoryList = await _repo.GetProductSubCategoryList(productCategoryIds);

                    //take the product sub category ids if product subcategory2 is null for the product
                    var productSubCategoryIds = productTransactions.Where(x => x.ProductCategorySub2Id == null).Select(x => x.ProductSubCategoryId).ToList();
                    //take the product sub category2 list
                    if (productSubCategoryIds != null && productSubCategoryIds.Any())
                        productSubCategory2List = await _repo.GetProductSubCategory2List(productSubCategoryIds);
                    //take the product sub category ids if product subcategory2 is null for the product

                    var productSubCategory2Ids = productTransactions.Where(x => x.ProductCategorySub3Id == null).Select(x => x.ProductCategorySub2Id.GetValueOrDefault()).ToList();
                    //take the product sub category2 list
                    if (productSubCategory2Ids != null && productSubCategory2Ids.Any())
                        productSubCategory3List = await _repo.GetProductSubCategory3List(productSubCategory2Ids);

                }
                //take the product file attachments
                var productFileAttachments = await _repo.GetProductFileAttachments(bookingId);

                if (bookingDetail.IsPickingRequired.GetValueOrDefault())
                {
                    //get the inspection picking data
                    inspectionPickingData = await _repo.GetInspectionPicking(bookingId);

                    //get the inspection picking contacts
                    if (inspectionPickingData.Any())
                    {
                        var pickingIds = inspectionPickingData.Select(x => x.Id).ToList();
                        inspectionPickingContacts = await _repo.GetInspectionPickingContacts(pickingIds);
                    }
                }

                var customerCheckPointList = await _customerCheckPointRepository.GetCustomerCheckPointList(bookingDetail.CustomerId, (int)Service.InspectionId);
                //map the booking details
                response.BookingDetails = _bookingmap.MapBookingData(bookingDetail, (x) => _fileManager.GetMimeType(x), serviceTypeList.ToList(), holdReason,
                                            customerContacts, supplierContacts, factoryContacts, buyers, brands, departments, merchandisers,
                                            shipmentTypes, productTransactions, poTransactions, containers, inspectionDFData, inspectionFiles,
                                            productSubCategoryList, productSubCategory2List, productSubCategory3List, productFileAttachments,
                                            prevBookingNoList, inspectionCsList, poColorTransactions, customerCheckPointList,
                                            inspectionPickingData, inspectionPickingContacts, _ApplicationContext.UserType);

                //check the picking is done
                response.BookingDetails.isPickingDone = false;
                if (poTransactions != null && poTransactions.Any())
                {
                    //take only the potransaction where picking qty is greater than zero
                    var poTransIds = poTransactions.Where(x => x.PickingQuantity > 0 && x.Active.HasValue && x.Active.Value).Select(x => x.Id).ToList();
                    if (poTransIds.Any())
                        response.BookingDetails.isPickingDone = await _pickingRepository.GetInspectionPickingExists(poTransIds);

                }


                // check booking is invoiced
                response.IsBookingInvoiced = await _repo.IsBookingInvoiced(bookingId);

                //get booking service configuration data
                if (serviceTypeList != null && serviceTypeList.Any())
                {
                    var customerServiceConfigResponse = _customerServiceManager.
                               ServiceByCustomerAndServiceTypeID(bookingDetail.CustomerId,
                                                       serviceTypeList.FirstOrDefault().serviceTypeId);
                    if (customerServiceConfigResponse != null && customerServiceConfigResponse.Result == EditCustomerServiceConfigResult.Success)
                    {
                        response.BookingServiceConfig = customerServiceConfigResponse.CustomerServiceConfigData;
                    }
                }

                if (response.BookingDetails == null)
                {
                    return new EditInspectionBookingResponse() { Result = EditInspectionBookingResult.CannotGetBookingDetails };
                }
                else
                {
                    this.setBookingRole(response);
                }
            }

            response.UserType = _ApplicationContext.UserType;
            response.Result = EditInspectionBookingResult.Success;

            return response;
        }

        /// <summary>
        /// Add inspection booking data
        /// </summary>
        /// <returns></returns>
        public async Task<EditInspectionBookingResponse> AddInspectionData()
        {
            EditInspectionBookingResponse response = new EditInspectionBookingResponse();
            //fetch supplier list or factory list based on the user type (new booking)
            switch (_ApplicationContext.UserType)
            {

                case UserTypeEnum.Supplier:
                    {
                        var suplist = await _suppliermanager.GetSuppliersByUserType(null);
                        response.SupplierList = suplist.Data;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        //fact list
                        var factlist = await _suppliermanager.GetFactorysByUserType(null, null);
                        response.FactoryList = factlist.Data;
                        break;
                    }
            }

            this.setBookingRole(response);

            response.UserType = _ApplicationContext.UserType;
            response.Result = EditInspectionBookingResult.Success;

            return response;
        }

        public InspectionBookingDetails RemoveCompletedPODetail(InspectionBookingDetails bookingDetail)
        {
            if (bookingDetail != null)
            {
                if (bookingDetail.InspectionPoList != null && bookingDetail.InspectionPoList.Count() > 0)
                {
                    bookingDetail.InspectionPoList = bookingDetail.InspectionPoList.Where(x => x.PoReminingQuantity != 0);
                }
            }
            return bookingDetail;
        }

        public async Task<PickingAndCombineOrderResponse> GetPickingAndCombineOrders(int id)
        {
            var response = new PickingAndCombineOrderResponse();
            try
            {
                if (id != 0)
                {
                    response.CombineOrderResponse = await _combineOrdermanager.GetCombineOrderDetails(id);
                    response.PickingResponse = await _pickingManager.GetPickingDetails(id);
                }

                response.Result = PickingAndCombineOrderResult.Success;
            }
            catch (Exception ex)
            {
                return new PickingAndCombineOrderResponse() { Result = PickingAndCombineOrderResult.CannotGetCombineOrderList };
            }
            return response;
        }

        /// <summary>
        /// Get the list Booking Office
        /// </summary>
        /// <returns>Office List</returns>
        public OfficeSummaryResponse GetBookingOffice()
        {
            var response = new OfficeSummaryResponse();
            //office list
            response.officeList = _office.GetOffices();
            //if (UserTypeEnum.InternalUser == _ApplicationContext.UserType)//logic for location configuration
            //{
            //    var _cusofficelist = _office.GetOfficesByUserId(_ApplicationContext.StaffId);
            //    response.officeList = _cusofficelist == null || _cusofficelist.Count() == 0 ? response.officeList : _cusofficelist;
            //}
            response.Result = OfficeSummaryResult.Success;
            return response;
        }
        /// <summary>
        /// To get customer related details by customer id for booking page.
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>CustomerDetails</returns>
        public async Task<BookingCustomerDetails> GetBookingDetailsByCustomerId(int customerId)
        {
            BookingCustomerDetails response = new BookingCustomerDetails();
            try
            {
                // customer brand list
                response.CustomerBrandList = await _customerManager.GetCustomerBrandsByUserId(customerId, _ApplicationContext.UserId);

                //customer department list
                response.CustomerDepartmentList = await _customerManager.GetCustomerDepartmentByUserId(customerId, _ApplicationContext.UserId);

                response.CustomerBuyerList = await _customerManager.GetCustomerBuyerByUserId(customerId);

                response.CustomerMerchandiserList = await _customerManager.GetCustomerMerchandiserById(customerId);

                //customer season list
                response.SeasonList = await _customerManager.GetCustomerSeason(customerId);
                if (response.SeasonList == null || response.SeasonList.Count() == 0)
                    response.SeasonList = await _referencemanager.GetSeasons();

                //supplier list
                var supplierlistresponse = await _suppliermanager.GetSuppliersByUserType(customerId, isBookingRequest: true);
                if (supplierlistresponse.Result == SupplierListResult.Success && supplierlistresponse.Data.Count() > 0)
                    response.SupplierList = supplierlistresponse.Data;
                else
                    return new BookingCustomerDetails() { Result = EditBookingResult.CannotGetSupplierList };

                //get customer collection
                response.Collection = await _customerManager.GetCustomerCollection(customerId);

                var priceCardResponse = await _customerManager.GetCustomerPriceCategory(customerId);

                //get customer price category
                if (priceCardResponse != null && priceCardResponse.Result == DataSourceResult.Success)
                {
                    response.PriceCategory = priceCardResponse.DataSourceList;
                }


                if (UserTypeEnum.Supplier == _ApplicationContext.UserType)
                {
                    var supresponse = await GetSupplierDetailsByCustomerIdSupplierId(customerId, _ApplicationContext.SupplierId, null);
                    if (supresponse?.Result == EditInspectionBookingResult.GetSupplierDetailsBySupplierCUstomerIdSuccess)
                    {
                        response.SupplierCode = supresponse?.SupplierCode;
                        response.SupplierContactList = supresponse?.SupplierContactList;

                    }
                }

                if (UserTypeEnum.Factory == _ApplicationContext.UserType)
                {
                    var factresponse = await GetFactoryDetailsByCustomerIdFactoryId(customerId, _ApplicationContext.FactoryId, null);
                    if (factresponse?.Result == EditInspectionBookingResult.GetFactoryDetailsByIdSuccess)
                    {
                        response.FactoryCode = factresponse?.FactoryCode;
                        response.FactoryContactList = factresponse?.FactoryContactList;
                        response.OfficeId = factresponse.OfficeId;
                    }
                }
                //take the booking default comments(qc booking comments)
                response.BookingDefaultComments = await _repo.GetCustomerBookingDefaulComments(customerId);

                response.Result = EditBookingResult.GetBookingDetailsByCustomerIdSuccess;
            }
            catch (Exception)
            {
                return new BookingCustomerDetails() { Result = EditBookingResult.CanotGetCustomerDetails };
            }
            return response;

        }

        /// <summary>
        /// To get specific inspection booking by inspection id
        /// </summary>
        /// <param name="inspectionId">inspectionId</param>
        /// <returns></returns>

        public async Task<GetInspectionResponse> GetInspection(int inspectionId)
        {
            try
            {
                var response = new GetInspectionResponse();
                var InspectionTransaction = await _repo.GetInspectionByID(inspectionId);
                if (InspectionTransaction == null)
                {
                    response.Result = InspectionResult.CannotGetInspection;
                    return response;
                }
                response.InspectionBookingDetails = _mapper.Map<InspectionBookingDetails>(InspectionTransaction);
                response.Result = InspectionResult.Success;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
        /// Get the quotation involved query
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private IQueryable<InspTransaction> GetQuotationDetails(InspectionSummarySearchRequest request, IQueryable<InspTransaction> bookingData)
        {
            //if quotation pending is true , fetch only booking with quotation check point configured 
            // status allocateqc,verified,confirmed,rescheduled not exists in quotation table
            // if the quotation pending is true from booking screen 
            if (request.IsQuotationSearch || (request.QuotationsStatusIdlst != null && request.QuotationsStatusIdlst.Any() && !request.QuotationsStatusIdlst.Any(x => x != (int)QuotationStatus.QuotationPending)))
            {
                //take only the customer has quotation required access
                bookingData = bookingData.Where(x => x.Customer.CuCheckPoints.Any(y => y.Active && y.ServiceId == (int)Service.InspectionId &&
                            y.CheckpointTypeId == (int)CheckPointTypeEnum.QuotationRequired &&
                            (!y.CuCheckPointsCountries.Any(a => a.Active) || (y.CuCheckPointsCountries.Any(a => a.Active) && x.Factory.SuAddresses.Any(z => z.AddressTypeId == (int)Supplier_Address_Type.HeadOffice && y.CuCheckPointsCountries.Where(a => a.Active).Select(b => b.CountryId).Contains(z.CountryId)))) &&
                            (!y.CuCheckPointsServiceTypes.Any(a => a.Active) || (y.CuCheckPointsServiceTypes.Any(a => a.Active) && x.InspTranServiceTypes.Any(z => z.Active && y.CuCheckPointsServiceTypes.Where(a => a.Active).Select(b => b.ServiceType.ServiceTypeId).Contains(z.ServiceTypeId))))
                            ));

                //check booking not invoiced
                bookingData = bookingData.Where(x => !x.InvAutTranDetails.Any(z => z.InvoiceStatus != (int)InvoiceStatus.Cancelled));

                //quotation not done for the booking
                bookingData = bookingData.Where(x => !x.QuQuotationInsps.Any(y => y.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled));

                //show quotation pending only in specific status(allocateqc,verifed,confirmed,rescheduled)
                bookingData = bookingData.Where(x => QuotationPendingBookingStatus.Contains(x.StatusId));

            }

            //Filter based on Quotation Status
            else if (request.QuotationsStatusIdlst != null && request.QuotationsStatusIdlst.Any())
            {
                bookingData = bookingData.Where(x => x.QuQuotationInsps.Any(y => request.QuotationsStatusIdlst.Contains(y.IdQuotationNavigation.IdStatus)));
            }

            return bookingData;

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
                     Priority = key.Priority,
                     StatusId = key.StatusId
                     // StatusColor = BookingSummaryInspectionStatusColor.GetValueOrDefault(key.StatusId, "")
                 }).OrderBy(x => x.Priority).ToListAsync();
        }

        /// <summary>
        /// execute the final booking list with pagination
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private async Task<List<InspectionBookingItems>> GetInspectionBookingData(IQueryable<InspTransaction> bookingData, int skip, int take)
        {
            return await bookingData.Select(x => new InspectionBookingItems
            {
                BookingId = x.Id,
                CustomerId = x.CustomerId,
                SupplierId = x.SupplierId,
                FactoryId = x.FactoryId,
                CustomerName = x.Customer.CustomerName,
                SupplierName = x.Supplier.SupplierName,
                FactoryName = x.Factory.SupplierName,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo,
                FirstServiceDateFrom = x.FirstServiceDateFrom,
                FirstServiceDateTo = x.FirstServiceDateTo,
                Office = x.Office.LocationName,
                OfficeId = x.OfficeId,
                StatusId = x.StatusId,
                StatusName = x.Status.Status,
                StatusPriority = x.Status.Priority,
                BookingCreatedBy = x.CreatedBy,
                PreviousBookingNo = x.PreviousBookingNo,
                ApplyDate = x.CreatedOn.GetValueOrDefault(),
                CustomerBookingNo = x.CustomerBookingNo,
                BookingAPiRemarks = x.ApiBookingComments,
                IsPicking = x.IsPickingRequired ?? false,
                IsEAQF = x.IsEaqf,
                PriceCategoryId = x.PriceCategoryId,
                PriceCategoryName = x.PriceCategory.Name,
                CollectionId = x.CollectionId,
                CollectionName = x.Collection.Name,
                UserTypeId = x.CreatedByNavigation.UserTypeId,
                IsCombineRequired = x.IsCombineRequired,
                CreatedByStaff = x.CreatedByNavigation.Staff.PersonName,
                CreatedByCustomer = x.CreatedByNavigation.CustomerContact.ContactName,
                CreatedBySupplier = x.CreatedByNavigation.SupplierContact.ContactName,
                CreatedByFactory = x.CreatedByNavigation.FactoryContact.ContactName,
                ProductCategory = x.ProductCategory.Name,
                ProductSubCategory = x.ProductSubCategory.Name,
                ProductType = x.ProductSubCategory2.Name,
                IsSameDayReport = x.IsSameDayReport,
                ReportRequest = x.ReportRequest,
                Instructions = x.CusBookingComments,
                CreatedOn = x.CreatedOn.ToString(),
                CreatedOnEaqf = x.CreatedOn.GetValueOrDefault().ToString(StandardISO8601DateTimeFormat),
                BookingType = x.BookingType

            }).OrderBy(x => x.ServiceDateFrom).Skip(skip).Take(take).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking data query by brand and dept
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private IQueryable<InspTransaction> GetInspectionByBrandAndDept(InspectionSummarySearchRequest request, IQueryable<InspTransaction> bookingData)
        {
            //apply brand filter directly if there is no AE Filter selected
            if ((request.UserIdList == null || !request.UserIdList.Any()) && request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
            {
                bookingData = bookingData.Where(x => x.InspTranCuBrands.Any(y => y.Active && request.SelectedBrandIdList.Contains(y.BrandId)));
            }
            //apply department filter directly if there is no AE Filter selected
            if ((request.UserIdList == null || !request.UserIdList.Any()) && request.SelectedDeptIdList != null && request.SelectedDeptIdList.Any())
            {
                bookingData = bookingData.Where(x => x.InspTranCuDepartments.Any(y => y.Active && request.SelectedDeptIdList.Contains(y.DepartmentId)));
            }

            return bookingData;
        }

        /// <summary>
        /// Get the booking data with the AE configured data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private async Task<IQueryable<InspTransaction>> GetAEConfiguredData(InspectionSummarySearchRequest request, IQueryable<InspTransaction> bookingData)
        {
            if (request.UserIdList != null && request.UserIdList.Any())
            {
                //user config list 
                var AEConfigList = await _userRepo.GetAECustomerConfigList(request?.UserIdList);

                if (AEConfigList != null && AEConfigList.Any())
                {
                    //select distinct customer from user config list
                    var AEConfiguredCustomerList = AEConfigList.Select(z => z.CustomerId.GetValueOrDefault()).Distinct().ToList();

                    if (AEConfiguredCustomerList.Any())
                    {
                        //take common customers between AE configured customer list and request customer list
                        if (request.CustomerList.Any())
                            AEConfiguredCustomerList = AEConfiguredCustomerList.Intersect(request.CustomerList).ToList();
                        //apply the customer list in the booking query
                        if (AEConfiguredCustomerList != null && AEConfiguredCustomerList.Any())
                            bookingData = bookingData.Where(x => AEConfiguredCustomerList.Contains(x.CustomerId));
                    }

                    //select ae configured dept list along with the customer
                    var AEConfiguredDeptCustomerList = AEConfigList.Where(x => AEConfiguredCustomerList.Contains(x.CustomerId.GetValueOrDefault()) &&
                                                       x.DepartmentId > 0).Select(x => new CustomerDeptData() { DeptId = x.DepartmentId, CustomerId = x.CustomerId }).ToList();

                    if (AEConfiguredDeptCustomerList != null && AEConfiguredDeptCustomerList.Any())
                    {
                        //get the ae configured department ids
                        var AEConfiguredDeptIds = AEConfiguredDeptCustomerList.Where(x => x.DeptId > 0).Select(x => x.DeptId.GetValueOrDefault()).Distinct().ToList();

                        //take common dept ids between AE Configured departments and request department list
                        if (request.SelectedDeptIdList != null && request.SelectedDeptIdList.Any())
                            AEConfiguredDeptIds = AEConfiguredDeptIds.Intersect(request.SelectedDeptIdList).ToList();

                        //get the department mapped customer list
                        var deptMappedCustomerList = AEConfiguredDeptCustomerList.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();

                        //get the customer list which not mapped to the department
                        var deptNotMappedCustomerList = AEConfiguredCustomerList.Where(x => !deptMappedCustomerList.Contains(x)).
                                                                                Select(x => x).Distinct().ToList();

                        //apply the ae configured dept list in the booking query
                        if (deptMappedCustomerList != null && deptMappedCustomerList.Any())

                            bookingData = bookingData.Where(x => (deptNotMappedCustomerList.Contains(x.CustomerId)) ||
                                        (x.InspTranCuDepartments.Any(y => y.Active && AEConfiguredDeptIds.Contains(y.DepartmentId)
                                         && deptMappedCustomerList.Contains(y.Inspection.CustomerId))));
                    }

                    //select ae configured brand list along with the customer
                    var AEConfiguredBrandCustomerList = AEConfigList.Where(x => AEConfiguredCustomerList.Contains(x.CustomerId.GetValueOrDefault()) &&
                                                        x.BrandId > 0).Select(x => new CustomerBrandData() { BrandId = x.BrandId, CustomerId = x.CustomerId }).ToList();

                    if (AEConfiguredBrandCustomerList != null && AEConfiguredBrandCustomerList.Any())
                    {
                        //get the ae configured brand ids
                        var AEConfiguredBrandIds = AEConfiguredBrandCustomerList.Where(x => x.BrandId > 0).Select(x => x.BrandId.GetValueOrDefault()).Distinct().ToList();

                        //take common brands between ae configured brand list and request customer list
                        if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
                            AEConfiguredBrandIds = AEConfiguredBrandIds.Intersect(request.SelectedBrandIdList).ToList();

                        //take the customer list where brand configured for the ae
                        var brandMappedCustomerList = AEConfiguredBrandCustomerList.Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();

                        //take the customer list where brand not configured for the ae
                        var brandNotMappedCustomerList = AEConfiguredCustomerList.Where(x => !brandMappedCustomerList.Contains(x)).
                                                                                Select(x => x).Distinct().ToList();
                        //apply the brand filter in the booking query
                        if (brandMappedCustomerList != null && brandMappedCustomerList.Any())

                            bookingData = bookingData.Where(x => (brandNotMappedCustomerList.Contains(x.CustomerId)) ||
                                        (x.InspTranCuBrands.Any(y => y.Active && AEConfiguredBrandIds.Contains(y.BrandId)
                                                    && brandMappedCustomerList.Contains(y.Inspection.CustomerId))));
                    }
                }
            }

            return bookingData;
        }

        private async Task<IEnumerable<CSConfigDetail>> GetAEConfigDetails(List<int?> distinctCustIds, List<int?> distinctOfficeIds, List<BookingDeptAccess> bookingDeptAccessList, List<BookingBrandAccess> bookingBrandAccessList)
        {


            //request a user access
            var userAccessFilter = new UserAccess
            {
                OfficeIds = distinctOfficeIds,
                CustomerIds = distinctCustIds,
                DepartmentIds = bookingDeptAccessList.Select(x => (int?)x.DeptId).Distinct(),
                BrandIds = bookingBrandAccessList.Select(x => (int?)x.BrandId).Distinct(),
                ServiceId = (int)Service.InspectionId
            };

            //get AE name with request config userAccessFilter
            var CSConfigList = await _userRepo.GetUserListByCusDeptBrandData(userAccessFilter);

            return CSConfigList;
        }

        /// <summary>
        /// Get the booking summary data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="IsExport"></param>
        /// <returns></returns>
        public async Task<BookingSummarySearchResponse> GetAllInspectionsData(InspectionSummarySearchRequest request, bool IsExport = false)
        {
            if (request == null)
                return new BookingSummarySearchResponse() { Result = BookingSummarySearchResponseResult.NotFound };

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

            //hide unannounced booking for supplier or factory login
            if (_ApplicationContext.UserType == UserTypeEnum.Supplier || _ApplicationContext.UserType == UserTypeEnum.Factory)
            {
                inspectionQuery = inspectionQuery.Where(x =>
                        (InspectedStatusList.Contains(x.StatusId) || x.BookingType != (int)InspectionBookingTypeEnum.UnAnnounced));
            }

            var inspectionQueryRequest = _sharedInspection.GetInspectionQueryRequestMap(request);

            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            //get the quotation involved query
            bookingData = GetQuotationDetails(request, bookingData);

            //get the ae configured data query
            bookingData = await GetAEConfiguredData(request, bookingData);

            bookingData = GetInspectionByBrandAndDept(request, bookingData);

            //assign the total count
            response.TotalCount = await bookingData.Select(x => x.Id).CountAsync();

            //execute the status list in the booking summary page
            var inspectionStatusList = await GetInspectionStatusList(bookingData);

            // Apply status color
            inspectionStatusList.ForEach(x => x.StatusColor = BookingSummaryInspectionStatusColor.GetValueOrDefault(x.StatusId, ""));

            //execute and get the booking detail data
            var inspectionBookingItems = await GetInspectionBookingData(bookingData, skip, take);


            if (inspectionBookingItems == null || !inspectionBookingItems.Any())
                return new BookingSummarySearchResponse() { Result = BookingSummarySearchResponseResult.NotFound };

            var bookingIds = inspectionBookingItems.Select(x => x.BookingId).ToList();

            //get the po details
            var poDetails = await _repo.GetBookingPOTransactionDetails(bookingIds);

            //Get the service Type for the bookings
            var serviceTypeList = await _repo.GetServiceType(bookingIds);

            //factory country required for pending quotation 
            var factoryCountryData = await _repo.GetFactorycountryId(bookingIds);

            var quotationDetails = new List<PoDetails>();
            quotationDetails = await _repo.GetBookingQuotationDetailsbybookingId(bookingIds);

            //get dept id and booking id by booking
            var bookingDeptAccessList = await _repo.GetDeptBookingIdsByBookingIds(bookingIds);

            //get brand id and booking id  by booking
            var bookingBrandAccessList = await _repo.GetBrandBookingIdsByBookingIds(bookingIds);

            //get buyer details and booking id  by booking
            var bookingBuyerAccessList = await _repo.GetBuyerBookingIdsByBookingIds(bookingIds);

            var containerList = await _repo.GetBookingContainer(bookingIds);

            //get  customer id list
            var distinctCusIdList = inspectionBookingItems.Where(x => x.CustomerId > 0).Select(x => x.CustomerId).Distinct().ToList();

            //get office id list
            var distinctOfficeIdList = inspectionBookingItems.Where(x => x.OfficeId > 0).Select(x => x.OfficeId).Distinct().ToList();

            //Get the CS Config Details
            var CSConfigList = await GetAEConfigDetails(distinctCusIdList, distinctOfficeIdList, bookingDeptAccessList, bookingBrandAccessList);

            //Get the booking report summary link
            List<BookingReportSummaryLinkRepo> bookingReportSummaryLink = null;

            var enumEntityName = (Company)_filterService.GetCompanyId();
            string entityName = enumEntityName.ToString().ToUpper();

            //get customer id list
            var customerIdList = inspectionBookingItems?.Where(x => x.CustomerId > 0).Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();

            //get supplier id list
            var supplierIdList = inspectionBookingItems?.Where(x => x.SupplierId > 0).Select(x => x.SupplierId.GetValueOrDefault()).Distinct().ToList();

            //get supplier code list
            var supplierCodeList = await _supplierManager.GetSupplierCode(customerIdList, supplierIdList);

            var _resultdata = inspectionBookingItems.Select(x => _bookingmap.GetInspectionSearchResult(x, inspectionStatusList, CSConfigList, _ApplicationContext.RoleList, (int)_ApplicationContext.UserType, poDetails, serviceTypeList, quotationDetails, factoryCountryData, bookingReportSummaryLink,
                                bookingBrandAccessList, bookingDeptAccessList, bookingBuyerAccessList, containerList, entityName, supplierCodeList));

            var _quotationStatusList = await _quotationRepository.GetStatusList();

            var _userRole = getBookingRole();
            return new BookingSummarySearchResponse()
            {
                Result = BookingSummarySearchResponseResult.Success,
                TotalCount = response.TotalCount,
                Index = request.Index.Value,
                PageSize = request.pageSize.Value,
                PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                Data = _resultdata,
                InspectionStatuslst = inspectionStatusList,
                QuotationStatuslst = _quotationStatusList.Select(_quotationmap.GetQuotationStatuswithColor),
                InternalUserRole = _userRole
            };
        }


        /// Get CS Names by customer,Office,brand,department
        public async Task<CSConfigResponse> getCSNames(UserAccess userAccess)
        {
            CSConfigResponse response = new CSConfigResponse();
            try
            {
                if (userAccess.CustomerId <= 0)
                {
                    response.Result = CSConfigResult.RequestNotValid;
                    return response;
                }

                var userAccessFilter = new UserAccess
                {
                    OfficeIds = userAccess.OfficeIds,
                    CustomerIds = new List<int?>() { userAccess.CustomerId },
                    DepartmentIds = userAccess.DepartmentIds,
                    BrandIds = userAccess.BrandIds,
                    ServiceId = userAccess.ServiceId
                };

                //get AE name with request config userAccessFilter
                var CSConfigList = await _userRepo.GetUserListByCusDeptBrandData(userAccessFilter);

                var CSNames = CSConfigList?.Select(x => x.CsName).Distinct().ToList();

                if (CSNames.Any())
                    response.CsName = string.Join(",", CSNames);

                response.Result = CSConfigResult.Success;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CSConfigListResponse> getCSList(UserAccess userAccess)
        {
            CSConfigListResponse response = new CSConfigListResponse();
            try
            {
                if (userAccess.CustomerId <= 0)
                {
                    response.Result = CSConfigListResult.RequestNotValid;
                    return response;
                }

                var userAccessFilter = new UserAccess
                {
                    OfficeIds = userAccess.OfficeIds,
                    CustomerIds = new List<int?>() { userAccess.CustomerId },
                    DepartmentIds = userAccess.DepartmentIds,
                    BrandIds = userAccess.BrandIds,
                    ServiceId = userAccess.ServiceId
                };

                //get AE name with request config userAccessFilter
                var CSConfigList = await _userRepo.GetUserListByCusDeptBrandData(userAccessFilter);

                var CSList = CSConfigList?.Select(x => new CommonDataSource()
                { Id = x.CsId, Name = x.CsName })
                                .Distinct().ToList();

                if (CSList.Any())
                    response.CsList = CSList;

                response.Result = CSConfigListResult.Success;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void CSListBasedCustomerLocation(int customerId, int locationId)
        {
            //_repo.CSListBasedCustomerLocation(customerId, locationId);
        }
        public async Task<ReportSummaryResponse> GetAllInspectionReportProducts(InspectionSummarySearchRequest request)
        {

            if (request == null)
                return new ReportSummaryResponse() { Result = ReportSummaryResponseResult.NotFound };

            var response = new ReportSummaryResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            //filter data based on user type
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        request.CustomerId = request?.CustomerId == null ? _ApplicationContext.CustomerId : 0;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        request.FactoryIdlst = request.FactoryIdlst != null && request.FactoryIdlst.Count() > 0 ?
                            request.FactoryIdlst : new List<int>().Append(_ApplicationContext.FactoryId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        request.SupplierId = request.SupplierId == null ? _ApplicationContext.SupplierId : 0;
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
                if (request?.CustomerId != null)
                    cuslist.Add(request.CustomerId.Value);
            }

            var fbReportInfoList = await _fbManager.GetFBReportDetails();

            var data = _repo.GetAllInspectionsReports();

            if (request != null && cuslist != null && cuslist.Count() > 0)
            {
                data = data.Where(x => cuslist.Contains(x.CustomerId));
            }

            if (request != null && request.SupplierId != 0 && request.SupplierId != null)
            {
                data = data.Where(x => x.SupplierId == request.SupplierId);
            }

            if (request != null && request.FactoryIdlst != null && request.FactoryIdlst.Count() > 0)
            {
                data = data.Where(x => x.FactoryId > 0 && request.FactoryIdlst.ToList().Contains(x.FactoryId.GetValueOrDefault()));
            }

            // only  QC and CS user 
            if (_ApplicationContext.UserProfileList.Contains(4))
            {
                var qcStaffList = await _humanResourceRepository.GetstaffsByParentIdwithQCProfile(_ApplicationContext.StaffId, true, 4);

                if (qcStaffList != null)
                {
                    var staffIDs = qcStaffList.Select(x => x.Id);
                    data = data.Where(x => x.SchScheduleQcs.Any(y => y.Active && staffIDs.Contains(y.Qcid)));
                }
            }
            //else if (_ApplicationContext.UserProfileList.Contains(2))
            //{
            //    data = data.Where(x => x.SchScheduleCS.Any(y => y.Active && y.Csid == _ApplicationContext.StaffId));
            //}

            if (request != null && request.Officeidlst != null && request.Officeidlst.Count() > 0 && data.Any(x => x.OfficeId != null))
            {
                data = data.Where(x => x.OfficeId != null && request.Officeidlst.ToList().Contains(x.OfficeId.Value));
            }

            if (request != null && request.StatusIdlst != null && request.StatusIdlst.Count() > 0)
            {
                data = data.Where(x => request.StatusIdlst.ToList().Contains(x.StatusId));
            }

            if (Enum.TryParse(request.SearchTypeId.ToString(), out SearchType _seachtypeenum))
            {
                switch (_seachtypeenum)
                {
                    case SearchType.BookingNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()) && int.TryParse(request.SearchTypeText?.Trim(), out int bookid))
                            {
                                data = data.Where(x => x.Id == bookid);
                            }
                            break;
                        }

                    case SearchType.PoNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()))
                            {
                                data = data.Where(x => x.InspPurchaseOrderTransactions.Where(z => z.Active.HasValue && z.Active.Value).
                                Any(y => y.Po != null && EF.Functions.Like(y.Po.Pono, $"%{request.SearchTypeText.Trim()}%")));
                            }
                            break;
                        }

                }
                if (Enum.TryParse(request.DateTypeid.ToString(), out SearchType _datesearchtype))
                {
                    switch (_datesearchtype)
                    {
                        case SearchType.ApplyDate:
                            {
                                if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                                {
                                    //data = data.Where(x => x.CreatedOn >= (request.FromDate.ToDateTime()) && x.CreatedOn <= (request.ToDate.ToDateTime()));
                                    data = data.Where(x => EF.Functions.DateDiffDay(request.FromDate.ToDateTime(), x.CreatedOn) >= 0 &&
                                                    EF.Functions.DateDiffDay(x.CreatedOn, request.ToDate.ToDateTime()) >= 0);
                                }
                                break;
                            }
                        case SearchType.ServiceDate:
                            {
                                if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                                {
                                    //  data = data.Where(x => x.ServiceDateFrom >= (request.FromDate.ToDateTime()) && x.ServiceDateTo <= (request.ToDate.ToDateTime()));
                                    data = data.Where(x => !((x.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.ServiceDateTo < request.FromDate.ToDateTime())));
                                }
                                break;
                            }
                    }
                }
            }

            response.TotalCount = data.Count();
            try
            {

                if (response.TotalCount == 0)
                {
                    response.Result = ReportSummaryResponseResult.NotFound;
                    return response;
                }

                // var statusItem=data.Select(x=>x.Status)

                var items = data.GroupBy(p => p.StatusId, p => p, (key, _data) =>
                new InspectionStatus
                {
                    Id = key,
                    StatusName = _data.Where(x => x.StatusId == key).Select(x => x.Status.Status).FirstOrDefault(),
                    Priority = _data.Where(x => x.StatusId == key).Select(x => x.Status.Priority).FirstOrDefault(),
                    TotalCount = _data.Count()
                }).OrderBy(x => x.Priority).ToList();

                var result = data.Skip(skip).Take(take).ToList();
                if (result == null || !result.Any())
                    return new ReportSummaryResponse() { Result = ReportSummaryResponseResult.NotFound };

                var _resultdata = result.Select(x => _bookingmap.GetInspectionReportResult(x, items, _ApplicationContext.StaffId, fbReportInfoList));

                var _statuslist = items.Select(x => _bookingmap.GetBookingStatuswithColor(x));
                var _userRole = getBookingRole();
                return new ReportSummaryResponse()
                {
                    Result = ReportSummaryResponseResult.Success,
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

        //get holiday list based on country,location. 
        public async Task<Boolean> IsHolidayExists(HolidayRequest request)
        {
            try
            {
                InspectionHolidaySummaryList _holidayDetails = new InspectionHolidaySummaryList();
                //get holiday + weekends list 
                _holidayDetails = await _inspBookingRuleContactManager.GetInspBookingHolidaysDate(request.FactoryCountryId, request.FactoryId);
                var _wdate = request.ServiceDateFrom.ToDateTime();
                var cancelRescheduleCondition = _holidayDetails?.Result == HolidayResult.Success ?
                    HolidayNotExists(request.ServiceDateFrom.ToDateTime(), _holidayDetails?.HolidaysDate) :
                    HolidayNotExists(request.ServiceDateFrom.ToDateTime(), null);
                //get boolean value for hide/show the cancel/reschedule button in inspection summary page based on holidays + weekends
                return cancelRescheduleCondition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //before 1 working day hide cance/reschedule button for external user. If that(before 1 working day) day holiday, get before date. 
        private Boolean HolidayNotExists(DateTime serviceDateFrom, IEnumerable<DateTime> holidayDates)
        {
            try
            {
                var _tdyDate = DateTime.Now.Date;
                if (serviceDateFrom.Date < _tdyDate)
                {
                    return false;
                }
                else
                {
                    //if before one working day(day before yesterday) from service from date - we have to hide the button
                    DateTime _loopPreviousDate = serviceDateFrom.AddDays(-2);
                    var filterPreviousDate = serviceDateFrom.AddDays(-1);
                    int loopExitCheck = 0;
                    int workingDayspan = 0;
                    //get holiday for loop today to service date(day before yesterday)
                    var _holidayDates = holidayDates != null ? holidayDates.Where(x => x.Date >= _tdyDate && x.Date <= filterPreviousDate) : null;
                    var _serviceDayMinusOne = _holidayDates != null && _holidayDates.Count() > 0 ? _holidayDates.Contains(filterPreviousDate) : false;
                    if (_holidayDates != null && _holidayDates.Count() > 0)
                    {
                        foreach (var item in _holidayDates.Select((value, i) => new { i, value }))
                        {
                            loopExitCheck = item.i;
                            if (_loopPreviousDate >= _tdyDate) //today date greater than previous date 
                            {
                                //previous date contains in holiday list true minus 1 day from previous date
                                var _tempDate = _holidayDates.Contains(_loopPreviousDate);
                                if (_tempDate)
                                {
                                    _loopPreviousDate = _loopPreviousDate.AddDays(-1);
                                    continue;
                                }
                                else
                                {
                                    if (_loopPreviousDate >= _tdyDate)
                                        break;
                                }
                            }
                            else
                                return false;
                        }
                    }
                    var _loopCount = loopExitCheck + 1;
                    //Get days of _loopPreviousDate sub from serviceDateFrom
                    var datespan = serviceDateFrom.Subtract(_loopPreviousDate).Days;
                    //Get working day from count of datespan and looping count 
                    var workingDay = _serviceDayMinusOne ? (datespan - _loopCount) - 1 : (datespan - _loopCount);
                    //Get workingday count before service date. If less than 1 means false the return
                    workingDayspan = workingDayspan >= 1 ? workingDayspan : workingDay;

                    if ((_loopPreviousDate < _tdyDate) || (_loopPreviousDate < _tdyDate.AddDays(1) && workingDayspan < 1 &&
                         _holidayDates != null && _holidayDates.Count() > 0 && (_holidayDates.Count() == _loopCount)))
                    {
                        return false; //show the cancel/reshedule popup 
                    }
                    else
                    {
                        return true; //show the cancel/reshedule page
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get factory details by Customer id and Factory id
        /// </summary>
        /// <param name="cusid"></param>
        /// <param name="factid"></param>
        /// <returns></returns>
        public async Task<EditInspectionFactDetails> GetFactoryDetailsByCustomerIdFactoryId(int? cusid, int factid, int? bookingId)
        {
            EditInspectionFactDetails response = new EditInspectionFactDetails();
            try
            {
                var factdetails = await _suppliermanager.GetSupplierOrFactoryDetails(factid);
                var office = await _office.GetOfficeByFactoryid(factid);

                //factory details
                if (factdetails == null)
                    return new EditInspectionFactDetails() { Result = EditInspectionBookingResult.CannotGetFactoryDetails };
                else
                {
                    var factaddress = factdetails?.SupplierDetails?.AddressList.FirstOrDefault();

                    response.FactoryAddress = factaddress?.Way;
                    response.FactoryRegionalAddress = factaddress?.LocalLanguage;
                    response.PhoneNumber = factdetails?.SupplierDetails?.Phone;
                    var factcountryid = factdetails.SupplierDetails.AddressList.Select(x => x.CountryId).FirstOrDefault();
                    // get holidays
                    if (office != null)
                    {
                        response.OfficeId = office.Id;
                        // cs details

                    }

                }
                if (_ApplicationContext.UserType == UserTypeEnum.Factory)
                {
                    //supplier list
                    var supplierlistresponse = await _suppliermanager.GetSuppliersByUserType(cusid);
                    if (supplierlistresponse.Result == SupplierListResult.Success && supplierlistresponse.Data.Count() > 0)
                        response.SupplierList = supplierlistresponse.Data;
                    else
                        return new EditInspectionFactDetails() { Result = EditInspectionBookingResult.CannotGetSupplierList };
                }
                if (cusid != null)
                {
                    //factory code
                    response.FactoryCode = factdetails?.SupplierDetails.CustomerList?.Where(x => x.Id == cusid).Select(x => x.Code).FirstOrDefault();

                    //factory contact list
                    response.FactoryContactList = await _suppliermanager.GetFactoryContactsById(factid, cusid.Value);
                    if (response.FactoryContactList == null || response.FactoryContactList.Count() == 0)
                        return new EditInspectionFactDetails() { Result = EditInspectionBookingResult.CannotGetFactoryContactList };
                }
                //fetch the inactive factory contact when edit booking
                if (bookingId > 0)
                {
                    //get the booking involved inactive factory contacts
                    var bookingInactiveFactoryContacts = await _repo.GetEditBookingFactoryContacts(bookingId);
                    //append the inactive list to master list
                    response.FactoryContactList = response.FactoryContactList.Concat(bookingInactiveFactoryContacts);
                    //take distinct value
                    response.FactoryContactList = response.FactoryContactList.Distinct().ToList();
                }

                response.Result = EditInspectionBookingResult.GetFactoryDetailsByIdSuccess;
            }
            catch (Exception ex)
            {
                return new EditInspectionFactDetails() { Result = EditInspectionBookingResult.CannotGetFactoryDetails };
            }
            return response;
        }
        /// <summary>
        /// Get supplier details by Customer id and Supplier id
        /// </summary>
        /// <param name="cusid"></param>
        /// <param name="supid"></param>
        /// <returns></returns>
        public async Task<EditInspectionBookingSupDetails> GetSupplierDetailsByCustomerIdSupplierId(int? cusid, int supid, int? bookingId)
        {
            EditInspectionBookingSupDetails response = new EditInspectionBookingSupDetails();

            try
            {

                //factory list
                var factlist = await _suppliermanager.GetFactorysByUserType(cusid, supid);
                if (factlist.Result == SupplierListResult.Success && factlist.Data.Count() > 0)
                    response.FactoryList = factlist.Data;

                var supplierDetails = await _supplierRepo.GetSupplierDetailById(supid);
                response.SupplierPhoneNumber = supplierDetails?.Phone;

                if (cusid != null)
                {
                    var supcode = await _suppliermanager.GetSupplierCode(supid, cusid.Value);
                    response.SupplierCode = supcode == null ? "" : supcode;

                    //supplier contact list
                    response.SupplierContactList = await _suppliermanager.GetSupplierContactsById(supid, cusid.Value);
                    if (response.SupplierContactList == null || response.SupplierContactList.Count() == 0)
                        return new EditInspectionBookingSupDetails() { Result = EditInspectionBookingResult.CannotGetSupplierContactList };
                    //fetch the inactive supplier contact when edit booking
                    if (bookingId > 0)
                    {
                        //fetch the booking involved inactive supplier contacts
                        var bookingInactiveSupplierContacts = await _repo.GetEditBookingSupplierContacts(bookingId);
                        //append the inactive contacts to master list
                        response.SupplierContactList = response.SupplierContactList.Concat(bookingInactiveSupplierContacts);
                        //take only distinct for safety check
                        response.SupplierContactList = response.SupplierContactList.Distinct().ToList();
                    }
                }

                response.Result = EditInspectionBookingResult.GetSupplierDetailsBySupplierCUstomerIdSuccess;
            }
            catch (Exception ex)
            {
                return new EditInspectionBookingSupDetails() { Result = EditInspectionBookingResult.CannotGetSupplierDetails };
            }
            return response;
        }

        /// <summary>
        /// Add the po and container transactiosn
        /// </summary>
        /// <param name="poList"></param>
        /// <param name="productList"></param>
        /// <param name="entity"></param>
        /// <param name="inspectionProductDetail"></param>
        /// <param name="serviceTypeId"></param>
        private void AddInspectionPoTransactions(List<SaveInspectionPOProductDetails> poList, List<SaveInspectionPOProductDetails> productList, InspTransaction bookingEntity, InspProductTransaction inspectionProductDetail, int serviceTypeId, int? userId)
        {
            foreach (var poItem in poList)
            {
                var inspectionPODetail = _bookingmap.MapProductPOTransaction(poItem);

                inspectionPODetail.CreatedBy = userId;
                inspectionPODetail.EntityId = _filterService.GetCompanyId();

                AddInspectionContainerTransaction(poItem, productList, serviceTypeId, bookingEntity, inspectionPODetail);

                inspectionProductDetail.InspPurchaseOrderTransactions.Add(inspectionPODetail);

                bookingEntity.InspPurchaseOrderTransactions.Add(inspectionPODetail);

                if (bookingEntity.IsPickingRequired.GetValueOrDefault())
                    //Save the inspection picking transactions
                    SaveInspectionPickingTransactions(poItem.SaveInspectionPickingList, bookingEntity, inspectionPODetail);

                _repo.AddEntity(inspectionPODetail);

            }
        }

        /// <summary>
        /// Save the inspection picking transactions
        /// </summary>
        /// <param name="pickingList"></param>
        /// <param name="bookingEntity"></param>
        /// <param name="inspectionPODetail"></param>
        private void SaveInspectionPickingTransactions(List<SaveInspectionPickingDetails> pickingList,
                        InspTransaction bookingEntity, InspPurchaseOrderTransaction inspectionPODetail)
        {
            //loop through the picking request data

            if (pickingList != null && pickingList.Any())
            {
                foreach (var pickingRequestData in pickingList)
                {
                    //map the inspection picking data
                    var pickingTransactionData = _bookingmap.MapInspectionPicking(pickingRequestData, _ApplicationContext.UserId);

                    //map the inspection picking contacts
                    foreach (var contact in pickingRequestData.PickingContactList)
                    {
                        var inspTranPickingContact = _bookingmap.MapInspectionPickingContact(pickingRequestData.LabType, contact.LabOrCustomerContactId, _ApplicationContext.UserId);
                        pickingTransactionData.InspTranPickingContacts.Add(inspTranPickingContact);
                    }
                    //add picking transaction data with booking entity
                    bookingEntity.InspTranPickings.Add(pickingTransactionData);
                    //add picking transaction data with inspection po detail entity
                    inspectionPODetail.InspTranPickings.Add(pickingTransactionData);
                }
            }
        }

        /// <summary>
        /// Add Inspection Po and Product Transaction on edititng product
        /// </summary>
        /// <param name="poDetail"></param>
        /// <param name="productList"></param>
        /// <param name="bookingEntity"></param>
        /// <param name="inspectionProductDetail"></param>
        /// <param name="serviceTypeId"></param>
        private void AddInspectionPoTransactionsOnEditProduct(SaveInspectionPOProductDetails poDetail, List<SaveInspectionPOProductDetails> productList, InspTransaction bookingEntity, InspProductTransaction inspectionProductDetail, int serviceTypeId)
        {

            var inspectionPODetail = _bookingmap.MapProductPOTransaction(poDetail);
            inspectionPODetail.CreatedBy = _ApplicationContext.UserId;
            inspectionPODetail.EntityId = _filterService.GetCompanyId();

            //save the inspection picking transactions
            if (bookingEntity.IsPickingRequired.GetValueOrDefault())
                SaveInspectionPickingTransactions(poDetail.SaveInspectionPickingList, bookingEntity, inspectionPODetail);

            AddInspectionContainerTransaction(poDetail, productList, serviceTypeId, bookingEntity, inspectionPODetail);

            inspectionProductDetail.InspPurchaseOrderTransactions.Add(inspectionPODetail);

            bookingEntity.InspPurchaseOrderTransactions.Add(inspectionPODetail);

            _repo.AddEntity(inspectionPODetail);

        }

        private void AddInspectionContainerTransaction(SaveInspectionPOProductDetails poDetail, List<SaveInspectionPOProductDetails> productList, int serviceTypeId, InspTransaction bookingEntity, InspPurchaseOrderTransaction inspectionPODetail)
        {
            int containerTotalBookingQty = 0;

            //if booking is container service and container not available in the container transactions
            if (ContainerServiceList.Contains(serviceTypeId) && poDetail.ContainerId > 0 && !bookingEntity.InspContainerTransactions.
                            Any(x => x.ContainerId == poDetail.ContainerId && x.Active.HasValue && x.Active.Value))
            {
                containerTotalBookingQty = productList.Where(x => x.ContainerId == poDetail.ContainerId).Sum(x => x.BookingQuantity);

                var containerTransaction = _bookingmap.MapContainerTransaction(poDetail.ContainerId.GetValueOrDefault(), containerTotalBookingQty);
                containerTransaction.EntityId = _filterService.GetCompanyId();
                containerTransaction.CreatedBy = _ApplicationContext.UserId;

                bookingEntity.InspContainerTransactions.Add(containerTransaction);

                _repo.AddEntity(containerTransaction);

            }

            //assign container id if service type is container
            if (ContainerServiceList.Contains(serviceTypeId))
                inspectionPODetail.ContainerRef = bookingEntity.InspContainerTransactions.
                                           FirstOrDefault(x => x.ContainerId == poDetail.ContainerId && x.Active.HasValue && x.Active.Value);
        }

        /// <summary>
        /// Add Inspection po and color transaction for the exiting product
        /// </summary>
        /// <param name="poDetail"></param>
        /// <param name="entity"></param>
        /// <param name="inspectionProductDetail"></param>
        private void AddInspectionPoAndColorTransactionOnEditProduct(SaveInspectionPOProductDetails poDetail, InspProductTransaction inspectionProductDetail, InspTransaction bookingEntity, List<SaveInspectionPOProductDetails> poColorList)
        {
            //add po transaction
            var inspectionPODetail = _bookingmap.MapProductPOForColorTransaction(poDetail, poColorList);
            inspectionPODetail.CreatedBy = _ApplicationContext.UserId;
            inspectionPODetail.EntityId = _filterService.GetCompanyId();

            //save inspection picking transactions
            if (bookingEntity.IsPickingRequired.GetValueOrDefault())
                SaveInspectionPickingTransactions(poDetail.SaveInspectionPickingList, bookingEntity, inspectionPODetail);

            //add color transaction
            var poColorDetail = _bookingmap.MapPOColorTransaction(poDetail);
            poColorDetail.CreatedBy = _ApplicationContext.UserId;
            poColorDetail.EntityId = _filterService.GetCompanyId();

            inspectionProductDetail.InspPurchaseOrderColorTransactions.Add(poColorDetail);
            inspectionPODetail.InspPurchaseOrderColorTransactions.Add(poColorDetail);

            inspectionProductDetail.InspPurchaseOrderTransactions.Add(inspectionPODetail);
            bookingEntity.InspPurchaseOrderTransactions.Add(inspectionPODetail);
            _repo.AddEntity(poColorDetail);
        }

        /// <summary>
        /// Add the color transaction for the exiting product and po data
        /// </summary>
        /// <param name="colorDetail"></param>
        /// <param name="inspectionPODetail"></param>
        /// <param name="inspectionProductDetail"></param>
        private void AddInspectionColorTransactionOnEditProduct(SaveInspectionPOProductDetails colorDetail, InspPurchaseOrderTransaction inspectionPODetail, InspProductTransaction inspectionProductDetail)
        {
            //add color transaction
            var poColorDetail = _bookingmap.MapPOColorTransaction(colorDetail);
            poColorDetail.CreatedBy = _ApplicationContext.UserId;
            poColorDetail.EntityId = _filterService.GetCompanyId();

            inspectionProductDetail.InspPurchaseOrderColorTransactions.Add(poColorDetail);
            inspectionPODetail.InspPurchaseOrderColorTransactions.Add(poColorDetail);
            _repo.AddEntity(poColorDetail);
        }



        /// <summary>
        /// add the inspection po and color transactions
        /// </summary>
        /// <param name="poColorList"></param>
        /// <param name="entity"></param>
        /// <param name="inspectionProductDetail"></param>
        private void AddInspectionPoAndColorTransactions(List<SaveInspectionPOProductDetails> poColorList, InspProductTransaction inspectionProductDetail, InspTransaction bookingEntity)
        {
            foreach (var poItem in poColorList)
            {
                //add po only if not exists in the purchase order transaction(since po and product will be duplicated for the color transaction in the request list)
                if (!(inspectionProductDetail.InspPurchaseOrderTransactions.Any(x => x.PoId == poItem.PoId && x.Active.HasValue && x.Active.Value)))
                {
                    //map the purchase order transaction
                    var inspectionPODetail = _bookingmap.MapProductPOForColorTransaction(poItem, poColorList);
                    inspectionPODetail.CreatedBy = inspectionProductDetail.CreatedBy;
                    inspectionPODetail.EntityId = _filterService.GetCompanyId();

                    //Save the inspection picking transactions
                    if (bookingEntity.IsPickingRequired.GetValueOrDefault())
                        SaveInspectionPickingTransactions(poItem.SaveInspectionPickingList, bookingEntity, inspectionPODetail);

                    //take the color info belongs to the po
                    var colorList = poColorList.Where(x => x.PoId == poItem.PoId).ToList();

                    //add the color transaction
                    if (colorList != null && colorList.Any())
                    {
                        foreach (var colorItem in colorList)
                        {
                            //map the po color transaction
                            var poColorDetail = _bookingmap.MapPOColorTransaction(colorItem);
                            poColorDetail.CreatedBy = _ApplicationContext.UserId;
                            poColorDetail.EntityId = _filterService.GetCompanyId();

                            inspectionProductDetail.InspPurchaseOrderColorTransactions.Add(poColorDetail);
                            inspectionPODetail.InspPurchaseOrderColorTransactions.Add(poColorDetail);
                            _repo.AddEntity(poColorDetail);
                        }
                    }

                    inspectionProductDetail.InspPurchaseOrderTransactions.Add(inspectionPODetail);

                    bookingEntity.InspPurchaseOrderTransactions.Add(inspectionPODetail);

                    _repo.AddEntity(inspectionPODetail);
                }
            }
        }

        /// <summary>
        /// Add/Update the booking product serial no
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        /// <param name="inspectionProductDetail"></param>
        private void UpdateBookingProductSerial(SaveInsepectionRequest request, InspTransaction bookingEntity, InspProductTransaction inspectionProductDetail)
        {
            inspectionProductDetail.BookingFormSerial = bookingEntity.InspProductTransactions.Select(x => x.BookingFormSerial).Max().GetValueOrDefault() + 1;
        }


        /// <summary>
        /// Map the product to the purchase order detail if it is not mapped
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task MapProductToPurchaseOrder(SaveInsepectionRequest request)
        {
            //get the non mapped product list
            var poProductNotMappedDataList = await GetPoProductNotMappedData(request);

            poProductNotMappedDataList = poProductNotMappedDataList.ToList();

            List<CuPurchaseOrder> purchaseOrderEntitiesToEdit = new List<CuPurchaseOrder>();

            //if any product is not mapped
            if (poProductNotMappedDataList.Any())
            {
                //get the poids
                var poList = poProductNotMappedDataList.Select(x => x.poId).Distinct().ToList();

                //get the purchse orders
                var purchaseOrders = await _poRepo.GetPurchaseOrderByPoIds(poList);

                if (purchaseOrders.Any())
                {
                    foreach (var poProductData in poProductNotMappedDataList)
                    {
                        //map the purchase order details
                        var purchaseOrder = PurchaseOrderDetailMap(purchaseOrders, poProductData, request);

                        purchaseOrderEntitiesToEdit.Add(purchaseOrder);

                    }
                    //edit the purchase order entities
                    _repo.EditEntities(purchaseOrderEntitiesToEdit);

                }


            }

        }

        /// <summary>
        /// add supplier factory to purchase order on booking
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task AddSupplierOrFactoryToPurchaseOrderOnBooking(SaveInsepectionRequest request)
        {
            var poIdList = request.InspectionProductList.Select(x => x.PoId).Distinct().ToList();
            //get the po list with supplier or factory
            var poList = await _poRepo.GetPurchaseOrderSupplierFactoryByPoIds(poIdList);

            //add supplier or factory
            foreach (var poData in poList)
            {
                if (request.SupplierId > 0)
                    AddPoSuppliers(new[] { request.SupplierId }.ToList(), poData, _ApplicationContext.UserId);

                if (request.FactoryId > 0)
                    AddPoFactories(new[] { request.FactoryId.GetValueOrDefault() }.ToList(), poData, _ApplicationContext.UserId);

            }

        }

        /// <summary>
        /// Map the purchase order detail
        /// </summary>
        /// <param name="purchaseOrders"></param>
        /// <param name="poProductData"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        private CuPurchaseOrder PurchaseOrderDetailMap(List<CuPurchaseOrder> purchaseOrders, PoProductData poProductData, SaveInsepectionRequest request)
        {
            var purchaseOrderDetail = new CuPurchaseOrderDetail();

            var purchaseOrder = purchaseOrders.FirstOrDefault(x => x.Id == poProductData.poId);

            if (!purchaseOrder.CuPurchaseOrderDetails.Any(x => x.ProductId == poProductData.productId))
            {
                var poProductList = request.InspectionProductList.Where(x => x.PoId == poProductData.poId &&
                          x.ProductId == poProductData.productId).ToList();

                if (poProductList != null)
                {
                    purchaseOrderDetail.Quantity = poProductList.Sum(x => x.BookingQuantity);
                    var poProductRequestData = poProductList.FirstOrDefault();
                    if (poProductRequestData != null)
                    {
                        purchaseOrderDetail.Etd = poProductRequestData.Etd?.ToNullableDateTime();
                        purchaseOrderDetail.DestinationCountryId = poProductRequestData.DestinationCountryID;
                    }
                }

                if (purchaseOrder != null)
                {
                    purchaseOrderDetail.ProductId = poProductData.productId;
                    //purchaseOrderDetail.SupplierId = request.SupplierId;
                    //purchaseOrderDetail.FactoryId = request.FactoryId;
                    purchaseOrderDetail.EntityId = _filterService.GetCompanyId();
                    purchaseOrderDetail.Active = true;
                    purchaseOrderDetail.CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    purchaseOrderDetail.CreatedTime = DateTime.Now;
                    purchaseOrderDetail.BookingStatus = (int)BookingProductStatus.Full;

                    purchaseOrder.CuPurchaseOrderDetails.Add(purchaseOrderDetail);
                }
            }

            return purchaseOrder;
        }

        /// <summary>
        /// Get the po product which is mapped in the purchase order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<List<PoProductData>> GetPoProductMappedData(SaveInsepectionRequest request)
        {
            //get the po product list
            var poProductRequestData = request.InspectionProductList.Select(x => new PoProductData()
            { poId = x.PoId, productId = x.ProductId }).ToList();

            //get the po list
            var poList = poProductRequestData.Select(x => x.poId).Distinct().ToList();

            //generate the search po request
            var bookingPoSearchData = new BookingPoSearchData();

            bookingPoSearchData.PoList = poList;
            bookingPoSearchData.CustomerId = request.CustomerId;
            bookingPoSearchData.SupplierId = request.SupplierId;

            //get the po data by po id,customerid,supplierid
            return await _repo.GetPoProductDataByPoAndCustomer(bookingPoSearchData);
        }

        private async Task<List<PoProductData>> GetPoProductNotMappedData(SaveInsepectionRequest request)
        {

            var poProductNotMappedDataList = new List<PoProductData>();

            if (request.InspectionProductList != null && request.InspectionProductList.Any())
            {
                //get the po product list from ther request data
                var poProductRequestData = request.InspectionProductList.Select(x => new PoProductData()
                { poId = x.PoId, productId = x.ProductId }).Distinct().ToList();

                //get the po product mapped data with the purchase order
                var poProductMappedData = await GetPoProductMappedData(request);


                //if some po product mapped with purchase order detail
                if (poProductMappedData.Any())
                {
                    //loop through the request data and push the po,product which is not available in the db
                    foreach (var poProductData in poProductRequestData)
                    {
                        if (!poProductMappedData.Any(x => x.poId == poProductData.poId
                                         && x.productId == poProductData.productId))
                        {
                            poProductNotMappedDataList.Add(poProductData);
                        }
                    }
                }
                //if no po product mapped then push all the data into non mapped list
                else
                {
                    poProductNotMappedDataList.AddRange(poProductRequestData);
                }

            }

            return poProductNotMappedDataList;
        }


        /// <summary>
        /// Add Inspection Product,Po,Color,Container Transaction Data
        /// Flow1->Product Transaction->PO Transaction->Color Transaction
        /// Flow2->Product Transaction->Po Transaction<-Container Transaction
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        private async Task AddInspectionPOProductList(SaveInsepectionRequest request, InspTransaction bookingEntity)
        {
            #region Variable Declaration/Initialization
            var inspectionPOProductList = request.InspectionProductList.Where(x => x.Id == 0).ToList();
            #endregion
            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
            if (inspectionPOProductList.Any())
            {
                var workLoadMatrixList = await GetWorkLoadMatrixData(inspectionPOProductList);

                //loop through the po product details
                foreach (var productItem in inspectionPOProductList)
                {
                    //add the product data if not availble in the product transactions
                    if (!bookingEntity.InspProductTransactions.Any(x => x.ProductId == productItem.ProductId && x.Active.HasValue && x.Active.Value))
                    {
                        //calculate the total booking qty for the products
                        var totalBookingQty = inspectionPOProductList.Where(x => x.ProductId == productItem.ProductId).
                                   Sum(x => x.BookingQuantity);

                        // Map the inspection Product details
                        var inspectionProductDetail = _bookingmap.MapAddProductTransactionEntity(productItem, totalBookingQty);
                        inspectionProductDetail.EntityId = _filterService.GetCompanyId();
                        inspectionProductDetail.CreatedBy = userId;

                        //update the booking product serial number
                        UpdateBookingProductSerial(request, bookingEntity, inspectionProductDetail);

                        //update aql quantity if booking not in service type
                        if (!ContainerServiceList.Contains(request.ServiceTypeId) && !productItem.IsDisplayMaster)
                        {
                            UpdateAQLQuantity(request, productItem, inspectionProductDetail, totalBookingQty);
                        }

                        //take the product file attachments
                        var productFileAttachments = request.InspectionProductList.Where(x => x.ProductId == productItem.ProductId).SelectMany(x => x.ProductFileAttachments).ToList();

                        UpdateProductRelatedData(productItem, bookingEntity, workLoadMatrixList);

                        //add or update product file attachment 
                        //UpdateProductFileAttachments(productItem, entity);

                        //get po details belongs to the product
                        var poList = request.InspectionProductList.Where(x => x.ProductId == productItem.ProductId).ToList();

                        //if po has color transactions
                        if (request.BusinessLine == (int)BusinessLine.SoftLine && poList.Any(x => !string.IsNullOrEmpty(x.ColorCode) && !string.IsNullOrEmpty(x.ColorName)))
                        {
                            AddInspectionPoAndColorTransactions(poList, inspectionProductDetail, bookingEntity);
                        }
                        // other than color transactions
                        else if (poList != null && poList.Any())
                        {

                            AddInspectionPoTransactions(poList, request.InspectionProductList, bookingEntity, inspectionProductDetail, request.ServiceTypeId, userId);
                        }

                        bookingEntity.InspProductTransactions.Add(inspectionProductDetail);

                    }
                    //on edit case add po,color,container for existing product
                    else if (request.Id > 0 && bookingEntity.InspProductTransactions.Any(x => x.ProductId == productItem.ProductId))
                    {
                        if (productItem.PoTransactionId <= 0)
                        {
                            //take the inspection Product data
                            var entityProductData = bookingEntity.InspProductTransactions.FirstOrDefault(x => x.ProductId == productItem.ProductId && x.Active.HasValue && x.Active.Value);

                            //color code and color name exists
                            if (request.BusinessLine == (int)BusinessLine.SoftLine && !string.IsNullOrEmpty(productItem.ColorCode)
                                    && !string.IsNullOrEmpty(productItem.ColorName))
                            {
                                //if po is avaialble and color data is not available then only the color transaction
                                if ((entityProductData.InspPurchaseOrderTransactions.Any(x => x.PoId == productItem.PoId
                                        && x.Active.HasValue && x.Active.Value
                                     && !x.InspPurchaseOrderColorTransactions.Any(y => y.ColorCode == productItem.ColorCode
                                             && y.ColorName == productItem.ColorName && y.Active.HasValue && y.Active.Value))))
                                {
                                    var inspectionPODetail = entityProductData.InspPurchaseOrderTransactions.FirstOrDefault(x => x.PoId == productItem.PoId);
                                    AddInspectionColorTransactionOnEditProduct(productItem, inspectionPODetail, entityProductData);
                                }
                                //if po and color transaction not available for the product then add both po and color transaction
                                else if (!(entityProductData.InspPurchaseOrderTransactions.Any(x => x.PoId == productItem.PoId
                                     && x.Active.HasValue && x.Active.Value
                                     && x.InspPurchaseOrderColorTransactions.Any(y => y.ColorCode == productItem.ColorCode
                                             && y.ColorName == productItem.ColorName && y.Active.HasValue && y.Active.Value))))
                                {
                                    var poColorList = inspectionPOProductList.Where(x => x.PoId == productItem.PoId).ToList();

                                    AddInspectionPoAndColorTransactionOnEditProduct(productItem, entityProductData, bookingEntity, poColorList);
                                }
                            }
                            //if service type is container service and po and container not exists then add
                            else if (ContainerServiceList.Contains(request.ServiceTypeId) && !(entityProductData.InspPurchaseOrderTransactions.Any(x => x.PoId == productItem.PoId && x.ContainerRef.ContainerId == productItem.ContainerId
                                                                            && x.Active.HasValue && x.Active.Value)))
                            {
                                AddInspectionPoTransactionsOnEditProduct(productItem, request.InspectionProductList.ToList(), bookingEntity, entityProductData, request.ServiceTypeId);
                            }
                            //normal flow
                            else if (!(entityProductData.InspPurchaseOrderTransactions.Any(x => x.PoId == productItem.PoId && x.Active.HasValue && x.Active.Value)))
                            {
                                AddInspectionPoTransactionsOnEditProduct(productItem, request.InspectionProductList.ToList(), bookingEntity, entityProductData, request.ServiceTypeId);
                            }
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Get the workload matrix data
        /// </summary>
        /// <param name="inspectionPOProductList"></param>
        /// <returns></returns>
        private async Task<List<WorkLoadMatrixData>> GetWorkLoadMatrixData(List<SaveInspectionPOProductDetails> inspectionPOProductList)
        {
            var productSubCategory3Ids = inspectionPOProductList.Where(x => x.ProductCategorySub3Id != null).
                                   Select(x => x.ProductCategorySub3Id.Value).Distinct().ToList();

            return await _workLoadMatrixRepository.GetWorkLoadMatrixByProductCatSub3List(productSubCategory3Ids);
        }

        /// <summary>
        /// Update Product Related Data
        /// </summary>
        /// <param name="productData"></param>
        /// <param name="bookingEntity"></param>
        /// <param name="productAttachmentList"></param>
        /// <returns></returns>
        private void UpdateProductRelatedData(SaveInspectionPOProductDetails productData, InspTransaction bookingEntity, List<WorkLoadMatrixData> workLoadMatrixList)
        {

            var entityProduct = _productRepo.GetCustomerProductById(productData.ProductId);

            if (entityProduct != null)
            {
                UpdateProductBaseData(productData, entityProduct, workLoadMatrixList);

                RemoveDBFileAttachmentNotAvailableInRequest(entityProduct, productData, productData.ProductFileAttachments);

                MapProductFileAttachment(entityProduct, productData, bookingEntity);

                _repo.EditEntity(entityProduct);
            }
        }

        /// <summary>
        /// Map the Product category to inspection
        /// </summary>
        /// <param name="productData"></param>
        /// <param name="bookingEntity"></param>
        private void UpdateProductCategoryDataToInspection(SaveInspectionPOProductDetails productData, InspTransaction bookingEntity)
        {
            if (productData != null)
            {
                bookingEntity.ProductCategoryId = productData.ProductCategoryId;
                bookingEntity.ProductSubCategoryId = productData.ProductSubCategoryId;
                bookingEntity.ProductSubCategory2Id = productData.ProductCategorySub2Id;
            }
        }

        /// <summary>
        /// Update the product base data in the booking
        /// </summary>
        /// <param name="productData"></param>
        /// <param name="EntityProduct"></param>
        private void UpdateProductBaseData(SaveInspectionPOProductDetails productData, CuProduct EntityProduct, List<WorkLoadMatrixData> workLoadMatrixList)
        {
            EntityProduct.ProductCategory = productData.ProductCategoryId;
            EntityProduct.ProductSubCategory = productData.ProductSubCategoryId;
            EntityProduct.ProductCategorySub2 = productData.ProductCategorySub2Id;
            EntityProduct.Barcode = productData.Barcode;
            EntityProduct.FactoryReference = productData.FactoryReference;

            EntityProduct.ProductCategorySub3 = productData.ProductCategorySub3Id;
            if (productData.ProductCategorySub3Id != null)
            {
                var workLoadMatrixData = workLoadMatrixList.FirstOrDefault(x => x.ProdCategorySub3Id == productData.ProductCategorySub3Id);
                if (workLoadMatrixData != null)
                {
                    EntityProduct.TimePreparation = workLoadMatrixData.PreparationTime;
                    EntityProduct.SampleSize8h = workLoadMatrixData.EightHourSampleSize;
                }
            }

            //EntityProduct.TimePreparation
        }

        /// <summary>
        /// Remove the product files not available in the request
        /// </summary>
        /// <param name="EntityProduct"></param>
        /// <param name="productData"></param>
        /// <param name="productAttachmentList"></param>
        private void RemoveDBFileAttachmentNotAvailableInRequest(CuProduct entityProduct, SaveInspectionPOProductDetails productData, List<ProductFileAttachment> productAttachmentList)
        {
            List<CuProductFileAttachment> lstPurchaseOrderFileList = new List<CuProductFileAttachment>();

            // Make InActive if data does not exist in the db.
            if (productAttachmentList != null)
            {
                var activeFiles = entityProduct.CuProductFileAttachments.Where(x => x.BookingId != null && x.BookingId == productData.InspectionId && x.Active);

                foreach (var item in activeFiles)
                {
                    if (!productData.ProductFileAttachments.Any(x => x.Id != 0 && x.Id == item.Id))
                    {
                        lstPurchaseOrderFileList.Add(item);
                        item.DeletedOn = DateTime.Now;
                        item.DeletedBy = _ApplicationContext.UserId;
                        item.Active = false;
                    }
                }
            }

            _repo.EditEntities(lstPurchaseOrderFileList);
        }

        /// <summary>
        /// Map the product file attachment
        /// </summary>
        /// <param name="productAttachmentList"></param>
        /// <param name="EntityProduct"></param>
        /// <param name="bookingEntity"></param>
        private void MapProductFileAttachment(CuProduct entityProduct, SaveInspectionPOProductDetails productData, InspTransaction bookingEntity)
        {
            if (productData.ProductFileAttachments != null && productData.ProductFileAttachments.Any())
            {
                foreach (var fileAttachment in productData.ProductFileAttachments.Where(x => x.Id == 0))
                {
                    var attachment = new CuProductFileAttachment();
                    attachment.UniqueId = fileAttachment.uniqueld;
                    attachment.ProductId = fileAttachment.ProductId;
                    attachment.UserId = _ApplicationContext.UserId;
                    attachment.FileName = fileAttachment.FileName;
                    attachment.FileUrl = fileAttachment.FileUrl;
                    attachment.UploadDate = DateTime.Now;
                    attachment.EntityId = _filterService.GetCompanyId();
                    attachment.Active = true;
                    attachment.FileTypeId = (int)ProductRefFileType.ProductRefPictures;
                    entityProduct.CuProductFileAttachments.Add(attachment);
                    bookingEntity.CuProductFileAttachments.Add(attachment);

                }
            }



        }

        /// <summary>
        /// To update inspection booking
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private void UpdateInspectionBooking(InspTransaction entity, SaveInsepectionRequest request, int userId)
        {
            entity.CustomerId = request.CustomerId;
            entity.SupplierId = request.SupplierId;
            entity.FactoryId = request.FactoryId;
            entity.OfficeId = request.OfficeId;
            entity.ApiBookingComments = request.ApiBookingComments;
            entity.ApplicantEmail = request.ApplicantEmail;
            entity.ApplicantName = request.ApplicantName;
            entity.ApplicantPhoneNo = request.ApplicantPhoneNo;
            entity.CusBookingComments = request.CusBookingComments;
            entity.InternalReferencePo = request.InternalReferencePo;
            entity.ServiceDateFrom = entity.StatusId == (int)BookingStatus.Received ? request.ServiceDateFrom.ToDateTime() : entity.ServiceDateFrom; //Update the dates only if the status is requested
            entity.ServiceDateTo = entity.StatusId == (int)BookingStatus.Received ? request.ServiceDateTo.ToDateTime() : entity.ServiceDateTo; //Update the dates only if the status is requested
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
            entity.FlexibleInspectionDate = request.IsFlexibleInspectionDate;
            entity.StatusId = request.StatusId;
            entity.IsPickingRequired = request.IsPickingRequired;
            entity.IsCombineRequired = request.IsCombineRequired;
            entity.CustomerBookingNo = request.CustomerBookingNo;
            entity.InternalComments = request.InternalComments;
            entity.QcbookingComments = request.QCBookingComments?.Trim() ?? "";
            entity.FirstServiceDateFrom = (entity.StatusId == (int)BookingStatus.Received || entity.StatusId == (int)BookingStatus.Verified) ?
                request.FirstServiceDateFrom.ToDateTime() : entity.FirstServiceDateFrom;
            entity.FirstServiceDateTo = (entity.StatusId == (int)BookingStatus.Received ||
                entity.StatusId == (int)BookingStatus.Verified) ? request.FirstServiceDateTo.ToDateTime() : entity.FirstServiceDateTo;
            entity.CollectionId = request.collectionId;
            entity.PriceCategoryId = request.priceCategoryId;
            entity.CompassBookingNo = request.CompassBookingNo;
            entity.SeasonId = request.SeasonId;
            entity.InspectionLocation = request.InspectionLocation;
            entity.SeasonYearId = request.SeasonYearId;
            entity.ShipmentPort = request.ShipmentPort;
            entity.BookingType = request.BookingType;
            entity.PaymentOptions = request.PaymentOptions;
            entity.ReInspectionType = request.ReinspectionType;
            entity.Gapdacorrelation = request.GAPDACorrelation;
            entity.Gapdaname = request.GAPDAName;
            entity.Gapdaemail = request.GAPDAEmail;
            entity.CuProductCategory = request.CuProductCategory;
        }

        /// <summary>
        /// remove the db container which is not available in the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        private void RemoveDBContainersNotAvailableInRequest(SaveInsepectionRequest request, InspTransaction bookingEntity)
        {
            //take the container transaction which is available in db and not available in the request
            //delete the container transaction list(which make inactive in the db
            if (ContainerServiceList.Contains(request.ServiceTypeId))
            {
                //take active container list
                var activeContainerList = request.InspectionProductList.Where(x => x.PoTransactionId > 0)
                                         .Select(x => x.ContainerId).Distinct().ToList();

                var DbContainers = bookingEntity.InspContainerTransactions.
                                             Where(x => !activeContainerList.Contains(x.ContainerId) && x.Active.HasValue && x.Active.Value).ToList();

                if (DbContainers.Count > 0)
                {
                    foreach (var item in DbContainers)
                    {
                        item.Active = false;
                        item.DeletedOn = DateTime.Now;
                        item.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    }
                    _repo.EditEntities(DbContainers);
                }

            }
            // below case will happen when user change service type from container to any other.
            else if (bookingEntity.InspContainerTransactions.Count > 0)
            {
                foreach (var item in bookingEntity.InspContainerTransactions)
                {
                    item.Active = false;
                    item.DeletedOn = DateTime.Now;
                    item.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                }
                _repo.EditEntities(bookingEntity.InspContainerTransactions);
            }
        }

        /// <summary>
        /// remove the db color transaction which is not available in the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        private void RemoveDBColorTransactionNotAvailableInRequest(SaveInsepectionRequest request, InspTransaction bookingEntity)
        {

            var lstPOColorTransactionsToremove = new List<InspPurchaseOrderColorTransaction>();

            //take the po color transaction ids
            var poColorTransactionIds = request.InspectionProductList.Where(x => x.ColorTransactionId > 0).Select(x => x.ColorTransactionId).ToList();

            //take the po color transaction which is available in db and not available in the request
            //delete the po color transaction list(which make inactive in the db
            var inspectionPOColorTransactions = bookingEntity.InspProductTransactions.SelectMany(x => x.InspPurchaseOrderTransactions).
                                                SelectMany(x => x.InspPurchaseOrderColorTransactions).
                                                Where(x => !poColorTransactionIds.Contains(x.Id) && x.Active.HasValue && x.Active.Value);

            foreach (var item in inspectionPOColorTransactions)
            {
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                item.Active = false;
                lstPOColorTransactionsToremove.Add(item);
            }

            _repo.EditEntities(lstPOColorTransactionsToremove);
        }


        /// <summary>
        /// remove the db purchase order which is not available in the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        private void RemoveDBPurchaseOrderNotAvailalbeInRequest(SaveInsepectionRequest request, InspTransaction bookingEntity)
        {
            var lstPOTransactionsToremove = new List<InspPurchaseOrderTransaction>();

            //take the po transaction list
            var poDetails = request.InspectionProductList.Where(x => x.PoTransactionId > 0).ToList();

            //take the po transaction ids
            var poTransactionIds = poDetails.Select(x => x.PoTransactionId).ToList();

            // get Inspection product poids from db which is not exist in the current request 
            var inspectionPOTransactions = bookingEntity.InspProductTransactions.SelectMany(x => x.InspPurchaseOrderTransactions).
                                             Where(x => !poTransactionIds.Contains(x.Id) && x.Active.HasValue && x.Active.Value);


            // do in active and update to db
            foreach (var item in inspectionPOTransactions)
            {
                //make the container inactive if no products belongs to the container
                if (item.ContainerRefId != null && bookingEntity.InspProductTransactions.SelectMany(x => x.InspPurchaseOrderTransactions)
                    .Any(x => x.Active.Value && x.ContainerRefId == item.ContainerRefId && x.Id != item.Id))
                {
                    item.ContainerRef.Active = false;
                }

                item.DeletedOn = DateTime.Now;
                item.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                item.Active = false;
                lstPOTransactionsToremove.Add(item);
            }

            _repo.EditEntities(lstPOTransactionsToremove);
        }

        /// <summary>
        /// Remove the picking data from db if not available from the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        private void RemoveDBPickingNotAvailableInRequest(SaveInsepectionRequest request, InspTransaction bookingEntity)
        {
            var lstPickingTransactionsToremove = new List<InspTranPicking>();

            //take the po product transaction list with picking data available
            var inspectionProductListFilteredwithPicking = request.InspectionProductList.
                Where(x => x.SaveInspectionPickingList != null && x.SaveInspectionPickingList.Any(y => y.Id > 0)).ToList();

            //take the po picking ids
            var poPickingIds = inspectionProductListFilteredwithPicking.
                SelectMany(x => x.SaveInspectionPickingList).Where(x => x.Id > 0).Select(x => x.Id).ToList();

            // get Inspection picking ids from db which is not exist in the current request 
            //var inspectionPickingTransactions = bookingEntity.InspProductTransactions.
            //    SelectMany(x => x.InspPurchaseOrderTransactions).
            //                                 SelectMany(x => x.InspTranPickings).
            //                                 Where(x => !poPickingIds.Contains(x.Id) && x.Active);

            var inspectionPickingTransactions = bookingEntity.InspTranPickings.
                                             Where(x => !poPickingIds.Contains(x.Id) && x.Active);


            // do in active and update to db
            foreach (var item in inspectionPickingTransactions)
            {
                item.DeletionDate = DateTime.Now;
                item.DeletedBy = _ApplicationContext.UserId;
                item.Active = false;
                lstPickingTransactionsToremove.Add(item);
            }

            _repo.EditEntities(lstPickingTransactionsToremove);
        }

        /// <summary>
        /// Remove the picking contacts from db if not available in the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        private void RemoveDBPickingContactsNotAvailableInRequest(SaveInsepectionRequest request, InspTransaction bookingEntity)
        {
            var lstPickingContactsToremove = new List<InspTranPickingContact>();

            //take the po transaction list with picking data
            var inspectionProductListFilteredwithPicking = request.InspectionProductList.
                Where(x => x.SaveInspectionPickingList != null && x.SaveInspectionPickingList.Any(y => y.Id > 0)).ToList();

            //take the po transaction ids
            var poPickingContactIds = inspectionProductListFilteredwithPicking.
                    SelectMany(x => x.SaveInspectionPickingList.Where(y => y.LabType == (int)LabTypeEnum.Lab)).
                    SelectMany(x => x.PickingContactList).Select(x => x.Id).Distinct().ToList();

            // get Inspection picking ids from db which is not exist in the current request 
            //var inspectionPickingContacts = bookingEntity.InspProductTransactions.SelectMany(x => x.InspPurchaseOrderTransactions).
            //                                 SelectMany(x => x.InspTranPickings).SelectMany(x => x.InspTranPickingContacts).
            //                                 Where(x => !poPickingContactIds.Contains(x.LabContactId.GetValueOrDefault()) && x.Active);

            var inspectionPickingContacts = bookingEntity.InspTranPickings.Where(x => x.Active).SelectMany(x => x.InspTranPickingContacts)
                                            .Where(x => x.Active && x.LabContactId > 0
                                                && !poPickingContactIds.Contains(x.Id));


            // do in active and update to db
            foreach (var item in inspectionPickingContacts)
            {
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _ApplicationContext.UserId;
                item.Active = false;
                lstPickingContactsToremove.Add(item);
            }

            var poPickingCustomerContactIds = inspectionProductListFilteredwithPicking.
                    SelectMany(x => x.SaveInspectionPickingList.Where(y => y.LabType == (int)LabTypeEnum.Customer)).
                    SelectMany(x => x.PickingContactList).Select(x => x.Id).Distinct().ToList();


            var inspectionPickingCustomerContacts = bookingEntity.InspTranPickings.Where(x => x.Active).SelectMany(x => x.InspTranPickingContacts)
                                            .Where(x => x.Active && x.CusContactId > 0
                                                && !poPickingCustomerContactIds.Contains(x.Id));
            // do in active and update to db
            foreach (var item in inspectionPickingCustomerContacts)
            {
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _ApplicationContext.UserId;
                item.Active = false;
                lstPickingContactsToremove.Add(item);
            }

            _repo.EditEntities(lstPickingContactsToremove);
        }


        /// <summary>
        /// remove the db product which is not available in the product request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        /// <returns></returns>
        private List<int> RemoveDBProductNotAvailableInRequest(SaveInsepectionRequest request, InspTransaction bookingEntity)
        {
            var lstProductDetailToEdit = new List<InspProductTransaction>();
            List<int> combineProductToRemove = new List<int>();

            var inspectionProductIds = request.InspectionProductList.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
            var lstProductTransactionsToremove = new List<InspProductTransaction>();
            var inspectionProductTransactions = bookingEntity.InspProductTransactions.
                                                 Where(x => !inspectionProductIds.Contains(x.Id) && x.Active.HasValue && x.Active.Value);

            if (inspectionProductTransactions != null && inspectionProductTransactions.Any())
            {
                //Suppose we have one po and product combination.if we delete that po that product will be deleted
                //that time corresponding combine logic will be removed for that product
                var combineProductId = inspectionProductTransactions.Where(x => x.CombineProductId != null
                                                            && x.Active.HasValue && x.Active.Value).Select(x => x.CombineProductId.Value);

                combineProductToRemove.AddRange(combineProductId);

                foreach (var item in inspectionProductTransactions)
                {
                    item.DeletedOn = DateTime.Now;
                    item.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    item.Active = false;
                    lstProductTransactionsToremove.Add(item);
                }
            }

            _repo.EditEntities(lstProductTransactionsToremove);

            return combineProductToRemove;
        }

        /// <summary>
        /// process the container transaction on purchase order edit
        /// </summary>
        /// <param name="request"></param>
        /// <param name="poDetail"></param>
        /// <param name="bookingEntity"></param>
        private void ProcessContainerOnPurchaseOrderEdit(SaveInsepectionRequest request, SaveInspectionPOProductDetails poDetail, InspTransaction bookingEntity)
        {
            if (ContainerServiceList.Contains(request.ServiceTypeId))
            {
                // take the container transaction from the db mapped to the po
                var containerTransactions = bookingEntity.InspContainerTransactions.
                         FirstOrDefault(x => x.ContainerId == poDetail.ContainerId && x.Active.HasValue && x.Active.Value);

                //if container is not available for the po then add the container transaction
                if (poDetail.ContainerId > 0 && containerTransactions == null)
                {
                    var totalBookingQuantity = request.InspectionProductList.Where(x => x.PoTransactionId > 0 && x.ContainerId == poDetail.ContainerId).Sum(x => x.BookingQuantity);
                    var containerTransaction = _bookingmap.MapContainerTransaction(poDetail.ContainerId.GetValueOrDefault(), totalBookingQuantity);
                    containerTransaction.EntityId = _filterService.GetCompanyId();
                    containerTransaction.CreatedBy = _ApplicationContext.UserId;
                    containerTransaction.Id = 0;
                    bookingEntity.InspContainerTransactions.Add(containerTransaction);
                    _repo.AddEntity(containerTransaction);
                }
                //else update the total booking qty for the existing container
                else if (containerTransactions.Id > 0)
                {
                    var totalBookingQuantity = request.InspectionProductList.Where(x => x.ContainerId == poDetail.ContainerId).Sum(x => x.BookingQuantity);

                    // update container total booking quantity
                    if (containerTransactions != null)
                    {
                        containerTransactions.TotalBookingQuantity = totalBookingQuantity;
                        _repo.EditEntity(containerTransactions);
                    }

                    //bookingPODetails.ContainerRefId = containerTransactions.Id;
                }


            }
        }

        /// <summary>
        /// remove the picking order
        /// </summary>
        /// <param name="entityPODetail"></param>
        private void RemovePickingFromPurchaseOrder(InspPurchaseOrderTransaction entityPODetail)
        {
            if (entityPODetail.InspTranPickings.Count() > 0)
            {
                foreach (var picking in entityPODetail.InspTranPickings)
                {
                    picking.Active = false;
                    picking.DeletedBy = _ApplicationContext.UserId;
                    picking.DeletionDate = DateTime.Now;

                    foreach (var pickingcontact in picking.InspTranPickingContacts)
                    {
                        pickingcontact.Active = false;
                    }
                }
            }
        }

        /// <summary>
        /// Process the purchase order on edit booking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="poDetail"></param>
        /// <param name="bookingEntity"></param>
        /// <returns></returns>
        private InspPurchaseOrderTransaction ProcessPurchaseOrderRequestOnEditBooking(SaveInsepectionRequest request, SaveInspectionPOProductDetails poDetail, InspTransaction bookingEntity, List<SaveInspectionPOProductDetails> productPOList)
        {

            var entityPODetail = bookingEntity.InspPurchaseOrderTransactions.
                                FirstOrDefault(x => x.Active.HasValue && x.Active.Value && x.Id == poDetail.PoTransactionId);

            if (entityPODetail != null)
            {

                entityPODetail = _bookingmap.MapProductPOTransactionOnEdit(request, poDetail, entityPODetail, productPOList);


                entityPODetail.UpdatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

                if (bookingEntity.IsPickingRequired.GetValueOrDefault() && poDetail.SaveInspectionPickingList != null && poDetail.SaveInspectionPickingList.Any())
                {
                    //take the picking list to update
                    var pickingListToUpdate = poDetail.SaveInspectionPickingList.Where(x => x.Id > 0).ToList();

                    if (pickingListToUpdate.Any())
                        //update the picking data
                        UpdatePickingDataOnEditBooking(pickingListToUpdate, bookingEntity);

                    //take the picking list to create
                    var pickingListToCreate = poDetail.SaveInspectionPickingList.Where(x => x.Id <= 0).ToList();

                    if (pickingListToCreate.Any())
                        //Save the inspection picking transactions
                        SaveInspectionPickingTransactions(pickingListToCreate, bookingEntity, entityPODetail);
                }

                ProcessContainerOnPurchaseOrderEdit(request, poDetail, bookingEntity);

                if (ContainerServiceList.Contains(request.ServiceTypeId))
                    entityPODetail.ContainerRef = bookingEntity.InspContainerTransactions.
                                FirstOrDefault(x => x.ContainerId == poDetail.ContainerId && x.Active.HasValue && x.Active.Value);

                //if picking quantity changes then remove the picking for the purchase order transaction
                //if (poDetail.PickingQuantity.GetValueOrDefault() != entityPODetail.PickingQuantity.GetValueOrDefault())
                //{
                //    RemovePickingFromPurchaseOrder(entityPODetail);
                //}

                UpdateColorOrderTransaction(poDetail, bookingEntity);
            }

            return entityPODetail;
        }

        /// <summary>
        /// Update inspection picking data on edit booking
        /// </summary>
        /// <param name="saveInspectionPickingList"></param>
        /// <param name="bookingEntity"></param>
        private void UpdatePickingDataOnEditBooking(List<SaveInspectionPickingDetails> saveInspectionPickingList, InspTransaction bookingEntity)
        {
            if (saveInspectionPickingList != null && saveInspectionPickingList.Any())
            {
                foreach (var pickingData in saveInspectionPickingList)
                {
                    //get the inspection picking entity
                    var entityPickingDetail = bookingEntity.InspPurchaseOrderTransactions.
                                        SelectMany(x => x.InspTranPickings).Where(x => x.Id == pickingData.Id).FirstOrDefault();

                    _bookingmap.MapUpdateInspectionPicking(pickingData, entityPickingDetail, _ApplicationContext.UserId);
                }
            }
        }

        /// <summary>
        /// Update the color order transaction
        /// </summary>
        /// <param name="poDetail"></param>
        /// <param name="bookingEntity"></param>
        private void UpdateColorOrderTransaction(SaveInspectionPOProductDetails poDetail, InspTransaction bookingEntity)
        {
            //take the color transaction details
            var colorTransactionDetail = bookingEntity.InspPurchaseOrderTransactions.SelectMany(x => x.InspPurchaseOrderColorTransactions).
                                                FirstOrDefault(x => x.Id == poDetail.ColorTransactionId);
            //update the color transaction
            if (colorTransactionDetail != null)
            {
                colorTransactionDetail.ColorCode = poDetail.ColorCode;
                colorTransactionDetail.ColorName = poDetail.ColorName;
                colorTransactionDetail.BookingQuantity = poDetail.BookingQuantity;
            }
        }

        /// <summary>
        /// Process the sample quantity request on edit booking
        /// </summary>
        /// <param name="requestProductData"></param>
        /// <param name="entityProductData"></param>
        /// <param name="productTotalBookingQty"></param>
        /// <returns></returns>
        private List<int> ProcessSampleQuantityOnEditBooking(SaveInsepectionRequest request, SaveInspectionPOProductDetails requestProductData, InspProductTransaction entityProductData, int productTotalBookingQty)
        {
            List<int> combineProductToRemove = new List<int>();

            if ((entityProductData.ParentProductId != requestProductData.ParentProductId) ||
                             (entityProductData.Aql == (int)AqlType.AQLCustom
                             && entityProductData.SampleType == (int)SampleType.Others) ||
                             (entityProductData.Aql == (int)AqlType.AQLCustom && entityProductData.SampleType != requestProductData.SampleType) ||
                             entityProductData.Aql != requestProductData.Aql || entityProductData.Critical != requestProductData.Critical
                                  || entityProductData.Major != requestProductData.Major || entityProductData.Minor != requestProductData.Minor
                                  || entityProductData.TotalBookingQuantity != productTotalBookingQty)
            {
                //if product has combine group
                if (requestProductData.CombineProductId != null)
                {
                    requestProductData.AqlQuantity = entityProductData.AqlQuantity;
                    requestProductData.SampleType = entityProductData.SampleType;
                    combineProductToRemove.Add(requestProductData.CombineProductId.Value);
                }
                //if product dont have combine group
                else if (requestProductData.CombineProductId == null)
                {
                    if (!requestProductData.IsDisplayMaster)
                        UpdateAQLQuantity(request, requestProductData, entityProductData, productTotalBookingQty);
                    else if (requestProductData.IsDisplayMaster)
                    {
                        requestProductData.AqlQuantity = null;
                    }
                }
            }
            else if (entityProductData.Aql == requestProductData.Aql && entityProductData.Aql == (int)AqlType.AQLCustom)
            {
                requestProductData.AqlQuantity = entityProductData.AqlQuantity;
                requestProductData.SampleType = entityProductData.SampleType;
            }

            return combineProductToRemove;
        }

        /// <summary>
        /// Update the inspection po details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        /// <param name="currentBookingStatus"></param>
        /// <returns></returns>
        private async Task<bool> UpdateInspectionPODetails(SaveInsepectionRequest request, InspTransaction bookingEntity, int currentBookingStatus)
        {
            #region VariableDeclaration
            bool isCombineOrderDataChanged = false;
            List<int> combineProductToRemove = new List<int>();

            #endregion

            if (request.InspectionProductList != null && request.InspectionProductList.Any())
            {

                RemoveDBContainersNotAvailableInRequest(request, bookingEntity);

                RemoveDBColorTransactionNotAvailableInRequest(request, bookingEntity);

                RemoveDBPurchaseOrderNotAvailalbeInRequest(request, bookingEntity);
                if (bookingEntity.IsPickingRequired.GetValueOrDefault())
                {
                    //remove picking data from db if not available in the request
                    RemoveDBPickingNotAvailableInRequest(request, bookingEntity);

                    //remove the picking contacts from db if not available in the request
                    RemoveDBPickingContactsNotAvailableInRequest(request, bookingEntity);

                }

                combineProductToRemove = RemoveDBProductNotAvailableInRequest(request, bookingEntity);

                await AddInspectionPOProductList(request, bookingEntity);

                //set first product category info to inspection data(for Kpi purpose)
                UpdateProductCategoryDataToInspection(request.InspectionProductList.FirstOrDefault(), bookingEntity);

                await UpdateInspectionPOProductList(request, bookingEntity, combineProductToRemove);
            }

            return isCombineOrderDataChanged;
        }

        /// <summary>
        /// Update the product transaction
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingEntity"></param>
        /// <param name="combineProductToRemove"></param>
        private async Task UpdateInspectionPOProductList(SaveInsepectionRequest request, InspTransaction bookingEntity, List<int> combineProductToRemove)
        {
            #region VariableDeclaration
            bool isCombineOrderDataChanged = false;
            #endregion

            var lstProductDetailToEdit = new List<InspProductTransaction>();

            //take the product ids to update
            var updateProductIdList = request.InspectionProductList.Where(x => x.Id > 0).Select(x => x.Id).Distinct().ToList();

            if (updateProductIdList != null && updateProductIdList.Any())
            {
                var productSubCategory3Ids = request.InspectionProductList.Where(x => x.ProductCategorySub3Id != null).
                        Select(x => x.ProductCategorySub3Id.Value).Distinct().ToList();

                var workLoadMatrixList = await _workLoadMatrixRepository.GetWorkLoadMatrixByProductCatSub3List(productSubCategory3Ids);

                //loop through the product details
                foreach (var productRefId in updateProductIdList)
                {
                    //take the transaction row belongs to the product
                    var productPODetails = request.InspectionProductList.Where(x => x.Id == productRefId).ToList();

                    foreach (var poDetail in productPODetails)
                    {
                        var lstProductPODetailToEdit = new List<InspPurchaseOrderTransaction>();
                        //processing the po,container,color transaction 
                        var entityPODetail = ProcessPurchaseOrderRequestOnEditBooking(request, poDetail, bookingEntity, productPODetails);

                        lstProductPODetailToEdit.Add(entityPODetail);
                    }

                    //take the product detail in db
                    var entityProductData = bookingEntity.InspProductTransactions.FirstOrDefault(x => x.Active.HasValue && x.Active.Value && x.Id == productRefId);

                    //take the product detail in request
                    var requestProductData = request.InspectionProductList.FirstOrDefault(x => x.Id == productRefId);

                    var productId = requestProductData?.ProductId;

                    //get the total booking qty from the request
                    var productTotalBookingQty = request.InspectionProductList.Where(x => x.ProductId == productId).Sum(x => x.BookingQuantity);

                    if (request.ServiceTypeId != (int)InspectionServiceTypeEnum.Container)
                    {
                        var combineOrdersToRemoveOnProductDataChanged = ProcessSampleQuantityOnEditBooking(request, requestProductData, entityProductData, productTotalBookingQty);

                        if (combineOrdersToRemoveOnProductDataChanged != null & combineOrdersToRemoveOnProductDataChanged.Any())
                        {
                            isCombineOrderDataChanged = !isCombineOrderDataChanged;
                            combineProductToRemove.AddRange(combineOrdersToRemoveOnProductDataChanged);
                        }

                    }

                    _bookingmap.MapProductTransactionOnEditBooking(requestProductData, entityProductData);

                    entityProductData.TotalBookingQuantity = productTotalBookingQty;
                    entityProductData.UpdatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

                    lstProductDetailToEdit.Add(entityProductData);

                    var productTransactionRequest = request.InspectionProductList.FirstOrDefault(x => x.Id == productRefId);

                    UpdateProductRelatedData(productTransactionRequest, bookingEntity, workLoadMatrixList);

                }

                if (combineProductToRemove != null && combineProductToRemove.Any())
                {
                    var combineProductList = combineProductToRemove.Distinct().ToList();
                    UpdateAQLQuantityList(combineProductList, bookingEntity.InspProductTransactions, request);
                }

                if (lstProductDetailToEdit.Count > 0)
                    _repo.EditEntities(lstProductDetailToEdit);

            }
        }

        private void UpdateAQLQuantity(SaveInsepectionRequest request, SaveInspectionPOProductDetails requestData, InspProductTransaction inspectionProductDetail, int totalBookingQty)
        {

            // if the aql values is custom then directly bind the value from request (cusomized value by user)
            if (requestData.Aql == (int)AqlType.AQLCustom)
            {
                inspectionProductDetail.SampleType = requestData.SampleType;
                inspectionProductDetail.AqlQuantity = requestData.AqlQuantity != null ? requestData.AqlQuantity : 0;
            }
            else
            {

                var samplingQuantityBookingRequest = GenerateSamplingQuantityRequest(request, totalBookingQty, requestData.Aql, requestData.Critical, requestData.Major, requestData.Minor);

                var sampleSizeData = _combineOrdermanager.GetSamplingQuantity(samplingQuantityBookingRequest);

                //var sampleSizeData = _combineOrdermanager.GetSamplingQuantityByAqlandOrderQuantity(requestData.Aql, totalBookingQty,
                //                                                                      requestData.Critical.GetValueOrDefault(), requestData.Major.GetValueOrDefault(),
                //                                                                      requestData.Minor.GetValueOrDefault());

                requestData.AqlQuantity = sampleSizeData.Result;
                inspectionProductDetail.AqlQuantity = requestData.AqlQuantity;
                inspectionProductDetail.SampleType = null;
            }
        }

        /// <summary>
        /// Generate the sampling quantity request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderQuantity"></param>
        /// <param name="aqlId"></param>
        /// <param name="criticalId"></param>
        /// <param name="majorId"></param>
        /// <param name="minorId"></param>
        /// <returns></returns>
        private SamplingQuantityRequest GenerateSamplingQuantityRequest(SaveInsepectionRequest request, int? orderQuantity, int? aqlId, int? criticalId, int? majorId, int? minorId)
        {
            SamplingQuantityRequest samplingQuantityRequest = new SamplingQuantityRequest();
            samplingQuantityRequest.AqlId = aqlId.GetValueOrDefault();
            samplingQuantityRequest.CriticalId = criticalId.GetValueOrDefault();
            samplingQuantityRequest.MajorId = majorId.GetValueOrDefault();
            samplingQuantityRequest.MinorId = minorId.GetValueOrDefault();
            samplingQuantityRequest.OrderQuantity = orderQuantity.GetValueOrDefault();
            samplingQuantityRequest.CustomerId = request.CustomerId;
            samplingQuantityRequest.ServiceTypeId = request.ServiceTypeId;

            return samplingQuantityRequest;
        }

        //reset the combine logic for the product list
        private void UpdateAQLQuantityList(List<int> combineProductList, IEnumerable<InspProductTransaction> productTransactionList, SaveInsepectionRequest request)
        {
            foreach (var combineProductId in combineProductList)
            {
                var combineGroupProducts = productTransactionList.Where(x => x.CombineProductId == combineProductId);
                foreach (var item in combineGroupProducts)
                {
                    var poTransaction = productTransactionList.Where(x => x.ProductId == item.ProductId && x.Active.HasValue && x.Active.Value).FirstOrDefault();
                    if (poTransaction != null)
                    {
                        // if Aql is custom then take from request
                        if (item.Aql == (int)AqlType.AQLCustom)
                        {
                            poTransaction.CombineAqlQuantity = 0;
                            poTransaction.CombineProductId = null;
                            poTransaction.AqlQuantity = item.AqlQuantity != null ? item.AqlQuantity : 0;
                            poTransaction.SampleType = item.SampleType;
                        }
                        else
                        {
                            var samplingQuantityBookingRequest = GenerateSamplingQuantityRequest(request, item.TotalBookingQuantity, item.Aql, item.Critical, item.Major, item.Minor);

                            var sampleSizeData = _combineOrdermanager.GetSamplingQuantity(samplingQuantityBookingRequest);

                            //var sampleSizeData = _combineOrdermanager.GetSamplingQuantityByAqlandOrderQuantity(item.Aql, item.TotalBookingQuantity,
                            //                                                                          item.Critical.GetValueOrDefault(), item.Major.GetValueOrDefault(),
                            //                                                                          item.Minor.GetValueOrDefault());
                            poTransaction.CombineAqlQuantity = 0;
                            poTransaction.CombineProductId = null;
                            poTransaction.AqlQuantity = sampleSizeData.Result;
                            poTransaction.SampleType = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To update inspection booking customer contacts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionCustomerContactList(SaveInsepectionRequest request, InspTransaction entity)
        {
            // Purchase Order details
            var bookingCustomerContactsIds = request.InspectionCustomerContactList.Select(x => x).ToArray();

            var lstCustomerContactsToremove = new List<InspTranCuContact>();

            var bookingCustomerContacts = entity.InspTranCuContacts.Where(x => !bookingCustomerContactsIds.Contains(x.ContactId) && x.Active);

            var existcusContactlist = entity.InspTranCuContacts.Where(x => bookingCustomerContactsIds.Contains(x.ContactId) && x.Active);

            // Remove if data does not exist in the db.

            foreach (var item in bookingCustomerContacts)
            {
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                item.Active = false;
                lstCustomerContactsToremove.Add(item);
            }

            _repo.EditEntities(lstCustomerContactsToremove);

            // Update if data already exist in the db

            if (request.InspectionCustomerContactList != null)
            {
                // Add if data is new it means id = 0;
                foreach (var id in bookingCustomerContactsIds)
                {
                    if (!existcusContactlist.Any() || !(existcusContactlist.Any(x => x.ContactId == id)))
                    {
                        entity.InspTranCuContacts.Add(new InspTranCuContact()
                        {
                            ContactId = id,
                            Active = true,
                            CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }

            }

        }
        /// <summary>
        /// To update inspection booking supplier contacts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionSupplierContactList(SaveInsepectionRequest request, InspTransaction entity)
        {
            // Purchase Order details
            var bookingSupplierContactsIds = request.InspectionSupplierContactList.Select(x => x).ToArray();

            var lstSupplierContactsToremove = new List<InspTranSuContact>();

            var bookingSupplierContacts = entity.InspTranSuContacts.Where(x => !bookingSupplierContactsIds.Contains(x.ContactId) && x.Active);

            var existSupplierContacts = entity.InspTranSuContacts.Where(x => bookingSupplierContactsIds.Contains(x.ContactId) && x.Active);

            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

            // Remove if data does not exist in the db.

            foreach (var item in bookingSupplierContacts)
            {
                lstSupplierContactsToremove.Add(item);
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = userId;
                item.Active = false;
            }

            _repo.EditEntities(lstSupplierContactsToremove);

            // Update if data already exist in the db

            if (request.InspectionSupplierContactList != null)
            {

                foreach (var id in bookingSupplierContactsIds)
                {

                    if (!existSupplierContacts.Any() || !(existSupplierContacts.Any(x => x.ContactId == id)))
                    {
                        entity.InspTranSuContacts.Add(new InspTranSuContact()
                        {
                            ContactId = id,
                            Active = true,
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }
        /// <summary>
        /// <summary>
        /// To update inspection booking Factory contacts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionFactoryContactList(SaveInsepectionRequest request, InspTransaction entity)
        {
            // Purchase Order details
            var bookingFactoryContactsIds = request.InspectionFactoryContactList.Select(x => x).ToArray();

            var lstFactoryContactsToremove = new List<InspTranFaContact>();

            var bookingSupplierContacts = entity.InspTranFaContacts.Where(x => !bookingFactoryContactsIds.Contains(x.ContactId) && x.Active);

            var existSupplierContacts = entity.InspTranFaContacts.Where(x => bookingFactoryContactsIds.Contains(x.ContactId) && x.Active);

            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

            // Remove if data does not exist in the db.

            foreach (var item in bookingSupplierContacts)
            {
                lstFactoryContactsToremove.Add(item);
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = userId;
                item.Active = false;
            }

            _repo.EditEntities(lstFactoryContactsToremove);

            // Update if data already exist in the db

            if (request.InspectionFactoryContactList != null)
            {
                foreach (var item in bookingFactoryContactsIds)
                {
                    if (!existSupplierContacts.Any() || !(existSupplierContacts.Any(x => x.ContactId == item)))
                    {
                        entity.InspTranFaContacts.Add(new InspTranFaContact()
                        {
                            ContactId = item,
                            Active = true,
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }

        /// <summary>
        /// To update inspection booking Service Types 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>

        private void UpdateInspectionServiceTypeList(SaveInsepectionRequest request, InspTransaction entity)
        {
            var bookingServiceTypeIds = request.InspectionServiceTypeList.Select(x => x).ToArray();
            var lstServiceTypesToremove = new List<InspTranServiceType>();
            var bookingServiceTypes = entity.InspTranServiceTypes.Where(x => !bookingServiceTypeIds.Contains(x.ServiceTypeId) && x.Active);
            var existServiceTypelist = entity.InspTranServiceTypes.Where(x => bookingServiceTypeIds.Contains(x.ServiceTypeId) && x.Active);
            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
            // Remove if data does not exist in the db.

            foreach (var item in bookingServiceTypes)
            {
                lstServiceTypesToremove.Add(item);
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = userId;
                item.Active = false;
            }

            _repo.EditEntities(lstServiceTypesToremove);

            // Update if data already exist in the db

            if (request.InspectionServiceTypeList != null)
            {
                foreach (var item in bookingServiceTypeIds)
                {
                    if (!existServiceTypelist.Any() || existServiceTypelist.Any(x => x.ServiceTypeId != item))
                    {
                        entity.InspTranServiceTypes.Add(new InspTranServiceType()
                        {
                            ServiceTypeId = item,
                            Active = true,
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }

        private void UpdateInspectionCustomerBrandList(SaveInsepectionRequest request, InspTransaction entity)
        {
            // Purchase Order details
            var bookingCustomerBrandIds = request.InspectionCustomerBrandList.Select(x => x).ToArray();

            var lstCustomerBrandsToremove = new List<InspTranCuBrand>();

            var bookingCustomerBrands = entity.InspTranCuBrands.Where(x => !bookingCustomerBrandIds.Contains(x.BrandId) && x.Active);

            var existcusBrandlist = entity.InspTranCuBrands.Where(x => bookingCustomerBrandIds.Contains(x.BrandId) && x.Active);

            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

            //// Remove if data does not exist in the db.

            foreach (var item in bookingCustomerBrands)
            {
                item.Active = false;
                item.DeletedBy = userId;
                item.DeletedOn = DateTime.Now;
                lstCustomerBrandsToremove.Add(item);
            }

            _repo.EditEntities(lstCustomerBrandsToremove);

            // Update if data already exist in the db

            if (request.InspectionCustomerBrandList != null)
            {
                foreach (var id in bookingCustomerBrandIds)
                {
                    if (!existcusBrandlist.Any() || !existcusBrandlist.Any(x => x.BrandId == id))
                    {
                        entity.InspTranCuBrands.Add(new InspTranCuBrand()
                        {
                            BrandId = id,
                            Active = true,
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }
        /// <summary>
        /// update or remove customer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionCustomerBuyerList(SaveInsepectionRequest request, InspTransaction entity)
        {
            // Purchase Order details
            var bookingCustomerBuyerIds = request.InspectionCustomerBuyerList.Select(x => x).ToArray();

            var lstCustomerBuyersToremove = new List<InspTranCuBuyer>();

            var bookingCustomerBuyers = entity.InspTranCuBuyers.Where(x => !bookingCustomerBuyerIds.Contains(x.BuyerId) && x.Active);

            var existcusBuyerlist = entity.InspTranCuBuyers.Where(x => bookingCustomerBuyerIds.Contains(x.BuyerId) && x.Active);
            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

            // Remove if data does not exist in the db.

            foreach (var item in bookingCustomerBuyers)
            {
                item.Active = false;
                item.DeletedBy = userId;
                item.DeletedOn = DateTime.Now;
                lstCustomerBuyersToremove.Add(item);
            }

            _repo.EditEntities(lstCustomerBuyersToremove);

            // Update if data already exist in the db

            if (request.InspectionCustomerBuyerList != null)
            {
                // Add if data is new it means id = 0;
                foreach (var id in bookingCustomerBuyerIds)
                {
                    if (!existcusBuyerlist.Any() || !existcusBuyerlist.Any(x => x.BuyerId == id))
                    {
                        entity.InspTranCuBuyers.Add(new InspTranCuBuyer()
                        {
                            BuyerId = id,
                            Active = true,
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }
        /// <summary>
        /// update or remove customer department list
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionCustomerDepartmentList(SaveInsepectionRequest request, InspTransaction entity)
        {
            // Purchase Order details
            var bookingCustomerDepartmentIds = request.InspectionCustomerDepartmentList.Select(x => x).ToArray();

            var lstCustomerDepartmentsToremove = new List<InspTranCuDepartment>();

            var bookingCustomerDepartments = entity.InspTranCuDepartments.Where(x => !bookingCustomerDepartmentIds.Contains(x.DepartmentId) && x.Active);

            var existcusDepartmentlist = entity.InspTranCuDepartments.Where(x => bookingCustomerDepartmentIds.Contains(x.DepartmentId) && x.Active);

            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

            // Remove if data does not exist in the db.

            foreach (var item in bookingCustomerDepartments)
            {
                item.Active = false;
                item.DeletedBy = userId;
                item.DeletedOn = DateTime.Now;
                lstCustomerDepartmentsToremove.Add(item);
            }

            _repo.EditEntities(lstCustomerDepartmentsToremove);

            // Update if data already exist in the db

            if (request.InspectionCustomerDepartmentList != null)
            {

                foreach (var id in bookingCustomerDepartmentIds)
                {
                    if (!existcusDepartmentlist.Any() || !existcusDepartmentlist.Any(x => x.DepartmentId == id))
                    {
                        entity.InspTranCuDepartments.Add(new InspTranCuDepartment()
                        {
                            DepartmentId = id,
                            Active = true,
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }

        private void UpdateInspectionCs(SaveInsepectionRequest request, InspTransaction entity)
        {
            // Purchase Order details
            var bookingTranCsIds = request.CsList.Select(x => x).ToList();

            var lstTransCsToremove = new List<InspTranC>();

            var bookingTransCs = entity.InspTranCS.Where(x => !bookingTranCsIds.Contains(x.CsId.GetValueOrDefault())
                                            && x.Active.HasValue && x.Active.Value);

            var existCslist = entity.InspTranCS.Where(x => bookingTranCsIds.Contains(x.CsId.GetValueOrDefault())
                                            && x.Active.HasValue && x.Active.Value);

            var userId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;

            // Remove if data does not exist in the db.

            foreach (var item in bookingTransCs)
            {
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = userId;
                item.Active = false;
                lstTransCsToremove.Add(item);
            }

            _repo.EditEntities(lstTransCsToremove);

            // Update if data already exist in the db

            if (request.CsList != null)
            {

                foreach (var id in request.CsList)
                {
                    if (!existCslist.Any() || !existCslist.Any(x => x.CsId == id))
                    {
                        entity.InspTranCS.Add(new InspTranC()
                        {
                            CsId = id,
                            Active = true,
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }

            }

        }

        /// <summary>
        /// To add inspection booking customer contacts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>

        private void AddInspectionCustomerContactList(SaveInsepectionRequest request, InspTransaction entity)
        {
            if (request.InspectionCustomerContactList != null)
            {
                foreach (var customerContactId in request.InspectionCustomerContactList)
                {
                    //var inspectionCustomerContacts = _mapper.Map<InspTranCuContact>(item);

                    var _cuscontact = new InspTranCuContact()
                    {
                        Active = true,
                        ContactId = customerContactId,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    entity.InspTranCuContacts.Add(_cuscontact);
                    _repo.AddEntity(_cuscontact);
                }
            }
        }
        /// <summary>
        /// To add inspection booking factory contacts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInspectionFactoryContactList(SaveInsepectionRequest request, InspTransaction entity)
        {
            if (request.InspectionFactoryContactList != null)
            {
                foreach (var factoryContactId in request.InspectionFactoryContactList)
                {
                    //var inspectionFactoryContacts = _mapper.Map<InspTranFaContact>(item);
                    var _factorycontact = new InspTranFaContact()
                    {
                        Active = true,
                        ContactId = factoryContactId,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };
                    entity.InspTranFaContacts.Add(_factorycontact);
                    _repo.AddEntity(_factorycontact);
                }
            }
        }
        /// <summary>
        /// To add inspection booking supplier contacts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInspectionSupplierContactList(SaveInsepectionRequest request, InspTransaction entity)
        {
            if (request.InspectionSupplierContactList != null)
            {
                foreach (var supplierContactId in request.InspectionSupplierContactList)
                {
                    //var inspectionSupplierContacts = _mapper.Map<InspTranSuContact>(item);
                    var _supplierContact = new InspTranSuContact()
                    {
                        Active = true,
                        ContactId = supplierContactId,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };
                    entity.InspTranSuContacts.Add(_supplierContact);
                    _repo.AddEntity(_supplierContact);
                }
            }
        }
        /// <summary>
        /// To add inspection booking service types
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInspectionServiceTypeList(SaveInsepectionRequest request, InspTransaction entity)
        {
            if (request.InspectionServiceTypeList != null)
            {
                foreach (var serviceTypeId in request.InspectionServiceTypeList)
                {
                    //var inspectionPODetail = _mapper.Map<InspTranServiceType>(item);
                    var _serviceType = new InspTranServiceType()
                    {
                        Active = true,
                        ServiceTypeId = serviceTypeId,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };
                    entity.InspTranServiceTypes.Add(_serviceType);
                    _repo.AddEntity(_serviceType);
                }
            }
        }

        /// <summary>
        /// Adding customer brand list for booking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInspectionCustomerBrandList(SaveInsepectionRequest request, InspTransaction entity)
        {
            if (request.InspectionCustomerBrandList != null)
            {
                foreach (var customerBrandId in request.InspectionCustomerBrandList)
                {
                    var _cusbrand = new InspTranCuBrand()
                    {
                        Active = true,
                        BrandId = customerBrandId,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    entity.InspTranCuBrands.Add(_cusbrand);
                    _repo.AddEntity(_cusbrand);
                }
            }
        }
        /// <summary>
        /// Adding customer buyer list for booking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInspectionCustomerBuyerList(SaveInsepectionRequest request, InspTransaction entity)
        {
            if (request.InspectionCustomerBuyerList != null)
            {
                foreach (var customerBuyerId in request.InspectionCustomerBuyerList)
                {
                    var _cusbuyer = new InspTranCuBuyer()
                    {
                        Active = true,
                        BuyerId = customerBuyerId,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    entity.InspTranCuBuyers.Add(_cusbuyer);
                    _repo.AddEntity(_cusbuyer);
                }
            }
        }
        /// <summary>
        /// Adding customer department list for booking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInspectionCustomerDepartmentList(SaveInsepectionRequest request, InspTransaction entity)
        {
            if (request.InspectionCustomerDepartmentList != null)
            {
                foreach (var customerDeptId in request.InspectionCustomerDepartmentList)
                {
                    var _cusDepartment = new InspTranCuDepartment()
                    {
                        Active = true,
                        DepartmentId = customerDeptId,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    entity.InspTranCuDepartments.Add(_cusDepartment);
                    _repo.AddEntity(_cusDepartment);
                }
            }
        }



        private void AddFiles(IEnumerable<BookingFileAttachment> files, InspTransaction inspectionEntity, int? userId = null)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var ecFile = new InspTranFileAttachment
                    {
                        FileName = file.FileName?.Trim(),
                        UniqueId = file.uniqueld,
                        FileUrl = file.FileUrl,
                        Active = true,
                        UploadDate = DateTime.Now,
                        UserId = userId > 0 ? userId.GetValueOrDefault() : _ApplicationContext.UserId,
                        FileDescription = file.FileDescription,
                        FileAttachmentCategoryId = file.FileAttachmentCategoryId,
                    };

                    if (file.FileAttachmentCategoryId != (int)FileAttachmentCategory.GAP)
                    {
                        ecFile.ZipStatus = (int)ZipStatus.Pending;
                        ecFile.ZipTryCount = 0;
                        ecFile.IsbookingEmailNotification = file.IsBookingEmailNotification;
                        ecFile.IsReportSendToFb = file.IsReportSendToFB;
                    }
                    inspectionEntity.InspTranFileAttachments.Add(ecFile);
                }
            }

        }

        public async Task<UnitDetailsResponse> GetUnits()
        {
            UnitDetailsResponse response = new UnitDetailsResponse();
            var data = await _referenceRepo.GetUnits();
            if (data == null || data.Count == 0)
            {
                response.Result = UnitResult.CannotGetUnitList;
            }
            else
            {
                response.UnitList = data.Select(_refmap.GetUnit);
                response.Result = UnitResult.Success;
            }

            return response;
        }

        public async Task UploadProductFiles(Dictionary<string, byte[]> fileList, int bookingId)
        {
            var guidList = fileList.Select(x => x.Key);

            foreach (var file in fileList)
            {
                var productFileAttachment = _repo.GetProductFile(file.Key);
                productFileAttachment.BookingId = bookingId;
                await _repo.Save();
            }
        }

        public BookingPOListResponse GetPOListByCustomerAndProducts(int? customerid, int productcategoryid, int supplierId)
        {
            var response = new BookingPOListResponse();

            if (customerid != null)
            {
                var purchaseOrderList = _poRepo.GetPurchaseOrderDetailsByCustomerId(customerid);

                if (purchaseOrderList == null)
                {
                    response.Data = null;
                    return new BookingPOListResponse { Result = BookingPoListResult.NotFound };

                }
                else
                {
                    var bookingPOList = FilterPOByProducts(purchaseOrderList, productcategoryid, supplierId);
                    if (bookingPOList != null && bookingPOList.Count > 0)
                    {
                        response.Data = bookingPOList;
                    }
                    else
                    {
                        response.Data = null;
                        return new BookingPOListResponse { Result = BookingPoListResult.NotFound };
                    }

                }
            }
            response.Result = BookingPoListResult.Success;
            return response;
        }

        private List<BookingPO> FilterPOByProducts(IEnumerable<CuPurchaseOrder> purchaseOrderList, int? productCategoryId, int supplierId)
        {
            List<BookingPO> bookingPoList = new List<BookingPO>();
            foreach (var purchaseOrder in purchaseOrderList)
            {
                //var purchaseOrderDetails = purchaseOrder.CuPurchaseOrderDetails.
                //                            Where(x => (x.Product.ProductCategory == productCategoryId || x.Product.ProductCategory == null)
                //                            && (x.SupplierId == supplierId || x.SupplierId == null));
                //if (purchaseOrderDetails != null)
                //{
                //    if (purchaseOrderDetails.Count() > 0)
                //    {
                //        if (!bookingPoList.Any(x => x.pono == purchaseOrder.Pono))
                //        {
                //            BookingPO bookingPO = new BookingPO();
                //            bookingPO.id = purchaseOrder.Id;
                //            bookingPO.pono = purchaseOrder.Pono;
                //            bookingPoList.Add(bookingPO);
                //        }
                //    }
                //}

            }
            return bookingPoList;
        }

        public List<CustomerProduct> GetProductsByCustomerPOAndCategory(int customerId, int poid)
        {
            List<CustomerProduct> customerProductList = new List<CustomerProduct>();
            var data = _productRepo.GetCustomerProductsByCustomers(customerId);

            foreach (var productData in data)
            {
                // Add the code for product id filter to get unique po.
                var purchaseOrderDetail = productData.CuPurchaseOrderDetails.Where(x => x.PoId == poid && x.ProductId == productData.Id).FirstOrDefault();
                if (purchaseOrderDetail != null)
                {
                    if (purchaseOrderDetail.BookingStatus == (int)BookingProductStatus.NotUtlized)
                    {
                        customerProductList.Add(_mapper.Map<CustomerProduct>(productData));
                    }
                }
                else // if the product is not available with any po then pass to list for new booking.
                {
                    customerProductList.Add(_mapper.Map<CustomerProduct>(productData));
                }
            }

            return customerProductList;
        }

        public async Task<FileResponse> GetFile(int id)
        {
            var file = await _repo.GetFile(id);

            //if (file == null || file.File == null)
            //    return new FileResponse { Result = FileResult.NotFound };

            return new FileResponse
            {
                // Content = file.File,
                MimeType = _fileManager.GetMimeType(Path.GetExtension(file.FileName)),
                Result = FileResult.Success
            };
        }



        public ReInspectionTypeResponse GetReInspectionTypes()
        {
            var data = _referenceRepo.GetReInspectionTypes();

            if (data == null || !data.Any())
                return null;

            return _bookingmap.GetReInspectionTypes(data);
        }

        /// <summary>
        /// Adding status log for inspection booking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInspectionStatusLog(SaveInsepectionRequest request, InspTransaction entity)
        {
            entity.InspTranStatusLogs.Add(new InspTranStatusLog()
            {
                CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                StatusId = request.StatusId,
                ServiceDateFrom = request.ServiceDateFrom.ToDateTime(),
                ServiceDateTo = request.ServiceDateTo.ToDateTime(),
                StatusChangeDate = DateTime.Now,
                EntityId = _filterService.GetCompanyId()
            });
        }

        /// To get customer contact details by customer id for booking page.
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>CustomerDetails</returns>
        public async Task<BookingCustomerContactDetails> GetCustomerContacts(BookingCustomerContactRequest request)
        {
            BookingCustomerContactDetails response = new BookingCustomerContactDetails();
            try
            {
                var cuContacts = new List<CuContact>();
                //Bring all the customer contacts
                var CustomerContactList = await _customerContactRepo.GetCustomerContacts(request.customerId);

                //Filter only operational contacts
                CustomerContactList = CustomerContactList.Where(x => x.Active == true && x.CuCustomerContactTypes.Any(y => y.ContactTypeId == 1)).ToList();

                //Bring Inactive Customer Contacts Services means we removed the mapping for the services with the customer contacts when editing
                var inactiveCustomerServices = CustomerContactList.Where(x => x.CuContactServices.All(y => y.Active == false) && x.CuContactServices.Count() != 0).ToList();

                //Filter Inspection specific customer services
                CustomerContactList = CustomerContactList.Where(x => x.CuContactServices.Any(y => y.ServiceId == request.customerServiceId && y.Active)
                                                                                            || x.CuContactServices.Count() == 0).ToList();

                if (inactiveCustomerServices.Count() != 0)
                {
                    CustomerContactList.AddRange(inactiveCustomerServices.Except(CustomerContactList));
                }

                if (request.brandIdlst != null && request.brandIdlst.Count() > 0)
                {
                    //Bring Inactive Customer Contacts Brands means we removed the mapping for the brands with the customer contacts when editing
                    var inactiveCustomerBrands = CustomerContactList.Where(x => x.CuContactBrands.All(y => y.Active == false) && x.CuContactBrands.Count() != 0).ToList();

                    if (inactiveCustomerBrands.Count() != 0)
                    {
                        cuContacts.AddRange(inactiveCustomerBrands);
                    }

                    var contactbrandList = CustomerContactList.Where(x => x.CuContactBrands.Any(y => request.brandIdlst.Contains(y.BrandId) && y.Active)
                                                                                        || x.CuContactBrands.Count() == 0).ToList();
                    cuContacts.AddRange(contactbrandList.Except(cuContacts));

                }

                if (request.deptIdlst != null && request.deptIdlst.Count() > 0)
                {
                    //Bring Inactive Customer Contacts Departments means we removed the mapping for the departments with the customer contacts when editing
                    var inactiveCustomerDepts = CustomerContactList.Where(x => x.CuContactDepartments.All(y => y.Active == false) && x.CuContactDepartments.Count() != 0).ToList();

                    if (inactiveCustomerDepts.Count() != 0)
                    {
                        cuContacts.AddRange(inactiveCustomerDepts.Except(cuContacts));
                    }

                    var contactdepartmentList = CustomerContactList.Where(x => x.CuContactDepartments.Any(y => request.deptIdlst.Contains(y.DepartmentId) && y.Active)
                                                                                                        || x.CuContactDepartments.Count() == 0).ToList();
                    cuContacts.AddRange(contactdepartmentList.Except(cuContacts));

                }

                if (cuContacts != null && cuContacts.Count() > 0)
                {
                    response.CustomerContactList = cuContacts.Select(x => _customermap.GetCustomerContact(x)).OrderBy(x => x.ContactName);
                    response.Result = BookingCustomerContactResult.Success;
                }
                else if (cuContacts.Count() == 0 && (request.brandIdlst == null || request.brandIdlst.Count() == 0) &&
                    (request.deptIdlst == null || request.deptIdlst.Count() == 0))
                {
                    response.CustomerContactList = CustomerContactList.Select(x => _customermap.GetCustomerContact(x)).OrderBy(x => x.ContactName);
                    response.Result = BookingCustomerContactResult.Success;
                }
                else
                {
                    response.Result = BookingCustomerContactResult.NotFound;
                }
                //fetch the inactive customer contact when edit booking

                if (request.bookingId > 0)
                {
                    //fetch the booking involved inactive customer contacts 
                    var bookingCustomerContacts = await _repo.GetEditBookingCustomerContacts(request.bookingId);
                    //append the inactive contact list to master
                    response.CustomerContactList = response.CustomerContactList.Concat(bookingCustomerContacts);
                    response.CustomerContactList = response.CustomerContactList.Distinct().ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
                //return new BookingCustomerContactDetails() { Result = BookingCustomerContactResult.NotFound };
            }
            return response;

        }

        public IQueryable<InspTransaction> GetExcludedInspections(int statusId)
        {
            //Exclude inspection with cancel status
            var inspectionList = _repo.GetExcludedInspectionByStatus(statusId);
            //Exclude Reinspection booking
            inspectionList = inspectionList.Where(x => x.PreviousBookingNo == null);
            return inspectionList;
        }


        public async Task<SetInspNotifyResponse> BookingTaskNotification(int id, bool isCombineOrderDataChanged, int statusId, SaveInsepectionRequest request)
        {
            if (_dictStatuses.TryGetValue((BookingStatus)statusId, out Func<int, bool, SaveInsepectionRequest, Task<SetInspNotifyResponse>> func))
                return await func(id, isCombineOrderDataChanged, request);

            return new SetInspNotifyResponse { Result = SetInspStatusResult.CannotUpdateStatus };
        }

        /// <summary>
        /// Get booking products and reports
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<BookingProductAndReportDataResponse> GetBookingAndProductsAndReports(int bookingId, int reportId, int containerId)
        {

            // get booking data
            var bookingData = await _repo.GetBookingData(bookingId);

            //get the booking service types
            var bookingServiceTypes = await _repo.GetBookingServiceTypes(bookingId);

            //get the booking customer brands
            var bookingCustomerBrands = await _repo.GetBookingBrands(bookingId);

            //get the booking customer departments
            var bookingCustomerDepartments = await _repo.GetBookingDepartments(bookingId);

            if (bookingServiceTypes != null && bookingServiceTypes.Any())
            {
                bookingData.ServiceTypeIds = bookingServiceTypes;
            }

            if (bookingCustomerBrands != null && bookingCustomerBrands.Any())
            {
                bookingData.BrandIds = bookingCustomerBrands;
            }

            if (bookingCustomerDepartments != null && bookingCustomerDepartments.Any())
            {
                bookingData.DepartmentIds = bookingCustomerDepartments;
            }

            // get report data 
            var reportData = await _repo.GetFbReports(reportId);

            reportData.AdditionalPhotos = new List<string>();

            //var fbReportAdditionalPhotos = await _repo.GetFBReportAdditionalPhotos(reportId);

            //if (fbReportAdditionalPhotos != null && fbReportAdditionalPhotos.Any())
            //{
            //    reportData.AdditionalPhotos = fbReportAdditionalPhotos;
            //}

            // get report customer decision data 
            var reportCustomerDecision = await _repo.GetReportCustomerDecision(reportId);

            // get customer check point count
            var customerCheckPoint = await _repo.GetCusCPByCusServiceId(bookingData.CustomerId, (int)Service.InspectionId, (int)CheckPointTypeEnum.CustomerDecisionRequired);
            bool checkPointExists = false;

            if (customerCheckPoint > 0)
            {
                checkPointExists = true;
                //if no brand or dept or service type is selected, then checkpoint is applied to all the brands, depts and service types
                var brandCheckpoint = true;
                var deptCheckpoint = true;
                var serviceCheckpoint = true;

                var brandData = await _customerCheckPointRepository.GetCustomerCheckPointBrand(new int[] { customerCheckPoint }.ToList());

                if (brandData.Any())
                {
                    brandCheckpoint = bookingData.BrandIds.Any(x => brandData.Any(y => y.Id == x));
                }

                var deptData = await _customerCheckPointRepository.GetCustomerCheckPointDept(new int[] { customerCheckPoint }.ToList());
                if (deptData.Any())
                {
                    deptCheckpoint = bookingData.DepartmentIds.Any(x => deptData.Any(y => y.Id == x));
                }

                var serviceTypeData = await _customerCheckPointRepository.GetCustomerCheckPointServiceType(new int[] { customerCheckPoint }.ToList());
                if (serviceTypeData.Any())
                {
                    serviceCheckpoint = bookingData.ServiceTypeIds.Any(x => serviceTypeData.Any(y => y.ServiceTypeId == x));
                }

                if (!brandCheckpoint || !deptCheckpoint || !serviceCheckpoint)
                {
                    checkPointExists = false;
                }

            }

            bookingData.IsCustomerCheckPoint = checkPointExists;

            //set Inspection date

            if (reportData != null)
            {
                if (reportData.StartDate == reportData.ToDate)
                {
                    reportData.InspectionDate = reportData.StartDate?.ToString(StandardDateFormat);
                }
                else
                {
                    if (reportData.ToDate != null)
                    {
                        reportData.InspectionDate = reportData.ToDate?.ToString(StandardDateFormat);
                    }
                    else
                    {
                        reportData.InspectionDate = reportData.StartDate?.ToString(StandardDateFormat);
                    }
                }

                if (reportCustomerDecision != null)
                {
                    if (!string.IsNullOrEmpty(reportCustomerDecision.CustomerDecisionCustomStatus))
                    {
                        reportData.CustomerDecisionStatus = reportCustomerDecision.CustomerDecisionCustomStatus;
                    }
                    else
                    {
                        reportData.CustomerDecisionStatus = reportCustomerDecision.CustomerDecisionStatus;
                    }

                    reportData.CustomerDecisionComments = reportCustomerDecision.Comments;
                    reportData.CustomerResultId = reportCustomerDecision.CustomerResultId;
                }
            }

            // get booking and report products data
            var bookingIds = new[] { bookingId };
            var reportProductList = (!ContainerServiceList.Contains(bookingData.InspectionTypeId)) ? await _repo.GetReportsProductListByBooking(bookingId, reportId) : await _repo.GetContainerListByBooking(bookingIds, containerId);
            var destinationCountryList = await _repo.GetBookingPoList(new[] { bookingId }.ToList());

            // set combine count 

            if (reportProductList != null)
            {
                reportData.TotalQuantity = reportProductList.Sum(x => x.BookingQuantity.GetValueOrDefault());
                reportData.TotalPresentedQty = reportProductList.Sum(x => x.PresentedQuantity.GetValueOrDefault());
                reportData.TotalInspecttedQty = reportProductList.Sum(x => x.InspectedQuantity.GetValueOrDefault());

                foreach (var item in reportProductList)
                {
                    if (item.CombineProductId > 0)
                    {
                        item.CombineProductCount = reportProductList.Where(x => x.CombineProductId == item.CombineProductId).Count();
                    }
                    // set default value
                    item.Major = item.Major == 0 ? null : item.Major;
                    item.Minor = item.Minor == 0 ? null : item.Minor;
                    item.Critical = item.Critical == 0 ? null : item.Critical;

                    item.DestinationCountry = string.Join(", ", destinationCountryList.Where(x => x.ProductRefId == item.InspectionPoId || x.ContainerRefId == item.ContainerId).Select(x => x.DestinationCountry).Distinct().ToList());
                    item.PoNumber = destinationCountryList.Where(x => x.ProductRefId == item.InspectionPoId || x.ContainerRefId == item.ContainerId).Select(x => x.PoNumber).Distinct().ToList();
                }
            }
            if (bookingData != null && reportData != null)
            {
                return new BookingProductAndReportDataResponse()
                {
                    BookingData = bookingData,
                    ReportData = reportData,
                    ReportProducts = reportProductList?.ToList(),
                    Result = BookingDataResponseResult.Success
                };
            }
            else
            {
                return new BookingProductAndReportDataResponse { Result = BookingDataResponseResult.Failure };
            }
        }

        /// <summary>
        /// get booking products by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<BookingProductsResponse> GetBookingProductsAndStatus(int bookingId)
        {
            string servicedate = string.Empty;
            // get booking products
            var bookingIds = new List<int>() { bookingId };
            var productList = await _repo.GetProductListByBooking(bookingIds);
            //take po transaction data 
            var poTransactionList = await _repo.GetBookingPoList(bookingIds);

            // get booking log status 

            var bookingLogStatus = _eventLogRepo.GetLogStatusByBooking(bookingId);

            var currentBookingStatus = productList.FirstOrDefault()?.BookingStatus;

            var currenbookingdate = productList.FirstOrDefault()?.CreatedDate?.ToString(StandardDateFormat);

            var bookinginfo = productList.FirstOrDefault();

            if (bookinginfo != null && bookinginfo.ServiceStartDate.HasValue && bookinginfo.ServiceEndDate.HasValue)
            {
                if (bookinginfo.ServiceStartDate.Value == bookinginfo.ServiceEndDate.Value)
                {
                    servicedate = bookinginfo.ServiceEndDate.Value.ToString(StandardDateFormat);
                }
                else
                {
                    servicedate = bookinginfo.ServiceStartDate.Value.ToString(StandardDateFormat) + "-" + bookinginfo.ServiceEndDate.Value.ToString(StandardDateFormat);
                }
            }

            var actualStatusList = GetBookingReportStatus(bookingId, bookingLogStatus, currentBookingStatus, currenbookingdate, servicedate);

            // set inspection Date
            foreach (var item in productList)
            {

                //if the products are combined and AQL is not selected, make the first product in the list as parent product
                var isAQLSelected = productList.Where(z => z.CombineProductId == item.CombineProductId && z.CombineAqlQuantity.GetValueOrDefault() > 0).Count();

                //apply po transaction data
                if (poTransactionList != null && poTransactionList.Any())
                {
                    var poList = poTransactionList.Where(x => x.InspectionId == item.BookingId && x.ProductRefId == item.Id).ToList();

                    item.PoNumberList = poList.Select(x => x.PoNumber).Distinct().ToList();

                    item.PoNumber = string.Join(" ,", item.PoNumberList);

                    item.PoNumberCount = item.PoNumberList.Count();
                }

                // Both date same take from date 
                if (item.ServiceStartDate == item.ServiceEndDate)
                {
                    item.InspectionDate = item?.ServiceStartDate?.ToString(StandardDateFormat);
                }
                else
                {
                    // both date not same but endate not null then take end date
                    if (item.ServiceEndDate != null)
                    {
                        item.InspectionDate = item?.ServiceEndDate?.ToString(StandardDateFormat);
                    }
                    else
                    {
                        item.InspectionDate = item?.ServiceStartDate?.ToString(StandardDateFormat);
                    }
                }

                // set image URL
                if (!string.IsNullOrEmpty(item.ProductImage))
                {
                    item.ProductImageUrl = item.ProductImage;
                }
                // set combine orders 
                item.CombineProductCount = item.CombineProductId > 0 ?
                                            productList.Where(x => x.CombineProductId ==
                                            item.CombineProductId).Count() : 1;


                item.IsParentProduct = (item.CombineProductId.GetValueOrDefault() == 0) ? true : (item.CombineAqlQuantity != null &&
                                        item.CombineAqlQuantity != 0) ? true : (isAQLSelected == 0 &&
                                        productList.Where(z => z.CombineProductId == item.CombineProductId).FirstOrDefault().ProductId == item.ProductId
                                        ? true : false);

            }

            // get booking status
            if (productList != null)
            {
                return new BookingProductsResponse()
                {
                    BookingStatusList = actualStatusList,
                    BookingProductsList = productList.
                                           OrderBy(x => x.CombineProductId).ThenByDescending(x => x.IsParentProduct).ThenBy(x => x.ProductId).ToList(),
                    Result = BookingProductsResponseResult.Success
                };

            }

            return new BookingProductsResponse() { Result = BookingProductsResponseResult.Success };
        }

        /// <summary>
        /// get only the booking products by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<BookingProductDataResponse> GetBookingProducts(int bookingId)
        {
            // get booking products
            var bookingIds = new[] { bookingId };
            var productList = await _repo.GetProductListByBooking(bookingIds);

            // set inspection Date
            foreach (var item in productList)
            {

                //if the products are combined and AQL is not selected, make the first product in the list as parent product
                var isAQLSelected = productList.Where(z => z.CombineProductId == item.CombineProductId && z.CombineAqlQuantity.GetValueOrDefault() > 0).Count();

                // Both date same take from date 
                if (item.ServiceStartDate == item.ServiceEndDate)
                {
                    item.InspectionDate = item?.ServiceStartDate?.ToString(StandardDateFormat);
                }
                else
                {
                    // both date not same but endate not null then take end date
                    if (item.ServiceEndDate != null)
                    {
                        item.InspectionDate = item?.ServiceEndDate?.ToString(StandardDateFormat);
                    }
                    else
                    {
                        item.InspectionDate = item?.ServiceStartDate?.ToString(StandardDateFormat);
                    }
                }

                // set image URL
                if (!string.IsNullOrEmpty(item.ProductImage))
                {
                    item.ProductImageUrl = item.ProductImage;
                }
                // set combine orders 
                item.CombineProductCount = item.CombineProductId > 0 ?
                                            productList.Where(x => x.CombineProductId ==
                                            item.CombineProductId).Count() : 1;


                item.IsParentProduct = (item.CombineProductId.GetValueOrDefault() == 0) ? true : (item.CombineAqlQuantity != null &&
                                        item.CombineAqlQuantity != 0) ? true : (isAQLSelected == 0 &&
                                        productList.Where(z => z.CombineProductId == item.CombineProductId).FirstOrDefault().ProductId == item.ProductId ? true : false);

            }

            // get booking status
            if (productList != null)
            {
                return new BookingProductDataResponse()
                {
                    BookingProductsList = productList.
                                           OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ThenBy(x => x.ProductName).ToList(),
                    Result = BookingProductDataResponseResult.Success
                };

            }

            return new BookingProductDataResponse() { Result = BookingProductDataResponseResult.NotAvailable };
        }


        /// <summary>
        /// get booking containers by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<BookingContainerResponse> GetBookingContainersAndStatus(int bookingId)
        {
            string servicedate = string.Empty;
            // get booking products
            var bookingIds = new[] { bookingId };
            var containerList = await _repo.GetContainerListByBooking(bookingIds);

            // get booking log status 

            var bookingLogStatus = _eventLogRepo.GetLogStatusByBooking(bookingId);

            var currentBookingStatus = containerList.FirstOrDefault()?.BookingStatus;

            var currenbookingdate = containerList.FirstOrDefault()?.CreatedDate?.ToString(StandardDateFormat);


            var bookinginfo = containerList.FirstOrDefault();
            if (bookinginfo != null && bookinginfo.ServiceStartDate.HasValue && bookinginfo.ServiceEndDate.HasValue)
            {
                if (bookinginfo.ServiceStartDate.Value == bookinginfo.ServiceEndDate.Value)
                {
                    servicedate = bookinginfo.ServiceEndDate.Value.ToString(StandardDateFormat);
                }
                else
                {
                    servicedate = bookinginfo.ServiceStartDate.Value.ToString(StandardDateFormat) + "-" + bookinginfo.ServiceEndDate.Value.ToString(StandardDateFormat);
                }
            }

            var actualStatusList = GetBookingReportStatus(bookingId, bookingLogStatus, currentBookingStatus, currenbookingdate, servicedate);


            // get booking status
            if (containerList != null)
            {

                // set inspection Date
                foreach (var item in containerList)
                {
                    // Both date same take from date 
                    if (item.ServiceStartDate == item.ServiceEndDate)
                    {
                        item.InspectionDate = item?.ServiceStartDate?.ToString(StandardDateFormat);
                    }
                    else
                    {
                        // both date not same but endate not null then take end date
                        if (item.ServiceEndDate != null)
                        {
                            item.InspectionDate = item?.ServiceEndDate?.ToString(StandardDateFormat);
                        }
                        else
                        {
                            item.InspectionDate = item?.ServiceStartDate?.ToString(StandardDateFormat);
                        }
                    }
                }


                return new BookingContainerResponse()
                {
                    BookingStatusList = actualStatusList,
                    BookingContainerList = _bookingmap.MapBookingContainerList(containerList, null),
                    Result = BookingProductsResponseResult.Success
                };

            }

            return new BookingContainerResponse() { Result = BookingProductsResponseResult.Failure };
        }


        /// <summary>
        /// get booking containers by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<BookingContainerResponse> GetBookingContainers(int bookingId)
        {
            // get booking products
            var bookingIds = new[] { bookingId };
            var containerList = await _repo.GetContainerListByBooking(bookingIds);
            var qcList = await _reportRepository.GetQCDetails(bookingId);
            // get booking status
            if (containerList != null)
            {
                return new BookingContainerResponse()
                {
                    BookingContainerList = _bookingmap.MapBookingContainerList(containerList, qcList),
                    Result = BookingProductsResponseResult.Success
                };

            }

            return new BookingContainerResponse() { Result = BookingProductsResponseResult.Failure };
        }

        /// <summary>
        /// get booking product po list by booking id and product ref id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="productRefId"></param>
        /// <returns></returns>
        public async Task<BookingProductPOResponse> GetBookingProductPoList(int bookingId, int productRefId)
        {
            // get booking po products
            var bookingIds = new[] { bookingId };
            List<BookingPoDataRequest> objPOList = new List<BookingPoDataRequest>();
            var productPoList = await _repo.GetProductPOListByBooking(bookingIds, productRefId);

            if (productPoList == null)
            {
                return new BookingProductPOResponse() { BookingProductPoList = null, Result = BookingProductPOResponseResult.NotAvailable };
            }

            foreach (var item in productPoList)
            {
                objPOList.Add(mapPOData(item));
            }

            return new BookingProductPOResponse() { BookingProductPoList = objPOList.OrderBy(x => x.PoNumber).ToList(), Result = BookingProductPOResponseResult.Success };
        }
        /// <summary>
        /// get booking po list by product and container and booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="containerRefId"></param>
        /// <param name="productRefId"></param>
        /// <returns></returns>
        public async Task<BookingProductPOResponse> GetBookingProductPoList(int bookingId, int containerRefId, int productRefId)
        {
            // get booking po products
            var bookingIds = new[] { bookingId };
            List<BookingPoDataRequest> objPOList = new List<BookingPoDataRequest>();
            var productPoList = await _repo.GetContainerPoListByBooking(bookingIds, containerRefId, productRefId);

            if (productPoList == null)
            {
                return new BookingProductPOResponse() { BookingProductPoList = null, Result = BookingProductPOResponseResult.NotAvailable };
            }

            foreach (var item in productPoList)
            {
                objPOList.Add(mapPOData(item));
            }

            return new BookingProductPOResponse() { BookingProductPoList = objPOList.OrderBy(x => x.PoNumber).ToList(), Result = BookingProductPOResponseResult.Success };
        }

        /// <summary>
        /// get booking container product list by booking and container 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="containerRefId"></param>
        /// <returns></returns>
        public async Task<BookingContainerProductResponse> GetBookingContainerProductList(int bookingId, int containerRefId)
        {

            // get booking container product list by booking and container 
            var bookingIds = new[] { bookingId };

            var containerProductList = await _repo.GetContainerProductListByBooking(bookingIds, containerRefId);

            if (containerProductList == null)
            {
                return new BookingContainerProductResponse() { BookingProductList = null, Result = BookingProductPOResponseResult.NotAvailable };
            }

            return new BookingContainerProductResponse() { BookingProductList = containerProductList.OrderBy(x => x.ProductId).ToList(), Result = BookingProductPOResponseResult.Success };
        }

        /// <summary>
        /// map po details from db
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private BookingPoDataRequest mapPOData(BookingPoData entity)
        {
            var poData = new BookingPoDataRequest()
            {
                Id = entity.Id,
                BookingId = entity.BookingId,
                DestinationCountry = entity.DestinationCountry,
                BookingQuantity = entity.BookingQuantity,
                InspectedQuantity = entity.InspectedQuantity,
                Etd = entity.Etd?.ToString(StandardDateFormat) ?? string.Empty,
                SRDate = entity.SRDate,
                IsPlaceHolderVisible = entity.IsPlaceHolderVisible,
                PoNumber = entity.PoNumber
            };

            return poData;
        }


        /// <summary>
        /// Get inspection summary list by report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<InspectionReportSummaryRepsonse> GetInspectionSummary(int reportId)
        {
            var summaryList = await _repo.GetInspectionSummary(reportId);

            var summaryPhotos = await _repo.GetInspectionSummaryPhoto(reportId);

            foreach (var item in summaryList)
            {
                item.Photos = summaryPhotos.Where(x => x.Id == item.Id).Select(x => x.Name).ToList();
            }

            if (summaryList != null)
            {
                return new InspectionReportSummaryRepsonse()
                {
                    InspectionReportSummaryList = summaryList.ToList(),
                    Result = InspectionReportSummaryRepsonseResult.Success
                };
            }

            return new InspectionReportSummaryRepsonse()
            {
                InspectionReportSummaryList = null,
                Result = InspectionReportSummaryRepsonseResult.Success
            };
        }

        /// <summary>
        /// Get Inspection Defect list by report id 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<InspectionDefectsRepsonse> GetInspectionDefects(int reportId)
        {
            var defectList = new List<InspectionReportDefects>();
            var insepctionDefectList = await _repo.GetInspectionDefects(reportId);

            var distinctDefectNames = insepctionDefectList.Select(x => x.Description).Distinct().ToList();

            foreach (var item in distinctDefectNames)
            {
                var data = insepctionDefectList.Where(x => x.Description == item);
                if (data.Any(x => x.Critical > 0))
                {
                    var list = new InspectionReportDefects()
                    {
                        Description = item,
                        Critical = data.Sum(x => x.Critical.GetValueOrDefault()),
                    };
                    defectList.Add(list);
                }
                if (data.Any(x => x.Major > 0))
                {
                    var list = new InspectionReportDefects()
                    {
                        Description = item,
                        Major = data.Sum(x => x.Major.GetValueOrDefault())
                    };
                    defectList.Add(list);
                }
                if (data.Any(x => x.Minor > 0))
                {
                    var list = new InspectionReportDefects()
                    {
                        Description = item,
                        Minor = data.Sum(x => x.Minor.GetValueOrDefault())
                    };
                    defectList.Add(list);
                }
            }

            if (insepctionDefectList != null)
            {
                return new InspectionDefectsRepsonse()
                {
                    InspectionDefectList = defectList.ToList(),
                    Result = InspectionDefectsRepsonseResult.Success
                };
            }

            return new InspectionDefectsRepsonse()
            {
                InspectionDefectList = null,
                Result = InspectionDefectsRepsonseResult.Failure
            };

        }
        /// <summary>
        /// Get Insepction Defects by report and Inspection
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="inspPoId"></param>
        /// <returns></returns>
        public async Task<InspectionDefectsRepsonse> GetInspectionDefectsByReportandInspection(int reportId, int productRefId)
        {


            var insppotransactionIds = await _repo.GetPoTransactionIdsByProductRefId(productRefId);

            var insepctionDefectList = await _repo.GetInspectionDefects(reportId, insppotransactionIds);

            if (insepctionDefectList != null)
            {
                return new InspectionDefectsRepsonse()
                {
                    InspectionDefectList = insepctionDefectList.ToList(),
                    Result = InspectionDefectsRepsonseResult.Success
                };
            }

            return new InspectionDefectsRepsonse()
            {
                InspectionDefectList = null,
                Result = InspectionDefectsRepsonseResult.Failure
            };

        }

        /// <summary>
        /// Get inspection defects by container and reports
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="containerRefId"></param>
        /// <returns></returns>
        public async Task<InspectionDefectsRepsonse> GetInspectionDefectsByReportandContainer(int reportId, int containerRefId)
        {

            var insppotransactionIds = await _repo.GetPoTransactionIdsByContainerRefId(containerRefId);

            var insepctionDefectList = await _repo.GetInspectionDefects(reportId, insppotransactionIds);

            if (insepctionDefectList != null)
            {
                return new InspectionDefectsRepsonse()
                {
                    InspectionDefectList = insepctionDefectList.ToList(),
                    Result = InspectionDefectsRepsonseResult.Success
                };
            }

            return new InspectionDefectsRepsonse()
            {
                InspectionDefectList = null,
                Result = InspectionDefectsRepsonseResult.Failure
            };
        }

        /// <summary>
        /// get booking status and data
        /// </summary>
        /// <param name="bookingLogStatus"></param>
        /// <param name="currentBookingStatus"></param>
        /// <returns></returns>
        private List<BookingRepoStatus> GetBookingReportStatus(int bookingId, List<BookingLogStatus> bookingLogStatus,
            int? currentBookingStatus, string currentDate, string servicedate, bool isCallFromEAQF = false)
        {
            var standardDateFormatTime = isCallFromEAQF ? StandardISO8601DateTimeFormat : StandardDateFormat;

            List<BookingRepoStatus> statusList = new List<BookingRepoStatus>();
            statusList.Add(new BookingRepoStatus()
            {
                Id = 1,
                StatusId = (int)BookingStatus.Received,
                StatusName = BookingStatusCustomList[1].ToString(),
                StatusDesc = "",
                IconType = BookingStatusCustomList[8].ToString(),
                StatusDate = currentDate

            });

            statusList.Add(new BookingRepoStatus()
            {
                Id = 2,
                StatusId = (int)BookingStatus.Confirmed,
                StatusName = BookingStatusCustomList[2].ToString(),
                StatusDesc = "",
                IconType = BookingStatusCustomList[9].ToString(),
                StatusDate = bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.Confirmed).
                              OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)

            });

            statusList.Add(new BookingRepoStatus()
            {
                Id = 3,
                StatusId = (int)BookingStatus.AllocateQC,
                StatusName = BookingStatusCustomList[3].ToString(),
                StatusDesc = "",
                IconType = BookingStatusCustomList[9].ToString(),
                StatusDate = bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.AllocateQC).
                              OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)

            });

            statusList.Add(new BookingRepoStatus()
            {
                Id = 4,
                StatusId = (int)BookingStatus.Inspected,
                StatusName = BookingStatusCustomList[11].ToString(),
                StatusDesc = "",
                IconType = BookingStatusCustomList[9].ToString(),
                StatusDate = servicedate
            });

            statusList.Add(new BookingRepoStatus()
            {
                Id = 5,
                StatusId = (int)BookingStatus.ReportSent,
                StatusName = BookingStatusCustomList[4].ToString(),
                StatusDesc = "",
                IconType = BookingStatusCustomList[9].ToString(),
                StatusDate = bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.ReportSent).
                           OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)

            });

            // set current status as in progress 

            var currentStatusData = statusList.FirstOrDefault(x => x.StatusId == currentBookingStatus);

            if (currentStatusData != null)
            {
                // check any item has inprogress
                var currentInprogressItemId = currentStatusData.Id + 1;
                var currentInprogerssData = statusList.FirstOrDefault(x => x.Id == currentInprogressItemId);

                if (currentInprogerssData != null)
                {
                    currentInprogerssData.StatusDesc = BookingStatusCustomList[10].ToString();
                    currentInprogerssData.StatusDate = BookingStatusCustomList[10].ToString();
                    currentInprogerssData.IconType = BookingStatusCustomList[7].ToString();

                    foreach (var item in statusList.Where(x => x.Id < currentInprogerssData.Id))
                    {
                        item.IconType = BookingStatusCustomList[8].ToString();
                        item.StatusDesc = item.StatusDate;
                        item.StatusDate = item.StatusDate ?? BookingStatusCustomList[8].ToString();
                    }


                    foreach (var item in statusList.Where(x => x.Id > currentInprogerssData.Id))
                    {
                        item.IconType = BookingStatusCustomList[9].ToString();
                        item.StatusDesc = BookingStatusCustomList[12].ToString();
                        item.StatusDate = BookingStatusCustomList[12].ToString();
                    }
                }
                else // if no item then all things done
                {
                    foreach (var item in statusList)
                    {
                        item.IconType = BookingStatusCustomList[8].ToString();
                        item.StatusDesc = item.StatusDate;
                        item.StatusDate = item.StatusDate ?? BookingStatusCustomList[8].ToString();
                    }
                }
            }
            // if the status is hold or cancel
            else if (currentBookingStatus == (int)BookingStatus.Hold || currentBookingStatus == (int)BookingStatus.Cancel)
            {
                var previousStatus = bookingLogStatus.Where(x => x.BookingId == bookingId).
                              OrderByDescending(x => x.CreatedDate).Skip(1).FirstOrDefault();

                if (previousStatus != null)
                {
                    var prevStatusData = statusList.Where(x => x.StatusId == previousStatus.StatusId)?.FirstOrDefault();

                    if (prevStatusData != null)
                    {
                        // check any item has inprogress
                        var inprogressItemId = prevStatusData.Id + 1;
                        var currentInprogerssData = statusList.FirstOrDefault(x => x.Id == inprogressItemId);

                        if (currentInprogerssData != null)
                        {
                            int index = 0;

                            foreach (var item in statusList.Where(x => x.Id < currentInprogerssData.Id))
                            {
                                item.IconType = BookingStatusCustomList[8].ToString();
                                item.StatusDesc = item.StatusDate;
                                item.StatusDate = item.StatusDate ?? BookingStatusCustomList[8].ToString();
                                index++;
                            }

                            statusList.Insert(index, new BookingRepoStatus()
                            {
                                StatusId = currentBookingStatus == (int)BookingStatus.Hold ? (int)BookingStatus.Hold : (int)BookingStatus.Cancel,
                                StatusName = currentBookingStatus == (int)BookingStatus.Hold ? BookingStatusCustomList[5].ToString() :
                                BookingStatusCustomList[6].ToString(),
                                StatusDesc = BookingStatusCustomList[10].ToString(),
                                IconType = BookingStatusCustomList[7].ToString(),
                                StatusDate = currentBookingStatus == (int)BookingStatus.Hold ? bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.Hold).
                                OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime) :
                                bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.Cancel).
                                OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)
                            });

                            foreach (var item in statusList.Where(x => x.Id >= currentInprogerssData.Id))
                            {
                                item.IconType = BookingStatusCustomList[9].ToString();
                                item.StatusDesc = BookingStatusCustomList[12].ToString();
                                item.StatusDate = BookingStatusCustomList[12].ToString();
                            }
                        }
                    }
                    else // only verified status it will come here.
                    {
                        foreach (var item in statusList.Where(x => x.Id > 1))
                        {
                            item.IconType = BookingStatusCustomList[9].ToString();
                            item.StatusDesc = "will update soon";
                            item.StatusDate = "will update soon";
                        }
                        // After Requested place hold or cancel if there is no data mapped
                        statusList.Insert(1, new BookingRepoStatus()
                        {
                            StatusId = currentBookingStatus == (int)BookingStatus.Hold ? (int)BookingStatus.Hold : (int)BookingStatus.Cancel,
                            StatusName = currentBookingStatus == (int)BookingStatus.Hold ? BookingStatusCustomList[5].ToString() :
                              BookingStatusCustomList[6].ToString(),
                            StatusDesc = BookingStatusCustomList[10].ToString(),
                            IconType = BookingStatusCustomList[7].ToString(),
                            StatusDate = currentBookingStatus == (int)BookingStatus.Hold ? bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.Hold).
                              OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime) :
                              bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.Cancel).
                              OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)
                        });
                    }
                }
                else //if cancelled in requested status
                {
                    //set the booking received to done
                    var requestedStatus = statusList.Where(x => x.StatusId == (int)BookingStatus.Received)?.FirstOrDefault();
                    requestedStatus.IconType = BookingStatusCustomList[8].ToString();
                    requestedStatus.StatusDesc = requestedStatus.StatusDate;
                    requestedStatus.StatusDate = requestedStatus.StatusDate ?? BookingStatusCustomList[8].ToString();

                    //set the second status at index 1 to cancel
                    statusList.Insert(1, new BookingRepoStatus()
                    {
                        StatusId = (int)BookingStatus.Cancel,
                        StatusName = BookingStatusCustomList[6].ToString(),
                        StatusDesc = BookingStatusCustomList[10].ToString(),
                        IconType = BookingStatusCustomList[7].ToString(),
                        StatusDate = bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.Cancel).
                                OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)
                    });

                    //set the remaining status to not started
                    foreach (var item in statusList.Where(x => x.Id > 1))
                    {
                        item.IconType = BookingStatusCustomList[9].ToString();
                        item.StatusDesc = BookingStatusCustomList[12].ToString();
                        item.StatusDate = BookingStatusCustomList[12].ToString();
                    }

                }

            }

            else // verified state
            {
                foreach (var item in statusList.Where(x => x.Id > 1))
                {
                    item.IconType = BookingStatusCustomList[9].ToString();
                    item.StatusDesc = "will update soon";
                    item.StatusDate = "will update soon";
                }
            }

            return statusList;
        }


        private async Task<SetInspNotifyResponse> ToRequestBooking(int bookingId, bool isCombineOrderDataChanged, SaveInsepectionRequest request)
        {
            var response = new SetInspNotifyResponse();
            response.BookingId = bookingId;

            var bookingData = await _repo.GetInspectionCustomerContactByID(bookingId);
            response.StatusId = bookingData.StatusId;
            response.IsEdit = isEdit;

            if (bookingData.Id > 0)
            {
                //Mark the split booking task as done
                var requestSplitTask = await UpdateTask(bookingData.Id, new[] { (int)TaskType.SplitInspectionBooking }, false, true);

                var requestTask = await UpdateTask(bookingData.Id, new[] { (int)TaskType.VerifyInspection }, false, true);


                if ((requestSplitTask != null && requestSplitTask.Any()) || (requestTask != null && requestTask.Any()))
                {
                    response.StatusName = BookingStatusNames.Modified.ToString(); //"Modified";
                    isEdit = true;

                    if (requestTask != null && requestTask.Any())
                    {
                        // send email to customer if the internal user modified and enabled the checkbox or customer created and modified the booking 
                        if ((_ApplicationContext.UserType == UserTypeEnum.InternalUser && request.IsCustomerEmailSend) ||
                            (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer &&
                            _ApplicationContext.UserType == UserTypeEnum.Customer))
                        {
                            response.CustomerEmail = bookingData.InspTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                                Aggregate((x, y) => x + ";" + y);
                        }
                    }
                }
                else
                {
                    response.StatusName = BookingStatusNames.Requested.ToString();  //"requested";
                    isEdit = false;
                    if (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer || request.IsCustomerEmailSend)
                    {
                        response.CustomerEmail = bookingData.InspTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                            Aggregate((x, y) => x + ";" + y);
                    }
                }

                //Get product category details
                var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
                //Get Department details
                var departmentData = await _repo.GetBookingDepartmentList(new[] { bookingId });
                //Get Brand details
                var brandData = await _repo.GetBookingBrandList(new[] { bookingId });

                //factory country 
                int? factoryCountryId = null;
                if (request.FactoryId.HasValue)
                {
                    var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(request.FactoryId.Value);
                    if (factoryCountryData.Result == SupplierListResult.Success)
                        factoryCountryId = factoryCountryData.countryId;
                }


                var userAccessFilter = new UserAccess
                {
                    OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                    ServiceId = (int)Service.InspectionId,
                    CustomerId = bookingData.CustomerId,
                    RoleId = (int)RoleEnum.InspectionRequest,
                    ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                    DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    FactoryCountryId = factoryCountryId
                };

                var userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                var toRecipients = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                // Add Task 
                await AddTask(bookingData.SplitPreviousBookingNo != null ? TaskType.SplitInspectionBooking :
                    TaskType.VerifyInspection, bookingId, (int)RoleEnum.InspectionVerified, toRecipients.Select(x => x.Id),
                    request.UserId > 0 ? request.UserId : _ApplicationContext.UserId);

                if (userListByRoleAccess != null && userListByRoleAccess.Count() > 0)
                {
                    if (isEdit)
                    {
                        await AddNotification(NotificationType.InspectionModified, bookingId, userListByRoleAccess.Select(x => x.Id));
                    }

                    else
                    {
                        await AddNotification(NotificationType.InspectionRequested, bookingId, userListByRoleAccess.Select(x => x.Id));
                    }
                }
                response.UserList = userListByRoleAccess;
                response.ToRecipients = toRecipients;
            }
            return response;
        }

        private async Task<SetInspNotifyResponse> ToVerifyBooking(int bookingId, bool isCombineOrderDataChanged, SaveInsepectionRequest request)
        {
            var response = new SetInspNotifyResponse();
            bool brandCheckpoint = false;
            bool deptCheckpoint = false;
            bool serviceTypeCheckpoint = false;
            response.BookingId = bookingId;
            response.StatusName = BookingStatusNames.Verified.ToString(); // "Verified";

            var bookingData = await _repo.GetInspectionCustomerContactByID(bookingId);
            response.StatusId = bookingData.StatusId;
            IEnumerable<User> toRecipients = new List<User>();

            var quotCount = await _repo.GetCustomersByCustomerId(bookingData.CustomerId);
            bool checkpointExists = quotCount != null && quotCount.Id > 0;

            //if no brand or dept or service type is selected, then checkpoint is applied to all the brands, depts and service types
            if (checkpointExists)
            {
                brandCheckpoint = true;
                deptCheckpoint = true;
                serviceTypeCheckpoint = true;

                if (quotCount.CuCheckPointsBrands != null && quotCount.CuCheckPointsBrands.Any())
                {
                    brandCheckpoint = request.InspectionCustomerBrandList.Any(x => quotCount.CuCheckPointsBrands.Any(y => y.Active && y.BrandId == x));
                }

                if (quotCount.CuCheckPointsDepartments != null && quotCount.CuCheckPointsDepartments.Any())
                {
                    deptCheckpoint = request.InspectionCustomerDepartmentList.Any(x => quotCount.CuCheckPointsDepartments.Any(y => y.Active && y.DeptId == x));
                }

                if (quotCount.CuCheckPointsServiceTypes != null && quotCount.CuCheckPointsServiceTypes.Any())
                {
                    serviceTypeCheckpoint = quotCount.CuCheckPointsServiceTypes.Where(x => x.Active).Select(x => x.ServiceTypeId).Contains(request.ServiceTypeId);
                }

                if (!brandCheckpoint || !deptCheckpoint || !serviceTypeCheckpoint)
                {
                    checkpointExists = false;
                }
            }

            // Check the quotation required logic in customer checkpoint module.
            // if yes send the task to quotation team.
            // if no send the task to schedule team.

            //Mark the request task done
            var requestTask = await UpdateTask(bookingData.Id, new[] { (int)TaskType.VerifyInspection }, false, true);

            //Mark the split booking task as done
            var requestSplitTask = await UpdateTask(bookingData.Id, new[] { (int)TaskType.SplitInspectionBooking }, false, true);

            //Get product category details
            var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });

            //Get Department details
            var departmentData = await _repo.GetBookingDepartmentList(new[] { bookingId });
            //Get Brand details
            var brandData = await _repo.GetBookingBrandList(new[] { bookingId });

            //factory country 
            int? factoryCountryId = null;
            if (request.FactoryId.HasValue)
            {
                var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(request.FactoryId.Value);
                if (factoryCountryData.Result == SupplierListResult.Success)
                    factoryCountryId = factoryCountryData.countryId;
            }

            var userAccessFilter = new UserAccess
            {
                OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                ServiceId = (int)Service.InspectionId,
                CustomerId = bookingData.CustomerId,
                RoleId = (int)RoleEnum.QuotationRequest,
                ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };

            if (checkpointExists && (requestTask != null && requestTask.Any()) || (requestSplitTask != null && requestSplitTask.Any()))
            {
                toRecipients = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                //Add Task to create quotation if customer has quotaion required checkpoint
                await AddTask(TaskType.QuotationPending, bookingId, (int)RoleEnum.QuotationRequest, toRecipients.Select(x => x.Id));

                response.ToRecipients = toRecipients;
            }


            if ((requestSplitTask != null && requestSplitTask.Any()) || (requestTask != null && requestTask.Any()))
            {
                userAccessFilter.RoleId = (int)RoleEnum.InspectionConfirmed;

                var getConfirmedUsers = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                toRecipients = toRecipients.Concat(getConfirmedUsers);

                // Add Task 
                await AddTask(TaskType.ConfirmInspection, bookingId, (int)RoleEnum.InspectionConfirmed, getConfirmedUsers.Select(x => x.Id));

                //Fetch users for email and notification
                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                var userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);


                if (userListByRoleAccess != null && userListByRoleAccess.Count() > 0)
                {
                    await AddNotification(NotificationType.InspectionVerified, bookingId, userListByRoleAccess.Select(x => x.Id));
                }
                response.UserList = userListByRoleAccess;
                response.ToRecipients = toRecipients;
            }

            else
            {
                response.StatusName = BookingStatusNames.Modified.ToString();
                response.IsEdit = true;
                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                var verifyUserRole = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                if (isCombineOrderDataChanged)
                {
                    var quotationUsers = await BookingQuotationExists(bookingId, bookingData.OfficeId, bookingData.CustomerId, NotificationType.BookingQuantityChange);
                    await AddNotification(NotificationType.BookingQuantityChange, bookingId, verifyUserRole.Select(x => x.Id));
                    if (quotationUsers.Any())
                    {
                        verifyUserRole = verifyUserRole.Concat(quotationUsers);
                    }
                    response.ToRecipients = verifyUserRole;
                }
                else
                {
                    response.ToRecipients = verifyUserRole;
                    await AddNotification(NotificationType.InspectionModified, bookingId, verifyUserRole.Select(x => x.Id));
                }
            }
            return response;
        }

        private async Task<SetInspNotifyResponse> ToConfirmBooking(int bookingId, bool isCombineOrderDataChanged, SaveInsepectionRequest request)
        {

            var response = new SetInspNotifyResponse();
            bool brandCheckpoint = false;
            bool deptCheckpoint = false;
            bool serviceTypeCheckpoint = false;
            response.BookingId = bookingId;
            var bookingData = await _repo.GetInspectionCustomerContactByID(bookingId);
            response.StatusId = bookingData.StatusId;
            response.StatusName = BookingStatusNames.Confirmed.ToString();

            var quotaCount = await _repo.GetCustomersByCustomerId(bookingData.CustomerId);
            bool checkpointExists = quotaCount != null && quotaCount.Id > 0;

            //if no brand or dept or service type is selected, then checkpoint is applied to all the brands, depts and service types
            if (checkpointExists)
            {
                brandCheckpoint = true;
                deptCheckpoint = true;
                serviceTypeCheckpoint = true;


                if (quotaCount.CuCheckPointsBrands != null && quotaCount.CuCheckPointsBrands.Any())
                {
                    brandCheckpoint = request.InspectionCustomerBrandList.Any(x => quotaCount.CuCheckPointsBrands.Any(y => y.BrandId == x));
                }

                if (quotaCount.CuCheckPointsDepartments != null && quotaCount.CuCheckPointsDepartments.Any())
                {
                    deptCheckpoint = request.InspectionCustomerDepartmentList.Any(x => quotaCount.CuCheckPointsDepartments.Any(y => y.DeptId == x));
                }

                if (quotaCount.CuCheckPointsServiceTypes != null && quotaCount.CuCheckPointsServiceTypes.Any())
                {
                    serviceTypeCheckpoint = quotaCount.CuCheckPointsServiceTypes.Select(x => x.ServiceTypeId).Contains(request.ServiceTypeId);
                }

                if (!brandCheckpoint || !deptCheckpoint || !serviceTypeCheckpoint)
                {
                    checkpointExists = false;
                }
            }

            // Update task
            //await _userManager.EditTask(x => x.LinkId == bookingId && x.TaskTypeId == (int)TaskType.VerifyInspection
            //            && !x.IsDone, (x) => { x.IsDone = true; });

            // Mark the Verified task Done
            var confirmTask = await UpdateTask(bookingData.Id, new[] { (int)TaskType.ConfirmInspection }, false, true);

            //get product category details
            var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
            //Get Department details
            var departmentData = await _repo.GetBookingDepartmentList(new[] { bookingId });
            //Get Brand details
            var brandData = await _repo.GetBookingBrandList(new[] { bookingId });

            //get factory country id by factory id for filter the data access users
            int? factoryCountryId = null;
            if (request.FactoryId.HasValue)
            {
                var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(request.FactoryId.Value);
                if (factoryCountryData.Result == SupplierListResult.Success)
                    factoryCountryId = factoryCountryData.countryId;
            }

            var userAccessFilter = new UserAccess
            {
                OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                ServiceId = (int)Service.InspectionId,
                CustomerId = bookingData.CustomerId,
                RoleId = (int)RoleEnum.InspectionScheduled,
                ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };
            response.ToRecipients = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            //logic is for non quotation customers.
            if ((confirmTask != null && confirmTask.Any()) && !checkpointExists)
            {
                //Add Task to the scheduled team if no customer checkpoint for quotation
                var scheduleTask = await _repo.GetTask(bookingData.Id, new[] { (int)TaskType.ScheduleInspection }, false);

                if (scheduleTask == null || scheduleTask.Count() == 0)
                {
                    //Add Task
                    await AddTask(TaskType.ScheduleInspection, bookingId, (int)RoleEnum.InspectionScheduled, response.ToRecipients.Select(x => x.Id));
                }
            }

            if (confirmTask != null && confirmTask.Any())
            {
                userAccessFilter.RoleId = (int)RoleEnum.InspectionConfirmed;
                var userListbyRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                userListbyRoleAccess = userListbyRoleAccess.Concat(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));

                // send notification
                if (userListbyRoleAccess != null && userListbyRoleAccess.Count() > 0)
                {
                    await AddNotification(NotificationType.InspectionConfirmed, bookingId, userListbyRoleAccess.Select(x => x.Id));
                }

                //Check if Quotation is created
                if (checkpointExists)
                {
                    var bookingdates = await _repo.GetLastServiceDate(bookingId);
                    if (bookingdates.ServiceDateFrom.Date.CompareTo(bookingData.ServiceDateFrom.Date) != 0 || bookingdates.ServiceDateTo.Date.CompareTo(bookingData.ServiceDateTo.Date) != 0)
                    {
                        var quotationUsers = await BookingQuotationExists(bookingId, bookingData.OfficeId, bookingData.CustomerId, NotificationType.InspectionRescheduled);

                        if (quotationUsers.Any())
                        {
                            userListbyRoleAccess = userListbyRoleAccess.Concat(quotationUsers);
                            response.quotationExists = true;
                        }
                    }

                    if (response.quotationExists)
                    {
                        //Add Task to the scheduled team if no customer checkpoint for quotation
                        var scheduleTask = await _repo.GetTask(bookingData.Id, new[] { (int)TaskType.ScheduleInspection }, false);

                        if (scheduleTask == null || scheduleTask.Count() == 0)
                        {
                            //Add Task
                            await AddTask(TaskType.ScheduleInspection, bookingId, (int)RoleEnum.InspectionScheduled, response.ToRecipients.Select(x => x.Id));
                        }
                    }
                }

                if (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer || request.IsCustomerEmailSend)
                {
                    response.CustomerEmail = bookingData.InspTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                        Aggregate((x, y) => x + ";" + y);
                }

                response.UserList = userListbyRoleAccess;
            }
            else
            {
                response.StatusName = BookingStatusNames.Modified.ToString();
                response.IsEdit = true;
                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                var verifyUserRole = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                if (isCombineOrderDataChanged)
                {
                    var quotationUsers = await BookingQuotationExists(bookingId, bookingData.OfficeId, bookingData.CustomerId, NotificationType.BookingQuantityChange);
                    await AddNotification(NotificationType.BookingQuantityChange, bookingId, verifyUserRole.Select(x => x.Id));
                    if (quotationUsers.Any())
                    {
                        verifyUserRole = verifyUserRole.Concat(quotationUsers);
                    }
                    response.ToRecipients = verifyUserRole;
                }
                else
                {
                    response.ToRecipients = verifyUserRole;
                    await AddNotification(NotificationType.InspectionModified, bookingId, verifyUserRole.Select(x => x.Id));
                }
                if (request.IsCustomerEmailSend)
                {
                    response.CustomerEmail = bookingData.InspTranCuContacts.Where(x => x.Active).Select(x => x.Contact.Email).Distinct().ToList().
                        Aggregate((x, y) => x + ";" + y);
                }
            }

            if (response.ToRecipients != null && response.ToRecipients.Any(x => x.OnsiteEmail != null))
            {
                foreach (var item in response.ToRecipients)
                {
                    if (!string.IsNullOrEmpty(item.OnsiteEmail) && IsValidEmail(item.OnsiteEmail))
                    {
                        item.EmailAddress = item.OnsiteEmail;
                    }
                }
            }

            if (response.UserList != null && response.UserList.Any(x => x.OnsiteEmail != null))
            {
                foreach (var item in response.UserList)
                {
                    if (!string.IsNullOrEmpty(item.OnsiteEmail) && IsValidEmail(item.OnsiteEmail))
                    {
                        item.EmailAddress = item.OnsiteEmail;
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// get confirmation email users
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SetInspNotifyResponse> GetConfirmEmailUsers(int bookingId, SaveInsepectionRequest request)
        {
            var response = new SetInspNotifyResponse();
            bool brandCheckpoint = false;
            bool deptCheckpoint = false;
            bool serviceTypeCheckpoint = false;
            response.BookingId = bookingId;
            var bookingData = await _repo.GetInspectionCustomerContactByID(bookingId);
            response.StatusId = bookingData.StatusId;
            response.StatusName = BookingStatusNames.Confirmed.ToString();

            var quotaCount = await _repo.GetCustomersByCustomerId(bookingData.CustomerId);
            bool checkpointExists = quotaCount != null && quotaCount.Id > 0;

            //if no brand or dept or service type is selected, then checkpoint is applied to all the brands, depts and service types
            if (checkpointExists)
            {
                brandCheckpoint = true;
                deptCheckpoint = true;
                serviceTypeCheckpoint = true;

                if (quotaCount.CuCheckPointsBrands != null && quotaCount.CuCheckPointsBrands.Any())
                {
                    brandCheckpoint = request.InspectionCustomerBrandList.Any(x => quotaCount.CuCheckPointsBrands.Any(y => y.BrandId == x));
                }

                if (quotaCount.CuCheckPointsDepartments != null && quotaCount.CuCheckPointsDepartments.Any())
                {
                    deptCheckpoint = request.InspectionCustomerDepartmentList.Any(x => quotaCount.CuCheckPointsDepartments.Any(y => y.DeptId == x));
                }

                if (quotaCount.CuCheckPointsServiceTypes != null && quotaCount.CuCheckPointsServiceTypes.Any())
                {
                    serviceTypeCheckpoint = quotaCount.CuCheckPointsServiceTypes.Select(x => x.ServiceTypeId).Contains(request.ServiceTypeId);
                }

                if (!brandCheckpoint || !deptCheckpoint || !serviceTypeCheckpoint)
                {
                    checkpointExists = false;
                }
            }


            //get product category details
            var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
            //Get Department details
            var departmentData = await _repo.GetBookingDepartmentList(new[] { bookingId });
            //Get Brand details
            var brandData = await _repo.GetBookingBrandList(new[] { bookingId });

            //get factory country id by factory id for filter the data access users
            int? factoryCountryId = null;
            if (request.FactoryId.HasValue)
            {
                var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(request.FactoryId.Value);
                if (factoryCountryData.Result == SupplierListResult.Success)
                    factoryCountryId = factoryCountryData.countryId;
            }

            var userAccessFilter = new UserAccess
            {
                OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                ServiceId = (int)Service.InspectionId,
                CustomerId = bookingData.CustomerId,
                RoleId = (int)RoleEnum.InspectionScheduled,
                ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };
            response.ToRecipients = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            userAccessFilter.RoleId = (int)RoleEnum.InspectionConfirmed;
            var userListbyRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
            userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
            userListbyRoleAccess = userListbyRoleAccess.Concat(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));


            //Check if Quotation is created
            if (checkpointExists)
            {
                var bookingdates = await _repo.GetLastServiceDate(bookingId);
                if (bookingdates.ServiceDateFrom.Date.CompareTo(bookingData.ServiceDateFrom.Date) != 0 || bookingdates.ServiceDateTo.Date.CompareTo(bookingData.ServiceDateTo.Date) != 0)
                {
                    var quotationUsers = await BookingQuotationExists(bookingId, bookingData.OfficeId, bookingData.CustomerId, NotificationType.InspectionRescheduled);

                    if (quotationUsers.Any())
                    {
                        userListbyRoleAccess = userListbyRoleAccess.Concat(quotationUsers);
                        response.quotationExists = true;
                    }
                }
            }

            if (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer || request.IsCustomerEmailSend)
            {
                response.CustomerEmail = bookingData.InspTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                    Aggregate((x, y) => x + ";" + y);
            }

            response.UserList = userListbyRoleAccess;

            if (response.ToRecipients != null && response.ToRecipients.Any(x => x.OnsiteEmail != null))
            {
                foreach (var item in response.ToRecipients)
                {
                    if (!string.IsNullOrEmpty(item.OnsiteEmail) && IsValidEmail(item.OnsiteEmail))
                    {
                        item.EmailAddress = item.OnsiteEmail;
                    }
                }
            }

            if (response.UserList != null && response.UserList.Any(x => x.OnsiteEmail != null))
            {
                foreach (var item in response.UserList)
                {
                    if (!string.IsNullOrEmpty(item.OnsiteEmail) && IsValidEmail(item.OnsiteEmail))
                    {
                        item.EmailAddress = item.OnsiteEmail;
                    }
                }
            }
            return response;
        }

        private async Task<SetInspNotifyResponse> ToScheduleBooking(int bookingId, bool isCombineOrderDataChanged, SaveInsepectionRequest request)
        {
            var response = new SetInspNotifyResponse();
            response.BookingId = bookingId;

            var bookingData = await _repo.GetInspectionCustomerContactByID(bookingId);
            response.StatusId = bookingData.StatusId;
            response.StatusName = BookingStatusNames.Scheduled.ToString(); // "Scheduled";

            // Update Task 
            await UpdateTask(bookingData.Id, new[] { (int)TaskType.ScheduleInspection }, false, true);

            return response;
        }

        private async Task<SetInspNotifyResponse> ToCancelBooking(int bookingId, bool isCombineOrderDataChanged, SaveInsepectionRequest request)
        {
            var response = new SetInspNotifyResponse();
            response.BookingId = bookingId;
            var bookingData = await _repo.GetInspectionCustomerContactByID(bookingId);

            response.StatusId = bookingData.StatusId;
            response.StatusName = BookingStatusNames.Cancelled.ToString();  //"Cancelled";
            IEnumerable<User> userListByRoleAccess = new List<User>();

            int lastStatus = await _repo.GetLastStatus(bookingId);

            if (lastStatus > 0)
            {
                //Mark the task as done
                await UpdateTask(bookingId, new[] { (int)TaskType.VerifyInspection, (int)TaskType.ConfirmInspection, (int)TaskType.ScheduleInspection, (int)TaskType.QuotationPending }, false, true);

                //MidTask requestTask = await _repo.GetLastTask(bookingId);
            }

            //get product category details
            var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
            //Get Department details
            var departmentData = await _repo.GetBookingDepartmentList(new[] { bookingId });
            //Get Brand details
            var brandData = await _repo.GetBookingBrandList(new[] { bookingId });

            response.OfficeId = bookingData.OfficeId;

            //factory country 
            int? factoryCountryId = null;
            if (request.FactoryId.HasValue)
            {
                var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(request.FactoryId.Value);
                if (factoryCountryData.Result == SupplierListResult.Success)
                    factoryCountryId = factoryCountryData.countryId;
            }

            var userAccessFilter = new UserAccess
            {
                OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                ServiceId = (int)Service.InspectionId,
                CustomerId = bookingData.CustomerId,
                RoleId = (int)RoleEnum.InspectionConfirmed,
                ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };
            if (lastStatus != ((int)BookingStatus.Received) && lastStatus != ((int)BookingStatus.Verified))
            {
                userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                userListByRoleAccess = userListByRoleAccess.Concat(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
            }
            else if (lastStatus == ((int)BookingStatus.Received))
            {
                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
            }
            else if (lastStatus == ((int)BookingStatus.Verified))
            {
                userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                userListByRoleAccess = userListByRoleAccess.Concat(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
            }

            if (userListByRoleAccess != null && userListByRoleAccess.Count() > 0)
            {
                await AddNotification(NotificationType.InspectionCancelled, bookingId, userListByRoleAccess.Select(x => x.Id));
            }

            if (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer)
            {
                response.CustomerEmail = bookingData.InspTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                    Aggregate((x, y) => x + ";" + y);
            }
            response.ToRecipients = userListByRoleAccess;
            return response;
        }

        private async Task<SetInspNotifyResponse> ToRescheduleBooking(int bookingId, bool isCombineOrderDataChanged, SaveInsepectionRequest request)
        {
            var response = new SetInspNotifyResponse();
            response.BookingId = bookingId;
            var bookingData = await _repo.GetInspectionCustomerContactByID(bookingId);

            response.StatusId = bookingData.StatusId;
            IEnumerable<User> userListByRoleAccess = new List<User>();
            response.StatusName = BookingStatusNames.Rescheduled.ToString(); // "Rescheduled";

            int lastStatus = await _repo.GetLastStatus(bookingId);

            //Mark the task done
            await UpdateTask(bookingId, new[] { (int)TaskType.VerifyInspection, (int)TaskType.ConfirmInspection }, false, true);

            //get product category details
            var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
            //Get Department details
            var departmentData = await _repo.GetBookingDepartmentList(new[] { bookingId });
            //Get Brand details
            var brandData = await _repo.GetBookingBrandList(new[] { bookingId });

            //factory country 

            int? factoryCountryId = null;
            if (request.FactoryId.HasValue)
            {
                var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(request.FactoryId.Value);
                if (factoryCountryData.Result == SupplierListResult.Success)
                    factoryCountryId = factoryCountryData.countryId;
            }

            var userAccessFilter = new UserAccess
            {
                OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                ServiceId = (int)Service.InspectionId,
                CustomerId = bookingData.CustomerId,
                RoleId = (int)RoleEnum.InspectionVerified,
                ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };
            // _repo.Save(task, false);

            userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            userAccessFilter.RoleId = (int)RoleEnum.InspectionConfirmed;

            response.ToRecipients = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            //Add Task
            await AddTask(TaskType.ConfirmInspection, bookingId, (int)RoleEnum.InspectionConfirmed, response?.ToRecipients?.Select(x => x.Id));

            //Check if Quotation is created
            var quotationDetails = await _cancelBookingRepository.BookingQuotationExists(bookingId);
            if (quotationDetails != null)
            {
                int[] statusIdList = new int[] { (int)QuotationStatus.CustomerValidated, (int)QuotationStatus.CustomerRejected, (int)QuotationStatus.SentToClient };
                userAccessFilter = new UserAccess
                {
                    OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                    ServiceId = (int)Service.InspectionId,
                    CustomerId = bookingData.CustomerId,
                    RoleId = (int)RoleEnum.QuotationRequest,
                    ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                    DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    FactoryCountryId = factoryCountryId
                };
                userListByRoleAccess = userListByRoleAccess.Concat(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
                response.quotationExists = true;

                if ((int)QuotationPaidBy.customer == quotationDetails.IdQuotationNavigation.BillingPaidById)
                {
                    //get all(Customer, Supplier, Factory, Internal User) the email check box enable from quotation page
                    if (response.ToRecipients != null)
                    {
                        response.ToRecipients = response.ToRecipients.Concat(await _quotationRepository.CustomerEmailIdQuotation(quotationDetails.IdQuotation, statusIdList));
                    }
                }
                else if ((int)QuotationPaidBy.factory == quotationDetails.IdQuotationNavigation.BillingPaidById)
                {
                    if (response.ToRecipients != null)
                    {
                        response.ToRecipients = response.ToRecipients.Concat(await _quotationRepository.FactoryEmailIdQuotation(quotationDetails.IdQuotation, statusIdList));
                    }
                }
                else if ((int)QuotationPaidBy.supplier == quotationDetails.IdQuotationNavigation.BillingPaidById)
                {
                    if (response.ToRecipients != null)
                    {
                        response.ToRecipients = response.ToRecipients.Concat(await _quotationRepository.SupplierEmailIdQuotation(quotationDetails.IdQuotation, statusIdList));
                    }
                }
                response.QuotationId = quotationDetails.IdQuotation;
            }

            if (userListByRoleAccess != null && userListByRoleAccess.Count() > 0)
            {
                await AddNotification(NotificationType.InspectionRescheduled, bookingId, userListByRoleAccess.Select(x => x.Id));
            }

            if (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer)
            {
                response.CustomerEmail = bookingData.InspTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                    Aggregate((x, y) => x + ";" + y);
            }

            response.UserList = userListByRoleAccess;
            return response;
        }

        /// <summary>
        /// Hold booking notification
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="isCombineOrderDataChanged"></param>
        /// <returns></returns>
        private async Task<SetInspNotifyResponse> ToHoldBooking(int bookingId, bool isCombineOrderDataChanged, SaveInsepectionRequest request)
        {
            var response = new SetInspNotifyResponse();
            IEnumerable<User> userListByRoleAccess = new List<User>();

            // set booking id in the response
            response.BookingId = bookingId;

            // get booking customer contacts
            var bookingData = await _repo.GetInspectionCustomerContactByID(bookingId);

            // set current booking status id and name
            response.StatusId = bookingData.StatusId;
            response.StatusName = BookingStatusNames.Hold.ToString(); // "Hold";     

            // create task for confirm from while doing hold status
            int lastStatus = await _repo.GetLastStatus(bookingId);

            //get product category details
            var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
            //Get Department details
            var departmentData = await _repo.GetBookingDepartmentList(new[] { bookingId });
            //Get Brand details
            var brandData = await _repo.GetBookingBrandList(new[] { bookingId });

            //factory country 
            int? factoryCountryId = null;
            if (request.FactoryId.HasValue)
            {
                var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(request.FactoryId.Value);
                if (factoryCountryData.Result == SupplierListResult.Success)
                    factoryCountryId = factoryCountryData.countryId;
            }

            var userAccessFilter = new UserAccess
            {
                OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                ServiceId = (int)Service.InspectionId,
                CustomerId = bookingData.CustomerId,
                RoleId = (int)RoleEnum.InspectionVerified,
                ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };

            // get and set Request Role
            userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            userAccessFilter.RoleId = (int)RoleEnum.InspectionConfirmed;

            var getConfirmedUsers = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
            // set Confirmed Role
            userListByRoleAccess = userListByRoleAccess.Concat(getConfirmedUsers);

            response.ToRecipients = userListByRoleAccess;

            // Mark the last task done and create new confirmed task
            if (lastStatus == (int)BookingStatus.Confirmed || lastStatus == (int)BookingStatus.AllocateQC)
            {
                //Mark the task done
                await UpdateTask(bookingId, new[] { (int)TaskType.VerifyInspection, (int)TaskType.ConfirmInspection }, false, true);

                //Add Task
                await AddTask(TaskType.ConfirmInspection, bookingId, (int)RoleEnum.InspectionConfirmed, getConfirmedUsers.Select(x => x.Id));
            }

            //Check if Quotation is created
            var quotationDetails = await _cancelBookingRepository.BookingQuotationExists(bookingId);
            if (quotationDetails != null)
            {
                int[] statusIdList = new int[] { (int)QuotationStatus.CustomerValidated, (int)QuotationStatus.CustomerRejected, (int)QuotationStatus.SentToClient };
                userAccessFilter = new UserAccess
                {
                    OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                    ServiceId = (int)Service.InspectionId,
                    CustomerId = bookingData.CustomerId,
                    RoleId = (int)RoleEnum.QuotationRequest,
                    ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                    DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    FactoryCountryId = factoryCountryId
                };
                userListByRoleAccess = userListByRoleAccess.Concat(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
                response.quotationExists = true;

                if ((int)QuotationPaidBy.customer == quotationDetails.IdQuotationNavigation.BillingPaidById)
                {
                    //get all(Customer, Supplier, Factory, Internal User) the email check box enable from quotation page
                    if (response.ToRecipients != null)
                    {
                        response.ToRecipients = response.ToRecipients.Concat(await _quotationRepository.CustomerEmailIdQuotation(quotationDetails.IdQuotation, statusIdList));
                    }
                }
                else if ((int)QuotationPaidBy.factory == quotationDetails.IdQuotationNavigation.BillingPaidById)
                {
                    if (response.ToRecipients != null)
                    {
                        response.ToRecipients = response.ToRecipients.Concat(await _quotationRepository.FactoryEmailIdQuotation(quotationDetails.IdQuotation, statusIdList));

                    }
                }
                else if ((int)QuotationPaidBy.supplier == quotationDetails.IdQuotationNavigation.BillingPaidById)
                {
                    if (response.ToRecipients != null)
                    {
                        response.ToRecipients = response.ToRecipients.Concat(await _quotationRepository.SupplierEmailIdQuotation(quotationDetails.IdQuotation, statusIdList));

                    }
                }
                response.QuotationId = quotationDetails.IdQuotation;
            }

            if (userListByRoleAccess != null && userListByRoleAccess.Count() > 0)
            {
                await AddNotification(NotificationType.InspectionHold, bookingId, userListByRoleAccess.Select(x => x.Id));
            }

            if (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer || request.IsCustomerEmailSend)
            {
                response.CustomerEmail = bookingData.InspTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                    Aggregate((x, y) => x + ";" + y);
            }

            response.UserList = response.ToRecipients;
            return response;
        }

        /// <summary>
        /// Remove qc and cs once the booking is hold
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        private async Task RemoveQCAndCSFromSchedule(int bookingId)
        {
            try
            {

                //get CS list from sch_schedule_cs table using booking id
                var csList = await _schRepo.GetCSDetails(bookingId);

                await _scheduleManager.UpdateScheduleQcMandayOnCancelReschedule(bookingId, false);

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

        //update destination country and factory reference to purchaseorder detail when we updated from booking
        private void UpdateCountryAndFactoryRefWithPO(InspPoTransaction inspectionPODetail, int? destinationCountryID, string factoryReference, DateObject etd)
        {
            if (inspectionPODetail != null)
            {
                var purchaseOrderDetail = inspectionPODetail.PoDetail;
                purchaseOrderDetail.DestinationCountryId = destinationCountryID;
                // purchaseOrderDetail.Product.FactoryReference = factoryReference;
                purchaseOrderDetail.Etd = etd?.ToDateTime();
            }
        }



        /// <summary>
        /// To update inspection booking for Confirm Role Users
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private async Task UpdateConfirmInspectionBooking(InspTransaction entity, SaveInsepectionRequest request, int userId, SaveInspectionBookingResponse response)
        {
            AddInspectionStatusLog(request, entity);

            entity.CusBookingComments = request.CusBookingComments;
            entity.ServiceDateFrom = request.ServiceDateFrom.ToDateTime();
            entity.ServiceDateTo = request.ServiceDateTo.ToDateTime();
            entity.InternalComments = request.InternalComments;
            entity.QcbookingComments = request.QCBookingComments?.Trim();
            entity.ApiBookingComments = request.ApiBookingComments?.Trim();
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
            entity.StatusId = request.StatusId;
            await _repo.EditInspectionBooking(entity);
            response.Id = entity.Id;
            response.Result = SaveInspectionBookingResult.Success;
        }

        /// <summary>
        /// Update the hold status and reason
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task UpdateHoldInspectionBooking(InspTransaction entity, SaveInsepectionRequest request, int userId, SaveInspectionBookingResponse response)
        {
            AddInspectionStatusLog(request, entity);

            AddInspectionHoldReason(entity, request);

            entity.CusBookingComments = request.CusBookingComments;
            entity.InternalComments = request.InternalComments;
            entity.QcbookingComments = request.QCBookingComments.Trim();
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
            entity.StatusId = request.StatusId;

            await RemoveQCAndCSFromSchedule(entity.Id);

            await _repo.EditInspectionBooking(entity);
            response.Id = entity.Id;
            response.Result = SaveInspectionBookingResult.Success;
        }

        /// <summary>
        /// add inspection booking hold reason 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private void AddInspectionHoldReason(InspTransaction entity, SaveInsepectionRequest request)
        {
            entity.InspTranHoldReasons.Add(new InspTranHoldReason()
            {
                ReasonType = request.HoldReasonTypeId,
                Comment = request.HoldReason,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                Active = true
            });
        }

        //Create Notification
        private async Task AddNotification(NotificationType notificationType, int bookingId, IEnumerable<int> idUserList)
        {
            if (idUserList != null && idUserList.Any())
            {
                foreach (int idUser in idUserList)
                {
                    // Add new notification for user request
                    var notification = new MidNotification
                    {
                        Id = Guid.NewGuid(),
                        IsRead = false,
                        LinkId = bookingId,
                        UserId = idUser,
                        NotifTypeId = (int)notificationType,
                        CreatedOn = DateTime.Now,
                        EntityId = _filterService.GetCompanyId()
                    };

                    _repo.AddEntity(notification);
                }

                //Save
                await _repo.Save();

            }
        }

        //Create Task
        private async Task AddTask(TaskType taskType, int bookingId, int roleId, IEnumerable<int> userIdList, int? loggingUserId = 0)
        {
            if (userIdList != null && userIdList.Any())
            {
                foreach (int userId in userIdList.Where(x => x > 0))
                {
                    // Add new task for user request
                    var task = new MidTask
                    {
                        Id = Guid.NewGuid(),
                        LinkId = bookingId,
                        UserId = loggingUserId > 0 ? loggingUserId.GetValueOrDefault() : _ApplicationContext.UserId,
                        IsDone = false,
                        TaskTypeId = (int)taskType,
                        ReportTo = userId,
                        CreatedBy = _ApplicationContext?.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = _filterService.GetCompanyId()
                    };

                    _repo.AddEntity(task);
                }
                //Save
                await _repo.Save();
            }
        }

        private async Task<IEnumerable<User>> BookingQuotationExists(int bookingId, int? officeId, int customerId, NotificationType type)
        {
            var quotationDetails = await _cancelBookingRepository.BookingQuotationExists(bookingId);
            if (quotationDetails != null)
            {
                //get product category details
                var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
                //Get Department details
                var departmentData = await _repo.GetBookingDepartmentList(new[] { bookingId });
                //Get Brand details
                var brandData = await _repo.GetBookingBrandList(new[] { bookingId });

                var booking = await _repo.GetBookingTransaction(bookingId);

                //factory country 
                int? factoryCountryId = null;
                if (booking.FactoryId.HasValue)
                {
                    var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(booking.FactoryId.Value);
                    if (factoryCountryData.Result == SupplierListResult.Success)
                        factoryCountryId = factoryCountryData.countryId;
                }

                var userAccessFilter = new UserAccess
                {
                    OfficeId = officeId != null ? officeId.Value : 0,
                    ServiceId = (int)Service.InspectionId,
                    CustomerId = customerId,
                    RoleId = (int)RoleEnum.QuotationRequest,
                    ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                    DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    FactoryCountryId = factoryCountryId
                };
                var quotationUsers = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                if (quotationUsers != null && quotationUsers.Count() > 0)
                {
                    await AddNotification(type, bookingId, quotationUsers.Select(x => x.Id));
                }
                return quotationUsers;
            }

            return new List<User>();
        }
        ///<summary>
        ///get service type for booking id list
        ///</summary>
        /// <param name="bookingIdList"></param>
        /// <returns>service type details</returns>
        public async Task<IEnumerable<ServiceTypeList>> GetServiceTypeList(IEnumerable<int> bookingIdList)
        {
            return await _repo.GetServiceType(bookingIdList);
        }

        /// <summary>
        /// Add new inspectiondftransaction details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInspectionDFTransactionList(IEnumerable<InspectionDFTransactions> inspectionDFTransactions, InspTransaction entity, bool isEditBooking)
        {
            if (isEditBooking)
            {
                inspectionDFTransactions = inspectionDFTransactions.Where(x => x.Id <= 0);
            }
            // Add inspection po list
            if (inspectionDFTransactions != null)
            {
                foreach (var item in inspectionDFTransactions)
                {
                    //var inspectionPODetail = _bookingmap.MapDFCustomerConfigurationEntity(item,_ApplicationContext.UserId);
                    var inspetionDFTransaction = _bookingmap.MapDFCustomerConfigurationEntity(item, _ApplicationContext.UserId);

                    entity.InspDfTransactions.Add(inspetionDFTransaction);
                    _repo.AddEntity(inspetionDFTransaction);
                }
            }
        }

        /// <summary>
        /// To update inspection Df Transaction Details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionDfTransaction(SaveInsepectionRequest request, InspTransaction entity)
        {
            // Purchase Order details
            var InspectionDFTransactionIds = request.InspectionDFTransactions.Where(x => x.Id > 0).Select(x => x.Id).ToArray();

            var lstDfTransactionToremove = new List<InspDfTransaction>();

            var InspectionDFTransactions = entity.InspDfTransactions.Where(x => !InspectionDFTransactionIds.Contains(x.Id) && x.Active);

            // Make InActive if data does not exist in the db.

            foreach (var item in InspectionDFTransactions)
            {
                lstDfTransactionToremove.Add(item);
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                item.Active = false;
            }

            _repo.EditEntities(lstDfTransactionToremove);

            // Update if data already exist in the db

            if (request.InspectionDFTransactions != null)
            {
                // Add if data is new it means id = 0;
                AddInspectionDFTransactionList(request.InspectionDFTransactions, entity, true);

                // Update if data already exist in the db
                var lstInspectionTransactionToEdit = new List<InspDfTransaction>();
                foreach (var item in request.InspectionDFTransactions.Where(x => x.Id > 0))
                {
                    var inspectionDFTransaction = entity.InspDfTransactions.FirstOrDefault(x => x.Id == item.Id);
                    if (inspectionDFTransaction != null)
                    {

                        inspectionDFTransaction.Id = item.Id;
                        inspectionDFTransaction.BookingId = item.BookingId;
                        inspectionDFTransaction.ControlConfigurationId = item.ControlConfigurationId;
                        inspectionDFTransaction.Value = item.Value;
                        inspectionDFTransaction.UpdatedBy = _ApplicationContext.UserId;
                        inspectionDFTransaction.UpdatedOn = DateTime.Now;

                        lstInspectionTransactionToEdit.Add(inspectionDFTransaction);

                    }
                }

                if (lstInspectionTransactionToEdit.Count > 0)
                    _repo.EditEntities(lstInspectionTransactionToEdit);
            }

        }

        //Export the booking summary search to Excel
        public async Task<InspectionExportData> ExportReportDataSummary(InspectionSummarySearchRequest request)
        {
            InspectionExportData bookingExportData = new InspectionExportData();
            var bookingDFDataList = new List<InspectionBookingDFData>();

            //get the booking summary request
            request = await GetInspectionSummaryRequest(request);

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetInspectionQueryRequestMap(request);

            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            //get the quotation involved query
            bookingData = GetQuotationDetails(request, bookingData);

            //get the ae configured data query
            bookingData = await GetAEConfiguredData(request, bookingData);

            bookingData = GetInspectionByBrandAndDept(request, bookingData);

            //Get the booking information
            var bookingDetails = await bookingData.Select(x => new InspectionBookingExportData()
            {
                BookingNo = x.Id,
                CustomerBookingNo = x.CustomerBookingNo,
                OfficeId = x.OfficeId,
                CustomerId = x.CustomerId,
                Customer = x.Customer.CustomerName,
                Supplier = x.Supplier.SupplierName,
                SupplierId = x.SupplierId,
                Factory = x.Factory.SupplierName,
                ApplyDate = x.CreatedOn,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo,
                FirstServiceDateFrom = x.FirstServiceDateFrom,
                FirstServiceDateTo = x.FirstServiceDateTo,
                UpdatedDate = x.UpdatedOn,
                Status = x.Status.Status,
                StatusId = x.StatusId,
                Office = x.Office.LocationName,
                PriceCategory = x.PriceCategory.Name,
                Collection = x.Collection.Name,
                CreatedByStaff = x.CreatedByNavigation.Staff.PersonName,
                CreatedByCustomer = x.CreatedByNavigation.CustomerContact.ContactName,
                CreatedBySupplier = x.CreatedByNavigation.SupplierContact.ContactName,
                CreatedByFactory = x.CreatedByNavigation.FactoryContact.ContactName,
                UserTypeId = x.CreatedByNavigation.UserTypeId,
                Picking = x.IsPickingRequired,
                CustomerBookingRemarks = x.CusBookingComments
            }).AsNoTracking().ToListAsync();

            //Pick only the booking Ids to fetch Products
            var bookingIds = bookingData.Select(x => x.Id);

            //get dept id and booking id by booking
            var bookingDeptAccessList = await _repo.GetDeptBookingIdsByBookingQuery(bookingIds);

            //get brand id and booking id  by booking
            var bookingBrandAccessList = await _repo.GetBrandBookingIdsByBookingQuery(bookingIds);

            //get buyer details and booking id  by booking
            var bookingBuyerAccessList = await _repo.GetBuyerBookingIdsByBookingQuery(bookingIds);

            var quotationData = await _repo.GetBookingQuotationDetailsbybookingId(bookingIds);

            //get the service type list
            var serviceTypeList = await _repo.GetServiceTypeByBookingQuery(bookingIds);

            var factoryData = await _repo.GetFactorycountryByBookingQuery(bookingIds);

            //get  customer id list
            var distinctCusIdList = bookingDetails.Where(x => x.CustomerId > 0).Select(x => x.CustomerId).Distinct().ToList();

            //get office id list
            var distinctOfficeIdList = bookingDetails.Where(x => x.OfficeId > 0).Select(x => x.OfficeId).Distinct().ToList();

            //Get the CS Config Details
            var CSConfigList = await GetAEConfigDetails(distinctCusIdList, distinctOfficeIdList, bookingDeptAccessList, bookingBrandAccessList);

            //get the non container booking query
            var nonContainerBookingIds = bookingData.Where(x => x.InspTranServiceTypes.Any(y => y.Active && y.ServiceTypeId != (int)InspectionServiceTypeEnum.Container)).
                        Select(x => x.Id);

            //get the product data for the non container booking data along with the report details
            var productTransactionList = await _repo.GetBookingProductPoList(nonContainerBookingIds);

            //get the product data for the container booking data along with the report details
            var containerProductList = await _repo.GetContainerBookingProductList(bookingIds);
            //join the product transaction with the container product list
            var productList = productTransactionList.Union(containerProductList).ToList();
            //take the po details 
            var poDetails = await _repo.GetBookingPoListByBookingQuery(bookingIds);
            //take the dynamic field response
            var dfBookingFieldsResponse = await _dynamicFieldManager.GetBookingDFDataByBookings(bookingIds);

            if (dfBookingFieldsResponse.Result == InspectionBookingDFDataResult.Success)
                bookingDFDataList = dfBookingFieldsResponse.bookingDFDataList;

            var statusLogs = await _repo.GetBookingStatusLogsByQuery(bookingIds);

            var inspectionHoldReasons = await _repo.GetInspectionHoldReasons(bookingIds);

            var inspectionCancelReasons = await _repo.GetInspectionCancelReasons(bookingIds);

            var enumEntityName = (Company)_filterService.GetCompanyId();

            string entityName = enumEntityName.ToString().ToUpper();

            //get customer id list
            var customerIdList = bookingDetails?.Where(x => x.CustomerId > 0).Select(x => x.CustomerId.GetValueOrDefault()).Distinct().ToList();

            //get supplier id list
            var supplierIdList = bookingDetails?.Select(x => x.SupplierId).Distinct().ToList();

            //get supplier code list
            var supplierCodeList = await _supplierManager.GetSupplierCode(customerIdList, supplierIdList);


            //Map the products with the booking details
            var result = _bookingmap.MapProductDataToBooking(productList, bookingDetails, bookingDeptAccessList, bookingBrandAccessList, bookingBuyerAccessList, serviceTypeList.ToList(),
                                        CSConfigList.ToList(), quotationData, poDetails, factoryData, statusLogs, inspectionHoldReasons, inspectionCancelReasons, entityName,
                                        supplierCodeList).ToList();

            //convert the list to datatable
            var dtBookingTable = _helper.ConvertToDataTable(result.ToList());
            //map the booking dynamic fields with the datatable
            _bookingmap.MapBookingWithDynamicFields(dtBookingTable, bookingDFDataList);
            bookingExportData.bookingList = dtBookingTable;

            return bookingExportData;

        }

        //get product category details
        public async Task<IEnumerable<BookingProductCategory>> GetProductCategoryDetails(IEnumerable<int> bookingIds)
        {
            return await _repo.GetProductCategoryDetails(bookingIds);
        }

        //get product category list
        public async Task<List<BookingProductCategoryData>> GetProductCategoryList(IEnumerable<int> bookingIds)
        {
            var response = new List<BookingProductCategoryData>();
            foreach (var bookingId in bookingIds)
            {
                var productCategoryList = await _repo.GetProductCategoryDetails(new[] { bookingId });
                if (productCategoryList != null && productCategoryList.Any())
                {
                    var data = new BookingProductCategoryData();
                    data.BookingId = bookingId;
                    data.ProductCategory = string.Join(",", productCategoryList.Select(x => x.ProductCategoryName).Distinct().ToList());
                    data.ProductSubCategory2 = string.Join(",", productCategoryList.Select(x => x.ProductCategorySub2Name).Distinct().ToList());
                    response.Add(data);
                }
            }
            return response;

        }

        //update task(mid_task table) 
        public async Task<IEnumerable<MidTask>> UpdateTask(int bookingId, IEnumerable<int> typeIdList, bool oldTaskDoneValue, bool newTaskDoneValue)
        {
            IEnumerable<MidTask> getTasks = await _repo.GetTask(bookingId, typeIdList, oldTaskDoneValue);
            foreach (var task in getTasks)
            {
                if (task != null)
                {
                    task.IsDone = newTaskDoneValue;
                    task.UpdatedBy = _ApplicationContext?.UserId;
                    task.UpdatedOn = DateTime.Now;
                }
            }
            _repo.SaveList(getTasks);
            return getTasks;
        }
        //get booking data by booking id
        public async Task<InspTransaction> GetBookingData(int bookingId)
        {
            return await _repo.GetBookingTransaction(bookingId);
        }

        /// <summary>
        /// Adding customer merchandiser list for booking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInspectionCustomerMerchandiserList(SaveInsepectionRequest request, InspTransaction entity)
        {
            if (request.InspectionCustomerMerchandiserList != null)
            {
                foreach (var customerMerchandiserId in request.InspectionCustomerMerchandiserList)
                {
                    var _cusmerchandiser = new InspTranCuMerchandiser()
                    {
                        Active = true,
                        MerchandiserId = customerMerchandiserId,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    entity.InspTranCuMerchandisers.Add(_cusmerchandiser);
                    _repo.AddEntity(_cusmerchandiser);
                }
            }
        }

        private void AddInspectionShipmentTypes(SaveInsepectionRequest request, InspTransaction entity)
        {
            if (request.ShipmentTypeIds != null)
            {
                foreach (var ShipmentTypeId in request.ShipmentTypeIds)
                {
                    var shipmentType = new InspTranShipmentType()
                    {
                        Active = true,
                        ShipmentTypeId = ShipmentTypeId,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    entity.InspTranShipmentTypes.Add(shipmentType);
                    _repo.AddEntity(shipmentType);
                }
            }
        }

        /// <summary>
        /// update or remove customer merchandiser
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionCustomerMerchandiserList(SaveInsepectionRequest request, InspTransaction entity)
        {
            var merchandiserIds = request.InspectionCustomerMerchandiserList.Select(x => x).ToArray();

            var lstCustomerMerchandisersToremove = new List<InspTranCuMerchandiser>();

            var bookingCustomerMerchandisers = entity.InspTranCuMerchandisers.Where(x => !merchandiserIds.Contains(x.MerchandiserId) && x.Active);

            var existcusBuyerlist = entity.InspTranCuMerchandisers.Where(x => merchandiserIds.Contains(x.MerchandiserId) && x.Active);

            // Remove if data does not exist in the db.

            foreach (var item in bookingCustomerMerchandisers)
            {
                item.Active = false;
                item.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                item.DeletedOn = DateTime.Now;
                lstCustomerMerchandisersToremove.Add(item);
            }

            _repo.EditEntities(lstCustomerMerchandisersToremove);

            // Update if data already exist in the db

            if (request.InspectionCustomerMerchandiserList != null)
            {
                // Add if data is new it means id = 0;
                foreach (var id in merchandiserIds)
                {
                    if (!existcusBuyerlist.Any() || !existcusBuyerlist.Any(x => x.MerchandiserId == id))
                    {
                        entity.InspTranCuMerchandisers.Add(new InspTranCuMerchandiser()
                        {
                            MerchandiserId = id,
                            Active = true,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }

        /// <summary>
        /// update or add new shipment types for the booking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionShipmentTypes(SaveInsepectionRequest request, InspTransaction entity)
        {
            var lstInspShipmentTypesToRemove = new List<InspTranShipmentType>();

            var inspectionShipmentTypes = entity.InspTranShipmentTypes.Where(x => !request.ShipmentTypeIds.Contains(x.ShipmentTypeId) && x.Active.HasValue && x.Active.Value);

            var existShipmentTypes = entity.InspTranShipmentTypes.Where(x => request.ShipmentTypeIds.Contains(x.ShipmentTypeId) && x.Active.HasValue && x.Active.Value);

            // Remove if data does not exist in the db.

            foreach (var item in inspectionShipmentTypes)
            {
                item.Active = false;
                item.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                item.DeletedOn = DateTime.Now;
                lstInspShipmentTypesToRemove.Add(item);
            }

            _repo.EditEntities(lstInspShipmentTypesToRemove);

            // Update if data already exist in the db

            // Add if data is new it means id = 0;
            foreach (var id in request.ShipmentTypeIds)
            {
                if (!existShipmentTypes.Any() || !existShipmentTypes.Any(x => x.ShipmentTypeId == id))
                {
                    entity.InspTranShipmentTypes.Add(new InspTranShipmentType()
                    {
                        ShipmentTypeId = id,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    });
                }
            }
        }

        // get booking details based on booking ids
        public async Task<IEnumerable<BookingDetail>> GetBookingData(IEnumerable<int> bookingIds)
        {
            var data = await _repo.GetBookingData(bookingIds);

            var factoryLocationData = await _repo.GetFactorycountryId(bookingIds);
            var serviceTypeIds = await _repo.GetServiceType(bookingIds);
            var brandIds = await _repo.GetBrandBookingIdsByBookingIds(bookingIds);
            var buyerIds = await _repo.GetBuyerBookingIdsByBookingIds(bookingIds);
            var deptIds = await _repo.GetDeptBookingIdsByBookingIds(bookingIds);

            return data.Select(x => new BookingDetail
            {
                CustomerId = x.CustomerId,
                SupplierId = x.SupplierId,
                BookingId = x.BookingId,
                CountryIds = factoryLocationData.Where(y => y.BookingId == x.BookingId).Select(y => y.FactoryCountryId).ToList(),
                ServiceTypeIds = serviceTypeIds.Where(y => y.InspectionId == x.BookingId).Select(y => y.serviceTypeId).ToList(),
                ProvinceIds = factoryLocationData.Where(y => y.BookingId == x.BookingId).Select(y => y.FactoryProvinceId).ToList(),
                BrandIds = brandIds.Where(y => y.BookingId == x.BookingId).Select(y => y.BrandId).ToList(),
                BuyerIds = buyerIds.Where(y => y.BookingId == x.BookingId).Select(y => y.BuyerId).ToList(),
                DepartmentIds = deptIds.Where(y => y.BookingId == x.BookingId).Select(y => y.DeptId).ToList(),
                PriceCategoryId = x.PriceCategoryId,
                ServiceFrom = x.ServiceFrom,
                ServiceTo = x.ServiceTo,
                OfficeId = x.OfficeId
            });
        }

        //Get fb report Template List
        public async Task<DataSourceResponse> GetFbTemplateList()
        {
            DataSourceResponse response = new DataSourceResponse();

            try
            {
                response.DataSourceList = await _referenceRepo.GetFbTemplateList();
                response.Result = DataSourceResult.Success;
            }

            catch (Exception e)
            {
                response.Result = DataSourceResult.Failed;
            }

            return response;
        }

        //get booking information
        public async Task<BookingInformation> GetBookingInformation(int bookingId)
        {
            try
            {
                if (bookingId <= 0)
                    return new BookingInformation() { Result = BookingDataResponseResult.NotAvailable };

                var response = new BookingInformation();

                response.BookingInfo = await _repo.GetBookingInfo(bookingId);
                response.Result = BookingDataResponseResult.Success;

                return response;
            }
            catch (Exception ex)
            {
                return new BookingInformation() { Result = BookingDataResponseResult.Failure };
                //throw ex;
            }
        }
        /// <summary>
        /// Check booking is processed or not
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<bool> CheckBookingIsProcessed(int bookingId)
        {
            return await _repo.CheckBookingIsProcessed(bookingId);
        }

        //get price category based on customer id and product sub category 2 id 
        public async Task<PriceCategoryResponse> GetPriceCategoryByCustomerIdPCSub2Id(PriceCategoryRequest request)
        {
            try
            {
                if (request == null)
                    return new PriceCategoryResponse() { Result = PriceCategoryResult.RequestNotCorrectFormat };

                var response = new PriceCategoryResponse();

                var priceCategoryDetails = await _repo.GetPriceCategory(request);

                if (priceCategoryDetails != null && priceCategoryDetails.Count() > 0)
                {
                    if (priceCategoryDetails.Count() == 1)
                    {
                        if (request.PriceCategoryId != null)
                        {
                            // selected price category id and db price category id same
                            if (priceCategoryDetails.FirstOrDefault().PriceCategoryId == request.PriceCategoryId)
                            {
                                response.Result = PriceCategoryResult.Success;
                            }
                            else
                            {
                                //selected price category id different from db
                                response.Result = PriceCategoryResult.MismatchPriceCategory;
                                response.PriceCategoryName = priceCategoryDetails.FirstOrDefault().PriceCategoryName;
                            }
                        }
                        else
                        {
                            //not selected price category id but we get value 
                            response.Result = PriceCategoryResult.SelectPriceCategory;
                            response.PriceCategoryId = priceCategoryDetails.FirstOrDefault().PriceCategoryId;
                        }
                    }
                    else
                    {
                        //we get more price category data from db 
                        response.Result = PriceCategoryResult.MultiplePriceCategory;
                        response.PriceCategoryName = string.Join(",", priceCategoryDetails.Select(x => x.PriceCategoryName).ToList());
                    }
                }
                else
                {
                    //not configure CU_PriceCategory_PCSub2 - no data 
                    response.Result = PriceCategoryResult.NodataFound;
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* If reschedule happens need to update existing service date as inactive */
        private async Task UpdateQuotationServiceDate(BookingDateInfo request, InspTransaction entity)
        {
            try
            {
                //if request and entity as same date we should not update quotation date
                if (!(request.ServiceFromDate.ToDateTime() == entity.ServiceDateFrom &&
                       request.ServiceToDate.ToDateTime() == entity.ServiceDateTo))
                {
                    bool quotationExists = await _quotationRepository.QuotationInspExists(request.BookingId);
                    if (quotationExists)
                    {
                        var quotationInspDateMandayList = await _quotationRepository.GetQuotationInspManDay(request.BookingId);

                        if (quotationInspDateMandayList != null && quotationInspDateMandayList.Count() > 0)
                        {
                            var quotationRecord = quotationInspDateMandayList.FirstOrDefault();
                            //if data exist make as inactive 
                            foreach (QuQuotationInspManday manDayInsp in quotationInspDateMandayList)
                            {
                                manDayInsp.Active = false;
                                manDayInsp.DeletedDate = DateTime.Now;
                            }
                            _quotationRepository.EditEntities(quotationInspDateMandayList);

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
                    _quotationRepository.AddEntity(dateManday);
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

                _quotationRepository.EditEntity(objQuotation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //get department details
        public async Task<List<CommonDataSource>> GetBookingDepartmentList(IEnumerable<int> bookingIds)
        {
            return await _repo.GetBookingDepartmentList(bookingIds);
        }

        //get brand details
        public async Task<List<CommonDataSource>> GetBookingBrandList(IEnumerable<int> bookingIds)
        {
            return await _repo.GetBookingBrandList(bookingIds);
        }

        //this method is to fetch the data for the mobile app
        public async Task<InspSummaryMobileDetaiResponse> GetInspDetailMobileSummary(int reportId)
        {
            var response = new InspSummaryMobileDetaiResponse();

            try
            {
                var paramList = new ReportData();

                paramList.productDataList = await _repo.GetMobileProductListByBooking(reportId);

                // get booking data
                var bookingId = paramList.productDataList.Select(x => x.BookingId).FirstOrDefault();
                paramList.bookingData = await _repo.GetBookingData(bookingId);

                var bookingServiceTypes = await _repo.GetBookingServiceTypes(bookingId);

                //get the booking customer brands
                var bookingCustomerBrands = await _repo.GetBookingBrands(bookingId);

                //get the booking customer departments
                var bookingCustomerDepartments = await _repo.GetBookingDepartments(bookingId);

                if (bookingServiceTypes != null && bookingServiceTypes.Any())
                {
                    paramList.bookingData.ServiceTypeIds = bookingServiceTypes;
                }

                if (bookingCustomerBrands != null && bookingCustomerBrands.Any())
                {
                    paramList.bookingData.BrandIds = bookingCustomerBrands;
                }

                if (bookingCustomerDepartments != null && bookingCustomerDepartments.Any())
                {
                    paramList.bookingData.DepartmentIds = bookingCustomerDepartments;
                }

                // get report customer decision data 
                var reportIdList = new[] { reportId }.ToList();
                paramList.cusDecision = await _repo.GetMobileReportCustomerDecisionByReport(reportIdList);

                //fetch defect List
                paramList.defects = await _repo.GetMobileInspectionDefectsByReport(reportIdList);

                var defectList = paramList.defects.Select(x => x.Id).ToList();
                paramList.defectImageData = await _repo.GetMobileInspectionDefectsImageByReport(defectList);

                //fetch the data from fb_inspsummary
                paramList.inspectionReportSummaries = await _repo.GetMobileInspectionSummaryByReport(reportIdList);

                //fetch the data from fb_insp_subsummary
                paramList.fbReportInspSubSummaries = await _repo.GetMobileFBInspSummaryResult(reportIdList);

                //get the customer decisions configured for the customer
                paramList.customerDecisionModes = _customerDecisionManager.GetCustomerDecisionList(paramList.bookingData.CustomerId).Result.CustomerDecisionList;

                response.data = _inspsummobilemap.InspDetailSummaryMap(paramList);

                response.meta = new MobileResult { success = true, message = "" };
            }

            catch (Exception e)
            {
                response.meta = new MobileResult { success = false, message = "Inspection Detail Summary fetch failed." };
            }

            return response;
        }

        public async Task<List<BookingServiceType>> GetBookingServiceTypes(IEnumerable<int> bookingIds)
        {
            return await _repo.GetBookingServiceTypes(bookingIds);
        }

        //get insp summary for mobile app
        public async Task<InspSummaryMobileResponse> GetMobileInspSummary(InspSummaryMobileRequest request)
        {
            var response = new InspSummaryMobileResponse();

            try
            {
                InspectionSummarySearchRequest inspRequest = RequestMobileMap.MapInspRequest(request);
                var bookingData = await GetAllInspectionsData(inspRequest, false);

                if (bookingData.Data != null && bookingData.Data.Any())
                {
                    var bookingIdList = bookingData.Data.Where(x => !ContainerServiceList.Contains(x.ServiceTypeId)).Select(x => x.BookingId).ToList();

                    var containerBookingIdList = bookingData.Data.Where(x => !ContainerServiceList.Contains(x.ServiceTypeId)).Select(x => x.BookingId).ToList();

                    var reportdata = await _repo.GetReportDataByBooking(bookingIdList);
                    var containerReportData = await _repo.GetContainerReportDataByBooking(containerBookingIdList);
                    reportdata.AddRange(containerReportData);

                    response.data = _inspsummobilemap.InspSummaryMap(bookingData.Data.ToList(), request.pageIndex, reportdata);
                }
                response.meta = new MobileResult { success = true, message = "" };
            }

            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Inspection Summary fetch failed." };
            }
            return response;
        }

        //fetch the product and status timeline for the mobile app
        public async Task<BookingProductMobileResponse> GetMobileBookingProductsAndStatusTimeline(int bookingId)
        {
            var response = new BookingProductMobileResponse();
            try
            {
                var data = await GetBookingProductsAndStatusMobile(bookingId);
                response.data = _inspsummobilemap.BookingProductsMap(data);
                response.meta = new MobileResult { success = true, message = "" };
            }

            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Booking product fetch failed." };
            }

            return response;
        }

        public async Task<FilterDataSourceResponse> GetMobileFactoryCountry(CommonCountrySourceRequest request)
        {
            var response = new FilterDataSourceResponse();

            try
            {
                var countryList = await _locationManager.GetCountryDataSource(request);
                var _key = 1;

                response.data = countryList.DataSourceList.Select(x => new FilterDataSource
                {
                    key = _key++,
                    id = x.Id,
                    name = x.Name
                }).ToList();

                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Country list fetch failed." };
            }

            return response;
        }

        public async Task<FilterDataSourceResponse> GetMobileSupplierFactory(CommonDataSourceRequest request)
        {
            var response = new FilterDataSourceResponse();

            try
            {
                var supList = await _suppliermanager.GetSupplierDataSource(request);
                var _key = 1;

                response.data = supList.DataSourceList.Select(x => new FilterDataSource
                {
                    key = _key++,
                    id = x.Id,
                    name = x.Name
                }).ToList();

                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Supplier list fetch failed." };
            }

            return response;
        }

        public async Task<FilterDataSourceResponse> GetMobileCustomer(CommonDataSourceRequest request)
        {
            var response = new FilterDataSourceResponse();

            try
            {
                var customerList = await _customerManager.GetCustomerDataSource(request);
                var _key = 1;

                response.data = customerList.DataSourceList.Select(x => new FilterDataSource
                {
                    key = _key++,
                    id = x.Id,
                    name = x.Name
                }).ToList();

                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Customer list fetch failed." };
            }

            return response;
        }

        public async Task<FilterDataSourceResponse> GetMobileDepartment(CommonCustomerSourceRequest request)
        {
            var response = new FilterDataSourceResponse();

            try
            {
                var deptList = await _cusDeptManager.GetDepartmentDataSource(request);
                var _key = 1;

                response.data = deptList.DataSourceList.Select(x => new FilterDataSource
                {
                    key = _key++,
                    id = x.Id,
                    name = x.Name
                }).ToList();

                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Department list fetch failed." };
            }

            return response;
        }

        public async Task<FilterDataSourceResponse> GetMobileCollection(CommonCustomerSourceRequest request)
        {
            var response = new FilterDataSourceResponse();

            try
            {
                var collectionList = await _cusCollectionManager.GetCollectionDataSource(request);
                var _key = 1;

                response.data = collectionList.DataSourceList.Select(x => new FilterDataSource
                {
                    key = _key++,
                    id = x.Id,
                    name = x.Name
                }).ToList();

                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Collection list fetch failed." };
            }

            return response;
        }

        public async Task<FilterDataSourceResponse> GetMobileBuyer(CommonCustomerSourceRequest request)
        {
            var response = new FilterDataSourceResponse();

            try
            {
                var buyerList = await _cusBuyerManager.GetBuyerDataSource(request);
                var _key = 1;

                response.data = buyerList.DataSourceList.Select(x => new FilterDataSource
                {
                    key = _key++,
                    id = x.Id,
                    name = x.Name
                }).ToList();

                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Buyer list fetch failed." };
            }

            return response;
        }

        public async Task<CommonFilterListResponse> GetMobileCommonFilter()
        {
            var response = new CommonFilterListResponse();
            var res = new CommonFilterList();

            try
            {
                var statusList = await _repo.GetBookingStatus();
                var statusKey = 1;
                res.statusList = statusList.Select(x => new FilterDataSource
                {
                    key = statusKey++,
                    id = x.Id,
                    name = x.Status
                }).ToList();

                var serviceTypeList = await _referencemanager.GetServiceList();
                var serviceTypeKey = 1;
                res.serviceTypeList = serviceTypeList.DataSourceList.Select(x => new FilterDataSource
                {
                    key = serviceTypeKey++,
                    id = x.Id,
                    name = x.Name
                }).ToList();

                response.data = res;
                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception e)
            {
                response.meta = new MobileResult { success = false, message = "Status and Service Type List fetch failed" };
            }
            return response;
        }

        //get product picking and quotation information for validation when deleting a product from the booking
        public async Task<ProductValidationResponse> BookingProductValidationInfo(int bookingId, int poTranId, int prodId)
        {
            try
            {
                var quotationData = await _cancelBookingRepository.BookingQuotationExists(bookingId);

                var pickingExists = await _repo.GetPickingData(poTranId, prodId);

                if (quotationData != null && quotationData.IdQuotation > 0 && pickingExists)
                {
                    return new ProductValidationResponse { Result = ProductValidationResult.PickingAndQuotationExists };
                }

                else if (pickingExists)
                    return new ProductValidationResponse { Result = ProductValidationResult.PickingExists };

                else if (quotationData != null && quotationData.IdQuotation > 0)
                    return new ProductValidationResponse { Result = ProductValidationResult.QuotationExists };

                else
                    return new ProductValidationResponse { Result = ProductValidationResult.Success };

            }
            catch (Exception ex)
            {
                return new ProductValidationResponse { Result = ProductValidationResult.Fail };
            }
        }
        /// <summary>
        /// get last order applicant details for customer, supplier, factory login by userid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public async Task<ApplicantStaffResponse> GetApplicantInfoById()
        {
            var response = new ApplicantStaffResponse()
            {
                ApplicantInfo = new BookingStaffInfo()
            };

            InspectionBookingApplicantItems applicantDetails = await _repo.GetInspectionDetails(_ApplicationContext.UserId);

            if (applicantDetails != null)
            {
                response.ApplicantInfo.CompanyEmail = applicantDetails.ApplicantEmail;
                response.ApplicantInfo.CompanyPhone = applicantDetails.ApplicatPhoneNo;
                response.ApplicantInfo.StaffName = applicantDetails.ApplicantName;

                response.Result = ApplicantStaffResponseResult.Success;
            }
            return response;
        }

        //Update the office id in the quotation if office id is updated in booking save
        public async Task UpdateQuotationOffice(int bookingId, int officeId)
        {
            var data = await _quotationRepository.GetBookingQuotationDetails(bookingId);

            if (data != null && data.OfficeId != officeId)
            {
                data.OfficeId = officeId;
                _repo.Save(data, true);
            }
        }
        /// <summary>
        /// get booking data
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<BookingDate> getInspBookingDateDetails(int bookingId)
        {
            return await _repo.getInspBookingDateDetails(bookingId);
        }

        /// <summary>
        /// get booking products by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<MobileBookingProducts> GetBookingProductsAndStatusMobile(int bookingId)
        {
            string servicedate = string.Empty;
            // get booking products
            var bookingIds = new List<int>() { bookingId };
            var productList = await _repo.GetProductListByBookingByPO(bookingIds);

            // get booking log status 

            var bookingLogStatus = _eventLogRepo.GetLogStatusByBooking(bookingId);

            var currentBookingStatus = productList.FirstOrDefault()?.BookingStatus;

            var currenbookingdate = productList.FirstOrDefault()?.CreatedDate?.ToString(StandardDateFormat);


            var bookinginfo = productList.FirstOrDefault();
            if (bookinginfo != null && bookinginfo.ServiceDateFrom.HasValue && bookinginfo.ServiceDateTo.HasValue)
            {
                if (bookinginfo.ServiceDateFrom.Value == bookinginfo.ServiceDateTo.Value)
                {
                    servicedate = bookinginfo.ServiceDateTo.Value.ToString(StandardDateFormat);
                }
                else
                {
                    servicedate = bookinginfo.ServiceDateFrom.Value.ToString(StandardDateFormat) + "-" + bookinginfo.ServiceDateTo.Value.ToString(StandardDateFormat);
                }
            }

            var actualStatusList = GetBookingReportStatus(bookingId, bookingLogStatus, currentBookingStatus, currenbookingdate, servicedate);

            var result = _inspreportmobilemap.InspReportMap(productList);

            // get booking status
            if (productList != null)
            {
                return new MobileBookingProducts()
                {
                    BookingStatusList = actualStatusList,
                    BookingProductsList = result.
                                           OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ThenBy(x => x.ProductName).ToList(),
                    Result = BookingProductsResponseResult.Success
                };

            }

            return new MobileBookingProducts() { Result = BookingProductsResponseResult.Success };
        }

        //Export the products data for a booking
        public async Task<List<BookingProductsExportData>> ExportBookingProductSummary(int bookingId)
        {
            var list = new[] { bookingId };
            var productData = await _repo.GetProductPoListByBooking(list);
            //set only one AQL combine qty
            var combine = productData.Where(x => x.CombineProductId > 0 && x.CombineAqlQuantity > 0).ToList();
            foreach (var item in combine.Select(x => x.CombineProductId).Distinct())
            {
                var combinepolist = combine.Where(x => x.CombineProductId == item).ToList();
                for (var index = 0; index < combinepolist.Count; index++)
                {
                    if (index != 0)
                        combinepolist[index].CombineAqlQuantity = 0;
                }
            }
            //set only one AQL Qty
            var noncombine = productData.Where(x => (!x.CombineProductId.HasValue || x.CombineProductId == 0) && (!x.CombineAqlQuantity.HasValue || x.CombineAqlQuantity == 0) && x.AqlQty >= 0).ToList();
            foreach (var item in noncombine.Select(x => x.ProductId).Distinct())
            {
                var noncombinepolst = noncombine.Where(x => x.ProductId == item).ToList();
                for (var index = 0; index < noncombinepolst.Count; index++)
                {
                    if (index != 0)
                        noncombinepolst[index].AqlQty = 0;
                }
            }

            productData = productData.OrderBy(x => x.ProductSerialNo);


            return productData.ToList();
        }

        /// <summary>
        /// get booking status list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetBookingStatusList()
        {
            var response = new DataSourceResponse();

            var statusList = await _repo.GetBookingStatus();

            if (statusList == null || !statusList.Any())
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = statusList.Select(x => new CommonDataSource()
                {
                    Name = x.Status,
                    Id = x.Id
                });
                response.Result = DataSourceResult.Success;
            }
            return response;
        }
        /// <summary>
        /// get booking details from insp_transaction as iqueryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<InspectionBookingItems> GetAllInspections()
        {
            return _repo.GetAllInspections();
        }

        /// <summary>
        /// get report id list by booking number list
        /// </summary>
        /// <param name="bookingNoList"></param>
        /// <returns></returns>
        public async Task<List<BookingReportData>> GetReportDataByBooking(List<int> bookingNoList)
        {
            return await _repo.GetReportDataByBooking(bookingNoList);
        }
        /// <summary>
        /// get report id list by booking number list
        /// </summary>
        /// <param name="bookingNoList"></param>
        /// <returns></returns>
        public async Task<List<BookingReportData>> GetReportDataByQueryableBooking(IQueryable<int> bookingNoList)
        {
            return await _repo.GetReportDataByQueryableBooking(bookingNoList);
        }

        public IQueryable<int> GetReportIdDataByQueryableBooking(IQueryable<int> bookingNoList)
        {
            return _repo.GetReportIdDataByQueryableBooking(bookingNoList);
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validate the booking po details
        /// </summary>
        /// <param name="productValidateData"></param>
        /// <returns></returns>
        public async Task<List<ProductValidateData>> BookingProductValidation(List<ProductValidateData> productValidateData)
        {
            //take the distinct bookingids
            var bookingIds = productValidateData.Select(x => x.BookingId).Distinct().ToList();

            //take the distinct potransactionids
            var poTransactionIds = productValidateData.Select(x => x.PoTransactionId).Distinct().ToList();

            //get the booking ids involved in the quotation
            var quotationBookingIds = await _repo.BookingQuotationExists(bookingIds);

            if (quotationBookingIds != null && quotationBookingIds.Any())
            {
                //apply the quotationexists to true for the bookings involved in the quotation.
                productValidateData.Where(x => quotationBookingIds.Contains(x.BookingId)).ToList().ForEach(x => x.QuotationExists = true);
            }

            //get the picking information data
            var pickingPoTransactionIds = await _repo.GetPickingExists(poTransactionIds);

            if (pickingPoTransactionIds != null && pickingPoTransactionIds.Any())
            {
                //apply the pickingexists to true for the po involved in the picking
                productValidateData.Where(x => pickingPoTransactionIds.Contains(x.PoTransactionId)).ToList().ForEach(x => x.PickingExists = true);
            }

            //get the product report data
            var reportPoTransactionIds = await _repo.GetProductReport(poTransactionIds);

            if (reportPoTransactionIds != null && reportPoTransactionIds.Any())
            {
                //apply the reportexists to true for which the po has the report data
                productValidateData.Where(x => reportPoTransactionIds.Contains(x.PoTransactionId)).ToList().ForEach(x => x.ReportExists = true);
            }

            return productValidateData;
        }

        /// <summary>
        /// Get the edit booking details by customer id and booking id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<EditBookingCustomerDetails> GetEditBookingDetailByCustomerId(int customerId, int bookingId)
        {
            var response = new EditBookingCustomerDetails();
            try
            {
                //get the processed edit booking response
                response = await ProcessBookingCustomerRelatedDetails(customerId, bookingId);
                response.Result = EditBookingCustomerResult.Success;
            }
            catch (Exception)
            {
                return new EditBookingCustomerDetails() { Result = EditBookingCustomerResult.CannotGetDetails };
            }
            return response;

        }

        /// <summary>
        /// proccess booking details
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        private async Task<EditBookingCustomerDetails> ProcessBookingCustomerRelatedDetails(int customerId, int bookingId)
        {
            EditBookingCustomerDetails response = new EditBookingCustomerDetails();

            // customer brand list
            var customerBrandList = await _customerRepo.GetEditBookingBrands(customerId, bookingId);
            if (customerBrandList != null && customerBrandList.Any())
                response.CustomerBrandList = customerBrandList;
            else
                response.Result = EditBookingCustomerResult.CustomerBrandNotFound;

            // customer department list
            var customerDepartmentList = await _customerRepo.GetEditBookingDepartments(customerId, bookingId);
            if (customerDepartmentList != null && customerDepartmentList.Any())
                response.CustomerDepartmentList = customerDepartmentList;
            else
                response.Result = EditBookingCustomerResult.CustomerDeptNotFound;

            // customer buyer list
            var customerBuyerList = await _customerRepo.GetEditBookingBuyers(customerId, bookingId);
            if (customerBuyerList != null && customerBuyerList.Any())
                response.CustomerBuyerList = customerBuyerList;
            else
                response.Result = EditBookingCustomerResult.CustomerBuyerNotFound;

            // customer merchandiser list
            var customerMerchandiserList = await _customerRepo.GetEditBookingMerchandisers(customerId, bookingId);
            if (customerMerchandiserList != null && customerMerchandiserList.Any())
                response.CustomerMerchandiserList = customerMerchandiserList;
            else
                response.Result = EditBookingCustomerResult.MerchandiserNotFound;

            //supplier list
            var supplierList = await _suppliermanager.GetEditBookingSuppliers(customerId, bookingId);
            if (supplierList != null && supplierList.Any())
                response.SupplierList = supplierList;
            else
                response.Result = EditBookingCustomerResult.SupplierNotFound;

            // customer collection list
            var collectionList = await _customerRepo.GetEditBookingCollection(customerId, bookingId);
            if (collectionList != null && collectionList.Any())
                response.Collection = collectionList;
            else
                response.Result = EditBookingCustomerResult.CollectionNotFound;

            // customer price category list
            var priceCategoryList = await _customerRepo.GetEditBookingPriceCategory(customerId, bookingId);
            if (priceCategoryList != null && priceCategoryList.Any())
                response.PriceCategory = priceCategoryList;
            else
                response.Result = EditBookingCustomerResult.PriceCategoryNotFound;

            var serviceTypeList = await _customerRepo.GetEditBookingServiceType(customerId, bookingId);

            if (serviceTypeList != null && serviceTypeList.Any())
                response.CustomerServiceList = serviceTypeList;
            else
                response.Result = EditBookingCustomerResult.CustomerServiceTypeNotFound;

            return response;
        }


        /// <summary>
        /// Get the edit booking office details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEditBookingOffice(int bookingId)
        {
            //get the office data
            var bookingOffice = await _repo.GetEditBookingOffice(bookingId);
            if (bookingOffice != null && bookingOffice.Any())
                return new DataSourceResponse() { DataSourceList = bookingOffice, Result = DataSourceResult.Success };
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get the edit booking unit details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEditBookingUnit(int bookingId)
        {
            var bookingUnitList = await _repo.GetEditBookingUnit(bookingId);
            if (bookingUnitList != null && bookingUnitList.Any())
                return new DataSourceResponse() { DataSourceList = bookingUnitList, Result = DataSourceResult.Success };
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }
        /// <summary>
        /// Get Inspection Hold Reason Types
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetHoldReasonTypes()
        {
            var bookingHoldReasonTypes = await _repo.GetHoldReasonTypes();
            if (bookingHoldReasonTypes != null && bookingHoldReasonTypes.Any())
                return new DataSourceResponse() { DataSourceList = bookingHoldReasonTypes, Result = DataSourceResult.Success };
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get the booking involved inspection locations and active location list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEditBookingInspectionLocations(int bookingId)
        {
            var inspectionLocations = await _repo.GetEditBookingInspectionLocations(bookingId);
            if (inspectionLocations != null && inspectionLocations.Any())
                return new DataSourceResponse() { DataSourceList = inspectionLocations, Result = DataSourceResult.Success };
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }
        /// <summary>
        /// Get the booking involved shipment list and active shipment list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEditBookingShipmentTypes(int bookingId)
        {
            var inspectionShipmentTypes = await _repo.GetEditBookingShipmentTypes(bookingId);
            if (inspectionShipmentTypes != null && inspectionShipmentTypes.Any())
                return new DataSourceResponse() { DataSourceList = inspectionShipmentTypes, Result = DataSourceResult.Success };
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get the booking involved customer product categories and active product categories
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEditBookingCuProductCategory(int customerId, int bookingId)
        {
            var productCategories = await _repo.GetEditBookingCuProductCategory(customerId, bookingId);
            if (productCategories != null && productCategories.Any())
                return new DataSourceResponse() { DataSourceList = productCategories, Result = DataSourceResult.Success };
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get the booking involved customer seasona and booking involved season
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEditBookingCustomerSeason(int? customerId, int bookingId)
        {
            var seasonConfig = _customerRepo.GetCustomerSeasonConfigQuery();

            if (customerId > 0)
            {
                seasonConfig = seasonConfig.Where(x => (x.CustomerId == customerId && x.Active.Value) ||
                                        (x.IsDefault.HasValue && x.IsDefault.Value && x.Active.Value)
                                        || x.InspTransactions.Any(y => y.Id == bookingId));
            }


            var data = await seasonConfig.Select(x => new CommonDataSource() { Id = x.Id, Name = x.Season.Name }).AsNoTracking().ToListAsync();
            if (data != null && data.Any())
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get booking involved business lines and active business lines
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetEditBookingBusinessLines(int bookingId)
        {
            var inspectionBusinessLines = await _repo.GetEditBookingBusinessLines(bookingId);
            if (inspectionBusinessLines != null && inspectionBusinessLines.Any())
                return new DataSourceResponse() { DataSourceList = inspectionBusinessLines, Result = DataSourceResult.Success };
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get booking Details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<BookingDataRepo> GetBookingDetails(int bookingId)
        {
            return await _repo.GetBookingDetails(bookingId);
        }
        /// <summary>
        /// Get Factory country  
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<FactoryCountry>> GetFactorycountryId(IEnumerable<int> bookingId)
        {
            return await _repo.GetFactorycountryId(bookingId);
        }
        /// <summary>
        /// Get Factory country  
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingProductinfo>> GetProductItemByBooking(int bookingId)
        {
            return await _repo.GetProductItemByBooking(bookingId);
        }
        public async Task<List<BookingProductPoRepo>> GetBookingProductsPoItemsByProductRefIds(int bookingId)
        {
            return await _repo.GetBookingProductsPoItemsByProductRefIds(bookingId);
        }
        public IQueryable<int> GetInspectionNo()
        {
            return _repo.GetInspectionNo();
        }
        //get booking data by id
        public async Task<BookingDataInfoResponse> GetBookingInfoDetails(int bookingId)
        {
            BookingDataInfoResponse response = new BookingDataInfoResponse();


            if (bookingId <= 0)
            {
                response.Result = BookingDatainfoResult.Failed;
                return response;
            }
            //Get booking Details based on booking number
            var bookingDetails = await _repo.GetBookingDetails(bookingId);

            //Get the service Type for the booking
            List<int> lstBook = new List<int>();
            lstBook.Add(bookingId);
            var serviceTypeList = await _repo.GetServiceType(lstBook);
            var serviceType = serviceTypeList.FirstOrDefault();
            //Get factory details
            var factoryDetails = await _repo.GetFactorycountryId(lstBook);

            //Get productDetails  
            var _bookingQty = 0;
            var _productName = "";
            if (serviceType.serviceTypeId != (int)InspectionServiceTypeEnum.Container)
            {
                var productDetails = await _repo.GetProductItemByBooking(bookingId);
                _productName = productDetails.Select(x => x.ProductName).FirstOrDefault();
                _bookingQty = productDetails.Sum(x => x.BookingQuantity).GetValueOrDefault();
            }

            var _data = new BookingDataInfo
            {
                BookingNo = bookingDetails.BookingNo,
                CustomerName = bookingDetails.CustomerName,
                CustomerId = bookingDetails.CustomerId,
                SupplierName = bookingDetails.SupplierName.Equals(bookingDetails.RegionalSupplierName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(bookingDetails.RegionalSupplierName) ? bookingDetails.SupplierName : bookingDetails.SupplierName + " (" + bookingDetails.RegionalSupplierName + ")",
                FactoryName = bookingDetails.FactoryName.Equals(bookingDetails.RegionalFactoryName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(bookingDetails.RegionalFactoryName) ? bookingDetails.FactoryName : bookingDetails.FactoryName + " (" + bookingDetails.RegionalFactoryName + ")",
                ServiceDateFrom = bookingDetails.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = bookingDetails.ServiceDateTo.ToString(StandardDateFormat),
                ServiceTypeId = serviceType.serviceTypeId,
                ServiceType = serviceType.serviceTypeName,
                FirstProductName = _productName,
                BookingQty = _bookingQty,
                CountryName = factoryDetails.FirstOrDefault().CountryName,
                OfficeName = bookingDetails.Office,
                BookingStatus = bookingDetails.BookingStatus,
            };
            response.Data = _data;
            response.Result = BookingDatainfoResult.Success;
            return response;
        }
        //get all booking no data source
        public async Task<BookingNoDataSourceResponse> GetBookingNoDataSource(BookingNoDataSourceRequest request)
        {
            var response = new BookingNoDataSourceResponse();



            var data = _repo.GetInspectionNo();

            if (data == null)
            {
                response.Result = BookingDatainfoResult.RequestNotCorrectFormat;
                return response;
            }
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => EF.Functions.Like(x.ToString(), $"%{request.SearchText.Trim()}%"));
            }

            if (request.Id > 0)
            {
                data = data.Where(x => x == request.Id);
            }

            var bookingNolist = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

            if (bookingNolist == null || !bookingNolist.Any())
                response.Result = BookingDatainfoResult.CannotGetList;
            else
            {
                response.DataSourceList = bookingNolist.Select(x => new BookingNo
                {
                    Id = x,
                    Name = x.ToString()
                }).OrderBy(x => x.Id).ToList();
                response.Result = BookingDatainfoResult.Success;
            }
            return response;
        }

        public async Task<bool> GetInspectionPickingExists(InspectionPickingExistRequest request)
        {
            var pickingExists = false;
            if (request != null)
            {
                pickingExists = await _pickingRepository.GetInspectionPickingExists(request.PoTransactionIds);
            }
            return pickingExists;
        }

        /// <summary>
        /// Get the inspection base details with the products base details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<InspectionProductBaseDetailResponse> GetInspectionProductBaseDetails(int bookingId)
        {
            var response = new InspectionProductBaseDetailResponse() { Result = InspectionProductBaseDetailResult.NotFound };
            //get the booking base details
            var inspectionDetail = await _repo.GetBookingDetails(bookingId);

            if (inspectionDetail != null)
            {
                var inspectionBaseAndProductDetail = MapInspectionBaseDetail(inspectionDetail);

                var serviceTypes = await _repo.GetBookingServiceTypes(new[] { bookingId });

                if (serviceTypes != null && serviceTypes.Any())
                {
                    inspectionBaseAndProductDetail.ServiceType = serviceTypes.FirstOrDefault()?.ServiceTypeName;
                }

                var factoryDetails = await _repo.GetFactorycountryId(new[] { bookingId });

                MapFactoryCountryDetails(factoryDetails, inspectionBaseAndProductDetail);

                var productDetails = await _repo.GetProductItemByBooking(bookingId);

                MapProductDetails(productDetails.ToList(), inspectionBaseAndProductDetail);

                response.InspectionBaseDetail = inspectionBaseAndProductDetail;
                response.Result = InspectionProductBaseDetailResult.Success;
            }
            return response;
        }

        /// <summary>
        /// Map the inspection base detail
        /// </summary>
        /// <param name="inspectionDetail"></param>
        /// <returns></returns>
        private InspectionBaseAndProductDetails MapInspectionBaseDetail(BookingDataRepo inspectionDetail)
        {
            return new InspectionBaseAndProductDetails()
            {
                BookingNo = inspectionDetail.BookingNo,
                Status = inspectionDetail.Status,
                Customer = inspectionDetail.CustomerName,
                Supplier = inspectionDetail.SupplierName,
                Factory = inspectionDetail.FactoryName,
                InspectionDate = inspectionDetail.ServiceDateFrom == inspectionDetail.ServiceDateTo ?
                inspectionDetail.ServiceDateFrom.ToString(StandardDateFormat) : inspectionDetail.ServiceDateFrom.ToString(StandardDateFormat) + " - "
                + inspectionDetail.ServiceDateTo.ToString(StandardDateFormat)
            };
        }

        /// <summary>
        /// Map the factory country details
        /// </summary>
        /// <param name="factoryDetails"></param>
        /// <param name="inspectionBaseAndProductDetail"></param>
        /// <returns></returns>
        private InspectionBaseAndProductDetails MapFactoryCountryDetails(List<FactoryCountry> factoryDetails, InspectionBaseAndProductDetails inspectionBaseAndProductDetail)
        {
            if (factoryDetails.Any())
            {
                var factoryDetail = factoryDetails.FirstOrDefault();
                inspectionBaseAndProductDetail.Country = factoryDetail.CountryName;
                inspectionBaseAndProductDetail.Province = factoryDetail.ProvinceName;
                inspectionBaseAndProductDetail.City = factoryDetail.CityName;
                inspectionBaseAndProductDetail.County = factoryDetail.CountyName;
                inspectionBaseAndProductDetail.Town = factoryDetail.TownName;

            }

            return inspectionBaseAndProductDetail;
        }

        /// <summary>
        /// Map the product details
        /// </summary>
        /// <param name="productDetails"></param>
        /// <param name="inspectionBaseAndProductDetail"></param>
        /// <returns></returns>
        private InspectionBaseAndProductDetails MapProductDetails(List<BookingProductinfo> productDetails, InspectionBaseAndProductDetails inspectionBaseAndProductDetail)
        {
            if (productDetails.Any())
            {
                inspectionBaseAndProductDetail.TotalBookingQuantity = productDetails.Sum(x => x.BookingQuantity.GetValueOrDefault());
                inspectionBaseAndProductDetail.Unit = string.Join(", ", productDetails.Select(x => x.UnitName).Distinct().ToList());

                inspectionBaseAndProductDetail.productBaseDetails = productDetails.Select(x => new InpectionProductBaseDetail()
                {
                    ProductName = x.ProductName,
                    ProductDesc = x.ProductDescription,
                    Unit = x.UnitName,
                    Quantity = x.BookingQuantity.GetValueOrDefault()
                }).ToList();
            }

            return inspectionBaseAndProductDetail;
        }

        public async Task<IEnumerable<EntMasterConfig>> GetMasterConfiguration()
        {
            return await _userConfigRepo.GetMasterConfiguration();
        }

        /// <summary>
        /// Get the Ent Page Field Access
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<EntPageFieldAccessResponse> GetEntPageFieldAccess(EntPageRequest request)
        {
            var response = new EntPageFieldAccessResponse() { Result = EntPageFieldAccessResult.NotFound };
            request.EntityId = _filterService.GetCompanyId();
            request.UserTypeId = (int)_ApplicationContext.UserType;
            var entPageFileAccess = await _repo.GetEntPageFieldAccess(request);
            if (entPageFileAccess.Any())
            {
                response.Result = EntPageFieldAccessResult.Success;
                response.EntPageFieldAccess = entPageFileAccess;
            }
            return response;
        }

        /// <summary>
        /// Save the master contact data(customer,supplier,factory)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveMasterContactResponse> SaveMasterContact(SaveMasterContactRequest request)
        {
            SaveMasterContactResponse response = null;
            if (request != null)
            {
                switch (request.MasterContactTypeId)
                {
                    case (int)MasterContactType.Customer:
                        {
                            response = await AddCustomerContact(request);
                            break;
                        }
                    case (int)MasterContactType.Supplier:
                        {
                            response = await AddSupplierOrFactoryContact(request, (int)Supplier_Type.Supplier_Agent);
                            break;
                        }
                    case (int)MasterContactType.Factory:
                        {
                            response = await AddSupplierOrFactoryContact(request, (int)Supplier_Type.Factory);
                            break;
                        }
                }
            }
            return response;
        }

        /// <summary>
        /// Add the customer contacts
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<SaveMasterContactResponse> AddCustomerContact(SaveMasterContactRequest request)
        {
            SaveMasterContactResponse response = new SaveMasterContactResponse() { Result = SaveMasterContactResult.Failed };
            if (request.contactList != null && request.contactList.Any())
            {
                List<CuContact> contactList = new List<CuContact>();

                var entityId = _filterService.GetCompanyId();

                var serviceId = (int)Service.InspectionId;

                //get the email ids
                var emailIds = request.contactList.Where(x => x.EmailId != null).Select(x => x.EmailId.ToLower()).ToList();

                //get the email ids mapped for the contacts from list of emails
                var customerContactEmailIds = await _customerContactRepo.GetContactEmailIds(emailIds);

                //if available then send the duplicate email ids
                if (customerContactEmailIds.Any())
                    return new SaveMasterContactResponse() { DuplicateEmailIds = customerContactEmailIds, Result = SaveMasterContactResult.DuplicateEmailFound };

                var headOfficeId = await _customerRepo.GetCustomerHeadOfficeAddressById(request.CustomerId);

                //add contact list data
                foreach (var contact in request.contactList)
                {

                    var customerContact = AddCustomerContact(contact, request, headOfficeId, entityId);

                    AddCustomerContactType(customerContact);

                    AddCustomerContactService(customerContact, serviceId);

                    AddCustomerContactEntity(customerContact, entityId);

                    AddCustomerContactEntityServiceMap(customerContact, serviceId, entityId);

                    contactList.Add(customerContact);
                }

                _repo.SaveList(contactList, false);
                response.Result = SaveMasterContactResult.Success;
            }
            return response;
        }

        /// <summary>
        /// Add the customer contact data
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="request"></param>
        /// <param name="headOfficeId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        private CuContact AddCustomerContact(MasterContact contact, SaveMasterContactRequest request, int headOfficeId, int entityId)
        {
            return new CuContact()
            {
                ContactName = contact.Name,
                Email = contact.EmailId,
                Phone = contact.PhoneNo,
                CustomerId = request.CustomerId,
                Office = headOfficeId,
                Active = true,
                CreatedBy = _ApplicationContext.UserId,
                PrimaryEntity = entityId,
                CreatedOn = DateTime.Now
            };

        }

        /// <summary>
        /// Add default customer contact type
        /// </summary>
        /// <param name="customerContact"></param>
        private void AddCustomerContactType(CuContact customerContact)
        {
            var customerContactType = new CuCustomerContactType() { ContactTypeId = (int)CustomerContactType.Operation };

            customerContact.CuCustomerContactTypes.Add(customerContactType);

            _repo.AddEntity(customerContactType);
        }

        /// <summary>
        /// Add default customer contact service
        /// </summary>
        /// <param name="customerContact"></param>
        /// <param name="serviceId"></param>
        private void AddCustomerContactService(CuContact customerContact, int serviceId)
        {
            customerContact.CuContactServices = new List<CuContactService>();

            var cuContactService = new CuContactService
            {
                ServiceId = serviceId,
                Active = true,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now
            };

            customerContact.CuContactServices.Add(cuContactService);

            _repo.AddEntity(cuContactService);
        }

        /// <summary>
        /// Add default customer contact entity
        /// </summary>
        /// <param name="customerContact"></param>
        /// <param name="entityId"></param>
        private void AddCustomerContactEntity(CuContact customerContact, int entityId)
        {
            customerContact.CuContactEntityMaps = new List<CuContactEntityMap>();

            var cuContactEntity = new CuContactEntityMap
            {
                EntityId = entityId
            };

            customerContact.CuContactEntityMaps.Add(cuContactEntity);

            _repo.AddEntity(cuContactEntity);
        }

        /// <summary>
        /// Add default customer contact entity service map
        /// </summary>
        /// <param name="customerContact"></param>
        /// <param name="serviceId"></param>
        /// <param name="entityId"></param>
        private void AddCustomerContactEntityServiceMap(CuContact customerContact, int serviceId, int entityId)
        {

            customerContact.CuContactEntityServiceMaps = new List<CuContactEntityServiceMap>();

            var cuContactServiceEntity = new CuContactEntityServiceMap
            {
                EntityId = entityId,
                ServiceId = serviceId
            };

            _repo.AddEntity(cuContactServiceEntity);

            customerContact.CuContactEntityServiceMaps.Add(cuContactServiceEntity);
        }


        /// <summary>
        /// Add supplier or factory contacts
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<SaveMasterContactResponse> AddSupplierOrFactoryContact(SaveMasterContactRequest request, int typeId)
        {
            SaveMasterContactResponse response = new SaveMasterContactResponse() { Result = SaveMasterContactResult.Failed };
            if (request.contactList != null && request.contactList.Any())
            {
                List<SuContact> contactList = new List<SuContact>();

                var entityId = _filterService.GetCompanyId();

                var serviceId = (int)Service.InspectionId;

                //get the email ids
                var emailIds = request.contactList.Where(x => x.EmailId != null).Select(x => x.EmailId).ToList();

                emailIds = emailIds.ConvertAll(d => d.ToLower());

                //get the email ids mapped for the contacts from list of emails
                var supplierContactEmailIds = await _supplierRepo.GetContactEmailIds(emailIds, typeId);

                //if available then send the duplicate email ids
                if (supplierContactEmailIds.Any())
                    return new SaveMasterContactResponse() { DuplicateEmailIds = supplierContactEmailIds, Result = SaveMasterContactResult.DuplicateEmailFound };

                foreach (var contact in request.contactList)
                {

                    var supplierContact = AddSupplierContact(contact, request, entityId, typeId);

                    AddSupplierOrFactoryContactServices(supplierContact, serviceId);

                    AddSupplierOrFactoryContactEntity(supplierContact, entityId);

                    AddSupplierOrFactoryContactServiceEntityMap(supplierContact, entityId, serviceId);

                    MapCustomerWithSupplierOrFactoryContact(supplierContact, request);

                    contactList.Add(supplierContact);
                }

                _repo.SaveList(contactList, false);

                response.Result = SaveMasterContactResult.Success;
            }

            return response;
        }

        /// <summary>
        /// Add supplier or factory contact
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="request"></param>
        /// <param name="entityId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        private SuContact AddSupplierContact(MasterContact contact, SaveMasterContactRequest request, int entityId, int typeId)
        {
            return new SuContact()
            {
                ContactName = contact.Name,
                Mail = contact.EmailId,
                Phone = contact.PhoneNo,
                SupplierId = typeId == (int)Supplier_Type.Supplier_Agent ? request.SupplierId.GetValueOrDefault() : request.FactoryId.GetValueOrDefault(),
                Active = true,
                PrimaryEntity = entityId,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now
            };
        }

        /// <summary>
        /// Add Supplier or factory contact services
        /// </summary>
        /// <param name="supplierContact"></param>
        /// <param name="serviceId"></param>
        private void AddSupplierOrFactoryContactServices(SuContact supplierContact, int serviceId)
        {
            supplierContact.SuContactApiServices = new List<SuContactApiService>();

            var suContactAPIServices = new SuContactApiService
            {
                ServiceId = serviceId,
                Active = true,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now
            };

            supplierContact.SuContactApiServices.Add(suContactAPIServices);
            _repo.AddEntity(suContactAPIServices);
        }

        /// <summary>
        /// Add Supplier or factory contact entity
        /// </summary>
        /// <param name="supplierContact"></param>
        /// <param name="entityId"></param>
        private void AddSupplierOrFactoryContactEntity(SuContact supplierContact, int entityId)
        {
            supplierContact.SuContactEntityMaps = new List<SuContactEntityMap>();

            var suContactEntity = new SuContactEntityMap
            {
                EntityId = entityId
            };

            supplierContact.SuContactEntityMaps.Add(suContactEntity);
            _repo.AddEntity(suContactEntity);
        }

        /// <summary>
        /// Add supplier or factory contact entity map
        /// </summary>
        /// <param name="supplierContact"></param>
        /// <param name="entityId"></param>
        /// <param name="serviceId"></param>
        private void AddSupplierOrFactoryContactServiceEntityMap(SuContact supplierContact, int entityId, int serviceId)
        {
            supplierContact.SuContactEntityServiceMaps = new List<SuContactEntityServiceMap>();

            var suContactServiceEntity = new SuContactEntityServiceMap
            {
                EntityId = entityId,
                ServiceId = serviceId
            };

            supplierContact.SuContactEntityServiceMaps.Add(suContactServiceEntity);
            _repo.AddEntity(suContactServiceEntity);
        }

        /// <summary>
        /// Map supplier or factory contact with the customer
        /// </summary>
        /// <param name="supplierContact"></param>
        /// <param name="request"></param>
        private void MapCustomerWithSupplierOrFactoryContact(SuContact supplierContact, SaveMasterContactRequest request)
        {
            supplierContact.SuSupplierCustomerContacts = new List<SuSupplierCustomerContact>();

            var suSupplierCustomerContact = new SuSupplierCustomerContact()
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId.GetValueOrDefault(),
            };

            supplierContact.SuSupplierCustomerContacts.Add(suSupplierCustomerContact);
            _repo.AddEntity(suSupplierCustomerContact);
        }

        public async Task<IEnumerable<ServiceTypeList>> GetServiceType(IEnumerable<int> bookingIds)
        {
            return await _repo.GetServiceType(bookingIds);
        }

        /// <summary>
        /// Get the po product details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<POProductDetailResponse> GetPoProductDetails(PoProductDetailRequest request)
        {
            var response = new POProductDetailResponse();

            var poProductDetail = new POProductDetail();

            //generate the search po request
            var bookingPoProductSearchData = new BookingPoProductSearchData();

            bookingPoProductSearchData.PoId = request.PoId;
            bookingPoProductSearchData.ProductId = request.ProductId;
            bookingPoProductSearchData.CustomerId = request.CustomerId;
            bookingPoProductSearchData.SupplierId = request.SupplierId;

            ////get the po data by po id,customerid,supplierid
            var purchaseOrderDetailAvailable = await _repo.CheckPoDetailAvailableByPoProductDetail(bookingPoProductSearchData);

            if (purchaseOrderDetailAvailable)
            {
                var purchaseOrderDetails = _poRepo.GetPurchaseOrderDetailsAndPOSupplierData();

                if (bookingPoProductSearchData.PoId > 0)
                    purchaseOrderDetails = purchaseOrderDetails.Where(x => x.PoId == bookingPoProductSearchData.PoId);
                if (bookingPoProductSearchData.ProductId > 0)
                    purchaseOrderDetails = purchaseOrderDetails.Where(x => x.ProductId == bookingPoProductSearchData.ProductId);
                if (bookingPoProductSearchData.CustomerId > 0)
                    purchaseOrderDetails = purchaseOrderDetails.Where(x => x.Po.CustomerId == bookingPoProductSearchData.CustomerId);

                poProductDetail = await purchaseOrderDetails.Select(x => new POProductDetail()
                {
                    Id = x.Product.Id,
                    Name = x.Product.ProductId,
                    Description = x.Product.ProductDescription,
                    PoQuantity = x.Quantity,
                    ProductCategoryId = x.Product.ProductCategory,
                    ProductCategoryName = x.Product.ProductCategoryNavigation.Name,
                    ProductSubCategoryId = x.Product.ProductSubCategory,
                    ProductSubCategoryName = x.Product.ProductSubCategoryNavigation.Name,
                    ProductSubCategory2Id = x.Product.ProductCategorySub2,
                    ProductSubCategory2Name = x.Product.ProductCategorySub2Navigation.Name,
                    ProductSubCategory3Id = x.Product.ProductCategorySub3,
                    ProductSubCategory3Name = x.Product.ProductCategorySub3Navigation.Name,
                    BarCode = x.Product.Barcode,
                    FactoryReference = x.Product.FactoryReference,
                    IsNewProduct = x.Product.IsNewProduct,
                    Remarks = x.Product.Remarks,
                    DestinationCountryId = x.DestinationCountryId,
                    Etd = x.Etd != null ? Static_Data_Common.GetCustomDate(x.Etd) : null,
                    ProductImageCount = x.Product.CuProductFileAttachments.Where(x => x.Active && x.FileTypeId.HasValue && x.FileTypeId.Value == (int)ProductRefFileType.ProductRefPictures).Select(x => x.Id).Count()
                }).FirstOrDefaultAsync();

            }
            else if (!purchaseOrderDetailAvailable)
            {
                var customerProductData = _productRepo.GetCustomerProductDataSource();

                if (bookingPoProductSearchData.ProductId > 0)
                    customerProductData = customerProductData.Where(x => x.Id == bookingPoProductSearchData.ProductId);

                poProductDetail = await customerProductData.Select(x => new POProductDetail()
                {
                    Id = x.Id,
                    Name = x.ProductId,
                    Description = x.ProductDescription,
                    ProductCategoryId = x.ProductCategory,
                    ProductCategoryName = x.ProductCategoryNavigation.Name,
                    ProductSubCategoryId = x.ProductSubCategory,
                    ProductSubCategoryName = x.ProductSubCategoryNavigation.Name,
                    ProductSubCategory2Id = x.ProductCategorySub2,
                    ProductSubCategory2Name = x.ProductCategorySub2Navigation.Name,
                    ProductSubCategory3Id = x.ProductCategorySub3,
                    ProductSubCategory3Name = x.ProductCategorySub3Navigation.Name,
                    BarCode = x.Barcode,
                    FactoryReference = x.FactoryReference,
                    IsNewProduct = x.IsNewProduct,
                    Remarks = x.Remarks,
                    ProductImageCount = x.CuProductFileAttachments.Where(x => x.Active && x.FileTypeId.HasValue && x.FileTypeId.Value == (int)ProductRefFileType.ProductRefPictures).Select(x => x.Id).Count()
                }).FirstOrDefaultAsync();

            }

            if (poProductDetail != null)
            {
                response.POProductDetail = poProductDetail;
                response.Result = POProductDetailResult.Success;
            }
            else
                response.Result = POProductDetailResult.NotFound;

            //_repo.GetProductMap
            return response;
        }

        /// <summary>
        /// Get the booking file attachment data
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<BookingFileZipResponse> GetBookingFileAttachment(int bookingId)
        {
            var response = new BookingFileZipResponse();
            var fileZipAttachment = await _repo.GetBookingFileAttachment(bookingId);
            if (fileZipAttachment != null)
            {
                response.FileAttachment = fileZipAttachment;
                response.Result = BookingFileZipResult.Success;
            }
            else
                response.Result = BookingFileZipResult.NotFound;

            return response;

        }
        public async Task<IEnumerable<ProductTranData>> GetProductDetails(IEnumerable<int> bookingIds)
        {
            return await _repo.GetProductDetails(bookingIds);
        }

        /// <summary>
        /// get po details
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingProductPoRepo>> GetPoDataByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _repo.GetPoDataByBookingIds(bookingIds);
        }

        /// <summary>
        /// Get the purchase order sample file
        /// </summary>
        /// <returns></returns>
        public async Task<FileResponse> GetPurchaseOrderSampleFile()
        {
            var response = new PurchaseOrderSampleResponse();

            var filepath = _env.WebRootPath;
            var masterConfigs = await GetMasterConfiguration();

            var fileUrl = masterConfigs.Where(x => x.EntityId == _filterService.GetCompanyId()
                                            && x.Type == (int)EntityConfigMaster.ImportPurchaseOrderUpload).
                                            Select(x => x.Value).FirstOrDefault();

            //concat the filename and url
            var fileName = string.Concat(filepath, fileUrl);

            if (string.IsNullOrEmpty(fileName))
                return null;
            //read the file content
            var filecontent = FileParser.ReadFiletoByteArray(fileName);

            if (filecontent == null)
                return null;

            return new FileResponse
            {
                Content = filecontent,
                MimeType = _fileManager.GetMimeType(Path.GetExtension(fileName)),
                Result = FileResult.Success
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="factoryId"></param>
        /// <param name="bookingStatusId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetCCEmailConfigurationEmailsByCustomer(int customerId, int factoryId, int bookingStatusId)
        {
            List<string> ccEmails = null;
            if (await _customerCheckPointRepository.IsCustomerCheckpointConfigured(customerId, (int)CheckPointTypeEnum.SendBookingEmailToCustomer))
            {
                var factoryAddress = await _supplierRepo.GetSupplierHeadOfficeAddress(factoryId);
                if (factoryAddress != null)
                {
                    //based on factory country id found the data
                    var inspBookingEmailConfiguration = await _repo.GetCCEmailConfigurationEmailsByCustomer(customerId, factoryAddress.countryId, bookingStatusId);
                    if (inspBookingEmailConfiguration == null)
                    {
                        //if based on factory country is not found, then fetch the default data (means factory country id is null)
                        inspBookingEmailConfiguration = await _repo.GetCCEmailConfigurationEmailsByCustomer(customerId, null, bookingStatusId);
                    }
                    if (inspBookingEmailConfiguration != null)
                        return inspBookingEmailConfiguration.Email.Split(";").Distinct().Where(y => IsValidEmail(y)).ToList();
                }
            }
            return ccEmails;
        }
        /// <summary>
        /// get booking details
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingDetail>> GetBookingDetails(IEnumerable<int> bookingIds)
        {
            return await _repo.GetBookingData(bookingIds);
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
        public async Task<EaqfErrorResponse> ValidateEaqfBooking(SaveEaqfInsepectionRequest request)
        {
            if (request.Vendor == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { RequiredProdutDetail });
            }

            if (request.EaqfInspectionProductList == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { RequiredVendor });
            }

            if (string.IsNullOrEmpty(request.Vendor.Name.Trim()))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidVendor });
            }

            if (string.IsNullOrEmpty(request.Vendor.Country.Trim()))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidCountry });
            }

            if (!(await _productManagementRepository.IsProductCategoryAvailbyId(request.ProductCategoryId.Value, _filterService.GetCompanyId())))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidProductCategory });
            }

            if (!(await _productManagementRepository.IsProductSubCategoryAvailbyId(request.ProductSubCategoryId.Value, request.ProductCategoryId.Value, _filterService.GetCompanyId())))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidProductSubCategory });
            }

            if (!(await _productManagementRepository.IsProductSub2CategoryAvailbyId(request.ProductType, request.ProductSubCategoryId.Value, _filterService.GetCompanyId())))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidProductSub2Category });
            }

            if (!(await _referenceRepo.IsServiceTypeMappedWithBusinessLine(request.ServiceTypeId, request.BusinessLine, _filterService.GetCompanyId())))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidServiceType });
            }

            if (!(await _referenceRepo.IsValidEntity(_filterService.GetCompanyId())))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidServiceType });
            }

            var customer = _customerRepo.GetCustomerByID(request.CustomerId);
            if (customer == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { CustomerNotFound });
            }

            if (request.BookingId != 0 && !(await _repo.CheckInspectionBookingByCustomerId(request.BookingId, request.CustomerId)))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidBookingForCustomer });
            }

            return null;
        }

        /// <summary>
        /// Add the supplier data to purchase orders
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddPoSuppliers(List<int> supplierIds, CuPurchaseOrder entity, int userId)
        {
            if (supplierIds != null && supplierIds.Any())
            {
                int userIdData = userId > 0 ? userId : _ApplicationContext.UserId;
                foreach (var supplierId in supplierIds)
                {
                    if (!entity.CuPoSuppliers.Any(x => x.SupplierId == supplierId && x.Active.HasValue && x.Active.Value))
                    {
                        CuPoSupplier poSupplier = new CuPoSupplier();
                        poSupplier.SupplierId = supplierId;
                        poSupplier.CreatedBy = userIdData;
                        poSupplier.CreatedOn = DateTime.Now;
                        poSupplier.Active = true;
                        _repo.AddEntity(poSupplier);
                        entity.CuPoSuppliers.Add(poSupplier);
                    }
                }

            }
        }

        /// <summary>
        /// Add the factories to purchase order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddPoFactories(List<int> factoryIds, CuPurchaseOrder entity, int userId)
        {
            if (factoryIds != null && factoryIds.Any())
            {
                int userIdData = userId > 0 ? userId : _ApplicationContext.UserId;
                foreach (var factoryId in factoryIds)
                {
                    if (!entity.CuPoFactories.Any(x => x.FactoryId == factoryId && x.Active.HasValue && x.Active.Value))
                    {
                        CuPoFactory poFactory = new CuPoFactory();
                        poFactory.FactoryId = factoryId;
                        poFactory.Active = true;
                        poFactory.CreatedBy = userIdData;
                        poFactory.CreatedOn = DateTime.Now;
                        _repo.AddEntity(poFactory);
                        entity.CuPoFactories.Add(poFactory);
                    }

                }
            }
        }

        /// <summary>
        /// Eaqf event update
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> EaqfBookingEventUpdate(int bookingId, EaqfEvent request)
        {
            if (request == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Request object is not valid" });
            }

            if (bookingId <= 0)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Booking id is not valid" });
            }

            if (string.IsNullOrEmpty(request.InvoiceNo))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invoice number is not valid" });
            }

            int userId = request.UserId;

            var bookingEntity = await _repo.GetBookingDataUptoInspected(bookingId);

            if (bookingEntity == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "System can not cancel the booking due to report generated" });
            }

            // check the booking status is valid to cancel it

            if (request.Classification.Trim().ToLower() == CancelRefund.ToLower())
            {
                await CancelBooking(bookingId, userId, (int)CancelReasonType.RequestByEaqf, (int)CancelTimeType.After48Hours, bookingEntity);
                await CancelManualInvoice(bookingId, userId, request.InvoiceNo);
                await CancelQuotation(bookingId, userId);
            }
            if (request.Classification.Trim().ToLower() == CancelPartialRefund.ToLower())
            {
                await CancelBooking(bookingId, userId, (int)CancelReasonType.RequestByEaqf, (int)CancelTimeType.TwentFourHoursTo48Hours, bookingEntity);
                await UpdateManualInvoice(bookingId, userId, request.InvoiceNo);
                await UpdateQuotation(bookingId, userId);
                await CancelQuotation(bookingId, userId);
            }
            if (request.Classification.Trim().ToLower() == CancelNoRefund.ToLower())
            {
                await CancelBooking(bookingId, userId, (int)CancelReasonType.RequestByEaqf, (int)CancelTimeType.LessThan24, bookingEntity);
                await CancelQuotation(bookingId, userId);
            }

            return new EaqfGetSuccessResponse()
            {
                message = "Success",
                statusCode = HttpStatusCode.OK
            };
        }

        private async Task<object> CancelBooking(int bookingId, int userId, int reasontypeId, int timeTypeId, InspTransaction bookingEntity)
        {
            var bookingCancelRequest = new InspTranCancel
            {
                InspectionId = bookingId,
                InternalComments = "Booking cancelled from EAQF",
                ReasonTypeId = reasontypeId,
                TimeTypeId = timeTypeId,
                CreatedBy = userId > 0 ? userId : null
            };

            int id = await _cancelBookingRepository.SaveCancelDetail(bookingCancelRequest);

            if (id > 0)
            {
                bookingEntity.StatusId = (int)BookingStatus.Cancel;
                bookingEntity.Id = bookingId;
                bookingEntity.InternalComments = bookingEntity.InternalComments + "Cancelled on " + DateTime.Now.ToString(StandardDateFormat) + " due to booking cancelled from EAQF";

                var statusLog = new InspTranStatusLog
                {
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    StatusId = (int)BookingStatus.Cancel,
                    StatusChangeDate = DateTime.Now,
                    EntityId = bookingEntity.EntityId
                };

                bookingEntity.InspTranStatusLogs.Add(statusLog);
                _cancelBookingRepository.Save(bookingEntity, true); //update booking status.
            }

            return bookingEntity;
        }

        /// <summary>
        /// Get manual invoice and cancel it.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<object> CancelManualInvoice(int bookingId, int userId, string invoiceNo)
        {
            var response = new DeleteManualInvoiceResponse();

            var manualInvoice = await _manualInvoiceRepo.GetManualInvoiceByBookingIdAndInvoice(bookingId, invoiceNo);
            if (manualInvoice == null)
            {
                response.Result = ManualInvoiceResult.NotFound;
                return response;
            }
            var items = manualInvoice.InvManTranDetails.Where(x => x.Id == manualInvoice.Id);
            items.ToList().ForEach(x =>
            {
                x.Active = false;
                x.Remarks = "100% refund due to cancel the booking on 48h or more";
                x.DeletedBy = userId;
                x.DeletedOn = DateTime.Now;
            });

            _repo.EditEntities<InvManTranDetail>(items);

            manualInvoice.Status = (int)InvoiceStatus.Cancelled;
            manualInvoice.DeletedBy = userId;
            manualInvoice.DeletedOn = DateTime.Now;

            _repo.EditEntity<InvManTransaction>(manualInvoice);
            await _repo.Save();

            response.Result = ManualInvoiceResult.Success;
            return response;
        }

        private async Task<object> UpdateManualInvoice(int bookingId, int userId, string invoiceNo)
        {
            var response = new DeleteManualInvoiceResponse();

            var manualInvoice = await _manualInvoiceRepo.GetManualInvoiceByBookingIdAndInvoice(bookingId, invoiceNo);
            if (manualInvoice == null)
            {
                response.Result = ManualInvoiceResult.NotFound;
                return response;
            }

            manualInvoice.InvManTranDetails.Add(new InvManTranDetail()
            {
                Discount = 100,
                Subtotal = -100,
                Active = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                Description = "Cancel Partial Refund",
                Remarks = "Cancel Partial Refund from EAQF"
            });

            // update total amount

            manualInvoice.TotalAmount = manualInvoice.InvManTranDetails.Where(x => x.Active.Value).Sum(x => x.Subtotal);

            manualInvoice.UpdatedBy = userId;
            manualInvoice.UpdatedOn = DateTime.Now;

            _repo.EditEntity<InvManTransaction>(manualInvoice);

            await _repo.Save();

            response.Result = ManualInvoiceResult.Success;
            return response;
        }




        private async Task<object> CancelQuotation(int bookingId, int userId)
        {
            //get the quotation id
            var quotationId = await _cancelBookingRepository.GetBookingQuotationDetails(bookingId);
            if (quotationId > 0)
            {
                var quotationInspDateMandayList = await _quotationRepository.GetQuotationInspManDay(bookingId);

                if (quotationInspDateMandayList != null && quotationInspDateMandayList.Count() > 0)
                {
                    var quotationRecord = quotationInspDateMandayList.FirstOrDefault();
                    //if data exist make as inactive 
                    foreach (QuQuotationInspManday manDayInsp in quotationInspDateMandayList)
                    {
                        manDayInsp.Active = false;
                        manDayInsp.DeletedDate = DateTime.Now;
                        manDayInsp.DeletedBy = userId;
                    }
                    _quotationRepository.EditEntities(quotationInspDateMandayList);

                    // update quotation

                    if (quotationRecord != null && quotationRecord.Quotation != null)
                    {
                        quotationRecord.Quotation.ApiInternalRemark = "100% refund due to cancel the booking on 48h or more";
                        quotationRecord.Quotation.IdStatus = (int)QuotationStatus.Canceled;
                        _quotationRepository.EditEntity(quotationRecord.Quotation);
                        await _quotationRepository.Save();
                    }
                }
            }
            return bookingId;
        }

        private async Task<object> UpdateQuotation(int bookingId, int userId)
        {
            //get the quotation id
            var quotationId = await _cancelBookingRepository.GetBookingQuotationDetails(bookingId);
            if (quotationId > 0)
            {
                var quotationInspDateMandayList = await _quotationRepository.GetQuotationInspManDay(bookingId);
                var quotationRecord = quotationInspDateMandayList.FirstOrDefault();

                // update quotation

                if (quotationRecord != null && quotationRecord.Quotation != null)
                {
                    quotationRecord.Quotation.Discount = quotationRecord.Quotation.Discount + 100;
                    quotationRecord.Quotation.TotalCost = (quotationRecord.Quotation.TotalCost - quotationRecord.Quotation.Discount.GetValueOrDefault());
                    quotationRecord.Quotation.ApiInternalRemark = "24h to 48h add 100 as discount";
                    _quotationRepository.EditEntity(quotationRecord.Quotation);
                    await _quotationRepository.Save();
                }
            }
            return bookingId;
        }

        public async Task<object> SaveEaqfInspectionBooking(SaveEaqfInsepectionRequest request)
        {
            var entityId = _filterService.GetCompanyId();
            var userId = request.UserId;
            SuSupplier suSupplier = null;
            SaveSupplierResponse supplierResponse = null;
            SaveInsepectionRequest eaqfBooking = new SaveInsepectionRequest();

            eaqfBooking.UserId = userId;
            eaqfBooking.UserType = (int)UserTypeEnum.Customer;

            //Get customer contact details
            var customerContactId = await _userAccountRepository.GetCustomerContactIdByUser(userId);
            var cus = await _customerContactRepo.GetCustomerContactsList(new[] { customerContactId }.ToList());
            eaqfBooking.InspectionCustomerContactList = cus.Select(x => x.Id);

            eaqfBooking.ApplicantName = cus.Select(x => x.ContactName).FirstOrDefault();
            eaqfBooking.ApplicantEmail = cus.Select(x => x.Email).FirstOrDefault();
            eaqfBooking.ApplicantPhoneNo = cus.Select(x => x.Phone).FirstOrDefault();

            if (request.CustomerId != cus.Select(x => x.CustomerId).FirstOrDefault())
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { CustomerNotMapped });
            }


            //added custom validations if validation fails return response
            var errorResponse = await ValidateEaqfBooking(request);
            if (errorResponse != null)
                return errorResponse;

            // Add  Booking Log information - in the Event booking log table.

            await _eventBookingLog.SaveLogInformation(new EventBookingLogInfo()
            {
                Id = 0,
                AuditId = 0,
                BookingId = request.BookingId,
                LogInformation = JsonConvert.SerializeObject(request),
                UserId = userId
            });

            //if id =0 then check the vendor available in our system . if yes then take the vendor id and map it. if not then create new vendor
            if (request.Vendor != null)
            {
                if (request.Vendor.Id == 0)
                {
                    if (!string.IsNullOrEmpty(request.Vendor.Name.Trim()))
                    {
                        suSupplier = await _supplierRepo.GetSupplierByName(request.Vendor.Name, (int)Supplier_Type.Supplier_Agent);

                        if (suSupplier == null)
                        {
                            supplierResponse = await _supplierManager.SaveEaqfSupplier(request.Vendor, (int)Supplier_Type.Supplier_Agent, request.CustomerId, request.UserId, null);
                            eaqfBooking.SupplierId = supplierResponse.Id;
                        }
                        else
                        {
                            eaqfBooking.SupplierId = suSupplier.Id;

                            var isSupplierMapped = await _supplierRepo.IsSupplierExistsByCustomer(suSupplier.Id, request.CustomerId, (int)Supplier_Type.Supplier_Agent);

                            if (!isSupplierMapped)
                            {
                                var suSupplierDetails = await _supplierRepo.GetSupplierById(suSupplier.Id, (int)Supplier_Type.Supplier_Agent);
                                suSupplierDetails.SuSupplierCustomers.Add(new SuSupplierCustomer { CustomerId = request.CustomerId, Code = "" });

                                _supplierRepo.EditEntity(suSupplierDetails);
                                await _supplierRepo.Save();
                            }

                            /// check for supplier contact
                            if (request.Vendor.eaqfSupplierContacts.Count > 0)
                            {
                                var suSupplierDetails = await _supplierRepo.GetSupplierById(suSupplier.Id, (int)Supplier_Type.Supplier_Agent);
                                var isNewAdded = false;
                                foreach (var supContact in request.Vendor.eaqfSupplierContacts)
                                {
                                    if (suSupplierDetails.SuContacts != null && !suSupplierDetails.SuContacts.Any(a => a.ContactName == supContact.ContactName))
                                    {
                                        isNewAdded = true;
                                        var suNewContacts = new SuContact()
                                        {
                                            ContactName = supContact.ContactName,
                                            Phone = supContact.ContactPhone,
                                            PrimaryEntity = entityId,
                                            CreatedOn = DateTime.Now,
                                            CreatedBy = userId
                                        };
                                        suNewContacts.SuSupplierCustomerContacts.Add(new SuSupplierCustomerContact()
                                        {
                                            SupplierId = eaqfBooking.SupplierId,
                                            CustomerId = request.CustomerId
                                        });
                                        suNewContacts.SuContactEntityMaps.Add(new SuContactEntityMap()
                                        {
                                            EntityId = entityId
                                        });

                                        suSupplierDetails.SuContacts.Add(suNewContacts);
                                    }
                                }

                                if (isNewAdded)
                                {
                                    _supplierRepo.EditEntity(suSupplierDetails);
                                    await _supplierRepo.Save();
                                }
                            }
                        }
                    }
                }
                else
                {
                    var sup = await _supplierRepo.GetSupplierById(request.Vendor.Id, (int)Supplier_Type.Supplier_Agent);

                    if (sup == null)
                    {
                        return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidVendor });
                    }
                    else
                    {
                        await _supplierManager.SaveEaqfSupplier(request.Vendor, (int)Supplier_Type.Supplier_Agent, request.CustomerId, request.UserId, null);
                        eaqfBooking.SupplierId = request.Vendor.Id;
                    }

                }
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { BadRequest });
            }

            if (request.Factory != null)
            {
                //if id=0 then check the factory available in our system . if yes then take the factory id and map it. if not then create new factory.
                if (request.Factory.Id == 0)
                {

                    if (!string.IsNullOrEmpty(request.Factory.Name.Trim()))
                    {
                        suSupplier = await _supplierRepo.GetSupplierByName(request.Factory.Name, (int)Supplier_Type.Factory);

                        if (suSupplier == null)
                        {
                            supplierResponse = await _supplierManager.SaveEaqfSupplier(request.Factory, (int)Supplier_Type.Factory, request.CustomerId, request.UserId, eaqfBooking.SupplierId);
                            eaqfBooking.FactoryId = supplierResponse.Id;
                        }
                        else
                        {
                            eaqfBooking.FactoryId = suSupplier.Id;
                            var isFactoryMapped = await _supplierRepo.IsSupplierExistsByCustomer(suSupplier.Id, request.CustomerId, (int)Supplier_Type.Factory);

                            if (!isFactoryMapped)
                            {
                                var suSupplierDetails = await _supplierRepo.GetSupplierById(suSupplier.Id, (int)Supplier_Type.Factory);
                                suSupplierDetails.SuSupplierCustomers.Add(new SuSupplierCustomer { CustomerId = request.CustomerId });

                                _supplierRepo.EditEntity(suSupplierDetails);
                                await _supplierRepo.Save();
                            }

                            /// check for factory contact
                            if (request.Factory.eaqfSupplierContacts.Count > 0)
                            {
                                var suSupplierDetails = await _supplierRepo.GetSupplierById(suSupplier.Id, (int)Supplier_Type.Factory);
                                var isNewAdded = false;
                                foreach (var supContact in request.Factory.eaqfSupplierContacts)
                                {
                                    if (suSupplierDetails.SuContacts != null && !suSupplierDetails.SuContacts.Any(a => a.ContactName == supContact.ContactName))
                                    {
                                        isNewAdded = true;
                                        var suNewContacts = new SuContact()
                                        {
                                            ContactName = supContact.ContactName,
                                            Phone = supContact.ContactPhone,
                                            PrimaryEntity = entityId,
                                            CreatedOn = DateTime.Now,
                                            CreatedBy = userId
                                        };
                                        suNewContacts.SuSupplierCustomerContacts.Add(new SuSupplierCustomerContact()
                                        {
                                            SupplierId = eaqfBooking.FactoryId.GetValueOrDefault(),
                                            CustomerId = request.CustomerId
                                        });
                                        suNewContacts.SuContactEntityMaps.Add(new SuContactEntityMap()
                                        {
                                            EntityId = entityId
                                        });

                                        suSupplierDetails.SuContacts.Add(suNewContacts);
                                    }
                                }

                                if (isNewAdded)
                                {
                                    _supplierRepo.EditEntity(suSupplierDetails);
                                    await _supplierRepo.Save();
                                }
                            }
                        }
                    }
                }
                else
                {
                    var fac = await _supplierRepo.GetSupplierById(request.Factory.Id, (int)Supplier_Type.Factory);

                    if (fac == null)
                    {
                        return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { FactoryNotFound });
                    }
                    else
                    {
                        await _supplierManager.SaveEaqfSupplier(request.Factory, (int)Supplier_Type.Factory, request.CustomerId, request.UserId, eaqfBooking.SupplierId);
                        eaqfBooking.FactoryId = request.Factory.Id;
                    }
                }
            }

            eaqfBooking.Id = request.BookingId;
            eaqfBooking.GuidId = Guid.NewGuid();
            eaqfBooking.CustomerId = request.CustomerId;
            eaqfBooking.InspectionServiceTypeList = new[] { request.ServiceTypeId };
            eaqfBooking.ServiceDateFrom = request.ServiceDateFrom;
            eaqfBooking.ServiceDateTo = request.ServiceDateTo;
            var masterConfigs = await GetMasterConfiguration();
            eaqfBooking.OfficeId = (Convert.ToInt32(masterConfigs.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.DefaultOfficeForEAQF && x.Active == true)?.Value));

            var supplierContactlst = await _supplierRepo.GetSuppliercontactById(eaqfBooking.SupplierId, request.CustomerId);

            if (supplierContactlst.Count == 0)
            {
                var supContacts = await _supplierRepo.GetSupplierContactById(eaqfBooking.SupplierId);
                eaqfBooking.InspectionSupplierContactList = supContacts.Select(x => x.ContactId.Value).Distinct().ToList();
            }
            else
            {
                var vendorContacts = request.Vendor.eaqfSupplierContacts.Select(s => s.ContactName).Distinct().ToList();
                eaqfBooking.InspectionSupplierContactList = supplierContactlst.Where(w => vendorContacts.Contains(w.ContactName)).Select(x => x.Id).Distinct().ToList();

                if (!eaqfBooking.InspectionSupplierContactList.Any())
                {
                    if (supplierContactlst.Count == 0)
                    {
                        eaqfBooking.InspectionSupplierContactList = (await _supplierRepo.GetSupplierContactById(eaqfBooking.SupplierId)).Select(x => x.ContactId.Value).Distinct().ToList();
                    }
                    else
                    {
                        eaqfBooking.InspectionSupplierContactList = supplierContactlst.Select(x => x.Id).Distinct().ToList();
                    }
                }
            }

            var factoryContactlst = await _supplierRepo.GetSuppliercontactById(eaqfBooking.FactoryId.GetValueOrDefault(), request.CustomerId);
            if (factoryContactlst.Count == 0)
            {
                var faContacts = await _supplierRepo.GetSupplierContactById(eaqfBooking.FactoryId.GetValueOrDefault());
                eaqfBooking.InspectionFactoryContactList = faContacts.Select(x => x.ContactId).Distinct().ToList();
            }
            else
            {
                var factoryContacts = request.Factory.eaqfSupplierContacts.Select(s => s.ContactName).Distinct().ToList();
                eaqfBooking.InspectionFactoryContactList = factoryContactlst.Where(w => factoryContacts.Contains(w.ContactName)).Select(x => x?.Id).Distinct().ToList();

                if (eaqfBooking.InspectionFactoryContactList.Count() == 0)
                {
                    if (factoryContactlst.Count == 0)
                    {
                        eaqfBooking.InspectionFactoryContactList = (await _supplierRepo.GetSupplierContactById(eaqfBooking.FactoryId.GetValueOrDefault())).Select(x => x?.ContactId).Distinct().ToList();
                    }
                    else
                    {
                        eaqfBooking.InspectionFactoryContactList = factoryContactlst.Select(x => x?.Id).Distinct().ToList();
                    }
                }
            }


            eaqfBooking.BusinessLine = request.BusinessLine;
            eaqfBooking.StatusId = (int)BookingStatusNames.Requested;
            eaqfBooking.CusBookingComments = request.Instructions;
            eaqfBooking.FirstServiceDateFrom = request.ServiceDateFrom;
            eaqfBooking.FirstServiceDateTo = request.ServiceDateTo;

            eaqfBooking.IsEaqf = true;
            eaqfBooking.IsSameDayReport = request.IsSameDayReport;
            eaqfBooking.ReportRequest = request.ReportRequest;
            eaqfBooking.CreatedBy = userId;
            eaqfBooking.CustomerBookingNo = request.EaqfRef;
            eaqfBooking.InspectionProductList = new List<SaveInspectionPOProductDetails>();
            foreach (var item in request.EaqfInspectionProductList)
            {
                CuProduct cuProduct = null;
                var isNewProduct = false;

                //check color code and color name required for softline
                if (request.BusinessLine == (int)BusinessLine.SoftLine && string.IsNullOrEmpty(item.ColorCode) || string.IsNullOrEmpty(item.ColorName))
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { ColorDataRequired });
                }

                var country = await _repo.GetSingleAsync<RefCountry>(x => x.Active && x.Alpha2Code.ToUpper().Trim() == item.DestinationCountry.ToUpper().Trim());

                if (!string.IsNullOrEmpty(item.ProductReference.Trim()))
                {
                    cuProduct = await _productRepo.GetProductRefByCustomer(request.CustomerId, item.ProductReference.Trim());
                    if (cuProduct == null)
                    {
                        isNewProduct = true;
                        cuProduct = new CuProduct()
                        {
                            Active = true,
                            ProductId = item.ProductReference,
                            CustomerId = request.CustomerId,
                            ProductDescription = item.Description,
                            CreatedBy = userId,
                            CreatedTime = DateTime.Now,
                            EntityId = entityId,
                            ProductCategory = request.ProductCategoryId,
                            ProductSubCategory = request.ProductSubCategoryId,
                            ProductCategorySub2 = request.ProductType
                        };
                        _repo.AddEntity(cuProduct);
                    }
                }

                if (!string.IsNullOrEmpty(item.PoNo.Trim()))
                {
                    var purchaseOrder = _poRepo.GetPurchaseOrderDetailsByCustomerAndPO(request.CustomerId, item.PoNo);
                    if (purchaseOrder == null)
                    {
                        purchaseOrder = new CuPurchaseOrder()
                        {
                            Pono = item.PoNo,
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now,
                            EntityId = entityId,
                            CustomerId = request.CustomerId,
                            Active = true,
                        };

                        if (eaqfBooking.SupplierId > 0)
                            AddPoSuppliers(new[] { eaqfBooking.SupplierId }.ToList(), purchaseOrder, userId);

                        if (eaqfBooking.FactoryId > 0)
                            AddPoFactories(new[] { eaqfBooking.FactoryId.GetValueOrDefault() }.ToList(), purchaseOrder, userId);

                        //AddInspectionPOS

                        purchaseOrder.CuPurchaseOrderDetails.Add(new CuPurchaseOrderDetail()
                        {
                            Active = true,
                            CreatedBy = userId,
                            CreatedTime = DateTime.Now,
                            Product = cuProduct,
                            EntityId = entityId,
                            //SupplierId = eaqfBooking.SupplierId,
                            //FactoryId = eaqfBooking.FactoryId,
                            Quantity = item.Quantity,
                            DestinationCountryId = country?.Id
                        });
                        _poRepo.AddEntity(purchaseOrder);
                    }
                    else
                    {
                        var poId = purchaseOrder.Id;

                        if (eaqfBooking.SupplierId > 0 && !purchaseOrder.CuPoSuppliers.Any(x => x.Active.Value && x.SupplierId == eaqfBooking.SupplierId))
                            AddPoSuppliers(new[] { eaqfBooking.SupplierId }.ToList(), purchaseOrder, userId);

                        if (eaqfBooking.FactoryId > 0 && !purchaseOrder.CuPoFactories.Any(x => x.Active.Value && x.FactoryId == eaqfBooking.FactoryId))
                            AddPoFactories(new[] { eaqfBooking.FactoryId.GetValueOrDefault() }.ToList(), purchaseOrder, userId);

                        if (isNewProduct)
                        {
                            purchaseOrder.CuPurchaseOrderDetails.Add(new CuPurchaseOrderDetail()
                            {
                                Active = true,
                                CreatedBy = userId,
                                CreatedTime = DateTime.Now,
                                Product = cuProduct,
                                EntityId = entityId,
                                //SupplierId = eaqfBooking.SupplierId,
                                //FactoryId = eaqfBooking.FactoryId,
                                Quantity = item.Quantity,
                                DestinationCountryId = country?.Id
                            });
                        }
                        else
                        {
                            var isProductMapped = await _poRepo.CheckPoProductIsMappedToBooking(poId, cuProduct.Id);

                            if (!isProductMapped)
                            {
                                var cuPurchaseOrderDetail = new CuPurchaseOrderDetail()
                                {
                                    Active = true,
                                    CreatedBy = userId,
                                    CreatedTime = DateTime.Now,
                                    ProductId = cuProduct.Id,
                                    EntityId = entityId,
                                    //SupplierId = eaqfBooking.SupplierId,
                                    //FactoryId = eaqfBooking.FactoryId,
                                    Quantity = item.Quantity,
                                    DestinationCountryId = country?.Id
                                };
                                purchaseOrder.CuPurchaseOrderDetails.Add(cuPurchaseOrderDetail);
                                _repo.AddEntity(cuPurchaseOrderDetail);
                            }
                            else
                            {
                                var cuEaqfPurchaseOrderDetails = purchaseOrder.CuPurchaseOrderDetails.FirstOrDefault(x => x.Active.Value && x.PoId == poId && x.ProductId == cuProduct.Id);
                                if (cuEaqfPurchaseOrderDetails != null)
                                {
                                    cuEaqfPurchaseOrderDetails.Active = true;
                                    cuEaqfPurchaseOrderDetails.CreatedBy = userId;
                                    cuEaqfPurchaseOrderDetails.CreatedTime = DateTime.Now;
                                    cuEaqfPurchaseOrderDetails.ProductId = cuProduct.Id;
                                    cuEaqfPurchaseOrderDetails.EntityId = entityId;
                                    //cuEaqfPurchaseOrderDetails.SupplierId = eaqfBooking.SupplierId;
                                    //cuEaqfPurchaseOrderDetails.FactoryId = eaqfBooking.FactoryId;
                                    cuEaqfPurchaseOrderDetails.Quantity = item.Quantity;
                                    cuEaqfPurchaseOrderDetails.DestinationCountryId = country?.Id;
                                }
                            }
                        }
                        _poRepo.EditEntity(purchaseOrder);
                    }

                    await _poRepo.Save();
                    var objInsPOProductDetails = new SaveInspectionPOProductDetails();

                    objInsPOProductDetails.PoId = purchaseOrder.Id;
                    objInsPOProductDetails.PoName = item.PoNo;
                    objInsPOProductDetails.ProductId = cuProduct.Id;
                    objInsPOProductDetails.ProductDesc = item.Description;
                    objInsPOProductDetails.Unit = item.Unit;
                    objInsPOProductDetails.Critical = item.AQLCritical;
                    objInsPOProductDetails.Aql = item.AqlLevel;
                    objInsPOProductDetails.Major = item.AQLMajor;
                    objInsPOProductDetails.Minor = item.AQLMinor;
                    objInsPOProductDetails.BookingQuantity = item.Quantity;
                    objInsPOProductDetails.ColorCode = item.ColorCode;
                    objInsPOProductDetails.ColorName = item.ColorName;
                    objInsPOProductDetails.ProductCategoryId = request.ProductCategoryId;
                    objInsPOProductDetails.ProductSubCategoryId = request.ProductSubCategoryId;
                    objInsPOProductDetails.ProductCategorySub2Id = request.ProductType;
                    objInsPOProductDetails.DestinationCountryID = country?.Id;
                    objInsPOProductDetails.IsGoldenSampleAvailable = request.IsGoldenSampleAvailable;
                    objInsPOProductDetails.GoldenSampleComments = request.GoldenSampleComments;
                    objInsPOProductDetails.IsSampleCollection = request.IsSampleCollection;
                    objInsPOProductDetails.SampleCollectionComments = request.SampleCollectionComments;
                    objInsPOProductDetails.ProductionStatus = request.ProductionStatus;
                    objInsPOProductDetails.PackingStatus = request.PackingStatus;
                    objInsPOProductDetails.ProductFileAttachments = new List<ProductFileAttachment>();

                    eaqfBooking.InspectionProductList.Add(objInsPOProductDetails);
                }
            }

            foreach (var file in request.Attachments)
            {
                eaqfBooking.InspectionFileAttachmentList = request.Attachments.Select(x => new BookingFileAttachment()
                {
                    uniqueld = Guid.NewGuid().ToString(),
                    FileName = x.FileName,
                    FileUrl = x.Url,
                    MimeType = x.FileType
                }).ToList();
            }

            var response = await SaveInspectionBooking(eaqfBooking);

            if (response.Result != SaveInspectionBookingResult.Success)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { response.Result.ToString() });
            }
            return new EaqfGetSuccessResponse()
            {
                message = "Success",
                statusCode = HttpStatusCode.OK,

                data = new BookingEaqfResponse()
                {
                    MissionId = response.Id,
                    VendorId = eaqfBooking.SupplierId,
                    FactoryId = eaqfBooking.FactoryId.GetValueOrDefault(),
                    IsTechincalDocumentsAddedOrRemoved = true,
                    StatusId = eaqfBooking.StatusId,
                    SaveInsepectionRequest = eaqfBooking
                }
            };
        }

        /// <summary>
        /// get factory country details for booking
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<FactoryCountry>> GetBookingFactorycountryDetails(IEnumerable<int> bookingIds)
        {
            return await _repo.GetFactorycountryId(bookingIds);
        }

        public async Task<EaqfErrorResponse> ValidateGetEaqfBooking(GetEaqfInspectionBookingRequest request)
        {
            var customer = _customerRepo.GetCustomerByID(request.CustomerId);

            if (request == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { BadRequest });
            }
            if (customer == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { CustomerNotFound });
            }
            if (request.CustomerId <= 0)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { CustomerNotFound });
            }

            return null;
        }
        public async Task<object> GetEaqfInspectionBooking(GetEaqfInspectionBookingRequest request)
        {
            var response = new GetEaqfInspectionBookingResponse();
            try
            {
                //added custom validations if validation fails return response
                var errorResponse = await ValidateGetEaqfBooking(request);
                if (errorResponse != null)
                    return errorResponse;

                InspectionSummarySearchRequest inspectionSummarySearchRequest = new InspectionSummarySearchRequest();

                inspectionSummarySearchRequest.Index = request.Index;
                inspectionSummarySearchRequest.pageSize = request.pageSize;
                inspectionSummarySearchRequest.CustomerId = request.CustomerId;
                try
                {
                    inspectionSummarySearchRequest.ServiceTypelst = request.ServiceType.Split(",").Select(x => int.Parse(x)).ToList();
                }
                catch (Exception)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Service Type is Invalid" });
                }

                inspectionSummarySearchRequest.DateTypeid = (int)SearchType.ServiceDate;
                DateTime fromDate;
                DateTime toDate;
                if (DateTime.TryParseExact(request.ServiceFromDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                        DateTimeStyles.None, out fromDate))
                {
                    inspectionSummarySearchRequest.FromDate = new DateObject() { Year = fromDate.Year, Month = fromDate.Month, Day = fromDate.Day };
                }
                else
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceFromDate });
                }
                if (DateTime.TryParseExact(request.ServiceToDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                        DateTimeStyles.None, out toDate))
                {
                    inspectionSummarySearchRequest.ToDate = new DateObject() { Year = toDate.Year, Month = toDate.Month, Day = toDate.Day };
                }
                else
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceToDate });
                }

                if (fromDate > toDate)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { GreterThanTodate });
                }

                var inspectionData = await GetAllInspectionsData(inspectionSummarySearchRequest);

                response.Index = inspectionData.Index;
                response.PageSize = inspectionData.PageSize;
                response.PageCount = inspectionData.PageCount;
                response.TotalCount = inspectionData.TotalCount;
                if (inspectionData.TotalCount == 0)
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = DataNotFound,
                        statusCode = HttpStatusCode.OK
                    };
                }
                var bookingIds = inspectionData.Data.Select(x => x.BookingId).ToList();

                var bookingLogStatus = _eventLogRepo.GetLogStatusByBooking(bookingIds);
                var eaqfBookingList = new List<EaqfBookingItem>();
                foreach (var item in inspectionData.Data)
                {
                    string servicedate = string.Empty;
                    var bookingLogStatues = bookingLogStatus.Where(x => x.BookingId == item.BookingId).ToList();

                    if (item.ServiceDateFromEaqf == item.ServiceDateToEaqf)
                    {
                        servicedate = item.ServiceDateToEaqf;
                    }
                    else
                    {
                        servicedate = item.ServiceDateFromEaqf + "-" + item.ServiceDateToEaqf;
                    }

                    var actualStatusList = GetBookingReportStatus(item.BookingId, bookingLogStatues, item.StatusId, item.CreatedDateEaqf, servicedate, true);
                    var eaqfBooking = new EaqfBookingItem()
                    {
                        BookingId = item.BookingId,
                        CustomerName = item.CustomerName,
                        ServiceType = item.ServiceType,
                        ServiceDateFrom = item.ServiceDateFromEaqf,
                        ServiceDateTo = item.ServiceDateToEaqf,
                        ProductCategory = item.ProductCategory,
                        VendorName = item.SupplierName,
                        FactoryName = item.FactoryName,
                        FactoryState = item.FactoryState,
                        FactoryCity = item.FactoryCity,
                        FactoryCountry = item.FactoryCountryName,
                        ProductSubCategory = item.ProductSubCategory,
                        ProductType = item.ProductType,
                        IsSameDayReport = item.IsSameDayReport,
                        ReportRequest = item.ReportRequest,
                        Instructions = item.Instructions,
                        InspectionStatus = actualStatusList,
                        EaqfRef = item.CustomerBookingNo
                    };

                    eaqfBookingList.Add(eaqfBooking);
                    response.EaqfBookingData = eaqfBookingList;
                }

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

        public async Task<object> GetEaqfInspectionReportBooking(string bookingIds)
        {
            var response = new GetEaqfInspectionBookingReportResponse();
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

                var inspectionData = await _repo.GetEaqfBookingReportDetails(bookingList);

                if (inspectionData == null || !inspectionData.Any())
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = BadRequest,
                        statusCode = HttpStatusCode.BadRequest,
                        data = inspectionData
                    };
                }
                response.TotalCount = inspectionData.Count();
                response.EaqfBookingReportData = inspectionData;
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
        /// Get the po product list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BookingPOProductListResponse> GetPoProductListBooking(BookingPOProductDataSourceRequest request)
        {

            try
            {
                var response = new BookingPOProductListResponse();

                if (request.filterPoProduct == (int)FilterPoProductEnum.ProductByPo)
                {
                    //Get the purchase order details Iqueryable
                    var data = _poRepo.GetPurchaseOrderDetails();

                    //apply the po number search text filter
                    if (!string.IsNullOrWhiteSpace(request.SearchText))
                    {
                        data = data.Where(x => EF.Functions.Like(x.Product.ProductId, $"%{request.SearchText.Trim()}%"));
                    }

                    //apply selected po id filter
                    if (request.PoId > 0)
                    {
                        data = data.Where(x => x.PoId == request.PoId);
                    }

                    //add customer id
                    if (request.CustomerIds != null && request.CustomerIds.Any())
                    {
                        data = data.Where(x => request.CustomerIds.Contains(x.Po.CustomerId.GetValueOrDefault()));
                    }

                    if (request.SupplierId > 0)
                    {
                        data = data.Where(x => x.Po.CuPoSuppliers.Any(y => y.Active.Value && y.SupplierId == request.SupplierId));
                    }

                    //execute the product list
                    var productList = await data.Skip(request.Skip).Take(request.Take).
                                            Select(x => new BookingPOProductData()
                                            {
                                                Id = x.Product.Id,
                                                Name = x.Product.ProductId,
                                                Description = x.Product.ProductDescription,
                                                PoQuantity = x.Quantity,
                                                ProductCategoryId = x.Product.ProductCategory,
                                                ProductCategoryName = x.Product.ProductCategoryNavigation.Name,
                                                ProductSubCategoryId = x.Product.ProductSubCategory,
                                                ProductSubCategoryName = x.Product.ProductSubCategoryNavigation.Name,
                                                ProductSubCategory2Id = x.Product.ProductCategorySub2,
                                                ProductSubCategory2Name = x.Product.ProductCategorySub2Navigation.Name,
                                                ProductSubCategory3Id = x.Product.ProductCategorySub3,
                                                ProductSubCategory3Name = x.Product.ProductCategorySub3Navigation.Name,
                                                BarCode = x.Product.Barcode,
                                                FactoryReference = x.Product.FactoryReference,
                                                IsNewProduct = x.Product.IsNewProduct,
                                                Remarks = x.Product.Remarks,
                                                ProductImageCount = x.Product.CuProductFileAttachments.Where(x => x.Active).Select(x => x.Id).Count()

                                            }).AsNoTracking().ToListAsync();

                    if (productList == null || !productList.Any())
                        response.Result = BookingPOProductResult.NotFound;

                    else
                    {
                        response.ProductList = productList;
                        response.Result = BookingPOProductResult.Success;
                    }
                }
                else if (request.filterPoProduct == (int)FilterPoProductEnum.ProductByCustomer)
                {
                    response = await GetCustomerProductDetailsDataSourceList(request);
                }

                return response;

            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<BookingPOProductListResponse> GetCustomerProductDetailsDataSourceList(BookingPOProductDataSourceRequest request)
        {
            try
            {
                var response = new BookingPOProductListResponse();

                //Get the purchase order details Iqueryable
                var data = _productRepo.GetCustomerProductDataSource();

                //apply the po number search text filter
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => EF.Functions.Like(x.ProductId, $"%{request.SearchText.Trim()}%"));
                }

                //apply customer filter
                if (request.CustomerIds != null && request.CustomerIds.Any())
                    data = data.Where(y => request.CustomerIds.Contains(y.CustomerId));

                //execute the product list
                var productList = await data.Skip(request.Skip).Take(request.Take).
                                        Select(x => new BookingPOProductData()
                                        {
                                            Id = x.Id,
                                            Name = x.ProductId,
                                            Description = x.ProductDescription,
                                            ProductCategoryId = x.ProductCategory,
                                            ProductCategoryName = x.ProductCategoryNavigation.Name,
                                            ProductSubCategoryId = x.ProductSubCategory,
                                            ProductSubCategoryName = x.ProductSubCategoryNavigation.Name,
                                            ProductSubCategory2Id = x.ProductCategorySub2,
                                            ProductSubCategory2Name = x.ProductCategorySub2Navigation.Name,
                                            ProductSubCategory3Id = x.ProductCategorySub3,
                                            ProductSubCategory3Name = x.ProductCategorySub3Navigation.Name,
                                            BarCode = x.Barcode,
                                            FactoryReference = x.FactoryReference,
                                            IsNewProduct = x.IsNewProduct,
                                            Remarks = x.Remarks,
                                            ProductImageCount = x.CuProductFileAttachments.Where(x => x.Active && x.FileTypeId.HasValue && x.FileTypeId.Value == (int)ProductRefFileType.ProductRefPictures).Select(x => x.Id).Count()

                                        }).AsNoTracking().ToListAsync();

                if (productList == null || !productList.Any())
                    response.Result = BookingPOProductResult.NotFound;

                else
                {
                    response.ProductList = productList;
                    response.Result = BookingPOProductResult.Success;
                }
                return response;

            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}