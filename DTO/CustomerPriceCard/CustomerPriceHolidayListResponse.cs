using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CustomerPriceCard
{
    public class CustomerPriceHolidayListResponse
    {
        public IEnumerable<CommonDataSource> CustomerPriceHolidayList { get; set; }

        public CustomerPriceHolidayListResult Result { get; set; }
    }

    public enum CustomerPriceHolidayListResult
    {
        Success = 1,
        CannotGetHolidayList = 2
    }
}
