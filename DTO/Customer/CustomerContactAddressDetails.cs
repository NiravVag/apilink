using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class CustomerContactAddressDetails
    {
        public List<CommonAddressDataSource> AddressList { get; set; }
        public List<CustomerContactBaseData> ContactList { get; set; }
        public CustomerAddressContactResult Result { get; set; }
    }

    public class CustomerContactBaseData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CustomerId { get; set; }
    }

    public enum CustomerAddressContactResult
    {
        Success = 1,
        AddressNotFound = 2,
        ContactNotFound=3
    }

    public class CustomerBaseAddress
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string Address { get; set; }
        public string RegionalLanguage { get; set; }
        public int? CountryId { get; set; }
    }
}
