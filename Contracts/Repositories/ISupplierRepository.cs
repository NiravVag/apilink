using DTO.CommonClass;
using DTO.DefectDashboard;
using DTO.FullBridge;
using DTO.Quotation;
using DTO.Supplier;
using DTO.User;
using DTO.UserAccount;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ISupplierRepository : IRepository
    {

        Task<bool> IsSupplierExists(SupplierDetails request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SuSupplier> GetSupplierDetails(int id);

        /// <summary>
        /// Add supplier
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> AddSupplier(SuSupplier entity);

        /// <summary>
        /// Edit supplier
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> EditSupplier(SuSupplier entity);

        /// <summary>
        /// Remove Supplier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> RemoveSupplier(int id);

        /// <summary>
        ///  Get Supplier types
        /// </summary>
        /// <returns></returns>
        Task<List<SuType>> GetTypes();

        /// <summary>
        /// Get levels
        /// </summary>
        /// <returns></returns>
        Task<List<SuLevel>> GetLevels();

        /// <summary>
        /// Get Owners
        /// </summary>
        /// <returns></returns>
        Task<List<SuOwnlerShip>> GetOwners();

        /// <summary>
        /// Get Address types
        /// </summary>
        /// <returns></returns>
        Task<List<SuAddressType>> GetAddressTypes();

        /// <summary>
        /// Get Supplier by CustomerId
        /// </summary>
        /// <returns></returns>
        Task<List<SuSupplier>> GetSupplierByCustomerId(int customerId);

        /// <summary>
        /// Get Supplier Contacts By Id , customer id
        /// </summary>
        /// <returns></returns>
        Task<List<SuContact>> GetSuppliercontactById(int Supid, int cusid);

        /// <summary>
        /// Get factory by CustomerId
        /// </summary>
        /// <returns></returns>
        Task<List<SuSupplier>> GetFactoryByCustomerId(int customerId);

        /// <summary>
        /// Get factory by CustomerId and supplier Id
        /// </summary>
        /// <returns></returns>
        Task<List<SuSupplier>> GetFactoryByCustomerIdSupplierId(int? customerId, int? supplierId);

        /// <summary>
        /// Get factory by CustomerId
        /// </summary>
        /// <returns></returns>
        Task<List<SuSupplier>> GetFactoryBySupplierId(int Supid);

        /// <summary>
        /// Get supplier by factoryid
        /// </summary>
        /// <returns></returns>
        Task<List<SuSupplier>> GetSupplierByfactId(int factid);

        IEnumerable<SuSupplier> GetAllSuppliers();

        /// <summary>
        /// Get supplier / factory contact details
        /// </summary>
        /// <returns></returns>
        Task<List<SuContact>> GetSupplierContactsList(List<int> contactids);

        /// <summary>
        /// Get CreditTerm List
        /// </summary>
        /// <returns></returns>
        Task<List<SuCreditTerm>> GetCreditTerms();

        /// <summary>
        /// Get Status
        /// </summary>
        /// <returns></returns>
        Task<List<SuStatus>> GetStatus();

        /// <summary>
        /// Get supplier by SupplierName
        /// </summary>
        /// <returns></returns>
        IQueryable<SuSupplier> GetAllSuppliersByName(string supName);

        /// <summary>
        /// Get the supplier datasource repo
        /// </summary>
        /// <returns></returns>
        IQueryable<SuSupplier> GetSupplierDataSource();

        /// <summary>
        /// Get supplier head office regional/english address By supplier ID
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns>supplier address </returns>
        Task<SupplierAddress> GetSupplierHeadOfficeAddress(int supplierId);

        /// <summary>
        /// Get Supplier Contacts By Id
        /// </summary>
        /// <returns></returns>
        Task<List<Contact>> GetSupplierContactById(int Supid);

        /// <summary>
        /// Fetch Supplier code
        /// </summary>
        /// <param name="supplierId"></param>
        /// /// <param name="customerId"></param>
        /// <returns>supplier address </returns>
        Task<List<SupplierCode>> GetSupplierCode(List<int> customerId, List<int> supplierIds);

        Task<List<SupplierCode>> GetSupplierCode(IQueryable<int> bookingIds);

        /// <summary>
        /// Get Supplier Code By supplier Id and customer Id
        /// </summary>
        /// <returns></returns>
        Task<string> GetSupplierCode(int Supid, int cusid);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<string> GetSupplierHeadOfficeAddressById(int supplierId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name=" lst supplierId"></param>
        /// <returns></returns>
        Task<List<SupplierAddress>> GetSupplierOfficeAddressBylstId(List<int> lstsupplierId);

        Task<List<SupplierAddress>> GetSupplierOfficeAddressBySupplierIds(List<int> lstsupplierId);

        /// <summary>
        /// get supplier factory details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SuSupplier> GetSupplierORFactoryDetails(int id);

        /// <summary>
        /// get supplier by countryIds
        /// </summary>
        /// <param name="countrylist"></param>
        /// <returns></returns>
        Task<List<int>> GetFactoryByCountryId(List<int> countrylist);

        /// <summary>
        /// get supplier by provinceId
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        Task<List<int>> GetSupplierByProvinceId(int provinceId);

        /// <summary>
        /// Get the factory id by supplier id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<int> GetFactoryIdBySupplierId(int supplierId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierId"></param>Su
        /// <returns></returns>
        Task<FBSupplierMasterData> GetFBSupplierData(int supplierId);
        IQueryable<SuSupplier> GetAllSuppliersAndCountryList();
        Task<IEnumerable<SupplierAddressData>> GetSupplierAddressDataList(IEnumerable<int> supplierIds);
        IQueryable<SuSupplier> GetSuppliersSearchData();
        Task<IEnumerable<SupplierInvolvedData>> GetSupplierInvolvedItemsCount(IEnumerable<int> supplierIds);
        IQueryable<SupplierSearchItemRepo> GetSuppliersSearchChildData(int id, int supplierType);
        Task<IEnumerable<SupplierAddressData>> GetSupplierAddressDataByIds(List<int> supplierIds);

        Task<IEnumerable<SupplierAddressData>> GetSupplierAddressDataByIds(IQueryable<int> supplierIds);

        /// <summary>
        /// get supplierId contacts by supplierId id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetSupplierContactListbySupplier(int supplierId);

        Task<List<SupplierGeoLocation>> GetSupplierGeoLocations(IEnumerable<int> lstsupplierId);
        /// <summary>
        /// get supplier or factory address details
        /// </summary>
        /// <param name="lstsupplierId"></param>
        /// <returns></returns>
        Task<IEnumerable<SupplierGeoLocation>> GetSupplierOrFactoryLocations(IEnumerable<int?> lstSupplierId);

        /// <summary>
        /// get the supplier or factory contats by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="supType"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetSupplierContactByBooking(int bookingId, int supType);

        Task<List<CommonDataSource>> GetSupplierContactByBookingForAudit(int bookingId, int supType);
        /// <summary>
        /// Get supplier data
        /// </summary>
        /// <param name="supplierIdList"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetSupplierById(List<int> supplierIdList);

        /// <summary>
        /// get the supplier list for virtual scroll
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        IQueryable<SuSupplier> GetSupplierDataSourceList(int typeId);

        Task<List<CountryListModel>> GetFactoryCountryById(IEnumerable<int> factoryIds);

        Task<List<CommonDataSource>> GetEditBookingSuppliersByCustId(int? customerId, int bookingId);

        Task<List<CommonDataSource>> GetEditBookingSuppliersBySupId(int supplierId);

        Task<List<CommonDataSource>> GetEditBookingSuppliersByfactId(int factoryId, int bookingId);

        Task<List<SupplierCustomerRepo>> GetSupplierCustomer(IQueryable<int> supplierIds);

        Task<List<SupplierContactRepo>> GetSupplierContactDetailsBySupplierIdQuery(IQueryable<int> supplierIds);

        Task<List<SupplierExportRepo>> GetFactoryDetailsBySupplierIdQuery(IQueryable<int> supplierIds);

        Task<List<SupplierServiceExportRepo>> GetSuAPIServiceBySupplierIdQuery(IQueryable<int> supplierIds);

        Task<string> GetSupplierContactEmailbyUserid(int Userid);
        Task<List<int>> GetSupplierContactServiceIds(int supplierContactId, int primaryEntity);

        Task<CommonDataSource> GetSupplierContactPrimaryEntity(int supplierContactId);

        Task<List<CommonDataSource>> GetAddressCountry(IQueryable<int> supplierid);

        Task<List<string>> GetContactEmailIds(List<string> emailIds, int typeId);

        Task<List<suppliercontact>> GetBaseSupplierContactDataById(int supplierId);

        Task<List<SupplierData>> SupplierDetailsExists(SupplierDetails request);

        Task<SuSupplier> GetSupplierDetailById(int id);
        /// <summary>
        /// Get SupplierId by Code from SuSupplierCustomer Table
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<int> GetSupplierIdByCode(string code);


        Task<SuSupplier> GetSupplierByName(string name, int type);

        Task<SuSupplier> GetSupplierById(int supplierId, int type);
        Task<bool> IsSupplierExistsByCustomer(int supplierId, int clientId, int type);
        Task<List<SuContact>> GetSupplierContactBySupId(int supid);

        /// <summary>
        /// Get Address id by supplier id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<int> GetAddressIdBySuppllierId(int supplierId);
        Task<List<SupplierCustomerData>> GetSupplierCustomerData(List<int> customerIds);

        Task<List<SuLevelCustomDto>> GetSupplierLevelByCustomerId(int customerId);

        Task<List<SupplierGradeRepo>> GetSupplierGradeDetailsBySupplierId(int supplierId);
        Task<List<SuGrade>> GetSupplierGradesBySupplierId(int supplierId);
        Task<List<SupplierGradeRepo>> GetGradeByCustomerSupplier(int customerId, int supplierId);

        Task<EmailEntityResponse> GetSupplierContactEmailEntityByUserId(int userId);

        Task<SuSupplier> GetSupplierDataBySupplierIdAndEntityId(int supplierId, int entityId);

        Task<List<SupplierAddressData>> GetSupplierAddressBySupplierIds(IEnumerable<int> supplierIds);

        Task<List<SupplierGradeRepo>> GetGradeByCustomersAndSuppliers(List<int> customerIds, List<int> supplierIds);
    }
}
