using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Quotation
{
    public class FactoryBookingInfoRequest
    {
        public int FactoryId { get; set; }
        public IEnumerable<int> BookingIds { get; set; }
    }

    public class FactoryBookingInfoResponse
    {
        public IEnumerable<FactoryBookingInfo> FactoryBookingInfolst { get; set; }
        public FactoryBookingInfoResult Result { get; set; }
    }

    public class FactoryBookingInfo
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceDate { get; set; }
        public int ProductCount { get; set; }
        public int ReportCount { get; set; }
        public int SampleSize { get; set; }
        public double Manday { get; set; }
        public double InspectionFee { get; set; }
        public double TravelCost { get; set; }
        public double OtherCost { get; set; }
        public double? SuggestedManday { get; set; }
        public bool IsQuotation { get; set; }
        public string ServiceTypeName { get; set; }
    }

    public enum FactoryBookingInfoResult
    {
        Success = 1,
        NotFound = 2,
        RequestNotCorrectFormat = 3
    }
}
