using BI.Maps;
using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.DynamicFields;
using DTO.ExtraFees;
using DTO.Inspection;
using DTO.Invoice;
using DTO.InvoicePreview;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static BI.TenantProvider;
using InvoicePreview = Entities.Enums.InvoicePreview;

namespace BI
{
    public class InvoicePreviewManager : ApiCommonData, IInvoicePreviewManager
    {

        private readonly IInvoicePreivewRepository _repo = null;
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly IInspectionBookingRepository _bookingRepository = null;
        private readonly IAuditRepository _auditRepository = null;
        private readonly ISupplierManager _supplierManager = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly IClaimRepository _claimRepository = null;
        private readonly InvoiceMap _invoicemap = null;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IManualInvoiceRepository _manualInvoiceRepository = null;
        private readonly IKpiCustomRepository _kpiCustomRepository;
        private readonly IAPIUserContext _applicationUserContext;
        private readonly ITenantProvider _tenantProvider;
        private readonly IHelper _helper;
        private readonly IFileManager _fileManager;
        private readonly IDynamicFieldRepository _dynamicFieldRepository = null;
        private static IConfiguration _Configuration = null;

        public InvoicePreviewManager(IInvoicePreivewRepository repo, IInvoiceRepository invoiceRepository, IInspectionBookingRepository bookingRepository, IKpiCustomRepository kpiCustomRepository,
            IAuditRepository auditRepository, IScheduleRepository scheduleRepository,
            IManualInvoiceRepository manualInvoiceRepository, IHelper helper, IFileManager fileManager,
            IClaimRepository claimRepository, IAPIUserContext applicationUserContext, ITenantProvider tenantProvider,
            ISupplierManager supplierManager, ICustomerRepository customerRepository, IDynamicFieldRepository dynamicFieldRepository,
            IConfiguration configuration)
        {
            _invoiceRepository = invoiceRepository;
            _repo = repo;
            _bookingRepository = bookingRepository;
            _auditRepository = auditRepository;
            _supplierManager = supplierManager;
            _customerRepository = customerRepository;
            _claimRepository = claimRepository;
            _invoicemap = new InvoiceMap();
            _scheduleRepository = scheduleRepository;
            _manualInvoiceRepository = manualInvoiceRepository;
            _kpiCustomRepository = kpiCustomRepository;
            _applicationUserContext = applicationUserContext;
            _tenantProvider = tenantProvider;
            _helper = helper;
            _fileManager = fileManager;
            _dynamicFieldRepository = dynamicFieldRepository;
            _Configuration = configuration;
        }

        //invoice type fetch from enums
        public IEnumerable<DataCommon> GetInvoiceType()
        {
            var invoiceTypeList = new List<DataCommon>();

            var invoiceType = new DataCommon
            {
                Key = Convert.ToString((int)InvoiceType.Automation),
                Value = InvoiceType.Automation.ToString()
            };

            invoiceTypeList.Add(invoiceType);

            var invoiceType1 = new DataCommon
            {
                Key = Convert.ToString((int)InvoiceType.Manual),
                Value = InvoiceType.Manual.ToString()
            };

            invoiceTypeList.Add(invoiceType1);

            return invoiceTypeList;
        }

        //invoice preview fetch from enums
        public IEnumerable<DataCommon> GetInvoicePreview()
        {
            var invoicePreviewList = new List<DataCommon>()
            {
                new DataCommon
                 {
                     Key = Convert.ToString((int)InvoicePreview.Booking),
                     Value = InvoicePreview.Booking.ToString()
                 },
              new DataCommon
                {
                    Key = Convert.ToString((int)InvoicePreview.Product),
                    Value = InvoicePreview.Product.ToString()
                },
              new DataCommon
                {
                    Key = Convert.ToString((int)InvoicePreview.SimpleInvoice),
                    Value = InvoicePreview.SimpleInvoice.ToString()
                },
               new DataCommon
                {
                    Key = Convert.ToString((int)InvoicePreview.PO),
                    Value = InvoicePreview.PO.ToString()
                },
                new DataCommon
                {
                    Key = Convert.ToString((int)InvoicePreview.InvoiceExtraFee),
                    Value = InvoicePreview.InvoiceExtraFee.ToString()
                },
                new DataCommon
                {
                    Key = Convert.ToString((int)InvoicePreview.Audit),
                    Value = InvoicePreview.Audit.ToString()
                },
                new DataCommon
                {
                    Key = Convert.ToString((int)InvoicePreview.ManualInvoice),
                    Value = InvoicePreview.ManualInvoice.ToString()
                },
                 new DataCommon
                {
                    Key = Convert.ToString((int)InvoicePreview.CreditNote),
                    Value = InvoicePreview.CreditNote.ToString()
                }
            };

            return invoicePreviewList;

        }

        //get bank details with tax
        public async Task<IEnumerable<InvoiceBankPreview>> GetBankInfo()
        {
            try
            {
                var bankDetails = new List<InvoiceBankPreview>();

                var bankTaxDetails = new List<InvoiceBankTaxRepo>();

                var bankList = await _repo.GetBankDetails();

                if (bankList.Any())
                {
                    var bankIds = bankList.Select(x => x.Id).ToList();
                    bankTaxDetails = await _repo.GetBankTaxDetails(bankIds);
                    bankList.ForEach(x => x.InvoiceBankTaxList = bankTaxDetails.Where(y => y.BankId == x.Id).ToList());
                }

                foreach (var item in bankList)
                {
                    //map bank data
                    var invoiceDetail = _invoicemap.BankDetailsMap(item);

                    foreach (var taxItem in item.InvoiceBankTaxList)
                    {
                        //map tax
                        var taxDetails = _invoicemap.BankTaxDetailsMap(taxItem);

                        invoiceDetail.InvoiceBankTaxList.Add(taxDetails);
                    }
                    bankDetails.Add(invoiceDetail);
                }

                return bankDetails;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //get invoice Details
        public async Task<IEnumerable<List<DataCommon>>> GetInvoiceDetails(string invoiceNo, int invoicePreview)
        {
            try
            {
                var invoiceDetailList = new List<List<DataCommon>>();
                List<InvoiceDetailsRepo> invoiceData = null;

                switch (invoicePreview)
                {
                    case (int)InvoicePreview.ManualInvoice:
                        invoiceData = await _manualInvoiceRepository.GetManualInvoiceDetails(invoiceNo);
                        var taxList = await _manualInvoiceRepository.GetManualInvoiceTaxDetails(invoiceData.Select(x => x.Id).ToList());
                        invoiceData.ForEach(x => x.BankTaxId = taxList.Where(y => y.InvoiceId == x.Id).Select(y => y.TaxId).ToList());
                        invoiceDetailList = await FrameInvoiceDetailsData(invoicePreview, invoiceData, null, null, null, null, null, null, null, null, null, null);
                        break;
                    case (int)InvoicePreview.InvoiceExtraFee:
                        //get extra fee details
                        invoiceData = await _repo.GetExtraFeeInvoiceDetails(invoiceNo);
                        break;
                    case (int)InvoicePreview.CreditNote:
                        invoiceData = await _claimRepository.GetCreditNoteDetails(invoiceNo);
                        break;
                    default: //for the Booking, Product, SimpleInvoice, PO, Audit - InvoicePreview
                        invoiceData = await _repo.GetInvoiceDetails(invoiceNo);
                        break;

                }

                if (invoiceData.Any() && invoicePreview != (int)InvoicePreview.ManualInvoice) // tax logic is missing for manual invoice
                {
                    var invoiceIdList = invoiceData.Select(x => x.Id).ToList();
                    var taxList = await _repo.GetInvoiceTaxDetails(invoiceIdList);
                    invoiceData.ForEach(x => x.BankTaxId = taxList.Where(y => y.InvoiceId == x.Id).Select(y => y.TaxId).ToList());
                }

                //for not the Manual Invoice
                if (invoicePreview != (int)InvoicePreview.ManualInvoice)
                {
                    // no audit service available
                    if (!invoiceData.Any(x => x.ServiceId == (int)Service.AuditId))
                    {
                        //get booking ids from invoice data
                        var bookingIds = invoiceData.Select(x => (int)x.InspectionId).ToList();

                        //get invoice ids
                        var invoiceIds = invoiceData.Select(x => x.Id);

                        //get billed contacts by invoice ids
                        var billedContactsList = await _repo.GetInvoiceBilledContacts(invoiceIds);

                        if (bookingIds.Any())
                        {
                            //product list by bookingids
                            var productPoCountryList = await _repo.GetBookingProductData(bookingIds);

                            //get total booking qty, inspected qty by booking ids
                            var bookingQuantityDetails = await _invoiceRepository.GetBookingQuantityDetails(bookingIds);

                            //get booking details by booking ids
                            var bookingList = await _repo.GetInvoiceBookingData(bookingIds);

                            //get dynamic fields data by booking ids
                            var bookingDFData = await _dynamicFieldRepository.GetBookingDFDataByBookingIds(bookingIds);

                            if (bookingList.Any())
                            {
                                var brandList = await _bookingRepository.GetBrandBookingIdsByBookingIds(bookingIds);
                                var buyerList = await _bookingRepository.GetBuyerBookingIdsByBookingIds(bookingIds);
                                var departmentList = await _bookingRepository.GetDeptBookingIdsByBookingIds(bookingIds);
                                var bookingContactList = await _bookingRepository.GetBookingCustomerContacts(bookingIds);
                                var merchandiserList = await _bookingRepository.GetMerchandiserContactsByBookingIds(bookingIds);
                                var poColorList = await _bookingRepository.GetPOColorTransactionsByBookingIds(bookingIds);
                                var scheduleStaffItems = await _scheduleRepository.GetQCBookingDetails(bookingIds);

                                var reportResults = await _bookingRepository.GetReportResultByInspectionId(bookingIds);
                                foreach (var item in bookingList)
                                {
                                    item.CustomerDepartment = departmentList.Where(y => y.BookingId == item.BookingNo).Select(z => z.DeptName).ToList();
                                    item.CustomerBrand = brandList.Where(y => y.BookingId == item.BookingNo).Select(z => z.BrandName).ToList();
                                    item.CustomerBuyer = buyerList.Where(y => y.BookingId == item.BookingNo).Select(z => z.BuyerName).ToList();
                                    item.CustomerContact = bookingContactList.Where(y => y.BookingId == item.BookingNo).Select(z => z.CustomerContactName).ToList();
                                    item.Merchandiser = merchandiserList.Where(y => y.BookingId == item.BookingNo).Select(z => z.MerchandiserContactName).ToList();
                                    item.InspPurchaseOrderColors = poColorList.Where(y => y.BookingId == item.BookingNo).ToList();
                                    item.ScheduleStaffItems = scheduleStaffItems.Where(y => y.BookingId == item.BookingNo).ToList();
                                    item.ReportResults = reportResults.Where(y => y.InspectionId == item.BookingNo).ToList();
                                }
                            }
                            //get service type names by booking ids
                            var serviceTypeList = await _bookingRepository.GetBookingServiceTypes(bookingIds);

                            //get produt list by booking ids
                            var ProductList = await _invoiceRepository.GetProductListByBooking(bookingIds);

                            //get quotation no by booking ids
                            var QuotationNoList = await _invoiceRepository.GetBookingQuotationDetails(bookingIds);

                            //factory address by factory id from booking list
                            var factoryLocationList = await _supplierManager.GetSupplierGeoLocation(bookingList.Where(x => x.FactoryId > 0).Select(x => x.FactoryId.GetValueOrDefault()).Distinct());

                            //get extra fees
                            var extrafees = await _repo.GetExtraFeeByBooking(bookingIds);

                            if (extrafees.Any())
                            {
                                var extraFeeIds = extrafees.Select(x => x.ExtraFeeId).ToList();
                                var extarFeeTaxList = await _repo.GetExtraFeesBankTaxList(extraFeeIds);
                                extrafees.ForEach(x => x.BankTaxList = extarFeeTaxList.Where(y => y.ExtraFeeId == x.ExtraFeeId).Select(z => z.TaxId).ToList());
                            }

                            var extraFeesIds = extrafees.Select(x => x.ExtraFeeId);

                            var extraFeeTypeList = await _repo.GetExtraFeeTypeByBooking(extraFeesIds);

                            var fbBookingQty = await _repo.GetInspectionQuantities(bookingIds);

                            //map the data
                            invoiceDetailList = await FrameInvoiceDetailsData(invoicePreview, invoiceData, bookingQuantityDetails, bookingList,
                                serviceTypeList, ProductList, QuotationNoList, factoryLocationList, productPoCountryList, billedContactsList, extrafees, extraFeeTypeList,
                                fbBookingQty, null, bookingDFData);

                        }
                    }
                    else
                    {
                        //get audit ids from invoice data
                        var auditIds = invoiceData.Select(x => (int)x.AuditId).ToList();

                        //get invoice ids
                        var invoiceIds = invoiceData.Select(x => x.Id);

                        //get billed contacts by invoice ids
                        var billedContactsList = await _repo.GetInvoiceBilledContacts(invoiceIds);

                        if (auditIds.Any())
                        {
                            //product list by bookingids
                            var productPoCountryList = new List<InvoiceBookingProductsData>();

                            //get total booking qty, inspected qty by booking ids
                            var bookingQuantityDetails = new List<InvoiceBookingQuantityDetails>();

                            //get booking details by audit ids
                            var bookingList = await _repo.GetInvoiceAuditBookingData(auditIds);

                            //get service type names by audit booking ids
                            var serviceTypeList = await _bookingRepository.GetAuditServiceTypes(auditIds);

                            //get produt list by audit booking ids
                            var ProductList = new List<InvoiceBookingProductsData>();

                            //get quotation no by booking ids
                            var QuotationNoList = await _invoiceRepository.GetAuditBookingQuotationDetails(auditIds);

                            //factory address by factory id from booking list
                            var factoryLocationList = await _supplierManager.GetSupplierGeoLocation(bookingList.Where(x => x.FactoryId > 0).Select(x => x.FactoryId.GetValueOrDefault()).Distinct());

                            //get extra fees
                            var extrafees = await _repo.GetExtraFeeByAuditBooking(auditIds);

                            if (extrafees.Any())
                            {
                                var extraFeeIds = extrafees.Select(x => x.ExtraFeeId).ToList();
                                var extarFeeTaxList = await _repo.GetExtraFeesBankTaxList(extraFeeIds);
                                extrafees.ForEach(x => x.BankTaxList = extarFeeTaxList.Where(y => y.ExtraFeeId == x.ExtraFeeId).Select(z => z.TaxId).ToList());
                            }

                            var extraFeesIds = extrafees.Select(x => x.ExtraFeeId);

                            var extraFeeTypeList = await _repo.GetExtraFeeTypeByBooking(extraFeesIds);

                            var auditTranFaProfiles = await _auditRepository.GetAuditTranFaProfiles(bookingList.Select(x => x.BookingNo).ToList());
                            //map the data
                            invoiceDetailList = await FrameInvoiceDetailsData(invoicePreview, invoiceData, bookingQuantityDetails, bookingList,
                                serviceTypeList, ProductList, QuotationNoList, factoryLocationList, productPoCountryList, billedContactsList, extrafees, extraFeeTypeList);
                        }

                    }
                }

                return invoiceDetailList;

            }
            catch (Exception ex)
            {
                //logger need to add
                throw ex;
            }
        }

        // frame Object as key value pair 
        private List<DataCommon> FrameInvoiceKeyValue(InvoiceDetailsPreview invoiceDetail)
        {
            var invoiceList = new List<DataCommon>();

            PropertyInfo[] properties = typeof(InvoiceDetailsPreview).GetProperties();

            //loop the InvoiceDetailsPreview properties
            foreach (PropertyInfo property in properties)
            {
                var _value = property.GetValue(invoiceDetail, null);

                var invoiceKeyData = new DataCommon
                {
                    Key = property.Name,
                    Value = _value?.ToString()
                };

                invoiceList.Add(invoiceKeyData);
            }

            return invoiceList;
        }
        private async Task<string> GetAddress(int billedto, InvoiceBookingPDFDetail bookingItem)
        {
            var billedaddress = "";
            switch (billedto)
            {
                case (int)InvoiceTo.Customer:
                    {
                        var customeraddress = await _customerRepository.GetCustomerAddressByListCusId(new List<int>() { bookingItem.customerid });
                        if (customeraddress.Any(x => x.AddressType == (int)RefAddressTypeEnum.Accounting))
                        {
                            billedaddress = customeraddress.Where(x => x.AddressType == (int)RefAddressTypeEnum.Accounting && x.CustomerId == bookingItem.customerid).Select(x => x.EnglishAddress).FirstOrDefault() ?? string.Empty;
                        }
                        else
                        {
                            billedaddress = customeraddress.Where(x => x.AddressType == (int)RefAddressTypeEnum.HeadOffice && x.CustomerId == bookingItem.customerid).Select(x => x.EnglishAddress)?.FirstOrDefault() ?? string.Empty;
                        }
                        break;
                    }
                case (int)InvoiceTo.Supplier:
                    {
                        var supplieraddress = await _supplierManager.GetSupplierOfficeAddressBylstId(new List<int>() { bookingItem.supplierid });
                        if (supplieraddress.Where(x => x.SupplierAddresstype == (int)SuAddressTypeEnum.Accounting).Any())
                        {
                            billedaddress = supplieraddress.Where(x => x.SupplierAddresstype == (int)SuAddressTypeEnum.Accounting && x.SupplierId == bookingItem.supplierid).Select(x => x.Address).FirstOrDefault();
                        }
                        else
                        {
                            billedaddress = supplieraddress.Where(x => x.SupplierAddresstype == (int)SuAddressTypeEnum.Headoffice && x.SupplierId == bookingItem.supplierid).Select(x => x.Address)?.FirstOrDefault();
                        }
                        break;
                    }
                case (int)InvoiceTo.Factory:
                    {
                        var factAddressList = await _supplierManager.GetSupplierOfficeAddressBylstId(new List<int>() { bookingItem.factoryid.GetValueOrDefault() });
                        if (factAddressList.Where(x => x.SupplierAddresstype == (int)SuAddressTypeEnum.Accounting).Any())
                        {
                            billedaddress = factAddressList.Where(x => x.SupplierAddresstype == (int)SuAddressTypeEnum.Accounting && x.SupplierId == bookingItem.factoryid).Select(x => x.Address).FirstOrDefault();
                        }
                        else
                        {
                            billedaddress = factAddressList.Where(x => x.SupplierAddresstype == (int)SuAddressTypeEnum.Headoffice && x.SupplierId == bookingItem.factoryid).Select(x => x.Address)?.FirstOrDefault();
                        }
                        break;
                    }
            }
            return billedaddress;
        }
        private async Task<List<List<DataCommon>>> FrameInvoiceDetailsData(int invoicePreview,
                    List<InvoiceDetailsRepo> invoiceDetailList, List<InvoiceBookingQuantityDetails> totalQtyBookingList,
                    List<InvoiceBookingPDFDetail> bookingList, List<BookingServiceType> serviceTypeList,
                    List<InvoiceBookingProductsData> ProductList, List<InvoiceBookingQuotation> QuotationNoList, List<SupplierGeoLocation> factoryLocationList,
                    List<InvoiceBookingProductsData> productPoCountryList, List<BilledContactsName> billedContactsList, List<ExtraFeeData> extrafees,
                    List<ExtraFeeTypeData> extraFeeTypeList, List<BookingQuantity> fbBookingQty = null, List<Entities.AudTranFaProfile> audTranFaProfiles = null,
                    List<InspectionBookingDFRepo> bookingDFData = null)
        {

            var invoiceList = new List<List<DataCommon>>();
            var invoiceData = new InvoiceDetailsPreview();
            InvoiceDetailsRepo invoiceRepoData;
            var entityId = _tenantProvider.GetCompanyId();

            //if booking level
            if (invoicePreview == (int)InvoicePreview.Booking)
            {
                if (entityId != (int)Company.api)
                {
                    bookingList = bookingList.OrderBy(x => x.ServiceDateTo).ThenBy(x => x.FactoryName).ToList();
                }
                //loop by booking level
                foreach (var bookingItem in bookingList)
                {
                    invoiceRepoData = invoiceDetailList.FirstOrDefault(x => x.InspectionId == bookingItem.BookingNo);

                    //invoice details map
                    _invoicemap.InvoiceDataMap(invoiceData, invoiceRepoData, billedContactsList, extrafees);

                    //booking details map
                    _invoicemap.BookingDataMap(bookingItem, ProductList, totalQtyBookingList, invoiceData, serviceTypeList, QuotationNoList,
                        invoiceRepoData, factoryLocationList, productPoCountryList);

                    int extraFeeId = extrafees.Where(x => x.BookingId == bookingItem.BookingNo && x.BilledTo == invoiceRepoData.InvoiceTo).Select(x => x.ExtraFeeId).FirstOrDefault();

                    var extratypeList = extraFeeTypeList.Where(x => x.ExtraFeeId == extraFeeId).ToList();

                    invoiceData.ExtraFeeTypeBooking = string.Join(", ", extratypeList.Select(x => x.ExtraFeeType).ToArray());
                    invoiceData.Colors = string.Join(", ", bookingItem.InspPurchaseOrderColors.Select(y => y.ColorName).Distinct().ToList());
                    invoiceData.ExtraFeeRemarks = extrafees.Where(x => x.BookingId == bookingItem.BookingNo).Select(x => x.Remarks).FirstOrDefault() ?? string.Empty;
                    invoiceData.ReportResult = string.Join(", ", bookingItem.ReportResults.Select(x => x.Result).Distinct().ToList());
                    invoiceData.Report = string.Join(", ", ProductList.Where(x => x.BookingId == bookingItem.BookingNo).Select(x => x.ReportNumber).Distinct().ToList()) ?? string.Empty;
                    invoiceData.ProductRef = string.Join(", ", ProductList.Where(x => x.BookingId == bookingItem.BookingNo).Select(x => x.ProductName).Distinct().ToList()) ?? string.Empty;
                    invoiceData.ProductDesc = string.Join(", ", ProductList.Where(x => x.BookingId == bookingItem.BookingNo).Select(x => x.ProductDescription).Distinct().ToList()) ?? string.Empty;
                    invoiceData.AQLQty = ProductList.Any(x => x.BookingId == bookingItem.BookingNo && x.CombineProductId.HasValue) ? (ProductList.Where(x => x.BookingId == bookingItem.BookingNo && x.CombineProductId.HasValue)
                        .Sum(x => x.CombineAQLQty.GetValueOrDefault()) + (ProductList.Where(x => x.BookingId == bookingItem.BookingNo && !x.CombineProductId.HasValue)
                        .Sum(x => x.AQLQty.GetValueOrDefault())).ToString()) : (ProductList.Where(x => x.BookingId == bookingItem.BookingNo && !x.CombineProductId.HasValue).Sum(x => x.AQLQty.GetValueOrDefault())).ToString();
                    invoiceData.InvoiceCode = bookingDFData.Where(x => x.BookingNo == bookingItem.BookingNo && x.ControlConfigurationId == InvoiceCodeControlConfigurationId).Select(x => x.DFValue).FirstOrDefault();
                    var keyInvoiceData = FrameInvoiceKeyValue(invoiceData);

                    invoiceList.Add(keyInvoiceData);

                    //after added to list - initialize the data for each 
                    invoiceData = new InvoiceDetailsPreview();
                }
            }
            //product level
            else if (invoicePreview == (int)InvoicePreview.Product)
            {
                if (entityId != (int)Company.api)
                {
                    ProductList = ProductList.OrderBy(x => x.ServiceDateTo).ThenBy(x => x.FactoryName).ToList();
                }
                int prevBookingId = 0;
                //loop by product level
                foreach (var productItem in ProductList)
                {
                    var bookingItem = bookingList.FirstOrDefault(x => x.BookingNo == productItem.BookingId);
                    if (prevBookingId != productItem.BookingId)
                    {
                        invoiceRepoData = invoiceDetailList.FirstOrDefault(x => x.InspectionId == productItem.BookingId);

                        //invoice details map
                        _invoicemap.InvoiceDataMap(invoiceData, invoiceRepoData, billedContactsList, extrafees);

                        //booking details map
                        _invoicemap.BookingDataMap(bookingItem, ProductList, totalQtyBookingList, invoiceData, serviceTypeList,
                            QuotationNoList, invoiceRepoData, factoryLocationList, productPoCountryList);

                        int extraFeeId = extrafees.Where(x => x.BookingId == productItem.BookingId && x.BilledTo == invoiceRepoData.InvoiceTo).Select(x => x.ExtraFeeId).FirstOrDefault();

                        var extratypeList = extraFeeTypeList.Where(x => x.ExtraFeeId == extraFeeId).ToList();

                        invoiceData.ExtraFeeTypeBooking = string.Join(", ", extratypeList.Select(x => x.ExtraFeeType).ToArray());
                        invoiceData.ExtraFeeRemarks = extrafees.Where(x => x.BookingId == bookingItem.BookingNo).Select(x => x.Remarks).FirstOrDefault() ?? string.Empty;

                        prevBookingId = productItem.BookingId;
                    }
                    invoiceData.Colors = string.Join(", ", bookingItem?.InspPurchaseOrderColors?.Where(x => x.ProductRefId == productItem.Id).Select(y => y.ColorName).Distinct().ToList()) ?? string.Empty;
                    invoiceData.ReportResult = string.Join(", ", bookingItem?.ReportResults?.Where(x => x.ProductRefId == productItem.Id).Select(y => y.Result).Distinct().ToList()) ?? string.Empty;
                    invoiceData.Report = productItem?.ReportNumber?.ToString() ?? string.Empty;
                    invoiceData.ProductRef = productItem?.ProductName ?? string.Empty;
                    invoiceData.ProductDesc = productItem?.ProductDescription ?? string.Empty;
                    invoiceData.ProductBookingQty = productItem?.ProductBookingQuantity.ToString() ?? string.Empty;
                    invoiceData.ProductBarCode = productItem?.ProductBarCode ?? string.Empty;
                    invoiceData.FactoryReference = productItem?.FactoryReference ?? string.Empty;
                    invoiceData.AQLQty = productItem.CombineProductId.HasValue ? productItem.CombineAQLQty.GetValueOrDefault().ToString() : productItem.AQLQty.GetValueOrDefault().ToString();
                    invoiceData.DestinationCountry = string.Join(", ", productPoCountryList?.Where(x => x.BookingId == productItem?.BookingId && x.Id == productItem?.Id)
                                                  .Select(x => x.DestinationCountry).Distinct().ToList());
                    invoiceData.ProductPO = string.Join(", ", productPoCountryList?.Where(x => x.BookingId == productItem?.BookingId && x.Id == productItem?.Id)
                                                            .Select(x => x.PoNumber).Distinct().ToList());

                    var poIdList = productPoCountryList?.Where(x => x.BookingId == productItem?.BookingId && x.Id == productItem?.Id).Select(x => x.PoTranId).ToList();

                    var fbBookingQuantityDetails = fbBookingQty?.Where(x => x.BookingId == productItem?.BookingId && poIdList.Contains(x.InspPOTransId));
                    invoiceData.ProductInspectedQty = fbBookingQuantityDetails.Where(x => x.InspectedQty > 0).Sum(x => x.InspectedQty).ToString() ?? string.Empty;
                    invoiceData.ProductPresentedQty = fbBookingQuantityDetails.Where(x => x.PresentedQty > 0).Sum(x => x.PresentedQty).ToString() ?? string.Empty;
                    //category
                    invoiceData.CustomerProductCategory = productItem?.ProductCategory ?? string.Empty;
                    invoiceData.ProductSubCategory = productItem?.ProductSubCategory ?? string.Empty;
                    invoiceData.ProductSubCategory2 = productItem?.ProductSubCategory2 ?? string.Empty;
                    invoiceData.InvoiceCode = bookingDFData.Where(x => x.BookingNo == bookingItem.BookingNo && x.ControlConfigurationId == InvoiceCodeControlConfigurationId).Select(x => x.DFValue).FirstOrDefault();

                    var keyInvoiceData = FrameInvoiceKeyValue(invoiceData);

                    invoiceList.Add(keyInvoiceData);

                    //after added to list - initialize the data for each 
                    invoiceData = new InvoiceDetailsPreview();
                }
            }

            else if (invoicePreview == (int)InvoicePreview.SimpleInvoice)
            {
                invoiceRepoData = invoiceDetailList.FirstOrDefault();

                //invoice details map
                _invoicemap.InvoiceDataMap(invoiceData, invoiceRepoData, billedContactsList, extrafees);

                //booking details map
                _invoicemap.BookingDataMapForSimpleInvoice(bookingList, ProductList, totalQtyBookingList, invoiceData, serviceTypeList, QuotationNoList,
                    invoiceRepoData, factoryLocationList, productPoCountryList);

                var extraFeeamount = extrafees.Where(x => x.BilledTo == invoiceRepoData.InvoiceTo.GetValueOrDefault() && x.ExtraFee.HasValue)?.Sum(x => x.ExtraFee) ?? 0;

                //this has only one line invoice. so need to show total invoice.
                invoiceData.TotalInvoiceFees = Convert.ToString(invoiceDetailList.Where(x => x.TotalInvoiceFees.HasValue)?.Sum(y => y.TotalInvoiceFees.Value) + extraFeeamount);

                var keyInvoiceData = FrameInvoiceKeyValue(invoiceData);

                invoiceList.Add(keyInvoiceData);
            }

            //product level
            else if (invoicePreview == (int)InvoicePreview.PO)
            {
                if (entityId != (int)Company.api)
                {
                    productPoCountryList = productPoCountryList.OrderBy(x => x.ServiceDateTo).ThenBy(x => x.FactoryName).ToList();
                }
                int prevBookingId = 0;
                //loop by po level
                foreach (var poItem in productPoCountryList)
                {
                    var bookingItem = bookingList.FirstOrDefault(x => x.BookingNo == poItem.BookingId);
                    if (prevBookingId != poItem.BookingId)
                    {
                        invoiceRepoData = invoiceDetailList.FirstOrDefault(x => x.InspectionId == poItem.BookingId);

                        //invoice details map
                        _invoicemap.InvoiceDataMap(invoiceData, invoiceRepoData, billedContactsList, extrafees);

                        //booking details map
                        _invoicemap.BookingDataMap(bookingItem, ProductList, totalQtyBookingList, invoiceData, serviceTypeList,
                            QuotationNoList, invoiceRepoData, factoryLocationList, productPoCountryList);

                        int extraFeeId = extrafees.Where(x => x.BookingId == poItem.BookingId && x.BilledTo == invoiceRepoData.InvoiceTo).Select(x => x.ExtraFeeId).FirstOrDefault();

                        var extratypeList = extraFeeTypeList.Where(x => x.ExtraFeeId == extraFeeId).ToList();

                        invoiceData.ExtraFeeTypeBooking = string.Join(", ", extratypeList.Select(x => x.ExtraFeeType).ToArray());
                        invoiceData.ExtraFeeRemarks = extrafees.Where(x => x.BookingId == bookingItem.BookingNo).Select(x => x.Remarks).FirstOrDefault() ?? string.Empty;

                        prevBookingId = poItem.BookingId;
                    }

                    List<InvoiceBookingProductsData> productDatas = null;
                    if (entityId != (int)Company.api)
                    {
                        productDatas = ProductList.Where(x => x.Id == poItem.Id).OrderBy(x => x.ServiceDateTo).ThenBy(x => x.FactoryName).ToList();
                    }
                    else
                    {
                        productDatas = ProductList.Where(x => x.Id == poItem.Id).OrderBy(x => x.ProductName).ToList();
                    }
                    foreach (var productItem in productDatas)
                    {
                        invoiceData.ProductRef = productItem?.ProductName ?? string.Empty;
                        invoiceData.ProductDesc = productItem?.ProductDescription ?? string.Empty;

                        //category
                        invoiceData.CustomerProductCategory = productItem?.ProductCategory ?? string.Empty;
                        invoiceData.ProductSubCategory = productItem?.ProductSubCategory ?? string.Empty;
                        invoiceData.ProductSubCategory2 = productItem?.ProductSubCategory2 ?? string.Empty;

                        //po booking qty
                        invoiceData.ProductBookingQty = poItem?.POBookingQty.ToString() ?? string.Empty;

                        invoiceData.ProductBarCode = productItem?.ProductBarCode ?? string.Empty;
                        invoiceData.ETD = poItem?.ETD?.ToString(StandardDateFormat) ?? string.Empty;
                        invoiceData.DestinationCountry = poItem?.DestinationCountry ?? string.Empty;
                        invoiceData.ProductPO = poItem?.PoNumber ?? string.Empty;

                        invoiceData.CustomerProductCategory = productItem?.ProductCategory ?? string.Empty;
                        invoiceData.FactoryReference = productItem?.FactoryReference ?? string.Empty;

                        invoiceData.Colors = string.Join(", ", bookingItem?.InspPurchaseOrderColors?.Where(x => x.ProductRefId == poItem.Id).Select(y => y.ColorName).Distinct().ToList()) ?? string.Empty;
                        invoiceData.ReportResult = string.Join(", ", bookingItem?.ReportResults?.Where(x => x.ProductRefId == poItem.Id).Select(y => y.Result).Distinct().ToList()) ?? string.Empty;
                        var fbBookingQtyDetails = fbBookingQty?.FirstOrDefault(x => poItem?.PoTranId == x.InspPOTransId);
                        invoiceData.ProductInspectedQty = fbBookingQtyDetails?.InspectedQty?.ToString() ?? string.Empty;
                        invoiceData.ProductPresentedQty = fbBookingQtyDetails?.PresentedQty?.ToString() ?? string.Empty;
                        invoiceData.InvoiceCode = bookingDFData.Where(x => x.BookingNo == bookingItem.BookingNo && x.ControlConfigurationId == InvoiceCodeControlConfigurationId).Select(x => x.DFValue).FirstOrDefault();

                        var keyInvoiceData = FrameInvoiceKeyValue(invoiceData);

                        invoiceList.Add(keyInvoiceData);

                        //after added to list - initialize the data for each 
                        invoiceData = new InvoiceDetailsPreview();
                    }
                }
            }
            else if (invoicePreview == (int)InvoicePreview.InvoiceExtraFee)
            {
                var bookingItem = bookingList.FirstOrDefault();
                invoiceRepoData = invoiceDetailList.FirstOrDefault();
                var billedto = invoiceRepoData.InvoiceTo.GetValueOrDefault();

                var extrafeeContactsList = await _repo.GetExtraFeeBilledContacts(new List<int>() { invoiceRepoData.Id });

                _invoicemap.ExtraFeeMap(invoiceData, invoiceRepoData, extrafees, bookingItem, extrafeeContactsList);

                if (invoiceRepoData.ServiceId == (int)Service.AuditId)
                {
                    //audit booking details map
                    _invoicemap.AuditDataMap(bookingItem, ProductList, totalQtyBookingList, invoiceData, serviceTypeList, QuotationNoList,
                        invoiceRepoData, factoryLocationList, productPoCountryList, audTranFaProfiles);
                }
                else if (invoiceRepoData.ServiceId == (int)Service.InspectionId)
                {
                    //insp booking details map
                    _invoicemap.BookingDataMap(bookingItem, ProductList, totalQtyBookingList, invoiceData, serviceTypeList, QuotationNoList,
                        invoiceRepoData, factoryLocationList, productPoCountryList);

                    invoiceData.InvoiceCode = bookingDFData?.Where(x => x.BookingNo == bookingItem.BookingNo && x.ControlConfigurationId == InvoiceCodeControlConfigurationId).Select(x => x.DFValue).FirstOrDefault();
                }

                int extraFeeId = extrafees.Where(x => x.BookingId == bookingItem.BookingNo && x.BilledTo == invoiceRepoData.InvoiceTo &&
                                    x.ServiceId == invoiceRepoData.ServiceId).Select(x => x.ExtraFeeId).FirstOrDefault();

                var extratypeList = extraFeeTypeList.Where(x => x.ExtraFeeId == extraFeeId).ToList();

                invoiceData.ExtraFeeTypeBooking = string.Join(", ", extratypeList.Select(x => x.ExtraFeeType).ToArray());
                invoiceData.ExtraFeeRemarks = extrafees.Where(x => x.BookingId == bookingItem.BookingNo).Select(x => x.Remarks).FirstOrDefault() ?? string.Empty;


                invoiceData.ReportResult = bookingItem?.ReportResults != null ? string.Join(", ", bookingItem?.ReportResults?.Select(y => y.Result).Distinct().ToList()) : string.Empty;

                foreach (var extraTypeItem in extratypeList)
                {
                    invoiceData.ExtraFeeTypeRemarks = extraTypeItem.Remarks ?? string.Empty;
                    invoiceData.ExtraFeeType = extraTypeItem.ExtraFeeType ?? string.Empty;

                    //if exchange rate available then multiply with it.
                    if (extraTypeItem.ExtraFee != null && extraTypeItem.ExchangeRate != null)
                    {
                        invoiceData.ExtraFee = Convert.ToString(extraTypeItem.ExtraFee * extraTypeItem.ExchangeRate);
                    }
                    else
                    {
                        invoiceData.ExtraFee = extraTypeItem.ExtraFee?.ToString() ?? string.Empty;
                    }

                    var keyInvoiceData = FrameInvoiceKeyValue(invoiceData);

                    invoiceList.Add(keyInvoiceData);

                    //after added to list - initialize the data for each 
                    invoiceData = new InvoiceDetailsPreview();
                }
            }

            else if (invoicePreview == (int)InvoicePreview.Audit)
            {
                //loop by booking level
                foreach (var bookingItem in bookingList)
                {
                    invoiceRepoData = invoiceDetailList.FirstOrDefault(x => x.AuditId == bookingItem.BookingNo);

                    //invoice details map
                    _invoicemap.InvoiceDataMap(invoiceData, invoiceRepoData, billedContactsList, extrafees);

                    //booking details map
                    _invoicemap.AuditDataMap(bookingItem, ProductList, totalQtyBookingList, invoiceData, serviceTypeList, QuotationNoList,
                        invoiceRepoData, factoryLocationList, productPoCountryList, audTranFaProfiles);

                    int extraFeeId = extrafees.Where(x => x.BookingId == bookingItem.BookingNo && x.BilledTo == invoiceRepoData.InvoiceTo && x.ServiceId == (int)Service.AuditId)
                        .Select(x => x.ExtraFeeId).FirstOrDefault();

                    var extratypeList = extraFeeTypeList.Where(x => x.ExtraFeeId == extraFeeId).ToList();

                    invoiceData.ExtraFeeTypeBooking = string.Join(", ", extratypeList.Select(x => x.ExtraFeeType).ToArray());

                    invoiceData.ExtraFeeRemarks = extrafees.Where(x => x.BookingId == bookingItem.BookingNo).Select(x => x.Remarks).FirstOrDefault() ?? string.Empty;



                    var keyInvoiceData = FrameInvoiceKeyValue(invoiceData);

                    invoiceList.Add(keyInvoiceData);

                    //after added to list - initialize the data for each 
                    invoiceData = new InvoiceDetailsPreview();
                }
            }
            else if (invoicePreview == (int)InvoicePreview.ManualInvoice)
            {
                foreach (var invoiceDetails in invoiceDetailList)
                {
                    _invoicemap.InvoiceDataMap(invoiceData, invoiceDetails, null, null);
                    invoiceData.ServiceDateFromToDate = invoiceDetails.ServiceFromDate?.ToString(StandardDateFormat) + "-" + invoiceDetails.ServiceToDate?.ToString(StandardDateFormat);
                    invoiceData.TotalInvoiceFees = Convert.ToString(invoiceDetails.TotalInvoiceFees + (invoiceDetails.TotalInvoiceFees * invoiceDetails.TaxValue));
                    invoiceData.Country = invoiceDetails.Country;
                    invoiceData.Attention = invoiceDetails.Attention;
                    var keyInvoiceData = FrameInvoiceKeyValue(invoiceData);

                    invoiceList.Add(keyInvoiceData);
                }
            }
            else if (invoicePreview == (int)InvoicePreview.CreditNote)
            {
                var bookingIds = bookingList.Select(x => x.BookingNo).ToList();
                var bookingContactList = await _bookingRepository.GetBookingCustomerContacts(bookingIds);
                var merchandiserList = await _bookingRepository.GetMerchandiserContactsByBookingIds(bookingIds);
                var scheduleStaffItems = await _scheduleRepository.GetQCBookingDetails(bookingIds);
                foreach (var invoiceDetails in invoiceDetailList)
                {
                    invoiceRepoData = invoiceDetailList.FirstOrDefault(x => x.InspectionId == invoiceDetails.InspectionId);
                    var bookingItem = bookingList.FirstOrDefault(x => x.BookingNo == invoiceDetails.InspectionId);

                    //invoice details map
                    _invoicemap.InvoiceDataMap(invoiceData, invoiceRepoData, billedContactsList, extrafees);

                    //booking details map
                    _invoicemap.BookingDataMap(bookingItem, ProductList, totalQtyBookingList, invoiceData, serviceTypeList, QuotationNoList,
                        invoiceRepoData, factoryLocationList, productPoCountryList);

                    int extraFeeId = extrafees.Where(x => x.BookingId == bookingItem.BookingNo && x.BilledTo == invoiceRepoData.InvoiceTo).Select(x => x.ExtraFeeId).FirstOrDefault();

                    var extratypeList = extraFeeTypeList.Where(x => x.ExtraFeeId == extraFeeId).ToList();

                    invoiceData.ExtraFeeTypeBooking = string.Join(", ", extratypeList.Select(x => x.ExtraFeeType).ToArray());

                    invoiceData.ExtraFeeRemarks = extrafees.Where(x => x.BookingId == bookingItem.BookingNo).Select(x => x.Remarks).FirstOrDefault() ?? string.Empty;
                    invoiceData.Colors = string.Concat(", ", bookingItem?.InspPurchaseOrderColors?.Select(y => y.ColorName).Distinct().ToList());
                    invoiceData.ReportResult = string.Join(", ", bookingItem?.ReportResults?.Select(y => y.Result).Distinct().ToList()) ?? string.Empty;
                    var keyInvoiceData = FrameInvoiceKeyValue(invoiceData);

                    invoiceList.Add(keyInvoiceData);

                    //after added to list - initialize the data for each 
                    invoiceData = new InvoiceDetailsPreview();
                }
            }
            return invoiceList;
        }

        public async Task<SaveInvoicePdfResponse> SaveInvoicePdfUrl(SaveInvoicePdfUrl invoicePdfUrl)
        {
            //fetch the invoice from invocie no
            var invoice = await _invoiceRepository.GetInvoiceDetailsbyInvoiceNumber(invoicePdfUrl.InvoiceNo);
            if (invoice == null)
                return new SaveInvoicePdfResponse() { Result = SaveInvoicePdfResult.InvoiceNumberNotFound };

            //get the invoice transaction file list
            var invoiceFiles = await _invoiceRepository.GetInvoiceTransactionFiles(invoice.InvoiceId, (int)InvRefFileTypeEnum.InvoicePreview);
            //deactive the existing invoice preview file
            if (invoiceFiles != null && invoiceFiles.Any())
            {
                invoiceFiles.ForEach(x =>
                {
                    x.Active = false;
                    x.DeletedBy = invoicePdfUrl.CreatedBy;
                    x.DeletedOn = DateTime.Now;
                });

                _invoiceRepository.EditEntities(invoiceFiles);
            }


            //map the request to entity
            var invTranFile = _invoicemap.MapInvoiceTranFile(invoicePdfUrl, invoice.InvoiceId);
            //add invoice transaction file
            _invoiceRepository.AddEntity(invTranFile);

            await _invoiceRepository.Save();
            return new SaveInvoicePdfResponse() { Result = SaveInvoicePdfResult.Success };
        }

        public async Task<InvoiceDownloadResponse> GetInvoicePreviewFile(string invoiceNo, string invoiceFileName)
        {
            // validate the invoice no and decrypt the invoice no and check 
            string decryptedInvoiceNo = null;
            if (string.IsNullOrEmpty(invoiceNo))
                return new InvoiceDownloadResponse() { Error = InvoiceDownloadErrorMessages.InvoiceNoRequired, Result = InvoiceDownloadResult.InvoiceNoRequired };
            else
                decryptedInvoiceNo = EncryptionDecryption.DecryptStringAES(invoiceNo);

            if (string.IsNullOrWhiteSpace(decryptedInvoiceNo))
                return new InvoiceDownloadResponse() { Error = InvoiceDownloadErrorMessages.InvoiceNoRequired, Result = InvoiceDownloadResult.InvoiceNoRequired };
            //

            //if invoice file name null then the set invoice no as file name 
            string decryptedInvoiceFileName = null;
            if (!string.IsNullOrWhiteSpace(invoiceFileName))
                decryptedInvoiceFileName = EncryptionDecryption.DecryptStringAES(invoiceFileName);

            var invoice = await _invoiceRepository.GetInvoiceDetailsbyInvoiceNumber(decryptedInvoiceNo);
            if (invoice == null)
                return new InvoiceDownloadResponse() { Error = InvoiceDownloadErrorMessages.InvoiceNotFound, Result = InvoiceDownloadResult.InvoiceNotFound };

            var invoiceFiles = await _invoiceRepository.GetInvoiceTransactionFiles(invoice.InvoiceId, (int)InvRefFileTypeEnum.InvoicePreview);
            if (!invoiceFiles.Any())
            {
                return new InvoiceDownloadResponse() { Error = InvoiceDownloadErrorMessages.InvoiceFileNotFound, Result = InvoiceDownloadResult.InvoicePdfNotAvailable };
            }
            var invoiceUrl = invoiceFiles.FirstOrDefault()?.FilePath;
            var httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, invoiceUrl, null, null);

            var mimeType = _fileManager.GetMimeTypeByUrl(invoiceUrl);
            var extension = _fileManager.GetExtension(mimeType);

            return new InvoiceDownloadResponse()
            {
                Invoice = await httpResponse.Content.ReadAsByteArrayAsync(),
                FileName = (!string.IsNullOrWhiteSpace(decryptedInvoiceFileName) ? decryptedInvoiceFileName : decryptedInvoiceNo) + "." + extension,  //if invoice file name null then the set invoice no as file name
                Result = InvoiceDownloadResult.Success
            };
        }
    }
}
