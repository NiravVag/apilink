using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DTO.CommonClass;
using DTO.References;
using DTO.CustomerProducts;

namespace DAL.Repositories
{
    public class ReferenceRepository : Repository, IReferenceRepository
    {
        public ReferenceRepository(API_DBContext context) : base(context)
        {
        }
        public Task<List<RefSeason>> GetSeasons()
        {
            return _context.RefSeasons.Where(x => x.Active && x.Default.Value).ToListAsync();
        }

        public Task<List<RefSeasonYear>> GetSeasonsYear()
        {
            return _context.RefSeasonYears.Where(x => x.Active).ToListAsync();
        }

        public Task<List<RefUnit>> GetUnits()
        {
            return _context.RefUnits.Where(x => x.Active).ToListAsync();
        }

        public Task<List<RefPickType>> GetServicePickType()
        {
            return _context.RefPickTypes.Where(x => x.Active).ToListAsync();
        }

        public async Task<List<RefLevelPick1>> GetServiceLevelPickFirst()
        {
            return await _context.RefLevelPick1S.Where(x => x.Active).OrderByDescending(x => x.Value).ToListAsync();
        }

        public Task<List<RefLevelPick2>> GetServiceLevelPickSecond()
        {
            return _context.RefLevelPick2S.Where(x => x.Active).ToListAsync();
        }

        public async Task<List<RefPick1>> GetServicePickFirst()
        {
            return await _context.RefPick1S.Where(x => x.Active).ToListAsync();
        }

        public Task<List<RefPick2>> GetServicePickSecond()
        {
            return _context.RefPick2S.Where(x => x.Active).ToListAsync();
        }

        public Task<List<RefDefectClassification>> GetServiceDefectClassification()
        {
            return _context.RefDefectClassifications.Where(x => x.Active).ToListAsync();
        }

        public Task<List<RefReportUnit>> GetServiceReportUnit()
        {
            return _context.RefReportUnits.Where(x => x.Active).ToListAsync();
        }

        public Task<List<RefService>> GetServices()
        {
            return _context.RefServices.Where(x => x.Active).ToListAsync();
        }

        public Task<List<RefServiceType>> GetServiceTypes()
        {
            return _context.RefServiceTypes.Where(x => x.Active).OrderBy(x => x.Sort).ToListAsync();
        }

        public Task<List<RefProductCategory>> GetProductCategories()
        {
            return _context.RefProductCategories.Where(x => x.Active).ToListAsync();
        }

        public Task<List<RefProductCategorySub>> GetProductSubCategories(int productCategoryID)
        {
            return _context.RefProductCategorySubs.Where(x => x.ProductCategoryId == productCategoryID && x.Active).ToListAsync();
        }

        public Task<List<RefProductCategorySub2>> GetProductCategorySub2(int? productsubCategoryID)
        {
            return _context.RefProductCategorySub2S.Where(x => x.ProductSubCategoryId == productsubCategoryID && x.Active).ToListAsync();
        }

        public IEnumerable<RefCurrency> GetCurrencies()
        {
            return _context.RefCurrencies.Where(x => x.Active).OrderBy(x => x.CurrencyName);
        }

        public async Task<RefCurrency> GetCurrencyDataByCode(string currencyCode)
        {
            return await _context.RefCurrencies.FirstOrDefaultAsync(x => x.Active && x.CurrencyCodeA == currencyCode);
        }

        public async Task<CuCustomer> GetCustomerData(int customerId)
        {
            return await _context.CuCustomers
                 .Include(x => x.ItUserMasters)
                 .Include(x => x.CuCustomerBusinessCountries)
                 .FirstOrDefaultAsync(x => x.Active.Value && x.Id == customerId);
        }

        public IEnumerable<RefReInspectionType> GetReInspectionTypes()
        {
            return _context.RefReInspectionTypes.Where(x => x.Active == true).OrderBy(x => x.Name);
        }

        public async Task<IEnumerable<RefAqlPickSampleSizeAcceCode>> GetSampleSizeCodeForAcceQuality()
        {
            return await _context.RefAqlPickSampleSizeAcceCodes.ToListAsync();
        }

        public async Task<List<RefServiceType>> GetServiceTypeList(int customerId, int serviceId)
        {
            return await _context.RefServiceTypes
                .Join(_context.CuServiceTypes,
                service => service.Id,
                cuservice => cuservice.ServiceTypeId,
                (service, cuservice) => new { RefServiceType = service, CuServiceType = cuservice })
                .Where(x => x.CuServiceType.CustomerId == customerId && x.CuServiceType.ServiceId == serviceId &&
                   x.CuServiceType.Active && x.RefServiceType.Active).OrderBy(x => x.RefServiceType.Sort)
                .Select(res => new RefServiceType
                {
                    Id = res.RefServiceType.Id,
                    Name = res.RefServiceType.Name
                })
                .AsNoTracking().ToListAsync();
        }
        //get list from QuBillMethods table
        public async Task<IEnumerable<CommonDataSource>> GetBillingMethodList()
        {
            return await _context.QuBillMethods.OrderBy(x => x.Label)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Label
                }).ToListAsync();
        }

        //get list form QuPaidBies table
        public async Task<IEnumerable<CommonDataSource>> GetBillingToList()
        {
            return await _context.QuPaidBies.OrderBy(x => x.Label)
              .Select(x => new CommonDataSource
              {
                  Id = x.Id,
                  Name = x.Label
              }).ToListAsync();
        }

        //get list form QuPaidBies table by ids
        public async Task<IEnumerable<string>> GetBillingTosByIds(IEnumerable<int> ids)
        {
            return await _context.QuPaidBies.Where(x => ids.Contains(x.Id)).OrderBy(x => x.Label)
              .Select(x => x.Label).ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetProdCategoriesByProdCategoryIds(IEnumerable<int> productCategoryIds)
        {
            return await _context.RefProductCategories.Where(x => x.Active
                                    && productCategoryIds.Contains(x.Id))
                                    .Select(x => new CommonDataSource
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                    }).ToListAsync();
        }

        public async Task<List<ProductCategorySub2Data>> GetProductCategorySub2List(IEnumerable<int> productSubCategoryIds)
        {
            return await _context.RefProductCategorySub2S.Where(x => x.Active
                                    && productSubCategoryIds.Contains(x.ProductSubCategoryId))
                                    .Select(x => new ProductCategorySub2Data
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        ProductSubCategoryId = x.ProductSubCategoryId
                                    }).ToListAsync();
        }

        //fetch fb report template from FB_Report_Template table
        public async Task<List<CommonDataSource>> GetFbTemplateList()
        {
            return await _context.FbReportTemplates.Where(x => x.Active == true)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get sample type List for custom Aql
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomSampleSize>> GetCustomSampleSizeList()
        {
            return await _context.RefSampleTypes.Where(x => x.Active.HasValue && x.Active.Value)
                .Select(x => new CustomSampleSize
                {
                    Id = x.Id,
                    SampleType = x.SampleType,
                    SampleSize = x.SampleSize
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the API Services List
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetAPIServices()
        {
            return await _context.RefServices.Where(x => x.Active).Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        /// <summary>
        /// Get Billing Entity List 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetBillingEntityList()
        {
            return await _context.RefBillingEntities.Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        public async Task<IEnumerable<CommonDataSource>> GetInvoiceRequestTypeList()
        {
            return await _context.InvRefRequestTypes.Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }
        /// <summary>
        /// get invoice office list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetInvoiceOfficeList()
        {
            return await _context.InvRefOffices.Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }
        /// <summary>
        /// get invoice payment list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PaymentTypeSource>> GetInvoicePaymentTypeList()
        {
            return await _context.InvRefPaymentTerms.Select(x => new PaymentTypeSource { Id = x.Id, Name = x.Name, Duration = x.Duration }).ToListAsync();
        }
        /// <summary>
        /// get invoice bank list
        /// </summary>
        /// <param name="billingEntity"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BankData>> GetInvoiceBankList(int? billingEntity)
        {
            return await _context.InvRefBanks.Where(x => x.BillingEntity == billingEntity && x.Active.HasValue && x.Active.Value)
                    .Select(x => new BankData
                    {
                        Id = x.Id,
                        BankName = x.BankName,
                        CurrencyId = x.AccountCurrency.GetValueOrDefault(),
                        CurrencyCode = x.AccountCurrencyNavigation.CurrencyCodeA,
                        Remarks = x.Remarks
                    }).ToListAsync();
        }

        /// <summary>
        /// Get invoice fees type list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetInvoiceFeesTypeList()
        {
            return await _context.InvRefFeesFroms.Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        /// <summary>
        /// Get the Product Category IQueryable data
        /// </summary>
        /// <returns></returns>
        public IQueryable<RefProductCategory> GetProductCategoryDataSourceList()
        {
            return _context.RefProductCategories.
                    Where(x => x.Active);
        }

        /// <summary>
        /// Get the Product Sub Category IQueryable Data
        /// </summary>
        /// <returns></returns>
        public IQueryable<RefProductCategorySub> GetProductSubCategoryDataSourceList()
        {
            return _context.RefProductCategorySubs.
                    Where(x => x.Active);
        }

        /// <summary>
        /// Get the Product Sub Category 2 IQueryable Data
        /// </summary>
        /// <returns></returns>
        public IQueryable<RefProductCategorySub2> GetProductSubCategory2DataSourceList()
        {
            return _context.RefProductCategorySub2S.OrderBy(x => x.Name).
                    Where(x => x.Active);
        }

        /// <summary>
        /// get invoice extra type list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetInvoiceExtraTypeList()
        {
            return await _context.InvExfTypes.Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        /// <summary>
        /// Get the FB Report Results Master Data
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetFbReportResults()
        {
            return await _context.FbReportResults.Where(x => x.Active.HasValue && x.Active.Value)
                      .Select(x => new CommonDataSource { Id = x.Id, Name = x.ResultName }).ToListAsync();
        }

        /// <summary>
        /// get delimiter list
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmailSubjectDelimiter>> GetDelimiterList()
        {
            return await _context.RefDelimiters.Where(x => x.Active.Value).
                Select(x => new EmailSubjectDelimiter
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsFile = x.IsFile
                }).ToListAsync();
        }

        /// <summary>
        /// Get all the Office Locations
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetOfficeLocations()
        {
            return await _context.RefLocations.Where(x => x.Active).OrderBy(x => x.LocationName)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.LocationName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the date formats
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetDateFormats()
        {
            return await _context.RefDateFormats
                    .Select(x => new CommonDataSource
                    {
                        Id = x.Id,
                        Name = x.DateFormat
                    }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Customer Service type query
        /// </summary>
        /// <returns></returns>
        public IQueryable<RefServiceType> GetCustomerServiceTypeQuery()
        {
            return _context.RefServiceTypes.Where(x => x.Active).OrderBy(x => x.Sort);
        }

        /// <summary>
        /// Get the inspection location details
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInspectionLocation()
        {
            return await _context.InspRefInspectionLocations.Where(x => x.Active.Value)
                            .Select(x => new CommonDataSource()
                            {
                                Id = x.Id,
                                Name = x.Name
                            }).ToListAsync();
        }

        /// <summary>
        /// Get the inspection shipment types
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInspectionShipmentTypes()
        {
            return await _context.InspRefShipmentTypes.Where(x => x.Active.Value)
                            .Select(x => new CommonDataSource()
                            {
                                Id = x.Id,
                                Name = x.Name
                            }).ToListAsync();
        }
        /// <summary>
        /// Get the business lines
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetBusinessLines()
        {
            return await _context.RefBusinessLines
                            .Select(x => new CommonDataSource()
                            {
                                Id = x.Id,
                                Name = x.BusinessLine
                            }).ToListAsync();
        }
        /// <summary>
        /// fetch the inactive service types for the bookings
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<ServiceTypeData>> GetEditBookingServiceTypes(int? bookingId)
        {
            return await _context.InspTranServiceTypes.Where(x => !x.Active && x.InspectionId == bookingId)
                            .Select(x => new ServiceTypeData()
                            {
                                Id = x.ServiceType.Id,
                                Name = x.ServiceType.Name,
                                ShowServiceDateTo = x.ServiceType.ShowServiceDateTo
                            }).ToListAsync();
        }

        /// <summary>
        /// get application entity list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetEntityList()
        {
            return await _context.ApEntities.Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        /// <summary>
        /// Get the list of entity mapped to customer contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerEntityList(int contactId)
        {
            return await _context.CuContactEntityMaps.Where(x => x.ContactId == contactId).
                Select(x => new CommonDataSource { Id = x.Entity.Id, Name = x.Entity.Name }).ToListAsync();
        }

        /// <summary>
        /// Get the supplier list mapped to the suppliers
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetSupplierEntityList(int contactId)
        {
            return await _context.SuContactEntityMaps.Where(x => x.ContactId == contactId).
                Select(x => new CommonDataSource { Id = x.Entity.Id, Name = x.Entity.Name }).ToListAsync();
        }

        /// <summary>
        /// Get the internal user list mapped to staff
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInternalUserEntityList(int staffId)
        {
            return await _context.HrEntityMaps.Where(x => x.StaffId == staffId).
                Select(x => new CommonDataSource { Id = x.Entity.Id, Name = x.Entity.Name }).ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetTripTypeDataSource()
        {
            return await _context.EcAutRefTripTypes.Where(x => x.Active.Value)
                                  .Select(x => new CommonDataSource
                                  {
                                      Id = x.Id,
                                      Name = x.Name,
                                  }).OrderBy(x => x.Name).ToListAsync();
        }
        /// <summary>
        /// Get the Product Sub Category 3 IQueryable Data
        /// </summary>
        /// <returns></returns>
        public IQueryable<RefProductCategorySub3> GetProductSubCategory3DataSourceList()
        {
            return _context.RefProductCategorySub3S.OrderBy(x => x.Name).
                    Where(x => x.Active);
        }
        public async Task<List<CommonDataSource>> GetBillingFrequencyList()
        {
            return await _context.InvRefBillingFreequencies.Where(x => x.Active.Value)
                                  .Select(x => new CommonDataSource
                                  {
                                      Id = x.Id,
                                      Name = x.Name,
                                  }).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }
        public async Task<List<CommonDataSource>> GetBillingQuantityTypeList()
        {
            return await _context.InspRefQuantityTypes.Where(x => x.Active.Value)
                                  .Select(x => new CommonDataSource
                                  {
                                      Id = x.Id,
                                      Name = x.Name,
                                  }).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetInterventionTypeList()
        {
            return await _context.InvRefInterventionTypes.Where(x => x.Active.Value)
                                  .Select(x => new CommonDataSource
                                  {
                                      Id = x.Id,
                                      Name = x.Name,
                                  }).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        public async Task<List<EntFeatureDetail>> GetEntityFeatureList()
        {
            return await _context.EntFeatureDetails.Where(x => x.Active.Value)
                                 .AsNoTracking().ToListAsync();
        }
        public async Task<List<CommonDataSource>> GetServiceTypesByServiceId(IEnumerable<int> serviceIds)
        {
            return await _context.RefServiceTypes.Where(x => x.Active && x.ServiceId.HasValue && serviceIds.Contains(x.ServiceId.Value)).OrderBy(x => x.Sort)
            .Select(x => new CommonDataSource()
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get Dp point list
        /// </summary>
        /// <returns></returns>
        public async Task<List<InspRefDpPoint>> GetDpPointList()
        {
            return await _context.InspRefDpPoints.Where(x => x.Active).AsNoTracking().ToListAsync();
        }

        public IQueryable<HrStaff> GetStaffDataSource()
        {
            return _context.HrStaffs.Where(x => x.Active.Value).OrderBy(x => x.PersonName);
        }

        public async Task<bool> CheckUserHasInvoiceAccess(int staffId)
        {
            return await _context.InvDaTransactions.AnyAsync(x => x.Active && x.StaffId == staffId);
        }

        public async Task<bool> CheckEntityFeature(int featureId)
        {
            return await _context.EntFeatureDetails.AsNoTracking().AnyAsync(x => x.Active == true && x.FeatureId == featureId);
        }

        public async Task<IEnumerable<BankData>> GetBankList(int? billingEntity)
        {
            return await _context.InvRefBanks.Where(x => x.BillingEntity == billingEntity && x.Active.HasValue && x.Active.Value)
                .Select(x => new BankData
                {
                    Id = x.Id,
                    BankName = x.BankName,
                    CurrencyId = x.AccountCurrency.GetValueOrDefault(),
                    CurrencyCode = x.AccountCurrencyNavigation.CurrencyCodeA,
                    Remarks = x.Remarks
                }).ToListAsync();
        }

        public async Task<RefCountry> GetCountryDetailsByAlphaCode(string alphaTwoCode)
        {
            return await _context.RefCountries.Where(x => x.Alpha2Code.ToLower() == alphaTwoCode.ToLower() && x.Active).FirstOrDefaultAsync();
        }

        public async Task<RefProductCategorySub> GetProductCategoryBySubDetail(string productCategorySub)
        {
            return await _context.RefProductCategorySubs.Where(x => x.Name.ToLower() == productCategorySub.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<string> GetUnitName(int Unit)
        {
            return await _context.RefUnits.Where(x => x.Id == Unit).Select(x => x.Name).FirstOrDefaultAsync();
        }



        public async Task<CommonDataSource> GetSeasonById(int? customerSeasonId)
        {
            return await _context.CuSeasonConfigs.Where(x => x.Active.Value && x.Id == customerSeasonId).
                 Select(x => new CommonDataSource() { Id = x.Id, Name = x.Season.Name })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the inspection booking types
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInspectionBookingTypes()
        {
            return await _context.InspRefBookingTypes.Where(x => x.Active.Value).
                Select(x => new CommonDataSource()
                {
                    Id = x.Id,
                    Name = x.Name
                }).
                AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the inspection payment options
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInspectionPaymentOptions(int customerId)
        {
            return await _context.InspRefPaymentOptions.Where(x => x.Active.Value && x.CustomerId == customerId).
                Select(x => new CommonDataSource()
                {
                    Id = x.Id,
                    Name = x.Name
                }).
                AsNoTracking().ToListAsync();
        }

        public IQueryable<RefProductCategory> GetProductsCategories()
        {
            return _context.RefProductCategories.Where(x => x.Active);
        }

        public Task<bool> IsServiceTypeMappedWithBusinessLine(int serviceId, int businessLineId, int entityId)
        {
            return _context.RefServiceTypes.AnyAsync(a => a.Id == serviceId && a.Active && a.BusinessLineId == businessLineId && a.EntityId == entityId);
        }

        public Task<bool> IsValidEntity(int entityId)
        {
            return _context.ApEntities.AnyAsync(x => x.Id == entityId && x.Active);
        }
    }

}
