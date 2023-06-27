using DTO.Supplier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.Location;
using Entities;
using DTO.CommonClass;
using System.Linq;
using DTO.DefectDashboard;
using DTO.Quotation;

namespace Contracts.Managers
{
    public interface ISupplierManager
    {
        /// <summary>
        /// Get Supplier Suammary 
        /// </summary>
        /// <returns></returns>
        Task<SupplierSummaryResponse> GetSupplierSummary();

        /// <summary>
        /// Delete supplier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SupplierDeleteResponse> DeleteSupplier(int id);

        /// <summary>
        /// Get Edit supplier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EditSupplierResponse> GetEditSupplier(int? id);

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveSupplierResponse> Save(SupplierDetails request);

        /// <summary>
        /// Get suppliers by Customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<SupplierListResponse> GetSuppliersByCustomerId(int customerId);

        /// <summary>
        /// Get supplier Contacts by id
        /// </summary>
        /// <param name="supplierId"></param>
        ///  /// <param name="cusid"></param>
        /// <returns></returns>
        Task<IEnumerable<suppliercontact>> GetSupplierContactsById(int supid, int cusid);

        /// <summary>
        /// Get factory by Customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<SupplierListResponse> GetFactoryByCustomerId(int customerId);

        /// <summary>
        /// Get factory by supplier id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<SupplierListResponse> GetFactoryBySupplierId(int supplierId);

        /// <summary>
        /// Get factory by Customer id and supplier id 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<SupplierListResponse> GetFactoryByCustomerSupplierId(int? customerId, int? supplierId);

        /// <summary>
        /// Get factory Contacts by id
        /// </summary>
        /// <param name="supid"></param>
        /// <param name="cusid"></param>
        /// <returns></returns>
        Task<IEnumerable<suppliercontact>> GetFactoryContactsById(int factid, int cusid);

        /// <summary>
        /// Get suppliers by User Type
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<SupplierListResponse> GetSuppliersByUserType(int? CustomerId, bool isBookingRequest = false);

        /// <summary>
        /// Get factory by User Type
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<SupplierListResponse> GetFactorysByUserType(int? CustomerId, int? SupplierId);

        /// <summary>
        /// Get supplier By FactId
        /// </summary>
        /// <param name="factoryId"></param>
        /// <returns></returns>
        Task<SupplierListResponse> GetSupplierByFactId(int factoryId);

        /// <summary>
        /// Get supplier By SupplierName/ID
        /// </summary>
        /// <param name="factoryId"></param>
        /// <returns></returns>
        Task<SupplierListResponse> GetSupplierByName(string supName, int type);

        /// <summary>
        /// Get supplier head office regional/english address By supplier ID
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns>supplier address </returns>
        Task<SupplierAddress> GetSupplierHeadOfficeAddress(int supplierId);

        /// <summary>
        /// Get Supplier Code By supplier Id and customer Id
        /// </summary>
        /// <returns></returns>
        Task<string> GetSupplierCode(int Supid, int cusid);

        Task<List<SupplierAddress>> GetSupplierOfficeAddressBylstId(List<int> lstsupplierId);

        /// <summary>
        /// Get supplier or factory details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EditSupplierResponse> GetSupplierOrFactoryDetails(int? id);

        /// <summary>
        /// Get the factory id by supplier id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<int> GetFactoryIdBySupplierId(int supplierId);
        /// <summary>
        /// Get supplier or factory details by countryId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<int>> GetFactoryByCountryId(List<int> countrylist);

        /// <summary>
        /// Get supplier or factory details for vertical scroll
        /// </summary>
        /// <param name="search text, customer Id"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetSupplierDataSource(CommonDataSourceRequest request);

        Task<DataSourceResponse> GetFactoryOrSupplierList(CommonSupplierSourceRequest request);

        Task<SupplierSearchItemResponse> GetSupplierSearchData(SupplierSearchRequestNew request);

        Task<SupplierSearchItemResponse> GetSupplierSearchChildData(int id, int supplierType);

        /// <summary>
        /// Get the supplier contacts by supplier id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetSupplierContactsbySupplier(int supplierId);

        Task<List<SupplierGeoLocation>> GetSupplierGeoLocation(IEnumerable<int> supplierIds);
        /// <summary>
        /// get supplier or factory address details
        /// </summary>
        /// <param name="supplierIds"></param>
        /// <returns></returns>
        Task<IEnumerable<SupplierGeoLocation>> GetSupplierOrFactoryLocations(IEnumerable<int?> supplierIds);

        /// <summary>
        /// Get the supplier contacts
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetSupplierContactByBooking(int bookingId, int supType, int serviceType);

        /// <summary>
        /// Get supplier or factory details for vertical scroll
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetSupplierDataSourceList(SupplierDataSourceRequest request);

        Task<DataSourceResponse> GetFactoryDataSourceBySupplier(CommonDataSourceRequest request);

        /// <summary>
        /// get supplier data
        /// </summary>
        /// <param name="supplierIdList"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetSupplierById(List<int> supplierIdList);

        /// <summary>
        /// get supplier and factory list without any dependancy
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetSupplierList(CommonDataSourceRequest request);

        Task<DataSourceResponse> GetFactoryrList(CommonDataSourceRequest request);

        IQueryable<SuSupplier> GetSupplierList();

        Task<List<CountryListModel>> GetFactoryCountryById(IEnumerable<int> factoryIds);

        Task<List<CommonDataSource>> GetEditBookingSuppliers(int? customerId, int bookingId);

        Task<SupplierExportDataResponse> GetSupplierSummaryExportDetails(SupplierSearchRequestNew request);

        Task<List<CommonDataSource>> GetSupplierByCountryDataSourceNew(CommonSupplierSourceRequest request);

        Task<AddressDataSourceResponse> GetSupplierFactorAddressById(int supplierFactoryId);

        Task<SupplierContactDataResponse> GetBaseSupplierContactData(int supplierId);

        Task<SupplierDataResponse> GetExistSupplierDetails(SupplierDetails request);

        Task<SaveSupplierResponse> UpdateSupplierEntity(SupplierData request);

        Task<List<SupplierCode>> GetSupplierCode(List<int> customerIds, List<int> supplierIds);

        Task<DataSourceResponse> GetSupplierLevelByCustomerId(int customerId);

        Task<SaveSupplierResponse> SaveEaqfSupplier(EaqfSupplierDetails request, int type, int customerId, int? userId, int? supplierId = null);

        Task<SupplierGradeResponse> GetGradeBySupplierIdAndCustomerIdAndBookingIds(SupplierGradeRequest request);
    }
}
