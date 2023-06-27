using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class InspectionBookingDFData
    {
        public int BookingNo { get; set; }
        public int DFTransactionID { get; set; }
        public string FbReference { get; set; }
        public string DFName { get; set; }
        public string DFValue { get; set; }
        public int ControlConfigId { get; set; }
    }

    public class InspectionBookingDFDataResponse
    {
        public List<InspectionBookingDFData> bookingDFDataList { get; set; }

        public InspectionBookingDFDataResult Result { get; set; }
    }

    public enum InspectionBookingDFDataResult
    {
        Success = 1,
        NotFound = 2
    }
}
