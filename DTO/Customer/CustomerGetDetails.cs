using System.Collections.Generic;

namespace DTO.Customer
{
    public class CustomerGetDetails
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int? UserId { get; set; }
        public List<GetEaqfCustomerAddressData> AddressList { get; set; }
    }
    public class CuAddressesData
    {
        public int? CustomerId { get; set; }
        public string Alpha2Code { get; set; }
    }
    public class CuContactData
    {
        public int? CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public int? UserId { get; set; }
    }
    public class GetEaqfCustomer
    {
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<CustomerGetDetails> customerList { get; set; }
    }
}
