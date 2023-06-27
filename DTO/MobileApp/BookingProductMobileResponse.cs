using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class BookingProductMobileResponse
    {
        public MobileResult meta { get; set; }
        public BookingProductData data { get; set; }
    }

    public class BookingProductData
    {
        public List<MobileInspectionReportProducData> bookingProductsList { get; set; }
        public List<BookingRepoStatus> bookingStatusList { get; set; }
        public string productUpdatedDate { get; set; }
    }
}
