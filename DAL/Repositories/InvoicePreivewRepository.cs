using Contracts.Repositories;
using DTO.ExtraFees;
using DTO.Invoice;
using DTO.InvoicePreview;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class InvoicePreivewRepository : Repository, IInvoicePreivewRepository
    {
        public InvoicePreivewRepository(API_DBContext context) : base(context)
        {

        }

        //get all bank details with tax information
        public async Task<List<InvoiceBankRepo>> GetBankDetails()
        {
            return await _context.InvRefBanks.Where(x => x.Active.Value)
                .Select(x => new InvoiceBankRepo
                {
                    AccountName = x.AccountName,
                    AccountNumber = x.AccountNumber,
                    ChopLink = x.ChopFileUrl,
                    SignLink = x.SignatureFileUrl,
                    Id = x.Id
                }).ToListAsync();
        }

        public async Task<List<InvoiceBankTaxRepo>> GetBankTaxDetails(List<int> BankIdList)
        {
            return await _context.InvTranBankTaxes.Where(x => x.Active && BankIdList.Contains(x.BankId))

                    .Select(y => new InvoiceBankTaxRepo
                    {
                        TaxId = y.Id,
                        BankId = y.BankId,
                        FromDate = y.FromDate,
                        Name = y.TaxName,
                        Value = y.TaxValue,
                        ToDate = y.ToDate
                    }).AsNoTracking().ToListAsync();
        }
        //get extra fee details 
        public async Task<List<InvoiceDetailsRepo>> GetExtraFeeInvoiceDetails(string invoiceNumber)
        {
            return await _context.InvExfTransactions.Where(x => x.ExtraFeeInvoiceNo == invoiceNumber
                        && x.StatusId != (int)ExtraFeeStatus.Cancelled)
                .Select(x => new InvoiceDetailsRepo
                {
                    InspectionId = x.InspectionId,
                    ServiceId = x.ServiceId,
                    Id = x.Id,
                    InvoiceTo = x.BilledTo,
                    BilledName = x.BilledName,
                    BilledAddress = x.BilledAddress,
                    PaymentTerm = x.PaymentTerms,
                    PaymentDurations = x.PaymentDuration,
                    InvoiceNumber = x.ExtraFeeInvoiceNo,
                    Currency = x.InvoiceCurrency.CurrencyCodeA,
                    InvoiceDate = x.ExtraFeeInvoiceDate,
                    AuditId = x.AuditId,
                    CreatedOn = x.CreatedOn
                }).ToListAsync();
        }

        //get invoice details by invoice number
        public async Task<List<InvoiceDetailsRepo>> GetInvoiceDetails(string invoiceNumber)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceNo == invoiceNumber &&
                                                          x.InvoiceStatus != (int)InvoiceStatus.Cancelled)
                .Select(x => new InvoiceDetailsRepo
                {
                    InvoiceDate = x.InvoiceDate,
                    InvoiceNumber = x.InvoiceNo,
                    ManDay = x.ManDays,
                    PaymentDuration = x.PaymentDuration,
                    PaymentTerm = x.PaymentTerms,
                    PostDate = x.PostedDate,
                    Remarks = x.Remarks,
                    Subject = x.Subject,
                    Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    InspectionId = x.InspectionId,
                    AuditId = x.AuditId,
                    ServiceId = x.ServiceId,
                    BilledAddress = x.InvoicedAddress,
                    BilledMethod = x.InvoiceMethod,
                    BilledName = x.InvoicedName,

                    UnitPrice = x.UnitPrice,
                    TaxValue = x.TaxValue,
                    TravelOtherFees = x.TravelOtherFees,
                    TotalTravelFee = x.TravelTotalFees,
                    TotalInvoiceFees = x.TotalInvoiceFees,
                    OtherCost = x.OtherFees,
                    LandCost = x.TravelLandFees,
                    AirCost = x.TravelAirFees,
                    Discount = x.Discount,
                    HotelCost = x.HotelFees,
                    InspFee = x.InspectionFees,

                    OfficeAddress = x.OfficeNavigation.Address,
                    OfficeFax = x.OfficeNavigation.Fax,
                    OfficeMail = x.OfficeNavigation.Mail,
                    OfficeName = x.OfficeNavigation.Name,
                    OfficePhone = x.OfficeNavigation.Phone,
                    OfficeWebsite = x.OfficeNavigation.Website,

                    AccountId = x.Bank.Id,
                    AccountNumber = x.Bank.AccountNumber,
                    BankAddress = x.Bank.BankAddress,
                    BankName = x.Bank.BankName,
                    BankSwiftCode = x.Bank.SwiftCode,
                    AccountName = x.Bank.AccountName,
                    Id = x.Id,
                    InvoiceTo = x.InvoiceTo,
                    CreatedOn = x.CreatedOn
                }).ToListAsync();
        }

        public async Task<List<InvoiceTaxData>> GetInvoiceTaxDetails(List<int> InvoiceIds)
        {
            return await _context.InvAutTranTaxes.Where(x => InvoiceIds.Contains(x.InvoiceId.GetValueOrDefault()))
                .Select(x => new InvoiceTaxData
                {
                    InvoiceId = x.InvoiceId,
                    TaxId = x.TaxId
                }).ToListAsync();
        }

        //get booking details based on invoice booking ids
        public async Task<List<InvoiceBookingPDFDetail>> GetInvoiceBookingData(IEnumerable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id))
                    .Select(x => new InvoiceBookingPDFDetail
                    {
                        CustomerName = x.Customer.CustomerName,
                        customerid = x.CustomerId,
                        FactoryName = x.Factory.SupplierName,
                        factoryid = x.FactoryId,
                        SupplierName = x.Supplier.SupplierName,
                        SupplierCode = x.Supplier.SuSupplierCustomers.Select(y => y.Code).FirstOrDefault(),
                        supplierid = x.SupplierId,
                        BookingNo = x.Id,
                        FactoryId = x.FactoryId,
                        ServiceDateFrom = x.ServiceDateFrom,
                        ServiceDateTo = x.ServiceDateTo,
                        CustomerCollection = x.Collection.Name,
                        CustomerPriceCategory = x.PriceCategory.Name,
                        CustomerBookingNo = x.CustomerBookingNo,
                        InspectionLocation = x.InspectionLocationNavigation.Name,
                        Season = x.Season.Season.Name,
                        SeasonYear = x.SeasonYear.Year
                    }).AsNoTracking().OrderBy(x => x.ServiceDateTo).ThenBy(x => x.FactoryName).ToListAsync();
        }

        /// <summary>
        /// get invoice audit data
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InvoiceBookingPDFDetail>> GetInvoiceAuditBookingData(IEnumerable<int> auditIds)
        {
            return await _context.AudTransactions.Where(x => auditIds.Contains(x.Id))
                    .Select(x => new InvoiceBookingPDFDetail
                    {
                        CustomerName = x.Customer.CustomerName,
                        FactoryName = x.Factory.SupplierName,
                        SupplierName = x.Supplier.SupplierName,
                        SupplierCode = x.Supplier.SuSupplierCustomers.Select(y => y.Code).FirstOrDefault(),
                        BookingNo = x.Id,
                        FactoryId = x.FactoryId,
                        ServiceDateFrom = x.ServiceDateFrom,
                        ServiceDateTo = x.ServiceDateTo,
                        CustomerDept = x.Department.Name,
                        Brand = x.Brand.Name
                    }).AsNoTracking().OrderBy(x => x.ServiceDateTo).ThenBy(x => x.FactoryName).ToListAsync();
        }

        //get billed contact invoice
        public async Task<List<BilledContactsName>> GetInvoiceBilledContacts(IEnumerable<int> invoiceIds)
        {
            return await _context.InvAutTranContactDetails.Where(x => invoiceIds.Contains((int)x.InvoiceId))
                .Select(x => new BilledContactsName
                {
                    CustomerContactName = x.CustomerContact.ContactName,
                    SupplierContactName = x.SupplierContact.ContactName,
                    FactoryContactName = x.FactoryContact.ContactName,
                    InvoiceId = x.InvoiceId
                }).ToListAsync();
        }

        //get extra fee contacts
        public async Task<List<BilledContactsName>> GetExtraFeeBilledContacts(IEnumerable<int> ExtraFeeIds)
        {
            return await _context.InvExfContactDetails.Where(x => ExtraFeeIds.Contains((int)x.ExtraFeeId))
                .Select(x => new BilledContactsName
                {
                    CustomerContactName = x.CustomerContact.ContactName,
                    SupplierContactName = x.SupplierContact.ContactName,
                    FactoryContactName = x.FactoryContact.ContactName,
                    InvoiceId = x.Id
                }).ToListAsync();
        }

        //get prdouct details based on booking ids
        public async Task<List<InvoiceBookingProductsData>> GetBookingProductData(IEnumerable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active.Value)
                      .Select(x => new InvoiceBookingProductsData
                      {
                          BookingId = x.InspectionId,
                          Id = x.ProductRefId,
                          PoNumber = x.Po.Pono,
                          DestinationCountry = x.DestinationCountry.CountryName,
                          POBookingQty = x.BookingQuantity,
                          ETD = x.Etd,
                          POId = x.PoId,
                          PoTranId = x.Id,
                          ProductName = x.ProductRef.Product.ProductId,
                          FactoryName = x.Inspection.Factory.SupplierName,
                          ServiceDateTo = x.Inspection.ServiceDateTo
                      }).OrderBy(x => x.PoNumber).ThenBy(x => x.ProductName).ToListAsync();
        }

        /// <summary>
        /// fetch the extra fee for bookings
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ExtraFeeData>> GetExtraFeeByBooking(List<int> bookingIds)
        {
            return await _context.InvExfTransactions.Where(x => x.StatusId != (int)ExtraFeeStatus.Cancelled && x.Active.Value && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
               .Select(x => new ExtraFeeData
               {
                   BookingId = x.InspectionId,
                   InvoiceId = x.InvoiceId,
                   ExtraFee = x.TotalExtraFee,
                   BilledTo = x.BilledTo,
                   BilledAddress = x.BilledAddress,
                   PaymentTerms = x.PaymentTerms,
                   Remarks = x.Remarks,
                   ExtraFeeId = x.Id,
                   ExtraFeeCurrency = x.Currency.CurrencyCodeA,
                   BankId = x.BankId,
                   BankName = x.Bank.BankName,
                   BankAddress = x.Bank.BankAddress,
                   BankSwiftCode = x.Bank.SwiftCode,
                   AccountNumber = x.Bank.AccountNumber,
                   AccountName = x.Bank.AccountName,
                   OfficeAddress = x.Office.Address,
                   OfficeFax = x.Office.Fax,
                   OfficeMail = x.Office.Mail,
                   OfficeName = x.Office.Name,
                   OfficePhone = x.Office.Phone,
                   OfficeWebsite = x.Office.Website,
                   ServiceId = x.ServiceId
               }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ExtraFeesTaxData>> GetExtraFeesBankTaxList(List<int> extraFeeIds)
        {
            return await _context.InvExtTranTaxes.Where(x => extraFeeIds.Contains(x.ExtraFeeId.GetValueOrDefault()))
               .Select(x => new ExtraFeesTaxData
               {
                   ExtraFeeId = x.ExtraFeeId,
                   TaxId = x.TaxId
               }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// extarfess by booking id
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ExtraFeeData>> GetExtraFeeByAuditBooking(List<int> bookingIds)
        {
            return await _context.InvExfTransactions.Where(x => x.StatusId != (int)ExtraFeeStatus.Cancelled && x.Active.Value && bookingIds.Contains(x.AuditId.GetValueOrDefault()))
               .Select(x => new ExtraFeeData
               {
                   BookingId = x.AuditId,
                   InvoiceId = x.InvoiceId,
                   ExtraFee = x.TotalExtraFee,
                   BilledTo = x.BilledTo,
                   Remarks = x.Remarks,
                   ExtraFeeId = x.Id,
                   ExtraFeeCurrency = x.Currency.CurrencyName,
                   BankId = x.BankId,
                   BankName = x.Bank.BankName,
                   BankAddress = x.Bank.BankAddress,
                   BankSwiftCode = x.Bank.SwiftCode,
                   AccountNumber = x.Bank.AccountNumber,
                   AccountName = x.Bank.AccountName,
                   ServiceId = x.ServiceId
               }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get extra fee type details
        /// </summary>
        /// <param name="extraFeeIds"></param>
        /// <returns></returns>
        public async Task<List<ExtraFeeTypeData>> GetExtraFeeTypeByBooking(IEnumerable<int> extraFeeIds)
        {
            return await _context.InvExfTranDetails.Where(x => x.Active.Value &&
                    extraFeeIds.Contains(x.ExftransactionId.GetValueOrDefault()))
               .Select(x => new ExtraFeeTypeData
               {
                   ExtraFee = x.ExtraFees,
                   ExtraFeeType = x.ExtraFeeTypeNavigation.Name,
                   Remarks = x.Remarks,
                   ExtraFeeId = x.ExftransactionId,
                   ExchangeRate = x.Exftransaction.ExchangeRate
               }).ToListAsync();
        }

        /// <summary>
        /// Get the Shipment quantity from FB 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingQuantity>> GetInspectionQuantities(List<int> bookingIds)
        {
            return await _context.FbReportQuantityDetails
                .Where(x => bookingIds.Contains(x.InspPoTransaction.InspectionId) && x.Active.Value)
                .Select(x => new BookingQuantity
                {
                    BookingId = x.InspPoTransaction.InspectionId,
                    InspectedQty = x.InspectedQuantity,
                    PresentedQty = x.PresentedQuantity,
                    InspPOTransId = x.InspPoTransactionId
                }).ToListAsync();
        }
    }
}
