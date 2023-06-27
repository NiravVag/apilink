using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingPOListResponse
    {
        public IEnumerable<BookingPO> Data { get; set; }
        public BookingPoListResult Result { get; set; }

    }

    public enum BookingPoListResult
    {
        Success = 1,
        NotFound = 2
    }

    public class BookingPO
    {
        public int id { get; set; }

        public string pono { get; set; }
    }

    public class BookingPoTransaction
    {
        public int Id { get; set; }

        public int InspectionId { get; set; }

        public string PoNumber { get; set; }
        public string DestinationCountry { get; set; }
        public int ProductRefId { get; set; }
        public int ContainerRefId { get; set; }
    }

}
