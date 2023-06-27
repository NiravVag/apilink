using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public enum ZohoInvoiceTypeEnum
    {
        MonthlyInvoice=1,
        PreInvoice = 2
    }

    public enum ZohoCountryEnum
    {
       China=38
    }

    public enum ZohoCustomerRequestEnum
    {
        Save = 1,
        Update=2
    }

    public enum ZohoCustomerAddressTypeEnum
    {
        HeadOffice = 1,
        Accounting=2
    }

    public enum ZohoCustomerContactType
    {
        Operations=1,
        Accounting=2
    }

    public class ZohoCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GLCode { get; set; }
        public string EmailId { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class ZohoCustomerContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ZohoCustomerAddress
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int AddressType { get; set; }
    }
}
