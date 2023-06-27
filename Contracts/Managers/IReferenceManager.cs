using DTO.Customer;
using DTO.CustomerProducts;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.ProductManagement;
using DTO.Quotation;
using DTO.CommonClass;
using System.Linq;
using Entities;
using DTO.HumanResource;

namespace Contracts.Managers
{
    public interface IReferenceManager
    {
        /// <summary>
        /// Get Units
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>Unit</returns>
        Task<IEnumerable<Unit>> GetUnits();

        /// <summary>
        /// Get Season
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>Season</returns>
        Task<IEnumerable<Season>> GetSeasons();

        /// <summary>
        /// Get Season Year
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>SeasonYear</returns>
        Task<SeasonYearResponse> GetSeasonsYear();

        /// <summary>
        /// Get Product Categories
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>ProductCategorySummaryResponse</returns>
        Task<ProductCategoryResponse> GetProductCategories();

        /// <summary>
        /// Get Product Sub Categories
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>ProductSubCategoryResponse</returns>
        Task<ProductSubCategoryResponse> GetProductSubCategories(int productCategoryID);

        /// <summary>
        /// Get Product Category Sub2
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>ProductCategorySub2Response</returns>
        Task<ProductCategorySub2Response> GetProductCategorySub2(int? productSubCategoryID);

        /// <summary>
        /// Get currencies
        /// </summary>
        /// <returns></returns>
        IEnumerable<Currency> GetCurrencies();

        /// <summary>
        /// Get services
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Service>> GetServices();

        /// <summary>
        /// Get Service Type List List
        /// </summary>
        /// <param name="customerId, serviceId"></param>
        /// <returns></returns>
        Task<QuotationDataSourceResponse> GetServiceTypeList(int customerId, int serviceId);
        /// <summary>
        /// GetServiceList
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetServiceList();
        /// <summary>
        /// GetBillingToList
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetBillingToList();
        /// <summary>
        /// GetBillingMethodList
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetBillingMethodList();
        /// <summary>
        /// Get custom sample size list
        /// </summary>
        /// <returns></returns>
        Task<CustomSampleSizeResponse> GetCustomSampleSizeList();

        /// <summary>
        /// Get the API Services
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetAPIServices();
        /// <summary>
        /// get currency list
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetCurrencyList();

        /// <summary>
        /// Get billing entity list
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetBillingEntityList();
        /// <summary>
        /// Get Invoice Request type list
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetInvoiceRequestTypeList();
        /// <summary>
        /// Get Invoice Bank List
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetInvoiceBankList(int? billingEntity);
        /// <summary>
        /// Get invoice fees types list
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetInvoiceFeesTypeList();
        /// <summary>
        /// Get invoice office list
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetInvoiceOfficeList();
        /// <summary>
        /// Get invoice payment type list
        /// </summary>
        /// <returns></returns>
        Task<PaymentTypeResponse> GetInvoicePaymentTypeList();

        Task<DataSourceResponse> GetProductCategorySourceList(ProductCategoryDataSourceRequest request);

        Task<DataSourceResponse> GetProductSubCategorySourceList(ProductSubCategoryDataSourceRequest request);
        /// <summary>
        /// Get service type by customer id and service id
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetServiceTypeListByCusService(int customerId, int serviceId);
        Task<DataSourceResponse> GetInvoiceExtraTypeList();
        Task<DataSourceResponse> GetProductSubCategory2SourceList(ProductSubCategory2DataSourceRequest request);
        Task<DataSourceResponse> GetServiceDataList();
        Task<DataSourceResponse> GetFBResultList();
        Task<EmailSubjectDelimiterResponse> GetDelimiterList();
        Task<DataSourceResponse> GetOfficeLocations();
        Task<DataSourceResponse> GetEntityList();
        Task<DataSourceResponse> GetUserEntityList(int userType, int id);
        Task<DataSourceResponse> GetDateFormats();
        Task<ServiceTypeResponse> GetCustomerServiceTypes(ServiceTypeRequest request);
        Task<DataSourceResponse> GetInspectionLocations();
        Task<DataSourceResponse> GetInspectionShipmentTypes();
        Task<DataSourceResponse> GetBusinessLines();
        Task<DataSourceResponse> GetTripTypeList();
        Task<DataSourceResponse> GetProductSubCategory3SourceList(ProductSubCategory3DataSourceRequest request);
        Task<List<CommonDataSource>> GetProdCategoriesByProdCategoryIds(IEnumerable<int> prodCategoryIds);
        Task<DataSourceResponse> GetServiceTypesByServiceIds(IEnumerable<int> serviceIds);
        Task<DataSourceResponse> GetBillingFrequncyList();
        Task<DataSourceResponse> GetBillingQuantityTypeList();
        Task<DataSourceResponse> GetInterventionTypeList();
        Task<List<int>> GetEntityFeatureList();
        Task<DataSourceResponse> GetStaffSourceList(CommonDataSourceRequest request);
        Task<DataSourceResponse> GetExpertiseList();
        Task<bool> CheckUserHasInvoiceAccess();
        Task<IEnumerable<CommonBankDataSource>> GetBankList(int? billingEntity);
        Task<CurrencyDataSourceResponse> GetCurrencyListWithCurrencyCode();
        Task<HRStaffResponse> GetOutSourceStaffList();

        Task<bool> IsEntityFeatureExist(int featureId);
        Task<object> GetEAQFProductsCategories(string ProductLineName, int ProductLineId);
        Task<object> GetEAQFServices(int CustomerId, string ServiceCategoryName, int ServiceCategoryId);
        Task<object> GetEAQFProductType(string productCategoryName, string productSubCategoryName);

        Task<InspectionBookingTypeResponse> GetInspectionBookingTypeList();

        Task<InspectionPaymentOptionsResponse> GetInspectionPaymentOptions(int customerId);
        Task<RefCurrency> GetCurrencyData(string currencyCode);
        Task<CuCustomer> GetCustomerData(int customerId);

        Task<ServiceTypeResponse> GetAuditServiceTypes();

    }
}
