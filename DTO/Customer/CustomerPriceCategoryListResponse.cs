using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerPriceCategoryListResponse
    {
        public IEnumerable<CommonDataSource> CustomerPriceCategoryList { get; set; }

        public CustomerPriceCategoryResult Result { get; set; }
    }

    public enum CustomerPriceCategoryResult
    {
        Success = 1,
        CannotGetPriceCategory = 2
    }
}
