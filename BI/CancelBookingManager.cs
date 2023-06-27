using BI.Cache;
using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.CancelBooking;
using DTO.Common;
using DTO.DataAccess;
using DTO.Eaqf;
using DTO.ExtraFees;
using DTO.Inspection;
using DTO.Invoice;
using DTO.MasterConfig;
using DTO.Quotation;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BI
{
    public class CancelBookingManager : ApiCommonData, ICancelBookingManager
    {
        private ICacheManager _cache = null;
        private readonly ICancelBookingRepository _cancelBookingRepository = null;
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly IQuotationManager _quotationManager = null;
        private readonly IScheduleManager _scheduleManager = null;
        private readonly IFBReportManager _fbReportManager = null;
        private readonly IUserRightsManager _userManager = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IQuotationRepository _quotRepo = null;
        private readonly ITenantProvider _filterService = null;
        private readonly BookingMap _bookingmap = null;
        private readonly LocationMap _locationmap = null;
        private readonly EAQFSettings _eaqfSettings = null;
        private readonly IHelper _helper = null;
        private readonly IInspectionBookingManager _inspManager = null;

        private readonly ISupplierManager _supplierManager = null;
        #region Constructor
        public CancelBookingManager(ICancelBookingRepository cancelBookingRepository, ICacheManager cache, IAPIUserContext applicationContext,
            IQuotationManager quotationManager, IScheduleManager scheduleManager, IInvoiceRepository invoiceRepository,
            IFBReportManager fBReportManager, IUserRightsManager userManager, IInspectionBookingRepository inspRepo, IQuotationRepository quotRepo,
            ITenantProvider filterService,
            IScheduleRepository scheduleRepository, IOptions<EAQFSettings> eaqfSettings, IHelper helper, IInspectionBookingManager inspManager, ISupplierManager supplierManager)
        {
            _cache = cache;
            _cancelBookingRepository = cancelBookingRepository;
            _applicationContext = applicationContext;
            _quotationManager = quotationManager;
            _scheduleManager = scheduleManager;
            _invoiceRepository = invoiceRepository;
            _fbReportManager = fBReportManager;
            _userManager = userManager;
            _inspRepo = inspRepo;
            _quotRepo = quotRepo;
            _filterService = filterService;
            _bookingmap = new BookingMap();
            _locationmap = new LocationMap();
            _eaqfSettings = eaqfSettings.Value;
            _helper = helper;
            _inspManager = inspManager;
            _supplierManager = supplierManager;
        }
        #endregion Constructor

        #region CancelBooking
        public async Task<BookingCancelRescheduleResponse> GetReason(int bookingId)
        {
            try
            {
                var response = new BookingCancelRescheduleResponse();
                var getBookingDetail = await _cancelBookingRepository.GetCancelBookingDetails(bookingId);
                if (getBookingDetail == null)
                    return new BookingCancelRescheduleResponse() { Result = BookingCancelRescheduleReasonsResult.CannotGetBookingDetail };
                var data = await _cancelBookingRepository.GetBookingCancelReasons(getBookingDetail.CustomerId);
                if (data == null || data.Count == 0)
                    return new BookingCancelRescheduleResponse() { Result = BookingCancelRescheduleReasonsResult.NotFound };
                response.ResponseList = data.Select(_bookingmap.GetBookingCancelReasons).ToArray();
                response.Result = BookingCancelRescheduleReasonsResult.Success;
                return response;
            }
            catch (Exception)
            {
                return new BookingCancelRescheduleResponse() { Result = BookingCancelRescheduleReasonsResult.NotFound };
            }
        }

        /// <summary>
        /// Save or update booking cancel details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveCancelBookingResponse> SaveBookingCancelDetails(SaveCancelBookingRequest request, string fbToken)
        {
            SaveCancelBookingResponse response = null;
            int quotationBookingCount = 0;
            try
            {
                //get the quotation id
                var quotationId = await _cancelBookingRepository.GetBookingQuotationDetails(request.BookingId);

                if (quotationId > 0)
                    //get the number of bookings involved in the quotation
                    quotationBookingCount = await _cancelBookingRepository.GetQuotationBookingCount(quotationId);

                //if more than one booking involved in the quotation then return error
                if (quotationBookingCount > 1)
                    return new SaveCancelBookingResponse() { Result = SaveCancelBookingResponseResult.RemoveMultiBookingQuotation };

                int id = 0;
                var entity = await _cancelBookingRepository.GetBookingDetailsById(request.BookingId);


                if (entity == null)
                    return new SaveCancelBookingResponse() { Result = SaveCancelBookingResponseResult.BookingNotFound };


                if (entity?.InspTranCancels?.Count > 0)
                {
                    var valueCancel = entity.InspTranCancels.FirstOrDefault();
                    id = await _cancelBookingRepository.EditCancel(_bookingmap.UpdateCancelEntity(request, valueCancel, _applicationContext.UserId));
                }
                else
                {
                    id = await _cancelBookingRepository.SaveCancelDetail(_bookingmap.SaveCancelEntity(request, _applicationContext.UserId));
                }

                if (id > 0)
                {
                    var reason = await _cancelBookingRepository.GetBookingCancelReasonsById(request.ReasonTypeId);
                    entity.StatusId = (int)BookingStatus.Cancel;
                    entity.Id = request.BookingId;
                    entity.InternalComments = entity.InternalComments + "Cancelled on " + DateTime.Now.ToString(StandardDateFormat) + " due to " + reason.Reason + " - " + request.Comment;
                    entity.InspTranStatusLogs.Add(_bookingmap.BookingStatusLogForCancel(request, entity, _applicationContext.UserId));
                    _cancelBookingRepository.Save(entity, true); //update booking status.

                    // update quotation part
                    var data = new SetStatusBusinessRequest();
                    var InvoiceData = new CancelInvoiceData();

                    if (quotationId > 0 && quotationBookingCount == 1)
                    {
                        var quotationData = await _cancelBookingRepository.GetQuotationData(quotationId);

                        if (quotationData != null)
                        {
                            data.Id = quotationData.Id;
                            data.CusComment = quotationData.CustomerRemark;
                            data.IdStatus = QuotationStatus.Canceled;
                            data.ApiRemark = quotationData.ApiRemark;
                            data.ApiInternalRemark = quotationData.ApiInternalRemark;

                        }
                    }

                    // delete invoice if the booking is mapped with invoice
                    var invoiceTransations = await _invoiceRepository.GetInvoiceListByBookingId(new[] { request.BookingId });

                    if (invoiceTransations != null)
                    {

                        var invoiceTransaction = invoiceTransations.FirstOrDefault();
                        var entityId = _filterService.GetCompanyId();
                        if (invoiceTransaction != null)
                        {
                            InvoiceData.InvoiceId = invoiceTransaction.Id;
                            InvoiceData.InvoiceNumber = invoiceTransaction.InvoicedName;
                            // when cancel the invoice and delete the invoice id mapping from extra fees and update the status
                            foreach (var extrafee in invoiceTransaction.InvExfTransactions.Where(x => x.InspectionId == request.BookingId && x.StatusId != (int)ExtraFeeStatus.Cancelled))
                            {
                                extrafee.InvoiceId = null;
                                extrafee.StatusId = (int)ExtraFeeStatus.Pending;
                                extrafee.UpdatedBy = _applicationContext.UserId;
                                extrafee.UpdatedOn = DateTime.Now;

                                extrafee.InvExfTranStatusLogs.Add(new InvExfTranStatusLog()
                                {
                                    CreatedBy = _applicationContext.UserId,
                                    CreatedOn = DateTime.Now,
                                    InspectionId = extrafee.InspectionId,
                                    StatusId = (int)ExtraFeeStatus.Pending,
                                    EntityId = entityId
                                });
                            }

                            invoiceTransaction.InvoiceStatus = (int)InvoiceStatus.Cancelled;

                            //remove the invoice transaction contacts
                            _invoiceRepository.RemoveEntities(invoiceTransations.SelectMany(x => x.InvAutTranContactDetails));

                            _invoiceRepository.EditEntities(invoiceTransations);

                            await _invoiceRepository.Save();

                            //return the invoice users with inspection cancel notification role
                            //get product category details
                            var productCategoryList = await _inspRepo.GetProductCategoryDetails(new[] { entity.Id });
                            //Get Department details
                            var departmentData = await _inspRepo.GetBookingDepartmentList(new[] { entity.Id });
                            //Get Brand details
                            var brandData = await _inspRepo.GetBookingBrandList(new[] { entity.Id });

                            //factory country 
                            int? factoryCountryId = null;
                            if (entity.FactoryId.HasValue)
                            {
                                var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(entity.FactoryId.Value);
                                if (factoryCountryData.Result == SupplierListResult.Success)
                                    factoryCountryId = factoryCountryData.countryId;
                            }

                            var userAccessFilter = new UserAccess
                            {
                                OfficeId = entity.OfficeId != null ? entity.OfficeId.Value : 0,
                                ServiceId = (int)Service.InspectionId,
                                CustomerId = entity.CustomerId,
                                RoleId = (int)RoleEnum.Accounting_InspectionCancel,
                                ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                                FactoryCountryId = factoryCountryId
                            };
                            InvoiceData.InvoiceCancelUserEmail = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);
                        }
                    }

                    int fbMissionId = 0;
                    await _fbReportManager.DeleteFBMission(request.BookingId, fbMissionId, fbToken);

                    await _scheduleManager.UpdateScheduleQcMandayOnCancelReschedule(request.BookingId, request.IsCancelKeepAllocatedQC);


                    response = new SaveCancelBookingResponse() { Result = SaveCancelBookingResponseResult.Success, QuotationData = data, CancelInvoiceData = InvoiceData, BookingType = entity.BookingType, IsEaqf = entity.IsEaqf.GetValueOrDefault() };
                }
                else
                    response = new SaveCancelBookingResponse() { Result = SaveCancelBookingResponseResult.BookingStatusNotUpdated };
            }


            catch (Exception ex)
            {
                return new SaveCancelBookingResponse() { Result = SaveCancelBookingResponseResult.CancelBookingNotAdded };
            }

            return response;
        }

        //quotation exists without cancel, user don't allow to cancel the booking
        private async Task<bool> isQuotationExists(int bookingId)
        {
            bool QuotationBookingId = false;
            QuInspProduct quotationDetails = await _cancelBookingRepository.BookingQuotationExists(bookingId);
            if (quotationDetails == null)
            {
                QuotationBookingId = true;
            }
            return QuotationBookingId;
        }

        /// <summary>
        /// Delete invoice by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        private async Task<int> DeleteInvoiceByBooking(int bookingId)
        {
            var invoiceTransations = await _invoiceRepository.GetInvoiceListByBookingId(new[] { bookingId });
            if (invoiceTransations != null)
            {
                var invoiceTransation = invoiceTransations.FirstOrDefault();
                var entityId = _filterService.GetCompanyId();
                // when cancel the invoice and delete the invoice id mapping from extra fees and update the status
                foreach (var extrafee in invoiceTransation.InvExfTransactions.Where(x => x.InspectionId == bookingId && x.StatusId != (int)ExtraFeeStatus.Cancelled))
                {
                    extrafee.InvoiceId = null;
                    extrafee.StatusId = (int)ExtraFeeStatus.Pending;
                    extrafee.UpdatedBy = _applicationContext.UserId;
                    extrafee.UpdatedOn = DateTime.Now;

                    extrafee.InvExfTranStatusLogs.Add(new InvExfTranStatusLog()
                    {
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        InspectionId = extrafee.InspectionId,
                        StatusId = (int)ExtraFeeStatus.Pending,
                        EntityId = entityId,
                    });
                }

                invoiceTransation.InvoiceStatus = (int)InvoiceStatus.Cancelled;

                //remove the invoice transaction contacts
                _invoiceRepository.RemoveEntities(invoiceTransations.SelectMany(x => x.InvAutTranContactDetails));

                _invoiceRepository.EditEntities(invoiceTransations);

                await _invoiceRepository.Save();
            }

            return 1;
        }


        /// <summary>
        /// delete quotation by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>

        private async Task<int> DeleteQuotationByBooking(int bookingId)
        {
            // GetBookingQuotationDetails
            var quotationId = await _cancelBookingRepository.GetBookingQuotationDetails(bookingId);
            if (quotationId > 0)
            {
                var quotationData = await _cancelBookingRepository.GetQuotationData(quotationId);

                if (quotationData != null)
                {
                    if (quotationData.QuQuotationInsps.Count > 1) // more than one booking
                    {
                        _invoiceRepository.RemoveEntities(quotationData.QuQuotationInsps);
                    }
                    else if (quotationData.QuQuotationInsps.Count == 1)
                    {
                        _invoiceRepository.RemoveEntities(quotationData.QuQuotationInsps);
                        quotationData.IdStatus = (int)QuotationStatus.Canceled;
                    }

                    _invoiceRepository.EditEntity(quotationData);

                    await _invoiceRepository.Save();
                }
            }

            return 1;
        }


        public async Task<SaveCancelBookingRequest> GetCancelDetail(int bookingId)
        {
            try
            {
                var response = new SaveCancelBookingRequest();
                var cancelData = await _cancelBookingRepository.GetCancelDetailsById(bookingId);
                if (cancelData == null)
                    return null;
                else
                    response = _bookingmap.EditCancelEntity(cancelData);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion CancelBooking
        #region RescheduleBooking
        public async Task<BookingCancelRescheduleResponse> GetRescheduleReason(int bookingId)
        {
            try
            {
                var response = new BookingCancelRescheduleResponse();
                var getBookingDetail = await _cancelBookingRepository.GetCancelBookingDetails(bookingId);
                if (getBookingDetail == null)
                    return new BookingCancelRescheduleResponse() { Result = BookingCancelRescheduleReasonsResult.CannotGetBookingDetail };
                var data = await _cancelBookingRepository.GetBookingRescheduleReasons(getBookingDetail.CustomerId);
                if (data == null || data.Count == 0)
                    return new BookingCancelRescheduleResponse() { Result = BookingCancelRescheduleReasonsResult.NotFound };
                response.ResponseList = data.Select(_bookingmap.GetBookingRescheduleReasons).ToArray();
                response.Result = BookingCancelRescheduleReasonsResult.Success;
                return response;
            }
            catch (Exception)
            {
                return new BookingCancelRescheduleResponse() { Result = BookingCancelRescheduleReasonsResult.NotFound };
            }
        }
        public async Task<SaveRescheduleResponse> SaveRescheduleDetails(SaveRescheduleRequest request, string fbToken)
        {
            var response = new SaveRescheduleResponse();
            bool isKeepQCForTravelExpense = false;
            try
            {
                int id = 0;
                int userId = request.UserId > 0 ? request.UserId : _applicationContext.UserId;
                var entity = await _cancelBookingRepository.GetBookingDetailsById(request.BookingId);
                if (entity == null)
                    return new SaveRescheduleResponse() { Result = SaveRescheduleResult.BookingNotFound };

                var serviceFromDate = request.ServiceFromDate.ToNullableDateTime();
                var fromDate = serviceFromDate.HasValue ? serviceFromDate.Value.ToString(StandardDateFormat) : string.Empty;

                var serviceToDate = request.ServiceToDate.ToNullableDateTime();
                var toDate = serviceToDate.HasValue ? serviceToDate.Value.ToString(StandardDateFormat) : string.Empty;

                var firstServiceFromDate = request.FirstServiceDateFrom.ToNullableDateTime();
                var firstFromDate = firstServiceFromDate.HasValue ? firstServiceFromDate.Value.ToString(StandardDateFormat) : string.Empty;

                var firstServiceToDate = request.FirstServiceDateTo.ToNullableDateTime();
                var firstToDate = firstServiceToDate.HasValue ? firstServiceToDate.Value.ToString(StandardDateFormat) : string.Empty;

                //service from date not formated
                if (!DateTime.TryParseExact(fromDate.ToString(), StandardDateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime SerFromDate))
                {
                    return new SaveRescheduleResponse() { Result = SaveRescheduleResult.ServiceFromDateFormatWasWrong };
                }
                //service to date not formated
                if (!DateTime.TryParseExact(toDate.ToString(), StandardDateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime SerToDate))
                {
                    return new SaveRescheduleResponse() { Result = SaveRescheduleResult.ServiceToDateFormatWasWrong };
                }

                //service from date not formated
                if (!DateTime.TryParseExact(firstFromDate.ToString(), StandardDateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime firstSerFromDate))
                {
                    return new SaveRescheduleResponse() { Result = SaveRescheduleResult.ServiceFromDateFormatWasWrong };
                }
                //service to date not formated
                if (!DateTime.TryParseExact(firstToDate.ToString(), StandardDateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime firstSerToDate))
                {
                    return new SaveRescheduleResponse() { Result = SaveRescheduleResult.ServiceToDateFormatWasWrong };
                }

                if (entity?.InspTranReschedules?.Count > 0)
                {
                    var valueReschedule = entity.InspTranReschedules.FirstOrDefault();
                    id = await _cancelBookingRepository.EditReschedule(_bookingmap.UpdateRescheduleEntity(request, valueReschedule, userId));
                }
                else
                    id = _cancelBookingRepository.SaveReschedule(_bookingmap.SaveRescheduleEntity(request, userId));

                if (id > 0)
                {

                    var entityBooking = new InspTransaction()
                    {
                        ServiceDateFrom = entity.ServiceDateFrom,
                        ServiceDateTo = entity.ServiceDateTo
                    };

                    var bookingInfo = new BookingDateInfo()
                    {
                        BookingId = request.BookingId,
                        ServiceFromDate = request.ServiceFromDate,
                        ServiceToDate = request.ServiceToDate
                    };

                    //keep existing QC & report checker in schdule table while reschdule booking
                    if (request.IsKeepAllocatedQC)
                    {
                        //if rescheduled serviceToDate is less that the previous serviceToDate
                        if (request.ServiceToDate.ToDateTime() < entity.ServiceDateTo)
                        {
                            await _quotationManager.UpdateQuotationServiceDate(bookingInfo, entity);

                            await _scheduleManager.UpdateScheduleOnRescheduleToLesserDate(bookingInfo, entity);
                        }

                        else
                        {
                            await _quotationManager.UpdateQuotationServiceDateReschdule(bookingInfo, entityBooking, userId);
                        }
                    }
                    else
                    {
                        //schedule report checker  & QC as inactive
                        if (entity.StatusId == (int)BookingStatus.AllocateQC && entity.Id > 0)
                        {
                            await _scheduleManager.UpdateScheduleOnReschedule(entity.Id);

                            await _scheduleManager.UpdateScheduleQcMandayOnCancelReschedule(request.BookingId, isKeepQCForTravelExpense);
                        }

                        //update quotation service date 
                        await _quotationManager.UpdateQuotationServiceDate(bookingInfo, entityBooking);
                    }

                    //Fetch the REschedule reasons for the booking
                    var reasonList = await GetRescheduleReason(request.BookingId);
                    //Filter the reasons to fetch the current reason
                    var reason = reasonList.ResponseList.Where(x => x.Id == request.ReasonTypeId).Select(x => x.Reason).FirstOrDefault();
                    //Form the date string for the comment. if multiple dates seperate with - else specify one date
                    var date = request.ServiceFromDate.ToDateTime() == request.ServiceToDate.ToDateTime() ? request.ServiceFromDate.ToDateTime().ToString(StandardDateFormat) : request.ServiceFromDate.ToDateTime().ToString(StandardDateFormat) + " - " + request.ServiceToDate.ToDateTime().ToString(StandardDateFormat);
                    //Map the internal comments
                    entity.InternalComments = entity.InternalComments + "Rescheduled on " + DateTime.Now.ToString(StandardDateFormat) + " due to " + reason + " (Service Date - " + date + " )\n";

                    //update booking status and service date.
                    _bookingmap.UpdateBookingStatusServiceDate(request, userId, entity);
                    //Adding status log for inspection booking
                    entity.InspTranStatusLogs.Add(_bookingmap.BookingStatusLog(request, entity, userId));

                    //update the rescheduled date in FB
                    if (entity.FbMissionId != null)
                    {
                        await _fbReportManager.UpdateFBBookingDetails(request.BookingId, fbToken);
                    }

                    _cancelBookingRepository.Save(entity, true);



                    return new SaveRescheduleResponse() { Result = SaveRescheduleResult.Success, BookingType = entity.BookingType, IsEaqf = entity.IsEaqf.GetValueOrDefault() };
                }
                else
                    return new SaveRescheduleResponse() { Result = SaveRescheduleResult.BookingStatusNotUpdated };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SaveRescheduleRequest> GetRescheduleDetail(int bookingId)
        {
            try
            {
                var response = new SaveRescheduleRequest();
                var data = await _cancelBookingRepository.GetRescheduleDetailsById(bookingId);
                if (data == null)
                    return null;
                else
                    response = _bookingmap.EditRescheduleEntity(data);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion RescheduleBooking
        #region CancelRescheduleBooking
        //Get currency to load drop down value
        public async Task<CurrencyResponse> GetCurrency()
        {
            try
            {
                var response = new CurrencyResponse();
                var currencies = await _cache.CacheTryGetValueSet(CacheKeys.AllCurrencies,
             () => _cancelBookingRepository.GetCurrencies());
                if (currencies == null || !currencies.Any())
                    return new CurrencyResponse() { Result = CurrencyResult.CurrencyNotFound };
                response.CurrencyList = currencies.Select(_locationmap.GetCurrency).ToArray();
                response.Result = CurrencyResult.Success;
                return response;
            }
            catch (Exception)
            {
                return new CurrencyResponse() { Result = CurrencyResult.CurrencyNotFound };
            }
        }
        public async Task<EditCancelBookingResponse> GetCancelBookingDetails(int bookingId, int bookingStatus)
        {
            try
            {
                var response = new EditCancelBookingResponse();
                int quotationBookingsCount = 0; //to check if the quotation of this booking has more than one booking
                var data = await _cancelBookingRepository.GetCancelBookingDetails(bookingId);
                var isRescheduleBooking = await RescheduleBookingWithOutCancelQuotation();
                if (data == null)
                    return new EditCancelBookingResponse() { Result = CancelBookingResponseResult.CannotGetBookingDetails };

                if (bookingStatus == (int)BookingStatus.Rescheduled)
                {
                    response.RescheduleItem = await GetRescheduleDetail(bookingId);
                    quotationBookingsCount = await _quotRepo.GetBookingsByQuotation(bookingId);
                }

                else if (bookingStatus == (int)BookingStatus.Cancel)
                    response.CancelItem = await GetCancelDetail(bookingId);
                response.ItemDetails = _bookingmap.GetCancelBookingItem(data, quotationBookingsCount, isRescheduleBooking);
                response.Result = CancelBookingResponseResult.success;
                return response;
            }
            catch (Exception)
            {
                return new EditCancelBookingResponse() { Result = CancelBookingResponseResult.CannotGetBookingDetails };
            }
        }

        public async Task<SaveCancelBookingRequest> GetCancelDetails(int bookingId)
        {
            return await GetCancelDetail(bookingId);
        }

        public async Task<SaveRescheduleRequest> GetRescheduleDetails(int bookingId)
        {
            return await GetRescheduleDetail(bookingId);
        }
        private async Task<bool> RescheduleBookingWithOutCancelQuotation()
        {
            var masterConfings = await _inspManager.GetMasterConfiguration();
            return masterConfings.Any(x => x.Type == (int)EntityConfigMaster.RescheduleTheBookingWithoutCancelTheQuotation);
        }
        #endregion CancelRescheduleBooking        
    }
}
