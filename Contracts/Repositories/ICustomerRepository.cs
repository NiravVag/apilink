using DTO.CommonClass;
using DTO.Customer;
using DTO.FullBridge;
using DTO.Quotation;
using DTO.User;
using DTO.UserAccount;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICustomerRepository : IRepository
    {
        /// <summary>
        /// Get Customers Items
        /// </summary>
        /// <returns></returns>
        Task<List<CuCustomer>> GetCustomersItems();
        Task<List<CuCustomer>> GetCustomerbyId(int? id);
        Task<int> AddCustomer(CuCustomer entity);

        IEnumerable<CuCustomer> GetAllCustomersItems();
        Task<int> EditCustomer(CuCustomer entity);
        Task<CuCustomer> GetCustomerDetails(int? id);
        Task<bool> RemoveCustomer(int id);
        Task<List<CuCustomerGroup>> GetCustomerGroup();


        Task<List<Language>> GetLanguage();
        Task<List<RefProspectStatus>> GetProspectStatus();
        Task<List<RefMarketSegment>> GetMarketSegment();
        Task<List<RefBusinessType>> GetBusinessType();
        Task<List<RefAddressType>> GetAddressType();
        Task<List<RefInvoiceType>> GetInvoiceType();
        Task<List<CuRefAccountingLeader>> GetAccountingLeader();
        Task<List<CuRefActivitiesLevel>> GetActivitiesLevel();
        Task<List<CuRefRelationshipStatus>> GetRelationshipStatus();
        Task<List<CuRefBrandPriority>> GetBrandPriority();


        /// <summary>
        /// Get Customers Items By User Id
        /// </summary>
        /// <returns></returns>
        Task<List<CuCustomer>> GetCustomersByUserId(int UserId);

        /// <summary>
        /// Get Customer's Brands By UserId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="UserId"></param>
        /// <returns>List<CuBrand></returns>
        Task<List<CuBrand>> GetCustomerBrandsByUserId(int CustomerId);

        /// <summary>
        /// Get Customer's Departments By UserId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="UserId"></param>
        /// <returns>List<CuDepartment></returns>
        Task<List<CuDepartment>> GetCustomerDepartmentByUserId(int CustomerId);

        /// <summary>
        /// Get Customer's Season By UserId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>List<CuSeason></returns>
        Task<List<CuSeason>> GetCustomerSeason(int CustomerId);



        /// <summary>
        /// Get Audit Service Type
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>List<CuSeason></returns>
        Task<List<CuServiceType>> GetCustomerAuditServiceType(int CustomerId);

        /// <summary>
        /// Get Inspection Service Type
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<List<CuServiceType>> GetCustomerInspectionServiceType(int CustomerId);

        /// <summary>
        /// Get Customers Items By SupplierId
        /// </summary>
        /// <returns></returns>
        Task<List<CuCustomer>> GetCustomersBySupplierId(int SupplierId);

        CuCustomer GetCustomerByID(int? customerID);

        Task<List<CuBuyer>> GetCustomerBuyerByUserId(int CustomerId);

        /// <summary>
        /// Get CustomerList ByCountry And Service
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        Task<IEnumerable<DataSource>> GetCustomerListByCountryAndService(int countryId, int serviceId);




        /// <summary>
        /// Get ReInspection Service Types
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="ProductCategoryId"></param>
        /// <returns></returns>
        Task<List<CuServiceType>> GetReInspectionServiceType(int CustomerId);

        /// <summary>
        /// Get customer list
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CuCustomer>> GetCustomerList();
        /// <summary>
        /// get customer list based on country id and service id
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="serviceId"></param>
        /// <returns>list of customer</returns>
        Task<IEnumerable<CuCustomer>> GetEditCustomerListByCountryAndService(int countryId, int serviceId);

        /// <summary>
        /// Get Customer Contacts By Id
        /// </summary>
        /// <returns></returns>
        Task<List<Contact>> GetCustomerContacts(int customerId);
        /// <summary>
        /// Get Customer data By Id
        /// </summary>
        /// <returns></returns>
        Task<CuCustomer> GetCustomerItemById(int customerId);
        /// <summary>
        /// Get Customer Data By ZohoId
        /// </summary>
        /// <param name="zohoId"></param>
        /// <returns></returns>
        Task<ZohoCustomer> GetCustomerByZohoId(long zohoId);
        Task<List<ZohoCustomer>> GetCustomerDetailsByName(string Name);
        Task<List<ZohoCustomerAddress>> GetZohoCustomerAddressById(int customerId);
        Task<List<CustomerSource>> GetCustomerByName(string Name);
        Task<List<CustomerSource>> GetCustomerByGLCode(string glCode);
        Task<CuCustomer> GetCustomerDetailsByZohoID(long id);
        Task<CuCustomer> GetCustomerDetailsByGLCode(string glCode);
        Task<List<CustomerSource>> GetOtherAcountGLCode(string glCode, int customerId);
        Task<List<CustomerSource>> GetOtherAcountCustomerName(string name, int customerId);
        Task<List<CustomerSource>> GetCustomerByEmail(string email);
        Task<List<CustomerSource>> GetOtherAccountEmail(string email, int customerId);
        Task<List<CommonDataSource>> GetCustomerMerchandiserById(int CustomerId);
        Task<int> GetCustomerHeadOfficeAddressById(int customerId);
        Task<List<CustomerAccountingAddress>> GetCustomerAddressByListCusId(List<int> lstcustomerId);
        Task<List<CustomerAccountingAddress>> GetCustomerAddressByCusIds(List<int> lstcustomerId);
        Task<List<CustomerCustomStatus>> GetCustomStatusNameByCustomer(List<int> customerIds);
        Task<List<CommonDataSource>> GetCustomerCollection(int CustomerId);
        Task<List<CommonDataSource>> GetCustomerPriceCategory(int CustomerId);
        Task<List<CommonDataSource>> GetCustomerByNameAutocomplete(string customerName);
        Task<List<CommonDataSource>> GetCustomerById(List<int> customerIds);

        /// <summary>
        /// Get the customer brands by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetCustomerBrands(int CustomerId);

        /// <summary>
        /// Get the customer departments by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetCustomerDepartments(int CustomerId);

        /// <summary>
        /// Get the customer buyers by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetCustomerBuyers(int CustomerId);
        /// <summary>
        /// Get the customer departments by customer id
        /// </summary>
        /// <param name="CustomerIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ParentDataSource>> GetCustomerDepartments(IEnumerable<int> customerIds);
        /// <summary>
        /// Get the customer brands by customer id
        /// </summary>
        /// <param name="CustomerIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ParentDataSource>> GetCustomerBrands(IEnumerable<int> customerIds);

        /// <summary>
        /// Get the FB Customer Data
        /// </summary>
        /// <returns></returns>
        Task<FBCustomerMasterData> GetFBCustomerDataById(int customerId);
        /// <summary>
        /// Get Customer contacts
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetCustomerContactListbyCustomer(int CustomerId);
        /// <summary>
        /// Get Customer address by customer 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<List<CommonAddressDataSource>> GetCustomerAddress(int CustomerId);
        IQueryable<CuCustomer> GetCustomerDataSource();
        /// <summary>
        /// get active price category list by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IQueryable<CommonDataSource> GetPriceCategoryDataSource(int customerId);

        IQueryable<CustomerDataSource> GetCustomerDataSourceFromSupplier();

        Task<CustomerKamDetail> GetCustomerKAMDetails(int customerId);

        Task<List<CommonDataSource>> GetEditBookingBrands(int CustomerId, int bookingId);

        Task<List<CommonDataSource>> GetEditBookingDepartments(int CustomerId, int bookingId);

        Task<List<CommonDataSource>> GetEditBookingBuyers(int CustomerId, int bookingId);

        Task<List<CommonDataSource>> GetEditBookingPriceCategory(int CustomerId, int bookingId);

        Task<List<CommonDataSource>> GetEditBookingMerchandisers(int CustomerId, int bookingId);

        Task<List<CommonDataSource>> GetEditBookingCollection(int CustomerId, int bookingId);

        Task<List<CommonDataSource>> GetEditBookingServiceType(int customerId, int bookingId);

        Task<List<CommonDataSource>> GetCustomerProductCategories(int customerId);

        IQueryable<CuSeasonConfig> GetCustomerSeasonConfigQuery();

        Task<string> GetCustomerContactEmailbyUserid(int Userid);

        Task<List<int>> GetCustomerContactServiceIds(int customerContactId, int primaryEntity);

        Task<CommonDataSource> GetCustomerContactPrimaryEntity(int customerContactId);

        Task<CustomerPriceData> GetCustomerPriceData(int customerId);

        Task<List<CommonDataSource>> GetCustomerProductCategoryList(int CustomerId);

        Task<List<CommonDataSource>> GetCustomerProductSubCategoryList(int CustomerId, List<int?> ProductCategory);

        Task<List<ParentDataSource>> GetCustomerProductCategoryListByBookingIds(IEnumerable<int> bookingIds);
        Task<List<CommonDataSource>> GetCustomerEntityByCustomerId(int customerId);
        Task<List<CommonDataSource>> GetCustomerSisterCompanyByCustomerId(int customerId);

        Task<List<CuContactEntityMap>> GetCustomerContactEntityMapByCustomerId(IEnumerable<int> contactIds);
        Task<List<CuContactEntityServiceMap>> GetCustomerContactEntityServiceMapByCustomerId(IEnumerable<int> contactIds);

        Task<List<CommonDataSource>> GetCustomerProductSub2CategoryList(List<int?> productCategory, List<int?> productSubCategory);
        Task<List<ParentDataSource>> GetCustomerProductCategoryByProductSubCategoryIds(List<int> productSubCategoryIds);
        Task<List<ParentDataSource>> GetCustomerProductTypeByProductCategoryIds(List<int> productSubCategory2Ids);

        Task<CuCustomer> GetCustomerItemByIdForCFL(int customerId);

        Task<List<CommonDataSource>> GetCustomerProductsByName(int customerId, List<string> productNameList);
        Task<EmailEntityResponse> GetCustomerContactEmailEntityByUserId(int userId);
        Task<bool> IsCustomerExists(Expression<Func<CuCustomer, bool>> predicate);

        Task<List<CuContactSisterCompany>> GetSisterCompaniesContactByCustomerContactIds(IEnumerable<int> contactIds);
        Task<List<int>> GetSisterCompanieIdsByCustomerContactId(int contactId);

        Task<List<LocationDto>> GetCustomerAddressByCustomerIds(List<int?> customerIds);
        Task<CuCustomer> GetCustomerDetailsByCustomerId(int id);
        Task<int> AddCustomerAddress(CuAddress entity);
        Task<CuAddress> GetEaqfCustomerAddress(int customerId, int addressId);
        Task<int> EditCustomerAddress(CuAddress entity);
        Task<int> AddCustomerContact(CuContact entity);
        Task<int> EditCustomerContact(CuContact entity);
        Task<CuCustomer> GetEaqfCustomerDetailsByCustomerId(int customerId);
        Task<bool> IsCustomerContactExists(Expression<Func<CuContact, bool>> predicate);

        Task<CuCustomer> GetCustomerDataByCustomerIdAndEntityId(int customerId, int entityId);
        Task<List<CustomerContactBaseData>> GetCustomerContact(int customerId);
    }
}
