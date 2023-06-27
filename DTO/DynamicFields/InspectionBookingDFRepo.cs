using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class InspectionBookingDFRepo
    {
        public int BookingNo { get; set; }
        public int DFTransactionId { get; set; }
        public string DFName { get; set; }
        public string DFValue { get; set; }
        public string FbReference { get; set; }
        public int ControlType { get; set; }
        public int? DFSourceType { get; set; }
        public int ControlConfigurationId { get; set; }
    }

    public class ControlAttributeRepo
    {
        public int ControlAttributeId { get; set; }
        public string Value { get; set; }
    }

    public class DFDataSourceRepo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DataSourceTypeId { get; set; }
    }
}
