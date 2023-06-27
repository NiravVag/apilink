using AutoMapper;
using BI.Maps;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Audit;
using DTO.BookingRuleContact;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.DataAccess;
using DTO.Eaqf;
using DTO.EventBookingLog;
using DTO.ExtraFees;
using DTO.File;
using DTO.Inspection;
using DTO.Invoice;
using DTO.MasterConfig;
using DTO.OfficeLocation;
using DTO.Quotation;
using DTO.References;
using DTO.RepoRequest.Audit;
using DTO.RepoRequest.Enum;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class AuditManager : ApiCommonData, IAuditManager
    {
        #region Declaration 
        private IAuditRepository _auditRepository = null;
        private ILogger _logger = null;
        private ICustomerManager _customerManager = null;
        private ILocationManager _locationmanager = null;
        private ICustomerContactManager _customerContactManager = null;
        private ICustomerContactRepository _customerContactRepo = null;
        private ICustomerRepository _customerRepo = null;
        private IReferenceManager _referencemanager = null;
        private ISupplierManager _suppliermanager = null;
        private IFileManager _fileManager = null;
        private readonly IEventBookingLogRepository _eventLogRepo = null;
        private bool isEdit;
        private IHumanResourceManager _humanresourcemanager = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly IMapper _mapper = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IQuotationManager _quotationManager = null;
        private readonly IConfiguration _configuration = null;
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly AuditMap _auditmap = null;
        private ITenantProvider _filterService = null;
        private readonly IUserConfigRepository _userConfigRepo = null;
        private readonly ICustomerCheckPointRepository _customerCheckPointRepository = null;
        private readonly ISupplierRepository _supplierRepo = null;
        private readonly IUserRepository _userRepo = null;
        private readonly IReferenceRepository _referenceRepo = null;
        private readonly BookingMap _bookingmap = null;
        private readonly IUserRightsManager _userManager = null;
        private readonly ICancelBookingRepository _cancelBookingRepository = null;
        private readonly IQuotationRepository _quotationRepository = null;
        private readonly IUserAccountRepository _userAccountRepository = null;
        private readonly IEventBookingLogManager _eventBookingLog = null;
        private IDictionary<AuditBookingStatus, Func<int, AuditDetails, Task<SetInspNotifyResponse>>> _dictStatuses = null;

        #endregion Declaration

        #region Constructor
        public AuditManager(IAuditRepository auditRepository, ILogger<AuditManager> logger,
            ICustomerManager customermanager, ILocationManager locationmanager,
            IReferenceManager referencemanager, ISupplierManager suppliermanager,
            ISupplierRepository supplierRepo
            , IFileManager fileManager, IHumanResourceManager humanresource,
            ICustomerContactManager customercontact, IOfficeLocationManager office,
            IUserRepository userRepo,
            IReferenceRepository referenceRepo,
            IUserAccountRepository userAccountRepository,
            IEventBookingLogRepository eventLogRepo,
            IUserRightsManager userManager,
            ICancelBookingRepository cancelBookingRepository,
            ICustomerRepository customerRepo,
            ICustomerContactRepository customerContactRepo,
            IEventBookingLogManager eventBookingLog,
            ICustomerCheckPointRepository customerCheckPointRepository,
            IQuotationRepository quotationRepository,
            IMapper mapper, IAPIUserContext applicationContext, IQuotationManager quotationManager, IConfiguration configuration,
            IInvoiceRepository invoiceRepository, ITenantProvider filterService, IUserConfigRepository userConfigRepo)
        {
            _auditRepository = auditRepository;
            _logger = logger;
            _customerManager = customermanager;
            _locationmanager = locationmanager;
            _referencemanager = referencemanager;
            _suppliermanager = suppliermanager;
            _fileManager = fileManager;
            _humanresourcemanager = humanresource;
            _customerContactManager = customercontact;
            _office = office;
            _mapper = mapper;
            _userManager = userManager;
            _eventBookingLog = eventBookingLog;
            _ApplicationContext = applicationContext;
            _quotationManager = quotationManager;
            _configuration = configuration;
            _invoiceRepository = invoiceRepository;
            _auditmap = new AuditMap();
            _filterService = filterService;
            _userConfigRepo = userConfigRepo;
            _userAccountRepository = userAccountRepository;
            _customerCheckPointRepository = customerCheckPointRepository;
            _supplierRepo = supplierRepo;
            _userRepo = userRepo;
            _referenceRepo = referenceRepo;
            _quotationRepository = quotationRepository;
            _customerContactRepo = customerContactRepo;
            _bookingmap = new BookingMap();
            _eventLogRepo = eventLogRepo;
            _customerRepo = customerRepo;
            _cancelBookingRepository = cancelBookingRepository;
            _dictStatuses = new Dictionary<AuditBookingStatus, Func<int, AuditDetails, Task<SetInspNotifyResponse>>>() {
                                    { AuditBookingStatus.Received, ToRequestBooking},
                                    { AuditBookingStatus.Confirmed, ToConfirmBooking },
                                    { AuditBookingStatus.Cancel, ToCancelBooking },
                                    { AuditBookingStatus.Rescheduled, ToRescheduleBooking }
                               };
        }
        #endregion Constructor

        #region Common Method

        public async Task<AuditEvaluationRoundResponse> GetEvaluationRound()
        {
            var response = new AuditEvaluationRoundResponse();
            var data = await _auditRepository.GetEvaluationRound();
            if (data == null || data.Count == 0)
                return new AuditEvaluationRoundResponse() { Result = AuditEvaluationRoundResponseResult.Error };
            response.EvaluationRoundList = data.Select(x => _auditmap.GetEvaluationRound(x));
            response.Result = AuditEvaluationRoundResponseResult.Success;
            return response;
        }

        private async Task<SetInspNotifyResponse> ToRequestBooking(int bookingId, AuditDetails request)
        {
            var response = new SetInspNotifyResponse();
            response.BookingId = bookingId;
            var auditBookingData = await _auditRepository.GetAuditInspectionCustomerContactByID(bookingId);

            if (auditBookingData != null && auditBookingData.Id > 0)
            {
                response.StatusId = auditBookingData.StatusId;
                response.IsEdit = isEdit;

                //  var requestTask = await UpdateTask(auditBookingData.Id, new[] { (int)TaskType.ConfirmInspection }, false, true);

                if (request.AuditId > 0) // Edit Audit booking case
                {

                    response.StatusName = BookingStatusNames.Modified.ToString();
                    isEdit = true;
                    // send email to customer if the internal user modified and enabled the checkbox or customer created and modified the booking 
                    if ((_ApplicationContext.UserType == UserTypeEnum.InternalUser && request.IsCustomerEmailSend) ||
                        (auditBookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer &&
                        _ApplicationContext.UserType == UserTypeEnum.Customer))
                    {
                        if (auditBookingData.AudTranCuContacts.Any(x => x.Active && x.Contact.Active.Value))
                            response.CustomerEmail = auditBookingData.AudTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                                Aggregate((x, y) => x + ";" + y);
                    }

                }
                else // new Audit booking case
                {
                    response.StatusName = BookingStatusNames.Requested.ToString();
                    isEdit = false;
                    if (auditBookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer || request.IsCustomerEmailSend)
                    {
                        if (auditBookingData.AudTranCuContacts.Any(x => x.Active && x.Contact.Active.Value))
                            response.CustomerEmail = auditBookingData.AudTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                            Aggregate((x, y) => x + ";" + y);
                    }
                }

                // get department details
                var departmentData = new List<CommonDataSource> { new CommonDataSource()
                        { Id = auditBookingData.DepartmentId.GetValueOrDefault(),Name=auditBookingData.DepartmentId.GetValueOrDefault().ToString() } };
                //Get Brand details
                var brandData = new List<CommonDataSource> { new CommonDataSource()
                        { Id = auditBookingData.BrandId.GetValueOrDefault(),Name=auditBookingData.BrandId.GetValueOrDefault().ToString() } };


                //factory country 
                int? factoryCountryId = null;
                var factoryCountryData = await _suppliermanager.GetSupplierHeadOfficeAddress(auditBookingData.FactoryId);
                if (factoryCountryData.Result == SupplierListResult.Success)
                    factoryCountryId = factoryCountryData.countryId;

                var userAccessFilter = new UserAccess
                {
                    OfficeId = auditBookingData.OfficeId != null ? auditBookingData.OfficeId.Value : 0,
                    ServiceId = (int)Entities.Enums.Service.AuditId,
                    CustomerId = auditBookingData.CustomerId,
                    RoleId = (int)RoleEnum.InspectionRequest,
                    ProductCategoryIds = Enumerable.Empty<int?>(),
                    DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    FactoryCountryId = factoryCountryId
                };

                var userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                var toRecipients = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                // Add Task 
                //await AddTask(TaskType.ConfirmInspection, bookingId, (int)RoleEnum.InspectionVerified, toRecipients.Select(x => x.Id),
                //    request.UserId > 0 ? request.UserId : _ApplicationContext.UserId);

                //if (userListByRoleAccess != null && userListByRoleAccess.Count() > 0)
                //{
                //    if (isEdit)
                //    {
                //        await AddNotification(NotificationType.InspectionModified, bookingId, userListByRoleAccess.Select(x => x.Id));
                //    }

                //    else
                //    {
                //        await AddNotification(NotificationType.InspectionRequested, bookingId, userListByRoleAccess.Select(x => x.Id));
                //    }
                //}

                response.UserList = userListByRoleAccess;
                response.ToRecipients = toRecipients;
            }
            return response;
        }

        private async Task<SetInspNotifyResponse> ToConfirmBooking(int bookingId, AuditDetails request)
        {

            var response = new SetInspNotifyResponse();
            bool brandCheckpoint = false;
            bool deptCheckpoint = false;
            bool serviceTypeCheckpoint = false;
            response.BookingId = bookingId;
            var bookingData = await _auditRepository.GetAuditInspectionCustomerContactByID(bookingId);
            response.StatusId = bookingData.StatusId;
            response.StatusName = BookingStatusNames.Confirmed.ToString();

            var quotaCount = await _auditRepository.GetAuditServiceCustomersByCustomerId(bookingData.CustomerId);
            bool checkpointExists = quotaCount != null && quotaCount.Id > 0;

            //if no brand or dept or service type is selected, then checkpoint is applied to all the brands, depts and service types
            if (checkpointExists)
            {
                brandCheckpoint = true;
                deptCheckpoint = true;
                serviceTypeCheckpoint = true;

                if (quotaCount.CuCheckPointsBrands != null && quotaCount.CuCheckPointsBrands.Any())
                {
                    brandCheckpoint = quotaCount.CuCheckPointsBrands.Any(y => y.BrandId == request.BrandId);
                }

                if (quotaCount.CuCheckPointsDepartments != null && quotaCount.CuCheckPointsDepartments.Any())
                {
                    deptCheckpoint = quotaCount.CuCheckPointsDepartments.Any(y => y.DeptId == request.DepartmentId);
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

            // Mark the Verified task Done
            // var confirmTask = await UpdateTask(bookingData.Id, new[] { (int)TaskType.ConfirmInspection }, false, true);

            // get department details
            var departmentData = new List<CommonDataSource>();
            var brandData = new List<CommonDataSource>();
            if (bookingData.DepartmentId.GetValueOrDefault() > 0)

                departmentData = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingData.DepartmentId.GetValueOrDefault(),Name=bookingData.DepartmentId.GetValueOrDefault().ToString() } };
            //Get Brand details
            if (bookingData.BrandId.GetValueOrDefault() > 0)
                brandData = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingData.BrandId.GetValueOrDefault(),Name=bookingData.BrandId.GetValueOrDefault().ToString() } };

            //factory country 
            int? factoryCountryId = null;
            var factoryCountryData = await _suppliermanager.GetSupplierHeadOfficeAddress(bookingData.FactoryId);
            if (factoryCountryData.Result == SupplierListResult.Success)
                factoryCountryId = factoryCountryData.countryId;

            var userAccessFilter = new UserAccess
            {
                OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                ServiceId = (int)Entities.Enums.Service.AuditId,
                CustomerId = bookingData.CustomerId,
                RoleId = (int)RoleEnum.InspectionScheduled,
                ProductCategoryIds = Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };
            response.ToRecipients = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            //logic is for non quotation customers.
            //if ((confirmTask != null && confirmTask.Any()) && !checkpointExists)
            //{
            //    //Add Task to the scheduled team if no customer checkpoint for quotation
            //    var scheduleTask = await _repo.GetTask(bookingData.Id, new[] { (int)TaskType.ScheduleInspection }, false);

            //    if (scheduleTask == null || scheduleTask.Count() == 0)
            //    {
            //        //Add Task
            //        await AddTask(TaskType.ScheduleInspection, bookingId, (int)RoleEnum.InspectionScheduled, response.ToRecipients.Select(x => x.Id));
            //    }
            //}

            if (request.AuditId > 0)
            {
                userAccessFilter.RoleId = (int)RoleEnum.InspectionConfirmed;
                var userListbyRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
                userListbyRoleAccess = userListbyRoleAccess.Concat(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));

                //// send notification
                //if (userListbyRoleAccess != null && userListbyRoleAccess.Count() > 0)
                //{
                //    await AddNotification(NotificationType.InspectionConfirmed, bookingId, userListbyRoleAccess.Select(x => x.Id));
                //}

                //Check if Quotation is created
                //if (checkpointExists)
                //{
                //    var bookingdates = await _repo.GetLastServiceDate(bookingId);
                //    if (bookingdates.ServiceDateFrom.Date.CompareTo(bookingData.ServiceDateFrom.Date) != 0 || bookingdates.ServiceDateTo.Date.CompareTo(bookingData.ServiceDateTo.Date) != 0)
                //    {
                //        var quotationUsers = await BookingQuotationExists(bookingId, bookingData.OfficeId, bookingData.CustomerId, NotificationType.InspectionRescheduled);

                //        if (quotationUsers.Any())
                //        {
                //            userListbyRoleAccess = userListbyRoleAccess.Concat(quotationUsers);
                //            response.quotationExists = true;
                //        }
                //    }

                //    if (response.quotationExists)
                //    {
                //        //Add Task to the scheduled team if no customer checkpoint for quotation
                //        var scheduleTask = await _repo.GetTask(bookingData.Id, new[] { (int)TaskType.ScheduleInspection }, false);

                //        if (scheduleTask == null || scheduleTask.Count() == 0)
                //        {
                //            //Add Task
                //            await AddTask(TaskType.ScheduleInspection, bookingId, (int)RoleEnum.InspectionScheduled, response.ToRecipients.Select(x => x.Id));
                //        }
                //    }
                //}

                if (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer || request.IsCustomerEmailSend)
                {
                    response.CustomerEmail = bookingData.AudTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
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
                response.ToRecipients = verifyUserRole;

                //if (isCombineOrderDataChanged)
                //{
                //    var quotationUsers = await BookingQuotationExists(bookingId, bookingData.OfficeId, bookingData.CustomerId, NotificationType.BookingQuantityChange);
                //    await AddNotification(NotificationType.BookingQuantityChange, bookingId, verifyUserRole.Select(x => x.Id));
                //    if (quotationUsers.Any())
                //    {
                //        verifyUserRole = verifyUserRole.Concat(quotationUsers);
                //    }
                //    response.ToRecipients = verifyUserRole;
                //}
                //else
                //{
                //    response.ToRecipients = verifyUserRole;
                //    await AddNotification(NotificationType.InspectionModified, bookingId, verifyUserRole.Select(x => x.Id));
                //}
                if (request.IsCustomerEmailSend)
                {
                    response.CustomerEmail = bookingData.AudTranCuContacts.Where(x => x.Active).Select(x => x.Contact.Email).Distinct().ToList().
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

        private async Task<SetInspNotifyResponse> ToRescheduleBooking(int bookingId, AuditDetails request)
        {
            var response = new SetInspNotifyResponse();
            response.BookingId = bookingId;
            var bookingData = await _auditRepository.GetAuditInspectionCustomerContactByID(bookingId);

            response.StatusId = bookingData.StatusId;
            // IEnumerable<User> userListByRoleAccess = new List<User>();
            response.StatusName = BookingStatusNames.Rescheduled.ToString();

            //int lastStatus = await _repo.GetLastStatus(bookingId);

            ////Mark the task done
            //await UpdateTask(bookingId, new[] { (int)TaskType.VerifyInspection, (int)TaskType.ConfirmInspection }, false, true);

            ////get product category details
            //var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
            // get department details
            var departmentData = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingData.DepartmentId.GetValueOrDefault(),Name=bookingData.DepartmentId.GetValueOrDefault().ToString() } };
            //Get Brand details
            var brandData = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingData.BrandId.GetValueOrDefault(),Name=bookingData.BrandId.GetValueOrDefault().ToString() } };

            //factory country 
            int? factoryCountryId = null;
            var factoryCountryData = await _suppliermanager.GetSupplierHeadOfficeAddress(bookingData.FactoryId);
            if (factoryCountryData.Result == SupplierListResult.Success)
                factoryCountryId = factoryCountryData.countryId;

            var userAccessFilter = new UserAccess
            {
                OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                ServiceId = (int)Entities.Enums.Service.AuditId,
                CustomerId = bookingData.CustomerId,
                RoleId = (int)RoleEnum.InspectionVerified,
                ProductCategoryIds = Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };
            // _repo.Save(task, false);

            var userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            userAccessFilter.RoleId = (int)RoleEnum.InspectionConfirmed;

            response.ToRecipients = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            //Add Task
            // await AddTask(TaskType.ConfirmInspection, bookingId, (int)RoleEnum.InspectionConfirmed, response?.ToRecipients?.Select(x => x.Id));

            //Check if Quotation is created
            var quotationDetails = await _cancelBookingRepository.BookingQuotationExists(bookingId);
            if (quotationDetails != null)
            {
                int[] statusIdList = new int[] { (int)QuotationStatus.CustomerValidated, (int)QuotationStatus.CustomerRejected, (int)QuotationStatus.SentToClient };
                userAccessFilter = new UserAccess
                {
                    OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                    ServiceId = (int)Entities.Enums.Service.AuditId,
                    CustomerId = bookingData.CustomerId,
                    RoleId = (int)RoleEnum.QuotationRequest,
                    ProductCategoryIds = Enumerable.Empty<int?>(),
                    DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                    BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>()
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

            //if (userListByRoleAccess != null && userListByRoleAccess.Count() > 0)
            //{
            //    await AddNotification(NotificationType.InspectionRescheduled, bookingId, userListByRoleAccess.Select(x => x.Id));
            //}

            if (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer)
            {
                response.CustomerEmail = bookingData.AudTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                    Aggregate((x, y) => x + ";" + y);
            }

            response.UserList = userListByRoleAccess;
            return response;
        }

        private async Task<SetInspNotifyResponse> ToCancelBooking(int bookingId, AuditDetails request)
        {
            var response = new SetInspNotifyResponse();
            response.BookingId = bookingId;
            var bookingData = await _auditRepository.GetAuditInspectionCustomerContactByID(bookingId);

            response.StatusId = bookingData.StatusId;
            response.StatusName = BookingStatusNames.Cancelled.ToString();

            //int lastStatus = await _repo.GetLastStatus(bookingId);

            //if (lastStatus > 0)
            //{
            //    //Mark the task as done
            //    await UpdateTask(bookingId, new[] { (int)TaskType.VerifyInspection, (int)TaskType.ConfirmInspection, (int)TaskType.ScheduleInspection, (int)TaskType.QuotationPending }, false, true);

            //    //MidTask requestTask = await _repo.GetLastTask(bookingId);
            //}

            // get department details
            var departmentData = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingData.DepartmentId.GetValueOrDefault(),Name=bookingData.DepartmentId.GetValueOrDefault().ToString() } };
            //Get Brand details
            var brandData = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingData.BrandId.GetValueOrDefault(),Name=bookingData.BrandId.GetValueOrDefault().ToString() } };

            //factory country 
            int? factoryCountryId = null;
            var factoryCountryData = await _suppliermanager.GetSupplierHeadOfficeAddress(bookingData.FactoryId);
            if (factoryCountryData.Result == SupplierListResult.Success)
                factoryCountryId = factoryCountryData.countryId;

            response.OfficeId = bookingData.OfficeId;

            var userAccessFilter = new UserAccess
            {
                OfficeId = bookingData.OfficeId != null ? bookingData.OfficeId.Value : 0,
                ServiceId = (int)Entities.Enums.Service.AuditId,
                CustomerId = bookingData.CustomerId,
                RoleId = (int)RoleEnum.InspectionConfirmed,
                ProductCategoryIds = Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };

            var userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            //if (lastStatus != ((int)BookingStatus.Received) && lastStatus != ((int)BookingStatus.Verified))
            //{
            //    userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
            //    userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
            //    userListByRoleAccess = userListByRoleAccess.Concat(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
            //}
            //else if (lastStatus == ((int)BookingStatus.Received))
            //{
            //    userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
            //    userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
            //}
            //else if (lastStatus == ((int)BookingStatus.Verified))
            //{
            //    userListByRoleAccess = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
            //    userAccessFilter.RoleId = (int)RoleEnum.InspectionVerified;
            //    userListByRoleAccess = userListByRoleAccess.Concat(await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter));
            //}

            //if (userListByRoleAccess != null && userListByRoleAccess.Count() > 0)
            //{
            //    await AddNotification(NotificationType.InspectionCancelled, bookingId, userListByRoleAccess.Select(x => x.Id));
            //}

            if (bookingData.CreatedByNavigation.UserTypeId == (int)UserTypeEnum.Customer)
            {
                response.CustomerEmail = bookingData.AudTranCuContacts.Where(x => x.Active && x.Contact.Active.Value).Select(x => x.Contact.Email).Distinct().ToList().
                    Aggregate((x, y) => x + ";" + y);
            }
            response.ToRecipients = userListByRoleAccess;
            return response;
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

                    _auditRepository.AddEntity(task);
                }
                //Save
                await _auditRepository.Save();
            }
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

                    _auditRepository.AddEntity(notification);
                }

                //Save
                await _auditRepository.Save();

            }
        }

        //update task(mid_task table) 
        public async Task<IEnumerable<MidTask>> UpdateTask(int bookingId, IEnumerable<int> typeIdList, bool oldTaskDoneValue, bool newTaskDoneValue)
        {
            IEnumerable<MidTask> getTasks = await _auditRepository.GetTask(bookingId, typeIdList, oldTaskDoneValue);
            foreach (var task in getTasks)
            {
                if (task != null)
                {
                    task.IsDone = newTaskDoneValue;
                    task.UpdatedBy = _ApplicationContext?.UserId;
                    task.UpdatedOn = DateTime.Now;
                }
            }
            _auditRepository.SaveList(getTasks);
            return getTasks;
        }

        #endregion

        public async Task<EditAuditResponse> EditAudit(int? id)
        {
            var response = new EditAuditResponse() { AuditDetails = new AuditDetails() };
            try
            {
                if (id != null)
                {
                    var entity = await _auditRepository.GetAuditDetailsById(id.Value);
                    if (entity == null)
                        return new EditAuditResponse() { Result = EditAuditResult.CannotGetAuditDetails };

                    response.AuditDetails = _auditmap.MapAuditDetails(entity, (x) => _fileManager.GetMimeType(x));

                    if (response.AuditDetails == null)
                        return new EditAuditResponse() { Result = EditAuditResult.CannotGetAuditDetails };
                }
                else
                {
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
                }
                response.UserType = _ApplicationContext.UserType;
                response.Result = EditAuditResult.Success;
            }
            catch (Exception ex)
            {
                return new EditAuditResponse() { Result = EditAuditResult.CannotGetAuditDetails };
            }
            return response;
        }

        public async Task<EditAuidtCusDetails> GetAuditDetailsByCustomerId(int customerid)
        {
            EditAuidtCusDetails response = new EditAuidtCusDetails();
            try
            {

                //customer contact list
                response.CustomerContactList = await _customerContactManager.GetCustomerContacts(customerid);
                if (response.CustomerContactList == null || response.CustomerContactList.Count() == 0)
                    return new EditAuidtCusDetails() { Result = EditAuditResult.CannotGetCustomerContactList };

                // customer brand list
                response.CustomerBrandList = await _customerManager.GetCustomerBrandsByUserId(customerid, _ApplicationContext.UserId);

                //customer department list
                response.CustomerDepartmentList = await _customerManager.GetCustomerDepartmentByUserId(customerid, _ApplicationContext.UserId);

                //customer season list
                response.SeasonList = await _customerManager.GetCustomerSeason(customerid);
                if (response.SeasonList == null || response.SeasonList.Count() == 0)
                    response.SeasonList = await _referencemanager.GetSeasons();

                //supplier list
                var supplierlistresponse = await _suppliermanager.GetSuppliersByUserType(customerid, true);
                if (supplierlistresponse.Result == DTO.Supplier.SupplierListResult.Success && supplierlistresponse.Data.Count() > 0)
                    response.SupplierList = supplierlistresponse.Data;
                else
                    return new EditAuidtCusDetails() { Result = EditAuditResult.CannotGetSupplierList };

                ////service type
                //var serviceTypeRequest = new ServiceTypeRequest()
                //{
                //    CustomerId = customerid,
                //    ServiceId = (int)Entities.Enums.Service.AuditId
                //};
                //var serviceTypeList = await _referencemanager.GetCustomerServiceTypes(serviceTypeRequest);

                //if (serviceTypeList == null || serviceTypeList.Result == ServiceTypeResult.NotFound ||
                //    serviceTypeList.ServiceTypeList == null || !serviceTypeList.ServiceTypeList.Any())
                //    return new EditAuidtCusDetails() { Result = EditAuditResult.CannotGetServiceTypeList };

                //response.CustomerServiceList = serviceTypeList.ServiceTypeList;

                if (UserTypeEnum.Supplier == _ApplicationContext.UserType)
                {
                    var supresponse = await GetSupplierDetailsByCustomerIdSupplierId(customerid, _ApplicationContext.SupplierId);
                    if (supresponse?.Result == EditAuditResult.GetSupplierDetailsBySupplierCUstomerIdSuccess)
                    {
                        response.SupplierCode = supresponse?.SupplierCode;
                        response.SupplierContactList = supresponse?.SupplierContactList;

                    }
                }

                if (UserTypeEnum.Factory == _ApplicationContext.UserType)
                {
                    var factresponse = await GetFactoryDetailsByCustomerIdFactoryId(customerid, _ApplicationContext.FactoryId);
                    if (factresponse?.Result == EditAuditResult.GetFactoryDetailsByIdSuccess)
                    {
                        response.FactoryCode = factresponse?.FactoryCode;
                        response.FactoryContactList = factresponse?.FactoryContactList;
                        response.OfficeId = factresponse.OfficeId;
                    }
                }

                response.Result = EditAuditResult.GetAuditDetailsByCustomerIdSuccess;
            }
            catch (Exception ex)
            {
                return new EditAuidtCusDetails() { Result = EditAuditResult.CanotGetCustomerDetails };
            }
            return response;
        }

        public async Task<EditAuditSupDetails> GetSupplierDetailsByCustomerIdSupplierId(int? cusid, int supid)
        {
            EditAuditSupDetails response = new EditAuditSupDetails();

            try
            {
                var supplierdetails = await _suppliermanager.GetEditSupplier(supid);

                //supplier details
                if (supplierdetails == null)
                    return new EditAuditSupDetails() { Result = EditAuditResult.CannotGetSupplierDetails };

                //factory list
                var factlist = await _suppliermanager.GetFactorysByUserType(cusid, supid);
                if (factlist.Result == SupplierListResult.Success && factlist.Data.Count() > 0)
                    response.FactoryList = factlist.Data;
                else
                    return new EditAuditSupDetails() { Result = EditAuditResult.CannotGetFactoryDetails };

                if (cusid != null)
                {
                    //spplier code
                    var supcode = supplierdetails?.SupplierDetails?.CustomerList?.Where(x => x.Id == cusid).Select(x => x.Code).FirstOrDefault();
                    response.SupplierCode = supcode == null ? "" : supcode;

                    //supplier contact list
                    response.SupplierContactList = await _suppliermanager.GetSupplierContactsById(supid, cusid.Value);
                    if (response.SupplierContactList == null || response.SupplierContactList.Count() == 0)
                        return new EditAuditSupDetails() { Result = EditAuditResult.CannotGetSupplierContactList };
                }

                response.Result = EditAuditResult.GetSupplierDetailsBySupplierCUstomerIdSuccess;
            }
            catch (Exception ex)
            {
                return new EditAuditSupDetails() { Result = EditAuditResult.CannotGetSupplierDetails };
            }
            return response;
        }

        public async Task<IEnumerable<EntMasterConfig>> GetMasterConfiguration()
        {
            return await _userConfigRepo.GetMasterConfiguration();
        }

        public async Task<EditAuditFactDetails> GetFactoryDetailsByCustomerIdFactoryId(int? cusid, int factid)
        {
            EditAuditFactDetails response = new EditAuditFactDetails();
            try
            {
                var factdetails = await _suppliermanager.GetEditSupplier(factid);
                var office = await _office.GetOfficeByFactoryid(factid);

                //factory details
                if (factdetails == null)
                    return new EditAuditFactDetails() { Result = EditAuditResult.CannotGetFactoryDetails };
                else
                {
                    var factaddress = factdetails?.SupplierDetails?.AddressList.FirstOrDefault();
                    response.FactoryAddress = factaddress?.Way;
                    response.FactoryRegionalAddress = factaddress?.LocalLanguage;
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
                        return new EditAuditFactDetails() { Result = EditAuditResult.CannotGetSupplierList };
                }
                if (cusid != null)
                {
                    //factory code
                    response.FactoryCode = factdetails?.SupplierDetails.CustomerList?.Where(x => x.Id == cusid).Select(x => x.Code).FirstOrDefault();

                    //factory contact list
                    response.FactoryContactList = await _suppliermanager.GetFactoryContactsById(factid, cusid.Value);
                    if (response.FactoryContactList == null || response.FactoryContactList.Count() == 0)
                        return new EditAuditFactDetails() { Result = EditAuditResult.CannotGetFactoryContactList };
                }
                response.Result = EditAuditResult.GetFactoryDetailsByIdSuccess;
            }
            catch (Exception ex)
            {
                return new EditAuditFactDetails() { Result = EditAuditResult.CannotGetFactoryDetails };
            }
            return response;
        }

        public async Task<SaveAuditResponse> SaveAudit(AuditDetails request)
        {
            var requestlog = JsonConvert.SerializeObject(request);
            _logger.LogInformation(requestlog);//log auditdetails.
            if (request.AuditId == 0)
                return await AddAudit(request);

            return await UpdateAudit(request);
        }

        public async Task<SaveAuditResponse> UpdateAudit(AuditDetails request)
        {
            var response = new SaveAuditResponse();
            try
            {
                var entity = await _auditRepository.GetAuditDetailsById(request.AuditId);
                if (entity == null)
                    return new SaveAuditResponse() { Result = SaveAuditResult.AuditNotFound };

                var isMissionUpdated = entity.ServiceDateFrom != request.ServiceDateFrom.ToDateTime() ||
                    entity.ServiceDateTo != request.ServiceDateTo.ToDateTime() ||
                    entity.CustomerId != request.CustomerId || entity.FactoryId != request.FactoryId ||
                    entity.SupplierId != request.SupplierId ||
                    entity.OfficeId != request.OfficeId ||
                    entity.AuditTypeId != request.AuditTypeid ||
                    entity.EvalutionId != request.EvaluationRoundId ||
                    entity.AudTranServiceTypes.FirstOrDefault()?.ServiceTypeId != request.ServiceTypeId ||
                    Enumerable.SequenceEqual(entity.AudTranSuContacts.Select(x => x.ContactId).OrderBy(e => e), request.SupplierContactListItems.OrderBy(e => e));

                //Audit Details
                entity.CustomerId = request.CustomerId;
                entity.StatusId = request.StatusId;
                entity.BrandId = request.BrandId;
                entity.DepartmentId = request.DepartmentId;
                entity.SeasonId = request.SeasonId;
                entity.SeasonYearId = request.SeasonYearId;
                entity.SupplierId = request.SupplierId;
                entity.FactoryId = request.FactoryId;
                entity.ServiceDateFrom = request.ServiceDateFrom.ToDateTime();
                entity.ServiceDateTo = request.ServiceDateTo.ToDateTime();
                entity.EvalutionId = request.EvaluationRoundId;
                entity.CusBookingComments = request.CustomerComments?.Trim();
                entity.ApiBookingComments = request.APIComments?.Trim();
                entity.InternalComments = request.InternalComments?.Trim();
                entity.OfficeId = request.OfficeId;
                entity.PoNumber = string.IsNullOrEmpty(request.PoNumber) ? "Aud_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") : request.PoNumber;
                entity.ApplicantEmail = request.ApplicantEmail?.Trim();
                entity.ApplicantName = request.ApplicantName?.Trim();
                entity.ApplicantPhNo = request.ApplicantPhNo?.Trim();
                entity.AuditTypeId = request.AuditTypeid;
                entity.CustomerBookingNo = request.CustomerBookingNo;
                entity.CuProductCategory = request.AuditProductCategoryId;
                //factory profile
                var faprofileentity = entity.AudTranFaProfiles?.FirstOrDefault();
                if (faprofileentity != null)
                {
                    faprofileentity.Accrediations = request.Accreditations?.Trim();
                    faprofileentity.AdministrativeStaff = request.AdminStaff;
                    faprofileentity.AnnualHolidays = request.AnnualHolidays?.Trim();
                    faprofileentity.AnnualProduction = request.AnnualProduction?.Trim();
                    faprofileentity.CompanyOpenTime = request.OpenHour?.Trim();
                    faprofileentity.CompanySurfaceArea = request.FactorySurfaceArea?.Trim();
                    faprofileentity.CreatedDate = request.FactoryCreationDate?.ToDateTime();
                    faprofileentity.IndustryTradeAssociation = request.TradeAssociation?.Trim();
                    faprofileentity.InvestmentBackground = request.Investment?.Trim();
                    faprofileentity.MaximumCapacity = request.MaximumCapacity?.Trim();
                    faprofileentity.NoOfCustomer = request.NoOfCustomers?.Trim();
                    faprofileentity.NoOfSuppliersComponent = request.NoOfSuppliers?.Trim();
                    faprofileentity.NumberOfSites = request.NumberOfSites?.Trim();
                    faprofileentity.PercentageCusTotalCapacity = request.TotalCapacityByCustomer?.Trim();
                    faprofileentity.PossibilityOfExtension = request.FactoryExtension?.Trim();
                    faprofileentity.ProductionStaff = request.ProductionStaff;
                    faprofileentity.PublicLiabilityInsurance = request.Liability;
                    faprofileentity.QualityStaff = request.QualityStaff;
                    faprofileentity.SalesStaff = request.SalesStaff;
                    faprofileentity.TotalStaff = request.TotalStaff;
                    faprofileentity.TypeOfProductManufactured = request.ManufactureProducts?.Trim();
                    faprofileentity.TypesOfBrands = request.BrandsProduced?.Trim();
                    _auditRepository.EditEntity(faprofileentity);
                }
                else
                {
                    AddFactoryProfile(entity, request);
                }

                //auditor and cs 
                if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                {
                    Update_Auditor_CS(request, entity);
                }

                //customer contact
                var cusconlist = request.CustomerContactListItems.Select(x => x).ToArray();
                var removecusconlist = entity.AudTranCuContacts.Where(x => !cusconlist.Contains(x.ContactId) && x.Active);
                var existcuslist = entity.AudTranCuContacts.Where(x => cusconlist.Contains(x.ContactId) && x.Active);
                var lstcuscon = new List<AudTranCuContact>();
                foreach (var _contact in removecusconlist)
                {
                    _contact.Active = false;
                    _contact.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    _contact.DeletedOn = DateTime.Now;
                    lstcuscon.Add(_contact);
                }
                if (lstcuscon.Count() > 0)
                    _auditRepository.EditEntities(lstcuscon);

                foreach (var id in cusconlist)
                {
                    if (!existcuslist.Any() || !existcuslist.Any(x => x.ContactId == id))
                    {
                        entity.AudTranCuContacts.Add(new AudTranCuContact()
                        {
                            ContactId = id,
                            Active = true,
                            CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
                //audit work process
                var audworklist = request.AuditworkprocessItems.Select(x => x).ToArray();
                var removeaudwplist = entity.AudTranWorkProcesses.Where(x => !audworklist.Contains(x.WorkProcessId) && x.Active);
                var existaudwplist = entity.AudTranWorkProcesses.Where(x => audworklist.Contains(x.WorkProcessId) && x.Active);
                var lstwp = new List<AudTranWorkProcess>();
                foreach (var wp in removeaudwplist)
                {
                    wp.Active = false;
                    wp.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    wp.DeletedOn = DateTime.Now;
                    lstwp.Add(wp);
                }
                if (lstwp.Count() > 0)
                    _auditRepository.EditEntities(lstwp);

                foreach (var id in audworklist)
                {
                    if (!existaudwplist.Any() || !existaudwplist.Any(x => x.WorkProcessId == id))
                    {
                        entity.AudTranWorkProcesses.Add(new AudTranWorkProcess()
                        {
                            WorkProcessId = id,
                            Active = true,
                            CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }

                //supplier list
                var supconlist = request.SupplierContactListItems.Select(x => x).ToArray();
                var removesupconlist = entity.AudTranSuContacts.Where(x => !supconlist.Contains(x.ContactId) && x.Active);
                var existsupconlist = entity.AudTranSuContacts.Where(x => supconlist.Contains(x.ContactId) && x.Active);
                var lstsupcon = new List<AudTranSuContact>();
                foreach (var _contact in removesupconlist)
                {
                    _contact.Active = false;
                    _contact.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    _contact.DeletedOn = DateTime.Now;
                    lstsupcon.Add(_contact);
                }
                if (lstsupcon.Count() > 0)
                    _auditRepository.EditEntities(lstsupcon);

                foreach (var id in supconlist)
                {
                    if (!existsupconlist.Any() || !existsupconlist.Any(x => x.ContactId == id))
                    {
                        entity.AudTranSuContacts.Add(new AudTranSuContact()
                        {
                            ContactId = id,
                            Active = true,
                            CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
                //factorylist
                var faconlist = request.FactoryContactListItems.Select(x => x).ToArray();
                var removefaconlist = entity.AudTranFaContacts.Where(x => !faconlist.Contains(x.ContactId) && x.Active);
                var existfaconlist = entity.AudTranFaContacts.Where(x => faconlist.Contains(x.ContactId) && x.Active);
                var lstfaccon = new List<AudTranFaContact>();
                foreach (var _contact in removefaconlist)
                {
                    _contact.Active = false;
                    _contact.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    _contact.DeletedOn = DateTime.Now;
                    lstfaccon.Add(_contact);
                }
                if (lstfaccon.Count() > 0)
                    _auditRepository.EditEntities(lstfaccon);

                foreach (var id in faconlist)
                {
                    if (!existfaconlist.Any() || !existfaconlist.Any(x => x.ContactId == id))
                    {
                        entity.AudTranFaContacts.Add(new AudTranFaContact()
                        {
                            ContactId = id,
                            Active = true,
                            CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
                // service type
                var servicelist = request.ServiceTypeId;
                var existservicelist = entity.AudTranServiceTypes.Where(x => x.ServiceTypeId != servicelist && x.Active);
                var lstservicelist = new List<AudTranServiceType>();
                foreach (var _service in existservicelist)
                {
                    _service.Active = false;
                    _service.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    _service.DeletedOn = DateTime.Now;
                    lstservicelist.Add(_service);
                }
                if (lstservicelist.Count() > 0)
                {
                    _auditRepository.EditEntities(existservicelist);

                    entity.AudTranServiceTypes.Add(new AudTranServiceType()
                    {
                        ServiceTypeId = servicelist,
                        Active = true,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    });
                }
                entity.AudTranStatusLogs.Add(new AudTranStatusLog()
                {
                    CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    StatusId = request.StatusId,
                    EntityId = _filterService.GetCompanyId()
                });

                //attched files
                request.Attachments = request?.Attachments ?? new HashSet<AuditAttachment>();

                var newfiles = request.Attachments.Where(x => x.Id == 0);

                //removed files
                var fiIds = request.Attachments.Where(x => x.Id > 0).Select(x => x.Id);
                var filesToremove = entity.AudTranFileAttachments.Where(x => !fiIds.Contains(x.Id));
                var lstremove = new List<AudTranFileAttachment>();
                foreach (var fileItem in filesToremove.ToList())
                {
                    fileItem.Active = false;
                    fileItem.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    fileItem.DeletedOn = DateTime.Now;
                    lstremove.Add(fileItem);
                    entity.AudTranFileAttachments.Add(fileItem);
                }


                //Updated
                var filesToUpdate = request.Attachments.Where(x => x.Id > 0);
                foreach (var fileItem in filesToUpdate)
                {
                    var fileEntity = entity.AudTranFileAttachments.FirstOrDefault(x => x.Id == fileItem.Id);

                    if (fileEntity != null)
                    {
                        fileEntity.FileName = fileItem.FileName;
                        fileEntity.UploadDate = DateTime.Now;
                        fileEntity.UserId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                        fileEntity.FileUrl = fileItem.FileUrl;
                        fileEntity.Active = true;
                    }
                }

                AddFiles(newfiles, entity);

                await _auditRepository.UpdateAudit(entity);
                response.Id = entity.Id;

                if (response.Id <= 0)
                    new SaveAuditResponse() { Result = SaveAuditResult.AuditNotUpdated };

                response.IsMissionUpdated = isMissionUpdated;
                response.Result = SaveAuditResult.Success;
            }
            catch (Exception)
            {
                return new SaveAuditResponse() { Result = SaveAuditResult.AuditNotSaved };
            }
            return response;
        }

        public async Task<SaveAuditResponse> AddAudit(AuditDetails request)
        {
            var response = new SaveAuditResponse();
            try
            {
                var entityId = _filterService.GetCompanyId();
                //audit details
                var entity = new AudTransaction()
                {
                    GuidId = Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    StatusId = request.StatusId,
                    BrandId = request.BrandId == 0 ? null : request.BrandId,
                    DepartmentId = request.DepartmentId == 0 ? null : request.DepartmentId,
                    SeasonId = request.SeasonId == 0 ? null : request.SeasonId,
                    SeasonYearId = request.SeasonYearId == 0 ? null : request.SeasonYearId,
                    SupplierId = request.SupplierId,
                    FactoryId = request.FactoryId,
                    ServiceDateFrom = request.ServiceDateFrom.ToDateTime(),
                    ServiceDateTo = request.ServiceDateTo.ToDateTime(),
                    EvalutionId = request.EvaluationRoundId == 0 ? null : request.EvaluationRoundId,
                    CusBookingComments = request.CustomerComments?.Trim(),
                    ApiBookingComments = request.APIComments?.Trim(),
                    InternalComments = request.InternalComments?.Trim(),
                    OfficeId = request.OfficeId,
                    PoNumber = string.IsNullOrEmpty(request.PoNumber) ? "Aud_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") : request.PoNumber,
                    CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    EntityId = entityId,
                    ApplicantEmail = request.ApplicantEmail?.Trim(),
                    ApplicantName = request.ApplicantName?.Trim(),
                    ApplicantPhNo = request.ApplicantPhNo?.Trim(),
                    AuditTypeId = request.AuditTypeid,
                    IsEaqf = request.IsEaqf,
                    CustomerBookingNo = request.CustomerBookingNo,
                    CuProductCategory = request.AuditProductCategoryId
                };
                //audit customer contact 
                if (request.CustomerContactListItems != null)
                {
                    foreach (var cuscontact in request.CustomerContactListItems)
                    {
                        var _cuscontact = new AudTranCuContact()
                        {
                            Active = true,
                            ContactId = cuscontact,
                            CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        };
                        entity.AudTranCuContacts.Add(_cuscontact);
                        _auditRepository.AddEntity(_cuscontact);
                    }

                }
                //audit supplier contact
                if (request.SupplierContactListItems != null)
                {
                    foreach (var supcontact in request.SupplierContactListItems)
                    {
                        var _supcontact = new AudTranSuContact()
                        {
                            Active = true,
                            ContactId = supcontact,
                            CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        };
                        entity.AudTranSuContacts.Add(_supcontact);
                        _auditRepository.AddEntity(_supcontact);
                    }

                }
                //audit supplier contact
                if (request.FactoryContactListItems != null)
                {
                    foreach (var facontact in request.FactoryContactListItems)
                    {
                        var _facontact = new AudTranFaContact()
                        {
                            Active = true,
                            ContactId = facontact,
                            CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        };
                        entity.AudTranFaContacts.Add(_facontact);
                        _auditRepository.AddEntity(_facontact);
                    }

                }
                //audit work process
                if (request.AuditworkprocessItems != null)
                {
                    foreach (var workprocess in request.AuditworkprocessItems)
                    {
                        var _audwp = new AudTranWorkProcess()
                        {
                            Active = true,
                            WorkProcessId = workprocess,
                            CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        };
                        entity.AudTranWorkProcesses.Add(_audwp);
                        _auditRepository.AddEntity(_audwp);
                    }

                }
                //audit service type
                var _servicecontact = new AudTranServiceType()
                {
                    Active = true,
                    ServiceTypeId = request.ServiceTypeId,
                    CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now
                };
                entity.AudTranServiceTypes.Add(_servicecontact);
                _auditRepository.AddEntity(_servicecontact);

                AddFactoryProfile(entity, request);

                entity.AudTranStatusLogs.Add(new AudTranStatusLog()
                {
                    CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    StatusId = request.StatusId,
                    EntityId = entityId
                });

                //auditor and cs 
                if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                {
                    request.AuditCS = request.AuditCS == null ? new List<int>() : request.AuditCS;
                    request.Auditors = request.Auditors == null ? new List<int>() : request.Auditors;
                    Update_Auditor_CS(request, entity);
                }

                AddFiles(request.Attachments, entity);

                var reportnoresponse = await GetAuditReportNo(request.CustomerId, request.FactoryId);
                if (reportnoresponse == null || reportnoresponse.Result != AuditReportNoResponseResult.success)
                    return new SaveAuditResponse() { Result = SaveAuditResult.AuditNotSaved };
                else
                    entity.ReportNo = reportnoresponse.ReportNo;


                var id = await _auditRepository.AddAudit(entity);
                if (id <= 0)
                    return new SaveAuditResponse() { Result = SaveAuditResult.AuditNotSaved };

                return new SaveAuditResponse() { Id = id, Result = SaveAuditResult.Success };

            }
            catch (Exception ex)
            {
                return new SaveAuditResponse() { Result = SaveAuditResult.AuditNotSaved };
            }
        }

        private void AddFactoryProfile(AudTransaction entity, AuditDetails request)
        {
            //audit factory profile
            var _faprofile = new AudTranFaProfile()
            {
                Active = true,
                Accrediations = request.Accreditations?.Trim(),
                AdministrativeStaff = request.AdminStaff,
                AnnualHolidays = request.AnnualHolidays?.Trim(),
                AnnualProduction = request.AnnualProduction?.Trim(),
                CompanyOpenTime = request.OpenHour?.Trim(),
                CompanySurfaceArea = request.FactorySurfaceArea?.Trim(),
                CreatedDate = request.FactoryCreationDate?.ToDateTime(),
                IndustryTradeAssociation = request.TradeAssociation?.Trim(),
                InvestmentBackground = request.Investment?.Trim(),
                MaximumCapacity = request.MaximumCapacity?.Trim(),
                NoOfCustomer = request.NoOfCustomers?.Trim(),
                NoOfSuppliersComponent = request.NoOfSuppliers?.Trim(),
                NumberOfSites = request.NumberOfSites?.Trim(),
                PercentageCusTotalCapacity = request.TotalCapacityByCustomer?.Trim(),
                PossibilityOfExtension = request.FactoryExtension?.Trim(),
                ProductionStaff = request.ProductionStaff,
                PublicLiabilityInsurance = request.Liability?.Trim(),
                QualityStaff = request.QualityStaff,
                SalesStaff = request.SalesStaff,
                TotalStaff = request.TotalStaff,
                TypeOfProductManufactured = request.ManufactureProducts?.Trim(),
                TypesOfBrands = request.BrandsProduced?.Trim(),
                CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                CreatedOn = DateTime.Now
            };
            entity.AudTranFaProfiles.Add(_faprofile);
        }

        //public async Task UploadFiles(int auditid, Dictionary<Guid, byte[]> fileList)
        //{
        //    var guidList = fileList.Select(x => x.Key);
        //    var data = await _auditRepository.GetReceptFiles(auditid, guidList);

        //    foreach (var item in data)
        //        item.File = fileList[item.GuidId];

        //    await _auditRepository.Save();

        //}

        private void AddFiles(IEnumerable<AuditAttachment> files, AudTransaction audit)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var ecFile = new AudTranFileAttachment
                    {
                        FileName = file.FileName,
                        UniqueId = file.uniqueld,
                        UploadDate = DateTime.Now,
                        UserId = audit.CreatedBy.GetValueOrDefault() > 0 ? audit.CreatedBy.GetValueOrDefault() : _ApplicationContext.UserId,
                        FileUrl = file.FileUrl,
                        Active = true
                    };
                    audit.AudTranFileAttachments.Add(ecFile);
                }
            }

        }

        private void Update_Auditor_CS(AuditDetails request, AudTransaction entity)
        {
            //auditor 
            var audlist = request.Auditors.Select(x => x).ToArray();
            var removeaudlist = entity.AudTranAuditors.Where(x => !audlist.Contains(x.StaffId) && x.Active);
            var existcuslist = entity.AudTranAuditors.Where(x => audlist.Contains(x.StaffId) && x.Active);
            var lstaud = new List<AudTranAuditor>();
            foreach (var auditor in removeaudlist)
            {
                auditor.Active = false;
                auditor.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                auditor.DeletedTime = DateTime.Now;
                lstaud.Add(auditor);
            }
            if (lstaud.Count() > 0)
                _auditRepository.EditEntities(lstaud);

            foreach (var id in audlist)
            {
                if (!existcuslist.Any() || !existcuslist.Any(x => x.StaffId == id))
                {
                    entity.AudTranAuditors.Add(new AudTranAuditor()
                    {
                        StaffId = id,
                        Active = true,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedTime = DateTime.Now
                    });
                }
            }

            //cs 
            var audcslist = request?.AuditCS.Select(x => x).ToArray();
            var removeaudcslist = entity.AudTranCS.Where(x => !audcslist.Contains(x.StaffId) && x.Active);
            var existaudcslist = entity.AudTranCS.Where(x => audcslist.Contains(x.StaffId) && x.Active);
            var lstcsaud = new List<AudTranC>();
            foreach (var cs in removeaudcslist)
            {
                cs.Active = false;
                cs.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                cs.DeletedTime = DateTime.Now;
                lstcsaud.Add(cs);
            }
            if (lstcsaud.Count() > 0)
                _auditRepository.EditEntities(lstcsaud);

            foreach (var id in audcslist)
            {
                if (!existaudcslist.Any() || !existaudcslist.Any(x => x.StaffId == id))
                {
                    entity.AudTranCS.Add(new AudTranC()
                    {
                        StaffId = id,
                        Active = true,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedTime = DateTime.Now
                    });
                }
            }
        }

        //public async Task<FileResponse> GetFile(int id)
        //{
        //    var file = await _auditRepository.GetFile(id);

        //    if (file == null)
        //        return new FileResponse { Result = FileResult.NotFound };

        //    return new FileResponse
        //    {
        //        Content = file.File,
        //        MimeType = _fileManager.GetMimeType(Path.GetExtension(file.FileName)),
        //        Result = FileResult.Success
        //    };
        //}

        public async Task<AuditBookingRuleResponse> GetAuditBookingRules(int customerid, int factid)
        {
            try
            {
                var factdetails = await _suppliermanager.GetEditSupplier(factid);
                var factoryCountryid = factdetails.SupplierDetails.AddressList.Select(x => x.CountryId).FirstOrDefault();
                var office = await _office.GetOfficeByFactoryid(factid);
                var response = new AuditBookingRuleResponse() { RuleDetails = new AuditBookingRule(), Result = new AuditBookingRuleResult() };
                //holidays
                var data = await _auditRepository.GetAuditBookingRule();
                var holidays = await _humanresourcemanager.GetHolidaysByRange(DateTime.Now, DateTime.Now.AddYears(1), factoryCountryid, office?.Id ?? 0);
                response.RuleDetails.Holidays = holidays;
                //getting audit lead days and description
                if (data.Any(x => x.CustomerId == customerid && x.FactoryCountryId == factoryCountryid && x.Active))//customer & country configuration
                {
                    var ruledetails = data.Where(x => x.CustomerId == customerid && x.FactoryCountryId == factoryCountryid && x.Active).FirstOrDefault();
                    if (ruledetails == null || ruledetails.BookingRule == string.Empty || ruledetails.LeadDays == 0)
                        return new AuditBookingRuleResponse() { Result = AuditBookingRuleResult.NotFound };
                    response.RuleDetails.RuleDescription = ruledetails?.BookingRule;
                    response.RuleDetails.LeadDays = ruledetails?.LeadDays ?? 0;
                    response.Result = AuditBookingRuleResult.Success;
                    return response;
                }
                else if (data.Any(x => x.IsDefault && x.Active))//default configuration
                {
                    var ruledetails = data.Where(x => x.IsDefault && x.Active).FirstOrDefault();
                    if (ruledetails == null || ruledetails.BookingRule == string.Empty || ruledetails.LeadDays == 0)
                        return new AuditBookingRuleResponse() { Result = AuditBookingRuleResult.NotFound };
                    response.RuleDetails.RuleDescription = ruledetails?.BookingRule;
                    response.RuleDetails.LeadDays = ruledetails?.LeadDays ?? 0;
                    response.Result = AuditBookingRuleResult.Success;
                    return response;
                }
                else
                {
                    return new AuditBookingRuleResponse() { Result = AuditBookingRuleResult.NotFound };
                }

            }
            catch (Exception ex)
            {
                return new AuditBookingRuleResponse() { Result = AuditBookingRuleResult.NotFound };
            }
        }

        public async Task<AuditReportNoResponse> GetAuditReportNo(int customerid, int factoryid)
        {
            var response = new AuditReportNoResponse();
            var reportno = await CreateReportNo(customerid, factoryid);
            if (string.IsNullOrEmpty(reportno))
                new AuditReportNoResponse() { Result = AuditReportNoResponseResult.Fail };
            bool isreportexists = await _auditRepository.IsReportNoExists(reportno, customerid);
            if (isreportexists)
                await GetAuditReportNo(customerid, factoryid);
            response.ReportNo = reportno;
            response.Result = AuditReportNoResponseResult.success;
            return response;
        }

        public async Task<string> CreateReportNo(int customerid, int factoryid)
        {
            string _reportno = "";
            StringBuilder sbr = new StringBuilder();
            try
            {
                var data = await _auditRepository.GetCusLastReportNo(customerid);
                var _cusdetails = await _customerManager.GetEditCustomer(customerid);
                if (_cusdetails != null && _cusdetails.Result == EditCustomerResult.CannotGetCustomer)
                    return null;
                string _customername = _cusdetails?.CustomerDetails?.Name?.ToUpper().Replace(" ", "");
                string _customerid = _cusdetails?.CustomerDetails?.Id.ToString();
                string month = DateTime.Now.Month.ToString();
                string year = DateTime.Now.Year.ToString();

                if (data == null)
                {
                    sbr.AppendFormat("AUD-{0}-{1}-{2}-1", (_customername.Length > 3 ? _customername.Substring(0, 3) : _customername), _customerid, year);
                }
                else
                {
                    string reportno = data.ReportNo ?? "";
                    if (reportno == null)
                        return null;
                    string[] _lstreportno = reportno.Split('-');
                    if (_lstreportno.Length > 0)
                    {
                        if (int.TryParse(_lstreportno[_lstreportno.Length - 1], out int no))
                        {
                            no++;
                            if (int.TryParse(_lstreportno[_lstreportno.Length - 2], out int _reportyear))
                                no = DateTime.Now.Year == _reportyear ? no : 1;
                            sbr.AppendFormat("AUD-{0}-{1}-{2}-{3}", (_customername.Length > 3 ? _customername.Substring(0, 3) : _customername), _customerid, year, no);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                _reportno = sbr.ToString();
            }
            catch (Exception ex)
            {
                return _reportno;
            }
            return _reportno;
        }

        public async Task<AuditBookingContactResponse> GetAuditBookingContactInformation(int factid)
        {
            var response = new AuditBookingContactResponse() { };
            var office = await _office.GetOfficeByFactoryid(factid);
            if (office == null)
                return new AuditBookingContactResponse() { Result = EditAuditResult.CannotGetOfficeList };
            var factdetails = await _suppliermanager.GetEditSupplier(factid);
            var FactoryCountryId = factdetails.SupplierDetails.AddressList.Select(x => x.CountryId).FirstOrDefault();

            var data = await _auditRepository.GetAuditBookingContacts(office.Id);
            if (data == null || data.Count == 0)
                return new AuditBookingContactResponse() { Result = EditAuditResult.CannotGetContactDetails };
            if (data.Any(x => x.FactoryCountryId == FactoryCountryId))
            {
                response.ContactDetails = data.Where(x => x.FactoryCountryId == FactoryCountryId).Select(_auditmap.GetBookingContact).FirstOrDefault();
            }
            else
            {
                response.ContactDetails = data.Select(_auditmap.GetBookingContact).FirstOrDefault();
            }
            response.Result = EditAuditResult.GetContactDetailsSuccess;
            return response;
        }

        public async Task<AuditSummaryResponse> GetAuditSummary()
        {
            var response = new AuditSummaryResponse();

            var commonDataSourceRequest = new CommonDataSourceRequest()
            {
                IsVirtualScroll = false
            };
            //customer list
            var cuslistresponse = await _customerManager.GetCustomerByUserType(commonDataSourceRequest);
            if (cuslistresponse == null || cuslistresponse.Result != DataSourceResult.Success || cuslistresponse.DataSourceList.Count() == 0)
                return new AuditSummaryResponse() { Result = AuditSummaryResponseResult.failed };
            response.CustomerList = cuslistresponse.DataSourceList;

            //office list
            //response.OfficeList = UserTypeEnum.InternalUser != _ApplicationContext.UserType ? _office.GetOffices() : _office.GetOfficesByUserId(_ApplicationContext.StaffId);
            //if (response.OfficeList == null || response.OfficeList.Count() == 0)
            //  return new AuditSummaryResponse() { Result = AuditSummaryResponseResult.failed };

            //office list
            response.OfficeList = _office.GetOffices();
            if (UserTypeEnum.InternalUser == _ApplicationContext.UserType)//logic for location configuration
            {
                var _cusofficelist = _office.GetOfficesByUserId(_ApplicationContext.StaffId);
                response.OfficeList = _cusofficelist == null || _cusofficelist.Count() == 0 ? response.OfficeList : _cusofficelist;
            }

            //status list
            var statuslist = await _auditRepository.GetAuditStatus();
            if (statuslist != null && statuslist.Count == 0)
                return new AuditSummaryResponse() { Result = AuditSummaryResponseResult.failed };
            response.StatusList = statuslist.Select(_auditmap.GetAuditStatus);

            //supplier list
            var supplierlistresponse = await _suppliermanager.GetSuppliersByUserType(null);
            if (supplierlistresponse.Result == SupplierListResult.Success && supplierlistresponse.Data.Count() > 0)
                response.SupplierList = supplierlistresponse.Data;

            //factory list
            var factlistresponse = await _suppliermanager.GetFactorysByUserType(null, null);
            if (factlistresponse.Result == SupplierListResult.Success && factlistresponse.Data.Count() > 0)
                response.FactoryList = factlistresponse.Data;

            response.Result = AuditSummaryResponseResult.success;
            return response;
        }


        public async Task<SetInspNotifyResponse> BookingTaskNotification(int id, int statusId, AuditDetails request)
        {
            if (_dictStatuses.TryGetValue((AuditBookingStatus)statusId, out Func<int, AuditDetails, Task<SetInspNotifyResponse>> func))
                return await func(id, request);

            return new SetInspNotifyResponse { Result = SetInspStatusResult.CannotUpdateStatus };
        }

        public async Task<AuditStatusResponse> GetAuditStatuses()
        {
            var statuslist = await _auditRepository.GetAuditStatus();
            if (statuslist != null && statuslist.Count == 0)
                return new AuditStatusResponse() { Result = AuditStatusResponseResult.Error };
            var statuslistresponse = statuslist.Select(_auditmap.GetAuditStatus);
            return new AuditStatusResponse() { Auditstatuslist = statuslistresponse, Result = AuditStatusResponseResult.success };
        }
        public async Task<AuditSummarySearchResponse> SearchAuditSummary(AuditSummarySearchRequest request)
        {
            try
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
                var cuslist = new List<int>();
                if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                {
                    if (request.Officeidlst != null && request.Officeidlst.Count() == 0)
                    {
                        var _cusofficelist = _office.GetOfficesByUserId(_ApplicationContext.StaffId);
                        request.Officeidlst = _cusofficelist == null || _cusofficelist.Count() == 0 ? request.Officeidlst : _cusofficelist.Select(x => (int?)x.Id);
                    }

                    if (request?.CustomerId != null)
                        cuslist.Add(request.CustomerId.Value);
                }
                else
                {
                    if (request?.CustomerId != null)
                        cuslist.Add(request.CustomerId.Value);
                }
                var reporequest = new AuditSummaryRepoRequest()
                {
                    Customerlst = cuslist,
                    SupplierId = request.SupplierId ?? 0,
                    DatetypeId = request.DateTypeid,
                    Fromdate = request.FromDate?.ToDateTime(),
                    Todate = request.ToDate?.ToDateTime(),
                    SearchText = request.SearchTypeText?.Trim(),
                    SearchTypeId = request.SearchTypeId,
                    skip = skip,
                    take = take,
                    FactoryIdlst = request.FactoryIdlst,
                    Statuslst = request.StatusIdlst,
                    OfficeIdlst = request.Officeidlst,
                    AuditorIdList = request.AuditorIdList,
                    FactoryCountryIdList = request.FactoryCountryIdList,
                    ServiceTypeIdList = request.ServiceTypeIdList
                };

                //get data as iqueryable audit
                var data = SearchAuditSummary(reporequest);

                //get audit ids
                var auditIdList = data.Select(x => x.Id);

                var serviceTypeList = await _auditRepository.GetServiceTypeDataByAudit(auditIdList);

                var factoryList = await _auditRepository.GetFactoryCountryDataByAudit(auditIdList);

                var auditorList = await _auditRepository.GetAuditorDataByAudit(auditIdList);

                var quotationStatusList = await _auditRepository.GetQuotationDataByAudit(auditIdList);


                int total = await data.CountAsync();

                if (total <= 0)
                    return new AuditSummarySearchResponse() { Result = AuditSummarySearchResponseResult.NotFound };

                var statusList = await data.Select(x => new AuditRepoStatus { Id = x.StatusId, StatusName = x.Status.Status })
                    .GroupBy(p => new { p.Id, p.StatusName }, p => p, (key, _data) =>
                new AuditRepoStatus
                {
                    Id = key.Id,
                    StatusName = key.StatusName,
                    TotalCount = _data.Count()
                })
                    .ToListAsync();


                var auditData = data.Select(x => new AuditRepoItem()
                {
                    AuditId = x.Id,
                    CustomerName = x.Customer.CustomerName,
                    FactoryName = x.Factory.SupplierName,
                    PoNumber = x.PoNumber,
                    ReportNo = x.ReportNo,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    SupplierName = x.Supplier.SupplierName,
                    Office = x.Office.LocationName,
                    StatusId = x.StatusId,
                    BookingCreatedBy = x.CreatedByNavigation.UserTypeId,
                    CustomerBookingNo = x.CustomerBookingNo,
                    CreatedOnEaqf = x.CreatedOn.GetValueOrDefault().ToString(StandardISO8601DateTimeFormat)
                });

                var result = await auditData.Skip(skip).Take(take).ToListAsync();
                var auditIds = result.Select(x => x.AuditId);
                var _statuslist = statusList.Select(_auditmap.GetAuditStatus);
                var supplierCustomerList = await _auditRepository.GetSupplierCustomerDataByAudit(auditIds);
                var factoryCustomerList = await _auditRepository.GetFactoryCustomerDataByAudit(auditIds);

                var _resultdata = result.Select(x => _auditmap.GetAuditSearchItem(x, _statuslist, serviceTypeList, factoryList, auditorList, quotationStatusList, supplierCustomerList, factoryCustomerList));

                return new AuditSummarySearchResponse()
                {
                    Result = AuditSummarySearchResponseResult.Success,
                    TotalCount = total,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (total / request.pageSize.Value) + (total % request.pageSize.Value > 0 ? 1 : 0),
                    Data = _resultdata,
                    AuditStatuslst = _statuslist
                };
            }
            catch (Exception ex)
            {
                return new AuditSummarySearchResponse() { Result = AuditSummarySearchResponseResult.NotFound };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IQueryable<AudTransaction> SearchAuditSummary(AuditSummaryRepoRequest request)
        {
            var data = _auditRepository.GetAuditMainData();

            if (request != null && request.Customerlst != null && request.Customerlst.Any())
            {
                data = data.Where(x => request.Customerlst.Contains(x.CustomerId));
            }

            if (request != null && request.SupplierId != 0)
            {
                data = data.Where(x => x.SupplierId == request.SupplierId);
            }

            if (request != null && request.FactoryIdlst != null && request.FactoryIdlst.Any())
            {
                data = data.Where(x => request.FactoryIdlst.ToList().Contains(x.FactoryId));
            }

            if (request != null && request.OfficeIdlst != null && request.OfficeIdlst.Any())
            {
                data = data.Where(x => request.OfficeIdlst.ToList().Contains(x.OfficeId.Value));
            }

            if (request != null && request.Statuslst != null && request.Statuslst.Any())
            {
                data = data.Where(x => request.Statuslst.ToList().Contains(x.StatusId));
            }

            //apply factory country filter
            if (request.FactoryCountryIdList != null && request.FactoryCountryIdList.Any())
            {
                data = data.Where(x => x.Factory.SuAddresses.Any(y => request.FactoryCountryIdList.Contains(y.CountryId)));
            }

            //apply auditor(staff) filter
            if (request.AuditorIdList != null && request.AuditorIdList.Any())
            {
                data = data.Where(x => x.AudTranAuditors.Any(y => request.AuditorIdList.Contains(y.StaffId)));
            }

            //apply servicetype filter
            if (request.ServiceTypeIdList != null && request.ServiceTypeIdList.Any())
            {
                data = data.Where(x => x.AudTranServiceTypes.Any(y => y.Active && request.ServiceTypeIdList.Contains(y.ServiceTypeId)));
            }

            if (Enum.TryParse(request.SearchTypeId.ToString(), out SearchType _seachtypeenum))
            {
                switch (_seachtypeenum)
                {
                    case SearchType.BookingNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchText) && int.TryParse(request.SearchText, out int bookid))
                            {
                                data = data.Where(x => x.Id == bookid);
                            }
                            break;
                        }
                    case SearchType.ReportNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchText))
                            {
                                data = data.Where(x => x.ReportNo.Contains(request.SearchText));
                            }
                            break;
                        }
                    case SearchType.PoNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchText))
                            {
                                data = data.Where(x => x.PoNumber.Contains(request.SearchText));
                            }
                            break;
                        }
                    case SearchType.CustomerBookingNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchText))
                            {
                                data = data.Where(x => x.CustomerBookingNo.Contains(request.SearchText));
                            }
                            break;
                        }
                }
                if (Enum.TryParse(request.DatetypeId.ToString(), out SearchType _datesearchtype))
                {
                    switch (_datesearchtype)
                    {
                        case SearchType.ApplyDate:
                            {
                                if (request.Fromdate != null && request.Todate != null)
                                {
                                    //  data = data.Where(x => x.CreatedOn >= request.Fromdate.Value && x.CreatedOn <= request.Todate.Value);
                                    data = data.Where(x => EF.Functions.DateDiffDay(request.Fromdate, x.CreatedOn) >= 0 &&
                                                    EF.Functions.DateDiffDay(x.CreatedOn, request.Todate) >= 0);
                                }
                                break;
                            }
                        case SearchType.ServiceDate:
                            {
                                if (request.Fromdate != null && request.Todate != null)
                                {
                                    // data = data.Where(x => x.ServiceDateFrom >= request.Fromdate.Value && x.ServiceDateTo <= request.Todate.Value);
                                    data = data.Where(x => !((x.ServiceDateFrom > request.Todate.Value) || (x.ServiceDateTo < request.Fromdate.Value)));
                                }
                                break;
                            }
                    }
                }
            }
            return data;
        }

        public async Task<EditCancelRescheduleAuditResponse> GetAuditCancelDetails(int id, int optypeid)
        {
            var response = new EditCancelRescheduleAuditResponse();
            var data = await _auditRepository.GetAuditCancelDetails(id);
            if (data == null)
                return new EditCancelRescheduleAuditResponse() { Result = CancelAuditResponseResult.CannotGetAuditDetails };
            response.Data = _auditmap.GetAuditCancelItem(data);
            response.Data.ReasonTypes = await GetCancelRescheduleAuditReason(data.CustomerId, optypeid);
            response.ItemDetails = _auditmap.MapAuditCancelDetails(data, optypeid);
            var bookingrule = await GetAuditBookingRules(data.CustomerId, data.FactoryId);
            if (bookingrule != null && bookingrule.Result == AuditBookingRuleResult.NotFound)
                return new EditCancelRescheduleAuditResponse() { Result = CancelAuditResponseResult.CannotGetAuditDetails };
            response.Data.HolidayDates = bookingrule.RuleDetails.Holidays;
            response.Data.LeadTime = bookingrule.RuleDetails.LeadDays;
            response.Data.CurrencyLst = _referencemanager.GetCurrencies();
            response.Result = CancelAuditResponseResult.success;

            return response;
        }

        public async Task<IEnumerable<AuditCancelRescheduleReasons>> GetCancelRescheduleAuditReason(int? customerid, int optypeid)
        {
            var data = await _auditRepository.GetAuditCancelRescheduleReasons(customerid, optypeid);
            if (data == null || data.Count == 0)
                return null;
            return data.Select(_auditmap.GetAuditCancelRescheduleReasons);
        }

        public async Task<SaveCancelRescheduleAuditResponse> SaveAuditCancelDetails(AuditSaveCancelRescheduleItem request)
        {
            try
            {
                DateTime previousServiceDateFrom = new DateTime();
                DateTime previousServiceDateTo = new DateTime();
                var entity = await _auditRepository.GetAuditDetailsById(request.AuditId);
                if (entity == null)
                    return new SaveCancelRescheduleAuditResponse() { Result = SaveCancelAuditResult.AuditNotFound };

                var requestlog = JsonConvert.SerializeObject(request);
                _logger.LogInformation(requestlog);//log auditdetails.


                if (request.Optypeid == (int)AuditBookingOperationType.Cancel)
                {
                    entity.StatusId = (int)Entities.Enums.AuditStatus.Cancel;
                }
                else if (request.Optypeid == (int)AuditBookingOperationType.Reschedule)
                {
                    // take previuos serivice date
                    previousServiceDateFrom = entity.ServiceDateFrom;
                    previousServiceDateTo = entity.ServiceDateTo;

                    entity.StatusId = (int)Entities.Enums.AuditStatus.Rescheduled;
                    entity.ServiceDateFrom = request.Servicedatefrom != null ? request.Servicedatefrom.ToDateTime() : entity.ServiceDateFrom;
                    entity.ServiceDateTo = request.Servicedateto != null ? request.Servicedateto.ToDateTime() : entity.ServiceDateTo;
                }
                var cancelentity = entity.AudTranCancelReschedules.Where(x => x.AuditId == request.AuditId).FirstOrDefault();
                if (cancelentity == null || request.Optypeid == (int)AuditBookingOperationType.Reschedule)
                {

                    entity.AudTranCancelReschedules.Add(new AudTranCancelReschedule
                    {
                        OperationTypeId = request.Optypeid,
                        Comments = request.Comment,
                        CurrencyId = request.CurrencyId,
                        CreatedBy = _ApplicationContext.UserId,
                        InternalComments = request.Internalcomment,
                        ReasonTypeId = request.Reasontypeid.Value,
                        TimeTypeId = request.Cancelrescheduletimetype,
                        TravellingExpense = request.Travelexpense != null ? Convert.ToDecimal(request.Travelexpense.Value) : 0
                    });
                }
                else if (cancelentity != null && request.Optypeid == (int)AuditBookingOperationType.Cancel)
                {
                    cancelentity.OperationTypeId = request.Optypeid;
                    cancelentity.Comments = request.Comment;
                    cancelentity.CurrencyId = request.CurrencyId;
                    cancelentity.InternalComments = request.Internalcomment;
                    cancelentity.CreatedBy = _ApplicationContext.UserId;
                    cancelentity.ReasonTypeId = request.Reasontypeid.Value;
                    cancelentity.TimeTypeId = request.Cancelrescheduletimetype;
                    cancelentity.TravellingExpense = request.Travelexpense != null ? Convert.ToDecimal(request.Travelexpense.Value) : 0;
                    _auditRepository.EditEntity(cancelentity);
                }
                //auditor and cs 

                var removeauditor = entity.AudTranAuditors.Where(x => x.Active);
                var lstaud = new List<AudTranAuditor>();
                foreach (var qc in removeauditor)
                {
                    qc.Active = false;
                    qc.DeletedBy = _ApplicationContext.UserId;
                    qc.DeletedTime = DateTime.Now;
                    lstaud.Add(qc);
                }
                if (lstaud.Count() > 0)
                    _auditRepository.EditEntities(lstaud);

                var entityId = _filterService.GetCompanyId();
                entity.AudTranStatusLogs.Add(new AudTranStatusLog()
                {
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    StatusId = entity.StatusId,
                    EntityId = entityId,
                });
                _auditRepository.Save(entity, true);

                //cancel the quotation  
                if (request.Optypeid == (int)AuditBookingOperationType.Cancel)
                {
                    int quotationid = await _quotationManager.GetQuotationIdByAuditid(request.AuditId);
                    if (quotationid != 0)
                    {
                        var requestManager = new SetStatusBusinessRequest
                        {
                            Id = quotationid,
                            IdStatus = QuotationStatus.Canceled,
                            OnSendEmail = null,
                            ApiRemark = "",
                            Url = string.Format(_configuration["UrlQuotation"], quotationid),
                            ApiInternalRemark = "quotation cancelled while cancelled the audit #" + request.AuditId + ""
                        };
                        await _quotationManager.SetStatus(requestManager);
                    }

                    var invoiceTransations = await _invoiceRepository.GetInvoiceListByAuditId(new[] { request.AuditId });

                    if (invoiceTransations != null)
                    {

                        var invoiceTransaction = invoiceTransations.FirstOrDefault();

                        if (invoiceTransaction != null)
                        {
                            // when cancel the invoice and delete the invoice id mapping from extra fees and update the status
                            foreach (var extrafee in invoiceTransaction.InvExfTransactions.Where(x => x.InspectionId == request.AuditId && x.StatusId != (int)ExtraFeeStatus.Cancelled))
                            {
                                extrafee.InvoiceId = null;
                                extrafee.StatusId = (int)ExtraFeeStatus.Pending;
                                extrafee.UpdatedBy = _ApplicationContext.UserId;
                                extrafee.UpdatedOn = DateTime.Now;

                                extrafee.InvExfTranStatusLogs.Add(new InvExfTranStatusLog()
                                {
                                    CreatedBy = _ApplicationContext.UserId,
                                    CreatedOn = DateTime.Now,
                                    InspectionId = extrafee.InspectionId,
                                    StatusId = (int)ExtraFeeStatus.Pending,
                                    EntityId = entityId
                                });
                            }

                            invoiceTransaction.UpdatedOn = DateTime.Now;
                            invoiceTransaction.UpdatedBy = _ApplicationContext.UserId;
                            invoiceTransaction.InvoiceStatus = (int)InvoiceStatus.Cancelled;
                            invoiceTransaction.Remarks = "Invoice cancelled when Audit booking cancelled";

                            //remove the invoice transaction contacts
                            _invoiceRepository.RemoveEntities(invoiceTransations.SelectMany(x => x.InvAutTranContactDetails));

                            _invoiceRepository.EditEntities(invoiceTransations);

                            await _invoiceRepository.Save();
                        }
                    }
                }
                return new SaveCancelRescheduleAuditResponse()
                {
                    FactoryId = entity.FactoryId,
                    CustomerId = entity.CustomerId,
                    PrevServiceDateFrom = previousServiceDateFrom,
                    PrevServiceDateTo = previousServiceDateTo,
                    Result = SaveCancelAuditResult.Success
                };
            }
            catch (Exception)
            {
                return new SaveCancelRescheduleAuditResponse() { Result = SaveCancelAuditResult.AuditNotUpdated };
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

        public async Task<EaqfErrorResponse> ValidateEaqfBooking(SaveEaqfAuditRequest request)
        {
            if (request.Vendor == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { RequiredProdutDetail });
            }

            if (string.IsNullOrEmpty(request.Vendor.Name.Trim()))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidVendor });
            }

            if (string.IsNullOrEmpty(request.Vendor.Country.Trim()))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidCountry });
            }

            if (!(await _referenceRepo.IsValidEntity(_filterService.GetCompanyId())))
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidServiceType });
            }

            var customer = _customerManager.GetCustomerbyId(request.CustomerId);
            if (customer == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { CustomerNotFound });
            }


            return null;
        }


        public async Task<object> SaveEaqfAuditBooking(SaveEaqfAuditRequest request)
        {
            var entityId = _filterService.GetCompanyId();
            var userId = request.UserId;
            SuSupplier suSupplier = null;
            SaveSupplierResponse supplierResponse = null;
            AuditDetails eaqfBooking = new AuditDetails();

            eaqfBooking.UserId = userId;

            //Get customer contact details
            var customerContactId = await _userAccountRepository.GetCustomerContactIdByUser(userId);
            var cus = await _customerContactRepo.GetCustomerContactsList(new[] { customerContactId }.ToList());

            eaqfBooking.ApplicantName = cus.Select(x => x.ContactName).FirstOrDefault();
            eaqfBooking.ApplicantEmail = cus.Select(x => x.Email).FirstOrDefault();


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
                            supplierResponse = await _suppliermanager.SaveEaqfSupplier(request.Vendor, (int)Supplier_Type.Supplier_Agent, request.CustomerId, request.UserId, null);
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
                        await _suppliermanager.SaveEaqfSupplier(request.Vendor, (int)Supplier_Type.Supplier_Agent, request.CustomerId, request.UserId, null);
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
                            supplierResponse = await _suppliermanager.SaveEaqfSupplier(request.Factory, (int)Supplier_Type.Factory, request.CustomerId, request.UserId, eaqfBooking.SupplierId);
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
                                            SupplierId = eaqfBooking.FactoryId,
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
                        await _suppliermanager.SaveEaqfSupplier(request.Factory, (int)Supplier_Type.Factory, request.CustomerId, request.UserId, eaqfBooking.SupplierId);
                        eaqfBooking.FactoryId = request.Factory.Id;
                    }
                }
            }

            eaqfBooking.AuditId = request.BookingId;
            eaqfBooking.CustomerId = request.CustomerId;
            eaqfBooking.ServiceTypeId = request.ServiceTypeId;
            eaqfBooking.ServiceDateFrom = request.ServiceDateFrom;
            eaqfBooking.ServiceDateTo = request.ServiceDateTo;
            var masterConfigs = await GetMasterConfiguration();
            eaqfBooking.OfficeId = (Convert.ToInt32(masterConfigs.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.DefaultOfficeForEAQF && x.Active == true)?.Value));
            eaqfBooking.StatusId = (int)BookingStatusNames.Requested;
            eaqfBooking.IsEaqf = true;
            eaqfBooking.TotalStaff = request.TotalStaff;
            eaqfBooking.AuditTypeid = request.AuditType;
            eaqfBooking.FactorySurfaceArea = request.SurfaceArea;
            eaqfBooking.CreatedbyUserType = userId;
            eaqfBooking.CustomerBookingNo = request.EaqfRef;

            var response = await SaveAudit(eaqfBooking);

            if (response.Result != SaveAuditResult.Success)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { response.Result.ToString() });
            }
            eaqfBooking.AuditId = response.Id;

            return new EaqfGetSuccessResponse()
            {
                message = "Success",
                statusCode = HttpStatusCode.OK,
                data = new AuditEaqfResponse()
                {
                    MissionId = response.Id,
                    VendorId = eaqfBooking.SupplierId,
                    FactoryId = eaqfBooking.FactoryId,
                    IsTechincalDocumentsAddedOrRemoved = true,
                    StatusId = eaqfBooking.StatusId,
                    SaveAuditRequest = eaqfBooking
                }
            };
        }

        public OfficeSummaryResponse GetAuditOffice()
        {
            var response = new OfficeSummaryResponse();
            //office list
            response.officeList = _office.GetOffices();
            if (UserTypeEnum.InternalUser == _ApplicationContext.UserType)//logic for location configuration
            {
                var _cusofficelist = _office.GetOfficesByUserId(_ApplicationContext.StaffId);
                response.officeList = _cusofficelist == null || _cusofficelist.Count() == 0 ? response.officeList : _cusofficelist;
            }
            response.Result = OfficeSummaryResult.Success;
            return response;
        }

        public async Task<AuditTypeResponse> GetAuditType()
        {
            var response = new AuditTypeResponse();
            var data = await _auditRepository.GetAuditType();
            if (data == null || data.Count == 0)
                return new AuditTypeResponse() { Result = AuditTypeResponseResult.success };
            response.AuditTypes = data.Select(x => _auditmap.MapAuditType(x));
            response.Result = AuditTypeResponseResult.success;
            return response;
        }

        public async Task<AuditWorkprocessResponse> GetAuditWorkProcess()
        {
            var response = new AuditWorkprocessResponse();
            var data = await _auditRepository.GetAuditWorkProcess();
            if (data == null || data.Count == 0)
                return new AuditWorkprocessResponse() { Result = AuditWorkprocessResponseResult.error };
            response.AuditWorkProcessList = _mapper.Map<IEnumerable<AuditWorkProcess>>(data);
            response.Result = AuditWorkprocessResponseResult.success;
            return response;
        }

        public async Task<AuditCSResponse> GetAuditCS(int factid, int? cusid)
        {
            var response = new AuditCSResponse();
            var office = await _office.GetOfficeByFactoryid(factid);
            if (office == null)
                return new AuditCSResponse() { Result = AuditCSResponseResult.error };
            response.AuditCS = cusid != null ? await _humanresourcemanager.GetAllCSByLocationCusId(office.Id, cusid.Value) : await _humanresourcemanager.GetAllCSByLocationCusId(office.Id);
            if (response.AuditCS == null || response.AuditCS.Count() == 0)
                return new AuditCSResponse() { Result = AuditCSResponseResult.error };
            response.Result = AuditCSResponseResult.success;
            return response;
        }

        public async Task<AuditBasicDetailsResponse> GetAuditBasicDetails(int id)
        {
            var response = new AuditBasicDetailsResponse();
            var data = await _auditRepository.GetAuditBasicDetails(id);
            if (data == null)
                return new AuditBasicDetailsResponse() { Result = AuditBasicDetailsResponseResult.CannotGetAuditDetails };
            response.Data = _auditmap.GetAuditBasicDetails(data);
            response.Result = AuditBasicDetailsResponseResult.success;
            return response;
        }

        public async Task<SaveAuditReportResponse> SaveAuditReportDetails(AuditReportDetails request)
        {
            var entity = await _auditRepository.GetAuditReportDetails(request.Auidtid);
            if (entity == null)
                return new SaveAuditReportResponse() { Result = SaveAuditReportResponseResult.AuditNotFound };

            var requestlog = JsonConvert.SerializeObject(request);
            _logger.LogInformation(requestlog);//log audit report details.

            entity.StatusId = (int)Entities.Enums.AuditStatus.Audited;

            var reportentity = entity.AudTranReportDetails.Where(x => x.AuditId == request.Auidtid && x.Active).FirstOrDefault();
            var auditorsentity = entity.AudTranAuditors.Where(x => x.AuditId == request.Auidtid && x.Active);
            var reportsentity = entity.AudTranReports.Where(x => x.AuditId == request.Auidtid && x.Active);
            if (reportentity == null)
            {
                entity.AudTranReportDetails.Add(new AudTranReportDetail()
                {
                    ServiceDateFrom = request.Servicedatefrom != null ? request.Servicedatefrom.ToDateTime() : entity.ServiceDateFrom,
                    ServiceDateTo = request.Servicedateto != null ? request.Servicedateto.ToDateTime() : entity.ServiceDateTo,
                    CreatedDate = DateTime.Now,
                    UserId = _ApplicationContext.StaffId,
                    Active = true,
                    Comments = request.Comment
                });
            }
            else
            {
                reportentity.ServiceDateFrom = request.Servicedatefrom != null ? request.Servicedatefrom.ToDateTime() : entity.ServiceDateFrom;
                reportentity.ServiceDateTo = request.Servicedateto != null ? request.Servicedateto.ToDateTime() : entity.ServiceDateTo;
                reportentity.Comments = request.Comment?.Trim();
                _auditRepository.EditEntity(reportentity);
            }
            if (reportsentity != null && reportsentity.Count() > 0 && request.Attachments != null && request.Attachments.Where(x => x.IsNew).Count() > 0)
            {
                foreach (var _delreport in reportsentity)
                {
                    _delreport.Active = false;
                }
                _auditRepository.EditEntities(reportsentity);
            }

            //old table
            var fiIds_ = request.Attachments.Where(x => x.Id > 0).Select(x => x.Id);
            var filesToremove_ = entity.AudTranReport1S.Where(x => !fiIds_.Contains(x.Id) && x.AuditId == request.Auidtid);
            var lstremove_ = new List<AudTranReport1>();

            foreach (var fileItem in filesToremove_.ToList())
            {
                lstremove_.Add(fileItem);
            }
            _auditRepository.RemoveEntities(lstremove_);

            //new table
            var fiIds = request.Attachments.Where(x => x.Id > 0).Select(x => x.Id);
            var filesToremove = entity.AudTranReports.Where(x => !fiIds.Contains(x.Id) && x.AuditId == request.Auidtid);
            foreach (var fileItem in filesToremove.ToList())
            {
                fileItem.Active = false;
                fileItem.DeletedBy = _ApplicationContext.UserId;
                fileItem.DeletedOn = DateTime.Now;
                entity.AudTranReports.Add(fileItem);
            }

            if (request.Attachments != null && request.Attachments.Count() > 0 && request.Attachments.Where(x => x.IsNew).Count() > 0)
            {
                foreach (var file in request.Attachments)
                {
                    var ecFile = new AudTranReport
                    {
                        FileName = file.FileName,
                        UniqueId = file.Uniqueld,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _ApplicationContext.UserId,
                        Active = true,
                        FileUrl = file.FileUrl
                    };
                    entity.AudTranReports.Add(ecFile);
                }
            }

            if (auditorsentity != null && auditorsentity.Count() > 0)
            {
                foreach (var _auditor in auditorsentity)
                {
                    _auditor.IsAudited = request.Auditors.Contains(_auditor.StaffId) ? true : false;
                }
                _auditRepository.EditEntities(auditorsentity);
            }
            else
                return new SaveAuditReportResponse() { Result = SaveAuditReportResponseResult.NoAuditorsFound };

            entity.AudTranStatusLogs.Add(new AudTranStatusLog()
            {
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                StatusId = entity.StatusId,
                EntityId = _filterService.GetCompanyId()
            });
            _auditRepository.Save(entity, true);
            return new SaveAuditReportResponse() { Result = SaveAuditReportResponseResult.Success };
        }

        public async Task<AuditorResponse> GetScheduledAuditors(int auditid)
        {
            var response = new AuditorResponse();
            var data = await _auditRepository.GetScheduledAuditors(auditid);
            if (data == null)
                return new AuditorResponse() { Result = AuditorResponseResult.error };
            response.Auditors = data.Select(x => _auditmap.GetAuditor(x));
            response.Result = AuditorResponseResult.success;
            return response;
        }

        public async Task<AuditReportSummary> GetAuditReportSummary(int auditid)
        {
            var response = new AuditReportSummary();
            var data = await _auditRepository.GetAuditReportDetails(auditid);
            if (data == null)
                return new AuditReportSummary() { Result = AuditReportSummaryResult.error };
            response.Data = _auditmap.MapAuditReportSummary(data, (x) => _fileManager.GetMimeType(x));
            response.Result = AuditReportSummaryResult.success;
            return response;
        }

        public async Task<FileResponse> GetAuditReport(int id)
        {
            var file = await _auditRepository.GetAuditReport(id);

            if (file == null)
                return new FileResponse { Result = FileResult.NotFound };

            return new FileResponse
            {
                Content = file.File,
                MimeType = _fileManager.GetMimeType(Path.GetExtension(file.FileName)),
                Result = FileResult.Success
            };
        }

        //public async Task UploadReportFiles(int auditid, Dictionary<Guid, byte[]> fileList)
        //{
        //    var guidList = fileList.Select(x => x.Key);
        //    var data = await _auditRepository.GetReportFiles(auditid, guidList);

        //    foreach (var item in data)
        //    {
        //        item.File = fileList[item.GuidId];
        //        //item.FileName = "";
        //        //item.FileUrl = "";
        //    }

        //    await _auditRepository.Save();
        //}

        public async Task<AuditServiceTypeResponse> GetAuditServiceType(int customerid)
        {
            var servicetype = await _customerManager.GetCustomerAuditServiceType(customerid);
            if (servicetype == null || !servicetype.Any())
                return new AuditServiceTypeResponse() { result = AuditServiceTypeResponseResult.error };
            return new AuditServiceTypeResponse() { auditServiceTypes = servicetype, result = AuditServiceTypeResponseResult.success };
        }
        /// <summary>
        /// Get Audit Details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<BookingDataRepo> GetAuditDetails(int bookingId)
        {
            return await _auditRepository.GetAuditDetails(bookingId);
        }
        /// <summary>
        /// Get Factory country  
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<AuditServiceTypeRepoResponse>> GetServiceTypeDataByAudit(int auditId)
        {
            return await _auditRepository.GetServiceTypeDataByAudit(auditId);
        }
        /// <summary>
        /// Get Factory country  
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<AuditFactoryCountryRepoResponse>> GetFactoryCountryDataByAudit(int auditId)
        {
            return await _auditRepository.GetFactoryCountryDataByAudit(auditId);
        }
        public IQueryable<int> GetAuditNo()
        {
            return _auditRepository.GetAuditNo();
        }
        //get all booking no data source
        public async Task<BookingNoDataSourceResponse> GetBookingNoDataSource(BookingNoDataSourceRequest request)
        {
            var response = new BookingNoDataSourceResponse();

            var data = _auditRepository.GetAuditNo();

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
        /// <summary>
        /// get audit details
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AuditCusFactDetails>> GetAuditDetails(IEnumerable<int> auditIds)
        {
            return await _auditRepository.GetAuditDetails(auditIds);
        }
        /// <summary>
        /// get factory address for audit
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<FactoryCountry>> GetFactoryAddressDetailsByAuditIds(IEnumerable<int> auditIds)
        {
            return await _auditRepository.GetFactoryAddressDetailsByAuditIds(auditIds);
        }

        /// <summary>
        /// audit service type details
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<AuditServiceTypeData>> GetAuditServiceTypeList(List<int> auditIds)
        {
            return await _auditRepository.GetAuditServiceTypeList(auditIds);
        }

        public async Task<AuditProductCategoryResponse> GetProductCategory(int customerId, int serviceTypeId)
        {
            var productCategoryData = _auditRepository.GetProductCategory();
            var productCategoryList = await productCategoryData.Where(x => x.CustomerId == customerId && x.ServiceType == serviceTypeId)
                                    .Select(x => new AuditProductCategory()
                                    {
                                        Id = x.Id,
                                        Name = x.Name
                                    }).ToListAsync();

            if (productCategoryList == null || !productCategoryList.Any())
                return new AuditProductCategoryResponse { Result = AuditProductCategoryResult.CannotGetList };
            else
                return new AuditProductCategoryResponse
                {
                    Data = productCategoryList,
                    Result = AuditProductCategoryResult.Success
                };
        }
        public async Task<List<string>> GetCCEmailConfigurationEmailsByCustomer(int customerId, int factoryId, int bookingStatusId)
        {
            List<string> ccEmails = null;
            if (await _customerCheckPointRepository.IsCustomerCheckpointConfigured(customerId, (int)CheckPointTypeEnum.SendBookingEmailToCustomer))
            {
                var factoryAddress = await _supplierRepo.GetSupplierHeadOfficeAddress(factoryId);
                if (factoryAddress != null)
                {
                    //based on factory country id found the data
                    var inspBookingEmailConfiguration = await _auditRepository.GetCCEmailConfigurationEmailsByCustomer(customerId, factoryAddress.countryId, bookingStatusId);
                    if (inspBookingEmailConfiguration == null)
                    {
                        //if based on factory country is not found, then fetch the default data (means factory country id is null)
                        inspBookingEmailConfiguration = await _auditRepository.GetCCEmailConfigurationEmailsByCustomer(customerId, null, bookingStatusId);
                    }
                    if (inspBookingEmailConfiguration != null)
                        return inspBookingEmailConfiguration.Email.Split(";").Distinct().Where(y => IsValidEmail(y)).ToList();
                }
            }
            return ccEmails;
        }

        private async Task<InspBookingContactResponse> GetInspBookingContactInformation(UserAccess userAccessFilter)
        {
            try
            {
                var response = new InspBookingContactResponse() { };
                var data = await _userManager.GetCustomerServiceBookingContacts(userAccessFilter);
                if (data == null || data.Count() == 0)
                    return new InspBookingContactResponse() { Result = BookingContactResult.CannotGetContactDetails };
                response.BookingContactList = data;
                return response;

            }
            catch (Exception ex)
            {
                return new InspBookingContactResponse() { Result = BookingContactResult.CannotGetOfficeList };
            }

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

        public async Task<BookingMailRequest> GetBookingMailDetail(int bookingId, AuditDetails requestEmail, bool? isEdit, int? userId = 0)
        {
            var bookingEmailRequest = new BookingMailRequest();
            var bookingMapEmailData = new BookingMapEmailData();

            try
            {
                var user = await _auditRepository.GetUserName(userId > 0 ? userId.GetValueOrDefault() : _ApplicationContext.UserId);
                bookingEmailRequest.UserName = user.FullName;
                bookingEmailRequest.BookingId = bookingId;

                var bookingDetail = await _auditRepository.GetAuditBookingDetails(bookingId);

                // Booking detail

                if (bookingDetail != null)
                {

                    bookingMapEmailData.BookingDetail = bookingDetail;


                    //get the service types
                    bookingMapEmailData.ServiceTypes = await _auditRepository.GetAuditBookingServiceTypes(new[] { bookingId });

                    //get the booking hold reasons
                    // bookingMapEmailData.BookingHoldReasons = await _repo.GetInspectionHoldReasons(bookingId);

                    //get the brand list
                    if (bookingDetail.BrandId.GetValueOrDefault() > 0)
                        bookingMapEmailData.BrandList = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingDetail.BrandId.GetValueOrDefault(),Name=bookingDetail.BrandName } };

                    //get the department list
                    if (bookingDetail.DepartmentId.GetValueOrDefault() > 0)
                        bookingMapEmailData.DepartmentList = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingDetail.DepartmentId.GetValueOrDefault(),Name=bookingDetail.DepartmentName } };

                    // Set AE user list - nothing but user who has Inspection Verified Role

                    var aeUserAccessFilter = new UserAccess
                    {
                        OfficeId = bookingDetail.OfficeId != null ? bookingDetail.OfficeId.Value : 0,
                        ServiceId = (int)Entities.Enums.Service.AuditId,
                        CustomerId = bookingDetail.CustomerId,
                        RoleId = (int)RoleEnum.InspectionVerified,
                        ProductCategoryIds = Enumerable.Empty<int?>(),

                        DepartmentIds = bookingDetail.DepartmentId.GetValueOrDefault() > 0
                        && bookingMapEmailData.DepartmentList.Any() ?
                        bookingMapEmailData.DepartmentList?.Select(x => (int?)x.Id).Distinct() :
                        Enumerable.Empty<int?>(),

                        BrandIds = bookingDetail.BrandId.GetValueOrDefault() > 0 &&
                        bookingMapEmailData.BrandList.Any() ?
                        bookingMapEmailData.BrandList?.Select(x => (int?)x.Id).Distinct()
                        : Enumerable.Empty<int?>()
                    };
                    var AEUser = await _userManager.GetUserListByRoleOfficeServiceCustomer(aeUserAccessFilter);
                    bookingEmailRequest.AEUserEmail = string.Join(", ", AEUser.Select(x => x.EmailAddress));
                    ////get the buyer list
                    //bookingMapEmailData.BuyerList = await _repo.GetBookingBuyerList(new[] { bookingId });

                    //get the factory contact list
                    bookingMapEmailData.Factcontactlist = await _auditRepository.GetFactoryContactsByBookingIds(new[] { bookingId }.ToList());

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
                    bookingMapEmailData.InspSupplierContacts = await _auditRepository.GetSupplierContactsByBookingIds(new[] { bookingId }.ToList());

                    //get the season details
                    bookingMapEmailData.CustomerSeasonData = await _referenceRepo.GetSeasonById(bookingDetail.CustomerSeasonId);

                    //map the booking details
                    bookingEmailRequest = _bookingmap.MapBookingForEmail(bookingEmailRequest, bookingMapEmailData, (int)CountryEnum.China);

                    ////fetch the total containers
                    //var bookingContainers = await _repo.GetBookingContainer(new[] { bookingId });
                    //var totalContainers = bookingContainers.Where(x => x.ContainerId.HasValue).Select(x => x.ContainerId).Distinct().Count();
                    //bookingEmailRequest.TotalContainers = totalContainers;

                    ////get the product transaction bookingEmailRequest
                    //var productTransactions = await _repo.GetProductTransactionList(bookingId);

                    ////get the purchase order bookingEmailRequest
                    //var purchaseOrderTransactions = await _repo.GetPurchaseOrderTransactionList(bookingId);

                    //if (bookingEmailRequest.BusinessLine == (int)BusinessLine.SoftLine)
                    //{
                    //    var _poColorTransactionList = await _repo.GetPOColorTransactions(bookingId);
                    //    bookingEmailRequest.InspectionPoList = GetColorPOProductDetails(productTransactions, purchaseOrderTransactions, _poColorTransactionList);
                    //}
                    //else if (bookingEmailRequest.BusinessLine == (int)BusinessLine.HardLine)
                    //{
                    //    bookingEmailRequest.InspectionPoList = GetPODetails(productTransactions, purchaseOrderTransactions);
                    //}

                    //get the inspection file attachments
                    bookingEmailRequest.InspectionFileAttachments = await _auditRepository.GetAuditBookingMappedFiles(new List<int>() { bookingId });

                    // bookingEmailRequest.SplitBookingId = bookingDetail.SplitPreviousBookingNo == null ? 0 : bookingDetail.SplitPreviousBookingNo.Value;

                    //Get product category details
                    // var productCategoryList = await GetProductCategoryDetails(new[] { bookingId });
                    //Get Department details
                    var departmentbookingEmailRequest = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingDetail.DepartmentId.GetValueOrDefault(),Name=bookingDetail.DepartmentName } };
                    //Get Brand details
                    var brandbookingEmailRequest = new List<CommonDataSource> { new CommonDataSource()
                        { Id = bookingDetail.BrandId.GetValueOrDefault(),Name=bookingDetail.BrandName } };

                    var userAccessFilter = new UserAccess
                    {
                        OfficeId = bookingDetail.OfficeId != null ? bookingDetail.OfficeId.Value : 0,
                        ServiceId = (int)Entities.Enums.Service.AuditId,
                        CustomerId = bookingDetail.CustomerId,
                        RoleId = (int)RoleEnum.InspectionVerified,
                        ProductCategoryIds = Enumerable.Empty<int?>(),
                        DepartmentIds = departmentbookingEmailRequest.Any() ? departmentbookingEmailRequest?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                        BrandIds = brandbookingEmailRequest.Any() ? brandbookingEmailRequest?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>()
                    };
                    //Get the user details based on the DA_UserCustomer and DA_UserRoleNotificationByOffice tables
                    InspBookingContactResponse bookingContact = await GetInspBookingContactInformation(userAccessFilter);

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


                    bookingEmailRequest.IsEmailRequired = requestEmail.IsEmailRequired;

                    // Booking cancel
                    var cancelbookingEmailRequest = await _cancelBookingRepository.GetAuditCancelDetailsById(bookingId);
                    if (cancelbookingEmailRequest != null)
                    {
                        bookingEmailRequest.ReasonType = cancelbookingEmailRequest.ReasonType?.Reason;
                        bookingEmailRequest.Comment = cancelbookingEmailRequest.Comments;
                        bookingEmailRequest.CanceledBy = cancelbookingEmailRequest.CreatedByNavigation?.FullName;
                    }

                    // Booking reschedule
                    var schedulebookingEmailRequest = await _cancelBookingRepository.GetAuditRescheduleDetailsById(bookingId);
                    if (schedulebookingEmailRequest != null)
                    {
                        bookingEmailRequest.RescheduleServiceDateFrom = requestEmail.ServiceDateFrom.ToDateTime().ToString(StandardDateFormat);
                        bookingEmailRequest.RescheduleServiceDateTo = requestEmail.ServiceDateTo.ToDateTime().ToString(StandardDateFormat);
                        bookingEmailRequest.ReasonType = schedulebookingEmailRequest.ReasonType.Reason;
                        bookingEmailRequest.Comment = schedulebookingEmailRequest.Comments;
                    }
                    if (bookingEmailRequest.StatusId == (int)AuditBookingStatus.Cancel)
                        bookingEmailRequest.StatusName = "Cancelled";
                    else if (bookingEmailRequest.StatusId == (int)AuditBookingStatus.Rescheduled)
                        bookingEmailRequest.StatusName = "Rescheduled";
                    else if (bookingEmailRequest.StatusId == (int)AuditBookingStatus.Received && isEdit.HasValue && isEdit.Value)
                        bookingEmailRequest.StatusName = "Modified";

                    // file path set for special note in confirmed e-mail
                    if (bookingEmailRequest.StatusId == (int)AuditBookingStatus.Confirmed)
                    {
                        var filepath = _ApplicationContext.AppBaseUrl;

                        var masterConfings = await GetMasterConfiguration();
                        var inspConfirmTermsEnglish = masterConfings.Where(x => x.Type == (int)EntityConfigMaster.AuditConfirmedEnglishFooter).Select(x => x.Value).FirstOrDefault();
                        var inspConfirmTermsChinese = masterConfings.Where(x => x.Type == (int)EntityConfigMaster.AuditConfirmedChineseFooter).Select(x => x.Value).FirstOrDefault();

                        bookingEmailRequest.InspConfirmEngDocPath = string.Concat(filepath, inspConfirmTermsEnglish);
                        bookingEmailRequest.InspConfirmCnDocPath = string.Concat(filepath, inspConfirmTermsChinese);

                        bookingEmailRequest.InspectionConfirmFooter = masterConfings.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.AuditConfirmedEnglishFooter && x.CountryId != (int)CountryEnum.China)?.Value;

                        bookingEmailRequest.InspectionConfirmFooter = bookingEmailRequest.InspectionConfirmFooter.Replace("{docpath}", bookingEmailRequest.InspConfirmEngDocPath);

                        if (bookingEmailRequest.IsChinaCountry)
                        {
                            bookingEmailRequest.InspectionConfirmChineseFooter = masterConfings.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.InspectionConfirmFooter && x.CountryId == (int)CountryEnum.China)?.Value;
                            bookingEmailRequest.InspectionConfirmChineseFooter = bookingEmailRequest.InspectionConfirmChineseFooter?.Replace("{docpath}", bookingEmailRequest.InspConfirmEngDocPath);
                        }

                    }

                    if (bookingEmailRequest.StatusId == (int)AuditBookingStatus.Rescheduled)
                    {
                        var masterConfings = await GetMasterConfiguration();

                        bookingEmailRequest.InspectionRescheduleEnglishFooter = masterConfings.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.AuditRescheduleEnglishFooter && x.CountryId != (int)CountryEnum.China)?.Value;

                        if (bookingEmailRequest.IsChinaCountry)
                        {
                            bookingEmailRequest.InspectionRescheduleChineseFooter = masterConfings.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.AuditRescheduleChineseFooter && x.CountryId == (int)CountryEnum.China)?.Value;
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

        public async Task<object> GetEaqfAuditBookingDetails(GetEaqfInspectionBookingRequest request)
        {
            var response = new GetEaqfAuditBookingResponse();
            try
            {
                //added custom validations if validation fails return response
                var errorResponse = await ValidateGetEaqfBooking(request);
                if (errorResponse != null)
                    return errorResponse;

                AuditSummarySearchRequest auditSummarySearchRequest = new AuditSummarySearchRequest();

                auditSummarySearchRequest.Index = request.Index;
                auditSummarySearchRequest.pageSize = request.pageSize;
                auditSummarySearchRequest.CustomerId = request.CustomerId;
                try
                {
                    auditSummarySearchRequest.ServiceTypeIdList = request.ServiceType.Split(",").Select(x => int.Parse(x)).ToList();
                }
                catch (Exception)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Service Type is Invalid" });
                }

                auditSummarySearchRequest.DateTypeid = (int)SearchType.ServiceDate;
                DateTime fromDate;
                DateTime toDate;
                if (DateTime.TryParseExact(request.ServiceFromDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                        DateTimeStyles.None, out fromDate))
                {
                    auditSummarySearchRequest.FromDate = new DateObject() { Year = fromDate.Year, Month = fromDate.Month, Day = fromDate.Day };
                }
                else
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceFromDate });
                }
                if (DateTime.TryParseExact(request.ServiceToDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                        DateTimeStyles.None, out toDate))
                {
                    auditSummarySearchRequest.ToDate = new DateObject() { Year = toDate.Year, Month = toDate.Month, Day = toDate.Day };
                }
                else
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceToDate });
                }

                if (fromDate > toDate)
                {
                    return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { GreterThanTodate });
                }

                var inspectionData = await SearchAuditSummary(auditSummarySearchRequest);

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
                var bookingIds = inspectionData.Data.Select(x => x.AuditId).ToList();

                var bookingLogStatusList = _eventLogRepo.GetAuditLogStatusByBooking(bookingIds);

                var eaqfBookingList = new List<EaqfAuditBookingItem>();
                foreach (var item in inspectionData.Data)
                {
                    string servicedate = string.Empty;
                    var bookingLogStatus = bookingLogStatusList.Where(x => x.BookingId == item.AuditId).ToList();

                    if (item.ServiceDateFrom == item.ServiceDateTo)
                    {
                        servicedate = item.ServiceDateTo;
                    }
                    else
                    {
                        servicedate = item.ServiceDateFrom + "-" + item.ServiceDateTo;
                    }

                    var actualStatusList = GetBookingReportStatus(item.AuditId, bookingLogStatus, item.StatusId, item.CreatedOnEaqf, servicedate, true);

                    var eaqfBooking = new EaqfAuditBookingItem()
                    {
                        BookingId = item.AuditId,
                        CustomerName = item.CustomerName,
                        ServiceType = item.ServiceType,
                        ServiceDateFrom = item.ServiceDateFrom,
                        ServiceDateTo = item.ServiceDateTo,
                        VendorName = item.SupplierName,
                        FactoryName = item.FactoryName,
                        FactoryState = item.FactoryState,
                        FactoryCity = item.FactoryCity,
                        FactoryCountry = item.FactoryCountry,                       
                        AuditStatus = actualStatusList,
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

        private List<BookingRepoStatus> GetBookingReportStatus(int bookingId, List<BookingLogStatus> bookingLogStatus,
            int? currentBookingStatus, string currentDate, string servicedate, bool isCallFromEAQF = false)
        {
            var standardDateFormatTime = isCallFromEAQF ? StandardISO8601DateTimeFormat : StandardDateFormat;

            List<BookingRepoStatus> statusList = new List<BookingRepoStatus>();
            statusList.Add(new BookingRepoStatus()
            {
                Id = 1,
                StatusId = (int)AuditBookingStatus.Received,
                StatusName = BookingStatusCustomList[1].ToString(),
                StatusDesc = "",
                IconType = BookingStatusCustomList[8].ToString(),
                StatusDate = currentDate

            });

            statusList.Add(new BookingRepoStatus()
            {
                Id = 2,
                StatusId = (int)AuditBookingStatus.Confirmed,
                StatusName = BookingStatusCustomList[2].ToString(),
                StatusDesc = "",
                IconType = BookingStatusCustomList[9].ToString(),
                StatusDate = bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.Confirmed).
                              OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)

            });

            statusList.Add(new BookingRepoStatus()
            {
                Id = 3,
                StatusId = (int)AuditBookingStatus.Scheduled,
                StatusName = BookingStatusCustomList[3].ToString(),
                StatusDesc = "",
                IconType = BookingStatusCustomList[9].ToString(),
                StatusDate = bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.AllocateQC).
                              OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)

            });

            statusList.Add(new BookingRepoStatus()
            {
                Id = 4,
                StatusId = (int)AuditBookingStatus.Audited,
                StatusName = BookingStatusCustomList[11].ToString(),
                StatusDesc = "",
                IconType = BookingStatusCustomList[9].ToString(),
                StatusDate = servicedate
            });

            //statusList.Add(new BookingRepoStatus()
            //{
            //    Id = 5,
            //    StatusId = (int)AuditBookingStatus.ReportSent,
            //    StatusName = BookingStatusCustomList[4].ToString(),
            //    StatusDesc = "",
            //    IconType = BookingStatusCustomList[9].ToString(),
            //    StatusDate = bookingLogStatus.Where(x => x.StatusId == (int)BookingStatus.ReportSent).
            //               OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)

            //});

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
            else if (currentBookingStatus == (int)AuditBookingStatus.Cancel)
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
                                StatusId = (int)AuditBookingStatus.Cancel,
                                StatusName = BookingStatusCustomList[6].ToString(),
                                StatusDesc = BookingStatusCustomList[10].ToString(),
                                IconType = BookingStatusCustomList[7].ToString(),
                                StatusDate = bookingLogStatus.Where(x => x.StatusId == (int)AuditBookingStatus.Cancel).
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
                            StatusId = (int)AuditBookingStatus.Cancel,
                            StatusName = BookingStatusCustomList[6].ToString(),
                            StatusDesc = BookingStatusCustomList[10].ToString(),
                            IconType = BookingStatusCustomList[7].ToString(),
                            StatusDate = 
                              bookingLogStatus.Where(x => x.StatusId == (int)AuditBookingStatus.Cancel).
                              OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate?.ToString(standardDateFormatTime)
                        });
                    }
                }
                else //if cancelled in requested status
                {
                    //set the booking received to done
                    var requestedStatus = statusList.Where(x => x.StatusId == (int)AuditBookingStatus.Received)?.FirstOrDefault();
                    requestedStatus.IconType = BookingStatusCustomList[8].ToString();
                    requestedStatus.StatusDesc = requestedStatus.StatusDate;
                    requestedStatus.StatusDate = requestedStatus.StatusDate ?? BookingStatusCustomList[8].ToString();

                    //set the second status at index 1 to cancel
                    statusList.Insert(1, new BookingRepoStatus()
                    {
                        StatusId = (int)AuditBookingStatus.Cancel,
                        StatusName = BookingStatusCustomList[6].ToString(),
                        StatusDesc = BookingStatusCustomList[10].ToString(),
                        IconType = BookingStatusCustomList[7].ToString(),
                        StatusDate = bookingLogStatus.Where(x => x.StatusId == (int)AuditBookingStatus.Cancel).
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

        public async Task<object> GetAuditEaqfReportBooking(string bookingIds)
        {
            var response = new GetEaqfAuditBookingReportResponse();
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

                var bookingData = await _auditRepository.GetAuditEaqfBookingReportDetails(bookingList);

                if (bookingData == null || !bookingData.Any())
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = BadRequest,
                        statusCode = HttpStatusCode.BadRequest,
                        data = bookingData
                    };
                }
                response.TotalCount = bookingData.Count();
                response.EaqfBookingReportData = bookingData;
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
    }
}
