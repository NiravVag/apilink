using DTO.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionPicking
{
    public class CustomerContactsResponse
    {
        public List<CustomerAddressData> CustomerAddressList { get; set; }

        public IEnumerable<CustomerContact> CustomerContactList { get; set; }

        public CustomerContactResponseResult Result { get; set; }
    }

    public enum CustomerContactResponseResult
    {
        success = 1,
        addressnotfound = 2,
        contactnotfound = 3
    }
}
