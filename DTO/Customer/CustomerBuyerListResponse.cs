using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerBuyerListResponse
    {
        public IEnumerable<CommonDataSource> CustomerBuyersList { get; set; }

        public CustomerBuyersListResult Result { get; set; }
    }

    public enum CustomerBuyersListResult
    {
        Success = 1,
        CannotGetBuyers = 2
    }
}
