using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class InspectionBaseAndProductDetails
    {
        public int BookingNo { get; set; }

        public string Status { get; set; }
        public string Customer { get; set; }

        public string Supplier { get; set; }

        public string Factory { get; set; }

        public string InspectionDate { get; set; }

        public int TotalBookingQuantity { get; set; }
        public string Unit { get; set; }

        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County  { get; set; }
        public string Town { get; set; }

        public string ServiceType { get; set; }

        public List<InpectionProductBaseDetail> productBaseDetails { get; set; }
    }


    public class InpectionProductBaseDetail
    {
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
    }

    public class InspectionProductBaseDetailResponse
    {
        public InspectionBaseAndProductDetails InspectionBaseDetail { get; set; }

        public InspectionProductBaseDetailResult Result { get; set; }
    }

    public enum InspectionProductBaseDetailResult
    {
        Success=1,
        NotFound=2
    }
}
