using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerBrandsListResponse
    {
        public IEnumerable<CommonDataSource> CustomerBrandsList { get; set; }

        public CustomerBrandsListResult Result { get; set; }
    }

    public enum CustomerBrandsListResult
    {
        Success = 1,
        CannotGetBrands = 2
    }
}
