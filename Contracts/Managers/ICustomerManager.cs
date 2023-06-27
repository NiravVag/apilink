using DTO.CommonClass;
using DTO.Customer;
using DTO.FullBridge;
using DTO.InvoicePreview;
using DTO.References;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICustomerManager
    {
        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns><IEnumerable<CustomerItem></returns>
        Task<IEnumerable<CustomerItem>> GetCustomerItems();
        Task<CustomerSummaryResponse> GetCustomerSummary();
        Task<SaveCustomerResponse> Save(CustomerDetails request);
        Task<CustomerDeleteResponse> DeleteCustomer(int id);
        CustomerSearchResponse GetCustomerData(CustomerSearchRequest request);
        Task<EditCustomerResponse> GetEditCustomer(int? id);
        Task<CustomerSummaryResponse> GetCustomerbyId(int? id);
        Task<CustomerGroupResponse> GetCustomerGroup();

        Task<CustomerSourceResponse> GetLanguage();
        Task<CustomerSourceResponse> GetProspectStatus();
        Task<CustomerSourceResponse> GetMarketSegment();
        Task<CustomerSourceResponse> GetBusinessType();
        Task<CustomerSourceResponse> GetAddressType();
        Task<CustomerSourceResponse> GetInvoiceType();


        Task<CustomerSourceResponse> GetAccountingLeader();
        Task<CustomerSourceResponse> GetActivitiesLevel();
        Task<CustomerSourceResponse> GetRelationshipStatus();
        Task<CustomerSourceResponse> GetBrandPriority();



        Task<DataSourceResponse> GetCustomerEntityList(int customerId);
        Task<DataSourceResponse> GetCustomerSisterCompany(int customerId);


        /// <summary>
        /// Get Customer's Brands By UserId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="UserId"></param>
        /// <returns>IEnumerable<CuBrand></returns>
        Task<IEnumerable<CustomerBrand>> GetCustomerBrandsByUserId(int CustomerId, int UserId);

        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns><IEnumerable<CustomerItem></returns>
        Task<IEnumerable<CustomerItem>> GetCustomersByUserId(int UserId);

        /// <summary>
        /// Get Customer By Suplier Id
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns><IEnumerable<CustomerItem></returns>
        Task<IEnumerable<CustomerItem>> GetCustomersBySupplierId(int SupplierId);

        /// <summary>
        /// Get Customer's Departments By UserId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="UserId"></param>
        /// <returns>IEnumerable<CuDepartment></returns>
        Task<IEnumerable<CustomerDepartment>> GetCustomerDepartmentByUserId(int CustomerId, int UserId);

        /// <summary>
        /// Get Customer's Season By UserId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>IEnumerable<CuSeason></returns>
        Task<IEnumerable<Season>> GetCustomerSeason(int CustomerId);



        /// <summary>
        /// Get Customer's Audit Service Type
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>IEnumerable<CustomerContact></returns>
        Task<IEnumerable<ServiceType>> GetCustomerAuditServiceType(int CustomerId);

        /// <summary>
        /// Get Customer By User Type
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns><IEnumerable<CustomerItem></returns>
        Task<CustomerSummaryResponse> GetCustomersByUserType();

        Task<CustomerServiceTypeResponse> GetCustomerInspectionServiceType(int CustomerId);

        Task<IEnumerable<CustomerBuyers>> GetCustomerBuyerByUserId(int CustomerId);

        List<CustomerAddressData> GetCustomerAddress(int CustomerId);

        Task<CustomerServiceTypeResponse> GetReInspectionServiceType(int CustomerId);

        Task<CustomerSummaryResponse> GetCustomerByCheckPointUsertType();
        /// <summary>
        /// Get Customer data By Id
        /// </summary>
        /// <returns></returns>
        Task<CustomerItem> GetCustomerItemById(int customerId);

        Task<object> SaveZohoCRMCustomer(SaveCustomerCrmRequest request);
        /// <summary>
        /// update the zoho cust
        /// </summary>
        /// <param name="request"></param>
        /// <param name="zohoCustomerId"></param>
        /// <returns></returns>
        Task<object> UpdateZohoCustomer(SaveCustomerCrmRequest request, long zohoCustomerId);
        /// <summary>
        /// get the customer detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<object> GetCustomerByZohoId(long id);
        /// <summary>
        /// get the customer details by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ZohoCustomerDetailResponse> GetCustomerDetailsByName(string name);
        Task<LinkErrorResponse> ValidateSaveCustomerRequest(SaveCustomerCrmRequest request, int requestType);
        Task<object> GetCustomerByGLCode(string glCode);
        Task<IEnumerable<CommonDataSource>> GetCustomerMerchandiserById(int CustomerId);
        Task<List<CustomerAccountingAddress>> GetCustomerAddressByListCusId(List<int> lstcustomerId);
        Task<IEnumerable<CommonDataSource>> GetCustomerCollection(int CustomerId);
        Task<DataSourceResponse> GetCustomerByName(string customerName);
        Task<DataSourceResponse> GetCustomerPriceCategory(int CustomerId);

        /// <summary>
        /// Get the customer brands by customerid
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCustomerBrands(int CustomerId);

        /// <summary>
        /// Get the customer departments by customerid
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCustomerDepartments(int CustomerId);

        /// <summary>
        /// Get the customer buyers by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCustomerBuyers(int CustomerId);

        /// <summary>
        /// Get the customer departments by customer ids
        /// </summary>
        /// <param name="CustomerIds"></param>
        /// <returns></returns>
        Task<ParentDataSourceResponse> GetCustomerDepartments(IEnumerable<int> customerIds);

        /// <summary>
        /// Get the customer brands by customerIds
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        Task<ParentDataSourceResponse> GetCustomerBrands(IEnumerable<int> customerIds);

        /// <summary>
        /// Get Customer Contacts by Customer
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCustomerContactsbyCustomer(int CustomerId);
        /// <summary>
        /// Get customer Address by Customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<AddressDataSourceResponse> GetCustomerAddressbyCustomer(int CustomerId);

        /// <summary>
        /// Get the customer by customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCustomerByCustomerId(int customerId);

        Task<IEnumerable<DataCommon>> GetCustomerItemList();
        Task<DataSourceResponse> GetCustomerDataSource(CommonDataSourceRequest request);
        Task<DataSourceResponse> GetPriceCategoryDataSource(CommonCustomerSourceRequest request);

        Task<CustomerGLCodeResponse> GetCustomerGLCodeSourceList(CustomerDataSourceRequest request);

        Task<CustomerDataSourceResponse> GetCustomerDataSourceBySupplier(CommonDataSourceRequest request);

        Task<CustomerKamDetail> GetCustomerKAMDetails(int customerId);
        Task<DataSourceResponse> GetCustomerProductCategories(int customerId);
        Task<DataSourceResponse> GetCustomerSeasonConfiguration(int? customerId);
        Task<CustomerPriceData> GetCustomerPriceData(int CustomerId);
        Task<DataSourceResponse> GetCustomerProductCategoryList(int CustomerId);
        Task<DataSourceResponse> GetCustomerProductSubCategoryList(int CustomerId, List<int?> ProductCategory);
        Task<DataSourceResponse> GetCustomerProductSub2CategoryList(CustomerSubCategory2Request customerSubCategory);
        Task<object> GetEAQFCustomer(int Index, int PageSize, string CompanyName, string Email);
        Task<DataSourceResponse> GetCustomerByUserType(CommonDataSourceRequest request);

        Task<object> SaveEAQFCustomer(SaveEaqfCustomerRequest request);
        Task<object> UpdateEAQFCustomer(int customerId, SaveEaqfCustomerRequest request);
        Task<object> SaveEAQFCustomerAddress(int customerId, SaveEaqfCustomerAddressRequest request);
        Task<object> UpdateEAQFCustomerAddress(int customerId, int addressId, UpdateEaqfCustomerAddressRequest request);
        Task<object> SaveEAQFCustomerContact(int customerId, SaveEaqfCustomerContactRequest request);
        Task<object> UpdateEAQFCustomerContact(int customerId, int contactId, SaveEaqfCustomerContactRequest request);
        Task<CustomerContactAddressDetails> GetCustomerContactAddressDetails(int customerId);
    }
}
