using DTO.Common;

namespace DTO.CancelBooking
{
    public class HolidayRequest
    {
        public DateObject ServiceDateFrom { get; set; }
        public int FactoryCountryId { get; set; }
        public int FactoryId { get; set; }
    }
}
