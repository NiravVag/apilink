
using System.Collections.Generic;

namespace DTO.InspectionCertificate
{
    public class InspectionCertificateBookingRequest
    {
        public int Id { get; set; }
        public int ICId { get; set; }
        public int BookingProductId { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public int? PoColorId { get; set; }
        public int ShipmentQty { get; set; }
        public int Active { get; set; }
        public int BookingNumber { get; set; }
        public string PONo { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string DestinationCountry { get; set; }
        public string Unit { get; set; }
        public double? RemainingQty { get; set; }
        public double PresentedQty { get; set; }
        public double TotalICQty { get; set; }
        public int? BusinessLine { get; set; }
    }
    public class DropDown
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public enum DropdownResult
    {
        Success = 1,
        CannotGetList = 2,
        Failed = 3
    }
    public class ICTitleResponse
    {
        public List<DropDown> DropDownList { get; set; }
        public DropdownResult Result { get; set; }
    }
}
