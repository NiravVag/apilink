using Contracts.Managers;
using Contracts.Repositories;
using DTO.Invoice;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BI.Maps;
using Entities.Enums;
using DTO.RepoRequest.Enum;
using DTO.Report;
using DTO.Common;
using Entities;
using DTO.Inspection;
using DTO.CommonClass;
using DTO.RepoRequest.Audit;
using Microsoft.Extensions.Configuration;
using DTO.ExtraFees;

namespace BI
{
    public class InvoiceStatusManager : ApiCommonData, IInvoiceStatusManager
    {
        private readonly IInvoiceStatusRepository _repository = null;
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly IInspectionBookingRepository _inspRepository = null;
        private readonly InvoiceStatusSummaryMap _invoicestatus = null;
        private readonly IAuditRepository _auditRepository = null;
        private readonly IInvoiceDataAccessManager _invoiceDataAccessManager = null;
        private readonly AuditMap _auditmap = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private static IConfiguration _configuration = null;
        private ITenantProvider _filterService = null;
        private IHumanResourceRepository _humanResourceRepository;

        public InvoiceStatusManager(IInvoiceStatusRepository repository, IInvoiceRepository invoiceRepository,
        IInspectionBookingManager inspectionBookingManager, IInspectionBookingRepository inspRepository,
        IAPIUserContext ApplicationContext, IInvoiceDataAccessManager invoiceDataAccessManager, IHumanResourceRepository humanResourceRepository,
        IAuditRepository auditRepository, IConfiguration configuration, ITenantProvider filterService)
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _inspRepository = inspRepository;
            _invoicestatus = new InvoiceStatusSummaryMap();
            _auditRepository = auditRepository;
            _auditmap = new AuditMap();
            _ApplicationContext = ApplicationContext;
            _invoiceDataAccessManager = invoiceDataAccessManager;
            _configuration = configuration;
            _filterService = filterService;
            _humanResourceRepository = humanResourceRepository;
        }
        /// <summary>
        /// invoice booking and audit Status summary
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<InvoiceStatusSummaryResponse> GetInvoiceStatusSummary(InvoiceStatusSummaryRequest requestDto)
        {
            if (requestDto == null)
                return new InvoiceStatusSummaryResponse() { Result = InvoiceStatusResult.NotFound };

            var data = await GetAllInvoiceStatusSummary(requestDto);

            if (data.invoiceStatusDataList == null || !data.invoiceStatusDataList.Any())
            {
                return new InvoiceStatusSummaryResponse() { Result = InvoiceStatusResult.NotFound };
            }

            var bangladesh_BankId = _configuration["Bangladesh_BankId"].ToString();

            var result = _invoicestatus.MapInvoiceStatusBookingSummary(data);

            var response = new InvoiceStatusSummaryResponse()
            {
                Result = InvoiceStatusResult.Success,
                InvoiceStatuslst = data.InvoiceInspectionStatusList,
                InvoiceAuditStatusList = data.InvoiceAuditStatusList,
                Data = result,
                TotalCount = data.TotalCount,
                PageSize = requestDto.pageSize.Value,
                Index = requestDto.Index.Value,
                Bangladesh_BankId = bangladesh_BankId,
                PageCount = (result.Count() / requestDto.pageSize.Value) + (result.Count() % requestDto.pageSize.Value > 0 ? 1 : 0)
            };

            return response;
        }
        /// <summary>
        /// get All invoice Status 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<InvoiceStatusSummaryResult> GetAllInvoiceStatusSummary(InvoiceStatusSummaryRequest requestDto)
        {
            var response = new InvoiceStatusSummaryResult();
            var inspectionResult = new InvoiceInspSummary();
            var auditResult = new InvoiceAudSummary();
            IEnumerable<ServiceTypeList> serviceTypeList = Enumerable.Empty<ServiceTypeList>();
            List<InvoiceBookingQuotation> quotationList = null;

            if (requestDto.Index == null || requestDto.Index.Value <= 0)
                requestDto.Index = 1;

            if (requestDto.pageSize == null || requestDto.pageSize.Value == 0)
                requestDto.pageSize = 10;

            int skip = (requestDto.Index.Value - 1) * requestDto.pageSize.Value;

            if (requestDto.ServiceId == (int)Service.InspectionId)
            {
                inspectionResult = await GetInspectionDetails(requestDto, skip);
            }
            else if (requestDto.ServiceId == (int)Service.AuditId)
            {
                auditResult = await GetAuditDetails(requestDto, skip);
            }

            if (requestDto.ServiceId == (int)Service.InspectionId)
            {
                if (inspectionResult == null)
                {
                    response.invoiceStatusDataList = null;
                    return response;
                }
                response.invoiceStatusDataList = inspectionResult.BookingDataList;

                //fetch the booking and invoice details
                var _bookingIds = inspectionResult.BookingDataList.Select(x => x.BookingId.GetValueOrDefault()).ToList();

                quotationList = await _invoiceRepository.GetQueryableBookingQuotationDetails(_bookingIds);


                response.serviceTypeList = serviceTypeList.Where(x => inspectionResult.BookingDataList.Select(y => y.BookingId).Contains(x.InspectionId)).ToList();

                response.TotalCount = inspectionResult.TotalCount;

                var bookingIds = response.invoiceStatusDataList.Select(x => x.BookingId).ToList();

                response.InvoiceList = await _repository.GetInspBookingInvoiceList(bookingIds);

                response.ExtraFeesInvoiceList = await _repository.GetExtraFeeInvoiceList(bookingIds);

                response.ServiceId = (int)Service.InspectionId;

                response.InspectionHoldReasons = await _inspRepository.GetInspectionHoldReasons(_bookingIds);

                //get brand data
                response.BookingBrandList = await _inspRepository.GetBrandBookingIdsByBookingIds(_bookingIds);

                var invoiceInspectionStatusList = await GetInvoiceInpectionStatusList(inspectionResult.InspTransaction);
                // Apply status color
                invoiceInspectionStatusList.ForEach(x => x.StatusColor = BookingSummaryInspectionStatusColor.GetValueOrDefault(x.StatusId, ""));

                response.InvoiceInspectionStatusList = invoiceInspectionStatusList;
            }
            else if (requestDto.ServiceId == (int)Service.AuditId)
            {
                if (auditResult == null)
                {
                    response.invoiceStatusDataList = null;
                    return response;
                }

                //fetch the Audit and invoice details
                var _AuditIds = auditResult.AudTransaction.Select(x => x.Id);
                quotationList = await _invoiceRepository.GetQueryableAuditBookingQuotationDetails(_AuditIds);
                response.ExtraFeesInvoiceList = await _repository.GetExtraFeeInvoiceListByAuditIds(_AuditIds);
                response.invoiceStatusDataList = auditResult.BookingDataList;
                response.serviceTypeList = serviceTypeList.Where(x => auditResult.BookingDataList.Select(y => y.BookingId).Contains(x.InspectionId)).ToList();
                //response.quotationList = quotationList.Where(x => auditResult.BookingDataList.Select(y => y.BookingId).Contains(x.BookingNo)).ToList();

                response.InvoiceList = await _repository.GetAuditBookingInvoiceList(response.invoiceStatusDataList.Select(x => x.BookingId).ToList());
                response.TotalCount = auditResult.TotalCount;

                response.ServiceId = (int)Service.AuditId;

                var invoiceAuditStatusList = await GetInvoiceAuditStatusList(auditResult.AudTransaction);
                var _statuslist = invoiceAuditStatusList.Select(_auditmap.GetInvoiceAuditStatus);
                response.InvoiceAuditStatusList = _statuslist.ToList();
            }
            else if (requestDto.ServiceId == (int)Service.Tcf)
            {
                response.invoiceStatusDataList = null;
                return response;
            }



            if (quotationList != null && quotationList.Any())
            {
                var bankIdList = quotationList.Select(x => x.BankId).ToList();

                var taxList = await _invoiceRepository.GetBankValidTaxDetails(bankIdList);

                // update tax list by bank
                foreach (var item in quotationList)
                {
                    item.TaxList = taxList.Where(x => x.BankId == item.BankId).ToList();
                }

                var supplierIdList = quotationList.Select(x => x.SupplierId).Distinct().ToList();
                var factoryIdList = quotationList.Select(x => x.FactoryId).Distinct().ToList();

                response.FactoryContactIdList = await _invoiceRepository.GetFactoryContactIdList(factoryIdList);
                response.SupplierAddressIdList = await _invoiceRepository.GetSupplierAddressList(supplierIdList);
                response.SupplierContactIdList = await _invoiceRepository.GetSupplierContactIdList(supplierIdList);

                response.quotationList = quotationList.ToList();
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        private async Task<InvoiceAudSummary> GetAuditDetails(InvoiceStatusSummaryRequest requestDto, int skip)
        {
            var response = new InvoiceAudSummary();

            var audQueryData = _repository.GetAuditDetailsQuery();

            if (_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.Accounting))
            {
                //get invoice data access data based on staff id
                var invoiceDataAccess = await _invoiceDataAccessManager.GetStaffInvoiceDataAcesss(_ApplicationContext.StaffId);
                if (invoiceDataAccess == null)
                {
                    //if invoice data access is not available then record not found of invoice status
                    return null;
                }

                if (invoiceDataAccess != null)
                {
                    if (invoiceDataAccess.CustomerIds.Any())
                        audQueryData = audQueryData.Where(x => invoiceDataAccess.CustomerIds.Contains(x.CustomerId));

                    if (invoiceDataAccess.OfficeIds.Any())
                        audQueryData = audQueryData.Where(x => x.OfficeId.HasValue && invoiceDataAccess.OfficeIds.Contains(x.OfficeId.Value));

                    if (invoiceDataAccess.InvoiceTypes.Any())
                        audQueryData = audQueryData.Where(x => x.QuQuotationAudits.Any(y => y.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled) ?
                        x.QuQuotationAudits.Any(z => z.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled && z.IdQuotationNavigation.PaymentTerms.HasValue && invoiceDataAccess.InvoiceTypes.Contains(z.IdQuotationNavigation.PaymentTerms.Value))
                        : invoiceDataAccess.InvoiceTypes.Contains((int)INVInvoiceType.Monthly));
                }
            }


            //filter based on service date from and to / invoice date
            if (Enum.TryParse(requestDto.DateTypeId.ToString(), out SearchType _datesearchtype))
            {
                if (requestDto.InvoiceFromDate?.ToDateTime() != null && requestDto.InvoiceToDate?.ToDateTime() != null)
                {
                    switch (_datesearchtype)
                    {
                        case SearchType.ServiceDate:
                            {
                                audQueryData = audQueryData.Where(x => !((x.ServiceDateFrom.Date > requestDto.InvoiceToDate.ToDateTime().Date) || (x.ServiceDateTo.Date < requestDto.InvoiceFromDate.ToDateTime().Date)));
                                break;
                            }
                    }
                }
            }

            //filter by customerId
            if (requestDto.CustomerId > 0)
            {
                audQueryData = audQueryData.Where(x => x.CustomerId == requestDto.CustomerId);
            }

            //filter by supplierId
            if (requestDto.SupplierId > 0)
            {
                audQueryData = audQueryData.Where(x => x.SupplierId == requestDto.SupplierId);
            }

            //filter by factoryId
            if (requestDto.FactoryId > 0)
            {
                audQueryData = audQueryData.Where(x => x.FactoryId == requestDto.FactoryId);
            }

            //filter by Booking Id
            if (requestDto.SearchTypeId == (int)SearchType.BookingNo)
            {
                if (!string.IsNullOrEmpty(requestDto.SearchTypeText?.Trim()) && int.TryParse(requestDto.SearchTypeText?.Trim(), out int bookid))
                {
                    audQueryData = audQueryData.Where(x => x.Id == bookid);
                }
            }

            if (requestDto.StatusIdlst != null && requestDto.StatusIdlst.Any())
            {
                audQueryData = audQueryData.Where(x => requestDto.StatusIdlst.ToList().Contains(x.StatusId));
            }

            var officeControls = await _humanResourceRepository.GetOfficesByStaff(_ApplicationContext.StaffId);
            var officeIds = officeControls.Select(x => x.LocationId).ToList();

            //apply office list filter
            if (requestDto.OfficeIdList != null && requestDto.OfficeIdList.Any())
            {
                if (officeIds.Any())
                {
                    requestDto.OfficeIdList = requestDto.OfficeIdList.Where(x => officeIds.Contains(x.GetValueOrDefault()));
                }
                audQueryData = audQueryData.Where(x => x.OfficeId != null && requestDto.OfficeIdList.Contains(x.OfficeId.Value));
            }
            else
            {
                if (officeIds.Any())
                    audQueryData = audQueryData.Where(x => officeIds.Contains(x.OfficeId.GetValueOrDefault()));
            }

            //apply factory country list filter
            if (requestDto.FactoryCountryIds != null && requestDto.FactoryCountryIds.Any())
            {
                audQueryData = audQueryData.Where(x => x.Factory.SuAddresses.Any(y => y.AddressTypeId == (int)RefAddressTypeEnum.HeadOffice && requestDto.FactoryCountryIds.Contains(y.CountryId)));
            }

            var idList = audQueryData.Select(x => x.Id);

            //filter by invoice type id
            if (requestDto.InvoiceType > 0)
            {
                if (requestDto.InvoiceType == (int)INVInvoiceType.PreInvoice)
                {
                    //quotation type "pre-invoice" and quotation status "customer validated"
                    audQueryData = audQueryData.Where(z => z.QuQuotationAudits.Any(x => x.IdQuotationNavigation.PaymentTerms
                            == (int)INVInvoiceType.PreInvoice && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated));
                }
                else if (requestDto.InvoiceType == (int)INVInvoiceType.Monthly)
                {
                    //booking status inspected (check the collection of inspected status)
                    audQueryData = audQueryData.Where(x => AuditedStatusList.Contains(x.StatusId));

                    //quotation type "monthly invoice" 
                    audQueryData = audQueryData.Where(z => z.QuQuotationAudits.Any(x => x.IdQuotationNavigation.PaymentTerms
                                    == (int)INVInvoiceType.Monthly) || !z.QuQuotationAudits.Any());
                }
            }

            if (requestDto.InvoiceStatusId != null && requestDto.InvoiceStatusId.Any())
            {
                //actual status list
                requestDto.ActualInvoiceStatusId = ActualInvoiceStatusIds;

                //if invoiced and pending selected or invoiced selected or pending selected 
                if (requestDto.InvoiceStatusId.Contains((int)InvoiceStatusSummaryStatusList.Invoiced) &&
                  requestDto.InvoiceStatusId.Contains((int)InvoiceStatusSummaryStatusList.Pending))
                {
                    audQueryData = audQueryData.Where(x => x.InvAutTranDetails.Any(y => requestDto.ActualInvoiceStatusId.Contains(y.InvoiceStatus.GetValueOrDefault())) ||
                                            !x.InvAutTranDetails.Any());

                    audQueryData = audQueryData.OrderByDescending(x => !x.InvAutTranDetails.Any());

                }
                else if (requestDto.InvoiceStatusId.Contains((int)InvoiceStatusSummaryStatusList.Invoiced))
                {
                    audQueryData = audQueryData.Where(x => x.InvAutTranDetails.Any(y => requestDto.ActualInvoiceStatusId.Contains(y.InvoiceStatus.GetValueOrDefault())));
                }
                else if (requestDto.InvoiceStatusId.Contains((int)InvoiceStatusSummaryStatusList.Pending))
                {
                    audQueryData = audQueryData.Where(x => !x.InvAutTranDetails.Any());
                }
            }
            else
            {
                audQueryData = audQueryData.OrderByDescending(x => !x.InvAutTranDetails.Any());
            }

            //payment status id filter
            if (requestDto.PaymentStatusIdList != null && requestDto.PaymentStatusIdList.Any())
            {
                audQueryData = audQueryData.Where(x => x.InvAutTranDetails.Any(y => requestDto.PaymentStatusIdList.Contains(y.InvoicePaymentStatus)));
            }
            response.TotalCount = await audQueryData.CountAsync();

            var invoiceList = await audQueryData.Skip(skip).Take(requestDto.pageSize.GetValueOrDefault()).Select(x => new InvoiceStatusSummaryItem()
            {
                BookingId = x.Id,
                BookingServiceDateFrom = x.ServiceDateFrom,
                BookingServiceDateTo = x.ServiceDateTo,
                CustomerId = x.CustomerId,
                SupplierId = x.SupplierId,
                FactoryId = x.FactoryId,
                CustomerName = x.Customer.CustomerName,
                SupplierName = x.Supplier.SupplierName,
                FactoryName = x.Factory.SupplierName,
                StatusName = x.Status.Status,
                StatusId = x.StatusId
            }).OrderByDescending(x => x.BookingId).AsNoTracking().ToListAsync();

            response.AudTransaction = audQueryData;

            response.BookingDataList = invoiceList;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        private async Task<InvoiceInspSummary> GetInspectionDetails(InvoiceStatusSummaryRequest requestDto, int skip)
        {
            var response = new InvoiceInspSummary();

            //get invoice data access data based on staff id            




            var inspQueryData = _repository.GetBookingDetailsQuery();

            if (_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.Accounting))
            {
                var invoiceDataAccess = await _invoiceDataAccessManager.GetStaffInvoiceDataAcesss(_ApplicationContext.StaffId);
                if (invoiceDataAccess == null)
                {
                    //if invoice data access is not available then record not found of invoice status
                    return null;
                }
                if (invoiceDataAccess != null)
                {
                    // if customer configured then show based on customer, if any customer is not configured then show all data
                    if (invoiceDataAccess.CustomerIds.Any())
                        inspQueryData = inspQueryData.Where(x => invoiceDataAccess.CustomerIds.Contains(x.CustomerId));

                    // if office configured then show based on office, if any office is not configured then show all data
                    if (invoiceDataAccess.OfficeIds.Any())
                        inspQueryData = inspQueryData.Where(x => x.OfficeId.HasValue && invoiceDataAccess.OfficeIds.Contains(x.OfficeId.Value));

                    // if invoice types configured then show based on quotation invoice payment terms, if any invoice type is not configured then show all data
                    //and if Quotation is not avaiable then monthly invoice consider
                    if (invoiceDataAccess.InvoiceTypes.Any())
                        inspQueryData = inspQueryData.Where(x => x.QuQuotationInsps.Any(y => y.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled) ?
                        x.QuQuotationInsps.Any(z => z.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled && z.IdQuotationNavigation.PaymentTerms.HasValue && invoiceDataAccess.InvoiceTypes.Contains(z.IdQuotationNavigation.PaymentTerms.Value))
                        : invoiceDataAccess.InvoiceTypes.Contains((int)INVInvoiceType.Monthly));
                }
            }

            //filter based on service date from and to / invoice date
            if (Enum.TryParse(requestDto.DateTypeId.ToString(), out SearchType _datesearchtype))
            {
                if (requestDto.InvoiceFromDate?.ToDateTime() != null && requestDto.InvoiceToDate?.ToDateTime() != null)
                {
                    switch (_datesearchtype)
                    {
                        case SearchType.ServiceDate:
                            {
                                inspQueryData = inspQueryData.Where(x => !((x.ServiceDateFrom.Date > requestDto.InvoiceToDate.ToDateTime().Date) || (x.ServiceDateTo.Date < requestDto.InvoiceFromDate.ToDateTime().Date)));
                                break;
                            }
                    }
                }
            }

            //filter by customerId
            if (requestDto.CustomerId > 0)
            {
                inspQueryData = inspQueryData.Where(x => x.CustomerId == requestDto.CustomerId);
            }

            //filter by supplierId
            if (requestDto.SupplierId > 0)
            {
                inspQueryData = inspQueryData.Where(x => x.SupplierId == requestDto.SupplierId);
            }

            //filter by factoryId
            if (requestDto.FactoryId > 0)
            {
                inspQueryData = inspQueryData.Where(x => x.FactoryId == requestDto.FactoryId);
            }

            //filter by Booking Id
            if (requestDto.SearchTypeId == (int)SearchType.BookingNo)
            {
                if (!string.IsNullOrEmpty(requestDto.SearchTypeText?.Trim()) && int.TryParse(requestDto.SearchTypeText?.Trim(), out int bookid))
                {
                    inspQueryData = inspQueryData.Where(x => x.Id == bookid);
                }
            }

            if (requestDto.StatusIdlst != null && requestDto.StatusIdlst.Any())
            {
                inspQueryData = inspQueryData.Where(x => requestDto.StatusIdlst.ToList().Contains(x.StatusId));
            }

            var officeControls = await _humanResourceRepository.GetOfficesByStaff(_ApplicationContext.StaffId);
            var officeIds = officeControls.Select(x => x.LocationId).ToList();
            //apply office list filter
            if (requestDto.OfficeIdList != null && requestDto.OfficeIdList.Any())
            {
                if (officeIds.Any())
                {
                    requestDto.OfficeIdList = requestDto.OfficeIdList.Where(x => officeIds.Contains(x.GetValueOrDefault()));
                }
                inspQueryData = inspQueryData.Where(x => x.OfficeId != null && requestDto.OfficeIdList.Contains(x.OfficeId.Value));
            }
            else
            {
                if (officeIds.Any())
                    inspQueryData = inspQueryData.Where(x => officeIds.Contains(x.OfficeId.GetValueOrDefault()));
            }


            //apply factory country list filter
            if (requestDto.FactoryCountryIds != null && requestDto.FactoryCountryIds.Any())
            {
                inspQueryData = inspQueryData.Where(x => x.Factory.SuAddresses.Any(y => y.AddressTypeId == (int)RefAddressTypeEnum.HeadOffice && requestDto.FactoryCountryIds.Contains(y.CountryId)));
            }

            //apply brand list filter
            if (requestDto.BrandIdList != null && requestDto.BrandIdList.Any())
            {
                inspQueryData = inspQueryData.Where(x => x.InspTranCuBrands.Any(y => y.Active && requestDto.BrandIdList.Contains(y.BrandId)));
            }

            //filter by invoice type id
            if (requestDto.InvoiceType > 0)
            {
                if (requestDto.InvoiceType == (int)INVInvoiceType.PreInvoice)
                {
                    //quotation type "pre-invoice" and quotation status "customer validated"
                    inspQueryData = inspQueryData.Where(z => z.QuQuotationInsps.Any(x => x.IdQuotationNavigation.PaymentTerms
                            == (int)INVInvoiceType.PreInvoice && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated));
                }
                else if (requestDto.InvoiceType == (int)INVInvoiceType.Monthly)
                {
                    //booking status inspected (check the collection of inspected status)
                    inspQueryData = inspQueryData.Where(x => InspectedStatusList.Contains(x.StatusId));

                    //quotation type "monthly invoice" 
                    inspQueryData = inspQueryData.Where(z => z.QuQuotationInsps.Any(x => x.IdQuotationNavigation.PaymentTerms
                                    == (int)INVInvoiceType.Monthly &&
                                    x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated) || !z.QuQuotationInsps.Any());
                }
            }

            if (requestDto.InvoiceStatusId != null && requestDto.InvoiceStatusId.Any())
            {
                //actual status list
                requestDto.ActualInvoiceStatusId = ActualInvoiceStatusIds;
                var extraFeeInvoiceStatus = ActualExtraFeesInvoiceStatusIds;

                //if invoiced and pending selected or invoiced selected or pending selected 
                if (requestDto.InvoiceStatusId.Contains((int)InvoiceStatusSummaryStatusList.Invoiced) &&
                  requestDto.InvoiceStatusId.Contains((int)InvoiceStatusSummaryStatusList.Pending))
                {
                    inspQueryData = inspQueryData.Where(x => x.InvAutTranDetails.Any(y => requestDto.ActualInvoiceStatusId.Contains(y.InvoiceStatus.GetValueOrDefault())) ||
                                            !x.InvAutTranDetails.Any(y => y.InvoiceStatus != (int)InvoiceStatus.Cancelled) || x.InvExfTransactions.Any(y =>
                        extraFeeInvoiceStatus.Contains(y.StatusId.GetValueOrDefault())));

                    inspQueryData = inspQueryData.OrderByDescending(x => !x.InvAutTranDetails.Any(y => y.InvoiceStatus != (int)InvoiceStatus.Cancelled));
                }
                else if (requestDto.InvoiceStatusId.Contains((int)InvoiceStatusSummaryStatusList.Invoiced))
                {
                    inspQueryData = inspQueryData.Where(x => x.InvAutTranDetails.Any(y =>
                    requestDto.ActualInvoiceStatusId.Contains(y.InvoiceStatus.GetValueOrDefault()) || x.InvExfTransactions.Any(y =>
                       (int)ExtraFeeStatus.Invoiced == y.StatusId.GetValueOrDefault())));
                }
                else if (requestDto.InvoiceStatusId.Contains((int)InvoiceStatusSummaryStatusList.Pending))
                {
                    inspQueryData = inspQueryData.Where(x => !x.InvAutTranDetails.Any(x => x.InvoiceStatus != (int)InvoiceStatus.Cancelled) || x.InvExfTransactions.Any(y =>
                           (int)ExtraFeeStatus.Pending == y.StatusId.GetValueOrDefault()));
                }
            }

            //payment status id filter
            if (requestDto.PaymentStatusIdList != null && requestDto.PaymentStatusIdList.Any())
            {
                inspQueryData = inspQueryData.Where(x => x.InvAutTranDetails.Any(y => y.InvoiceStatus != (int)InvoiceStatus.Cancelled &&
                                 requestDto.PaymentStatusIdList.Contains(y.InvoicePaymentStatus)) ||
                                 x.InvExfTransactions.Any(y => y.StatusId != (int)ExtraFeeStatus.Cancelled &&
                                 requestDto.PaymentStatusIdList.Contains(y.PaymentStatus)));
            }

            response.TotalCount = await inspQueryData.CountAsync();

            var invoiceList = await inspQueryData.Skip(skip).Take(requestDto.pageSize.GetValueOrDefault()).Select(x => new InvoiceStatusSummaryItem()
            {
                BookingId = x.Id,
                BookingServiceDateFrom = x.ServiceDateFrom,
                BookingServiceDateTo = x.ServiceDateTo,
                CustomerId = x.CustomerId,
                SupplierId = x.SupplierId,
                FactoryId = x.FactoryId,
                CustomerName = x.Customer.CustomerName,
                SupplierName = x.Supplier.SupplierName,
                FactoryName = x.Factory.SupplierName,
                StatusId = x.StatusId,
                StatusName = x.Status.Status,
                OfficeId = x.OfficeId.GetValueOrDefault(),
                InvoiceTypeId = x.QuQuotationInsps.FirstOrDefault(x => x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).IdQuotationNavigation.PaymentTerms,
                FactoryCountry = x.Factory.SuAddresses.FirstOrDefault(x => x.AddressTypeId == (int)RefAddressTypeEnum.HeadOffice).Country.CountryName
            }).OrderByDescending(x => x.BookingId).AsNoTracking().ToListAsync();


            response.InspTransaction = inspQueryData;

            response.BookingDataList = invoiceList;

            return response;

        }

        /// <summary>
        /// invoice Status summary Export
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<ExportInvoiceStatus>> ExportInvoiceStatusSummary(InvoiceStatusSummaryRequest requestDto)
        {
            List<ExportInvoiceStatus> response = new List<ExportInvoiceStatus>();
            var data = await GetAllInvoiceStatusSummary(requestDto);
            if (data.invoiceStatusDataList == null || !data.invoiceStatusDataList.Any())
            {
                return response;
            }

            var invoiceCommunicationList = await _repository.GetInvoiceCommunicationByInvoiceNoList(data.InvoiceList.Select(x => x.InvoiceNo));

            response = InvoiceStatusSummaryMap.MapExportInvoiceStatusSummary(data, invoiceCommunicationList);

            return response;
        }


        private async Task<List<InspectionStatus>> GetInvoiceInpectionStatusList(IQueryable<InspTransaction> bookingData)
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

        private async Task<List<AuditRepoStatus>> GetInvoiceAuditStatusList(IQueryable<AudTransaction> auditData)
        {
            return await auditData.Select(x => new AuditRepoStatus { Id = x.StatusId, StatusName = x.Status.Status })
                   .GroupBy(p => new { p.Id, p.StatusName }, p => p, (key, _data) =>
               new AuditRepoStatus
               {
                   Id = key.Id,
                   StatusName = key.StatusName,
                   TotalCount = _data.Count()
               }).ToListAsync();
        }
        public async Task<DataSourceResponse> GetStatusListByService(int serviceId)
        {
            var response = new DataSourceResponse();
            if (serviceId == (int)Service.InspectionId)
            {
                var statusList = await _inspRepository.GetBookingStatus();
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
            }
            else if (serviceId == (int)Service.AuditId)
            {
                var auditStatuslist = await _auditRepository.GetAuditStatus();
                if (auditStatuslist == null || !auditStatuslist.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = auditStatuslist.Select(x => new CommonDataSource()
                    {
                        Name = x.Status,
                        Id = x.Id
                    });
                    response.Result = DataSourceResult.Success;
                }
            }
            return response;
        }
        /// <summary>
        /// save communication data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<InvoiceCommunicationSaveResponse> SaveInvoiceCommunication(InvoiceCommunicationSaveRequest request)
        {
            if (request != null)
            {
                if (string.IsNullOrWhiteSpace(request.Comment))
                {
                    return new InvoiceCommunicationSaveResponse() { Result = InvoiceCommunicationSaveResultResponse.RequestNotCorrectFormat };

                }
                else if (string.IsNullOrWhiteSpace(request.InvoiceNo))
                {
                    return new InvoiceCommunicationSaveResponse() { Result = InvoiceCommunicationSaveResultResponse.RequestNotCorrectFormat };
                }
                else
                {
                    InvAutTranCommunication invAutTranCommunicationEntity = new InvAutTranCommunication()
                    {
                        Active = true,
                        Comment = request.Comment,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        InvoiceNumber = request.InvoiceNo,
                        EntityId = _filterService.GetCompanyId()
                    };
                    _repository.AddEntity(invAutTranCommunicationEntity);
                    await _repository.Save();
                    return new InvoiceCommunicationSaveResponse() { Result = InvoiceCommunicationSaveResultResponse.Success };
                }
            }
            else
            {
                return new InvoiceCommunicationSaveResponse() { Result = InvoiceCommunicationSaveResultResponse.RequestNotCorrectFormat };
            }


        }
        /// <summary>
        /// get communication data by invoice number
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        public async Task<InvoiceCommunicationTableResponse> GetInvoiceCommunicationData(string invoiceNo)
        {
            if (!string.IsNullOrWhiteSpace(invoiceNo))
            {
                var communicationData = await _repository.GetInvoiceCommunicationData(invoiceNo);

                if (communicationData.Any())
                {
                    var data = communicationData.Select(x => InvoiceStatusSummaryMap.MapInvoiceCommunication(x));

                    return new InvoiceCommunicationTableResponse()
                    {
                        InvoiceCommunicationTableList = data.ToList(),
                        Result = InvoiceCommunicationTableResultResponse.Success
                    };

                }
                else
                {
                    return new InvoiceCommunicationTableResponse()
                    {
                        Result = InvoiceCommunicationTableResultResponse.NotFound
                    };
                }
            }
            else
            {
                return new InvoiceCommunicationTableResponse()
                {
                    Result = InvoiceCommunicationTableResultResponse.RequestNotCorrectFormat
                };
            }
        }
    }
}
