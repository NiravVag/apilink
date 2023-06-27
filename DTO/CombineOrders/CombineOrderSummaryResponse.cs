using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CombineOrders
{
    public class CombineOrderSummaryResponse
    {
        public IEnumerable<CombineOrders> CombineOrdersList { get; set; }

        public bool IsBookingRequestRole { get; set; }

        public bool IsBookingConfirmRole { get; set; }

        public bool IsBookingVerifyRole { get; set; }

        public int bookingStatus { get; set; }

        public int totalNumberofReports { get; set; }

        public CombineOrdersSummaryResponseResult Result { get; set; }

    }

    public class CombineOrderSamplingData
    {

        public int ProductId { get; set; }
        public int CombineProductId { get; set; }
        public int OrderQuantity { get; set; }
        public int AqlQuantity { get; set; }
        public int SamplingQuantity { get; set; }
    }

    public enum CombineOrdersSummaryResponseResult
    {
        success = 1,
        failed = 2
    }
}
