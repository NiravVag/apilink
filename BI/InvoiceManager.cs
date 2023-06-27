using Contracts.Managers;
using Contracts.Repositories;
using DTO.Invoice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DTO.Common;
using Microsoft.EntityFrameworkCore;
using DTO.Customer;
using Entities;
using DTO.CustomerPriceCard;
using BI.Maps;
using DTO.CommonClass;
using Entities.Enums;
using DTO.RepoRequest.Enum;
using System.Net.Http;
using BI.Utilities;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using DTO.Inspection;
using DTO.ExtraFees;
using DTO.Quotation;
using DTO.Report;
using static BI.TenantProvider;
using System.Globalization;

namespace BI
{
    public class InvoiceManager : ApiCommonData, IInvoiceManager
    {
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly ICustomerManager _customerManager = null;
        private readonly ISupplierManager _supplierManager = null;
        private readonly IInspectionBookingRepository _inspRepository = null;
        private readonly IHelper _helper = null;
        private static IConfiguration _configuration = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly InvoiceMap _invoicemap = null;
        private readonly InvoiceSummaryMap _invoicesummarymap = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IAuditRepository _auditRepository = null;
        private readonly IScheduleRepository _scheduleRepository = null;
        private readonly ITravelMatrixManager _travelMatrixManager = null;
        private readonly IInvoiceDataAccessManager _invoiceDataAccessManager = null;
        private readonly IReferenceRepository _referenceRepository = null;
        private readonly IInvoiceDataAccessRepository _invoiceDataAccessRepository = null;


        public InvoiceManager(IInvoiceRepository invoiceRepository, ICustomerManager customerManager, ISupplierManager supplierManager,
        IInspectionBookingManager inspectionBookingManager, IHelper helper, IInspectionBookingRepository inspRepository, IConfiguration configuration,
        IAPIUserContext ApplicationContext, ITenantProvider filterService, IAuditRepository auditRepository, IScheduleRepository scheduleRepository,
            ITravelMatrixManager travelMatrixManager, IInvoiceDataAccessManager invoiceDataAccessManager, IReferenceRepository referenceRepository,
            IInvoiceDataAccessRepository invoiceDataAccessRepository)
        {
            _invoiceRepository = invoiceRepository;
            _inspRepository = inspRepository;
            _helper = helper;
            _configuration = configuration;
            _customerManager = customerManager;
            _supplierManager = supplierManager;
            _ApplicationContext = ApplicationContext;
            _invoicemap = new InvoiceMap();
            _invoicesummarymap = new InvoiceSummaryMap();
            _filterService = filterService;
            _auditRepository = auditRepository;
            _scheduleRepository = scheduleRepository;
            _travelMatrixManager = travelMatrixManager;
            _invoiceDataAccessManager = invoiceDataAccessManager;
            _referenceRepository = referenceRepository;
            _invoiceDataAccessRepository = invoiceDataAccessRepository;
        }

        /// <summary>
        /// Generate invoice
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<InvoiceGenerateResponse> InspectionInvoiceGenerate(InvoiceGenerateRequest requestDto)
        {
            var ruleConfigDictionary = new Dictionary<int, CustomerPriceCardRepo>();
            var invoiceList = new List<InvoiceDetail>();
            List<QuotationBooking> quotationBookings = null;

            // get all the customer price card rule based on customer and range of inspection dates
            var ruleConfigList = await _invoiceRepository.GetPriceCardRuleList(requestDto);

            // added sorting - similar to pricard fetch
            ruleConfigList = ruleConfigList.OrderByDescending(y => y.PeriodFrom.ToDateTime()).ThenBy(z => z.CustomerName);

            if (ruleConfigList == null || !ruleConfigList.Any())
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoPricecardRuleFound };

            await SetRuleDataList(ruleConfigList);

            // get existing travel invoice based on the request 
            var invtravelBookings = await _invoiceRepository.GetTravelExpenseInvoiceOrderIds(requestDto);

            // get existing inspection invoice based on the request 
            var invInvoiceBookings = await _invoiceRepository.GetInspectionInvoiceOrderIds(requestDto);

            // get common invoice bookings
            var invoicedBookings = invtravelBookings.Intersect(invInvoiceBookings).ToList();

            if (invInvoiceBookings.Any())
            {
                quotationBookings = await _invoiceRepository.GetQuotationInspectionInvoiceOrderIds(invInvoiceBookings);
            }

            // get all the booking data based on customer and range of inspection dates
            var bookingList = await GetInspectioDatabyInvoiceRequest(requestDto, invoicedBookings);

            // check exisiting invoiced bookings 
            if (!requestDto.IsInspection && requestDto.IsTravelExpense)
            {
                bookingList = bookingList.Where(x => !invtravelBookings.Contains(x.BookingId)).ToList();
            }

            if (requestDto.IsInspection && !requestDto.IsTravelExpense)
            {
                bookingList = bookingList.Where(x => !invInvoiceBookings.Contains(x.BookingId)).ToList();
            }
            if (_ApplicationContext.StaffId > 0 && bookingList.Any())
            {
                var invoiceAccessData = await _invoiceDataAccessManager.GetStaffInvoiceDataAcesss(_ApplicationContext.StaffId);
                if (invoiceAccessData == null)
                {
                    return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoInvoiceDataAccess };
                }

                var customerIdAccess = invoiceAccessData.CustomerIds.ToList();

                var officeIdAccess = invoiceAccessData.OfficeIds.ToList();

                var invoiceTypeAccess = invoiceAccessData.InvoiceTypes.ToList();

                if (customerIdAccess.Any())
                {
                    bookingList = bookingList.Where(x => customerIdAccess.Contains(x.CustomerId)).ToList();
                }

                if (officeIdAccess.Any())
                {
                    bookingList = bookingList.Where(x => officeIdAccess.Contains(x.OfficeId)).ToList();
                }

                if (!bookingList.Any())
                    return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoInvoiceDataAccess };

                var invoiceType = requestDto.InvoiceType.HasValue ? requestDto.InvoiceType : (int)INVInvoiceType.Monthly;
                if (invoiceTypeAccess.Any() && !invoiceTypeAccess.Any(x => x == invoiceType) && bookingList.Any())
                {
                    return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoInvoiceDataAccess };
                }
            }


            if (!bookingList.Any())
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoInspectionFound };

            // Loop each inspection and map price card rule with booking
            foreach (var inspectedOrder in bookingList)
            {
                // if not new invoice booking apply group by logic
                if (!requestDto.IsNewBookingInvoice)
                {
                    inspectedOrder.GroupBy = GetGroupByValues(requestDto, inspectedOrder);

                    if (string.IsNullOrWhiteSpace(inspectedOrder.GroupBy))
                    {
                        inspectedOrder.GroupBy = "NA"; // No groupby values are set
                    }
                }

                var ruleConfigs = await GetRuleConfigListbyBookingFilter(inspectedOrder, ruleConfigList);

                var ruleConfig = await GetRuleConfigData(inspectedOrder, ruleConfigs);

                if (ruleConfig != null)
                {
                    inspectedOrder.RuleConfig = ruleConfig;
                    //  Add rule config to dictionary for process later
                    if (!ruleConfigDictionary.ContainsKey(ruleConfig.Id))
                    {
                        ruleConfigDictionary.Add(ruleConfig.Id, ruleConfig);
                    }
                    inspectedOrder.IsInvalid = false;
                }
                else
                {
                    // Error: Cannot find rule for this order
                    inspectedOrder.IsInvalid = true;
                }
            }

            bookingList = bookingList.Where(o => o.IsInvalid == false).ToList();

            if (!bookingList.Any())
            {
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoRuleMapped };
            }
            // Billing Method per rule config
            foreach (var ruleConfig in ruleConfigDictionary.Values)
            {
                invoiceList = await GetInvoiceListbyPriceCalculations(bookingList.ToList(), invoiceList, ruleConfig, requestDto, quotationBookings);
            }

            // no invoice configured for specified Rule and booking
            if (invoiceList.Count == 0)
            {
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoInvoiceConfigured, InvoiceData = null };
            }

            if (requestDto.IsNewBookingInvoice)
            {
                // set invoice number for available bookings
                if (invoiceList.Count > 0)
                    await SetInvoiceNumberAndPostDateForNewBooking(invoiceList, requestDto);
            }
            else
            {
                // set invoice number by gorup by value
                if (invoiceList.Count > 0)
                {
                    await SetInvoiceNumberAndPostDate(bookingList.ToList(), invoiceList, requestDto);

                    // check split invoice logic present
                    if (requestDto.IsInspection && bookingList.Where(x => x.GroupBy != "NA").Select(o => o.GroupBy).Distinct().Any())
                    {
                        // Min fee
                        invoiceList = this.UpdateMinFee(invoiceList, invoicedBookings);
                    }

                }

            }

            // update travel expense values 
            if (requestDto.IsTravelExpense)
            {
                // Get existing invoice for group of factory id and inspection date
                if (invoiceList.Count > 0)
                {
                    var dicExistingInvoices = await this.GetTransactionIdsByFacIdAndInspectionDate(bookingList.ToList());
                    await UpdateTravelMatrixValue(invoiceList, requestDto, dicExistingInvoices);
                }
            }
            else
            {
                if (invoiceList.Count > 0)
                {
                    SetDefaultTravelFees(invoiceList);
                }
            }

            if (!requestDto.IsInspection && invoiceList.Count > 0)
            {
                ClearInspectionFees(invoiceList);
            }

            var listOfInvoice = await SaveInvoiceList(invoiceList, requestDto);

            await _invoiceRepository.Save();

            await LinkInspectionExtraFees(listOfInvoice);

            await _invoiceRepository.Save();

            return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.Success, InvoiceData = listOfInvoice };
        }

        public async Task SetRuleDataList(IEnumerable<CustomerPriceCardRepo> ruleConfigList)
        {

            if (ruleConfigList.Any())
            {
                var ruleIds = ruleConfigList.Select(x => x.Id).ToList();

                var supplierIdList = await _invoiceRepository.GetPrSuppliers(ruleIds);
                var productCategoryIdList = await _invoiceRepository.GetPrProductCategories(ruleIds);
                var productSubCategoryIdList = await _invoiceRepository.GetPrProductSubCategories(ruleIds);
                var serviceTypeIdList = await _invoiceRepository.GetPrServiceTypes(ruleIds);
                var factoryCountryIdList = await _invoiceRepository.GetPrCountries(ruleIds);
                var factoryProvinceIdList = await _invoiceRepository.GetPrProvince(ruleIds);
                var factoryCityIdList = await _invoiceRepository.GetPrCity(ruleIds);
                var departmentIdList = await _invoiceRepository.GetPrDepartment(ruleIds);
                var brandIdList = await _invoiceRepository.GetPrBrand(ruleIds);
                var buyerIdList = await _invoiceRepository.GetPrBuyer(ruleIds);
                var priceCategoryIdList = await _invoiceRepository.GetPrPriceCategory(ruleIds);
                var holidayTypeIdList = await _invoiceRepository.GetPrHolidayType(ruleIds);
                var contactList = await _invoiceRepository.GetPrContacts(ruleIds);
                var subCategory2List = await _invoiceRepository.GetPrSubCategoryList(ruleIds);
                var specialRuleList = await _invoiceRepository.GetPrSpecialRules(ruleIds);

                foreach (var rule in ruleConfigList)
                {
                    rule.SupplierIdList = supplierIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.BrandIdList = brandIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.BuyerIdList = buyerIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.DepartmentIdList = departmentIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.ProductCategoryIdList = productCategoryIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.ProductSubCategoryIdList = productSubCategoryIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.PriceCategoryIdList = priceCategoryIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.ServiceTypeIdList = serviceTypeIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.FactoryCountryIdList = factoryCountryIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.FactoryProvinceIdList = factoryProvinceIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.FactoryCityIdList = factoryCityIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.HolidayTypeIdList = holidayTypeIdList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.InvoiceRequestContact = contactList.Where(x => x.PriceCardId == rule.Id).Select(x => x.Id).Distinct().ToList();
                    rule.SubCategory2List = subCategory2List.Where(x => x.CuPriceCardId.GetValueOrDefault() == rule.Id).ToList();
                    rule.RuleList = specialRuleList.Where(x => x.CuPriceCardId.GetValueOrDefault() == rule.Id).ToList();
                }
            }
        }

        /// <summary>
        /// Link Audit extrafees.
        /// </summary>
        /// <param name="listOfInvoice"></param>
        /// <returns></returns>
        private async Task LinkAuditExtraFees(List<string> listOfInvoice)
        {
            var entityId = _filterService.GetCompanyId();
            foreach (var item in listOfInvoice)
            {
                // link extra fees table with invoice id

                var invoiceDetails = await _invoiceRepository.GetAuditInvoiceTransactionDetails(item);

                var auditIdList = invoiceDetails.Select(x => x.AuditNo).ToList();

                if (invoiceDetails.Any() && invoiceDetails.FirstOrDefault() != null)
                {
                    var extraFees = await _invoiceRepository.
                    GetAuditActiveExtraFeeReference(auditIdList,
                    invoiceDetails.FirstOrDefault().BilledTo.GetValueOrDefault(), invoiceDetails.FirstOrDefault().InvoiceCurrency.GetValueOrDefault(),
                    invoiceDetails.FirstOrDefault().BankId.GetValueOrDefault());

                    if (extraFees.Any())
                    {
                        foreach (var extraFee in extraFees)
                        {
                            if (extraFee != null)
                            {
                                extraFee.InvoiceId = invoiceDetails.FirstOrDefault(x => x.AuditNo == extraFee.AuditId)?.Id;
                                extraFee.StatusId = (int)ExtraFeeStatus.Invoiced;
                                extraFee.UpdatedBy = _ApplicationContext.UserId;
                                extraFee.UpdatedOn = DateTime.Now;
                                // Add log here
                                extraFee.InvExfTranStatusLogs.Add(new InvExfTranStatusLog()
                                {
                                    CreatedBy = _ApplicationContext.UserId,
                                    CreatedOn = DateTime.Now,
                                    AuditId = extraFee.AuditId,
                                    StatusId = (int)ExtraFeeStatus.Pending,
                                    EntityId = entityId
                                });
                            }
                        }
                        _invoiceRepository.EditEntities(extraFees);
                    }
                }
            }
        }

        /// <summary>
        /// Audit Invoice generation
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<InvoiceGenerateResponse> AuditInvoiceGenerate(InvoiceGenerateRequest requestDto)
        {
            var ruleConfigDictionary = new Dictionary<int, CustomerPriceCardRepo>();
            var invoiceList = new List<InvoiceDetail>();
            List<QuotationBooking> quotationBookings = null;

            // get all the customer price card rule based on customer and range of inspection dates
            var ruleConfigList = await _invoiceRepository.GetPriceCardRuleList(requestDto);

            // added sorting - similar to pricard fetch
            ruleConfigList = ruleConfigList.OrderByDescending(y => y.PeriodFrom.ToDateTime()).ThenBy(z => z.CustomerName);

            if (ruleConfigList == null || !ruleConfigList.Any())
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoPricecardRuleFound };

            await SetRuleDataList(ruleConfigList);

            // get existing travel invoice based on the request 
            var invtravelAudits = await _invoiceRepository.GetTravelExpenseInvoiceAuditOrderIds(requestDto);

            // get existing audit invoice based on the request 
            var invInvoiceAudits = await _invoiceRepository.GetAuditInvoiceOrderIds(requestDto);

            // get common invoice audits
            var invoicedAudits = invtravelAudits.Intersect(invInvoiceAudits).ToList();

            if (invInvoiceAudits.Any())
            {
                quotationBookings = await _invoiceRepository.GetQuotationAuditInvoiceOrderIds(invInvoiceAudits);
            }

            // get all the audit data based on customer and range of audit dates
            var auditList = await GetAuditDatabyInvoiceRequest(requestDto, invoicedAudits);

            // check exisiting invoiced bookings 
            if (!requestDto.IsInspection && requestDto.IsTravelExpense)
            {
                auditList = auditList.Where(x => !invtravelAudits.Contains(x.BookingId)).ToList();
            }

            if (requestDto.IsInspection && !requestDto.IsTravelExpense)
            {
                auditList = auditList.Where(x => !invInvoiceAudits.Contains(x.BookingId)).ToList();
            }

            if (!auditList.Any())
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoInspectionFound };

            // Loop each audit and map price card rule with audit
            foreach (var auditOrder in auditList)
            {
                // if not new invoice audit apply group by logic
                if (!requestDto.IsNewBookingInvoice)
                {
                    auditOrder.GroupBy = GetGroupByValues(requestDto, auditOrder);

                    if (string.IsNullOrWhiteSpace(auditOrder.GroupBy))
                    {
                        auditOrder.GroupBy = "NA"; // No groupby values are set
                    }
                }

                var ruleConfigs = await GetRuleConfigListbyBookingFilter(auditOrder, ruleConfigList);

                var ruleConfig = await GetRuleConfigData(auditOrder, ruleConfigs);

                if (ruleConfig != null)
                {
                    auditOrder.RuleConfig = ruleConfig;
                    //  Add rule config to dictionary for process later
                    if (!ruleConfigDictionary.ContainsKey(ruleConfig.Id))
                    {
                        ruleConfigDictionary.Add(ruleConfig.Id, ruleConfig);
                    }
                    auditOrder.IsInvalid = false;
                }
                else
                {
                    // Error: Cannot find rule for this order
                    auditOrder.IsInvalid = true;
                }
            }

            auditList = auditList.Where(o => !o.IsInvalid).ToList();

            if (!auditList.Any())
            {
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoRuleMapped };
            }
            // Billing Method per rule config
            foreach (var ruleConfig in ruleConfigDictionary.Values)
            {
                switch (ruleConfig.BillingMethodId)
                {
                    case (int)PriceBillingMethod.ManDay:

                        if (ruleConfig.InvoiceRequestType != null)
                        {
                            switch (ruleConfig.InvoiceInspFeeFrom)
                            {
                                case (int)InvoiceFeesFrom.Quotation:
                                    invoiceList = await CalculateAuditFeesFromQuotation(auditList.ToList(), invoiceList, ruleConfig, requestDto, quotationBookings);
                                    break;

                                case (int)InvoiceFeesFrom.Invoice:
                                    invoiceList = await CalculateAuditFeesFromInvoiceRule(auditList.ToList(), invoiceList, ruleConfig, requestDto, quotationBookings);
                                    break;
                            }
                        }
                        break;

                    case (int)PriceBillingMethod.Intervention:
                        switch (ruleConfig.InvoiceInspFeeFrom)
                        {
                            case (int)InvoiceFeesFrom.Invoice:
                                if (ruleConfig.InterventionType != null && ruleConfig.InterventionType == (int)InterventionType.Range)
                                {
                                    invoiceList = await CalculatePerInterventionRangeAudit(auditList.ToList(), invoiceList, ruleConfig, requestDto, quotationBookings);
                                }
                                else if (ruleConfig.InterventionType != null && ruleConfig.InterventionType == (int)InterventionType.PerStyle)
                                {
                                    invoiceList = CalculatePerInterventionAuditPerStyle(auditList.ToList(), invoiceList, ruleConfig, requestDto);
                                }
                                break;
                        }

                        break;
                }
            }

            // no invoice configured for specified Rule and audits
            if (invoiceList.Count == 0)
            {
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoInvoiceConfigured, InvoiceData = null };
            }

            if (requestDto.IsNewBookingInvoice)
            {
                // set invoice number for available audits
                if (invoiceList.Count > 0)
                    SetInvoiceNumberAndPostDateForNewAudit(invoiceList, requestDto);
            }
            else
            {
                // set invoice number by gorup by value
                if (invoiceList.Count > 0)
                    await SetAuditInvoiceNumberAndPostDate(auditList.ToList(), invoiceList, requestDto);
            }

            // update travel expense values 
            if (requestDto.IsTravelExpense)
            {
                // Get existing invoice for group of factory id and inspection date
                if (invoiceList.Count > 0)
                {
                    var dicExistingInvoices = await this.GetTransactionIdsByFacIdAndInspectionDate(auditList.ToList());
                    await UpdateAuditTravelMatrixValue(invoiceList, requestDto, dicExistingInvoices);
                }
            }
            else
            {
                if (invoiceList.Count > 0)
                {
                    SetDefaultTravelFees(invoiceList);
                }
            }

            if (!requestDto.IsInspection && invoiceList.Count > 0)
            {
                ClearInspectionFees(invoiceList);
            }

            var listOfInvoice = await SaveInvoiceList(invoiceList, requestDto);

            await _invoiceRepository.Save();

            await LinkAuditExtraFees(listOfInvoice);

            await _invoiceRepository.Save();

            return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.Success, InvoiceData = listOfInvoice };
        }

        /// <summary>
        /// Link extrafees 
        /// </summary>
        /// <param name="listOfInvoice"></param>
        /// <returns></returns>
        private async Task LinkInspectionExtraFees(List<string> listOfInvoice)
        {
            var entityId = _filterService.GetCompanyId();
            foreach (var item in listOfInvoice)
            {
                // link extra fees table with invoice id

                var invoiceDetails = await _invoiceRepository.GetInvoiceTransactionDetails(item);

                var bookingIdList = invoiceDetails.Select(x => x.BookingNo).ToList();

                if (invoiceDetails.Any() && invoiceDetails.FirstOrDefault() != null)
                {
                    var extraFees = await _invoiceRepository.
                    GetBookingActiveExtraFeeReference(bookingIdList,
                    invoiceDetails.FirstOrDefault().BilledTo.GetValueOrDefault(), invoiceDetails.FirstOrDefault().InvoiceCurrency.GetValueOrDefault(),
                    invoiceDetails.FirstOrDefault().BankId.GetValueOrDefault());

                    foreach (var extraFee in extraFees)
                    {
                        if (extraFee != null)
                        {
                            extraFee.InvoiceId = invoiceDetails.FirstOrDefault(x => x.BookingNo == extraFee.InspectionId)?.Id;
                            extraFee.StatusId = (int)ExtraFeeStatus.Invoiced;
                            extraFee.UpdatedBy = _ApplicationContext.UserId;
                            extraFee.UpdatedOn = DateTime.Now;
                            // Add log here
                            extraFee.InvExfTranStatusLogs.Add(new InvExfTranStatusLog()
                            {
                                CreatedBy = _ApplicationContext.UserId,
                                CreatedOn = DateTime.Now,
                                InspectionId = extraFee.InspectionId,
                                StatusId = (int)ExtraFeeStatus.Pending,
                                EntityId = entityId
                            });
                        }
                    }
                    _invoiceRepository.EditEntities(extraFees);
                }
            }
        }



        /// <summary>
        /// Get invoice List by factory and dates
        /// </summary>
        /// <param name="orderTransactions"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, List<InvAutTranDetail>>> GetTransactionIdsByFacIdAndInspectionDate(List<InvoiceBookingDetail> orderTransactions)
        {
            var dicExistingInvoices = new Dictionary<string, List<InvAutTranDetail>>();
            var dateAndFactoryIds = (from order in orderTransactions
                                     orderby order.ServiceTo, order.FactoryId
                                     select new
                                     {
                                         order.ServiceTo,
                                         order.FactoryId
                                     }).Distinct();
            foreach (var dateAndFactoryId in dateAndFactoryIds)
            {
                var invoices = await _invoiceRepository.GetInvoiceOrdersSameFacIdAndInspectionDate
                             (orderTransactions[0].CustomerId, dateAndFactoryId.FactoryId.GetValueOrDefault(), dateAndFactoryId.ServiceTo);

                dicExistingInvoices.Add(Combine(dateAndFactoryId.FactoryId.GetValueOrDefault(), dateAndFactoryId.ServiceTo), invoices);


            }

            return dicExistingInvoices;
        }

        /// <summary>
        /// Combine Date and factory
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="inspectionDate"></param>
        /// <returns></returns>
        private string Combine(int factoryId, DateTime inspectionDate)
        {
            return factoryId + "_" + inspectionDate.ToString(StandardDateFormat);
        }

        /// <summary>
        /// Set tax value and total invoice amount
        /// </summary>
        /// <param name="invoiceList"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task<List<string>> SaveInvoiceList(List<InvoiceDetail> invoiceList, InvoiceGenerateRequest requestDto)
        {
            List<string> listOfInvoice = new List<string>();

            // if we have bank selection when generate the invoice then override the bank id with request bank id.
            if (invoiceList.Any() && requestDto.BankAccount.GetValueOrDefault() > 0)
            {
                invoiceList.ForEach(x => x.BankId = requestDto.BankAccount.GetValueOrDefault());
            }
            // get tax total
            var taxList = await GetTotalTaxValue(invoiceList, requestDto);
            var _entityId = _filterService.GetCompanyId();
            foreach (var invoice in invoiceList)
            {
                // apply Exchange rate with all the fees
                setExchangeRateAndInvoiceCurrency(requestDto, invoice);
                // set billing Address
                await setBillingAddress(requestDto, invoice);
                // set the additional tax for bangladesh country
                invoice.AdditionalBdTax = requestDto.AdditionalTax;
                // set total invoice amount
                setTotalInvoiceFeesAndTaxAmount(invoice, taxList);
                // save invoice
                var savedInvoice = await SaveInvoice(invoice, requestDto, taxList, _entityId);

                foreach (var item in savedInvoice)
                {
                    listOfInvoice.Add(item.InvoiceNo);
                }

            }
            // return unique invoice number.
            return listOfInvoice.Where(x => x != string.Empty).Distinct().ToList();
        }

        /// <summary>
        /// Save Invoice details
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="requestDto"></param>
        /// <param name="taxDetails"></param>
        private async Task<List<InvAutTranDetail>> SaveInvoice(InvoiceDetail invoice, InvoiceGenerateRequest requestDto, List<InvoiceBankTax> taxDetails, int _entityId)
        {
            List<InvAutTranDetail> invoiceNumberList = new List<InvAutTranDetail>();
            if (invoice != null)
            {
                var invoiceEntitiy = new InvAutTranDetail()
                {
                    Id = 0,
                    InvoiceNo = invoice.InvoiceNo,
                    InvoiceDate = invoice.InvoiceDate,
                    PostedDate = invoice.PostedDate,
                    ServiceId = requestDto.Service,

                    UnitPrice = invoice.UnitPrice.GetValueOrDefault(),
                    InspectionFees = Math.Round(invoice.InspectionFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),

                    TravelAirFees = Math.Round(invoice.TravelAirFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                    TravelLandFees = Math.Round(invoice.TravelLandFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                    TravelOtherFees = Math.Round(invoice.TravelOtherFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),

                    TravelTotalFees = Math.Round(invoice.TravelTotalFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),


                    HotelFees = Math.Round(invoice.HotelFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),

                    OtherFees = Math.Round(invoice.OtherFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),

                    Discount = Math.Round(invoice.Discount.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),

                    TotalTaxAmount = Math.Round(invoice.TotalTaxAmount.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),

                    TaxValue = Math.Round(invoice.TaxValue.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),

                    TotalInvoiceFees = Math.Round(invoice.TotalInvoiceFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),

                    TotalSampleSize = invoice.TotalSampleSize,
                    PriceCardCurrency = invoice.PriceCardCurrency,
                    InvoiceCurrency = invoice.InvoiceCurrency,
                    ExchangeRate = invoice.ExchangeRate,
                    RuleExchangeRate = null,
                    InvoiceTo = invoice.InvoiceTo,
                    InvoiceMethod = invoice.InvoiceMethod,
                    ManDays = invoice.ManDays,
                    TravelMatrixType = invoice.TravelMatrixType,
                    InvoicedName = invoice.InvoicedName,
                    InvoicedAddress = invoice.InvoicedAddress,
                    Office = invoice.Office,
                    PaymentTerms = invoice.PaymentTerms,
                    PaymentDuration = invoice.PaymentDuration,
                    BankId = requestDto.BankAccount.GetValueOrDefault() > 0 ? requestDto.BankAccount.GetValueOrDefault() : invoice.BankId,
                    IsAutomation = invoice.IsAutomation,
                    IsInspection = invoice.IsInspection,
                    IsTravelExpense = invoice.IsTravelExpense,
                    InspectionId = invoice.InspectionId,
                    AuditId = invoice.AuditId,
                    InvoiceStatus = invoice.InvoiceStatus,
                    InvoicePaymentStatus = invoice.InvoicePaymentStatus,
                    InvoicePaymentDate = invoice.InvoicePaymentDate,
                    RuleId = invoice.RuleId,
                    InvoiceType = invoice.InvoiceType,

                    CalculateInspectionFee = invoice.CalculateInspectionFee,
                    CalculateTravelExpense = invoice.CalculateTravelExpense,
                    CalculateHotelFee = invoice.CalculateHotelFee,
                    CalculateDiscountFee = invoice.CalculateDiscountFee,
                    CalculateOtherFee = invoice.CalculateOtherFee,
                    Remarks = invoice.Remarks,
                    Subject = invoice.Subject,
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    ProrateBookingNumbers = invoice.ProrateBookingNumbers,
                    EntityId = _entityId,
                    PriceCalculationType = invoice.PriceCalculationtype,
                    AdditionalBdTax = invoice.AdditionalBdTax
                };


                // Add tax info based on the invoice bank details
                if (taxDetails.Count > 0)
                {

                    foreach (var tax in taxDetails)
                    {

                        var taxinfo = new InvAutTranTax()
                        {
                            TaxId = tax.Id,
                            InvoiceId = invoice.Id,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now.Date
                        };

                        _invoiceRepository.AddEntity(taxinfo);
                        invoiceEntitiy.InvAutTranTaxes.Add(taxinfo);
                    }

                }

                // Add invoice request contacts based on invoiceto flag

                switch (requestDto.InvoiceTo)
                {
                    case (int)InvoiceTo.Supplier:

                        foreach (var id in requestDto.SupplierInfo.ContactPersonIdList)
                        {
                            var contact = new InvAutTranContactDetail()
                            {
                                SupplierContactId = id,
                                InvoiceId = invoice.Id
                            };
                            _invoiceRepository.AddEntity(contact);
                            invoiceEntitiy.InvAutTranContactDetails.Add(contact);
                        }
                        break;

                    case (int)InvoiceTo.Factory:
                        foreach (var id in requestDto.SupplierInfo.ContactPersonIdList)
                        {

                            var contact = new InvAutTranContactDetail()
                            {
                                FactoryContactId = id,
                                InvoiceId = invoice.Id
                            };
                            _invoiceRepository.AddEntity(contact);
                            invoiceEntitiy.InvAutTranContactDetails.Add(contact);
                        }
                        break;

                    case (int)InvoiceTo.Customer:

                        if (invoice.Rule.InvoiceRequestContact != null && invoice.Rule.InvoiceRequestContact.Any())
                        {
                            foreach (var id in invoice.Rule.InvoiceRequestContact)
                            {
                                var contact = new InvAutTranContactDetail()
                                {
                                    CustomerContactId = id,
                                    InvoiceId = invoice.Id
                                };
                                _invoiceRepository.AddEntity(contact);
                                invoiceEntitiy.InvAutTranContactDetails.Add(contact);
                            }
                        }

                        break;
                }

                // update invoice status log               
                var logData = new InvAutTranStatusLog
                {
                    InvoiceId = invoice.Id,
                    InspectionId = invoice.InspectionId,
                    StatusId = (int)InvoiceStatus.Created,
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    EntityId = _entityId,
                };

                _invoiceRepository.AddEntity(logData);
                invoiceEntitiy.InvAutTranStatusLogs.Add(logData);


                invoiceNumberList.Add(invoiceEntitiy);
                _invoiceRepository.AddEntity(invoiceEntitiy);

            }
            return invoiceNumberList.Distinct().ToList();
        }

        private void setExchangeRateAndInvoiceCurrency(InvoiceGenerateRequest requestDto, InvoiceDetail invoice)
        {
            if (requestDto.ExchangeRate != null)
            {
                invoice.InspectionFees = invoice.InspectionFees * requestDto.ExchangeRate.GetValueOrDefault();
                invoice.UnitPrice = invoice.UnitPrice * requestDto.ExchangeRate.GetValueOrDefault();

                invoice.TravelAirFees = invoice.TravelAirFees * requestDto.ExchangeRate.GetValueOrDefault();
                invoice.TravelLandFees = invoice.TravelLandFees * requestDto.ExchangeRate.GetValueOrDefault();

                invoice.TravelTotalFees = invoice.TravelTotalFees * requestDto.ExchangeRate.GetValueOrDefault();
                invoice.HotelFees = invoice.HotelFees * requestDto.ExchangeRate.GetValueOrDefault();
                invoice.Discount = invoice.Discount * requestDto.ExchangeRate.GetValueOrDefault();
                invoice.OtherFees = invoice.OtherFees * requestDto.ExchangeRate.GetValueOrDefault();

                invoice.ExchangeRate = requestDto.ExchangeRate.GetValueOrDefault();
            }

            if (requestDto.CurrencyId != null)
            {
                invoice.InvoiceCurrency = requestDto.CurrencyId.GetValueOrDefault();
            }
        }

        /// <summary>
        /// get total tax value
        /// </summary>
        /// <param name="invoiceList"></param>
        /// <returns></returns>
        private async Task<List<InvoiceBankTax>> GetTotalTaxValue(List<InvoiceDetail> invoiceList, InvoiceGenerateRequest requestDto)
        {
            var bankIds = invoiceList.Select(i => i.BankId).Distinct().ToList();

            if (bankIds.Any() && bankIds.FirstOrDefault() != null)
            {
                var bankId = bankIds.FirstOrDefault().Value;
                var minInspectionDate = invoiceList.Min(i => i.InspectionDate);
                var maxInspectionDate = invoiceList.Max(i => i.InspectionDate);
                var taxes = await _invoiceRepository.GetTaxDetails(bankId, maxInspectionDate, minInspectionDate);
                return taxes.ToList();
            }
            return new List<InvoiceBankTax>();
        }

        private void setTotalInvoiceFeesAndTaxAmount(InvoiceDetail invoice, List<InvoiceBankTax> taxDetails)
        {
            invoice.TotalInvoiceFees = 0;

            var inspectionFees = Math.Round(invoice.InspectionFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
            var travelTotalFees = Math.Round(invoice.TravelTotalFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
            var hotelFees = Math.Round(invoice.HotelFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
            var otherFees = Math.Round(invoice.OtherFees.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);

            var tempTotal = Math.Round(inspectionFees + travelTotalFees + hotelFees + otherFees, InvoiceRoundUpValue, MidpointRounding.AwayFromZero);


            double totalFeeWithAdditional = 0;
            double additionalTaxValue = 0;
            // apply additional tax value for bangladesh country
            var bangladesh_BankId = _configuration["Bangladesh_BankId"].ToString();
            var selectedBankList = bangladesh_BankId.Split(',');
            if (invoice.BankId != null && selectedBankList.Contains(invoice.BankId.ToString()))
            {
                if (invoice.AdditionalBdTax > 0)
                {
                    totalFeeWithAdditional = Math.Round((tempTotal * 100 / (100 - invoice.AdditionalBdTax.GetValueOrDefault())), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
                }
                else
                {
                    // set default additional tax value as 10 percent
                    invoice.AdditionalBdTax = DefaultAdditionalTax;
                    totalFeeWithAdditional = Math.Round((tempTotal * 100 / (100 - invoice.AdditionalBdTax.GetValueOrDefault())), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
                }

                additionalTaxValue = totalFeeWithAdditional - tempTotal;
                invoice.InspectionFees = invoice.InspectionFees + additionalTaxValue;
            }

            invoice.TotalInvoiceFees = tempTotal + additionalTaxValue;

            var taxTotal = taxDetails.Select(t => (double)t.TaxValue).Sum();

            invoice.TaxValue = taxTotal;
            invoice.TotalTaxAmount = Math.Round(invoice.TotalInvoiceFees.GetValueOrDefault() * taxTotal, InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
            invoice.TotalInvoiceFees = invoice.TotalInvoiceFees + invoice.TotalTaxAmount;
            if (invoice.Discount > 0)
                invoice.TotalInvoiceFees = invoice.TotalInvoiceFees - invoice.Discount;
        }

        /// <summary>
        /// set billing address
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        private async Task setBillingAddress(InvoiceGenerateRequest requestDto, InvoiceDetail invoice)
        {
            invoice.InvoiceDate = DateTime.Now;
            if (requestDto.InvoiceTo == (int)InvoiceTo.Supplier)
            {
                var supplier = requestDto.SupplierInfo;
                invoice.InvoicedAddress = supplier.BillingAddress;
                invoice.InvoicedName = supplier.BilledName;
                invoice.InvoiceTo = (int)InvoiceTo.Supplier;
            }
            else if (requestDto.InvoiceTo == (int)InvoiceTo.Factory)
            {
                var supplier = requestDto.SupplierInfo;
                invoice.InvoicedAddress = supplier.BillingAddress;
                invoice.InvoicedName = supplier.BilledName;
                invoice.InvoiceTo = (int)InvoiceTo.Factory;
            }
            else if (requestDto.InvoiceTo == (int)InvoiceTo.Customer)
            {
                var invoicingRequestItem = await this.GetBillingAddress(invoice.Rule, invoice);
                if (invoicingRequestItem != null)
                {
                    invoice.InvoicedAddress = invoicingRequestItem.BilledAddress;
                    invoice.InvoicedName = invoicingRequestItem.BilledName;
                    invoice.InvoiceTo = (int)InvoiceTo.Customer;
                    invoice.Rule.InvoiceRequestContact = invoicingRequestItem.InvoiceRequestContactList;
                }
            }
        }

        /// <summary>
        /// get billing address based on the rules
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        private async Task<PriceInvoiceRequest> GetBillingAddress(CustomerPriceCardRepo rule, InvoiceDetail invoice)
        {
            var defaultInvoicingRequestDto = new PriceInvoiceRequest
            {
                BilledName = rule.InvoiceRequestBilledName,
                BilledAddress = rule.InvoiceRequestAddress,
                InvoiceRequestContactList = rule.InvoiceRequestContact
            };

            switch (rule.InvoiceRequestType)
            {
                case (int)InvoiceRequestType.NotApplicable:
                    return defaultInvoicingRequestDto;
                case (int)InvoiceRequestType.Brand:

                    var brandData = await _invoiceRepository.GetInvoiceRequestAddressByBrand(new List<int?> { rule.Id }, invoice.BookingBrandIds);

                    if ((brandData.FirstOrDefault() == null) && (rule.InvoiceRequestSelectAll == true))
                    {
                        return defaultInvoicingRequestDto;
                    }
                    else
                    {
                        var invoiceRequestIds = brandData.Select(x => x.Id).ToList();
                        var contactList = await _invoiceRepository.GetInvoiceRequestContacts(invoiceRequestIds);

                        foreach (var item in brandData)
                        {
                            item.InvoiceRequestContactList = contactList.Where(x => x.InvoiceRequestId == item.Id).Select(x => x.ContactId).ToList();
                        }
                        return new PriceInvoiceRequest()
                        {
                            BilledAddress = brandData.FirstOrDefault()?.BilledAddress,
                            BilledName = brandData.FirstOrDefault().BilledName,
                            InvoiceRequestContactList = brandData.FirstOrDefault().InvoiceRequestContactList
                        };
                    }
                case (int)InvoiceRequestType.Department:
                    var departmentData = await _invoiceRepository.GetInvoiceRequestAddressByDepartment(new List<int?> { rule.Id }, invoice.BookingDepartmentIds);

                    if ((departmentData.FirstOrDefault() == null) && (rule.InvoiceRequestSelectAll == true))
                    {
                        return defaultInvoicingRequestDto;
                    }
                    else
                    {
                        var invoiceRequestIds = departmentData.Select(x => x.Id).ToList();
                        var contactList = await _invoiceRepository.GetInvoiceRequestContacts(invoiceRequestIds);

                        foreach (var item in departmentData)
                        {
                            item.InvoiceRequestContactList = contactList.Where(x => x.InvoiceRequestId == item.Id).Select(x => x.ContactId).ToList();
                        }
                        return new PriceInvoiceRequest()
                        {
                            BilledAddress = departmentData.FirstOrDefault()?.BilledAddress,
                            BilledName = departmentData.FirstOrDefault()?.BilledName,
                            InvoiceRequestContactList = departmentData.FirstOrDefault().InvoiceRequestContactList
                        };
                    }
                case (int)InvoiceRequestType.Buyer:
                    var buyerData = await _invoiceRepository.GetInvoiceRequestAddressByBuyer(new List<int?> { rule.Id }, invoice.BookingBuyerIds);

                    if ((buyerData.FirstOrDefault() == null) && (rule.InvoiceRequestSelectAll == true))
                    {
                        return defaultInvoicingRequestDto;
                    }
                    else
                    {
                        var invoiceRequestIds = buyerData.Select(x => x.Id).ToList();
                        var contactList = await _invoiceRepository.GetInvoiceRequestContacts(invoiceRequestIds);

                        foreach (var item in buyerData)
                        {
                            item.InvoiceRequestContactList = contactList.Where(x => x.InvoiceRequestId == item.Id).Select(x => x.ContactId).ToList();
                        }

                        return new PriceInvoiceRequest()
                        {
                            BilledAddress = buyerData.FirstOrDefault()?.BilledAddress,
                            BilledName = buyerData.FirstOrDefault()?.BilledName,
                            InvoiceRequestContactList = buyerData.FirstOrDefault()?.InvoiceRequestContactList
                        };
                    }
                default:
                    return defaultInvoicingRequestDto;
            }
        }

        private void SetDefaultTravelFees(List<InvoiceDetail> invoiceList)
        {
            foreach (var invoice in invoiceList)
            {
                invoice.TravelLandFees = 0;
                invoice.TravelAirFees = 0;
                invoice.HotelFees = 0;
                invoice.ExchangeRate = 0;
                invoice.TravelOtherFees = 0;
                invoice.TravelTotalFees = 0;
            }
        }

        /// <summary>
        /// clear inspection fees
        /// </summary>
        /// <param name="invoiceList"></param>
        private void ClearInspectionFees(List<InvoiceDetail> invoiceList)
        {
            // Clear inspection fee 
            foreach (var invoice in invoiceList)
            {
                invoice.InspectionFees = 0;
                invoice.UnitPrice = 0;
                invoice.IsInspection = false;
            }
        }

        /// <summary>
        /// update travel matrix values
        /// </summary>
        /// <param name="bookingList"></param>
        /// <param name="invoiceList"></param>
        /// <param name="requestDto"></param>
        private async Task UpdateTravelMatrixValue(List<InvoiceDetail> invoiceList, InvoiceGenerateRequest requestDto, Dictionary<string, List<InvAutTranDetail>> dicExistingInvoices)
        {

            // take only rules quotation and careffour with manday > 1 - only from quotation

            var invoiceListQuotation = invoiceList.Where(x =>

                                       x.Rule.InvoiceTravelExpense == (int)InvoiceFeesFrom.Quotation ||

                                       (x.Rule.InvoiceTravelExpense == (int)InvoiceFeesFrom.Carrefour && x.ManDays.GetValueOrDefault() > 1));

            foreach (var itemData in invoiceListQuotation)
            {
                switch (itemData.Rule.InvoiceTravelExpense)
                {
                    case (int)InvoiceFeesFrom.Quotation:
                        await CalculateTravelMatrixFromQuotation(itemData, requestDto);
                        break;

                    case (int)InvoiceFeesFrom.Carrefour:
                        if (itemData.ManDays.GetValueOrDefault() > 1)
                        {
                            await CalculateTravelMatrixFromQuotation(itemData, requestDto);
                        }
                        break;
                }
            }

            // take only rules Invoice and careffour with manday = 1 - only from travel matrix
            var invoiceListTravelMatrix = invoiceList.Where(x =>

                                       x.Rule.InvoiceTravelExpense == (int)InvoiceFeesFrom.Invoice ||

                                       (x.Rule.InvoiceTravelExpense == (int)InvoiceFeesFrom.Carrefour && x.ManDays.GetValueOrDefault() == 1));


            var dateAndFactoryIds = (from invoice in invoiceListTravelMatrix

                                     orderby invoice.InspectionDate, invoice.FactoryId
                                     select new
                                     {
                                         invoice.InspectionDate,
                                         invoice.FactoryId
                                     }).Distinct();


            foreach (var dateAndFactoryId in dateAndFactoryIds)
            {

                var updated = false;
                foreach (var invoice in invoiceListTravelMatrix)
                {
                    if ((invoice.InspectionDate == dateAndFactoryId.InspectionDate)
                       && (invoice.FactoryId == dateAndFactoryId.FactoryId))
                    {
                        if (!updated)
                        {
                            // check from existing invoices - check already calculated for same date and factory
                            var checkTravelTotalFees = 0;
                            if (dicExistingInvoices.TryGetValue(Combine(dateAndFactoryId.FactoryId.GetValueOrDefault(), dateAndFactoryId.InspectionDate),
                                out var existingInvoices))
                            {
                                checkTravelTotalFees = existingInvoices.Count(i => i.TravelTotalFees > 0);
                            }
                            if (checkTravelTotalFees > 0 || invoiceListTravelMatrix.
                                Any(x => x.InspectionDate == dateAndFactoryId.InspectionDate &&
                                x.FactoryId == dateAndFactoryId.FactoryId && x.TravelTotalFees > 0))

                            {
                                updated = true;
                                invoice.TravelLandFees = 0;
                                invoice.TravelAirFees = 0;
                                invoice.TravelOtherFees = 0;
                                invoice.TravelTotalFees = 0;
                                invoice.HotelFees = 0;
                                continue;
                            }
                            switch (invoice.Rule.InvoiceTravelExpense)
                            {
                                case (int)InvoiceFeesFrom.Invoice:
                                    await CalculateTravelMatrixBasedOnPriceCard(invoice, invoice.ManDays.GetValueOrDefault());
                                    break;

                                case (int)InvoiceFeesFrom.Carrefour:

                                    if (invoice.ManDays.GetValueOrDefault() == 1)
                                    {
                                        await CalculateTravelMatrixBasedOnPriceCard(invoice, invoice.ManDays.GetValueOrDefault());
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// update audit travel matrix value
        /// </summary>
        /// <param name="invoiceList"></param>
        /// <param name="requestDto"></param>
        /// <param name="dicExistingInvoices"></param>
        /// <returns></returns>
        private async Task UpdateAuditTravelMatrixValue(List<InvoiceDetail> invoiceList, InvoiceGenerateRequest requestDto, Dictionary<string, List<InvAutTranDetail>> dicExistingInvoices)
        {
            var dateAndFactoryIds = (from invoice in invoiceList
                                     orderby invoice.InspectionDate, invoice.FactoryId
                                     select new
                                     {
                                         invoice.InspectionDate,
                                         invoice.FactoryId
                                     }).Distinct();


            foreach (var dateAndFactoryId in dateAndFactoryIds)
            {

                var updated = false;
                foreach (var invoice in invoiceList)
                {
                    if ((invoice.InspectionDate == dateAndFactoryId.InspectionDate)
                       && (invoice.FactoryId == dateAndFactoryId.FactoryId))
                    {
                        if (!updated)
                        {
                            // check from existing invoices - check already calculated for same date and factory
                            var checkTravelTotalFees = 0;
                            if (dicExistingInvoices.TryGetValue(Combine(dateAndFactoryId.FactoryId.GetValueOrDefault(), dateAndFactoryId.InspectionDate),
                                out var existingInvoices))
                            {
                                checkTravelTotalFees = existingInvoices.Count(i => i.TravelTotalFees > 0);
                            }
                            if (checkTravelTotalFees > 0 || invoiceList.
                                Any(x => x.InspectionDate == dateAndFactoryId.InspectionDate &&
                                x.FactoryId == dateAndFactoryId.FactoryId && x.TravelTotalFees > 0))

                            {
                                updated = true;
                                invoice.TravelLandFees = 0;
                                invoice.TravelAirFees = 0;
                                invoice.TravelOtherFees = 0;
                                invoice.TravelTotalFees = 0;
                                invoice.HotelFees = 0;
                                continue;
                            }
                            switch (invoice.Rule.InvoiceTravelExpense)
                            {
                                case (int)InvoiceFeesFrom.Invoice:
                                    await CalculateTravelMatrixBasedOnPriceCard(invoice, invoice.ManDays.GetValueOrDefault());
                                    break;

                                case (int)InvoiceFeesFrom.Quotation:
                                    await CalculateTravelMatrixFromQuotationAudit(invoice, requestDto);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// get travel matrix 
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="numberOfInspector"></param>
        /// <returns></returns>
        private async Task CalculateTravelMatrixBasedOnPriceCard(InvoiceDetail invoice, double manDay)
        {
            QuotationMatrixRequest travelRequest = new QuotationMatrixRequest()
            {
                CountyId = invoice.FactoryCountyId,
                CityId = invoice.FactoryCityId,
                ProvinceId = invoice.FactoryProvinceId,
                CurrencyId = invoice.InvoiceCurrency.GetValueOrDefault(),
                MatrixTypeId = invoice.TravelMatrixType,
                customerId = invoice.Rule?.CustomerId
            };
            // get travel matrix data

            var travelMatrixData = await _travelMatrixManager.GetTravelMatrixList(travelRequest);

            double? landCost = 0;
            double? airCost = 0;
            double? hotelCost = 0;
            double? otherTravelCost = 0;
            var travelData = travelMatrixData.FirstOrDefault();
            if (travelData != null)
            {
                var busTotalCost = (travelData.BusCost.GetValueOrDefault() + (travelData.BusCost.GetValueOrDefault() * travelData.MarkUpCost.GetValueOrDefault())) / travelData.FixExchangeRate.GetValueOrDefault();
                var taxiTotalCost = (travelData.TaxiCost.GetValueOrDefault() + (travelData.TaxiCost.GetValueOrDefault() * travelData.MarkUpCost.GetValueOrDefault())) / travelData.FixExchangeRate.GetValueOrDefault();
                var trainTotalCost = (travelData.TrainCost.GetValueOrDefault() + (travelData.TrainCost.GetValueOrDefault() * travelData.MarkUpCost.GetValueOrDefault())) / travelData.FixExchangeRate.GetValueOrDefault();
                var airTotalCost = (travelData.AirCost.GetValueOrDefault() + (travelData.AirCost.GetValueOrDefault() * travelData.MarkUpCostAir.GetValueOrDefault())) / travelData.FixExchangeRate.GetValueOrDefault();

                landCost = (busTotalCost + taxiTotalCost + trainTotalCost) * manDay;
                airCost = airTotalCost * manDay;
                hotelCost = (travelData.HotelCost.GetValueOrDefault() / travelData.FixExchangeRate.GetValueOrDefault()) * manDay;
                otherTravelCost = ((travelData.OtherCost.GetValueOrDefault() + (travelData.OtherCost.GetValueOrDefault() * travelData.MarkUpCost.GetValueOrDefault())) / travelData.FixExchangeRate.GetValueOrDefault()) * manDay;
                // set exchange rate from travel matrix
                invoice.ExchangeRate = travelData.FixExchangeRate.GetValueOrDefault();
            }
            invoice.TravelLandFees = landCost.GetValueOrDefault();
            invoice.TravelAirFees = airCost.GetValueOrDefault();
            invoice.HotelFees = hotelCost.GetValueOrDefault();
            invoice.TravelOtherFees = otherTravelCost.GetValueOrDefault();

            // calculate total travel fees
            invoice.TravelTotalFees = (invoice.TravelLandFees + invoice.TravelAirFees + invoice.TravelOtherFees);
            invoice.IsTravelExpense = true;
        }

        /// <summary>
        /// get travel data from quotation
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="numberOfInspector"></param>
        /// <returns></returns>
        private async Task CalculateTravelMatrixFromQuotation(InvoiceDetail invoice, InvoiceGenerateRequest requestDto)
        {

            // get travel matrix data
            var travelDataFromQuotation = await _invoiceRepository.GetQuotationDataByBookingNo(invoice.InspectionId.GetValueOrDefault());
            double? landCost = 0;
            double? airCost = 0;
            double? hotelCost = 0;

            var travelData = travelDataFromQuotation.FirstOrDefault();
            if (travelData != null)
            {
                landCost = travelData.TravelLandCost;
                airCost = travelData.TravelAirCost;
                hotelCost = travelData.TravelHotelCost;
            }
            // check quotation billing to and from request billing to option is same
            if (invoice.Rule.InvoiceTravelExpense == (int)InvoiceFeesFrom.Quotation)
            {
                if (travelData != null && travelData.BillingTo == requestDto.InvoiceTo)
                {
                    invoice.TravelLandFees = landCost.GetValueOrDefault();
                    invoice.TravelAirFees = airCost.GetValueOrDefault();
                    invoice.TravelTotalFees = (invoice.TravelLandFees + invoice.TravelAirFees);
                    invoice.HotelFees = hotelCost.GetValueOrDefault();
                    invoice.IsTravelExpense = true;
                }
            }
            else
            {
                if (travelData != null)
                {
                    invoice.TravelLandFees = landCost.GetValueOrDefault();
                    invoice.TravelAirFees = airCost.GetValueOrDefault();
                    invoice.TravelTotalFees = (invoice.TravelLandFees + invoice.TravelAirFees);
                    invoice.HotelFees = hotelCost.GetValueOrDefault();
                    invoice.IsTravelExpense = true;
                }
            }
        }

        /// <summary>
        /// calculate travel matrix from quotation audit.
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task CalculateTravelMatrixFromQuotationAudit(InvoiceDetail invoice, InvoiceGenerateRequest requestDto)
        {

            // get travel matrix data
            var travelDataFromQuotation = await _invoiceRepository.GetQuotationDataByAuditNo(invoice.AuditId.GetValueOrDefault());
            double? landCost = 0;
            double? airCost = 0;
            double? hotelCost = 0;

            var travelData = travelDataFromQuotation.FirstOrDefault();
            if (travelData != null)
            {
                landCost = travelData.TravelLandCost;
                airCost = travelData.TravelAirCost;
                hotelCost = travelData.TravelHotelCost;
            }
            // check quotation billing to and from request billing to option is same
            if (invoice.Rule.InvoiceTravelExpense == (int)InvoiceFeesFrom.Quotation)
            {
                if (travelData != null && travelData.BillingTo == requestDto.InvoiceTo)
                {
                    invoice.TravelLandFees = landCost.GetValueOrDefault();
                    invoice.TravelAirFees = airCost.GetValueOrDefault();
                    invoice.TravelTotalFees = (invoice.TravelLandFees + invoice.TravelAirFees);
                    invoice.HotelFees = hotelCost.GetValueOrDefault();
                    invoice.IsTravelExpense = true;
                }
            }
            else
            {
                if (travelData != null)
                {
                    invoice.TravelLandFees = landCost.GetValueOrDefault();
                    invoice.TravelAirFees = airCost.GetValueOrDefault();
                    invoice.TravelTotalFees = (invoice.TravelLandFees + invoice.TravelAirFees);
                    invoice.HotelFees = hotelCost.GetValueOrDefault();
                    invoice.IsTravelExpense = true;
                }
            }
        }

        /// <summary>
        /// Set Invoice number with split invoice concept
        /// </summary>
        /// <param name="bookingList"></param>
        /// <param name="invoiceList"></param>
        /// <param name="requestDto"></param>
        private async Task SetInvoiceNumberAndPostDate(List<InvoiceBookingDetail> bookingList, List<InvoiceDetail> invoiceList, InvoiceGenerateRequest requestDto)
        {
            // Group by, invoice no
            var groupByList = bookingList.Select(o => o.GroupBy).Distinct();

            var featureList = await _referenceRepository.GetEntityFeatureList();

            foreach (var groupByValue in groupByList)
            {
                var ruleConfig = bookingList.Where(o => o.GroupBy == groupByValue).Select(o => o.RuleConfig).FirstOrDefault();
                if ((ruleConfig != null) && (ruleConfig.Id > 0))
                {
                    string invoiceNumber = string.Empty;

                    if (requestDto.InvoiceType == (int)INVInvoiceType.Monthly)
                    {
                        invoiceNumber = await GetMonthlyInvoiceNumber(invoiceList, ruleConfig, requestDto, groupByValue, featureList);
                    }
                    else
                    {
                        invoiceNumber = await GetInvoiceNumber(invoiceList, ruleConfig, requestDto, groupByValue, featureList);
                    }

                    foreach (var invoice in invoiceList)
                    {
                        if (invoice.GroupBy.Equals(groupByValue))
                        {
                            invoice.InvoiceNo = invoiceNumber;
                        }
                    }
                    // Post date
                    if (invoiceList.Any())
                    {
                        var postDate = this.GetPostDateByInvoiceNo(invoiceNumber, invoiceList);

                        foreach (var invoice in invoiceList)
                        {
                            if (invoice.InvoiceNo != null && invoice.InvoiceNo.Equals(invoiceNumber))
                            {
                                invoice.PostedDate = postDate;
                            }
                        }
                    }

                }
            }
        }

        private async Task SetAuditInvoiceNumberAndPostDate(List<InvoiceBookingDetail> bookingList, List<InvoiceDetail> invoiceList, InvoiceGenerateRequest requestDto)
        {
            // Group by, invoice no
            var groupByList = bookingList.Select(o => o.GroupBy).Distinct();

            var featureList = await _referenceRepository.GetEntityFeatureList();

            foreach (var groupByValue in groupByList)
            {
                var ruleConfig = bookingList.Where(o => o.GroupBy == groupByValue).Select(o => o.RuleConfig).FirstOrDefault();
                if ((ruleConfig != null) && (ruleConfig.Id > 0))
                {
                    string invoiceNumber = string.Empty;

                    if (requestDto.InvoiceType == (int)INVInvoiceType.Monthly)
                    {
                        invoiceNumber = await GetMonthlyInvoiceNumber(invoiceList, ruleConfig, requestDto, groupByValue, featureList);
                    }
                    else
                    {
                        invoiceNumber = await GetInvoiceNumber(invoiceList, ruleConfig, requestDto, groupByValue, featureList);
                    }

                    foreach (var invoice in invoiceList)
                    {
                        if (invoice.GroupBy.Equals(groupByValue))
                        {
                            invoice.InvoiceNo = invoiceNumber;
                        }
                    }
                    // Post date
                    if (invoiceList.Any())
                    {
                        var postDate = this.GetAuditPostDateByInvoiceNo(invoiceNumber, invoiceList);

                        foreach (var invoice in invoiceList)
                        {
                            if (invoice.InvoiceNo != null && invoice.InvoiceNo.Equals(invoiceNumber))
                            {
                                invoice.PostedDate = postDate;
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Set invoice number for new booking 
        /// </summary>
        /// <param name="bookingList"></param>
        /// <param name="invoiceList"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task SetInvoiceNumberAndPostDateForNewBooking(List<InvoiceDetail> invoiceList, InvoiceGenerateRequest requestDto)
        {

            // set invoice number for available booking
            if (!string.IsNullOrWhiteSpace(requestDto.InvoiceNumber))
            {

                var invoiceDetails = await GetInvoiceDetailsbyInvoiceNumber(requestDto.InvoiceNumber);

                foreach (var invoice in invoiceList)
                {
                    invoice.InvoiceNo = requestDto.InvoiceNumber;

                    if (invoiceDetails != null)
                    {
                        invoice.InvoiceDate = invoiceDetails.InvoiceDate;
                        invoice.InvoicePaymentDate = invoiceDetails.PaymentDate;
                        invoice.InvoicePaymentStatus = invoiceDetails.PaymentStatus;
                    }
                }
            }

            // Post date
            if (invoiceList.Any())
            {
                var postDate = this.GetPostDateByInvoiceNo(requestDto.InvoiceNumber, invoiceList);

                foreach (var invoice in invoiceList)
                {
                    if (invoice.InvoiceNo != null)
                    {
                        invoice.PostedDate = postDate;
                    }
                }
            }
        }
        /// <summary>
        /// Set invoice number and post date
        /// </summary>
        /// <param name="invoiceList"></param>
        /// <param name="requestDto"></param>
        private void SetInvoiceNumberAndPostDateForNewAudit(List<InvoiceDetail> invoiceList, InvoiceGenerateRequest requestDto)
        {

            // set invoice number for available booking
            foreach (var invoice in invoiceList)
            {
                if (!string.IsNullOrWhiteSpace(requestDto.InvoiceNumber))
                {
                    invoice.InvoiceNo = requestDto.InvoiceNumber;
                }
            }

            // Post date
            if (invoiceList.Any())
            {
                var postDate = this.GetAuditPostDateByInvoiceNo(requestDto.InvoiceNumber, invoiceList);

                foreach (var invoice in invoiceList)
                {
                    if (invoice.InvoiceNo != null)
                    {
                        invoice.PostedDate = postDate;
                    }
                }
            }
        }

        /// <summary>
        /// get post date for invoice
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="invoiceList"></param>
        /// <returns></returns>
        private DateTime? GetPostDateByInvoiceNo(string invoiceNo, List<InvoiceDetail> invoiceList)
        {
            var bookingIds = from invoice in invoiceList
                             where invoice.InvoiceNo == invoiceNo
                             select invoice.InspectionId;

            var inspectionDate = _invoiceRepository.GetInvoiceBookingLastDate(bookingIds);
            return inspectionDate;
        }

        /// <summary>
        /// Get Audit post date by invoice number
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="invoiceList"></param>
        /// <returns></returns>
        private DateTime? GetAuditPostDateByInvoiceNo(string invoiceNo, List<InvoiceDetail> invoiceList)
        {
            var auditIds = from invoice in invoiceList
                           where invoice.InvoiceNo == invoiceNo
                           select invoice.AuditId;

            var auditDate = _invoiceRepository.GetInvoiceAuditLastDate(auditIds);
            return auditDate;
        }

        /// <summary>
        /// get invoice details by invoice no
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>

        private async Task<InvoiceDataForNewBooking> GetInvoiceDetailsbyInvoiceNumber(string invoiceNumber)
        {

            var invoiceData = await _invoiceRepository.GetInvoiceDetailsbyInvoiceNumber(invoiceNumber);
            return invoiceData;
        }

        /// <summary>
        /// get invoice number 
        /// </summary>
        /// <param name="invoiceList"></param>
        /// <param name="ruleConfig"></param>
        /// <param name="requestDto"></param>
        /// <param name="groupByValue"></param>
        /// <returns></returns>
        private async Task<string> GetInvoiceNumber(List<InvoiceDetail> invoiceList,
                 CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, string groupByValue, List<EntFeatureDetail> entFeatureDetails)
        {
            var prefix = "INV"; // default prefix
            var invoiceNo = 0;
            var invoiceDigitLimit = 4;
            var rulePreFix = "C"; // default rule prefix

            if (requestDto.InvoiceTo == (int)InvoiceTo.Customer)
            {
                rulePreFix = "C";
            }
            else if (requestDto.InvoiceTo == (int)InvoiceTo.Supplier)
            {
                rulePreFix = "S";
            }
            else if (requestDto.InvoiceTo == (int)InvoiceTo.Factory)
            {
                rulePreFix = "F";
            }

            if (!string.IsNullOrWhiteSpace(ruleConfig.InvoiceNoPrefix))
            {
                prefix = ruleConfig.InvoiceNoPrefix;
            }

            if (!string.IsNullOrWhiteSpace(ruleConfig.InvoiceNoDigit))
            {
                invoiceDigitLimit = Int32.Parse(ruleConfig.InvoiceNoDigit);
            }

            var factoryCountry = string.Empty;
            var year = DateTime.Now.ToString("yy");
            var order = invoiceList.FirstOrDefault(o => o.GroupBy == groupByValue);

            if (order != null)
            {
                if (string.IsNullOrEmpty(order.FactoryCountryCode))
                {
                    factoryCountry = order.FactoryCountryName;
                }
                else
                {
                    factoryCountry = order.FactoryCountryCode;
                }
            }
            prefix = prefix + "-" + factoryCountry + "-" + rulePreFix + year;

            var currentInvoiceNo = invoiceList.Where(i => !string.IsNullOrWhiteSpace(i.InvoiceNo) && i.InvoiceNo.StartsWith(prefix)).Max(i => i.InvoiceNo);

            if (!string.IsNullOrWhiteSpace(currentInvoiceNo) && prefix.Length + invoiceDigitLimit <= currentInvoiceNo.Length)
            {
                currentInvoiceNo = currentInvoiceNo.Substring(prefix.Length + 1, invoiceDigitLimit);

                if (int.TryParse(currentInvoiceNo, out invoiceNo))
                {
                    invoiceNo = invoiceNo + 1;
                }
            }
            else
            {
                var dbInvoiceNo = await _invoiceRepository.GetInvoiceNumber(prefix);

                if (!string.IsNullOrWhiteSpace(dbInvoiceNo))
                {
                    var strInvoiceNumber = dbInvoiceNo.Substring((dbInvoiceNo.Length + 1) - invoiceDigitLimit);

                    if (int.TryParse(strInvoiceNumber, out invoiceNo))
                    {
                        invoiceNo = invoiceNo + 1;
                    }
                }
            }

            if (invoiceNo == 0)
            {
                invoiceNo = 1;
            }

            string invoiceNumber = string.Empty;
            do
            {

                invoiceNumber = FormatInvoiceNumber(invoiceNo, invoiceDigitLimit, prefix);
                invoiceNo = invoiceNo + 1;

            } while (await _invoiceRepository.CheckInvoiceNumberExist(invoiceNumber));

            return invoiceNumber;
        }



        private async Task<string> GetMonthlyInvoiceNumber(List<InvoiceDetail> invoiceList,
                CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, string groupByValue, List<EntFeatureDetail> entFeatureDetails)
        {
            var prefix = "INV"; // default prefix
            var invoiceNo = 0;
            var invoiceDigitLimit = 4;
            var rulePreFix = string.Empty;

            // bill to is present then add it 
            if (entFeatureDetails.Any() && entFeatureDetails.Any(x => x.FeatureId == (int)Entities.Enums.EntityFeature.AddBillToInInvoiceNumber))
            {
                if (requestDto.InvoiceTo == (int)InvoiceTo.Customer)
                {
                    rulePreFix = "C";
                }
                else if (requestDto.InvoiceTo == (int)InvoiceTo.Supplier)
                {
                    rulePreFix = "S";
                }
                else if (requestDto.InvoiceTo == (int)InvoiceTo.Factory)
                {
                    rulePreFix = "F";
                }
            }

            if (!string.IsNullOrWhiteSpace(ruleConfig.InvoiceNoPrefix))
            {
                prefix = ruleConfig.InvoiceNoPrefix;
            }

            if (!string.IsNullOrWhiteSpace(ruleConfig.InvoiceNoDigit))
            {
                invoiceDigitLimit = Int32.Parse(ruleConfig.InvoiceNoDigit);
            }

            var factoryCountry = string.Empty;
            var year = DateTime.Now.ToString("yy");
            var order = invoiceList.FirstOrDefault(o => o.GroupBy == groupByValue);

            // factory country is enabled then add it 
            if (order != null && entFeatureDetails.Any() && entFeatureDetails.Any(x => x.FeatureId == (int)Entities.Enums.EntityFeature.AddFactoryCountryInInvoiceNumber))
            {
                if (string.IsNullOrEmpty(order.FactoryCountryCode))
                {
                    factoryCountry = order.FactoryCountryName;
                }
                else
                {
                    factoryCountry = order.FactoryCountryCode;
                }
            }

            if (!string.IsNullOrEmpty(factoryCountry))
            {
                prefix = prefix + "-" + factoryCountry;
            }

            if (!string.IsNullOrEmpty(rulePreFix))
            {
                prefix = prefix + "-" + rulePreFix;
            }

            prefix = prefix + "-" + year;

            var currentInvoiceNo = invoiceList.Where(i => !string.IsNullOrWhiteSpace(i.InvoiceNo) && i.InvoiceNo.StartsWith(prefix)).Max(i => i.InvoiceNo);

            if (!string.IsNullOrWhiteSpace(currentInvoiceNo) && prefix.Length + invoiceDigitLimit <= currentInvoiceNo.Length)
            {
                currentInvoiceNo = currentInvoiceNo.Substring(prefix.Length + 1, invoiceDigitLimit);

                if (int.TryParse(currentInvoiceNo, out invoiceNo))
                {
                    invoiceNo = invoiceNo + 1;
                }
            }
            else
            {
                var dbInvoiceNo = await _invoiceRepository.GetInvoiceNumber(prefix);

                if (!string.IsNullOrWhiteSpace(dbInvoiceNo))
                {
                    var strInvoiceNumber = dbInvoiceNo.Substring((dbInvoiceNo.Length + 1) - invoiceDigitLimit);

                    if (int.TryParse(strInvoiceNumber, out invoiceNo))
                    {
                        invoiceNo = invoiceNo + 1;
                    }
                }
            }

            if (invoiceNo == 0)
            {
                invoiceNo = 1;
            }

            string invoiceNumber = string.Empty;
            do
            {

                invoiceNumber = FormatInvoiceNumber(invoiceNo, invoiceDigitLimit, prefix);
                invoiceNo = invoiceNo + 1;

            } while (await _invoiceRepository.CheckInvoiceNumberExist(invoiceNumber));

            return invoiceNumber;
        }





        /// <summary>
        /// Format Invoice number 
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="invoiceDigitLimit"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string FormatInvoiceNumber(int invoiceNo, int invoiceDigitLimit, string prefix)
        {
            var strInvoiceNo = invoiceNo + string.Empty;
            var appendLength = invoiceDigitLimit - strInvoiceNo.Length;
            if (appendLength >= 0)
            {
                for (var i = 1; i <= appendLength; i++)
                {
                    strInvoiceNo = "0" + strInvoiceNo;
                }
            }
            else
            {
                return string.Empty;
            }

            var invoiceNumber = prefix + "-" + strInvoiceNo;
            return invoiceNumber;
        }

        /// <summary>
        /// Calculate inspection fees by sampling
        /// </summary>
        /// <param name="orderList"></param>
        /// <returns></returns>
        private async Task<List<InvoiceDetail>> CalculateInspectionFeesFromInvoice(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings)
        {

            // get only rule specific inspection
            var orderList = (from order in orderTransactions
                             where order.RuleConfig.Id == ruleConfig.Id
                             select order).ToList();

            var bookingIds = orderList.Select(x => x.BookingId).Distinct().ToList();

            foreach (var orderTransaction in orderList)
            {
                // check already booking is not added for invoice
                if (!invoiceList.Any(x => x.InspectionId == orderTransaction.BookingId))
                {
                    var samplingUnitprice = await GetSamplingAndUnitPrice(orderTransaction.BookingId, ruleConfig);
                    double discountFees = 0;
                    double otherFees = 0;
                    double manDays = 0;
                    string strRemarks = string.Empty;

                    //If the calculated amount greater than Max.Fee then take MaxFee else take calculated Amount. 
                    if (ruleConfig.MaxFeeStyle > 0 && samplingUnitprice.UnitPrice > ruleConfig.MaxFeeStyle)
                    {
                        samplingUnitprice.UnitPrice = ruleConfig.MaxFeeStyle.GetValueOrDefault();
                        strRemarks = ApiCommonData.MaximumFee + samplingUnitprice.UnitPrice.ToString(ApiCommonData.NumberFormat);
                    }

                    //if the calculate unit price less than the min billing day take the min billing day
                    if (ruleConfig.MinBillingDay > 0 && samplingUnitprice.UnitPrice < ruleConfig.MinBillingDay)
                    {
                        samplingUnitprice.UnitPrice = ruleConfig.MinBillingDay.GetValueOrDefault();
                        strRemarks = ApiCommonData.MinimumFee + samplingUnitprice.UnitPrice.ToString(ApiCommonData.NumberFormat);
                    }

                    var invoiceTransaction = new InvoiceDetail()
                    {
                        IsAutomation = true,
                        ExchangeRate = null,
                        InvoiceMethod = ruleConfig.BillingMethodId,
                        InvoiceCurrency = ruleConfig.CurrencyId,
                        PriceCardCurrency = ruleConfig.CurrencyId,
                        InspectionDate = orderTransaction.ServiceTo,
                        RuleExchangeRate = null,
                        InvoiceDate = DateTime.Now.Date,
                        InvoiceStatus = (int)InvoiceStatus.Created,
                        InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                        FactoryId = orderTransaction.FactoryId,
                        FactoryCountryId = orderTransaction.FactoryCountryId,
                        FactoryCountryName = orderTransaction.FactoryCountryName,
                        FactoryCountryCode = orderTransaction.FactoryCountryCode,

                        FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                        FactoryProvinceId = orderTransaction.FactoryProvinceId.GetValueOrDefault(),
                        FactoryCityId = orderTransaction.FactoryCityId.GetValueOrDefault(),

                        UnitPrice = samplingUnitprice.UnitPrice,
                        InspectionFees = samplingUnitprice.UnitPrice,
                        TotalSampleSize = samplingUnitprice.TotalSamplingSize,
                        TotalInvoiceFees = 0,
                        InspectionId = orderTransaction.BookingId,
                        Subject = "",
                        OtherFees = otherFees,
                        Discount = discountFees,
                        ManDays = manDays,
                        RuleId = ruleConfig.Id,
                        InvoiceType = requestDto.InvoiceType,
                        TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                        InvoiceTo = ruleConfig.BillingToId,
                        CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                        CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                        CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                        CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                        CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                        BankId = ruleConfig.BankAccount,
                        PaymentTerms = ruleConfig.PaymentTerms,
                        PaymentDuration = ruleConfig.PaymentDuration.ToString(),
                        IsInspection = true,
                        InvoicePaymentDate = null,
                        Remarks = strRemarks,
                        GroupBy = orderTransaction.GroupBy,
                        Rule = ruleConfig,
                        BookingDepartmentIds = orderTransaction.DepartmentIds,
                        BookingBuyerIds = orderTransaction.BuyerIds,
                        BookingBrandIds = orderTransaction.BrandIds,
                        Office = ruleConfig.InvoiceOffice
                    };
                    invoiceList.Add(invoiceTransaction);
                }
            }
            return invoiceList;
        }

        /// <summary>
        /// Calculate Inspection Fees By CarrefourSampling
        /// </summary>
        /// <param name="orderTransactions"></param>
        /// <param name="invoiceList"></param>
        /// <param name="ruleConfig"></param>
        /// <returns></returns>
        public async Task<List<InvoiceDetail>> CalculateInspectionFeesByCarrefourSampling(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings, bool isFromQuotation = false)
        {

            double prorateUnitPricePerBooking = 0;
            // get only rule specific inspection
            var orderList = (from order in orderTransactions
                             where order.RuleConfig.Id == ruleConfig.Id
                             select order).ToList();

            // group booking by service todate and factory and price category
            var groupedBookingOrderList = orderList.GroupBy(x => new { x.ServiceTo, x.FactoryId, x.PriceCategoryId }).Select(y => y).ToList();

            foreach (var bookingDataList in groupedBookingOrderList)
            {
                // get booking id list per day and factory
                var perDayFactoryAndPriceBookingIdList = bookingDataList.Select(x => x.BookingId).ToList();

                // get unit price per day and factory with entire booking products
                var samplingUnitpriceDetails = await GetCarrefourUnitPrice(perDayFactoryAndPriceBookingIdList, ruleConfig);

                var quotationDataList = await _invoiceRepository.GetQuotationDataByBookingIdsList(perDayFactoryAndPriceBookingIdList);

                foreach (var bookingData in bookingDataList)
                {
                    // check already booking is not added for invoice
                    if (!invoiceList.Any(x => x.InspectionId == bookingData.BookingId))
                    {
                        // skip master products and take other products and sum booking quantity from per day and factory booking list
                        double totalQtyPerBooking = samplingUnitpriceDetails.bookingProductList.
                            Where(x => x.BookingId == bookingData.BookingId && (x.IsDispalyMaster == null || !x.IsDispalyMaster.GetValueOrDefault()))
                            .Select(x => x.BookingQuantity).Sum();

                        double totalBookingQty = samplingUnitpriceDetails.bookingProductList.
                       Where(x => x.IsDispalyMaster == null || !x.IsDispalyMaster.GetValueOrDefault())
                      .Select(x => x.BookingQuantity).Sum();


                        // set minimum billing fees

                        if (ruleConfig.MinBillingDay != null && ruleConfig.MinBillingDay > samplingUnitpriceDetails.UnitPrice)
                        {
                            samplingUnitpriceDetails.UnitPrice = ruleConfig.MinBillingDay.GetValueOrDefault();
                        }

                        if (totalBookingQty > 0)
                            // calculate unit price based on prorate 
                            prorateUnitPricePerBooking = (totalQtyPerBooking / totalBookingQty)
                                                              * samplingUnitpriceDetails.UnitPrice;

                        prorateUnitPricePerBooking = Math.Round(prorateUnitPricePerBooking, InvoiceRoundUpValue,
                                                     MidpointRounding.AwayFromZero);

                        // set discount fees and other fees from quotation if already configured
                        double discountFees = 0;
                        double otherFees = 0;
                        double manDays = 0;

                        var quotationData = quotationDataList.FirstOrDefault(x => x.BookingId == bookingData.BookingId);

                        // set other fees and discount,mandays from quotation
                        if (quotationData != null)
                        {
                            // add discount and other fees only one time for quotation level.
                            if ((!invoiceList.Any(x => x.QuotationId == quotationData.QuotationId)) &&
                      (quotationBookings == null || (quotationBookings != null && !quotationBookings.Any(x => x.QuotationId ==
                       quotationData.QuotationId))))
                            {
                                if (ruleConfig.InvoiceOtherFeeFrom == (int)InvoiceFeesFrom.Quotation)
                                    otherFees = quotationData.OtherCost;

                                if (ruleConfig.InvoiceDiscountFeeFrom == (int)InvoiceFeesFrom.Quotation)
                                    discountFees = quotationData.Discount;
                            }

                            manDays = quotationData.Mandays;
                        }


                        // if quotation data billed to and invoice ui billed to same then generate invoice.
                        if (quotationData != null && quotationData.BillingTo == requestDto.InvoiceTo)
                        {
                            var invoiceTransaction = new InvoiceDetail()
                            {
                                IsAutomation = true,
                                ExchangeRate = null,
                                InvoiceMethod = ruleConfig.BillingMethodId,
                                InvoiceCurrency = ruleConfig.CurrencyId,
                                PriceCardCurrency = ruleConfig.CurrencyId,
                                InspectionDate = bookingData.ServiceTo,
                                RuleExchangeRate = null,
                                InvoiceDate = DateTime.Now.Date,
                                InvoiceStatus = (int)InvoiceStatus.Created,
                                InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                                FactoryId = bookingData.FactoryId,
                                FactoryCountryId = bookingData.FactoryCountryId,
                                FactoryCountyId = bookingData.FactoryCountyId.GetValueOrDefault(),
                                FactoryCountryName = bookingData.FactoryCountryName,
                                FactoryCountryCode = bookingData.FactoryCountryCode,
                                UnitPrice = prorateUnitPricePerBooking,// no unit price concept for sampling 
                                InspectionFees = prorateUnitPricePerBooking,
                                TotalSampleSize = samplingUnitpriceDetails.TotalSamplingSize,
                                TotalInvoiceFees = 0,
                                Discount = discountFees,
                                OtherFees = otherFees,
                                InspectionId = bookingData.BookingId,
                                Subject = "",
                                ManDays = manDays,
                                RuleId = ruleConfig.Id,
                                TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                                InvoiceTo = ruleConfig.BillingToId,
                                CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                                CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                                CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                                CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                                CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                                BankId = ruleConfig.BankAccount,
                                PaymentTerms = quotationData.PaymentTermsValue,
                                PaymentDuration = quotationData.PaymentTermsCount.ToString(),
                                IsInspection = true,
                                InvoicePaymentDate = null,
                                Remarks = "Calculate Inspection Fees By Carrefour Sampling - Total Qty Per booking = " +
                               "" + totalQtyPerBooking.ToString() + " Total Booking Quantity =" + totalBookingQty.ToString() +
                               "Total Unit Price =" + samplingUnitpriceDetails.UnitPrice.ToString(),
                                GroupBy = bookingData.GroupBy,
                                Rule = ruleConfig,
                                BookingDepartmentIds = bookingData.DepartmentIds,
                                BookingBuyerIds = bookingData.BuyerIds,
                                BookingBrandIds = bookingData.BrandIds,
                                InvoiceType = requestDto.InvoiceType,
                                Office = ruleConfig.InvoiceOffice,
                                QuotationId = quotationData.QuotationId,
                                ProrateBookingNumbers = string.Join(",", perDayFactoryAndPriceBookingIdList.Select(bookingId => bookingId.ToString()).ToArray())
                            };
                            invoiceList.Add(invoiceTransaction);
                        }
                        else if (isFromQuotation)
                        {
                            var invoiceTransaction = new InvoiceDetail()
                            {
                                InspectionId = bookingData.BookingId,
                                CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                                InspectionFees = prorateUnitPricePerBooking,
                                OtherFees = otherFees
                            };
                            invoiceList.Add(invoiceTransaction);
                        }
                    }
                }

                // check sampling unit price and prorate price are equal 
                var prorateTotalPerDay = Math.Round(invoiceList.Where(x => perDayFactoryAndPriceBookingIdList.
                Contains(x.InspectionId.GetValueOrDefault())).
                Select(x => x.InspectionFees).Sum().GetValueOrDefault(), InvoiceRoundUpValue,
                                                     MidpointRounding.AwayFromZero);


                var samplingUnitPrice = samplingUnitpriceDetails.UnitPrice;

                if (prorateTotalPerDay != samplingUnitPrice)
                {
                    var difference = samplingUnitPrice - prorateTotalPerDay;

                    if (difference <= Math.Abs(ProrateMaxRoundValue))
                    {
                        var lastRecord = invoiceList.LastOrDefault(x => perDayFactoryAndPriceBookingIdList.
                                          Contains(x.InspectionId.GetValueOrDefault()));

                        if (lastRecord != null)
                        {
                            lastRecord.InspectionFees = Math.Round((lastRecord.InspectionFees + difference).GetValueOrDefault(),
                                                        InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
                        }
                    }
                }
            }

            return invoiceList;
        }



        /// <summary>
        /// Calculate Inspection Fees From Carrefour ByManDay
        /// </summary>
        /// <param name="orderTransactions"></param>
        /// <param name="invoiceList"></param>
        /// <param name="ruleConfig"></param>
        /// <returns></returns>
        public async Task<List<InvoiceDetail>> CalculateInspectionFeesFromCarrefourByManDay(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings, bool isFromQuotation = false)
        {

            var nonPickingService = new List<int> { (int)InspectionServiceTypeEnum.FinalRandomInspection, (int)InspectionServiceTypeEnum.FinalRandomReInspection, (int)InspectionServiceTypeEnum.RemoteInspectionmonitoring };

            var pickingservicetype = _configuration["Picking_ServiceType"].Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Int32.Parse(x.Trim())).ToList();
            // Rule has picking service type then calculate the inspection fees
            if (ruleConfig.ServiceTypeIdList.Any(y => pickingservicetype.Any(z => z == y)))
            {
                // group booking by service todate and factory
                var groupedBookingOrderList = orderTransactions.GroupBy(x => new { x.ServiceTo, x.FactoryId }).Select(y => y).ToList();

                foreach (var perdayAndFactoryBookings in groupedBookingOrderList)
                {
                    // get booking service type with picking  order list
                    var pickingOrderList = (from order in perdayAndFactoryBookings.Select(x => x)
                                            where order.ServiceTypeIds.Any(y => pickingservicetype.Any(z => z == y))
                                            select order).ToList();

                    // get booking service type with FR and FR-RE order list
                    var nonPickingOrderList = (from order in perdayAndFactoryBookings.Select(x => x)
                                               where nonPickingService.Any(x => order.ServiceTypeIds.Any(y => y == x))
                                               select order).ToList();

                    if (pickingOrderList.Count > 0)
                    {
                        var nonPickingOrderInspection = nonPickingOrderList.Select(x => x.BookingId).ToList();
                        // get non picking products by booking
                        var nonPickingProductList = await _invoiceRepository.GetProductDetailsByBookingList(nonPickingOrderInspection);


                        var pickingBookingNumber = pickingOrderList.Select(x => x.BookingId).ToList();

                        // get picking products by booking
                        var pickingProductList = await _invoiceRepository.GetProductDetailsByBookingList(pickingBookingNumber);

                        int pickingProductCount = 0;
                        double inspectionFees = 0;
                        int maxProductCount = ruleConfig.MaxProductCount.GetValueOrDefault();

                        foreach (var pickingProduct in pickingProductList)
                        {
                            // check picking product is not exist in the service type FRI-FRIRE
                            if (!nonPickingProductList.Any(x => x.ProductId == pickingProduct.ProductId))
                            {
                                pickingProductCount = pickingProductCount + 1;
                            }
                        }

                        // if we have non picking products and check picking product count
                        if (nonPickingProductList.Any() && pickingProductCount <= PickingCount)
                        {
                            inspectionFees = 0;
                            await AddInvoiceList(pickingOrderList, invoiceList, ruleConfig, inspectionFees, requestDto, pickingProductList, quotationBookings, isFromQuotation);
                        }
                        else
                        {
                            // if we have non picking products and check max product count with picking product count.
                            if (nonPickingProductList.Any() && maxProductCount >= pickingProductCount)
                            {
                                inspectionFees = ruleConfig.UnitPrice;
                                await AddInvoiceList(pickingOrderList, invoiceList, ruleConfig, inspectionFees, requestDto, pickingProductList, quotationBookings, isFromQuotation);
                            }
                            else
                            {
                                // PriceToEachProduct checked means

                                if (ruleConfig.PriceToEachProduct.GetValueOrDefault() && maxProductCount > 0)
                                {
                                    inspectionFees = inspectionFees + ruleConfig.UnitPrice;

                                    var reminingPickingProductCount = pickingProductCount - maxProductCount;

                                    // remining picking product count logic changed 

                                    // if the picking product is 12 then 12- 5(max product count) = 7 , [ruleConfig.UnitPrice +  7 * product price]

                                    if (reminingPickingProductCount > 0)
                                    {
                                        inspectionFees = inspectionFees + (reminingPickingProductCount * ruleConfig.ProductPrice.GetValueOrDefault());
                                    }
                                    await AddInvoiceList(pickingOrderList, invoiceList, ruleConfig, inspectionFees, requestDto, pickingProductList, quotationBookings, isFromQuotation);
                                }
                                else
                                {
                                    if (maxProductCount > 0)
                                    {
                                        do
                                        {
                                            inspectionFees = inspectionFees + ruleConfig.UnitPrice;
                                            pickingProductCount = pickingProductCount - maxProductCount;
                                        }
                                        while (pickingProductCount > 0 && maxProductCount <= pickingProductCount);

                                        if (pickingProductCount > 0)
                                        {
                                            inspectionFees = inspectionFees + (pickingProductCount * ruleConfig.UnitPrice);
                                        }
                                        await AddInvoiceList(pickingOrderList, invoiceList, ruleConfig, inspectionFees, requestDto, pickingProductList, quotationBookings, isFromQuotation);
                                    }
                                }
                            }

                        }
                    }
                }
            }

            return invoiceList;
        }

        private async Task AddInvoiceList(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList,
            CustomerPriceCardRepo ruleConfig, double inspectionFees, InvoiceGenerateRequest requestDto, List<CustomerPriceBookingProducts> pickingProducts, List<QuotationBooking> quotationBookings, bool isFromQuotation)
        {

            if (orderTransactions != null && orderTransactions.Any())
            {
                var bookingIds = orderTransactions.Select(x => x.BookingId).Distinct().ToList();
                var quotationDataList = await _invoiceRepository.GetQuotationDataByBookingIdsList(bookingIds);

                foreach (var orderTransaction in orderTransactions)
                {
                    // check invoice booking already added.
                    if (!invoiceList.Any(x => x.InspectionId == orderTransaction.BookingId))
                    {
                        double discountFees = 0;
                        double otherFees = 0;
                        double manDays = 0;

                        double prorateInspectionFees = 0;

                        if (pickingProducts.Count > 0)
                            prorateInspectionFees = (Convert.ToDouble(pickingProducts.Count(x => x.BookingId == orderTransaction.BookingId))
                               / Convert.ToDouble(pickingProducts.Count)) * inspectionFees;

                        var quotationData = quotationDataList.FirstOrDefault(x => x.BookingId == orderTransaction.BookingId);

                        if (quotationData != null)
                        {
                            // add discount and other fees only one time for quotation level.
                            if ((!invoiceList.Any(x => x.QuotationId == quotationData.QuotationId)) &&
                        (quotationBookings == null || (quotationBookings != null && !quotationBookings.Any(x => x.QuotationId ==
                         quotationData.QuotationId))))
                            {
                                if (ruleConfig.InvoiceOtherFeeFrom == (int)InvoiceFeesFrom.Quotation)
                                    otherFees = quotationData.OtherCost;

                                if (ruleConfig.InvoiceDiscountFeeFrom == (int)InvoiceFeesFrom.Quotation)
                                    discountFees = quotationData.Discount;
                            }
                            manDays = quotationData.Mandays;
                        }

                        // if quotation data billed to and invoice ui billed to same then generate invoice.
                        if (quotationData != null && quotationData.BillingTo == requestDto.InvoiceTo)
                        {
                            var invoiceTransaction = new InvoiceDetail()
                            {
                                IsAutomation = true,
                                ExchangeRate = null,
                                InvoiceMethod = ruleConfig.BillingMethodId,
                                InvoiceCurrency = ruleConfig.CurrencyId,
                                PriceCardCurrency = ruleConfig.CurrencyId,
                                RuleExchangeRate = null,
                                InvoiceDate = DateTime.Now,
                                InspectionDate = orderTransaction.ServiceTo,
                                InvoiceStatus = (int)InvoiceStatus.Created,
                                InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                                FactoryId = orderTransaction.FactoryId,
                                FactoryCountryId = orderTransaction.FactoryCountryId,
                                FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                                FactoryCountryName = orderTransaction.FactoryCountryName,
                                FactoryCountryCode = orderTransaction.FactoryCountryCode,
                                UnitPrice = prorateInspectionFees,
                                InspectionFees = prorateInspectionFees,
                                TotalInvoiceFees = 0,
                                OtherFees = otherFees,
                                Discount = discountFees,
                                InspectionId = orderTransaction.BookingId,
                                Subject = "",
                                ManDays = manDays,
                                RuleId = ruleConfig.Id,
                                TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                                InvoiceTo = ruleConfig.BillingToId,
                                CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                                CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                                CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                                CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                                CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                                BankId = ruleConfig.BankAccount,
                                PaymentTerms = quotationData.PaymentTermsValue,
                                PaymentDuration = quotationData.PaymentTermsCount?.ToString(),
                                IsInspection = true,
                                InvoicePaymentDate = null,
                                Remarks = "Calculate Inspection Fees By Carrefour Manday - Total Inspection Fees =" + prorateInspectionFees.ToString(),
                                GroupBy = orderTransaction.GroupBy,
                                Rule = ruleConfig,
                                BookingDepartmentIds = orderTransaction.DepartmentIds,
                                BookingBuyerIds = orderTransaction.BuyerIds,
                                BookingBrandIds = orderTransaction.BrandIds,
                                InvoiceType = requestDto.InvoiceType,
                                Office = ruleConfig.InvoiceOffice,
                                QuotationId = quotationData.QuotationId
                            };
                            invoiceList.Add(invoiceTransaction);
                        }
                        else if (isFromQuotation)
                        {
                            var invoiceTransaction = new InvoiceDetail()
                            {
                                InspectionId = orderTransaction.BookingId,
                                CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                                InspectionFees = prorateInspectionFees,
                                OtherFees = otherFees,
                            };
                            invoiceList.Add(invoiceTransaction);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// GetInvoice List by Price complex type
        /// </summary>
        /// <param name="orderTransactions"></param>
        /// <param name="invoiceList"></param>
        /// <param name="ruleConfig"></param>
        /// <param name="requestDto"></param>
        /// <param name="quotationBookings"></param>
        /// <returns></returns>
        public async Task<List<InvoiceDetail>> GetInvoiceListbyPriceCalculations(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings, bool isFromQuotation = false)

        {
            switch (ruleConfig.BillingMethodId)
            {
                case (int)PriceBillingMethod.ManDay:

                    if (ruleConfig.InvoiceRequestType != null)
                    {
                        switch (ruleConfig.InvoiceInspFeeFrom)
                        {
                            case (int)InvoiceFeesFrom.Quotation:

                                invoiceList = await CalculateInspectionFeesFromQuotation(orderTransactions, invoiceList, ruleConfig, requestDto, quotationBookings);

                                break;

                            case (int)InvoiceFeesFrom.Invoice:

                                if (ruleConfig.PriceComplexType == (int)PriceComplexType.Complex)
                                {
                                    invoiceList = await CalculateInspectionFeesFromInvoiceMandayRule(orderTransactions, invoiceList, ruleConfig, requestDto);
                                }

                                else
                                {
                                    invoiceList = await CalculateInspectionFeesFromInvoiceMandaySimpleRule(orderTransactions, invoiceList, ruleConfig, requestDto);
                                }

                                break;

                            case (int)InvoiceFeesFrom.Carrefour:
                                invoiceList = await CalculateInspectionFeesFromCarrefourByManDay(orderTransactions, invoiceList, ruleConfig, requestDto, quotationBookings, isFromQuotation);
                                break;
                        }
                    }
                    break;

                case (int)PriceBillingMethod.Sampling:

                    if (ruleConfig.InvoiceRequestType != null)
                    {
                        switch (ruleConfig.InvoiceInspFeeFrom)
                        {
                            case (int)InvoiceFeesFrom.Invoice:

                                if (ruleConfig.PriceComplexType == (int)PriceComplexType.Complex)
                                {
                                    invoiceList = await CalculateInspectionFeesFromInvoice(orderTransactions, invoiceList, ruleConfig, requestDto, quotationBookings);
                                }
                                else
                                {
                                    invoiceList = await CalculateInspectionFeesFromInvoice(orderTransactions, invoiceList, ruleConfig, requestDto, quotationBookings);
                                }

                                break;
                            case (int)InvoiceFeesFrom.Quotation:
                                invoiceList = await CalculateInspectionFeesFromQuotation(orderTransactions, invoiceList, ruleConfig, requestDto, quotationBookings);
                                break;
                            case (int)InvoiceFeesFrom.Carrefour:
                                // Add sorting by customer booking number
                                invoiceList = await CalculateInspectionFeesByCarrefourSampling(orderTransactions, invoiceList, ruleConfig, requestDto, quotationBookings, isFromQuotation);
                                break;
                        }
                    }
                    break;

                case (int)PriceBillingMethod.PieceRate:

                    if (ruleConfig.InvoiceRequestType != null)
                    {
                        switch (ruleConfig.InvoiceInspFeeFrom)
                        {
                            case (int)InvoiceFeesFrom.Quotation:
                                invoiceList = await CalculateInspectionFeesFromQuotation(orderTransactions, invoiceList, ruleConfig, requestDto, quotationBookings);
                                break;

                            case (int)InvoiceFeesFrom.Invoice:
                                var dicExistingInvoices = await this.GetTransactionIdsByFacIdAndInspectionDate(orderTransactions);
                                invoiceList = await CalculateInspectionFeesFromInvoicePieceRateRule(orderTransactions, invoiceList, ruleConfig, requestDto, dicExistingInvoices);
                                break;
                        }
                    }
                    break;

                case (int)PriceBillingMethod.Intervention:
                    if (ruleConfig.InvoiceRequestType != null)
                    {
                        switch (ruleConfig.InvoiceInspFeeFrom)
                        {
                            case (int)InvoiceFeesFrom.Quotation:
                                invoiceList = await CalculateInspectionFeesFromQuotation(orderTransactions, invoiceList, ruleConfig, requestDto, quotationBookings);
                                break;

                            case (int)InvoiceFeesFrom.Invoice:

                                if (ruleConfig.InterventionType != null && ruleConfig.InterventionType == (int)InterventionType.PerStyle)
                                {
                                    invoiceList = CalculatePerInterventionInspectionPerStyle(orderTransactions, invoiceList, ruleConfig, requestDto);
                                }
                                break;
                        }
                    }
                    break;
            }
            return invoiceList;

        }


        /// <summary>
        /// Calculate per intervention style range
        /// </summary>
        /// <param name="orderList"></param>
        /// <param name="invoices"></param>
        /// <param name="ruleConfig"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private List<InvoiceDetail> CalculatePerInterventionInspectionPerStyle(List<InvoiceBookingDetail> orderList,
            List<InvoiceDetail> invoices, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto)
        {
            double unitPrice = 0;
            if ((ruleConfig.InterventionType != null && ruleConfig.InterventionType == (int)InterventionType.PerStyle) && (ruleConfig.UnitPrice > 0))
            {
                unitPrice = ruleConfig.UnitPrice;
            }

            // Grouping by split booking invoice 
            var groupByList = orderList.Select(o => o.GroupBy).Distinct();

            foreach (var groupByValue in groupByList)
            {
                var groupOrderList = orderList.Where(o => o.GroupBy == groupByValue).ToList();

                var inspections = (from orderConclusion in groupOrderList
                                   orderby orderConclusion.ServiceTo
                                   select new
                                   {
                                       InspectionDate = orderConclusion.ServiceTo,
                                       orderConclusion.BookingId,
                                       InspectionDay = orderConclusion.ServiceTo.Year.ToString() + orderConclusion.ServiceTo.Month.ToString(TWO_DIGIT_FORMAT) + orderConclusion.ServiceTo.Day.ToString(TWO_DIGIT_FORMAT),
                                       InspectionMonth = orderConclusion.ServiceTo.Year.ToString() + orderConclusion.ServiceTo.Month.ToString(TWO_DIGIT_FORMAT),
                                       InspectionWeek = orderConclusion.ServiceTo.Year.ToString() + GetWeek(orderConclusion.ServiceTo).ToString(TWO_DIGIT_FORMAT)
                                   }).ToList();

                // Special rule
                var theSpecialRule = ruleConfig.RuleList.FirstOrDefault();

                if (theSpecialRule != null)
                {
                    if (theSpecialRule.Max_Style_Per_Day != null && theSpecialRule.Max_Style_Per_Day > 0)
                    {
                        // Calculate per day
                        var inspectionPerDays = (from inspection in inspections
                                                 group inspection by inspection.InspectionDay into dayGroup
                                                 select new
                                                 {
                                                     InspectionDay = dayGroup.Key,
                                                     NumberOfInspection = dayGroup.Count()
                                                 }).ToList();
                        foreach (var inspectionPerDay in inspectionPerDays)
                        {
                            var tranSerIdList = inspections.Where(i => i.InspectionDay == inspectionPerDay.InspectionDay)
                                                                .Select(i => i.BookingId).ToList();
                            double totalPrice = 0;
                            var priceCalculationType = (int)PriceCalculationType.Normal;
                            var remark = string.Empty;
                            if (inspectionPerDay.NumberOfInspection >= theSpecialRule.Max_Style_Per_Day)
                            {
                                totalPrice = theSpecialRule.UnitPrice.Value;
                                priceCalculationType = (int)PriceCalculationType.SpecialPrice;
                                remark = "Max Style Per Day: " + theSpecialRule.Max_Style_Per_Day;
                            }
                            else
                            {
                                totalPrice = inspectionPerDay.NumberOfInspection * unitPrice;
                                if (totalPrice < ruleConfig.MinBillingDay)
                                {
                                    totalPrice = ruleConfig.MinBillingDay.GetValueOrDefault();
                                    priceCalculationType = (int)PriceCalculationType.MinFee;
                                }
                            }
                            var returnedInvoices = this.CreateInterventionInvoiceTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, totalPrice, priceCalculationType, remark, requestDto, invoices);
                            invoices.AddRange(returnedInvoices);
                        }
                    }
                    else if (theSpecialRule.Max_Style_Per_Week != null && theSpecialRule.Max_Style_Per_Week > 0)
                    {
                        // Calculate per week
                        var inspectionPerWeeks = (from inspection in inspections
                                                  group inspection by inspection.InspectionWeek into weekGroup
                                                  select new
                                                  {
                                                      InspectionWeek = weekGroup.Key,
                                                      NumberOfInspection = weekGroup.Count()
                                                  }).ToList();
                        var remark = string.Empty;
                        foreach (var inspectionPerWeek in inspectionPerWeeks)
                        {
                            var tranSerIdList = inspections.Where(i => i.InspectionWeek == inspectionPerWeek.InspectionWeek)
                                                                .Select(i => i.BookingId).ToList();
                            double totalPrice = 0;

                            if (inspectionPerWeek.NumberOfInspection >= theSpecialRule.Max_Style_Per_Week)
                            {
                                totalPrice = theSpecialRule.UnitPrice.GetValueOrDefault();
                                var priceCalculationType = (int)PriceCalculationType.SpecialPrice;
                                remark = "Max Style Per Week: " + theSpecialRule.Max_Style_Per_Week;
                                var returnedInvoices = this.CreateInterventionInvoiceTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, totalPrice, priceCalculationType, remark, requestDto, invoices);
                                invoices.AddRange(returnedInvoices);
                            }
                            else
                            {
                                var returnedInvoices = this.CreateNormalInterventionInvoiceTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, requestDto, invoices);
                                invoices.AddRange(returnedInvoices);
                            }
                        }
                    }
                    else if (theSpecialRule.Max_Style_per_Month != null && theSpecialRule.Max_Style_per_Month > 0)
                    {
                        // Calculate per month
                        var inspectionPerMonths = (from inspection in inspections
                                                   group inspection by inspection.InspectionMonth into monthGroup
                                                   select new
                                                   {
                                                       InspectionMonth = monthGroup.Key,
                                                       NumberOfInspection = monthGroup.Count()
                                                   }).ToList();
                        var remark = string.Empty;
                        foreach (var inspectionPerMonth in inspectionPerMonths)
                        {
                            var tranSerIdList = inspections.Where(i => i.InspectionMonth == inspectionPerMonth.InspectionMonth)
                                                                .Select(i => i.BookingId).ToList();
                            double totalPrice = 0;

                            if (inspectionPerMonth.NumberOfInspection >= theSpecialRule.Max_Style_per_Month)
                            {
                                totalPrice = theSpecialRule.UnitPrice.GetValueOrDefault();
                                var priceCalculationType = (int)PriceCalculationType.SpecialPrice;
                                remark = "Max Style Per Month: " + theSpecialRule.Max_Style_per_Month;
                                var returnedInvoices = this.CreateInterventionInvoiceTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, totalPrice, priceCalculationType, remark, requestDto, invoices);
                                invoices.AddRange(returnedInvoices);
                            }
                            else
                            {
                                var returnedInvoices = this.CreateNormalInterventionInvoiceTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, requestDto, invoices);
                                invoices.AddRange(returnedInvoices);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var orderTransaction in groupOrderList)
                    {
                        var invoiceTransaction = this.CreateInterventionInvoiceTransaction(ruleConfig, orderTransaction, unitPrice, unitPrice, string.Empty, (int)PriceCalculationType.Normal, requestDto);
                        invoices.Add(invoiceTransaction);
                    }
                }
            }

            return invoices;
        }



        /// <summary>
        /// Calculate Per intervention style  in audit
        /// </summary>
        /// <param name="orderList"></param>
        /// <param name="invoices"></param>
        /// <param name="ruleConfig"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private List<InvoiceDetail> CalculatePerInterventionAuditPerStyle(List<InvoiceBookingDetail> orderList,
           List<InvoiceDetail> invoices, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto)
        {
            double unitPrice = 0;
            if ((ruleConfig.InterventionType != null && ruleConfig.InterventionType == (int)InterventionType.PerStyle) && (ruleConfig.UnitPrice > 0))
            {
                unitPrice = ruleConfig.UnitPrice;
            }

            // Grouping by split booking invoice 
            var groupByList = orderList.Select(o => o.GroupBy).Distinct();

            foreach (var groupByValue in groupByList)
            {
                var groupOrderList = orderList.Where(o => o.GroupBy == groupByValue).ToList();

                var inspections = (from orderConclusion in groupOrderList
                                   orderby orderConclusion.ServiceTo
                                   select new
                                   {
                                       InspectionDate = orderConclusion.ServiceTo,
                                       orderConclusion.AuditId,
                                       InspectionDay = orderConclusion.ServiceTo.Year.ToString() + orderConclusion.ServiceTo.Month.ToString(TWO_DIGIT_FORMAT) + orderConclusion.ServiceTo.Day.ToString(TWO_DIGIT_FORMAT),
                                       InspectionMonth = orderConclusion.ServiceTo.Year.ToString() + orderConclusion.ServiceTo.Month.ToString(TWO_DIGIT_FORMAT),
                                       InspectionWeek = orderConclusion.ServiceTo.Year.ToString() + GetWeek(orderConclusion.ServiceTo).ToString(TWO_DIGIT_FORMAT)
                                   }).ToList();

                // Special rule
                var theSpecialRule = ruleConfig.RuleList.FirstOrDefault();

                if (theSpecialRule != null)
                {
                    if (theSpecialRule.Max_Style_Per_Day != null && theSpecialRule.Max_Style_Per_Day > 0)
                    {
                        // Calculate per day
                        var auditPerDays = (from inspection in inspections
                                            group inspection by inspection.InspectionDay into dayGroup
                                            select new
                                            {
                                                InspectionDay = dayGroup.Key,
                                                NumberOfAudit = dayGroup.Count()
                                            }).ToList();
                        foreach (var auditPerDay in auditPerDays)
                        {
                            var tranSerIdList = inspections.Where(i => i.InspectionDay == auditPerDay.InspectionDay)
                                                                .Select(i => i.AuditId).ToList();
                            double totalPrice = 0;
                            var priceCalculationType = (int)PriceCalculationType.Normal;
                            var remark = string.Empty;
                            if (auditPerDay.NumberOfAudit >= theSpecialRule.Max_Style_Per_Day)
                            {
                                totalPrice = theSpecialRule.UnitPrice.Value;
                                priceCalculationType = (int)PriceCalculationType.SpecialPrice;
                                remark = "Max Style Per Day: " + theSpecialRule.Max_Style_Per_Day;
                            }
                            else
                            {
                                totalPrice = auditPerDay.NumberOfAudit * unitPrice;
                                if (totalPrice < ruleConfig.MinBillingDay)
                                {
                                    totalPrice = ruleConfig.MinBillingDay.GetValueOrDefault();
                                    priceCalculationType = (int)PriceCalculationType.MinFee;
                                }
                            }
                            var returnedInvoices = this.CreateInterventionInvoiceAuditTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, totalPrice, priceCalculationType, remark, requestDto);
                            invoices.AddRange(returnedInvoices);
                        }
                    }
                    else if (theSpecialRule.Max_Style_Per_Week != null && theSpecialRule.Max_Style_Per_Week > 0)
                    {
                        // Calculate per week
                        var auditPerWeeks = (from inspection in inspections
                                             group inspection by inspection.InspectionWeek into weekGroup
                                             select new
                                             {
                                                 InspectionWeek = weekGroup.Key,
                                                 NumberOfInspection = weekGroup.Count()
                                             }).ToList();
                        var remark = string.Empty;
                        foreach (var inspectionPerWeek in auditPerWeeks)
                        {
                            var tranSerIdList = inspections.Where(i => i.InspectionWeek == inspectionPerWeek.InspectionWeek)
                                                                .Select(i => i.AuditId).ToList();
                            double totalPrice = 0;

                            if (inspectionPerWeek.NumberOfInspection >= theSpecialRule.Max_Style_Per_Week)
                            {
                                totalPrice = theSpecialRule.UnitPrice.GetValueOrDefault();
                                var priceCalculationType = (int)PriceCalculationType.SpecialPrice;
                                remark = "Max Style Per Week: " + theSpecialRule.Max_Style_Per_Week;
                                var returnedInvoices = this.CreateInterventionInvoiceAuditTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, totalPrice, priceCalculationType, remark, requestDto);
                                invoices.AddRange(returnedInvoices);
                            }
                            else
                            {
                                var returnedInvoices = this.CreateNormalInterventionAuditInvoiceTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, requestDto);
                                invoices.AddRange(returnedInvoices);
                            }
                        }
                    }
                    else if (theSpecialRule.Max_Style_per_Month != null && theSpecialRule.Max_Style_per_Month > 0)
                    {
                        // Calculate per month
                        var auditPerMonths = (from inspection in inspections
                                              group inspection by inspection.InspectionMonth into monthGroup
                                              select new
                                              {
                                                  InspectionMonth = monthGroup.Key,
                                                  NumberOfInspection = monthGroup.Count()
                                              }).ToList();
                        var remark = string.Empty;
                        foreach (var inspectionPerMonth in auditPerMonths)
                        {
                            var tranSerIdList = inspections.Where(i => i.InspectionMonth == inspectionPerMonth.InspectionMonth)
                                                                .Select(i => i.AuditId).ToList();
                            double totalPrice = 0;
                            if (inspectionPerMonth.NumberOfInspection >= theSpecialRule.Max_Style_per_Month)
                            {
                                totalPrice = theSpecialRule.UnitPrice.GetValueOrDefault();
                                var priceCalculationType = (int)PriceCalculationType.SpecialPrice;
                                remark = "Max Style Per Month: " + theSpecialRule.Max_Style_per_Month;
                                var returnedInvoices = this.CreateInterventionInvoiceAuditTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, totalPrice, priceCalculationType, remark, requestDto);
                                invoices.AddRange(returnedInvoices);
                            }
                            else
                            {
                                var returnedInvoices = this.CreateNormalInterventionAuditInvoiceTransactions(ruleConfig, orderList, tranSerIdList, unitPrice, requestDto);
                                invoices.AddRange(returnedInvoices);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var orderTransaction in groupOrderList)
                    {
                        var invoiceTransaction = this.CreateInterventionInvoiceAuditTransaction(ruleConfig, orderTransaction, unitPrice, unitPrice, string.Empty, (int)PriceCalculationType.Normal, requestDto);
                        invoices.Add(invoiceTransaction);
                    }
                }
            }

            return invoices;
        }


        private List<InvoiceDetail> CreateNormalInterventionInvoiceTransactions(CustomerPriceCardRepo ruleConfig,
            List<InvoiceBookingDetail> orderList, List<int> tranSerIds, double unitPrice, InvoiceGenerateRequest requestDto, List<InvoiceDetail> invoiceDetailList)
        {
            var invoices = new List<InvoiceDetail>();
            var orderTransactionList = orderList.Where(o => tranSerIds.Contains(o.BookingId)).ToList();
            foreach (var orderTransaction in orderTransactionList)
            {
                if (!invoiceDetailList.Any(x => x.InspectionId == orderTransaction.BookingId))
                {
                    var invoiceTransaction = this.CreateInterventionInvoiceTransaction(ruleConfig, orderTransaction, unitPrice, unitPrice, string.Empty, (int)PriceCalculationType.Normal, requestDto);
                    invoices.Add(invoiceTransaction);
                }
            }
            return invoices;
        }

        private List<InvoiceDetail> CreateNormalInterventionAuditInvoiceTransactions(CustomerPriceCardRepo ruleConfig,
         List<InvoiceBookingDetail> orderList, List<int> tranSerIds, double unitPrice, InvoiceGenerateRequest requestDto)
        {
            var invoices = new List<InvoiceDetail>();
            var orderTransactionList = orderList.Where(o => tranSerIds.Contains(o.BookingId)).ToList();
            foreach (var orderTransaction in orderTransactionList)
            {
                var invoiceTransaction = this.CreateInterventionInvoiceAuditTransaction(ruleConfig, orderTransaction, unitPrice, unitPrice, string.Empty, (int)PriceCalculationType.Normal, requestDto);
                invoices.Add(invoiceTransaction);
            }
            return invoices;
        }

        private List<InvoiceDetail> CreateInterventionInvoiceTransactions(CustomerPriceCardRepo ruleConfig,
        List<InvoiceBookingDetail> orderList, List<int> tranSerIds, double unitPrice, double price, int priceCalculationType,
        string remark, InvoiceGenerateRequest requestDto, List<InvoiceDetail> InvoiceList)
        {
            var invoices = new List<InvoiceDetail>();
            var orderTransactionList = orderList.Where(o => tranSerIds.Contains(o.BookingId)).ToList();
            var firstRow = true;
            foreach (var orderTransaction in orderTransactionList)
            {
                double totalPrice = 0;
                double calculatedUnitPrice = 0;
                var invoiceRemark = string.Empty;
                var currentPriceCalculationType = (int)PriceCalculationType.Normal;
                if (firstRow)
                {
                    totalPrice = price;
                    calculatedUnitPrice = unitPrice;
                    currentPriceCalculationType = priceCalculationType;
                    firstRow = false;
                    invoiceRemark = remark;
                }
                if (!InvoiceList.Any(x => x.InspectionId == orderTransaction.BookingId))
                {
                    var invoiceTransaction = this.CreateInterventionInvoiceTransaction(ruleConfig, orderTransaction, calculatedUnitPrice, totalPrice, invoiceRemark, currentPriceCalculationType, requestDto);
                    invoices.Add(invoiceTransaction);
                }
            }
            return invoices;
        }

        private List<InvoiceDetail> CreateInterventionInvoiceAuditTransactions(CustomerPriceCardRepo ruleConfig,
        List<InvoiceBookingDetail> orderList, List<int> tranSerIds, double unitPrice, double price, int priceCalculationType,
        string remark, InvoiceGenerateRequest requestDto)
        {
            var invoices = new List<InvoiceDetail>();
            var orderTransactionList = orderList.Where(o => tranSerIds.Contains(o.AuditId)).ToList();
            var firstRow = true;
            foreach (var orderTransaction in orderTransactionList)
            {
                double totalPrice = 0;
                double calculatedUnitPrice = 0;
                var invoiceRemark = string.Empty;
                var currentPriceCalculationType = (int)PriceCalculationType.Normal;
                if (firstRow)
                {
                    totalPrice = price;
                    calculatedUnitPrice = unitPrice;
                    currentPriceCalculationType = priceCalculationType;
                    firstRow = false;
                    invoiceRemark = remark;
                }

                var invoiceTransaction = this.CreateInterventionInvoiceAuditTransaction(ruleConfig, orderTransaction, calculatedUnitPrice, totalPrice, invoiceRemark, 0, requestDto);
                invoices.Add(invoiceTransaction);
            }
            return invoices;
        }


        private InvoiceDetail CreateInterventionInvoiceTransaction(CustomerPriceCardRepo ruleConfig, InvoiceBookingDetail orderTransaction,
            double unitPrice, double totalPrice, string remark, int priceCalculationType, InvoiceGenerateRequest requestDto)
        {

            var invoiceTransaction = new InvoiceDetail()
            {
                IsAutomation = true,
                ExchangeRate = null,
                InvoiceMethod = ruleConfig.BillingMethodId,
                InvoiceCurrency = ruleConfig.CurrencyId,
                PriceCardCurrency = ruleConfig.CurrencyId,
                RuleExchangeRate = null,
                InvoiceDate = DateTime.Now,
                InspectionDate = orderTransaction.ServiceTo,
                InvoiceStatus = (int)InvoiceStatus.Created,
                InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                FactoryId = orderTransaction.FactoryId,
                FactoryCountryId = orderTransaction.FactoryCountryId,
                FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                FactoryCountryName = orderTransaction.FactoryCountryName,
                FactoryCountryCode = orderTransaction.FactoryCountryCode,
                UnitPrice = unitPrice,
                InspectionFees = totalPrice,
                TotalInvoiceFees = 0,
                TotalSampleSize = 0,
                OtherFees = 0,
                Discount = 0,
                InspectionId = orderTransaction.BookingId,
                Subject = "",
                ManDays = 0,
                RuleId = ruleConfig.Id,
                TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                InvoiceTo = ruleConfig.BillingToId,
                CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                BankId = ruleConfig.BankAccount,
                PaymentTerms = ruleConfig.PaymentTerms,
                PaymentDuration = ruleConfig.PaymentDuration?.ToString(),
                IsInspection = true,
                InvoicePaymentDate = null,
                Remarks = remark,
                GroupBy = orderTransaction.GroupBy,
                Rule = ruleConfig,
                BookingDepartmentIds = orderTransaction.DepartmentIds,
                BookingBuyerIds = orderTransaction.BuyerIds,
                BookingBrandIds = orderTransaction.BrandIds,
                Office = ruleConfig.InvoiceOffice,
                InvoiceType = requestDto.InvoiceType,
                PriceCalculationtype = priceCalculationType
            };

            return invoiceTransaction;
        }


        private InvoiceDetail CreateInterventionInvoiceAuditTransaction(CustomerPriceCardRepo ruleConfig, InvoiceBookingDetail orderTransaction,
           double unitPrice, double totalPrice, string remark, int priceCalculationType, InvoiceGenerateRequest requestDto)
        {

            var invoiceTransaction = new InvoiceDetail()
            {
                IsAutomation = true,
                ExchangeRate = null,
                InvoiceMethod = ruleConfig.BillingMethodId,
                InvoiceCurrency = ruleConfig.CurrencyId,
                PriceCardCurrency = ruleConfig.CurrencyId,
                RuleExchangeRate = null,
                InvoiceDate = DateTime.Now,
                InspectionDate = orderTransaction.ServiceTo,
                InvoiceStatus = (int)InvoiceStatus.Created,
                InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                FactoryId = orderTransaction.FactoryId,
                FactoryCountryId = orderTransaction.FactoryCountryId,
                FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                FactoryCountryName = orderTransaction.FactoryCountryName,
                FactoryCountryCode = orderTransaction.FactoryCountryCode,
                UnitPrice = unitPrice,
                InspectionFees = totalPrice,
                TotalInvoiceFees = 0,
                TotalSampleSize = 0,
                OtherFees = 0,
                Discount = 0,
                InspectionId = null,
                Subject = "",
                ManDays = 0,
                RuleId = ruleConfig.Id,
                TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                InvoiceTo = ruleConfig.BillingToId,
                CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                BankId = ruleConfig.BankAccount,
                PaymentTerms = ruleConfig.PaymentTerms,
                PaymentDuration = ruleConfig.PaymentDuration?.ToString(),
                IsInspection = true,
                InvoicePaymentDate = null,
                Remarks = remark,
                GroupBy = orderTransaction.GroupBy,
                Rule = ruleConfig,
                BookingDepartmentIds = orderTransaction.DepartmentIds,
                BookingBuyerIds = orderTransaction.BuyerIds,
                BookingBrandIds = orderTransaction.BrandIds,
                Office = ruleConfig.InvoiceOffice,
                InvoiceType = requestDto.InvoiceType,
                PriceCalculationtype = priceCalculationType,
                AuditId = orderTransaction.AuditId
            };

            return invoiceTransaction;
        }


        /// <summary>
        /// get quotation data 
        /// </summary>
        /// <param name="orderTransactions"></param>
        /// <param name="invoiceList"></param>
        /// <param name="ruleConfig"></param>
        /// <returns></returns>
        private async Task<List<InvoiceDetail>> CalculateInspectionFeesFromQuotation(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings)
        {

            // get only rule specific inspection
            var orderList = (from order in orderTransactions
                             where order.RuleConfig.Id == ruleConfig.Id
                             select order).ToList();

            var bookingIds = orderList.Select(x => x.BookingId).Distinct().ToList();
            var quotationDataList = await _invoiceRepository.GetQuotationDataByBookingIdsList(bookingIds);

            foreach (var orderTransaction in orderList)
            {
                // check already booking is not added for invoice
                if (!invoiceList.Any(x => x.InspectionId == orderTransaction.BookingId))
                {
                    double inspectionFees = 0;
                    double unitPrice = 0;
                    double otherFees = 0;
                    double mandays = 0;
                    double discount = 0;

                    var quotationData = quotationDataList.FirstOrDefault(x => x.BookingId == orderTransaction.BookingId);

                    if (quotationData != null)
                    {
                        inspectionFees = quotationData.InspectionFees;
                        unitPrice = quotationData.UnitPrice;

                        if (ruleConfig.BillingMethodId == (int)(PriceBillingMethod.ManDay))
                            mandays = quotationData.Mandays;

                        if ((!invoiceList.Any(x => x.QuotationId == quotationData.QuotationId)) &&
                             (quotationBookings == null || (quotationBookings != null && !quotationBookings.Any(x => x.QuotationId ==
                              quotationData.QuotationId))))
                        {
                            // set other fees and discount 
                            if (ruleConfig.InvoiceOtherFeeFrom == (int)InvoiceFeesFrom.Quotation)
                                otherFees = quotationData.OtherCost;

                            if (ruleConfig.InvoiceDiscountFeeFrom == (int)InvoiceFeesFrom.Quotation)
                                discount = quotationData.Discount;
                        }
                    }

                    // if quotation data billed to and invoice ui billed to same then generate invoice.
                    if (quotationData != null && quotationData.BillingTo == requestDto.InvoiceTo
                        && quotationData.InvoiceType == requestDto.InvoiceType)
                    {
                        var invoiceTransaction = new InvoiceDetail()
                        {
                            IsAutomation = true,
                            ExchangeRate = null,
                            InvoiceMethod = ruleConfig.BillingMethodId,
                            InvoiceCurrency = ruleConfig.CurrencyId,
                            PriceCardCurrency = ruleConfig.CurrencyId,
                            RuleExchangeRate = null,
                            InvoiceDate = DateTime.Now,
                            InspectionDate = orderTransaction.ServiceTo,
                            InvoiceStatus = (int)InvoiceStatus.Created,
                            InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                            FactoryId = orderTransaction.FactoryId,
                            FactoryCountryId = orderTransaction.FactoryCountryId,
                            FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                            FactoryCountryName = orderTransaction.FactoryCountryName,
                            FactoryCountryCode = orderTransaction.FactoryCountryCode,
                            FactoryCityId = orderTransaction.FactoryCityId.GetValueOrDefault(),
                            FactoryProvinceId = orderTransaction.FactoryProvinceId.GetValueOrDefault(),
                            UnitPrice = unitPrice,
                            InspectionFees = inspectionFees,
                            TotalInvoiceFees = 0,
                            TotalSampleSize = 0,
                            OtherFees = otherFees,
                            Discount = discount,
                            InspectionId = orderTransaction.BookingId,
                            Subject = "",
                            ManDays = mandays,
                            RuleId = ruleConfig.Id,
                            TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                            InvoiceTo = ruleConfig.BillingToId,
                            CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                            CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                            CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                            CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                            CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                            BankId = ruleConfig.BankAccount,
                            PaymentTerms = quotationData.PaymentTermsValue,
                            PaymentDuration = quotationData.PaymentTermsCount?.ToString(),
                            IsInspection = true,
                            InvoicePaymentDate = null,
                            Remarks = "",
                            GroupBy = orderTransaction.GroupBy,
                            Rule = ruleConfig,
                            BookingDepartmentIds = orderTransaction.DepartmentIds,
                            BookingBuyerIds = orderTransaction.BuyerIds,
                            BookingBrandIds = orderTransaction.BrandIds,
                            Office = ruleConfig.InvoiceOffice,
                            QuotationId = quotationData.QuotationId,
                            InvoiceType = requestDto.InvoiceType
                        };
                        invoiceList.Add(invoiceTransaction);
                    }
                }
            }
            return invoiceList;
        }
        /// <summary>
        /// Calculate inspection fees from the manday new logic
        /// </summary>
        /// <param name="orderTransactions"></param>
        /// <param name="invoiceList"></param>
        /// <param name="ruleConfig"></param>
        /// <param name="requestDto"></param>
        /// <param name="quotationBookings"></param>
        /// <returns></returns>
        private async Task<List<InvoiceDetail>> CalculateInspectionFeesFromInvoiceMandayRule(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto)
        {
            bool isPlitByBrand = false;

            if (requestDto.SplitInvoice != null && requestDto.SplitInvoice.Any())
                isPlitByBrand = SplitByBrand(requestDto.SplitInvoice.ToList());

            var dateAndFactoryIds = (from order in orderTransactions
                                     where order.RuleConfig.Id == ruleConfig.Id
                                     orderby order.ServiceTo, order.FactoryId
                                     select new
                                     {
                                         order.ServiceTo,
                                         order.FactoryId
                                     }).Distinct();

            var bookingIds = orderTransactions.Select(x => x.BookingId).Distinct().ToList();
            var inspectionQuantityList = await _inspRepository.GetBookingQuantityDetails(bookingIds);
            var containerReportList = await _inspRepository.GetContainerReports(bookingIds);
            var inspectionReportQuantityList = await _inspRepository.GetBookingReportQuantityDetails(bookingIds);

            // set yards to meter.
            ConvertYardsToMeter(inspectionQuantityList.ToList(), inspectionReportQuantityList.ToList());

            foreach (var dateAndFactoryId in dateAndFactoryIds)
            {

                var bookingOrderList = (from order in orderTransactions
                                        where order.FactoryId == dateAndFactoryId.FactoryId
                                         && order.ServiceTo == dateAndFactoryId.ServiceTo
                                         && order.RuleConfig.Id == ruleConfig.Id
                                        select order).ToList();

                var groupedBookingIds = bookingOrderList.Select(x => x.BookingId).Distinct().ToList();

                // define the quantity by group of booking by factory and service date
                double totalInspectedQty = 0;
                var totalReportCount = 0;
                var groupedProductList = inspectionQuantityList.Where(x => groupedBookingIds.Contains(x.BookingId)).ToList();
                var groupedContainerList = containerReportList.Where(x => groupedBookingIds.Contains(x.BookingId)).ToList();
                var totalOrderQty = inspectionQuantityList.Where(x => groupedBookingIds.Contains(x.BookingId)).Sum(x => x.BookingQuantity);
                var totalPresentedQty = inspectionReportQuantityList.Where(x => groupedBookingIds.Contains(x.BookingId)).Sum(x => x.PresentedQuantity);

                if (requestDto.InvoiceType.GetValueOrDefault() == (int)INVInvoiceType.Monthly)
                {
                    totalInspectedQty = inspectionReportQuantityList.Where(x => groupedBookingIds.Contains(x.BookingId)).Sum(x => x.InspectedQuantity.GetValueOrDefault());
                    totalReportCount = inspectionReportQuantityList.Where(x => groupedBookingIds.Contains(x.BookingId)).Select(x => x.ReportId).Distinct().Count();
                }
                else // pre invoice logic will get change
                {
                    var invoiceReportData = GetPreInvoiceReportCountAndInspectQty(bookingOrderList, groupedProductList, groupedBookingIds, groupedContainerList);
                    totalInspectedQty = invoiceReportData.Item1;
                    totalReportCount = invoiceReportData.Item2;
                }

                var isAlreadyUpdate = false;

                foreach (var orderTransaction in bookingOrderList)
                {
                    var remark = string.Empty;
                    decimal unitPrice = 0;
                    var mandays = 0;
                    decimal inspectionFee = 0;
                    double totalQuantity = 0;
                    var priceCalculationType = (int)PriceCalculationType.Normal;
                    var firstBrand = string.Empty;

                    // check already booking is not added for invoice
                    if (!isAlreadyUpdate)
                    {

                        switch (ruleConfig.BillQuantityType)
                        {
                            case (int)InvoiceQuantityType.OrderQuantity:
                                totalQuantity = totalOrderQty;
                                break;
                            case (int)InvoiceQuantityType.InspectedQuantity:
                                totalQuantity = totalInspectedQty;
                                break;
                            case (int)InvoiceQuantityType.PresentedQuantity:
                                totalQuantity = totalPresentedQty.GetValueOrDefault();
                                break;
                        }

                        var manDayUnitPriceDetails = CalculateMandayUnitPrice((decimal)totalQuantity, totalReportCount, ruleConfig, groupedProductList);

                        unitPrice = manDayUnitPriceDetails.Item1;
                        mandays = manDayUnitPriceDetails.Item2;
                        priceCalculationType = manDayUnitPriceDetails.Item3;
                        remark = manDayUnitPriceDetails.Item4;
                        inspectionFee = unitPrice * mandays;
                        isAlreadyUpdate = true;

                        //// check minimum fee logic only one time by date and factory 

                        var minFee = (decimal)ruleConfig.MinBillingDay.GetValueOrDefault();

                        if (minFee > 0 && minFee > inspectionFee)
                        {
                            remark = MinimumFee + (minFee).ToString(NumberFormat);
                            priceCalculationType = (int)PriceCalculationType.MinFee;
                            unitPrice = minFee;
                            inspectionFee = minFee;
                        }

                        if (isPlitByBrand)
                        {
                            firstBrand = string.Join(",", orderTransaction.BrandIds.Select(n => n.ToString()).ToArray());
                        }
                    }
                    else
                    {
                        if (isPlitByBrand && (firstBrand != string.Join(",", orderTransaction.BrandIds.Select(n => n.ToString()).ToArray())))
                        {
                            remark = CombineOrderBrand.ToString();
                        }
                    }

                    if (!invoiceList.Any(x => x.InspectionId == orderTransaction.BookingId))
                    {

                        var invoiceTransaction = new InvoiceDetail()
                        {
                            IsAutomation = true,
                            ExchangeRate = null,
                            InvoiceMethod = ruleConfig.BillingMethodId,
                            InvoiceCurrency = ruleConfig.CurrencyId,
                            PriceCardCurrency = ruleConfig.CurrencyId,
                            RuleExchangeRate = null,
                            InvoiceDate = DateTime.Now,
                            InspectionDate = orderTransaction.ServiceTo,
                            InvoiceStatus = (int)InvoiceStatus.Created,
                            InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                            FactoryId = orderTransaction.FactoryId,
                            FactoryCountryId = orderTransaction.FactoryCountryId,
                            FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                            FactoryCountryName = orderTransaction.FactoryCountryName,
                            FactoryCountryCode = orderTransaction.FactoryCountryCode,
                            FactoryCityId = orderTransaction.FactoryCityId.GetValueOrDefault(),
                            FactoryProvinceId = orderTransaction.FactoryProvinceId.GetValueOrDefault(),
                            UnitPrice = (double)unitPrice,
                            InspectionFees = (double)inspectionFee,
                            TotalInvoiceFees = 0,
                            TotalSampleSize = 0,
                            InspectionId = orderTransaction.BookingId,
                            Subject = "",
                            ManDays = mandays,
                            RuleId = ruleConfig.Id,
                            TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                            InvoiceTo = ruleConfig.BillingToId,
                            CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                            CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                            CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                            CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                            CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                            BankId = ruleConfig.BankAccount,
                            PaymentTerms = ruleConfig.PaymentTerms,
                            PaymentDuration = ruleConfig.PaymentDuration?.ToString(),
                            IsInspection = true,
                            InvoicePaymentDate = null,
                            Remarks = remark,
                            GroupBy = orderTransaction.GroupBy,
                            Rule = ruleConfig,
                            BookingDepartmentIds = orderTransaction.DepartmentIds,
                            BookingBuyerIds = orderTransaction.BuyerIds,
                            BookingBrandIds = orderTransaction.BrandIds,
                            Office = ruleConfig.InvoiceOffice,
                            InvoiceType = requestDto.InvoiceType,
                            PriceCalculationtype = priceCalculationType
                        };
                        invoiceList.Add(invoiceTransaction);
                    }
                }
            }
            return invoiceList;
        }

        /// <summary>
        /// Convert yards to meter.
        /// </summary>
        /// <param name="inspectionQuantityList"></param>
        /// <param name="inspectionReportQuantityList"></param>
        private void ConvertYardsToMeter(List<BookingQuantityData> inspectionQuantityList, List<BookingReportQuantityData> inspectionReportQuantityList)
        {
            // set yards to meter 
            foreach (var item in inspectionQuantityList)
            {
                if (item.UnitId == (int)BookingProductUnitType.Yards)
                {
                    item.BookingQuantity = item.BookingQuantity * 0.9144;
                }
            }

            // set yards to meter 
            foreach (var item in inspectionReportQuantityList)
            {
                var ProductInfo = inspectionQuantityList.FirstOrDefault(x => x.FbReportId == item.ReportId);

                if (ProductInfo != null && ProductInfo.UnitId == (int)BookingProductUnitType.Yards)
                {
                    item.InspectedQuantity = item.InspectedQuantity * 0.9144;
                    item.PresentedQuantity = item.PresentedQuantity * 0.9144;
                }
            }
        }

        private async Task<List<InvoiceDetail>> CalculateInspectionFeesFromInvoicePieceRateRule(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, Dictionary<string, List<InvAutTranDetail>> dicExistingInvoices)
        {
            bool isPlitByBrand = false;

            // check any split invoice by brand is availble 
            if (requestDto.SplitInvoice != null && requestDto.SplitInvoice.Any())
                isPlitByBrand = SplitByBrand(requestDto.SplitInvoice.ToList());

            // group date and factory bookings based on the current rule mapped booking list
            var dateAndFactoryIds = (from order in orderTransactions
                                     where order.RuleConfig.Id == ruleConfig.Id
                                     orderby order.ServiceTo, order.FactoryId
                                     select new
                                     {
                                         order.ServiceTo,
                                         order.FactoryId
                                     }).Distinct();

            // get all the booking related informations based on distinct bookings
            var bookingIds = orderTransactions.Select(x => x.BookingId).Distinct().ToList();

            // get order Quantity,inspected qty details by booking list
            var inspectionQuantityList = await _inspRepository.GetBookingQuantityDetails(bookingIds);
            // get container report details by booking list
            var containerReportList = await _inspRepository.GetContainerReports(bookingIds);
            // get report quantity details for presented qty by booking list
            var inspectionReportQuantityList = await _inspRepository.GetBookingReportQuantityDetails(bookingIds);

            ConvertYardsToMeter(inspectionQuantityList.ToList(), inspectionReportQuantityList.ToList());

            // Loop each group by factory and service date
            foreach (var dateAndFactoryId in dateAndFactoryIds)
            {

                // each day - in the factory how many booking present with specific mapped rule 
                var bookingOrderList = (from order in orderTransactions
                                        where order.FactoryId == dateAndFactoryId.FactoryId
                                         && order.ServiceTo == dateAndFactoryId.ServiceTo
                                         && order.RuleConfig.Id == ruleConfig.Id
                                        select order).ToList();

                // get unique booking ids for current loop of  service date and factory
                var groupedBookingIds = bookingOrderList.Select(x => x.BookingId).Distinct().ToList();

                // define the quantity by group of booking by factory and service date
                double totalInspectedQty = 0;
                var groupedProductList = inspectionQuantityList.Where(x => groupedBookingIds.Contains(x.BookingId)).ToList();
                var groupedContainerList = containerReportList.Where(x => groupedBookingIds.Contains(x.BookingId)).ToList();

                var totalOrderQty = inspectionQuantityList.Where(x => groupedBookingIds.Contains(x.BookingId)).Sum(x => x.BookingQuantity);
                var totalPresentedQty = inspectionReportQuantityList.Where(x => groupedBookingIds.Contains(x.BookingId)).Sum(x => x.PresentedQuantity);

                if (requestDto.InvoiceType.GetValueOrDefault() == (int)INVInvoiceType.Monthly)
                {
                    totalInspectedQty = inspectionReportQuantityList.Where(x => groupedBookingIds.Contains(x.BookingId)).Sum(x => x.InspectedQuantity.GetValueOrDefault());
                }
                else // pre invoice logic will get change
                {
                    var invoiceReportData = GetPreInvoiceReportCountAndInspectQty(bookingOrderList, groupedProductList, groupedBookingIds, groupedContainerList);
                    totalInspectedQty = invoiceReportData.Item1;
                }

                // Check existing invoices
                if (dicExistingInvoices.TryGetValue(Combine(dateAndFactoryId.FactoryId.GetValueOrDefault(), dateAndFactoryId.ServiceTo),
                                out var existingInvoices))
                {
                    if (existingInvoices.Count > 0) // Existing invoice with the same factory id and inspection date
                    {
                        var invoiceMatchCondition = existingInvoices.Where(i => (i.PriceCalculationType != null) && (i.PriceCalculationType != (int)PriceCalculationType.Normal))
                                                                    .ToList();
                        if (invoiceMatchCondition.Count > 0)
                        {
                            // Already create order with the specific min, max, special rule
                            foreach (var orderTransaction in orderTransactions)
                            {
                                // check already booking is not added for invoice
                                if (!invoiceList.Any(x => x.InspectionId == orderTransaction.BookingId))
                                {
                                    var invoiceTransaction = this.CreateInvoiceTransaction(ruleConfig, orderTransaction, groupedProductList, inspectionReportQuantityList.ToList(), 0, 0, requestDto, string.Empty);
                                    // update mandays and price calculation type
                                    invoiceTransaction.ManDays = 0;
                                    invoiceTransaction.PriceCalculationtype = (int)PriceCalculationType.Normal;
                                    invoiceList.Add(invoiceTransaction);
                                }
                            }
                        }
                        else
                        {
                            foreach (var order in orderTransactions)
                            {
                                // check already booking is not added for invoice
                                if (!invoiceList.Any(x => x.InspectionId == order.BookingId))
                                {
                                    var invoiceTransaction = this.CreateNormalInvoiceTransaction(ruleConfig, order, groupedProductList, inspectionReportQuantityList.ToList(), totalOrderQty, 0, requestDto);
                                    invoiceList.Add(invoiceTransaction);
                                }
                            }
                        }
                    }

                    // existing invoice is not present 
                    else
                    {
                        var bookingIdList = bookingOrderList.Select(x => x.BookingId).ToList();
                        var customerId = bookingOrderList.FirstOrDefault().CustomerId;

                        // get the existing invoice orders 
                        var existingBookingOrders = await _inspRepository.GetOrdersOnSameFactoryAndSameDate(bookingIdList, dateAndFactoryId.FactoryId.GetValueOrDefault(), customerId, dateAndFactoryId.ServiceTo);

                        // fetch existing booking orders ids
                        var existingOrders = existingBookingOrders.Select(x => x.BookingId).Distinct().ToList();

                        var existingInspectionQuantityList = await _inspRepository.GetBookingQuantityDetails(bookingIds);
                        var existingContainerReportList = await _inspRepository.GetContainerReports(bookingIds);
                        var existingInspectionReportQuantityList = await _inspRepository.GetBookingReportQuantityDetails(bookingIds);

                        // define the quantity by group of booking by factory and service date
                        double existingTotalInspectedQty = 0;
                        var existingGroupedProductList = existingInspectionQuantityList.Where(x => existingOrders.Contains(x.BookingId)).ToList();
                        var existingGroupedContainerList = existingContainerReportList.Where(x => existingOrders.Contains(x.BookingId)).ToList();
                        var existingTotalOrderQty = existingInspectionQuantityList.Where(x => existingOrders.Contains(x.BookingId)).Sum(x => x.BookingQuantity);
                        var existingTotalPresentedQty = existingInspectionReportQuantityList.Where(x => existingOrders.Contains(x.BookingId)).Sum(x => x.PresentedQuantity);

                        if (requestDto.InvoiceType.GetValueOrDefault() == (int)INVInvoiceType.Monthly)
                        {
                            existingTotalInspectedQty = existingInspectionReportQuantityList.Where(x => existingOrders.Contains(x.BookingId)).Sum(x => x.InspectedQuantity.GetValueOrDefault());
                        }
                        else // pre invoice logic will get change
                        {
                            var invoiceReportData = GetPreInvoiceReportCountAndInspectQty(bookingOrderList, existingGroupedProductList, existingOrders, existingGroupedContainerList);
                            existingTotalInspectedQty = invoiceReportData.Item1;
                        }

                        var matchSpecialRule = false;
                        double pieceRateMinBilling = 0;
                        double additionalFee = 0;
                        double specialUnitPrice = 0;
                        var remark = string.Empty;


                        double totalQuantity = 0;
                        double existingTotalQuantity = 0;

                        switch (ruleConfig.BillQuantityType)
                        {
                            case (int)InvoiceQuantityType.OrderQuantity:
                                totalQuantity = totalOrderQty;
                                existingTotalQuantity = existingTotalOrderQty;
                                break;
                            case (int)InvoiceQuantityType.InspectedQuantity:
                                totalQuantity = totalInspectedQty;
                                existingTotalQuantity = existingTotalInspectedQty;
                                break;
                            case (int)InvoiceQuantityType.PresentedQuantity:
                                totalQuantity = totalPresentedQty.GetValueOrDefault();
                                existingTotalQuantity = existingTotalPresentedQty.GetValueOrDefault();
                                break;
                        }


                        if (totalQuantity > 0)
                        {

                            totalQuantity = totalQuantity + existingTotalQuantity;

                            var specialRules = ruleConfig.RuleList;

                            foreach (var specialRule in specialRules)
                            {
                                if ((specialRule.PieceRate_Billing_Q_Start <= totalQuantity) && (totalQuantity <= specialRule.Piecerate_Billing_Q_End))
                                {
                                    matchSpecialRule = true;
                                    pieceRateMinBilling = specialRule.Piecerate_MinBilling.Value;
                                    if (specialRule.AdditionalFee != null)
                                    {
                                        additionalFee = specialRule.AdditionalFee.Value;
                                    }

                                    if (specialRule.UnitPrice != null)
                                    {
                                        specialUnitPrice = specialRule.UnitPrice.Value;
                                    }
                                    remark = $"Special Rule: From {specialRule.PieceRate_Billing_Q_Start} to {specialRule.Piecerate_Billing_Q_End}.";
                                    break;
                                }
                            }


                            // Match special rule
                            if (matchSpecialRule)
                            {
                                var useSpecialRule = false;
                                // Check inspection fee
                                var inspectionFeeBySpecialUnitPrice = additionalFee;
                                var inspectionFeeByMinBilling = pieceRateMinBilling + additionalFee;
                                if (additionalFee > 0)
                                {
                                    useSpecialRule = true;
                                }

                                if (specialUnitPrice > 0)
                                {
                                    useSpecialRule = true;
                                    inspectionFeeBySpecialUnitPrice = inspectionFeeBySpecialUnitPrice + specialUnitPrice * totalQuantity;
                                }
                                else
                                {
                                    inspectionFeeBySpecialUnitPrice = 0;
                                }

                                var isMinInspectionFee = false;
                                var specialInspectionFee = inspectionFeeBySpecialUnitPrice;
                                if (inspectionFeeBySpecialUnitPrice < inspectionFeeByMinBilling)
                                {
                                    specialInspectionFee = inspectionFeeByMinBilling;
                                    isMinInspectionFee = true;
                                    remark = remark + " " + MinimumFee + inspectionFeeByMinBilling.ToString(NumberFormat);
                                    useSpecialRule = true;
                                }
                                if (!useSpecialRule) // Clear remark because special rule not applied (Item 27)
                                {
                                    remark = string.Empty;
                                }

                                var isAlreadyUpdate = false;
                                var isCombineWithOtherOrders = false;
                                var countOrder = 0;

                                foreach (var order in bookingOrderList)
                                {
                                    if (!invoiceList.Any(x => x.InspectionId == order.BookingId))
                                    {
                                        countOrder++;

                                        if (!isAlreadyUpdate)
                                        {
                                            var unitPrice = specialUnitPrice;

                                            isAlreadyUpdate = true;

                                            // Check the inspectionFee with other order transactions of the same visit of different rules

                                            var sameDayAndFactoryTranSerIds = (from orderTransaction in orderTransactions
                                                                               where orderTransaction.FactoryId == dateAndFactoryId.FactoryId
                                                                                && orderTransaction.ServiceTo == dateAndFactoryId.ServiceTo
                                                                                && orderTransaction.RuleConfig.Id != ruleConfig.Id
                                                                               select orderTransaction.BookingId).ToList();

                                            var sameDayAndFactoryInvoiceInspectionFee = (from invoice in invoiceList
                                                                                         where sameDayAndFactoryTranSerIds.Contains(invoice.InspectionId.GetValueOrDefault())
                                                                                            && invoice.InspectionFees > DoubleMinimumValue
                                                                                         select invoice.InspectionFees.Value).Sum();


                                            if (specialInspectionFee > sameDayAndFactoryInvoiceInspectionFee)
                                            {
                                                foreach (var invoice in invoiceList)
                                                {
                                                    if (sameDayAndFactoryTranSerIds.Contains(invoice.InspectionId.Value))
                                                    {
                                                        invoice.UnitPrice = 0;
                                                        invoice.InspectionFees = 0;
                                                        StringBuilder strRemarks = new StringBuilder();
                                                        strRemarks.Append(invoice.Remarks).Append(". ").Append(CombineSpecialCase);
                                                        invoice.Remarks = strRemarks.ToString();
                                                    }
                                                }

                                                var invoiceTransaction = this.CreateNormalInvoiceTransaction(ruleConfig, order, groupedProductList,
                                                existingInspectionReportQuantityList.ToList(), unitPrice, totalQuantity, requestDto);
                                                if (additionalFee > 0)
                                                {
                                                    invoiceTransaction.InspectionFees = invoiceTransaction.InspectionFees + (double)(additionalFee);
                                                    var remarkFee = Math.Round(additionalFee, 2);

                                                    invoiceTransaction.Remarks = remark + " Additional Fee " + remarkFee + ".";
                                                }
                                                else
                                                {
                                                    invoiceTransaction.Remarks = remark;
                                                }

                                                if (isMinInspectionFee)
                                                {
                                                    invoiceTransaction = this.CreateInvoiceTransaction(ruleConfig, order, groupedProductList,
                                                existingInspectionReportQuantityList.ToList(), unitPrice, specialInspectionFee, requestDto, remark);
                                                }

                                                invoiceTransaction.PriceCalculationtype = (int)PriceCalculationType.SpecialPrice;


                                                // set booking data list
                                                var brandIdList = await _inspRepository.GetBrandBookingIdsByBookingIds(bookingIdList);

                                                if (existingBookingOrders.Any())
                                                {
                                                    foreach (var booking in existingBookingOrders)
                                                    {
                                                        booking.BrandIds = brandIdList.Where(x => x.BookingId == booking.BookingId).Select(x => x.BrandId).ToList();
                                                    }
                                                }

                                                if ((existingBookingOrders.Count > 0) && (isPlitByBrand))
                                                {
                                                    var otherBrandOrders = existingBookingOrders.Where(e => string.Join(",", e.BrandIds.Select(n => n.ToString()).ToArray())
                                                                                           != string.Join(",", order.BrandIds.Select(n => n.ToString()).ToArray())).ToList();
                                                    if (otherBrandOrders.Count > 0 && isPlitByBrand)
                                                    {
                                                        StringBuilder strRemarks = new StringBuilder();
                                                        strRemarks.Append(invoiceTransaction.Remarks).Append(". ").Append(CombineOrderString);
                                                        invoiceTransaction.Remarks = strRemarks.ToString();
                                                    }
                                                }
                                                invoiceList.Add(invoiceTransaction);
                                            }
                                            else
                                            {
                                                isCombineWithOtherOrders = true;
                                                // Update current with 0 
                                                var invoiceTransaction = this.CreateInvoiceTransaction(ruleConfig, order, groupedProductList,
                                                existingInspectionReportQuantityList.ToList(), 0, 0, requestDto, string.Empty);
                                                invoiceList.Add(invoiceTransaction);
                                            }
                                        }
                                        else
                                        {
                                            double unitPrice = 0;                                         
                                            if (!isCombineWithOtherOrders)
                                            {
                                                unitPrice = specialUnitPrice;
                                            }
                                            var invoiceTransaction = this.CreateNormalInvoiceTransaction(ruleConfig, order, groupedProductList,
                                                existingInspectionReportQuantityList.ToList(), unitPrice, totalQuantity, requestDto);
                                            invoiceTransaction.Remarks = remark;

                                            if (isMinInspectionFee)
                                            {
                                                invoiceTransaction = this.CreateInvoiceTransaction(ruleConfig, order, groupedProductList,
                                                existingInspectionReportQuantityList.ToList(), 0, 0, requestDto, string.Empty);
                                            }
                                            invoiceList.Add(invoiceTransaction);
                                        }
                                    }
                                }
                            }

                            else
                            {
                                // Normal rules
                                var newInvoices = new List<InvoiceDetail>();
                                var tmpInvoicesForExistingOrders = new List<InvoiceDetail>();
                                foreach (var order in bookingOrderList)
                                {
                                    if (!invoiceList.Any(x => x.InspectionId == order.BookingId))
                                    {
                                        var invoiceTransaction = this.CreateNormalInvoiceTransaction(ruleConfig, order, groupedProductList,
                                                existingInspectionReportQuantityList.ToList(), 0, totalQuantity, requestDto);
                                        newInvoices.Add(invoiceTransaction);
                                    }
                                }

                                foreach (var existingOrder in existingBookingOrders)
                                {
                                    var invoiceTransaction = this.CreateNormalInvoiceTransaction(ruleConfig, existingOrder, groupedProductList,
                                                existingInspectionReportQuantityList.ToList(), 0, existingTotalOrderQty, requestDto);
                                    tmpInvoicesForExistingOrders.Add(invoiceTransaction);
                                }
                                // Check min fee, max fee
                                // Only check min fee, max fee for normal cases. Not special cases
                                newInvoices = this.CheckMinFee(newInvoices, ruleConfig, tmpInvoicesForExistingOrders);
                                invoiceList.AddRange(newInvoices);
                            }
                        }
                        else
                        {
                            foreach (var orderTransaction in bookingOrderList)
                            {
                                if (!invoiceList.Any(x => x.InspectionId == orderTransaction.BookingId))
                                {
                                    var invoiceTransaction = this.CreateInvoiceTransaction(ruleConfig, orderTransaction, groupedProductList,
                                                existingInspectionReportQuantityList.ToList(), 0, 0, requestDto, string.Empty);
                                    // update mandays and price calculation type
                                    invoiceTransaction.ManDays = 0;
                                    invoiceTransaction.PriceCalculationtype = (int)PriceCalculationType.Normal;
                                    invoiceList.Add(invoiceTransaction);
                                }
                            }
                        }
                    }
                }
            }
            return invoiceList;
        }

        private List<InvoiceDetail> CheckMinFee(List<InvoiceDetail> checkedInvoices,
          CustomerPriceCardRepo ruleConfig, List<InvoiceDetail> tmpInvoices)
        {
            // Check min fee
            var dTotalFees = checkedInvoices.Sum(s => s.InspectionFees ?? 0);
            var dTmpTotalFees = tmpInvoices.Sum(s => s.InspectionFees ?? 0);
            var totalFees = new decimal(dTotalFees + dTmpTotalFees);
            var configuredMinBilling = (decimal)ruleConfig.MinBillingDay.GetValueOrDefault();
            decimal returnedAmount = 0;
            var remark = string.Empty;
            var priceCalculationType = (int)PriceCalculationType.Normal;

            if (totalFees > 0 && totalFees < configuredMinBilling)
            {
                returnedAmount = configuredMinBilling;

                remark = MinimumFee + returnedAmount.ToString(NumberFormat);

                priceCalculationType = (int)PriceCalculationType.MinFee;
                if (tmpInvoices.Count > 0)
                {
                    remark = remark + ". " + CombineOrderString;
                }
            }

            if (!string.IsNullOrWhiteSpace(remark))
            {
                var isAlreadyUpdate = false;
                foreach (var invoice in checkedInvoices)
                {
                    if (isAlreadyUpdate == false)
                    {
                        isAlreadyUpdate = true;
                        invoice.InspectionFees = (double)returnedAmount;
                        invoice.PriceCalculationtype = priceCalculationType;
                        invoice.Remarks = invoice.Remarks + " " + remark;
                    }
                    else
                    {
                        invoice.InspectionFees = 0;
                        invoice.UnitPrice = 0;
                    }
                }
            }

            return checkedInvoices;
        }

        private InvoiceDetail CreateNormalInvoiceTransaction(CustomerPriceCardRepo ruleConfig,
                          InvoiceBookingDetail orderTransaction, List<BookingQuantityData> groupedProductList,
                          List<BookingReportQuantityData> bookingReportQuantityDatas,
                              double specialUnitPrice, double totalQuantity, InvoiceGenerateRequest requestDto)
        {
            var remark = string.Empty;
            var priceCalculationType = (int)PriceCalculationType.Normal;
            var unitPrice = specialUnitPrice;
            if (specialUnitPrice == 0)
            {
                unitPrice = this.GetNormalUnitPrice(ruleConfig, groupedProductList);
            }
            else
            {
                priceCalculationType = (int)PriceCalculationType.SpecialPrice;
            }

            var inspectionFees = GetInspectionFee(unitPrice, orderTransaction.BookingId,
                groupedProductList, bookingReportQuantityDatas, ruleConfig);

            // Check max fee per style
            if (ruleConfig.MaxFeeStyle > 0)
            {
                if (inspectionFees > ruleConfig.MaxFeeStyle)
                {
                    inspectionFees = ruleConfig.MaxFeeStyle.HasValue ? ruleConfig.MaxFeeStyle.Value : 0;
                    remark = MaximumFee + inspectionFees.ToString(NumberFormat);
                    priceCalculationType = (int)PriceCalculationType.MaxFee;
                }
            }
            var invoiceTransaction = new InvoiceDetail()
            {
                IsAutomation = true,
                ExchangeRate = null,
                InvoiceMethod = ruleConfig.BillingMethodId,
                InvoiceCurrency = ruleConfig.CurrencyId,
                PriceCardCurrency = ruleConfig.CurrencyId,
                RuleExchangeRate = null,
                InvoiceDate = DateTime.Now,
                InspectionDate = orderTransaction.ServiceTo,
                InvoiceStatus = (int)InvoiceStatus.Created,
                InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                FactoryId = orderTransaction.FactoryId,
                FactoryCountryId = orderTransaction.FactoryCountryId,
                FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                FactoryCountryName = orderTransaction.FactoryCountryName,
                FactoryCountryCode = orderTransaction.FactoryCountryCode,
                FactoryCityId = orderTransaction.FactoryCityId.GetValueOrDefault(),
                FactoryProvinceId = orderTransaction.FactoryProvinceId.GetValueOrDefault(),
                UnitPrice = unitPrice,
                BilledQuantity = totalQuantity,
                BilledQuantityType = ruleConfig.BillQuantityType,
                InspectionFees = inspectionFees,
                TotalInvoiceFees = 0,
                TotalSampleSize = 0,
                InspectionId = orderTransaction.BookingId,
                Subject = "",
                ManDays = 0,
                RuleId = ruleConfig.Id,
                TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                InvoiceTo = ruleConfig.BillingToId,
                CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                BankId = ruleConfig.BankAccount,
                PaymentTerms = ruleConfig.PaymentTerms,
                PaymentDuration = ruleConfig.PaymentDuration?.ToString(),
                IsInspection = true,
                InvoicePaymentDate = null,
                Remarks = remark,
                GroupBy = orderTransaction.GroupBy,
                Rule = ruleConfig,
                BookingDepartmentIds = orderTransaction.DepartmentIds,
                BookingBuyerIds = orderTransaction.BuyerIds,
                BookingBrandIds = orderTransaction.BrandIds,
                Office = ruleConfig.InvoiceOffice,
                InvoiceType = requestDto.InvoiceType,
                PriceCalculationtype = priceCalculationType
            };


            switch (ruleConfig.BillQuantityType)
            {
                case (int)InvoiceQuantityType.OrderQuantity:
                    invoiceTransaction.BilledQuantity = groupedProductList.Where(x => x.BookingId == orderTransaction.BookingId).Sum(x => x.BookingQuantity);
                    break;
                case (int)InvoiceQuantityType.InspectedQuantity:
                    invoiceTransaction.BilledQuantity = bookingReportQuantityDatas.Where(x => x.BookingId == orderTransaction.BookingId).Sum(x => x.InspectedQuantity.GetValueOrDefault());
                    break;
                case (int)InvoiceQuantityType.PresentedQuantity:
                    invoiceTransaction.BilledQuantity = bookingReportQuantityDatas.Where(x => x.BookingId == orderTransaction.BookingId).Sum(x => x.PresentedQuantity.GetValueOrDefault());
                    break;
            }

            return invoiceTransaction;
        }

        private double GetInspectionFee(double unitPrice, int bookingId,
            IEnumerable<BookingQuantityData> inspectionQuantityList,
            IEnumerable<BookingReportQuantityData> inspectionReportQuantityList,
            CustomerPriceCardRepo ruleConfig)
        {
            double checkedQuantity = 0;
            switch (ruleConfig.BillQuantityType)
            {
                case (int)InvoiceQuantityType.OrderQuantity:
                    checkedQuantity = inspectionQuantityList.Where(x => x.BookingId == bookingId).Sum(x => x.BookingQuantity);
                    break;
                case (int)InvoiceQuantityType.InspectedQuantity:
                    checkedQuantity = inspectionReportQuantityList.Where(x => x.BookingId == bookingId).Sum(x => x.InspectedQuantity.GetValueOrDefault());
                    break;
                case (int)InvoiceQuantityType.PresentedQuantity:
                    checkedQuantity = inspectionReportQuantityList.Where(x => x.BookingId == bookingId).Sum(x => x.PresentedQuantity.GetValueOrDefault());
                    break;
            }

            var inspectionFees = unitPrice * checkedQuantity;
            return inspectionFees;
        }


        private InvoiceDetail CreateInvoiceTransaction(CustomerPriceCardRepo ruleConfig, InvoiceBookingDetail orderTransaction, List<BookingQuantityData> groupedProductList,
                          List<BookingReportQuantityData> bookingReportQuantityDatas,
                            double unitPrice, double inspectionFees, InvoiceGenerateRequest requestDto, string remark)
        {

            var invoiceTransaction = new InvoiceDetail()
            {
                IsAutomation = true,
                ExchangeRate = null,
                InvoiceMethod = ruleConfig.BillingMethodId,
                InvoiceCurrency = ruleConfig.CurrencyId,
                PriceCardCurrency = ruleConfig.CurrencyId,
                RuleExchangeRate = null,
                InvoiceDate = DateTime.Now,
                InspectionDate = orderTransaction.ServiceTo,
                InvoiceStatus = (int)InvoiceStatus.Created,
                InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                FactoryId = orderTransaction.FactoryId,
                FactoryCountryId = orderTransaction.FactoryCountryId,
                FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                FactoryCountryName = orderTransaction.FactoryCountryName,
                FactoryCityId = orderTransaction.FactoryCityId.GetValueOrDefault(),
                FactoryProvinceId = orderTransaction.FactoryProvinceId.GetValueOrDefault(),
                UnitPrice = unitPrice,
                BilledQuantityType = ruleConfig.BillQuantityType,
                BilledQuantity = 0,
                InspectionFees = inspectionFees,
                TotalInvoiceFees = 0,
                TotalSampleSize = 0,
                InspectionId = orderTransaction.BookingId,
                Subject = "",
                ManDays = 0,
                RuleId = ruleConfig.Id,
                TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                InvoiceTo = ruleConfig.BillingToId,
                CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                BankId = ruleConfig.BankAccount,
                PaymentTerms = ruleConfig.PaymentTerms,
                PaymentDuration = ruleConfig.PaymentDuration?.ToString(),
                IsInspection = true,
                InvoicePaymentDate = null,
                Remarks = remark,
                GroupBy = orderTransaction.GroupBy,
                Rule = ruleConfig,
                BookingDepartmentIds = orderTransaction.DepartmentIds,
                BookingBuyerIds = orderTransaction.BuyerIds,
                BookingBrandIds = orderTransaction.BrandIds,
                Office = ruleConfig.InvoiceOffice,
                InvoiceType = requestDto.InvoiceType
            };


            switch (ruleConfig.BillQuantityType)
            {
                case (int)InvoiceQuantityType.OrderQuantity:
                    invoiceTransaction.BilledQuantity = groupedProductList.Where(x => x.BookingId == orderTransaction.BookingId).Sum(x => x.BookingQuantity);
                    break;
                case (int)InvoiceQuantityType.InspectedQuantity:
                    invoiceTransaction.BilledQuantity = bookingReportQuantityDatas.Where(x => x.BookingId == orderTransaction.BookingId).Sum(x => x.InspectedQuantity.GetValueOrDefault());
                    break;
                case (int)InvoiceQuantityType.PresentedQuantity:
                    invoiceTransaction.BilledQuantity = bookingReportQuantityDatas.Where(x => x.BookingId == orderTransaction.BookingId).Sum(x => x.PresentedQuantity.GetValueOrDefault());
                    break;
            }

            return invoiceTransaction;
        }

        private double GetNormalUnitPrice(CustomerPriceCardRepo ruleConfig, List<BookingQuantityData> groupedProductList)
        {
            var subCategory2Ids = groupedProductList.Select(x => x.ProductSub2CategoryId).ToList();

            var billingMethod = ruleConfig.SubCategory2List.FirstOrDefault(x => subCategory2Ids.Contains(x.SubCategory2Id));

            double unitPrice = 0;

            if (billingMethod != null)
            {
                unitPrice = billingMethod.UnitPrice.Value;
            }

            else
            {
                if (ruleConfig.SubCategorySelectAll.GetValueOrDefault())
                {
                    // Default values
                    unitPrice = ruleConfig.UnitPrice;
                }
            }
            return unitPrice;
        }


        private async Task<List<InvoiceDetail>> CalculateInspectionFeesFromInvoiceMandaySimpleRule(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto)
        {

            var dateAndFactoryIds = (from order in orderTransactions
                                     where order.RuleConfig.Id == ruleConfig.Id
                                     orderby order.ServiceTo, order.FactoryId
                                     select new
                                     {
                                         order.ServiceTo,
                                         order.FactoryId
                                     }).Distinct();

            foreach (var dateAndFactoryId in dateAndFactoryIds)
            {

                var bookingOrderList = (from order in orderTransactions
                                        where order.FactoryId == dateAndFactoryId.FactoryId
                                         && order.ServiceTo == dateAndFactoryId.ServiceTo
                                         && order.RuleConfig.Id == ruleConfig.Id
                                        select order).ToList();


                var isAlreadyUpdate = false;

                foreach (var orderTransaction in bookingOrderList)
                {
                    double unitPrice = 0;
                    double inspectionFee = 0;
                    int mandays = 0;
                    var priceCalculationType = (int)PriceCalculationType.Normal;
                    var remark = string.Empty;

                    // check already booking is not added for invoice
                    if (!isAlreadyUpdate)
                    {
                        unitPrice = ruleConfig.UnitPrice;
                        // manday logic missing here with inspection fees calculations we have to add later.
                        inspectionFee = ruleConfig.UnitPrice;
                        isAlreadyUpdate = true;
                    }

                    if (!invoiceList.Any(x => x.InspectionId == orderTransaction.BookingId))
                    {

                        var invoiceTransaction = new InvoiceDetail()
                        {
                            IsAutomation = true,
                            ExchangeRate = null,
                            InvoiceMethod = ruleConfig.BillingMethodId,
                            InvoiceCurrency = ruleConfig.CurrencyId,
                            PriceCardCurrency = ruleConfig.CurrencyId,
                            RuleExchangeRate = null,
                            InvoiceDate = DateTime.Now,
                            InspectionDate = orderTransaction.ServiceTo,
                            InvoiceStatus = (int)InvoiceStatus.Created,
                            InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                            FactoryId = orderTransaction.FactoryId,
                            FactoryCountryId = orderTransaction.FactoryCountryId,
                            FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                            FactoryCountryName = orderTransaction.FactoryCountryName,
                            FactoryCityId = orderTransaction.FactoryCityId.GetValueOrDefault(),
                            FactoryProvinceId = orderTransaction.FactoryProvinceId.GetValueOrDefault(),
                            UnitPrice = unitPrice,
                            InspectionFees = inspectionFee,
                            TotalInvoiceFees = 0,
                            TotalSampleSize = 0,
                            InspectionId = orderTransaction.BookingId,
                            Subject = "",
                            ManDays = mandays,
                            RuleId = ruleConfig.Id,
                            TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                            InvoiceTo = ruleConfig.BillingToId,
                            CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                            CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                            CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                            CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                            CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                            BankId = ruleConfig.BankAccount,
                            PaymentTerms = ruleConfig.PaymentTerms,
                            PaymentDuration = ruleConfig.PaymentDuration?.ToString(),
                            IsInspection = true,
                            InvoicePaymentDate = null,
                            Remarks = remark,
                            GroupBy = orderTransaction.GroupBy,
                            Rule = ruleConfig,
                            BookingDepartmentIds = orderTransaction.DepartmentIds,
                            BookingBuyerIds = orderTransaction.BuyerIds,
                            BookingBrandIds = orderTransaction.BrandIds,
                            Office = ruleConfig.InvoiceOffice,
                            InvoiceType = requestDto.InvoiceType,
                            PriceCalculationtype = priceCalculationType
                        };
                        invoiceList.Add(invoiceTransaction);
                    }
                }
            }
            return invoiceList;
        }



        private List<InvoiceDetail> UpdateMinFee(List<InvoiceDetail> invoiceTransactions,
                List<int> isInspectionTranSerIds)
        {

            var dateAndFactoryIds = (from invoice in invoiceTransactions
                                     orderby invoice.InspectionDate, invoice.FactoryId
                                     select new
                                     {
                                         invoice.InspectionDate,
                                         invoice.FactoryId
                                     }).Distinct();
            foreach (var dateAndFactoryId in dateAndFactoryIds)
            {
                var invoiceByDateFactory = (from invoice in invoiceTransactions
                                            where invoice.InspectionDate == dateAndFactoryId.InspectionDate
                                             && invoice.FactoryId == dateAndFactoryId.FactoryId
                                            select invoice).ToList();

                if (invoiceByDateFactory.Count == 0)
                {
                    continue;
                }

                var ruleConfig = invoiceByDateFactory[0].Rule;
                if (ruleConfig == null)
                {
                    continue;
                }

                if (ruleConfig.InvoiceInspFeeFrom == (int)InvoiceFeesFrom.Invoice && (ruleConfig.BillingMethodId == (int)PriceBillingMethod.Sampling)
                    || (ruleConfig.BillingMethodId == (int)PriceBillingMethod.ManDay))
                {
                    var totalInvoice = invoiceByDateFactory.Sum(i => i.InspectionFees ?? 0);

                    var minFee = (decimal)ruleConfig.MinBillingDay.GetValueOrDefault();


                    if (minFee > 0)
                    {
                        if ((decimal)totalInvoice < minFee)
                        {
                            if (invoiceByDateFactory.Count == 1)
                            {
                                foreach (var invoice in invoiceTransactions)
                                {
                                    if ((invoice.InspectionDate == dateAndFactoryId.InspectionDate)
                                        && (invoice.FactoryId == dateAndFactoryId.FactoryId))
                                    {
                                        if ((invoice.InspectionId != null) && !isInspectionTranSerIds.Contains(invoice.InspectionId.Value))
                                        {
                                            invoice.InspectionFees = (double)minFee;
                                            invoice.Remarks = MinimumFee + minFee.ToString(NumberFormat);
                                            invoice.PriceCalculationtype = (int)PriceCalculationType.MinFee;
                                        }

                                        break;
                                    }
                                }
                            }
                            else if (invoiceByDateFactory.Count > 1)
                            {
                                var count = 1;
                                foreach (var invoice in invoiceTransactions)
                                {
                                    if ((invoice.InspectionDate == dateAndFactoryId.InspectionDate)
                                        && (invoice.FactoryId == dateAndFactoryId.FactoryId))
                                    {
                                        if ((invoice.InspectionId != null) && isInspectionTranSerIds.Contains(invoice.InspectionId.Value))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (count == 1)
                                            {
                                                invoice.InspectionFees = (double)minFee;
                                                invoice.Remarks = MinimumFee + minFee.ToString(NumberFormat);
                                                invoice.PriceCalculationtype = (int)PriceCalculationType.MinFee;
                                                count++;
                                            }
                                            else
                                            {
                                                invoice.InspectionFees = 0;
                                                invoice.UnitPrice = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return invoiceTransactions;
        }

        /// <summary>
        /// check split by brand invoice is available
        /// </summary>
        /// <param name="splitInvoiceDetails"></param>
        /// <returns></returns>
        public static bool SplitByBrand(List<int> splitInvoiceDetails)
        {
            foreach (var splitInvoiceDetail in splitInvoiceDetails)
            {
                switch (splitInvoiceDetail)
                {
                    case (int)InvoiceGenerationGroupBy.Brand:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        private Tuple<int, int> GetPreInvoiceReportCountAndInspectQty(List<InvoiceBookingDetail> bookingDataList, List<BookingQuantityData> bookingProducts, List<int> groupedBookingIds, List<BookingQuantityData> containerReports)
        {
            int totalInspectedQty = 0;
            int totalReportCount = 0;

            foreach (var bookingId in groupedBookingIds)
            {
                // get combine and non combine product items for specific booking 
                var combineItems = bookingProducts.Where(x => x.CombineProductId > 0 && x.BookingId == bookingId).ToList();
                var NoncombineItems = bookingProducts.Where(x => x.CombineProductId == null && x.BookingId == bookingId).ToList();
                var containerReportList = containerReports.Where(x => x.BookingId == bookingId && x.ContainerId > 0).ToList();

                var bookingData = bookingDataList.FirstOrDefault(x => x.BookingId == bookingId);

                if (bookingData != null && !bookingData.ServiceTypeIds.Contains((int)InspectionServiceTypeEnum.Container))
                {

                    totalInspectedQty = totalInspectedQty + combineItems.Sum(x => x.CombineAqlQuantity.GetValueOrDefault()) +
                                        NoncombineItems.Sum(x => x.AqlQuantity.GetValueOrDefault());

                    totalReportCount = totalReportCount + combineItems.Select(x => x.CombineProductId).Distinct().Count() +
                      NoncombineItems.Select(x => x.ProductId).Distinct().Count();
                }
                else
                {
                    // container case inspected qty will be zero
                    totalInspectedQty = totalInspectedQty + 0;
                    totalReportCount = totalReportCount + containerReportList.Select(x => x.ContainerId).Distinct().Count();
                }
            }

            return Tuple.Create<int, int>(totalInspectedQty, totalReportCount);
        }

        /// <summary>
        /// Calculate unit price for billing method manday 
        /// </summary>
        /// <param name="totalQuantity"></param>
        /// <param name="totalReports"></param>
        /// <param name="ruleConfig"></param>
        /// <returns></returns>
        private Tuple<decimal, int, int, string> CalculateMandayUnitPrice(decimal totalQuantity, int totalReports,
                  CustomerPriceCardRepo ruleConfig, List<BookingQuantityData> bookingProducts)
        {
            decimal returnedUnitPrice = 0;
            var returnedManday = 1;
            decimal configuredProductivity = 0;
            decimal configuredMaxNoOfReports = 0;
            decimal configuredQuantityBuffer = 0;
            decimal configuredUnitPrice = 0;
            var priceCalculationType = (int)PriceCalculationType.Normal;
            string remark = string.Empty;

            if (!ruleConfig.SubCategory2List.Any())
            {
                // sub category2 is not configured but select all is true
                if (ruleConfig.SubCategorySelectAll.GetValueOrDefault())
                {
                    //// Default values
                    configuredProductivity = (decimal)ruleConfig.MandayProductivity.GetValueOrDefault();
                    configuredMaxNoOfReports = (decimal)ruleConfig.MandayReports.GetValueOrDefault();
                    configuredQuantityBuffer = (decimal)ruleConfig.MandayBuffer.GetValueOrDefault();
                    configuredUnitPrice = (decimal)ruleConfig.UnitPrice;
                }
            }
            else
            {
                // sub category2 is configured and filter for grouped booking level 
                var subCategory2IdList = bookingProducts.Select(x => x.ProductSub2CategoryId).Distinct().ToList();
                var subCategory2Manday = ruleConfig.SubCategory2List.FirstOrDefault(x => subCategory2IdList.Contains(x.SubCategory2Id));
                if (subCategory2Manday != null)
                {
                    configuredProductivity = subCategory2Manday.MandayProductivity.GetValueOrDefault();
                    configuredMaxNoOfReports = subCategory2Manday.MandayReports.GetValueOrDefault();
                    configuredQuantityBuffer = (decimal)subCategory2Manday.MandayBuffer.GetValueOrDefault();
                    configuredUnitPrice = (decimal)subCategory2Manday.UnitPrice.GetValueOrDefault();
                }
            }

            // Checking special rules
            var matchSpecialRule = false;
            foreach (var specialRule in ruleConfig.RuleList)
            {
                if ((specialRule.MandayProductivity >= totalQuantity) && (specialRule.MandayReports >= totalReports))
                {
                    matchSpecialRule = true;
                    returnedUnitPrice = (decimal)specialRule.UnitPrice.GetValueOrDefault();
                    priceCalculationType = (int)PriceCalculationType.SpecialPrice;
                    remark = SpecialFee.ToString();
                    break;
                }
            }

            // Checking normal rules
            if (!matchSpecialRule)
            {
                returnedUnitPrice = configuredUnitPrice;
                decimal mandayQuantity = 1;
                decimal mandayInspectionReport = 1;
                if (configuredProductivity > 0)
                {
                    mandayQuantity = totalQuantity / configuredProductivity;
                    if (mandayQuantity <= 1)
                    {
                        mandayQuantity = 1;
                    }
                    else
                    {
                        var ceilingMandayQuantity = Math.Ceiling(mandayQuantity);
                        var floorMandayQuantity = Math.Floor(mandayQuantity);

                        // check the Buffer Productivity 
                        var valueWithBuffer = floorMandayQuantity * configuredProductivity + (configuredQuantityBuffer / configuredProductivity) * 100;

                        if (totalQuantity > valueWithBuffer)
                        {
                            mandayQuantity = ceilingMandayQuantity;
                        }
                        else
                        {
                            mandayQuantity = floorMandayQuantity;
                        }
                    }
                }

                if (configuredMaxNoOfReports > 0)
                {
                    mandayInspectionReport = totalReports / configuredMaxNoOfReports;
                    mandayInspectionReport = Math.Ceiling(mandayInspectionReport);
                }

                returnedManday = decimal.ToInt32(mandayQuantity);
                if (returnedManday < mandayInspectionReport)
                {
                    returnedManday = decimal.ToInt32(mandayInspectionReport);
                }
            }
            return Tuple.Create<decimal, int, int, string>(returnedUnitPrice, returnedManday, priceCalculationType, remark);
        }

        /// <summary>
        /// get Calculate Audit Fees From Quotation
        /// </summary>
        /// <param name="orderTransactions"></param>
        /// <param name="invoiceList"></param>
        /// <param name="ruleConfig"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task<List<InvoiceDetail>> CalculateAuditFeesFromQuotation(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings)
        {

            // get only rule specific inspection
            var orderList = (from order in orderTransactions
                             where order.RuleConfig.Id == ruleConfig.Id
                             select order).ToList();

            var auditIds = orderList.Select(x => x.AuditId).Distinct().ToList();
            var quotationDataList = await _invoiceRepository.GetQuotationDataByAuditList(auditIds);

            foreach (var orderTransaction in orderList)
            {
                // check already audit is not added for invoice
                if (!invoiceList.Any(x => x.AuditId == orderTransaction.AuditId))
                {
                    double inspectionFees = 0;
                    double unitPrice = 0;
                    double otherFees = 0;
                    double mandays = 0;
                    double discount = 0;

                    var quotationData = quotationDataList.FirstOrDefault(x => x.BookingId == orderTransaction.AuditId);

                    if (quotationData != null)
                    {
                        inspectionFees = quotationData.InspectionFees;
                        unitPrice = quotationData.UnitPrice;
                        mandays = quotationData.Mandays;

                        if ((!invoiceList.Any(x => x.QuotationId == quotationData.QuotationId)) &&
                           (quotationBookings == null || (quotationBookings != null && !quotationBookings.Any(x => x.QuotationId ==
                            quotationData.QuotationId))))
                        {
                            // set other fees and discount 
                            if (ruleConfig.InvoiceOtherFeeFrom == (int)InvoiceFeesFrom.Quotation)
                                otherFees = quotationData.OtherCost;

                            if (ruleConfig.InvoiceDiscountFeeFrom == (int)InvoiceFeesFrom.Quotation)
                                discount = quotationData.Discount;
                        }
                    }

                    // if quotation data billed to and invoice ui billed to same then generate invoice.
                    if (quotationData != null && quotationData.BillingTo == requestDto.InvoiceTo)
                    {
                        var invoiceTransaction = new InvoiceDetail()
                        {
                            IsAutomation = true,
                            ExchangeRate = null,
                            InvoiceMethod = ruleConfig.BillingMethodId,
                            InvoiceCurrency = ruleConfig.CurrencyId,
                            PriceCardCurrency = ruleConfig.CurrencyId,
                            RuleExchangeRate = null,
                            InvoiceDate = DateTime.Now,
                            InspectionDate = orderTransaction.ServiceTo,
                            InvoiceStatus = (int)InvoiceStatus.Created,
                            InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                            FactoryId = orderTransaction.FactoryId,
                            FactoryCountryId = orderTransaction.FactoryCountryId,
                            FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                            FactoryCountryName = orderTransaction.FactoryCountryName,
                            UnitPrice = unitPrice,
                            InspectionFees = inspectionFees,
                            TotalInvoiceFees = 0,
                            TotalSampleSize = 0,
                            OtherFees = otherFees,
                            Discount = discount,
                            InspectionId = null,
                            Subject = "",
                            ManDays = mandays,
                            RuleId = ruleConfig.Id,
                            TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                            InvoiceTo = ruleConfig.BillingToId,
                            CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                            CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                            CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                            CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                            CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                            BankId = ruleConfig.BankAccount,
                            PaymentTerms = quotationData.PaymentTermsValue,
                            PaymentDuration = quotationData.PaymentTermsCount?.ToString(),
                            IsInspection = true,
                            InvoicePaymentDate = null,
                            Remarks = "",
                            GroupBy = orderTransaction.GroupBy,
                            Rule = ruleConfig,
                            BookingDepartmentIds = orderTransaction.DepartmentIds,
                            BookingBuyerIds = orderTransaction.BuyerIds,
                            BookingBrandIds = orderTransaction.BrandIds,
                            Office = ruleConfig.InvoiceOffice,
                            InvoiceType = requestDto.InvoiceType,
                            AuditId = orderTransaction.AuditId,
                            QuotationId = quotationData.QuotationId
                        };
                        invoiceList.Add(invoiceTransaction);
                    }
                }
            }
            return invoiceList;
        }

        private async Task<List<InvoiceDetail>> CalculateAuditFeesFromInvoiceRule(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings)
        {

            // get only rule specific inspection
            var orderList = (from order in orderTransactions
                             where order.RuleConfig.Id == ruleConfig.Id
                             select order).ToList();

            foreach (var orderTransaction in orderList)
            {
                // check already audit is not added for invoice
                if (!invoiceList.Any(x => x.AuditId == orderTransaction.AuditId))
                {
                    double inspectionFees = ruleConfig.UnitPrice;
                    double unitPrice = ruleConfig.UnitPrice;
                    double otherFees = 0;
                    double mandays = 1;
                    double discount = 0;


                    var invoiceTransaction = new InvoiceDetail()
                    {
                        IsAutomation = true,
                        ExchangeRate = null,
                        InvoiceMethod = ruleConfig.BillingMethodId,
                        InvoiceCurrency = ruleConfig.CurrencyId,
                        PriceCardCurrency = ruleConfig.CurrencyId,
                        RuleExchangeRate = null,
                        InvoiceDate = DateTime.Now,
                        InspectionDate = orderTransaction.ServiceTo,
                        InvoiceStatus = (int)InvoiceStatus.Created,
                        InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                        FactoryId = orderTransaction.FactoryId,
                        FactoryCountryId = orderTransaction.FactoryCountryId,
                        FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                        FactoryCityId = orderTransaction.FactoryCityId.GetValueOrDefault(),
                        FactoryProvinceId = orderTransaction.FactoryProvinceId.GetValueOrDefault(),
                        FactoryCountryName = orderTransaction.FactoryCountryName,
                        UnitPrice = unitPrice,
                        InspectionFees = inspectionFees,
                        TotalInvoiceFees = 0,
                        TotalSampleSize = 0,
                        OtherFees = otherFees,
                        Discount = discount,
                        InspectionId = null,
                        Subject = "",
                        ManDays = mandays,
                        RuleId = ruleConfig.Id,
                        TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                        InvoiceTo = ruleConfig.BillingToId,
                        CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                        CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                        CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                        CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                        CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                        BankId = ruleConfig.BankAccount,
                        PaymentTerms = ruleConfig.PaymentTerms,
                        PaymentDuration = ruleConfig.PaymentDuration?.ToString(),
                        IsInspection = true,
                        InvoicePaymentDate = null,
                        Remarks = "",
                        GroupBy = orderTransaction.GroupBy,
                        Rule = ruleConfig,
                        BookingDepartmentIds = orderTransaction.DepartmentIds,
                        BookingBuyerIds = orderTransaction.BuyerIds,
                        BookingBrandIds = orderTransaction.BrandIds,
                        Office = ruleConfig.InvoiceOffice,
                        InvoiceType = requestDto.InvoiceType,
                        AuditId = orderTransaction.AuditId
                    };
                    invoiceList.Add(invoiceTransaction);
                }
            }
            return invoiceList;
        }


        /// <summary>
        /// Calculate the logic per intervention range 
        /// </summary>
        /// <param name="orderList"></param>
        /// <param name="invoiceList"></param>
        /// <param name="ruleConfig"></param>
        /// <param name="requestDto"></param>
        /// <param name="quotationBookings"></param>
        /// <returns></returns>
        private async Task<List<InvoiceDetail>> CalculatePerInterventionRangeAudit(List<InvoiceBookingDetail> orderList, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings)
        {

            var dateAndFactoryIds = (from order in orderList
                                     where order.RuleConfig.Id == ruleConfig.Id
                                     orderby order.ServiceTo, order.FactoryId
                                     select new
                                     {
                                         order.ServiceTo,
                                         order.FactoryId
                                     }).Distinct();

            foreach (var dateAndFactoryId in dateAndFactoryIds)
            {
                var orderListByDate = (from order in orderList
                                       where order.FactoryId == dateAndFactoryId.FactoryId
                                        && order.ServiceTo == dateAndFactoryId.ServiceTo
                                        && order.RuleConfig.Id == ruleConfig.Id
                                       select order).ToList();

                foreach (var orderTransaction in orderListByDate)
                {
                    double unitPrice = 0;
                    var totalStaff = 0;

                    if (orderTransaction.TotalStaff != null)
                    {
                        totalStaff = orderTransaction.TotalStaff.Value;
                    }

                    foreach (var range in ruleConfig.RuleList)
                    {
                        if ((totalStaff >= range.PerInterventionRange1) && (totalStaff <= range.PerInterventionRange2))
                        {
                            unitPrice = range.UnitPrice ?? 0;
                            break;
                        }
                    }

                    if (unitPrice > 0)
                    {
                        var invoiceTransaction = new InvoiceDetail()
                        {
                            IsAutomation = true,
                            ExchangeRate = null,
                            InvoiceMethod = ruleConfig.BillingMethodId,
                            InvoiceCurrency = ruleConfig.CurrencyId,
                            PriceCardCurrency = ruleConfig.CurrencyId,
                            RuleExchangeRate = null,
                            InvoiceDate = DateTime.Now,
                            InspectionDate = orderTransaction.ServiceTo,
                            InvoiceStatus = (int)InvoiceStatus.Created,
                            InvoicePaymentStatus = (int)InvoicePaymentStatus.NotPaid,
                            FactoryId = orderTransaction.FactoryId,
                            FactoryCountryId = orderTransaction.FactoryCountryId,
                            FactoryCountyId = orderTransaction.FactoryCountyId.GetValueOrDefault(),
                            FactoryCountryName = orderTransaction.FactoryCountryName,
                            UnitPrice = unitPrice,
                            InspectionFees = unitPrice,
                            TotalInvoiceFees = 0,
                            TotalSampleSize = 0,
                            OtherFees = 0,
                            Discount = 0,
                            InspectionId = null,
                            Subject = "",
                            ManDays = 0,
                            RuleId = ruleConfig.Id,
                            TravelMatrixType = ruleConfig.TravelMatrixTypeId,
                            InvoiceTo = ruleConfig.BillingToId,
                            CalculateInspectionFee = ruleConfig.InvoiceInspFeeFrom,
                            CalculateDiscountFee = ruleConfig.InvoiceDiscountFeeFrom,
                            CalculateHotelFee = ruleConfig.InvoiceHotelFeeFrom,
                            CalculateOtherFee = ruleConfig.InvoiceOtherFeeFrom,
                            CalculateTravelExpense = ruleConfig.InvoiceTravelExpense,
                            BankId = ruleConfig.BankAccount,
                            PaymentTerms = ruleConfig.PaymentTerms,
                            PaymentDuration = ruleConfig.PaymentDuration?.ToString(),
                            IsInspection = true,
                            InvoicePaymentDate = null,
                            Remarks = "",
                            GroupBy = orderTransaction.GroupBy,
                            Rule = ruleConfig,
                            BookingDepartmentIds = orderTransaction.DepartmentIds,
                            BookingBuyerIds = orderTransaction.BuyerIds,
                            BookingBrandIds = orderTransaction.BrandIds,
                            Office = ruleConfig.InvoiceOffice,
                            InvoiceType = requestDto.InvoiceType,
                            AuditId = orderTransaction.AuditId
                        };
                        invoiceList.Add(invoiceTransaction);
                    }
                }

                // Check Min, Max fee
                var decTotalFees = invoiceList.Select(i => i.InspectionFees ?? 0).Sum();
                var remark = string.Empty;
                var priceCalculationType = 0;
                if (decTotalFees < ruleConfig.MinBillingDay)
                {
                    decTotalFees = ruleConfig.MinBillingDay.GetValueOrDefault();
                    remark = MinimumFee + decTotalFees.ToString(NumberFormat);
                    priceCalculationType = (int)PriceCalculationType.MinFee;
                }
                else if (decTotalFees > ruleConfig.MaxFeeStyle)
                {
                    decTotalFees = (double)ruleConfig.MaxFeeStyle;

                    remark = MaximumFee + decTotalFees.ToString(NumberFormat);

                    priceCalculationType = (int)PriceCalculationType.MaxFee;
                }

                if (!string.IsNullOrWhiteSpace(remark))
                {
                    var first = true;
                    foreach (var invoice in invoiceList)
                    {
                        if (first)
                        {
                            invoice.InspectionFees = decTotalFees;
                            invoice.Remarks = invoice.Remarks + remark;
                            invoice.PriceCalculationtype = priceCalculationType;
                            first = false;
                        }
                        else
                        {
                            invoice.InspectionFees = 0;
                            invoice.UnitPrice = 0;
                        }
                    }
                }
            }

            return invoiceList;
        }

        private async Task<InvoiceSamplingUnitPrice> GetCarrefourUnitPrice(List<int> bookingIdList, CustomerPriceCardRepo ruleConfig)
        {
            #region VariableDeclaration

            double unitPrice = 0;
            int? totalSampling = 0;
            int? totalSampleSizewithnextItem = 0;
            int totalBookingQty = 0;
            int maxProductCount = 0;
            bool sampleSizeBySet = false;
            int rowIndex = 0;
            var resultData = new InvoiceSamplingUnitPrice();
            List<CombineBookingGroup> combineGroupIds = new List<CombineBookingGroup>();
            List<CustomerPriceBookingProduct> bookingProductList = new List<CustomerPriceBookingProduct>();
            CustomerPriceBookingProduct nextBookingProduct = null;

            #endregion

            //Get inspection product details by booking list based on perdayandfactory
            var productTransactions = await _invoiceRepository.GetProductDetailsByBookingList(bookingIdList);

            // set yards to meter 
            foreach (var item in productTransactions)
            {
                if (item.UnitId == (int)BookingProductUnitType.Yards)
                {
                    item.BookingQuantity = item.BookingQuantity * 0.9144;
                }
            }

            //sort the booking by customer booking number and product form serial
            productTransactions = productTransactions.OrderBy(x => x.CusbookingNumber).
                ThenByDescending(x => x.FormSerialNumber.HasValue).ThenBy(x => x.FormSerialNumber).ToList();

            //Get price card rule
            var customerPriceCardRule = ruleConfig;

            if (productTransactions.Any() && customerPriceCardRule != null)
            {
                //get samplesizebyset from the rule            
                sampleSizeBySet = customerPriceCardRule.SampleSizeBySet.GetValueOrDefault();

                //get maxproductcount from the rule

                maxProductCount = customerPriceCardRule.MaxProductCount.GetValueOrDefault();

                // Quotation sampling method applied for all the products except master products
                var noDisplayMasterandChildProductList = productTransactions.
                    Where(x => !x.IsDispalyMaster.GetValueOrDefault()).ToList();


                var productLength = noDisplayMasterandChildProductList.Count;

                // set max lenth from product count 
                if (maxProductCount == 0)
                {
                    maxProductCount = productLength;
                }

                for (int i = 0; i <= noDisplayMasterandChildProductList.Count - 1; i++)
                {
                    // check grouped combine items and add only ones - no child product combine
                    if (noDisplayMasterandChildProductList[i].CombineProductId != null && noDisplayMasterandChildProductList[i].ParentProductId == null &&
                        (combineGroupIds.Where(x => x.BookingId == noDisplayMasterandChildProductList[i].BookingId &&
                        x.CombineGroupId == noDisplayMasterandChildProductList[i].CombineProductId.GetValueOrDefault()).Count() == 0))
                    {
                        //take combine aql quantity.since combine aql quantity will be updated for one product per combine group
                        var combineAQLQuantity = noDisplayMasterandChildProductList.Where(x => x.CombineProductId == noDisplayMasterandChildProductList[i].CombineProductId
                                                       && x.BookingId == noDisplayMasterandChildProductList[i].BookingId
                                                       && x.CombinedAQLQuantity > 0).Select(x => x.CombinedAQLQuantity).FirstOrDefault();
                        //add the customer price booking product
                        bookingProductList.Add(AddCustomerPriceCardTransaction(noDisplayMasterandChildProductList[i], sampleSizeBySet, combineAQLQuantity));
                        //add the combine product id into the combinegroupids list
                        combineGroupIds.Add(new CombineBookingGroup()
                        { CombineGroupId = noDisplayMasterandChildProductList[i].CombineProductId.GetValueOrDefault(), BookingId = noDisplayMasterandChildProductList[i].BookingId });
                    }
                    // only child product combine
                    else if (noDisplayMasterandChildProductList[i].CombineProductId != null && noDisplayMasterandChildProductList[i].ParentProductId != null)
                    {


                        // take combine aql quantity.since combine aql quantity will be updated for one product per combine group
                        var combineAQLQuantity = noDisplayMasterandChildProductList.Where(x => x.CombineProductId == noDisplayMasterandChildProductList[i].CombineProductId
                                                       && x.BookingId == noDisplayMasterandChildProductList[i].BookingId
                                                       && x.CombinedAQLQuantity > 0).Select(x => x.CombinedAQLQuantity).FirstOrDefault();

                        CustomerPriceBookingProduct bookingProduct = new CustomerPriceBookingProduct();
                        bookingProduct.Id = noDisplayMasterandChildProductList[i].ProductId;
                        bookingProduct.Name = noDisplayMasterandChildProductList[i].ProductName;
                        double totalDisplayGroupBookingQty = 0;

                        totalDisplayGroupBookingQty = noDisplayMasterandChildProductList.Where(x => x.CombineProductId == noDisplayMasterandChildProductList[i].CombineProductId
                                                       && x.BookingId == noDisplayMasterandChildProductList[i].BookingId
                                                       ).Select(x => x.BookingQuantity).Sum();

                        double prorateQty = 0;

                        if (totalDisplayGroupBookingQty > 0)
                        {
                            prorateQty = (double)noDisplayMasterandChildProductList[i].BookingQuantity / totalDisplayGroupBookingQty;

                            bookingProduct.SamplingSize = (int)(prorateQty * combineAQLQuantity.GetValueOrDefault());
                        }

                        bookingProductList.Add(bookingProduct);

                    }

                    else if (noDisplayMasterandChildProductList[i].CombineProductId == null)
                    {
                        //add the customer price booking product
                        bookingProductList.Add(AddCustomerPriceCardTransaction(noDisplayMasterandChildProductList[i], sampleSizeBySet, null));
                    }

                    //update row read in the product list
                    rowIndex = rowIndex + 1;
                    if (i < noDisplayMasterandChildProductList.Count - 1)
                    {
                        // check grouped combine items and add only ones
                        if (noDisplayMasterandChildProductList[i + 1]?.CombineProductId != null &&
                            (combineGroupIds.Where(x => x.BookingId == noDisplayMasterandChildProductList[i + 1]?.BookingId &&
                            x.CombineGroupId == noDisplayMasterandChildProductList[i + 1]?.CombineProductId.GetValueOrDefault()).Count() == 0))
                        {
                            //take combine aql quantity.since combine aql quantity will be updated for one product per combine group
                            var combineAQLQuantity = noDisplayMasterandChildProductList.Where(x => x.CombineProductId == noDisplayMasterandChildProductList[i + 1]?.CombineProductId
                                                           && x.BookingId == noDisplayMasterandChildProductList[i + 1]?.BookingId
                                                           && x.CombinedAQLQuantity > 0).Select(x => x.CombinedAQLQuantity).FirstOrDefault();

                            nextBookingProduct = AddCustomerPriceCardTransaction(noDisplayMasterandChildProductList[i + 1], sampleSizeBySet, combineAQLQuantity);


                        }
                        // child product logic
                        else if (noDisplayMasterandChildProductList[i + 1].CombineProductId != null && noDisplayMasterandChildProductList[i + 1].ParentProductId != null)
                        {

                            // take combine aql quantity.since combine aql quantity will be updated for one product per combine group
                            var combineAQLQuantity = noDisplayMasterandChildProductList.Where(x => x.CombineProductId == noDisplayMasterandChildProductList[i + 1].CombineProductId
                                                           && x.BookingId == noDisplayMasterandChildProductList[i + 1].BookingId
                                                           && x.CombinedAQLQuantity > 0).Select(x => x.CombinedAQLQuantity).FirstOrDefault();

                            nextBookingProduct = new CustomerPriceBookingProduct();
                            nextBookingProduct.Id = noDisplayMasterandChildProductList[i + 1].ProductId;
                            nextBookingProduct.Name = noDisplayMasterandChildProductList[i + 1].ProductName;
                            double totalDisplayGroupBookingQty = 0;

                            totalDisplayGroupBookingQty = noDisplayMasterandChildProductList.Where(x => x.CombineProductId == noDisplayMasterandChildProductList[i + 1].CombineProductId
                                                           && x.BookingId == noDisplayMasterandChildProductList[i + 1].BookingId
                                                           ).Select(x => x.BookingQuantity).Sum();

                            double prorateQty = 0;

                            if (totalDisplayGroupBookingQty > 0)
                            {
                                prorateQty = (double)noDisplayMasterandChildProductList[i + 1].BookingQuantity / totalDisplayGroupBookingQty;

                                nextBookingProduct.SamplingSize = (int)(prorateQty * combineAQLQuantity.GetValueOrDefault());
                            }
                        }

                        else if (noDisplayMasterandChildProductList[i + 1]?.CombineProductId == null)
                        {
                            //add the customer price booking product
                            nextBookingProduct = AddCustomerPriceCardTransaction(noDisplayMasterandChildProductList[i + 1], sampleSizeBySet, null);

                        }
                    }
                    else
                    {
                        nextBookingProduct = new CustomerPriceBookingProduct();
                    }


                    totalSampleSizewithnextItem = bookingProductList.Select(x => x.SamplingSize).Sum() + nextBookingProduct?.SamplingSize;

                    var isMaxSampleSizeExceed = CheckMaxSampleSize(totalSampleSizewithnextItem.GetValueOrDefault(), customerPriceCardRule);

                    //calculate the unit price if booking product list count equals to the maxproductcount(if it is set)
                    //or all the rows in the product list has been read
                    if ((maxProductCount > 0 && (bookingProductList.Count() == maxProductCount || isMaxSampleSizeExceed)) || (productLength - rowIndex) == 0)
                    {
                        //calculate the unit price with sampling data
                        unitPrice = unitPrice + CalculateUnitPrice(bookingProductList, customerPriceCardRule);
                        totalSampling = totalSampling + bookingProductList.Sum(x => x.SamplingSize);
                        //clear the product list so that the list will be fresh for the next cycle.
                        bookingProductList.Clear();
                    }
                }
            }
            resultData.UnitPrice = unitPrice;
            resultData.TotalSamplingSize = totalSampling;
            resultData.TotalBookingQuantity = totalBookingQty;
            resultData.bookingProductList = productTransactions;
            return resultData;
        }


        private async Task<InvoiceSamplingUnitPrice> GetSamplingAndUnitPrice(int bookingId, CustomerPriceCardRepo ruleConfig)
        {
            #region VariableDeclaration

            double unitPrice = 0;
            int? totalSampling = 0;
            int maxProductCount = 0;
            bool sampleSizeBySet = false;
            int rowIndex = 0;
            var resultData = new InvoiceSamplingUnitPrice();
            List<int> combineGroupIds = new List<int>();
            List<CustomerPriceBookingProduct> bookingProductList = new List<CustomerPriceBookingProduct>();

            #endregion

            //Get the Inspection Product Transaction Data
            var productTransactions = await _invoiceRepository.GetBookingProductDetails(bookingId);
            //Get the customer price card sampling data
            var customerPriceCardSampling = ruleConfig;
            if (productTransactions != null && productTransactions.Any() && customerPriceCardSampling != null)
            {
                //Sort the product transaction data by product name
                productTransactions = productTransactions.OrderBy(x => x.ProductName).ToList();

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
                        unitPrice = unitPrice + CalculateSamplingUnitPrice(bookingProductList, customerPriceCardSampling);
                        totalSampling = totalSampling + bookingProductList.Sum(x => x.SamplingSize);
                        //clear the product list so that the list will be fresh for the next cycle.
                        bookingProductList.Clear();
                    }

                }
            }

            resultData.UnitPrice = unitPrice;
            resultData.TotalSamplingSize = totalSampling;
            return resultData;
        }



        private bool CheckMaxSampleSize(int totalSamplingSize, CustomerPriceCardRepo customerPriceCardSampling)
        {
            //assign the max sample size
            var maxSampleSize = customerPriceCardSampling.MaxSampleSize.GetValueOrDefault();

            return totalSamplingSize > maxSampleSize;
        }

        private double GetQuantityBasedOnBilledQuantity(int billedQuantity, CustomerPriceBookingProducts productTransaction)
        {
            switch (billedQuantity)
            {
                case (int)QuantityType.Ordered:
                    return productTransaction.BookingQuantity;
                case (int)QuantityType.Presented:
                    return productTransaction.PresentedQuantity.GetValueOrDefault();
                case (int)QuantityType.Inspected:
                    return productTransaction.InspectedQuanity.GetValueOrDefault();
                default:
                    return 0;
            }
        }

        private double CalculateSamplingUnitPrice(List<CustomerPriceBookingProduct> bookingProductList, CustomerPriceCardRepo customerPriceCardSampling)
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
                    unitPrice = unitPrice + GetQuantitySamplingValue(bookingProductList, totalSamplingSize, customerPriceCardSampling);
                }
                //if total sampling size greater than max sample size
                else if (totalSamplingSize > maxSampleSize)
                {
                    //if additional sample size is configured
                    if (customerPriceCardSampling.AdditionalSampleSize != null && customerPriceCardSampling.AdditionalSampleSize > 0)
                    {
                        //take the unit price for the max sample size
                        unitPrice = unitPrice + GetQuantitySamplingValue(bookingProductList, maxSampleSize, customerPriceCardSampling);

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
                        unitPrice = unitPrice + GetQuantitySamplingValue(bookingProductList, maxSampleSize, customerPriceCardSampling);
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
                            unitPrice = unitPrice + GetQuantitySamplingValue(bookingProductList, remainingSampleSize, customerPriceCardSampling);

                    }
                }
            }

            return unitPrice;
        }


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
                    unitPrice = unitPrice + GetQuantitySamplingValue(bookingProductList, totalSamplingSize, customerPriceCardSampling);
                }
                //if total sampling size greater than max sample size
                else if (totalSamplingSize > maxSampleSize)
                {
                    //if additional sample size is configured
                    if (customerPriceCardSampling.AdditionalSampleSize != null && customerPriceCardSampling.AdditionalSampleSize > 0)
                    {
                        //take the unit price for the max sample size
                        unitPrice = unitPrice + GetQuantitySamplingValue(bookingProductList, maxSampleSize, customerPriceCardSampling);

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
                        unitPrice = unitPrice + GetQuantitySamplingValue(bookingProductList, maxSampleSize, customerPriceCardSampling);
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
                            unitPrice = unitPrice + GetQuantitySamplingValue(bookingProductList, remainingSampleSize, customerPriceCardSampling);

                    }
                }
            }

            return unitPrice;
        }


        /// <summary>
        /// Get Quantity Sampliing Value based on the Rule Configuration
        /// </summary>
        /// <param name="totalSamplingSize"></param>
        /// <param name="customerPriceCardSampling"></param>
        /// <returns></returns>
        private double GetQuantitySamplingValue(List<CustomerPriceBookingProduct> bookingProductList, int totalSamplingSize, CustomerPriceCardRepo customerPriceCardSampling)
        {

            if (customerPriceCardSampling.PriceComplexType == null || customerPriceCardSampling.PriceComplexType == (int)PriceComplexType.Simple)
            {
                return GetCommonSamplingValue(totalSamplingSize, customerPriceCardSampling);
            }

            else if (customerPriceCardSampling.PriceComplexType == (int)PriceComplexType.Complex)
            {
                if (!customerPriceCardSampling.SubCategory2List.Any())
                {
                    if (customerPriceCardSampling.SubCategorySelectAll.GetValueOrDefault())
                    {
                        return GetCommonSamplingValue(totalSamplingSize, customerPriceCardSampling);
                    }
                }
                else
                {
                    var subCategory2IdList = bookingProductList.Where(x => x.SubCategory2Id != null).Select(x => x.SubCategory2Id).ToList();
                    var subCategory2Sampling = customerPriceCardSampling.
                        SubCategory2List.FirstOrDefault(x => subCategory2IdList.Contains(x.SubCategory2Id));

                    if (subCategory2Sampling != null)
                    {
                        return GetSubcategorySamplingValue(totalSamplingSize, subCategory2Sampling);
                    }
                }
            }
            return 0;
        }

        private double GetCommonSamplingValue(int totalSamplingSize, CustomerPriceCardRepo customerPriceCardSampling)
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

        private double GetSubcategorySamplingValue(int totalSamplingSize, PriceSubCategory customerPriceCardSampling)
        {
            if (totalSamplingSize >= 1 && totalSamplingSize <= 8 && customerPriceCardSampling.AQL_QTY_8.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_8.GetValueOrDefault();

            else if (totalSamplingSize >= 9 && totalSamplingSize <= 13 && customerPriceCardSampling.AQL_QTY_13.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_13.GetValueOrDefault();

            else if (totalSamplingSize >= 14 && totalSamplingSize <= 20 && customerPriceCardSampling.AQL_QTY_20.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_20.GetValueOrDefault();

            else if (totalSamplingSize >= 21 && totalSamplingSize <= 32 && customerPriceCardSampling.AQL_QTY_32.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_32.GetValueOrDefault();

            else if (totalSamplingSize >= 33 && totalSamplingSize <= 50 && customerPriceCardSampling.AQL_QTY_50.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_50.GetValueOrDefault();

            else if (totalSamplingSize >= 51 && totalSamplingSize <= 80 && customerPriceCardSampling.AQL_QTY_80.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_80.GetValueOrDefault();

            else if (totalSamplingSize >= 81 && totalSamplingSize <= 125 && customerPriceCardSampling.AQL_QTY_125.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_125.GetValueOrDefault();

            else if (totalSamplingSize >= 126 && totalSamplingSize <= 200 && customerPriceCardSampling.AQL_QTY_200.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_200.GetValueOrDefault();

            else if (totalSamplingSize >= 201 && totalSamplingSize <= 315 && customerPriceCardSampling.AQL_QTY_315.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_315.GetValueOrDefault();

            else if (totalSamplingSize >= 316 && totalSamplingSize <= 500 && customerPriceCardSampling.AQL_QTY_500.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_500.GetValueOrDefault();

            else if (totalSamplingSize >= 501 && totalSamplingSize <= 800 && customerPriceCardSampling.AQL_QTY_800.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_800.GetValueOrDefault();

            else if (totalSamplingSize >= 801 && totalSamplingSize <= 1250 && customerPriceCardSampling.AQL_QTY_1250.GetValueOrDefault() > 0)
                return customerPriceCardSampling.AQL_QTY_1250.GetValueOrDefault();
            return 0;
        }

        private CustomerPriceBookingProduct AddCustomerPriceCardTransaction(CustomerPriceBookingProducts productTransaction, bool sampleSizeBySet, int? combineAQLQuantity)
        {
            CustomerPriceBookingProduct bookingProduct = new CustomerPriceBookingProduct();
            if (productTransaction != null)
            {
                bookingProduct.Id = productTransaction.ProductId;
                bookingProduct.Name = productTransaction.ProductName;
                bookingProduct.BookingQuantity = productTransaction.BookingQuantity;
                bookingProduct.SubCategory2Id = productTransaction.SubCategory2Id;

                //if the product is combine assign the conbined aql quantity
                if (productTransaction.CombineProductId != null && combineAQLQuantity != null && combineAQLQuantity > 0)
                    bookingProduct.SamplingSize = combineAQLQuantity.GetValueOrDefault();
                //if the product is non combined add the aql quantity
                else if (productTransaction.CombineProductId == null && productTransaction.AQLQuantity != null && productTransaction.AQLQuantity > 0)
                    bookingProduct.SamplingSize = productTransaction.AQLQuantity.GetValueOrDefault();

                //if samplesizebyset is enabled calculate samplingsize with unitcount value
                if (sampleSizeBySet && productTransaction.UnitCount != null && productTransaction.UnitCount > 0)
                    bookingProduct.SamplingSize = bookingProduct.SamplingSize * productTransaction.UnitCount.GetValueOrDefault();
            }

            return bookingProduct;
        }


        /// <summary>
        /// get group by values
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="inspectedOrder"></param>
        /// <returns></returns>
        private string GetGroupByValues(InvoiceGenerateRequest requestDto, InvoiceBookingDetail inspectedOrder)
        {
            var groupBy = new StringBuilder(string.Empty);

            if (requestDto.SplitInvoice != null && requestDto.SplitInvoice.Any())
            {
                foreach (var splitInvoiceDetail in requestDto.SplitInvoice)
                {
                    switch (splitInvoiceDetail)
                    {
                        case (int)InvoiceGenerationGroupBy.Supplier:
                            groupBy.Append(inspectedOrder.SupplierId.ToString() + ',');
                            break;

                        case (int)InvoiceGenerationGroupBy.ServiceType:
                            groupBy.Append(string.Join(",", inspectedOrder.ServiceTypeIds.Select(n => n.ToString()).ToArray()) + ',');
                            break;

                        case (int)InvoiceGenerationGroupBy.Country:
                            groupBy.Append(inspectedOrder.FactoryCountryId.ToString() + ',');
                            break;

                        case (int)InvoiceGenerationGroupBy.Brand:
                            groupBy.Append(string.Join(",", inspectedOrder.BrandIds.Select(n => n.ToString()).ToArray()) + ',');
                            break;

                        case (int)InvoiceGenerationGroupBy.Department:
                            groupBy.Append(string.Join(",", inspectedOrder.DepartmentIds.Select(n => n.ToString()).ToArray()) + ',');
                            break;

                        case (int)InvoiceGenerationGroupBy.Buyer:
                            groupBy.Append(string.Join(",", inspectedOrder.BuyerIds.Select(n => n.ToString()).ToArray()) + ',');
                            break;

                        case (int)InvoiceGenerationGroupBy.CustomerContact:
                            groupBy.Append(string.Join(",", inspectedOrder.CustomerContactIds.Select(n => n.ToString()).ToArray()) + ',');
                            break;

                        case (int)InvoiceGenerationGroupBy.ProductCategory:
                            groupBy.Append(inspectedOrder.ProductCategoryIds.ToString() + ',');
                            break;

                        case (int)InvoiceGenerationGroupBy.BookingNo:
                            groupBy.Append(inspectedOrder.BookingId.ToString() + ',');
                            break;
                    }
                }
            }

            return groupBy.ToString();
        }


        /// <summary>
        /// get rule config based on the booking and price card combinations
        /// </summary>
        /// <param name="orderTransactionDto"></param>
        /// <param name="ruleConfigs"></param>
        /// <returns></returns>
        public async Task<List<CustomerPriceCardRepo>> GetRuleConfigListbyBookingFilter(InvoiceBookingDetail orderTransactionDto, IEnumerable<CustomerPriceCardRepo> ruleConfigs)
        {
            var ruleConfig = ruleConfigs.Where(x => !((x.PeriodFrom.ToDateTime() > orderTransactionDto.ServiceTo) ||
                                                                     (x.PeriodTo.ToDateTime() < orderTransactionDto.ServiceFrom))).ToList();

            //booking supplier id contains in price card record
            if (ruleConfig.Select(x => x.SupplierIdList) != null && ruleConfig.Where(x => x.SupplierIdList.Any()).Any())
            {
                ruleConfig = ruleConfig.Where(x => (!x.SupplierIdList.Any()) || x.SupplierIdList.Contains(orderTransactionDto.SupplierId)).ToList();
            }

            //booking ServiceTypeIdList contains in price card record
            if (ruleConfig.Select(x => x.ServiceTypeIdList) != null && ruleConfig.Where(x => x.ServiceTypeIdList.Any()).Any())
            {
                ruleConfig = ruleConfig.Where(x => (!x.ServiceTypeIdList.Any()) || orderTransactionDto.ServiceTypeIds.Any(x.ServiceTypeIdList.Contains)).ToList();
            }

            //booking CountryIdList contains in price card record
            if (ruleConfig.Select(x => x.FactoryCountryIdList) != null && ruleConfig.Where(x => x.FactoryCountryIdList.Any()).Any())
            {
                ruleConfig = ruleConfig.Where(x => (!x.FactoryCountryIdList.Any()) || x.FactoryCountryIdList.Contains(orderTransactionDto.FactoryCountryId)).ToList();
            }

            if (ruleConfig.Select(x => x.FactoryProvinceIdList) != null && ruleConfig.Where(x => x.FactoryProvinceIdList.Any()).Any())
            {
                if (orderTransactionDto.FactoryProvinceId != null)
                    ruleConfig = ruleConfig.Where(x => (!x.FactoryProvinceIdList.Any()) || x.FactoryProvinceIdList.Contains(orderTransactionDto.FactoryProvinceId.Value)).ToList();
            }

            if (ruleConfig.Select(x => x.FactoryCityIdList) != null && ruleConfig.Where(x => x.FactoryCityIdList.Any()).Any())
            {
                if (orderTransactionDto.FactoryCityId != null)
                    ruleConfig = ruleConfig.Where(x => (!x.FactoryCityIdList.Any()) || x.FactoryCityIdList.Contains(orderTransactionDto.FactoryCityId.Value)).ToList();
            }

            //booking BrandList contains in price card record
            if (ruleConfig.Select(x => x.BrandIdList) != null && ruleConfig.Where(x => x.BrandIdList.Any()).Any())
            {
                if (orderTransactionDto.BrandIds != null && orderTransactionDto.BrandIds.Any())
                    ruleConfig = ruleConfig.Where(x => (!x.BrandIdList.Any()) || orderTransactionDto.BrandIds.Any(x.BrandIdList.Contains)).ToList();
            }

            //booking BuyerList contains in price card record
            if (ruleConfig.Select(x => x.BuyerIdList) != null && ruleConfig.Where(x => x.BuyerIdList.Any()).Any())
            {
                if (orderTransactionDto.BuyerIds != null && orderTransactionDto.BuyerIds.Any())
                    ruleConfig = ruleConfig.Where(x => (!x.BuyerIdList.Any()) || orderTransactionDto.BuyerIds.Any(x.BuyerIdList.Contains)).ToList();
            }

            //booking DepartmentList contains in price card record
            if (ruleConfig.Select(x => x.DepartmentIdList) != null && ruleConfig.Where(x => x.DepartmentIdList.Any()).Any())
            {
                if (orderTransactionDto.DepartmentIds != null && orderTransactionDto.DepartmentIds.Any())
                    ruleConfig = ruleConfig.Where(x => (!x.DepartmentIdList.Any()) || orderTransactionDto.DepartmentIds.Any(x.DepartmentIdList.Contains)).ToList();
            }
            // apply price category filter
            if (ruleConfig.Select(x => x.PriceCategoryIdList) != null && ruleConfig.Where(x => x.PriceCategoryIdList.Any()).Any())
            {
                ruleConfig = ruleConfig.Where(x => (!x.PriceCategoryIdList.Any()) || x.PriceCategoryIdList.Contains(orderTransactionDto.PriceCategoryId)).ToList();
            }

            // apply product category filter
            if (ruleConfig.Select(x => x.ProductCategoryIdList) != null && ruleConfig.Any(x => x.ProductCategoryIdList.Any()))
            {
                if (orderTransactionDto.ProductCategoryIds != null && orderTransactionDto.ProductCategoryIds.Any())
                    ruleConfig = ruleConfig.Where(x => (!x.ProductCategoryIdList.Any()) || orderTransactionDto.ProductCategoryIds.Any(x.ProductCategoryIdList.Contains)).ToList();
            }

            // apply product sub category filter
            if (ruleConfig.Select(x => x.ProductSubCategoryIdList) != null && ruleConfig.Any(x => x.ProductSubCategoryIdList.Any()))
            {
                if (orderTransactionDto.ProductSubCategoryIds != null && orderTransactionDto.ProductSubCategoryIds.Any())
                    ruleConfig = ruleConfig.Where(x => (!x.ProductSubCategoryIdList.Any()) || orderTransactionDto.ProductSubCategoryIds.Any(x.ProductSubCategoryIdList.Contains)).ToList();
            }

            return ruleConfig;
        }

        public async Task<CustomerPriceCardRepo> GetRuleConfigData(InvoiceBookingDetail orderTransactionDto, IEnumerable<CustomerPriceCardRepo> ruleConfigs)
        {

            if (ruleConfigs.Any())
            {

                if (ruleConfigs.FirstOrDefault().InvoiceRequestType == null ||
                    ruleConfigs.FirstOrDefault().InvoiceRequestType == (int)InvoiceRequestType.NotApplicable)
                {
                    return ruleConfigs.FirstOrDefault();
                }

                // Rule with invoiceConfigured
                var invoiceRules = ruleConfigs.Where(r => r.IsInvoiceConfigured == true).ToList();

                if (invoiceRules.Count > 0)
                {
                    switch (ruleConfigs.FirstOrDefault().InvoiceRequestType)
                    {
                        case (int)InvoiceRequestType.Department:
                            ruleConfigs.FirstOrDefault().InvDepartmentIdList = orderTransactionDto.DepartmentIds;
                            break;
                        case (int)InvoiceRequestType.Brand:
                            ruleConfigs.FirstOrDefault().InvBrandIdList = orderTransactionDto.BrandIds;
                            break;
                        case (int)InvoiceRequestType.Buyer:
                            ruleConfigs.FirstOrDefault().InvBuyerIdList = orderTransactionDto.BuyerIds;
                            break;
                    }

                    return ruleConfigs.FirstOrDefault();
                }

            }

            return null;
        }

        /// <summary>
        /// get invoice booking details 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvoiceBookingDetail>> GetInspectioDatabyInvoiceRequest(InvoiceGenerateRequest requestDto, List<int> invoicedBookings)
        {

            List<InvoiceBookingDetail> exceutedBookingData = null;

            //get all bookings (Iqueryable)
            var bookingdata = _invoiceRepository.GetAllInvoiceBookingData();

            if (requestDto.InvoiceType.GetValueOrDefault() == (int)INVInvoiceType.Monthly)
            {
                //filter based on status - only validated and inspected bookings
                var inspectedStatusList = InspectedStatusList.Select(x => x);
                bookingdata = bookingdata.Where(x => inspectedStatusList.Contains(x.StatusId));
            }
            else if (requestDto.InvoiceType.GetValueOrDefault() == (int)INVInvoiceType.PreInvoice) // pre invoice will allow all the booking status except cancel and hold.
            {
                //filter booking status other than cancel and hold
                var invalidStatusList = InValidBookingStatusList.Select(x => x);
                bookingdata = bookingdata.Where(x => !invalidStatusList.Contains(x.StatusId));
            }

            // filter existing invoiced bookings
            if (invoicedBookings.Any())
            {
                bookingdata = bookingdata.Where(x => !invoicedBookings.Contains(x.BookingId));
            }

            //filter based on booking no
            if (requestDto.BookingNoList != null && requestDto.BookingNoList.Any())
            {
                bookingdata = bookingdata.Where(x => requestDto.BookingNoList.Contains(x.BookingId));
            }

            if (!requestDto.IsNewBookingInvoice)
            {
                //filter based on service date from and to 
                if (requestDto.RealInspectionFromDate != null && requestDto.RealInspectionToDate != null)
                {
                    bookingdata = bookingdata.Where(x => (x.ServiceTo >= requestDto.RealInspectionFromDate.ToDateTime().Date) &&
                    (x.ServiceTo <= requestDto.RealInspectionToDate.ToDateTime().Date));
                }



                //filter based on customer Id
                if (requestDto.CustomerId > 0)
                {
                    bookingdata = bookingdata.Where(x => x.CustomerId == requestDto.CustomerId);
                }

                //filter based on supplier list
                if (requestDto.SupplierInfo != null && requestDto.SupplierInfo.SupplierId > 0)
                {
                    if (requestDto.InvoiceTo == (int)InvoiceTo.Supplier)
                        bookingdata = bookingdata.Where(x => x.SupplierId == requestDto.SupplierInfo.SupplierId);

                    else if (requestDto.InvoiceTo == (int)InvoiceTo.Factory)
                        bookingdata = bookingdata.Where(x => x.FactoryId == requestDto.SupplierInfo.SupplierId);
                }

                exceutedBookingData = await bookingdata.ToListAsync();

                var bookingIdList = exceutedBookingData.Select(x => x.BookingId).ToList();

                // invoice too available then apply this filter
                if (requestDto.InvoiceTo > 0)
                {
                    var quotationHavingBookingList = new List<InvoiceBookingDetail>();
                    var quotationNotHavingBookingList = new List<InvoiceBookingDetail>();

                    var billedToBookingList = await _invoiceRepository.GetQuotationDataByBookingNoList(bookingIdList);

                    var quotationNotHavingBookingIds = bookingIdList.Where(x => !billedToBookingList.Select(x => x.BookingId).Contains(x)).ToList();

                    var filterbyBilledTo = billedToBookingList.Where(x => requestDto.InvoiceTo == x.BillingTo).Select(x => x.BookingId).ToList();

                    if (filterbyBilledTo.Any() && !requestDto.IsFromQuotation)
                    {
                        quotationHavingBookingList = exceutedBookingData.Where(x => filterbyBilledTo.Contains(x.BookingId)).ToList();
                    }
                    else
                    {
                        quotationHavingBookingList = exceutedBookingData.Where(x => !quotationNotHavingBookingIds.Contains(x.BookingId)).ToList();
                    }

                    if (quotationNotHavingBookingIds.Any())
                    {
                        quotationNotHavingBookingList = exceutedBookingData.Where(x => quotationNotHavingBookingIds.Contains(x.BookingId)).ToList();
                    }
                    // merge quotation and non quotation bookings 
                    exceutedBookingData = quotationHavingBookingList.Concat(quotationNotHavingBookingList).ToList();
                }


                //get the factory id list
                var factoryIdList = exceutedBookingData.Where(x => x.FactoryId > 0).Select(x => x.FactoryId.GetValueOrDefault()).ToList();

                //get the booking factory list
                var bookingFactoryDetails = await _invoiceRepository.GetBookingFactoryDetails(factoryIdList);

                //get the executed booking data
                exceutedBookingData = _invoicemap.MapFactoryDataToInvoiceBooking(exceutedBookingData, bookingFactoryDetails).ToList();


                // set booking data list
                var serviceTypeIdList = await _inspRepository.GetServiceType(bookingIdList);
                var brandIdList = await _inspRepository.GetBrandBookingIdsByBookingIds(bookingIdList);
                var buyerIdList = await _inspRepository.GetBuyerBookingIdsByBookingIds(bookingIdList);
                var departmentIdList = await _inspRepository.GetDeptBookingIdsByBookingIds(bookingIdList);
                var customerContactIdList = await _inspRepository.GetBookingCustomerContacts(bookingIdList);
                var productCategoryList = await _inspRepository.GetBookingPriceProductCategory(bookingIdList);
                var productSubCategoryList = await _inspRepository.GetBookingPriceProductSubCategory(bookingIdList);

                if (exceutedBookingData.Any())
                {
                    foreach (var booking in exceutedBookingData)
                    {
                        booking.ServiceTypeIds = serviceTypeIdList.Where(x => x.InspectionId == booking.BookingId).Select(x => x.serviceTypeId).ToList();
                        booking.BrandIds = brandIdList.Where(x => x.BookingId == booking.BookingId).Select(x => x.BrandId).ToList();
                        booking.BuyerIds = buyerIdList.Where(x => x.BookingId == booking.BookingId).Select(x => x.BuyerId).ToList();
                        booking.DepartmentIds = departmentIdList.Where(x => x.BookingId == booking.BookingId).Select(x => x.DeptId).ToList();
                        booking.CustomerContactIds = customerContactIdList.Where(x => x.BookingId == booking.BookingId).Select(x => x.CustomerContactId).ToList();
                        booking.ProductCategoryIds = productCategoryList.Where(x => x.BookingId == booking.BookingId && x.ProductCategoryId.HasValue).Select(x => x.ProductCategoryId.Value).ToList();
                        booking.ProductSubCategoryIds = productSubCategoryList.Where(x => x.BookingId == booking.BookingId && x.ProductSubCategoryId.HasValue).Select(x => x.ProductSubCategoryId.Value).ToList();
                    }
                }

                //filter based on supplier list
                if (requestDto.SupplierList != null && requestDto.SupplierList.Any())
                {
                    exceutedBookingData = exceutedBookingData.Where(x => requestDto.SupplierList.Contains(x.SupplierId)).ToList();
                }

                // filter based on service type list
                if (requestDto.ServiceTypes != null && requestDto.ServiceTypes.Any())
                {
                    exceutedBookingData = exceutedBookingData.Where(x => requestDto.ServiceTypes.Any(y => x.ServiceTypeIds.Any(z => z == y))).ToList();
                }

                // filter based on customer contacts
                if (requestDto.CustomerContacts != null && requestDto.CustomerContacts.Any())
                {
                    exceutedBookingData = exceutedBookingData.Where(x => requestDto.CustomerContacts.Any(y => x.CustomerContactIds.Any(z => z == y))).ToList();
                }

                //filter based on brand
                if (requestDto.BrandIdList != null && requestDto.BrandIdList.Any())
                {
                    exceutedBookingData = exceutedBookingData.Where(x => requestDto.BrandIdList.Any(y => x.BrandIds.Any(z => z == y))).ToList();
                }

                //filter based on department
                if (requestDto.DepartmentIdList != null && requestDto.DepartmentIdList.Any())
                {
                    exceutedBookingData = exceutedBookingData.Where(x => requestDto.DepartmentIdList.Any(y => x.DepartmentIds.Any(z => z == y))).ToList();
                }

                //filter based on buyer
                if (requestDto.BuyerIdList != null && requestDto.BuyerIdList.Any())
                {
                    exceutedBookingData = exceutedBookingData.Where(x => requestDto.BuyerIdList.Any(y => x.BuyerIds.Any(z => z == y))).ToList();
                }

                //filter based on product category list
                if (requestDto.ProductCategoryIdList != null && requestDto.ProductCategoryIdList.Any())
                {
                    exceutedBookingData = exceutedBookingData.Where(x => requestDto.ProductCategoryIdList.Any(y => x.ProductCategoryIds.Any(z => z == y))).ToList();
                }

                //filter based on product sub category list
                if (requestDto.ProductSubCategoryIdList != null && requestDto.ProductSubCategoryIdList.Any())
                {
                    exceutedBookingData = exceutedBookingData.Where(x => requestDto.ProductSubCategoryIdList.Any(y => x.ProductSubCategoryIds.Any(z => z == y))).ToList();
                }

                //filter based on factory country
                if (requestDto.FactoryCountryList != null && requestDto.FactoryCountryList.Any())
                {
                    exceutedBookingData = exceutedBookingData.Where(x => requestDto.FactoryCountryList.Contains(x.FactoryCountryId)).ToList();
                }
            }
            else
            {
                exceutedBookingData = await bookingdata.ToListAsync();

                //get the factory id list
                var factoryIdList = exceutedBookingData.Where(x => x.FactoryId > 0).Select(x => x.FactoryId.GetValueOrDefault()).ToList();

                //get the booking factory list
                var bookingFactoryDetails = await _invoiceRepository.GetBookingFactoryDetails(factoryIdList);

                //get the executed booking data
                exceutedBookingData = _invoicemap.MapFactoryDataToInvoiceBooking(exceutedBookingData, bookingFactoryDetails).ToList();

                if (exceutedBookingData.Any())
                {
                    var bookingIds = exceutedBookingData.Select(x => x.BookingId).ToList();
                    // set booking data list
                    var serviceTypeIdList = await _inspRepository.GetServiceType(bookingIds);
                    var brandIdList = await _inspRepository.GetBrandBookingIdsByBookingIds(bookingIds);
                    var buyerIdList = await _inspRepository.GetBuyerBookingIdsByBookingIds(bookingIds);
                    var departmentIdList = await _inspRepository.GetDeptBookingIdsByBookingIds(bookingIds);
                    var customerContactIdList = await _inspRepository.GetBookingCustomerContacts(bookingIds);
                    var productCategoryList = await _inspRepository.GetBookingPriceProductCategory(bookingIds);
                    var productSubCategoryList = await _inspRepository.GetBookingPriceProductSubCategory(bookingIds);

                    foreach (var booking in exceutedBookingData)
                    {
                        booking.ServiceTypeIds = serviceTypeIdList.Where(x => x.InspectionId == booking.BookingId).Select(x => x.serviceTypeId).ToList();
                        booking.BrandIds = brandIdList.Where(x => x.BookingId == booking.BookingId).Select(x => x.BrandId).ToList();
                        booking.BuyerIds = buyerIdList.Where(x => x.BookingId == booking.BookingId).Select(x => x.BuyerId).ToList();
                        booking.DepartmentIds = departmentIdList.Where(x => x.BookingId == booking.BookingId).Select(x => x.DeptId).ToList();
                        booking.CustomerContactIds = customerContactIdList.Where(x => x.BookingId == booking.BookingId).Select(x => x.CustomerContactId).ToList();
                        booking.ProductCategoryIds = productCategoryList.Where(x => x.BookingId == booking.BookingId && x.ProductCategoryId.HasValue).Select(x => x.ProductCategoryId.Value).ToList();
                        booking.ProductSubCategoryIds = productSubCategoryList.Where(x => x.BookingId == booking.BookingId && x.ProductSubCategoryId.HasValue).Select(x => x.ProductSubCategoryId.Value).ToList();
                    }
                }
            }

            return exceutedBookingData;
        }

        /// <summary>
        /// get invoice audit 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="invoicedAudits"></param>
        /// <returns></returns>

        private async Task<IEnumerable<InvoiceBookingDetail>> GetAuditDatabyInvoiceRequest(InvoiceGenerateRequest requestDto, List<int> invoicedAudits)
        {

            List<InvoiceBookingDetail> exceutedAuditData = null;

            // get all bookings (Iqueryable)
            var auditData = _invoiceRepository.GetAllInvoiceAuditData();

            if ((requestDto.InvoiceType.GetValueOrDefault() == (int)INVInvoiceType.Monthly)) // monthly invoice
            {
                var auditedStatusList = AuditedStatusList.Select(x => x);
                auditData = auditData.Where(x => auditedStatusList.Contains(x.StatusId));
            }
            else // pre invoice 
            {
                var preinvoiceStatusList = PreInvoiceAuditStatusList.Select(x => x);
                auditData = auditData.Where(x => preinvoiceStatusList.Contains(x.StatusId));
            }

            // filter existing invoiced audits
            if (invoicedAudits.Any())
            {
                auditData = auditData.Where(x => !invoicedAudits.Contains(x.AuditId));
            }

            //filter based on audit no
            if (requestDto.BookingNoList != null && requestDto.BookingNoList.Any())
            {
                auditData = auditData.Where(x => requestDto.BookingNoList.Contains(x.AuditId));
            }

            if (!requestDto.IsNewBookingInvoice)
            {
                //filter based on service date from and to 
                if (requestDto.RealInspectionFromDate != null && requestDto.RealInspectionToDate != null)
                {
                    auditData = auditData.Where(x => (x.ServiceTo >= requestDto.RealInspectionFromDate.ToDateTime().Date) &&
                    (x.ServiceTo <= requestDto.RealInspectionToDate.ToDateTime().Date));
                }

                //filter based on customer Id
                if (requestDto.CustomerId > 0)
                {
                    auditData = auditData.Where(x => x.CustomerId == requestDto.CustomerId);
                }

                //filter based on supplier list
                if (requestDto.SupplierInfo != null && requestDto.SupplierInfo.SupplierId > 0)
                {
                    if (requestDto.InvoiceTo == (int)InvoiceTo.Supplier)
                        auditData = auditData.Where(x => x.SupplierId == requestDto.SupplierInfo.SupplierId);

                    else if (requestDto.InvoiceTo == (int)InvoiceTo.Factory)
                        auditData = auditData.Where(x => x.FactoryId == requestDto.SupplierInfo.SupplierId);
                }

                exceutedAuditData = await auditData.ToListAsync();

                var auditIdList = exceutedAuditData.Select(x => x.AuditId).ToList();

                var auditContacts = await _auditRepository.GetAuditContacts(auditIdList);

                var auditServiceTypes = await _auditRepository.GetAuditServiceTypeList(auditIdList);

                foreach (var audit in exceutedAuditData)
                {
                    audit.CustomerContactIds = auditContacts.Where(x => x.AuditId == audit.AuditId).Select(x => x.ContactId).ToList();
                    audit.ServiceTypeIds = auditServiceTypes.Where(x => x.AuditId == audit.AuditId).Select(x => x.ServiceTypeId).ToList();
                }

                // check billed to with quotation audit

                var billedToAuditList = await _invoiceRepository.GetQuotationDataByAuditNoList(auditIdList);

                if (billedToAuditList.Any())
                {
                    var filterbyBilledTo = billedToAuditList.Where(x => requestDto.InvoiceTo == x.BillingTo).Select(x => x.AuditId).ToList();

                    exceutedAuditData = exceutedAuditData.Where(x => filterbyBilledTo.Contains(x.AuditId)).ToList();
                }

                //filter based on supplier list
                if (requestDto.SupplierList != null && requestDto.SupplierList.Any())
                {
                    exceutedAuditData = exceutedAuditData.Where(x => requestDto.SupplierList.Contains(x.SupplierId)).ToList();
                }

                // filter based on service type list
                if (requestDto.ServiceTypes != null && requestDto.ServiceTypes.Any())
                {
                    exceutedAuditData = exceutedAuditData.Where(x => requestDto.ServiceTypes.Any(y => x.ServiceTypeIds.Any(z => z == y))).ToList();
                }

                // filter based on customer contacts
                if (requestDto.CustomerContacts != null && requestDto.CustomerContacts.Any())
                {
                    exceutedAuditData = exceutedAuditData.Where(x => requestDto.CustomerContacts.Any(y => x.CustomerContactIds.Any(z => z == y))).ToList();
                }

                //filter based on brand
                if (requestDto.BrandIdList != null && requestDto.BrandIdList.Any())
                {
                    exceutedAuditData = exceutedAuditData.Where(x => requestDto.BrandIdList.Contains(x.AuditBrandId.GetValueOrDefault())).ToList();
                }

                //filter based on department
                if (requestDto.DepartmentIdList != null && requestDto.DepartmentIdList.Any())
                {
                    exceutedAuditData = exceutedAuditData.Where(x => requestDto.DepartmentIdList.Contains(x.AuditDepartmentId.GetValueOrDefault())).ToList();
                }

                //filter based on factory country
                if (requestDto.FactoryCountryList != null && requestDto.FactoryCountryList.Any())
                {
                    exceutedAuditData = exceutedAuditData.Where(x => requestDto.FactoryCountryList.Contains(x.FactoryCountryId)).ToList();
                }
            }
            else
            {
                exceutedAuditData = await auditData.ToListAsync();

                var auditIdList = exceutedAuditData.Select(x => x.AuditId).ToList();

                var auditContacts = await _auditRepository.GetAuditContacts(auditIdList);

                var auditServiceTypes = await _auditRepository.GetAuditServiceTypeList(auditIdList);

                foreach (var audit in exceutedAuditData)
                {
                    audit.CustomerContactIds = auditContacts.Where(x => x.AuditId == audit.AuditId).Select(x => x.ContactId).ToList();
                    audit.ServiceTypeIds = auditServiceTypes.Where(x => x.AuditId == audit.AuditId).Select(x => x.ServiceTypeId).ToList();
                }
            }

            return exceutedAuditData;
        }


        /// <summary>
        /// Get the Invoice Base Details
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        public async Task<InvoiceBaseDetailResponse> GetInvoiceBaseDetails(string invoiceNo, int serviceId)
        {
            //Get the invoice base detail repo 
            InvoiceBaseDetailRepo invoiceBookingDetailRepo = null;

            if (serviceId == (int)Service.InspectionId)
            {
                invoiceBookingDetailRepo = await _invoiceRepository.GetInvoiceBaseDetails(invoiceNo);

                if (invoiceBookingDetailRepo != null)
                {
                    invoiceBookingDetailRepo.ContactList = await _invoiceRepository.GetInvoiceContactDetails(invoiceBookingDetailRepo.InvoiceId);
                }
            }
            else
            {
                invoiceBookingDetailRepo = await _invoiceRepository.GetAuditInvoiceBaseDetails(invoiceNo);
                if (invoiceBookingDetailRepo != null)
                {
                    invoiceBookingDetailRepo.ContactList = await _invoiceRepository.GetInvoiceContactDetails(invoiceBookingDetailRepo.InvoiceId);
                }
            }

            //if data not found set not found response
            if (invoiceBookingDetailRepo == null)
                return new InvoiceBaseDetailResponse { Result = InvoiceBaseDetailResult.NotFound };

            //if data is there map to invoiceBookingDetail and send response
            var invoiceBaseDetail = _invoicemap.MapInvoiceBaseDetails(invoiceBookingDetailRepo);
            return new InvoiceBaseDetailResponse { Result = InvoiceBaseDetailResult.Success, InvoiceBaseDetail = invoiceBaseDetail };

        }

        /// <summary>
        /// Get Invoice BilledAddress Master Data based on the billToId
        /// </summary>
        /// <param name="billToId"></param>
        /// <param name="searchId"></param>
        /// <returns></returns>
        public async Task<InvoiceBilledAddressResponse> GetInvoiceBilledAddress(int billToId, int searchId)
        {
            InvoiceBilledAddressResponse response = new InvoiceBilledAddressResponse();
            //fetch the billingaddress data based on billToId
            switch (billToId)
            {
                case (int)InvoiceTo.Customer:
                    //get the customer address by customer id
                    var customerAddress = await _customerManager.GetCustomerAddressByListCusId(new List<int>() { searchId });
                    if (customerAddress != null && customerAddress.Count() > 0)
                    {
                        //Map the customer address to common datasource list
                        response.billedAddress = _invoicemap.MapCustomerBilledAddress(customerAddress);
                        response.Result = InvoiceBilledAddressResult.Success;
                    }
                    break;
                case (int)InvoiceTo.Supplier:
                    //get the supplier address by supplier id
                    var supplierAddress = await _supplierManager.GetSupplierOfficeAddressBylstId(new List<int>() { searchId });
                    if (supplierAddress != null && supplierAddress.Count() > 0)
                    {
                        //Map the supplier address to common datasource list
                        response.billedAddress = _invoicemap.MapSupplierFactoryBilledAddress(supplierAddress);
                        response.Result = InvoiceBilledAddressResult.Success;
                    }
                    break;
                case (int)InvoiceTo.Factory:
                    //get the factory address by factory id
                    var factoryAddress = await _supplierManager.GetSupplierOfficeAddressBylstId(new List<int>() { searchId });
                    if (factoryAddress != null && factoryAddress.Count() > 0)
                    {
                        //Map the factory address to common datasource list
                        response.billedAddress = _invoicemap.MapSupplierFactoryBilledAddress(factoryAddress);
                        response.Result = InvoiceBilledAddressResult.Success;
                    }
                    break;
            }

            //if DataSourceList is null then send addressnotfound result
            if (response.billedAddress == null || response.billedAddress.Count() == 0)
                response.Result = InvoiceBilledAddressResult.AddressNotFound;

            return response;
        }

        /// <summary>
        /// Get Invoice Contacts based on the billToId
        /// </summary>
        /// <param name="billToId"></param>
        /// <param name="searchId"></param>
        /// <returns></returns>
        public async Task<InvoiceContactsResponse> GetInvoiceContacts(int billToId, int searchId)
        {
            InvoiceContactsResponse response = new InvoiceContactsResponse();
            //fetch the billingaddress data based on billToId
            switch (billToId)
            {
                case (int)InvoiceTo.Customer:
                    //get the customer contacts by customer id
                    var customerContacts = await _customerManager.GetCustomerContactsbyCustomer(searchId);
                    if (customerContacts != null && customerContacts.DataSourceList != null && customerContacts.DataSourceList.Count() > 0)
                    {
                        //Map the customer contacts to common datasource list
                        response.Contacts = customerContacts.DataSourceList;
                        response.Result = InvoiceBilledContactsResult.Success;
                    }
                    break;
                case (int)InvoiceTo.Supplier:
                    //get the supplier contacts by supplier id
                    var supplierContacts = await _supplierManager.GetSupplierContactsbySupplier(searchId);
                    if (supplierContacts != null && supplierContacts.DataSourceList != null && supplierContacts.DataSourceList.Count() > 0)
                    {
                        //Map the supplier contacts to common datasource list
                        response.Contacts = supplierContacts.DataSourceList;
                        response.Result = InvoiceBilledContactsResult.Success;
                    }
                    break;
                case (int)InvoiceTo.Factory:
                    //get the supplier contacts by supplier id
                    var factoryContacts = await _supplierManager.GetSupplierContactsbySupplier(searchId);
                    if (factoryContacts != null && factoryContacts.DataSourceList != null && factoryContacts.DataSourceList.Count() > 0)
                    {
                        //Map the supplier contacts to common datasource list
                        response.Contacts = factoryContacts.DataSourceList;
                        response.Result = InvoiceBilledContactsResult.Success;
                    }
                    break;
            }

            //if DataSourceList is null then send addressnotfound result
            if (response.Contacts == null || response.Contacts.Count() == 0)
                response.Result = InvoiceBilledContactsResult.ContactsNotFound;

            return response;
        }

        /// <summary>
        /// Get the Invoice Payment Status List
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInvoicePaymentStatus()
        {
            var paymentStatus = await _invoiceRepository.GetInvoicePaymentStatus();
            if (paymentStatus == null || paymentStatus.Count() == 0)
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse() { DataSourceList = paymentStatus, Result = DataSourceResult.Success };
        }

        public async Task<DataSourceResponse> GetInvoicePaymentOffice()
        {
            var paymentStatus = await _invoiceRepository.GetInvoicePaymentStatus();
            if (paymentStatus == null || paymentStatus.Count() == 0)
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse() { DataSourceList = paymentStatus, Result = DataSourceResult.Success };
        }

        public async Task<InvoiceTransactionDetailsResponse> GetInvoiceTransactionDetails(string invoiceNo, int serviceId)
        {
            InvoiceTransactionDetailsResponse response = new InvoiceTransactionDetailsResponse();

            List<InvoiceTransactionDetailRepo> invoiceTransactionDetails = null;

            if (serviceId == (int)Service.InspectionId)
            {
                invoiceTransactionDetails = await _invoiceRepository.GetInvoiceTransactionDetails(invoiceNo);
            }
            else if (serviceId == (int)Service.AuditId)
            {
                invoiceTransactionDetails = await _invoiceRepository.GetAuditInvoiceTransactionDetails(invoiceNo);
            }

            if (invoiceTransactionDetails != null && invoiceTransactionDetails.Any())
            {
                if (serviceId == (int)Service.InspectionId)
                {
                    var bookingIds = invoiceTransactionDetails.Select(x => x.BookingNo.Value);
                    var bookingQuantityDetails = await _invoiceRepository.GetBookingQuantityDetails(bookingIds);
                    var bookingQuotationDetails = await _invoiceRepository.GetBookingQuotationDetails(bookingIds);
                    var bookingServiceTypes = await _invoiceRepository.GetBookingServiceTypes(bookingIds);
                    var factoryIds = invoiceTransactionDetails.Where(x => x.FactoryId > 0).Select(x => x.FactoryId.GetValueOrDefault());
                    var factoryGeoLocations = await _supplierManager.GetSupplierGeoLocation(factoryIds);

                    var productList = await _inspRepository.GetScheduleProductListByBooking(bookingIds);
                    var ContainerList = await _inspRepository.GetContainerListByBooking(bookingIds);


                    var distinctInvoiceIds = invoiceTransactionDetails.Select(x => x.Id).Distinct().ToList();
                    var extraFeesList = await _invoiceRepository.GetInvoiceExtraFeeItem(distinctInvoiceIds);

                    var transactionDetails = _invoicemap.MapInvoiceTransactionDetails(invoiceTransactionDetails, bookingQuantityDetails,
                                                        bookingQuotationDetails, factoryGeoLocations, bookingServiceTypes, extraFeesList.ToList());

                    // set report count details 
                    foreach (var item in transactionDetails)
                    {
                        if (bookingServiceTypes.Where(x => x.BookingNo == item.BookingNo).Select(x => x.ServiceTypeId).FirstOrDefault() == (int)InspectionServiceTypeEnum.Container)
                        {
                            item.ReportCount = ContainerList.Where(x => x.ContainerId > 0 && x.BookingId == item.BookingNo)
                                                 .Select(x => x.ContainerId).Distinct().Count();
                        }
                        else
                        {
                            item.ReportCount = productList.Count(x => x.CombineProductId == 0 && x.BookingId == item.BookingNo) +
                                productList.Where(x => x.CombineProductId != 0 && x.BookingId == item.BookingNo).Select(x => x.CombineProductId).Distinct().Count();
                        }
                    }

                    if (transactionDetails == null || transactionDetails.Count == 0)
                        response.Result = InvoiceTransactionDetailsResult.DataNotFound;

                    else
                    {
                        response.transactionDetails = transactionDetails;
                        response.Result = InvoiceTransactionDetailsResult.Success;
                    }
                }

                else
                {
                    var bookingIds = invoiceTransactionDetails.Select(x => x.AuditNo.Value);
                    var bookingQuantityDetails = new List<InvoiceBookingQuantityDetails>();
                    var bookingQuotationDetails = await _invoiceRepository.GetAuditBookingQuotationDetails(bookingIds);
                    var bookingServiceTypes = await _invoiceRepository.GetAuditBookingServiceTypes(bookingIds);
                    var factoryIds = invoiceTransactionDetails.Where(x => x.FactoryId > 0).Select(x => x.FactoryId.GetValueOrDefault());
                    var factoryGeoLocations = await _supplierManager.GetSupplierGeoLocation(factoryIds);

                    var distinctInvoiceIds = invoiceTransactionDetails.Select(x => x.Id).Distinct().ToList();
                    var extraFeesList = await _invoiceRepository.GetInvoiceExtraFeeItem(distinctInvoiceIds);

                    var transactionDetails = _invoicemap.MapAuditInvoiceTransactionDetails(invoiceTransactionDetails, bookingQuantityDetails,
                                                        bookingQuotationDetails, factoryGeoLocations, bookingServiceTypes, extraFeesList.ToList());

                    if (transactionDetails == null || transactionDetails.Count == 0)
                        response.Result = InvoiceTransactionDetailsResult.DataNotFound;

                    else
                    {
                        response.transactionDetails = transactionDetails;
                        response.Result = InvoiceTransactionDetailsResult.Success;
                    }
                }

            }

            return response;

        }

        public async Task<InvoiceBookingMoreInfoResponse> GetInvoiceBookingMoreInfo(int bookingId)
        {
            var bookingInfo = await _invoiceRepository.GetInvoiceBookingMoreInfo(bookingId);
            if (bookingInfo == null)
            {
                return new InvoiceBookingMoreInfoResponse { Result = InvoiceBookingMoreInfoResult.NotFound };
            }

            //Get Department details
            var departmentData = await _inspRepository.GetBookingDepartmentList(new[] { bookingId });
            //Get Brand details
            var brandData = await _inspRepository.GetBookingBrandList(new[] { bookingId });

            //Get Brand details
            var QcList = await _scheduleRepository.GetQCNameList(bookingId);

            bookingInfo.Departments = string.Join(",", departmentData.Select(x => x.Name).ToList());
            bookingInfo.Brands = string.Join(",", brandData.Select(x => x.Name).ToList());
            bookingInfo.QCNames = string.Join(",", QcList.Select(x => x).ToList());

            return new InvoiceBookingMoreInfoResponse { Result = InvoiceBookingMoreInfoResult.Success, InvoiceBookingMoreInfo = bookingInfo };
        }

        /// <summary>
        /// Update the invoice transaction details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UpdateInvoiceDetailsResponse> UpdateInvoiceTransaction(UpdateInvoiceDetailRequest request)
        {
            //if invoice detail is not null
            if (request != null && request.invoiceBaseDetail != null && request.invoiceDetails != null && request.invoiceDetails.Any())
            {
                var invoiceIds = request.invoiceDetails.Select(x => x.Id);

                //get the invoice transaction list by invoice ids
                var invoiceTransactionList = await _invoiceRepository.GetInvoiceListById(invoiceIds);

                var entityId = _filterService.GetCompanyId();
                //loop through the invoice transaction details
                foreach (var invoice in request.invoiceDetails)
                {
                    //get the invoice transaction by id
                    var invoiceTransaction = invoiceTransactionList.FirstOrDefault(x => x.Id == invoice.Id);

                    //add the invoice transaction status log
                    if (request.invoiceBaseDetail.ServiceId == (int)Service.InspectionId)
                    {
                        var invoiceTransStatusLog = _invoicemap.MapInvoiceTransactionStatusLog(invoice.Id, invoice.BookingNo,
                                                                                       (int)InvoiceStatus.Modified, _ApplicationContext.UserId, entityId);
                        _invoiceRepository.AddEntity(invoiceTransStatusLog);

                        //map the invoice transaction details
                        _invoicemap.MapUpdateInvoiceDetails(invoiceTransaction, invoice, request.invoiceBaseDetail, _ApplicationContext.UserId);
                    }
                    else if (request.invoiceBaseDetail.ServiceId == (int)Service.AuditId)
                    {
                        var invoiceTransStatusLog = _invoicemap.MapAuditInvoiceTransactionStatusLog(invoice.Id, invoice.BookingNo,
                                                                                                     (int)InvoiceStatus.Modified, _ApplicationContext.UserId, entityId);
                        _invoiceRepository.AddEntity(invoiceTransStatusLog);

                        //map the invoice transaction details
                        _invoicemap.MapUpdateAuditInvoiceDetails(invoiceTransaction, invoice, request.invoiceBaseDetail, _ApplicationContext.UserId);
                    }


                    //Remove the existing contacts
                    _invoiceRepository.RemoveEntities(invoiceTransaction.InvAutTranContactDetails);

                    // add the invoice transaction contacts
                    switch (request.invoiceBaseDetail.BillTo)
                    {
                        case (int)InvoiceTo.Supplier:

                            foreach (var id in request.invoiceBaseDetail.ContactIds)
                            {
                                var contact = new InvAutTranContactDetail()
                                {
                                    SupplierContactId = id,
                                    InvoiceId = invoice.Id
                                };
                                _invoiceRepository.AddEntity(contact);
                                invoiceTransaction.InvAutTranContactDetails.Add(contact);
                            }
                            break;

                        case (int)InvoiceTo.Factory:
                            foreach (var id in request.invoiceBaseDetail.ContactIds)
                            {

                                var contact = new InvAutTranContactDetail()
                                {
                                    FactoryContactId = id,
                                    InvoiceId = invoice.Id
                                };
                                _invoiceRepository.AddEntity(contact);
                                invoiceTransaction.InvAutTranContactDetails.Add(contact);
                            }
                            break;

                        case (int)InvoiceTo.Customer:
                            foreach (var id in request.invoiceBaseDetail.ContactIds)
                            {
                                var contact = new InvAutTranContactDetail()
                                {
                                    CustomerContactId = id,
                                    InvoiceId = invoice.Id
                                };
                                _invoiceRepository.AddEntity(contact);
                                invoiceTransaction.InvAutTranContactDetails.Add(contact);
                            }
                            break;
                    }

                    _invoiceRepository.EditEntity(invoiceTransaction);

                    await _invoiceRepository.Save();


                }

                return new UpdateInvoiceDetailsResponse { Result = UpdateInvoiceDetailResult.Success };
            }

            return new UpdateInvoiceDetailsResponse { Result = UpdateInvoiceDetailResult.Failure };
        }

        /// <summary>
        /// Delete the invoice detail by id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public async Task<DeleteInvoiceDetailResponse> DeleteInvoiceDetail(int invoiceId)
        {
            var invoiceTransations = await _invoiceRepository.GetInvoiceListById(new[] { invoiceId });
            if (invoiceTransations != null)
            {
                var invoiceTransation = invoiceTransations.FirstOrDefault();
                invoiceTransation.InvoiceStatus = (int)InvoiceStatus.Cancelled;

                var entityId = _filterService.GetCompanyId();
                // when cancel the invoice and delete the invoice id mapping from extra fees and update the status
                foreach (var extrafee in invoiceTransation.InvExfTransactions.Where(x => x.StatusId != (int)ExtraFeeStatus.Cancelled))
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

                if (invoiceTransation.ServiceId == (int)Service.InspectionId)
                {
                    //add invoice status log
                    AddInvoiceStatusLog(invoiceTransation);
                }
                else if (invoiceTransation.ServiceId == (int)Service.AuditId)
                {
                    AddAuditInvoiceStatusLog(invoiceTransation);
                }

                //remove the invoice transaction contacts
                _invoiceRepository.RemoveEntities(invoiceTransations.SelectMany(x => x.InvAutTranContactDetails));

                await _invoiceRepository.Save();
                return new DeleteInvoiceDetailResponse { Result = DeleteInvoiceDetailResult.DeleteSuccess };
            }

            return new DeleteInvoiceDetailResponse { Result = DeleteInvoiceDetailResult.DeleteFailed };
        }

        /// <summary>
        /// Add the invoice transaction status log
        /// </summary>
        /// <param name="invoiceDetail"></param>
        private void AddInvoiceStatusLog(InvAutTranDetail invoiceDetail)
        {
            //add the invoice transaction status log
            var invoiceTransStatusLog = _invoicemap.MapInvoiceTransactionStatusLog(invoiceDetail.Id, invoiceDetail.InspectionId.GetValueOrDefault(),
                                                                (int)InvoiceStatus.Cancelled, _ApplicationContext.UserId, _filterService.GetCompanyId());
            _invoiceRepository.AddEntity(invoiceTransStatusLog);
        }

        private void AddAuditInvoiceStatusLog(InvAutTranDetail invoiceDetail)
        {
            //add the invoice transaction status log
            var invoiceTransStatusLog = _invoicemap.MapAuditInvoiceTransactionStatusLog(invoiceDetail.Id, invoiceDetail.AuditId.GetValueOrDefault(),
                                                                (int)InvoiceStatus.Cancelled, _ApplicationContext.UserId, _filterService.GetCompanyId());
            _invoiceRepository.AddEntity(invoiceTransStatusLog);
        }

        /// <summary>
        /// get invoice summary
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<InvoiceSummarySearchResult> GetInvoiceSummary(InvoiceSummaryRequest requestDto)
        {
            var response = new InvoiceSummarySearchResult();

            if (requestDto.Index == null || requestDto.Index.Value <= 0)
                requestDto.Index = 1;

            if (requestDto.pageSize == null || requestDto.pageSize.Value == 0)
                requestDto.pageSize = 10;

            int skip = (requestDto.Index.Value - 1) * requestDto.pageSize.Value;

            var invoiceData = _invoiceRepository.GetInvoiceDetailsByServiceType(requestDto.ServiceId);

            if (invoiceData != null)
            {
                //filter based on service date from and to / invoice date
                if (Enum.TryParse(requestDto.DateTypeId.ToString(), out SearchType _datesearchtype))
                {
                    if (requestDto.InvoiceFromDate?.ToDateTime() != null && requestDto.InvoiceToDate?.ToDateTime() != null)
                    {
                        switch (_datesearchtype)
                        {
                            case SearchType.InvoiceDate:
                                {
                                    invoiceData = invoiceData.Where(x => (x.InvoiceDate.HasValue && x.InvoiceDate.Value.Date >= requestDto.InvoiceFromDate.ToDateTime().Date) &&
                                                (x.InvoiceDate.HasValue && x.InvoiceDate.Value.Date <= requestDto.InvoiceToDate.ToDateTime().Date));
                                    break;
                                }
                            case SearchType.ServiceDate:
                                {
                                    invoiceData = invoiceData.Where(x => !((x.Inspection.ServiceDateFrom.Date > requestDto.InvoiceToDate.ToDateTime().Date) || (x.Inspection.ServiceDateTo.Date < requestDto.InvoiceFromDate.ToDateTime().Date)));
                                    break;
                                }
                        }
                    }
                }

                //filter by invoice To
                if (requestDto.InvoiceTo > 0)
                {
                    invoiceData = requestDto.InvoiceTo == (int)QuotationPaidBy.customer ? invoiceData.Where(x => x.InvoiceTo == (int)QuotationPaidBy.customer) : requestDto.InvoiceTo == (int)QuotationPaidBy.supplier ? invoiceData.Where(x => x.InvoiceTo == (int)QuotationPaidBy.supplier) : invoiceData.Where(x => x.InvoiceTo == (int)QuotationPaidBy.factory);
                }

                //filter by invoice Type
                if (requestDto.InvoiceType > 0)
                {
                    invoiceData = invoiceData.Where(x => x.InvoiceType == requestDto.InvoiceType);
                }

                //filter by customerId
                if (requestDto.CustomerId > 0)
                {
                    invoiceData = invoiceData.Where(x => x.Inspection.CustomerId == requestDto.CustomerId);
                }

                //filter by supplierId
                if (requestDto.SupplierId > 0)
                {
                    invoiceData = invoiceData.Where(x => x.Inspection.SupplierId == requestDto.SupplierId);
                }

                //filter by factoryId
                if (requestDto.FactoryId > 0)
                {
                    invoiceData = invoiceData.Where(x => x.Inspection.FactoryId == requestDto.FactoryId);
                }

                //filter by Booking Id
                if (requestDto.SearchTypeId == (int)SearchType.BookingNo)
                {
                    if (!string.IsNullOrEmpty(requestDto.SearchTypeText?.Trim()) && int.TryParse(requestDto.SearchTypeText?.Trim(), out int bookid))
                    {
                        invoiceData = invoiceData.Where(x => x.InspectionId == bookid);
                    }
                }

                //filter by invoice No
                if (requestDto.SearchTypeId == (int)SearchType.InvoiceNo)
                {
                    if (!string.IsNullOrEmpty(requestDto.SearchTypeText?.Trim()))
                    {
                        invoiceData = invoiceData.Where(x => x.InvoiceNo == requestDto.SearchTypeText.Trim());
                    }
                }

                if (requestDto.InvoiceStatusId != null && requestDto.InvoiceStatusId.Any())
                {
                    invoiceData = invoiceData.Where(x => requestDto.InvoiceStatusId.Contains(x.InvoiceStatus.GetValueOrDefault()));
                }

                //fetch the first 10 invoice Ids
                invoiceData = invoiceData.OrderByDescending(x => x.InvoiceNo);
                var distinctInvoiceNo = await invoiceData.Select(x => x.InvoiceNo).Distinct().ToListAsync();
                var invoiceNoList = distinctInvoiceNo.Skip(skip).Take(requestDto.pageSize.GetValueOrDefault()).ToList();

                if (invoiceNoList != null && invoiceNoList.Any())
                {

                    IEnumerable<InvoiceSummaryItem> invoiceResultData = null;
                    List<QuotationInspectionTravelCost> quotationmanday = null;
                    if (requestDto.ServiceId == (int)Service.InspectionId)
                    {
                        //fetch the booking and invoice details
                        invoiceResultData = await _invoiceRepository.GetInvoiceDetailsByInvoiceNo(invoiceNoList);
                        //Get the service Type for the bookings
                        var lstbookingid = invoiceResultData.Select(x => x.BookingId.GetValueOrDefault()).Distinct().ToList();
                        response.serviceTypeList = await _inspRepository.GetServiceType(lstbookingid);
                        quotationmanday = await _invoiceRepository.GetQuotationDataByBookingIdsList(lstbookingid);

                    }
                    else
                    {
                        invoiceResultData = await _invoiceRepository.GetAuditInvoiceDetailsByInvoiceNo(invoiceNoList);
                        //Get the service Type for the bookings
                        var lstbookingid = invoiceResultData.Select(x => x.AuditId.GetValueOrDefault()).Distinct().ToList();
                        response.serviceTypeList = await _inspRepository.GetAuditServiceType(lstbookingid);
                        quotationmanday = await _invoiceRepository.GetQuotationDataByAuditList(lstbookingid);
                    }

                    var factoryCountries = await _invoiceRepository.GetBookingFactoryDetails(invoiceResultData.Select(y => y.FactoryId.GetValueOrDefault()).ToList());
                    invoiceResultData = _invoicemap.MapinspQuotationDataToInvoice(invoiceResultData.ToList(), quotationmanday, factoryCountries);
                    var distinctInvoiceIds = invoiceResultData.Select(x => x.Id).Distinct().ToList();
                    var invoiceExtraFees = await _invoiceRepository.GetInvoiceExtraFeeItem(distinctInvoiceIds);

                    response.invoiceDataList = invoiceResultData.ToList();
                    response.invoiceExtraFeeList = invoiceExtraFees.ToList();
                    response.TotalCount = await invoiceData.Select(x => x.InvoiceNo).Distinct().CountAsync();

                    var invoiceList = await invoiceData.GroupBy(x => x.InvoiceNo).Select(x => x.FirstOrDefault())
                        .Select(y => new InvoiceSummaryItem()
                        {
                            StatusId = y.InvoiceStatus.GetValueOrDefault(),
                            StatusName = y.InvoiceStatusNavigation.Name,
                            InvoiceNo = y.InvoiceNo
                        }).ToListAsync();

                    response.StatusCountList = invoiceList.Select(x => new { x.StatusId, x.StatusName, x.InvoiceNo })
                           .GroupBy(p => new { p.StatusId, p.StatusName }, p => p, (key, _data) =>
                          new InspectionStatus
                          {
                              Id = key.StatusId,
                              StatusName = key.StatusName,
                              TotalCount = _data.Count(),
                              StatusColor = InvoiceStatusColor.GetValueOrDefault(key.StatusId, "")
                          }).ToList();
                }
            }
            return response;
        }

        /// <summary>
        /// get invoice summary
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<InvoiceSummarySearchResult> GetAllInvoiceSummary(InvoiceSummaryRequest requestDto)
        {
            var response = new InvoiceSummarySearchResult();


            if (requestDto.Index == null || requestDto.Index.Value <= 0)
                requestDto.Index = 1;

            if (requestDto.pageSize == null || requestDto.pageSize.Value == 0)
                requestDto.pageSize = 10;

            int skip = (requestDto.Index.Value - 1) * requestDto.pageSize.Value;

            var invoiceData = _invoiceRepository.GetInvoiceDetailsByServiceType(requestDto.ServiceId);

            //filter based on service date from and to / invoice date
            if (Enum.TryParse(requestDto.DateTypeId.ToString(), out SearchType _datesearchtype))
            {
                if (requestDto.InvoiceFromDate?.ToDateTime() != null && requestDto.InvoiceToDate?.ToDateTime() != null)
                {
                    switch (_datesearchtype)
                    {
                        case SearchType.InvoiceDate:
                            {
                                invoiceData = invoiceData.Where(x => (x.InvoiceDate.HasValue && x.InvoiceDate.Value.Date >= requestDto.InvoiceFromDate.ToDateTime().Date) &&
                                            (x.InvoiceDate.HasValue && x.InvoiceDate.Value.Date <= requestDto.InvoiceToDate.ToDateTime().Date));
                                break;
                            }
                        case SearchType.ServiceDate:
                            {
                                if (requestDto.ServiceId == (int)Service.InspectionId)
                                {
                                    invoiceData = invoiceData.Where(x => !((x.Inspection.ServiceDateFrom.Date > requestDto.InvoiceToDate.ToDateTime().Date) || (x.Inspection.ServiceDateTo.Date < requestDto.InvoiceFromDate.ToDateTime().Date)));
                                }
                                else if (requestDto.ServiceId == (int)Service.AuditId)
                                {
                                    invoiceData = invoiceData.Where(x => !((x.Audit.ServiceDateFrom.Date > requestDto.InvoiceToDate.ToDateTime().Date) || (x.Audit.ServiceDateTo.Date < requestDto.InvoiceFromDate.ToDateTime().Date)));
                                }
                                break;
                            }
                    }
                }
            }

            //filter by invoice To
            if (requestDto.InvoiceTo > 0)
            {
                invoiceData = requestDto.InvoiceTo == (int)QuotationPaidBy.customer ? invoiceData.Where(x => x.InvoiceTo == (int)QuotationPaidBy.customer) : requestDto.InvoiceTo == (int)QuotationPaidBy.supplier ? invoiceData.Where(x => x.InvoiceTo == (int)QuotationPaidBy.supplier) : invoiceData.Where(x => x.InvoiceTo == (int)QuotationPaidBy.factory);
            }

            //filter by invoice Type
            if (requestDto.InvoiceType > 0)
            {
                invoiceData = invoiceData.Where(x => x.InvoiceType == requestDto.InvoiceType);
            }

            //filter by customerId
            if (requestDto.CustomerId > 0)
            {
                if (requestDto.ServiceId == (int)Service.InspectionId)
                {
                    invoiceData = invoiceData.Where(x => x.Inspection.CustomerId == requestDto.CustomerId);
                }
                else
                {
                    invoiceData = invoiceData.Where(x => x.Audit.CustomerId == requestDto.CustomerId);
                }
            }

            //filter by supplierId
            if (requestDto.SupplierId > 0)
            {
                if (requestDto.ServiceId == (int)Service.InspectionId)
                {
                    invoiceData = invoiceData.Where(x => x.Inspection.SupplierId == requestDto.SupplierId);
                }
                else
                {
                    invoiceData = invoiceData.Where(x => x.Audit.SupplierId == requestDto.SupplierId);
                }
            }

            //filter by factoryId
            if (requestDto.FactoryId > 0)
            {
                if (requestDto.ServiceId == (int)Service.InspectionId)
                {
                    invoiceData = invoiceData.Where(x => x.Inspection.FactoryId == requestDto.FactoryId);
                }
                else
                {
                    invoiceData = invoiceData.Where(x => x.Audit.FactoryId == requestDto.FactoryId);
                }
            }

            //filter by Booking Id
            if (requestDto.SearchTypeId == (int)SearchType.BookingNo)
            {
                if (!string.IsNullOrEmpty(requestDto.SearchTypeText?.Trim()) && int.TryParse(requestDto.SearchTypeText?.Trim(), out int bookid))
                {
                    if (requestDto.ServiceId == (int)Service.InspectionId)
                    {
                        invoiceData = invoiceData.Where(x => x.InspectionId == bookid);
                    }
                    else
                    {
                        invoiceData = invoiceData.Where(x => x.AuditId == bookid);
                    }
                }
            }

            if (requestDto.FactoryCountryIds != null && requestDto.FactoryCountryIds.Any())
            {
                if (requestDto.ServiceId == (int)Service.InspectionId)
                {
                    invoiceData = invoiceData.Where(x => x.Inspection.Factory.SuAddresses.Any(y => y.AddressTypeId == (int)RefAddressTypeEnum.HeadOffice && requestDto.FactoryCountryIds.Contains(y.CountryId)));
                }
                else
                {
                    invoiceData = invoiceData.Where(x => x.Audit.Factory.SuAddresses.Any(y => y.AddressTypeId == (int)RefAddressTypeEnum.HeadOffice && requestDto.FactoryCountryIds.Contains(y.CountryId)));
                }
            }

            //filter by invoice No
            if (requestDto.SearchTypeId == (int)SearchType.InvoiceNo)
            {
                if (!string.IsNullOrEmpty(requestDto.SearchTypeText?.Trim()))
                {
                    invoiceData = invoiceData.Where(x => x.InvoiceNo == requestDto.SearchTypeText.Trim());
                }
            }

            if (requestDto.InvoiceStatusId != null && requestDto.InvoiceStatusId.Any())
            {
                invoiceData = invoiceData.Where(x => requestDto.InvoiceStatusId.Contains(x.InvoiceStatus.GetValueOrDefault()));
            }


            // check invoice data access and display the data based onn that filter
            if (_ApplicationContext.StaffId > 0)
            {
                var invoiceAccessData = await _invoiceDataAccessRepository.GetStaffInvoiceDataAccess(_ApplicationContext.StaffId);

                if (invoiceAccessData != null)
                {
                    var customerIdAccess = invoiceAccessData.InvDaCustomers.Where(x => x.Active).Select(x => x.CustomerId).Distinct().ToList();

                    var officeIdAccess = invoiceAccessData.InvDaOffices.Where(x => x.Active).Select(x => x.OfficeId).Distinct().ToList();

                    var invoiceTypeAccess = invoiceAccessData.InvDaInvoiceTypes.Where(x => x.Active).Select(x => x.InvoiceTypeId).Distinct().ToList();

                    if (customerIdAccess.Any())
                    {
                        if (requestDto.ServiceId == (int)Service.InspectionId)
                        {
                            invoiceData = invoiceData.Where(x => customerIdAccess.Contains(x.Inspection.CustomerId));
                        }
                        else
                        {
                            invoiceData = invoiceData.Where(x => customerIdAccess.Contains(x.Audit.CustomerId));
                        }
                    }

                    if (officeIdAccess.Any())
                    {
                        if (requestDto.ServiceId == (int)Service.InspectionId)
                        {
                            invoiceData = invoiceData.Where(x => officeIdAccess.Contains(x.Inspection.OfficeId.GetValueOrDefault()));
                        }
                        else
                        {
                            invoiceData = invoiceData.Where(x => officeIdAccess.Contains(x.Audit.OfficeId.GetValueOrDefault()));
                        }
                    }

                    if (invoiceTypeAccess.Any())
                    {
                        invoiceData = invoiceData.Where(x => invoiceTypeAccess.Contains(x.InvoiceType.GetValueOrDefault()));
                    }
                }
            }

            if (requestDto.OfficeIdList != null && requestDto.OfficeIdList.Any())
            {
                invoiceData = invoiceData.Where(x => requestDto.OfficeIdList.Contains(x.Office.GetValueOrDefault()));
            }

            if (requestDto.PaymentStatusIdList != null && requestDto.PaymentStatusIdList.Any())
            {
                invoiceData = invoiceData.Where(x => requestDto.PaymentStatusIdList.Contains(x.InvoicePaymentStatus.GetValueOrDefault()));
            }

            if (requestDto.BillingMethodIdList != null && requestDto.BillingMethodIdList.Any())
            {
                invoiceData = invoiceData.Where(x => requestDto.BillingMethodIdList.Contains(x.InvoiceMethod.GetValueOrDefault()));
            }

            //fetch the first 10 invoice Ids
            invoiceData = invoiceData.OrderByDescending(x => x.InvoiceNo);

            var invoiceList = await invoiceData.GroupBy(x => new { InvoiceNo = x.InvoiceNo, StatusId = x.InvoiceStatus.GetValueOrDefault(), StatusName = x.InvoiceStatusNavigation.Name }).Select(x => new InvoiceSummaryItem()
            {
                StatusId = x.Key.StatusId,
                StatusName = x.Key.StatusName,
                InvoiceNo = x.Key.InvoiceNo
            }).ToListAsync();

            response.TotalCount = await invoiceData.Select(x => x.InvoiceNo).Distinct().CountAsync();

            var invoiceNoList = invoiceData.Select(x => x.InvoiceNo).Distinct().Skip(skip).Take(requestDto.pageSize.GetValueOrDefault());

            IQueryable<InvoiceSummaryItem> invoiceResultData = null;
            IEnumerable<ServiceTypeList> serviceTypeList;

            var dataList = new List<InvoiceSummaryItem>();
            List<QuotationInspectionTravelCost> quotationmanday = null;
            if (requestDto.ServiceId == (int)Service.InspectionId)
            {
                //fetch the booking and invoice details
                invoiceResultData = _invoiceRepository.GetQueryableInvoiceDetailsByInvoiceNo(invoiceNoList);
                var _bookingIds = _invoiceRepository.GetQueryableInvoiceBookingNo(invoiceNoList);
                serviceTypeList = await _inspRepository.GetServiceTypeList(_bookingIds);

                dataList = await invoiceResultData.AsNoTracking().ToListAsync();
                var bookingidlst = dataList.Where(x => x.BookingId.HasValue).Select(y => y.BookingId.Value).ToList();
                response.serviceTypeList = serviceTypeList.Where(x => bookingidlst.Contains(x.InspectionId)).ToList();
                quotationmanday = await _invoiceRepository.GetQuotationDataByBookingIdsList(bookingidlst);
            }
            else if (requestDto.ServiceId == (int)Service.AuditId)
            {
                //fetch the Audit and invoice details
                invoiceResultData = _invoiceRepository.GetQueryableAuditInvoiceDetailsByInvoiceNo(invoiceNoList);
                var _AuditIds = _invoiceRepository.GetQueryableInvoiceAuditNo(invoiceNoList);
                serviceTypeList = await _inspRepository.GetQueryableAuditServiceType(_AuditIds);

                dataList = await invoiceResultData.AsNoTracking().ToListAsync();
                var bookingidlst = dataList.Where(x => x.AuditId.HasValue).Select(y => y.AuditId.Value).ToList();
                response.serviceTypeList = serviceTypeList.Where(x => bookingidlst.Contains(x.AuditId)).ToList();
                quotationmanday = await _invoiceRepository.GetQuotationDataByAuditList(bookingidlst);
            }
            //get factory countries
            var factoryCountries = await _invoiceRepository.GetBookingFactoryDetails(dataList.Select(x => x.FactoryId.GetValueOrDefault()).Distinct().ToList());
            dataList = _invoicemap.MapinspQuotationDataToInvoice(dataList, quotationmanday, factoryCountries);

            var distinctInvoiceIds = invoiceData.Select(x => x.Id).Distinct();
            var invoiceExtraFees = await _invoiceRepository.GetQueryableInvoiceExtraFeeItem(distinctInvoiceIds);
            response.invoiceDataList = dataList;
            response.invoiceExtraFeeList = invoiceExtraFees.ToList();

            response.StatusCountList = invoiceList.GroupBy(p => new { p.StatusId, p.StatusName }, p => p, (key, _data) =>
                        new InspectionStatus
                        {
                            Id = key.StatusId,
                            StatusName = key.StatusName,
                            TotalCount = _data.Count(),
                            StatusColor = InvoiceStatusColor.GetValueOrDefault(key.StatusId, "")
                        }).ToList();

            return response;
        }

        /// <summary>
        /// invoice search summary
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<InvoiceSummaryResponse> GetInvoiceSearchSummary(InvoiceSummaryRequest requestDto)
        {
            if (requestDto == null)
                return new InvoiceSummaryResponse() { Result = InvoiceSummaryResult.NotFound };

            var data = await GetAllInvoiceSummary(requestDto);

            if (data.invoiceDataList == null || !data.invoiceDataList.Any())
            {
                return new InvoiceSummaryResponse() { Result = InvoiceSummaryResult.NotFound };
            }

            var result = _invoicesummarymap.MapInvoiceSummary(data.invoiceDataList, data.serviceTypeList, data.invoiceExtraFeeList);

            var response = new InvoiceSummaryResponse()
            {
                Result = InvoiceSummaryResult.Success,
                Data = result,
                TotalCount = data.TotalCount,
                PageSize = requestDto.pageSize.Value,
                Index = requestDto.Index.Value,
                InvoiceStatuslst = data.StatusCountList,
                PageCount = (result.Count() / requestDto.pageSize.Value) + (result.Count() % requestDto.pageSize.Value > 0 ? 1 : 0)
            };

            return response;
        }

        /// <summary>
        /// call invoice report project API to get the report template names
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<InvoiceReportTemplateResponse> GetInvoiceReportTemplates(InvoiceReportTemplateRequest invoiceReportTemplateRequest)
        {
            var response = new InvoiceReportTemplateResponse();
            var result = new InvoiceReportTemplate();

            //Fetch the inoice pdf project request settings from appsettings
            IEnumerable<IConfigurationSection> config = null;
            var Entityid = _filterService.GetCompanyId();
            if (Entityid == (int)Company.api)
            {
                config = _configuration.GetSection("InvoiceViewSettings:API").GetChildren();
            }
            else if (Entityid == (int)Company.sgt)
            {
                config = _configuration.GetSection("InvoiceViewSettings:SGT").GetChildren();
            }
            else if (Entityid == (int)Company.aqf)
            {
                config = _configuration.GetSection("InvoiceViewSettings:AQF").GetChildren();
            }

            string requestUrl = config.Where(x => x.Key == "RequestUrl").Select(x => x.Value).FirstOrDefault(); //"invoicepdf/api/report/getreporttemplates?page=1&take=7";
            string baseUrl = config.Where(x => x.Key == "BaseUrl").Select(x => x.Value).FirstOrDefault(); //"https://sgtlink.net";

            InvoiceViewData objAccount = new InvoiceViewData()
            {
                CustomerId = invoiceReportTemplateRequest.CustomerId,
                InvoiceType = "0",
                TemplateName = "",
                EntityId = config.Where(x => x.Key == "EntityId").Select(x => x.Value).FirstOrDefault(),
                InvoicePreview = invoiceReportTemplateRequest.InvoicePreviewTypes != null && invoiceReportTemplateRequest.InvoicePreviewTypes.Count > 0 ? string.Join(",", invoiceReportTemplateRequest.InvoicePreviewTypes) : "0",
                IsServerRequest = true
            };

            //invoicepdf project API call
            HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, requestUrl, objAccount, baseUrl, "");

            if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                response.Result = InvoiceSummaryResult.NotFound;
                return response;
            }

            //convert the response content to json
            var receiveStream = await httpResponse.Content.ReadAsStringAsync();

            if (receiveStream == null)
            {
                response.Result = InvoiceSummaryResult.NotFound;
                return response;
            }

            //deserialize json to object
            result = JsonConvert.DeserializeObject<InvoiceReportTemplate>(receiveStream);
            response.ResultList = _invoicesummarymap.MapInvoiceReportTemplates(result);

            if (response.ResultList == null || !response.ResultList.Any())
            {
                response.Result = InvoiceSummaryResult.NotFound;
                return response;
            }

            response.Result = InvoiceSummaryResult.Success;

            return response;
        }

        public async Task<InvoiceCancelResponse> CancelInvoice(string invoiceId)
        {
            var response = new InvoiceCancelResponse();
            var data = await _invoiceRepository.GetInvoice(invoiceId);
            var entityId = _filterService.GetCompanyId();
            foreach (var invoice in data)
            {
                // when cancel the invoice and delete the invoice id mapping from extra fees and update the status
                foreach (var extrafee in invoice.InvExfTransactions.Where(x => x.StatusId != (int)ExtraFeeStatus.Cancelled))
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

                invoice.InvoiceStatus = (int)InvoiceStatus.Cancelled;
                invoice.UpdatedBy = _ApplicationContext.UserId;
                invoice.UpdatedOn = DateTime.Now;
                // add log based on the service
                if (invoice.ServiceId == (int)Service.InspectionId)
                {
                    AddInvoiceStatusLog(invoice);
                }
                else if (invoice.ServiceId == (int)Service.AuditId)
                {
                    AddAuditInvoiceStatusLog(invoice);
                }

            }

            await _invoiceRepository.Save();

            response.Result = InvoiceSummaryResult.Success;

            return response;
        }

        /// <summary>
        /// invoice search summary Export
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<ExportInvoiceBookingData>> ExportInvoiceSearchSummary(InvoiceSummaryRequest requestDto)
        {


            var result = await GetAllInvoiceSummary(requestDto);

            var response = result.invoiceDataList.Select(x => _invoicesummarymap.MapInvoiceExport(x, result.serviceTypeList, result.invoiceExtraFeeList)).ToList();

            return response;
        }

        /// <summary>
        /// get the booking details for each invoice
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<InvoiceBookingSummaryResponse> GetInvoiceBookingSearchSummary(string invoiceNo, int serviceId)
        {
            var response = new InvoiceBookingSummaryResponse();
            var invoiceNoList = new[] { invoiceNo }.ToList();
            IEnumerable<InvoiceSummaryItem> data = null;

            if (serviceId == (int)Service.InspectionId)
            {
                data = await _invoiceRepository.GetInvoiceDetailsByInvoiceNo(invoiceNoList);
            }
            else
            {
                data = await _invoiceRepository.GetAuditInvoiceDetailsByInvoiceNo(invoiceNoList);
            }

            var invoiceIdList = data.Select(x => x.Id).Distinct().ToList();

            var invoiceExtarFees = await _invoiceRepository.GetInvoiceExtraFeeItem(invoiceIdList);

            var factoryCountries = await _invoiceRepository.GetBookingFactoryDetails(data.Select(x => x.FactoryId.GetValueOrDefault()).ToList());
            if (serviceId == (int)Service.InspectionId)
            {
                response.Data = data.Select(x => _invoicesummarymap.MapInvoiceBooking(x, invoiceExtarFees.ToList(), factoryCountries)).ToList();
            }
            else if (serviceId == (int)Service.AuditId)
            {
                response.Data = data.Select(x => _invoicesummarymap.MapInvoiceAudit(x, invoiceExtarFees.ToList(), factoryCountries)).ToList();
            }

            if (response.Data == null || !response.Data.Any())
            {
                response.Result = InvoiceSummaryResult.NotFound;
                return response;
            }
            response.Result = InvoiceSummaryResult.Success;
            return response;
        }

        public async Task<DataSourceResponse> GetInvoiceOffice()
        {
            var paymentStatus = await _invoiceRepository.GetInvoiceOffice();
            if (paymentStatus == null || paymentStatus.Count() == 0)
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse() { DataSourceList = paymentStatus, Result = DataSourceResult.Success };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        public async Task<InvoiceMoExistsResult> CheckInvoiceNumberExist(string invoiceNumber)
        {
            var response = await _invoiceRepository.CheckInvoiceNumberExist(invoiceNumber);
            if (response)
                return new InvoiceMoExistsResult { isInvoiceNoExists = true };
            return new InvoiceMoExistsResult { isInvoiceNoExists = false };
        }

        public async Task<InvoiceBookingProductsResponse> GetInvoiceBookingProducts(int bookingId)
        {
            var bookingProducts = await _invoiceRepository.GetInvoiceBookingProducts(new[] { bookingId });
            if (bookingProducts == null || bookingProducts.Count() == 0)
                return new InvoiceBookingProductsResponse { Result = InvoiceBookingProductResult.NotFound };

            //get the booking po number list
            var bookingPoNumberList = await _inspRepository.GetPoNoListByBookingIds(new[] { bookingId });

            foreach (var bookingProduct in bookingProducts)
            {
                bookingProduct.PoNumber = string.Join(",", bookingPoNumberList.Where(x => x.ProductRefId == bookingProduct.ProductRefId).Select(x => x.PoNumber).ToList());
            }

            return new InvoiceBookingProductsResponse() { Result = InvoiceBookingProductResult.Success, InvoiceBookingProducts = bookingProducts };
        }

        /// <summary>
        /// fetch all the invoice status
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInvoiceStatusList()
        {
            var data = await _invoiceRepository.GetInvoiceStatus();

            if (data == null || !data.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }

        /// <summary>
        /// Get invoice new booking details
        /// </summary>
        /// <returns></returns>
        public async Task<InvoiceNewBookingResponse> GetNewInvoiceBookingData(NewBookingInvoiceSearch request)
        {
            var listOfBookingData = new List<InvoiceNewBookingDetail>();

            IQueryable<InvoiceNewBookingDetailRepo> bookingdata;

            if (request.ServiceId == (int)Service.InspectionId)
            {
                bookingdata = _invoiceRepository.GetNewInvoiceBookingData();

                if (request.InvoiceType == (int)INVInvoiceType.Monthly)
                {
                    //filter based on status - only validated and inspected bookings
                    var inspectedStatusList = InspectedStatusList.Select(x => x);
                    bookingdata = bookingdata.Where(x => inspectedStatusList.Contains(x.StatusId));
                }
                else // pre invoice will allow all the booking status except cancel and hold.
                {
                    //filter booking status other than cancel and hold
                    var invalidStatusList = InValidBookingStatusList.Select(x => x);
                    bookingdata = bookingdata.Where(x => !invalidStatusList.Contains(x.StatusId));
                }
            }
            else
            {
                bookingdata = _invoiceRepository.GetNewInvoiceAuditBookingData();

                if ((request.InvoiceType == (int)INVInvoiceType.Monthly))
                {
                    var inspectedStatusList = AuditedStatusList.Select(x => x);
                    bookingdata = bookingdata.Where(x => inspectedStatusList.Contains(x.StatusId));
                }
                else
                {
                    var invalidStatusList = PreInvoiceAuditStatusList.Select(x => x);
                    bookingdata = bookingdata.Where(x => invalidStatusList.Contains(x.StatusId));
                }
            }

            if (request.CustomerId > 0)
            {
                bookingdata = bookingdata.Where(x => x.CustomerId == request.CustomerId);
            }

            if (request.SupplierId > 0)
            {
                bookingdata = bookingdata.Where(x => x.SupplierId == request.SupplierId);
            }

            if (request.FactoryId > 0)
            {
                bookingdata = bookingdata.Where(x => x.FactoryId == request.FactoryId);
            }

            if (request.BookingNumber > 0)
            {
                bookingdata = bookingdata.Where(x => x.BookingId == request.BookingNumber);
            }

            if (request.BookingStartDate != null && request.BookingEndDate != null)
            {
                bookingdata = bookingdata.Where(x => (x.ServiceToDate >= request.BookingStartDate.ToDateTime()) &&
                  (x.ServiceToDate <= request.BookingEndDate.ToDateTime()));
            }

            var resultData = await bookingdata.ToListAsync();

            var searchBookingIds = resultData.Select(x => x.BookingId).Distinct().ToList();

            List<int> activeInvoiceBookingIds;

            if (request.ServiceId == (int)Service.InspectionId)
            {
                // Check active invoice booking filter
                activeInvoiceBookingIds = await _invoiceRepository.GetActiveInvoiceInspectionIdList(searchBookingIds);
            }
            else
            {
                // Check active invoice audit booking filter
                activeInvoiceBookingIds = await _invoiceRepository.GetActiveInvoiceAuditIdList(searchBookingIds);
            }

            resultData = resultData.Where(x => !activeInvoiceBookingIds.Contains(x.BookingId)).ToList();

            if (!resultData.Any())
            {
                return new InvoiceNewBookingResponse { Result = InvoiceNewBookingResult.nodata };
            }

            foreach (var item in resultData)
            {
                List<QuotationInspectionTravelCost> quotationData = null;
                // check with quotation data billed to option
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    quotationData = await _invoiceRepository.GetQuotationDataByBookingNo(item.BookingId);
                }
                else if (request.ServiceId == (int)Service.AuditId)
                {
                    quotationData = await _invoiceRepository.GetQuotationDataByAuditNo(item.BookingId);
                }
                if ((quotationData.Any() && quotationData?.FirstOrDefault()?.InvoiceType == request.InvoiceType
                    && quotationData?.FirstOrDefault()?.BillingTo == request.BilledTo)
                    || (!quotationData.Any() && request.InvoiceType == (int)INVInvoiceType.Monthly))
                {
                    var invoiceNewBookingDetail = new InvoiceNewBookingDetail()
                    {
                        BookingId = item.BookingId,
                        CustomerName = item.CustomerName,
                        CustomerId = item.CustomerId,
                        FactoryId = item.FactoryId,
                        SupplierId = item.SupplierId,
                        PriceCategoryId = item.PriceCategoryId,
                        FactoryName = item.FactoryName,
                        SupplierName = item.SupplierName,
                        ServiceTypeName = item.ServiceTypeName,
                        ServiceDate = string.Empty,
                        PriceCategoryName = item.PriceCategoryName,
                        BookingQuantity = item.BookingQuantity
                    };

                    if (item.ServiceFromDate == item.ServiceToDate)
                    {
                        invoiceNewBookingDetail.ServiceDate = item.ServiceFromDate.ToString(StandardDateFormat);
                    }
                    else
                    {
                        invoiceNewBookingDetail.ServiceDate = item.ServiceToDate.ToString(StandardDateFormat);
                    }

                    listOfBookingData.Add(invoiceNewBookingDetail);
                }
            }

            if (!listOfBookingData.Any())
            {
                return new InvoiceNewBookingResponse { Result = InvoiceNewBookingResult.nodata };
            }

            return new InvoiceNewBookingResponse
            {
                BookingList = listOfBookingData,
                Result = InvoiceNewBookingResult.success
            };
        }

        /// <summary>
        /// fetch all the kpi templates from REF_KPI_Teamplate with type = invoice
        /// </summary>
        /// <returns></returns>
        public async Task<InvoiceKpiTemplateResponse> GetInvoiceKpiTemplate(InvoiceKpiTemplateRequest request)
        {
            var data = _invoiceRepository.GetQueryable<RefKpiTeamplate>(x => x.Active && x.TypeId == (int)KpiTemplateType.Invoice);

            if (request.CustomerIds != null && request.CustomerIds.Any())
                data = data.Where(x => !x.RefKpiTeamplateCustomers.Any() || x.RefKpiTeamplateCustomers.Any(y => request.CustomerIds.Any(x => x == y.CustomerId)));

            var res = await data.Select(x => new DTO.Kpi.KPITemplate
            {
                Id = x.Id,
                Name = x.Name,
                TypeId = x.TypeId
            }).AsNoTracking().ToListAsync();

            if (res == null || !res.Any())
            {
                return new InvoiceKpiTemplateResponse { Result = InvoiceSummaryResult.NotFound };
            }

            return new InvoiceKpiTemplateResponse
            {
                TemplateList = res,
                Result = InvoiceSummaryResult.Success
            };
        }
        /// <summary>
        /// get tax details
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="maxInspectionDate"></param>
        /// <param name="minInspectionDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvoiceBankTax>> GetTaxDetails(int bankId, DateTime maxInspectionDate, DateTime minInspectionDate)
        {
            return await _invoiceRepository.GetTaxDetails(bankId, maxInspectionDate, minInspectionDate);
        }

        public async Task<IEnumerable<CustomerPriceCardRepo>> GetPriceCardRuleList(InvoiceGenerateRequest requestDto)
        {
            return await _invoiceRepository.GetPriceCardRuleList(requestDto);
        }

        /// <summary>
        /// Check invoice pdf created or not
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<InvoicePdfCreatedResponse> CheckInvoicePdfCreated(InvoicePdfCreatedRequest request)
        {

            List<string> pdfNotAvailableInvoiceList = new List<string>();

            if (request == null)
                return new InvoicePdfCreatedResponse() { Result = InvoicePdfCreatedResponseResult.RequestIsNotValid };

            if (request.InvoiceNumbers == null || !request.InvoiceNumbers.Any())
                return new InvoicePdfCreatedResponse() { Result = InvoicePdfCreatedResponseResult.RequestIsNotValid };

            var dbInvoiceList = await _invoiceRepository.getInvoicePdfFiles(request.InvoiceNumbers);

            if (dbInvoiceList == null || !dbInvoiceList.Any())
            {
                return new InvoicePdfCreatedResponse() { InvoiceNumbers = request.InvoiceNumbers, Result = InvoicePdfCreatedResponseResult.PdfNotCreatedToAnyInvoice };
            }

            if (dbInvoiceList.Any() && request.InvoiceNumbers != null && request.InvoiceNumbers.Any())
            {
                foreach (var invoice in request.InvoiceNumbers)
                {
                    if (!dbInvoiceList.Contains(invoice))
                    {
                        pdfNotAvailableInvoiceList.Add(invoice);
                    }
                }
            }

            if (pdfNotAvailableInvoiceList.Any())
            {
                return new InvoicePdfCreatedResponse() { InvoiceNumbers = pdfNotAvailableInvoiceList, Result = InvoicePdfCreatedResponseResult.PdfCreatedToFewInvoice };
            }

            return new InvoicePdfCreatedResponse() { Result = InvoicePdfCreatedResponseResult.PdfCreatedToAllInvoice };
        }

        private int GetWeek(DateTime date)
        {
            if (date != null)
            {
                var calendar = CultureInfo.CurrentCulture.Calendar;
                return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            }
            return 0;
        }
    }
}