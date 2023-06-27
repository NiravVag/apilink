using DTO.CommonClass;
using DTO.Customer;
using DTO.References;
using DTO.UserAccount;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DTO.Common.ApiCommonData;

namespace Contracts.Managers
{
    public interface ICustomerContactManager
    {
        Task<IEnumerable<CustomerContact>> GetCustomerContacts(int CustomerId);
        CustomerContactSearchResponse GetCustomerContactData(CustomerContactSearchRequest request);
        Task<CustomerContactSummaryResponse> GetCustomerContactSummary(CustomerContactSummaryRequest request);
        Task<CustomerContactDeleteResponse> DeleteCustomerContact(int id);
        Task<EditCustomerContactResponse> GetEditCustomerContact(int? id);
        Task<CustomerContactResponse> GetCustomerContact(int? customerID);
        Task<SaveCustomerContactResponse> Save(CustomerContactDetails request);
		Task<EditCustomerContactResponse> GetContactBrandByCusId(int customerId);
        Task<CustomerContactDataResponse> GetCustomerContact(int customerId, int contactId);
        Task<object> SaveZohoCRMContact(SaveZohoCrmCustomerContactDetails request, string zohoCustomerId,string zohoContactId);
        Task<object> UpdateZohoCRMContact(SaveZohoCrmCustomerContactDetails request, string zohoCustomerId, string zohoContactId);
        Task<object> GetCustomerContactByZohoData(long zohoCustomerId, long zohoContactId);
        Task<object> GetCustomerContactByEmailAndID(long zohoContactId, string email);
        Task<object> GetCustomerContactByZohoCustomerId(long zohoCustomerId);
        Task<LinkErrorResponse> ValidateSaveZohoRequest(string customerId, string customerContactId, SaveZohoCrmCustomerContactDetails request,int requestType);
        //Task<LinkErrorResponse> ValidateUpdateZohoRequest(string customerId, string customerContactId, SaveZohoCrmCustomerContactDetails request, int requestType);
        Task<DataSourceResponse> GetCustomerContactByBooking(int bookingId);
        Task<DataSourceResponse> GetCustomerContactDataSourceList(CustomerContactDataSourceRequest request);
        Task<DataSourceResponse> GetCustomerContactByBookingAndService(int bookingId, int serviceId);
        Task<CustomerContactSummaryResponse> GetCustomerContactsByCustomerId(int customerId);
        Task<SaveUserResponse> CreateCustomerContactUserCredential(CustomerContactUserRequest request, CustomerContactCredentialsFrom contactrequestfrom);
        Task<IEnumerable<CustomerBrand>> GetCustomerBrandsByUserId(int CustomerId, int UserId);
        Task<IEnumerable<CustomerDepartment>> GetCustomerDepartmentByUserId(int CustomerId, int UserId);
    }
}
