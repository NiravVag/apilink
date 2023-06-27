using Contracts.Repositories;
using DAL.Helper;
using DTO.CommonClass;
using DTO.ExtraFees;
using DTO.Invoice;
using DTO.Kpi;
using DTO.Quotation;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class KpiCustomRepository : Repository, IKpiCustomRepository
    {
        private static IConfiguration _configuration = null;

        public KpiCustomRepository(API_DBContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        //Get the Template List
        public IQueryable<RefKpiTeamplate> GetTemplateList()
        {
            return _context.RefKpiTeamplates.Where(x => x.Active);
        }

        public IQueryable<KpiInspectionBookingItems> GetAllInspections()
        {
            return _context.InspTransactions
                .Select(x => new KpiInspectionBookingItems
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
                    ApplyDate = x.CreatedOn.GetValueOrDefault(),
                    Status = x.Status,
                    Customer = x.Customer,
                    CustomerBookingNo = x.CustomerBookingNo,
                    BookingAPiRemarks = x.ApiBookingComments,
                    IsPicking = x.IsPickingRequired.GetValueOrDefault(),
                    InspectionType = x.ReInspectionType,
                    CollectionName = x.Collection.Name,
                    PriceCategory = x.PriceCategory.Name,
                    Season = x.Season.Season.Name,
                    SeasonYear = x.SeasonYear.Year,
                    CreatedBy = x.CreatedByNavigation.FullName,
                    InspectionLocation = x.InspectionLocationNavigation.Name,
                    ShipmentDate = x.ShipmentDate,
                    BookingType = x.BookingTypeNavigation.Name,
                    GapPaymentOption = x.PaymentOptionsNavigation.Name
                }).OrderByDescending(x => x.ServiceDateTo);
        }


        public async Task<List<KpiInspectionBookingItems>> GetBookingItemsbyBookingIdAsQuery(IQueryable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id))
                .Select(x => new KpiInspectionBookingItems
                {
                    BookingId = x.Id,
                    CustomerId = x.CustomerId,
                    SupplierId = x.SupplierId,
                    FactoryId = x.FactoryId,
                    CustomerName = x.Customer.CustomerName,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    FactoryCountry = x.Factory.SuAddresses.FirstOrDefault().Country.CountryName,
                    FactoryProvince = x.Factory.SuAddresses.FirstOrDefault().Region.ProvinceName,
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
                    ApplyDate = x.CreatedOn.GetValueOrDefault(),
                    Status = x.Status,
                    Customer = x.Customer,
                    CustomerBookingNo = x.CustomerBookingNo,
                    BookingAPiRemarks = x.ApiBookingComments,
                    IsPicking = x.IsPickingRequired.GetValueOrDefault(),
                    InspectionType = x.ReInspectionType,
                    CollectionName = x.Collection.Name,
                    PriceCategoryId = x.PriceCategoryId,
                    PriceCategory = x.PriceCategory.Name,
                    PreviousBookingNo = x.PreviousBookingNo,
                    Season = x.Season.Season.Name,
                    SeasonYear = x.SeasonYear.Year,
                    CustomerProductCategory = x.CuProductCategoryNavigation.Name
                }).OrderByDescending(x => x.ServiceDateTo).ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetXeroxInvoiceFirstSetItems(IQueryable<int> bookingIds, List<int> invoiceTypeList)
        {
            return await _context.InvAutTranDetails.Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault())
            && invoiceTypeList.Contains(x.InvoiceType.GetValueOrDefault())
            && x.InvoiceStatus != (int)InvoiceStatus.Cancelled && x.InspectionFees.GetValueOrDefault() > 0)
                .Select(x => new XeroInvoiceData
                {
                    ContactName = x.InvoicedName,
                    EmailAddress = "",
                    POAddressLine1 = "",
                    POAddressLine2 = "",
                    POAddressLine3 = "",
                    POAddressLine4 = "",
                    POCity = "",
                    PORegion = "",
                    POPostalCode = "",
                    POCountry = "",
                    InvoiceNumber = x.InvoiceNo,
                    Reference = x.Inspection.Customer.CustomerName,
                    InvoiceDate = x.PostedDate != null ? x.PostedDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    DueDate = x.InvoiceDate != null ? x.PaymentDuration != null
                    && x.PaymentDuration != "" ?
                    x.InvoiceDate.GetValueOrDefault().AddDays(int.Parse(x.PaymentDuration)).ToString(StandardDateFormat)
                    : x.InvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    InventoryItemCode = "",
                    Description = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                    Quantity = 1,
                    UnitAmount = x.InspectionFees.GetValueOrDefault() + x.InvExfTransactions.Sum(y => y.ExtraFeeSubTotal.GetValueOrDefault()),
                    Discount = x.Discount.GetValueOrDefault(),
                    AccountCode = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).
                                  ServiceType.RefServiceTypeXeros.FirstOrDefault().XeroAccount,

                    TaxType = x.TaxValue.GetValueOrDefault().ToString() != "" ?
                    x.Bank.TaxNameInXero.Replace("0", (x.TaxValue.GetValueOrDefault() * 100).ToString()) : "Tax Exempt (0%)",

                    TrackingName1 = "Services and Departments",
                    TrackingOption1 = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).
                                  ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionName,
                    TrackingName2 = "Location",
                    TrackingOption2 = x.Inspection.Factory.SuAddresses.
                    FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
                    Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    BrandingTheme = "",
                    AccountName = x.Bank.AccountName
                }).AsNoTracking().ToListAsync();
        }
        public async Task<List<XeroInvoiceData>> GetXeroxInvoiceSecondSetItems(IQueryable<int> bookingIds, List<int> invoiceTypeList)
        {
            return await _context.InvAutTranDetails.Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault())
            && invoiceTypeList.Contains(x.InvoiceType.GetValueOrDefault())
            && x.InvoiceStatus != (int)InvoiceStatus.Cancelled && (x.TravelTotalFees.GetValueOrDefault() > 0 || x.HotelFees.GetValueOrDefault() > 0))
                .Select(x => new XeroInvoiceData
                {
                    ContactName = x.InvoicedName,
                    EmailAddress = "",
                    POAddressLine1 = "",
                    POAddressLine2 = "",
                    POAddressLine3 = "",
                    POAddressLine4 = "",
                    POCity = "",
                    PORegion = "",
                    POPostalCode = "",
                    POCountry = "",
                    InvoiceNumber = x.InvoiceNo,
                    Reference = x.Inspection.Customer.CustomerName,
                    InvoiceDate = x.PostedDate != null ? x.PostedDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    DueDate = x.InvoiceDate != null ? x.PaymentDuration != null
                    && x.PaymentDuration != "" ?
                    x.InvoiceDate.GetValueOrDefault().AddDays(int.Parse(x.PaymentDuration)).ToString(StandardDateFormat)
                    : x.InvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    InventoryItemCode = "",
                    Description = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                    Quantity = 1,
                    UnitAmount = x.TravelTotalFees.GetValueOrDefault() + x.HotelFees.GetValueOrDefault(),
                    Discount = x.Discount.GetValueOrDefault(),
                    AccountCode = "700100",
                    TaxType = x.TaxValue.GetValueOrDefault().ToString() != "" ?
                    x.Bank.TaxNameInXero.Replace("0", (x.TaxValue.GetValueOrDefault() * 100).ToString()) : "Tax Exempt (0%)",
                    TrackingName1 = "Services and Departments",
                    TrackingOption1 = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).
                                  ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionNameTravel,
                    TrackingName2 = "Location",
                    TrackingOption2 = x.Inspection.Factory.SuAddresses.
                    FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
                    Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    BrandingTheme = "",
                    AccountName = x.Bank.AccountName
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetXeroxInvoiceThirdSetItems(IQueryable<int> bookingIds, List<int> invoiceTypeList)
        {
            return await _context.InvAutTranDetails.Where(x =>
                 bookingIds.Contains(x.InspectionId.GetValueOrDefault()) && x.InvoiceStatus != (int)InvoiceStatus.Cancelled
                 && invoiceTypeList.Contains(x.InvoiceType.GetValueOrDefault())
                 && x.OtherFees.GetValueOrDefault() > 0)
                .Select(x => new XeroInvoiceData
                {
                    ContactName = x.InvoicedName,
                    EmailAddress = "",
                    POAddressLine1 = "",
                    POAddressLine2 = "",
                    POAddressLine3 = "",
                    POAddressLine4 = "",
                    POCity = "",
                    PORegion = "",
                    POPostalCode = "",
                    POCountry = "",
                    InvoiceNumber = x.InvoiceNo,
                    Reference = x.Inspection.Customer.CustomerName,
                    InvoiceDate = x.PostedDate != null ? x.PostedDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    DueDate = x.InvoiceDate != null ? x.PaymentDuration != null
                    && x.PaymentDuration != "" ?
                    x.InvoiceDate.GetValueOrDefault().AddDays(int.Parse(x.PaymentDuration)).ToString(StandardDateFormat)
                    : x.InvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    InventoryItemCode = "",
                    Description = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                    Quantity = 1,
                    UnitAmount = x.OtherFees.GetValueOrDefault(),
                    Discount = x.Discount.GetValueOrDefault(),
                    AccountCode = "700200",
                    TaxType = x.TaxValue.GetValueOrDefault().ToString() != "" ?
                    x.Bank.TaxNameInXero.Replace("0", (x.TaxValue.GetValueOrDefault() * 100).ToString()) : "Tax Exempt (0%)",
                    TrackingName1 = "Services and Departments",
                    TrackingOption1 = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).
                                  ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionNameTravel,
                    TrackingName2 = "Location",
                    TrackingOption2 = x.Inspection.Factory.SuAddresses.
                    FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
                    Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    BrandingTheme = "",
                    AccountName = x.Bank.AccountName
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetXeroxInvoiceFourthSetItems(IQueryable<int> bookingIds, List<int> invoiceTypeList)
        {
            return await _context.InvAutTranDetails.Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault())
            && invoiceTypeList.Contains(x.InvoiceType.GetValueOrDefault())
            && x.InvoiceStatus != (int)InvoiceStatus.Cancelled && x.TotalTaxAmount.GetValueOrDefault() > 0)
                .Select(x => new XeroInvoiceData
                {
                    ContactName = x.InvoicedName,
                    EmailAddress = "",
                    POAddressLine1 = "",
                    POAddressLine2 = "",
                    POAddressLine3 = "",
                    POAddressLine4 = "",
                    POCity = "",
                    PORegion = "",
                    POPostalCode = "",
                    POCountry = "",
                    InvoiceNumber = x.InvoiceNo,
                    Reference = x.Inspection.Customer.CustomerName,
                    InvoiceDate = x.PostedDate != null ? x.PostedDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    DueDate = x.InvoiceDate != null ? x.PaymentDuration != null
                    && x.PaymentDuration != "" ?
                    x.InvoiceDate.GetValueOrDefault().AddDays(int.Parse(x.PaymentDuration)).ToString(StandardDateFormat)
                    : x.InvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    InventoryItemCode = "",
                    Description = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                    Quantity = 1,
                    UnitAmount = x.TotalTaxAmount.GetValueOrDefault(),
                    Discount = x.Discount.GetValueOrDefault(),
                    AccountCode = "700200",
                    TaxType = x.TaxValue.GetValueOrDefault().ToString() != "" ?
                    x.Bank.TaxNameInXero.Replace("0", (x.TaxValue.GetValueOrDefault() * 100).ToString()) : "Tax Exempt (0%)",
                    TrackingName1 = "Services and Departments",
                    TrackingOption1 = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).
                                  ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionName,
                    TrackingName2 = "Location",
                    TrackingOption2 = x.Inspection.Factory.SuAddresses.
                    FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
                    Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    BrandingTheme = "",
                    AccountName = x.Bank.AccountName
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetXeroInvoiceFifthSetItems(IQueryable<int> bookingIds)
        {
            return await _context.InvExfTransactions.Where(x => x.StatusId == (int)ExtraFeeStatus.Invoiced && x.InvoiceId == null && x.Active.Value && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
                .Select(x => new XeroInvoiceData
                {
                    ContactName = x.BilledName,
                    EmailAddress = "",
                    POAddressLine1 = "",
                    POAddressLine2 = "",
                    POAddressLine3 = "",
                    POAddressLine4 = "",
                    POCity = "",
                    PORegion = "",
                    POPostalCode = "",
                    POCountry = "",
                    InvoiceNumber = x.ExtraFeeInvoiceNo,
                    Reference = x.Inspection.Customer.CustomerName,
                    InvoiceDate = x.ExtraFeeInvoiceDate != null ? x.ExtraFeeInvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    DueDate = x.ExtraFeeInvoiceDate != null ? x.PaymentDuration != null ?
                    x.ExtraFeeInvoiceDate.GetValueOrDefault().AddDays(x.PaymentDuration.GetValueOrDefault()).ToString(StandardDateFormat)
                    : x.ExtraFeeInvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                    InventoryItemCode = "",
                    Description = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                    Quantity = 1,
                    UnitAmount = x.ExtraFeeSubTotal.GetValueOrDefault(),
                    Discount = 0,
                    AccountCode = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).
                                  ServiceType.RefServiceTypeXeros.FirstOrDefault().XeroAccount,
                    TaxType = x.Tax.GetValueOrDefault().ToString() != "" ?
                    x.Bank.TaxNameInXero.Replace("0", (x.Tax.GetValueOrDefault() * 100).ToString()) : "Tax Exempt (0%)",
                    TrackingName1 = "Services and Departments",
                    TrackingOption1 = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).
                                  ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionName,
                    TrackingName2 = "Location",
                    TrackingOption2 = x.Inspection.Factory.SuAddresses.
                    FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
                    Currency = x.InvoiceCurrency.CurrencyCodeA,
                    BrandingTheme = "",
                    AccountName = x.Bank.AccountName
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetXeroExpenseItems(IQueryable<int> bookingIds)
        {
            var accountCodeList = new List<string>() { "800700", "801820", "801920" };
            return await _context.EcExpensesClaimDetais.Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault())
            && x.Active.Value && x.Expense.Active.Value && x.Amount > 0 && x.Expense.StatusId != (int)ExpenseClaimStatus.Cancelled)
                .Select(x => new XeroInvoiceData
                {
                    ContactName = x.Expense.Staff.PersonName,
                    EmailAddress = x.Expense.Staff.CompanyEmail,
                    POAddressLine1 = "",
                    POAddressLine2 = "",
                    POAddressLine3 = "",
                    POAddressLine4 = "",
                    POCity = "",
                    PORegion = "",
                    POPostalCode = "",
                    POCountry = "",
                    InvoiceNumber = x.Expense.ClaimNo,
                    Reference = "",
                    InvoiceDate = x.ExpenseDate.ToString(StandardDateFormat),
                    DueDate = null,
                    InventoryItemCode = "",
                    Description = x.Inspection.Customer.CustomerName != "" ?
                                    x.ExpenseType.Description + "-" + x.Inspection.Customer.CustomerName
                                    : x.ExpenseType.Description,
                    Quantity = 1,
                    UnitAmount = x.Amount,
                    Discount = 0,
                    AccountCode = x.Expense.Staff.EmployeeTypeId == (int)EmployeeTypeEnum.Permanent ? x.ExpenseType.XeroAccountCode : x.ExpenseType.XeroOutSourceAccountCode,
                    TaxType = "Tax Exempt (0%)",
                    TrackingName1 = "Services and Departments",
                    TrackingOption1 = accountCodeList.Contains(x.ExpenseType.XeroAccountCode)
                                      && x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).
                                      ServiceType.RefServiceTypeXeros.FirstOrDefault().Id > 0 ?
                                      x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).
                                      ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionNameTravel : x.Expense.Staff.Department.DepartmentName,
                    TrackingName2 = "Location",
                    TrackingOption2 = x.Expense.Country.CountryName,
                    Currency = x.Currency.CurrencyCodeA,
                    BrandingTheme = "",
                    AccountName = x.Expense.Staff.PayrollCompanyNavigation.CompanyName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Booking details with eco pack enabled
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<KpiInspectionBookingItems>> GetBookingEcoPackbyBookingQuery(IQueryable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => x.StatusId != (int)BookingStatus.Cancel && bookingIds.Contains(x.Id)
                                            && x.InspProductTransactions.Any(y => y.Active.Value && y.IsEcopack.Value))
                .Select(x => new KpiInspectionBookingItems
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
                    ApplyDate = x.CreatedOn.GetValueOrDefault(),
                    Status = x.Status,
                    Customer = x.Customer,
                    CustomerBookingNo = x.CustomerBookingNo,
                    BookingAPiRemarks = x.ApiBookingComments,
                    IsPicking = x.IsPickingRequired.GetValueOrDefault(),
                    InspectionType = x.ReInspectionType,
                    CollectionName = x.Collection.Name,
                    PriceCategory = x.PriceCategory.Name

                }).OrderByDescending(x => x.ServiceDateTo).ToListAsync();
        }

        /// <summary>
        /// GetAllInspection as  Query
        /// </summary>
        /// <returns></returns>
        public IQueryable<InspTransaction> GetAllInspectionQuery()
        {
            return _context.InspTransactions
                   .OrderByDescending(x => x.ServiceDateTo);
        }

        public async Task<List<KpiBookingProductsData>> GetProductListByBooking(IEnumerable<int> bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new KpiBookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductName = z.Product.ProductId,
                ProductId = z.ProductId,
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.TotalBookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.Product.ProductCategorySub2Navigation.Name,
                CreatedDate = z.Inspection.CreatedOn,
                ServiceStartDate = z.FbReport.ServiceFromDate,
                ServiceEndDate = z.FbReport.ServiceToDate,
                UpdatedDate = z.Inspection.UpdatedOn,
                CombineProductId = z.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault(),
                FactoryReference = z.Product.FactoryReference,
                AqlQty = z.AqlQuantity.GetValueOrDefault(),
                ReportId = z.FbReportId,
                AQLName = z.AqlNavigation.Value,
                ReportResult = z.FbReport.OverAllResult,
                Barcode = z.Product.Barcode,
                ReportResultId = z.FbReport.ResultId,
                CriticalMax = z.FbReport.CriticalMax,
                MajorMax = z.FbReport.MajorMax,
                MinorMax = z.FbReport.MinorMax,
                FBReportDetailId = z.FbReport.Id,
                UnitCount = z.UnitCount,
                UnitName = z.UnitNavigation.Name,
                ReportResultName = z.FbReport.Result.ResultName,
                BookingFormSerial = z.BookingFormSerial,
                CustomerBookingNumber = z.Inspection.CustomerBookingNo,
                IsNewProduct = z.Product.IsNewProduct,
                ReportNo = z.FbReport.ReportTitle,
                PresentedQty = z.FbReport.PresentedQty,
                OrderQty = z.FbReport.OrderQty,
                InspectedQty = z.FbReport.InspectedQty,
                ReportStatus = z.FbReport.FbReportStatusNavigation.FbstatusName,
                ExternalReportNo = z.FbReport.ExternalReportNumber
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<KpiBookingProductsData>> GetProductListByBooking(IQueryable<int> bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new KpiBookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductName = z.Product.ProductId,
                ProductId = z.ProductId,
                AQLName = z.AqlNavigation.Value,
                AqlQty = z.AqlQuantity.GetValueOrDefault(),
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.TotalBookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.Product.ProductCategorySub2Navigation.Name,
                CreatedDate = z.Inspection.CreatedOn,
                ServiceStartDate = z.FbReport.ServiceFromDate,
                ServiceEndDate = z.FbReport.ServiceToDate,
                UpdatedDate = z.Inspection.UpdatedOn,
                CombineProductId = z.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault(),
                FactoryReference = z.Product.FactoryReference,
                ReportId = z.FbReportId,
                ReportResult = z.FbReport.OverAllResult,
                Barcode = z.Product.Barcode,
                ReportResultId = z.FbReport.ResultId,
                ReportResultName = z.FbReport.Result.ResultName,
                BookingFormSerial = z.BookingFormSerial,
                CustomerBookingNumber = z.Inspection.CustomerBookingNo,
                InspectedQty = z.FbReport.InspectedQty
            }).AsNoTracking().ToListAsync();
        }

        //Get the Container item by booking
        public async Task<List<KpiBookingProductsData>> GetContainerListByBooking(IEnumerable<int> bookingId)
        {
            return await _context.InspContainerTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new KpiBookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                BookingQuantity = z.TotalBookingQuantity,
                ProductName = "",
                ReportId = z.FbReportId,
                BookingStatus = z.Inspection.StatusId,
                CreatedDate = z.Inspection.CreatedOn,
                ServiceStartDate = z.FbReport.ServiceFromDate,
                ServiceEndDate = z.FbReport.ServiceToDate,
                UpdatedDate = z.Inspection.UpdatedOn,
                ReportResult = z.FbReport.OverAllResult,
                ReportResultId = z.FbReport.ResultId,
                CriticalMax = z.FbReport.CriticalMax,
                MajorMax = z.FbReport.MajorMax,
                MinorMax = z.FbReport.MinorMax,
                FBReportDetailId = z.FbReport.Id,
                ReportResultName = z.FbReport.Result.ResultName,
                CustomerBookingNumber = z.Inspection.CustomerBookingNo,
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<KpiBookingProductsData>> GetContainerListByBooking(IQueryable<int> bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId) && y.ContainerRefId != null).Select(z => new KpiBookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductName = z.ProductRef.Product.ProductId,
                ProductId = z.ProductRef.ProductId,
                ProductDescription = z.ProductRef.Product.ProductDescription,
                BookingQuantity = z.ContainerRef.TotalBookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                ProductCategory = z.ProductRef.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.ProductRef.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.ProductRef.Product.ProductCategorySub2Navigation.Name,
                CreatedDate = z.Inspection.CreatedOn,
                ServiceStartDate = z.ContainerRef.FbReport.ServiceFromDate,
                ServiceEndDate = z.ContainerRef.FbReport.ServiceToDate,
                UpdatedDate = z.Inspection.UpdatedOn,
                CombineProductId = z.ProductRef.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.ProductRef.CombineAqlQuantity.GetValueOrDefault(),
                FactoryReference = z.ProductRef.Product.FactoryReference,
                ReportId = z.ContainerRef.FbReportId,
                ReportResult = z.ContainerRef.FbReport.OverAllResult,
                CustomerBookingNumber = z.Inspection.CustomerBookingNo,
                AQLName = z.ProductRef.AqlNavigation.Value,
                Etd = z.Etd
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<KpiPoDetails>> GetBookingPOTransactionDetails(List<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && bookingIds.Contains(x.InspectionId)).
                Select(p => new KpiPoDetails
                {
                    PoTransactionId = p.Id,
                    BookingId = p.InspectionId,
                    ProductId = p.ProductRef.Product.ProductId,
                    PoNumber = p.Po.Pono,
                    PickingQuantity = p.PickingQuantity,
                    ProductTransId = p.ProductRefId,
                    BookingQty = p.BookingQuantity,
                    ProductRefId = p.ProductRefId,
                    DestinationCountry = p.DestinationCountry.CountryName,
                    Etd = p.Etd
                }).AsNoTracking().ToListAsync();
        }


        public async Task<List<KpiPoDetails>> GetBookingPoDetails(IQueryable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && bookingIds.Contains(x.InspectionId)).
                Select(p => new KpiPoDetails
                {
                    PoTransactionId = p.Id,
                    BookingId = p.InspectionId,
                    ProductId = p.ProductRef.Product.ProductId,
                    PoNumber = p.Po.Pono,
                    PickingQuantity = p.PickingQuantity,
                    ProductTransId = p.ProductRefId,
                    BookingQty = p.BookingQuantity,
                    ProductRefId = p.ProductRefId,
                    Etd = p.Etd
                }).AsNoTracking().ToListAsync();
        }

        //Fetch the quotation details for client specific export
        public async Task<KPIQuotDetails> GetClientQuotationByBooking(List<int> bookingIds)
        {
            var response = new KPIQuotDetails();
            response.MandayList = await _context.QuQuotationInsps.Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                .Select(y => new KPIManday
                {
                    TravelManday = y.NoOfTravelManDay.GetValueOrDefault(),
                    BookingId = y.IdBooking,
                    Manday = y.NoOfManDay.GetValueOrDefault(),
                    UnitPrice = y.UnitPrice.GetValueOrDefault(),
                    TotalPrice = y.TotalCost.GetValueOrDefault(),
                    TravelTime = y.TravelTime,
                    QuotationId = y.IdQuotation,
                    InvoiceNumber = y.InvoiceNo,
                    InvoiceDate = y.InvoiceDate,
                    HotelCost = y.TravelHotel.GetValueOrDefault(),
                    InspFee = y.InspFees.GetValueOrDefault(),
                    Discount = y.IdQuotationNavigation.Discount
                }).AsNoTracking().ToListAsync();

            var quotationIds = response.MandayList.Select(x => x.QuotationId);

            response.QuotDetails = await _context.QuQuotations.Where(x => quotationIds.Contains(x.Id) && x.IdStatus != (int)QuotationStatus.Canceled)
                .Select(y => new ClientQuotationItem
                {
                    QuotationId = y.Id,
                    Booking = y.QuQuotationInsps,
                    QuotationDate = y.CreatedDate,
                    CustomerId = y.CustomerId,
                    SupplierId = y.SupplierId,
                    FactoryId = y.FactoryId,
                    SupplierName = y.Supplier.SupplierName,
                    FatoryName = y.Factory.SupplierName,
                    QuotationPrice = y.InspectionFees,
                    TravelCostAir = y.TravelCostsAir,
                    TravelCostLand = y.TravelCostsLand,
                    HotelCost = y.TravelCostsHotel,
                    OtherCost = y.OtherCosts,
                    ManDay = y.EstimatedManday,
                    CuServiceType = y.Customer.CuServiceTypes,
                    InspectionLocation = y.Factory.SuAddresses,
                    QuotationAPIcomment = y.ApiRemark,
                    BillPaidBy = y.BillingPaidById,
                    CurrencyName = y.Currency.CurrencyName,
                    BillPaidByName = y.BillingPaidBy.Label,
                    UnitPrice = y.QuQuotationInsps.Sum(x => x.UnitPrice).GetValueOrDefault()
                }).AsNoTracking().ToListAsync();

            return response;
        }

        public async Task<KPIExpenseQuotDetails> GetClientQuotationByBooking(IQueryable<int> bookingIds)
        {
            var response = new KPIExpenseQuotDetails();
            response.MandayList = await _context.QuQuotationInsps.Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                .Select(y => new KPIManday
                {
                    TravelManday = y.NoOfTravelManDay.GetValueOrDefault(),
                    BookingId = y.IdBooking,
                    Manday = y.NoOfManDay.GetValueOrDefault(),
                    UnitPrice = y.UnitPrice.GetValueOrDefault(),
                    TotalPrice = y.TotalCost.GetValueOrDefault(),
                    TravelTime = y.TravelTime,
                    QuotationId = y.IdQuotation,
                    InvoiceNumber = y.InvoiceNo,
                    InvoiceDate = y.InvoiceDate,
                    HotelCost = y.TravelHotel.GetValueOrDefault(),
                    InspFee = y.InspFees.GetValueOrDefault(),
                    Discount = y.IdQuotationNavigation.Discount
                }).AsNoTracking().ToListAsync();

            var quotationIds = response.MandayList.Select(x => x.QuotationId).Distinct();

            response.QuotDetails = await _context.QuQuotations.Where(x => quotationIds.Contains(x.Id) && x.IdStatus != (int)QuotationStatus.Canceled)
                .Select(y => new ExpenseClientQuotationItem
                {
                    QuotationId = y.Id,
                    QuotationPrice = y.InspectionFees,
                    TravelCostAir = y.TravelCostsAir,
                    TravelCostLand = y.TravelCostsLand,
                    HotelCost = y.TravelCostsHotel,
                    OtherCost = y.OtherCosts,
                    ManDay = y.EstimatedManday,
                    BillPaidBy = y.BillingPaidById,
                    CurrencyName = y.Currency.CurrencyName,
                    BillPaidByName = y.BillingPaidBy.Label,
                    Booking = y.QuQuotationInsps,
                }).AsNoTracking().ToListAsync();

            return response;
        }

        /// <summary>
        /// Get Quotation Details for carrefour
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<CarrefourQuoationDetails>> GetQuotationByBookings(IQueryable<int> bookingIds)
        {
            return await _context.QuQuotationInsps.Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                .Select(y => new CarrefourQuoationDetails
                {
                    QuotationId = y.IdQuotationNavigation.Id,
                    BookingId = y.IdBooking,
                    BillPaidByName = y.IdQuotationNavigation.BillingPaidBy.Label,
                    BillPaidById = y.IdQuotationNavigation.BillingPaidById
                }).AsNoTracking().Distinct().ToListAsync();
        }

        //Get the Shipment quantity from FB
        public async Task<List<BookingShipment>> GetInspectionQuantities(List<int> bookingIds)
        {
            return await _context.FbReportQuantityDetails
                .Where(x => bookingIds.Contains(x.InspPoTransaction.InspectionId) && x.Active == true)
                .Select(x => new BookingShipment
                {
                    BookingId = x.InspPoTransaction.InspectionId,
                    ShipmentQty = x.PresentedQuantity.GetValueOrDefault(),
                    ProductId = x.InspPoTransaction.ProductRef.Product,
                    TotalCartons = x.TotalCartons.GetValueOrDefault(),
                    ReportId = x.FbReportDetailId,
                    InspectedQty = x.InspectedQuantity
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Shipment quantity by booking id query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingShipment>> GetInspectionQuantities(IQueryable<int> bookingIds)
        {
            return await _context.FbReportQuantityDetails
                .Where(x => bookingIds.Contains(x.FbReportDetail.InspectionId.Value) && x.Active == true)
                .Select(x => new BookingShipment
                {
                    BookingId = x.InspPoTransaction.InspectionId,
                    ShipmentQty = x.PresentedQuantity.GetValueOrDefault(),
                    ProductId = x.InspPoTransaction.ProductRef.Product,
                    TotalCartons = x.TotalCartons.GetValueOrDefault(),
                    ReportId = x.FbReportDetailId,
                    InspectedQty = x.InspectedQuantity,
                    PresentedQty = x.PresentedQuantity,
                    OrderQty = x.OrderQuantity
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Shipment quantity by booking id query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingShipment>> GetInspectionQuantitiesByReportIds(IQueryable<int> reportIds)
        {
            return await _context.FbReportQuantityDetails
                .Where(x => reportIds.Contains(x.FbReportDetailId) && x.Active == true)
                .Select(x => new BookingShipment
                {
                    BookingId = x.InspPoTransaction.InspectionId,
                    ShipmentQty = x.PresentedQuantity.GetValueOrDefault(),
                    ProductId = x.InspPoTransaction.ProductRef.Product,
                    TotalCartons = x.TotalCartons.GetValueOrDefault(),
                    ReportId = x.FbReportDetailId,
                    InspectedQty = x.InspectedQuantity,
                    PresentedQty = x.PresentedQuantity
                }).AsNoTracking().ToListAsync();
        }

        public IQueryable<CuCustomer> GetCustomersItems()
        {
            return _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value);
        }

        public async Task<List<QuTranStatusLog>> GetQuotationStatusLogById(List<int> bookingIds)
        {
            return await _context.QuTranStatusLogs.Where(x => bookingIds.Contains(x.BookingId.Value) && x.StatusId == (int)QuotationStatus.CustomerValidated)
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<KPIMerchandiser>> GetMerchandiserByBooking(List<int> bookingIds)
        {
            return await _context.InspTranCuMerchandisers
                .Where(x => bookingIds.Contains(x.InspectionId))
                .Select(y => new KPIMerchandiser
                {
                    Id = y.Merchandiser.Id,
                    BookingId = y.InspectionId,
                    Name = y.Merchandiser.ContactName
                }).AsNoTracking().ToListAsync();
        }


        public async Task<List<KPIMerchandiser>> GetMerchandiserByBooking(IQueryable<int> bookingIds)
        {
            return await _context.InspTranCuMerchandisers
                .Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(y => new KPIMerchandiser
                {
                    Id = y.Merchandiser.Id,
                    BookingId = y.InspectionId,
                    Name = y.Merchandiser.ContactName
                }).AsNoTracking().ToListAsync();
        }
        //Fetch the Quotation Man Day for the bookings
        public async Task<List<QuotationManday>> GetQuotationManDay(List<int> bookingIds)
        {
            return await _context.QuQuotationInsps//.Include(x => x.Quotation)
                .Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled) //&& x.Active.HasValue && x.Active.Value)
                .Select(x => new QuotationManday
                {
                    BookingId = x.IdBooking,
                    QuotationId = x.IdQuotation,
                    ManDay = x.NoOfManDay,
                    LocationId = x.IdBookingNavigation.OfficeId.GetValueOrDefault(),
                    LocationName = x.IdBookingNavigation.Office.LocationName
                }).AsNoTracking().ToListAsync();
        }

        //Fetch the Quotation Man Day for the bookings
        public async Task<List<QuotationManday>> GetQuotationManDay(IQueryable<int> bookingIds)
        {
            return await _context.QuQuotationInsps//.Include(x => x.Quotation)
                .Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled) //&& x.Active.HasValue && x.Active.Value)
                .Select(x => new QuotationManday
                {
                    BookingId = x.IdBooking,
                    QuotationId = x.IdQuotation,
                    ManDay = x.NoOfManDay,
                    LocationId = x.IdBookingNavigation.OfficeId.GetValueOrDefault(),
                    LocationName = x.IdBookingNavigation.Office.LocationName
                }).AsNoTracking().ToListAsync();
        }

        //fetch the Qc Names from the FB table - Fb_Report_Qcdetails
        public async Task<List<CommonDataSource>> GetFbQcNames(List<int> reportIdList)
        {


            return await _context.FbReportQcdetails.Where(x => reportIdList.Contains(x.FbReportDetailId) && x.Active.Value)
                .Join(_context.ItUserMasters, fb => fb.QcId, staff => staff.FbUserId,
                (fb, staff) => new { FbReportQcdetail = fb, ItUserMaster = staff })
                .Join(_context.HrStaffs, user => user.ItUserMaster.StaffId, hr => hr.Id,
                (user, hr) => new { ItUserMaster = user, HrStaff = hr })
                .Select(x => new CommonDataSource
                {
                    Id = x.ItUserMaster.FbReportQcdetail.FbReportDetailId,
                    Name = x.HrStaff.PersonName
                }).AsNoTracking().ToListAsync();


        }

        /// <summary>
        /// Get the fb qc names by booking id
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetFbQcNames(IQueryable<int> bookingIds)
        {
            return await _context.FbReportQcdetails.Where(x => bookingIds.Contains(x.FbReportDetail.InspectionId.Value) && x.Active.Value)
               .Join(_context.ItUserMasters, fb => fb.QcId, staff => staff.FbUserId,
               (fb, staff) => new { FbReportQcdetail = fb, ItUserMaster = staff })
               .Join(_context.HrStaffs, user => user.ItUserMaster.StaffId, hr => hr.Id,
               (user, hr) => new { ItUserMaster = user, HrStaff = hr })
               .Select(x => new CommonDataSource
               {
                   Id = x.ItUserMaster.FbReportQcdetail.FbReportDetailId,
                   Name = x.HrStaff.PersonName
               }).AsNoTracking().ToListAsync();


        }

        //fetch the container Items
        public async Task<List<BookingContainerItem>> GetContainerItemsByReportId(List<int> BookingIdList)
        {
            return await _context.InspContainerTransactions.Where(x => x.FbReportId.HasValue && BookingIdList.Contains(x.InspectionId) && x.Active.HasValue && x.Active.Value)
                 .Select(x => new BookingContainerItem
                 {
                     BookingId = x.InspectionId,
                     ContainerId = x.ContainerId,
                     ReportId = x.FbReportId,
                     TotalBookingQty = x.TotalBookingQuantity,
                     ReportResultId = x.FbReport.ResultId
                 }).AsNoTracking().ToListAsync();
        }

        public async Task<List<BookingContainerItem>> GetContainerItemsByReportId(IQueryable<int> BookingIdList)
        {
            return await _context.InspContainerTransactions.Where(x => x.FbReportId.HasValue && BookingIdList.Contains(x.InspectionId) && x.Active.HasValue && x.Active.Value)
                 .Select(x => new BookingContainerItem
                 {
                     BookingId = x.InspectionId,
                     ContainerId = x.ContainerId,
                     ReportId = x.FbReportId,
                     TotalBookingQty = x.TotalBookingQuantity,
                     ReportResultId = x.FbReport.ResultId
                 }).AsNoTracking().ToListAsync();
        }
        //Get the customer Buyer
        public Task<List<BookingCustomerBuyer>> GetCustomerBuyerbyBooking(List<int> bookingIds)
        {
            return _context.InspTranCuBuyers.
                Where(x => bookingIds.Contains(x.InspectionId) && x.Active)
                .Select(x => new BookingCustomerBuyer
                {
                    Id = x.Buyer.Id,
                    BookingId = x.InspectionId,
                    BuyerName = x.Buyer.Name
                })
                .AsNoTracking().OrderBy(x => x.BuyerName).ToListAsync();
        }

        /// <summary>
        /// Get customer buyers by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public Task<List<BookingCustomerBuyer>> GetCustomerBuyerbyBookingQuery(IQueryable<int> bookingIds)
        {
            return _context.InspTranCuBuyers.
                Where(x => bookingIds.Contains(x.InspectionId) && x.Active)
                .Select(x => new BookingCustomerBuyer
                {
                    Id = x.Buyer.Id,
                    BookingId = x.InspectionId,
                    BuyerName = x.Buyer.Name
                })
                .AsNoTracking().OrderBy(x => x.BuyerName).ToListAsync();
        }

        //Get the Final decision date by Report Id from INSP_REP_CUS_Decision
        public async Task<List<CommonIdDate>> GetDecisionDateByReport(List<int> bookingIdList)
        {
            return await _context.InspRepCusDecisions
                .Where(x => x.Active.HasValue && x.Active.Value && bookingIdList.Contains(x.Report.InspectionId.GetValueOrDefault()))
                .Select(y => new CommonIdDate
                {
                    Id = y.ReportId,
                    Date = y.CreatedOn,
                    Name = y.CustomerResult.CustomDecisionName ?? y.CustomerResult.CusDec.Name
                }).AsNoTracking().ToListAsync();
        }

        //Get the Final decision date by Report Id from INSP_REP_CUS_Decision
        public async Task<List<CommonIdDate>> GetDecisionDateByReport(IQueryable<int> bookingIdList)
        {
            return await _context.InspRepCusDecisions
                .Where(x => x.Active.HasValue && x.Active.Value && bookingIdList.Contains(x.Report.InspectionId.GetValueOrDefault()))
                .Select(y => new CommonIdDate
                {
                    Id = y.ReportId,
                    Date = y.CreatedOn,
                    Name = y.CustomerResult.CustomDecisionName
                }).AsNoTracking().ToListAsync();
        }

        //Get the Final decision date by Report Id from INSP_REP_CUS_Decision
        public async Task<List<CommonIdDate>> GetICByReport(List<int> bookingIdList)
        {
            return await _context.InspIcTranProducts
                .Where(x => x.Active.Value && bookingIdList.Contains(x.BookingProduct.ProductRef.InspectionId))
                .Select(y => new CommonIdDate
                {
                    Id = y.BookingProduct.ProductRef.Id,
                    Date = y.Ic.ApprovalDate
                }).AsNoTracking().ToListAsync();
        }

        //Get the Final decision date by Report Id from INSP_REP_CUS_Decision
        public async Task<List<CommonIdDate>> GetICByReport(IQueryable<int> bookingIdList)
        {
            return await _context.InspIcTranProducts
                .Where(x => x.Active.Value && bookingIdList.Contains(x.BookingProduct.InspectionId))
                .Select(y => new CommonIdDate
                {
                    Id = y.BookingProduct.ProductRef.Id,
                    Date = y.Ic.ApprovalDate
                }).AsNoTracking().ToListAsync();
        }

        //Get Holiday List by date range
        public async Task<List<HrHolidayData>> GetHolidaysByDateRange(DateTime startdate, DateTime enddate)
        {
            return await _context.HrHolidays
                .Where(x => x.StartDate != null && (x.EndDate != null) && !(x.StartDate.Value > enddate || x.EndDate.Value < startdate))
                .Select(x => new HrHolidayData
                {
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).AsNoTracking().OrderBy(x => x.StartDate).ToListAsync();

        }

        //Get Holiday List by date range
        public async Task<List<HrHoliday>> GetHolidaysByRange(DateTime startdate, DateTime enddate)
        {
            return await _context.HrHolidays
                .Where(x => x.StartDate != null && (x.EndDate != null) && !(x.StartDate.Value > enddate || x.EndDate.Value < startdate))
               .AsNoTracking().OrderBy(x => x.StartDate).ToListAsync();

        }

        //Get Reinspection booking Id
        public async Task<List<Reinspection>> GetReinspectionBooking(List<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => x.PreviousBookingNo.HasValue && bookingIds.Contains(x.PreviousBookingNo.Value))
                .Select(x => new Reinspection
                {
                    BookingId = x.PreviousBookingNo.Value,
                    ReInspectionbookingId = x.Id
                }).AsNoTracking().ToListAsync();
        }

        //Get Reinspection booking Id
        public async Task<List<Reinspection>> GetReinspectionBookingByBookingQuey(IQueryable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => x.PreviousBookingNo.HasValue && bookingIds.Contains(x.PreviousBookingNo.Value))
                .Select(x => new Reinspection
                {
                    BookingId = x.PreviousBookingNo.Value,
                    ReInspectionbookingId = x.Id
                }).AsNoTracking().ToListAsync();
        }

        //Get Fb report problematic remarks by report Id
        public async Task<List<FbReportRemarks>> GetFbProblematicRemarks(List<int> reportIds)
        {
            return await _context.FbReportProblematicRemarks.Where(x => reportIds.Contains(x.FbReportSummary.FbReportDetailId) && x.FbReportSummary.Active.Value)
                .Select(x => new FbReportRemarks
                {
                    ReportId = x.FbReportSummary.FbReportDetailId,
                    ProductId = x.ProductId,
                    Remarks = x.Remarks,
                    Result = x.Result,
                    SubCategory = x.SubCategory,
                    SubCategory2 = x.SubCategory2,
                    InspSummaryId = x.FbReportSummaryId,
                    Reference = x.CustomerRemarkCode
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Fb report problematic remarks by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<FbReportRemarks>> GetFbProblematicRemarks(IQueryable<int> bookingIds)
        {
            return await _context.FbReportProblematicRemarks.Where(x => bookingIds.Contains(x.FbReportSummary.FbReportDetail.InspectionId.Value) && x.FbReportSummary.Active.Value)
                .Select(x => new FbReportRemarks
                {
                    ReportId = x.FbReportSummary.FbReportDetailId,
                    ProductId = x.ProductId,
                    Remarks = x.Remarks,
                    Result = x.Result,
                    SubCategory = x.SubCategory,
                    SubCategory2 = x.SubCategory2,
                    InspSummaryId = x.FbReportSummaryId,
                    Reference = x.CustomerRemarkCode
                }).AsNoTracking().ToListAsync();
        }

        //Get Fb report problematic remarks by report Id
        public async Task<List<FbReportRemarks>> GetFbProblematicRemarksEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.FbReportProblematicRemarks.Where(x => bookingIdList.Contains(x.FbReportSummary.FbReportDetail.InspectionId.GetValueOrDefault()) && x.FbReportSummary.Active.Value && x.Active.Value)
                .Select(x => new FbReportRemarks
                {
                    ReportId = x.FbReportSummary.FbReportDetailId,
                    ProductId = x.ProductId,
                    Remarks = x.Remarks,
                    Result = x.Result,
                    SubCategory = x.SubCategory,
                    SubCategory2 = x.SubCategory2,
                    InspSummaryId = x.FbReportSummaryId,
                    Reference = x.CustomerRemarkCode
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<KpiReportRemarksTemplateRepo>> GetReportRemarksDataEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.FbReportProblematicRemarks.
                Where(x => x.Active.Value && bookingIdList.Contains(x.FbReportSummary.FbReportDetail.InspectionId.GetValueOrDefault()))
                .Select(x => new KpiReportRemarksTemplateRepo()
                {
                    BookingNo = x.FbReportSummary.FbReportDetail.Inspection.Id,
                    CustomerBookingNo = x.FbReportSummary.FbReportDetail.Inspection.CustomerBookingNo,
                    CustomerName = x.FbReportSummary.FbReportDetail.Inspection.Customer.CustomerName,
                    CollectionName = x.FbReportSummary.FbReportDetail.Inspection.Collection.Name,
                    Office = x.FbReportSummary.FbReportDetail.Inspection.Office.LocationName,
                    SupplierName = x.FbReportSummary.FbReportDetail.Inspection.Supplier.SupplierName,
                    FactoryName = x.FbReportSummary.FbReportDetail.Inspection.Factory.SupplierName,
                    FactoryCountry = x.FbReportSummary.FbReportDetail.Inspection.Factory.SuAddresses.FirstOrDefault().Country.CountryName,
                    BillPaidBy = x.FbReportSummary.FbReportDetail.Inspection.QuQuotationInsps.Select(y => y.IdQuotationNavigation.BillingPaidById).FirstOrDefault(),
                    BookingStatus = x.FbReportSummary.FbReportDetail.Inspection.Status.Status,
                    InspectionStartDate = x.FbReportSummary.FbReportDetail.Inspection.FirstServiceDateFrom,
                    InspectionEndDate = x.FbReportSummary.FbReportDetail.Inspection.ServiceDateTo,
                    Month = x.FbReportSummary.FbReportDetail.Inspection.ServiceDateTo.Month,
                    Year = x.FbReportSummary.FbReportDetail.Inspection.ServiceDateTo.Year,
                    ReportResult = x.FbReportSummary.FbReportDetail.Result.ResultName,
                    ProductId = x.ProductId,
                    FBRemarkResult = x.Result,
                    ReportRemarks = x.Remarks,
                    ReportId = x.FbReportSummary.FbReportDetail.Id,
                    RemarkCategory = x.FbReportSummary.Name,
                    RemarkSubCategory = x.SubCategory,
                    RemarkSubCategory2 = x.SubCategory2,
                    CustomerRemarkCodeReference = x.CustomerRemarkCode,

                }).OrderBy(x => x.BookingNo).ToListAsync();
        }

        public async Task<List<KpiBookingProductsData>> GetFailedProductListByBooking(IEnumerable<int> bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId) && y.FbReport.ResultId == (int)FBReportResult.Fail).Select(z => new KpiBookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductName = z.Product.ProductId,
                ProductId = z.ProductId,
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.TotalBookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.Product.ProductCategorySub2Navigation.Name,
                CreatedDate = z.Inspection.CreatedOn,
                ServiceStartDate = z.FbReport.ServiceFromDate,
                ServiceEndDate = z.FbReport.ServiceToDate,
                UpdatedDate = z.Inspection.UpdatedOn,
                CombineProductId = z.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault(),
                FactoryReference = z.Product.FactoryReference,
                AqlQty = z.AqlQuantity.GetValueOrDefault(),
                ReportId = z.FbReportId,
                AQLName = z.AqlNavigation.Value,
                ReportResult = z.FbReport.Result.ResultName,
                Barcode = z.Product.Barcode,
                ReportResultId = z.FbReport.ResultId,
            }).AsNoTracking().ToListAsync();
        }

        //get list of fb sampletype by passing fbreportids
        public async Task<List<FBSampleType>> GetFBSampleTypeList(List<int> fbReportIds)
        {
            return await _context.FbReportSampleTypes.Where(x => fbReportIds.Contains(x.FbReportId) && x.Active.Value)
                .Select(x => new FBSampleType
                {
                    FBReportId = x.FbReportId,
                    Comments = x.Comments,
                    Description = x.Description,
                    SampleType = x.SampleType,
                    ProductId = x.ProductId
                }).AsNoTracking().ToListAsync();
        }

        //get list of fb otherinformation by passing fbreportids
        public async Task<List<FBOtherInformation>> GetFBOtherInformationList(List<int> fbReportIds)
        {
            return await _context.FbReportOtherInformations.Where(x => fbReportIds.Contains(x.FbReportId) && x.Active.Value)
                .Select(x => new FBOtherInformation
                {
                    FBReportId = x.FbReportId,
                    Remarks = x.Remarks,
                    SubCategory = x.SubCategory,
                    SubCategory2 = x.SubCategory2,
                    ProductId = x.ProductId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get list of fb otherinformation by booking query 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<FBOtherInformation>> GetFBOtherInformationList(IQueryable<int> bookingIds)
        {
            return await _context.FbReportOtherInformations.Where(x => bookingIds.Contains(x.FbReport.InspectionId.Value) && x.Active.Value)
                .Select(x => new FBOtherInformation
                {
                    FBReportId = x.FbReportId,
                    Remarks = x.Remarks,
                    SubCategory = x.SubCategory,
                    SubCategory2 = x.SubCategory2,
                    ProductId = x.ProductId
                }).AsNoTracking().ToListAsync();
        }

        //get list of fb report defects details by po ids
        public async Task<List<FBReportDefects>> GetFBDefects(IEnumerable<int> poIdList)
        {
            return await _context.FbReportInspDefects.Where(x => poIdList.Contains(x.InspPoTransactionId) && x.Active.Value).
                Select(x => new FBReportDefects
                {
                    Critical = x.Critical,
                    Major = x.Major,
                    Minor = x.Minor,
                    InspPoId = x.InspPoTransactionId,
                    FBReportDetailId = x.FbReportDetailId,
                    ProductId = x.InspPoTransaction.ProductRef.ProductId,
                    DefectCategory = x.CategoryName,
                    PoNumber = x.InspPoTransaction.Po.Pono,
                    DefectDesc = x.Description
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<FBReportDefects>> GetFBDefects(IQueryable<int> bookingIds)
        {
            return await _context.FbReportInspDefects.Where(x => bookingIds.Contains(x.FbReportDetail.InspectionId.Value) && x.Active.Value).
                Select(x => new FBReportDefects
                {
                    Critical = x.Critical,
                    Major = x.Major,
                    Minor = x.Minor,
                    InspPoId = x.InspPoTransactionId,
                    FBReportDetailId = x.FbReportDetailId,
                    ProductId = x.InspPoTransaction.ProductRef.ProductId,
                    DefectCategory = x.CategoryName,
                    PoNumber = x.InspPoTransaction.Po.Pono,
                    DefectDesc = x.Description,
                    DefectCode = x.Code,
                    Position = x.Position
                }).AsNoTracking().ToListAsync();
        }


        public async Task<List<FBReportDefects>> GetFBDefectsByReportIds(IEnumerable<int> reportIds)
        {
            return await _context.FbReportInspDefects.Where(x => reportIds.Contains(x.FbReportDetailId) && x.Active.Value).
                Select(x => new FBReportDefects
                {
                    Critical = x.Critical,
                    Major = x.Major,
                    Minor = x.Minor,
                    InspPoId = x.InspPoTransactionId,
                    FBReportDetailId = x.FbReportDetailId,
                    ProductId = x.InspPoTransaction.ProductRef.ProductId,
                    DefectCategory = x.CategoryName,
                    PoNumber = x.InspPoTransaction.Po.Pono,
                    DefectDesc = x.Description,
                    DefectCode = x.Code,
                    Position = x.Position,
                    DefectCheckpoint = x.DefectCheckPoint
                }).AsNoTracking().ToListAsync();
        }

        //get result of report details
        public async Task<List<FBReportInspSubSummary>> GetFBInspSummaryResult(IEnumerable<int> fbReportIdList)
        {
            return await _context.FbReportInspSubSummaries.Where(x => x.Active.Value && x.FbReportSummary.Active.Value &&
                            fbReportIdList.Contains(x.FbReportSummary.FbReportDetailId) &&
                            x.FbReportSummary.FbReportInspsumTypeId == (int)InspSummaryType.Main
                            && x.FbReportSummary.Name.ToLower() == FBInspSummaryMainNameWorkmanship.ToLower() &&
                            (x.Name.ToLower() == FBInspSummarySubNameCritical.ToLower() ||
                            x.Name.ToLower() == FBInspSummarySubNameMajor.ToLower() ||
                            x.Name.ToLower() == FBInspSummarySubNameMinor.ToLower())
                    ).
                    Select(x => new FBReportInspSubSummary
                    {
                        Result = x.Result,
                        FBReportId = x.FbReportSummary.FbReportDetailId,
                        Name = x.Name
                    }).AsNoTracking().ToListAsync();
        }

        //Get Supplier and Factory contacts by Bookng Id
        public async Task<List<BookingContacts>> GetContactNames(List<int> bookingIds)
        {
            var data = await _context.InspTranFaContacts.Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault()) && x.Active)
                 .Select(x => new BookingContacts
                 {
                     Id = x.Contact.Id,
                     BookingId = x.InspectionId.GetValueOrDefault(),
                     StaffName = x.Contact.ContactName,
                     Email = x.Contact.Mail,
                     Phone = x.Contact.Phone
                 }).AsNoTracking().ToListAsync();

            data.Concat(await _context.InspTranSuContacts.Where(x => bookingIds.Contains(x.InspectionId) && x.Active)
                 .Select(x => new BookingContacts
                 {
                     Id = x.Contact.Id,
                     BookingId = x.InspectionId,
                     StaffName = x.Contact.ContactName,
                     Email = x.Contact.Mail,
                     Phone = x.Contact.Phone
                 }).AsNoTracking().ToListAsync());

            return data;
        }

        //Get Customer contacts by Bookng Id
        public async Task<List<BookingContacts>> GetCustomerContactNames(List<int> bookingIds)
        {
            return await _context.InspTranCuContacts.Where(x => bookingIds.Contains(x.InspectionId) && x.Active)
                 .Select(x => new BookingContacts
                 {
                     Id = x.Contact.Id,
                     BookingId = x.InspectionId,
                     StaffName = x.Contact.ContactName,
                     Email = x.Contact.Email,
                     Phone = x.Contact.Phone
                 }).AsNoTracking().ToListAsync();
        }


        //get result of report details from Fb_Report_Inspsummary
        public async Task<List<FBReportInspSubSummary>> GetFBInspSummaryResultbyReport(IEnumerable<int> fbReportIdList)
        {
            return await _context.FbReportInspSummaries.Where(x => x.Active.HasValue && x.Active == true && fbReportIdList.Contains(x.FbReportDetailId) && x.FbReportInspsumTypeId == (int)InspSummaryType.Main).OrderBy(x => x.Sort ?? int.MaxValue)
                .Select(y => new FBReportInspSubSummary
                {
                    FBReportId = y.FbReportDetailId,
                    Result = y.ResultNavigation.ResultName,
                    Name = y.Name,
                    Id = y.Id,
                    ScoreValue = y.ScoreValue
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get fb inspection summary by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<FBReportInspSubSummary>> GetFBInspSummaryResultbyReport(IQueryable<int> bookingIds)
        {
            return await _context.FbReportInspSummaries.Where(x => x.Active.Value && bookingIds.Contains(x.FbReportDetail.InspectionId.Value) && x.FbReportInspsumTypeId == (int)InspSummaryType.Main).OrderBy(x => x.Sort ?? int.MaxValue)
                .Select(y => new FBReportInspSubSummary
                {
                    FBReportId = y.FbReportDetailId,
                    Result = y.ResultNavigation.ResultName,
                    Name = y.Name,
                    Id = y.Id
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<FBReportInspSubSummary>> GetFBInspSummaryResultbyReportIds(IQueryable<int> reportIds)
        {
            return await _context.FbReportInspSummaries.Where(x => x.Active.Value && reportIds.Contains(x.FbReportDetailId) && x.FbReportInspsumTypeId == (int)InspSummaryType.Main).OrderBy(x => x.Sort ?? int.MaxValue)
                .Select(y => new FBReportInspSubSummary
                {
                    FBReportId = y.FbReportDetailId,
                    Result = y.ResultNavigation.ResultName,
                    Name = y.Name,
                    Id = y.Id
                }).AsNoTracking().ToListAsync();
        }

        //Get the customer brand
        public Task<List<KPIMerchandiser>> GetCustomerBrandbyBooking(List<int> bookingIds)
        {
            return _context.InspTranCuBrands.
                Where(x => bookingIds.Contains(x.InspectionId) && x.Active)
                .Select(x => new KPIMerchandiser
                {
                    Id = x.Brand.Id,
                    BookingId = x.InspectionId,
                    Name = x.Brand.Name
                })
                .AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Get customer brands by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public Task<List<KPIMerchandiser>> GetCustomerBrandbyBookingQuery(IQueryable<int> bookingIds)
        {
            return _context.InspTranCuBrands.
                Where(x => bookingIds.Contains(x.InspectionId) && x.Active)
                .Select(x => new KPIMerchandiser
                {
                    Id = x.Brand.Id,
                    BookingId = x.InspectionId,
                    Name = x.Brand.Name
                })
                .AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        }

        //get comments from FB_report_Comments by fbreportids
        public async Task<List<FBOtherInformation>> GetFBReportComments(List<int> fbReportIds)
        {
            return await _context.FbReportComments.Where(x => fbReportIds.Contains(x.FbReportId) && x.Active.Value)
                .Select(x => new FBOtherInformation
                {
                    FBReportId = x.FbReportId,
                    Remarks = x.Comments,
                    ProductId = x.ProductId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// fetch the products with isecopack is true
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<KpiBookingProductsData>> GetProductListForEcoPack(List<int> bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId) && y.IsEcopack == true).Select(z => new KpiBookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductName = z.Product.ProductId,
                ProductId = z.ProductId,
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.TotalBookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.Product.ProductCategorySub2Navigation.Name,
                ReportResult = z.FbReport.OverAllResult,
                ReportId = z.FbReportId,
                ServiceEndDate = z.Inspection.ServiceDateTo,
                ServiceStartDate = z.Inspection.ServiceDateFrom
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// fetch the products where isecopack selected by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<KpiBookingProductsData>> GetProductListForEcoPack(IQueryable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingIds.Contains(y.InspectionId) && y.IsEcopack.Value).Select(z => new KpiBookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductName = z.Product.ProductId,
                ProductId = z.ProductId,
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.TotalBookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.Product.ProductCategorySub2Navigation.Name,
                ReportResult = z.FbReport.OverAllResult,
                ReportId = z.FbReportId,
                ServiceEndDate = z.Inspection.ServiceDateTo,
                ServiceStartDate = z.Inspection.ServiceDateFrom
            }).AsNoTracking().ToListAsync();
        }



        /// <summary>
        /// Get the report packing battery info
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<List<KpiReportBatteryItem>> GetReportBatteryInfo(List<int> reportIdList)
        {
            return await _context.FbReportPackingBatteryInfos.Where(x => x.Active == true && reportIdList.Contains(x.FbReportId))
                .Select(x => new KpiReportBatteryItem
                {
                    ReportId = x.FbReportId,
                    ProductId = x.ProductId,
                    BatteryModel = x.BatteryModel,
                    BatteryType = x.BatteryType,
                    Quantity = x.Quantity,
                    NetWeight = x.NetWeightPerQty
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Report battery info by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<KpiReportBatteryItem>> GetReportBatteryInfo(IQueryable<int> bookingIds)
        {
            return await _context.FbReportPackingBatteryInfos.Where(x => x.Active.Value
                                && bookingIds.Contains(x.FbReport.InspectionId.Value) && x.FbReport.InspProductTransactions.Any(y => y.Active.Value && y.IsEcopack.Value))
                .Select(x => new KpiReportBatteryItem
                {
                    ReportId = x.FbReportId,
                    ProductId = x.ProductId,
                    BatteryModel = x.BatteryModel,
                    BatteryType = x.BatteryType,
                    Quantity = x.Quantity,
                    NetWeight = x.NetWeightPerQty
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the report packing info
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<List<KpiReportPackingItem>> GetReportPackingInfo(List<int> reportIdList)
        {
            return await _context.FbReportPackingInfos.Where(x => x.Active == true && reportIdList.Contains(x.FbReportId))
                .Select(x => new KpiReportPackingItem
                {
                    ReportId = x.FbReportId,
                    ProductId = x.ProductId,
                    MaterialCode = x.MaterialType,
                    MaterialGroup = x.PackagingDesc,
                    Quantity = x.Quantity,
                    NetWeight = x.NetWeightPerQty,
                    Location = x.Location,
                    PieceNo = x.PieceNo
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Report PackingInfo by booking id query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<KpiReportPackingItem>> GetReportPackingInfo(IQueryable<int> bookingIds)
        {
            return await _context.FbReportPackingInfos.Where(x => x.Active.Value && bookingIds.Contains(x.FbReport.InspectionId.Value)
                                 && x.FbReport.InspProductTransactions.Any(y => y.Active.Value && y.IsEcopack.Value))
                .Select(x => new KpiReportPackingItem
                {
                    ReportId = x.FbReportId,
                    ProductId = x.ProductId,
                    MaterialCode = x.MaterialType,
                    MaterialGroup = x.PackagingDesc,
                    Quantity = x.Quantity,
                    NetWeight = x.NetWeightPerQty,
                    Location = x.Location,
                    PieceNo = x.PieceNo
                }).AsNoTracking().ToListAsync();
        }

        //get reschedule details by booking ids
        public async Task<List<RescheduleData>> GetRescheduleBookingDetails(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranReschedules.Where(x => bookingIds.Contains(x.InspectionId)).
                Select(x => new RescheduleData
                {
                    ReasonName = x.ReasonType.Reason,
                    BookingId = x.InspectionId,
                    CreatedOn = x.CreatedOn
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get reschdule details by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<RescheduleData>> GetRescheduleBookingDetails(IQueryable<int> bookingIds)
        {
            return await _context.InspTranReschedules.Where(x => bookingIds.Contains(x.InspectionId)).
                Select(x => new RescheduleData
                {
                    ReasonName = x.ReasonType.Reason,
                    BookingId = x.InspectionId,
                    CreatedOn = x.CreatedOn
                }).AsNoTracking().ToListAsync();
        }

        //get packing dimention details 
        public async Task<List<FbPackingDimention>> GetFBReportDimentionDetails(IEnumerable<int> fbReportIds)
        {
            return await _context.FbReportPackingDimentions.Where(x => fbReportIds.Contains(x.FbReportId) && x.Active.Value).
                Select(x => new FbPackingDimention
                {
                    ReportId = x.FbReportId,
                    ProductId = x.ProductId,
                    MeasuredValuesHeight = x.MeasuredValuesH,
                    MeasuredValuesLength = x.MeasuredValuesL,
                    MeasuredValuesWidth = x.MeasuredValuesW,
                    SpecClientValuesHeight = x.SpecClientValuesH,
                    SpecClientValuesLength = x.SpecClientValuesL,
                    SpecClientValuesWidth = x.SpecClientValuesW
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get packing dimension details by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<FbPackingDimention>> GetFBReportDimentionDetails(IQueryable<int> bookingIds)
        {
            return await _context.FbReportPackingDimentions.Where(x => bookingIds.Contains(x.FbReport.InspectionId.Value) && x.Active.Value).
                Select(x => new FbPackingDimention
                {
                    ReportId = x.FbReportId,
                    ProductId = x.ProductId,
                    MeasuredValuesHeight = x.MeasuredValuesH,
                    MeasuredValuesLength = x.MeasuredValuesL,
                    MeasuredValuesWidth = x.MeasuredValuesW,
                    SpecClientValuesHeight = x.SpecClientValuesH,
                    SpecClientValuesLength = x.SpecClientValuesL,
                    SpecClientValuesWidth = x.SpecClientValuesW
                }).AsNoTracking().ToListAsync();
        }

        //get packing weight details 
        public async Task<List<FbPackingWeight>> GetFBReportWeightDetails(IEnumerable<int> fbReportIds)
        {
            return await _context.FbReportPackingWeights.Where(x => fbReportIds.Contains(x.FbReportId) && x.Active.Value).
                Select(x => new FbPackingWeight
                {
                    ProductId = x.ProductId,
                    ReportId = x.FbReportId,
                    MeasuredValuesWeight = x.MeasuredValues,
                    SpecClientValuesWeight = x.SpecClientValues
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<FbPackingWeight>> GetFBReportWeightDetails(IQueryable<int> bookingIds)
        {
            return await _context.FbReportPackingWeights.Where(x => bookingIds.Contains(x.FbReport.InspectionId.Value) && x.Active.Value).
                Select(x => new FbPackingWeight
                {
                    ProductId = x.ProductId,
                    ReportId = x.FbReportId,
                    MeasuredValuesWeight = x.MeasuredValues,
                    SpecClientValuesWeight = x.SpecClientValues
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// fetch the invoice number for bookings
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<KpiInvoiceData>> GetInvoiceNoByBooking(List<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceStatus.HasValue && InvoicedStatusList.Contains(x.InvoiceStatus.Value) && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
               .Select(x => new KpiInvoiceData
               {
                   InvoiceId = x.Id,
                   InvoiceNo = x.InvoiceNo,
                   BookingId = x.InspectionId,
                   InvoiceDate = x.InvoiceDate,
                   InvoiceRemarks = x.Remarks,
                   InspFees = x.InspectionFees,
                   TravelFee = x.TravelTotalFees,
                   HotelFee = x.HotelFees,
                   TravelLandFee = x.TravelLandFees,
                   TravelAirFee = x.TravelAirFees,
                   TotalFee = x.TotalInvoiceFees,
                   CurrencyName = x.InvoiceCurrencyNavigation.CurrencyName,
                   OtherFee = x.OtherFees,
                   BilledTo = x.InvoiceTo,
                   Discount = x.Discount,
                   InvoiceName = x.InvoicedName,
                   ManDay = x.ManDays,
                   UnitPrice = x.UnitPrice,
                   InvoiceMethod = x.InvoiceMethodNavigation.Label,
                   InvoicePaymentStatus = x.InvoicePaymentStatus,
                   BilledToName = x.InvoiceToNavigation.Label
               }).AsNoTracking().ToListAsync();
        }

        public async Task<List<KpiInvoiceData>> GetInvoiceNoByBooking(IQueryable<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceStatus.HasValue && InvoicedStatusList.Contains(x.InvoiceStatus.Value) && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
               .Select(x => new KpiInvoiceData
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
                   Discount = x.Discount,
                   TaxValue = x.TaxValue,
                   BilledName = x.InvoiceToNavigation.Label,
                   PaymentTerms = x.PaymentTerms,
                   PaymentDuration = x.PaymentDuration,
                   InvoiceStatus = x.InvoiceStatusNavigation.Name,
                   PaymentDate = x.InvoicePaymentDate,
                   PaymentStatusName = x.InvoicePaymentStatusNavigation.Name,
                   CurrencyId = x.InvoiceCurrency

               }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the pre invoice iqueryable
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public IQueryable<InvAutTranDetail> GetInvoiceDetailsQueryable(IQueryable<int> bookingIds)
        {
            return _context.InvAutTranDetails.Where(x => x.InvoiceStatus.HasValue
                               && InvoicedStatusList.Contains(x.InvoiceStatus.Value)
                               && bookingIds.Contains(x.InspectionId.GetValueOrDefault()));
        }
        public async Task<List<KpiInvoiceData>> GetInvoiceDetailsByInvoiceNo(string invoiceNo)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceNo == invoiceNo && x.ServiceId == (int)Service.InspectionId &&
            x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled)
               .Select(x => new KpiInvoiceData
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
                   Discount = x.Discount,
                   TaxValue = x.TaxValue,
                   BilledName = x.InvoiceToNavigation.Label
               }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// fetch the extra fee for bookings
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<KpiExtraFeeData>> GetExtraFeeByBooking(List<int> bookingIds)
        {
            return await _context.InvExfTransactions.Where(x => x.StatusId != (int)ExtraFeeStatus.Cancelled && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
               .Select(x => new KpiExtraFeeData
               {
                   BookingId = x.InspectionId,
                   InvoiceId = x.InvoiceId,
                   ExtraFee = x.TotalExtraFee,
                   BilledTo = x.BilledTo,
                   ExtraFeeInvoiceNo = x.ExtraFeeInvoiceNo,
                   CurrencyName = x.Currency.CurrencyName,
                   PaymentStatus = x.PaymentStatus,
                   BilledName = x.BilledName,
                   BilledToName = x.BilledToNavigation.Label
               }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// fetch the extra fee for bookings by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<KpiExtraFeeData>> GetExtraFeeByBooking(IQueryable<int> bookingIds)
        {
            return await _context.InvExfTransactions.Where(x => x.StatusId != (int)ExtraFeeStatus.Cancelled && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
               .Select(x => new KpiExtraFeeData
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
                   PaymentDuration = x.PaymentDuration
               }).AsNoTracking().ToListAsync();
        }

        public IQueryable<InvExfTransaction> GetExtraFeeByBookingQuery(IQueryable<int> bookingIds)
        {
            return _context.InvExfTransactions.Where(x => x.StatusId != (int)ExtraFeeStatus.Cancelled
            && bookingIds.Contains(x.InspectionId.GetValueOrDefault()));

        }

        /// <summary>
        /// Get Extra Fee by invoice no
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        public async Task<List<KpiExtraFeeData>> GetExtraFeeByInvoiceNo(string invoiceNo)
        {
            return await _context.InvExfTransactions.Where(x => x.StatusId != (int)ExtraFeeStatus.Cancelled && x.Invoice.InvoiceNo == invoiceNo)
               .Select(x => new KpiExtraFeeData
               {
                   BookingId = x.InspectionId,
                   InvoiceId = x.InvoiceId,
                   ExtraFee = x.TotalExtraFee,
                   BilledTo = x.BilledTo
               }).AsNoTracking().ToListAsync();
        }

        public async Task<List<MdmDefectData>> GetMDMDefectData(KpiDbRequest request)
        {
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, Mdm_Kpi_sp, request);

            var list = await ADOHelper.ConvertDataTable<MdmDefectData>(dataSet.Tables[0]);

            return list;
        }

        /// <summary>
        /// Get defect summary data list
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<KpiDefectDataRepo>> GetDefectSummaryDataEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.FbReportInspDefects.
                Where(x => x.Active.Value && bookingIdList.Contains(x.FbReportDetail.InspectionId.GetValueOrDefault()))
                .Select(x => new KpiDefectDataRepo()
                {
                    PoId = x.InspPoTransactionId,
                    BookingNo = x.FbReportDetail.InspectionId.GetValueOrDefault(),
                    ReportId = x.FbReportDetail.Id,
                    DefectDesc = x.Description,
                    CriticalDefect = x.Critical.GetValueOrDefault(),
                    MajorDefect = x.Major.GetValueOrDefault(),
                    MinorDefect = x.Minor.GetValueOrDefault(),
                    DefectCategory = x.CategoryName,
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<KpiDefectInspectionRepo>> GetKpiDefectInspectionData(IQueryable<int> bookingIds)
        {
            return await _context.FbReportDetails.Where(x => x.Active == true && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
                .Select(x => new KpiDefectInspectionRepo()
                {
                    ReportId = x.Id,
                    CustomerBookingNo = x.Inspection.CustomerBookingNo,
                    CustomerName = x.Inspection.Customer.CustomerName,
                    CollectionName = x.Inspection.Collection.Name,
                    Office = x.Inspection.Office.LocationName,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    FactoryName = x.Inspection.Factory.SupplierName,
                    FactoryCountry = x.Inspection.Factory.SuAddresses.FirstOrDefault().Country.CountryName,
                    ServiceTypeName = x.Inspection.InspTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                    BookingStatus = x.Inspection.Status.Status,
                    InspectionStartDate = x.Inspection.ServiceDateFrom,
                    InspectionEndDate = x.Inspection.ServiceDateTo,
                    Month = x.Inspection.ServiceDateTo.Month,
                    Year = x.Inspection.ServiceDateTo.Year,
                    ReportResult = x.Result.ResultName,
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<KpiBookingBuyer>> GetKPIBookingBuyerDataEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.InspTranCuBuyers.
                Where(x => x.Active && bookingIdList.Contains(x.InspectionId))
                .Select(x => new KpiBookingBuyer()
                {
                    BookingId = x.InspectionId,
                    BuyerName = x.Buyer.Name,
                    Id = x.Buyer.Id
                }).ToListAsync();
        }

        public async Task<List<KpiBookingDepartment>> GetKPIBookingDepartmentDataEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.InspTranCuDepartments.
                Where(x => x.Active && bookingIdList.Contains(x.InspectionId))
                .Select(x => new KpiBookingDepartment()
                {
                    BookingId = x.InspectionId,
                    DepartmentName = x.Department.Name,
                    DepartmentCode = x.Department.Code,
                    Id = x.Department.Id
                }).ToListAsync();
        }


        public async Task<List<KpiBookingContact>> GetKPIBookingCustomerContactsDataEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.InspTranCuContacts.
                Where(x => x.Active && bookingIdList.Contains(x.InspectionId))
                .Select(x => new KpiBookingContact()
                {
                    BookingId = x.InspectionId,
                    ContactName = x.Contact.ContactName,
                    ContactEmail = x.Contact.Email
                }).ToListAsync();
        }


        /// <summary>
        /// Get defect summary data from SP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Tuple<List<KpiDefectDataRepo>, List<KpiBookingDepartment>,
            List<KpiBookingBuyer>, List<KpiBookingContact>>> GetDefectSummaryData(KpiBookingSPRequest request)
        {
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, Defect_Summary_Kpi_sp, request);

            var defectList = await ADOHelper.ConvertDataTable<KpiDefectDataRepo>(dataSet.Tables[0]);

            var departmentList = await ADOHelper.ConvertDataTable<KpiBookingDepartment>(dataSet.Tables[1]);

            var buyerList = await ADOHelper.ConvertDataTable<KpiBookingBuyer>(dataSet.Tables[2]);

            var contactList = await ADOHelper.ConvertDataTable<KpiBookingContact>(dataSet.Tables[3]);

            return new Tuple<List<KpiDefectDataRepo>, List<KpiBookingDepartment>, List<KpiBookingBuyer>,
                List<KpiBookingContact>>(defectList, departmentList, buyerList, contactList);
        }

        public IQueryable<InspectionPicking> GetInspectionPickingData()
        {
            return _context.InspTranPickings.Where(x => x.Active && (x.LabAddressId != null || x.CusAddressId != null)).
                            Select(x => new InspectionPicking()
                            {
                                ProductRefId = x.PoTran.ProductRefId,
                                ProductId = x.PoTran.ProductRef.ProductId,
                                ProductCategory = x.PoTran.ProductRef.Product.ProductCategoryNavigation.Name,
                                ProductSubCategory = x.PoTran.ProductRef.Product.ProductSubCategoryNavigation.Name,
                                ProductName = x.PoTran.ProductRef.Product.ProductId,
                                PoNumber = x.PoTran.Po.Pono,
                                CustomerName = x.PoTran.Inspection.Customer.CustomerName,
                                SupplierName = x.PoTran.Inspection.Supplier.SupplierName,
                                FactoryName = x.PoTran.Inspection.Factory.SupplierName,
                                InspectionId = x.PoTran.InspectionId,
                                ServiceDateFrom = x.PoTran.Inspection.ServiceDateFrom,
                                ServiceDateTo = x.PoTran.Inspection.ServiceDateTo,
                                LabCustomerId = x.CustomerId,
                                LabAddressId = x.LabAddressId,
                                LabName = x.Lab.LabName,
                                CustomerAddressId = x.CusAddressId,
                                PickingQuantity = x.PickingQty
                            });
        }
        /// <summary>
        /// Get the lab details by list of lab ids
        /// </summary>
        /// <param name="labIds"></param>
        /// <returns></returns>
        public async Task<List<LabDetails>> GetLabDetails(List<int> labIds)
        {
            return await _context.InspLabDetails.Where(x => x.Active.HasValue && x.Active.Value && labIds.Contains(x.Id)).
                    Select(x => new LabDetails() { LabId = x.Id, LabName = x.LabName }).ToListAsync();
        }

        public async Task<List<KpiAdeoFollowUpDataRepo>> GetAdeoFollowUpProductDataEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.Value && x.ProductRef.Active.Value && bookingIdList.Contains(x.InspectionId))
                .Select(x => new KpiAdeoFollowUpDataRepo
                {
                    BookingId = x.InspectionId,
                    Etd = x.Etd,
                    PoNumber = x.Po.Pono,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    BookingStatus = x.Inspection.Status.Status,
                    ServiceDateFrom = x.Inspection.ServiceDateFrom,
                    ServiceDateTo = x.Inspection.ServiceDateTo,
                    ReportStatus = x.ProductRef.FbReport.Result.ResultName,
                    ProductName = x.ProductRef.Product.ProductId,
                    Office = x.Inspection.Office.LocationName,
                    CustomerId = x.Inspection.CustomerId,
                    SupplierId = x.Inspection.SupplierId,
                    StatusId = x.Inspection.StatusId,
                    ReportId = x.ProductRef.FbReportId,
                    ProductId = x.ProductRef.Product.Id,
                    Barcode = x.ProductRef.Product.Barcode,
                    ReportResultId = x.ProductRef.FbReport.ResultId,
                    FactoryName = x.Inspection.Factory.SupplierName,
                    FactoryId = x.Inspection.FactoryId,
                    ProductDescription = x.ProductRef.Product.ProductDescription,
                    BookingQty = x.ProductRef.TotalBookingQuantity
                }).ToListAsync();
        }

        public async Task<List<KpiAdeoFollowUpDataRepo>> GetAdeoFollowUpContainerDataEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.Value && x.ProductRef.Active.Value && bookingIdList.Contains(x.InspectionId)
            && x.ContainerRefId > 0)
                .Select(x => new KpiAdeoFollowUpDataRepo
                {
                    BookingId = x.InspectionId,
                    Etd = x.Etd,
                    PoNumber = x.Po.Pono,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    BookingStatus = x.Inspection.Status.Status,
                    ServiceDateFrom = x.Inspection.ServiceDateFrom,
                    ServiceDateTo = x.Inspection.ServiceDateTo,
                    ReportStatus = x.ContainerRef.FbReport.Result.ResultName,
                    ProductName = x.ProductRef.Product.ProductId,
                    Office = x.Inspection.Office.LocationName,
                    CustomerId = x.Inspection.CustomerId,
                    SupplierId = x.Inspection.SupplierId,
                    StatusId = x.Inspection.StatusId,
                    ReportId = x.ContainerRef.FbReportId,
                    ProductId = x.ProductRef.Product.Id,
                    Barcode = x.ProductRef.Product.Barcode,
                    ReportResultId = x.ContainerRef.FbReport.ResultId,
                    FactoryName = x.Inspection.Factory.SupplierName,
                    FactoryId = x.Inspection.FactoryId,
                    ProductDescription = x.ProductRef.Product.ProductDescription,
                    BookingQty = x.ProductRef.TotalBookingQuantity
                }).ToListAsync();
        }

        public async Task<List<KpiAdeoFollowUpDataRepo>> GetAdeoFailedContainerDataByEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.Value && x.ProductRef.Active.Value && bookingIdList.Contains(x.InspectionId)
            && x.ContainerRefId > 0 && x.ContainerRef.FbReport.ResultId == (int)FBReportResult.Fail)
                .Select(x => new KpiAdeoFollowUpDataRepo
                {
                    BookingId = x.InspectionId,
                    ReportStatus = x.ContainerRef.FbReport.Result.ResultName,
                    ProductName = x.ProductRef.Product.ProductId,
                    ReportId = x.ContainerRef.FbReportId,
                    ProductId = x.ProductRef.Product.Id,
                    Barcode = x.ProductRef.Product.Barcode,
                    ReportResultId = x.ContainerRef.FbReport.ResultId,
                    ProductDescription = x.ProductRef.Product.ProductDescription,
                    BookingQty = x.ProductRef.TotalBookingQuantity
                }).ToListAsync();
        }

        public async Task<List<KpiAdeoFollowUpDataRepo>> GetAdeoFailedProductDataByEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && bookingIdList.Contains(x.InspectionId) &&
            x.FbReport.ResultId == (int)FBReportResult.Fail)
                .Select(x => new KpiAdeoFollowUpDataRepo
                {
                    BookingId = x.InspectionId,
                    ReportStatus = x.FbReport.Result.ResultName,
                    ProductName = x.Product.ProductId,
                    ReportId = x.FbReportId,
                    ProductId = x.Product.Id,
                    Barcode = x.Product.Barcode,
                    ReportResultId = x.FbReport.ResultId,
                    ProductDescription = x.Product.ProductDescription,
                    BookingQty = x.TotalBookingQuantity
                }).ToListAsync();
        }

        public async Task<List<KpiReportCommentsTemplateRepo>> GetReportCommentsDataEfCore(IQueryable<int> bookingIdList)
        {
            return await _context.FbReportComments.
                Where(x => x.Active.Value && bookingIdList.Contains(x.FbReport.InspectionId.GetValueOrDefault()))
                .Select(x => new KpiReportCommentsTemplateRepo()
                {
                    BookingNo = x.FbReport.Inspection.Id,
                    CustomerBookingNo = x.FbReport.Inspection.CustomerBookingNo,
                    CustomerName = x.FbReport.Inspection.Customer.CustomerName,
                    CollectionName = x.FbReport.Inspection.Collection.Name,
                    Office = x.FbReport.Inspection.Office.LocationName,
                    SupplierName = x.FbReport.Inspection.Supplier.SupplierName,
                    FactoryName = x.FbReport.Inspection.Factory.SupplierName,
                    BookingStatus = x.FbReport.Inspection.Status.Status,
                    InspectionStartDate = x.FbReport.Inspection.FirstServiceDateFrom,
                    InspectionEndDate = x.FbReport.Inspection.ServiceDateTo,
                    Month = x.FbReport.Inspection.ServiceDateTo.Month,
                    Year = x.FbReport.Inspection.ServiceDateTo.Year,
                    ReportResult = x.FbReport.Result.ResultName,
                    ProductId = x.ProductId,
                    ReportComments = x.Comments,
                    ReportId = x.FbReportId,
                    RemarkCategory = x.Category,
                    RemarkSubCategory = x.SubCategory,
                    RemarkSubCategory2 = x.SubCategory2,
                    CustomerRemarkCodeReference = x.CustomerReferenceCode,
                    CustomerId = x.FbReport.Inspection.CustomerId,
                    SupplierId = x.FbReport.Inspection.SupplierId,
                    FactoryId = x.FbReport.Inspection.Factory.Id
                }).OrderBy(x => x.BookingNo).ToListAsync();
        }

        public async Task<List<CustomerDecisionData>> GetCustomerDecisionData(List<int> bookingIds)
        {
            return await _context.InspRepCusDecisions.Where(x => x.Active.Value && bookingIds.Contains(x.Report.InspectionId.Value))
                .Select(x => new CustomerDecisionData
                {
                    ReportId = x.ReportId,
                    BookingId = x.Report.InspectionId,
                    CustomerDecisionResultId = x.CustomerResultId,
                    CustomerDecisionName = x.CustomerResult.CustomDecisionName,
                    CustomerDecisionComment = x.Comments
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<KPIDefectPurchaseOrderRepo>> GetKpiDefectPurchaseOrderData(IQueryable<int> bookingIdList)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active == true && bookingIdList.Contains(x.InspectionId))
                .Select(x => new KPIDefectPurchaseOrderRepo()
                {
                    PoId = x.Id,
                    BookingId = x.InspectionId,
                    PONumber = x.Po.Pono,
                    ProductName = x.ProductRef.Product.ProductId,
                    ProductDescription = x.ProductRef.Product.ProductDescription,
                    FactoryRef = x.ProductRef.Product.FactoryReference,
                    ProductCategory = x.ProductRef.Product.ProductCategoryNavigation.Name,
                    ProductSubCategory = x.ProductRef.Product.ProductSubCategoryNavigation.Name,
                    ProductSubCategory2 = x.ProductRef.Product.ProductCategorySub2Navigation.Name
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ExpencesClaimsItem>> GetBookingExpense(List<int> bookingIds)
        {
            return await _context.EcExpensesClaimDetais.Where(x => x.Active.Value && bookingIds.Contains(x.InspectionId.Value))
                .Select(x => new ExpencesClaimsItem
                {
                    ExpenseId = x.ExpenseId,
                    ClaimNo = x.Expense.ClaimNo,
                    ClaimDetailId = x.Id,
                    Amount = x.Amount,
                    AmmountHk = x.AmmountHk,
                    InspectionId = x.InspectionId.Value
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<InspectionProductCountDto>> GetProductsCountByInspectionIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => bookingIds.Contains(x.InspectionId)
                        && x.Active.HasValue && x.Active.Value).GroupBy(x => x.InspectionId)
                        .Select(x => new InspectionProductCountDto
                        {
                            InspectionId = x.Key,
                            ProductCount = x.Select(y => y.ProductId).Distinct().Count()
                        }).AsNoTracking().ToListAsync();
        }

        public async Task<List<InspectionQcKpiExpenseDetails>> GetQcKpiBookingExpenseDetails(IQueryable<int> bookingIds)
        {
            return await _context.EcExpensesClaimDetais.Where(x => x.Active.Value && x.Expense.StatusId != (int)ExpenseClaimStatus.Cancelled && bookingIds.Contains(x.InspectionId.Value))
                .Select(x => new InspectionQcKpiExpenseDetails
                {
                    InspectionId = x.InspectionId.GetValueOrDefault(),
                    QcId = x.Expense.StaffId,
                    ExpenseId = x.Expense.Id,
                    ClaimNumber = x.Expense.ClaimNo,
                    ClaimDate = x.Expense.ClaimDate,
                    OutsourceCompany = x.Expense.Staff.HroutSourceCompany.Name,
                    ClaimStatus = x.Expense.Status.Description,
                    OrderStatus = x.Inspection.Status.Status,
                    NumberOfManDay = x.ManDay,
                    ClaimAmount = x.Amount,
                    ClaimAmountHK = x.AmmountHk,
                    ServiceTax = x.TaxAmount,
                    ExpenseType = x.ExpenseTypeId,
                    ClaimRemarks = x.Expense.ExpensePurpose,
                    StartCity = x.StartCity.CityName,
                    EndCity = x.ArrivalCity.CityName,
                    ExpenseDate = x.ExpenseDate,
                    ExpenseTypeName = x.ExpenseType.Description,
                    TripTypeName = x.TripTypeNavigation.Name
                }).AsNoTracking().ToListAsync();
        }
        public async Task<List<InspectionQcKpiInvoiceDetails>> GetQcKpiBookingInvoiceDetails(IQueryable<int> bookingIds)
        {
            return await _context.InvAutTranDetails
                .Where(x => x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled && bookingIds.Contains(x.InspectionId.Value))
                .Select(x => new InspectionQcKpiInvoiceDetails
                {
                    InspectionId = x.InspectionId.GetValueOrDefault(),
                    InvoiceId = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceCurrency = x.InvoiceCurrencyNavigation.CurrencyName,
                    InvoiceCurrencyCode = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    InvoiceInspectionFees = x.InspectionFees,
                    InvoiceTravellingFees = x.TravelTotalFees,
                    InvoiceOtherFees = x.OtherFees,
                    InvoiceHotelFees = x.HotelFees,
                    InvoiceTotalFees = x.TotalInvoiceFees,
                    InvoiceTotalTax = x.TotalTaxAmount,
                    InvoiceManDay = x.ManDays
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CommonIdDate>> GetDecisionDateByReportId(List<int> reportIdlist)
        {
            return await _context.InspRepCusDecisions
                .Where(x => x.Active.Value && reportIdlist.Contains(x.ReportId))
                .Select(y => new CommonIdDate
                {
                    Id = y.ReportId,
                    Date = y.CreatedOn,
                    Name = y.CustomerResult.CustomDecisionName ?? y.CustomerResult.CusDec.Name
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<GapCustomerKpiReportData>> GetGapKpiReportData(IQueryable<int> bookingIds)
        {
            return await _context.FbReportDetails.Where(x => x.Active == true && bookingIds.Contains(x.InspectionId.GetValueOrDefault()) && x.FbReportStatus == (int)FBStatus.ReportValidated)
                .Select(x => new GapCustomerKpiReportData()
                {
                    ReportId = x.Id,
                    InspectionId = x.InspectionId.GetValueOrDefault(),
                    DACorrelationDone = x.DacorrelationDone,
                    DACorrelationEmail = x.DacorrelationEmail,
                    DACorrelationRate = x.DacorrelationRate,
                    FactoryTourDone = x.FactoryTourDone,
                    InspectionStartedDate = x.InspectionStartedDate,
                    InspectionSubmittedDate = x.InspectionSubmittedDate,
                    KeyStyleHighRisk = x.KeyStyleHighRisk,
                    Region = x.Region,
                    ReportResult = x.Result.ResultName,
                    InspectionEndTime = x.InspectionEndTime,
                    InspectionStartTime = x.InspectionStartTime,
                    ExternalReportNumber = x.ExternalReportNumber,
                    ProductCategory = x.ProductCategory,
                    LastAuditScore = x.LastAuditScore,
                    OtherCategory = x.Othercategory,
                    Market = x.Market,
                    TotalScore = x.TotalScore,
                    Grade = x.Grade,
                    ReportNo = x.ReportTitle
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Invoice Communication by invoice no list
        /// </summary>
        /// <param name="invoiceNoList"></param>
        /// <returns></returns>
        public async Task<List<InvoiceCommunication>> GetInvoiceCommunicationByInvoiceNo(List<string> invoiceNoList)
        {
            return await _context.InvAutTranCommunications.Where(x => x.Active.Value && invoiceNoList.Contains(x.InvoiceNumber.Trim().ToLower())).
                Select(x => new InvoiceCommunication() { InvoiceNo = x.InvoiceNumber, Comment = x.Comment }).AsNoTracking().ToListAsync();
        }

        public IQueryable<AudTransaction> GetAllAuditQuery()
        {
            return _context.AudTransactions
                               .OrderByDescending(x => x.ServiceDateTo);
        }

        public async Task<List<KpiAuditBookingItems>> GetAuditItemsbyAuditIdAsQuery(IQueryable<int> auditIds)
        {
            return await _context.AudTransactions.Where(x => auditIds.Contains(x.Id))
                .Select(x => new KpiAuditBookingItems
                {
                    AuditId = x.Id,
                    CustomerId = x.CustomerId,
                    SupplierId = x.SupplierId,
                    FactoryId = x.FactoryId,
                    CustomerName = x.Customer.CustomerName,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    Grade = x.Grade,
                    LastAuditScore = x.LastAuditScore,
                    MainCategory = x.MainCategory,
                    Market = x.Market,
                    OtherCategory = x.OtherCategory,
                    ReportNo = x.ReportNo,
                    ServiceDateTo = x.ServiceDateTo,
                    AuditStartTime = x.AuditStartTime,
                    AuditSubmittedTime = x.AuditEndTime,
                    AuditProdutCategory = x.CuProductCategoryNavigation.Name,
                    PivotAudit = x.ExternalReportNo,
                    ScoreValues = x.ScoreValue
                }).OrderByDescending(x => x.ServiceDateTo).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetAuditXeroInvoiceFirstSetItems(IQueryable<int> auditIds, List<int> invoiceTypeList)
        {
            return await _context.InvAutTranDetails.Where(x => auditIds.Contains(x.AuditId.GetValueOrDefault())
                    && invoiceTypeList.Contains(x.InvoiceType.GetValueOrDefault())
                    && x.InvoiceStatus != (int)InvoiceStatus.Cancelled && x.InspectionFees.GetValueOrDefault() > 0)
                        .Select(x => new XeroInvoiceData
                        {
                            ContactName = x.InvoicedName,
                            EmailAddress = "",
                            POAddressLine1 = "",
                            POAddressLine2 = "",
                            POAddressLine3 = "",
                            POAddressLine4 = "",
                            POCity = "",
                            PORegion = "",
                            POPostalCode = "",
                            POCountry = "",
                            InvoiceNumber = x.InvoiceNo,
                            Reference = x.Audit.Customer.CustomerName,
                            InvoiceDate = x.PostedDate != null ? x.PostedDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                            DueDate = x.InvoiceDate != null ? x.PaymentDuration != null
                            && x.PaymentDuration != "" ?
                            x.InvoiceDate.GetValueOrDefault().AddDays(int.Parse(x.PaymentDuration)).ToString(StandardDateFormat)
                            : x.InvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                            InventoryItemCode = "",
                            Description = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                            Quantity = 1,
                            UnitAmount = x.InspectionFees.GetValueOrDefault() + x.InvExfTransactions.Sum(x => x.ExtraFeeSubTotal.GetValueOrDefault()),
                            Discount = x.Discount.GetValueOrDefault(),
                            AccountCode = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).
                                          ServiceType.RefServiceTypeXeros.FirstOrDefault().XeroAccount,

                            TaxType = x.TaxValue.GetValueOrDefault().ToString() != "" ?
                            x.Bank.TaxNameInXero.Replace("0", (x.TaxValue.GetValueOrDefault() * 100).ToString()) : "Tax Exempt (0%)",

                            TrackingName1 = "Services and Departments",
                            TrackingOption1 = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).
                                          ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionName,
                            TrackingName2 = "Location",
                            TrackingOption2 = x.Audit.Factory.SuAddresses.
                            FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
                            Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                            BrandingTheme = "",
                            AccountName = x.Bank.AccountName
                        }).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetAuditXeroxInvoiceSecondSetItems(IQueryable<int> auditIds, List<int> invoiceTypeList)
        {
            return await _context.InvAutTranDetails.Where(x => auditIds.Contains(x.AuditId.GetValueOrDefault())
                    && invoiceTypeList.Contains(x.InvoiceType.GetValueOrDefault())
                    && x.InvoiceStatus != (int)InvoiceStatus.Cancelled && (x.TravelTotalFees.GetValueOrDefault() > 0 || x.HotelFees.GetValueOrDefault() > 0))
                        .Select(x => new XeroInvoiceData
                        {
                            ContactName = x.InvoicedName,
                            EmailAddress = "",
                            POAddressLine1 = "",
                            POAddressLine2 = "",
                            POAddressLine3 = "",
                            POAddressLine4 = "",
                            POCity = "",
                            PORegion = "",
                            POPostalCode = "",
                            POCountry = "",
                            InvoiceNumber = x.InvoiceNo,
                            Reference = x.Audit.Customer.CustomerName,
                            InvoiceDate = x.PostedDate != null ? x.PostedDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                            DueDate = x.InvoiceDate != null ? x.PaymentDuration != null
                            && x.PaymentDuration != "" ?
                            x.InvoiceDate.GetValueOrDefault().AddDays(int.Parse(x.PaymentDuration)).ToString(StandardDateFormat)
                            : x.InvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                            InventoryItemCode = "",
                            Description = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                            Quantity = 1,
                            UnitAmount = x.TravelTotalFees.GetValueOrDefault() + x.HotelFees.GetValueOrDefault(),
                            Discount = x.Discount.GetValueOrDefault(),
                            AccountCode = "700100",
                            TaxType = x.TaxValue.GetValueOrDefault().ToString() != "" ?
                            x.Bank.TaxNameInXero.Replace("0", (x.TaxValue.GetValueOrDefault() * 100).ToString()) : "Tax Exempt (0%)",
                            TrackingName1 = "Services and Departments",
                            TrackingOption1 = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).
                                          ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionNameTravel,
                            TrackingName2 = "Location",
                            TrackingOption2 = x.Audit.Factory.SuAddresses.
                            FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
                            Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                            BrandingTheme = "",
                            AccountName = x.Bank.AccountName
                        }).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetAuditXeroxInvoiceThirdSetItems(IQueryable<int> auditIds, List<int> invoiceTypeList)
        {
            return await _context.InvAutTranDetails.Where(x => auditIds.Contains(x.AuditId.GetValueOrDefault())
                        && invoiceTypeList.Contains(x.InvoiceType.GetValueOrDefault())
                        && x.InvoiceStatus != (int)InvoiceStatus.Cancelled && x.OtherFees.GetValueOrDefault() > 0)
                            .Select(x => new XeroInvoiceData
                            {
                                ContactName = x.InvoicedName,
                                EmailAddress = "",
                                POAddressLine1 = "",
                                POAddressLine2 = "",
                                POAddressLine3 = "",
                                POAddressLine4 = "",
                                POCity = "",
                                PORegion = "",
                                POPostalCode = "",
                                POCountry = "",
                                InvoiceNumber = x.InvoiceNo,
                                Reference = x.Audit.Customer.CustomerName,
                                InvoiceDate = x.PostedDate != null ? x.PostedDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                                DueDate = x.InvoiceDate != null ? x.PaymentDuration != null
                                && x.PaymentDuration != "" ?
                                x.InvoiceDate.GetValueOrDefault().AddDays(int.Parse(x.PaymentDuration)).ToString(StandardDateFormat)
                                : x.InvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                                InventoryItemCode = "",
                                Description = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                                Quantity = 1,
                                UnitAmount = x.OtherFees.GetValueOrDefault(),
                                Discount = x.Discount.GetValueOrDefault(),
                                AccountCode = "700200",
                                TaxType = x.TaxValue.GetValueOrDefault().ToString() != "" ?
                                x.Bank.TaxNameInXero.Replace("0", (x.TaxValue.GetValueOrDefault() * 100).ToString()) : "Tax Exempt (0%)",
                                TrackingName1 = "Services and Departments",
                                TrackingOption1 = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).
                                                ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionNameTravel,
                                TrackingName2 = "Location",
                                TrackingOption2 = x.Audit.Factory.SuAddresses.
                                FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
                                Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                                BrandingTheme = "",
                                AccountName = x.Bank.AccountName
                            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetAuditXeroInvoiceFifthSetItems(IQueryable<int> auditIds)
        {
            return await _context.InvExfTransactions.Where(x => x.StatusId == (int)ExtraFeeStatus.Invoiced && x.InvoiceId == null && x.Active.Value && auditIds.Contains(x.AuditId.GetValueOrDefault()))
                    .Select(x => new XeroInvoiceData
                    {
                        ContactName = x.BilledName,
                        EmailAddress = "",
                        POAddressLine1 = "",
                        POAddressLine2 = "",
                        POAddressLine3 = "",
                        POAddressLine4 = "",
                        POCity = "",
                        PORegion = "",
                        POPostalCode = "",
                        POCountry = "",
                        InvoiceNumber = x.ExtraFeeInvoiceNo,
                        Reference = x.Audit.Customer.CustomerName,
                        InvoiceDate = x.ExtraFeeInvoiceDate != null ? x.ExtraFeeInvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                        DueDate = x.ExtraFeeInvoiceDate != null ? x.PaymentDuration != null ?
                        x.ExtraFeeInvoiceDate.GetValueOrDefault().AddDays(x.PaymentDuration.GetValueOrDefault()).ToString(StandardDateFormat)
                        : x.ExtraFeeInvoiceDate.GetValueOrDefault().ToString(StandardDateFormat) : "",
                        InventoryItemCode = "",
                        Description = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).ServiceType.Name,
                        Quantity = 1,
                        UnitAmount = x.ExtraFeeSubTotal.GetValueOrDefault(),
                        Discount = 0,
                        AccountCode = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).
                                      ServiceType.RefServiceTypeXeros.FirstOrDefault().XeroAccount,
                        TaxType = x.Tax.GetValueOrDefault().ToString() != "" ?
                        x.Bank.TaxNameInXero.Replace("0", (x.Tax.GetValueOrDefault() * 100).ToString()) : "Tax Exempt (0%)",
                        TrackingName1 = "Services and Departments",
                        TrackingOption1 = x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).
                                      ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionName,
                        TrackingName2 = "Location",
                        TrackingOption2 = x.Audit.Factory.SuAddresses.
                        FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
                        Currency = x.InvoiceCurrency.CurrencyCodeA,
                        BrandingTheme = "",
                        AccountName = x.Bank.AccountName
                    }).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetAuditXeroExpenseItems(IQueryable<int> auditIds)
        {
            var accountCodeList = new List<string>() { "800700", "801820", "801920" };
            return await _context.EcExpensesClaimDetais.Where(x => auditIds.Contains(x.AuditId.GetValueOrDefault())
            && x.Active.Value && x.Expense.Active.Value && x.Amount > 0 && x.Expense.StatusId != (int)ExpenseClaimStatus.Cancelled)
                .Select(x => new XeroInvoiceData
                {
                    ContactName = x.Expense.Staff.PersonName,
                    EmailAddress = x.Expense.Staff.CompanyEmail,
                    POAddressLine1 = "",
                    POAddressLine2 = "",
                    POAddressLine3 = "",
                    POAddressLine4 = "",
                    POCity = "",
                    PORegion = "",
                    POPostalCode = "",
                    POCountry = "",
                    InvoiceNumber = x.Expense.ClaimNo,
                    Reference = "",
                    InvoiceDate = x.ExpenseDate.ToString(StandardDateFormat),
                    DueDate = null,
                    InventoryItemCode = "",
                    Description = x.Audit.Customer.CustomerName != "" ?
                                    x.ExpenseType.Description + "-" + x.Audit.Customer.CustomerName
                                    : x.ExpenseType.Description,
                    Quantity = 1,
                    UnitAmount = x.Amount,
                    Discount = 0,
                    AccountCode = x.Expense.Staff.EmployeeTypeId == (int)EmployeeTypeEnum.Permanent ? x.ExpenseType.XeroAccountCode : x.ExpenseType.XeroOutSourceAccountCode,
                    TaxType = "Tax Exempt (0%)",
                    TrackingName1 = "Services and Departments",
                    TrackingOption1 = accountCodeList.Contains(x.ExpenseType.XeroAccountCode)
                                      && x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).
                                      ServiceType.RefServiceTypeXeros.FirstOrDefault().Id > 0 ?
                                      x.Audit.AudTranServiceTypes.FirstOrDefault(x => x.Active).
                                      ServiceType.RefServiceTypeXeros.FirstOrDefault().TrackingOptionNameTravel : x.Expense.Staff.Department.DepartmentName,
                    TrackingName2 = "Location",
                    TrackingOption2 = x.Expense.Country.CountryName,
                    Currency = x.Currency.CurrencyCodeA,
                    BrandingTheme = "",
                    AccountName = x.Expense.Staff.PayrollCompanyNavigation.CompanyName
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<XeroInvoiceData>> GetNonInspectionXeroExpenseItems(KpiRequest request)
        {
            return await _context.EcExpensesClaimDetais.Where(x => x.Active.Value && x.Expense.Active.Value && x.Expense.ClaimTypeId.GetValueOrDefault() == (int)ExpenseBookingDetailEnum.NonInspection &&
                x.ExpenseDate >= request.FromDate.ToDateTime() && x.ExpenseDate <= request.ToDate.ToDateTime() && x.Amount > 0 && x.Expense.StatusId != (int)ExpenseClaimStatus.Cancelled)
               .Select(x => new XeroInvoiceData
               {
                   ContactName = x.Expense.Staff.PersonName,
                   EmailAddress = x.Expense.Staff.CompanyEmail,
                   POAddressLine1 = "",
                   POAddressLine2 = "",
                   POAddressLine3 = "",
                   POAddressLine4 = "",
                   POCity = "",
                   PORegion = "",
                   POPostalCode = "",
                   POCountry = "",
                   InvoiceNumber = x.Expense.ClaimNo,
                   Reference = "",
                   InvoiceDate = x.ExpenseDate.ToString(StandardDateFormat),
                   DueDate = null,
                   InventoryItemCode = "",
                   Description = x.ExpenseType.Description,
                   Quantity = 1,
                   UnitAmount = x.Amount,
                   Discount = 0,
                   AccountCode = x.Expense.Staff.EmployeeTypeId == (int)EmployeeTypeEnum.Permanent ? x.ExpenseType.XeroAccountCode : x.ExpenseType.XeroOutSourceAccountCode,
                   TaxType = "Tax Exempt (0%)",
                   TrackingName1 = "Services and Departments",
                   TrackingOption1 = x.Expense.Staff.Department.DepartmentName,
                   TrackingName2 = "Location",
                   TrackingOption2 = x.Expense.Country.CountryName,
                   Currency = x.Currency.CurrencyCodeA,
                   BrandingTheme = "",
                   AccountName = x.Expense.Staff.PayrollCompanyNavigation.CompanyName
               }).AsNoTracking().ToListAsync();
        }
    }
}
