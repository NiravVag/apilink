using DTO.Common;
using DTO.DefectDashboard;
using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI.Maps
{
    public  class DefectDashboardMap: ApiCommonData
    {
        //map the booking report data
        public  BookingReportModel BookingReportDataMap(BookingReportData bookingReportData)
        {
            return new BookingReportModel()
            {
                BookingId = bookingReportData.BookingId,
                ReportId = bookingReportData.ReportId,
                FactoryId = bookingReportData.FactoryId
            };
        }
    }
}
