using DTO.CommonClass;
using DTO.Customer;
using DTO.User;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICustomerContactRepository : IRepository
    {

        /// <summary>
        /// Remove CustomerContacts
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool value</returns>
        Task<bool> RemoveCustomerContact(int id);
        /// <summary>
        /// Get Customer's Contacts
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>List<CuContact></returns>
        Task<List<CuContact>> GetCustomerContacts(int? CustomerId);
        CuCustomer GetCustomerContactsByID(int? customerID);
        CuContact GetCustomerContactByContactID(int? contactID);
        IEnumerable<CuAddress> GetCustomerAddressByCustomerID(int? customerID);
        IEnumerable<CuContactType> GetContactTypes();
        int AddCustomerContact(CuContact entity);
        int EditCustomerContact(CuContact entity);
        /// <summary>
        /// Get Customer's Contacts by customer contactId list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>List<CuContact></returns>
        Task<List<CuContact>> GetCustomerContactsList(List<int> cuscontactids);

		/// <summary>
		/// Save Contact Brand
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		int SaveContactBrand(CuContactBrand entity);

		/// <summary>
		/// Save Contact Department
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		int SaveContactDepartment(CuContactDepartment entity);

		/// <summary>
		/// Save Contact Service
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		int SaveContactService(CuContactService entity);
        /// <summary>
        /// Get customer contacts by customerid and contactid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="contactID"></param>
        /// <returns></returns>
        Task<CuContact> GetCustomerContacts(int customerId, int contactID);
        Task<CuContact> GetCustomerContactByZohoData(long zohoCustomerID, long zohoContactID);
        Task<CuContact> GetCustomerContactByEmailAndZohoID(long zohoContactID, string email);
        Task<List<CuContact>> GetCustomerContactByZohoId(long zohoCustomerId);
        Task<CuContact> GetCustomerContactByZohoContactId(long zohoContactId);
        Task<ZohoCustomerContact> GetCustomerContactByEmail(string email,int customerid);
        Task<ZohoCustomerContact> GetOtherContactByEmail(string email, int id);
        Task<List<CommonDataSource>> GetCustomerContactByBooking(int bookingId);

        IQueryable<CuContact> GetCustomerContactDataSourceList();

        Task<List<CommonDataSource>> GetAuditCustomerContactByBooking(int bookingId);
        Task<IEnumerable<CommonDataSource>> GetCustomerContactList(int customerId, int contactId);
        Task<List<string>> GetContactEmailIds(List<string> emailIds);
        Task<List<CustomerContact>> GetCustomerContactByCustomerId(int customerId);
        Task<List<ParentDataSource>> GetCustomerContactTypesByContactIds(IEnumerable<int> contactIds);
        Task<List<CuContactData>> GetCustomerContactByCustomerIds(IEnumerable<int> customerIds);
        Task<List<CuAddressesData>> GetCustomerAddressByCustomerId(IEnumerable<int> customerIds);
        Task<List<GetEaqfCustomerAddressData>> GetCustomerAddressListByCustomerId(IEnumerable<int> customerIds);
        Task<CuContact> GetCustomerContactByCustomerIdAndContactId(int customerId, int contactId);
        Task<List<CustomerContact>> GetCustomerContactByContactId(int contactId);
    }
}
