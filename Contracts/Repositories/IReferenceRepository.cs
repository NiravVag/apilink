using DTO.CommonClass;
using DTO.CustomerProducts;
using DTO.References;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IReferenceRepository : IRepository
    {
        /// <summary>
        /// Get Units
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefUnit</returns>
        Task<List<RefUnit>> GetUnits();

        /// <summary>
        /// Get Season
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefSeason</returns>
        Task<List<RefSeason>> GetSeasons();

        /// <summary>
        /// Get Season Year
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefSeasonYear</returns>
        Task<List<RefSeasonYear>> GetSeasonsYear();

        /// <summary>
        /// Get Reference Pick type
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefPickType</returns>
        Task<List<RefPickType>> GetServicePickType();

        /// <summary>
        /// Get Level Pick First
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefLevelPick1</returns>
        Task<List<RefLevelPick1>> GetServiceLevelPickFirst();

        /// <summary>
        /// Get Level Pick Second
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefLevelPick2</returns>
        Task<List<RefLevelPick2>> GetServiceLevelPickSecond();

        /// <summary>
        /// Get Service Pick First
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefPick1</returns>
        Task<List<RefPick1>> GetServicePickFirst();

        /// <summary>
        /// Get Service Pick Second
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefPick2</returns>
        Task<List<RefPick2>> GetServicePickSecond();

        /// <summary>
        /// Get Service Defect Classification
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefDefectClassification</returns>
        Task<List<RefDefectClassification>> GetServiceDefectClassification();

        /// <summary>
        /// Get Service Report Unit
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefReportUnit</returns>
        Task<List<RefReportUnit>> GetServiceReportUnit();

        /// <summary>
        /// Get Service Related Data
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefService</returns>
        Task<List<RefService>> GetServices();

        /// <summary>
        /// Get Service Types
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefServiceType</returns>
        Task<List<RefServiceType>> GetServiceTypes();

        /// <summary>
        /// Get ProductCategory
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefProductCategory</returns>
        Task<List<RefProductCategory>> GetProductCategories();

        /// <summary>
        /// Get Sub Product Category
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefProductCategorySub</returns>
        Task<List<RefProductCategorySub>> GetProductSubCategories(int productCategoryID);

        /// <summary>
        /// Get Product Category Sub 2
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>RefProductCategorySub2</returns>
        Task<List<RefProductCategorySub2>> GetProductCategorySub2(int? productsubCategoryID);

        /// <summary>
        /// Get currencies
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefCurrency> GetCurrencies();

        /// <summary>
        /// Get ReInspection Types
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefReInspectionType> GetReInspectionTypes();

        /// <summary>
        /// Get the Sample size Code for Acceptable quality 
        /// </summary>
        /// <param name="orderQuantity"></param>
        /// <returns></returns>
        Task<IEnumerable<RefAqlPickSampleSizeAcceCode>> GetSampleSizeCodeForAcceQuality();


        /// <summary>
        /// Get Service Type for a Customer and Service
        /// </summary>
        /// <returns></returns>
        Task<List<RefServiceType>> GetServiceTypeList(int customerId, int serviceId);
        /// <summary>
        /// GetBillingMethodList
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetBillingMethodList();
        /// <summary>
        /// GetBillingToList
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetBillingToList();

        /// <summary>
        /// Get Billing To names by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetBillingTosByIds(IEnumerable<int> ids);
        // <summary>
        /// Get Fb template List
        /// </summary>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetFbTemplateList();

        /// <summary>
        /// Get Custom sample size
        /// </summary>
        /// <returns></returns>
        Task<List<CustomSampleSize>> GetCustomSampleSizeList();

        /// <summary>
        /// Get the API Services List
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetAPIServices();

        /// <summary>
        /// Get billing entity list
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetBillingEntityList();
        /// <summary>
        /// Get invoice type request
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetInvoiceRequestTypeList();
        /// <summary>
        /// Get Invoice Office List
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetInvoiceOfficeList();
        /// <summary>
        /// Get Invoice Payment Type List
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PaymentTypeSource>> GetInvoicePaymentTypeList();
        /// <summary>
        /// Get Invoice bank list
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BankData>> GetInvoiceBankList(int? billingEntity);
        /// <summary>
        /// Get Invoice Fees Type List
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetInvoiceFeesTypeList();

        IQueryable<RefProductCategory> GetProductCategoryDataSourceList();

        IQueryable<RefProductCategorySub> GetProductSubCategoryDataSourceList();

        Task<IEnumerable<CommonDataSource>> GetInvoiceExtraTypeList();

        IQueryable<RefProductCategorySub2> GetProductSubCategory2DataSourceList();

        Task<IEnumerable<CommonDataSource>> GetFbReportResults();

        Task<List<EmailSubjectDelimiter>> GetDelimiterList();

        Task<List<CommonDataSource>> GetOfficeLocations();

        Task<IEnumerable<CommonDataSource>> GetEntityList();

        Task<List<CommonDataSource>> GetCustomerEntityList(int contactId);

        Task<List<CommonDataSource>> GetSupplierEntityList(int contactId);

        Task<List<CommonDataSource>> GetInternalUserEntityList(int staffId);

        Task<IEnumerable<CommonDataSource>> GetDateFormats();

        IQueryable<RefServiceType> GetCustomerServiceTypeQuery();

        Task<List<CommonDataSource>> GetInspectionLocation();

        Task<List<CommonDataSource>> GetInspectionShipmentTypes();

        Task<List<CommonDataSource>> GetBusinessLines();

        Task<List<ServiceTypeData>> GetEditBookingServiceTypes(int? bookingId);

        Task<List<CommonDataSource>> GetTripTypeDataSource();
        IQueryable<RefProductCategorySub3> GetProductSubCategory3DataSourceList();

        /// <summary>
        /// Get Product Categories by Product Category Ids
        /// </summary>
        /// <param name="productCategoryIds"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetProdCategoriesByProdCategoryIds(IEnumerable<int> productCategoryIds);

        /// <summary>
        /// Get Service Types by serviceIds
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetServiceTypesByServiceId(IEnumerable<int> serviceIds);
        Task<List<CommonDataSource>> GetBillingFrequencyList();
        Task<List<CommonDataSource>> GetBillingQuantityTypeList();
        Task<List<CommonDataSource>> GetInterventionTypeList();
        Task<List<EntFeatureDetail>> GetEntityFeatureList();
        /// <summary>
        /// Get Dp point list
        /// </summary>
        /// <returns></returns>
        Task<List<InspRefDpPoint>> GetDpPointList();
        IQueryable<HrStaff> GetStaffDataSource();
        Task<bool> CheckUserHasInvoiceAccess(int staffId);
        Task<IEnumerable<BankData>> GetBankList(int? billingEntity);

        /// <summary>
        /// Get Country details by AlphaTwoCode
        /// </summary>
        /// <param name="alphaTwoCode"></param>
        /// <returns></returns>
        Task<RefCountry> GetCountryDetailsByAlphaCode(string alphaTwoCode);
        Task<RefProductCategorySub> GetProductCategoryBySubDetail(string productCategorySub);
        Task<string> GetUnitName(int Unit);
        Task<CommonDataSource> GetSeasonById(int? customerSeasonId);
        /// <summary>
        /// Get ProductCategories
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>ProductCategories</returns>
        Task<bool> CheckEntityFeature(int featureId);
        Task<bool> IsServiceTypeMappedWithBusinessLine(int serviceId, int businessLineId, int entityId);

        Task<bool> IsValidEntity(int entityId);

        Task<List<CommonDataSource>> GetInspectionBookingTypes();

        Task<List<CommonDataSource>> GetInspectionPaymentOptions(int customerId);
        IQueryable<RefProductCategory> GetProductsCategories();
        Task<RefCurrency> GetCurrencyDataByCode(string currencyCode);

        Task<CuCustomer> GetCustomerData(int customerId);
    }
}
