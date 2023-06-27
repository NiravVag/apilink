using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerContactDataResponse
    {
        public CustomerContactData CustomerContactDetails { get; set; }

        public CustomerContactDataResult Result { get; set; }
    }

    public enum CustomerContactDataResult
    {
        Success = 1,
        CannotGetContactData = 2
    }

    public class CustomerContactListResponse
    {
        public List<CustomerContactData> CustomerContactDetails { get; set; }

        public CustomerContactDataResult Result { get; set; }
    }

    public enum CustomerContactListResult
    {
        Success = 1,
        CannotGetContactData = 2
    }
}
