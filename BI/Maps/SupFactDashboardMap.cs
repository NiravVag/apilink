using DTO.Common;
using DTO.Dashboard;
using DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public  class SupFactDashboardMap: ApiCommonData
    {
        //map the booking details
        public BookingDetails BookingDataMap(BookingDetailsRepo bookingdata, int userId, List<ServiceTypeList> serviceTypeList)
        {
            return new BookingDetails()
            {
                BookingId = bookingdata.BookingId,
                CountryName = bookingdata.CountryName,
                CustomerName = bookingdata.CustomerName,
                SupplierName = bookingdata.SupplierName,
                FactoryName = bookingdata.FactoryName,
                ServiceFromDate = bookingdata.ServiceFromDate.ToString(StandardDateFormat),
                ServiceToDate = bookingdata.ServiceToDate.ToString(StandardDateFormat),
                ServiceType = string.Join(", ", serviceTypeList?.Where(x => x.InspectionId == bookingdata.BookingId).Select(x => x.serviceTypeName).ToList()),
                IsEdit = bookingdata.CreatedBy.GetValueOrDefault() == userId
            };
        }
    }
}
