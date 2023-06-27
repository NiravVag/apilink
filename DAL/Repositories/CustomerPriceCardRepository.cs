using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerPriceCardRepository : Repository, ICustomerPriceCardRepository
    {
        public CustomerPriceCardRepository(API_DBContext context) : base(context)
        {

        }

        //get customer price card records
        public async Task<CuPrDetail> GetCustomerPriceCardDetails(int? id)
        {
            return await _context.CuPrDetails.Where(x => x.Active.Value && x.Id == id).
                Include(x => x.CuPrCountries).
                Include(x => x.CuPrProductCategories).
                Include(x => x.CuPrProductSubCategories).
                Include(x => x.CuPrProvinces).
                Include(x=>x.CuPrCities).
                Include(x => x.CuPrServiceTypes).
                Include(x => x.CuPrSuppliers).
                Include(x => x.CuPrDepartments).
                Include(x => x.CuPrBrands).
                Include(x => x.CuPrBuyers).
                Include(x => x.CuPrPriceCategories).
                Include(x => x.CuPrHolidayTypes).
                Include(x => x.InvTranInvoiceRequestContacts).
                Include(x => x.CuPrInspectionLocations).
                Include(x => x.CuPrTranSubcategories).
                Include(x => x.CuPrTranSpecialRules).
                Include(x => x.InvTranInvoiceRequests).
                ThenInclude(x => x.InvTranInvoiceRequestContacts).
                FirstOrDefaultAsync();
        }

        //fetch customer price card details
        public async Task<CustomerPriceCardRepo> GetCustomerPriceCardDetail(int? id)
        {
            return await _context.CuPrDetails.Where(x => x.Active.Value && x.Id == id).
                Select(x => new CustomerPriceCardRepo
                {
                    TariffTypeId = x.TravelMatrixTypeId,
                    BillingMethodId = x.BillingMethodId,
                    BillingToId = x.BillingToId,
                    CustomerId = x.CustomerId,
                    CurrencyId = x.CurrencyId,
                    Remarks = x.Remarks,
                    FreeTravelKM = x.FreeTravelKm.GetValueOrDefault(),
                    Id = x.Id,
                    ServiceId = x.ServiceId,
                    TaxIncluded = x.TaxIncluded == null ? null : x.TaxIncluded,
                    TravelIncluded = x.TravelIncluded == null ? null : x.TravelIncluded,
                    UnitPrice = x.UnitPrice,
                    HolidayPrice = x.HolidayPrice,
                    ProductPrice = x.ProductPrice,
                    PriceToEachProduct = x.PriceToEachProduct,
                    PeriodFrom = Static_Data_Common.GetCustomDate(x.PeriodFrom),
                    PeriodTo = Static_Data_Common.GetCustomDate(x.PeriodTo),

                    MaxProductCount = x.MaxProductCount,
                    SampleSizeBySet = x.SampleSizeBySet,
                    MinBillingDay = x.MinBillingDay,
                    MaxSampleSize = x.MaxSampleSize,
                    AdditionalSampleSize = x.AdditionalSampleSize,
                    AdditionalSamplePrice = x.AdditionalSamplePrice,
                    Quantity8 = x.Quantity8,
                    Quantity13 = x.Quantity13,
                    Quantity20 = x.Quantity20,
                    Quantity32 = x.Quantity32,
                    Quantity50 = x.Quantity50,
                    Quantity80 = x.Quantity80,
                    Quantity125 = x.Quantity125,
                    Quantity200 = x.Quantity200,
                    Quantity315 = x.Quantity315,
                    Quantity500 = x.Quantity500,
                    Quantity800 = x.Quantity800,
                    Quantity1250 = x.Quantity1250,

                    InvoiceRequestSelectAll = x.InvoiceRequestSelectAll,

                    IsInvoiceConfigured = x.IsInvoiceConfigured,

                    InvoiceRequestType = x.InvoiceRequestType,
                    PriceComplexType = x.PriceComplexType,
                    InvoiceRequestAddress = x.InvoiceRequestAddress,
                    InvoiceRequestBilledName = x.InvoiceRequestBilledName,

                    BillingEntity = x.BillingEntity,
                    BankAccount = x.BankAccount,
                    PaymentDuration = x.PaymentDuration,
                    PaymentTerms = x.PaymentTerms,

                    InvoiceNoDigit = x.InvoiceNoDigit,
                    InvoiceNoPrefix = x.InvoiceNoPrefix,

                    InvoiceOffice = x.InvoiceOffice,
                    InvoiceInspFeeFrom = x.InvoiceInspFeeFrom,
                    InvoiceHotelFeeFrom = x.InvoiceHotelFeeFrom,
                    InvoiceDiscountFeeFrom = x.InvoiceDiscountFeeFrom,
                    InvoiceOtherFeeFrom = x.InvoiceOtherFeeFrom,
                    InvoiceTravelExpense = x.InvoiceTmfeeFrom,

                    InterventionType = x.InterventionType,
                    BillQuantityType = x.BilledQuantityType,
                    BillFrequency = x.BillingFreequency,
                    MaxFeeStyle = x.MaxFeeStyle,
                    InvoiceSubject = x.InvoiceSubject,
                    IsSpecial = x.IsSpecial,
                    SubCategorySelectAll = x.SubCategorySelectAll,

                    MandayReports = x.MandayReportCount,
                    MandayProductivity = x.ManDayProductivity,
                    MandayBuffer = x.MandayBuffer

                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PriceInvoiceRequest>> GetCustomerPriceCardInvoiceRequest(int? priceCardId)
        {
            return await _context.InvTranInvoiceRequests.
                             Where(x => x.Active.Value && x.CuPriceCardId == priceCardId).
                             Select(x => new PriceInvoiceRequest()
                             {
                                 Id = x.Id,
                                 BilledName = x.BilledName,
                                 BilledAddress = x.BilledAddress,
                                 BrandId = x.BrandId,
                                 DepartmentId = x.DepartmentId,
                                 BuyerId = x.BuyerId,
                                 IsCommon = true,
                                 CuPriceCardId = x.CuPriceCardId,
                                 //InvoiceRequestContactList = x.InvTranInvoiceRequestContacts.Where(y => !y.IsCommon.Value && y.Active.Value).Select(y => y.ContactId.GetValueOrDefault()).ToList()

                             }).ToListAsync();
        }

        public async Task<IEnumerable<PriceSubCategory>> GetCustomerPriceCardSubCategory(int? priceCardId)
        {
            return await _context.CuPrTranSubcategories.
                             Where(x => x.Active.Value && x.CuPriceId == priceCardId).
                             Select(x => new PriceSubCategory()
                             {
                                 Id = x.Id,
                                 CuPriceCardId = x.CuPriceId,
                                 SubCategory2Id = x.SubCategory2Id,
                                 SubCategoryName = x.SubCategory2.Name,
                                 SubCategory2Name = x.SubCategory2.Name,
                                 UnitPrice = x.UnitPrice,
                                 IsCommon = true,
                                 MandayProductivity = x.MandayProductivity,
                                 MandayBuffer = x.MandayBuffer,
                                 MandayReports = x.MandayReports,
                                 AQL_QTY_125 = x.AqlQty125,
                                 AQL_QTY_1250 = x.AqlQty1250,
                                 AQL_QTY_13 = x.AqlQty13,
                                 AQL_QTY_20 = x.AqlQty20,
                                 AQL_QTY_200 = x.AqlQty200,
                                 AQL_QTY_315 = x.AqlQty315,
                                 AQL_QTY_32 = x.AqlQty32,
                                 AQL_QTY_50 = x.AqlQty50,
                                 AQL_QTY_500 = x.AqlQty500,
                                 AQL_QTY_8 = x.AqlQty8,
                                 AQL_QTY_80 = x.AqlQty80,
                                 AQL_QTY_800 = x.AqlQty800
                             }).ToListAsync();
        }


        public async Task<IEnumerable<PriceSpecialRule>> GetCustomerPriceRuleList(int? priceCardId)
        {
            return await _context.CuPrTranSpecialRules.
                             Where(x => x.Active.Value && x.CuPriceId == priceCardId).
                             Select(x => new PriceSpecialRule()
                             {
                                 Id = x.Id,
                                 CuPriceCardId = x.CuPriceId,
                                 UnitPrice = x.UnitPrice,
                                 MandayProductivity = x.MandayProductivity,
                                 MandayReports = x.MandayReports,
                                 Max_Style_Per_Day = x.MaxStylePerDay,
                                 Max_Style_per_Month = x.MaxStylePerMonth,
                                 Max_Style_Per_Week = x.MaxStylePerWeek,
                                 AdditionalFee = x.AdditionalFee,
                                 Interventionfee = x.InterventionFee,
                                 PieceRate_Billing_Q_Start = x.PieceRateBillingQStart,
                                 Piecerate_Billing_Q_End = x.PiecerateBillingQEnd,
                                 Piecerate_MinBilling = x.PiecerateMinBilling,
                                 PerInterventionRange1 = x.PerInterventionRange1,
                                 PerInterventionRange2 = x.PerInterventionRange2
                             }).ToListAsync();
        }

        //get all data details
        public IQueryable<CuPrDetail> GetAllData()
        {
            return _context.CuPrDetails.Where(x => x.Active.Value);
            //Select(x => new CustomerPriceCardSummaryData
            //{
            //    CustomerId = x.CustomerId,
            //    Id = x.Id,
            //    BillingMethodId = x.BillingMethodId,
            //    BillingToId = x.BillingToId,
            //    ServiceId = x.ServiceId,
            //    UnitPrice = x.UnitPrice,

            //    BillMethodName = x.BillingMethod.Label,
            //    BillToName = x.BillingTo.Label,
            //    CurrencyName = x.Currency.CurrencyName,
            //    CustomerName = x.Customer.CustomerName,
            //    ServiceName = x.Service.Name,

            //    DepartmentIdList = x.CuPrDepartments.Where(y => y.Active.Value).Select(y => y.Department.Id).ToList(),
            //    PriceCategoryIdList = x.CuPrPriceCategories.Where(y => y.Active.Value).Select(y => y.PriceCategory.Id).ToList(),

            //    DepartmentNameList = x.CuPrDepartments.Where(y => y.Active.Value).Select(y => y.Department.Name).ToList(),
            //    PriceCategoryList = x.CuPrPriceCategories.Where(y => y.Active.Value).Select(y => y.PriceCategory.Name).ToList(),

            //    TravelIncluded = x.TravelIncluded,
            //    TaxIncluded = x.TaxIncluded,

            //    CountryIdList = x.CuPrCountries.Where(y => y.Active.Value).Select(y => y.FactoryCountryId).ToList(),
            //    ProductCategoryIdList = x.CuPrProductCategories.Where(y => y.Active.Value).Select(y => y.ProductCategoryId).ToList(),
            //    ServiceTypeIdList = x.CuPrServiceTypes.Where(y => y.Active.Value).Select(y => y.ServiceTypeId).ToList(),

            //    CountryIdNameList = x.CuPrCountries.Where(y => y.Active.Value).Select(y => y.FactoryCountry.CountryName).ToList(),
            //    ServiceTypeNameList = x.CuPrServiceTypes.Where(y => y.Active.Value).Select(y => y.ServiceType.Name).ToList(),
            //    SupplierNameList = x.CuPrSuppliers.Where(y => y.Active.Value).Select(y => y.Supplier.SupplierName).ToList(),
            //    ProductCategoryNameList = x.CuPrProductCategories.Where(y => y.Active.Value).Select(y => y.ProductCategory.Name).ToList(),
            //    FactoryProvinceList = x.CuPrProvinces.Where(y => y.Active.Value).Select(y => y.FactoryProvince.ProvinceName).ToList(),

            //    FreeTraveKM = x.FreeTravelKm,
            //    Remarks = x.Remarks,

            //    PeriodFrom = x.PeriodFrom,
            //    PeriodTo = x.PeriodTo,
            //});
        }

        //customer price card exists check with DB
        public IQueryable<CuPrDetail> IsExists(CustomerPriceCard request)
        {
            return _context.CuPrDetails.Where(x => x.Active.Value);
        }

        //get unit price and filter data based on customer and service id
        public IQueryable<CuPrDetail> GetCustomerUnitPriceByCustomerIdServiceId(int customerId, int serviceId)
        {
            return _context.CuPrDetails.Where(x => x.CustomerId == customerId && x.ServiceId == serviceId && x.Active.Value);
            //.Select(x => new CustomerPriceCardDetails
            //{
            //    Id = x.Id,
            //    UnitPrice = x.UnitPrice,
            //    CountryIdList = x.CuPrCountries.Where(y => y.Active.Value).Select(y => y.FactoryCountryId).ToList(),
            //    ProductCategoryIdList = x.CuPrProductCategories.Where(y => y.Active.Value).Select(y => (int?)y.ProductCategoryId).ToList(),
            //    ServiceTypeIdList = x.CuPrServiceTypes.Where(y => y.Active.Value).Select(y => y.ServiceTypeId).ToList(),
            //    ProvinceIdList = x.CuPrProvinces.Where(y => y.Active.Value).Select(y => y.FactoryProvinceId).ToList(),
            //    SupplierIdList = x.CuPrSuppliers.Where(y => y.Active.Value).Select(y => y.SupplierId).ToList(),
            //    BrandIdList = x.CuPrBrands.Where(y => y.Active.Value).Select(y => y.BrandId.GetValueOrDefault()).ToList(),
            //    BuyerIdList = x.CuPrBuyers.Where(y => y.Active.Value).Select(y => y.BuyerId.GetValueOrDefault()).ToList(),
            //    DepartmentIdList = x.CuPrDepartments.Where(y => y.Active.Value).Select(y => y.DepartmentId.GetValueOrDefault()).ToList(),
            //    PriceCategoryList = x.CuPrPriceCategories.Where(y => y.Active.Value).Select(y => y.PriceCategoryId.GetValueOrDefault()).ToList(),
            //    HolidayTypeList = x.CuPrHolidayTypes.Where(y => y.Active.HasValue && x.Active.Value).ToList(),
            //    PeriodFrom = x.PeriodFrom.GetValueOrDefault(),
            //    PeriodTo = x.PeriodTo.GetValueOrDefault(),
            //    BillMethodId = x.BillingMethodId,
            //    BillToId = x.BillingToId,
            //    CurrencyId = x.CurrencyId,
            //    TravelMatrixTypeId = x.TravelMatrixTypeId,
            //    HolidayPrice = x.HolidayPrice,
            //    TaxIncluded = x.TaxIncluded,
            //    TravelIncluded = x.TravelIncluded,
            //    BillingMethodName = x.BillingMethod.Label,
            //    BillingTo = x.BillingTo.Label,
            //    CurrencyName = x.Currency.CurrencyName
            //}).ToListAsync();
        }
        //get customer price card details
        public async Task<List<QuotationCustomerPriceCardData>> GetCustomerPriceCardData(IEnumerable<int> PriceIdList)
        {
            return await _context.CuPrDetails.Where(x => x.Active.Value && PriceIdList.Contains(x.Id)).
                Select(x => new QuotationCustomerPriceCardData
                {
                    Id = x.Id,
                    Remarks = x.Remarks,
                    FreeTravelKM = x.FreeTravelKm,
                    TaxIncluded = x.TaxIncluded,
                    TravelIncluded = x.TravelIncluded,
                    UnitPrice = x.UnitPrice,
                    //ServiceTypeNameList = x.CuPrServiceTypes.Where(y => y.Active.Value).Select(y => y.ServiceType.Name).ToList(),
                    //ProductCategoryNameList = x.CuPrProductCategories.Where(y => y.Active.Value).Select(y => y.ProductCategory.Name).ToList(),
                    //FactoryCountryNameList = x.CuPrCountries.Where(y => y.Active.Value).Select(y => y.FactoryCountry.CountryName).ToList(),
                    //BrandNameList = x.CuPrBrands.Where(y => y.Active.HasValue && y.Active.Value).Select(y => y.Brand.Name).ToList(),
                    //BuyerNameList = x.CuPrBuyers.Where(y => y.Active.HasValue && y.Active.Value).Select(y => y.Buyer.Name).ToList(),
                    //PriceCategoryNameList = x.CuPrPriceCategories.Where(y => y.Active.HasValue && y.Active.Value).Select(y => y.PriceCategory.Name).ToList(),
                    //DepartmentNameList = x.CuPrDepartments.Where(y => y.Active.HasValue && y.Active.Value).Select(y => y.Department.Name).ToList(),
                    BillingMethodName = x.BillingMethod.Label,
                    BillingPaidBy = x.BillingTo.Label,
                    CustomerName = x.Customer.CustomerName,
                    PeriodFrom = x.PeriodFrom,
                    PeriodTo = x.PeriodTo,
                    TravelMatrixTypeId = x.TravelMatrixTypeId,
                    CurrencyId = x.CurrencyId,
                    BillingMethodId = x.BillingMethodId,
                    BillingPaidById = x.BillingToId,
                    CurrencyName = x.Currency.CurrencyName,
                    BillingEntityId = x.BillingEntity,
                    PaymentTermsValue = x.PaymentTerms,
                    PaymentTermsCount = x.PaymentDuration
                    //ProvinceNameList = x.CuPrProvinces.Where(y => y.Active.Value).Select(y => y.FactoryProvince.ProvinceName).ToList()
                }).ToListAsync();
        }



        public async Task<QuotationCustomerPriceCardData> GetQuotationCustomerPriceCardData(int ruleId)
        {
            return await _context.CuPrDetails.Where(x => x.Active.Value && x.Id == ruleId).
                Select(x => new QuotationCustomerPriceCardData
                {
                    Id = x.Id,
                    Remarks = x.Remarks,
                    FreeTravelKM = x.FreeTravelKm,
                    TaxIncluded = x.TaxIncluded,
                    TravelIncluded = x.TravelIncluded,
                    UnitPrice = x.UnitPrice,
                    BillingMethodName = x.BillingMethod.Label,
                    BillingPaidBy = x.BillingTo.Label,
                    CustomerName = x.Customer.CustomerName,
                    PeriodFrom = x.PeriodFrom,
                    PeriodTo = x.PeriodTo,
                    TravelMatrixTypeId = x.TravelMatrixTypeId,
                    CurrencyId = x.CurrencyId,
                    BillingMethodId = x.BillingMethodId,
                    BillingPaidById = x.BillingToId,
                    CurrencyName = x.Currency.CurrencyName
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the customer price holiday list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerPriceHolidayList()
        {
            return await _context.CuPrHolidayInfos.Where(x => x.Active.HasValue && x.Active.Value)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        //fetch customer price card details
        public async Task<List<ExportSummaryItem>> GetCustomerPriceCardDetailForExport(List<int> priceCardIsList)
        {
            return await _context.CuPrDetails.Where(x => x.Active.Value && priceCardIsList.Contains(x.Id)).
                Select(x => new ExportSummaryItem
                {
                    Id = x.Id,
                    CustomerName = x.Customer.CustomerName,
                    BillMethod = x.BillingMethod.Label,
                    BillPaidBy = x.BillingTo.Label,
                    Service = x.Service.Name,
                    CurrencyName = x.Currency.CurrencyName,
                    UnitPrice = x.UnitPrice,
                    FreeTravelKM = x.FreeTravelKm,
                    Remarks = x.Remarks,
                    PeriodFrom = x.PeriodFrom,
                    PeriodTo = x.PeriodTo,
                    TaxInclude = x.TaxIncluded,
                    TraveInclude = x.TravelIncluded,
                    MinBillingFeePerDay = x.MinBillingDay,
                    MaximumProductCount = x.MaxProductCount,
                    InvoiceRequest = x.InvoiceRequestTypeNavigation.Name,
                    BillingEntity = x.BillingEntityNavigation.Name,
                    InvoiceBank = x.BankAccountNavigation.BankName,
                    BilledName = x.InvoiceRequestBilledName,
                    BillingAddress = x.InvoiceRequestAddress,
                    InvoiceDigitalNo = x.InvoiceNoDigit,
                    InvoiceNoPrefix = x.InvoiceNoPrefix,
                    InvoiceOffice = x.InvoiceOfficeNavigation.Name,
                    PaymentTypeValue = x.PaymentTerms,
                    PaymentTerms = x.PaymentTerms,
                    InspectionFee = x.InvoiceInspFeeFromNavigation.Name,
                    TravelExpense = x.InvoiceTmfeeFromNavigation.Name,
                    Discount = x.InvoiceDiscountFeeFromNavigation.Name,
                    OtherFee = x.InvoiceOtherFeeFromNavigation.Name,
                    TariffType = x.TravelMatrixType.Name,
                    MaxSampleSize = x.MaxSampleSize,
                    CreatedByName = x.CreatedByNavigation.FullName,
                    CreatedOn = x.CreatedOn,
                    UpdatedByName = x.UpdatedByNavigation.FullName,
                    UpdatedOn = x.UpdatedOn
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the price card supplier
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrSuppliers(List<int> priceCardIdList)
        {
            return await _context.CuPrSuppliers.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPrId,
                    Id = x.SupplierId,
                    Name = x.Supplier.SupplierName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card product categories
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrProductCategories(List<int> priceCardIdList)
        {
            return await _context.CuPrProductCategories.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPrId,
                    Id = x.ProductCategoryId,
                    Name = x.ProductCategory.Name
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get price product sub category info 
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrProductSubCategories(List<int> priceCardIdList)
        {
            return await _context.CuPrProductSubCategories.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPrId,
                    Id = x.ProductSubCategoryId,
                    Name = x.ProductSubCategory.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card service types
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrServiceTypes(List<int> priceCardIdList)
        {
            return await _context.CuPrServiceTypes.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPrId,
                    Id = x.ServiceType.Id,
                    Name = x.ServiceType.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card countries
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrCountries(List<int> priceCardIdList)
        {
            return await _context.CuPrCountries.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CuPrCommonDataSource
                {
                    Id = x.FactoryCountry.Id,
                    PriceId = x.CuPrId,
                    Name = x.FactoryCountry.CountryName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card provinces
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrProvince(List<int> priceCardIdList)
        {
            return await _context.CuPrProvinces.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPrId,
                    Id = x.FactoryProvinceId,
                    Name = x.FactoryProvince.ProvinceName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card departments
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrDepartment(List<int> priceCardIdList)
        {
            return await _context.CuPrDepartments.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPriceId,
                    Id = x.DepartmentId.Value,
                    Name = x.Department.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card brands
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrBrand(List<int> priceCardIdList)
        {
            return await _context.CuPrBrands.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPriceId,
                    Id = x.BrandId.Value,
                    Name = x.Brand.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card buyers
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrBuyer(List<int> priceCardIdList)
        {
            return await _context.CuPrBuyers.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPriceId,
                    Id = x.BuyerId.Value,
                    Name = x.Buyer.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card price categories
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrPriceCategory(List<int> priceCardIdList)
        {
            return await _context.CuPrPriceCategories.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPriceId,
                    Id = x.PriceCategoryId.Value,
                    Name = x.PriceCategory.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card holiday types
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrHolidayType(List<int> priceCardIdList)
        {
            return await _context.CuPrHolidayTypes.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPriceId,
                    Id = x.HolidayInfoId.Value,
                    Name = x.HolidayInfo.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get price card inspection locations
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrInspectionLocation(List<int> priceCardIdList)
        {
            return await _context.CuPrInspectionLocations.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId.GetValueOrDefault()))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPriceId.GetValueOrDefault(),
                    Id = x.InspectionLocationId.Value,
                    Name = x.InspectionLocation.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card invoice request contacts
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrContacts(List<int> priceCardIdList)
        {
            return await _context.InvTranInvoiceRequestContacts.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceCardId.Value))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPriceCardId.Value,
                    Id = x.ContactId.Value,
                    Name = x.Contact.ContactName
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CuPrContactCommonDataSource>> GetInvoiceContacts(List<int> invoiceRequestIds)
        {
            return await _context.InvTranInvoiceRequestContacts.Where(y => y.InvoiceRequestId.HasValue && invoiceRequestIds.Contains(y.InvoiceRequestId.Value) && !y.IsCommon.Value && y.Active.Value)

                .Select(x => new CuPrContactCommonDataSource
                {
                    PriceId = x.CuPriceCardId,
                    InvoiceId = x.InvoiceRequestId,
                    Name = x.Contact.ContactName,
                    ContactId = x.ContactId
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CuPrHolidayType>> GetHolidayTypeList(List<int> priceCardIdList)
        {
            return await _context.CuPrHolidayTypes.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId)).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get customer price card for quotation with travel matrix info
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public async Task<QuotationCustomerPriceCard> GetQuotationPriceCard(int ruleId)
        {
            return await _context.CuPrDetails.Where(x => x.Id == ruleId)
                  .Select(x => new QuotationCustomerPriceCard()
                  {
                      CurrencyId = x.CurrencyId,
                      TravelMatrixTypeId = x.TravelMatrixTypeId
                  }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get price card city list
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CuPrCommonDataSource>> GetPrCity(List<int> priceCardIdList)
        {
            return await _context.CuPrCities.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CuPrCommonDataSource
                {
                    PriceId = x.CuPrId,
                    Id = x.FactoryCityId,
                    Name = x.FactoryCity.CityName
                }).AsNoTracking().ToListAsync();
        }
    }
}
